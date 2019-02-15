using System.IO;
using System.Collections.Generic;
using BH.oM.Common.Materials;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool CreateCollection(IEnumerable<Material> materials)
        {
            string path = CreateSectionFile("MATERIAL");
            List<string> midasMaterials = new List<string>();

            List<string> units = GetSectionText("UNIT",directory);

            string[] delimited = units[0].Split(',');
            string EUnit = delimited[0].Replace(" ", "") + "/" + delimited[1].Replace(" ", "") + "2";
            string tempCoeffUnit = "1/" + delimited[3].Replace(" ", "");
            string densityUnit = "1/" + delimited[3].Replace(" ", "");

            units = new List<string> { EUnit, tempCoeffUnit, densityUnit };

            foreach (Material material in materials)
            {
                midasMaterials.Add(Engine.MidasCivil.Convert.ToMCMaterial(material, units));
            }

            File.AppendAllLines(path, midasMaterials);

            return true;
        }
    }
}