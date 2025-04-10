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

using BH.oM.Analytical.Results;
using BH.oM.Adapter;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Requests;
using BH.oM.Structure.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Text;
using System.Text.Json;



namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {

        /***************************************************/
        /**** Private method - Read override            ****/
        /***************************************************/

        public IEnumerable<IResult> ReadResults(NodeResultRequest request, ActionConfig actionConfig)
        {
            List<IResult> results;
            List<int> objectIds = GetObjectIDs(request);
            List<string> loadCases = GetLoadcaseIDs(request);

            switch (request.ResultType)
            {
                case NodeResultType.NodeReaction:
                    if (m_midasCivilVersion == "9.5.0.nx")
                    {
                        results = Task.Run(() => ExtractNodeReactionAPI(objectIds, loadCases)).Result.ToList();
                    }

                    else
                        results = ExtractNodeReaction(objectIds, loadCases).ToList();

                    break;
                case NodeResultType.NodeDisplacement:
                    results = ExtractNodeDisplacement(objectIds, loadCases).ToList();
                    break;
                default:
                    Engine.Base.Compute.RecordError($"Result of type {request.ResultType} is not yet supported in the MidasCivil_Toolkit.");
                    results = new List<IResult>();
                    break;
            }
            results.Sort();
            return results;
        }

        /***************************************************/
        /**** Private  Methods                          ****/
        /***************************************************/

        private IEnumerable<IResult> ExtractNodeReaction(List<int> ids, List<string> loadcaseIds)
        {
            string filePath = m_directory + "\\Results\\Reaction(Global).xls";
            string csvPath = ExcelToCsv(filePath);
            List<String> nodeReactionText = File.ReadAllLines(csvPath).ToList();
            List<NodeReaction> nodeReactions = new List<NodeReaction>();
            for (int i = 9; i < nodeReactionText.Count; i++)
            {
                List<string> nodeReaction = nodeReactionText[i].Split(',').ToList(); ;
                if (nodeReactionText[i].Contains("SUMMATION"))
                {
                    break;
                }
                else
                {
                    if (ids.Contains(System.Convert.ToInt32(nodeReaction[2])) && loadcaseIds.Contains(nodeReaction[3]))
                    {
                        nodeReactions.Add(Adapters.MidasCivil.Convert.ToNodeReaction(nodeReaction, m_forceUnit, m_lengthUnit));
                    }
                }

            }

            return nodeReactions;
        }

        /***************************************************/

        private IEnumerable<IResult> ExtractNodeDisplacement(List<int> ids, List<string> loadcaseIds)
        {
            string filePath = m_directory + "\\Results\\Displacements(Global).xls";
            string csvPath = ExcelToCsv(filePath);
            List<String> nodeDisplacementText = File.ReadAllLines(csvPath).ToList();

            List<NodeDisplacement> nodeDisplacements = new List<NodeDisplacement>();
            for (int i = 9; i < nodeDisplacementText.Count; i++)
            {
                List<string> nodeDisplacement = nodeDisplacementText[i].Split(',').ToList(); ;
                if (nodeDisplacementText[i].Contains("SUMMATION"))
                {
                    break;
                }
                else
                {
                    if (ids.Contains(System.Convert.ToInt32(nodeDisplacement[2])) && loadcaseIds.Contains(nodeDisplacement[3]))
                    {
                        nodeDisplacements.Add(Adapters.MidasCivil.Convert.ToNodeDisplacement(nodeDisplacement, m_lengthUnit));
                    }
                }
            }

            return nodeDisplacements;
        }

        /***************************************************/

        private async Task<IEnumerable<IResult>> ExtractNodeReactionAPI(List<int> ids, List<string> loadcaseIds)
        {
            List<NodeReaction> nodeReactions = new List<NodeReaction>();

            const string endpoint = "post/TABLE"; //Will be the same for all

            string nodes = ids.Count > 0
             ? $"\"NODE_ELEMS\": {{ \"KEYS\": [{string.Join(", ", ids)}] }}," //Will be the same for all
             : string.Empty;

            string loadCases = loadcaseIds.Count > 0
            ? $"\"LOAD_CASE_NAMES\": [{string.Join(", ", loadcaseIds.Select(id => $"\"{id}\""))}]," //Will be the same for all
            : string.Empty;

            string jsonPayload = "{\"Argument\": " +
                "{\"TABLE_NAME\": \"Reaction(Global)\", \"TABLE_TYPE\": \"REACTIONG\", " +  //Different
                "\"UNIT\": {\"FORCE\": \"N\", \"DIST\": \"m\"}, \"STYLES\": {\"FORMAT\": \"Fixed\", \"PLACE\": 3}, " +
                nodes + loadCases +
                "\"COMPONENTS\": [\"Node\", \"Load\", \"FX\", \"FY\", \"FZ\", \"MX\", \"MY\", \"MZ\"]}}";

            var response = await SendRequestAsync(endpoint, HttpMethod.Post, jsonPayload).ConfigureAwait(false); //Will be the same for all
            string jsonResponse = await response.Content.ReadAsStringAsync(); //Will be the same for all


            using (JsonDocument doc = JsonDocument.Parse(jsonResponse)) 
            {
                var dataElement = doc.RootElement.GetProperty("Reaction(Global)").GetProperty("DATA"); //Different

                foreach (var item in dataElement.EnumerateArray())
                {
                    nodeReactions.Add(Adapters.MidasCivil.Convert.ToNodeReactionJson(item));    //Different
                }
            }
            
            return nodeReactions;
        }

    }
}




