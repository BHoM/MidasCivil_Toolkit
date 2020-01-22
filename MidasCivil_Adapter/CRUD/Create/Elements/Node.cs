using BH.oM.Structure.Elements;
using System.Collections.Generic;
using System.IO;


namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool CreateCollection(IEnumerable<Node> nodes)
        {
            string nodePath = CreateSectionFile("NODE");
            List<string> midasNodes = new List<string>();

            CreateGroups(nodes);

            foreach (Node node in nodes)
            {
                if(!(node.Support==null))
                {
                    if(Engine.MidasCivil.Compute.GetStiffnessVectorModulus(node.Support)>0)
                    {
                        PropertyAssignment(node.CustomData[AdapterIdName].ToString(), node.Support.Name, "SPRING");
                    }
                    else
                    {
                        PropertyAssignment(node.CustomData[AdapterIdName].ToString(), node.Support.Name, "CONSTRAINT");
                    }
                    
                }
                midasNodes.Add(Engine.MidasCivil.Convert.ToMCNode(node));
            }

            File.AppendAllLines(nodePath, midasNodes);

                return true;
        }

    }
}