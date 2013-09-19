using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PastaGameLibrary
{
	public interface IArea
	{
		Vector2 GetRandomPoint(Random random);
	}
	public class UnitPoint : IArea
	{
		Transform m_transform;
		Vector2 m_pos;

		Vector2 Position
		{
			get { return Vector2.Transform(m_pos, m_transform.GlobalTransform); }
		}

		public UnitPoint(Transform transform, Vector2 position)
		{
 			m_pos = position;
			m_transform = transform;
		}

		public Vector2 GetRandomPoint(Random random)
		{
			return Position;
		}
}
    public class Circle : IArea
    {
		private Transform m_transform;
        private float m_radius;

        public Circle(Transform transform, float radius)
        {
			m_transform = transform;
            m_radius = radius;
        }
        public Circle(float cX, float cY, float radius)
        {
            m_transform = new Transform();
			m_transform.PosX = cX;
			m_transform.PosY = cY;
            m_radius = radius;
        }
		public Circle(float radius)
		{
			m_transform = new Transform();
			m_radius = radius;
		}

		public Vector2 GetRandomPoint(Random random)
		{
			double angle = random.NextDouble() * Math.PI * 2;
			double radius = random.NextDouble() * m_radius;
			float x = (float)(Math.Cos(angle) * radius);
			float y = (float)(Math.Sin(angle) * radius);
			return new Vector2(x, y);
		}

		public Transform Transform
		{
			get { return m_transform; }
		}
        public float Radius
        {
            get { return m_radius; }
            set { m_radius = value; }
        }
        public double Perimeter
        {
            get { return m_radius * 2 * Math.PI; }
        }
        public double Area
        {
            get { return m_radius * m_radius * Math.PI; }
        }
    }
	public class LineSegment : IArea
	{
		Transform m_transform;
		Vector2 m_a, m_b;

		public Transform Transform
		{
			get { return m_transform; }
		}

		public LineSegment(Vector2 a, Vector2 b)
		{
			m_transform = new Transform();
		}

		public LineSegment(Transform transform, Vector2 a, Vector2 b)
		{
			m_a = a;
			m_b = b;
			m_transform = transform;
		}

		public Vector2 A_Local
		{
			get { return m_a; }
			set { m_a = value; }
		}

		public Vector2 B_Local
		{
			get { return m_b; }
			set { m_b = value; }
		}

		public Vector2 A_Global
		{
			get { return Vector2.Transform(m_a, m_transform.GlobalTransform); }
		}
		public Vector2 B_Global
		{
			get { return Vector2.Transform(m_b, m_transform.GlobalTransform); }
		}

		public float Gradient
		{
			get {
				Vector2 a = A_Global;
				Vector2 b = B_Global;
				return (b.Y - a.Y) / (b.X - a.X); }
		}
		public float Intercept
		{
			get {
				Vector2 a = A_Global;
				Vector2 b = B_Global;
				return (b.X * a.Y - a.X * b.Y) / (b.X - a.X); }
		}
		/// <summary>
		/// Vector perpendicular to Direction with the same length.
		/// </summary>
		public Vector2 Normal
		{
			get
			{
				Vector2 dir = Difference;
				return new Vector2(-Difference.Y, Difference.X);
			}
		}
		/// <summary>
		/// The difference between points B and A.
		/// </summary>
		public Vector2 Difference
		{
			get { return B_Global - A_Global; }
		}
		public bool IsVertical
		{
			get { return m_transform.Direction == Math.PI * 0.5; }
		}
		public bool IsHorizontal
		{
			get { return m_transform.Direction == 0; }
		}
		public bool IsParallel(StraightLine L2)
		{
			return (IsVertical && L2.IsVertical)
				|| Gradient == L2.Gradient;
		}
		public bool IsParallel(LineSegment L2)
		{
			return (IsVertical && L2.IsVertical)
				|| Gradient == L2.Gradient;
		}
		public bool BelongsToLine(StraightLine L2)
		{
			if (IsVertical != L2.IsVertical)
				return false;
			if (IsVertical && L2.IsVertical)
				return A_Global.X == L2.GetX(0);
			return Gradient == L2.Gradient
				&& Intercept == L2.Intercept;
		}
		public bool IsEqual(LineSegment L2)
		{
			return A_Global == L2.A_Global && B_Global == L2.B_Global;
		}

		public Vector2 GetRandomPoint(Random random)
		{
			Vector2 diff = B_Global - A_Global;
			Vector2 add = diff * (float)random.NextDouble();
			return A_Global + add;
		}
	}
	public class BB : IArea
	{
		Vector2 m_dimensions;
		Vector2 m_origin;
		Transform m_transform;

		Vector2 m_a, m_b, m_c, m_d;
		float m_left, m_right, m_top, m_bottom;

		public Transform Transform
		{
			get { return m_transform; }
		}
		public Vector2 Dimensions
		{
			get { return m_dimensions; }
			set { m_dimensions = value; }
		}
		public float Width
		{
			get { return m_dimensions.X; }
			//set { m_dimensions.X = value; }
		}
		public float Height
		{
			get { return m_dimensions.Y; }
			//set { m_dimensions.Y = value; }
		}
		public Vector2 Origin
		{
			get { return m_origin; }
		}

		public BB(Transform transform, Vector2 dimensions, Vector2 origin)
		{
			m_transform = transform;
			m_dimensions = dimensions;
			m_origin = origin;

			float ox = m_origin.X * m_dimensions.X;
			float oy = m_origin.Y * m_dimensions.Y;
			m_a = new Vector2(-ox, -oy);
			m_b = new Vector2(ox, -oy);
			m_c = new Vector2(ox, oy);
			m_d = new Vector2(-ox, oy);
		}
		public BB(Transform transform, Vector2 dimensions)
		{
			m_transform = transform;
			m_dimensions = dimensions;
			m_origin = new Vector2(0.5f, 0.5f);

			float ox = m_origin.X * m_dimensions.X;
			float oy = m_origin.Y * m_dimensions.Y;
			m_a = new Vector2(-ox, -oy);
			m_b = new Vector2(ox, -oy);
			m_c = new Vector2(ox, oy);
			m_d = new Vector2(-ox, oy);
		}

		public void GetBounds(out Vector2 a, out Vector2 b, out Vector2 c, out Vector2 d)
		{
			Matrix transform = m_transform.GlobalTransform;
			Vector2.Transform(ref m_a, ref transform, out a);
			Vector2.Transform(ref m_b, ref transform, out b);
			Vector2.Transform(ref m_c, ref transform, out c);
			Vector2.Transform(ref m_d, ref transform, out d);
		}

		public Vector2 GetRandomPoint(Random random)
		{
			Vector2 a, b, c, d;
			GetBounds(out a, out b, out c, out d);

			//Axis vectors
			a = b - a;
			b = c - a;

			//Random values along axis vectors
			c = a * (float)random.NextDouble();
			d = b * (float)random.NextDouble();

			//Return sum
			return c + d;
		}

	}
	public class AABB : IArea
	{
		Vector2 m_dimensions;
		Vector2 m_origin;
		Transform m_transform;


		public Transform Transform
		{
			get { return m_transform; }
		}
		public Vector2 Dimensions
		{
			get { return m_dimensions; }
			set { m_dimensions = value; }
		}
		public float Width
		{
			get { return m_dimensions.X; }
			set { m_dimensions.X = value; }
		}
		public float Height
		{
			get { return m_dimensions.Y; }
			set { m_dimensions.Y = value; }
		}
		public float Radius
		{
			get {
				float halfWidth = Width * 0.5f;
				float halfHeight = Height * 0.5f;
				return (float)Math.Sqrt(Math.Pow(halfWidth, 2) + Math.Pow(halfHeight, 2));
			}
		}
		public Vector2 Origin
		{
			get { return m_origin; }
			set
			{
				m_origin = value;
			}
		}
		public Rectangle GetBounds()
		{
			int cx = (int)m_dimensions.X;
			int cy = (int)m_dimensions.Y;
			float left, top, right, bottom;

			Vector2 pos, scl;
			float rot;
			if (m_transform == null)
			{
				rot = 0;
				pos = Vector2.Zero;
				scl = new Vector2(1, 1);
			}
			else
			{
				m_transform.GetGlobalComponents(out pos, out rot, out scl);
			}

			int x = (int)pos.X;
			int y = (int)pos.Y;
			int ox = (int)(m_origin.X * m_dimensions.X);
			int oy = (int)(m_origin.Y * m_dimensions.Y);

			#region No Rotation

			if (rot == 0)
			{
				if (scl.X == 1.0f)
				{
					left = x - ox;
					right = left + cx;
				}
				else
				{
					left = x - (int)(ox * scl.X);
					right = left + (int)(cx * scl.X);
				}

				if (scl.Y == 1.0f)
				{
					top = y - oy;
					bottom = top + cy;
				}
				else
				{
					top = y - (int)(oy * scl.Y);
					bottom = top + (int)(cy * scl.Y);
				}
			}
			#endregion
			#region Rotation
			else
			{
				if (scl.X != 1.0f)
				{
					ox = (int)(ox * scl.X);
					cx = (int)(cx * scl.X);
				}

				if (scl.Y != 1.0f)
				{
					oy = (int)(oy * scl.Y);
					cy = (int)(cy * scl.Y);
				}

				//double alpha = Angle * Math.PI / 180.0f;
				float cosa = (float)Math.Cos(rot);
				float sina = (float)Math.Sin(rot);

				int nx1;
				int ny1;

				int nx2 = -(int)(cy * sina);
				int ny2 = (int)(cy * cosa);

				int nx4 = (int)(cx * cosa);
				int ny4 = (int)(cx * sina);

				int nx3 = nx2 + nx4;
				int ny3 = ny2 + ny4;

				// Faire translation par rapport au hotspot
				int xBase = x - (int)(ox * cosa - oy * sina);
				int yBase = y - (int)(oy * cosa + ox * sina);

				nx1 = xBase;
				ny1 = yBase;

				nx2 += xBase;
				ny2 += yBase;

				nx3 += xBase;
				ny3 += yBase;

				nx4 += xBase;
				ny4 += yBase;

				// Calculer la nouvelle bounding box (à optimiser éventuellement)
				left = Math.Min(nx1, nx2);
				left = Math.Min(left, nx3);
				left = Math.Min(left, nx4);

				right = Math.Max(nx1, nx2);
				right = Math.Max(right, nx3);
				right = Math.Max(right, nx4);

				//right++;	// new

				top = Math.Min(ny1, ny2);
				top = Math.Min(top, ny3);
				top = Math.Min(top, ny4);

				bottom = Math.Max(ny1, ny2);
				bottom = Math.Max(bottom, ny3);
				bottom = Math.Max(bottom, ny4);

				//bottom++;	// new             
			}
			#endregion
			return new Rectangle((int)left, (int)top, (int)(right - left), (int)(bottom - top));
		}
		public Vector2 GetRandomPoint(Random random)
		{
			Vector2 result;
			Rectangle bounds = GetBounds();

			//Axis vectors
			result.X = bounds.Left + (bounds.Right - bounds.Left ) * (float)random.NextDouble();
			result.Y = bounds.Top + (bounds.Bottom - bounds.Top) * (float)random.NextDouble();

			return result;
		}

		public AABB(Transform transform, Vector2 dimensions)
		{
			m_origin = new Vector2(0.5f, 0.5f);
			m_dimensions = dimensions;
			m_transform = transform;
		}

		public AABB(Sprite sprite)
		{
			m_origin = sprite.Origin;
			m_dimensions = sprite.Dimensions;
			m_transform = sprite.Transform;
		}
	}
}
