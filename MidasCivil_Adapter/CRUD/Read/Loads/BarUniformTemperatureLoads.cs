/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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
using System.Linq;
using System.IO;
using BH.oM.Adapters.MidasCivil;
using BH.Engine.Adapter;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<ILoad> ReadBarUniformTemperatureLoads(List<string> ids = null)
        {
            List<ILoad> bhomBarUniformTemperatureLoads = new List<ILoad>();

            List<Loadcase> bhomLoadcases = ReadLoadcases();
            Dictionary<string, Loadcase> loadcaseDictionary = bhomLoadcases.ToDictionary(
                        x => x.Name);

            string[] loadcaseFolders = Directory.GetDirectories(m_directory + "\\TextFiles");

            int i = 1;

            foreach (string loadcaseFolder in loadcaseFolders)
            {
                string loadcase = Path.GetFileName(loadcaseFolder);
                List<string> barUniformTemperatureLoadText = GetSectionText(loadcase + "\\ELTEMPER");

                if (barUniformTemperatureLoadText.Count != 0)
                {
                    List<string> barComparison = new List<string>();
                    List<string> loadedBars = new List<string>();

                    foreach (string barUniformTemperatureLoad in barUniformTemperatureLoadText)
                    {
                        List<string> delimitted = barUniformTemperatureLoad.Split(',').ToList();
                        loadedBars.Add(delimitted[0].Trim());
                        delimitted.RemoveAt(0);
                        barComparison.Add(String.Join(",", delimitted));
                    }

                    if (barComparison.Count != 0)
                    {
                        List<Bar> bhomBars = ReadBars();
                        Dictionary<string, Bar> barDictionary = bhomBars.ToDictionary(
                                                                    x => x.AdapterId<string>(typeof(MidasCivilId)));
                        List<string> distinctBarLoads = barComparison.Distinct().ToList();

                        foreach (string distinctBarLoad in distinctBarLoads)
                        {
                            List<int> indexMatches = barComparison.Select((meshload, index) => new { meshload, index })
                                                       .Where(x => string.Equals(x.meshload, distinctBarLoad))
                                                       .Select(x => x.index)
                                                       .ToList();
                            List<string> matchingBars = new List<string>();
                            indexMatches.ForEach(x => matchingBars.Add(loadedBars[x]));

                            BarUniformTemperatureLoad bhomBarUniformTemperatureLoad =
                                Adapters.MidasCivil.Convert.ToBarUniformTemperatureLoad(
                                    distinctBarLoad, matchingBars, loadcase, loadcaseDictionary, barDictionary, i, m_temperatureUnit);

                            if (bhomBarUniformTemperatureLoad != null)
                                bhomBarUniformTemperatureLoads.Add(bhomBarUniformTemperatureLoad);

                            if ((string.IsNullOrWhiteSpace(distinctBarLoad.Split(',')[1].ToString())))
                            {
                                i = i + 1;
                            }

                        }

                    }
                }
            }
            return bhomBarUniformTemperatureLoads;
        }

        /***************************************************/

    }
}
