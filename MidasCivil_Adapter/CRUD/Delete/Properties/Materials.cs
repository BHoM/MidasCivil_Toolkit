using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public int DeleteMaterials(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids.Count() != 0)
            {
                string path = directory + "\\TextFiles\\" + "MATERIAL" + ".txt";

                if(File.Exists(path))
                {
                    List<string> stringIndex = ids.Cast<string>().ToList();

                    List<int> indices = stringIndex.Select(x => int.Parse(x)).ToList();

                    List<string> materials = File.ReadAllLines(path).ToList();
                    List<int> materialIndexes = new List<int>();

                    foreach (string material in materials)
                    {
                        if (material.Contains("*") || material.Contains(";") || string.IsNullOrWhiteSpace(material))
                        {
                            int clone = 0;
                            materialIndexes.Add(clone);
                        }
                        else
                        {
                            int clone = int.Parse(material.Split(',')[0].Trim());
                            materialIndexes.Add(clone);
                        }
                    }

                    foreach (int index in indices)
                    {
                        if (materialIndexes.Contains(index))
                        {
                            int materialIndex = materialIndexes.IndexOf(index);
                            materials[materialIndex] = "";
                        }
                    }

                    materials = materials.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

                    File.Delete(path);
                    File.WriteAllLines(path, materials.ToArray());
                }
            }
            return success;
        }
    }
}
