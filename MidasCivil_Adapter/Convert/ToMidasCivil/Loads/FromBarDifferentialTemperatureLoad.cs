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
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.Loads;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static List <string> FromBarDifferentialTemperatureLoad(this BarDifferentialTemperatureLoad load, string assignedBar, string temperatureUnit)
        {
            List <string> midasBarLoad = null;

            List<Bar> bars = load.Objects.Elements;

            var barGroups = bars.GroupBy(x => x.SectionProperty);

            foreach(List<Bar> barGroup in barGroups)
            {
                ISectionProperty sectionProperty = barGroup.First().SectionProperty;

                double width = sectionProperty.Vpy + sectionProperty.Vy;
                double Presetwidth = width / 2;
                double H1 = new double();
                double H2 = new double();
                double depth = sectionProperty.Vpz + sectionProperty.Vz;
                if (load.LoadDirection == DifferentialTemperatureLoadDirection.LocalY)
                {
                    midasBarLoad.Add(assignedBar + "," + "LY" + ",Bot , ," + "No");
                }
                else
                {
                    midasBarLoad.Add(assignedBar + "," + "LZ" + ",Bot , ," + "No");
                }
                    for (int i = 1; i < load.TemperatureProfile.Keys.Count; i++)
                {
                    H1 = depth * load.TemperatureProfile.Keys.ElementAt(i-1);
                    H2 = depth * load.TemperatureProfile.Keys.ElementAt(i);
                    
                    midasBarLoad.Add(load.Name + ",0,0," + Presetwidth + "," + H1 + "," + load.TemperatureProfile.Values.ElementAt(i-1) + "," + H2 + "," + load.TemperatureProfile.Values.ElementAt(i));

                }

            }
            return midasBarLoad;
        }

        /***************************************************/

    }
}