using System.IO;
using BH.oM.Structure.Constraints;
namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCSupport(this Constraint6DOF constraint6DOF)
        {
               string midasSupport = (
                    " " + "," +
                    Engine.MidasCivil.Query.SupportString(constraint6DOF) + "," +
                    constraint6DOF.Name
                    );

            return midasSupport;
        }
    }
}