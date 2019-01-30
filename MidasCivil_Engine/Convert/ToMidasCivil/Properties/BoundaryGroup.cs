using System.IO;
using BH.oM.Structure.Properties.Constraint;
namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static void ToMCBoundaryGroup(this Constraint6DOF support, string path)
        {
            using (StreamWriter boundaryGroupText = File.AppendText(path))
            {
                //Check what AUTOTYPE is, seems auto set to 0

                boundaryGroupText.WriteLine(
                    support.Name + "," +
                    "0"
                );
                boundaryGroupText.Close();
            }
        }
    }
}