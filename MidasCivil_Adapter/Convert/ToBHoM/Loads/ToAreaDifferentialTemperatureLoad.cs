/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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

using System.Collections.Generic;
using BH.oM.Adapters.MidasCivil;
using BH.Engine.Adapter;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static AreaDifferentialTemperatureLoad ToAreaDifferentialTemperatureLoad(string temperatureLoad, List<string> associatedFEMeshes, string loadcase,
            Dictionary<string, Loadcase> loadcaseDictionary, Dictionary<string, FEMesh> feMeshDictionary, int count, string temperatureUnit)
        {
            List<string> delimitted = new List<string>(temperatureLoad.Split(','));
            List<FEMesh> bhomAssociatedFEMeshes = new List<FEMesh>();
            Loadcase bhomLoadcase;
            loadcaseDictionary.TryGetValue(loadcase, out bhomLoadcase);
            foreach (string associatedFEMesh in associatedFEMeshes)
            {
                if (feMeshDictionary.ContainsKey(associatedFEMesh))
                {
                    FEMesh bhomAssociatedFEMesh;
                    feMeshDictionary.TryGetValue(associatedFEMesh, out bhomAssociatedFEMesh);
                    bhomAssociatedFEMeshes.Add(bhomAssociatedFEMesh);
                }
            }
            double temperature = double.Parse(delimitted[1].Trim());
            double temperatureOffset = double.Parse(delimitted[5].Trim());
            double topTemperature = temperature / 2 + temperatureOffset;
            double botTemperature = -temperature / 2 + temperatureOffset;
            string name;
            if (string.IsNullOrWhiteSpace(delimitted[4]))
            {
                name = "ADTL" + count;
            }
            else
            {
                name = delimitted[4].Trim();
            }
            if (bhomAssociatedFEMeshes.Count != 0)
            {
                AreaDifferentialTemperatureLoad bhomAreaDifferentialTemperatureLoad = Engine.Structure.Create.AreaDifferentialTemperatureLoad(
                    bhomLoadcase, topTemperature.DeltaTemperatureToSI(temperatureUnit), botTemperature.DeltaTemperatureToSI(temperatureUnit),
                    bhomAssociatedFEMeshes, name);
                bhomAreaDifferentialTemperatureLoad.SetAdapterId(typeof(MidasCivilId), bhomAreaDifferentialTemperatureLoad.Name);
                return bhomAreaDifferentialTemperatureLoad;
            }
            else
            {
                return null;
            }
        }

        /***************************************************/

    }
}





