using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PastaGameLibrary
{
    public class GeometryHelper
    {
		EquationSolver m_solver = new EquationSolver();

        public bool Contains(Circle container, Vector2 contained)
        {
            return GetDistance(container.Transform.PositionGlobal, contained) < container.Radius;
        }
        public bool Contains(Circle container, Circle contained)
        {
			return GetDistance(container.Transform.PositionGlobal, contained.Transform.PositionGlobal) + contained.Radius < container.Radius; 
        }
        public bool Contains(Circle container, LineSegment contained)
        {
            return Contains(container, contained.A_Global) && Contains(container, contained.B_Global);
        }
		public bool Contains(BB container, Vector2 contained)
		{
			Vector2 a, b, c, d;
			container.GetBounds(out a, out b, out c, out d);
			return (TestNormalCCW(ref d, ref c, ref contained)
				&& TestNormalCCW(ref c, ref b, ref contained)
				&& TestNormalCCW(ref b, ref a, ref contained)
				&& TestNormalCCW(ref a, ref d, ref contained));
		}
		private bool TestNormalCCW(ref Vector2 p1, ref Vector2 p2, ref Vector2 pX)
		{
			float a = p1.Y - p2.Y;
			float b = p2.X - p1.X;
			float c = -(a * p1.X + b * p1.Y);
			float d = a * pX.X + b * pX.Y + c;
			return d > 0;
		}

        public bool Intersects(Circle circle, StraightLine line)
        {
            //Circle center - Projection vector
			Vector2 XP = GetProjectionPoint(line, circle.Transform.PositionGlobal) - circle.Transform.PositionGlobal;
            if (XP.Length() < circle.Radius)
                return true;
            return false;
        }

        public bool IntersectsCircleLine(Circle circle, LineSegment segment)
        {
			return IntersectsCircleLine(circle.Transform.PositionGlobal, circle.Radius, segment.A_Global, segment.B_Global);
        }
		public bool IntersectsCircleLine(Vector2 c, float r, Vector2 a, Vector2 b)
		{
			Vector2 XP = GetProjectionPoint(a, b, c) - c;
			if (XP.Length() < r)
				return true;
			return false;
		}
        public bool IntersectsCircles(Circle circle1, Circle circle2)
        {
			return IntersectsCircles(circle1.Transform.PositionGlobal, circle1.Radius, circle2.Transform.PositionGlobal, circle2.Radius);
        }
		public bool IntersectsCircles(Vector2 cA, float rA, Vector2 cB, float rB)
		{
			return GetDistance(cA, cB) < rA + rB;
		}
		public bool IntersectsLines(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
		{
			float q = (A.Y - C.Y) * (D.X - C.X) - (A.X - C.X) * (D.Y - C.Y);
			float d = (B.X - A.X) * (D.Y - C.Y) - (B.Y - A.Y) * (D.X - C.X);
			if (d == 0)
				return false;
			float r = q / d;
			q = (A.Y - C.Y) * (B.X - A.X) - (A.X - C.X) * (B.Y - A.Y);
			float s = q / d;
			if (r < 0 || r > 1 || s < 0 || s > 1)
				return false;
			return true;
		}
		public bool IntersectsLines(LineSegment ls1, LineSegment ls2)
		{
			Vector2 A = ls1.A_Global, B = ls1.B_Global, C = ls2.A_Global, D = ls2.B_Global;
			return IntersectsLines(A, B, C, D);
		}

        public bool GetIntersection(Circle circle1, Circle circle2, out Vector2[] intersections)
        {
			float x0 = circle1.Transform.PositionGlobal.X;
			float y0 = circle1.Transform.PositionGlobal.Y;
            float r0 = circle1.Radius;
			float x1 = circle2.Transform.PositionGlobal.X;
			float y1 = circle2.Transform.PositionGlobal.Y;
            float r1 = circle2.Radius;

            float M = (x0 - x1) / (y0 - y1);

            if (y1 != y0)
            {
                float N = (r1 * r1 - r0 * r0 - x1 * x1 + x0 * x0 - y1 * y1 + y0 * y0) / (2 * (y0 - y1));

                //Setup polynomial equation
                float A = M * M + 1;
                float B = 2 * y0 * M - 2 * M * N - 2 * x0;
                float C = x0 * x0 + y0 * y0 + N * N - r0 * r0 - 2 * y0 * N;

				float[] xSolutions = m_solver.Poly_2(A, B, C);
                intersections = new Vector2[xSolutions.Length];
                if (xSolutions.Length == 0)
                    return false;

                for (int j = 0; j < intersections.Length; ++j)
                {
                    intersections[j].X = xSolutions[j];
                    intersections[j].Y = N - intersections[j].X * M;
                }
                return true;
            }
            else
            {
                float x = (r1 * r1 - r0 * r0 - x1 * x1 + x0 * x0)/ (2 * (x0 - x1));

                //Setup polynomial equation
                float A = 1;
                float B = -2 * y1;
                float C = x1 * x1 + x * x - 2 * x1 * x + y1 * y1 - r1 * r1;

                float[] ySolutions = m_solver.Poly_2(A, B, C);
                intersections = new Vector2[ySolutions.Length];
                if (ySolutions.Length == 0)
                    return false;
                for (int j = 0; j < intersections.Length; ++j)
                {
                    intersections[j].X = x;
                    intersections[j].Y = ySolutions[j];
                }
                return true;
            }


        }
        /// <summary>
        /// Get intersection points between a straight line and circle.
        /// </summary>
        /// <param name="line">The straight line used for the intersection.</param>
        /// <param name="circle">The circle used for the intersection.</param>
        /// <param name="intersections"></param>
        /// <returns></returns>
        public bool GetIntersection(StraightLine line, Circle circle, out Vector2[] intersections)
        {
            float g = line.Gradient;
            float i = line.Intercept;
			Vector2 c = circle.Transform.PositionGlobal;
            float r = circle.Radius;

            //Setup polynomial equation
            float A = 1 + g * g;
            float B = -2 * c.X - 2 * g * i + 2 * c.Y * g;
            float C = c.X * c.X + g * g + 2 * c.Y * i + c.Y * c.Y - r * r;

            float[] xSolutions = m_solver.Poly_2(A, B, C);
            intersections = new Vector2[xSolutions.Length];

            if (xSolutions.Length == 0)
                return false;

            for(int j = 0; j < intersections.Length; ++j)
            {
                intersections[j].X = xSolutions[j];
                intersections[j].Y = g * intersections[j].X + i;
            }

            return true;
        }
        /// <summary>
        /// Returns true if the lines are equal, false if they are not.
        /// </summary>
        /// <param name="lineA">First line to test.</param>
        /// <param name="lineB">Second Line to test.</param>
        /// <param name="intersection">The point of intersection.</param>
        /// <returns>True if the lines are equal</returns>
        public bool GetIntersection(StraightLine lineA, StraightLine lineB, out Vector2 intersection)
        {
            float x;
            float y;
            bool aV = lineA.IsVertical;
            bool bV = lineB.IsVertical;
            intersection = new Vector2();

            //Lines are equal: intersection.
            if (lineA.Equals(lineB))
                return true;

            //Lines are parallel: no intersection.
            if (lineA.IsParallel(lineB))
                return false;

            if (aV && !bV)
            {
                x = -lineA.C / lineA.A;
                y = (-lineB.A * x - lineB.C) / lineB.B;
                intersection = new Vector2(x, y);
                return true;
            }
            if (!aV && bV)
            {
                x = lineB.GetX(0);
                y = (-lineA.A * x - lineA.C) / lineA.B;
                intersection = new Vector2(x, y);
                return true;
            }
            if (!aV && !bV)
            {
                x = (lineB.Intercept - lineA.Intercept) / (lineA.Gradient - lineB.Gradient);
                y = lineA.Gradient * x + lineA.Intercept;
                intersection = new Vector2(x, y);
                return true;
            }

            throw new Exception("Well that wasn't supposed to happen...");
        }
        public bool GetIntersection(LineSegment segmentA, LineSegment segmentB, out Vector2 intersection)
        {
            intersection = Vector2.Zero;

            Vector2 A = segmentA.A_Global;
            Vector2 C = segmentB.A_Global;
            Vector2 I = segmentA.Difference;
            Vector2 J = segmentB.Difference;

            float m = -(-I.X * A.Y + I.X * C.Y + I.Y * A.X - I.Y * C.X) / (I.X * J.Y - I.Y * J.X);
            float k = -(A.X * J.Y - C.X * J.Y - J.X * A.Y + J.X * C.Y) / (I.X * J.Y - I.Y * J.X); ;

            if( 0 < m && m < 1 
                && 0 < k && k < 1)
            {
                intersection = A + k * I;
                return true;
            }
            return false;
        }

        public bool GetIntersection(Rectangle rectangle, LineSegment segment, out Vector2 intersection)
        {
            bool isIntersecting = false;

            Vector2 topLeft = new Vector2(rectangle.X, rectangle.Y);
            Vector2 topRight = new Vector2(rectangle.X + rectangle.Width, rectangle.Y);
            Vector2 bottomRight = new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height);
            Vector2 bottomLeft = new Vector2(rectangle.X, rectangle.Y + rectangle.Height);

            intersection = Vector2.Zero;
            Vector2 newIntersection;

            //if (GetIntersection(new LineSegment(topRight, bottomRight), segment, out intersection)
            //    || GetIntersection(new LineSegment(topRight, bottomRight), segment, out intersection)
            //    || GetIntersection(new LineSegment(bottomRight, bottomLeft), segment, out intersection)
            //    || GetIntersection(new LineSegment(topLeft, topRight), segment, out intersection))
            //    return true;

            isIntersecting = GetIntersection(new LineSegment(topLeft, topRight), segment, out intersection);

            if (GetIntersection(new LineSegment(topRight, bottomRight), segment, out newIntersection))
            {
                if (!isIntersecting)
                    intersection = newIntersection;
                else
                    intersection = (intersection + newIntersection) * 0.5f;
                isIntersecting = true;
            }

            if (GetIntersection(new LineSegment(bottomRight, bottomLeft), segment, out newIntersection))
            {
                if (!isIntersecting)
                    intersection = newIntersection;
                else
                    intersection = (intersection + newIntersection) * 0.5f;
                isIntersecting = true;
            }

            if (isIntersecting = GetIntersection(new LineSegment(bottomLeft, topLeft), segment, out newIntersection))
            {
                if (!isIntersecting)
                    intersection = newIntersection;
                else
                    intersection = (intersection + newIntersection) * 0.5f;
                isIntersecting = true;
            }
            return isIntersecting;
        }

		public double GetAngle(Transform a, Transform b)
		{
			double dist = GetDistance(a.Position, b.Position);
			double sine = (b.PosY - a.PosY) / dist;
			double cosine = (b.PosX - a.PosX) / dist;
			return GetAngle(cosine, sine);
		}
        public double GetAngle(double cosine, double sine)
        {
            if (sine > 0)
                return Math.Acos(cosine);
            return -Math.Acos(cosine);
        }
        public float GetDistance(Point A, Point B)
        {
            return GetDistance(A.X, A.Y, B.X, B.Y);
        }
        public float GetDistance(Vector2 A, Vector2 B)
        {
            return GetDistance(A.X, A.Y, B.X, B.Y);
        }
        public float GetDistance(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        public int GetSign(float x)
        {
            if (x > 0)
                return 1;
            if (x < 0)
                return -1;
            return 0;
        }
        public float GetProjectionRatio(Vector2 screen, Vector2 projector)
        {
            return Vector2.Dot(screen, projector) / Vector2.Dot(screen, screen);
        }
        public Vector2 GetProjectionPoint(StraightLine screen, Vector2 projector)
        {
            Vector2 pointA = new Vector2(0, screen.GetY(0));
            Vector2 AB = screen.Direction;
            Vector2 AX = Vector2.Subtract(projector, pointA);
            return pointA + AB * GetProjectionRatio(AB, AX);
        }


        public Vector2 GetProjectionPoint(LineSegment screen, Vector2 projector)
        {
			return GetProjectionPoint(screen.A_Global, screen.B_Global, projector);
        }

		public Vector2 GetProjectionPoint(Vector2 a, Vector2 b, Vector2 projector)
		{
			Vector2 AB = b - a;
			Vector2 AX = Vector2.Subtract(projector, a);
			float ratio = MathHelper.Clamp(GetProjectionRatio(AB, AX), 0, 1);
			return a + AB * ratio;
		}
    }
}
