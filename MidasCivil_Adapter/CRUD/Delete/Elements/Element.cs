using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Text.RegularExpressions;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public int DeleteElements(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids!=null)
            {
                string path = directory + "\\TextFiles\\" + "ELEMENT" + ".txt";

                if(File.Exists(path))
                {
                    List<string> stringIndex = ids.Cast<string>().ToList();

                    List<int> indices = ids.Cast<int>().ToList();

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
                            int clone = int.Parse(element.Split(',')[0].Replace(" ", ""));
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
