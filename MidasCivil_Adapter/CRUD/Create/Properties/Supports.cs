using System.Collections.Generic;
using BH.oM.Structure.Properties.Constraint;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool CreateCollection(IEnumerable<Constraint6DOF> supports)
        {
            //Code for creating a collection of nodes in the software

            foreach (Constraint6DOF constraint6DOF in supports)
            {
                Engine.MidasCivil.Convert.ToMCSupport(constraint6DOF);
            }

            return true;
        }
    }
}