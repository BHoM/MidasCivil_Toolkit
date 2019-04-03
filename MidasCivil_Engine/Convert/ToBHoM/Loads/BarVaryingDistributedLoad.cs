using System;
using System.Linq;
using System.Collections.Generic;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static BarVaryingDistributedLoad ToBHoMBarVaryingDistributedLoad(string barVaryingDistributedLoad, List<string> associatedBars, string loadcase, Dictionary<string, Loadcase> loadcaseDictionary, Dictionary<string, Bar> barDictionary, int count)
        {
            string[] delimitted = barVaryingDistributedLoad.Split(',');
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

            double XStartLoad = 0;
            double YStartLoad = 0;
            double ZStartLoad = 0;
            double XEndLoad = 0;
            double YEndLoad = 0;
            double ZEndLoad = 0;
            double startLoad = double.Parse(delimitted[10].Replace(" ", ""));
            double endLoad = double.Parse(delimitted[12].Replace(" ", ""));

            switch (direction)
            {
                case "X":
                    XStartLoad = startLoad;
                    XEndLoad = endLoad;
                    break;
                case "Y":
                    YStartLoad = startLoad;
                    YEndLoad = endLoad;
                    break;
                case "Z":
                    ZStartLoad = startLoad;
                    ZEndLoad = endLoad;
                    break;
            }

            Vector startLoadVector = new Vector
            {
                X = XStartLoad,
                Y = YStartLoad,
                Z = ZStartLoad
            };

            Vector endLoadVector = new Vector
            {
                X = XEndLoad,
                Y = YEndLoad,
                Z = ZEndLoad
            };

            double distA = double.Parse(delimitted[9].Replace(" ", ""));
            double distB = double.Parse(delimitted[11].Replace(" ", ""));

            if (double.Parse(delimitted[13].Replace(" ", ""))!=0 || double.Parse(delimitted[15].Replace(" ", ""))!= 0)
            {
                Engine.Reflection.Compute.RecordWarning("BHoM Bar Varying Distributed Load does not support non trapezoidal varying loads");
            }

            string name;

            if (string.IsNullOrWhiteSpace(delimitted[17]))
            {
                name = "VBL" + count;
            }
            else
            {
                name = delimitted[17].Replace(" ", "");
            }

            BarVaryingDistributedLoad bhomBarVaryingDistributedLoad;

            if (loadType == "UNILOAD")
            {
                bhomBarVaryingDistributedLoad = Engine.Structure.Create.BarVaryingDistributedLoad(bhomLoadcase, bhomAssociatedBars, distA, startLoadVector, null, distB, endLoadVector, null, axis, loadProjection, name);
                bhomBarVaryingDistributedLoad.CustomData[AdapterId] = bhomBarVaryingDistributedLoad.Name;
            }
            else
            {
                bhomBarVaryingDistributedLoad = Engine.Structure.Create.BarVaryingDistributedLoad(bhomLoadcase, bhomAssociatedBars, distA, null, startLoadVector, distB, null, endLoadVector, axis, loadProjection, name);
                bhomBarVaryingDistributedLoad.CustomData[AdapterId] = bhomBarVaryingDistributedLoad.Name;
            }

            return bhomBarVaryingDistributedLoad;
        }
    }
}

