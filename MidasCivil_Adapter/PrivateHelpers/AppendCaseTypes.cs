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
using System;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Generic;
using BH.oM.Data.Requests;
using System.Text.Json;
using System.Collections;
using BH.oM.Structure.Loads;
using Microsoft.Office.Interop.Excel;
using BH.oM.Analytical.Results;
using BH.Engine.Base;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private async Task<List<string>> AppendCaseTypes(IResultRequest request)
        {
            List<string> caseNames = new List<string>();

            if (request.Cases.Count == 0)
                return caseNames;
            else
            { 
                List<string> requestNames = GetLoadcaseIDs(request);

                Task<HttpResponseMessage> taskComb = SendRequestAsync("db/LCOM-GEN", HttpMethod.Get, "");
                Task<HttpResponseMessage> taskCase = SendRequestAsync("db/STLD", HttpMethod.Get, "");

                var responses = await Task.WhenAll(taskComb, taskCase).ConfigureAwait(false);

                if (!responses[0].IsSuccessStatusCode || !responses[1].IsSuccessStatusCode)
                {
                    Engine.Base.Compute.RecordError($"The connection to the Midas Civil model failed. Please try to reconnect.");
                    return null;
                }

                caseNames.AddRange(GetCaseName(await responses[0].Content.ReadAsStringAsync(), requestNames, "LCOM-GEN", "(CB)"));
                caseNames.AddRange(GetCaseName(await responses[1].Content.ReadAsStringAsync(), requestNames, "STLD", "(ST)"));

                if (caseNames.Count==0)
                    Engine.Base.Compute.RecordWarning($"No matching Loadcase or Loadcombination could be found in the model, the request will be sent without a Case filter. Please make sure to use the full name of each Case with the same formatting used in the Midas Civil model.");

                return caseNames;
            }
        }

        private List<string> GetCaseName(string response, List<string> filters, string location, string ending)
        {
            List<string> filteredNames = new List<string>();

            object parsedResponse = Engine.Serialiser.Convert.FromJson(response);
            Dictionary<string, object> caseInfo = parsedResponse.PropertyValue("CustomData").PropertyValue(location).PropertyValue("CustomData") as Dictionary<string, object>;

            if (caseInfo != null)
            {
                foreach (var item in caseInfo)
                {
                    Dictionary<string, object> combData = item.Value.PropertyValue("CustomData") as Dictionary<string, object>;
                    string name = combData["NAME"].ToString();
                    if (filters.Contains(name))
                    {
                        if (combData.ContainsKey("bCB") && combData["bCB"].ToString() == "True" && ending == "(CB)")
                            filteredNames.AddRange(new List<string> { name + "(CB:all)", name + "(CB:max)", name + "(CB:min)" });
                        else
                            filteredNames.Add(name + ending);
                    }
                }
            }
            return filteredNames;
        }
    }
}

