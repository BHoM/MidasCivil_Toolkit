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
using System.Collections.Generic;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private int GetMaxId(string section)
        {
            int maxID = 0;
            
            List<int> allID = new List<int>();
            List<string> text = GetSectionText(section);
            List<List<string>> delimitted = new List<List<string>>();
            text.ForEach(x =>  delimitted.Add(x.Split(',').ToList()));

            if (!(text.Count() == 0))
            {
                foreach (List<string> line in delimitted)
                {
                    if (Int32.TryParse(line[0],out maxID))
                    {
                        allID.Add(maxID);
                    }
                }
                allID.Sort();
                allID.Reverse();
                maxID = allID[0];
            }
            else
            {
                maxID = 1;
            }

            return maxID;
        }

    }
}
