using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using PastaGameLibrary;

namespace TestBed
{
    public class Gene
    {
        //Increase <= Ratio * BaseValue <= Ratio <= Chromosome
		Chromosome m_parent;
        protected int m_length, m_maxValue;
        protected int m_value;

        public float Ratio
        {
            get { return m_value / (float)m_maxValue; }
        }
        public int Value
        {
            get { return m_value; }
            set { m_value = value; }
        }
        public int Length
        {
            get { return m_length; }

            set 
            { 
                m_length = value;
            int _maxValue = 0;
            for (int i = 0; i < m_length; ++i)
                _maxValue += (int)Math.Pow(2, i);
            }
        }
        public virtual bool[] GetSequence()
        {
           
                bool[] sequence = new bool[Length];
                int remainder = m_value;
                int difference = 0;
                for (int i = sequence.Length - 1; i > 0; --i)
                {
                    difference = remainder - (int)Math.Pow(2, i);
                    if (difference >= 0)
                    {
                        remainder = difference;
                        sequence[i] = true;
                    }
                    else
                        sequence[i] = false;
                }
                return sequence;                        
            
        }

        public virtual void Print()
        {
#if DEBUG
            bool[] seq = GetSequence();
            string debugstring = "";
            for (int i = seq.Length - 1; i > -1; --i)
                if (seq[i])
                    debugstring += "1";
                else
                    debugstring += "0";
            debugstring += "  v= " + m_value;
            debugstring += "  r= " + Ratio;
            Debug.WriteLine(debugstring);
#endif
        }
        public Gene(Gene geneToCopy)
        {
            m_value = geneToCopy.m_value;
            m_maxValue = geneToCopy.m_maxValue;
            m_length = geneToCopy.m_length;
        }
        public Gene(int length)
        {
            m_value = 0;
            m_length = length;
            m_maxValue = (int)Math.Pow(2, m_length);
        }
        public Gene() : this(8)
        {}
    }

    public class Chromosome
    {
        protected Gene[] _genes;
        protected bool[] _chromosomeData;
        protected bool _isSynchronized;

		public Gene[] Genes
        {
            get { return _genes; }
        }
        public bool[] ChromosomeData
        {
            get { return _chromosomeData; }
            set {
                if (value.Length == _chromosomeData.Length)
                    _chromosomeData = value;
                else
                    throw new Exception("Size of new array does not match.");
            }
        }

        public Chromosome(Chromosome chromosomeToCopy)
        {
            _chromosomeData = new bool[chromosomeToCopy._chromosomeData.Length];
            _isSynchronized = chromosomeToCopy._isSynchronized;
			_genes = new Gene[chromosomeToCopy.Genes.Length];

            for(int i = 0; i < _genes.Length; ++i)
                _genes[i] = new Gene(chromosomeToCopy._genes[i]);
            Cook();
        }
		public Chromosome(Gene[] genes)
        {
            _isSynchronized = true;
            _genes = genes;
            Cook();
        }
		public Chromosome(List<Gene> genes)
        {
            _isSynchronized = true;
            _genes = genes.ToArray();
            Cook();
        }

        //Used to generate children for child classes of the Chromosome class
		public Chromosome MakeChild(Chromosome mate, int mutationAmount) 
        {
			Chromosome newChromosome = CrossOver(mate);
            for (int i = 0; i < mutationAmount; ++i)
                newChromosome.Mutate();
            newChromosome.Reheat();
            return newChromosome;
        }
		protected Chromosome CrossOver(Chromosome mate)
        {
            if (mate.ChromosomeData.Length != _chromosomeData.Length)
                throw new InvalidOperationException("The two chromosomes must have the same length!");
			Chromosome newChromosome = new Chromosome(this);

            int splitIndex = Globals.Random.Next(0, _chromosomeData.Length);
            bool[] newChromosomeData = new bool[_chromosomeData.Length];

            for (int i = 0; i < newChromosomeData.Length; ++i)
                if (i < splitIndex)
                    newChromosomeData[i] = _chromosomeData[i];
                else
                    newChromosomeData[i] = mate.ChromosomeData[i];

            newChromosome.ChromosomeData = newChromosomeData;
            newChromosome.Reheat();
            return newChromosome;
        }

        public void Print()
        {
#if DEBUG
            Debug.WriteLine("Genes:");
            for(int i = 0; i < _genes.Length; ++i)
                _genes[i].Print();
            Debug.WriteLine("");
#endif
        }
        public void PrintData()
        {
#if DEBUG
            string debugString = "";
            for (int i = 0; i < _chromosomeData.Length; ++i)
                if (_chromosomeData[i])
                    debugString += "1";
                else
                    debugString += "0";
            Debug.WriteLine(debugString);
#endif
        }
        public Dictionary<string, float> GetDifferences(Chromosome chromosome)
        {
            Dictionary<string, float> results = new Dictionary<string,float>();
            for(int i = 0; i < _genes.Length; ++i)
            {
#if DEBUG
                if (Object.ReferenceEquals(_genes[i], chromosome._genes[i]))
                    Debug.WriteLine("The two genes are the same reference!");
#endif
                float sub = chromosome._genes[i].Ratio - _genes[i].Ratio;
                if (sub != 0)
                    results.Add("d - " + i + ": ", sub);
            }
            return results;
        }

        /// <summary>
        /// Builds the chromosome data from the gene pool.
        /// </summary>
        public virtual void Cook()
        {
            _isSynchronized = true;
            List<bool[]> geneSequences = new List<bool[]>();
            for(int i = 0;  i < _genes.Length; ++i)
                geneSequences.Add(_genes[i].GetSequence());
            _chromosomeData = new ArrayHelper<bool>().AppendArrays(geneSequences);
        }
        /// <summary>
        /// Builds the gene pool from the chromosome data.
        /// </summary>
        public virtual void Reheat()
        {
            if (!_isSynchronized)
                throw new InvalidOperationException("New nodes have been added. The chromosome needs to be cooked before being reheated");

            int geneLength = 0;
            int startIndex = 0;
            int endIndex = 0;
            int geneValue = 0;
            for(int i = 0; i < _genes.Length; ++i)
            {
                geneValue = 0;
                geneLength = _genes[i].Length;
                startIndex = endIndex;
                endIndex = startIndex + geneLength;
                for (int j = 0; j < geneLength; ++j)
                    if (_chromosomeData[j + startIndex])
                        geneValue += (int)Math.Pow(2, j);
                _genes[i].Value = geneValue;
            }
        }

        /// <summary>
        /// Makes a new chromosome by combining two parents at a pivot point.
        /// </summary>
        /// <param name="mate">The other parent</param>
        /// <returns>The child chromosome. </returns>

        public void Mutate()
        {
            int mutationPoint = Globals.Random.Next(0, _chromosomeData.Length);
            _chromosomeData[mutationPoint] = !_chromosomeData[mutationPoint];
        }
        public void Mutate(int mutationAmount)
        {
            for (int i = 0; i < mutationAmount; ++i)
                Mutate();
        }
    }
}
