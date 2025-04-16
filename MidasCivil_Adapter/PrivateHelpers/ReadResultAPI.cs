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
using BH.oM.Analytical.Results;
using BH.oM.Structure.Results;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using BH.oM.Data.Requests;


namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private async Task<IEnumerable<IResult>> ExtractResultAPI(string resultType, List<int> ids, List<string> loadcaseIds)
        {
            List<IResult> results = new List<IResult>();

            const string endpoint = "post/TABLE";

            string jsonPayload = "";

            string units = "\"UNIT\": {\"FORCE\": \"N\", \"DIST\": \"m\"}, ";
            string format = "\"STYLES\": {\"FORMAT\": \"Fixed\", \"PLACE\": 3}";

            string objectIds = ids.Count > 0
             ? $"\"NODE_ELEMS\": {{ \"KEYS\": [{string.Join(", ", ids)}] }},"
             : string.Empty;

            string loadCases = loadcaseIds.Count > 0
             ? $"\"LOAD_CASE_NAMES\": [{string.Join(", ", loadcaseIds.Select(id => $"\"{id}\""))}],"
             : string.Empty;

            switch (resultType)
            {
                case "NodeReaction":
                    jsonPayload = "{\"Argument\": {" +
                    "\"TABLE_NAME\": \"Reaction(Global)\", \"TABLE_TYPE\": \"REACTIONG\", " +
                    "\"COMPONENTS\": [\"Node\", \"Load\", \"FX\", \"FY\", \"FZ\", \"MX\", \"MY\", \"MZ\"], " +
                    objectIds + loadCases + units + format +
                    "}}";
                    break;

                case "NodeDisplacement":
                    jsonPayload = "{\"Argument\": {" +
                    "\"TABLE_NAME\": \"Displacements(Global)\", \"TABLE_TYPE\": \"DISPLACEMENTG\", " +
                    "\"COMPONENTS\": [\"Node\", \"Load\", \"DX\", \"DY\", \"DZ\", \"RX\", \"RY\", \"RZ\"], " +
                    objectIds + loadCases + units + format +
                    "}}";
                    break;

                case "BarForce":
                    jsonPayload = "{\"Argument\": {" +
                    "\"TABLE_NAME\": \"BeamForce\", \"TABLE_TYPE\": \"BEAMFORCE\", " +
                    "\"COMPONENTS\": [\"Elem\", \"Load\", \"Part\", \"Axial\", \"Shear-y\", \"Shear-z\", \"Torsion\", \"Moment-y\", \"Moment-z\"], " +
                    objectIds + loadCases + units + format +
                    "}}";
                    break;

                case "BarStress":
                    jsonPayload = "{\"Argument\": {" +
                    "\"TABLE_NAME\": \"BeamStress\", \"TABLE_TYPE\": \"BEAMSTRESS\", " +
                    "\"COMPONENTS\": [\"Elem\", \"Load\", \"Part\", \"Axial\", \"Shear-y\", \"Shear-z\", \"Bend(+y)\", \"Bend(-y)\", \"Bend(+z)\", \"Bend(-z)\", \"Cb1(-y+z)\", \"Cb2(+y+z)\", \"Cb3(+y-z)\", \"Cb4(-y-z)\"], " +
                    objectIds + loadCases + units + format +
                    "}}";
                    break;

                case "Forces":
                     jsonPayload = "{\"Argument\": {" +
                     "\"TABLE_NAME\": \"PlateForce(UnitLength:Local)\", " +
                     "\"TABLE_TYPE\": \"PLATEFORCEUL\", " +
                     "\"COMPONENTS\": [\"Elem\", \"Load\", \"Node\", \"Fxx\", \"Fyy\", \"Fxy\", \"Mxx\", \"Myy\", \"Mxy\", \"Vxx\", \"Vyy\"], " +
                     objectIds + loadCases + units + format +
                    "}}";
                    break;

                case "Stresses":
                case "VonMises":
                    jsonPayload = "{\"Argument\": {" +
                    "\"TABLE_NAME\": \"PlateStress(Local)\", " +
                    "\"TABLE_TYPE\": \"PLATESTRESSL\", " +
                    "\"COMPONENTS\": [\"Elem\", \"Load\", \"Node\", \"Part\", \"Sig-xx\", \"Sig-yy\", \"Sig-xy\", \"Sig-Max\", \"Sig-Min\", \"Sig-EFF\"], " +
                    objectIds + loadCases + units + format +
                   "}}";
                    break;

                default:
                    Engine.Base.Compute.RecordError($"Pulling back results of type {resultType} is not yet supported through the MidasCivil API.");
                    return results;
            }
            
            var response = await SendRequestAsync(endpoint, HttpMethod.Post, jsonPayload).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                Engine.Base.Compute.RecordError($"Something went wrong with the request, please ensure the connected model is solved and check for errors in the MidasCivil window.");
                return results;
            }
            string jsonResponse = await response.Content.ReadAsStringAsync();

            if (jsonResponse.StartsWith("{\"message\":"))
            {
                Engine.Base.Compute.RecordError($"The conected model does not seem to contain any results matching the request. Please check the filters in your request.");
                return results;
            }

            using (JsonDocument doc = JsonDocument.Parse(jsonResponse)) 
            {
                var dataElement = new JsonElement();
                switch (resultType)
                {
                    case "NodeReaction":
                        dataElement = doc.RootElement.GetProperty("Reaction(Global)").GetProperty("DATA");
                        foreach (var item in dataElement.EnumerateArray())
                             results.Add(Adapters.MidasCivil.Convert.ToNodeReactionAPI(item)); 
                        break;

                    case "NodeDisplacement":
                        dataElement = doc.RootElement.GetProperty("Displacements(Global)").GetProperty("DATA");
                        foreach (var item in dataElement.EnumerateArray())
                            results.Add(Adapters.MidasCivil.Convert.ToNodeDisplacementAPI(item));
                        break;

                    case "BarForce":
                        dataElement = doc.RootElement.GetProperty("BeamForce").GetProperty("DATA");
                        foreach (var item in dataElement.EnumerateArray())
                            results.Add(Convert.ToBarForceAPI(item));
                        break;

                    case "BarStress":
                        dataElement = doc.RootElement.GetProperty("BeamStress").GetProperty("DATA");
                        foreach (var item in dataElement.EnumerateArray())
                            results.Add(Convert.ToBarStressAPI(item));
                        break;

                    case "Forces":
                        dataElement = doc.RootElement.GetProperty("PlateForce(UnitLength:Local)").GetProperty("DATA");
                        foreach (var item in dataElement.EnumerateArray())
                            results.Add(Convert.ToMeshForceAPI(item));
                        break;

                    case "Stresses":
                        dataElement = doc.RootElement.GetProperty("PlateStress(Local)").GetProperty("DATA");
                        foreach (var item in dataElement.EnumerateArray())
                        {
                            var itemList = item.EnumerateArray().Select(x => x.ToString()).ToList();

                            var topElement = itemList.Take(11).ToList();
                            results.Add(Convert.ToMeshStressAPI(topElement));

                            var bottomElement = itemList.Take(4).Concat(itemList.Skip(11)).ToList();
                            results.Add(Convert.ToMeshStressAPI(bottomElement));
                        }
                        break;

                    case "VonMises":
                        dataElement = doc.RootElement.GetProperty("PlateStress(Local)").GetProperty("DATA");
                        foreach (var item in dataElement.EnumerateArray())
                        {
                            var itemList = item.EnumerateArray().Select(x => x.ToString()).ToList();

                            var topElement = itemList.Take(11).ToList();
                            results.Add(Convert.ToMeshVonMisesAPI(topElement));

                            var bottomElement = itemList.Take(4).Concat(itemList.Skip(11)).ToList();
                            results.Add(Convert.ToMeshVonMisesAPI(bottomElement));
                        }
                        break;

                }
            }

            return results;
        }
    }
}
