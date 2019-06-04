using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;
using System.IO;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<ILoad> ReadPointLoads(List<string> ids = null)
        {
            List<ILoad> bhomPointLoads = new List<ILoad>();
            List<Loadcase> bhomLoadcases = ReadLoadcases();
            Dictionary<string, Loadcase> loadcaseDictionary = bhomLoadcases.ToDictionary(
                        x => x.Name);

            string[] loadcaseFolders = Directory.GetDirectories(directory + "\\TextFiles");

            int i = 1;

            foreach (string loadcaseFolder in loadcaseFolders)
            {
                string loadcase = Path.GetFileName(loadcaseFolder);
                List<string> PointLoadText = GetSectionText(loadcase + "\\CONLOAD");

                if (PointLoadText.Count != 0)
                {
                    List<Node> bhomNodes = ReadNodes();
                    Dictionary<string, Node> nodeDictionary = bhomNodes.ToDictionary(
                                                                x => x.CustomData[AdapterId].ToString());

                    List<string> PointLoadComparison = new List<string>();
                    List<string> PointLoadNodes = new List<string>();

                    foreach (string PointLoad in PointLoadText)
                    {
                        List<string> delimitted = PointLoad.Split(',').ToList();
                        PointLoadNodes.Add(delimitted[0].Trim());
                        delimitted.RemoveAt(0);
                        PointLoadComparison.Add(String.Join(",", delimitted));
                    }

                    List<string> distinctPointLoads = PointLoadComparison.Distinct().ToList();

                    foreach (string distinctPointLoad in distinctPointLoads)
                    {
                        List<int> indexMatches = PointLoadComparison.Select((pointload, index) => new { pointload, index })
                                                   .Where(x => string.Equals(x.pointload, distinctPointLoad))
                                                   .Select(x => x.index)
                                                   .ToList();
                        List<string> matchingNodes = new List<string>();
                        indexMatches.ForEach(x => matchingNodes.Add(PointLoadNodes[x]));
                        PointLoad bhomPointLoad = Engine.MidasCivil.Convert.ToBHoMPointLoad(distinctPointLoad, matchingNodes, loadcase, loadcaseDictionary, nodeDictionary, i);
                        bhomPointLoads.Add(bhomPointLoad);

                        if (String.IsNullOrWhiteSpace(distinctPointLoad.Split(',').ToList()[6]))
                        {
                            i = i + 1;
                        }

                    }
                }
            }

            return bhomPointLoads;
        }

    }
}