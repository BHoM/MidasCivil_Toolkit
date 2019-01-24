using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties.Section;
using BH.oM.Common.Materials;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        /***************************************************/
        /**** Adapter overload method                   ****/
        /***************************************************/

        protected override bool Create<T>(IEnumerable<T> objects, bool replaceAll = false)
        {
            bool success = true;        //boolean returning if the creation was successfull or not

            if(objects.Count() > 0)
            {
                if(objects.First() is Node)
                {
                    success = CreateCollection(objects as IEnumerable<Node>);
                }
            }
            //UpdateViews()             //If there exists a command for updating the views is the software call it now:

            return success;

        }
    }
}
