using System.IO;
using BH.oM.Structure.Properties.Constraint;
namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCBoundaryGroup(this Constraint6DOF support)
        {
                //Check what AUTOTYPE is, seems auto set to 0

                string midasBoundaryGroup = (
                    support.Name + "," +
                    "0"
                );

            return midasBoundaryGroup;
        }
    }
}