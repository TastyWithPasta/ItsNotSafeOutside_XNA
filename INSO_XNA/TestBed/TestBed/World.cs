using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using PastaGameLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestBed
{
	public static class World
	{
		const int CameraMoveSpeed = 5;


		const float ExtraBgZoom = -0.4f;
		const float BaseBgZoom = 0.8f;

		public static Camera2D cam_Main;

		static Camera2D cam_BgFront;
		static Camera2D cam_BgMiddle;
		static Camera2D cam_BgBack;
		static float[] backgroundZoomLevels = new float[3];

		static Transform t_0, t_1;

		public static UpdateList UL_Global;
		public static DrawingList DL_EarthTiles; //Ground tiles
		public static DrawingList DL_House; //Tiles on top of the ground
		public static DrawingList DL_GroundItems; //Enemies/items...
		public static DrawingList DL_MiddleGround; //Trees, sign posts
		public static DrawingList DL_Foreground; //

		public static DrawingList DL_ItemDrops;

		//Higher index = deeper
		public static DrawingList[] DL_BgLayers = new DrawingList[3];
		

		public static ParticleSystem PS_Coins;

		public static BasicEffect basicEffect;
		public static Effect customEffect;

		public static Transform GetFrontTransform()
		{
			if (t_0.PosX < t_1.PosX)
				return t_0;
			return t_1;
		}
		public static Transform GetBackTransform()
		{
			if (t_0.PosX > t_1.PosX)
				return t_0;
			return t_1;
		}
		public static Vector2 WorldScreenOrigin
		{
			get { return Vector2.Transform(Vector2.Zero, Matrix.Invert(World.cam_Main.CameraMatrix)); }
		}

		public static void Initialise(InsoGame game)
		{
			basicEffect = new BasicEffect(game.GraphicsDevice);
			Matrix projection = Matrix.CreateOrthographicOffCenter(0, game.ScreenWidth, game.ScreenHeight, 0, 0, 1);
			Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);

			basicEffect.World = Matrix.Identity;
			basicEffect.View = Matrix.Identity;
			basicEffect.Projection = halfPixelOffset * projection;

			basicEffect.TextureEnabled = true;
			basicEffect.VertexColorEnabled = true;


			customEffect = game.Content.Load<Effect>("Effects/Flash");

			//customEffect.Parameters["World"].SetValue(Matrix.Identity);
			//customEffect.Parameters["View"].SetValue(Matrix.Identity);
			//customEffect.Parameters["Projection"].SetValue(halfPixelOffset * projection);



			Vector2 focusPoint = new Vector2(Globals.TheGame.ScreenWidth * 0.5f, Globals.TheGame.ScreenHeight * 0.5f);

			cam_Main = new Camera2D(focusPoint);
			cam_Main.ScaleToZoom = true;

			for (int i = 0; i < backgroundZoomLevels.Length; ++i)
				backgroundZoomLevels[i] = (BaseBgZoom + (float)(i + 1) / (float)backgroundZoomLevels.Length) + ExtraBgZoom;

			cam_BgBack = new Camera2D(focusPoint);
			cam_BgBack.ScaleToZoom = true;
			cam_BgBack.Transform.ScaleUniform = 0.25f;
			cam_BgBack.Transform.ParentTransform = cam_Main.Transform;

			cam_BgMiddle = new Camera2D(focusPoint);
			cam_BgMiddle.ScaleToZoom = true;
			cam_BgMiddle.Transform.ScaleUniform = 0.5f;
			cam_BgMiddle.Transform.ParentTransform = cam_Main.Transform;

			cam_BgFront = new Camera2D(focusPoint);
			cam_BgFront.ScaleToZoom = true;
			cam_BgFront.Transform.ScaleUniform = 0.75f;
			cam_BgFront.Transform.ParentTransform = cam_Main.Transform;

			UL_Global = new UpdateList();
			DL_EarthTiles = new DrawingList();
			DL_House = new DrawingList();
			DL_GroundItems = new DrawingList();
			DL_MiddleGround = new DrawingList();
			DL_BgLayers[0] = new DrawingList();
			DL_BgLayers[1] = new DrawingList();
			DL_BgLayers[2] = new DrawingList();
			DL_ItemDrops = new DrawingList();
			DL_Foreground = new DrawingList();
		}

		public static void Swap()
		{
			Vector2 temp = t_0.Position;
			t_0.Position = t_1.Position;
			t_1.Position = temp;
		}

		public static void Update()
		{
			cam_Main.Update();

			//cam_BgFront.Transform.Position = cam_Main.Transform.Position;
			//cam_BgBack.Transform.Position = cam_Main.Transform.Position;
			//cam_BgMiddle.Transform.Position = cam_Main.Transform.Position;

			
			cam_BgBack.Update();
			cam_BgMiddle.Update();
			cam_BgFront.Update();
			UL_Global.Update();
			UpdateDebugControls();
		}

		public static void UpdateDebugControls()
		{
			if (Globals.kbs.IsKeyDown(Keys.Left))
				cam_Main.Transform.PosX -= CameraMoveSpeed;
			if (Globals.kbs.IsKeyDown(Keys.Right))
				cam_Main.Transform.PosX += CameraMoveSpeed;
			if (Globals.kbs.IsKeyDown(Keys.Up))
				cam_Main.Transform.PosY -= CameraMoveSpeed;
			if (Globals.kbs.IsKeyDown(Keys.Down))
				cam_Main.Transform.PosY += CameraMoveSpeed;
			if (Globals.kbs.IsKeyDown(Keys.LeftShift))
				cam_Main.Zoom *= 1.01f;
			if (Globals.kbs.IsKeyDown(Keys.LeftControl))
				cam_Main.Zoom *= 0.99f;
		}

		public static void Draw()
		{
			SpriteBatch sb = Globals.TheGame.SpriteBatch;

			sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, cam_BgBack.CameraMatrix);
			DL_BgLayers[2].Draw();
			sb.End();

			sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, cam_BgMiddle.CameraMatrix);
			DL_BgLayers[1].Draw();
			sb.End();

			sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, cam_BgFront.CameraMatrix);
			DL_BgLayers[0].Draw();
			sb.End();

			//customEffect.Parameters["View"].SetValue(cam_Main.CameraMatrix);
			basicEffect.View = cam_Main.CameraMatrix;
			basicEffect.VertexColorEnabled = true;
			sb.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, customEffect, cam_Main.CameraMatrix);
			DL_MiddleGround.Draw();
			DL_EarthTiles.Draw();
			DL_House.Draw();
			DL_GroundItems.Draw();
			DL_ItemDrops.Draw();
			DL_Foreground.Draw();
			sb.End();
			AttackManager.Slash.Draw();
		}
	}
}
