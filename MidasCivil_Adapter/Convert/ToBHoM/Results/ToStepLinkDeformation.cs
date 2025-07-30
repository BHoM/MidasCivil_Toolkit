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

using BH.oM.Structure.Loads;
using BH.oM.Structure.Results;
using System.Linq;
using System.Collections.Generic;
using BH.Adapter.Adapters.MidasCivil;
using System.Text.Json;
using System;
using BH.oM.Analytical.Results;

namespace BH.Adapter.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/
        public static TimeHistoryLinkDeformation ToStepLinkDeformation(KeyValuePair<(int Id, string Case, string Position), List<List<string>>> dataGroup)
        {
            int id = dataGroup.Key.Id;
            IComparable loadCase = dataGroup.Key.Case;
            int mode = -1;
            string position = dataGroup.Key.Position;

            List<StepLinkDeformation> stepLinkForces = dataGroup.Value
                .AsParallel()
                .Select(ToStepLinkDeformation)
                .ToList();

            return new TimeHistoryLinkDeformation(id, loadCase, mode, position, stepLinkForces);
        }

        public static StepLinkDeformation ToStepLinkDeformation(List<string> dataItem)
        {
            double ParseDouble(string s) => double.TryParse(s, out var result) ? result : 0.0;

            return new StepLinkDeformation(
                ParseDouble(dataItem[3]),
                ParseDouble(dataItem[4]),
                ParseDouble(dataItem[5]),
                ParseDouble(dataItem[6]),
                ParseDouble(dataItem[7]),
                ParseDouble(dataItem[8]),
                ParseDouble(dataItem[9])
            );
        }

    }
}
