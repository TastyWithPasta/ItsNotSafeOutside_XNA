using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PastaGameLibrary;

namespace TestBed
{
    public static class INSOSceneManager
    {
        public static GameScene GameScene;
		public static EndLevelScene EndLevelScene;
        
        //public static ShopScreen ShopScreen;

        static Scene[] Screens; //All screens 

        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            //GameScene = new GameScene(graphicsDevice);
            //GameScene.Initialize();
        }

		public static void LaunchGame()
		{
			//StartScreen.Reset();
			//StartScreen.Transition(Scene.SceneState.Active, null);
		}

		public static void StartGame()
		{
			//GameScreen.Transition(Scene.SceneState.Active, StartScreen);
			//GameScreen.StartFirstLevel();
			////GameScreen.StartNewLevel();
			//StartScreen.Transition(Scene.SceneState.Locked, GameScreen);
		}

		public static void StartScreenToHighScore()
		{
			//StartScreen.Transition(Scene.SceneState.Inactive, HighScoreScreen);
			//HighScoreScreen.Transition(Scene.SceneState.Active, StartScreen);
		}
		public static void HighScoreToStartScreen()
		{
			//StartScreen.Transition(Scene.SceneState.Active, null);
			//HighScoreScreen.Transition(Scene.SceneState.Inactive, null);
		}
		public static void LaunchScoreScreen()
        {
			//ShopScreen.ResetShopScreen();
			//ScoreScreen.ScoreIncreased = false;
			//ScoreScreen.Transition(Scene.SceneState.Active, null);
        }
        public static void RetractScoreScreen()
        {
			//ScoreScreen.Transition(Scene.SceneState.Inactive, null);
        }
    }
}
