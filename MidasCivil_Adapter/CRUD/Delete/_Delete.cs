using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using System;
using System.Collections.Generic;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        /***************************************************/
        /**** Adapter overload method                   ****/
        /***************************************************/

        protected override int Delete(Type type, IEnumerable<object> ids)
        {
            int success = 0;

            if (type == typeof(Node))
                success = DeleteNodes(ids);
            if (type == typeof(Bar))
                success = DeleteElements(ids);
            if (type == typeof(Loadcase))
                success = DeleteLoadcases(ids);

            return 0;
        }

        /***************************************************/
    }
}
