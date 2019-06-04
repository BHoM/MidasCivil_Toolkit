using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public int DeleteNodes(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids.Count() != 0)
            {
                string path = directory + "\\TextFiles\\" + "NODE" + ".txt";

                if(File.Exists(path))
                {
                    List<string> stringIndex = ids.Select(x => x.ToString()).ToList();

                    List<int> indices = stringIndex.Select(int.Parse).ToList();

                    List<string> nodes = File.ReadAllLines(path).ToList();

                    List<int> nodeIndexes = new List<int>();
                    foreach (string node in nodes)
                    {
                        if (node.Contains(";") || node.Contains("*"))
                        {
                            int clone = 0;
                            nodeIndexes.Add(clone);
                        }
                        else
                        {
                            int clone = int.Parse(node.Split(',')[0].Trim());
                            nodeIndexes.Add(clone);
                        }
                    }

                    foreach (int index in indices)
                    {
                        if (nodeIndexes.Contains(index))
                        {
                            int nodeIndex = nodeIndexes.IndexOf(index);
                            nodes[nodeIndex] = "";
                        }
                    }

                    nodes = nodes.Where(x => !string.IsNullOrEmpty(x)).ToList();

                    File.Delete(path);
                    File.WriteAllLines(path, nodes.ToArray());
                }
            }
            return success;
        }

    }
}
