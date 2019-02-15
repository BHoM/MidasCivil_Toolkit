using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;
using System.IO;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<PointForce> ReadPointForces(List<string> ids = null)
        {
            List<ILoad> bhomPointForces = new List<ILoad>();

            string[] loadcases = Directory.GetDirectories(directory);
            foreach (string loadcase in loadcases)
            {
                List<string> pointForceText = GetSectionText("CONLOAD",loadcase);
                

                if (pointForceText.Count!=0)
                {
                    List<Node> bhomNodes = ReadNodes();
                    List<int> nodeAssignments = new List<int>();
                    Dictionary<string, Node> nodeDictionary = bhomNodes.ToDictionary(
                        x => x.CustomData[AdapterId].ToString());

                    pointForceText.ForEach(x => nodeAssignments.Add(int.Parse(x.Split(',')[0])));

                    PointForce bhomPointForce = Engine.MidasCivil.Convert.ToPointForce(
                            lusasConcentratedLoad, nodeAssignments, nodeDictionary
                            );
                }
            }

            

            return bhomPointForces;
        }
    }
}