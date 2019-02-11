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
            int count = 1;

            foreach (Loadcase loadcase in loadcases)
            {
                midasLoadCases.Add(Engine.MidasCivil.Convert.ToMCLoadCase(loadcase,count));
                count++;
            }

            File.AppendAllLines(path, midasLoadCases);

            return true;
        }
    }
}