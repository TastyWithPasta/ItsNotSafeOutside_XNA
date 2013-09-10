using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PastaGameLibrary;


namespace PastaGameLibrary
{
	public class SpriteSheet
	{
		Texture2D m_texture;
		int m_rows, m_columns;
		int m_frameWidth, m_frameHeight;

		public SpriteSheet(Texture2D texture)
		{
			m_texture = texture;
			m_frameWidth = texture.Width;
			m_frameHeight = texture.Height;
			m_rows = 1;
			m_columns = 1;
		}
		public SpriteSheet(Texture2D texture, int rows, int columns)
		{
			m_texture = texture;
			m_rows = rows;
			m_columns = columns;
			m_frameWidth = m_texture.Width / m_columns;
			m_frameHeight = m_texture.Height / m_rows;
		}
		//public SpriteSheet(Texture2D texture, int frameWidth, int frameHeight)
		//{
		//    m_frameWidth = frameWidth;
		//    m_frameHeight = frameHeight;
		//    m_rows = m_texture.Height / m_frameHeight;
		//    m_columns = m_texture.Width / m_frameWidth;
		//}

		public Texture2D Texture
		{
			get { return m_texture; }
		}
		public int Rows
		{
			get { return m_rows; }
		}
		public int Columns
		{
			get { return m_columns; }
		}
		public int AmountOfFrames
		{
 			get{ return m_rows * m_columns;}
		}
		public int FrameWidth
		{
			get { return m_frameWidth; }
		}
		public int FrameHeight
		{
			get { return m_frameHeight; }
		}
	}
	public class Sprite : IPDrawable
	{
		MyGame m_theGame;

		private Rectangle m_srcRect = Rectangle.Empty;
		private SpriteSheet m_spriteSheet = null;
		private Vector2 m_origin = new Vector2(0.5f, 0.5f);
		private Vector2 m_renderOrigin = new Vector2(0.5f, 0.5f);
		private Vector4 m_colour = new Vector4(1,1,1,1);
		private Color m_renderColour = Color.White;
		private Transform m_transform = null;

		public Sprite(MyGame theGame, SpriteSheet spriteSheet, Transform transform)
		{
			m_theGame = theGame;
			SpriteSheet = spriteSheet;
			m_transform = transform;
			m_srcRect.Width = m_spriteSheet.FrameWidth;
			m_srcRect.Height = m_spriteSheet.FrameHeight;
			Origin = new Vector2(0.5f, 0.5f);
		}

		public Sprite(Sprite spriteToCopy, Transform transform)
		{
			m_theGame = spriteToCopy.m_theGame;
			m_srcRect = spriteToCopy.m_srcRect;
			m_spriteSheet = spriteToCopy.m_spriteSheet;
			m_origin = spriteToCopy.m_origin;
			m_renderOrigin = spriteToCopy.m_renderOrigin;
			m_colour = spriteToCopy.m_colour;
			m_renderColour = spriteToCopy.m_renderColour;
			m_transform = transform;
		}

		public MyGame TheGame
		{
			get { return m_theGame; }
		}
		public Transform Transform
		{
			get { return m_transform; }
		}
		public SpriteSheet SpriteSheet
		{
			get { return m_spriteSheet; }
			set
			{
				m_spriteSheet = value;
			}
		}
		public Vector4 Colour
		{
			get { return m_colour; }
			set { 
				m_colour = value;
				m_renderColour.R = (byte)(m_colour.X * m_colour.W * 255);
				m_renderColour.G = (byte)(m_colour.Y * m_colour.W * 255);
				m_renderColour.B = (byte)(m_colour.Z * m_colour.W * 255);
				m_renderColour.A = (byte)(m_colour.W * 255);
			}
		}
		public Vector2 Origin
		{
			get { return m_origin; }
			set {
				m_origin = value;
				m_renderOrigin.X = value.X * m_srcRect.Width;
				m_renderOrigin.Y = value.Y * m_srcRect.Height;
			}
		}
		public Vector2 Dimensions
		{
			get { return new Vector2(m_srcRect.Width, m_srcRect.Height); }
		}
		public float Width
		{
			get { return m_srcRect.Width; }
		}
		public float Height
		{
			get { return m_srcRect.Height; }
		}
		public float Alpha
		{
			get { return m_colour.W; }
			set { m_colour.W = value;
				m_renderColour.R = (byte)(m_colour.X * m_colour.W * 255);
				m_renderColour.G = (byte)(m_colour.Y * m_colour.W * 255);
				m_renderColour.B = (byte)(m_colour.Z * m_colour.W * 255);
				m_renderColour.A = (byte)(m_colour.W * 255);
			}
		}
		public Rectangle SourceRectangle
		{
			get { return m_srcRect; }
			set 
			{ 
				m_srcRect = value;
				Origin = m_origin; //Update origin
			}
		}
		public void SetFrame(int frame)
		{
			frame = Math.Min(m_spriteSheet.AmountOfFrames, Math.Max(0, frame));
			m_srcRect.X = frame % m_spriteSheet.Columns * m_srcRect.Width;
			m_srcRect.Y = frame / m_spriteSheet.Columns * m_srcRect.Height;
		}



		//protected override void OnAttach(IPActor container)
		//{
		//    m_transformComponent = Container.GetFirstComponent<TransformComponent>();
		//}
		public void Draw()
		{
			if (m_transform == null || m_spriteSheet == null)
				return;

			Vector2 position, scale;
			float rotation;
			m_transform.GetGlobalComponents(out position, out rotation, out scale);
			m_theGame.SpriteBatch.Draw(m_spriteSheet.Texture, position, SourceRectangle, m_renderColour, rotation, m_renderOrigin, scale, SpriteEffects.None, 0);

			//m_theGame.SpriteBatch.Draw(m_spriteSheet.Texture, DestinationRectangle, SourceRectangle, Colour, Rotation, m_renderOrigin, SpriteEffects.None, 0);
		}
	}
}
