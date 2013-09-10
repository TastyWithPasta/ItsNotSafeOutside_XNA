using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PastaGameLibrary;


namespace TestBed
{
	//public class StartScreen : DynamicMenuScene
	//{
	//    const float LeftCameraBound = -200.0f;
	//    const float BottomCameraBound = 100.0f;
	//    const float TransitionTimeInSeconds = 1.0f;

	//    IPInterpolation<float> _interpolator;
	//    Vector2 _cameraPositionCache;
	//    const float HighScoreTransitionTime = 1.0f;
	//    const int StartScreenLength = 15;

	//    //The position of the origin of the menu
	//    const float OriginX = -EarthTile.EarthTileWidth * StartScreenLength;
	//    const float OriginY = 0;

	//    Entity _origin;
	//    Sprite _billboard;
	//    Sprite[] _ground = new Sprite[StartScreenLength];
	//    int _totalWidth;
	//    BackgroundObjectManager[] _backgrounds;
	//    BackgroundObjectManager[] _clouds;
	//    float _transitionTimer;

	//    //HighScoreTable _highscores;

	//    public Entity Origin
	//    {
	//        get { return _origin; }
	//    }
	//    public int TotalWidth
	//    {
	//        get { return _totalWidth; }
	//    }

	//    public StartScreen() : base()
	//    {
	//        Texture2D texture;
	//        Sprite sprite;

	//        _origin = new Entity();
	//        _origin.Position = new Vector2(OriginX, OriginY);

	//        _interpolator = new PSineInterpolationf();

	//        int width = EarthTile.EarthTileWidth;
	//        _totalWidth = EarthTile.EarthTileWidth * StartScreenLength;
	//        Hotspot.BindParent(_origin);
	//        Hotspot.X = _totalWidth * 0.5f;
	//        Hotspot.Y = -200;
	//        for(int i = 0; i < StartScreenLength; ++i) {
	//            texture = EarthTile.GroundSectionTextures[Globals.random.Next(0, EarthTile.GroundSectionTextures.Length - 1)];
	//            sprite = new Sprite(House.GroundTexture);
	//            sprite.Origin = Vector2.Zero;
	//            sprite.BindParent(_origin);
	//            sprite.Width = width;
	//            sprite.X = width * i;
	//            sprite.Y = -40;
	//            _ground[i] = sprite;
	//        }
	//        _billboard = new Sprite(XNAWP_TextureLibrary.TextureLibrary.GetSpriteSheet("titlebillboard"));
	//        _billboard.Origin = new Vector2(_billboard.BaseSourceWidth * 0.5f, _billboard.BaseSourceHeight);
	//        _billboard.BindParent(_origin);
	//        _billboard.X = _totalWidth * 0.5f;
	//        _billboard.Y = 0;
	//        _screenState = ScreenState.Inactive;

	//        _topBound = -300;
	//        _rightBound = 500;
	//    }

	//    public void Reset()
	//    {
	//        INSOSceneManager.BackgroundElements.CreateStartScreenBackgrounds(this);
	//        _backgrounds = Background.Background.CreateHorizontalBackgrounds(StyleManager.LevelStyle.Country, TotalWidth, _origin.ScreenX + BackgroundObjectManager.DefaultSpacingX * 0.5f);
	//        _clouds = Background.Background.CreateClouds(new Vector2(3, 2), _origin.Position + new Vector2(300, -150), 0.85f);
	//    }

	//    public override void Transition(Scene.ScreenState stateToTransitionTo, Scene otherScreen)
	//    {
	//        _transitionTimer = 0;
	//        if (stateToTransitionTo == ScreenState.Active)
	//        {
	//            if (otherScreen == null)
	//            {
	//                World.cam_Main.LoseTarget();
	//                World.cam_Main.SetTarget(Hotspot);
	//                World.cam_Main.PanSharpness = 0.1f;
	//            }
	//            else if(otherScreen is ScoreScene)
	//            {
	//                World.cam_Main.LoseTarget();
	//                World.cam_Main.BindParent(Hotspot);
	//                _cameraPositionCache = World.cam_Main.Position;
	//                World.cam_Main.PanSharpness = 0.1f;
	//            }
	//        }
	//        base.Transition(stateToTransitionTo, otherScreen);
	//    }
	//    protected override void UpdateActive(Microsoft.Xna.Framework.GameTime gameTime)
	//    {
	//        base.SlideInBoundaries();
	//        if (World.cam_Main.TargetPosition.X == _rightBound)
	//        {
	//            INSOSceneManager.StartGame();
	//            return;
	//        }
	//        else if (World.cam_Main.TargetPosition.Y == _topBound)
	//        {
	//            INSOSceneManager.StartScreenToHighScore();
	//            return;
	//        }
	//    }

	//    private void SlideCamera()
	//    {
	//        INSOSceneManager.SlideMenuView(this, ScreenState.Inactive, ScreenState.Active);
	//        float x = World.cam_Main.TargetPosition.X;
	//        float y = World.cam_Main.TargetPosition.Y;

	//        if (y > BottomCameraBound) 
	//            y = BottomCameraBound;
	//        if (x < LeftCameraBound) 
	//            x = LeftCameraBound;

	//        World.cam_Main.SetTargetPosition(x, y);
	//    }

	//    protected override void UpdateTransition(GameTime gameTime)
	//    {
	//        _transitionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
	//        if (_otherScreen is GameScene)
	//        {
	//            _screenState = _nextState;
	//            return;
	//        }
	//        if (_otherScreen == null)
	//        {
	//            if (Vector2.Distance(World.cam_Main.TargetPosition, World.cam_Main.Position) < 100)
	//                _screenState = _nextState;
	//            return;
	//        }
	//        if (_nextState == ScreenState.Active)
	//        {
	//            float ratio = _transitionTimer / TransitionTimeInSeconds;
	//            if (ratio >= 1.0f)
	//            {
	//                _screenState = _nextState;
	//                return;
	//            }
	//            float x = _interpolator.GetInterpolation(_cameraPositionCache.X, 0, ratio);
	//            float y = _interpolator.GetInterpolation(_cameraPositionCache.Y, 0, ratio);
	//            World.cam_Main.SetTargetPosition(x, y);
	//        }
	//        else if (_nextState == ScreenState.Inactive)
	//        {
	//            if (_otherScreen.State == ScreenState.Active)
	//            {
	//                _screenState = _nextState;
	//                return;
	//            }
	//        }
	//    }

	//    public override void Draw(SpriteBatch spriteBatch)
	//    {
			

	//        //for (int i = 0; i < _backgrounds.Length; ++i)
	//        //{
	//        //    _backgrounds[i].Draw(spriteBatch);
	//        //}
	//        for (int i = 0; i < _clouds.Length; ++i)
	//        {
	//            _clouds[i].Draw(spriteBatch);
	//        }
			
	//        spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, World.cam_Main.CameraMatrix);
	//        //spriteBatch.Begin();
	//        base.Draw(spriteBatch);

	//        for (int i = 0; i < _ground.Length; ++i)
	//        {
	//            _ground[i].Draw(spriteBatch);
	//        }
	//        _billboard.Draw(spriteBatch);

	//        spriteBatch.End();
	//    }
	//}
}
