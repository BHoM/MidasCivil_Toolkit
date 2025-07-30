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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {

        private static Dictionary<(int Id, string Case, string Position), List<List<string>>> GroupTHResult(List<List<object>> data, bool usePosition)
        {
            var groupedData = new ConcurrentDictionary<(int, string, string), List<List<string>>>();

            Parallel.ForEach(data, item =>
            {
                int id = int.TryParse(item[1]?.ToString(), out var parsedId) ? parsedId : -1;
                string caseVal = item[2]?.ToString() ?? "";
                string position = usePosition ? item[4]?.ToString() ?? "" : "";

                var key = (id, caseVal, position);
                var stringRow = item.Select(x => x?.ToString() ?? "").ToList();

                groupedData.AddOrUpdate(
                    key,
                    _ => new List<List<string>> { stringRow },
                    (_, existingList) =>
                    {
                        lock (existingList)
                        {
                            existingList.Add(stringRow);
                            return existingList;
                        }
                    });
            });

            return new Dictionary<(int, string, string), List<List<string>>>(groupedData);
        }
    }
}
