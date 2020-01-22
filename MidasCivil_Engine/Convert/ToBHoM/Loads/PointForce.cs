using System;
using System.Collections.Generic;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static PointLoad ToBHoMPointLoad(this string PointLoad, List<string> associatedNodes, string loadcase, Dictionary<string, Loadcase> loadcaseDictionary, Dictionary<string,Node> nodeDictionary, int count)
        {
            string[] delimitted = PointLoad.Split(',');
            List<Node> bhomAssociatedNodes = new List<Node>();

            Loadcase bhomLoadcase;
            loadcaseDictionary.TryGetValue(loadcase, out bhomLoadcase);

            foreach (string associatedNode in associatedNodes)
            {
                Node bhomAssociatedNode;
                nodeDictionary.TryGetValue(associatedNode, out bhomAssociatedNode);
                bhomAssociatedNodes.Add(bhomAssociatedNode);
            }

            Vector forceVector = new Vector
            {
                X = double.Parse(delimitted[0].Trim()),
                Y = double.Parse(delimitted[1].Trim()),
                Z = double.Parse(delimitted[2].Trim())
            };

            Vector momentVector = new Vector
            {
                X = double.Parse(delimitted[3].Trim()),
                Y = double.Parse(delimitted[4].Trim()),
                Z = double.Parse(delimitted[5].Trim())
            };

            string name;

            if(string.IsNullOrWhiteSpace(delimitted[6]))
            {
                name = "PF" + count;
            }
            else
            {
                name = delimitted[6].Trim();
            } 

            IEnumerable<Node> nodes = bhomAssociatedNodes;

            PointLoad bhomPointLoad = Engine.Structure.Create.PointLoad(bhomLoadcase, nodes, forceVector, momentVector, LoadAxis.Global, name);
            bhomPointLoad.CustomData[AdapterIdName] = bhomPointLoad.Name;

            return bhomPointLoad;
        }
    }
}

