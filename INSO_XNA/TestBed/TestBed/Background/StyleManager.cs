using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace TestBed
{
    public static class StyleManager
    {
        public enum LevelStyle
        {
            Country,
			Clouds,
            //Town,
            //Ruins
        }

        public static int AmountOfStyles
        {
            get
            {
                int totalAmount = 0;
                foreach (FieldInfo x in typeof(LevelStyle).GetFields())
                    if (x.IsLiteral)
                        totalAmount++;
                return totalAmount;
            }
        }
    }
}
