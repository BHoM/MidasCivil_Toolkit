using System;
using System.Collections.Generic;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static PointForce ToBHoMPointForce(this string pointForce, List<string> associatedNodes, string loadcase, Dictionary<string, Loadcase> loadcaseDictionary, Dictionary<string,Node> nodeDictionary, int count)
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
                X = double.Parse(delimitted[0].Replace(" ", "")),
                Y = double.Parse(delimitted[1].Replace(" ", "")),
                Z = double.Parse(delimitted[2].Replace(" ", ""))
            };

            Vector momentVector = new Vector
            {
                X = double.Parse(delimitted[3].Replace(" ", "")),
                Y = double.Parse(delimitted[4].Replace(" ", "")),
                Z = double.Parse(delimitted[5].Replace(" ", ""))
            };

            string name;

            if(string.IsNullOrWhiteSpace(delimitted[6]))
            {
                name = "PF" + count;
            }
            else
            {
                name = delimitted[6].Replace(" ", "");
            }

            IEnumerable<Node> test = bhomAssociatedNodes;

            PointForce bhomPointForce = Engine.Structure.Create.PointForce(bhomLoadcase, test, forceVector, momentVector, LoadAxis.Global, name);

            return bhomPointForce;
        }
    }
}

