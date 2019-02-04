using System.IO;
using System;
using BH.oM.Structure.Elements;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static void ToMCElement(this Bar bar, string path)
        {
            using (StreamWriter elementText = File.AppendText(path))
            {
                elementText.WriteLine(bar.CustomData[AdapterId].ToString() + ", BEAM, 1, 1, " +
                                      bar.StartNode.CustomData[AdapterId].ToString() + ", " +
                                      bar.EndNode.CustomData[AdapterId].ToString() + ", 0, 0");
                elementText.Close();
            }
           
        }

        public static string ToMCElement(this FEMesh feMesh)
        {
            throw new NotImplementedException();
        }
    }
}