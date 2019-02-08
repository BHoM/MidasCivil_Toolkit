using BH.oM.Structure.Elements;
using System.Collections.Generic;
using System.IO;


namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool CreateCollection(IEnumerable<Node> nodes)
        {
            string path = CreateSectionFile("NODE");
            List<string> midasNodes = new List<string>();

            foreach (Node node in nodes)
            {
                if(!(node.Constraint==null))
                {
                    if(GetStiffnessVectorModulus(node.Constraint)>0)
                    {
                        PropertyAssignment(node.CustomData[AdapterId].ToString(), node.Constraint.Name, "SPRING");
                    }
                    else
                    {
                        PropertyAssignment(node.CustomData[AdapterId].ToString(), node.Constraint.Name, "CONSTRAINT");
                    }
                    
                }
                midasNodes.Add(Engine.MidasCivil.Convert.ToMCNode(node));
            }

            File.AppendAllLines(path, midasNodes);

                return true;
        }
    }
}