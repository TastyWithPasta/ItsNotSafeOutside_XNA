using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PastaGameLibrary;

namespace TestBed
{
	//public class ScoreScene : DynamicMenuScene
	//{
	//    public static int TotalGameScore = 0;

	//    public const int HorizontalOffset = 175;
	//    const int ComboScoreMultiplier = 200;
	//    const int SurvivorScoreMultiplier = 300;
	//    const float ScoreDisplayTime = 0.5f;
	//    const float TotalScoreIdleTime = 1.0f;

	//    bool _scoreIncreased;
	//    float _eventTimer;

	//    FinalScore ComboScore, SurvivorScore, PreviousLevelScore;
	//    TotalScore FinalScore;
	//    FinalScore[] _scoresDisplayed;

	//    public bool ScoreIncreased
	//    {
	//        get { return _scoreIncreased; }
	//        set { _scoreIncreased = value; }
	//    }

	//    public ScoreScene()
	//    {
	//        ComboScore = new FinalScore(TextureLibrary.GetSpriteSheet("lbl_combo"));
	//        SurvivorScore = new FinalScore(TextureLibrary.GetSpriteSheet("lbl_combo"));
	//        PreviousLevelScore = new FinalScore(TextureLibrary.GetSpriteSheet("lbl_combo"));
	//        FinalScore = new TotalScore();
	//        _scoresDisplayed = new FinalScore[] {
	//        PreviousLevelScore,
	//        SurvivorScore, 
	//        ComboScore,
	//        };
	//    }

	//    public override void Transition(ScreenState stateToTransitionTo, Scene otherScreen)
	//    {
	//        base.Transition(stateToTransitionTo, otherScreen);
	//        if (stateToTransitionTo == ScreenState.Active)
	//            if (_scoreIncreased)
	//                PopOut();
	//            else
	//                IntroPopOut();
	//        else
	//            PopIn();
	//    }
	//    public void AddToComboScore(int value)
	//    {
	//        ComboScore.ResetValue();
	//        ComboScore.AddToScore(value * ComboScoreMultiplier);
	//    }
	//    public void AddToPreviousLevelScore(int value)
	//    {
	//        PreviousLevelScore.ResetValue();
	//        PreviousLevelScore.AddToScore(value);
	//    }
	//    public void AddToSurvivorScore(int amountOfSurvivors)
	//    {
	//        SurvivorScore.ResetValue();
	//        SurvivorScore.AddToScore(SurvivorScoreMultiplier * GameScene.LevelGenerator.CurrentGenerationNumber * amountOfSurvivors);
	//    }
	//    private void AddToTotalScore()
	//    {
	//        FinalScore.AddToScore(PreviousLevelScore.Value + ComboScore.Value + SurvivorScore.Value);
	//    }

	//    private void IntroPopOut()
	//    {
	//        _eventTimer = 0;
            
	//        for (int i = 0; i < _scoresDisplayed.Length; ++i)
	//        {
	//            _scoresDisplayed[i].PlaceOnScreen(i);
	//            _scoresDisplayed[i].FreeSpin = true;
	//        }
	//        FinalScore.PlaceOnScreen();
	//        _scoreIncreased = false;
	//    }
	//    private void PopIn()
	//    {
	//        _eventTimer = 0;
	//        for (int i = 0; i < _scoresDisplayed.Length; ++i)
	//            _scoresDisplayed[i].PopIn();
	//        FinalScore.PopIn();
	//    }
	//    private void PopOut()
	//    {
	//        for (int i = 0; i < _scoresDisplayed.Length; ++i)
	//            _scoresDisplayed[i].PopOut();
	//        FinalScore.PopOut();
	//    }

	//    private void SetActive()
	//    {
	//        //for (int i = 0; i < _scoresDisplayed.Length; ++i)
	//        //    _scoresDisplayed[i].FreeSpin = false;
	//        _eventTimer = TotalScoreIdleTime;
	//        _screenState = ScreenState.Active;
	//        FinalScore.PopOut();
	//    }
	//    protected override void UpdateTransition(GameTime gameTime)
	//    {
            

	//        float gameSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
	//        _eventTimer -= gameSeconds;

	//        if (_nextState == ScreenState.Active)
	//        {
	//            if (_scoreIncreased)
	//            {
	//                SetActive();
	//                return;
	//            }

	//            while (_eventTimer < 0)
	//            {
	//                _eventTimer += ScoreDisplayTime / _scoresDisplayed.Length;
	//                for (int i = 0; i < _scoresDisplayed.Length; ++i)
	//                    if (_scoresDisplayed[i].IsIn)
	//                    {
	//                        _scoresDisplayed[i].PopOut();
	//                        break;
	//                    }

	//            }

	//            bool allFinished = true;
	//            for (int i = 0; i < _scoresDisplayed.Length; ++i)
	//                if (!_scoresDisplayed[i].IsOut)
	//                {
	//                    allFinished = false;
	//                    break;
	//                }
	//                else
	//                    if (_scoresDisplayed[i].FreeSpin)
	//                        _scoresDisplayed[i].FreeSpin = false;

	//            if (allFinished)
	//                SetActive();
	//        }

	//        if (_nextState == ScreenState.Inactive)
	//        {
	//            bool allFinished = true;
	//            for (int i = 0; i < _scoresDisplayed.Length; ++i)
	//                if (!_scoresDisplayed[i].IsIn)
	//                {
	//                    allFinished = false;
	//                    break;
	//                }

	//            FinalScore.Update(gameTime);

	//            if (allFinished)
	//                _screenState = ScreenState.Inactive;
	//        }


	//        for (int i = 0; i < _scoresDisplayed.Length; ++i)
	//            _scoresDisplayed[i].Update(gameTime);
	//    }
	//    protected override void UpdateActive(GameTime gameTime)
	//    {
	//        base.UpdateActive(gameTime);
	//        float gameSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
	//        _eventTimer -= gameSeconds;

	//        if (_eventTimer < 0 && !_scoreIncreased)
	//        {
	//            _scoreIncreased = true;
	//            AddToTotalScore();
	//        }
	//        for (int i = 0; i < _scoresDisplayed.Length; ++i)
	//            _scoresDisplayed[i].Update(gameTime);

	//        FinalScore.Update(gameTime);
	//    }

	//    public override void Draw(SpriteBatch spriteBatch)
	//    {
	//        if (_screenState != ScreenState.Inactive)
	//        {
	//            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, World.cam_Main.CameraMatrix);
	//            for (int i = 0; i < _scoresDisplayed.Length; ++i)
	//                _scoresDisplayed[i].Draw(spriteBatch);
	//            FinalScore.Draw(spriteBatch);
	//            spriteBatch.End();
	//        }
	//    }
	//}
}
