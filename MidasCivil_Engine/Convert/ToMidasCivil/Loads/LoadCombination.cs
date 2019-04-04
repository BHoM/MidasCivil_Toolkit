using BH.oM.Structure.Loads;
using System.Collections.Generic;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static List<string> ToMCLoadCombination(this LoadCombination loadCombination)
        {
            List<string> midasLoadCombination = new List<string>();

            string line1 = "NAME=" + loadCombination.Name + ", GEN, ACTIVE, 0, 0, , 0, 0";
            midasLoadCombination.Add(line1);

            string line2 = "";

            for (int i=0; i< loadCombination.LoadCases.Count; i++)
            {
                line2 = line2 + "ST, " + loadCombination.LoadCases[i].Item2.Name + ",";
                line2 = line2 + loadCombination.LoadCases[i].Item1.ToString() + ",";
            }

            midasLoadCombination.Add(line2);

            return midasLoadCombination;
        }
    }
}