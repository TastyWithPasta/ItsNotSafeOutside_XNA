using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PastaGameLibrary;

namespace TestBed
{
	//public class HighScoreEntry : GameObject
	//{

	//    public static SpriteFont HighScoreRankFont;
	//    public static SpriteFont HighScoreNameFont;
	//    public static SpriteFont HighScoreValueFont;

	//    TextActor _rank = new TextActor(HighScoreRankFont);
	//    TextActor _name = new TextActor(HighScoreNameFont);
	//    TextActor _score = new TextActor(HighScoreValueFont);

	//    const int NameOffset = 60;
	//    const int ScoreOffset = 360;

	//    public float Middle
	//    {
	//        get { return (NameOffset + ScoreOffset) * 0.5f; }
	//    }

	//    public HighScoreEntry(HighScore score)
	//        : base()
	//    {
	//        _rank.Text = score.rank.ToString();
	//        _rank.OriginRatio = new Vector2(0, 0);
	//        _name.Text = score.name;
	//        _name.OriginRatio = new Vector2(0, 0);
	//        _score.Text = score.score.ToString();
	//        _score.OriginRatio = new Vector2(0, 0);

	//        _rank.BindParent(this);
	//        _rank.Position = Vector2.Zero;
	//        _name.BindParent(this);
	//        _name.Position = new Vector2(NameOffset, 0);
	//        _score.BindParent(this);
	//        _score.Position = new Vector2(ScoreOffset, 0);
	//    }

	//    public void Draw(SpriteBatch spriteBatch)
	//    {
	//        _rank.Draw(spriteBatch, HighScoreRankFont);
	//        _name.Draw(spriteBatch, HighScoreNameFont);
	//        _score.Draw(spriteBatch, HighScoreValueFont);
	//    }
	//}

	//public class HighScoreTable : Entity
	//{
	//    const int ScoreSpacing = 75;
	//    List<HighScoreEntry> _highscores;

	//    public HighScoreTable()
	//        : base()
	//    {
	//    }

	//    public float MiddleHorizontal
	//    {
	//        get { return _highscores[0].Middle; }
	//    }
	//    public float Top
	//    {
	//        get { return 0; }
	//    }
	//    public float Bottom
	//    {
	//        get{ return _highscores.Count * ScoreSpacing; }
	//    }

	//    public void LoadHighScores()
	//    {
	//        _highscores = new List<HighScoreEntry>();
	//        List<HighScore> highscoreData = HighScoreManager.HighScores;

	//        for (int i = 0; i < highscoreData.Count; ++i)
	//        {
	//            HighScoreEntry entry = new HighScoreEntry(highscoreData[i]);
	//            entry.BindParent(this);
	//            entry.X = 0;
	//            entry.Y = i * ScoreSpacing;
	//            _highscores.Add(entry);
	//        }
	//    }

	//    public void Draw(SpriteBatch spriteBatch)
	//    {
	//        for (int i = 0; i < _highscores.Count; ++i)
	//        {
	//            _highscores[i].Draw(spriteBatch);
	//        }
	//    }
	//}

	//public class HighScoreScene : DynamicMenuScene
	//{
	//    const float TransitionTimeInSeconds = 1;

	//    bool _firstTime = true;
	//    IPInterpolation<float> _interpolator;
	//    Vector2 _cameraPositionCache;
	//    float _transitionTimer;
	//    HighScoreTable _table;

	//    public HighScoreScene(StartScreen startScreen)
	//    {
	//        _interpolator = new PSineInterpolationf();
	//        _table = new HighScoreTable();
	//        _table.BindParent(startScreen.Hotspot);
	//        _table.X = -100;
	//        _table.Y = -3000;
	//        Hotspot.BindParent(startScreen.Hotspot);
	//        Hotspot.X = 0;
	//        Hotspot.Y = -200;

	//    }

	//    public void Reset()
	//    {
	//        _firstTime = true;
	//    }

	//    public override void Transition(ScreenState stateToTransitionTo, Scene otherScreen)
	//    {
	//        _transitionTimer = 0;

	//        if (_firstTime)
	//        {
	//            _table.LoadHighScores();
	//            _firstTime = false;
	//        }

	//        if(stateToTransitionTo == ScreenState.Active)
	//        {
	//            World.cam_Main.SetTarget(_table);
	//            _cameraPositionCache = World.cam_Main.Position;
	//            World.cam_Main.SetTargetPosition(_cameraPositionCache);
	//            //World.cam_Main.SetTarget(_table);
	//            //World.cam_Main.PanSharpness = 0.02f;
	//        }
			
	//        base.Transition(stateToTransitionTo, otherScreen);
	//    }

	//    protected override void UpdateActive(GameTime gameTime)
	//    {
	//        base.UpdateActive(gameTime);
	//        if (TouchInput.IsScreenTouched)
	//        {
	//            World.cam_Main.PanSharpness = 0.1f;
	//            World.cam_Main.MoveTargetPosition(new Vector2(0, TouchInput.TouchDifference.Y * -1.5f));
	//        }
	//        if (World.cam_Main.TargetPosition.Y > _table.Bottom)
	//        {
	//            World.cam_Main.SetTargetPosition(World.cam_Main.X, _table.Bottom);
	//            INSOSceneManager.HighScoreToStartScreen();
	//        }

	//        if (World.cam_Main.TargetPosition.Y < _table.Top)
	//        {
	//            World.cam_Main.SetTargetPosition(World.cam_Main.X, _table.Top);
	//        }
	//    }

		

	//    protected override void UpdateTransition(GameTime gameTime)
	//    {
	//        _transitionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
	//        if (_nextState == ScreenState.Active)
	//        {
	//            float ratio = _transitionTimer / TransitionTimeInSeconds;
	//            if (ratio >= 1.0f)
	//            {
	//                _screenState = _nextState;
	//                return;
	//            }
	//            float x = _interpolator.GetInterpolation(_cameraPositionCache.X, _table.MiddleHorizontal, ratio);
	//            float y = _interpolator.GetInterpolation(_cameraPositionCache.Y, _table.Top, ratio);
	//            World.cam_Main.SetTargetPosition(x, y);
	//        }
	//    }

	//    public override void Draw(SpriteBatch spriteBatch)
	//    {
	//        spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, World.cam_Main.CameraMatrix);
	//        _table.Draw(spriteBatch);
	//        spriteBatch.End();	
	//    }
	//}
}
