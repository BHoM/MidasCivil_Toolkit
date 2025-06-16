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
using BH.Engine.Serialiser;
using BH.Engine.Base;


namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private async Task<IEnumerable<IResult>> ReadResult(string resultType, List<int> ids, List<string> loadcaseIds)
        {
            List<IResult> results = new List<IResult>();

            const string endpoint = "post/TABLE";

            string jsonPayload = "";
            string tableName = "";
            string tableType = "";
            string components = "";

            string units = "\"UNIT\": {\"FORCE\": \"N\", \"DIST\": \"m\"}, ";
            string format = "\"STYLES\": {\"FORMAT\": \"Fixed\", \"PLACE\": 6}";

            string objectIds = ids.Count > 0
             ? $"\"NODE_ELEMS\": {{ \"KEYS\": [{string.Join(", ", ids)}] }},"
             : string.Empty;

            string loadCases = loadcaseIds.Count > 0
             ? $"\"LOAD_CASE_NAMES\": [{string.Join(", ", loadcaseIds.Select(id => $"\"{id}\""))}],"
             : string.Empty;

            switch (resultType)
            {
                case "NodeReaction":
                    tableName = "\"TABLE_NAME\": \"Reaction(Global)\", ";
                    tableType = "\"TABLE_TYPE\": \"REACTIONG\", ";
                    components = "\"COMPONENTS\": [\"Node\", \"Load\", \"FX\", \"FY\", \"FZ\", \"MX\", \"MY\", \"MZ\"], ";
                    break;
                case "NodeDisplacement":
                    tableName = "\"TABLE_NAME\": \"Displacements(Global)\", ";
                    tableType = "\"TABLE_TYPE\": \"DISPLACEMENTG\", ";
                    components = "\"COMPONENTS\": [\"Node\", \"Load\", \"DX\", \"DY\", \"DZ\", \"RX\", \"RY\", \"RZ\"], ";
                    break;
                case "BarForce":
                    tableName = "\"TABLE_NAME\": \"BeamForce\", ";
                    tableType = "\"TABLE_TYPE\": \"BEAMFORCE\", ";
                    components = "\"COMPONENTS\": [\"Elem\", \"Load\", \"Part\", \"Axial\", \"Shear-y\", \"Shear-z\", \"Torsion\", \"Moment-y\", \"Moment-z\"], ";
                    break;
                case "BarStress":
                    tableName = "\"TABLE_NAME\": \"BeamStress\", ";
                    tableType = "\"TABLE_TYPE\": \"BEAMSTRESS\", ";
                    components = "\"COMPONENTS\": [\"Elem\", \"Load\", \"Part\", \"Axial\", \"Shear-y\", \"Shear-z\", \"Bend(+y)\", \"Bend(-y)\", \"Bend(+z)\", \"Bend(-z)\", \"Cb1(-y+z)\", \"Cb2(+y+z)\", \"Cb3(+y-z)\", \"Cb4(-y-z)\"], ";
                    break;
                case "Forces":
                    tableName = "\"TABLE_NAME\": \"PlateForce(UnitLength:Local)\", ";
                    tableType = "\"TABLE_TYPE\": \"PLATEFORCEUL\", ";
                    components = "\"COMPONENTS\": [\"Elem\", \"Load\", \"Node\", \"Fxx\", \"Fyy\", \"Fxy\", \"Mxx\", \"Myy\", \"Mxy\", \"Vxx\", \"Vyy\"], ";
                    break;
                case "Stresses":
                case "VonMises":
                    tableName = "\"TABLE_NAME\": \"PlateStress(Local)\", ";
                    tableType = "\"TABLE_TYPE\": \"PLATESTRESSL\", ";
                    components = "\"COMPONENTS\": [\"Elem\", \"Load\", \"Node\", \"Part\", \"Sig-xx\", \"Sig-yy\", \"Sig-xy\", \"Sig-Max\", \"Sig-Min\", \"Sig-EFF\"], ";
                    break;
                default:
                    Engine.Base.Compute.RecordError($"Pulling back results of type {resultType} is not yet supported through the MidasCivil API.");
                    return results;
            }

            jsonPayload = "{\"Argument\": {" +
                    tableName + tableType + components + objectIds + loadCases + units + format +
                    "}}";

            var response = await SendRequestAsync(endpoint, HttpMethod.Post, jsonPayload).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                Engine.Base.Compute.RecordError($"Something went wrong with the request, please ensure the connected model is solved and check for errors in the MidasCivil window.");
                return results;
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();

            if (jsonResponse.StartsWith("{\"message\":"))
            {
                Engine.Base.Compute.RecordError($"The connected model does not contain any results matching the request. Please check the filters in your request.");
                return results;
            }

            object parsedJson = Engine.Serialiser.Convert.FromJson(jsonResponse);

            switch (resultType)
            {
                case "NodeReaction":
                    object data = parsedJson.PropertyValue("CustomData").PropertyValue("Reaction(Global)").PropertyValue("DATA");
                    List<List<object>> resultItems = data as List<List<object>>;
                    foreach (var item in resultItems)
                        results.Add(Adapters.MidasCivil.Convert.ToNodeReaction(item, m_forceUnit, m_lengthUnit)); 
                    break;
                case "NodeDisplacement":
                    data = parsedJson.PropertyValue("CustomData").PropertyValue("Displacements(Global)").PropertyValue("DATA");
                    resultItems = data as List<List<object>>;
                        foreach (var item in resultItems)
                            results.Add(Adapters.MidasCivil.Convert.ToNodeDisplacement(item, m_lengthUnit));
                    break;
                case "BarForce":
                    data = parsedJson.PropertyValue("CustomData").PropertyValue("BeamForce").PropertyValue("DATA");
                    resultItems = data as List<List<object>>;
                    foreach (var item in resultItems)
                        results.Add(Convert.ToBarForce(item, m_forceUnit, m_lengthUnit));
                    break;
                case "BarStress":
                    data = parsedJson.PropertyValue("CustomData").PropertyValue("BeamStress").PropertyValue("DATA");
                    resultItems = data as List<List<object>>;
                    foreach (var item in resultItems)
                        results.Add(Convert.ToBarStress(item, m_forceUnit, m_lengthUnit));
                    break;
                case "Forces":
                    data = parsedJson.PropertyValue("CustomData").PropertyValue("PlateForce(UnitLength:Local)").PropertyValue("DATA");
                    resultItems = data as List<List<object>>;
                    foreach (var item in resultItems)
                        results.Add(Convert.ToMeshForce(item, m_forceUnit, m_lengthUnit));
                    break;
                case "Stresses":
                    data = parsedJson.PropertyValue("CustomData").PropertyValue("PlateStress(Local)").PropertyValue("DATA");
                    resultItems = data as List<List<object>>;
                    foreach (var item in resultItems)
                    {
                        List<string> itemList = item.Select(x => x.ToString()).ToList();

                        List<string> topElement = itemList.Take(11).ToList();
                        results.Add(Convert.ToMeshStressAPI(topElement));

                        if (itemList.Count > 11)
                        {
                            List<string> bottomElement = itemList.Take(4).Concat(itemList.Skip(11)).ToList();
                            results.Add(Convert.ToMeshStressAPI(bottomElement));
                        }
                    }
                    break;
                case "VonMises":
                    data = parsedJson.PropertyValue("CustomData").PropertyValue("PlateStress(Local)").PropertyValue("DATA");
                    resultItems = data as List<List<object>>;
                    foreach (var item in resultItems)
                    {
                        List<string> itemList = item.Select(x => x.ToString()).ToList();

                        List<string> topElement = itemList.Take(11).ToList();
                        results.Add(Convert.ToMeshVonMisesAPI(topElement));

                        if (itemList.Count > 11)
                        {
                            List<string> bottomElement = itemList.Take(4).Concat(itemList.Skip(11)).ToList();
                            results.Add(Convert.ToMeshVonMisesAPI(bottomElement));
                        }
                    }
                break;
            }
            return results;
        }
    }
}
