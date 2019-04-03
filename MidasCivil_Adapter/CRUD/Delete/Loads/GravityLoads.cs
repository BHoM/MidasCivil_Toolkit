using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public int DeleteGravityLoads(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids != null && ids.Count() != 0)
            {
                string[] loadcaseNames = Directory.GetDirectories(directory+ "\\TextFiles\\");

                foreach (string loadcaseName in loadcaseNames)
                {
                    string path = loadcaseName + "\\SELFWEIGHT.txt";
                    List<string> loadgroups = ids.Cast<string>().ToList();

                    if (File.Exists(path))
                    {
                        List<string> loads = File.ReadAllLines(path).ToList();

                        List<string> loadNames = new List<string>();
                        foreach (string load in loads)
                        {
                            if (load.Contains(";")|| string.IsNullOrWhiteSpace(load))
                            {
                                string clone = 0.ToString();
                                loadNames.Add(clone);
                            }
                            else
                            {
                                string clone = load.Split(',').Reverse().First().Replace(" ", "");
                                loadNames.Add(clone);
                            }
                        }

                        foreach (string loadgroup in loadgroups)
                        {
                            if (loadNames.Contains(loadgroup))
                            {
                                int nameIndex = loadNames.IndexOf(loadgroup);
                                loads[nameIndex] = "";
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
