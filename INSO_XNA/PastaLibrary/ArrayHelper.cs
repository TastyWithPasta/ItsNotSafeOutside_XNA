using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PastaGameLibrary
{
    public class ArrayHelper<T>
    {
        public ArrayHelper()
        { }
        public T[] AppendArrays(T[] lhs, T[] rhs)
        {
            T[] newArray = new T[lhs.Length + rhs.Length];
            for (int i = 0; i < lhs.Length; ++i)
                newArray[i] = lhs[i];
            for (int i = 0; i < rhs.Length; ++i)
                newArray[i + lhs.Length] = rhs[i];
            return newArray;
        }
        public T[] AppendArrays(List<T[]> arrays)
        {
            T[] newArray;
            int newArraySize = 0;
            for (int i = 0; i < arrays.Count; ++i)
                newArraySize += arrays[i].Length;
            newArray = new T[newArraySize];

            int currentIndex = 0;
            for (int i = 0; i < arrays.Count; ++i)
                for (int j = 0; j < arrays[i].Length; ++j)
                {
                    newArray[currentIndex] = arrays[i][j];
                    currentIndex++;
                }
            return newArray;
        }
        public T[] AppendArrays(T[][] arrays)
        {
            T[] newArray;
            int newArraySize = 0;
            for (int i = 0; i < arrays.Length; ++i)
                newArraySize += arrays[i].Length;
            newArray = new T[newArraySize];

            int currentIndex = 0;
            for(int i = 0; i < arrays.Length; ++i)
                for (int j = 0; j < arrays[i].Length; ++j)
                {
                    newArray[currentIndex] = arrays[i][j];
                    currentIndex++;
                }
            return newArray;
        }
    }
}
