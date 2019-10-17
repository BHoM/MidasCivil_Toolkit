using System.Collections.Generic;
using BH.oM.Structure.Constraints;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCSpring(this Constraint6DOF constraint6DOF, string version)
        {
            List<double> stiffness = Engine.MidasCivil.Query.SpringStiffness(constraint6DOF);

            string midasSpring = "";

            switch(version)
            {
                case "8.8.5":
                    string springFixity = Engine.MidasCivil.Query.SpringFixity(constraint6DOF);
                    midasSpring = (
                        " " + "," + "LINEAR" + "," + springFixity +
                        stiffness[0] + "," + stiffness[1] + "," + stiffness[2] + "," +
                        stiffness[3] + "," + stiffness[4] + "," + stiffness[5] + "," +
                        "NO, 0, 0, 0, 0, 0, 0," +
                        constraint6DOF.Name + "," +
                        "0, 0, 0, 0, 0"
                        );
                    break;
                default:
                    midasSpring = (
                        " " + "," + "LINEAR" + "," +
                        stiffness[0] + "," + stiffness[1] + "," + stiffness[2] + "," +
                        stiffness[3] + "," + stiffness[4] + "," + stiffness[5] + "," +
                        "NO, 0, 0, 0, 0, 0, 0," +
                        constraint6DOF.Name + "," +
                        "0, 0, 0, 0, 0"
                        );
                    break;
            }

           return midasSpring;
        }
    }
}