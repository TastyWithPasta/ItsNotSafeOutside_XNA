using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PastaGameLibrary;

namespace TestBed
{
	public enum AttackType
	{
		Slash,
		Shuriken,
		Fire,
		Fireworks
	};
	
	public class AttackComponent
	{
		bool m_isActive;
		AttackType m_type;
		float m_baseDelay = 100, m_delayMultiplier = 1;
		float m_baseDamage = 1, m_damageMultiplier = 1;

		public AttackComponent(float baseDamage, float baseDelay)
		{
			m_baseDamage = baseDamage;
			m_baseDelay = baseDelay;
		}

		public bool IsActive
		{
			get { return m_isActive; }
			set { m_isActive = value; }
		}

		public AttackType Type
		{
			get { return m_type; }
		}

		public float Delay
		{
			get { return m_baseDelay * m_delayMultiplier; }
		}

		public float Damage
		{
			get { return m_baseDamage * m_damageMultiplier; }
		}
	}

	//public class Attack : IPUpdatable, IPDrawable
	//{
	//    public static int AmountOfAttackTypes = 4;
	//    protected Camera2D _camera;
	//    protected float _baseDelay = 100, _delayMultiplier = 1;
	//    protected float _baseDamage = 1, _damageMultiplier = 1;
	//    private AttackType m_attackType;

	//    public int Delay
	//    { 
	//        get { return (int)(_baseDelay * _delayMultiplier); }
	//    }
	//    public float Damage
	//    {
	//        get { return _baseDamage * _damageMultiplier; }
	//    }

	//    public void UpgradeDamage(float ratio)
	//    {
	//        _damageMultiplier *= ratio;
	//    }

	//    public Attack(AttackType type)
	//    {
	//        m_attackType = type;
	//    }

	//    public void Update()
	//    {
	//    }

	//    public void Draw()
	//    {
	//    }
	//}
}
