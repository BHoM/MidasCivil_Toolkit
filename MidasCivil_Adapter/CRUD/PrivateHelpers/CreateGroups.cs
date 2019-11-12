using System.Collections.Generic;
using System.Linq;
using System.IO;
using BH.oM.Structure.Elements;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public void CreateGroups(IEnumerable<Node> nodes)
        {
            Dictionary<string, List<int>> existingNodeGroups = ReadTags("GROUP", 1);
            Dictionary<string, List<int>> existingElementGroups = ReadTags("GROUP", 2);
            Dictionary<string, string> groupsToAdd = new Dictionary<string, string>();
            Dictionary<string, string> existingStringElementGroups = new Dictionary<string, string>();
            List<string> groups = existingNodeGroups.Keys.ToList();

            if(groups.Count!=0)
            {
                List<string> keys = existingNodeGroups.Keys.ToList();

                foreach (string key in keys)
                {
                    List<int> nodeValues = new List<int>();
                    existingNodeGroups.TryGetValue(key, out nodeValues);
                    List<int> elementValues = new List<int>();
                    existingElementGroups.TryGetValue(key, out elementValues);
                    string nodeStringValue = "";
                    string elementStringValue = "";

                    foreach (int value in nodeValues)
                    {
                        nodeStringValue = nodeStringValue + " " + value.ToString();
                    }

                    foreach (int value in elementValues)
                    {
                        elementStringValue = elementStringValue + " " + value.ToString();
                    }

                    groupsToAdd.Add(key, nodeStringValue);
                    existingStringElementGroups.Add(key, elementStringValue);
                }
            }

            foreach (Node node in nodes)
            {
                foreach (string tag in node.Tags)
                {
                    if (!groupsToAdd.ContainsKey(tag))
                    {
                        groupsToAdd.Add(tag, node.CustomData[AdapterId].ToString());
                    }
                    else
                    {
                        string assignedNodes;
                        groupsToAdd.TryGetValue(tag, out assignedNodes);
                        string bhomID = node.CustomData[AdapterId].ToString();

                        if (!assignedNodes.Contains(bhomID))
                        {
                            assignedNodes = assignedNodes + " " + bhomID;
                        }

                        groupsToAdd[tag] = assignedNodes;
                    }
                }
            }

            List<string> totalKeys = groupsToAdd.Keys.ToList();
            File.Delete(directory + "\\TextFiles\\GROUP.txt");
            string path = CreateSectionFile("GROUP");
           

            using (StreamWriter sw = File.AppendText(path))
            {
                foreach (string key in totalKeys)
                {
                    string nodeList;
                    groupsToAdd.TryGetValue(key, out nodeList);
                    string elementList;
                    existingStringElementGroups.TryGetValue(key, out elementList);
                    string line = key + "," + nodeList + "," + elementList + ",0";
                    sw.WriteLine(line);
                }
                sw.Flush();
                sw.Close();
            }
        }

        public void CreateGroups(IEnumerable<Bar> bars)
        {
            Dictionary<string, List<int>> existingNodeGroups = ReadTags("GROUP", 1);
            Dictionary<string, List<int>> existingElementGroups = ReadTags("GROUP", 2);
            Dictionary<string, string> groupsToAdd = new Dictionary<string, string>();
            Dictionary<string, string> existingStringNodeGroups = new Dictionary<string, string>();
            List<string> groups = existingNodeGroups.Keys.ToList();

            if (groups.Count != 0)
            {
                List<string> keys = existingNodeGroups.Keys.ToList();

                foreach (string key in keys)
                {
                    List<int> nodeValues = new List<int>();
                    existingNodeGroups.TryGetValue(key, out nodeValues);
                    List<int> elementValues = new List<int>();
                    existingElementGroups.TryGetValue(key, out elementValues);
                    string nodeStringValue = "";
                    string elementStringValue = "";

                    foreach (int value in nodeValues)
                    {
                        nodeStringValue = nodeStringValue + " " + value.ToString();
                    }

                    foreach (int value in elementValues)
                    {
                        elementStringValue = elementStringValue + " " + value.ToString();
                    }

                    groupsToAdd.Add(key, elementStringValue);
                    existingStringNodeGroups.Add(key, nodeStringValue);
                }
            }

            foreach (Bar bar in bars)
            {
                foreach (string tag in bar.Tags)
                {
                    if (!groupsToAdd.ContainsKey(tag))
                    {
                        groupsToAdd.Add(tag, bar.CustomData[AdapterId].ToString());
                    }
                    else
                    {
                        string assignedBars;
                        groupsToAdd.TryGetValue(tag, out assignedBars);
                        string bhomID = bar.CustomData[AdapterId].ToString();

                        if (!assignedBars.Contains(bhomID))
                        {
                            assignedBars = assignedBars + " " + bhomID;
                        }

                        groupsToAdd[tag] = assignedBars;
                    }
                }
            }

            List<string> totalKeys = groupsToAdd.Keys.ToList();
            File.Delete(directory + "\\TextFiles\\GROUP.txt");
            string path = CreateSectionFile("GROUP");


            using (StreamWriter sw = File.AppendText(path))
            {
                foreach (string key in totalKeys)
                {
                    string nodeList;
                    existingStringNodeGroups.TryGetValue(key, out nodeList);
                    string elementList;
                    groupsToAdd.TryGetValue(key, out elementList);
                    string line = key + "," + nodeList + "," + elementList + ",0";
                    sw.WriteLine(line);
                }
                sw.Flush();
                sw.Close();
            }
        }

        public void CreateGroups(IEnumerable<FEMesh> meshes)
        {
            Dictionary<string, List<int>> existingNodeGroups = ReadTags("GROUP", 1);
            Dictionary<string, List<int>> existingElementGroups = ReadTags("GROUP", 2);
            Dictionary<string, string> groupsToAdd = new Dictionary<string, string>();
            Dictionary<string, string> existingStringNodeGroups = new Dictionary<string, string>();
            List<string> groups = existingNodeGroups.Keys.ToList();

            if (groups.Count != 0)
            {
                List<string> keys = existingNodeGroups.Keys.ToList();

                foreach (string key in keys)
                {
                    List<int> nodeValues = new List<int>();
                    existingNodeGroups.TryGetValue(key, out nodeValues);
                    List<int> elementValues = new List<int>();
                    existingElementGroups.TryGetValue(key, out elementValues);
                    string nodeStringValue = "";
                    string elementStringValue = "";

                    foreach (int value in nodeValues)
                    {
                        nodeStringValue = nodeStringValue + " " + value.ToString();
                    }

                    foreach (int value in elementValues)
                    {
                        elementStringValue = elementStringValue + " " + value.ToString();
                    }

                    groupsToAdd.Add(key, elementStringValue);
                    existingStringNodeGroups.Add(key, nodeStringValue);
                }
            }

            foreach (FEMesh mesh in meshes)
            {
                foreach (string tag in mesh.Tags)
                {
                    if (!groupsToAdd.ContainsKey(tag))
                    {
                        groupsToAdd.Add(tag, mesh.CustomData[AdapterId].ToString());
                    }
                    else
                    {
                        string assignedFEMesh;
                        groupsToAdd.TryGetValue(tag, out assignedFEMesh);
                        string bhomID = mesh.CustomData[AdapterId].ToString();

                        if (!assignedFEMesh.Contains(bhomID))
                        {
                            assignedFEMesh = assignedFEMesh + " " + bhomID;
                        }

                        groupsToAdd[tag] = assignedFEMesh;
                    }
                }
            }

            List<string> totalKeys = groupsToAdd.Keys.ToList();
            File.Delete(directory + "\\TextFiles\\GROUP.txt");
            string path = CreateSectionFile("GROUP");


            using (StreamWriter sw = File.AppendText(path))
            {
                foreach (string key in totalKeys)
                {
                    string nodeList;
                    existingStringNodeGroups.TryGetValue(key, out nodeList);
                    string elementList;
                    groupsToAdd.TryGetValue(key, out elementList);
                    string line = key + "," + nodeList + "," + elementList + ",0";
                    sw.WriteLine(line);
                }
                sw.Flush();
                sw.Close();
            }
        }
    }
}
