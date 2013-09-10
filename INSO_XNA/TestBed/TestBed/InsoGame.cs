using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PastaGameLibrary;

namespace TestBed
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class InsoGame : MyGame
	{
		ColliderGroup colliders;
		Level level;

		public ColliderGroup Colliders
		{
			get { return colliders; }
		}

		public InsoGame()
		{
			colliders = new ColliderGroup();
		}
		protected override void Initialize()
		{
			base.Initialize();

			Globals.Initialise(this);
			World.Initialise(this);
			AttackManager.Initialise(GraphicsDevice);
			DestructibleComponent.Initialise();

			List<BackgroundObject> results;
			results = BackgroundObject.CreateBackgroundObjects(5, null, new Vector2(3000, 1), new Vector2(500, 0), StyleManager.LevelStyle.Country, 0);
			for (int i = 0; i < results.Count; ++i)
				results[i].Transform.PosY -= 50;
			results = BackgroundObject.CreateBackgroundObjects(10, null, new Vector2(3000, 1), new Vector2(300, 0), StyleManager.LevelStyle.Country, 1);
			for (int i = 0; i < results.Count; ++i)
				results[i].Transform.PosY -= 100;
			results = BackgroundObject.CreateBackgroundObjects(10, null, new Vector2(3000, 1), new Vector2(300, 0), StyleManager.LevelStyle.Country, 2);
			for (int i = 0; i < results.Count; ++i)
				results[i].Transform.PosY -= 125;

			level = new Level(new Transform());
			level.BuildFirst();
		}

		protected override void LoadContent()
		{
			TextureLibrary.LoadContent(Content, "Textures");
		}

		protected override void UnloadContent()
		{
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();

			if(World.kbs.IsKeyDown(Keys.Space))
				Coin.SpawnCoin(COIN_TYPE.GOLD, Vector2.Zero);

			AttackManager.Update();
			World.Update();
		}


		protected override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			GraphicsDevice.Clear(Color.CornflowerBlue);
			World.Draw();
		}
	}
}
