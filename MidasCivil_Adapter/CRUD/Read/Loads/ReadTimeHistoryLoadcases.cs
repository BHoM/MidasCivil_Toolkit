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
        public async Task<List<ICase>> ReadTimeHistoryLoadcases(List<string> loadcaseIds = null)
        {
            List<ICase> bhomLoadCases = new List<ICase>();
            var response = await SendRequestAsync("db/THIS", HttpMethod.Get, "").ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                return bhomLoadCases;

            string json = await response.Content.ReadAsStringAsync();
            if (json.StartsWith("{\"message\":"))
                return bhomLoadCases;

            var thisDict = Engine.Serialiser.Convert.FromJson(json)?.PropertyValue("CustomData")?.PropertyValue("THIS")?.PropertyValue("CustomData") as Dictionary<string, object>;

            if (thisDict == null)
            {
                Compute.RecordError("Unable to read time history loadcases. Ensure time history loadcases are defined in the model.");
                return bhomLoadCases;
            }

            var foundLoadcases = new HashSet<string>();
            bool hasRequestedList = loadcaseIds != null && loadcaseIds.Count > 0;

            foreach (var entry in thisDict)
            {
                if (!int.TryParse(entry.Key, out int key))
                    continue;

                var common = entry.Value?.PropertyValue("CustomData")?.PropertyValue("COMMON")?.PropertyValue("CustomData") as Dictionary<string, object>;

                if (common == null)
                    continue;

                string name = common.TryGetValue("NAME", out object nameObj) ? nameObj?.ToString() : "Unnamed";
                foundLoadcases.Add(name);
                bool isRequested = hasRequestedList && loadcaseIds.Contains(name);
                bool notTimeHistory = !common.ContainsKey("ENDTIME");

                if (isRequested && notTimeHistory)
                {
                    Compute.RecordWarning($"Skipping '{name}' loadcase because it is a static time history loadcase with no time steps.");
                    continue;
                }

                if ((hasRequestedList && !isRequested) || notTimeHistory)
                    continue;

                bhomLoadCases.Add(Adapters.MidasCivil.Convert.ToTimeHistoryLoadcase(key, common));
            }

            if (hasRequestedList)
            {
                foreach (var missing in loadcaseIds.Except(foundLoadcases))
                {
                    Compute.RecordWarning($"Loadcase '{missing}' was not found in the model definition.");
                }
            }

            return bhomLoadCases;
        }
    }
}