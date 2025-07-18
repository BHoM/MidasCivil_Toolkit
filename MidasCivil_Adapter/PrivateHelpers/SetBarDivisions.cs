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

using System;
using BH.Engine.Base;
using BH.oM.Structure.Requests;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private string SetBarDivisions(BarResultRequest request)
        {
            string divisions = String.Empty;
            if (request.DivisionType == DivisionType.ExtremeValues)
                divisions = "\"PARTS\": [\"Part I\", \"Part J\"], ";
            else
                switch (request.Divisions)
                {
                    case 2:
                        divisions = "\"PARTS\": [\"Part I\", \"Part J\"], ";
                        break;
                    case 3:
                        divisions = "\"PARTS\": [\"Part I\", \"Part 2/4\", \"Part J\"], ";
                        break;
                    case 5:
                        break;
                    default:
                        Compute.RecordWarning($"Midas Civil only supports results at bar end-, mid- or quarter points. The possible options for the Divisions filter are 2, 3 or 5. Results for all available positions will be pulled for the current request.");
                        break;
                }
            return divisions;
        }
    }
}
