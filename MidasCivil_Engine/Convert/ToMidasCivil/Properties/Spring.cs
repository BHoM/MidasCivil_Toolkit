using System.Collections.Generic;
using System.IO;
using BH.oM.Structure.Properties.Constraint;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static void ToMCSpring(this Constraint6DOF constraint6DOF, string path)
        {
            List<double> stiffness = PrivateHelpers.GetSpringStiffness(constraint6DOF);

            using (StreamWriter supportText = File.AppendText(path))
            {
                supportText.WriteLine(
                    " " + "," + "LINEAR" + "," +
                    stiffness[0] + "," + stiffness[1] + "," + stiffness[2] + "," +
                    stiffness[3] + "," + stiffness[4] + "," + stiffness[5] + "," +
                    "NO, 0, 0, 0, 0, 0, 0," +
                    constraint6DOF.Name + "," +
                    "0, 0, 0, 0, 0"
                    );
                supportText.Close();
            }
        }
    }
}