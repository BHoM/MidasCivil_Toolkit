using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private int GetMaxElementID()
        {
            int maxID = 0;

            List<int> allID = new List<int>();
            List<string> elements = GetSectionText("ELEMENT");
            List<List<string>> delimitted = new List<List<string>>();
            elements.ForEach(x =>  delimitted.Add(x.Split(',').ToList()));

            if (elements!=null)
            {
                delimitted.ForEach(x => allID.Add(Convert.ToInt32(x[0])));
                allID.Sort();
                allID.Reverse();
                maxID = allID[0];
            }
            else
            {
                maxID = 1;
            }

            return maxID;
        }
    }
}
