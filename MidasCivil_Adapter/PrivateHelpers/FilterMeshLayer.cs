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

using BH.Engine.Base;
using BH.oM.Data.Requests;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Requests;
using BH.oM.Structure.Results;
using System;
using System.Collections.Generic;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<List<object>> FilterMeshLayer(MeshResultRequest request, List<object> resultItems)
        {
            List<List<object>> filteredResult = new List<List<object>>();

            List<object> upperResult = resultItems.GetRange(0,11);
            List<object> lowerResult = resultItems.ShallowClone();      
            lowerResult.RemoveRange(4, 7);

            switch (request.Layer)
            {
                case MeshResultLayer.Upper:
                    filteredResult.Add(upperResult);
                    break;
                case MeshResultLayer.Lower:
                    filteredResult.Add(lowerResult);
                    break;
                case MeshResultLayer.Middle:
                    Compute.RecordWarning("Mesh results for the middle layer can not be pulled from Midas Civil. Results for both the top and bottom layer will be returned.");
                    filteredResult.Add(upperResult);
                    filteredResult.Add(lowerResult);
                    break;
                case MeshResultLayer.Maximum:
                    filteredResult.Add(resultItems.GetRange(0, 4));
                    filteredResult[0].Add(null);
                    for (int i = 5; i < 11; i++)
                        filteredResult[0].Add(Math.Max(System.Convert.ToDouble(upperResult[i].ToString()), System.Convert.ToDouble(lowerResult[i].ToString())));
                    break;
                case MeshResultLayer.Minimum:
                    filteredResult.Add(resultItems.GetRange(0, 4));
                    filteredResult[0].Add(null); 
                    for (int i = 5; i < 11; i++)
                        filteredResult[0].Add(Math.Min(System.Convert.ToDouble(upperResult[i].ToString()), System.Convert.ToDouble(lowerResult[i].ToString())));
                    break;
                case MeshResultLayer.AbsoluteMaximum:
                    filteredResult.Add(resultItems.GetRange(0, 4));
                    filteredResult[0].Add(null);
                    for (int i = 5; i < 11; i++)
                        filteredResult[0].Add(Math.Max(Math.Abs(System.Convert.ToDouble(upperResult[i].ToString())), Math.Abs(System.Convert.ToDouble(lowerResult[i].ToString()))));
                    break;
                case MeshResultLayer.Arbitrary:
                    switch (request.LayerPosition)
                    {
                        case 0:
                            filteredResult.Add(lowerResult);
                            break;
                        case 1:
                            filteredResult.Add(upperResult);
                            break;
                        default:
                            Compute.RecordWarning("Mesh results can only be pulled for the top (1) or bottom (0) layer in Midas Civil. Results for both the top and bottom layers will be returned.");
                            filteredResult.Add(upperResult);
                            filteredResult.Add(lowerResult);
                            break;
                    }
                    break;
            }
            return filteredResult;
        }
    }
}
