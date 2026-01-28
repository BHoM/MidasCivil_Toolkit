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

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BH.Engine.Base;
using BH.oM.Structure.Loads;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public async Task<List<int>> getGeneralLink(List<int> objectIds = null)
        {
            List<int> ids = new List<int>();
            var response = await SendRequestAsync("db/NLNK", HttpMethod.Get, "").ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                return ids;

            string json = await response.Content.ReadAsStringAsync();
            if (json.StartsWith("{\"message\":"))
                return ids;

            var thisDict = Engine.Serialiser.Convert.FromJson(json)?.PropertyValue("CustomData")?.PropertyValue("NLNK")?.PropertyValue("CustomData") as Dictionary<string, object>;

            if (thisDict == null)
            {
                Compute.RecordError("Unable to read general links. Ensure that general links are defined in the model.");
                return ids;
            }

            bool filterId = objectIds != null && objectIds.Count > 0;

            foreach (var entry in thisDict)
            {
                if (int.TryParse(entry.Key, out int key))
                {
                    if (!filterId || objectIds.Contains(key))
                        ids.Add(key);
                }
            }

            if (filterId)
            {
                var missing = objectIds.Except(ids).ToList();
                foreach (var id in missing)
                {
                    Compute.RecordWarning($"General link '{id}' was not found in the model definition.");
                }
            }

            return ids;
        }
    }
}