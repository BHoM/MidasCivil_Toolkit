using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public int DeleteBarReleases(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids.Count()!=0)
            {
                string path = directory + "\\TextFiles\\" + "FRAME-RLS" + ".txt";

                if(File.Exists(path))
                {
                    List<string> names = ids.Cast<string>().ToList();
                    List<string> releases = File.ReadAllLines(path).ToList();
                    List<string> releaseNames = new List<string>();

                    for (int i=0; i<releases.Count; i++)
                    {
                        if (releases[i].Contains("*") || releases[i].Contains(";") || string.IsNullOrWhiteSpace(releases[i]))
                        {
                            string clone = "0";
                            releaseNames.Add(clone);
                        }
                        else
                        {
                            string clone = releases[i+1].Split(',')[7].Trim();
                            releaseNames.Add(clone);
                            releaseNames.Add(clone);
                            i++;
                        }
                    }

                    foreach (string name in names)
                    {
                        if (releaseNames.Contains(name))
                        {
                            var nameIndexes = releaseNames.Select((l, i) => new { l, i })
                                .Where(x => x.l == name)
                                .Select(x => x.i)
                                .ToList();

                            foreach (var index in nameIndexes)
                            {
                                releases[index] = "";
                            }
                        }
                    }

                    releases = releases.Where(x => !string.IsNullOrEmpty(x)).ToList();

                    File.Delete(path);
                    File.WriteAllLines(path, releases.ToArray());
                }
            }
            return success;
        }

    }
}
