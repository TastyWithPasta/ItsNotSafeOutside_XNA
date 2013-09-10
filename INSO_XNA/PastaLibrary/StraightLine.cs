using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PastaGameLibrary
{
	public struct StraightLine {
        private float _a, _b, _c;
        private float _baseX, _baseY; //Values used if line is vertical or horizontal
			
        /// <summary>
        /// Define a straight line by its constant parameters, following the equation: ax + by + c = 0.
        /// </summary>
        /// <param name="a">Constant a.</param>
        /// <param name="b">Constant b.</param>
        /// <param name="c">Constant c.</param>
		public StraightLine(float a, float b, float c) {
            _a = a;
            _b = b;
            _c = c;

            _baseX = 0;
            _baseY = 0;
		}
        /// <summary>
        /// Define a straight line by two points.
        /// </summary>
        /// <param name="a">Point A</param>
        /// <param name="b">Point B</param>
        public StraightLine(Vector2 a, Vector2 b)
        {
            _a = a.Y - b.Y;
            _b = b.X - a.X;
            _c = a.X * b.Y - b.X * a.Y;

            if (a.X == b.X)
                _baseX = a.X;
            else
                _baseX = 0;

            if (a.Y == b.Y)
                _baseY = a.Y;
            else
                _baseY = 0;
        }
        /// <summary>
        /// Define a straight line by using a line segment.
        /// </summary>
        /// <param name="ls">Line segment.</param>
        public StraightLine(LineSegment ls) : this(ls.A_Global, ls.B_Global)
        {}

        public float A { get { return _a; } }
        public float B { get { return _b; } }
        public float C { get { return _c; } }
        public float Gradient
        {
            get { return -_a/_b; }
        }
        public float Intercept
        {
            get { return - _c/_b; }
        }
        public Vector2 Normal
        {
            get { return new Vector2(_a, _b); }
        }
        public Vector2 Direction
        {
            get { return new Vector2(-_b, _a); }
        }
        public bool IsVertical
        {
            get { return _b == 0; }
        }
        public bool IsHorizontal
        {
            get { return _a == 0; }
        }
        public bool IsParallel(StraightLine L2)
        {
            return (IsVertical && L2.IsVertical)
                || Gradient == L2.Gradient;
        }
        public bool IsEqual(StraightLine L2)
        {
            return A == L2.A && B == L2.B && C == L2.C;
        }
        public float GetX(float y)
        {
            if (IsVertical)
                return _baseX;
            return -_b * y / _a - _c / _a;
        }
        public float GetY(float x)
        {
            if (IsHorizontal)
                return _baseY;
            return Gradient * x + Intercept;
        }
		   
        //public bool Intersects(Line l2){
        //    float q=(this.A.Y-l2.A.Y)*(l2.B.X-l2.A.X)-(this.A.X-l2.A.X)*(l2.B.Y-l2.A.Y);
        //    float d=(this.B.X-this.A.X)*(l2.B.Y-l2.A.Y)-(this.B.Y-this.A.Y)*(l2.B.X-l2.A.X);
        //    if (d==0)
        //        return false;
        //    float r=q/d;
        //    q=(this.A.Y-l2.A.Y)*(this.B.X-this.A.X)-(this.A.X-l2.A.X)*(this.B.Y-this.A.Y);
        //    float s=q/d;
        //    if (r<0 || r>1 || s<0 || s>1)
        //        return false;
        //    return true;
        //}
        //public bool Intersects(Rectangle r)
        //{
        //    return (r.Contains((int)A.X, (int)A.Y) && r.Contains((int)B.X, (int)B.Y) ||
        //           Intersects(new Line(new Vector2(r.X, r.Y), new Vector2(r.X + r.Width, r.Y))) ||
        //           Intersects(new Line(new Vector2(r.X + r.Width, r.Y), new Vector2(r.X + r.Width, r.Y + r.Height))) ||
        //           Intersects(new Line(new Vector2(r.X + r.Width, r.Y + r.Height), new Vector2(r.X, r.Y + r.Height))) ||
        //           Intersects(new Line(new Vector2(r.X, r.Y + r.Height), new Vector2(r.X, r.Y))));
        //}
        //public static Vector2 IntersectionPoint(Line l1, Line l2)
        //{
        //    return l1.IntersectionPoint(l2);
        //}
        //public Vector2 IntersectionPoint(Line l2)
        //{
        //    Vector2 result;
        //    float gradient = Gradient;
        //    float intercept = Intercept;
        //    result.X = (l2.Intercept - intercept) / (gradient - l2.Gradient);
        //    result.Y = gradient * result.X + intercept;
        //    return result;
        //}
	}
}

