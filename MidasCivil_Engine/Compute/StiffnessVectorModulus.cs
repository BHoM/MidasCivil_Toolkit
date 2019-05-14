using BH.oM.Geometry;
using BH.oM.Structure.Constraints;

namespace BH.Engine.MidasCivil
{
    public partial class Compute
    {
        public static double GetStiffnessVectorModulus(Constraint6DOF support)
        {
            Vector translationalStiffnessVector = new Vector()
            {
                X = support.TranslationalStiffnessX,
                Y = support.TranslationalStiffnessY,
                Z = support.TranslationalStiffnessZ
            };

            Vector rotationalStiffnessVector = new Vector()
            {
                X = support.RotationalStiffnessX,
                Y = support.RotationalStiffnessY,
                Z = support.RotationalStiffnessZ
            };

            return Engine.MidasCivil.Compute.Modulus(translationalStiffnessVector) + 
                Engine.MidasCivil.Compute.Modulus(rotationalStiffnessVector);
        }
    }
}
