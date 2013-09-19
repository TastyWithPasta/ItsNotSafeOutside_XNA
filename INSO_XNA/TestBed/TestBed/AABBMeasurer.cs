using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PastaGameLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestBed
{
	class AABBMeasurer
	{
		const float MoveSpeed = 20.0f;
		const float ScaleSpeed = 0.2f;
		static Color TextColor = new Color(0.6f, 0.0f, 0.0f, 1.0f);

		AABB m_aabb;
		string m_text;

		public AABBMeasurer(AABB aabb)
		{
			m_aabb = aabb;
		}

		public void Update()
		{
			float dt = (float)Globals.TheGame.ElapsedTime;

			if (Globals.kbs.IsKeyDown(Keys.Q))
				m_aabb.Transform.PosX -= dt * MoveSpeed;
			if (Globals.kbs.IsKeyDown(Keys.D))
				m_aabb.Transform.PosX += dt * MoveSpeed;
			if (Globals.kbs.IsKeyDown(Keys.S))
				m_aabb.Transform.PosY += dt * MoveSpeed;
			if (Globals.kbs.IsKeyDown(Keys.Z))
				m_aabb.Transform.PosY -= dt * MoveSpeed;

			if (Globals.kbs.IsKeyDown(Keys.K))
				m_aabb.Transform.SclX -= dt * ScaleSpeed;
			if (Globals.kbs.IsKeyDown(Keys.M))
				m_aabb.Transform.SclX += dt * ScaleSpeed;
			if (Globals.kbs.IsKeyDown(Keys.O))
				m_aabb.Transform.SclY += dt * ScaleSpeed;
			if (Globals.kbs.IsKeyDown(Keys.L))
				m_aabb.Transform.SclY -= dt * ScaleSpeed;

			m_text = "X = " + m_aabb.Transform.PosX 
				+ "\nY = " + m_aabb.Transform.PosY 
				+ "\nW = " + m_aabb.Width * m_aabb.Transform.SclX
				+ "\nH = " + m_aabb.Height * m_aabb.Transform.SclY;
		}

		public void Draw()
		{
			Globals.TheGame.SpriteBatch.Draw(TextureLibrary.PixelTexture, m_aabb.GetBounds(), new Color(0.2f, 0.0f, 0.0f, 0.2f));
			Globals.TheGame.SpriteBatch.DrawString(Globals.debugFont, m_text, m_aabb.Transform.PositionGlobal, TextColor);
		}
	}
}
