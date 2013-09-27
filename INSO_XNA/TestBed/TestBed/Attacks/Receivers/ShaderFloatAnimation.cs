using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using PastaGameLibrary;

namespace TestBed
{
	public class HitAnimation
	{
		const float HitDuration = 0.15f;
		static PSinePositiveInterpolation Interpolator = new PSinePositiveInterpolation();

		Effect m_shader;
		FloatPointerAnimation.FloatContainer m_value;
		FloatPointerAnimation m_hitFlashAnimation;

		public FloatPointerAnimation Animation
		{
			get { return m_hitFlashAnimation; }
		}

		public HitAnimation(Effect shader)
		{ 
			m_shader = shader;
			m_value = new FloatPointerAnimation.FloatContainer();
			m_value.Value = 0;
			m_hitFlashAnimation = new FloatPointerAnimation(Globals.TheGame, m_value, 1, 1);
			m_hitFlashAnimation.Interpolator = Interpolator;
			m_hitFlashAnimation.Timer.Interval = HitDuration;
		}

		public void Hit()
		{
			m_hitFlashAnimation.Start();
		}

		public void Update()
		{
			m_hitFlashAnimation.Update();
			if (!m_hitFlashAnimation.IsActive)
				m_value.Value = 0;
		}

		public void Apply()
		{
			m_shader.Parameters["Flash"].SetValue(m_value.Value);
		}

		public void Remove()
		{
			m_shader.Parameters["Flash"].SetValue(0);
		}
	}
}
