using System;
using System.Collections.Generic;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;
using System.Linq;
using System.IO;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<ILoad> ReadBarUniformlyDistributedLoads(List<string> ids = null)
        {
            List<ILoad> bhomBarUniformlyDistributedLoads = new List<ILoad>();
            List<Loadcase> bhomLoadcases = ReadLoadcases();
            Dictionary<string, Loadcase> loadcaseDictionary = bhomLoadcases.ToDictionary(
                        x => x.Name);

            string[] loadcaseFolders = Directory.GetDirectories(directory + "\\TextFiles");

            foreach (string loadcaseFolder in loadcaseFolders)
            {
                string loadcase = Path.GetFileName(loadcaseFolder);
                List<string> barUniformlyDistributedLoadText = GetSectionText(loadcase + "\\BEAMLOAD");

                if (barUniformlyDistributedLoadText.Count != 0)
                {
                    List<Bar> bhomBars = ReadBars();
                    Dictionary<string, Bar> barDictionary = bhomBars.ToDictionary(
                                                                x => x.CustomData[AdapterId].ToString());

                    List<string> barComparison = new List<string>();
                    List<string> loadedBars = new List<string>();

                    foreach (string barUniformlyDistributedLoad in barUniformlyDistributedLoadText)
                    {
                        List<string> delimitted = barUniformlyDistributedLoad.Split(',').ToList();
                        loadedBars.Add(delimitted[0].Replace(" ", ""));
                        delimitted.RemoveAt(0);

                        if(delimitted[10]==delimitted[12])
                        {
                            barComparison.Add(String.Join(",", delimitted));
                        }
                    }

                    List<string> distinctBarLoads = barComparison.Distinct().ToList();
                    List<List<string>> barIndices = new List<List<string>>();

                    foreach (string barLoad in distinctBarLoads)
                    {
                        List<int> indexMatches = barComparison.Select((barload, index) => new { barload, index })
                                                   .Where(x => string.Equals(x.barload, barLoad))
                                                   .Select(x => x.index)
                                                   .ToList();
                        List<string> matchingBars = new List<string>();
                        indexMatches.ForEach(x => matchingBars.Add(loadedBars[x]));
                        barIndices.Add(matchingBars);
                    }

                    for (int i = 0; i < distinctBarLoads.Count; i++)
                    {
                        BarUniformlyDistributedLoad bhomBarUniformlyDistributedLoad = Engine.MidasCivil.Convert.ToBHoMBarUniformlyDistributedLoad(distinctBarLoads[i], barIndices[i], loadcase, loadcaseDictionary, barDictionary, i + 1);
                        bhomBarUniformlyDistributedLoads.Add(bhomBarUniformlyDistributedLoad);
                    }

                }
            }
            return bhomBarUniformlyDistributedLoads;
        }
    }
}