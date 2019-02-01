using BH.oM.Structure.Elements;
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

            return 0;
        }

        /***************************************************/
    }
}
