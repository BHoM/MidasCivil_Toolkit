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
        public static BarTemperatureLoad ToBarTemperatureLoad(string temperatureLoad, List<string> associatedFEMeshes, string loadcase,
            Dictionary<string, Loadcase> loadcaseDictionary, Dictionary<string, Bar> barDictionary, int count, string temperatureUnit)
        {
            /***************************************************/
            /**** Public Methods                            ****/
            /***************************************************/

            string[] delimitted = temperatureLoad.Split(',');
            List<Bar> bhomAssociatedBars = new List<Bar>();

            Loadcase bhomLoadcase;
            loadcaseDictionary.TryGetValue(loadcase, out bhomLoadcase);

            Bar bhomAssociatedBar;
            foreach (string associatedFEMesh in associatedFEMeshes)
            {
                if (barDictionary.ContainsKey(associatedFEMesh))
                {
                    barDictionary.TryGetValue(associatedFEMesh, out bhomAssociatedBar);
                    bhomAssociatedBars.Add(bhomAssociatedBar);
                }
            }

            double temperature = double.Parse(delimitted[0].Trim()).DeltaTemperatureToSI(temperatureUnit);

            string name;

            if (string.IsNullOrWhiteSpace(delimitted[1]))
            {
                name = "ATL" + count;
            }
            else
            {
                name = delimitted[1].Trim();
            }

            if (bhomAssociatedBars.Count != 0)
            {
                BarTemperatureLoad bhombarUniformlyDistributedLoad = Engine.Structure.Create.BarTemperatureLoad(bhomLoadcase, temperature, bhomAssociatedBars, LoadAxis.Global, false, name);
                bhombarUniformlyDistributedLoad.CustomData[AdapterIdName] = bhombarUniformlyDistributedLoad.Name;
                return bhombarUniformlyDistributedLoad;
            }
            else
            {
                return null;
            }
        }

        /***************************************************/

    }
}

