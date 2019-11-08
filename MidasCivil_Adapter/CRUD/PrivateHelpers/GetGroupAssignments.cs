using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private HashSet<string> GetGroupAssignments(Dictionary<string,List<int>> dictionary, int id)
        {
            HashSet<string> groups = new HashSet<string>();
            List<string> keys = dictionary.Keys.ToList();

            foreach (string key in keys)
            {
                dictionary.TryGetValue(key, out List<int> values);
                if (values.Contains(id))
                    groups.Add(key);
            }

            return groups;
        }
    }
}
