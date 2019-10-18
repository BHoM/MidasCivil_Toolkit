using BH.oM.Structure.Constraints;
using System.Collections.Generic;

namespace BH.Engine.MidasCivil
{
    public partial class Query
    {
        public static string SpringFixity(Constraint6DOF constraint6DOF)
        {
            List<DOFType> freedoms = new List<DOFType>
            {
                constraint6DOF.TranslationX, constraint6DOF.TranslationY, constraint6DOF.TranslationZ,
                constraint6DOF.RotationX, constraint6DOF.RotationY, constraint6DOF.RotationZ
            };

            string support = "";

            foreach (DOFType freedom in freedoms)
            {
                if (!(Engine.MidasCivil.Query.SupportedDOFTypes(freedom)))
                {
                    Reflection.Compute.RecordWarning(
                        "Unsupported DOFType in " + constraint6DOF.Name + " assumed to be" + DOFType.Fixed);
                    support = support + "YES,";
                }

                else if (freedom == DOFType.Fixed)
                {
                    support = support + "YES,";
                }
                else 
                {
                    support = support + "NO,";
                }
            }

            return support;
        }
    }
}