using System.Collections.Generic;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static BarPointLoad ToBHoMBarPointLoad(string barPointLoad, List<string> associatedBars, string loadcase, Dictionary<string, Loadcase> loadcaseDictionary, Dictionary<string, Bar> barDictionary, int count)
        {
            string[] delimitted = barPointLoad.Split(',');
            Bar bhomAssociateBar;
            List<Bar> bhomAssociatedBars = new List<Bar>();

            Loadcase bhomLoadcase;
            loadcaseDictionary.TryGetValue(loadcase, out bhomLoadcase);

            foreach (string associatedBar in associatedBars)
            {
                barDictionary.TryGetValue(associatedBar, out bhomAssociateBar);
                bhomAssociatedBars.Add(bhomAssociateBar);
            }

            string loadType = delimitted[1].Replace(" ", "");
            string loadAxis = delimitted[2].Replace(" ", "").Substring(0, 1);
            string direction = delimitted[2].Replace(" ", "").Substring(1, 1);
            string projection = delimitted[3].Replace(" ", "");

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
            double load = double.Parse(delimitted[10].Replace(" ", ""));

            switch (direction)
            {
                case "X":
                    XLoad = load;
                    break;
                case "Y":
                    YLoad = load;
                    break;
                case "Z":
                    ZLoad = load;
                    break;
            }

            Vector loadVector = new Vector
            {
                X = XLoad,
                Y = YLoad,
                Z = ZLoad
            };

            double distA = double.Parse(delimitted[9].Replace(" ", ""));

            string name;

            if (string.IsNullOrWhiteSpace(delimitted[17]))
            {
                name = "BPL" + count;
            }
            else
            {
                name = delimitted[17].Replace(" ", "");
            }

            BarPointLoad bhomBarPointLoad;

            if (loadType == "CONLOAD")
            {
                bhomBarPointLoad = Engine.Structure.Create.BarPointLoad(bhomLoadcase,distA,bhomAssociatedBars, loadVector,null,axis,name);
                bhomBarPointLoad.CustomData[AdapterId] = bhomBarPointLoad.Name;
            }
            else
            {
                bhomBarPointLoad = Engine.Structure.Create.BarPointLoad(bhomLoadcase, distA, bhomAssociatedBars, null, loadVector, axis, name);
                bhomBarPointLoad.CustomData[AdapterId] = bhomBarPointLoad.Name;
            }

            return bhomBarPointLoad;
        }
    }
}

