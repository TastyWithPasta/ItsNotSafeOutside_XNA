using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PastaGameLibrary;

namespace TestBed
{
    class PogoZombie : GameObject
    {
		const float Speed = 50.0f;
		const int JumpForce = 800;
		const int JumpForceInterval = 200;

		AABBCollider m_collider;
		BodyPart m_head, m_armL, m_armR, m_upperB, m_lowerB, m_pogoStick;
		AABB m_impactBox;
		HitAnimation m_hitAnimation;
		ShurikenReceiver m_shurikenReceiver;
		ParticleSystem m_bodyParts;

		AABBMeasurer m_measurer;
		
        const float Gravity = 3.0f;
        bool _isWaitingToJump = true;
        float m_jumpForce = 500;
        float _jumpPause = 0.15f, _jumpPauseCounter;
		int m_currentFrame = 0;

		//        public Tpl_PogoZombie() 
		//    : base(typeof(PogoZombie), "e_pz")
		//{
		//    _hp = 3;
		//    _speed = 2.4f;
		//    _jumpForce = 25;
		//    _appearanceRate = 30;
		//}

        public PogoZombie() : base()
        {
			m_jumpForce = Globals.Random.Next(-JumpForceInterval, JumpForceInterval + 1) + JumpForce; 

			m_sprite = new Sprite(Globals.TheGame, TextureLibrary.GetSpriteSheet("zombie_pogo", 1, 8), Transform);
			m_sprite.Origin = new Vector2(0.5f, 1.0f);
			m_physics = new PhysicsComponent(Globals.TheGame, Transform);
			m_loot = Loot.GenerateLoot(12);

			m_currentFrame = 0;

			ObjectState.BeginAction = delegate()
			{
				m_sprite.SetFrame(m_currentFrame);
			};
			ObjectState.StandbyAction = delegate()
			{
				m_sprite.SetFrame(m_currentFrame + 1);
			};
			ObjectState.DestroyAction = delegate()
			{
				m_destructible.ClearHitEvents();
				Backpacker.HitPlayerColliders.Remove(m_collider);
			};

			m_collider = new AABBCollider(this, new AABB(m_sprite));
			Backpacker.HitPlayerColliders.Add(m_collider, delegate(Collider other) { m_physics.Throw(10.0f, -10.0f, 0.1f); });

			m_destructible = new DestructibleComponent(Transform, m_collider, m_collider.AABB, 4);
			m_hitAnimation = new HitAnimation(World.baseEffect);
			m_impactBox = new AABB(new Transform(Transform, true), new Vector2(20, 60));
			m_impactBox.Transform.Position = new Vector2(5, -54);
			m_shurikenReceiver = new ShurikenReceiver(Transform, m_impactBox, 5);
			m_measurer = new AABBMeasurer(m_impactBox);

			//_projections = new BodyPart[] {
			//    null,
			//    new BodyPart(this, new Vector2(0, -65), "bp_basezombiearm"),
			//    new BodyPart(this, new Vector2(-10, -55), "bp_basezombiearm"),
			//    new BodyPart(this, new Vector2(15, -75), "bp_basezombiehead"),               
			//};

			//_destroyProjections = new BodyPart[]
			//{
			//    new BodyPart(this, new Vector2(10, -50), "bp_basezombieupperb"),
			//    new BodyPart(this, new Vector2(10, -50), "bp_pogostick"),
			//    new BodyPart(this, new Vector2(10, -10), "bp_basezombielowerb")
			//};

			///
			///	Body parts
			///

			m_bodyParts = new ParticleSystem(Globals.TheGame, 6);
			
			m_head = new BodyPart(TextureLibrary.GetSpriteSheet("bp_basezombiehead"), new Transform(Transform, true));
			m_head.Transform.Position = new Vector2(7, -97);
			m_bodyParts.AddParticle(m_head);

			m_armR = new BodyPart(TextureLibrary.GetSpriteSheet("bp_basezombiearm"), new Transform(Transform, true));
			m_armR.Transform.Position = new Vector2(-20, -70);
			m_bodyParts.AddParticle(m_armR);

			m_lowerB = new BodyPart(TextureLibrary.GetSpriteSheet("bp_basezombielowerb"), new Transform(Transform, true));
			m_lowerB.Transform.Position = new Vector2(0, -40);
			m_bodyParts.AddParticle(m_lowerB);

			m_upperB = new BodyPart(TextureLibrary.GetSpriteSheet("bp_basezombieupperb"), new Transform(Transform, true));
			m_upperB.Transform.Position = new Vector2(0, -65);
			m_bodyParts.AddParticle(m_upperB);

			m_armL = new BodyPart(TextureLibrary.GetSpriteSheet("bp_basezombiearm"), new Transform(Transform, true));
			m_armL.Transform.Position = new Vector2(-20, -70);
			m_bodyParts.AddParticle(m_armL);

			m_pogoStick = new BodyPart(TextureLibrary.GetSpriteSheet("bp_pogostick"), new Transform(Transform, true));
			m_pogoStick.Transform.Position = new Vector2(-6, -40);
			m_bodyParts.AddParticle(m_pogoStick);	

			///
			///Hit events
			///
			m_destructible.SetHitEvent(AttackType.Shuriken, OnShurikenHit);
			m_destructible.SetHitEvent(AttackType.Slash, OnSliceHit);

			///
			/// Health events
			///
			m_destructible.AddHealthEvent(3, false, new MethodAction(delegate()
			{
				m_currentFrame = 2;
				m_sprite.SetFrame(m_currentFrame);
				m_head.Pop(-1.1f, 800, true);
			}));
			m_destructible.AddHealthEvent(2, false, new MethodAction(delegate()
			{
				m_currentFrame = 4;
				m_sprite.SetFrame(m_currentFrame);
				m_armL.Pop(-1.1f, 800, true);
			}));
			m_destructible.AddHealthEvent(1, false, new MethodAction(delegate()
			{
				m_currentFrame = 6;
				m_sprite.SetFrame(m_currentFrame);
				m_armR.Pop(-1.1f, 800, true);
			}));
			m_destructible.AddHealthEvent(0, false, new MethodAction(delegate()
			{
				m_upperB.Pop(-1.1f, 800, true);
				m_lowerB.Pop(-1.1f, 800, true);
				m_pogoStick.Pop(-1.1f, 200, true);
				ObjectState.Destroy();
			}));

			ObjectState.Begin();
        }

		private void OnShurikenHit(Collider other)
		{
			m_destructible.BaseHit(other);
			m_shurikenReceiver.OnHit();
			m_hitAnimation.Hit();
		}
		private void OnSliceHit(Collider other)
		{
			m_destructible.BaseHit(other);
			m_physics.Throw(75, -75, 0);
			m_hitAnimation.Hit();
		}

        private void JumpInactive()
        {
            m_physics.Throw(0, -m_jumpForce * 0.5f, 0);
            _isWaitingToJump = false;
        }
        private void JumpActive()
        {
			m_physics.Throw(-Speed, -m_jumpForce, 0);
            m_sprite.SetFrame(m_currentFrame);
            _isWaitingToJump = false;
        }

        public override void Update()
        {
			m_bodyParts.Update();
			m_physics.Update();
			m_collider.Update();
			m_destructible.Update();
			m_hitAnimation.Update();
			m_shurikenReceiver.Update();
			


			if (_isWaitingToJump)
			{
				_jumpPauseCounter -= (float)Globals.TheGame.ElapsedTime;
				if (_jumpPauseCounter < 0)
					if (ObjectState.State == ActorState.Active)
						JumpActive();
					else if (ObjectState.State == ActorState.StandBy)
						JumpInactive();
			}
			else
			{
				if (Transform.PosY >= 0)
				{
					Transform.PosY = 0;
					m_physics.Stop();
					_isWaitingToJump = true;
					_jumpPauseCounter = _jumpPause;
					m_sprite.SetFrame(m_currentFrame + 1);
				}
			}
        }

		public override void OnSpawn(Spawner section)
		{
			//base.OnSpawn(section);
		}

		public override void Draw()
		{
			if (ObjectState.State != ActorState.Destroyed)
			{
				m_destructible.Draw();
				m_hitAnimation.Apply();
				m_sprite.Draw();
				m_hitAnimation.Remove();
				m_shurikenReceiver.Draw();
			}

			m_bodyParts.Draw();
		}
    }
}
