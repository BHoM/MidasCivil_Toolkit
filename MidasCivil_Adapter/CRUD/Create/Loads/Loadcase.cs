using BH.oM.Structure.Loads;
using System.Collections.Generic;
using System.IO;


namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool CreateCollection(IEnumerable<Loadcase> loadcases)
        {
            string path = CreateSectionFile("STLDCASE");
            List<string> midasLoadCases = new List<string>();

            foreach (Loadcase loadcase in loadcases)
            {
                loadcase.CustomData[AdapterIdName] = loadcase.Name;
                Directory.CreateDirectory(directory + "\\TextFiles\\" + loadcase.Name);
                midasLoadCases.Add(Engine.MidasCivil.Convert.ToMCLoadCase(loadcase));
            }

            File.AppendAllLines(path, midasLoadCases);

            return true;
        }

    }
}