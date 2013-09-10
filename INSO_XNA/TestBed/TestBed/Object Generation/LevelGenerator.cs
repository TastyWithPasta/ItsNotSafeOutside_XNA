using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using PastaGameLibrary;

namespace TestBed
{

    public static class LevelGenerator
    {
		const int InitialMutationAmount = 1;
		const float TreeRatio = 0.5f;
		const int AmountOfSectionTypes = 2;
		const int LevelLength = 10;

		const int DifficultyDepth = 3;
        const int PreferredIncrement = 100;
        const int ElementsPerGeneration = 3;
        const int EnemyMutationAmount = 8;
        const int ObjectMutationAmount = 1;
        const int InitialHistoryCount = 1;

        static int _currentGenerationNumber = 0;
		static int _currentGenerationIndex = 0;
		static float _difficultyThreshold = 0;
		static Random random = new Random(3);
		static List<Level> _history = new List<Level>();

		public static SpawnType[] GroundSpawnTypes = 
		{
			new SpawnType(typeof(BasicZombie), "BasicZombie", 100),
			new SpawnType(null, "", 100),
			
			//new SpawnType(typeof(FatZombie), "FatZombie", 50),
			//new SpawnType(typeof(PogoZombie), "PogoZombie", 40), 
			//new SpawnType(typeof(RomanZombie), "RomanZombie", 30),
			//new SpawnType(typeof(KnightZombie), "KnightZombie", 15)
		};

		//static SpawnType[] MiddleGroundSpawnTypes = 
		//{
		//    new SpawnType(null, 50),
		//    new SpawnType(typeof(Tree), 50)
		//};


        public static int CurrentGenerationLevelIndex
        {
            get { return _currentGenerationIndex + 1; }
        }
        public static int CurrentGenerationNumber
        {
            get { return _currentGenerationNumber; }
        }

        public static Level GetNextLevel()
        {
			Level mate1, mate2;
            _history.Add(Globals.CurrentLevel);

			_currentGenerationIndex = 0;
			_currentGenerationNumber++;
			_difficultyThreshold = GetPreviousDifficulty();
			SelectRandomMates(out mate1, out mate2);

            Level child = new Level(mate1, mate2);
            //_currentLevelSetup.Print(_currentGenerationIndex);
            _currentGenerationIndex++;
            
            return child;
        }

        static float GetPreviousDifficulty()
        {
            float difficulty = 0;

			//Averages the difficulties of the last (DifficultyDepth) levels.
			int depthIndex = DifficultyDepth;
			for (int i = _history.Count - 1; i > -1 || depthIndex == 0; ++i)
			{
				difficulty += _history[i].Difficulty;
				depthIndex--;
			}
			difficulty /= _history.Count;
            return difficulty;
        }
		static void SelectRandomMates(out Level mate1, out Level mate2)
        {
            int index = random.Next(0, _history.Count - 1);
			mate1 = _history[index];

			index = random.Next(0, _history.Count - 1);
			mate2 = _history[index];
        }
    }
}
