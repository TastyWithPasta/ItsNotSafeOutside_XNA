using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PastaGameLibrary;


namespace TestBed
{
    class BasicZombie : GameObject
    {
		// public Tpl_BasicZombie() 
		//    : base(typeof(BasicZombie), "e_bz")
		//{
		//    _hp = 4;
		//    _speed = 1.5f;
		//    _appearanceRate = 90;
		//    _thumbnail = TextureLibrary.GetSpriteSheet("thb_zombie_base");
		//}

		const float HP = 4;
		const float Speed = 10.0f;

		ObjectState m_objectState = new ObjectState();
		AABBCollider m_collider;
		AABB m_impactBox; //Safe box where shurikens/fire impact effects can hit.
		SingleActionManager m_actionManager;
		BodyPart m_head, m_armL, m_armR, m_upperB, m_lowerB;
		SpriteSheetAnimation m_full, m_noHead, m_noLeftArm, m_noRightArm;
		ParticleSystem m_bodyParts;

		HitAnimation m_hitAnimation;
		ShurikenReceiver m_shurikenReceiver;

		/// <summary>
		/// DEBUG
		/// </summary>
		AABBMeasurer m_measurer;

        public BasicZombie() 
            : base()
        {
			m_physics = new PhysicsComponent(Globals.TheGame, Transform);
			m_sprite = new Sprite(Globals.TheGame, TextureLibrary.GetSpriteSheet("zombie_base", 1, 8), Transform);
			m_sprite.Origin = new Vector2(0.5f, 1.0f);
			m_loot = Loot.GenerateLoot(4);

			ObjectState.BeginAction = delegate() 
			{
				m_actionManager.StartNew(m_full);
			};
			ObjectState.StandbyAction = delegate()
			{
				m_actionManager.Stop();
				m_sprite.SetFrame(0);
			};
			ObjectState.DestroyAction = delegate()
			{
				m_destructible.ClearHitEvents();
				Backpacker.HitPlayerColliders.Remove(m_collider);
			};

			m_collider = new AABBCollider(this, new AABB(m_sprite));
			Backpacker.HitPlayerColliders.Add(m_collider, delegate(Collider other) { m_physics.Throw(10.0f, -10.0f, 0.1f); });
			m_destructible = new DestructibleComponent(Transform, m_collider, m_collider.AABB, HP);
			

			m_impactBox = new AABB(new Transform(Transform, true), new Vector2(22, 42));
			m_impactBox.Transform.Position = new Vector2(13, -44);
			m_measurer = new AABBMeasurer(m_impactBox);
			m_shurikenReceiver = new ShurikenReceiver(Transform, m_impactBox, 5);
			m_hitAnimation = new HitAnimation(World.baseEffect);

			m_full = new SpriteSheetAnimation(m_sprite, 0, 1, 0.5f, -1);
			m_noHead = new SpriteSheetAnimation(m_sprite, 2, 3, 0.5f, -1);
			m_noLeftArm = new SpriteSheetAnimation(m_sprite, 4, 5, 0.5f, -1);
			m_noRightArm = new SpriteSheetAnimation(m_sprite, 6, 7, 0.5f, -1);

			m_actionManager = new SingleActionManager();

			//Transform headTransform = new Transform(Transform, true
			//_projections = new BodyPart[] {
			//    null,
			//    new BodyPart(this, new Vector2(0, -65), "bp_basezombiearm"),
			//    new BodyPart(this, new Vector2(-10, -55), "bp_basezombiearm"),
			//    new BodyPart(this, new Vector2(15, -75), "bp_basezombiehead"),  
             // new BodyPart(this, new Vector2(10, -50), "bp_basezombieupperb"),
             //   new BodyPart(this, new Vector2(10, -10), "bp_basezombielowerb") 
			//};
			
			
			///
			/// Body Parts
			///
			m_bodyParts = new ParticleSystem(Globals.TheGame, 5);

			m_head = new BodyPart(TextureLibrary.GetSpriteSheet("bp_basezombiehead"), new Transform(Transform, true));
			m_head.Transform.Position = new Vector2(5, -50);
			m_bodyParts.AddParticle(m_head);

			m_armL = new BodyPart(TextureLibrary.GetSpriteSheet("bp_basezombiearm"), new Transform(Transform, true));
			m_armL.Transform.Position = new Vector2(5, -30);
			m_bodyParts.AddParticle(m_armL);

			m_armR = new BodyPart(TextureLibrary.GetSpriteSheet("bp_basezombiearm"), new Transform(Transform, true));
			m_armR.Transform.Position = new Vector2(-5, -30);
			m_bodyParts.AddParticle(m_armR);

			m_upperB = new BodyPart(TextureLibrary.GetSpriteSheet("bp_basezombieupperb"), new Transform(Transform, true));
			m_upperB.Transform.Position = new Vector2(0, -30);
			m_bodyParts.AddParticle(m_upperB);

			m_lowerB = new BodyPart(TextureLibrary.GetSpriteSheet("bp_basezombielowerb"), new Transform(Transform, true));
			m_lowerB.Transform.Position = new Vector2(0, -10);
			m_bodyParts.AddParticle(m_lowerB);

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
					m_actionManager.StartNew(m_noHead);
					m_head.Pop(-1.1f, 500, true);
				}));
			m_destructible.AddHealthEvent(2, false, new MethodAction(delegate()
			{
				m_actionManager.StartNew(m_noLeftArm);
				m_armL.Pop(-1.1f, 500, true);
			}));
			m_destructible.AddHealthEvent(1, false, new MethodAction(delegate()
			{
				m_actionManager.StartNew(m_noRightArm);
				m_armR.Pop(-1.1f, 500, true);
			}));
			m_destructible.AddHealthEvent(0, false, new MethodAction(delegate()
			{
				m_upperB.Pop(-1.1f, 500, true);
				m_lowerB.Pop(-1.1f, 500, true);
				ObjectState.Destroy();
			}));

			ObjectState.Begin();
        }

		public override void OnSpawn(Spawner section)
		{
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

        public override void Update()
        {
			m_bodyParts.Update();

			if (ObjectState.State != ActorState.Active)
				return;

			m_physics.Update();
			m_collider.Update();
			m_destructible.Update();
			m_actionManager.Update();
			m_shurikenReceiver.Update();
			m_hitAnimation.Update();

            if(!m_physics.IsProjected)
                Transform.PosX -= Speed * (float)Globals.TheGame.ElapsedTime;
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
