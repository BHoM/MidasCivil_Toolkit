using System.Collections.Generic;
using BH.oM.Structure.Elements;


namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool CreateCollection(IEnumerable<Node> nodes)
        {
            string path = CreateSectionText("NODE");

            foreach (Node node in nodes)
            {
                Engine.MidasCivil.Convert.ToMCNode(node, path);
            }

            return true;
        }
    }
}