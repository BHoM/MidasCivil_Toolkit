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

using BH.oM.Analytical.Results;
using BH.oM.Adapter;
using BH.oM.Structure.Requests;
using BH.oM.Structure.Results;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.Threading.Tasks;
using BH.oM.Structure.Loads;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {

        /***************************************************/
        /**** Private method - Read override            ****/
        /***************************************************/

        public IEnumerable<IResult> ReadResults(LinkResultRequest request, ActionConfig actionConfig)
        {
            List<IResult> results = new List<IResult>();
            List<int> objectIds = GetObjectIDs(request);

            List<string> loadcaseIds = new List<string>();

            if (request.Cases != null)
            {
                foreach (object thisCase in request.Cases)
                {
                    if (thisCase is ICase bhCase)
                        loadcaseIds.Add(bhCase.Name);
                    else if (thisCase is string caseId)
                        loadcaseIds.Add(caseId);
                }
            }
            switch (m_midasCivilVersion)
            {
                case "9.5.0.nx":
                case "9.5.5.nx":
                    List<TimeHistoryLoadcase> thLoadcases = Task.Run(() => ReadTimeHistoryLoadcases(loadcaseIds)).Result;

                    if (thLoadcases != null)
                    {
                        List<string> loadcaseNames = new List<string>();

                        foreach (var th in thLoadcases)
                        {
                            loadcaseNames.Add(th.Name);
                        }

                        results = Task.Run(() => ReadResultTimeHistory(request.ResultType.ToString(),objectIds,loadcaseNames)).Result.ToList();
                    }
                    break;

                default:
                    Engine.Base.Compute.RecordError(
                        $"Result of type {request.ResultType} is only supported in the Midas NX."
                    );
                    break;
            }
            results.Sort();
            return results;
        }
    }
}






