using System.IO;
using BH.oM.Structure.Properties.Constraint;
namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static void ToMCSupport(this Constraint6DOF constraint6DOF, string path)
        {
            using (StreamWriter supportText = File.AppendText(path))
            {
                supportText.WriteLine(
                    " " + "," +
                    PrivateHelpers.GetSupportString(constraint6DOF) + "," +
                    constraint6DOF.Name
                    );
                supportText.Close();
            }
        }
    }
}