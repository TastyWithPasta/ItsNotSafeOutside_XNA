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
		bool m_paused = false;
		float m_intervalInSeconds = 1;
		float m_timeRemainingInSeconds = 0;
		PTimerManager m_manager = null;

		public bool Paused
		{
			get { return m_paused; }
			set { m_paused = value; }
		}
		public float ProgressRatio
		{
			get { return  1 - m_timeRemainingInSeconds / m_intervalInSeconds; }
			set { m_timeRemainingInSeconds = (1 - value) * m_intervalInSeconds; }
		}
		public float Interval 
		{
			get { return m_intervalInSeconds; }
			set { m_intervalInSeconds = value; }
		}

		public PTimer(PTimerManager manager, TimerTask task)
		{
			m_manager = manager;
			m_manager.AddTimer(this);
			m_task = task;
		}

		void Delay(float delayTimeInSeconds)
		{
			m_timeRemainingInSeconds -= delayTimeInSeconds; 
		}
		void Reset()
		{
			m_timeRemainingInSeconds = 0;
		}

		internal void Update(float elapsedTime)
		{
			m_timeRemainingInSeconds -= elapsedTime;
			while (m_timeRemainingInSeconds < 0)
			{
				if(m_task != null)
					m_task();
				m_timeRemainingInSeconds += m_intervalInSeconds;
			}
		}

		public void Dispose()
		{
			m_manager.RemoveTimer(this);
		}
	}
}
