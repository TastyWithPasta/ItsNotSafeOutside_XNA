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

			cameraMove = new MoveToStaticAction(this, World.cam_Main.Transform, new Vector2(0, -350), false);
			cameraMove.Interpolator = new PSmoothstepInterpolation();
			cameraMove.Timer.Interval = 1.0f;

			cameraZoom = new ScaleToAction(this, World.cam_Main.Transform, new Vector2(0.5f, 0.5f), false);
			cameraZoom.Interpolator = new PSmoothstepInterpolation();
			cameraZoom.Timer.Interval = 1.2f;

			Globals.GameScene = new GameScene();
			Globals.GameScene.BuildNextLevel();
			Globals.GameScene.StartCurrentLevel();
			World.cam_Main.Transform.PosX = 500;
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

			Globals.kbs = Keyboard.GetState();
			TouchInput.Update();

			if (Globals.kbs.IsKeyDown(Keys.Space) && !cameraMove.IsActive)
			{
				World.cam_Main.Transform.PosY = 0;
				cameraMove.StartPosition = new Vector2(World.cam_Main.Transform.PosX, 0);
				cameraMove.Target = World.cam_Main.Transform.Position + new Vector2(0, -350);

				Globals.GameScene.BuildNextLevel();
				Globals.GameScene.StartCurrentLevel();

				//cameraZoom.Start();
				//cameraMove.Start();
				//Coin.SpawnCoin(COIN_TYPE.GOLD, Vector2.Zero);
			}
			cameraMove.Update();
			cameraZoom.Update();
			Globals.GameScene.Update();
			World.Update();
		}


		protected override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			GraphicsDevice.Clear(Color.CornflowerBlue);
			World.Draw();
			TouchInput.Draw();
		}
	}
}
