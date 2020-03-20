/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using BH.oM.Base;
using BH.oM.Adapter;
using BH.oM.Common;
using BH.oM.Data.Requests;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Requests;
using Application = Microsoft.Office.Interop.Excel.Application;
using Workbook = Microsoft.Office.Interop.Excel.Workbook;
using Worksheet = Microsoft.Office.Interop.Excel.Worksheet;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {

        /***************************************************/
        /**** Adapter  Methods                          ****/
        /***************************************************/

        protected override IEnumerable<IResult> ReadResults(Type type, IList ids = null, IList cases = null, int divisions = 5, ActionConfig actionConfig = null)
        {
            IResultRequest request = Engine.Structure.Create.IResultRequest(type, ids?.Cast<object>(), cases?.Cast<object>(), divisions);

            if (request != null)
                return this.ReadResults(request as dynamic, actionConfig);
            else
                return new List<IResult>();
        }

        /***************************************************/
        /**** Private  Methods - Index checking         ****/
        /***************************************************/

        private List<int> GetObjectIDs(IResultRequest request)
        {
            IList ids = request.ObjectIds;

            if (ids == null || ids.Count == 0)
            {
                return GetAllIds(request as dynamic);
            }
            else
            {
                if (ids is List<string>)
                    return (ids as List<string>).Select(x => int.Parse(x)).ToList();
                else if (ids is List<int>)
                    return ids as List<int>;
                else if (ids is List<double>)
                    return (ids as List<double>).Select(x => (int)Math.Round(x)).ToList();
                else
                {
                    List<int> idsOut = new List<int>();
                    foreach (object o in ids)
                    {
                        int id;
                        object idObj;
                        if (int.TryParse(o.ToString(), out id))
                        {
                            idsOut.Add(id);
                        }
                        else if (o is IBHoMObject && (o as IBHoMObject).CustomData.TryGetValue(AdapterIdName, out idObj) && int.TryParse(idObj.ToString(), out id))
                            idsOut.Add(id);
                    }
                    return idsOut;
                }
            }
        }

        private List<int> GetAllIds(NodeResultRequest request)
        {
            int maxIndex = GetMaxId("NODE");

            List<int> ids = new List<int>();
            for (int i = 1; i < maxIndex + 1; i++)
            {
                ids.Add(i);
            }

            return ids;
        }

        /***************************************************/

        private List<int> GetAllIds(BarResultRequest request)
        {
            int maxIndex = GetMaxId("ELEMENT");

            List<int> ids = new List<int>();
            for (int i = 1; i < maxIndex + 1; i++)
            {
                ids.Add(i);
            }

            return ids;
        }

        /***************************************************/

        private List<int> GetAllIds(MeshResultRequest request)
        {
            int maxIndex = GetMaxId("ELEMENT");

            List<int> ids = new List<int>();
            for (int i = 1; i < maxIndex + 1; i++)
            {
                ids.Add(i);
            }

            return ids;
        }

        /***************************************************/

        private List<string> GetLoadcaseIDs(IResultRequest request)
        {
            IList cases = request.Cases;

            List<string> caseNames = new List<string>();

            if (cases is List<string>)
                return (cases as List<string>);
            else if (cases is List<int>)
            {
                Engine.Reflection.Compute.RecordError("MidasCivil_Toolkit Loadcases do not have Ids as int, provide Loadcases or names.");
                return null;
            }

            else if (cases is List<double>)
            {
                Engine.Reflection.Compute.RecordError("MidasCivil_Toolkit Loadcases do not have Ids as doubles, provide Loadcases or names.");
                return null;
            }

            else if (cases is List<Loadcase>)
            {
                for (int i = 0; i < cases.Count; i++)
                {
                    caseNames.Add((cases[i] as Loadcase).Name);
                }
            }
            else if (cases is List<LoadCombination>)
            {
                foreach (object lComb in cases)
                {
                    foreach (Tuple<double, ICase> lCase in (lComb as LoadCombination).LoadCases)
                    {
                        caseNames.Add(lCase.Item2.Name);
                    }
                    caseNames.Add((lComb as LoadCombination).Name);
                }
            }
            else
            {
                caseNames = GetSectionText("STLDCASE").Select(x => x.Split(',')[0].Trim()).ToList();

                List<string> loadCombinationText = GetSectionText("LOADCOMB");
                for (int i = 0; i < loadCombinationText.Count; i += 2)
                {
                    caseNames.Add(loadCombinationText[i].Split(',')[0].Split('=')[1].Trim());
                }
            }


            return caseNames;
        }

        /***************************************************/

        private static string ExcelToCsv(string path)
        {
            string csvPath = GetCSVFile(path); ;
            if (!(File.Exists(path)))
            {
                Engine.Reflection.Compute.RecordError("No Excel file detected, please make sure you have exported the results from MidasCivil.");
                return null;
            }
            else
            {
                if (!(File.Exists(csvPath)))
                {
                    Application excel = new Application();
                    Workbook xlsFile = excel.Workbooks.Open(path);
                    Worksheet sheet = (Microsoft.Office.Interop.Excel.Worksheet)xlsFile.Sheets[1];

                    sheet.SaveAs(
                        csvPath,
                        Microsoft.Office.Interop.Excel.XlFileFormat.xlCSV,
                        Type.Missing,
                        Type.Missing,
                        Type.Missing,
                        Type.Missing,
                        Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared,
                        Type.Missing,
                        Type.Missing,
                        Type.Missing
                    );

                    xlsFile.Close();

                }
            }

            return csvPath;
        }

        /***************************************************/

        private static string GetCSVFile(string path)
        {
            return Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + ".csv";
        }

        /***************************************************/

    }
}
