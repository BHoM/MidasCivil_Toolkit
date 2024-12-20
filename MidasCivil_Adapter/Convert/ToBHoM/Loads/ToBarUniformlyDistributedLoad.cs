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

using System.Collections.Generic;
using BH.oM.Adapters.MidasCivil;
using BH.Engine.Adapter;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using Microsoft.Office.Interop.Excel;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static BarUniformlyDistributedLoad ToBarUniformlyDistributedLoad(string barUniformlyDistributedLoad, List<string> associatedBars,
            string loadcase, Dictionary<string, Loadcase> loadcaseDictionary, Dictionary<string, Bar> barDictionary,
            int count, string forceUnit, string lengthUnit)
        {
            string[] delimitted = barUniformlyDistributedLoad.Split(',');
            List<Bar> bhomAssociatedBars = new List<Bar>();

            Loadcase bhomLoadcase;
            loadcaseDictionary.TryGetValue(loadcase, out bhomLoadcase);

            Bar bhomAssociatedBar;
            foreach (string associatedBar in associatedBars)
            {
                barDictionary.TryGetValue(associatedBar, out bhomAssociatedBar);
                bhomAssociatedBars.Add(bhomAssociatedBar);
            }

            string loadType = delimitted[1].Trim();
            string loadAxis = delimitted[2].Trim().Substring(0, 1);
            string direction = delimitted[2].Trim().Substring(1, 1);
            string projection = delimitted[3].Trim();

            LoadAxis axis = LoadAxis.Global;
            bool loadProjection = false;

            if (loadAxis == "L")
            {
                axis = LoadAxis.Local;
            }

            if (projection == "YES")
            {
                loadProjection = true;
            }

            double XLoad = 0;
            double YLoad = 0;
            double ZLoad = 0;
            double force;
            if (loadType == "UNILOAD")
            {
                force = double.Parse(delimitted[10].Trim()).ForcePerLengthToSI(forceUnit, lengthUnit);
            }
            else
            {
                force = double.Parse(delimitted[10].Trim()).MomentPerLengthToSI(forceUnit, lengthUnit);
            }

            switch (direction)
            {
                case "X":
                    XLoad = force;
                    break;
                case "Y":
                    YLoad = force;
                    break;
                case "Z":
                    ZLoad = force;
                    break;
            }

            Vector loadVector = new Vector
            {
                X = XLoad,
                Y = YLoad,
                Z = ZLoad
            };

            string name;

            if (string.IsNullOrWhiteSpace(delimitted[17]))
            {
                name = "UBL" + count;
            }
            else
            {
                name = delimitted[17].Trim();
            }

            BarUniformlyDistributedLoad bhomBarUniformlyDistributedLoad;

            if (loadType == "UNILOAD")
            {
                bhomBarUniformlyDistributedLoad = Engine.Structure.Create.BarUniformlyDistributedLoad(
                    bhomLoadcase, bhomAssociatedBars, loadVector, null, axis, loadProjection, name);
                bhomBarUniformlyDistributedLoad.SetAdapterId(typeof(MidasCivilId), bhomBarUniformlyDistributedLoad.Name);
            }
            else
            {
                bhomBarUniformlyDistributedLoad = Engine.Structure.Create.BarUniformlyDistributedLoad(
                    bhomLoadcase, bhomAssociatedBars, null, loadVector, axis, loadProjection, name);
                bhomBarUniformlyDistributedLoad.SetAdapterId(typeof(MidasCivilId), bhomBarUniformlyDistributedLoad.Name);
            }

            return bhomBarUniformlyDistributedLoad;
        }

        /***************************************************/

    }
}






