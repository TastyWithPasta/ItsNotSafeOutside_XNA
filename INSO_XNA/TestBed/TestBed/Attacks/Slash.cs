using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using PastaGameLibrary;

namespace TestBed
{
	//public class CritStrikeIcon : Sprite, IParticle
	//{
	//    const float BaseTTL = 0.35f;
	//    const float BaseScale = 0.5f;
	//    const float ExtraScale = 1.5f;
	//    float _ttl = BaseTTL;

	//    public CritStrikeIcon()
	//        : base(TextureLibrary.GetSpriteSheet("crit_strike"))
	//    {
 
	//    }

	//    public override void Update(GameTime gameTime)
	//    {
	//        _ttl -= (float)gameTime.ElapsedGameTime.TotalSeconds;
	//        if (_ttl < 0)
	//            _ttl = 0;
	//        float ratio = _ttl / BaseTTL;
	//        ScaleUniform(BaseScale + ExtraScale * Math.Min(1, 2 * (1 - ratio)));
	//        _alpha = ratio;
	//    }


	//    public bool RemoveMe()
	//    {
	//        return _ttl == 0;
	//    }
	//}

    public class Slash : GameObject
    {
        enum SlashState
        {
            Inactive,
            Initialised,
            Slashing,
            Vanishing,
        }

        const int SliceRadius = 10;
        const int TriangleCount = 10;
        const float ShrinkSpeed = 0.20f;
        const int FreeTicks = 3;

        bool _enabled;
        bool _drawFlag;

        int _ticksSincePressed = 0;
        float _minSpeed = 15;
        float _maxLength = 300, _minLength = 100;
        float _shrinkingRatio = 0.0f;
        float _critStrikeChance = 0.0f, _critMutiplier = 3;
        bool _nextHitIsCritical;


		AttackComponent m_attack;
        SlashState m_slashState;
		LineCollider m_sliceCollider;
        Vector2 _startPosition, _endPosition, _previousPositionInSlash;
        Transform _slashStart, _slashEnd;
        Color _startColor = new Color(80, 80, 100, 100);
        Color _endColor = new Color(175, 175, 175, 100);

        GraphicsDevice _device;

        Matrix _world, _view, _projection;


		public AttackComponent Attack
		{
			get { return m_attack; }
		}

        //VertexPositionColor[] _vertices = new VertexPositionColor[TriangleCount + 2];
        VertexPositionColor[] _vertices = new VertexPositionColor[3];

        public void UpgradeCritStrikeChance()
        {
            if (_critStrikeChance == 0)
                _critStrikeChance = 0.1f;
            else
                _critStrikeChance *= 1.1f;
        }
        public void UpgradeRange(float ratio)
        {
            _maxLength *= ratio;
        }
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public Slash(GraphicsDevice device, int minLength)
        {
            //_vertexBuffer = new VertexBuffer(device, typeof(VertexPositionNormalTexture), _vertices.Length, BufferUsage.None);

			m_attack = new AttackComponent(1, 0.1f);
			m_attack.IsActive = true;

            _minLength = minLength;

            m_sliceCollider = new LineCollider(m_attack);
			_slashStart = new Transform();
			_slashEnd = new Transform();
            _slashStart.Scale = new Vector2(5, 5);
            _slashEnd.Scale = new Vector2(10, 10);

            _device = device;

            InitMatrices();
            InitVertices();

			World.DL_Foreground.Add(this, 0);
        }

        private void InitMatrices()
        {
            //_world = Matrix.CreateTranslation(Vector3.Zero);
			_world = Matrix.CreateTranslation(new Vector3(-Globals.TheGame.ScreenWidth * 0.5f, -Globals.TheGame.ScreenHeight * 0.5f, 0));
            //_world = Matrix.CreateTranslation(new Vector3(Globals.ScreenWidth * 0.5f, Globals.ScreenHeight * 0.5f, 0));

            //_view = Matrix.CreateLookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), Vector3.UnitY);
            _view = Matrix.CreateLookAt(new Vector3(0, 0, -10), new Vector3(0, 0, 0), -Vector3.UnitY);
            _projection = Matrix.CreateOrthographic(800, 480, 0.1f, 100f);
        }

        private void InitVertices()
        {
            _vertices[0].Position.X = 0;
            _vertices[0].Position.Y = 0;
            _vertices[0].Position.Z = 1;
            _vertices[0].Color = _startColor;

            _vertices[2].Position.X = Globals.TheGame.ScreenWidth;
            _vertices[2].Position.Y = 0;
            _vertices[2].Position.Z = 1;
            _vertices[2].Color = _endColor;

			_vertices[1].Position.X = Globals.TheGame.ScreenWidth;
			_vertices[1].Position.Y = Globals.TheGame.ScreenHeight;
            _vertices[1].Position.Z = 1;
            _vertices[1].Color = _endColor;
        }

        public void UpdateVertices()
        {
            if (_endPosition != _startPosition)
            {
                Vector2 direction = _endPosition - _startPosition;
                direction.Normalize();
                Vector2 normal = new Vector2(-direction.Y * SliceRadius * (1 - _shrinkingRatio),
                    direction.X * SliceRadius * (1 - _shrinkingRatio));

                _vertices[0].Position.X = _startPosition.X + (_endPosition.X - _startPosition.X) * _shrinkingRatio;
                _vertices[0].Position.Y = _startPosition.Y + (_endPosition.Y - _startPosition.Y) * _shrinkingRatio;

                _vertices[2].Position.X = _endPosition.X + normal.X;
                _vertices[2].Position.Y = _endPosition.Y + normal.Y;

                _vertices[1].Position.X = _endPosition.X - normal.X;
                _vertices[1].Position.Y = _endPosition.Y - normal.Y;
            }

        }

        public override void Update()
        {
            if (!m_attack.IsActive)
                return;

            switch (m_slashState)
            {
                case SlashState.Inactive:
                    if (TouchInput.IsScreenTouched)
                    {
                        InitSlash();
                        m_slashState = SlashState.Initialised;
                    }   
                    break;
                case SlashState.Initialised:
                    if (TouchInput.IsScreenTouched)
                    {
                        UpdateSlashPositions();
                        if (HasSlashOccured())
                        {
                            _drawFlag = true;
                            m_slashState = SlashState.Slashing;
                        }
                    }
                    else
                        m_slashState = SlashState.Inactive;
                    break;
                case SlashState.Slashing:
                    if (TouchInput.IsScreenTouched)
                    {
                        UpdateSlashPositions();
                        UpdateSlash();

                        if (IsSlashTooLong())
                        {
                            Vector2 direction = (_endPosition - _startPosition);
                            direction.Normalize();
                            _endPosition = _startPosition + _maxLength * direction;
                            
							//Do collisions
							DestructibleComponent.DestructibleColliders[(int)AttackType.Shuriken].DoCollision(m_sliceCollider, null);
                            
							m_slashState = SlashState.Vanishing;
                            break;
                        }
                        else
                            if (HasSlashStopped())
                            {
								//Do collisions
								DestructibleComponent.DestructibleColliders[(int)AttackType.Shuriken].DoCollision(m_sliceCollider, null);
                                m_slashState = SlashState.Vanishing;
                                break;
                            }
                    }
                    else
                        m_slashState = SlashState.Inactive;
                    break;
                case SlashState.Vanishing:
                    //Starts another slash immediately if the user taps the screen when the slash is still dissapearing
                    if (TouchInput.IsScreenTapped)
                    {
                        m_slashState = SlashState.Inactive;
                        Update();
                        return;
                    }
                    _shrinkingRatio = Math.Min(_shrinkingRatio + ShrinkSpeed, 1);
                    if (_shrinkingRatio == 1)
                    {
                        m_slashState = SlashState.Inactive;
                        _drawFlag = false;
                    }
                    break;
            }
            UpdateVertices();
        }


        private void InitSlash()
        {
            _shrinkingRatio = 0;
            _startPosition = _endPosition = TouchInput.TouchPosition;
            _drawFlag = false;
        }

        private void UpdateSlashPositions()
        {
            Vector2 currentPosition = TouchInput.TouchPosition;
#if DEBUG
            if (currentPosition == Vector2.Zero)
                Debug.WriteLine("CurrentPosition is zero");
#endif
            _previousPositionInSlash = _endPosition;
            _endPosition = currentPosition;
        }
        private void UpdateSlash()
        {
            _ticksSincePressed++;
            Matrix inversecam = Matrix.Invert(World.cam_Main.CameraMatrix);
            m_sliceCollider.Line.A_Local = Vector2.Transform(_startPosition, inversecam);
            m_sliceCollider.Line.B_Local = Vector2.Transform(_endPosition, inversecam);
        }

        private bool HasSlashOccured()
        {
            float distance = Vector2.Distance(_startPosition, _endPosition);
            return distance > _minLength;
        }
        private bool IsSlashTooLong()
        {
            float distance = Vector2.Distance(_startPosition, _endPosition);
            return distance > _maxLength;
        }
        private bool HasSlashStopped()
        {
            return (_ticksSincePressed > FreeTicks)
                && (Vector2.Distance(_previousPositionInSlash, _endPosition) < _minSpeed);
        }

        private void DoForcedCollisions(GameTime gameTime)
        {
			//Enemy[] enemies = _enemyManager.GetEnemiesOfType(typeof(RomanZombie));

			//RomanZombie tempRoman;
			//LineSegment shieldSegment;
			//for (int i = 0; i < enemies.Length; ++i)
			//{
			//    tempRoman = (RomanZombie)enemies[i];
			//    shieldSegment = tempRoman.ZombieShield.ShieldSegment;
			//    Vector2 normal = shieldSegment.Normal;
			//    normal.Normalize();

			//    if (tempRoman.HasShield &&
			//        CollisionMethods.TestCollision(_sliceSegment, shieldSegment)
			//        && Vector2.Dot(_sliceSegment.Difference, shieldSegment.Normal) > 0)
			//    {
			//        //Console.WriteLine(Vector2.Dot(_sliceSegment.Direction, romanZombie.ShieldNormal));
			//        StraightLine sl1 = new StraightLine(_sliceSegment);
			//        StraightLine sl2 = new StraightLine(shieldSegment);
			//        GeometryHelper.GetIntersection(sl1, sl2, out _endPosition);
			//        _endPosition = Vector2.Transform(_endPosition, _camera.CameraMatrix);
			//        //_sparkTexture.
			//        UpdateVertices();
			//        tempRoman.AbsorbHit();
			//        //DoCollisions(gameTime, _enemyManager);
			//        _slashState = SlashState.Inactive;                 
			//        break;
			//    }
			//}
        }


		//public override void CollisionResponse(Actor otherActor)
		//{
		//    Enemy otherEnemy = (Enemy)otherActor;

		//    Vector2 collisionPoint;
		//    GeometryHelper.GetIntersection(otherActor.BoundingRectangle, _sliceSegment, out collisionPoint);
		//    BaseHit hit = new BaseHit();
		//    hit.X = collisionPoint.X;
		//    hit.Y = collisionPoint.Y;
		//    hit.PlaceInFrontOf(otherActor);
		//    //_hits.AddExplosion(hit);

		//    RollCritStrike();
		//    otherEnemy.Hit(this);
		//    if(_nextHitIsCritical)
		//    {
		//        CritStrikeIcon crit = new CritStrikeIcon();
		//        crit.X = hit.X;
		//        crit.Y = hit.Y;
		//        _hits.AddExplosion(crit);
		//    }           
		//}
		//private void RollCritStrike()
		//{
		//    _nextHitIsCritical = Globals.random.NextDouble() < _critStrikeChance;
		//}


		public override void Draw()
        {
			//for (int i = 0; i < _sparks.Count; ++i)
			//    _sparks[i].Draw(spriteBatch);

            if (_drawFlag)
            {
                BasicEffect effect = new BasicEffect(_device);
                effect.LightingEnabled = false;
                effect.VertexColorEnabled = true;

                effect.World = _world;
                effect.View = _view;
                effect.Projection = _projection;
                effect.CurrentTechnique.Passes[0].Apply();
                //device.SetVertexBuffer(_vertexBuffer);
                //device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, _vertices, 0, TriangleCount);
                _device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, _vertices, 0, 1);
                //device.SetVertexBuffer(null);
            }

        }
    }
}
