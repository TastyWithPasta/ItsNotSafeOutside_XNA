using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using PastaGameLibrary;

namespace TestBed
{
    public class CounterNumber
    {
		Transform m_transform;
		Sprite m_sprite;
		Rectangle m_srcCache;

        int m_direction;
        bool m_lockScrolling = false;
        float m_baseVelocity = 6;
        float m_velocity = 0, m_spinPos = 0;
        int m_halfTexture;
        int m_textureInterval;
        int m_value = 0;
        int m_textureTarget = 0;
        bool m_freeSpin = false;

		public Transform Transform
		{
			get { return m_transform; }
		}
        public bool FreeSpin
        {
            get { return m_freeSpin; }
            set
            {
                m_freeSpin = value;

                if (m_freeSpin == true)
                {
                    m_lockScrolling = true;
                    m_direction = 1;
                }
            }
        }
        public int Value
        {
            get { return m_value; }
            set
            {
                m_textureTarget = value * m_textureInterval;

                if (value - m_value > 0)
                {
                    m_direction = 1;
                    if (m_textureTarget > m_halfTexture)
                        m_textureTarget -= m_halfTexture;
                }
                else
                {
                    if (!m_lockScrolling)
                    {
                        m_direction = -1;
                        if (m_textureTarget < m_halfTexture)
                            m_textureTarget += m_halfTexture;
                    }
                    else
                        m_direction = 1;
                }
                
                m_value = value;
                m_velocity = m_baseVelocity * m_direction;
            }
        }

		public float Width
		{
			get { return m_sprite.Width; }
		}
		public float Height
		{
			get { return m_sprite.Height; }
		}
		public float Alpha
		{
			get { return m_sprite.Alpha; }
			set { m_sprite.Alpha = value; }
		}

        public void ForceValue(int newValue)
        {
            m_textureTarget = newValue * m_textureInterval;
            m_spinPos = m_textureTarget;
        }
        public void SetValue(int newValue, int direction)
        {
            m_textureTarget = newValue * m_textureInterval;
            if (direction < 0 && !m_lockScrolling)
            {
                m_direction = -1;
                if (m_textureTarget < m_halfTexture)
                    m_textureTarget += m_halfTexture;
            }
            else
            {
                m_direction = 1;
                if (m_textureTarget > m_halfTexture)
                    m_textureTarget -= m_halfTexture;
            }
            m_value = newValue;
            m_velocity = m_baseVelocity * m_direction;
        }
        public float ScrollSpeed
        {
            get { return m_baseVelocity; }
            set { m_baseVelocity = value; }
        }
        public bool LockScrolling
        {
            get { return m_lockScrolling; }
            set { m_lockScrolling = value; }
        }

        public CounterNumber(Counter parent, SpriteSheet texture)
        {
			m_transform = new Transform(parent.Transform, true);
			m_sprite = new Sprite(Globals.TheGame, texture, m_transform);
			m_srcCache = m_sprite.SourceRectangle;
			m_textureInterval = (int)m_sprite.Height;
			m_halfTexture = m_textureInterval / 2;
        }
        public void Update()
        {
            float gameSeconds = (float)Globals.TheGame.ElapsedTime;

            if (m_freeSpin)
            {
                m_spinPos += m_velocity;
                if (m_spinPos > m_halfTexture)
                    m_spinPos -= m_halfTexture;
                m_srcCache.Y = (int)m_spinPos;
                return;
            }

            if (m_direction == 1)
            {               
                if (m_spinPos > m_halfTexture)
                {
                    m_spinPos -= m_halfTexture;
                    if (m_spinPos > m_textureTarget && m_velocity <= m_baseVelocity)
                    {
                        m_spinPos = m_textureTarget;
                        m_velocity = 0;
                    }
                }
                if (m_spinPos < m_textureTarget
                    && m_spinPos + m_velocity > m_textureTarget)
                    m_velocity = (m_textureTarget - m_spinPos);
            }
            else
            {                
                if (m_spinPos < m_halfTexture)
                {
                    m_spinPos += m_halfTexture;
                    if (m_spinPos < m_textureTarget && m_velocity <= -m_baseVelocity)
                    {
                        m_spinPos = m_textureTarget;
                        m_velocity = 0;
                    }
                }

                if (m_spinPos > m_textureTarget
                    && m_spinPos + m_velocity < m_textureTarget)
                    m_velocity = (m_textureTarget - m_spinPos);
            }
            if ((int)m_spinPos == m_textureTarget)
                m_velocity = 0;
            m_spinPos += m_velocity;
			m_srcCache.Y = (int)m_spinPos;

			m_sprite.SourceRectangle = m_srcCache;
        }

		public void Draw()
		{
			m_sprite.Draw();
		}
    }

    public class Counter
    {
		Transform m_transform;
        CounterNumber[] _counterNumbers;
        bool _lockScrolling;
        int _value;
        int _maxValue;
        float _scrollSpeed;
        float _scale = 1;
        bool _visible = true;
        bool _freeSpin = false;

		public Transform Transform
		{
			get { return m_transform; }
		}
        public bool FreeSpin
        {
            get { return _freeSpin; }
            set
            {
                _freeSpin = value;
                for (int i = 0; i < _counterNumbers.Length; ++i)
                    _counterNumbers[i].FreeSpin = value;
            }
        }
        public int Value
        {
            get { return _value; }
            set
            {
                int previousValue = _value;

                if (value > _maxValue)
                    _value = _maxValue;
                else
                    _value = value;

                if (_value < previousValue)
                    UpdateValue(-1);
                else
                    UpdateValue(1);
            }
        }
        public float ScrollSpeed
        {
            get { return _scrollSpeed; }
            set
            {
                _scrollSpeed = value;
                for (int i = 0; i < _counterNumbers.Length; ++i)
                    _counterNumbers[i].ScrollSpeed = value;
            }
        }
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }
        public bool LockScrolling
        {
            get { return _lockScrolling; }
            set { _lockScrolling = value;
            for (int i = 0; i < _counterNumbers.Length; ++i)
                    _counterNumbers[i].LockScrolling = value;
            }
        }
        public float Alpha
        {
            get { return _counterNumbers[0].Alpha; }
            set
            {
                for (int i = 0; i < _counterNumbers.Length; ++i)
                    _counterNumbers[i].Alpha = value;
            }
        }
		public float Width
		{
			get
			{
				float totalWidth = 0;
				for (int i = 0; i < _counterNumbers.Length; ++i)
					totalWidth += _counterNumbers[i].Width;

				return totalWidth;
			}
		}
		public float Height
		{
			get
			{
				float maxHeight = 0;
				for (int i = 0; i < _counterNumbers.Length; ++i)
					if (_counterNumbers[i].Height > maxHeight)
						maxHeight = _counterNumbers[i].Height;

				return maxHeight;
			}
		}

        public void SetRandomScrollSpeed(int min, int max)
        {
            _scrollSpeed = max;
                for (int i = 0; i < _counterNumbers.Length; ++i)
                    _counterNumbers[i].ScrollSpeed = Globals.Random.Next(min, max);
        }

        public Counter(int length, SpriteSheet counterTexture)
            : base()
        {
            _counterNumbers = new CounterNumber[length];           
            _maxValue = (int)Math.Pow(10, length) - 1;

            for (int i = 0; i < length; ++i)
            {
                _counterNumbers[i] = new CounterNumber(this, counterTexture);                
                _counterNumbers[i].Alpha = 0.5f;
				_counterNumbers[i].Transform.ParentTransform = m_transform;
				_counterNumbers[i].Transform.PosX = i * _counterNumbers[i].Width;
            }
        }

        public void Update()
        {
            for (int i = 0; i < _counterNumbers.Length; ++i)
                _counterNumbers[i].Update();
        }
        public void Draw()
        {
            //_background.Draw(spriteBatch);
            if(_visible)
            for (int i = 0; i < _counterNumbers.Length; ++i)
                _counterNumbers[i].Draw();
        }
        public void ForceValue(int value)
        {
            _value = value;
            int valueCopy = _value;
            int currentValue = 0;

            for (int i = _counterNumbers.Length - 1; i > -1; --i)
            {
                currentValue = valueCopy / (int)Math.Pow(10, i);

                _counterNumbers[i].ForceValue(currentValue);
                valueCopy -= currentValue * (int)Math.Pow(10, i);
            }
        }
        private void UpdateValue(int direction)
        {
            int valueCopy = _value;
            int currentValue = 0;

            for (int i = _counterNumbers.Length - 1; i > -1; --i)
            {
                currentValue = valueCopy / (int)Math.Pow(10, i);

                _counterNumbers[i].SetValue(currentValue, direction);
                valueCopy -= currentValue * (int)Math.Pow(10, i);
            }
        }
    }
}
