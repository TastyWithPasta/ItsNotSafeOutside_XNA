using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PastaGameLibrary
{
	public delegate void VoidDelegate();
	public class ActiveList
	{
		private PTimer m_timer;
		protected List<VoidDelegate> m_delegates;
		private MyGame m_theGame;

		public MyGame TheGame
		{
			get { return m_theGame; }
		}

		public ActiveList(MyGame theGame, float updateTickInSeconds)
		{
			m_theGame = theGame;
			m_timer = new PTimer(theGame.TimerManager, OnUpdate);
			m_timer.Interval = updateTickInSeconds;
		}
		public virtual void Add(VoidDelegate newTask)
		{
			m_delegates.Add(newTask);
		}
		public virtual void Remove(VoidDelegate taskToRemove)
		{
			m_delegates.Remove(taskToRemove);
		}

		protected virtual void OnUpdate()
		{
			for (int i = 0; i < m_delegates.Count; ++i)
				m_delegates[i]();
		}
	}
}
