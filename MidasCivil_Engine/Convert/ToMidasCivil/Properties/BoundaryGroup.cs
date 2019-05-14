using System.IO;
using BH.oM.Structure.Constraints;
namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCBoundaryGroup(string name)
        {
                //Check what AUTOTYPE is, seems auto set to 0

                string midasBoundaryGroup = (
                    name + "," +
                    "0"
                );

            return midasBoundaryGroup;
        }
    }
}