using BH.oM.Structure.Constraints;
using System.Collections.Generic;
using System.Linq;

namespace BH.Engine.MidasCivil
{
    public partial class Query
    {
        public static List<double> SpringStiffness(Constraint6DOF constraint6DOF)
        {
            List<double> stiffness = new List<double>();

            List<DOFType> fixities = new List<DOFType>() {
                constraint6DOF.TranslationX, constraint6DOF.TranslationY, constraint6DOF.TranslationZ,
                constraint6DOF.RotationX, constraint6DOF.RotationY, constraint6DOF.RotationZ
            };

            List<double> springs = new List<double>() {
                constraint6DOF.TranslationalStiffnessX, constraint6DOF.TranslationalStiffnessY, constraint6DOF.TranslationalStiffnessZ,
                constraint6DOF.RotationalStiffnessX,constraint6DOF.RotationalStiffnessY,constraint6DOF.RotationalStiffnessZ
            };

            for (int i = 0; i < fixities.Count(); i++)
            {
                DOFType freedom = fixities[i];

                if (freedom == DOFType.Fixed)
                {
                    if (i < 3)
                    {
                        stiffness.Add(100000);
                        if(!(springs[i]==100000))
                        Reflection.Compute.RecordWarning(
                            DOFType.Fixed + " used, this will overwrite the spring stiffness with 100,000 kN/m");
                    }
                    else
                    {
                        stiffness.Add(1E+016);
                        if (!(springs[i] == 1E+016))
                            Reflection.Compute.RecordWarning(
                                DOFType.Fixed + " used, this will overwrite the spring stiffness with 1E+016 kNm/rad");
                    }
                }
                else if (freedom == DOFType.Free)
                {
                    stiffness.Add(0);
                }
                else
                {
                    stiffness.Add(springs[i]);
                }
            }

            return stiffness;
        }
    }
}