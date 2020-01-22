using System.Collections.Generic;
using BH.oM.Adapter;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        /***************************************************/
        /**** Adapter overload method                   ****/
        /***************************************************/

        //Method being called for any object already existing in the model in terms of comparers is found.
        //Default implementation first deletes these objects, then creates new ones, if not applicable for the software, override this method

        protected override bool IUpdate<T>(IEnumerable<T> objects, ActionConfig actionConfig = null)
        {
                return base.IUpdate<T>(objects);
        }

        /***************************************************/

    }
}
