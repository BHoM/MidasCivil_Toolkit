using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public int DeleteBarTemperatureLoads(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids != null && ids.Count()!=0)
            {
                string[] loadcaseNames = Directory.GetDirectories(directory+ "\\TextFiles\\");

                foreach (string loadcaseName in loadcaseNames)
                {
                    string path = loadcaseName + "\\ELTEMPER.txt";
                    List<string> loadgroups = ids.Cast<string>().ToList();

                    if (File.Exists(path))
                    {
                        List<string> loads = File.ReadAllLines(path).ToList();

                        List<string> loadNames = new List<string>();
                        foreach (string load in loads)
                        {
                            if (load.Contains(";") || load.Contains("*") || string.IsNullOrWhiteSpace(load))
                            {
                                loadNames.Add("0");
                            }
                            else
                            {
                                string[] delimitted = load.Split(',');
                                string clone = delimitted[2].Replace(" ", "");
                                loadNames.Add(clone);
                            }
                        }

                        foreach (string loadgroup in loadgroups)
                        {
                            if (loadNames.Contains(loadgroup))
                            {
                                var nameIndexes = loadNames.Select((l, i) => new { l, i })
                                    .Where(x => x.l == loadgroup)
                                    .Select(x => x.i)
                                    .ToList();

                                foreach (var index in nameIndexes)
                                {
                                    loads[index] = "";
                                }
                            }
                        }

                        loads = loads.Where(x => !string.IsNullOrEmpty(x)).ToList();

                        File.Delete(path);
                        File.WriteAllLines(path, loads.ToArray());
                    }
                }
            }

            return success;
        }
    }
}
