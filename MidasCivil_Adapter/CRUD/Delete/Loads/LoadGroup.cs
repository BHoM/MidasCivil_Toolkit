using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public int DeleteLoadGroups(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids.Count() != 0)
            {
                string path = directory + "\\TextFiles\\" + "LOADGROUP" + ".txt";

                if (File.Exists(path))
                {
                    List<string> names = ids.Cast<string>().ToList();

                    List<string> loadcases = File.ReadAllLines(path).ToList();

                    List<string> loadcaseNames = new List<string>();
                    foreach (string loadcase in loadcases)
                    {
                        if (loadcase.Contains(";") || loadcase.Contains("*") || string.IsNullOrWhiteSpace(loadcase))
                        {
                            string clone = 0.ToString();
                            loadcaseNames.Add(clone);
                        }
                        else
                        {
                            string clone = loadcase.Split(',')[0].Trim();
                            loadcaseNames.Add(clone);
                        }
                    }

                    foreach (string name in names)
                    {
                        if (loadcaseNames.Contains(name))
                        {
                            int nameIndex = loadcaseNames.IndexOf(name);
                            loadcases[nameIndex] = "";
                        }
                    }

                    loadcases = loadcases.Where(x => !string.IsNullOrEmpty(x)).ToList();

                    File.Delete(path);
                    File.WriteAllLines(path, loadcases.ToArray());
                }
            }
            return success;
        }
    }
}
