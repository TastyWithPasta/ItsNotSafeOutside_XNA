using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PastaGameLibrary
{
    class EquationSolver
    {
        public float[] Poly_2(float A, float B, float C)
        {
            float discriminant = B * B - 4 * A * C;
          
            if (discriminant > 0)
                return new float[] {
                    (float)((-B - Math.Sqrt(discriminant)) / A * 2),
                    (float)((-B + Math.Sqrt(discriminant)) / A * 2)
                };
            if (discriminant < 0)
                return new float[0];
            if (discriminant == 0)
                return new float[1] { 
                    -B / (2 * A)
                };
            throw new Exception("Well that should  not happen... I am enbarassed");
        }
    }
}
