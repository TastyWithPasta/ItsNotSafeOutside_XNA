using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PastaGameLibrary;
using Microsoft.Xna.Framework;

namespace TestBed
{
	public class GameScene
	{
		const float WaveZoom = 2.0f;
		const float IntroZoom = 0.8f;
		const float IntroOnExitZoom = 1.2f;

		Backpacker[] m_backpackers;
		BackpackerGroup m_backpackerGroup;

		Level m_currentLevel;
		Level m_nextLevel;

		Sequence m_introSequence;
		MoveToStaticAction m_cameraIntroForwardPan, m_cameraIntroBackwardsPan;

		public GameScene()	
		{
			Concurrent cameraPan;
			Sequence panZoomSequence = new Sequence();

			m_introSequence = new Sequence();

			///
			/// Camera pan forward T = 5s
			///
			m_cameraIntroForwardPan = new MoveToStaticAction(Globals.TheGame, World.cam_Main.Transform, Vector2.Zero, false);
			m_cameraIntroForwardPan.Timer.Interval = 5.0f;
			m_cameraIntroForwardPan.Interpolator = new PSmoothstepInterpolation();

			ScaleToAction zoom1 = new ScaleToAction(Globals.TheGame, World.cam_Main.Transform, new Vector2(IntroZoom, IntroZoom), false);
			zoom1.Timer.Interval = 3.0f;
			zoom1.StartScale = new Vector2(WaveZoom, WaveZoom);
			zoom1.Interpolator = new PSmoothstepInterpolation();
			panZoomSequence.AddAction(zoom1);

			panZoomSequence.AddAction(new DelayAction(Globals.TheGame, 1.0f));

			ScaleToAction zoom2 = new ScaleToAction(Globals.TheGame, World.cam_Main.Transform, new Vector2(IntroOnExitZoom, IntroOnExitZoom), false);
			zoom2.Timer.Interval = 1.0f;
			zoom2.StartScale = zoom1.Target;
			zoom2.Interpolator = new PSmoothstepInterpolation();
			panZoomSequence.AddAction(zoom2);

			cameraPan = new Concurrent(new PastaGameLibrary.Action[] { m_cameraIntroForwardPan, panZoomSequence });
			m_introSequence.AddAction(cameraPan);

			m_introSequence.AddAction(new DelayAction(Globals.TheGame, 0.5f));

			///
			/// Camera pan backwards T = 3s
			/// 
			m_cameraIntroBackwardsPan = new MoveToStaticAction(Globals.TheGame, World.cam_Main.Transform, Vector2.Zero, false);
			m_cameraIntroBackwardsPan.Timer.Interval = 3.0f;
			m_cameraIntroBackwardsPan.Interpolator = new PSmoothstepInterpolation();
			zoom1 = new ScaleToAction(Globals.TheGame, World.cam_Main.Transform, new Vector2(1.0f, 1.0f), false);
			zoom1.Timer.Interval = 1.0f;
			zoom1.StartScale = zoom2.Target;
			zoom1.Interpolator = new PSmoothstepInterpolation();
			cameraPan = new Concurrent(new PastaGameLibrary.Action[] { m_cameraIntroBackwardsPan, zoom1 });
			m_introSequence.AddAction(cameraPan);
		}

		public void BuildNextLevel()
		{
			if (m_currentLevel == null)
			{
				m_currentLevel = new Level();
			}
			else
			{
				m_currentLevel.Clear();
				m_nextLevel.MoveLevelLeft();
				m_currentLevel = m_nextLevel;
			}

			 m_nextLevel = LevelGenerator.GetNextLevel(m_currentLevel);
		}

		public void StartCurrentLevel()
		{
			m_currentLevel.GenerateLevel();
		}
		public void StartIntro()
		{
			m_cameraIntroForwardPan.StartPosition = new Vector2(300, -100);
			m_cameraIntroForwardPan.Target = new Vector2(m_currentLevel.TotalLevelWidth, -100);
			m_cameraIntroBackwardsPan.StartPosition = new Vector2(m_currentLevel.TotalLevelWidth, -100); ;
			m_cameraIntroBackwardsPan.Target = new Vector2(500, -100); ;
			m_introSequence.Start();
		}

		public void Update()
		{
			m_introSequence.Update();
		}
	}
}
