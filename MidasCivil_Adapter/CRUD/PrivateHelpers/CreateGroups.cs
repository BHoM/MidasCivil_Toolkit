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
                    existingNodeGroups.TryGetValue(key, out List<int> nodeValues);
                    existingElementGroups.TryGetValue(key, out List<int> elementValues);
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
                        groupsToAdd.TryGetValue(tag, out string assignedNodes);
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
                    groupsToAdd.TryGetValue(key, out string nodeList);
                    existingStringElementGroups.TryGetValue(key, out string elementList);
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
                    existingNodeGroups.TryGetValue(key, out List<int> nodeValues);
                    existingElementGroups.TryGetValue(key, out List<int> elementValues);
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
                        groupsToAdd.TryGetValue(tag, out string assignedBars);
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
                    existingStringNodeGroups.TryGetValue(key, out string nodeList);
                    groupsToAdd.TryGetValue(key, out string elementList);
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
                    existingNodeGroups.TryGetValue(key, out List<int> nodeValues);
                    existingElementGroups.TryGetValue(key, out List<int> elementValues);
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
                        groupsToAdd.TryGetValue(tag, out string assignedFEMesh);
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
                    existingStringNodeGroups.TryGetValue(key, out string nodeList);
                    groupsToAdd.TryGetValue(key, out string elementList);
                    string line = key + "," + nodeList + "," + elementList + ",0";
                    sw.WriteLine(line);
                }
                sw.Flush();
                sw.Close();
            }
        }
    }
}
