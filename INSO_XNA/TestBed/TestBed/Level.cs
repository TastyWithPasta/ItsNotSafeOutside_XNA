using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PastaGameLibrary;

namespace TestBed
{

	/// <summary>
	/// 
	/// When constructor is called: 
	/// -Child spawners are created from parents (or without parents).
	/// -Landscape is generated.
	/// 
	/// When next level starts (backpackers get out):
	/// -Current Level is now Next Level
	/// -Next level is obtained from generator.
	/// 
	/// </summary>

    public class Level
    {
		// EnemyManager _enemyManager = null;

		public enum LevelObjectType
		{
			Backgrounds = 0,
			EarthTile = 1,
			GroundEnemy = 2,
		}

		const int LevelObjectTypeCount = 3;

		SpawnerGroup[] m_spawners = new SpawnerGroup[LevelObjectTypeCount];
		List<GameObject>[] m_levelObjects = new List<GameObject>[LevelObjectTypeCount];

		const int InitialMutationAmount = 1;
		const float TreeRatio = 0.5f;
		const int AmountOfSectionTypes = 2;

		const int InactiveTileCount = 1; //Tiles in front of the house
		const int ActiveTileCount = 5; //Level tiles on which the action takes place
		const int GroundEnemySpawnersPerTile = 5;

		public const int GroundSectionIndex = 0;
		public const int TreeSectionIndex = 1;

		House m_house;
		float m_difficulty;
		EarthTile[] m_earthTiles = new EarthTile[ActiveTileCount + InactiveTileCount]; //One extra tile is used for the house

		public int TileCount
		{
			get { return InactiveTileCount + ActiveTileCount; }
		}
		public float ActiveAreaWidth
		{
			get { return ActiveTileCount * EarthTile.EarthTileWidth; }
		}
		public float InactiveAreaWidth
		{
			get { return InactiveTileCount * EarthTile.EarthTileWidth; }
		}
        public float TotalLevelWidth
        {
			get { return (ActiveTileCount + InactiveTileCount) * EarthTile.EarthTileWidth; }
        }
		public float Difficulty
		{
			get { return m_difficulty; }
		}
		public SpawnerGroup GetSpawners(LevelObjectType type)
		{
			return m_spawners[(int)type];
		}

		public Level()
		{
			CreateSpawnersFirstTime();
			GenerateLandscape();
		}
		public Level(Level mum, Level dad)
		{
			CreateChildSpawners(LevelObjectType.GroundEnemy, mum, dad);
			GenerateLandscape();
		}

		public void Clear()
		{
			for (int i = 0; i < m_levelObjects.Length; ++i)
				if(m_levelObjects[i] != null)
					for (int j = 0; j < m_levelObjects[i].Count; ++j)
						m_levelObjects[i][j].ClearLists();

			for (int i = 0; i < m_levelObjects.Length; ++i)
				m_levelObjects[i].Clear();
			
			m_house.ClearLists();
		}

		//When next level is built, move level to the left
		public void MoveLevelLeft()
		{
			for (int i = 0; i < m_levelObjects.Length; ++i)
				if(m_levelObjects[i] != null)
					for (int j = 0; j < m_levelObjects[i].Count; ++j)
						m_levelObjects[i][j].Transform.PosX -= TotalLevelWidth;

			m_house.Transform.PosX -= TotalLevelWidth;
		}
		public void MoveLevelRight(Level left)
		{
			float offset = left.TotalLevelWidth;
			for (int i = 0; i < m_levelObjects.Length; ++i)
				if (m_levelObjects[i] != null)
					for (int j = 0; j < m_levelObjects[i].Count; ++j)
						m_levelObjects[i][j].Transform.PosX += offset;

			m_house.Transform.PosX += offset;
		}

		private void CreateSpawnersFirstTime()
		{
			///
			///Ground Spawners
			///
			Spawner[] groundSpawners = new Spawner[ActiveTileCount * GroundEnemySpawnersPerTile];
			float increment = ActiveAreaWidth / (float)groundSpawners.Length;
			for (int i = 0; i < groundSpawners.Length; ++i)
				groundSpawners[i] = new Spawner(new Gene(8), new PastaGameLibrary.UnitPoint(new Transform(), new Vector2(EarthTile.EarthTileWidth + i * increment, 0)));
			m_spawners[(int)LevelObjectType.GroundEnemy] = new SpawnerGroup(groundSpawners, LevelGenerator.GroundSpawnTypes);
		}
		private void CreateChildSpawners(LevelObjectType type, Level mum, Level dad)
		{
			SpawnerGroup dadSpawners = dad.GetSpawners(type);
			SpawnerGroup mumSpawners = mum.GetSpawners(type);
			SpawnerGroup newSpawners = new SpawnerGroup(mumSpawners);
			newSpawners.BuildFromParents(mumSpawners, dadSpawners);
			m_spawners[(int)type] = newSpawners;
		}

		private void GenerateLandscape()
		{
			List<GameObject> spawnedObjects = new List<GameObject>();

			m_house = new Hs_Building();

			///
			///	Backgrounds
			///
			List<BackgroundObject> backgrounds;
			backgrounds = BackgroundObject.CreateBackgroundObjects((int)(ActiveTileCount * 1.25f), null, new Vector2(TotalLevelWidth, 1), new Vector2(50, 0), StyleManager.LevelStyle.Country, 0);
			for (int i = 0; i < backgrounds.Count; ++i)
				backgrounds[i].Transform.PosY -= 20;
			spawnedObjects.AddRange(backgrounds);

			backgrounds = BackgroundObject.CreateBackgroundObjects((int)(ActiveTileCount * 1.5f), null, new Vector2(TotalLevelWidth, 1), new Vector2(50, 0), StyleManager.LevelStyle.Country, 1);
			for (int i = 0; i < backgrounds.Count; ++i)
				backgrounds[i].Transform.PosY -= 100;
			spawnedObjects.AddRange(backgrounds);

			backgrounds = BackgroundObject.CreateBackgroundObjects(ActiveTileCount * 2, null, new Vector2(TotalLevelWidth, 1), new Vector2(50, 0), StyleManager.LevelStyle.Country, 2);
			for (int i = 0; i < backgrounds.Count; ++i)
				backgrounds[i].Transform.PosY -= 200;
			spawnedObjects.AddRange(backgrounds);

			m_levelObjects[(int)LevelObjectType.Backgrounds] = spawnedObjects;

			///
			/// Earth Tiles
			/// 
			spawnedObjects = new List<GameObject>();
			for (int i = 0; i < m_earthTiles.Length; ++i)
			{
				m_earthTiles[i] = new EarthTile(i);
				spawnedObjects.Add(m_earthTiles[i]);
			}
			m_levelObjects[(int)LevelObjectType.EarthTile] = spawnedObjects;
		}
		public void GenerateLevel()
		{
			List<GameObject> spawnedObjects = new List<GameObject>();

			///
			/// Ground Enemies
			///
			spawnedObjects = m_spawners[(int)LevelObjectType.GroundEnemy].Spawn(World.UL_Global, World.DL_GroundItems);
			m_levelObjects[(int)LevelObjectType.GroundEnemy] = spawnedObjects;
		}

		private void InitialiseGroundSpawners()
		{
		//    Spawner[] groundSpawners = new Spawner[EarthTileCount];
		//    Transform transform;
		//    float increment = PlayAreaWidth / (float)GroundEnemySpawnerCount;
		//    for (int i = 0; i < groundSpawners.Length; ++i)
		//    {
		//        transform = new Transform(m_transform, true);
		//        Vector2 pos = new Vector2(i * increment, -42);
		//        groundSpawners[i] = new Spawner(new Gene(8), new PastaGameLibrary.UnitPoint(transform, pos));
		//    }
		//    m_spawners.Add("groundEnemies", new SpawnerGroup(groundSpawners, GroundSpawnTypes));
		}
		private void InitialisePostLevelSpawners()
		{
			//_levelSections[TreeSectionIndex] = new Tpl_Tree[(int)(LevelLength * TreeRatio)];
			//Tpl_Tree.placementIncrement = (int)(GroundSection.GroundSectionWidth / TreeRatio);
			//for (int i = 0; i < _levelSections[TreeSectionIndex].Length; ++i)
			//    _levelSections[TreeSectionIndex][i] = new Tpl_Tree(i);
		}
    }
}
