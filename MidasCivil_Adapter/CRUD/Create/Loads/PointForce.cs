using BH.oM.Structure.Loads;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public bool CreateCollection(IEnumerable<PointForce> pointForces)
        {
            string loadGroupPath = CreateSectionFile("LOAD-GROUP");

            foreach (PointForce pointForce in pointForces)
            {
                List<string> midasPointForces = new List<string>();
                string pointForcePath = CreateSectionFile(pointForce.Loadcase.Name + "\\CONLOAD");
                string midasLoadGroup = Engine.MidasCivil.Convert.ToMCLoadGroup(pointForce);

                List<string> assignedNodes = pointForce.Objects.Elements.Select(x => x.CustomData[AdapterId].ToString()).ToList();

                foreach (string assignedNode in assignedNodes)
                {
                   midasPointForces.Add(Engine.MidasCivil.Convert.ToMCPointForce(pointForce, assignedNode));
                }

                RemoveLoadEnd(pointForcePath);
                CompareLoadGroup(midasLoadGroup,loadGroupPath);

                File.AppendAllLines(pointForcePath, midasPointForces);
            }

            

            return true;
        }
    }
}
