using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.ComponentModel;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.MidasCivil
{
    public static partial class Compute
    {
        [Description("Combines all text files specified by type in to a single MidasCivilText (MCT) to be loaded in to MidasCivil")]
        [Input("filePath", "The same filepath used for the adapter (pointing to an mcb file)")]
        [Input("types", "BHoM object types to specify the text files to be combined. A null value will combine all text files.")]
        [Input("active", "Execute the method")]
        [Output("success","Was the execution successful?")]

        public static bool CombineTextFiles(string filepath, List<Type> types = null, bool active = false)
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

                string path = directory + "\\" + "COMBINED.mct";

                // Retrieve type strings: select all from directory if none provided

                List<string> typeNames = new List<string>();
                bool includeLoadcases = true;

                if (types.Count == 0)
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
                    types.ForEach(x => typeNames.Add(Engine.MidasCivil.Convert.BHoMType(x.ToString())));

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
                                Reflection.Compute.RecordError(dependent + " must have a " + independents[i] + " file associated with it");
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
                                if (new FileInfo(directory + "\\" + independent + ".txt").Length !=0)
                                {
                                    var input = File.OpenRead(directory + "\\" + independent + ".txt");
                                    input.CopyTo(combined);
                                    writer.Write(System.Environment.NewLine);
                                    writer.Flush();
                                }
                            }
                        }

                        typeNames = typeNames.Except(independents).ToList();

                        if (loadcases.Count() != 0 && includeLoadcases)
                        {
                            foreach (string loadcase in loadcases)
                            {
                                string loadcaseName = Path.GetFileName(loadcase);

                                List<string> loadNames = Directory.GetFiles(loadcase, "*.txt")
                                            .Select(Path.GetFileName)
                                            .ToList();

                                bool endIncluded = false;

                                foreach (string loadName in loadNames)
                                {
                                    List<string> contents = File.ReadAllLines(loadcase + "\\" + loadName).ToList();
                                    foreach (string line in contents)
                                    {
                                        if (line.Contains("; End of data") && new FileInfo(loadcase + "\\" + loadName).Length != 0)
                                        {
                                            loadNames.RemoveAt(loadNames.IndexOf(loadName));
                                            loadNames.Add(loadName);
                                            endIncluded = true;
                                            goto ESCAPE;
                                        }
                                    }
                                }

                                ESCAPE:

                                loadNames.Remove("USE-STLD.txt");
                                writer.Write("*USE-STLD, " + loadcaseName);
                                writer.Write(System.Environment.NewLine);
                                writer.Flush();

                                foreach (string load in loadNames)
                                {
                                    using (var input = File.OpenRead(loadcase + "\\" + load))
                                    {
                                        input.CopyTo(combined);
                                    }

                                    writer.Write(System.Environment.NewLine);
                                    writer.Flush();
                                }

                                if (!endIncluded)
                                {
                                    writer.Write("; End of data for load case " + loadcaseName);
                                    writer.Write(System.Environment.NewLine);
                                    writer.Write(System.Environment.NewLine);
                                    writer.Flush();
                                }
                            }
                        }

                        foreach (string file in typeNames)
                        {
                            using (var input = File.OpenRead(directory + "\\" + file + ".txt"))
                            {
                                if (new FileInfo(directory + "\\" + file + ".txt").Length != 0)
                                {
                                    input.CopyTo(combined);
                                }
                            }
                            writer.Write(System.Environment.NewLine);
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
