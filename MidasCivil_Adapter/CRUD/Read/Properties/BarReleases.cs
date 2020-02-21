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
using BH.oM.Structure.Constraints;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<BarRelease> ReadBarReleases(List<string> ids = null)
        {
            List<BarRelease> bhomBarReleases = new List<BarRelease>();

            int count = 1;

            List<string> barReleaseText = GetSectionText("FRAME-RLS");

            if (barReleaseText.Count != 0)
            {
                List<string> barComparison = new List<string>();
                List<string> releasedBars = new List<string>();

                for (int i = 0; i < barReleaseText.Count; i += 2)
                {
                    List<string> delimitted = barReleaseText[i].Split(',').ToList();
                    releasedBars.Add(delimitted[0].Trim());
                    delimitted.RemoveAt(0);
                    barComparison.Add(String.Join(",", delimitted).Trim() + "," + barReleaseText[i + 1].Trim());
                }

                List<string> distinctBarReleases = barComparison.Distinct().ToList();

                foreach (string distinctBarRelease in distinctBarReleases)
                {
                    List<int> indexMatches = barComparison.Select((barload, index) => new { barload, index })
                                               .Where(x => string.Equals(x.barload, distinctBarRelease))
                                               .Select(x => x.index)
                                               .ToList();
                    List<string> matchingBars = new List<string>();
                    indexMatches.ForEach(x => matchingBars.Add(releasedBars[x]));

                    BarRelease bhomBarRelease = Engine.MidasCivil.Convert.ToBarRelease(distinctBarRelease, count);
                    bhomBarReleases.Add(bhomBarRelease);

                    if ((distinctBarRelease.Split(',').ToList()[15].ToString() == " "))
                    {
                        count = count + 1;
                    }

                }
            }

            return bhomBarReleases;
        }

    }
}
