using BH.oM.Structure.Properties.Constraint;
using System.Collections.Generic;

namespace BH.Engine.MidasCivil
{
    public partial class Query
    {
        public static string SupportString(Constraint6DOF constraint6DOF)
        {
            List<DOFType> freedoms = new List<DOFType>
            {
                constraint6DOF.TranslationX, constraint6DOF.TranslationY, constraint6DOF.TranslationZ,
                constraint6DOF.RotationX, constraint6DOF.RotationY, constraint6DOF.RotationZ
            };

            string support = "";

            int dfd = (int)constraint6DOF.RotationX;

            foreach(DOFType freedom in freedoms)
            {
                if(Engine.MidasCivil.Query.SupportedDOFTypes(freedom))
                {
                    Reflection.Compute.RecordWarning(
                        "Unsupported DOFType in " + constraint6DOF.Name + " assumed to be" + DOFType.Free);
                    support = support + "0";
                }
                else if(freedom == DOFType.Free)
                {
                    support = support + "0";
                }
                else if (freedom == DOFType.Fixed)
                {
                    support = support + "1";
                }
            }
            return support;

        }
    }
}