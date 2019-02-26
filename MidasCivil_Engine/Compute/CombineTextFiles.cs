using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace BH.Engine.MidasCivil
{
    public partial class Compute
    {
        public static bool CombineTextFiles(string filepath, List<Type> files = null, bool active = false)
        {
            bool success = true;

            if (active)
            {
                string directory;
                List<string> delimited = filepath.Split(new Char[] { '\\' }).ToList();
                delimited.Reverse();
                delimited.RemoveAt(0);
                delimited.Reverse();
                directory = string.Join("\\", delimited) + "\\TextFiles";

                string path = directory + "\\" + "COMBINED.txt";

                // Retrieve type strings: select all from directory if none provided

                List<string> typeNames = new List<string>();
                bool includeLoadcases = true;

                if (files.Count == 0)
                {
                    typeNames = Directory.GetFiles(directory, "*.txt")
                        .Select(Path.GetFileName)
                        .ToList();

                    for (int i = 0; i < typeNames.Count; i++)
                    {
                        typeNames[i] = typeNames[i].Replace(".txt", "");
                    }
                }
                else
                {
                    files.ForEach(x => typeNames.Add(Engine.MidasCivil.Convert.BHoMType(x.ToString())));

                    if (!typeNames.Contains("LOADCASE"))
                    {
                        includeLoadcases = false;
                    }

                    typeNames.Remove("LOADCASE");
                }

                // Check type dependencies to see if valid

                List<string> independents = new List<string> { "NODE", "ELEMENT", "MATERIAL", "SECTION", "STLDCASE" };
                List<string> nodeDependencies = new List<string> { "ELEMENT", "CONSTRAINT", "GROUPS", "LOCALAXIS", "SPRING", "CONLOAD" };
                List<string> elementDependencies = new List<string> { "GROUPS", "BEAMLOAD", "PRESSURE", "ELTEMPER" };
                List<string> materialDependencies = new List<string> { "DGN-MATL" };
                List<string> sectionDependencies = new List<string> { "DGN-SECT" };
                List<string> loadcaseDependencies = new List<string> { "LOADCOMB", "LC-COLOUR" };

                List<List<string>> dependents = new List<List<string>>();
                dependents.Add(nodeDependencies);
                dependents.Add(elementDependencies);
                dependents.Add(materialDependencies);
                dependents.Add(sectionDependencies);
                dependents.Add(loadcaseDependencies);

                for (int i = 0; i < independents.Count; i++)
                {
                    if (!typeNames.Contains(independents[i]))
                    {
                        foreach (string dependent in dependents[i])
                        {
                            if (typeNames.Contains(dependent))
                                Reflection.Compute.RecordError(dependent + "must have a" + independents[i] + "type file associated with it");
                        }
                    }
                }

                independents.Insert(0, "REBAR-MATL-CODE");
                independents.Insert(0, "UNIT");
                independents.Insert(0, "VERSION");
                independents.Add("LOAD-GROUP");
                independents.Add("LOADCOMB");

                List<string> loadcases = Directory.GetDirectories(directory).ToList();

                using (var combined = File.Create(path))
                {
                    using (StreamWriter writer = new StreamWriter(combined))
                    {
                        foreach (string independent in independents)
                        {
                            if (typeNames.Contains(independent))
                            {
                                var input = File.OpenRead(directory + "\\" + independent + ".txt");
                                input.CopyTo(combined);
                                writer.Write(Environment.NewLine);
                                writer.Flush();
                            }
                        }

                        typeNames = typeNames.Except(independents).ToList();

                        if (loadcases.Count() != 0 && includeLoadcases)
                        {
                            foreach (string loadcase in loadcases)
                            {
                                List<string> loadNames = Directory.GetFiles(loadcase, "*.txt")
                                            .Select(Path.GetFileName)
                                            .ToList();

                                foreach (string loadName in loadNames)
                                {
                                    List<string> contents = File.ReadAllLines(loadcase + "\\" + loadName).ToList();
                                    foreach (string line in contents)
                                    {
                                        if (line.Contains("; End of data"))
                                        {
                                            loadNames.RemoveAt(loadNames.IndexOf(loadName));
                                            loadNames.Add(loadName);
                                            goto ESCAPE;
                                        }
                                    }
                                }

                                ESCAPE:

                                string start = loadNames.Find(x => x.Contains("USE-STLD"));

                                using (var input = File.OpenRead(loadcase + "\\" + start))
                                {
                                    input.CopyTo(combined);
                                    loadNames.Remove(start);
                                }
                                writer.Write(Environment.NewLine);
                                writer.Flush();

                                foreach (string load in loadNames)
                                {
                                    using (var input = File.OpenRead(loadcase + "\\" + load))
                                    {
                                        input.CopyTo(combined);
                                    }

                                    writer.Write(Environment.NewLine);
                                    writer.Flush();
                                }
                            }
                        }

                        foreach (string file in typeNames)
                        {
                            using (var input = File.OpenRead(directory + "\\" + file + ".txt"))
                            {
                                input.CopyTo(combined);
                            }
                            writer.Write(Environment.NewLine);
                            writer.Flush();
                        }

                        writer.Write("*ENDDATA");
                        writer.Flush();
                    }
                    combined.Close();
                }
            }

            return success;

        }

    }
}
