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

using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using BH.Engine.Base;
using System;
using System.Collections;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public async Task<Dictionary<string, double>> GetTimeHistoryInfo(List<string> loadcaseIds)
        {
            if (loadcaseIds == null || loadcaseIds.Count == 0)
            {
                Engine.Base.Compute.RecordError("No loadcase IDs provided.");
                return null;
            }

            var response = await SendRequestAsync("db/THIS", HttpMethod.Get, "").ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                Engine.Base.Compute.RecordError("Request failed.");
                return null;
            }

            string json = await response.Content.ReadAsStringAsync();

            if (json.StartsWith("{\"message\":"))
            {
                Engine.Base.Compute.RecordError("No THIS data found.");
                return null;
            }

            object parsed = Engine.Serialiser.Convert.FromJson(json);

            // DEBUG: Kolla om CustomData finns
            var customData = parsed.PropertyValue("CustomData");
            Engine.Base.Compute.RecordNote($"CustomData exists: {customData != null}");

            // DEBUG: Kolla om THIS finns
            var thisData = customData?.PropertyValue("THIS");
            Engine.Base.Compute.RecordNote($"THIS exists: {thisData != null}");

            // DEBUG: Kolla om THIS.CustomData finns
            var thisDict = thisData?.PropertyValue("CustomData") as Dictionary<string, object>;
            Engine.Base.Compute.RecordNote($"THIS.CustomData exists: {thisDict != null}, Count: {thisDict?.Count ?? 0}");

            if (thisDict == null)
            {
                Engine.Base.Compute.RecordError("JSON missing 'THIS.CustomData' property.");
                return null;
            }

            var result = new Dictionary<string, double>();
            foreach (var entry in thisDict)
            {
                var common = (entry.Value as Dictionary<string, object>)?["CustomData"].PropertyValue("COMMON") as Dictionary<string, object>;
                string name = common?["NAME"]?.ToString();

                if (!loadcaseIds.Contains(name))
                    continue;

                if (common.ContainsKey("ENDTIME") && double.TryParse(common["ENDTIME"].ToString(), out double endTime))
                    result[name] = endTime;
                else
                    Engine.Base.Compute.RecordError($"Loadcase '{name}' has no ENDTIME.");
            }

            return result.Count > 0 ? result : null;
        }
    }
}