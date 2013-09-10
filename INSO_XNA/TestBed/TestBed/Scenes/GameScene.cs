using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PastaGameLibrary;

namespace TestBed
{
    public class GameScene : Scene
    {
		const float TransitionTimeInSeconds = 5.0f;

        const float WalkSpeed = 3.0f;
        const int MoveOutInterval = 100;
        const int WavingWait = 500;
        const int Pan1Wait = 1000;
        const int NinjaWaveWait = 2000;
        const int ClearWait = 2000;
        const int JumpInterval = 100;
        const int WindowInterval = 700;
        const int CameraOffsetX = 280;
        const int CameraOffsetY = -100;
        const float LevelPanSpeed = 15;

        public enum GameState
        {
            Intro_PanStart,
            Intro_MoveOut,
            Intro_WaitWave,
            Intro_Wave,
            Intro_Pan1,
            Intro_NinjaWave,
            Intro_Pan2,
            Intro_Start,

            Gameplay,

            Outro_Clear,
            Outro_Jump,
            Outro_EnterHouse,
            Outro_Windows,
            Outro_TempWait,

            LevelCleared,
        }

        GraphicsDevice _device;
        Backpacker[] _backpackers;
        BackpackerGroup _backpackerGroup;

        GameState _currentState;

        

		bool _firstLevel;
        int _eventTimer;
        float _levelPanRatio;
        House _startHouse, _finishHouse;
		Vector2 _cameraHotSpot;

		float _transitionTimer;
		Vector2 _cameraPositionCache;
		IPInterpolation<float> _interpolator;


        public Backpacker[] Backpackers
        {
            get { return _backpackers; }
        }

        public GameScene(GraphicsDevice device)
            : base()
        {
            _screenState = SceneState.Inactive;
            _device = device;
			_interpolator = new PSineInterpolation();
        }

        #region State Methods

        private void SkipIntro()
        {
            //PlaceNinjaOnFinish();

            //_playerHotspot.X = _startHouse.RallyPoint;
            //for (int i = 0; i < _backpackers.Length; ++i)
            //{
            //    _backpackers[i].X = _startHouse.RallyPoint;

            //}

            //SetPan2();
        }
        public void UpdateSkipIntro()
        {
			//if (TouchInput.IsScreenTapped)
			//    SkipIntro();
        }

        private void SetPan1()
        {
			//PlaceNinjaOnFinish();
			//_levelPanRatio = 1;
			//_currentState = GameState.Intro_Pan1;

			//World.cam_Main.LoseTarget();
			//_cameraHotSpot.X = World.cam_Main.X;
			//_cameraHotSpot.Y = World.cam_Main.Y;

			////_camera.ZoomSharpness = 0.01f;
			//World.cam_Main.Zoom(1f);
        }
        private void UpdatePan1(GameTime gameTime)
        {
			//float travelRatio = Math.Max(0, _cameraHotSpot.X / _finishHouse.X);
			//_levelPanRatio = 1 + travelRatio * 1.5f;
			//_cameraHotSpot.X += LevelPanSpeed * _levelPanRatio;
			//_cameraHotSpot.Y = CameraOffsetY;
			//World.cam_Main.SetTargetPosition(_cameraHotSpot);

			//if (_cameraHotSpot.X > _finishHouse.X)
			//    SetNinjaWave();

			//for (int i = 0; i < _backpackers.Length; ++i)
			//    _backpackers[i].Update(gameTime);
        }

        private void SetPan2()
        {
			//_currentState = GameState.Intro_Pan2;

			//World.cam_Main.ZoomSharpness = 0.1f;
			//World.cam_Main.Zoom(1.0f);
			//World.cam_Main.PanSharpness = 0.1f;

			//_cameraHotSpot.X = _startHouse.RallyPoint + CameraOffsetX;
			//_cameraHotSpot.Y = CameraOffsetY;
        }
        private void UpdatePan2(GameTime gameTime)
        {
			//World.cam_Main.SetTargetPosition(_cameraHotSpot);
			//for (int i = 0; i < _backpackers.Length; ++i)
			//    _backpackers[i].Update(gameTime);

			//if (World.cam_Main.HasReachedTarget)
			//    SetGameplay();
        }

        private void SetNinjaWave()
        {
			//_currentState = GameState.Intro_NinjaWave;

			//World.cam_Main.PanSharpness = 0.04f;
			//World.cam_Main.ZoomSharpness = 0.02f;
			//World.cam_Main.Zoom(1.4f);
			//_cameraHotSpot.X = _finishHouse.ScreenX + _finishHouse.NinjaHotspot.X;
			//_cameraHotSpot.Y = _finishHouse.ScreenY + _finishHouse.NinjaHotspot.Y + 100;
			//World.cam_Main.SetTargetPosition(_cameraHotSpot);
			//_eventTimer = NinjaWaveWait;
        }
        private void UpdateNinjaWave(GameTime gameTime)
        {
			//if (_eventTimer < 0)
			//    SetPan2();
        }

        private void SetGameplay()
        {
			//_ninja.CurrentAnimation = new Animation("normal", 300, 0, 1);

			//_backpackerGroup = new BackpackerGroup(CurrentLevel, _backpackers);
			//_backpackerGroup.X = _startHouse.RallyPoint;
			//for (int i = 0; i < _backpackers.Length; ++i)
			//{
			//    _backpackers[i].SetTargetLinePosition(i);
			//    _backpackers[i].PlaceOnTarget();
			//    _backpackers[i].SetGameplay();
			//}

			//_currentState = GameState.Gameplay;
			//World.cam_Main.PanSharpness = 0.5f;
			////GameCamera.Zoom(0.8f);
			//World.cam_Main.SetTarget(_backpackerGroup, new Vector2(CameraOffsetX, CameraOffsetY));
			//GameScore.PopOut();
			//GameCoinBar.PopOut();
        }
        private void UpdateGameplay(GameTime gameTime)
        {
			//_backpackerGroup.X += WalkSpeed;

			//if (_backpackerGroup.X > CurrentLevel.LevelWidth)
			//    SetOutroClear();

			//for (int i = 0; i < _backpackers.Length; ++i)
			//    _backpackers[i].Update(gameTime);

			//_backpackerGroup.Update(gameTime);
        }

        private void SetPanStart()
        {
			//for (int i = 0; i < _backpackers.Length; ++i)
			//{
			//    _backpackers[i].PlaceBehind(_startHouse);
			//    _backpackers[i].SetTargetLinePosition(i);
			//    _backpackers[i].SetAtStart(_startHouse);
			//}

			//_currentState = GameState.Intro_PanStart;
			//World.cam_Main.PanSharpness = 0.2f;
			//World.cam_Main.ZoomSharpness = 0.05f;
			//World.cam_Main.Zoom(1.3f);

			//World.cam_Main.SetTargetPosition(_startHouse.RallyPoint - 100, -60);
        }
        private void UpdatePanStart(GameTime gameTime)
        {
			//if (World.cam_Main.HasReachedTarget)
			//    SetMoveOut();
        }

        private void SetMoveOut()
        {
            _currentState = GameState.Intro_MoveOut;
            _eventTimer = -1;
        }
        private void UpdateMoveOut()
        {
			//if (_eventTimer < 0)
			//{
			//    int movedoutCount = 0;
			//    _eventTimer = MoveOutInterval;

			//    for (int i = 0; i < _backpackers.Length; ++i)
			//        if (_backpackers[i].IsMovingOut || _backpackers[i].IsDead)
			//            movedoutCount++;

			//    if (movedoutCount != 3 && !_backpackers[movedoutCount].IsDead)
			//        _backpackers[movedoutCount].SetMovingOut(_startHouse);

			//    int finishedCount = 0;
			//    for (int i = 0; i < _backpackers.Length; ++i)
			//        if (_backpackers[i].IsOnTarget || _backpackers[i].IsDead)
			//            finishedCount++;

			//    if (finishedCount == 3)
			//        SetWaveWait();
			//}
        }

        private void SetWaveWait()
        {
			//_currentState = GameState.Intro_WaitWave;
			//_eventTimer = WavingWait;
        }
        private void UpdateWaveWait()
        {
			//if (_eventTimer < 0)
			//    SetWaving();
        }

        private void SetWaving()
        {
			//for (int i = 0; i < _backpackers.Length; ++i)
			//    _backpackers[i].SetWaving();
			//_eventTimer = Pan1Wait;
			//_currentState = GameState.Intro_Wave;
        }
        private void UpdateWaving()
        {
			//if (_eventTimer < 0)
			//    SetPan1();
        }

        private void SetOutroClear()
        {
			//int aliveTravellers = 3;

			//for (int i = 0; i < _backpackers.Length; ++i)
			//    if (_backpackers[i].IsDead)
			//        aliveTravellers--;

			//_currentState = GameState.Outro_Clear;

			//if (CurrentLevel.LevelEnemyManager.FinishEnemies(_finishHouse))
			//    _eventTimer = ClearWait;
			//else
			//    _eventTimer = 0;

			//InsoSceneManager.ScoreScreen.AddToComboScore(GameMoon.CurrentCombo * LevelGenerator.CurrentGenerationNumber);
			//InsoSceneManager.ScoreScreen.AddToSurvivorScore(aliveTravellers);
			//InsoSceneManager.ScoreScreen.AddToPreviousLevelScore(GameScore.Value);
			//InsoSceneManager.NewLevelScreen.SetStageNumber(LevelGenerator.CurrentGenerationNumber, LevelGenerator.CurrentGenerationLevelIndex);

			//for (int i = 0; i < _backpackers.Length; ++i)
			//{
			//    _backpackers[i].CurrentAnimation.SetCurrentFrame(0);
			//    _backpackers[i].CurrentAnimation.Pause();
			//}
        }
        private void UpdateOutroClear(GameTime gameTime)
        {
			//if (_eventTimer < 0)
			//    SetOutroJump();

			//for (int i = 0; i < _backpackers.Length; ++i)
			//    _backpackers[i].Update(gameTime);
			//CurrentLevel.Update(gameTime);
        }

        private void SetOutroJump()
        {
			//_currentState = GameState.Outro_Jump;

			//GameChest.BindHouse(_finishHouse);
			//GameChest.PopOnGround();

			//GameScore.PopIn();
			//GameCoinBar.PopIn();
        }
        private void UpdateOutroJump(GameTime gameTime)
        {
            if (_eventTimer < 0)
            {
                _eventTimer = JumpInterval;

                int jumpingCount = 0;
                int finishCount = 0;

                for (int i = 0; i < _backpackers.Length; ++i)
                    if (_backpackers[i].IsJumping || _backpackers[i].IsIdle || _backpackers[i].IsDead)
                        jumpingCount++;
                if (jumpingCount != 3)
                    _backpackers[jumpingCount].SetJumping();

                for (int i = 0; i < _backpackers.Length; ++i)
                    if (_backpackers[i].IsIdle || _backpackers[i].IsDead)
                        finishCount++;
                if (finishCount == 3)
                    SetOutroEnterHouse();
            }
        }

        private void SetOutroEnterHouse()
        {
			//World.cam_Main.PanSharpness = 0.075f;
			//World.cam_Main.LoseTarget();
			//SwitchToMenu(InsoSceneManager.ScoreScreen);
			//for (int i = 0; i < _backpackers.Length; ++i)
			//    _backpackers[i].SetMovingIn(_finishHouse);
			//_currentState = GameState.Outro_EnterHouse;
			//InsoSceneManager.LaunchScoreScreen();
        }
        private void UpdateOutroEnterHouse()
        {
			//int finishCount = 0;
			//for (int i = 0; i < _backpackers.Length; ++i)
			//{
			//    if (_backpackers[i].IsOnTarget || _backpackers[i].IsDead)
			//        finishCount++;
			//}

			//if (finishCount == 3 && InsoSceneManager.ScoreScreen.State == SceneState.Active)
			//    SetOutroWindows();
        }

        private void SetOutroWindows()
        {
			//_currentState = GameState.Outro_Windows;
			//GameCoinBar.EmptyBar();
			//GameChest.OpenChest();
        }
        private void UpdateOutroWindows(GameTime gameTime)
        {
			//if (_eventTimer < 0)
			//{
			//    _eventTimer = WindowInterval;

			//    int aliveTravellers = 3;
			//    int currentWindow = _finishHouse.HouseSprite.CurrentAnimation.CurrentFrame;

			//    for (int i = 0; i < _backpackers.Length; ++i)
			//        if (_backpackers[i].IsDead)
			//            aliveTravellers--;

			//    if (currentWindow < aliveTravellers)
			//    {
			//        _finishHouse.HouseSprite.CurrentAnimation.SetCurrentFrame(currentWindow + 1);
			//        _finishHouse.HouseSprite.Update(gameTime);
			//    }
			//    else
			//        SetTempWait();
			//}
        }

        private void SetClearMe()
        {
            //_currentState = GameState.LevelCleared;
        }

        private void SetTempWait()
        {
			//World.cam_Main.PanSharpness = 0.25f;
			//_currentState = GameState.Outro_TempWait;
			//GameMoon.Disable();
        }
        private void UpdateTempWait(GameTime gameTime)
        {
			//if (TouchInput.IsScreenTouched 
			//    && !_currentMenuScreen.IsSlideDisabled())
			//{
			//    World.cam_Main.LoseTarget();
			//    World.cam_Main.MoveTargetPosition(TouchInput.TouchDifference * -1);
			//}
			//else if (TouchInput.ScreenIsNoLongerTouched)
			//{
			//    SwitchToMenu(GetClosestMenu());
			//}

        }

        #endregion



        private void PlaceNinjaOnFinish()
        {
			//_ninja.BindHouse(_finishHouse);
        }

        public void Initialize()
        {
            _backpackers = new Backpacker[3];
            _backpackers[0] = new Backpacker(TextureLibrary.GetSpriteSheet("backpacker_0"));
            _backpackers[1] = new Backpacker(TextureLibrary.GetSpriteSheet("backpacker_1"));
            _backpackers[2] = new Backpacker(TextureLibrary.GetSpriteSheet("backpacker_2"));

            //GameScore = new Score(10, TextureLibrary.GetSpriteSheet("counter_vertical"));
            //GameCoinBar = new CoinBar();
            //GameChest = new Chest();

            //GamePointIndicators = new PointIndicators();
            //ObjectTemplateManager.Initialize();
            //LevelGenerator = new LevelGenerator();
            //StartNewLevel();

			
        }
        public void LoadNewLevel()
        {
			Globals.CurrentLevel = LevelGenerator.GetNextLevel();
			//_currentState = GameState.Intro_MoveOut;
			//ObjectTemplateManager.Reset();
			//CurrentLevel = new Level(_device, LevelGenerator.GetNextLevel());
        }
        private void ResetCamera()
        {
			//World.cam_Main.LoseTarget();
			//World.cam_Main.PanSharpness = 1.0f;
			//World.cam_Main.SetTargetPosition(World.cam_Main.X - (CurrentLevel.LevelWidth + _startHouse.TotalWidth), World.cam_Main.Y);
			//World.cam_Main.PlaceOnTarget();
			//World.cam_Main.Update();
			//World.cam_Main.UpdateMatrix();
        }
		public void StartFirstLevel()
		{
			_firstLevel = true;
			StartNewLevel();
			//InsoSceneManager.BackgroundElements.CreateFirstLevelBackgrounds(StyleManager.LevelStyle.Country, CurrentLevel, InsoSceneManager.StartScreen, _finishHouse);
		}
		public void StartNextLevel()
		{
			_firstLevel = false;
			StartNewLevel();
			//InsoSceneManager.BackgroundElements.CreateNextLevelBackgrounds(StyleManager.LevelStyle.Country, CurrentLevel, _startHouse, _finishHouse);
			ResetCamera();
		}

		private void StartNewLevel()
        {
            //_boughtUpgrade = null;
            //_ninja.SetRunning();
            //GameScore.ResetValue();

            LoadNewLevel();

            //AttackManager.Initialise(CurrentLevel);

            _cameraHotSpot = Vector2.Zero;

            //Globals.GameMoon.ResetAll();

            //Initialize backpackers
            for (int i = 0; i < _backpackers.Length; ++i)
                _backpackers[i].PlaceOnTarget();

            SetPanStart();
        }

		public override void Transition(Scene.SceneState stateToTransitionTo, Scene otherScreen)
		{
			//if(otherScreen is StartScreen)
			//{
			//    World.cam_Main.LoseTarget();
			//    _cameraPositionCache = World.cam_Main.Position;
			//    World.cam_Main.PanSharpness = 0.1f;
			//}
			//base.Transition(stateToTransitionTo, otherScreen);
		}
        protected override void UpdateActive(GameTime gameTime)
        {
            //if (TouchInput.IsScreenTapped)
            //    _camera.SetTargetPosition(_camera.X + TouchInput.TouchPosition.X - Globals.ScreenWidth * 0.5f, _camera.Y);

            //_eventTimer -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            #region Huge Switch
			//switch (_currentState)
			//{
			//    case GameState.Gameplay:
			//        UpdateGameplay(gameTime);
			//        break;
			//    case GameState.Intro_PanStart:
			//        UpdatePanStart(gameTime);
			//        UpdateSkipIntro();
			//        break;
			//    case GameState.Intro_MoveOut:
			//        UpdateMoveOut(gameTime);
			//        UpdateSkipIntro();
			//        break;
			//    case GameState.Intro_WaitWave:
			//        UpdateWaveWait(gameTime);
			//        UpdateSkipIntro();
			//        break;
			//    case GameState.Intro_Wave:
			//        UpdateWaving(gameTime);
			//        UpdateSkipIntro();
			//        break;
			//    case GameState.Intro_Pan1:
			//        UpdatePan1(gameTime);
			//        UpdateSkipIntro();
			//        break;
			//    case GameState.Intro_Pan2:
			//        UpdatePan2(gameTime);
			//        break;
			//    case GameState.Intro_NinjaWave:
			//        UpdateNinjaWave(gameTime);
			//        UpdateSkipIntro();
			//        break;
			//    case GameState.Outro_Clear:
			//        UpdateOutroClear(gameTime);
			//        break;
			//    case GameState.Outro_Jump:
			//        UpdateOutroJump(gameTime);
			//        break;
			//    case GameState.Outro_EnterHouse:
			//        UpdateOutroEnterHouse(gameTime);
			//        break;
			//    case GameState.Outro_Windows:
			//        UpdateOutroWindows(gameTime);
			//        break;
			//    case GameState.Outro_TempWait:
			//        UpdateTempWait(gameTime);
			//        break;
            //}
            #endregion

			//if (_currentState != GameState.Gameplay)
			//{
			//    _ninja.Update(gameTime);
			//    if (_boughtUpgrade != null)
			//        _boughtUpgrade.Update(gameTime);
			//}

			//GameMoon.Update(gameTime);
			//GameScore.Update(gameTime);
			//GameCoinBar.Update(gameTime);
			//GamePointIndicators.Update(gameTime);
			//GameChest.Update(gameTime);
			//CoinManager.Update(gameTime);
			//ExplosionManager.Update(gameTime);
        }
		protected override void UpdateTransition(GameTime gameTime)
		{
			//_transitionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
			//if (_nextState == SceneState.Active)
			//{
			//    float ratio = _transitionTimer / TransitionTimeInSeconds;
			//    if (ratio >= 1.0f)
			//    {
			//        _screenState = _nextState;
			//        return;
			//    }
			//    float x = _interpolator.GetInterpolation(_cameraPositionCache.X, 0, ratio);
			//    float y = _interpolator.GetInterpolation(_cameraPositionCache.Y, 0, ratio);
			//    World.cam_Main.SetTargetPosition(x, y);
			//}
		}
		
		//public override void Draw(SpriteBatch spriteBatch)
		//{
		//    spriteBatch.Begin();
		//    GameMoon.Draw(spriteBatch);
		//    spriteBatch.End();

		//    spriteBatch.Begin(SpriteSortMode.FrontToBack, null, null, null, null, null, World.cam_Main.CameraMatrix);
		//    if(!_firstLevel)
		//        _startHouse.Draw(spriteBatch);
		//    _finishHouse.Draw(spriteBatch);
		//    CurrentLevel.Draw(spriteBatch);
		//    for (int i = 0; i < _backpackers.Length; ++i)
		//        _backpackers[i].Draw(spriteBatch);
		//    spriteBatch.End();


		//    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, World.cam_Main.CameraMatrix);
		//    CoinManager.Draw(spriteBatch);
		//    ExplosionManager.Draw(spriteBatch);
		//    if (_currentState == GameState.Gameplay)
		//    {
		//        AttackManager.Draw(spriteBatch);
		//    }
		//    else
		//    {
		//        if (_boughtUpgrade != null)
		//            _boughtUpgrade.Draw(spriteBatch);
		//        _ninja.Draw(spriteBatch);
		//    }
		//    GameChest.Draw(spriteBatch);
		//    spriteBatch.End();

		//    spriteBatch.Begin();
		//    GameScore.Draw(spriteBatch);
		//    GameCoinBar.Draw(spriteBatch);
		//    GamePointIndicators.Draw(spriteBatch);
		//    spriteBatch.End();
		//}
    }
}
