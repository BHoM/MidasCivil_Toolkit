using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<Node> ReadNodes(List<string> ids = null)
        {
            List<Node> bhomNodes = new List<Node>();

            List<string> nodesText = GetSectionText(midasText, "*NODE");

            foreach (string node in nodesText)
            {
                Node bhomNode = Engine.MidasCivil.Convert.ToBHoMNode(node);
                bhomNodes.Add(bhomNode);
            }

            return bhomNodes;
        }
    }
}
