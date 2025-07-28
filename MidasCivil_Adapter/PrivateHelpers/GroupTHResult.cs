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

using BH.Engine.Base;
using BH.oM.Structure.Requests;
using System;
using System.Collections.Generic;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {

        private static Dictionary<(object Id, object Case, object Position), List<List<object>>> GroupTHResult(List<List<object>> data)
        {
            var groupedData = new Dictionary<(object, object, object), List<List<object>>>();

            foreach (var item in data)
            {
                var key = (item[1], item[2], item[4]); // Grouping by Id, Case, and Position

                if (!groupedData.ContainsKey(key))
                {
                    groupedData[key] = new List<List<object>>();
                }

                groupedData[key].Add(item);
            }

            return groupedData;
        }
    }
}
