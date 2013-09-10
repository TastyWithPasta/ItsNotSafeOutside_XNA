using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PastaGameLibrary
{
	delegate void CResponseDelegate();

	//interface IPCollider
	//{
	//    void DoCollisions(GameTime gameTime, IPCollider actor);
	//    bool TestCollision(IPCollider collider);
	//    void CollisionResponse(GameTime gameTime, IPCollider actor);
	//}

	
	public abstract class PComponent : IPComponent
	{
		bool m_enabled;
		IPActor m_container;

		public IPActor Container
		{
			get { return m_container; }
		}
		public bool Enabled
		{
			get { return m_enabled;	}
			set
			{
				m_enabled = value;
				if (m_enabled)
					OnEnable();
				else
					OnDisable();
			}
		}
		protected abstract void OnEnable();
		protected abstract void OnDisable();


		public virtual void Initialise()
		{ }

		public void Attach(IPActor container)
		{
			m_container = container;
			OnAttach(container);
		}
		protected abstract void OnAttach(IPActor container);
	}



	

}


