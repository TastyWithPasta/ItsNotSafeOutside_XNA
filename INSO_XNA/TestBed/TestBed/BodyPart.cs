using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PastaGameLibrary;

namespace TestBed
{
	public class PhysicsParticle : IParticle
	{
		const int DissapearTime = 20;

		MyGame m_theGame;
		Transform m_transform;
		Sprite m_sprite;
		PhysicsComponent m_physics;

		protected float m_ttl, m_maxTTL;

		public MyGame TheGame
		{
			get { return m_theGame; }
		}
		public Transform Transform
		{
			get { return m_transform; }
		}
		public Sprite Sprite
		{
			get{return m_sprite;}
		}
		public PhysicsComponent Physics
		{
			get{ return m_physics; }
		}

		public PhysicsParticle(MyGame theGame, Sprite sprite)
		{
			//_burntTexture = TextureManager.GetBurnt(textureName);
			//BindParent(parent);
			m_sprite = sprite;
			m_theGame = theGame;
			m_transform = sprite.Transform;
			m_physics = new PhysicsComponent(theGame, sprite.Transform);
		}

		public bool RemoveMe()
		{
			return m_ttl == 0;
		}

		public void Update()
		{
			float elapsed = (float)m_theGame.ElapsedTime;
			m_ttl -= elapsed;
			if (m_ttl < 0)
				return;

			float ratio = m_ttl / m_maxTTL;
			m_sprite.Alpha = (byte)(ratio * 255);

			m_physics.Update();
		}
		public void Draw()
		{
			m_sprite.Draw();
		}
	}


    public class BodyPart : IParticle
    {
        const int DissapearTime = 20;


		Transform m_transform;
		Sprite m_sprite;
		PhysicsComponent m_physics;

		public Transform Transform
		{
			get { return m_transform; }
		}

        float m_shrinkSpeed = 0.1f;
        bool m_hasPopped = false;

		public BodyPart(SpriteSheet spriteSheet, Transform transform)
        {
			m_transform = transform;
			m_sprite = new Sprite(Globals.TheGame, spriteSheet, m_transform);
			m_physics = new PhysicsComponent(Globals.TheGame, m_transform);
            //PlaceInFrontOf(parent);
            //Hide();
        }

        public bool RemoveMe()
        {
            return m_transform.SclX == 0; 
        }

        public void Update()
        {
			if (!m_hasPopped)
				return;

			m_physics.Update();

			if (!m_physics.IsProjected)
            {
				m_physics.Stop();

				if (m_transform.SclX > 0)
                {
					m_transform.SclX -= m_shrinkSpeed * (float)Globals.TheGame.ElapsedTime;
					m_transform.SclY -= m_shrinkSpeed * (float)Globals.TheGame.ElapsedTime;
                }
                else
                {
					m_transform.SclX = 0;
					m_transform.SclY = 0;
                }
            }
        }
		public void Draw()
		{
			if (m_hasPopped)
				m_sprite.Draw();
		}
       
        public void Pop(float angle, float speed, bool rotate)
        {
			//if (GetParent() != null)
			//{
			//    PlaceInFrontOf(GetParent());
			//    UnbindParent();
			//}       
			m_transform.Position = m_transform.PositionGlobal;
			m_transform.ParentTransform = null;
            speed += (float)(Globals.Random.NextDouble() * speed * 0.5 - speed * 0.25f);
            m_physics.Throw((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed, 0.5f);
            m_hasPopped = true;
        }
    }
}
