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

            List<string> nodesText = GetSectionText("NODE");
            List<string> supportText = GetSectionText("CONSTRAINT");
            List<string> springText = GetSectionText("SPRING");

            Dictionary<string, List<int>> nodeGroups = GetGroupAssignments("GROUP", 1);

            List<Constraint6DOF> supportsList = Read6DOFConstraints();
            Dictionary<string, Constraint6DOF> supports = supportsList.ToDictionary(x => x.Name.ToString());

            Dictionary<string, List<int>> supportAssignments = GetPropertyAssignments("CONSTRAINT","Support");
            Dictionary<string, List<int>> springAssignments = GetPropertyAssignments("SPRING", "Spring");

            foreach (string node in nodesText)
            {
                Node bhomNode = Engine.MidasCivil.Convert.ToBHoMNode(node, supports, supportAssignments, springAssignments);
                int bhomID = System.Convert.ToInt32(bhomNode.CustomData[AdapterId]);
                bhomNode.Tags = CheckGroups(nodeGroups, bhomID);
                bhomNodes.Add(bhomNode);
            }

            return bhomNodes;
        }

    }
}
