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

using System.Collections.Generic;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public Dictionary<string,List<int>> ReadTags(string section, int position)
        {
            List<string> sectionText = GetSectionText(section);

            Dictionary<string, List<int>> itemAssignments = new Dictionary<string, List<int>>();

            for (int i = 0; i < sectionText.Count(); i++)
            {
                string name = sectionText[i].Split(',')[0].Trim();
                string items = sectionText[i].Split(',')[position];

                List<int> itemAssignment = new List<int>();

                if (items.Contains(" ") || string.IsNullOrWhiteSpace(items))
                {
                    List<string> assignments = items.Split(' ').
                        Select(x=>x.Trim()).
                        Where(x => !string.IsNullOrEmpty(x)).
                        ToList();
                    itemAssignment = MidasCivilAdapter.GetAssignmentIds(assignments);
                }
                else
                {
                    itemAssignment.Add(int.Parse(items));
                }

                itemAssignments.Add(name, itemAssignment);
            }

            return itemAssignments;
        }

    }
}
