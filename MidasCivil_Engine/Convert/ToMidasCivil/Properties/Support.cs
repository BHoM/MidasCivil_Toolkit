using System.IO;
using BH.oM.Structure.Properties.Constraint;
namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCSupport(this Constraint6DOF constraint6DOF)
        {
               string midasSupport = (
                    " " + "," +
                    PrivateHelpers.GetSupportString(constraint6DOF) + "," +
                    constraint6DOF.Name
                    );

            return midasSupport;
        }
    }
}