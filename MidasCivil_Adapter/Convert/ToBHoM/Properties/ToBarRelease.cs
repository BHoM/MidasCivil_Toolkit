/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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

using BH.oM.Adapters.MidasCivil;
using BH.Engine.Adapter;
using BH.oM.Structure.Constraints;
using System.Collections.Generic;
using System.Linq;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static BarRelease ToBarRelease(string release, int count)
        {
            List<string> delimitted = release.Split(',').ToList();

            string releaseName = "";

            string startFixity = delimitted[1].Trim();
            string endFixity = delimitted[8].Trim();

            List<bool> bhomStartFixity = new List<bool>();
            List<bool> bhomEndFixity = new List<bool>();

            for (int i = 0; i < 6; i++)
            {
                bhomStartFixity.Add(FromFixity(startFixity.Substring(i, 1)));
                bhomEndFixity.Add(FromFixity(endFixity.Substring(i, 1)));
            }

            Constraint6DOF startConstraint = Engine.Structure.Create.Constraint6DOF(bhomStartFixity[0], bhomStartFixity[1], bhomStartFixity[2],
                                                                                       bhomStartFixity[3], bhomStartFixity[4], bhomStartFixity[5], "StartConstraint");
            Constraint6DOF endConstraint = Engine.Structure.Create.Constraint6DOF(bhomEndFixity[0], bhomEndFixity[1], bhomEndFixity[2],
                                                                                     bhomEndFixity[3], bhomEndFixity[4], bhomEndFixity[5], "EndConstraint");

            if (!string.IsNullOrWhiteSpace(delimitted[15]))
            {
                releaseName = delimitted[15].Trim();
            }
            else
            {
                releaseName = "BR" + count.ToString();
            }

            BarRelease bhomBarRelease = Engine.Structure.Create.BarRelease(startConstraint, endConstraint, releaseName);
            bhomBarRelease.SetAdapterId(typeof(MidasCivilId), bhomBarRelease.Name);

            return bhomBarRelease;
        }

        /***************************************************/

    }
}

