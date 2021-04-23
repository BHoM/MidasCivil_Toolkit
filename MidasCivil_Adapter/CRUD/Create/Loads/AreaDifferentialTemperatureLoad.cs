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

using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using System.Collections.Generic;
using System.IO;
using BH.Engine.Adapter;
using BH.oM.Adapters.MidasCivil;
using BH.Engine.Reflection;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public bool CreateCollection(IEnumerable<AreaDifferentialTemperatureLoad> areaDifferentialTemperatureLoads)
        {
            string loadGroupPath = CreateSectionFile("LOAD-GROUP");

            foreach (AreaDifferentialTemperatureLoad areaDifferentialTemperatureLoad in areaDifferentialTemperatureLoads)
            {
                List<string> midasTemperatureLoads = new List<string>();
                string FEMeshLoadPath = CreateSectionFile(areaDifferentialTemperatureLoad.Loadcase.Name + "\\THERGRAD");
                string midasLoadGroup = Adapters.MidasCivil.Convert.FromLoadGroup(areaDifferentialTemperatureLoad);
                List<IAreaElement> assignedElements = areaDifferentialTemperatureLoad.Objects.Elements;
                List<string> assignedFEMeshes = new List<string>();
                AreaUniformTemperatureLoad areaUniformTemperatureLoad = new AreaUniformTemperatureLoad();
                areaUniformTemperatureLoad.TemperatureChange = (areaDifferentialTemperatureLoad.TemperatureProfile[0] - areaDifferentialTemperatureLoad.TemperatureProfile[1]) / 2 + areaDifferentialTemperatureLoad.TemperatureProfile[1];
                areaUniformTemperatureLoad.Name = areaDifferentialTemperatureLoad.Name;
                areaUniformTemperatureLoad.Objects = areaDifferentialTemperatureLoad.Objects;
                areaUniformTemperatureLoad.Loadcase = areaDifferentialTemperatureLoad.Loadcase;
                Compute.RecordWarning("Given the limitation of Midas, an Area Differential Temperature Load is inputted as one Element Temperature Load and one Temperature Gradient Load");
                Compute.RecordWarning("Please ensure a separate Loadcase is used if user would like to push any Area Uniform Temperature Load in addition to Area Differential Temperature Load");
                CreateCollection(new List<AreaUniformTemperatureLoad>() { areaUniformTemperatureLoad });

                foreach (IAreaElement mesh in assignedElements)
                {
                    assignedFEMeshes.Add(mesh.AdapterId<string>(typeof(MidasCivilId)));
                }

                foreach (string assignedFEMesh in assignedFEMeshes)
                {
                    midasTemperatureLoads.Add(Adapters.MidasCivil.Convert.FromAreaDifferentialTemperatureLoad(areaDifferentialTemperatureLoad, assignedFEMesh, m_temperatureUnit));
                }
                CompareLoadGroup(midasLoadGroup, loadGroupPath);
                RemoveEndOfDataString(FEMeshLoadPath);
                File.AppendAllLines(FEMeshLoadPath, midasTemperatureLoads);
            }

            return true;
        }
    }
}
