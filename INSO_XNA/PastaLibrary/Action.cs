using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PastaGameLibrary
{
	public abstract class Action
	{
		bool m_isActive = false;
		bool m_isPaused = false;
		public bool IsActive
		{
			get { return m_isActive; }
		}

		public void Start()
		{
			if (m_isActive)
				Stop();
			m_isActive = true;
			m_isPaused = false;
			OnStart();
		}
		public void Stop()
		{
			m_isActive = false;
			OnStop();
		}
		public void Pause()
		{
			m_isPaused = true;
			OnPause();
		}
		public void Update()
		{
			if(!m_isActive || m_isPaused)
				return;
			OnUpdate();
		}

		protected abstract void OnUpdate();
		protected virtual void OnStart() { }
		protected virtual void OnStop() { }
		protected virtual void OnPause() { }
		protected virtual void OnResume() { }
	}
	public abstract class DurationAction : Action, IDisposable
	{
		MyGame m_theGame = null;
		PTimer m_timer = null;
		IPInterpolation<float> m_interpolator = new PLerpInterpolation();
		bool m_isLooping;

		public DurationAction(MyGame theGame, bool isLooping)
			: base()
		{
			m_theGame = theGame;
			PTimer.TimerTask task;
			if (isLooping)
				task = Start;
			else task = Stop;
			m_timer = new PTimer(m_theGame.TimerManager, task);
			m_isLooping = isLooping;
		}

		public IPInterpolation<float> Interpolator
		{
			get { return m_interpolator; }
			set { m_interpolator = value; }
		}
		public PTimer Timer
		{
			get { return m_timer; }
		}
		public MyGame TheGame
		{
			get { return m_theGame; }
		}
		public bool IsLooping
		{
			get { return m_isLooping; }
		}

		protected override void OnStart()
		{
			base.OnStart();
			m_timer.Start();
		}

		protected override void OnPause()
		{
			base.OnPause();
			m_timer.Pause();
		}

		protected override void OnStop()
		{
			base.OnStop();
			m_timer.Stop();
		}

		public void Dispose()
		{
			m_timer.Dispose();
		}
	}
	public class MethodAction : Action
	{
		public delegate void ActionMethod();
		ActionMethod m_actionMethod;

		public MethodAction(ActionMethod actionMethod)
			: base()
		{
			m_actionMethod = actionMethod;
		}

		protected override void OnUpdate()
		{
			//throw new NotImplementedException();
		}

		protected override void OnStart()
		{
			base.OnStart();
			m_actionMethod();
		}
	}
	public class Animation : DurationAction, IDisposable
	{
		int m_startFrame, m_endFrame;
		Sprite m_sprite;

		public int StartFrame
		{
			get { return m_startFrame; }
		}
		public int EndFrame
		{
			get { return m_endFrame; }
		}

		public Animation(Sprite sprite, int startFrame, int endFrame, float intervalInSeconds, bool isLooping) 
			 : base(sprite.TheGame, isLooping)
		{
			m_sprite = sprite;
			m_startFrame = startFrame;
			m_endFrame = endFrame;
			Timer.Interval = intervalInSeconds;
		}

		protected override void OnUpdate()
		{
			float interpolation = Interpolator.GetInterpolation(m_startFrame, m_endFrame + 1, Timer.ProgressRatio);
			int currentFrame = (int)interpolation;
			if (currentFrame <= m_endFrame)
				m_sprite.SetFrame(currentFrame);
		}
	}
	public class DelayAction : DurationAction
	{
		public DelayAction(MyGame theGame)
			: base(theGame, false)
		{}

		protected override void OnUpdate()
		{}
	}

	public class MoveToStaticAction : DurationAction, IDisposable
	{
		Transform m_transform = null;
		Vector2 m_target, m_startPos;

		public Vector2 Target
		{
			get { return m_target; }
			set { m_target = value; }
		}
		public Vector2 StartPosition
		{
			get { return m_startPos; }
			set { m_startPos = value; }
		}

		public MoveToStaticAction(MyGame theGame, Transform transform, Vector2 target, bool isLooping)
			: base(theGame, isLooping)
		{
			m_transform = transform;
			m_startPos = m_transform.Position;
			m_target = target;
		}

		protected override void OnUpdate()
		{
			float progressRatio = Timer.ProgressRatio;

			m_transform.Position = new Vector2(Interpolator.GetInterpolation(m_startPos.X, m_target.X, progressRatio),
												Interpolator.GetInterpolation(m_startPos.Y, m_target.Y, progressRatio));
		}
	}
	public class MoveToDynamicAction : DurationAction, IDisposable
	{
		Transform m_transform = null;
		Transform m_target;
		Vector2 m_startPos;

		public MoveToDynamicAction(MyGame theGame, Transform transform, Transform target, bool isLooping)
			: base(theGame, isLooping)
		{
			m_transform = transform;
			m_startPos = m_transform.Position;
			m_target = target;
		}
		protected override void OnUpdate()
		{
			float progressRatio = Timer.ProgressRatio;

			Vector2 targetPos = m_target.PositionGlobal;
			m_transform.Position = new Vector2(Interpolator.GetInterpolation(m_startPos.X, targetPos.X, progressRatio),
												Interpolator.GetInterpolation(m_startPos.Y, targetPos.Y, progressRatio));
		}
	}

	public class ScaleToAction : DurationAction, IDisposable
	{
		Transform m_transform = null;
		Vector2 m_target, m_startScale;

		public ScaleToAction(MyGame theGame, Transform transform, Vector2 target, bool isLooping)
			: base(theGame, isLooping)
		{
			m_transform = transform;
			m_startScale = m_transform.Scale;
			m_target = target;
		}
		protected override void OnUpdate()
		{
			float progressRatio = Timer.ProgressRatio;

			m_transform.Scale = new Vector2(Interpolator.GetInterpolation(m_startScale.X, m_target.X, progressRatio),
												Interpolator.GetInterpolation(m_startScale.Y, m_target.Y, progressRatio));
		}
	}
	public class RotateToStaticAction : DurationAction, IDisposable
	{
		float m_rotationTarget, m_startRot;
		Transform m_transform;

		public RotateToStaticAction(MyGame theGame, Transform transform, float target, bool isLooping)
			: base(theGame, isLooping)
		{
			m_transform = transform;
			m_startRot = (float)m_transform.Direction;
			m_rotationTarget = target;
		}
		protected override void OnUpdate()
		{
			m_transform.Direction = Interpolator.GetInterpolation(m_startRot, m_rotationTarget, Timer.ProgressRatio);
		}
	}
	public class Sequence : Action
	{
		List<Action> m_actions;
		int m_currentActive;

		protected override void OnStart()
		{
			m_actions[0].Start();
			m_currentActive = 0;
		}
		protected override void OnStop()
		{
			m_actions[m_currentActive].Stop();
		}
		protected override void OnUpdate()
		{
			m_actions[m_currentActive].Update();
			if (!m_actions[m_currentActive].IsActive)
			{
				m_currentActive++;
				if (m_currentActive >= m_actions.Count)
					Stop();
			}
		}
	}


	//A single action is active at any given time.
	public class SingleActionManager : IPUpdatable
	{
		Action m_currentAction;

		public void StartNew(Action currentAction)
		{
			if(m_currentAction != null)
				m_currentAction.Stop();
			m_currentAction = currentAction;
			m_currentAction.Start();
		}
		public bool IsActive
		{
			get { return m_currentAction.IsActive; }
		}
		public void Stop()
		{
			m_currentAction.Stop();
		}
		public void Start()
		{
			m_currentAction.Start();
		}
		public void Update()
		{
			if (m_currentAction != null)
				m_currentAction.Update();
		}
	}

	//Actions are destroyed when stopped. Normally, they will play only once.
	public class ExpendableActionManager : IPUpdatable
	{
		List<Action> m_expendableActions = new List<Action>();

		public void Add(Action action)
		{
			m_expendableActions.Add(action);
		}
		public void Remove(Action action)
		{
			m_expendableActions.Remove(action);
		}
		public void ClearAll()
		{
			m_expendableActions.Clear();
		}
		public void StartAll()
		{
			for (int i = 0; i < m_expendableActions.Count; ++i)
				m_expendableActions[i].Start();
		}
		public void Update()
		{
			for (int i = m_expendableActions.Count - 1; i > -1; --i)
			{
				if (m_expendableActions[i].IsActive)
					m_expendableActions[i].Update();
				else
					m_expendableActions.RemoveAt(i);
			}
		}
	}

	//Actions will not be destroyed when stopped.
	public class ConcurrentActionManager : IPUpdatable
	{
		List<Action> m_concurrentActions = new List<Action>();

		public void StartNew(Action action)
		{
			m_concurrentActions.Add(action);
			action.Start();
		}
		public void ClearAll()
		{
			m_concurrentActions.Clear();
		}
		public void StartAll()
		{
			for (int i = 0; i < m_concurrentActions.Count; ++i)
				m_concurrentActions[i].Start();
		}
		public void StopAll()
		{
			for (int i = 0; i < m_concurrentActions.Count; ++i)
				m_concurrentActions[i].Stop();
		}
		public void Update()
		{
			for (int i = 0; i < m_concurrentActions.Count; ++i)
				m_concurrentActions[i].Update();
		}
	}
}
