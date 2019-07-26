using BH.oM.Structure.Loads;
using System.Collections.Generic;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static List<string> ToMCGravityLoad(this GravityLoad gravityLoad)
        {
            List<string> midasGravityLoad = new List<string>(){
                "*SELFWEIGHT",
                gravityLoad.GravityDirection.X + "," + gravityLoad.GravityDirection.Y + "," + gravityLoad.GravityDirection.Z + "," + gravityLoad.Name
            };

            return midasGravityLoad;
        }
    }
}