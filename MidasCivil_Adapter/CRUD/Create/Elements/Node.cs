using System;
using System.Collections.Generic;
using BH.oM.Structure.Elements;


namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool CreateCollection(IEnumerable<Node> nodes)
        {
            //Code for creating a collection of nodes in the software

            foreach (Node node in nodes)
            {
                Engine.MidasCivil.Convert.ToMCNode(node);
            }
            throw new NotImplementedException();
        }
    }
}