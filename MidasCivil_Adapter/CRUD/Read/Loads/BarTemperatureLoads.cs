using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<ILoad> ReadBarTemperatureLoads(List<string> ids = null)
        {
            List<ILoad> bhomBarTemperatureLoads = new List<ILoad>();

            List<Loadcase> bhomLoadcases = ReadLoadcases();
            Dictionary<string, Loadcase> loadcaseDictionary = bhomLoadcases.ToDictionary(
                        x => x.Name);

            string[] loadcaseFolders = Directory.GetDirectories(directory + "\\TextFiles");

            int i = 1;

            foreach (string loadcaseFolder in loadcaseFolders)
            {
                string loadcase = Path.GetFileName(loadcaseFolder);
                List<string> BarTemperatureLoadText = GetSectionText(loadcase + "\\ELTEMPER");

                if (BarTemperatureLoadText.Count != 0)
                {
                    List<string> barComparison = new List<string>();
                    List<string> loadedBars = new List<string>();

                    foreach (string BarTemperatureLoad in BarTemperatureLoadText)
                    {
                        List<string> delimitted = BarTemperatureLoad.Split(',').ToList();
                        loadedBars.Add(delimitted[0].Trim());
                        delimitted.RemoveAt(0);
                        barComparison.Add(String.Join(",", delimitted));
                    }

                    if (barComparison.Count != 0)
                    {
                        List<Bar> bhomBars = ReadBars();
                        Dictionary<string, Bar> barDictionary = bhomBars.ToDictionary(
                                                                    x => x.CustomData[AdapterIdName].ToString());
                        List<string> distinctBarLoads = barComparison.Distinct().ToList();

                        foreach (string distinctBarLoad in distinctBarLoads)
                        {
                            List<int> indexMatches = barComparison.Select((meshload, index) => new { meshload, index })
                                                       .Where(x => string.Equals(x.meshload, distinctBarLoad))
                                                       .Select(x => x.index)
                                                       .ToList();
                            List<string> matchingBars = new List<string>();
                            indexMatches.ForEach(x => matchingBars.Add(loadedBars[x]));

                            BarTemperatureLoad bhomBarTemperatureLoad =
                                Engine.MidasCivil.Convert.ToBHoMBarTemperatureLoad(
                                    distinctBarLoad, matchingBars, loadcase, loadcaseDictionary, barDictionary, i);

                            if (bhomBarTemperatureLoad != null)
                                bhomBarTemperatureLoads.Add(bhomBarTemperatureLoad);

                            if ((string.IsNullOrWhiteSpace(distinctBarLoad.Split(',')[1].ToString())))
                            {
                                i = i + 1;
                            }

                        }

                    }
                }
            }
            return bhomBarTemperatureLoads;
        }
    }
}