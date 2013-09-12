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
		MoveToStaticAction cameraMove;
		ScaleToAction cameraZoom;

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

			cameraMove = new MoveToStaticAction(this, World.cam_Main.Transform, new Vector2(0, -350), false);
			cameraMove.Interpolator = new PSmoothstepInterpolation();
			cameraMove.Timer.Interval = 1.0f;

			cameraZoom = new ScaleToAction(this, World.cam_Main.Transform, new Vector2(0.5f, 0.5f), false);
			cameraZoom.Interpolator = new PSmoothstepInterpolation();
			cameraZoom.Timer.Interval = 1.2f;
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

			TouchInput.Update();

			if (World.kbs.IsKeyDown(Keys.Space) && !cameraMove.IsActive)
			{
				World.cam_Main.Transform.PosY = 0;
				cameraMove.StartPosition = new Vector2(World.cam_Main.Transform.PosX, 0);
				cameraMove.Target = World.cam_Main.Transform.Position + new Vector2(0, -350);

				cameraZoom.Start();
				cameraMove.Start();
				//Coin.SpawnCoin(COIN_TYPE.GOLD, Vector2.Zero);
			}

			cameraMove.Update();
			cameraZoom.Update();
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
