using System.Collections.Generic;
using System.IO;
using BH.oM.Structure.Constraints;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool CreateCollection(IEnumerable<Constraint6DOF> supports)
        {
            string boundaryGroupPath = CreateSectionFile("BNDR-GROUP");
            string supportPath = CreateSectionFile("CONSTRAINT");
            string springPath = CreateSectionFile("SPRING");

            List<string> midasSprings = new List<string>();
            List<string> midasSupports = new List<string>();

            foreach (Constraint6DOF constraint6DOF in supports)
            {
                string midasBoundaryGroup = Engine.MidasCivil.Convert.ToMCBoundaryGroup(constraint6DOF.Name);
                CompareGroup(midasBoundaryGroup, boundaryGroupPath);
            }

            foreach (Constraint6DOF constraint6DOF in supports)
            {
                if (Engine.MidasCivil.Compute.GetStiffnessVectorModulus(constraint6DOF) > 0)
                {
                    midasSprings.Add(Engine.MidasCivil.Convert.ToMCSpring(constraint6DOF, midasCivilVersion));
                    
                }
                else
                {
                    midasSupports.Add(Engine.MidasCivil.Convert.ToMCSupport(constraint6DOF));
                }
            }

            File.AppendAllLines(supportPath, midasSupports);
            File.AppendAllLines(springPath, midasSprings);

            return true;
        }

    }
}