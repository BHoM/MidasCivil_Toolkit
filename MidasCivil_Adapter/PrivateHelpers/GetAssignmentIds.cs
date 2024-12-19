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
using System.Collections.Generic;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        /***************************************************/
        /**** Internal Methods                          ****/
        /***************************************************/

        internal static List<int> GetAssignmentIds(List<string> assignments)
        {
            List<int> propertyAssignment = new List<int>();

            foreach (string assignment in assignments)
            {
                if (assignment.Contains("by"))
                {
                    propertyAssignment.AddRange(RangeBySplit(assignment, "to", "by"));
                }
                else if (assignment.Contains("to"))
                {
                    propertyAssignment.AddRange(RangeBySplit(assignment, "to"));
                }
                else if(assignment == "")
                {
                    continue;
                }
                else
                {
                    int id = System.Convert.ToInt32(assignment);
                    propertyAssignment.Add(id);
                }
            }

            return propertyAssignment;
        }

        /***************************************************/

        private static List<int> RangeBySplit(string text, string split)
        {
            string[] splitStringTo = text.Split(new[] { split }, StringSplitOptions.RemoveEmptyEntries);
            int start = System.Convert.ToInt32(splitStringTo[0]);
            int end = System.Convert.ToInt32(splitStringTo[1]);
            List<int> range = new List<int>(Enumerable.Range(start, end - start + 1));

            return range;
        }

        /***************************************************/

        private static List<int> RangeBySplit(string text, string split1, string split2)
        {
            string[] splitStringTo = text.Split(new[] { split1 }, StringSplitOptions.RemoveEmptyEntries);
            int start = System.Convert.ToInt32(splitStringTo[0]);
            string[] splitStringBy = splitStringTo[1].Split(new[] { split2 }, StringSplitOptions.RemoveEmptyEntries);
            int increment = System.Convert.ToInt32(splitStringBy[1]);
            int end = System.Convert.ToInt32(splitStringBy[0]);

            List<int> range = new List<int>(Enumerable.Range(start, end - start + 1).Where((x, i) => i % increment == 0));

            return range;
        }

        /***************************************************/

    }
}





