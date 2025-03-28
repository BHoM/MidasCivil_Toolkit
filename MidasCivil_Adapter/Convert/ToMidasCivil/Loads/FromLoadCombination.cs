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

using BH.oM.Structure.Loads;
using System.Collections.Generic;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static List<string> FromLoadCombination(this LoadCombination loadCombination, string version)
        {
            List<string> midasLoadCombination = new List<string>();

            string line1 = "";

            switch (version)
            {
                case "8.8.1":
                case "8.7.5":
                case "8.6.5":
                    line1 = "NAME=" + loadCombination.Name + ", GEN, ACTIVE, 0, 0, , 0, 0";
                    break;
                default:
                    line1 = "NAME=" + loadCombination.Name + ", GEN, ACTIVE, 0, 0, , 0, 0, 0";
                    break;
            }

            midasLoadCombination.Add(line1);
            string line2 =
                "ST, " + loadCombination.LoadCases[0].Item2.Name + "," + loadCombination.LoadCases[0].Item1.ToString();

            for (int i = 1; i < loadCombination.LoadCases.Count; i++)
            {
                line2 = line2 + ", ST, " + loadCombination.LoadCases[i].Item2.Name + "," + loadCombination.LoadCases[i].Item1.ToString();
            }

            midasLoadCombination.Add(line2);

            return midasLoadCombination;
        }

        /***************************************************/

    }
}




