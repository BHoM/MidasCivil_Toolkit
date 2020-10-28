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
using BH.oM.Adapters.MidasCivil;
using BH.Engine.Adapter;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static BarPointLoad ToBarPointLoad(string barPointLoad, List<string> associatedBars, string loadcase,
            Dictionary<string, Loadcase> loadcaseDictionary, Dictionary<string, Bar> barDictionary, int count, string forceUnit, string lengthUnit)
        {
            string[] delimitted = barPointLoad.Split(',');
            List<Bar> bhomAssociatedBars = new List<Bar>();

            Loadcase bhomLoadcase;
            loadcaseDictionary.TryGetValue(loadcase, out bhomLoadcase);

            foreach (string associatedBar in associatedBars)
            {
                Bar bhomAssociatedBar;
                barDictionary.TryGetValue(associatedBar, out bhomAssociatedBar);
                bhomAssociatedBars.Add(bhomAssociatedBar);
            }

            string loadType = delimitted[1].Trim();
            string loadAxis = delimitted[2].Trim().Substring(0, 1);
            string direction = delimitted[2].Trim().Substring(1, 1);

            LoadAxis axis = LoadAxis.Global;

            if (loadAxis == "L")
            {
                axis = LoadAxis.Local;
            }

            double load = 0;
            if (loadType == "CONLOAD")
            {
                load = double.Parse(delimitted[10].Trim()).ForceToSI(forceUnit);
            }
            else
            {
                load = double.Parse(delimitted[10].Trim()).MomentToSI(forceUnit, lengthUnit);
            }


            double XLoad = 0;
            double YLoad = 0;
            double ZLoad = 0;

            switch (direction)
            {
                case "X":
                    XLoad = load;
                    break;
                case "Y":
                    YLoad = load;
                    break;
                case "Z":
                    ZLoad = load;
                    break;
            }

            Vector loadVector = new Vector
            {
                X = XLoad,
                Y = YLoad,
                Z = ZLoad
            };

            double distA = double.Parse(delimitted[9].Trim());

            string name;

            if (string.IsNullOrWhiteSpace(delimitted[17]))
            {
                name = "BPL" + count;
            }
            else
            {
                name = delimitted[17].Trim();
            }

            BarPointLoad bhomBarPointLoad;

            if (loadType == "CONLOAD")
            {
                bhomBarPointLoad = Engine.Structure.Create.BarPointLoad(bhomLoadcase, distA, bhomAssociatedBars, loadVector, null, axis, name);
                bhomBarPointLoad.SetAdapterId(typeof(MidasCivilId), bhomBarPointLoad.Name);
            }
            else
            {
                bhomBarPointLoad = Engine.Structure.Create.BarPointLoad(bhomLoadcase, distA, bhomAssociatedBars, null, loadVector, axis, name);
                bhomBarPointLoad.SetAdapterId(typeof(MidasCivilId), bhomBarPointLoad.Name);
            }

            return bhomBarPointLoad;
        }

        /***************************************************/

    }
}

