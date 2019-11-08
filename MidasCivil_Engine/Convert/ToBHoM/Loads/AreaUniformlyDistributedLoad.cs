using System.Collections.Generic;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static AreaUniformlyDistributedLoad ToBHoMAreaUniformlyDistributedLoad(string areaUniformlyDistributedLoad, List<string> associatedFEMeshes, string loadcase, Dictionary<string, Loadcase> loadcaseDictionary, Dictionary<string, FEMesh> FEMeshDictionary, int count)
        {
            string[] delimitted = areaUniformlyDistributedLoad.Split(',');
            List<FEMesh> bhomAssociatedFEMeshes = new List<FEMesh>();

            loadcaseDictionary.TryGetValue(loadcase, out Loadcase bhomLoadcase);

            foreach (string associatedFEMesh in associatedFEMeshes)
            {
                FEMeshDictionary.TryGetValue(associatedFEMesh, out FEMesh bhomAssociateFEMesh);
                bhomAssociatedFEMeshes.Add(bhomAssociateFEMesh);
            }

            string loadAxis = delimitted[3].Trim().Substring(0, 1);
            string direction = delimitted[3].Trim().Substring(1, 1);
            string projection = delimitted[7].Trim();

            LoadAxis axis = LoadAxis.Global;
            bool loadProjection = false;

            if (loadAxis == "L")
            {
                axis = LoadAxis.Local;
            }

            if (projection == "YES")
            {
                loadProjection = true;
            }

            double XLoad = 0;
            double YLoad = 0;
            double ZLoad = 0;
            double force = double.Parse(delimitted[8].Trim());

            switch (direction)
            {
                case "X":
                    XLoad = force;
                    break;
                case "Y":
                    YLoad = force;
                    break;
                case "Z":
                    ZLoad = force;
                    break;
            }

            Vector loadVector = new Vector
            {
                X = XLoad,
                Y = YLoad,
                Z = ZLoad
            };

            string name;

            if (string.IsNullOrWhiteSpace(delimitted[13]))
            {
                name = "AL" + count;
            }
            else
            {
                name = delimitted[13].Trim();
            }

            AreaUniformlyDistributedLoad bhomAreaUniformlyDistributedLoad = Engine.Structure.Create.AreaUniformlyDistributedLoad(bhomLoadcase, loadVector, bhomAssociatedFEMeshes, axis, loadProjection, name);
            bhomAreaUniformlyDistributedLoad.CustomData[AdapterId] = bhomAreaUniformlyDistributedLoad.Name;

            return bhomAreaUniformlyDistributedLoad;
        }

    }
}

