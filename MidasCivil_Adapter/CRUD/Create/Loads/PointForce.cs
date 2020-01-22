using BH.oM.Structure.Loads;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public bool CreateCollection(IEnumerable<PointLoad> PointLoads)
        {
            string loadGroupPath = CreateSectionFile("LOAD-GROUP");

            foreach (PointLoad PointLoad in PointLoads)
            {
                List<string> midasPointLoads = new List<string>();
                string PointLoadPath = CreateSectionFile(PointLoad.Loadcase.Name + "\\CONLOAD");
                string midasLoadGroup = Engine.MidasCivil.Convert.ToMCLoadGroup(PointLoad);

                List<string> assignedNodes = PointLoad.Objects.Elements.Select(x => x.CustomData[AdapterIdName].ToString()).ToList();

                foreach (string assignedNode in assignedNodes)
                {
                   midasPointLoads.Add(Engine.MidasCivil.Convert.ToMCPointLoad(PointLoad, assignedNode));
                }

                RemoveLoadEnd(PointLoadPath);
                CompareLoadGroup(midasLoadGroup,loadGroupPath);
                File.AppendAllLines(PointLoadPath, midasPointLoads);
            }

            

            return true;
        }

    }
}
