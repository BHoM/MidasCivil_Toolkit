using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private HashSet<string> CheckGroups(Dictionary<string,List<int>> dictionary, int id)
        {
            HashSet<string> groups = new HashSet<string>();
            List<string> keys = dictionary.Keys.ToList();

            foreach (string key in keys)
            {
                List<int> values = new List<int>();
                dictionary.TryGetValue(key, out values);
                if (values.Contains(id))
                    groups.Add(key);
            }

            return groups;
        }
    }
}
