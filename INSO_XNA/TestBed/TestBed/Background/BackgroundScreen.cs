using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TestBed
{
	//class BackgroundGrass : Sprite
	//{
	//    const int VerticalOffset = 0;
	//    const float GrassHeight = 245;
	//    public BackgroundGrass() : base(InsoGame.Pixel)
	//    {
	//        _origin = new Vector2(0, 1);
	//        Position = new Vector2(- Globals.ScreenWidth * 0.5f, Globals.ScreenHeight * 0.5f + VerticalOffset);
	//        Color = new Color(0, 25, 0);
	//        _scale = new Vector2(Globals.ScreenWidth, GrassHeight);
	//    }
	//}

	//public class Background
	//{
	//    //Don't change this
	//    public const int AmountOfBackgroundLayers = 3;

	//    const int ExtraBackgroundOffsetIncrement = 00;
	//    const int ExtraBackgroundOffset = -120;
	//    const int BaseBackgroundOffset = -50;

	//    public static Camera2D GrassCamera = new Camera2D(Vector2.Zero);

	//    //Sprite _backgroundSprite;
	//    //BackgroundGrass _backgroundGrass;

	//    //Backgrounds to draw
	//    BackgroundObjectManager[] _backgrounds;

	//    public Background()
	//        : base()
	//    {
	//        _backgroundSprite = new Sprite(TextureLibrary.GetSpriteSheet("background"));
	//        _backgroundSprite.Origin = Vector2.Zero;
          
	//        GrassCamera = new Camera2D(new Vector2(Globals.ScreenWidth * 0.5f, Globals.ScreenHeight * 0.5f));

	//        LayerCameras = new BackgroundCamera[AmountOfBackgroundLayers];
	//        for (int i = 0; i < AmountOfBackgroundLayers; ++i)
	//            LayerCameras[i] = new BackgroundCamera(i);
	//    }

	//    public static BackgroundObjectManager[] CreateClouds(Vector2 size, Vector2 position, float cloudRatio)
	//    {
	//        BackgroundObjectManager[] backgrounds = new BackgroundObjectManager[AmountOfBackgroundLayers];
	//        //for (int i = backgrounds.Length - 1; i > -1 ; --i)
	//        for (int i = 0; i < backgrounds.Length; ++i)
	//        {
	//            //Sets the right index so that the furthest layer (at the back) is created first.
	//            //This helps with the immediate draw order.
	//            int index = backgrounds.Length - i - 1;
	//            backgrounds[index] = new BackgroundObjectManager(size, i, LayerCameras[i]);
	//            backgrounds[index].ObjectSpacing = new Vector2(400, 100);
	//            backgrounds[index].PlaceObjects(cloudRatio, StyleManager.LevelStyle.Clouds);
	//            backgrounds[index].Position = position;
	//        }
	//        return backgrounds;
	//    }

	//    public void CreateStartScreenBackgrounds(StartScreen startScreen)
	//    {
	//        _backgrounds = CreateHorizontalBackgrounds(StyleManager.LevelStyle.Country, startScreen.TotalWidth, startScreen.Origin.ScreenX + BackgroundObjectManager.DefaultSpacingX * 0.5f);
	//    }
	//    public void CreateFirstLevelBackgrounds(StyleManager.LevelStyle style, Level level, StartScreen startScreen, House endHouse)
	//    {
	//        int startHouseBackgroundAmount = (int)startScreen.TotalWidth / BackgroundObjectManager.DefaultSpacingX;
	//        int endHouseBackgroundAmount = (int)endHouse.TotalWidth / BackgroundObjectManager.DefaultSpacingX;
	//        int levelBackgroundAmount = (int)level.PlayAreaWidth / BackgroundObjectManager.DefaultSpacingX;
	//        CreateLevelBackgrounds(style, -startScreen.TotalWidth, startHouseBackgroundAmount, endHouseBackgroundAmount, levelBackgroundAmount);
	//    }
	//    public void CreateNextLevelBackgrounds(StyleManager.LevelStyle style, Level level, House startHouse, House endHouse)
	//    {
	//        int startHouseBackgroundAmount = (int)startHouse.TotalWidth / BackgroundObjectManager.DefaultSpacingX;
	//        int endHouseBackgroundAmount = (int)endHouse.TotalWidth / BackgroundObjectManager.DefaultSpacingX;
	//        int levelBackgroundAmount = (int)level.PlayAreaWidth / BackgroundObjectManager.DefaultSpacingX;
	//        CreateLevelBackgrounds(style, -startHouse.TotalWidth, startHouseBackgroundAmount, endHouseBackgroundAmount, levelBackgroundAmount);
	//    }
	//    private void CreateLevelBackgrounds(StyleManager.LevelStyle style, float offset, int startHouseBackgroundAmount, int endHouseBackgroundAmount, int levelBackgroundAmount)
	//    {
	//        BackgroundObjectManager[] newBackgrounds = new BackgroundObjectManager[AmountOfBackgroundLayers];
			
	//        int totalBackgroundAmount = startHouseBackgroundAmount + endHouseBackgroundAmount + levelBackgroundAmount;

	//        //for (int i = backgrounds.Length - 1; i > -1 ; --i)
	//        for (int i = 0; i < newBackgrounds.Length; ++i)
	//        {
	//            //Sets the right index so that the furthest layer (at the back) is created first.
	//            //This helps with the immediate draw order.
	//            int index = newBackgrounds.Length - i - 1;
	//            newBackgrounds[index] = new BackgroundObjectManager(new Vector2(totalBackgroundAmount, 1), i, LayerCameras[i]);
	//            newBackgrounds[index].CopyObjectsRow(_backgrounds[index], (int)_backgrounds[index].MaxBackgroundObjects.X - startHouseBackgroundAmount, 0, startHouseBackgroundAmount);
	//            newBackgrounds[index].PlaceObjects(1, StyleManager.LevelStyle.Country, startHouseBackgroundAmount, (int)(newBackgrounds[index].MaxBackgroundObjects.X - startHouseBackgroundAmount));
	//            newBackgrounds[index].X = offset;
	//            newBackgrounds[index].Y = BaseBackgroundOffset + (i / (float)AmountOfBackgroundLayers) * ExtraBackgroundOffset + ExtraBackgroundOffsetIncrement * i;
	//        }
	//        _backgrounds = newBackgrounds;
	//    }


	//    public static BackgroundObjectManager[] CreateHorizontalBackgrounds(StyleManager.LevelStyle style, float foregroundLength, float position)
	//    {
	//        BackgroundObjectManager[] backgrounds = new BackgroundObjectManager[AmountOfBackgroundLayers];
	//        //for (int i = backgrounds.Length - 1; i > -1 ; --i)
	//        for(int i = 0; i < backgrounds.Length; ++i)
	//        {
	//            //Sets the right index so that the furthest layer (at the back) is created first.
	//            //This helps with the immediate draw order.
	//            int index = backgrounds.Length - i - 1;
	//            backgrounds[index] = new BackgroundObjectManager(new Vector2(foregroundLength / BackgroundObjectManager.DefaultSpacingX, 1), i, LayerCameras[i]);
	//            backgrounds[index].PlaceObjects(1, StyleManager.LevelStyle.Country);
	//            backgrounds[index].X = position;
	//            backgrounds[index].Y = BaseBackgroundOffset + (i / (float)AmountOfBackgroundLayers) * ExtraBackgroundOffset + ExtraBackgroundOffsetIncrement * i;
	//        }
	//        return backgrounds;
	//    }

	//    public void Initialise()
	//    {
	//        _backgroundGrass = new BackgroundGrass();
	//        GrassCamera.PanSharpness = 1.0f;           
	//        GrassCamera.ZoomSharpness = 1;
	//        //GrassCamera.Zoom(0.5f);
	//    }

	//    public void Update(GameTime gameTime)
	//    {
	//        GrassCamera.SetTargetPosition(new Vector2(0, World.cam_Main.ScreenY * LayerCameras[AmountOfBackgroundLayers - 1].ZoomLevel));
	//        GrassCamera.Update();
	//        GrassCamera.UpdateMatrix();

	//        for (int i = 0; i < AmountOfBackgroundLayers; ++i)
	//        {
	//            LayerCameras[i].Update();
	//            LayerCameras[i].UpdateMatrix();
	//        }
	//    }

	//    public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
	//    {
	//        spriteBatch.Begin();
	//        _backgroundSprite.Draw(spriteBatch);
	//        spriteBatch.End();

	//        spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, GrassCamera.CameraMatrix);
	//        _backgroundGrass.Draw(spriteBatch);
	//        spriteBatch.End();

	//        for (int i = 0; i < _backgrounds.Length; ++i)
	//        {
	//            _backgrounds[i].Draw(spriteBatch);
	//        }
	//    }
    //}
}
