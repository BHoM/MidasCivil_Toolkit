using BH.oM.Structure.Loads;
using System.Collections.Generic;
using System.IO;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public bool CreateCollection(IEnumerable<LoadCombination> loadCombinations)
        {
            string path = CreateSectionFile("LOADCOMB");
            List<string> midasLoadCombinations = new List<string>();

            foreach (LoadCombination loadCombination in loadCombinations)
            {
                loadCombination.CustomData[AdapterId] = loadCombination.Name;
                midasLoadCombinations.AddRange(Engine.MidasCivil.Convert.ToMCLoadCombination(loadCombination));
            }

            File.AppendAllLines(path, midasLoadCombinations);

            return true;
        }
    }
}
