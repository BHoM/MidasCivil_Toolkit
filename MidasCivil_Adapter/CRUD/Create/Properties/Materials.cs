using System.IO;
using System.Collections.Generic;
using BH.oM.Structure.MaterialFragments;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool CreateCollection(IEnumerable<IMaterialFragment> materials)
        {
            string path = CreateSectionFile("MATERIAL");
            List<string> midasMaterials = new List<string>();

            foreach (IMaterialFragment material in materials)
            {
                midasMaterials.Add(Engine.MidasCivil.Convert.ToMCMaterial(material));
            }

            File.AppendAllLines(path, midasMaterials);

            return true;
        }

    }
}