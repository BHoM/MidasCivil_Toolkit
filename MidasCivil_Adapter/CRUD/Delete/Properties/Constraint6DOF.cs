using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public int DeleteConstraints(IEnumerable<object> ids)
        {
            int success = 1;

            if (ids.Count() != 0)
            {
                string constraintPath = directory + "\\TextFiles\\" + "CONSTRAINT" + ".txt";
                string springPath = directory + "\\TextFiles\\" + "SPRING" + ".txt";
                List<string> paths = new List<string> { constraintPath, springPath };

                foreach (string path in paths)
                {
                    List<string> names = ids.Cast<string>().ToList();
                    List<string> constraints = File.ReadAllLines(path).ToList();
                    List<string> constraintNames = new List<string>();

                    if (File.Exists(path))
                    {
                        foreach (string constraint in constraints)
                        {
                            if (constraint.Contains("*") || constraint.Contains(";"))
                            {
                                string clone = 0.ToString();
                                constraintNames.Add(clone);
                            }
                            else
                            {
                                if (path.Contains("CONSTRAINT") && !string.IsNullOrWhiteSpace(constraint))
                                    constraintNames.Add(constraint.Split(',').Reverse().First().Replace(" ", ""));
                                else if (!string.IsNullOrWhiteSpace(constraint))
                                    constraintNames.Add(constraint.Split(',')[15].Replace(" ", ""));
                            }
                        }

                        foreach (string name in names)
                        {
                            if (constraintNames.Contains(name))
                            {
                                int constraintIndex = constraintNames.IndexOf(name);
                                constraints[constraintIndex] = "";
                            }
                        }

                        constraints = constraints.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

                        File.Delete(path);
                        File.WriteAllLines(path, constraints.ToArray());
                    }
                }
            }
            return success;
        }
    }
}
