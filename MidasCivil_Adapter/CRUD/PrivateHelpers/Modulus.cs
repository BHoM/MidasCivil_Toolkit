using BH.oM.Geometry;
using System;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public double Modulus(Vector vector)
        {
            return  
                Math.Pow(vector.X, 2) +
                Math.Pow(vector.Y, 2) +
                Math.Pow(vector.Z, 2); ;
        }
    }
}
