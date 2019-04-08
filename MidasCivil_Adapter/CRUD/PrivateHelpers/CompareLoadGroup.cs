using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private void CompareLoadGroup(string loadGroup, string path)
        {
            string[] loadGroups = File.ReadAllLines(path);
            bool existing = false;

            for (int i = 0; i < loadGroups.Length; i++)
            {
                if (loadGroups[i].Contains(loadGroup))
                {
                    existing = true;
                    break;
                }
            }

            if (!existing)
            {
                using (StreamWriter sw = new StreamWriter(path, append:true))
                {
                    sw.WriteLine(loadGroup);
                }
            }
        }
    }
}
