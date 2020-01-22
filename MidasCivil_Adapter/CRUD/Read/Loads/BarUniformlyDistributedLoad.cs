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

            int i = 1;

            foreach (string loadcaseFolder in loadcaseFolders)
            {
                string loadcase = Path.GetFileName(loadcaseFolder);
                List<string> barUniformlyDistributedLoadText = GetSectionText(loadcase + "\\BEAMLOAD");

                if (barUniformlyDistributedLoadText.Contains("LINE"))
                {
                    Engine.Reflection.Compute.RecordWarning("MidasCivil_Toolkit does not support line loads");
                }

                List<string> barUniformLoads = barUniformlyDistributedLoadText.Where(x => x.Contains("UNILOAD")).ToList();
                barUniformLoads.AddRange(barUniformlyDistributedLoadText.Where(x => x.Contains("UNIMOMENT")).ToList());

                if (barUniformLoads.Count != 0)
                {
                    List<string> barComparison = new List<string>();
                    List<string> loadedBars = new List<string>();

                    foreach (string barUniformlyDistributedLoad in barUniformLoads)
                    {
                        List<string> delimitted = barUniformlyDistributedLoad.Split(',').ToList();

                        if (delimitted[11] == delimitted[13])
                        {
                            if (delimitted[10].Trim() == 0.ToString() && delimitted[12].Trim() == 1.ToString())
                            {
                                loadedBars.Add(delimitted[0].Trim());
                                delimitted.RemoveAt(0);
                                barComparison.Add(String.Join(",", delimitted));
                            }
                        }
                    }

                    List<List<Bar>> bhomLoadedBars = new List<List<Bar>>();

                    if (barComparison.Count != 0)
                    {
                        List<Bar> bhomBars = ReadBars();
                        Dictionary<string, Bar> barDictionary = bhomBars.ToDictionary(
                                                                    x => x.CustomData[AdapterIdName].ToString());
                        List<string> distinctBarLoads = barComparison.Distinct().ToList();

                        foreach (string distinctBarLoad in distinctBarLoads)
                        {
                            List<int> indexMatches = barComparison.Select((barload, index) => new { barload, index })
                                                       .Where(x => string.Equals(x.barload, distinctBarLoad))
                                                       .Select(x => x.index)
                                                       .ToList();
                            List<string> matchingBars = new List<string>();
                            indexMatches.ForEach(x => matchingBars.Add(loadedBars[x]));

                            BarUniformlyDistributedLoad bhomBarUniformlyDistributedLoad = 
                                Engine.MidasCivil.Convert.ToBHoMBarUniformlyDistributedLoad(
                                    distinctBarLoad, matchingBars, loadcase, loadcaseDictionary, barDictionary, i);
                            bhomBarUniformlyDistributedLoads.Add(bhomBarUniformlyDistributedLoad);


                            if ((distinctBarLoad.Split(',').ToList()[22].ToString() == " "))
                            {
                                i = i + 1;
                            }

                        }
                        
                    }
                }
            }

            return bhomBarUniformlyDistributedLoads;
        }

    }
}
