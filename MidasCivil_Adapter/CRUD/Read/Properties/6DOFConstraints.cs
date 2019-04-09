using System.Collections.Generic;
using BH.oM.Structure.Properties.Constraint;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<Constraint6DOF> Read6DOFConstraints(List<string> ids = null)
        {
            List<Constraint6DOF> bhom6DOFConstraints = new List<Constraint6DOF>();

            List<string> supports = GetSectionText("CONSTRAINT");

            for (int i = 0; i < supports.Count; i++)
            {
                Constraint6DOF bhomConstraint6DOF = Engine.MidasCivil.Convert.ToBHoMConstraint6DOF(
                    supports[i]);

                bhom6DOFConstraints.Add(bhomConstraint6DOF);
            }

            List<string> springs = GetSectionText("SPRING");

            for (int i = 0; i < springs.Count; i++)
            {
                Constraint6DOF bhomConstraint6DOF = Engine.MidasCivil.Convert.ToBHoMConstraint6DOF(
                    springs[i]);
                if(!(bhomConstraint6DOF==null))
                {
                    bhom6DOFConstraints.Add(bhomConstraint6DOF);
                }
            }
            return bhom6DOFConstraints;
        }
    }
}
