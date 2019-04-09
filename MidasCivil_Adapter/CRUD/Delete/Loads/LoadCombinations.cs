using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public int DeleteLoadCombinations(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids != null && ids.Count() != 0)
            {
                string path = directory + "\\TextFiles\\" + "LOADCOMB" + ".txt";

                if (File.Exists(path))
                {
                    List<string> names = ids.Cast<string>().ToList();

                    List<string> loadCombinations = File.ReadAllLines(path).ToList();

                    List<string> loadCombinationNames = new List<string>();

                    for (int i=0; i<loadCombinations.Count; i++)
                    {
                        if (loadCombinations[i].Contains(";") || loadCombinations[i].Contains("*"))
                        {
                            string clone = 0.ToString();
                            loadCombinationNames.Add(clone);
                        }
                        else
                        {
                            string clone = loadCombinations[i].Split(',')[0].Split('=')[1].Replace(" ", "");
                            loadCombinationNames.Add(clone);
                            loadCombinationNames.Add(clone);
                            i++;
                        }
                    }

                    foreach (string name in names)
                    {
                        if (loadCombinationNames.Contains(name))
                        {
                            var nameIndexes = loadCombinationNames.Select((l, i) => new { l, i })
                                .Where(x => x.l == name)
                                .Select(x => x.i)
                                .ToList();

                            foreach (var index in nameIndexes)
                            {
                                loadCombinations[index] = "";
                            }
                        }
                    }

                    loadCombinations = loadCombinations.Where(x => !string.IsNullOrEmpty(x)).ToList();

                    File.Delete(path);
                    File.WriteAllLines(path, loadCombinations.ToArray());
                }
            }
            return success;
        }

    }
}
