using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PastaGameLibrary;

namespace TestBed
{
    public class Level
    {
		Transform m_transform;
       // EnemyManager _enemyManager = null;

		Dictionary<string, SpawnerGroup> m_spawners;
		Dictionary<string, List<GameObject>> m_levelObjects;

		const int InitialMutationAmount = 1;
		const float TreeRatio = 0.5f;
		const int AmountOfSectionTypes = 2;

		const int EarthTileCount = 10;
		const int GroundEnemySpawnersPerTile = 3;
		const int GroundEnemySpawnerCount = 15;

		public const int GroundSectionIndex = 0;
		public const int TreeSectionIndex = 1;

		House m_house;
		float m_difficulty;
		EarthTile[] m_earthTiles = new EarthTile[EarthTileCount];

        public float PlayAreaWidth
        {
            get { return EarthTileCount * EarthTile.EarthTileWidth; }
        }
		public float Difficulty
		{
			get { return m_difficulty; }
		}
		public SpawnerGroup GetSpawners(string name)
		{
			return m_spawners[name];
		}

		public Level(Transform transform)
		{
			m_transform = transform;

			for (int i = 0; i < m_earthTiles.Length; ++i)
				m_earthTiles[i] = new EarthTile(i);

			m_house = new Hs_Building();
		}
		public Level(Level mum, Level dad)
		{
			m_spawners = new Dictionary<string, SpawnerGroup>();
			InitialiseGroundSpawners();
			InitialisePostLevelSpawners();

			CreateChildSpawners("groundEnemies", mum, dad);
		}

		public void Clear()
		{
			foreach (KeyValuePair<string, List<GameObject>> kvp in m_levelObjects)
				for (int i = 0; i < kvp.Value.Count; ++i)
					kvp.Value[i].ClearLists();
			m_levelObjects.Clear();
			m_house.ClearLists();
		}

		public void BuildFirst()
		{
			m_spawners = new Dictionary<string, SpawnerGroup>();

			Spawner[] groundSpawners = new Spawner[EarthTileCount * GroundEnemySpawnersPerTile];
			Transform transform;
			float increment = PlayAreaWidth / (float)groundSpawners.Length;
			for (int i = 0; i < groundSpawners.Length; ++i)
			{
				transform = new Transform(m_transform, true);
				Vector2 pos = new Vector2(i * increment, -42);
				groundSpawners[i] = new Spawner(new Gene(8), new PastaGameLibrary.UnitPoint(transform, pos));
			}
			m_spawners.Add("groundEnemies", new SpawnerGroup(groundSpawners, LevelGenerator.GroundSpawnTypes));

			m_spawners["groundEnemies"].Spawn(World.UL_Global, World.DL_GroundItems);
		}

		public void BuildFromParents(Level mum, Level dad)
		{
			m_spawners = new Dictionary<string, SpawnerGroup>();

			InitialiseGroundSpawners();
			InitialisePostLevelSpawners();
			CreateChildSpawners("groundEnemies", mum, dad);
		}

		private void CreateChildSpawners(string name, Level mum, Level dad)
		{
			SpawnerGroup dadSpawners = dad.GetSpawners(name);
			SpawnerGroup mumSpawners = mum.GetSpawners(name);
			SpawnerGroup newSpawners = new SpawnerGroup(mumSpawners);
			newSpawners.BuildFromParents(mumSpawners, dadSpawners);
			m_spawners.Add(name, newSpawners);
		}

		public SpawnerGroup GetSpawnerGroup(string name)
		{
			return m_spawners[name];
		}

		public void Build()
		{
			foreach (KeyValuePair<string, SpawnerGroup> kvp in m_spawners)
				m_levelObjects.Add(kvp.Key, kvp.Value.Spawn());
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
