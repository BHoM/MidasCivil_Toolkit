/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2026, the respective contributors. All rights reserved.
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
using BH.Engine.Adapter;
using BH.oM.Adapters.MidasCivil;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static TimeHistoryLoadcase ToTimeHistoryLoadcase(int key, Dictionary<string, object> common)
        {
            TimeHistoryLoadcase timeHistoryLoadcase = new TimeHistoryLoadcase
            {
                Name = common["NAME"]?.ToString(),
                Number = key,
                StartTime = 0,
                EndTime = System.Convert.ToDouble(common["ENDTIME"]),
                TimeStep = System.Convert.ToDouble(common["INC"]) * System.Convert.ToDouble(common["iOUT"]),
            };

            return timeHistoryLoadcase;

        }
    }
}

