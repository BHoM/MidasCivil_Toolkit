using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public int DeleteBarVaryingDistributedLoads(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids != null && ids.Count()!=0)
            {
                string[] loadcaseNames = Directory.GetDirectories(directory+ "\\TextFiles\\");

                foreach (string loadcaseName in loadcaseNames)
                {
                    string path = loadcaseName + "\\BEAMLOAD.txt";
                    List<string> loadgroups = ids.Cast<string>().ToList();

                    if (File.Exists(path))
                    {
                        List<string> loads = File.ReadAllLines(path).ToList();

                        List<string> loadNames = new List<string>();

                        foreach (string load in loads)
                        {
                            if (load.Contains(";") || loads.Contains("*"))
                            {
                                loadNames.Add("0");
                            }
                            else if (load.Contains("UNILOAD") || load.Contains("UNIMOMENT"))
                            {
                                string[] delimitted = load.Split(',');
                                if (delimitted[11] != delimitted[13] || double.Parse(delimitted[10].Replace(" ", "")) + double.Parse(delimitted[12].Replace(" ", "")) != 1)
                                {
                                    string loadName = delimitted[18].Replace(" ", "");
                                    loadNames.Add(loadName);
                                }
                                else
                                {
                                    loadNames.Add("0");
                                }
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
