using BH.oM.Structure.Elements;
namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCNode(this Node node)
        {
            string midasNode = 
                (
                    node.CustomData[AdapterId].ToString() + "," +
                    node.Position.X.ToString() + "," +
                    node.Position.Y.ToString() + "," +
                    node.Position.Z.ToString()
                );

            return midasNode;
        }
    }
}