using BH.oM.Structure.Elements;
using System.Collections.Generic;


namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool CreateCollection(IEnumerable<Node> nodes)
        {
            string path = CreateSectionFile("NODE");

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
                Engine.MidasCivil.Convert.ToMCNode(node, path);
            }

            return true;
        }
    }
}