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
using BH.oM.Geometry;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static BarVaryingDistributedLoad ToBarVaryingDistributedLoad(string barVaryingDistributedLoad, List<string> associatedBars, string loadcase,
            Dictionary<string, Loadcase> loadcaseDictionary, Dictionary<string, Bar> barDictionary, int count, string forceUnit, string lengthUnit)
        {
            string[] delimitted = barVaryingDistributedLoad.Split(',');
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

            double XStartLoad = 0;
            double YStartLoad = 0;
            double ZStartLoad = 0;
            double XEndLoad = 0;
            double YEndLoad = 0;
            double ZEndLoad = 0;

            double startLoad;
            double endLoad;
            if (loadType == "UNILOAD")
            {
                startLoad = double.Parse(delimitted[10].Trim()).ForcePerLengthToSI(forceUnit, lengthUnit);
                endLoad = double.Parse(delimitted[12].Trim()).ForcePerLengthToSI(forceUnit, lengthUnit);
            }
            else
            {
                startLoad = double.Parse(delimitted[10].Trim()).MomentPerLengthToSI(forceUnit, lengthUnit);
                endLoad = double.Parse(delimitted[12].Trim()).MomentPerLengthToSI(forceUnit, lengthUnit);
            }

            switch (direction)
            {
                case "X":
                    XStartLoad = startLoad;
                    XEndLoad = endLoad;
                    break;
                case "Y":
                    YStartLoad = startLoad;
                    YEndLoad = endLoad;
                    break;
                case "Z":
                    ZStartLoad = startLoad;
                    ZEndLoad = endLoad;
                    break;
            }

            Vector startLoadVector = new Vector
            {
                X = XStartLoad,
                Y = YStartLoad,
                Z = ZStartLoad
            };

            Vector endLoadVector = new Vector
            {
                X = XEndLoad,
                Y = YEndLoad,
                Z = ZEndLoad
            };

            double distA = double.Parse(delimitted[9].Trim());
            double distB = double.Parse(delimitted[11].Trim());

            if (double.Parse(delimitted[13].Trim()) != 0 || double.Parse(delimitted[15].Trim()) != 0)
            {
                Engine.Base.Compute.RecordWarning("BHoM Bar Varying Distributed Load does not support non trapezoidal varying loads");
            }

            string name;

            if (string.IsNullOrWhiteSpace(delimitted[17]))
            {
                name = "VBL" + count;
            }
            else
            {
                name = delimitted[17].Trim();
            }

            BarVaryingDistributedLoad bhomBarVaryingDistributedLoad;

            if (loadType == "UNILOAD")
            {
                bhomBarVaryingDistributedLoad = Engine.Structure.Create.BarVaryingDistributedLoad(
                    bhomLoadcase, bhomAssociatedBars, distA, startLoadVector, null, distB, endLoadVector, null, true, axis, loadProjection, name);
            }
            else
            {
                bhomBarVaryingDistributedLoad = Engine.Structure.Create.BarVaryingDistributedLoad(
                    bhomLoadcase, bhomAssociatedBars, distA, null, startLoadVector, distB, null, endLoadVector, true, axis, loadProjection, name);
            }

            //Return null if load failed to be created
            if (bhomBarVaryingDistributedLoad == null)
                return null;

            //Set name as adapter id
            bhomBarVaryingDistributedLoad.SetAdapterId(typeof(MidasCivilId), bhomBarVaryingDistributedLoad.Name);

            return bhomBarVaryingDistributedLoad;
        }

        /***************************************************/

    }
}





