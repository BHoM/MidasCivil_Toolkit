using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public int DeleteSurfaceProperties(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids != null)
            {
                string path = directory + "\\TextFiles\\" + "THICKNESS" + ".txt";

                if (File.Exists(path))
                {
                    List<string> stringIndex = ids.Cast<string>().ToList();

                    List<int> indices = stringIndex.Select(int.Parse).ToList();

                    List<string> thicknesses = File.ReadAllLines(path).ToList();

                    List<int> thicknessIndexes = new List<int>();

                    for (int i=0; i<thicknesses.Count; i++)
                    {
                        if (thicknesses[i].Contains(";") || thicknesses[i].Contains("*"))
                        {
                            int clone = 0;
                            thicknessIndexes.Add(clone);
                        }
                        else if (thicknesses[i].Contains("STIFFENED"))
                        {
                            int clone = 0;
                            thicknessIndexes.Add(clone);
                            thicknessIndexes.Add(clone);
                            thicknessIndexes.Add(clone);
                            i = i + 2;
                        }
                        else
                        {
                            int clone = int.Parse(thicknesses[i].Split(',')[0].Replace(" ", ""));
                            thicknessIndexes.Add(clone);
                        }
                    }

                    foreach (int index in indices)
                    {
                        if (thicknessIndexes.Contains(index))
                        {
                            int thicknessIndex = thicknessIndexes.IndexOf(index);
                            thicknesses[thicknessIndex] = "";
                        }
                    }

                    thicknesses = thicknesses.Where(x => !string.IsNullOrEmpty(x)).ToList();

                    File.Delete(path);
                    File.WriteAllLines(path, thicknesses.ToArray());
                }
            }
            return success;
        }
    }
}
