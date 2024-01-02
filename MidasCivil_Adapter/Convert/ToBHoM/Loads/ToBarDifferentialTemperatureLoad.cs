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
using BH.Engine.Base;
using BH.oM.Structure.SectionProperties;
using System.Linq;
using System.IO;
namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static BarDifferentialTemperatureLoad ToBarDifferentialTemperatureLoad(List<string> temperatureLoad, string loadcase,
            Dictionary<string, Loadcase> loadcaseDictionary, Dictionary<string, Bar> barDictionary, int count, string temperatureUnit, string matchingBar)
        {
            List<double> positions = new List<double>();
            List<double> temperatureList = new List<double>();
            DifferentialTemperatureLoadDirection loadDirection = new DifferentialTemperatureLoadDirection();
            double depth = new double();
            List<Bar> bhomAssociatedBars = new List<Bar>();
            Bar bhomAssociatedBar = new Bar();
            if (matchingBar.Contains(' '))
            {
                string[] matchingBars = matchingBar.Split(' ');
                foreach (string matchBar in matchingBars)
                {
                    barDictionary.TryGetValue(matchBar, out bhomAssociatedBar);
                    bhomAssociatedBars.Add(bhomAssociatedBar);
                }
            }
            else
            {
                barDictionary.TryGetValue(matchingBar, out bhomAssociatedBar);
                bhomAssociatedBars.Add(bhomAssociatedBar);
            }
            for (int j = 1; j < temperatureLoad.Count; j++)
            {
                string[] delimitted = temperatureLoad[j].Split(',');
                double absPositionTop = double.Parse(delimitted[4].Trim());
                double absPositionBot = double.Parse(delimitted[6].Trim());

                if (!positions.Contains(absPositionTop))
                {
                    positions.Add(absPositionTop);
                    temperatureList.Add(double.Parse(delimitted[5].Trim()).DeltaTemperatureToSI(temperatureUnit));
                }
                if (!positions.Contains(absPositionBot))
                {
                    positions.Add(absPositionBot);
                    temperatureList.Add(double.Parse(delimitted[7].Trim()).DeltaTemperatureToSI(temperatureUnit));
                }
            }
            if (temperatureLoad[0].Contains("LY"))
            {
                loadDirection = DifferentialTemperatureLoadDirection.LocalY;
                depth = bhomAssociatedBars[0].SectionProperty.Vpy + bhomAssociatedBars[0].SectionProperty.Vy;
            }
            else
            {
                loadDirection = DifferentialTemperatureLoadDirection.LocalZ;
                depth = bhomAssociatedBars[0].SectionProperty.Vpz + bhomAssociatedBars[0].SectionProperty.Vz;
            }
            List<double> normalisedPositions = new List<double>();

            foreach (double position in positions)
            {
                double normalisedPosition = position / depth;
                if ((1 - normalisedPosition) < 0.02)
                {
                    Compute.RecordWarning("The normalised top position of temperature profile is between 0.981 to 0.999 and has been assumed as 1.");
                    normalisedPosition = 1;
                }
                if ((normalisedPosition) < oM.Geometry.Tolerance.MacroDistance)
                {
                    normalisedPosition = 0;
                }
                normalisedPositions.Add(normalisedPosition);
            }

            Loadcase bhomLoadcase;
            loadcaseDictionary.TryGetValue(loadcase, out bhomLoadcase);
            string name;
            if (string.IsNullOrWhiteSpace(temperatureLoad[0].Split(',')[4]))
            {
                name = "BDL" + count;
            }
            else
            {
                name = temperatureLoad[0].Split(',')[4].Trim();
            }
            if (bhomAssociatedBars.Count() != 0)
            {
                BarDifferentialTemperatureLoad bhombarDifferentialTemperatureLoad = Engine.Structure.Create.BarDifferentialTemperatureLoad(bhomLoadcase, 
                    normalisedPositions, temperatureList, loadDirection, bhomAssociatedBars, name);
                bhombarDifferentialTemperatureLoad.SetAdapterId(typeof(MidasCivilId), bhombarDifferentialTemperatureLoad.Name);
                return bhombarDifferentialTemperatureLoad;
            }
            else
            {
                return null;
            }
        }

        /***************************************************/
    }
}






