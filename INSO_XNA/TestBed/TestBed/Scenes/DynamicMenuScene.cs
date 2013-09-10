using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PastaGameLibrary;
using Microsoft.Xna.Framework;

namespace TestBed
{
	public class DynamicMenuScene : Scene
	{
		protected enum MenuBoundary
		{
			None = -1,
			Left = 0,
			Top = 1,
			Right = 2,
			Bottom = 3,
		};

		bool _buttonWasPressed;
		Transform _hotspot;

		protected float _leftBound = -100;
		protected float _rightBound = 100;
		protected float _topBound = -100;
		protected float _bottomBound = 100;

		public Transform Hotspot
		{
			get { return _hotspot; }
			set { _hotspot = value; }
		}


		protected bool CameraHasCrossedBoundaries()
		{
			if (World.cam_Main.Transform.ParentTransform != _hotspot)
				return false;

			Vector2 cameraPos = World.cam_Main.Transform.PositionGlobal;
			float x = cameraPos.X;
			float y = cameraPos.Y;

			if (y > _bottomBound
				|| y < _topBound
				|| x < _leftBound
				|| x > _rightBound)
			{
				return true;
			}
			return false;
		}
		protected void SlideInBoundaries()
		{
			if (!TouchInput.IsScreenTouched || World.cam_Main.Transform.ParentTransform != _hotspot)
				return;

			World.cam_Main.Transform.Position += TouchInput.TouchDifference * -1.5f;

			Vector2 cameraPos = World.cam_Main.Transform.PositionGlobal;
			float x = cameraPos.X;
			float y = cameraPos.Y;

			if (y > _bottomBound)
			{
				y = _bottomBound;
			}
			else if (y < _topBound)
			{
				y = _topBound;
			}
			if (x < _leftBound)
			{
				x = _leftBound;
			}
			else if (x > _rightBound)
			{
				x = _rightBound;
			}

			World.cam_Main.Transform.PosX = x;
			World.cam_Main.Transform.PosY = y;
		}

		public void DisableSlide()
		{
			_buttonWasPressed = true;
		}
		public bool IsSlideDisabled()
		{
			return _buttonWasPressed;
		}

		public DynamicMenuScene()
			: base()
		{ }

		public override void Update(GameTime gameTime)
		{
			_buttonWasPressed = false;
			base.Update(gameTime);
		}

		public static void SlideMenuView(DynamicMenuScene sender, DynamicMenuScene[] targets, Scene.SceneState finalSenderState, Scene.SceneState finalTargetState)
		{
			DynamicMenuScene target = null;
			if (TouchInput.IsScreenTouched)
			{
				World.cam_Main.Transform.Position += TouchInput.TouchDifference * -1.2f;
			}
			else if (TouchInput.ScreenIsNoLongerTouched)
			{
				target = GetClosestMenu(targets);
				if (target == sender)
				{
					target.Transition(target.State, null);
					return;
				}
				if (sender != null)
					sender.Transition(finalSenderState, target);
				target.Transition(finalTargetState, sender);
			}
		}
		private static DynamicMenuScene GetClosestMenu(DynamicMenuScene[] menus)
		{
			float minDistance = 9999;
			int minIndex = 0;
			float distance = 0;
			for (int i = 0; i < menus.Length; ++i)
			{
				distance = Vector2.Distance(World.cam_Main.Transform.PositionGlobal, menus[i].Hotspot.PositionGlobal);
				if (distance < minDistance)
				{
					minDistance = distance;
					minIndex = i;
				}
			}
			return menus[minIndex];
		}
	}

}
