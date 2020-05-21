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
using System.IO;
using System.Linq;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<ILoad> ReadAreaUniformlyDistributedLoads(List<string> ids = null)
        {
            List<ILoad> bhomAreaUniformlyDistributedLoads = new List<ILoad>();

            List<Loadcase> bhomLoadcases = ReadLoadcases();
            Dictionary<string, Loadcase> loadcaseDictionary = bhomLoadcases.ToDictionary(
                        x => x.Name);

            string[] loadcaseFolders = Directory.GetDirectories(directory + "\\TextFiles");

            int i = 1;

            foreach (string loadcaseFolder in loadcaseFolders)
            {
                string loadcase = Path.GetFileName(loadcaseFolder);
                List<string> AreaUniformlyDistributedLoadText = GetSectionText(loadcase + "\\PRESSURE");

                if (AreaUniformlyDistributedLoadText.Count != 0)
                {
                    List<string> feMeshComparison = new List<string>();
                    List<string> loadedFEMeshes = new List<string>();

                    foreach (string AreaUniformlyDistributedLoad in AreaUniformlyDistributedLoadText)
                    {
                        List<string> delimitted = AreaUniformlyDistributedLoad.Split(',').ToList();
                        loadedFEMeshes.Add(delimitted[0].Trim());
                        delimitted.RemoveAt(0);
                        feMeshComparison.Add(String.Join(",", delimitted));
                    }

                    if (feMeshComparison.Count != 0)
                    {
                        List<FEMesh> bhomMeshes = ReadFEMeshes();
                        Dictionary<string, FEMesh> FEMeshDictionary = bhomMeshes.ToDictionary(
                                                                    x => x.CustomData[AdapterIdName].ToString());
                        List<string> distinctFEMeshLoads = feMeshComparison.Distinct().ToList();

                        foreach (string distinctFEMeshLoad in distinctFEMeshLoads)
                        {
                            List<int> indexMatches = feMeshComparison.Select((meshload, index) => new { meshload, index })
                                                       .Where(x => string.Equals(x.meshload, distinctFEMeshLoad))
                                                       .Select(x => x.index)
                                                       .ToList();
                            List<string> matchingFEMeshes = new List<string>();
                            indexMatches.ForEach(x => matchingFEMeshes.Add(loadedFEMeshes[x]));

                            AreaUniformlyDistributedLoad bhomAreaUniformlyDistributedLoad =
                                Adapter.Adapters.MidasCivil.Convert.ToAreaUniformlyDistributedLoad(
                                    distinctFEMeshLoad, matchingFEMeshes, loadcase, loadcaseDictionary, FEMeshDictionary, i, forceUnit, lengthUnit);

                            bhomAreaUniformlyDistributedLoads.Add(bhomAreaUniformlyDistributedLoad);

                            if ((string.IsNullOrWhiteSpace(distinctFEMeshLoad.Split(',')[13].ToString())))
                            {
                                i = i + 1;
                            }

                        }

                    }
                }
            }
            return bhomAreaUniformlyDistributedLoads;
        }

    }
}