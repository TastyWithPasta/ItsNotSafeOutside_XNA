using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PastaGameLibrary;

namespace TestBed
{
    class FatZombie : GameObject
    {
		//public Tpl_FatZombie() : base(typeof(FatZombie), "e_fz")
		//{
		//    _hp = 6;
		//    _speed = 1.0f;
		//    _appearanceRate = 40;
		//    _thumbnail = TextureLibrary.GetSpriteSheet("thb_zombie_phat");
		//}
		const int HP = 8;

		float m_speed = 1.0f;
		Sprite m_sprite;
		AABB m_impactBox;
		AABBCollider m_collider;
		SingleActionManager m_actionManager;
		BodyPart m_head1, m_head2, m_shoulders, m_armL, m_armR, m_upperB, m_lowerB;
		SpriteSheetAnimation m_full, m_noHead1, m_noHead2, m_noShoulders, m_noLeftArm, m_noRightArm;
		HitAnimation m_hitAnimation;
		ShurikenReceiver m_shurikenReceiver;
		ParticleSystem m_bodyParts;

		AABBMeasurer m_measurer;

        public FatZombie()
        {
			m_physics = new PhysicsComponent(Globals.TheGame, Transform);
			m_sprite = new Sprite(Globals.TheGame, TextureLibrary.GetSpriteSheet("zombie_phat", 1, 12), Transform);
			m_sprite.Origin = new Vector2(0.5f, 1);
			m_loot = Loot.GenerateLoot(12);

			ObjectState.BeginAction = delegate() { m_actionManager.StartNew(m_full); };
			ObjectState.StandbyAction = delegate() { m_actionManager.Stop(); m_sprite.SetFrame(0); };
			ObjectState.DestroyAction = delegate() { m_destructible.ClearHitEvents(); Backpacker.HitPlayerColliders.Remove(m_collider); };

			m_collider = new AABBCollider(this, new AABB(m_sprite));
			Backpacker.HitPlayerColliders.Add(m_collider, delegate(Collider other) { m_physics.Throw(10.0f, -10.0f, 0.1f); } );

			m_destructible = new DestructibleComponent(Transform, m_collider, m_collider.AABB, HP);

			m_impactBox = new AABB(new Transform(Transform, true), new Vector2(72, 67));
			m_impactBox.Transform.Position = new Vector2(13, -70);
			m_shurikenReceiver = new ShurikenReceiver(Transform, m_impactBox, 10);
			m_hitAnimation = new HitAnimation(World.baseEffect);
			m_measurer = new AABBMeasurer(new AABB(new Transform(Transform, true), new Vector2(50, 50)));

			m_full = new SpriteSheetAnimation(m_sprite, 0, 1, 0.7f, -1);
			m_noHead1 = new SpriteSheetAnimation(m_sprite, 2, 3, 0.7f, -1);
			m_noHead2 = new SpriteSheetAnimation(m_sprite, 4, 5, 0.7f, -1);
			m_noRightArm = new SpriteSheetAnimation(m_sprite, 6, 7, 0.7f, -1);
			m_noLeftArm = new SpriteSheetAnimation(m_sprite, 8, 9, 0.7f, -1);
			m_noShoulders = new SpriteSheetAnimation(m_sprite, 10, 11, 0.7f, -1);

			m_actionManager = new SingleActionManager();
			m_actionManager.StartNew(m_full);


			//_projections = new BodyPart[] 
			//{
			//    null,
			//    new BodyPart(this, new Vector2(0, -55), "bp_fatshoulders"),
			//    new BodyPart(this, new Vector2(-15, -75), "bp_basezombiearm"),
			//    new BodyPart(this, new Vector2(-15, -75), "bp_basezombiearm"),
			//    new BodyPart(this, new Vector2(-30, -100), "bp_fathead2"),
			//    new BodyPart(this, new Vector2(-30, -115), "bp_fathead1"),
			//};

			//_destroyProjections = new BodyPart[] {
			//    new BodyPart(this, new Vector2(0, -45), "bp_fatupperb"),
			//    new BodyPart(this, new Vector2(0, -10), "bp_fatlowerb"),
			//};
			m_bodyParts = new ParticleSystem(Globals.TheGame, 7);

			m_head1 = new BodyPart(TextureLibrary.GetSpriteSheet("bp_fathead1"), new Transform(Transform, true));
			m_head1.Transform.Position = new Vector2(-30, -110);
			m_bodyParts.AddParticle(m_head1);

			m_head2 = new BodyPart(TextureLibrary.GetSpriteSheet("bp_fathead2"), new Transform(Transform, true));
			m_head2.Transform.Position = new Vector2(-30, -100);
			m_bodyParts.AddParticle(m_head2);

			m_shoulders = new BodyPart(TextureLibrary.GetSpriteSheet("bp_fatshoulders"), new Transform(Transform, true));
			m_shoulders.Transform.Position = new Vector2(0, -80);
			m_bodyParts.AddParticle(m_shoulders);

			m_armL = new BodyPart(TextureLibrary.GetSpriteSheet("bp_basezombiearm"), new Transform(Transform, true));
			m_armL.Transform.Position = new Vector2(-15, -75);
			m_bodyParts.AddParticle(m_armL);

			m_armR = new BodyPart(TextureLibrary.GetSpriteSheet("bp_basezombiearm"), new Transform(Transform, true));
			m_armR.Transform.Position = new Vector2(-15, -75);
			m_bodyParts.AddParticle(m_armR);

			m_upperB = new BodyPart(TextureLibrary.GetSpriteSheet("bp_fatupperb"), new Transform(Transform, true));
			m_upperB.Transform.Position = new Vector2(15, -65);
			m_bodyParts.AddParticle(m_upperB);

			m_lowerB = new BodyPart(TextureLibrary.GetSpriteSheet("bp_fatlowerb"), new Transform(Transform, true));
			m_lowerB.Transform.Position = new Vector2(15, -27);
			m_bodyParts.AddParticle(m_lowerB);

			///
			///Hit events
			///
			m_destructible.SetHitEvent(AttackType.Shuriken, OnShurikenHit);
			m_destructible.SetHitEvent(AttackType.Slash, OnSliceHit);

			///
			/// Health events
			///
			m_destructible.AddHealthEvent(7, false, new MethodAction(delegate()
			{
				m_actionManager.StartNew(m_noHead1);
				m_head1.Pop(-1.1f, 500, true);
			}));
			m_destructible.AddHealthEvent(6, false, new MethodAction(delegate()
			{
				m_actionManager.StartNew(m_noHead2);
				m_head2.Pop(-1.1f, 500, true);
			}));
			m_destructible.AddHealthEvent(5, false, new MethodAction(delegate()
			{
				m_actionManager.StartNew(m_noRightArm);
				m_armR.Pop(-1.1f, 500, true);
			}));
			m_destructible.AddHealthEvent(4, false, new MethodAction(delegate()
			{
				m_actionManager.StartNew(m_noLeftArm);
				m_armL.Pop(-1.1f, 500, true);
			}));
			m_destructible.AddHealthEvent(3, false, new MethodAction(delegate()
			{
				m_actionManager.StartNew(m_noShoulders);
				m_shoulders.Pop(-1.1f, 500, true);
			}));
			m_destructible.AddHealthEvent(0, false, new MethodAction(delegate()
			{
				m_upperB.Pop(-1.1f, 500, true);
				m_lowerB.Pop(-1.1f, 500, true);
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


		public override void OnSpawn(Spawner section)
		{
		}

		public override void Update()
		{
			m_bodyParts.Update();

			//m_measurer.Update();

			if (ObjectState.State != ActorState.Active)
				return;

			m_physics.Update();
			m_collider.Update();
			m_destructible.Update();
			m_actionManager.Update();
			m_hitAnimation.Update();
			m_shurikenReceiver.Update();

			if (!m_physics.IsProjected)
				Transform.PosX -= m_speed;
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
			//m_measurer.Draw();
		}
    }
}
