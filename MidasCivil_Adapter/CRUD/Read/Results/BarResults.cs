﻿/*
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

using BH.oM.Common;
using BH.oM.Adapter;
using BH.oM.Structure.Requests;
using BH.oM.Structure.Results;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {

        /***************************************************/
        /**** Public method - Read override             ****/
        /***************************************************/

        public IEnumerable<IResult> ReadResults(BarResultRequest request, ActionConfig actionConfig)
        {
            List<IResult> results;
            List<int> objectIds = GetObjectIDs(request);
            List<string> loadCases = GetLoadcaseIDs(request);

            switch (request.ResultType)
            {
                case BarResultType.BarForce:
                    results = ExtractBarForce(objectIds, loadCases).ToList();
                    break;
                case BarResultType.BarStrain:
                    results = ExtractBarStrain(objectIds, loadCases).ToList();
                    break;
                case BarResultType.BarStress:
                    results = ExtractBarStress(objectIds, loadCases).ToList();
                    break;
                case BarResultType.BarDisplacement:
                    results = ExtractBarDisplacement(objectIds, loadCases).ToList();
                    break;
                default:
                    Engine.Reflection.Compute.RecordError($"Result of type {request.ResultType} is not yet supported in the MidasCivil_Toolkit.");
                    results = new List<IResult>();
                    break;
            }
            results.Sort();
            return results;
        }

        /***************************************************/
        /**** Private  Methods                          ****/
        /***************************************************/


        /***************************************************/

        private IEnumerable<IResult> ExtractBarStress(List<int> ids, List<string> loadcaseIds)
        {




            /***************************************************/
            string filePath = directory + "\\Beam Stress.xls";
            string csvPath = ExcelToCsv(filePath);
            List<String> BarstressText = File.ReadAllLines(csvPath).ToList();

            List<BarStress> Barstress = new List<BarStress>();
            for (int i = 9; i < BarstressText.Count; i++)
            {
                List<string> BarStress = BarstressText[i].Split(',').ToList(); ;
                if (BarstressText[i].Contains("SUMMATION"))
                {
                    break;
                }
                else
                {
                    if (ids.Contains(System.Convert.ToInt32(BarStress[2])) && loadcaseIds.Contains(BarStress[3]))
                    {
                        Barstress.Add(Engine.MidasCivil.Convert.ToBarStress(BarStress));
                    }
                }
            }
            return Barstress;

        }

        /***************************************************/

        private IEnumerable<IResult> ExtractBarStrain(List<int> ids, List<string> loadcaseIds)
        {
            return null;
        }

        /***************************************************/

        private IEnumerable<IResult> ExtractBarDisplacement(List<int> ids, List<string> loadcaseIds)
        {
            return null;
        }

        /***************************************************/



        private IEnumerable<IResult> ExtractBarForce(List<int> ids, List<string> loadcaseIds)
        {




            /***************************************************/
            string filePath = directory + "\\Beam Force.xls";
            string csvPath = ExcelToCsv(filePath);
            List<String> BarForceText = File.ReadAllLines(csvPath).ToList();

            List<BarForce> Barforces = new List<BarForce>();
            for (int i = 9; i < BarForceText.Count; i++)
            {
                List<string> BarForce = BarForceText[i].Split(',').ToList(); ;
                if (BarForceText[i].Contains("SUMMATION"))
                {
                    break;
                }
                else
                {
                    if (ids.Contains(System.Convert.ToInt32(BarForce[2])) && loadcaseIds.Contains(BarForce[3]))
                    {
                        Barforces.Add(Engine.MidasCivil.Convert.ToBarForce(BarForce));
                    }
                }
            }
            return Barforces;
        }

        /***************************************************/

    }
}
