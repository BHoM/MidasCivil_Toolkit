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
        private List<ILoad> ReadBarVaryingDistributedLoads(List<string> ids = null)
        {
            List<ILoad> bhomBarVaryingDistributedLoads = new List<ILoad>();
            List<Loadcase> bhomLoadcases = ReadLoadcases();
            Dictionary<string, Loadcase> loadcaseDictionary = bhomLoadcases.ToDictionary(
                        x => x.Name);

            string[] loadcaseFolders = Directory.GetDirectories(directory + "\\TextFiles");

            int i = 1;

            foreach (string loadcaseFolder in loadcaseFolders)
            {
                string loadcase = Path.GetFileName(loadcaseFolder);
                List<string> barVaryingDistributedLoadText = GetSectionText(loadcase + "\\BEAMLOAD");

                List<string> barVaryingLoads = barVaryingDistributedLoadText.Where(x => x.Contains("UNILOAD")).ToList();
                barVaryingLoads.AddRange(barVaryingDistributedLoadText.Where(x => x.Contains("UNIMOMENT")).ToList());

                if (barVaryingLoads.Count != 0)
                {
                    List<string> barComparison = new List<string>();
                    List<string> loadedBars = new List<string>();

                    foreach (string barVaryingLoad in barVaryingLoads)
                    {
                        List<string> delimitted = barVaryingLoad.Split(',').ToList();

                        if (delimitted[11] != delimitted[13] || double.Parse(delimitted[10].Trim()) + double.Parse(delimitted[12].Trim()) != 1)
                        {
                            loadedBars.Add(delimitted[0].Trim());
                            delimitted.RemoveAt(0);
                            barComparison.Add(String.Join(",", delimitted));
                        }
                    }

                    if (barComparison.Count != 0)
                    {
                        List<Bar> bhomBars = ReadBars();
                        Dictionary<string, Bar> barDictionary = bhomBars.ToDictionary(
                                                                    x => x.CustomData[AdapterId].ToString());

                        List<string> distinctBarLoads = barComparison.Distinct().ToList();

                        foreach (string distinctBarLoad in distinctBarLoads)
                        {
                            List<int> indexMatches = barComparison.Select((barload, index) => new { barload, index })
                                                       .Where(x => string.Equals(x.barload, distinctBarLoad))
                                                       .Select(x => x.index)
                                                       .ToList();
                            List<string> matchingBars = new List<string>();
                            indexMatches.ForEach(x => matchingBars.Add(loadedBars[x]));

                            BarVaryingDistributedLoad bhomBarVaryingDistributedLoad =
                                Engine.MidasCivil.Convert.ToBHoMBarVaryingDistributedLoad(
                                    distinctBarLoad, matchingBars, loadcase, loadcaseDictionary, barDictionary, i);
                            bhomBarVaryingDistributedLoads.Add(bhomBarVaryingDistributedLoad);


                            if (String.IsNullOrWhiteSpace(distinctBarLoad.Split(',').ToList()[22]))
                            {
                                i = i + 1;
                            }

                        }


                    }

                }
            }

            return bhomBarVaryingDistributedLoads;
        }

    }
}