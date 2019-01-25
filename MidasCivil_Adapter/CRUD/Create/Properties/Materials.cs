using System;
using System.Collections.Generic;
using BH.oM.Common.Materials;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool CreateCollection(IEnumerable<Material> materials)
        {
            //Code for creating a collection of nodes in the software

            foreach (Material material in materials)
            {
                Engine.MidasCivil.Convert.ToMCMaterial(material);
                //Tip: if the NextId method has been implemented you can get the id to be used for the creation out as (cast into applicable type used by the software):
            }
            throw new NotImplementedException();
        }
    }
}