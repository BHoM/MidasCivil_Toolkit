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

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public bool CreateCollection(IEnumerable<AreaDifferentialTemperatureLoad> areaDifferentialTemperatureLoads)
        {
            string loadGroupPath = CreateSectionFile("LOAD-GROUP");

            foreach (AreaDifferentialTemperatureLoad areaDifferentialTemperatureLoad in areaDifferentialTemperatureLoads)
            {
                List<string> midasTemperatureLoadsP1 = new List<string>();
                List<string> midasTemperatureLoadsP2 = new List<string>();
                string FEMeshLoadPathP1 = CreateSectionFile(areaDifferentialTemperatureLoad.Loadcase.Name + "\\THERGRAD");
                string FEMeshLoadPathP2 = CreateSectionFile(areaDifferentialTemperatureLoad.Loadcase.Name + "\\ELTEMPER");
                string midasLoadGroup = Adapters.MidasCivil.Convert.FromLoadGroup(areaDifferentialTemperatureLoad);
                List<IAreaElement> assignedElements = areaDifferentialTemperatureLoad.Objects.Elements;
                List<string> assignedFEMeshes = new List<string>();
                AreaUniformTemperatureLoad areaUniformTemperatureLoad = new AreaUniformTemperatureLoad();
                areaUniformTemperatureLoad.TemperatureChange = (areaDifferentialTemperatureLoad.TemperatureProfile[0] - areaDifferentialTemperatureLoad.TemperatureProfile[1]) / 2;
                areaUniformTemperatureLoad.Name = areaDifferentialTemperatureLoad.Name;
                foreach (IAreaElement mesh in assignedElements)
                {
                    assignedFEMeshes.Add(mesh.AdapterId<string>(typeof(MidasCivilId)));
                }

                foreach (string assignedFEMesh in assignedFEMeshes)
                {
                    midasTemperatureLoadsP1.Add(Adapters.MidasCivil.Convert.FromAreaDifferentialTemperatureLoad(areaDifferentialTemperatureLoad, assignedFEMesh, m_temperatureUnit));
                    midasTemperatureLoadsP2.Add(Adapters.MidasCivil.Convert.FromAreaUniformTemperatureLoad(areaUniformTemperatureLoad, assignedFEMesh, m_temperatureUnit));
                }
                CompareLoadGroup(midasLoadGroup, loadGroupPath);
                RemoveEndOfDataString(FEMeshLoadPathP1);
                File.AppendAllLines(FEMeshLoadPathP1, midasTemperatureLoadsP1);
                RemoveEndOfDataString(FEMeshLoadPathP2);
                File.AppendAllLines(FEMeshLoadPathP2, midasTemperatureLoadsP2);

            }

            return true;
        }
    }
}
