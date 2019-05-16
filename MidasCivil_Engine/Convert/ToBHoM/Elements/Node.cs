using BH.oM.Geometry;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.Elements;
using System.Collections.Generic;
using System.Linq;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static Node ToBHoMNode(this string node, Dictionary<string, Constraint6DOF> supports,
            Dictionary<string, List<int>> supportAssignments, Dictionary<string, List<int>> springAssignments)
        {
            List<string> delimitted = node.Split(',').ToList();

            Node bhomNode = Structure.Create.Node(
                new Point
                {
                    X = double.Parse(delimitted[1].Replace(" ", "")),
                    Y = double.Parse(delimitted[2].Replace(" ", "")),
                    Z = double.Parse(delimitted[3].Replace(" ", ""))
                }
                );

            bhomNode.CustomData[AdapterId] = delimitted[0].Replace(" ", "");
            int bhomID = System.Convert.ToInt32(bhomNode.CustomData[AdapterId]);

            string supportName = "";

            foreach (KeyValuePair<string, List<int>> supportAssignment in supportAssignments)
            {
                if (supportAssignment.Value.Contains(bhomID))
                {
                    supportName = supportAssignment.Key;
                    break;
                }
            }

            foreach (KeyValuePair<string, List<int>> springAssignment in springAssignments)
            {
                if (springAssignment.Value.Contains(bhomID))
                {
                    supportName = springAssignment.Key;
                    break;
                }
            }

            Constraint6DOF nodeConstraint = null;
            if (!(supportName == ""))
            {
                supports.TryGetValue(supportName, out nodeConstraint);
            }


            bhomNode.Constraint = nodeConstraint;


            return bhomNode;
        }

        public static Node PointToNode (Point point)
        {
            Node node = new Node { Position = point };
            return node;
        }
    }
}
