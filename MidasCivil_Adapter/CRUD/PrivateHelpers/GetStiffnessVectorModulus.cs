using BH.oM.Geometry;
using BH.oM.Structure.Properties.Constraint;


namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public double GetStiffnessVectorModulus(Constraint6DOF support)
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



            return Modulus(translationalStiffnessVector) + Modulus(rotationalStiffnessVector);
        }
    }
}
