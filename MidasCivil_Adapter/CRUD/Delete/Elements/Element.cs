using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public int DeleteElements(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids.Count() != 0)
            {
                string path = directory + "\\TextFiles\\" + "ELEMENT" + ".txt";

                if(File.Exists(path))
                {
                    List<string> stringIndex = ids.Select(x => x.ToString()).ToList();

                    List<int> indices = stringIndex.Select(int.Parse).ToList();

                    List<string> elements = File.ReadAllLines(path).ToList();
                    List<int> elementIndexes = new List<int>();

                    foreach (string element in elements)
                    {
                        if (element.Contains("*") || element.Contains(";"))
                        {
                            int clone = 0;
                            elementIndexes.Add(clone);
                        }
                        else
                        {
                            int clone = int.Parse(element.Split(',')[0].Trim());
                            elementIndexes.Add(clone);
                        }
                    }

                    foreach (int index in indices)
                    {
                        if (elementIndexes.Contains(index))
                        {
                            int elementIndex = elementIndexes.IndexOf(index);
                            elements[elementIndex] = "";
                        }
                    }

                    elements = elements.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

                    File.Delete(path);
                    File.WriteAllLines(path, elements.ToArray());
                }
            }
            return success;
        }

    }
}
