using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public int DeleteLoadcases(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids != null)
            {
                string path = directory + "\\TextFiles\\" + "STLDCASE" + ".txt";

                if(File.Exists(path))
                {
                }
            }
            return success;
        }
    }
}
