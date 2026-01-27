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
using System.Linq;
using BH.Engine.Base;
using System;
using System.Collections;
using BH.oM.Structure.Loads;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public async Task<List<TimeHistoryLoadcase>> ReadTimeHistoryLoadcases(List<string> loadcaseIds = null)
        {
            var bhomLoadCases = new List<TimeHistoryLoadcase>();

            var response = await SendRequestAsync("db/THIS", HttpMethod.Get, "").ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                Engine.Base.Compute.RecordError("Unable to read time history loadcases, please ensure the connected model is solved and check for errors in the MidasCivil window.");
                return bhomLoadCases;
            }

            string json = await response.Content.ReadAsStringAsync();
            if (json.StartsWith("{\"message\":"))
                return bhomLoadCases;   

            var rootData = Engine.Serialiser.Convert.FromJson(json)
                .PropertyValue("CustomData") as Dictionary<string, object>;

            if (!(rootData?.TryGetValue("THIS", out object thisObj) ?? false))
            {
                return bhomLoadCases;
            }

            var thisDict = thisObj.PropertyValue("CustomData") as Dictionary<string, object>;
            if (thisDict == null)
            {
                Engine.Base.Compute.RecordError("Unable to read time history loadcases. Ensure that time history loadcases are defined in the model.");
                return bhomLoadCases;
            }

            foreach (var entry in thisDict)
            {
                if (!int.TryParse(entry.Key, out int key))
                    continue;

                var common = entry.Value.PropertyValue("CustomData")?.PropertyValue("COMMON")?.PropertyValue("CustomData") as Dictionary<string, object>;

                if (common == null)
                    continue;

                string name = common.TryGetValue("NAME", out object nameObj) ? nameObj?.ToString() : "Unnamed";

                bool hasRequestedList = loadcaseIds != null && loadcaseIds.Count > 0;
                bool isRequested = hasRequestedList && loadcaseIds.Contains(name);
                bool notTimeHistory = !common.ContainsKey("ENDTIME");

                if (isRequested && notTimeHistory)
                {
                    Engine.Base.Compute.RecordNote(
                        $"Skipping '{name}' loadcase because it is not defined as a time history loadcase with timesteps."
                    );
                    continue;
                }

                if ((hasRequestedList && !isRequested) || notTimeHistory)
                    continue;

                bhomLoadCases.Add(Adapters.MidasCivil.Convert.ToTimeHistoryLoadcase(key, common));
            }

            return bhomLoadCases;
        }

    }
}