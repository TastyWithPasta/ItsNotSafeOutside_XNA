using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PastaGameLibrary;
using Microsoft.Xna.Framework;

namespace TestBed
{
	class ImpactEffect : IParticle
	{
		Sprite m_impactSprite;
		SpriteSheetAnimation m_spriteAnimation;

		public ImpactEffect(Transform transform, SpriteSheet effectSheet, float effectLength)
		{
			m_impactSprite = new Sprite(Globals.TheGame, effectSheet, transform);
			m_impactSprite.Origin = new Microsoft.Xna.Framework.Vector2(0.1f, 0.9f);
			m_impactSprite.Alpha = 0.3f;
			m_spriteAnimation = new SpriteSheetAnimation(m_impactSprite, 0, effectSheet.AmountOfFrames - 1, effectLength, false);
			m_spriteAnimation.Start();
		}

		public void Update()
		{
			m_spriteAnimation.Update();
		}

		public bool RemoveMe()
		{
			return !m_spriteAnimation.IsActive;
		}

		public void Draw()
		{
			m_impactSprite.Draw();
		}
	}

	class ShurikenReceiver
	{
		public static SpriteSheet ImpactTexture;


		Transform m_transform;
		AABB m_shurikenBounds;
		Sprite[] m_impacts;
		ParticleSystem m_hitParticles;
		ParticleGenerator<PhysicsParticle> m_generator;
		GeometryHelper m_helper;
		int currentIndex = 0;

		public ShurikenReceiver(Transform transform, AABB shurikenBounds, int impactBufferSize)
		{
			m_transform = transform;
			m_shurikenBounds = shurikenBounds;
			m_impacts = new Sprite[impactBufferSize];
			m_helper = new GeometryHelper();
			m_hitParticles = new ParticleSystem(Globals.TheGame, 2);
			m_generator = new ParticleGenerator<PhysicsParticle>(Globals.TheGame, m_hitParticles);
		}

		public void OnHit(Vector2 position)
		{
			Transform transform = new Transform(m_transform, true);
			Sprite impactSprite = new Sprite(Globals.TheGame, TextureLibrary.GetSpriteSheet("shuriken_impact"), transform);

			transform.Position = position - m_transform.PositionGlobal;
			transform.Direction = Globals.Random.NextDouble();
			//transform.Direction += Globals.Random.NextDouble() - 0.5;

			transform.SclX = 1 + (float)(Globals.Random.NextDouble() * 0.5) + 0.5f;

			if (currentIndex == m_impacts.Length)
				currentIndex = 0;
			m_impacts[currentIndex] = impactSprite;
			currentIndex++;
		}
		public void OnHit()
		{
			Transform transform = new Transform(m_transform, true);
			Sprite impactSprite = new Sprite(Globals.TheGame, TextureLibrary.GetSpriteSheet("shuriken_impact"), transform);

			transform.Position = m_shurikenBounds.GetRandomPoint(Globals.Random) - m_transform.PositionGlobal;
			transform.Direction = m_helper.GetAngle(m_shurikenBounds.Transform, transform);
			//transform.Direction += Globals.Random.NextDouble() - 0.5;

			Transform impactEffectTransform = new Transform(transform);
			impactEffectTransform.ScaleUniform = 6;
			impactEffectTransform.Direction += 0.5f;
			m_hitParticles.AddParticle(new ImpactEffect(impactEffectTransform, ImpactTexture, 0.15f));

			double distance = m_helper.GetDistance(m_shurikenBounds.Transform.PositionGlobal, transform.PositionGlobal);
			transform.SclX = (float)(distance / m_shurikenBounds.Radius) + 0.2f;
			transform.SclY = 1 + (float)(Globals.Random.NextDouble() * 0.5);

			if (currentIndex == m_impacts.Length)
				currentIndex = 0;
			m_impacts[currentIndex] = impactSprite;
			currentIndex++;
		}

		public void Update()
		{
			m_hitParticles.Update();
		}

		public void Draw()
		{
			for (int i = 0; i < m_impacts.Length; ++i)
				if(m_impacts[i] != null)
					m_impacts[i].Draw();
			m_hitParticles.Draw();
		}
	}
}
