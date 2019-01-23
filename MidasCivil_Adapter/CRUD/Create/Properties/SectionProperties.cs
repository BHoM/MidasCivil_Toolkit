using System;
using System.Collections.Generic;
using BH.oM.Structure.Properties.Section;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool CreateCollection(IEnumerable<ISectionProperty> sectionProperties)
        {
            //Code for creating a collection of nodes in the software

            foreach (ISectionProperty sectionProperty in sectionProperties)
            {
                Engine.MidasCivil.Convert.ToMCSectionProperty(sectionProperty);
                //Tip: if the NextId method has been implemented you can get the id to be used for the creation out as (cast into applicable type used by the software):
            }
            throw new NotImplementedException();
        }
    }
}