using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using PastaGameLibrary;

namespace TestBed
{
    public class Camera2D
    {
		float m_zoom = 1;
		Transform m_transform;
		Matrix m_centerMatrix, m_cameraMatrix;
		bool m_scaleToZoom = false;

		public Transform Transform
		{
			get { return m_transform; }
		}

		public bool ScaleToZoom
		{
			get { return m_scaleToZoom; }
			set { m_scaleToZoom = value; }
		}
        public float Zoom
        {
			get {
				if (m_scaleToZoom)
					return m_transform.SclX;
				return m_zoom; 
			}
			set {
				if (m_scaleToZoom)
					m_transform.ScaleUniform = value;
				m_zoom = value; 
			}
        }
        public Matrix CameraMatrix
        {
            get { 
                return m_cameraMatrix * m_centerMatrix; 
            }
        }

		private void UpdateCameraMatrix()
		{
			Matrix rotM, scaleM, posM, temp;

			Vector2 posGlobal = m_transform.PositionGlobal;
			Matrix.CreateRotationZ((float)m_transform.Direction, out rotM);
			if (m_scaleToZoom)
			{
				m_zoom = m_transform.ScaleGlobal.X;

				////Negates parent base zoom (1) so that it doesn't set the base zoom to two (parent(1) + child(1) = 2)
				//if (m_transform.ParentTransform != null)
				//    m_zoom--;
			}
			Matrix.CreateScale(m_zoom, m_zoom, 1, out scaleM);
			Matrix.CreateTranslation(posGlobal.X, -posGlobal.Y, 0, out posM);

			//Different order than transform matrix!
			Matrix.Multiply(ref posM, ref scaleM, out temp);
			Matrix.Multiply(ref temp, ref rotM, out m_cameraMatrix);
		}

        public Camera2D(Vector2 focusPoint)
        {
			m_transform = new Transform();
			m_centerMatrix = Matrix.CreateTranslation(Globals.TheGame.ScreenWidth * 0.5f, Globals.TheGame.ScreenHeight * 0.5f, 0);
        }

		//public void MoveTargetPosition(float x, float y)
		//{
		//    _panTarget.X += x;
		//    _panTarget.Y += y;
		//}
		//public void MoveTargetPosition(Vector2 moveVector)
		//{
		//    _panTarget.X += moveVector.X;
		//    _panTarget.Y += moveVector.Y;
		//}
		//public void SetTargetPosition(float x, float y)
		//{
		//    _panTarget.X = x;
		//    _panTarget.Y = y;
		//}
		//public void SetTargetPosition(Vector2 targetVector)
		//{
		//    _panTarget = targetVector;
		//}

		//public void SetTarget(Transform target)
		//{
		//    SetTarget(target, Vector2.Zero);
		//}
		//public void SetTarget(Transform target, Vector2 focusPoint)
		//{
		//    m_transform.ParentTransform = target;
		//    _panTarget = focusPoint;
		//}

		//public void PlaceOnTarget()
		//{
		//    m_transform.PosX = _panTarget.X;
		//    m_transform.PosY = _panTarget.Y;
		//}
		//public void LoseTarget()
		//{
		//    if (m_transform.ParentTransform != null)
		//        _panTarget += m_transform.ParentTransform.PositionGlobal;
		//    m_transform.ParentTransform = null;
		//}
        public void Update()
        {
			//m_transform.Position += (_panTarget - m_transform.Position) * _panSharpness;
			//m_transform.ScaleUniform = m_transform.SclX + (_zoomTarget - m_transform.SclX) * _zoomSharpness;
			UpdateCameraMatrix();
        }
    }
}
