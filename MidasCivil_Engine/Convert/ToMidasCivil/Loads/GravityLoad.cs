using BH.oM.Structure.Loads;
using System.Collections.Generic;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCGravityLoad(this GravityLoad gravityLoad)
        {
            string midasGravityLoad = "*SELFWEIGHT, " + gravityLoad.GravityDirection.X + "," +
                                                       gravityLoad.GravityDirection.Y + "," +
                                                       gravityLoad.GravityDirection.Z + "," +
                                                       gravityLoad.Name;
            return midasGravityLoad;
        }
    }
}