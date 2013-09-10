using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TestBed
{
	public class EndLevelScene : Scene
	{
		DynamicMenuScene[] _dynamicMenuScreens;
		DynamicMenuScene _currentMenuScreen;

		public EndLevelScene()
		{
			_dynamicMenuScreens = new DynamicMenuScene[] 
            {
                new NewLevelScene(),
            };
		}

		private void SwitchToMenu(DynamicMenuScene newMenu)
		{
			World.cam_Main.Transform.Position = newMenu.Hotspot.PositionGlobal;
			if (newMenu == _currentMenuScreen)
				return;
			if (_currentMenuScreen != null)
				_currentMenuScreen.Transition(SceneState.Inactive, null);
			_currentMenuScreen = newMenu;
			_currentMenuScreen.Transition(SceneState.Active, null);
			//Globals.Ninja.MoveNinja(_currentMenuScreen);
		}

		private void PlaceHotspots()
		{
			//InsoSceneManager.ScoreScreen.Hotspot.Position = new Vector2(_finishHouse.HouseSprite.ScreenX - 30, CameraOffsetY - 80);
			//InsoSceneManager.ShopScreen.Hotspot.Position = InsoSceneManager.ScoreScreen.Hotspot.Position + new Vector2(-100, -300);
			//NewLevelScreen.Hotspot.Position = new Vector2(_finishHouse.RallyPoint - 100, -150);
		}
	}
}
