using System.Collections.Generic;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static BarUniformlyDistributedLoad ToBHoMBarUniformlyDistributedLoad(string barUniformlyDistributedLoad, List<string> associatedBars, string loadcase, Dictionary<string, Loadcase> loadcaseDictionary, Dictionary<string, Bar> barDictionary, int count)
        {
            string[] delimitted = barUniformlyDistributedLoad.Split(',');
            List<Bar> bhomAssociatedBars = new List<Bar>();

            Loadcase bhomLoadcase;
            loadcaseDictionary.TryGetValue(loadcase, out bhomLoadcase);

            foreach (string associatedBar in associatedBars)
            {
                Bar bhomAssociatedBar;
                barDictionary.TryGetValue(associatedBar, out bhomAssociatedBar);
                bhomAssociatedBars.Add(bhomAssociatedBar);
            }

            string loadType = delimitted[1].Trim();
            string loadAxis = delimitted[2].Trim().Substring(0, 1);
            string direction = delimitted[2].Trim().Substring(1, 1);
            string projection = delimitted[3].Trim();

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
            double force = double.Parse(delimitted[10].Trim());

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

            if (string.IsNullOrWhiteSpace(delimitted[17]))
            {
                name = "UBL" + count;
            }
            else
            {
                name = delimitted[17].Trim();
            }

            BarUniformlyDistributedLoad bhomBarUniformlyDistributedLoad;

            if (loadType == "UNILOAD")
            {
                bhomBarUniformlyDistributedLoad = Engine.Structure.Create.BarUniformlyDistributedLoad(bhomLoadcase, bhomAssociatedBars, loadVector, null, axis, loadProjection, name);
                bhomBarUniformlyDistributedLoad.CustomData[AdapterIdName] = bhomBarUniformlyDistributedLoad.Name;
            }
            else
            {
                bhomBarUniformlyDistributedLoad = Engine.Structure.Create.BarUniformlyDistributedLoad(bhomLoadcase, bhomAssociatedBars, null, loadVector, axis, loadProjection, name);
                bhomBarUniformlyDistributedLoad.CustomData[AdapterIdName] = bhomBarUniformlyDistributedLoad.Name;
            }

            return bhomBarUniformlyDistributedLoad;
        }
    }
}

