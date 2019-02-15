using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties.Constraint;
using System.Collections.Generic;
using System.Linq;


namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<Node> ReadNodes(List<string> ids = null)
        {
            List<Node> bhomNodes = new List<Node>();

            List<string> nodesText = GetSectionText("NODE",directory);
            List<string> supportText = GetSectionText("CONSTRAINT",directory);
            List<string> springText = GetSectionText("SPRING",directory);

            List<Constraint6DOF> supportsList = Read6DOFConstraints();
            Dictionary<string, Constraint6DOF> supports = supportsList.ToDictionary(x => x.Name.ToString());

            Dictionary<string, List<int>> supportAssignments = GetPropertyAssignments("CONSTRAINT","Support");
            Dictionary<string, List<int>> springAssignments = GetPropertyAssignments("SPRING", "Spring");

            foreach (string node in nodesText)
            {
                Node bhomNode = Engine.MidasCivil.Convert.ToBHoMNode(node, supports, supportAssignments, springAssignments);
                bhomNodes.Add(bhomNode);
            }

            return bhomNodes;
        }
    }
}
