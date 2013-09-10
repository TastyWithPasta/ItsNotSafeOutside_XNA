using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PastaGameLibrary;

namespace TestBed
{
	public class SpawnType
	{
		public Type objectType;
		public string objectName;
		public float appearanceRate;

		public SpawnType(Type type, string name, float rate)
		{
			objectType = type;
			objectName = name;
			appearanceRate = rate;
		}
		public SpawnType(SpawnType type)
		{
			objectType = type.objectType;
			objectName = type.objectName;
			appearanceRate = type.appearanceRate;
		}
	}
	public class SpawnerGroup
	{
		Spawner[] m_spawners;
		SpawnType[] m_spawnTypes;

		int m_mutationsPerReproduction = 0;
		Chromosome m_chromosome;

		public Chromosome Chromosome
		{
			get { return m_chromosome; }
			set { m_chromosome = value; }
		}

		public SpawnerGroup(Spawner[] spawners, SpawnType[] spawnTypes)
		{
			m_spawners = spawners;
			m_spawnTypes = spawnTypes;

			Gene[] genes = new Gene[spawners.Length];
			for(int i = 0; i < m_spawners.Length; ++i)
				genes[i] = m_spawners[i].Gene;
			m_chromosome = new Chromosome(genes);

			float totalAppearanceRates = 0;
			for (int i = 0; i < spawnTypes.Length; ++i)
				totalAppearanceRates += m_spawnTypes[i].appearanceRate;
			m_spawnTypes[0].appearanceRate = m_spawnTypes[0].appearanceRate / totalAppearanceRates;
			for (int i = 1; i < spawnTypes.Length; ++i)
				m_spawnTypes[i].appearanceRate = m_spawnTypes[i].appearanceRate / totalAppearanceRates + m_spawnTypes[i - 1].appearanceRate;

#if DEBUG
			string debugstring = "Ratios : ";

			for (int i = 0; i < spawnTypes.Length; ++i)
				debugstring += " " + m_spawnTypes[i].appearanceRate / totalAppearanceRates;

#endif
		}
		public SpawnerGroup(SpawnerGroup groupToCopy)
		{
			m_spawners = new Spawner[groupToCopy.m_spawners.Length];
			for (int i = 0; i < m_spawners.Length; ++i)
				m_spawners[i] = new Spawner(groupToCopy.m_spawners[i]);
			m_spawnTypes = new SpawnType[groupToCopy.m_spawnTypes.Length];
			for (int i = 0; i < m_spawnTypes.Length; ++i)
				m_spawnTypes[i] = new SpawnType(groupToCopy.m_spawnTypes[i]);
		}


		public void BuildFromParents(SpawnerGroup mum, SpawnerGroup dad)
		{
			if (mum == null || dad == null)
				return;

			m_chromosome = mum.m_chromosome.MakeChild(dad.m_chromosome, m_mutationsPerReproduction);
			m_chromosome.Mutate(m_mutationsPerReproduction);
			m_chromosome.Reheat();
			for (int i = 0; i < m_spawners.Length; ++i)
				m_spawners[i].Gene = m_chromosome.Genes[i];

			m_chromosome.PrintData();
		}

		private Type GetTypeToSpawn(float ratio)
		{
			for (int i = 0; i < m_spawnTypes.Length; ++i)
				if (ratio < m_spawnTypes[i].appearanceRate)
					return m_spawnTypes[i].objectType;
			return null;
		}

		public List<GameObject> Spawn()
		{
			List<GameObject> spawned = new List<GameObject>();
			for (int i = 0; i < m_spawners.Length; ++i)
			{
				Type typeToSpawn = GetTypeToSpawn(m_spawners[i].SectionRatio);
				if (typeToSpawn == null)
					continue;
			}
			return spawned;
		}
		public List<GameObject> Spawn(UpdateList updateList, DrawingList drawingList)
		{
			List<GameObject> spawned = new List<GameObject>();
			GameObject gameObject;
			for (int i = 0; i < m_spawners.Length; ++i)
			{
				Type typeToSpawn = GetTypeToSpawn(m_spawners[i].SectionRatio);
				if (typeToSpawn == null)
					continue;
				gameObject = m_spawners[i].Spawn(typeToSpawn);
				spawned.Add(gameObject);
				updateList.Add(gameObject, 0);
				drawingList.Add(gameObject, 0);
			}
			return spawned;
		}
	}

	public class Spawner
	{
		GameObject m_owner;
		Gene m_gene;
		IArea m_spawnArea;

		public Object Owner
		{
			get { return m_owner; }
		}
		public Gene Gene
		{
			get { return m_gene; }
			set { m_gene = value; }
		}
		public float SectionRatio
		{
			get { return m_gene.Ratio; }
		}

		public Spawner(GameObject owner, Gene sectionGene, IArea spawnArea)
		{
			m_owner = owner;
			m_gene = sectionGene;
			m_spawnArea = spawnArea;
		}
		public Spawner(Gene sectionGene, IArea spawnArea)
		{
			m_owner = null;
			m_gene = sectionGene;
			m_spawnArea = spawnArea;
		}
		public Spawner(Spawner spawnerToCopy)
		{
			m_owner = spawnerToCopy.m_owner;
			m_gene = new Gene(spawnerToCopy.m_gene);
			m_spawnArea = spawnerToCopy.m_spawnArea;
		}
		public GameObject Spawn(Type typeToSpawn)
		{
			GameObject gameObject = (GameObject)Activator.CreateInstance(typeToSpawn);
			gameObject.Transform.Position = m_spawnArea.GetRandomPoint(Globals.Random);
			gameObject.OnSpawn(this);
			return gameObject;
		}
	} 
}
