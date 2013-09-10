using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PastaGameLibrary;

namespace TestBed
{
    public class NewLevelButton : Button
    {
        Counter _generation, _section;

        const float FreeSpinTime = 0.5f;
        const float ScrollSpeed = 5.0f;
        float _freeSpinTimer;

		SingleActionManager m_popAction;
		Animation m_animation;
		MoveToStaticAction m_in;
		MoveToStaticAction m_out;


        public NewLevelButton()
            : base(new Sprite(Globals.TheGame, TextureLibrary.GetSpriteSheet("btn_newlevel", 1, 4), new Transform()))
        {
			m_animation = new Animation(m_buttonSprite, 0, 3, 0.5f, true);
			m_popAction = new SingleActionManager();

			m_in = new MoveToStaticAction(Globals.TheGame, m_transform, new Vector2(150, 300), false);
			m_out = new MoveToStaticAction(Globals.TheGame, m_transform, new Vector2(150, 0), false);
			m_transform.Position = new Vector2(150, 300);

			_generation = new Counter(2, TextureLibrary.GetSpriteSheet("levelscore_vertical", 20, 1));
			_generation.Transform.ParentTransform = m_transform;
			
			_section = new Counter(1, TextureLibrary.GetSpriteSheet("levelscore_vertical", 20, 1));
			_section.Transform.ParentTransform = m_transform;

			_generation.Alpha = _section.Alpha = 1.0f;

            _generation.ScrollSpeed = _section.ScrollSpeed = 50;
        }

        public void SetCounters(int generation, int section)
        {
            _generation.Value = generation;
            _section.Value = section;
        }

		public bool Finished
		{
			get { return m_popAction.IsActive; }
		}
		public void PopIn()
		{
			m_popAction.StartNew(m_in);
		}
        public void PopOut()
        {
            _section.FreeSpin = true;
            _generation.FreeSpin = true;
			m_popAction.StartNew(m_out);
            _freeSpinTimer = FreeSpinTime;
        }

        public override void OnValidate()
        {
            base.OnValidate();
			//INSOSceneManager.NewLevelScreen.Transition(Scene.SceneState.Inactive, null);
            //INSOSceneManager.GameScreen.StartNextLevel();
        }

        public void Update()
        {
			m_animation.Update();
			m_popAction.Update();
            _freeSpinTimer -= (float)Globals.TheGame.ElapsedTime;
            if (_freeSpinTimer < 0 && _generation.FreeSpin)
            {
                _generation.FreeSpin = false;
                _section.FreeSpin = false;
            }
        }

        public void Draw()
        {
            _generation.Draw();
            _section.Draw();
        }
    }

    public class NewLevelScene : DynamicMenuScene
    {
        NewLevelButton _newLevelButton;
        
        public NewLevelScene()
            : base()
        {
            _newLevelButton = new NewLevelButton();
            _newLevelButton.Transform.ParentTransform = Hotspot;
        }

        public void SetStageNumber(int generation, int section)
        {
            _newLevelButton.SetCounters(generation, section);
        }

        public void Transition(Scene.SceneState stateToTransitionTo, Scene otherScreen)
        {
            base.Transition(stateToTransitionTo, otherScreen);
			if (stateToTransitionTo == SceneState.Active)
                _newLevelButton.PopOut();
			else if (stateToTransitionTo == SceneState.Inactive)
                _newLevelButton.PopIn();
        }

        protected override void UpdateTransition(GameTime gameTime)
        {
            _newLevelButton.Update(gameTime);

			if (_newLevelButton.Finished)
				_screenState = _nextState;
        }
        protected override void UpdateActive(GameTime gameTime)
        {
            _newLevelButton.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
			spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, World.cam_Main.CameraMatrix);
            _newLevelButton.Draw();
            spriteBatch.End();
        }
    }
}
