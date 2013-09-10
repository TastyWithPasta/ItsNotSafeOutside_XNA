using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PastaGameLibrary
{
	public class Transform
	{
		Matrix m_localTransform;
		Vector2 m_position = Vector2.Zero;
		Vector2 m_scale = Vector2.One;	
		double m_direction = 0;
		bool m_updateTransformMatrixFlag = true;

		Transform m_parentTransform = null;

		public Transform()
		{}
		public Transform(Transform other)
		{
			m_localTransform = other.m_localTransform;
			m_position = other.m_position;
			m_scale = other.m_scale;
			m_direction = other.m_direction;
			m_updateTransformMatrixFlag = other.m_updateTransformMatrixFlag;
			m_parentTransform = other.m_parentTransform;
		}
		public Transform(Transform other, bool isParent)
		{
			if (isParent)
				m_parentTransform = other;
			else
			{
				m_localTransform = other.m_localTransform;
				m_position = other.m_position;
				m_scale = other.m_scale;
				m_direction = other.m_direction;
				m_updateTransformMatrixFlag = other.m_updateTransformMatrixFlag;
				m_parentTransform = other.m_parentTransform;
			}
		}

		public double DirX
		{
			get { return Math.Cos(m_direction); }
			//set
			//{
			//    if (value > 1)
			//        value = 1.0;
			//    else if (value < -1)
			//        value = -1;
			//    _direction = m_helper.GetAngle(value, Math.Sin(m_direction));
			//}
		}
		public double DirY
		{
			get { return Math.Sin(m_direction); }
			//set
			//{
			//    if (value > 1)
			//        value = 1.0;
			//    else if (value < -1)
			//        value = -1;
			//    _direction = GeometryHelper.GetAngle(Math.Cos(_direction), value);
			//}
		}
		public float PosX
		{
			get { return m_position.X; }
			set {
				m_updateTransformMatrixFlag = true;
				m_position.X = value; 
			}
		}
		public float PosY
		{
			get { return m_position.Y; }
			set {
				m_updateTransformMatrixFlag = true;
				m_position.Y = value; }
		} 
		public Vector2 Position
		{
			get
			{
				return m_position;
			}
			set
			{
				m_updateTransformMatrixFlag = true;
				m_position = value;
			}
		}
		public Vector2 PositionGlobal
		{
			get
			{
				if (m_parentTransform == null)
					return Position;
				return Vector2.Transform(Vector2.Zero, GlobalTransform);
			}
			set
			{
				throw new NotImplementedException();
			}
		}
		public double DirectionGlobal
		{
			get
			{
				if (m_parentTransform == null)
					return Direction;
				return Direction + m_parentTransform.DirectionGlobal;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public float SclX
		{
			get { return m_scale.X; }
			set { m_scale.X = value;
			m_updateTransformMatrixFlag = true;
			}
		}
		public float SclY
		{
			get { return m_scale.Y; }
			set { m_scale.Y = value;
			m_updateTransformMatrixFlag = true;
			}
		}
		public float ScaleUniform
		{
			set { m_scale.X = value; m_scale.Y = value;
			m_updateTransformMatrixFlag = true;
			}
		}
		public Vector2 Scale
		{
			get { return m_scale; }
			set {
				m_updateTransformMatrixFlag = true;
				m_scale = value; }
		}
		public Vector2 ScaleGlobal
		{
			get {
				if (m_parentTransform == null)
					return m_scale;
				return m_scale * m_parentTransform.ScaleGlobal; }
		}
		public Transform ParentTransform
		{
			get { return m_parentTransform; }
			set { m_parentTransform = value; }
		}
		public Vector2 DirectionUnit
		{
			get
			{
				return new Vector2((float)Math.Cos(m_direction), (float)Math.Sin(m_direction));
			}
			set
			{
				throw new NotImplementedException();
			}
		}
		public double Direction
		{
			get
			{
				return m_direction;
			}
			set
			{
				m_direction = value;
				m_updateTransformMatrixFlag = true;
			}
		}
		public Matrix LocalTransform
		{
			get {
				if (m_updateTransformMatrixFlag)
				{
					UpdateLocalTransform();
					m_updateTransformMatrixFlag = false;
				}
				return m_localTransform; }
		}
		public Matrix GlobalTransform
		{
			get
			{
				if (m_parentTransform == null)
					return LocalTransform;
				return LocalTransform * m_parentTransform.GlobalTransform;
			}
		}

		//From ZoomEngine, publicly available
		private void UpdateLocalTransform()
		{
			Matrix rotM, scaleM, posM, temp;

			Matrix.CreateRotationZ((float)Direction, out rotM);
			Matrix.CreateScale(Scale.X, Scale.Y, 1, out scaleM);
			Matrix.CreateTranslation(Position.X, Position.Y, 0, out posM);

			//m_localTransform = posM * scaleM * rotM;
			Matrix.Multiply(ref scaleM, ref rotM, out temp);
			Matrix.Multiply(ref temp, ref posM, out m_localTransform);

			//Camera transform
			//Matrix.Multiply(ref posM, ref scaleM, out temp);
			//Matrix.Multiply(ref temp, ref rotM, out m_localTransform);
		}
		public void GetGlobalComponents(out Vector2 positionGlobal, out float rotationGlobal, out Vector2 scaleGlobal)
		{
		    if (m_parentTransform == null)
		    {
		        positionGlobal = Position;
		        rotationGlobal = (float)Direction;
		        scaleGlobal = Scale;
		        return;
		    }

		    Vector3 position3, scale3;
		    Quaternion rotationQ;
		    GlobalTransform.Decompose(out scale3, out rotationQ, out position3);
		    Vector2 direction = Vector2.Transform(Vector2.UnitX, rotationQ);
		    rotationGlobal = (float)Math.Atan2((double)(direction.Y), (double)(direction.X));
		    positionGlobal = new Vector2(position3.X, position3.Y);
		    scaleGlobal = new Vector2(scale3.X, scale3.Y);
		}
	}
}
