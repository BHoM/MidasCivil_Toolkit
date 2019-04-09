using System;
using System.Linq;
using System.Collections.Generic;
using BH.oM.Structure.Loads;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<LoadCombination> ReadLoadCombinations(List<string> ids = null)
        {
            List<LoadCombination> bhomLoadCombinations = new List<LoadCombination>();
            List<string> loadCombinationText = GetSectionText("LOADCOMB");

            IEnumerable<Loadcase> bhomLoadCases = ReadLoadcases();
            Dictionary<string, Loadcase> bhomLoadCaseDictionary = bhomLoadCases.ToDictionary(
                x => x.CustomData[AdapterId].ToString());

            for (int i=0; i<loadCombinationText.Count; i+=2)
            {
                LoadCombination bhomLoadCombination = Engine.MidasCivil.Convert.ToBHoMLoadCombination(loadCombinationText[i], loadCombinationText[i+1], bhomLoadCaseDictionary);
                bhomLoadCombinations.Add(bhomLoadCombination);
            }

            return bhomLoadCombinations;
        }

    }
}