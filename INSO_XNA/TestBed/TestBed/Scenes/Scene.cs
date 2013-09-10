using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PastaGameLibrary;

namespace TestBed
{
	public class Scene
	{
		public enum SceneState
		{
			Inactive, //Screen is neither drawn nor updated
			Transitioning,
			Active,
			Locked, //Screen is drawn but mostly not updated
		}

		protected Scene _otherScreen;
		protected SceneState _screenState = SceneState.Inactive;
		protected SceneState _nextState;

		public SceneState State
		{
			get { return _screenState; }
		}

		public Scene()
		{ }

		public virtual void Transition(SceneState stateToTransitionTo, Scene otherScreen)
		{
			_otherScreen = otherScreen;
			_screenState = SceneState.Transitioning;
			_nextState = stateToTransitionTo;
		}

		public virtual void Update(GameTime gameTime)
		{
			switch (_screenState)
			{
				case SceneState.Active:
					UpdateActive(gameTime);
					break;
				case SceneState.Locked:
					UpdateLocked(gameTime);
					break;
				case SceneState.Transitioning:
					UpdateTransition(gameTime);
					break;
			}
		}

		protected virtual void UpdateTransition(GameTime gameTime)
		{
			_screenState = _nextState;
		}
		protected virtual void UpdateActive(GameTime gameTime)
		{ }
		protected virtual void UpdateLocked(GameTime gameTime)
		{ }

		public virtual void Draw(SpriteBatch spriteBatch)
		{

		}
	}

	public class SceneManager
	{
		protected Scene[] _screens;

		public void Update(GameTime gameTime)
		{
			for (int i = 0; i < _screens.Length; ++i)
				if (_screens[i].State != Scene.SceneState.Inactive)
					_screens[i].Update(gameTime);
		}
		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < _screens.Length; ++i)
				if (_screens[i].State != Scene.SceneState.Inactive)
					_screens[i].Draw(spriteBatch);
		}

		/// <summary>
		/// Exits from Screen 1 to Screen 2
		/// </summary>
		/// <param name="screen1">Name of Screen 1 (Plays Outro)</param>
		/// <param name="screen2">Name of Screen 2 (Plays Intro)</param>
		public void Transition(Scene screen1, Scene screen2)
		{
			screen1.Transition(Scene.SceneState.Inactive, screen2);
			screen2.Transition(Scene.SceneState.Active, screen1);
		}
		/// <summary>
		/// Locks a scene while another appears.
		/// </summary>
		/// <param name="screenToLock">Name of the screen to lock. (Plays Lock)</param>
		/// <param name="newScreen">Name of the screen to appear. (Plays Intro)</param>
		public void Intro_Lock(Scene screenToLock, Scene newScreen)
		{
			screenToLock.Transition(Scene.SceneState.Locked, newScreen);
			newScreen.Transition(Scene.SceneState.Active, screenToLock);
		}
		/// <summary>
		/// Unlocks a previously locked scene and transitions out of the currently displayed screen.
		/// </summary>
		/// <param name="screenToUnlock"></param>
		/// <param name="oldScreen"></param>
		public void Outro_Unlock(Scene screenToUnlock, Scene oldScreen)
		{
			screenToUnlock.Transition(Scene.SceneState.Locked, oldScreen);
			oldScreen.Transition(Scene.SceneState.Inactive, screenToUnlock);
		}
	}
}
