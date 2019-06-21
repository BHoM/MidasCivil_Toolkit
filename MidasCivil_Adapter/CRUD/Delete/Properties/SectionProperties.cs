using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public int DeleteSectionProperties(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids.Count() != 0)
            {
                string path = directory + "\\TextFiles\\" + "SECTION" + ".txt";

                if(File.Exists(path))
                {
                    List<string> stringIndex = ids.Cast<string>().ToList();

                    List<int> indices = stringIndex.Select(x => int.Parse(x)).ToList();

                    List<string> sectionProperties = File.ReadAllLines(path).ToList();
                    List<int> sectionIndexes = new List<int>();

                    foreach (string sectionProperty in sectionProperties)
                    {
                        if (sectionProperty.Contains("*") || sectionProperty.Contains(";") || string.IsNullOrWhiteSpace(sectionProperty))
                        {
                            int clone = 0;
                            sectionIndexes.Add(clone);
                        }
                        else
                        {
                            int clone = int.Parse(sectionProperty.Split(',')[0].Trim());
                            sectionIndexes.Add(clone);
                        }
                    }

                    foreach (int index in indices)
                    {
                        if (sectionIndexes.Contains(index))
                        {
                            int sectionIndex = sectionIndexes.IndexOf(index);
                            sectionProperties[sectionIndex] = "";
                        }
                    }

                    sectionProperties = sectionProperties.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

                    File.Delete(path);
                    File.WriteAllLines(path, sectionProperties.ToArray());
                }
            }
            return success;
        }
    }
}
