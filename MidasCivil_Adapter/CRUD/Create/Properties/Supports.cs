using System.Collections.Generic;
using BH.oM.Structure.Properties.Constraint;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool CreateCollection(IEnumerable<Constraint6DOF> supports)
        {
            string boundaryGroupPath = CreateSectionFile("BNDR-GROUP");
            string supportPath = CreateSectionFile("CONSTRAINT");
            string springPath = CreateSectionFile("SPRING");

            foreach (Constraint6DOF constraint6DOF in supports)
            {
                Engine.MidasCivil.Convert.ToMCBoundaryGroup(constraint6DOF, boundaryGroupPath);
            }

            foreach (Constraint6DOF constraint6DOF in supports)
            {
                if (GetStiffnessVectorModulus(constraint6DOF) > 0)
                {
                    Engine.MidasCivil.Convert.ToMCSpring(constraint6DOF, springPath);
                }
                else
                {
                    Engine.MidasCivil.Convert.ToMCSupport(constraint6DOF, supportPath);
                }
            }
            return true;
        }
    }
}