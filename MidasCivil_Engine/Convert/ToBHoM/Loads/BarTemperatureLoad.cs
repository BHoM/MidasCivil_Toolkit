using System.Collections.Generic;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static BarTemperatureLoad ToBHoMBarTemperatureLoad(string temperatureLoad, List<string> associatedFEMeshes, string loadcase, Dictionary<string, Loadcase> loadcaseDictionary, Dictionary<string, Bar> barDictionary, int count)
        {
            string[] delimitted = temperatureLoad.Split(',');
            List<Bar> bhomAssociatedBars = new List<Bar>();

            loadcaseDictionary.TryGetValue(loadcase, out Loadcase bhomLoadcase);

            foreach (string associatedFEMesh in associatedFEMeshes)
            {
                if (barDictionary.ContainsKey(associatedFEMesh))
                {
                    barDictionary.TryGetValue(associatedFEMesh, out Bar bhomAssociateBar);
                    bhomAssociatedBars.Add(bhomAssociateBar);
                }
            }

            double temperature = double.Parse(delimitted[0].Trim());

            string name;

            if (string.IsNullOrWhiteSpace(delimitted[1]))
            {
                name = "ATL" + count;
            }
            else
            {
                name = delimitted[1].Trim();
            }

            if (bhomAssociatedBars.Count != 0)
            {
                BarTemperatureLoad bhombarUniformlyDistributedLoad = Engine.Structure.Create.BarTemperatureLoad(bhomLoadcase, temperature, bhomAssociatedBars, LoadAxis.Global, false, name);
                bhombarUniformlyDistributedLoad.CustomData[AdapterId] = bhombarUniformlyDistributedLoad.Name;
                return bhombarUniformlyDistributedLoad;
            }
            else
            {
                return null;
            }
        }
    }
}

