using System;
using System.Collections.Generic;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using BH.oM.Base;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static GravityLoad ToBHoMGravityLoad(List<BHoMObject> objects, string gravityLoad, string loadcase, Dictionary<string,Loadcase> loadcaseDictionary, int count)
        {
            string[] delimitted = gravityLoad.Split(',');

            Loadcase bhomLoadcase;
            loadcaseDictionary.TryGetValue(loadcase, out bhomLoadcase);

            Vector direction = new Vector { X = double.Parse(delimitted[1].Trim()),
                                            Y = double.Parse(delimitted[2].Trim()),
                                            Z = double.Parse(delimitted[3].Trim())};
            string name;

            if (string.IsNullOrWhiteSpace(delimitted[4]))
            {
                name = "GL" + count;
            }
            else
            {
                name = delimitted[4].Trim();
            }

            GravityLoad bhomGravityLoad = BH.Engine.Structure.Create.GravityLoad(bhomLoadcase, direction, objects, name);
            bhomGravityLoad.CustomData[AdapterId] = bhomGravityLoad.Name;

            return bhomGravityLoad;
        }
    }
}


