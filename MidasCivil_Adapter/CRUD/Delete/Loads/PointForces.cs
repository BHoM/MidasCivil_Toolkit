using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public int DeletePointForces(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids.Count()!=0)
            {
                string[] loadcaseNames = Directory.GetDirectories(directory+ "\\TextFiles\\");

                foreach (string loadcaseName in loadcaseNames)
                {
                    string path = loadcaseName + "\\CONLOAD.txt";
                    List<string> loadgroups = ids.Cast<string>().ToList();

                    if (File.Exists(path))
                    {
                        List<string> loads = File.ReadAllLines(path).ToList();

                        List<string> loadNames = new List<string>();
                        foreach (string load in loads)
                        {
                            if (!load.Contains(";") || load.Contains("*") || string.IsNullOrWhiteSpace(load))
                            {
                                loadNames.Add("0");
                            }
                            else
                            {
                                string loadName = load.Split(',').Reverse().First().Replace(" ", "");
                                loadNames.Add(loadName);
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
