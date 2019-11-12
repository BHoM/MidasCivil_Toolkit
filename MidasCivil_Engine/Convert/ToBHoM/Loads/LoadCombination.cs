using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Loads;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static LoadCombination ToBHoMLoadCombination(string loadCombination1, string loadCombination2, Dictionary<string, Loadcase> bhomLoadCaseDictionary)
        {
            List<string> delimittedLine1 = loadCombination1.Split(',').ToList();
            List<string> delimittedLine2 = loadCombination2.Split(',').ToList();

            List<Loadcase> associatedLoadcases = new List<Loadcase>();
            List<double> loadFactors = new List<double>();

            for (int i=1; i<delimittedLine2.Count; i+=3)
            {
                Loadcase bhomLoadcase;
                bhomLoadCaseDictionary.TryGetValue(delimittedLine2[i].Trim(), out bhomLoadcase);
                associatedLoadcases.Add(bhomLoadcase);
                loadFactors.Add(double.Parse(delimittedLine2[i+1].Trim()));
            }

            string name = delimittedLine1[0].Split('=')[1].Trim();
            int number = 0;

            LoadCombination bhomLoadCombination = BH.Engine.Structure.Create.LoadCombination(name, number, associatedLoadcases, loadFactors);
            bhomLoadCombination.CustomData[AdapterId] = name;

            return bhomLoadCombination;
        }
    }
}

