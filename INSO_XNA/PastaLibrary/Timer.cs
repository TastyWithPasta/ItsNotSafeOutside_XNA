using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PastaGameLibrary
{
	public class PTimerManager
	{
		static List<PTimer> timers = new List<PTimer>();

		public PTimerManager()
		{ }

		internal void AddTimer(PTimer timer)
		{
			timers.Add(timer);
		}
		internal void RemoveTimer(PTimer timer)
		{
			timers.Remove(timer);
		}
		public void UpdateTimers(GameTime gameTime)
		{
			float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
			for (int i = 0; i < timers.Count; ++i)
			{
				timers[i].Update(elapsedTime);  
			}
		}
	}

	public class PTimer : IDisposable
	{
		public delegate void TimerTask();
		TimerTask m_task;
		bool m_paused = true;
		float m_intervalInSeconds = 1;
		float m_timeRemainingInSeconds = 0;
		PTimerManager m_manager = null;

		public bool IsPaused
		{
			get { return m_paused; }
		}
		public float ProgressRatio
		{
			get { return  1 - m_timeRemainingInSeconds / m_intervalInSeconds; }
			set { m_timeRemainingInSeconds = (1 - value) * m_intervalInSeconds; }
		}
		public float Interval 
		{
			get { return m_intervalInSeconds; }
			set { 
				m_intervalInSeconds = value;
				if (m_paused)
					m_timeRemainingInSeconds = m_intervalInSeconds;
			}
		}

		public PTimer(PTimerManager manager, TimerTask task)
		{
			m_manager = manager;
			m_manager.AddTimer(this);
			m_task = task;
			m_timeRemainingInSeconds = m_intervalInSeconds;
		}

		public void Delay(float delayTimeInSeconds)
		{
			m_timeRemainingInSeconds -= delayTimeInSeconds; 
		}
		public void Start()
		{
			m_paused = false;
		}
		public void Pause()
		{
			m_paused = true;
		}
		public void Stop()
		{
			m_timeRemainingInSeconds = m_intervalInSeconds;
			m_paused = true;
		}

		internal void Update(float elapsedTime)
		{
			if (m_paused)
				return;
			m_timeRemainingInSeconds -= elapsedTime;
			while (m_timeRemainingInSeconds < 0)
			{
				m_timeRemainingInSeconds += m_intervalInSeconds;
				if(m_task != null)
					m_task();
			}
		}

		public void Dispose()
		{
			m_manager.RemoveTimer(this);
		}
	}
}
