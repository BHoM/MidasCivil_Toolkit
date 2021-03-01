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

        private List<ILoad> ReadBarDifferentialTemperatureLoads(List<string> ids = null)
        {
            List<ILoad> bhomBarDifferentialTemperatureLoads = new List<ILoad>();

            List<Loadcase> bhomLoadcases = ReadLoadcases();
            Dictionary<string, Loadcase> loadcaseDictionary = bhomLoadcases.ToDictionary(
                        x => x.Name);
            string[] loadcaseFolders = Directory.GetDirectories(m_directory + "\\TextFiles");
            int i = 1;
            foreach (string loadcaseFolder in loadcaseFolders)
            {
                string loadcase = Path.GetFileName(loadcaseFolder);
                List<string> barDifferentialTemperatureLoadText = GetSectionText(loadcase + "\\BSTEMPER");
                if (barDifferentialTemperatureLoadText.Count != 0)
                {
                    List<List<string>> barDifferntialTemperatureSets = new List<List<string>>();
                    List<string> barID = new List<string>();
                    List<int> loadIndexes = new List<int>();
                    List<string> delimitted = new List<string>();
                    for (int j = 0; j < barDifferentialTemperatureLoadText.Count; j++)
                    {
                        if (!barDifferentialTemperatureLoadText[j].Contains("ELEMENT"))
                        {
                            loadIndexes.Add(j);
                            delimitted = barDifferentialTemperatureLoadText[j].Split(',').ToList();
                            barID.Add(delimitted[0].Trim());
                        }
                    }
                    loadIndexes.Add(barDifferentialTemperatureLoadText.Count);
                    //This is needed to iterate over all BarDifferentialTemperatureLoads as it is input over several lines
                    for (int j = 0; j < loadIndexes.Count() - 1; j++)
                    {
                        barDifferntialTemperatureSets.Add(new List<string>());
                        for (int k = loadIndexes[j]; k < loadIndexes[j + 1]; k++)
                        {
                            barDifferntialTemperatureSets[j].Add(String.Join(",", barDifferentialTemperatureLoadText[k].Split(',').ToList()));
                        }
                    }
                    if (barDifferntialTemperatureSets.Count != 0)
                    {
                        List<Bar> bhomBars = ReadBars();
                        Dictionary<string, Bar> barDictionary = bhomBars.ToDictionary(x => x.AdapterId<string>(typeof(MidasCivilId)));
                        for (int j = 0; j < barDifferntialTemperatureSets.Count; j++)
                        {
                            BarDifferentialTemperatureLoad bhomBarDifferentialTemperatureLoad =
                                Adapters.MidasCivil.Convert.ToBarDifferentialTemperatureLoad(
                                     barDifferntialTemperatureSets[j], loadcase, loadcaseDictionary, barDictionary, i, m_temperatureUnit, barID[j]);
                            if (bhomBarDifferentialTemperatureLoad != null)
                                bhomBarDifferentialTemperatureLoads.Add(bhomBarDifferentialTemperatureLoad);
                        }
                    }
                }
            }
            return bhomBarDifferentialTemperatureLoads;
        }
        /***************************************************/
    }
}
