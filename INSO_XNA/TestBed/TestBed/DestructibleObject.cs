using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PastaGameLibrary;

namespace TestBed
{
	public class MiniFire
	{
		public static SpriteSheet MiniFireTexture;

		Transform m_transform;
		Sprite m_sprite;
		SpriteSheetAnimation m_animation;

		public MiniFire(Transform parent, Vector2 position)
		{
			m_transform = new Transform(parent, true);
			m_transform.Position = position;
			float scale = (float)Globals.Random.NextDouble() * 0.5f + 0.5f;
			m_transform.Scale = new Vector2(scale, scale);
			m_sprite = new Sprite(Globals.TheGame, MiniFireTexture, m_transform);
			m_animation = new SpriteSheetAnimation(m_sprite, 0, 3, 0.1f, -1);
		}

		public void Update()
		{
			m_animation.Update();
		}
		public void Draw()
		{
			m_sprite.Draw();
		}
	}


	public class DestructibleComponent : IPUpdatable
	{
		class HealthEvent
		{
			public PastaGameLibrary.Action action = null;
			public float healthThreshold = 0;
			public bool allowSkip = true;
		}
		bool m_isBurning = false;

		public static ColliderGroup[] DestructibleColliders = new ColliderGroup[AttackManager.AmountOfAttackTypes];

		ColliderGroup.OnCollision[] m_attackHitEventCache = new ColliderGroup.OnCollision[AttackManager.AmountOfAttackTypes]; //Contains the collision reaction methods to various weapons
		List<HealthEvent> m_healthEvents = null; //Contains events triggered when the object reaches a certain health threshold.
		DestructibleState _currentDestructibleState;
		Transform m_transform;
		Collider m_collider;
		PTimer m_burnTimer = null;
		float _hpCurrent, _hpMax;
		float m_burnTickDamage;
		bool m_isPulledByRocket;

		AABB m_fireBounds = null; //Defines a rectangle from the origin where mini flames can be generated
		float _miniFireRatio = 0.001f; //Defines the maximum amount of minifires based on the area of the fireRectangle
		//BurnSmoke[] _smoke = null;
		MiniFire[] _miniFires = null;
		ParticleSystem m_burnSmoke;
		
		public float Hp
		{
			get { return _hpCurrent; }
		}
		public bool IsFullHealth
		{
			get { return _hpCurrent == _hpMax; }
		}
		public bool IsBurning
		{
			get { return m_isBurning; }
		}

		public DestructibleComponent(Transform transform, Collider destructibleCollider, AABB fireBounds, float maxHP)
		{
			m_transform = transform;
			m_collider = destructibleCollider;
			_hpMax = _hpCurrent = maxHP;
		}
		public DestructibleComponent(Transform transform, Collider destructibleCollider, float maxHP)
		{
			m_transform = transform;
			m_collider = destructibleCollider;
			_hpMax = _hpCurrent = maxHP;
		}

		public static void Initialise()
		{
			for (int i = 0; i < DestructibleColliders.Length; ++i)
				DestructibleColliders[i] = new ColliderGroup();
		}

		public void SetHitEvent(AttackType attackType, ColliderGroup.OnCollision attackHitEvent)
		{
			int i = (int)attackType;
			if (m_attackHitEventCache[i] == null)
				DestructibleColliders[i].Add(m_collider, attackHitEvent);
			else
				DestructibleColliders[i].ReplaceAction(m_collider, attackHitEvent);
			m_attackHitEventCache[i] = attackHitEvent;
		}
		public void ClearHitEvents()
		{
			for (int i = 0; i < m_attackHitEventCache.Length; ++i)
			{
				if (m_attackHitEventCache[i] != null)
					DestructibleColliders[i].Remove(m_collider, m_attackHitEventCache[i]);
				m_attackHitEventCache[i] = null;
			}
		}

		public void BaseHit(Collider other)
		{
			AttackComponent attack = (AttackComponent)other.Owner;
			DealDamage(attack);
		}
		public void DealDamage(AttackComponent attack)
		{
            //int prevHP = _hpMax;
            float prevHP = _hpCurrent;
            _hpCurrent = Math.Max(0, _hpCurrent - attack.Damage);

			if (m_healthEvents != null)
			{
				List<HealthEvent> selectedEvents = new List<HealthEvent>();
				
				//Fills the selectedEvents list from lowest to highest hpThreshold
				for (int j = 0; j < m_healthEvents.Count; ++j)
				{
					if (m_healthEvents[j].healthThreshold >= _hpCurrent
						&& m_healthEvents[j].healthThreshold < prevHP)
						selectedEvents.Add(m_healthEvents[j]);
				}

				if (selectedEvents.Count == 0)
					return;

				//Checks each element to see if it can be played (not the last one)
				for (int i = selectedEvents.Count - 1; i > 0; --i)
					if (selectedEvents[i].allowSkip)
						selectedEvents[i].action.Start();

				//Last event (lowest hp threshold) is always played
				selectedEvents[0].action.Start();
			}
		}
		
		//public void Burn(Fireball fire)
		//{
		//    if(m_isBurning)
		//        return;
		//    if (m_burnTimer == null)
		//        m_burnTimer = new PTimer(Globals.TheGame.TimerManager, BurnHit);
		//    m_isBurning = true;
		//    m_burnTickDamage = fire.Damage;

		//    float area = m_fireBounds.Width * m_fireBounds.Height;

		//    Vector2 position;
		//    Rectangle fireRectangle = m_fireBounds.GetBounds();
		//    int amountOfMiniFires = InsoGame.Random.Next(Math.Max(1, (int)( area * _miniFireRatio * 0.5f)), Math.Max(2, (int)(area * _miniFireRatio)));
		//    _miniFires = new MiniFire[amountOfMiniFires];
		//    for (int i = 0; i < _miniFires.Length; ++i)
		//    {
		//        position.X = fireRectangle.X + InsoGame.Random.Next((int)(-m_fireBounds.Width * 0.5f), (int)(m_fireBounds.Width * 0.5f));
		//        position.Y = fireRectangle.Y + InsoGame.Random.Next((int)(-m_fireBounds.Height), 0);
		//        _miniFires[i] = new MiniFire(m_transform, position);
		//    }

		//    _smoke = new BurnSmoke[Math.Max(5, _miniFires.Length)];
		//    for (int i = 0; i < _smoke.Length; ++i)
		//    {
		//        _smoke[i] = new BurnSmoke(m_fireBounds);
		//        _smoke[i].Initialise();
		//    }
		//}

		/// <summary>
		/// Action sustained when the object is burned by fire
		/// </summary>
		private void BurnHit()
		{ 
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="healthThreshold"></param>
		/// <param name="allowSkip">True will execute the action even if the chunk of health is big enough to reach the next threshold before this one.</param>
		/// <param name="action">The action to execute</param>
		public void AddHealthEvent(float healthThreshold, bool allowSkip, PastaGameLibrary.Action action)
		{
			if(m_healthEvents == null)
				m_healthEvents = new List<HealthEvent>();

			HealthEvent newEvent = new HealthEvent();
			newEvent.action = action;
			newEvent.healthThreshold = healthThreshold;
			newEvent.allowSkip = allowSkip;
			if (m_healthEvents.Count == 0)
			{
				m_healthEvents.Add(newEvent);
				return;
			}
			int i;
			for (i = 0; i < m_healthEvents.Count; ++i)
				if (newEvent.healthThreshold > m_healthEvents[i].healthThreshold)
					break;
			m_healthEvents.Insert(i, newEvent);
		}

		public void Update()
		{
		}

		public void Draw()
		{
			if (_miniFires != null)
				for (int i = 0; i < _miniFires.Length; ++i)
					_miniFires[i].Draw();
		}
	}

    public enum DestructibleState
    {
        Untouched,
        StillStanding,
        FallingDown,
        Destroyed
    }
}
