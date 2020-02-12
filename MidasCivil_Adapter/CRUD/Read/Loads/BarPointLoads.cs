/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

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
        private List<ILoad> ReadBarPointLoads(List<string> ids = null)
        {
            List<ILoad> bhomBarPointLoads = new List<ILoad>();
            List<Loadcase> bhomLoadcases = ReadLoadcases();
            Dictionary<string, Loadcase> loadcaseDictionary = bhomLoadcases.ToDictionary(
                        x => x.Name);

            string[] loadcaseFolders = Directory.GetDirectories(directory + "\\TextFiles");

            int j = 1;

            foreach (string loadcaseFolder in loadcaseFolders)
            {
                string loadcase = Path.GetFileName(loadcaseFolder);
                List<string> barPointLoadText = GetSectionText(loadcase + "\\BEAMLOAD");

                List<string> barPointLoads = barPointLoadText.Where(x => x.Contains("CONLOAD")).ToList();
                barPointLoads.AddRange(barPointLoadText.Where(x => x.Contains("CONMOMENT")).ToList());

                if (barPointLoads.Count != 0)
                {
                    List<string> barComparison = new List<string>();
                    List<string> loadedBars = new List<string>();

                    foreach (string barPointLoad in barPointLoads)
                    {
                        List<double> pointLoads = new List<double>();
                        List<double> pointLoadDistances = new List<double>();
                        List<string> delimitted = barPointLoad.Split(',').ToList();
                        string bar = delimitted[0].Trim();
                        delimitted.RemoveAt(0);

                        for (int i = 10; i <= 16; i += 2)
                        {
                            pointLoads.Add(double.Parse(delimitted[i].Trim()));
                            pointLoadDistances.Add(double.Parse(delimitted[i - 1].Trim()));
                            delimitted[i] = 0.ToString();
                            delimitted[i - 1] = 0.ToString();
                        }

                        int count = 0;

                        for (int i = 0; i < 4; i++)
                        {
                            if (pointLoads[i] != 0)
                            {
                                delimitted[9] = pointLoadDistances[i].ToString();
                                delimitted[10] = pointLoads[i].ToString();
                                barComparison.Add(String.Join(",", delimitted));
                                count++;
                            }
                        }

                        for (int i = 0; i <= count - 1; i++)
                        {
                            loadedBars.Add(bar);
                        }
                    }

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

                            BarPointLoad bhomBarPointLoad = Engine.MidasCivil.Convert.ToBHoMBarPointLoad(distinctBarLoad, matchingBars, loadcase, loadcaseDictionary, barDictionary, j);
                            bhomBarPointLoads.Add(bhomBarPointLoad);

                            if (String.IsNullOrWhiteSpace(distinctBarLoad.Split(',').ToList()[22]))

                            {
                                j = j + 1;
                            }
                        }
                    }

                }
            }

            return bhomBarPointLoads;
        }

    }
}