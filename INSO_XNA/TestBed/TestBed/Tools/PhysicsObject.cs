using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PastaGameLibrary;

namespace TestBed
{
    public class PhysicsComponent : IPUpdatable
    {
		MyGame m_myGame;		
		Transform m_transform;

        private float m_stillnessTimer;
		private float m_previousY;
		public float StillnessTime = 0.3f;
        public int GroundLevel = 0;
        public float Mass, Friction, Restitution, AirFriction;
		private float m_angleIncrement;
		private bool m_isProjected;
		private Vector2 m_velocity;

		public bool IsProjected
		{
			get { return m_isProjected; }
		}

        public PhysicsComponent(MyGame theGame, Transform transform)
        {
			m_myGame = theGame;
			m_transform = transform;
            Mass = 1;
            Friction = 1;
            Restitution = 1;
            AirFriction = 1;
        }

		public PhysicsComponent(PhysicsComponent objectToCopy, Transform transform)
		{
			m_myGame = objectToCopy.m_myGame;
			m_transform = transform;
			Mass = objectToCopy.Mass;
			Friction = objectToCopy.Friction;
			Restitution = objectToCopy.Restitution;
			AirFriction = objectToCopy.AirFriction;
		}

        public void Update()
        {
            if (m_isProjected)
                UpdateProjectionPhysics();
        }      
        public void Throw(float fx, float fy, float angleIncrementRange)
        {
                m_stillnessTimer = StillnessTime;
                m_isProjected = true;
                m_velocity.X = fx;
                m_velocity.Y = fy;

                if (angleIncrementRange != 0)
                {
					m_transform.Direction += (float)(Globals.Random.NextDouble() * 0.5 - 0.25);
					m_angleIncrement = (float)(Globals.Random.NextDouble() * angleIncrementRange - angleIncrementRange * 0.5);
                }
                else
                   m_angleIncrement = 0;
        }
		public void Stop()
		{
			m_isProjected = false;
			m_velocity = Vector2.Zero;
			m_angleIncrement = 0;
		}
        private void UpdateProjectionPhysics()
        {
			float dt = (float)Globals.TheGame.ElapsedTime;
			m_previousY = m_transform.PosY;

			m_transform.Direction += m_angleIncrement * dt;

			m_velocity.Y += 9.8f * Mass * dt; //294 = 9.8 * 3 * 10 => 1m = 20 px
            m_velocity *= AirFriction;
			m_transform.PosX += m_velocity.X * dt;
			m_transform.PosY += m_velocity.Y * dt;

			if (m_transform.PosY >= GroundLevel)
            {
                m_velocity.X *= 0.5f * Friction;
                m_angleIncrement *= 0.5f;
            }
			if (m_transform.PosY > GroundLevel)
            {
				m_transform.PosY = GroundLevel;
                m_velocity.Y *= -0.4f * Restitution;
            }

			if (m_previousY == m_transform.PosY)
            {
				m_stillnessTimer -= dt;
                if (m_stillnessTimer < 0)
                    m_isProjected = false;
            }
            
        }

        //private void UpdateProjectionPhysics()
        //{
        //    _velocity.X *= 0.0995f * _friction;
        //    _velocity.Y += 0.1f * _mass;
        //    X += _velocity.X;
        //    Y += _velocity.Y;
        //    Angle += _angleIncrement;

        //    if (Y >= 0)
        //    {
        //        _angleIncrement *= 0.5f;
        //        _velocity.X *= 0.08f * _friction;
        //    }
        //    if (Y > 0)
        //    {
        //        Y = 0;
        //        _velocity.Y *= -0.05f * _restitution;
        //    }
        //    if (_velocity.Length() < 0.75f)
        //    {
        //        Y = 0;
        //        _isProjected = false;
        //    }
        //}

     
            //switch (_currentState)
            //{
            //    case BodyPartState.Popped:
            //        Angle += _angleIncrement;
            //        _velocity.Y += 2.0f;
            //        X += _velocity.X;
            //        Y += _velocity.Y;

            //        if (Y >= 0)
            //        {
            //            _velocity.X *= 0.5f;
            //            _angleIncrement *= 0.5f;
            //            //_scale.X -= _shrinkSpeed;
            //            //_scale.Y -= _shrinkSpeed;
            //        }   
            //        if (Y > 0)
            //        {
            //            Y = 0;
            //            _velocity.Y *= -0.4f;                       
            //        }
                                       

            //        if (_scale.X < 0)
            //            _scale = Vector2.Zero;

            //        break;
            //}
    }
}
