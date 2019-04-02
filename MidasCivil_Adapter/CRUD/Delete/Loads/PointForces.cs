using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public int DeletePointForces(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids != null)
            {
                string[] loadcaseNames = Directory.GetDirectories(directory);

                foreach (string loadcaseName in loadcaseNames)
                {
                    string path = directory + "\\TextFiles\\" + loadcaseName + "\\CONLOAD" + ".txt";

                    if (File.Exists(path))
                    {
                        List<string> loadgroups = ids.Cast<string>().ToList();

                        List<string> pointForces = File.ReadAllLines(path).ToList();

                        List<string> pointForceNames = new List<string>();
                        foreach (string pointForce in pointForces)
                        {
                            if (pointForces.Contains(";") || pointForces.Contains("*"))
                            {
                                string clone = 0.ToString();
                                pointForceNames.Add(clone);
                            }
                            else
                            {
                                string clone = pointForce.Split(',').Reverse().First().Replace(" ", "");
                                pointForceNames.Add(clone);
                            }
                        }

                        foreach (string loadgroup in loadgroups)
                        {
                            if (loadcaseNames.Contains(loadgroup))
                            {
                                int nameIndex = pointForceNames.IndexOf(loadgroup);
                                pointForces[nameIndex] = "";
                            }
                        }

                        loadgroups = loadgroups.Where(x => !string.IsNullOrEmpty(x)).ToList();

                        File.Delete(path);
                        File.WriteAllLines(path, loadgroups.ToArray());
                    }
                }
            }

            return success;
        }
    }
}
