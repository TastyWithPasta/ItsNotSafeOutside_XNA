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

		float m_speed = 0.5f;
		AABBCollider m_collider;
		SingleActionManager m_actionManager;
		BodyPart m_head, m_armL, m_armR, m_upperB, m_lowerB;
		Animation m_full, m_noHead, m_noLeftArm, m_noRightArm;

        public BasicZombie() 
            : base()
        {
			m_physics = new PhysicsComponent(Globals.TheGame, Transform);
			m_sprite = new Sprite(Globals.TheGame, TextureLibrary.GetSpriteSheet("zombie_base", 1, 8), Transform);
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

			m_collider = new AABBCollider(this, new AABB(Transform, m_sprite.Dimensions));
			Backpacker.HitPlayerColliders.Add(m_collider, delegate(Collider other) { m_physics.Throw(10.0f, -10.0f, 0.1f); });

			m_destructible = new DestructibleComponent(Transform, m_collider, m_collider.AABB, 4);

			m_full = new Animation(m_sprite, 0, 1, 0.5f, true);
			m_noHead = new Animation(m_sprite, 2, 3, 0.5f, true);
			m_noLeftArm = new Animation(m_sprite, 4, 5, 0.5f, true);
			m_noRightArm = new Animation(m_sprite, 6, 7, 0.5f, true);

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
			m_head = new BodyPart(TextureLibrary.GetSpriteSheet("bp_basezombiehead"), new Transform(Transform, true));
			m_head.Transform.Position = new Vector2(5, -80);
			m_armL = new BodyPart(TextureLibrary.GetSpriteSheet("bp_basezombiearm"), new Transform(Transform, true));
			m_armL.Transform.Position = new Vector2(5, -50);
			m_armR = new BodyPart(TextureLibrary.GetSpriteSheet("bp_basezombiearm"), new Transform(Transform, true));
			m_armR.Transform.Position = new Vector2(-5, -50);
			m_upperB = new BodyPart(TextureLibrary.GetSpriteSheet("bp_basezombieupperb"), new Transform(Transform, true));
			m_upperB.Transform.Position = new Vector2(0, -45);
			m_lowerB = new BodyPart(TextureLibrary.GetSpriteSheet("bp_basezombielowerb"), new Transform(Transform, true));
			m_upperB.Transform.Position = new Vector2(0, -30);

			///
			///Hit events
			///
			m_destructible.SetHitEvent(AttackType.Shuriken, m_destructible.BaseHit);
			m_destructible.SetHitEvent(AttackType.Slash, m_destructible.BaseHit);

			///
			/// Health events
			///
			m_destructible.AddHealthEvent(3, false, new MethodAction(delegate()
				{
					m_actionManager.StartNew(m_noHead);
					m_head.Pop(-1.1f, 10, true);
				}));
			m_destructible.AddHealthEvent(2, false, new MethodAction(delegate()
			{
				m_actionManager.StartNew(m_noLeftArm);
				m_armL.Pop(-1.1f, 27, true);
			}));
			m_destructible.AddHealthEvent(1, false, new MethodAction(delegate()
			{
				m_actionManager.StartNew(m_noRightArm);
				m_armR.Pop(-1.1f, 27, true);
			}));
			m_destructible.AddHealthEvent(0, false, new MethodAction(delegate()
			{
				m_upperB.Pop(-1.1f, 27, true);
				m_lowerB.Pop(-1.1f, 27, true);
				ObjectState.Destroy();
			}));

			ObjectState.Begin();
        }

		public override void OnSpawn(Spawner section)
		{
			World.UL_Global.Add(this, 0);
			World.DL_GroundItems.Add(this, 0);
		}

        public override void Update()
        {
			m_head.Update();
			m_armL.Update();
			m_armR.Update();
			m_upperB.Update();
			m_lowerB.Update();

			if (ObjectState.State != ActorState.Active)
				return;

			m_physics.Update();
			m_collider.Update();
			m_destructible.Update();
			m_actionManager.Update();
			
            if(!m_physics.IsProjected)
                Transform.PosX -= m_speed * (float)Globals.TheGame.ElapsedTime;
        }
		public override void Draw()
		{
			if (ObjectState.State != ActorState.Destroyed)
			{
				m_destructible.Draw();
				m_sprite.Draw();
			}

			m_head.Draw();
			m_armL.Draw();
			m_armR.Draw();
			m_upperB.Draw();
			m_lowerB.Draw();
		}
    }
}
