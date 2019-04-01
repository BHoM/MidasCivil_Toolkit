using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private int GetMaxID(string section)
        {
            int maxID = 0;
            
            List<int> allID = new List<int>();
            List<string> text = GetSectionText(section);
            List<List<string>> delimitted = new List<List<string>>();
            text.ForEach(x =>  delimitted.Add(x.Split(',').ToList()));

            if (text != null)
            {
                foreach (List<string> line in delimitted)
                {
                    if (Int32.TryParse(line[0],out maxID))
                    {
                        allID.Add(maxID);
                    }
                }
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
