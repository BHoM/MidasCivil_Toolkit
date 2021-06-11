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
using BH.Engine.Reflection;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<ILoad> ReadAreaDifferentialTemperatureLoads(List<string> ids = null)
        {
            List<ILoad> bhomAreaDifferentialTemperatureLoads = new List<ILoad>();

            List<Loadcase> bhomLoadcases = ReadLoadcases();
            Dictionary<string, Loadcase> loadcaseDictionary = bhomLoadcases.ToDictionary(
                        x => x.Name);

            string[] loadcaseFolders = Directory.GetDirectories(m_directory + "\\TextFiles");

            foreach (string loadcaseFolder in loadcaseFolders)
            {
                string loadcase = Path.GetFileName(loadcaseFolder);
                List<string> areaDifferentialTemperatureLoadText = GetSectionText(loadcase + "\\THERGRAD");
                List<string> areaUniformTemperatureLoadText = GetSectionText(loadcase + "\\ELTEMPER");
                List<string> feMeshComparison = new List<string>();
                List<string> loadedFEMeshes = new List<string>();

                if (areaDifferentialTemperatureLoadText.Count != 0)
                {
                    List<string> areaDifferentialTempatureElements = areaDifferentialTemperatureLoadText.Select(x => x.Split(',')[0]).ToList();
                    List<string> areaUniformTempatureElements = areaUniformTemperatureLoadText.Select(x => x.Split(',')[0]).ToList();
                    for (int i = 0; i < areaDifferentialTempatureElements.Count; i++)
                    {
                        List<string> differentialDelimitted = areaDifferentialTemperatureLoadText[i].Split(',').ToList();
                        loadedFEMeshes.Add(areaDifferentialTempatureElements[i].Trim());
                        differentialDelimitted.RemoveAt(0);
                        if (areaUniformTempatureElements.Count != 0)
                        {
                            if (areaDifferentialTempatureElements[i] == areaUniformTempatureElements[i])
                            {
                                List<string> globalDelimitted = areaUniformTemperatureLoadText[i].Split(',').ToList();
                                //When an area uniform temp offset load is detected,  it is added as an additional string to the area diff. temp list of string.
                                differentialDelimitted.Add(globalDelimitted[1].Trim());
                            }
                            else
                            {

                                Compute.RecordWarning("No Area Uniform Temperature Load is integrated as part of the Area Differential Temperature Load at Element number "
                                    + areaDifferentialTempatureElements[i].Trim().ToString() + " \n due to the two loads are assigned to different element." +
                                " \n Area Differential Temperature load will be applied at the centroid of the cross section at Element number "
                                + areaDifferentialTempatureElements[i].Trim().ToString());
                                differentialDelimitted.Add(string.Format("0"));
                            }
                        }
                        else
                        {
                            Compute.RecordWarning("No Area Uniform Temperature Load is detected at Element number " 
                            + areaDifferentialTempatureElements[i].Trim().ToString()+
                            ". \n Area Differential Temperature load will be applied at the centroid of the cross section at Element number" 
                            + areaDifferentialTempatureElements[i].Trim().ToString());
                            differentialDelimitted.Add(string.Format("0"));
                        }
                        feMeshComparison.Add(String.Join(",", differentialDelimitted));
                    }
                    if (feMeshComparison.Count != 0)
                    {
                        List<FEMesh> bhomMeshes = ReadFEMeshes();
                        Dictionary<string, FEMesh> feMeshDictionary = bhomMeshes.ToDictionary(
                                                                    x => x.AdapterId<string>(typeof(MidasCivilId)));
                        List<string> distinctFEMeshLoads = feMeshComparison.Distinct().ToList();
                        int i = 1;
                        foreach (string distinctFEMeshLoad in distinctFEMeshLoads)
                        {
                            List<int> indexMatches = feMeshComparison.Select((meshload, index) => new { meshload, index })
                                                       .Where(x => string.Equals(x.meshload, distinctFEMeshLoad))
                                                       .Select(x => x.index)
                                                       .ToList();
                            List<string> matchingFEMeshes = new List<string>();
                            indexMatches.ForEach(x => matchingFEMeshes.Add(loadedFEMeshes[x]));
                            AreaDifferentialTemperatureLoad bhomAreaDifferentialTemperatureLoad =
                                Adapters.MidasCivil.Convert.ToAreaDifferentialTemperatureLoad(
                                    distinctFEMeshLoad, matchingFEMeshes, loadcase, loadcaseDictionary, feMeshDictionary, i, m_temperatureUnit);

                            if (bhomAreaDifferentialTemperatureLoad != null)
                                bhomAreaDifferentialTemperatureLoads.Add(bhomAreaDifferentialTemperatureLoad);

                            if ((string.IsNullOrWhiteSpace(distinctFEMeshLoad.Split(',')[1].ToString())))
                            {
                                i = i + 1;
                            }
                        }

                    }
                }
            }
            return bhomAreaDifferentialTemperatureLoads;
        }
    }
}
