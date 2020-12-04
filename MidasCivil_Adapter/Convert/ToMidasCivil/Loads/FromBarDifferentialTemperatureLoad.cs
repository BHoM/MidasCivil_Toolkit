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
            List <string> midasBarLoad = new List<string>() ;

            var barGroups = load.Objects.Elements.GroupBy(x => x.SectionProperty);
                if (load.TemperatureProfile.Keys.Count > 20)
                {
                Engine.Reflection.Compute.RecordWarning("There is a maximium of 20 layers in the temperature profile");
                    return null;
                }
            var barGroup = barGroups.First();

            //foreach (var barGroup in barGroups)
            //{
                if (barGroup.First().SectionProperty == null)
                {
                    Engine.Reflection.Compute.RecordWarning("Section Property is required for inputting differential temperature load");
                    return null;
                }
                ISectionProperty sectionProperty = barGroup.First().SectionProperty;

                double presetWidth = sectionProperty.Vpy + sectionProperty.Vy / 2;
                double bottomTemperature = new double();
                double topTemperature = new double();
                double depth = sectionProperty.Vpz + sectionProperty.Vz;
                string firstLine;
                double Reference = load.TemperatureProfile.Keys.Count - 1;

                string loadDirection = "";

                switch (load.LoadDirection)
                {
                    case DifferentialTemperatureLoadDirection.LocalY:
                        loadDirection = "LY";
                        break;
                    case DifferentialTemperatureLoadDirection.LocalZ:
                        loadDirection = "LZ";
                        break;
                }


                firstLine = assignedBar + "," + loadDirection + ",Bot ," + Reference + ", ," + "No";
                midasBarLoad.Add(firstLine);

                string nLine;

                for (int i = 1; i < load.TemperatureProfile.Keys.Count; i++)
                {
                    bottomTemperature = depth * load.TemperatureProfile.Keys.ElementAt(i-1);
                    topTemperature = depth * load.TemperatureProfile.Keys.ElementAt(i);
                    nLine = "ELEMENT" + ",0,0," + presetWidth + "," + bottomTemperature + "," + load.TemperatureProfile.Values.ElementAt(i-1) + "," + topTemperature + "," + load.TemperatureProfile.Values.ElementAt(i);
                    midasBarLoad.Add(nLine);
                }
               
            //}
            return midasBarLoad;
        }

        /***************************************************/

    }
}
