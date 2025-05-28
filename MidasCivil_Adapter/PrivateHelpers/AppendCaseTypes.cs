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
        public async Task<List<string>> AppendCaseTypes(IResultRequest request)
        {
            List<string> caseNames = new List<string>();

            if (request.Cases.Count == 0)
                return caseNames;

            else
            {
                IList cases = request.Cases;
                List<string> requestNames = new List<string>();
                foreach (object thisCase in cases)
                {
                    if (thisCase is ICase)
                    {
                        ICase bhCase = thisCase as ICase;
                        requestNames.Add(bhCase.Name.ToString());
                    }
                    else if (thisCase is string)
                        requestNames.Add(thisCase as string);
                }

                Task<HttpResponseMessage> taskComb = SendRequestAsync("db/LCOM-GEN", HttpMethod.Get, "");
                Task<HttpResponseMessage> taskCase = SendRequestAsync("db/STLD", HttpMethod.Get, "");

                var responses = await Task.WhenAll(taskComb, taskCase).ConfigureAwait(false);

                if (!responses[0].IsSuccessStatusCode || !responses[1].IsSuccessStatusCode)
                {
                    Engine.Base.Compute.RecordError($"The existing Cases from the Midas Civil model could not be read. The result request will be sent without the case filter.");
                    return caseNames;
                }

                string responseComb = await responses[0].Content.ReadAsStringAsync();
                string responseCase = await responses[1].Content.ReadAsStringAsync();

                object parsedJson = Engine.Serialiser.Convert.FromJson(responseComb);
                object dataElement = parsedJson.PropertyValue("CustomData").PropertyValue("LCOM-GEN");
                object names = dataElement.PropertyValue("CustomData");

                //foreach (item in resultItem)
                //{
                    //string name = item.Value.GetProperty("NAME").GetString();
                    //if (requestNames.Contains(name))
                        //caseNames.Add(name + "(CB)");
                //} 
                

                //using (JsonDocument doc = JsonDocument.Parse(responseCase))
                //{
                    //JsonElement dataElement = doc.RootElement.GetProperty("STLD");
                    //foreach (JsonProperty item in dataElement.EnumerateObject())
                    //{
                        //string name = item.Value.GetProperty("NAME").GetString();
                        //if (requestNames.Contains(name))
                            //caseNames.Add(name + "(ST)");
                    //}
                //}

                if (caseNames.Count != requestNames.Count)
                    Engine.Base.Compute.RecordWarning($"At least one Case has been removed from the filters since a matching name could not be found in the Midas Civil model.");
                
                return caseNames;
            }
        }
    }
}
