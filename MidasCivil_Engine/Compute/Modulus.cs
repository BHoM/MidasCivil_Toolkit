using BH.oM.Geometry;
using System;

namespace BH.Engine.MidasCivil
{
    public partial class Compute
    {
        public static double Modulus(Vector vector)
        {
            return  
                Math.Sqrt(Math.Pow(vector.X, 2) +
                Math.Pow(vector.Y, 2) +
                Math.Pow(vector.Z, 2)) ;
        }
    }
}
