using System.Collections.Generic;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static AreaTemperatureLoad ToBHoMAreaTemperatureLoad(string temperatureLoad, List<string> associatedFEMeshes, string loadcase, Dictionary<string, Loadcase> loadcaseDictionary, Dictionary<string, FEMesh> FEMeshDictionary, int count)
        {
            string[] delimitted = temperatureLoad.Split(',');
            List<FEMesh> bhomAssociatedFEMeshes = new List<FEMesh>();

            loadcaseDictionary.TryGetValue(loadcase, out Loadcase bhomLoadcase);

            foreach (string associatedFEMesh in associatedFEMeshes)
            {
                if (FEMeshDictionary.ContainsKey(associatedFEMesh))
                {
                    FEMeshDictionary.TryGetValue(associatedFEMesh, out FEMesh bhomAssociateFEMesh);
                    bhomAssociatedFEMeshes.Add(bhomAssociateFEMesh);
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

            if (bhomAssociatedFEMeshes.Count!=0)
            {
                AreaTemperatureLoad bhomAreaUniformlyDistributedLoad = Engine.Structure.Create.AreaTemperatureLoad(bhomLoadcase, temperature, bhomAssociatedFEMeshes, LoadAxis.Global, false, name);
                bhomAreaUniformlyDistributedLoad.CustomData[AdapterId] = bhomAreaUniformlyDistributedLoad.Name;
                return bhomAreaUniformlyDistributedLoad;
            }
            else
            {
                return null;
            }
        }

    }
}

