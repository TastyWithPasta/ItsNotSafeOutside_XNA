using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TestBed
{
	public static class TouchInput
	{
		static MouseState m_currentTouch, m_previousTouch;
        static Camera2D _camera;
        static Vector2 _cameraTouchPosition, _prevCameraTouchPosition;

        public static bool ScreenIsNoLongerTouched
        {
            get { return ScreenWasTouched && !IsScreenTouched; }
        }
		public static bool ScreenWasTouched
		{
			get{ return m_previousTouch.LeftButton == ButtonState.Pressed; }
		}
        public static bool IsScreenTouched
        {
            get { return m_currentTouch.LeftButton == ButtonState.Pressed; }
        }
        public static bool IsScreenTapped
        {
            get { return IsScreenTouched && !ScreenWasTouched;  }
        }
        public static Vector2 PrevTouchPosition
        {
            get
            {
                if (ScreenWasTouched)
                    return new Vector2(m_previousTouch.X, m_previousTouch.Y);
                else
                    return TouchPosition;
            }
        }
        public static Vector2 TouchPosition
        {
            get
            {
                if (IsScreenTouched)
                    return new Vector2(m_currentTouch.X, m_currentTouch.Y);
                else
                    return Vector2.Zero;
            }
        }
        public static Vector2 TouchDifference
        {
            get {
                if (TouchPosition == Vector2.Zero)
                    return Vector2.Zero;
                return TouchPosition - PrevTouchPosition; 
            }
        }
        public static Vector2 PreviousCameraTouchPosition
        {
            get { return _prevCameraTouchPosition; }
        }
        public static Vector2 CameraTouchPosition
        {
            get { return _cameraTouchPosition; }
        }

        public static void BindCamera(Camera2D camera)
        {
            _camera = camera;
        }

        public static void Update()
        {
            m_previousTouch = m_currentTouch;
            m_currentTouch = Mouse.GetState();
            if (_camera != null)
            {
                if (ScreenWasTouched)
                    _prevCameraTouchPosition = _cameraTouchPosition; 
                _cameraTouchPosition = Vector2.Transform(TouchPosition, Matrix.Invert(_camera.CameraMatrix));
                if (!ScreenWasTouched)
                    _prevCameraTouchPosition = _cameraTouchPosition;
            }
        }

        public static bool TouchInZone(Rectangle zone)
        {
            if (!IsScreenTouched)
                return false;
            Vector2 p = TouchPosition;
            return zone.Contains((int)p.X, (int)p.Y);
        }
        public static bool TapInZone(Rectangle zone)
        {
            if (!IsScreenTapped)
                return false;
            Vector2 p = TouchPosition;
            return zone.Contains((int)p.X, (int)p.Y);
        }
    }
}

	//public static class TouchInput
	//{
	//    static TouchCollection _tc, _prevtc;
	//    static Camera2D _camera;
	//    static Vector2 _cameraTouchPosition, _prevCameraTouchPosition;

	//    public static bool ScreenIsNoLongerTouched
	//    {
	//        get { return _prevtc.Count != 0 && _tc.Count == 0; }
	//    }
	//    public static bool IsScreenTouched
	//    {
	//        get { return _tc.Count > 0; }
	//    }
	//    public static bool IsScreenTapped
	//    {
	//        get { return _tc.Count > 0 && _prevtc.Count == 0; }
	//    }
	//    public static Vector2 PrevTouchPosition
	//    {
	//        get
	//        {
	//            if (_prevtc.Count > 0)
	//                return _prevtc[0].Position;
	//            else
	//                return TouchPosition;
	//        }
	//    }
	//    public static Vector2 TouchPosition
	//    {
	//        get
	//        {
	//            if (IsScreenTouched)
	//                return _tc[0].Position;
	//            else
	//                return Vector2.Zero;
	//        }
	//    }
	//    public static Vector2 TouchDifference
	//    {
	//        get {
	//            if (TouchPosition == Vector2.Zero)
	//                return Vector2.Zero;
	//            return TouchPosition - PrevTouchPosition; 
	//        }
	//    }
	//    public static Vector2 PreviousCameraTouchPosition
	//    {
	//        get { return _prevCameraTouchPosition; }
	//    }
	//    public static Vector2 CameraTouchPosition
	//    {
	//        get { return _cameraTouchPosition; }
	//    }

	//    public static void BindCamera(Camera2D camera)
	//    {
	//        _camera = camera;
	//    }

	//    public static void Update()
	//    {
	//        _prevtc = _tc;
	//        _tc = TouchPanel.GetState();
	//        if (_camera != null)
	//        {
	//            if (_prevtc.Count != 0)
	//                _prevCameraTouchPosition = _cameraTouchPosition; 
	//            _cameraTouchPosition = Vector2.Transform(TouchPosition, Matrix.Invert(_camera.CameraMatrix));
	//            if (_prevtc.Count == 0)
	//                _prevCameraTouchPosition = _cameraTouchPosition;
	//        }
	//    }

	//    public static bool TouchInZone(Rectangle zone)
	//    {
	//        if (!IsScreenTouched)
	//            return false;
	//        Vector2 p = TouchPosition;
	//        return zone.Contains((int)p.X, (int)p.Y);
	//    }
	//    public static bool TapInZone(Rectangle zone)
	//    {
	//        if (!IsScreenTapped)
	//            return false;
	//        Vector2 p = TouchPosition;
	//        return zone.Contains((int)p.X, (int)p.Y);
	//    }
	//}

