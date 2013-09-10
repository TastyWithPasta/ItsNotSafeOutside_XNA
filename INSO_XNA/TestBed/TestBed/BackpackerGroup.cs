using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PastaGameLibrary;

namespace TestBed
{
    public class BackpackerGroup
    {
        public static Backpacker PartyLeader;
        Backpacker[] _backpackers;
		Transform m_transform;

		public Transform Transform
		{
			get { return m_transform; }
		}

        public BackpackerGroup(Level level, Backpacker[] backpackers) 
            : base()
        {
			m_transform = new Transform();
            _backpackers = backpackers;
            for (int i = 0; i < _backpackers.Length; ++i)
                _backpackers[i].InitializeInGroup(level, this);
            UpdatePartyLeader();
        }

        public void SetPartyLeader(int index)
        {
            for (int i = 0; i < _backpackers.Length; ++i)
                _backpackers[i].IsLeader = false;
            _backpackers[index].IsLeader = true;
        }

        public void Update(GameTime gameTime)
        {
            PartyLeader.DoCollisions(gameTime);
        }
        public void UpdatePartyLeader()
        {
            PartyLeader = null;

            for (int i = 0; i < _backpackers.Length; ++i)
                if (!_backpackers[i].IsDead)
                {
                    if (PartyLeader == null)
                    {
                        PartyLeader = _backpackers[i];
                        PartyLeader.IsLeader = true;
                    }
                    else
                        _backpackers[i].IsLeader = false;
                }
                else
                    _backpackers[i].IsLeader = false;
        }
    }
}
