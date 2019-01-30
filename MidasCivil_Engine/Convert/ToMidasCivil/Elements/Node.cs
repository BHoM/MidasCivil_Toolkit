using System.IO;
using BH.oM.Structure.Elements;
namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static void ToMCNode(this Node node, string path)
        {
            using (StreamWriter nodeText = File.AppendText(path))
            {
                nodeText.WriteLine(
                    node.CustomData[AdapterId].ToString() + "," +
                    node.Coordinates.Origin.X + "," +
                    node.Coordinates.Origin.Y.ToString() + "," +
                    node.Coordinates.Origin.Z.ToString()
                );
                nodeText.Close();
            }
        }
    }
}