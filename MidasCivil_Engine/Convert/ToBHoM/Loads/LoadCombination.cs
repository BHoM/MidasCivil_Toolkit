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
            Loadcase bhomLoadcase;

            for (int i=1; i<delimittedLine2.Count; i+=3)
            {
                    bhomLoadCaseDictionary.TryGetValue(delimittedLine2[i].Replace(" ", ""), out bhomLoadcase);
                    associatedLoadcases.Add(bhomLoadcase);
                    loadFactors.Add(double.Parse(delimittedLine2[i+1].Replace(" ", "")));
            }

            string name = delimittedLine1[0].Split('=')[1].Replace(" ", "");
            int number = 0;

            LoadCombination bhomLoadCombination = BH.Engine.Structure.Create.LoadCombination(name, number, associatedLoadcases, loadFactors);
            bhomLoadCombination.CustomData[AdapterId] = name;

            return bhomLoadCombination;
        }
    }
}

