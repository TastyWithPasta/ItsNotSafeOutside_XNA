using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PastaGameLibrary;

namespace TestBed
{
    public enum BackpackerState
    {
        Idle,
        MovingOut,
        Waving,
        InGameplay,
        Jumping,
        Dead,
        Gone,
    }

    public class Backpacker : GameObject
    {
		public static ColliderGroup HitPlayerColliders = new ColliderGroup();

        static Random random = new Random();

        const int TravellerSpacing = 35;
        const int StartOffset = -200;
        const int MoveOutSpeed = 10;

        float m_targetX;
        float m_jumpVelocity;
        bool m_isLeader;
        Level m_level;
        BackpackerState m_currentState;
        BackpackerGroup m_group;
		SingleActionManager m_actionManager;
		AABBCollider m_collider;

        public Backpacker(SpriteSheet spriteSheet)
            : base()
        {
			m_actionManager = new SingleActionManager();
			m_sprite = new Sprite(Globals.TheGame, spriteSheet, Transform);
			m_collider = new AABBCollider(this, new AABB(Transform, m_sprite.Dimensions));
            m_currentState = BackpackerState.Idle;
        }

        public bool IsLeader
        {
            get { return m_isLeader; }
            set { m_isLeader = value; }
        }
        public bool IsMovingOut
        {
            get { return m_currentState == BackpackerState.MovingOut; }
        }
        public bool IsJumping
        {
            get { return m_currentState == BackpackerState.Jumping; }
        }
        public bool IsIdle
        {
            get
            {
                return m_currentState == BackpackerState.Idle;
            }
        }
        public bool IsDead
        {
            get { return m_currentState == BackpackerState.Dead || m_currentState == BackpackerState.Gone; }
        }
        public bool IsInGameplay
        {
            get { return m_currentState == BackpackerState.InGameplay; }
        }
        public bool IsOnTarget
        {
            get { return Math.Abs(Transform.PosX - m_targetX) < 1; }
        }

        public void InitializeInGroup(Level level, BackpackerGroup group)
        {
			Transform.PosX = 0;
			Transform.PosY = 0;
            m_level = level;
            m_group = group;
			Transform.ParentTransform = group.Transform;
        }
        public void PlaceOnTarget()
        {
			Transform.PosX = m_targetX;
        }
        public void SetTargetLinePosition(int index)
        {
            m_targetX = -index * TravellerSpacing + random.Next(-5, 6);            
        }

        private void Die()
        {
			m_actionManager.StartNew(new Animation(m_sprite, 6, 7, 300, true));
            //UnbindParent();
            m_currentState = BackpackerState.Dead;
            m_group.UpdatePartyLeader();
        }

        public void DoCollisions(GameTime gameTime)
        {
			HitPlayerColliders.DoCollision(m_collider, CollisionResponse);
        }
        public void CollisionResponse(Collider other)
        {
			//if (other.Owner is Bat)
			//    {
			//        Bat temp = (Bat)other.Owner;
			//        temp.Hit(AttackManager.Finisher);
			//    }

			//if (other.Owner is Enemy)
			//{
				
                
			//    else
			//    {
			//        GameObject temp = (Enemy)other.Owner;
			//        temp(30, -12, 0);
			//    }
                Die();
                return;
            //}
			//else if (otherActor is TombStone)
			//{
			//    TombStone tombStone = (TombStone)otherActor;
			//    tombStone.UpdateSpawning(gameTime);
			//}
        }

        public void SetAtStart(House startHouse)
        {
            if (!IsDead)
            {
				Transform.ParentTransform = null;
                m_currentState = BackpackerState.Idle;

				if (startHouse == null)
				{
					Transform.PosX = -100;
				}
				else
				{
					Transform.PosX = startHouse.HouseExitPoint;
				}
            }
        }
        public void SetMovingOut(House startHouse)
        {
			//if (!IsDead)
			//{
			//    m_currentState = BackpackerState.MovingOut;
			//    m_targetX += startHouse.RallyPoint;
			//    m_actionManager.StartNew(new Animation(m_sprite, 0, 1, 0.09f, true));
			//}
        }
        public void SetMovingIn(House finishHouse)
        {
            if (!IsDead)
            {
                m_currentState = BackpackerState.MovingOut;
               // PlaceBetween(finishHouse.HouseSprite, finishHouse.EntranceMask);
                m_actionManager.StartNew(new Animation(m_sprite, 0, 1, 0.05f, true));
                m_targetX = finishHouse.HouseExitPoint;
				Transform.ParentTransform = null;
            }
        }
        public void SetWaving()
        {
            if (!IsDead)
            {
                m_currentState = BackpackerState.Waving;
				m_actionManager.StartNew(new Animation(m_sprite, 2, 3, 0.15f, true));
            }
        }
        public void SetGameplay()
        {
            if (!IsDead)
            {
				Transform.ParentTransform = m_group.Transform;
                m_currentState = BackpackerState.InGameplay;
                m_actionManager.StartNew(new Animation(m_sprite, 0, 1, 0.5f, true));
                //CurrentAnimation.SetCurrentFrame(1);
            }
        }
        public void SetJumping()
        {
            if (!IsDead)
            {
                m_currentState = BackpackerState.Jumping;
                m_jumpVelocity = -20 + random.Next(-10, 10);
                //CurrentAnimation = new Animation("all", 100, 0, 6);
				m_sprite.SetFrame(5);
                //CurrentAnimation.Pause();
            }
        }

        public override void Update()
        {
			m_actionManager.Update();

            if (m_currentState == BackpackerState.InGameplay
                && !IsDead)
            {
                Transform.PosX += (m_targetX - Transform.PosX) * 0.1f;
            }
            else if (m_currentState == BackpackerState.Dead)
            {
                Transform.PosY -= 3;
                if (m_sprite.Alpha > 0)
					m_sprite.Alpha -= 0.01f;
                //else
                //    _currentState = BackpackerState.Gone;
            }

            else if (m_currentState == BackpackerState.MovingOut)
            {
                if (Transform.PosX < m_targetX)
					Transform.PosX += MoveOutSpeed;
				if (Transform.PosX >= m_targetX)
                {
					Transform.PosX = m_targetX;
					m_actionManager.Stop();
                    m_sprite.SetFrame(0);
                }
            }

            else if (m_currentState == BackpackerState.Jumping)
            {
                Transform.PosY += m_jumpVelocity;
                m_jumpVelocity += 4;
				if (Transform.PosY > 0)
                {
					Transform.PosY = 0;
                    m_currentState = BackpackerState.Idle;
					m_actionManager.Stop();
					m_sprite.SetFrame(0);
                }      
            }
        }
		public override void Draw()
		{
			m_sprite.Draw();
		}
    }
}
