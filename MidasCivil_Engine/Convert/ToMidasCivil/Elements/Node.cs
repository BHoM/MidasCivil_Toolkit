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
                    node.Coordinates.Origin.X + "," +
                    node.Coordinates.Origin.Y.ToString() + "," +
                    node.Coordinates.Origin.Z.ToString()
                );

            return midasNode;
        }
    }
}