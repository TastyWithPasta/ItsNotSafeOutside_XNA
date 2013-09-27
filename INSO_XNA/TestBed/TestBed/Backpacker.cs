using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PastaGameLibrary;

namespace TestBed
{
    public enum BackpackerState
    {
        Idle,
        MovingOut,
        Waving,
        InGameplay,
        Jumping,
        Dead,
        Gone,
    }

    public class Backpacker : GameObject
    {
		public static ColliderGroup HitPlayerColliders = new ColliderGroup();

        static Random random = new Random();

        const int TravellerSpacing = 35;
        const int StartOffset = -200;
        const int MoveOutSpeed = 10;

        float m_targetX;
        float m_jumpVelocity;
        bool m_isLeader;
        Level m_level;
        BackpackerState m_currentState;
		SingleActionManager m_actionManager;
		AABBCollider m_collider;

        public Backpacker(SpriteSheet spriteSheet, int index)
            : base()
        {
			m_actionManager = new SingleActionManager();
			m_sprite = new Sprite(Globals.TheGame, spriteSheet, Transform);
			m_sprite.Origin = new Vector2(0.5f, 1.0f);
			m_collider = new AABBCollider(this, new AABB(m_sprite));
            m_currentState = BackpackerState.Idle;
        }

        public bool IsLeader
        {
            get { return m_isLeader; }
            set { m_isLeader = value; }
        }
        public bool IsMovingOut
        {
            get { return m_currentState == BackpackerState.MovingOut; }
        }
        public bool IsJumping
        {
            get { return m_currentState == BackpackerState.Jumping; }
        }
        public bool IsIdle
        {
            get
            {
                return m_currentState == BackpackerState.Idle;
            }
        }
        public bool IsDead
        {
            get { return m_currentState == BackpackerState.Dead || m_currentState == BackpackerState.Gone; }
        }
        public bool IsInGameplay
        {
            get { return m_currentState == BackpackerState.InGameplay; }
        }
        public bool IsOnTarget
        {
            get { return Math.Abs(Transform.PosX - m_targetX) < 1; }
        }

        public void PlaceOnTarget()
        {
			Transform.PosX = m_targetX;
        }
        public void SetTargetLinePosition(int index)
        {
            m_targetX = -index * TravellerSpacing + random.Next(-5, 6);            
        }

        private void Die()
        {
			m_actionManager.StartNew(new SpriteSheetAnimation(m_sprite, 6, 7, 300, -1));
            //UnbindParent();
            m_currentState = BackpackerState.Dead;
        }

        public void DoCollisions(GameTime gameTime)
        {
			HitPlayerColliders.DoCollision(m_collider, CollisionResponse);
        }
        public void CollisionResponse(Collider other)
        {
			//if (other.Owner is Bat)
			//    {
			//        Bat temp = (Bat)other.Owner;
			//        temp.Hit(AttackManager.Finisher);
			//    }

			//if (other.Owner is Enemy)
			//{
				
                
			//    else
			//    {
			//        GameObject temp = (Enemy)other.Owner;
			//        temp(30, -12, 0);
			//    }
                Die();
                return;
            //}
			//else if (otherActor is TombStone)
			//{
			//    TombStone tombStone = (TombStone)otherActor;
			//    tombStone.UpdateSpawning(gameTime);
			//}
        }

        public void SetAtStart(House startHouse)
        {
            if (!IsDead)
            {
				Transform.ParentTransform = null;
                m_currentState = BackpackerState.Idle;

				if (startHouse == null)
				{
					Transform.PosX = -100;
				}
				else
				{
					Transform.PosX = startHouse.HouseExitPoint;
				}
            }
        }
        public void SetMovingOut(House startHouse)
        {
			//if (!IsDead)
			//{
			//    m_currentState = BackpackerState.MovingOut;
			//    m_targetX += startHouse.RallyPoint;
			//    m_actionManager.StartNew(new Animation(m_sprite, 0, 1, 0.09f, true));
			//}
        }
        public void SetMovingIn(House finishHouse)
        {
            if (!IsDead)
            {
                m_currentState = BackpackerState.MovingOut;
               // PlaceBetween(finishHouse.HouseSprite, finishHouse.EntranceMask);
                m_actionManager.StartNew(new SpriteSheetAnimation(m_sprite, 0, 1, 0.05f, -1));
                m_targetX = finishHouse.HouseExitPoint;
				Transform.ParentTransform = null;
            }
        }
        public void SetWaving()
        {
            if (!IsDead)
            {
                m_currentState = BackpackerState.Waving;
				m_actionManager.StartNew(new SpriteSheetAnimation(m_sprite, 2, 3, 0.15f, -1));
            }
        }
        public void SetGameplay()
        {
            if (!IsDead)
            {
                m_currentState = BackpackerState.InGameplay;
                m_actionManager.StartNew(new SpriteSheetAnimation(m_sprite, 0, 1, 0.5f, -1));
                //CurrentAnimation.SetCurrentFrame(1);
            }
        }
        public void SetJumping()
        {
            if (!IsDead)
            {
                m_currentState = BackpackerState.Jumping;
                m_jumpVelocity = -20 + random.Next(-10, 10);
                //CurrentAnimation = new Animation("all", 100, 0, 6);
				m_sprite.SetFrame(5);
                //CurrentAnimation.Pause();
            }
        }

        public override void Update()
        {
			m_actionManager.Update();

            if (m_currentState == BackpackerState.InGameplay
                && !IsDead)
            {
                Transform.PosX += (m_targetX - Transform.PosX) * 0.1f;
            }
            else if (m_currentState == BackpackerState.Dead)
            {
                Transform.PosY -= 3;
                if (m_sprite.Alpha > 0)
					m_sprite.Alpha -= 0.01f;
                //else
                //    _currentState = BackpackerState.Gone;
            }

            else if (m_currentState == BackpackerState.MovingOut)
            {
                if (Transform.PosX < m_targetX)
					Transform.PosX += MoveOutSpeed;
				if (Transform.PosX >= m_targetX)
                {
					Transform.PosX = m_targetX;
					m_actionManager.Stop();
                    m_sprite.SetFrame(0);
                }
            }

            else if (m_currentState == BackpackerState.Jumping)
            {
                Transform.PosY += m_jumpVelocity;
                m_jumpVelocity += 4;
				if (Transform.PosY > 0)
                {
					Transform.PosY = 0;
                    m_currentState = BackpackerState.Idle;
					m_actionManager.Stop();
					m_sprite.SetFrame(0);
                }      
            }
        }
		public override void Draw()
		{
			m_sprite.Draw();
		}
    }


	public static class Party
	{
		const int MeetingPoint = 300; //Where the hikers stop and wave

		public static Backpacker[] Backpackers = new Backpacker[3];

		static Transform s_transform;
		static Concurrent s_movingOutAnimation;

		public static void Initialise()
		{
			s_transform = new Transform();
			Backpackers[0] = new Backpacker(TextureLibrary.GetSpriteSheet("backpacker_0", 1, 8), 0);
			Backpackers[1] = new Backpacker(TextureLibrary.GetSpriteSheet("backpacker_1", 1, 8), 1);
			Backpackers[2] = new Backpacker(TextureLibrary.GetSpriteSheet("backpacker_2", 1, 8), 2);

			for (int i = 0; i < Backpackers.Length; ++i)
			{
				World.DL_Backpackers.Add(Backpackers[i], 0);
				World.UL_Global.Add(Backpackers[i], 0);
			}

			InitialiseMovingOutAnimation();
		}

		static void InitialiseMovingOutAnimation()
		{
			#region Backpacker 0

			Sequence waveAnimation_0 = new Sequence();

			MoveToStaticAction walkMove_0 = new MoveToStaticAction(Globals.TheGame, Backpackers[0].Transform, new Vector2(MeetingPoint, 0), 1);
			walkMove_0.Timer.Interval = 0.5f;
			SpriteSheetAnimation walkAnim_0 = new SpriteSheetAnimation(Backpackers[0].Sprite, 0, 1, 0.1f, 5);
			waveAnimation_0.AddAction(new Concurrent(new PastaGameLibrary.Action[] { walkMove_0, walkAnim_0 }));
			waveAnimation_0.AddAction(new SpriteSheetAnimation(Backpackers[0].Sprite, 0, 0, 0.5f, 1));
			waveAnimation_0.AddAction(new SpriteSheetAnimation(Backpackers[0].Sprite, 2, 3, 0.2f, 4));
			waveAnimation_0.AddAction(new SpriteSheetAnimation(Backpackers[0].Sprite, 0, 0, 0.1f, 1));

			#endregion

			#region Backpacker 1

			Sequence waveAnimation_1 = new Sequence();

			MoveToStaticAction walkMove_1 = new MoveToStaticAction(Globals.TheGame, Backpackers[1].Transform, new Vector2(MeetingPoint - 40, 0), 1);
			walkMove_1.Timer.Interval = 0.5f;
			SpriteSheetAnimation walkAnim_1 = new SpriteSheetAnimation(Backpackers[1].Sprite, 0, 1, 0.1f, 5);
			waveAnimation_1.AddAction(new DelayAction(Globals.TheGame, 0.2f));
			waveAnimation_1.AddAction(new Concurrent(new PastaGameLibrary.Action[] { walkMove_1, walkAnim_1 }));
			waveAnimation_1.AddAction(new SpriteSheetAnimation(Backpackers[1].Sprite, 0, 0, 0.5f, 1));
			waveAnimation_1.AddAction(new SpriteSheetAnimation(Backpackers[1].Sprite, 2, 3, 0.1f, 5));
			waveAnimation_1.AddAction(new SpriteSheetAnimation(Backpackers[1].Sprite, 0, 0, 0.1f, 1));

			#endregion

			#region Backpacker 2

			Sequence waveAnimation_2 = new Sequence();

			MoveToStaticAction walkMove_2 = new MoveToStaticAction(Globals.TheGame, Backpackers[2].Transform, new Vector2(MeetingPoint - 80, 0), 1);
			walkMove_2.Timer.Interval = 0.5f;
			SpriteSheetAnimation walkAnim_2 = new SpriteSheetAnimation(Backpackers[2].Sprite, 0, 1, 0.1f, 5);
			waveAnimation_2.AddAction(new DelayAction(Globals.TheGame, 0.4f));
			waveAnimation_2.AddAction(new Concurrent(new PastaGameLibrary.Action[] { walkMove_2, walkAnim_2 }));
			waveAnimation_2.AddAction(new SpriteSheetAnimation(Backpackers[2].Sprite, 2, 3, 0.2f, 3));
			waveAnimation_2.AddAction(new SpriteSheetAnimation(Backpackers[2].Sprite, 0, 0, 0.1f, 1));

			#endregion

			s_movingOutAnimation = new Concurrent(new PastaGameLibrary.Action[] { waveAnimation_0, waveAnimation_1, waveAnimation_2 });
		}
		public static Concurrent GetMovingOutAnimation()
		{
			return s_movingOutAnimation;
		}

		public static Backpacker GetLeader()
		{
			if(!Backpackers[0].IsDead)
				return Backpackers[0];
			else if(!Backpackers[1].IsDead)
				return Backpackers[1];
			else return Backpackers[2];
		}
	}
}
