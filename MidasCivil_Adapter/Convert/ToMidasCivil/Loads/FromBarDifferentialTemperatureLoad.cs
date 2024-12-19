/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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

        public static List<string> FromBarDifferentialTemperatureLoad(this BarDifferentialTemperatureLoad load, string ids, ISectionProperty sectionProperty, string temperatureUnit, string lengthUnit)
        {
            string loadDirection = "";
            double depth = sectionProperty.Vpz + sectionProperty.Vz;
            switch (load.LoadDirection)
            {
                case DifferentialTemperatureLoadDirection.LocalY:
                    loadDirection = "LY";
                    depth = sectionProperty.Vpy + sectionProperty.Vy;
                    break;
                case DifferentialTemperatureLoadDirection.LocalZ:
                    loadDirection = "LZ";
                    break;
            }
            double presetWidth = sectionProperty.Area / depth;
            double temperatureProfileCount = load.TemperatureProfile.Keys.Count - 1;
            string firstLine = ids.Trim() + "," + loadDirection + ",Bot ," + temperatureProfileCount + "," + load.Name + "," + "No";
            List<string> midasBarLoad = new List<string>();
            midasBarLoad.Add(firstLine);
            for (int i = 1; i < load.TemperatureProfile.Keys.Count; i++)
            {
                double bottomTemperature = depth * load.TemperatureProfile.Keys.ElementAt(i - 1).LengthFromSI(lengthUnit);
                double topTemperature = depth * load.TemperatureProfile.Keys.ElementAt(i).LengthFromSI(lengthUnit);
                //nLine represents the temperature at layer n 
                string nLine = "ELEMENT" + ",0,0," + presetWidth.LengthFromSI(lengthUnit) + "," + bottomTemperature + "," + load.TemperatureProfile.Values.ElementAt(i - 1).DeltaTemperatureFromSI(temperatureUnit) + "," +
                    topTemperature + "," + load.TemperatureProfile.Values.ElementAt(i).DeltaTemperatureFromSI(temperatureUnit);
                midasBarLoad.Add(nLine);
            }
            return midasBarLoad;
        }
        /***************************************************/
    }
}




