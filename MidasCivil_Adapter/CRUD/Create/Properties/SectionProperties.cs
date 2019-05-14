using System.IO;
using System.Collections.Generic;
using BH.oM.Structure.SectionProperties;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool CreateCollection(IEnumerable<ISectionProperty> sectionProperties)
        {
            string path = CreateSectionFile("SECTION");
            List<string> midasSectionProperties = new List<string>();

            foreach (ISectionProperty sectionProperty in sectionProperties)
            {
                midasSectionProperties.Add(Engine.MidasCivil.Convert.ToMCSectionProperty(sectionProperty));
            }

            File.AppendAllLines(path, midasSectionProperties);

            return true;
        }

    }
}