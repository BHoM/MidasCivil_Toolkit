using System;
using System.Collections.Generic;
using BH.oM.Structure.Elements;


namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool CreateCollection(IEnumerable<Bar> bars)
        {
            //Code for creating a collection of nodes in the software

            foreach (Bar bar in bars)
            {
                Engine.MidasCivil.Convert.ToMCElement(bar);
                //Tip: if the NextId method has been implemented you can get the id to be used for the creation out as (cast into applicable type used by the software):
            }
            throw new NotImplementedException();
        }
    }
}