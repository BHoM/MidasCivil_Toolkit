using System.IO;
using System.Collections.Generic;
using BH.oM.Structure.Constraints;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool CreateCollection(IEnumerable<BarRelease> releases)
        {
            string path = CreateSectionFile("FRAME-RLS");
            string boundaryGroupPath = CreateSectionFile("BNDR-GROUP");
            List<string> midasBarReleases = new List<string>();

            foreach (BarRelease release in releases)
            {
                string midasBoundaryGroup = Engine.MidasCivil.Convert.ToMCBoundaryGroup(release.Name);
                CompareGroup(midasBoundaryGroup, boundaryGroupPath);
                midasBarReleases.AddRange(Engine.MidasCivil.Convert.ToMCBarRelease(release));
            }

            File.AppendAllLines(path, midasBarReleases);

            return true;
        }
    }
}