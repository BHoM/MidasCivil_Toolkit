using System.IO;
using System.Collections.Generic;
using BH.oM.Structure.Elements;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool CreateCollection(IEnumerable<RigidLink> links)
        {
            string path = CreateSectionFile("RIGIDLINK");
            string boundaryGroupPath = CreateSectionFile("BNDR-GROUP");
            List<string> midasRigidLinks = new List<string>();

            foreach (RigidLink link in links)
            {
                string midasBoundaryGroup = Engine.MidasCivil.Convert.ToMCBoundaryGroup(link.Name);
                CompareGroup(midasBoundaryGroup, boundaryGroupPath);
                midasRigidLinks.Add(Engine.MidasCivil.Convert.ToMCRigidLink(link));
            }

            File.AppendAllLines(path, midasRigidLinks);

            return true;
        }
    }
}