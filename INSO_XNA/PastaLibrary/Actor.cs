using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PastaGameLibrary
{
    public class Actor
    {
		static uint currentID = 0;
		uint _ID = 0;

		MyGame m_theGame;
		Actor m_parent = null;
		Transform m_transform;

		List<Actor> m_children = new List<Actor>();

        public Actor(MyGame theGame) 
        {
			m_theGame = theGame;

			//Sets the entity's ID to a unique ID.
			_ID = currentID;
			++currentID;
			m_parent = null;

			m_transform = new Transform();
        }
        public void Dispose()
        {
			UnbindParent();
			UnbindChildren();
        }

		public int ID
		{
			get { return ID; }
		}
		public Actor Parent
		{
			get { return m_parent; }
		}
		public List<Actor> Children
		{
			get { return m_children; }
		}
		public MyGame TheGame
		{
			get { return m_theGame; }
		}

		public bool BindParent(Actor parent)
		{
			if (m_parent == parent)
				return false;
			if (m_parent != null)
				UnbindParent();
			m_parent = parent;
			m_parent.BindChild(this);
			m_transform.ParentTransform = parent.Transform;
			return true;
		}
		public bool BindChild(Actor child)
		{
			if (m_children.Contains(child))
				return false;
			m_children.Add(child);
			child.BindParent(this);
			return true;
		}
		public void UnbindParent()
		{
			if (m_parent == null)
				return;
			Actor temp = m_parent;
			m_parent = null;
			temp.UnbindChild(this);
		}
		public void UnbindChild(Actor child)
		{
			if (!m_children.Contains(child))
			{
				return;
			}
			m_children.Remove(child);
			child.UnbindParent();
		}
		public void UnbindChildren()
		{
			while(m_children.Count > 0)
			{
				UnbindChild(m_children[0]);
			}
		}

		//public void StartAll()
		//{
		//    for (int i = 0; i < m_activeActions.Count; ++i)
		//        m_activeActions[i].Start();
		//}
		//public void StopAll()
		//{
		//    for (int i = 0; i < m_activeActions.Count; ++i)
		//        m_activeActions[i].Stop();
		//}

		public Transform Transform
		{
			get { return m_transform; }
		}

		//Two distinct types of methods since drawing and updating are handled separately by XNA.
		public virtual void Update()
		{
			//OnUpdate();
		}
		public virtual void Draw()
		{
			//OnDraw();
		}
	}
}
