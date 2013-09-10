using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PastaGameLibrary;

namespace TestBed
{
    public class Button : IPDrawable
    {
        public enum ButtonState
        {
            Released,
            Pushed,
            Locked,
        }

        protected Vector2 m_touchPosition, m_previousTouchPosition;
		protected Transform m_transform;
        protected Sprite m_buttonSprite;
        protected ButtonState m_currentState;
		protected AABB m_aabb;

        public Button(Sprite sprite) 
            : base()
        {
			m_buttonSprite = sprite;
			m_transform = m_buttonSprite.Transform;
			m_aabb = new AABB(m_transform, m_buttonSprite.Dimensions);
        }

		public Transform Transform
		{
			get { return m_transform; }
		}

        public Sprite Buttonsprite
        {
            get { return m_buttonSprite; }
        }

        public void Update(GameTime gameTime)
        {
            m_previousTouchPosition = TouchInput.PreviousCameraTouchPosition;
            m_touchPosition = TouchInput.CameraTouchPosition;

            switch (m_currentState)
            {
                case ButtonState.Pushed:
                    UpdatePush(gameTime);
                    break;
                case ButtonState.Released:
                    UpdateRelease(gameTime);
                    break;
            }
        }
        public virtual void Draw()
        {
            m_buttonSprite.Draw();
        }

        public virtual void UpdateRelease(GameTime gameTime)
        {
            if (TouchInput.IsScreenTapped &&
                m_aabb.GetBounds().Contains((int)m_touchPosition.X, (int)m_touchPosition.Y))
                OnPush();
        }
        public virtual void UpdatePush(GameTime gameTime)
        {
            if (TouchInput.ScreenIsNoLongerTouched)
            {
                if (m_aabb.GetBounds().Contains((int)m_previousTouchPosition.X, (int)m_previousTouchPosition.Y))
                    OnValidate();
                else
                    OnRelease();
            }
            else if(!TouchInput.IsScreenTouched)
            {
				if (!m_aabb.GetBounds().Contains((int)m_touchPosition.X, (int)m_touchPosition.Y))
                    OnRelease();
            }
        }
        public virtual void OnValidate()
        {
            OnRelease();
        }
        public virtual void OnPush()
        {
            if (m_currentState == ButtonState.Released)
                m_transform.ScaleUniform = 0.8f;
            m_currentState = ButtonState.Pushed;
        }
        public virtual void OnRelease()
        {
            if (m_currentState == ButtonState.Pushed)
				m_transform.ScaleUniform = 1.0f;
            m_currentState = ButtonState.Released;
        }
        public virtual void Lock()
        {
            m_currentState = ButtonState.Locked;
        }
        public virtual void Unlock()
        {
            m_currentState = ButtonState.Released;
        }
    }
}
