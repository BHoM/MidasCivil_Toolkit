using System.IO;
using System.Collections.Generic;
using BH.oM.Structure.Elements;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool CreateCollection(IEnumerable<Bar> bars)
        {
            string path = CreateSectionFile("ELEMENT");
            List<string> midasElements = new List<string>();

            CreateGroups(bars);

            foreach (Bar bar in bars)
            {
                midasElements.Add(Engine.MidasCivil.Convert.ToMCElement(bar));
            }

            File.AppendAllLines(path, midasElements);

            return true;
        }

        private bool CreateCollection(IEnumerable<FEMesh> meshes)
        {
            string path = CreateSectionFile("ELEMENT");
            List<string> midasElements = new List<string>();

            CreateGroups(meshes);

            foreach (FEMesh mesh in meshes)
            {
                midasElements.Add(Engine.MidasCivil.Convert.ToMCElement(mesh));
            }

            File.AppendAllLines(path, midasElements);

            return true;
        }

    }
}