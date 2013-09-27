using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PastaGameLibrary;

namespace TestBed
{
    public class Shuriken : GameObject
    {
        Vector2 m_attackPosition;

		AttackComponent m_attack;
		SpriteSheetAnimation m_animation;
		PointCollider m_pointCollider;

		public Vector2 LastAttackPosition
		{
			get { return m_attackPosition; }
		}

        public Shuriken() : base()
        {
			m_attack = new AttackComponent(1.0f, 0.1f);
			m_sprite = new Sprite(Globals.TheGame, TextureLibrary.GetSpriteSheet("atk_shuriken", 1, 5), m_transform);
			m_sprite.Origin = new Vector2(0.5f, 0.0f);
			m_transform.Scale = new Vector2(3.0f, 3.0f);
			m_animation = new SpriteSheetAnimation(m_sprite, 0, 4, 0.1f, 1);
			m_pointCollider = new PointCollider(m_attack, Transform);

			World.UL_Global.Add(this, 0);
			World.DL_Foreground.Add(this, 0);
        }

        private void AddHit(Actor enemy, float x, float y)
        {
            //BaseHit hit = new BaseHit();         
            //hit.X = x;
            //hit.Y = y;
            //hit.PlaceInFrontOf(enemy);
            //_hits.AddExplosion(hit);
        }
        public override void Update()
        {
            if (TouchInput.IsScreenTapped)
            {
				m_attackPosition = Vector2.Transform(TouchInput.TouchPosition, Matrix.Invert(World.cam_Main.CameraMatrix));
				m_transform.Position = m_attackPosition;
				DestructibleComponent.DestructibleColliders[(int)AttackType.Shuriken].DoCollision(m_pointCollider, null);
				m_animation.Start();

				float halfScreenWidth = Globals.TheGame.ScreenWidth * 0.5f;
				//float screenHeight = Globals.TheGame.ScreenHeight;
				float ratio = (TouchInput.TouchPosition.X - halfScreenWidth) / halfScreenWidth;
				m_transform.Direction = -1 * ratio + (float)(Globals.Random.NextDouble() - 0.5f) * 0.5f;
				m_transform.PosY += Globals.Random.Next(-10, 10);
			}

			m_animation.Update();
        }

		public override void Draw()
		{
			if (m_animation.IsActive)
				m_sprite.Draw();
		}
    }
}
