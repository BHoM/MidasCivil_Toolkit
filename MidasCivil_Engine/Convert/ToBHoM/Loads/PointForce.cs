using System;
using System.Collections.Generic;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static PointForce ToBHoMPointForce(this string pointForce, List<string> associatedNodes, string loadcase, Dictionary<string, Loadcase> loadcaseDictionary, Dictionary<string,Node> nodeDictionary)
        {
            string[] delimitted = pointForce.Split(',');
            Node bhomAssociateNode;
            List<Node> bhomAssociatedNodes = new List<Node>();

            Loadcase bhomLoadcase;
            loadcaseDictionary.TryGetValue(loadcase, out bhomLoadcase);

            foreach (string associatedNode in associatedNodes)
            {
                nodeDictionary.TryGetValue(associatedNode, out bhomAssociateNode);
                bhomAssociatedNodes.Add(bhomAssociateNode);
            }

            Vector forceVector = new Vector
            {
                X = double.Parse(delimitted[1].Replace(" ", "")),
                Y = double.Parse(delimitted[2].Replace(" ", "")),
                Z = double.Parse(delimitted[3].Replace(" ", ""))
            };

            Vector momentVector = new Vector
            {
                X = double.Parse(delimitted[4].Replace(" ", "")),
                Y = double.Parse(delimitted[5].Replace(" ", "")),
                Z = double.Parse(delimitted[6].Replace(" ", ""))
            };

            PointForce bhomPointForce = Engine.Structure.Create.PointForce(bhomLoadcase, bhomAssociatedNodes, forceVector, momentVector, LoadAxis.Global);

            return bhomPointForce;
        }
    }
}

