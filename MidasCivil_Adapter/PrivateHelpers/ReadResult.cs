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
using BH.oM.Analytical.Results;
using BH.oM.Structure.Results;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using BH.oM.Data.Requests;
using BH.Engine.Serialiser;
using BH.Engine.Base;
using BH.oM.Structure.Requests;


namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private async Task<IEnumerable<IResult>> ReadResult(string resultType, List<int> ids, List<string> loadcaseIds, string locations = "", MeshResultRequest meshRequest = null)
        {
            List<IResult> results = new List<IResult>();

            string jsonPayload = "";
            string endpoint = "post/TABLE";

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
                case "NodeDeformationTimeHistory":
                    break;
                default:
                    Engine.Base.Compute.RecordError($"Pulling back results of type {resultType} is not yet supported through the MidasCivil API.");
                    return results;
            }

            jsonPayload = "{\"Argument\": {" +
                    tableName + tableType + components + objectIds + loadCases + locations + units + format +
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
                Engine.Base.Compute.RecordError($"The connected model does not contain any results matching the request. Please check the ObjectId filter and only include IDs present in the model.");
                return results;
            }

            object parsedJson = Engine.Serialiser.Convert.FromJson(jsonResponse);

            List<List<object>> resultItems = new List<List<object>>();
            object data = new object();
            switch (resultType)
            {
                case "NodeReaction":
                    switch (m_midasCivilVersion)
                    {
                        case "9.5.0.nx":
                            data = parsedJson.PropertyValue("CustomData")?.PropertyValue("Reaction(Global)")?.PropertyValue("DATA");
                            break;
                        default:
                            data = parsedJson.PropertyValue("CustomData")?.PropertyValue("ReactionGlobal")?.PropertyValue("DATA");
                            break;
                    }
                    resultItems = data as List<List<object>>;
                    if (resultItems.IsNullOrEmpty())
                        Engine.Base.Compute.RecordError($"No NodeReaction could be found for the selected Node/Nodes.");
                    else
                        foreach (List<object> item in resultItems)
                            results.Add(Adapters.MidasCivil.Convert.ToNodeReaction(item));
                    break;
                case "NodeDisplacement":
                    switch (m_midasCivilVersion)
                    {
                        case "9.5.0.nx":
                            data = parsedJson.PropertyValue("CustomData")?.PropertyValue("Displacements(Global)")?.PropertyValue("DATA");
                            break;
                        default:
                            data = parsedJson.PropertyValue("CustomData")?.PropertyValue("DisplacementsGlobal")?.PropertyValue("DATA");
                            break;
                    }
                    resultItems = data as List<List<object>>;
                    foreach (List<object> item in resultItems)
                        results.Add(Adapters.MidasCivil.Convert.ToNodeDisplacement(item));
                    break;
                case "BarForce":
                    data = parsedJson.PropertyValue("CustomData")?.PropertyValue("BeamForce")?.PropertyValue("DATA");
                    resultItems = data as List<List<object>>;
                    foreach (List<object> item in resultItems)
                        results.Add(Convert.ToBarForce(item));
                    break;
                case "BarStress":
                    data = parsedJson.PropertyValue("CustomData")?.PropertyValue("BeamStress")?.PropertyValue("DATA");
                    resultItems = data as List<List<object>>;
                    foreach (List<object> item in resultItems)
                        results.Add(Convert.ToBarStress(item));
                    break;
                case "Forces":
                    switch (m_midasCivilVersion)
                    {
                        case "9.5.0.nx":
                            data = parsedJson.PropertyValue("CustomData")?.PropertyValue("PlateForce(UnitLength:Local)")?.PropertyValue("DATA");
                            break;
                        default:
                            data = parsedJson.PropertyValue("CustomData")?.PropertyValue("PlateForceUnitLengthLocal")?.PropertyValue("DATA");
                            break;
                    }
                    resultItems = data as List<List<object>>;
                    foreach (List<object> item in resultItems)
                        results.Add(Convert.ToMeshForce(item));
                    break;
                case "Stresses":
                    switch (m_midasCivilVersion)
                    {
                        case "9.5.0.nx":
                            data = parsedJson.PropertyValue("CustomData")?.PropertyValue("PlateStress(Local)")?.PropertyValue("DATA");
                            break;
                        default:
                            data = parsedJson.PropertyValue("CustomData")?.PropertyValue("PlateStressLocal")?.PropertyValue("DATA");
                            break;
                    }
                    resultItems = data as List<List<object>>;
                    foreach (List<object> item in resultItems)
                    {
                        List<List<object>> meshStresses = FilterMeshLayer(meshRequest, item);
                        foreach (List<object> meshStress in meshStresses)
                            results.Add(Convert.ToMeshStress(meshStress, meshRequest));
                    }
                    break;
                case "VonMises":
                    switch (m_midasCivilVersion)
                    {
                        case "9.5.0.nx":
                            data = parsedJson.PropertyValue("CustomData")?.PropertyValue("PlateStress(Local)")?.PropertyValue("DATA");
                            break;
                        default:
                            data = parsedJson.PropertyValue("CustomData")?.PropertyValue("PlateStressLocal")?.PropertyValue("DATA");
                            break;
                    }
                    resultItems = data as List<List<object>>;
                    foreach (List<object> item in resultItems)
                    {
                        List<List<object>> meshVonMises = FilterMeshLayer(meshRequest, item);
                        foreach (List<object> meshStress in meshVonMises)
                            results.Add(Convert.ToMeshVonMises(meshStress, meshRequest));
                    }
                    break;

            }
            return results;
        }

        /***************************************************/

        private async Task<IEnumerable<IResult>> ReadResultTimeHistory(string resultType, List<int> ids, List<string> loadcaseNames)
        {
            List<IResult> results = new List<IResult>();

            string exportPath = "";
            if (m_outputFolder != null)
            {
                exportPath = "\"EXPORT_PATH\": \"" + m_outputFolder.Replace("\\", "\\\\") + "\\\\TH_GlinkDeform_Out.JSON\",";
            }
            else
            {
                Engine.Base.Compute.RecordError("Time history request failed. Ensure a output folder is defined in the midas civil settings for the adapter."); 
            }

            string thComponents = "";
            string thTableType = "";
            string propertyName = "";
            string thEndpoint = "post/TEXT";

            ids = ids.Distinct().ToList();

            string loadCaseName = loadcaseNames.Count > 0
            ? $"\"TH_CASE_NAME\": [{string.Join(", ", loadcaseNames.Select(id => $"\"{id}\""))}],"
            : string.Empty;

            if (resultType == "LinkDisplacement")
            {
                thTableType = "\"TEXT_TYPE\": \"TH_GLINKDEFORM\", ";
                thComponents = "\"COMPONENTS\": [\"Key\", \"Node1\", \"Node2\", \"Load\", \"Time/Step\", \"DX\", \"DY\", \"DZ\", \"RX\", \"RY\", \"RZ\"], ";
                propertyName = "TH_GLINKDEFORM";
            }
            else // LinkForce
            {
                thTableType = "\"TEXT_TYPE\": \"TH_GLINKFORCE\", ";
                thComponents = "\"COMPONENTS\": [\"Key\", \"Node1\", \"Node2\", \"Load\", \"Time/Step\", \"FX\", \"FY\", \"FZ\", \"MX\", \"MY\", \"MZ\"], ";
                propertyName = "TH_GLINKFORCE";
            }

            foreach (int id in ids)
            {

                string payload = "{"
                    + "\"Argument\": {"
                    + thTableType
                    + exportPath
                    + "\"UNIT\": {\"FORCE\": \"KN\", \"DIST\": \"M\"},"
                    + "\"STYLES\": {\"FORMAT\": \"Fixed\", \"PLACE\": 6},"
                    + thComponents
                    + $"\"NODE_ELEMS\": {{ \"KEYS\": [{id}] }},"
                    + loadCaseName
                    + $"\"STEP\": {{\"FROM\": 0, \"TO\": 0, \"STEPS\": 1}}"
                    + "}"
                    + "}";

                var responseTH = await SendRequestAsync(thEndpoint, HttpMethod.Post, payload);

                if (!responseTH.IsSuccessStatusCode)
                {
                    Engine.Base.Compute.RecordError($"Time history request failed. Ensure the model is solved.");
                }

                string jsonTHResponse = await responseTH.Content.ReadAsStringAsync();

                if (jsonTHResponse.StartsWith("{\"message\":"))
                {
                    Engine.Base.Compute.RecordError($"No time history results found.");
                    return null;
                }

                object parsedTHJson = Engine.Serialiser.Convert.FromJson(jsonTHResponse);
                object thData = parsedTHJson.PropertyValue("CustomData")?.PropertyValue(propertyName)?.PropertyValue("DATA");

                List<List<object>> thResultItems = thData as List<List<object>>;

                foreach (List<object> item in thResultItems)
                {
                    if (resultType == "LinkDisplacement")
                    {
                        results.Add(Convert.ToLinkDisplacement(item));
                    }
                    else // LinkForce
                    {
                        results.Add(Convert.ToLinkForce(item));
                    }
                }
            }

            return results;  
        }
        /***************************************************/
    }
}

