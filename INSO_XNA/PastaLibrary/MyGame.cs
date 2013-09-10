using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PastaGameLibrary
{
	public class MyGame : Game
	{
		GameTime m_gameTime = null;
		TimeSpan m_elapsedTime;
		PTimerManager m_timerManager = null;
		GraphicsDeviceManager m_graphics;
		SpriteBatch m_spriteBatch;

		public double ElapsedTime
		{
			get { return m_elapsedTime.TotalSeconds; }
		}
		public GraphicsDeviceManager Graphics
		{
			get { return m_graphics; }
		}
		public int ScreenWidth
		{
			get { return m_graphics.PreferredBackBufferWidth; }
		}
		public int ScreenHeight
		{
			get { return m_graphics.PreferredBackBufferHeight; }
		}
		public SpriteBatch SpriteBatch
		{
			get { return m_spriteBatch; }
		}
		public PTimerManager TimerManager
		{
			get { return m_timerManager; }
			set { m_timerManager = value; }
		}

		public MyGame()
		{
			m_graphics = new GraphicsDeviceManager(this);
 			m_timerManager = new PTimerManager();
		}

		protected override void Initialize()
		{
			Content.RootDirectory = "Content";
			base.Initialize();
			m_spriteBatch = new SpriteBatch(GraphicsDevice);
		}

		//No need for base.Update, since it only updates XNA-specific components
		protected override void Update(GameTime gameTime)
		{
			m_gameTime = gameTime;
			m_elapsedTime = gameTime.ElapsedGameTime;
			m_timerManager.UpdateTimers(m_gameTime);
		}
		//No need for base.Draw, since it only draws XNA-specific components
		protected override void Draw(GameTime gameTime)
		{
			m_gameTime = gameTime;
			m_elapsedTime = gameTime.ElapsedGameTime;
		}
	}
}
