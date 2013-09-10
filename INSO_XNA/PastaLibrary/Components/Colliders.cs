using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PastaGameLibrary;

namespace PastaGameLibrary
{
	public class CollisionMethods
	{
		public const int AmountOfColliderTypes = 5;
		public enum ColliderType
		{
			Point = 0,
			Circle = 1,
			Line = 2,
			AABB = 3,
			Mask = 4,
		};

		delegate bool CollisionDelegate(Collider a, Collider b);
		CollisionDelegate[,] m_collisionMethods;
		GeometryHelper m_helper = new GeometryHelper();

		public CollisionMethods()
		{
			m_collisionMethods = new CollisionDelegate[AmountOfColliderTypes, AmountOfColliderTypes];
			for (int i = 0; i < m_collisionMethods.GetLength(0); ++i)
				for (int j = 0; j < m_collisionMethods.GetLength(0); ++j)
					m_collisionMethods[i, j] = null;

			m_collisionMethods[0, 0] = PointPoint;
			m_collisionMethods[0, 1] = PointCircle;
			m_collisionMethods[0, 2] = PointLine;
			m_collisionMethods[0, 3] = PointAABB;
			m_collisionMethods[0, 4] = PointMask;

			m_collisionMethods[1, 1] = CircleCircle;
			m_collisionMethods[1, 2] = CircleLine;
			m_collisionMethods[1, 3] = CircleAABB;
			m_collisionMethods[1, 4] = CircleMask;

			m_collisionMethods[2, 2] = LineLine;
			m_collisionMethods[2, 3] = LineAABB;
			m_collisionMethods[2, 4] = LineMask;

			m_collisionMethods[3, 3] = AABBAABB;
			m_collisionMethods[3, 4] = AABBMask;

			m_collisionMethods[3, 4] = MaskMask;
		}

		public bool TestCollision(Collider a, Collider b)
		{
			int ia = (int)a.Type;
			int ib = (int)b.Type;
			return m_collisionMethods[ia, ib](a, b);
		}

		public bool PointPoint(Collider point1, Collider point2)
		{
			Vector2 p1 = ((PointCollider)point1).Point;
			Vector2 p2 = ((PointCollider)point2).Point;
			return p1.X == p2.X && p1.Y == p2.Y;
		}
		public bool PointCircle(Collider point, Collider circle)
		{
			Vector2 p = ((PointCollider)point).Point;
			Circle c = ((CircleCollider)circle).Circle;
			return m_helper.Contains(c, p);
		}
		public bool PointLine(Collider point, Collider line)
		{
			throw new NotImplementedException();
		}
		public bool PointAABB(Collider point, Collider aabb)
		{
			Vector2 p = ((PointCollider)point).Point;
			Rectangle r = ((AABBCollider)aabb).Bounds;
			return (p.X > r.Left &&
				p.Y > r.Top &&
				p.X < r.Right &&
				p.Y < r.Bottom);
		}
		public bool PointMask(Collider point, Collider mask)
		{
			throw new NotImplementedException();
		}

		public bool CircleCircle(Collider circle1, Collider circle2)
		{
			Circle c1 = ((CircleCollider)circle1).Circle;
			Circle c2 = ((CircleCollider)circle2).Circle;
			return m_helper.GetDistance(c1.Transform.PosX, c1.Transform.PosY, c2.Transform.PosX, c2.Transform.PosY) < c1.Radius + c2.Radius;
		}
		public bool CircleLine(Collider circle, Collider line)
		{
			throw new NotImplementedException();
		}
		public bool CircleAABB(Collider circle, Collider aabb)
		{
			throw new NotImplementedException();
		}
		public bool CircleMask(Collider circle, Collider mask)
		{
			throw new NotImplementedException();
		}

		public bool LineLine(Collider line1, Collider line2)
		{
			LineSegment ls1 = ((LineCollider)line1).Line;
			LineSegment ls2 = ((LineCollider)line2).Line;
			return m_helper.IntersectsLines(ls1, ls2);
		}
		public bool LineAABB(Collider line, Collider aabb)
		{
			LineSegment ls = ((LineCollider)line).Line;
			LineSegment ls2 = new LineSegment(Vector2.Zero, Vector2.Zero);
			Rectangle r = ((AABBCollider)aabb).Bounds;

			if (r.Contains(new Point((int)ls.A_Global.X, (int)ls.A_Global.Y))
				|| r.Contains(new Point((int)ls.B_Global.X, (int)ls.B_Global.Y))
				|| m_helper.IntersectsLines(ls.A_Global, ls.B_Global, new Vector2(r.Left, r.Top), new Vector2(r.Right, r.Top))
				|| m_helper.IntersectsLines(ls.A_Global, ls.B_Global, new Vector2(r.Right, r.Top), new Vector2(r.Right, r.Bottom))
				|| m_helper.IntersectsLines(ls.A_Global, ls.B_Global, new Vector2(r.Right, r.Bottom), new Vector2(r.Left, r.Bottom))
				|| m_helper.IntersectsLines(ls.A_Global, ls.B_Global, new Vector2(r.Left, r.Bottom), new Vector2(r.Left, r.Top)))
				return true;
			return false;
		}
		public bool LineMask(Collider line, Collider mask)
		{
			throw new NotImplementedException();
		}

		public bool AABBAABB(Collider aabb1, Collider aabb2)
		{
			Rectangle r1 = ((AABBCollider)aabb1).Bounds;
			Rectangle r2 = ((AABBCollider)aabb2).Bounds;
			return r1.Intersects(r2);
		}
		public bool AABBMask(Collider aabb, Collider mask)
		{
			throw new NotImplementedException();
		}

		public bool MaskMask(Collider mask1, Collider mask2)
		{
			throw new NotImplementedException();

			//// Stolen from http://create.msdn.com/en-US/education/catalog/tutorial/collision_2d_perpixel
			//int top = Math.Max(rectangleA.Top, rectangleB.Top);
			//int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
			//int left = Math.Max(rectangleA.Left, rectangleB.Left);
			//int right = Math.Min(rectangleA.Right, rectangleB.Right);
			//for (int y = top; y < bottom; y++)
			//{
			//    for (int x = left; x < right; x++)
			//    {
			//        Color colorA = dataA[(x - rectangleA.Left) + (y - rectangleA.Top) * rectangleA.Width];
			//        Color colorB = dataB[(x - rectangleB.Left) + (y - rectangleB.Top) * rectangleB.Width];
			//        if (colorA.A > 0 && colorB.A > 0)
			//            return true;
			//    }
			//}
			//return false;
		}

		//public static bool Pixel_Simple(GameObject o1, GameObject o2)
		//{
		//    if (!TestCollision(o1.BoundingRect, o2.BoundingRect))
		//        return false;

		//    if (o1.Angle == 0 && o2.Angle == 0)
		//        return PixelPerfectSimple(o1.Rect, o1.TextureData, o2.Rect, o2.TextureData);
		//    Matrix a2b = Matrix.CreateTranslation(-o1.Origin.X, -o1.Origin.Y, 0) * Matrix.CreateRotationZ(o1.Rotation) * Matrix.CreateTranslation(o1.Position.X, o1.Position.Y, 0) * Matrix.Invert(Matrix.CreateTranslation(-o2.Origin.X, -o2.Origin.Y, 0) * Matrix.CreateRotationZ(o2.Rotation) * Matrix.CreateTranslation(o2.Position.X, o2.Position.Y, 0));
		//    Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, a2b);
		//    Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, a2b);
		//    Vector2 o2y = Vector2.Transform(Vector2.Zero, a2b);
		//    for (int y1 = 0; y1 < o1.Rect.Height; y1++)
		//    {
		//        Vector2 o2pos = o2y;
		//        for (int x1 = 0; x1 < o1.Rect.Width; x1++)
		//        {
		//            int x2 = (int)Math.Round(o2pos.X);
		//            int y2 = (int)Math.Round(o2pos.Y);
		//            if (x2 > 0 && x2 < o2.Rect.Width && y2 > 0 && y2 < o2.Rect.Height)
		//                if (o1.TextureData[x1 + y1 * o1.Rect.Width].A > 0 && o2.TextureData[x2 + y2 * o2.Rect.Width].A > 0)
		//                    return true;
		//            o2pos += stepX;
		//        }
		//        o2y += stepY;
		//    }
		//    return false;
		//}
		//public static bool Pixel_Rectangle(GameObject go, Rectangle r)
		//{
		//    throw new NotImplementedException();
		//}
	}

	public class ColliderGroup
	{
		public delegate void OnCollision(Collider other);
		class CollisionEntry
		{
			public OnCollision onCollision;
			public Collider collider;
		}

		protected CollisionMethods m_methods = new CollisionMethods();
		List<CollisionEntry> m_entries = new List<CollisionEntry>();

		public ColliderGroup()
		{ }

		/// <summary>
		/// Add a member to the group
		/// </summary>
		/// <param name="collider">Collider used to test the collision</param>
		/// <param name="action">Action to perform when collision occurs. May be null.</param>
		public void Add(Collider collider, OnCollision action)
		{
			CollisionEntry entry = new CollisionEntry();
			entry.collider = collider;
			entry.onCollision = action;

			m_entries.Add(entry);
			collider.m_groups.Add(this);
		}
		public void ReplaceAction(Collider collider, OnCollision action)
		{
			for (int i = 0; i < m_entries.Count; ++i)
				if (m_entries[i].collider == collider)
				{
					m_entries[i].onCollision = action;
					return;
				}
		}
		public void Remove(Collider collider, OnCollision action)
		{
			for (int i = 0; i < m_entries.Count; ++i)
				if (m_entries[i].collider == collider && m_entries[i].onCollision == action)
				{
					m_entries.RemoveAt(i);
					return;
				}
		}
		public void Remove(Collider collider)
		{
			for(int i = 0; i < m_entries.Count; ++i)
				if (m_entries[i].collider == collider)
				{
					m_entries.RemoveAt(i);
					return;
				}
		}

		/// <summary>
		/// Test collisions with a single other collider.
		/// </summary>
		/// <param name="objectToTest">Collider to test</param>
		/// <param name="action">Action to perform when collision occurs. May be null.</param>
		public void DoCollision(Collider objectToTest, OnCollision action)
		{
			for (int i = 0; i < m_entries.Count; ++i)
				if (m_methods.TestCollision(objectToTest, m_entries[i].collider))
				{
					if (m_entries[i].onCollision != null)
						m_entries[i].onCollision(objectToTest);
					if (action != null)
						action(m_entries[i].collider);
				}
		}

		/// <summary>
		/// Test collisions with another group.
		/// </summary>
		/// <param name="groupToTest">Other group to test.</param>
		public void DoCollision(ColliderGroup groupToTest)
		{
			for (int i = 0; i < m_entries.Count; ++i)
				for(int j = 0; j < groupToTest.m_entries.Count; ++j)
					if (m_methods.TestCollision(m_entries[i].collider, groupToTest.m_entries[j].collider))
					{
						if(m_entries[i].onCollision != null)
							m_entries[i].onCollision(groupToTest.m_entries[j].collider);
						if (groupToTest.m_entries[j].onCollision != null)
							groupToTest.m_entries[j].onCollision(m_entries[i].collider);
					}
		}

		/// <summary>
		/// Test collision in between the members of the group.
		/// </summary>
		public void DoCollision()
		{
			for (int i = 0; i < m_entries.Count; ++i)
				for (int j = i + 1; j < m_entries.Count; ++j)
					if(m_methods.TestCollision(m_entries[i].collider, m_entries[j].collider))
					{
						if(m_entries[i].onCollision != null)
							m_entries[i].onCollision(m_entries[j].collider);
						if(m_entries[j].onCollision != null)
							m_entries[j].onCollision(m_entries[i].collider);
					}
		}
	}


	public abstract class Collider : IDisposable, IPUpdatable
	{
		internal List<ColliderGroup> m_groups = new List<ColliderGroup>();
		CollisionMethods.ColliderType m_type;
		Object m_owner;

		public Object Owner
		{
			get { return m_owner; }
		}
		public CollisionMethods.ColliderType Type
		{
			get { return m_type; }
		}

		public Collider(Object owner, CollisionMethods.ColliderType type)
		{
			m_type = type;
			m_owner = owner;
		}

		public abstract void Update();

		public void Dispose()
		{
			for (int i = 0; i < m_groups.Count; ++i)
				m_groups[i].Remove(this);
		}
	}

	public class PointCollider : Collider
	{
		Vector2 m_point;
		Transform m_transform;

		public Vector2 Point
		{
			get { return m_point; }
		}

		public PointCollider(Object owner, Transform transform) : base(owner, CollisionMethods.ColliderType.Point)
		{
			m_transform = transform;
		}
		public override void Update()
		{
			m_point = m_transform.PositionGlobal;
		}
	}

	public class CircleCollider : Collider
	{
		Circle m_circle;

		public Circle Circle
		{
			get { return m_circle; }
		}

		public CircleCollider(Object owner)
			: base(owner, CollisionMethods.ColliderType.Circle)
		{
			m_circle = new Circle(1);
		}

		public CircleCollider(Object owner, Transform transform)
			: base(owner, CollisionMethods.ColliderType.Circle)
		{
			m_circle = new Circle(transform, 1);
		}

		public override void Update()
		{
			//throw new NotImplementedException();
		}
	}
	public class LineCollider : Collider
	{
		LineSegment m_line;

		public LineSegment Line
		{
			get { return m_line; }
		}

		public LineCollider(Object owner)
			: base(owner, CollisionMethods.ColliderType.Line)
		{
			m_line = new LineSegment(Vector2.Zero, Vector2.Zero);
		}

		public override void Update()
		{
			//throw new NotImplementedException();
		}
	}
	public class AABBCollider : Collider
	{
		AABB m_aabb;
		Rectangle m_bounds;

		public Rectangle Bounds
		{
			get { return m_bounds; }
		}
		public AABB AABB
		{
			get { return m_aabb; }
		}

		public AABBCollider(Object owner, AABB aabb)
			: base(owner, CollisionMethods.ColliderType.AABB)
		{
			m_aabb = aabb;
		}
		public override void Update()
		{
			m_bounds = m_aabb.GetBounds(); //Update bounds only once (save processing power)
		}
	}
	public class MaskCollider : Collider
	{
		Sprite m_sprite;

		public Sprite Sprite
		{
			get { return m_sprite; }
		}

		public MaskCollider(Object owner, Sprite sprite)
			: base(owner, CollisionMethods.ColliderType.Mask)
		{
			m_sprite = sprite;
		}

		public override void Update()
		{
		}
	}
}
