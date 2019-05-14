using System.IO;
using System.Collections.Generic;
using BH.oM.Physical.Materials;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool CreateCollection(IEnumerable<Material> materials)
        {
            string path = CreateSectionFile("MATERIAL");
            List<string> midasMaterials = new List<string>();

            List<string> units = new List<string>();

            if(File.Exists(directory+"\\TextFiles\\"+"UNIT.txt"))
            {
                List<string> unitSection = GetSectionText("UNIT");

                string[] delimited = unitSection[0].Split(',');
                string EUnit = delimited[0].Replace(" ", "") + "/" + delimited[1].Replace(" ", "") + "2";
                string tempCoeffUnit = "1/" + delimited[3].Replace(" ", "");
                string densityUnit = "1/" + delimited[3].Replace(" ", "");
                units.Add(EUnit);
                units.Add(tempCoeffUnit);
                units.Add(densityUnit);
            }

            foreach (Material material in materials)
            {
                midasMaterials.Add(Engine.MidasCivil.Convert.ToMCMaterial(material, units));
            }

            File.AppendAllLines(path, midasMaterials);

            return true;
        }

    }
}