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

using System.Collections.Generic;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static AreaTemperatureLoad ToAreaTemperatureLoad(string temperatureLoad, List<string> associatedFEMeshes, string loadcase,
            Dictionary<string, Loadcase> loadcaseDictionary, Dictionary<string, FEMesh> femeshDictionary, int count, string temperatureUnit)
        {
            string[] delimitted = temperatureLoad.Split(',');
            List<FEMesh> bhomAssociatedFEMeshes = new List<FEMesh>();

            Loadcase bhomLoadcase;
            loadcaseDictionary.TryGetValue(loadcase, out bhomLoadcase);

            foreach (string associatedFEMesh in associatedFEMeshes)
            {
                if (femeshDictionary.ContainsKey(associatedFEMesh))
                {
                    FEMesh bhomAssociatedFEMesh;
                    femeshDictionary.TryGetValue(associatedFEMesh, out bhomAssociatedFEMesh);
                    bhomAssociatedFEMeshes.Add(bhomAssociatedFEMesh);
                }
            }

            double temperature = double.Parse(delimitted[0].Trim());

            string name;

            if (string.IsNullOrWhiteSpace(delimitted[1]))
            {
                name = "ATL" + count;
            }
            else
            {
                name = delimitted[1].Trim();
            }

            if (bhomAssociatedFEMeshes.Count != 0)
            {
                AreaTemperatureLoad bhomAreaUniformlyDistributedLoad = Engine.Structure.Create.AreaTemperatureLoad(bhomLoadcase, temperature.DeltaTemperatureToSI(temperatureUnit),
                    bhomAssociatedFEMeshes, LoadAxis.Global, false, name);
                bhomAreaUniformlyDistributedLoad.CustomData[AdapterIdName] = bhomAreaUniformlyDistributedLoad.Name;
                return bhomAreaUniformlyDistributedLoad;
            }
            else
            {
                return null;
            }
        }

        /***************************************************/

    }
}

