using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PastaGameLibrary;

namespace PastaGameLibrary
{
	public interface IParticle : IPUpdatable, IPDrawable
	{
		bool RemoveMe();
	}

	public abstract class StandardParticle : IParticle
	{
		static float Gravity = 9.81f;

		MyGame m_theGame;

		protected Sprite m_sprite;
		protected Vector2 m_vel, m_acc, m_force;
		protected float m_drag = 1, m_mass = 1, m_ttl = 1, m_maxTTL = 1;

		public StandardParticle(MyGame theGame, Sprite sprite)
		{
			m_theGame = theGame;
			m_sprite = sprite;
		}

		public void AddForce(Vector2 force)
		{
			m_force += force;
		}

		public bool RemoveMe()
		{
			return m_ttl <= 0;
		}

		public void Update()
		{
			float elapsed = (float)m_theGame.ElapsedTime;
			m_ttl -= elapsed;
			if (m_ttl < 0)
				return;

			float ratio = m_ttl / m_maxTTL;

			m_force.Y += Gravity;
			m_acc = m_force / m_mass;
			m_vel += m_acc * elapsed;
			m_sprite.Transform.Position += m_vel * elapsed * m_drag;
			
			m_force.X = m_force.Y = 0;
			m_sprite.Alpha = (byte)(ratio * 255);
		}

		public void Draw()
		{
			m_sprite.Draw();
		}
	}

	public class ParticleSystem : IPUpdatable, IPDrawable
	{
		MyGame m_theGame;
		internal IParticle[] m_particles;
		int m_amountOfParticles = 0;

		public int Count
		{
			get { return m_amountOfParticles; }
		}

		public ParticleSystem(MyGame theGame, int maxParticles)
		{
			m_theGame = theGame;
			m_particles = new IParticle[maxParticles];
		}

		public void AddParticle(IParticle particle)
		{
			for (int i = 0; i < m_particles.Length; ++i)
				if (m_particles[i] == null)
				{
					m_particles[i] = particle;
					m_amountOfParticles++;
				}
		}

		public void Update()
		{
			for (int i = 0; i < m_particles.Length; ++i)
			{
				m_particles[i].Update();
				if (m_particles[i].RemoveMe())
				{
					m_particles[i] = null;
					m_amountOfParticles--;
				}
			}
		}
		public void Draw()
		{
			for (int i = 0; i < m_particles.Length; ++i)
				m_particles[i].Draw();
		}
	}

	public class ParticleGenerator<T>
		where T : IParticle
	{
		MyGame m_theGame;
		ParticleSystem m_system;
		bool m_automatic = false;
		float m_generationInterval = 100;
		float m_generationTimer;
		object[] m_autoArgs = null;

		public bool Automatic
		{
			get { return m_automatic; }
			set { m_automatic = value; }
		}
		public float GenerationInterval
		{
			get { return m_generationInterval; }
			set { m_generationInterval = value; }
		}
		public ParticleGenerator(MyGame theGame, ParticleSystem system)
			: base()
		{
			m_theGame = theGame;
			m_system = system;
			m_generationTimer = m_generationInterval;
		}
		public ParticleGenerator(MyGame theGame, ParticleSystem system, object[] autoArgs)
			: base()
		{
			m_theGame = theGame;
			m_system = system;
			m_generationTimer = m_generationInterval;
			m_autoArgs = autoArgs;
		}
		public void ResetGenerationTimer()
		{
			m_generationTimer = m_generationInterval;
		}

		/// <summary>
		/// Generate a certain amount of particles. The particles need a constructor the game instance as sole parameter.
		/// </summary>
		/// <param name="amount">The amount of particles to generate</param>
		public void Generate(int amount, object[] args)
		{
			int lastIndex = 0;
			for (int j = 0; j < amount; ++j)
				for (int i = lastIndex; i < m_system.m_particles.Length; ++i)
					if (m_system.m_particles[i] == null)
					{
						m_system.m_particles[i] = (IParticle)Activator.CreateInstance(typeof(T), args);
						lastIndex = i + 1;
						break;
					}
		}
		/// <summary>
		/// Generate a particle. The particles need a constructor taking an array of object arguments.
		/// </summary>
		public void Generate(object[] args)
		{
			m_system.AddParticle((IParticle)Activator.CreateInstance(typeof(T), args));
		}

		/// <summary>
		/// Generate a particle. The particles need a constructor taking an array of object arguments.
		/// Uses the m_autoArgs parameters.
		/// </summary>
		public void Generate()
		{
			m_system.AddParticle((IParticle)Activator.CreateInstance(typeof(T), m_autoArgs));
		}

		public void Update()
		{
			float elapsed = (float)m_theGame.ElapsedTime;
			if (m_automatic)
			{
				m_generationTimer -= elapsed;
				while (m_generationTimer <= 0)
				{
					m_generationTimer = m_generationInterval + m_generationTimer;
					Generate();
				}
			}
		}
	}
}
