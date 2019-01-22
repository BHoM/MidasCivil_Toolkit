using BH.oM.Geometry;
using BH.oM.Structure.Elements;
using System.Collections.Generic;
using System.Linq;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static Node ToBHoMNode(this string node)
        {
            List<string> delimitted = node.Split(',').ToList();

            Node bhomNode = Structure.Create.Node(
                new Point {
                    X = double.Parse(delimitted[1]),
                    Y = double.Parse(delimitted[2]),
                    Z = double.Parse(delimitted[3])}
                );

            bhomNode.CustomData["Midas_id"] = delimitted[0];

            return bhomNode;
        }
    }
}
