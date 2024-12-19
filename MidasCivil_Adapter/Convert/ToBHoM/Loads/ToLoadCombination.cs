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
using BH.oM.Adapters.MidasCivil;
using BH.Engine.Adapter;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Loads;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static LoadCombination ToLoadCombination(string loadCombination1, string loadCombination2, Dictionary<string, Loadcase> bhomLoadCaseDictionary)
        {
            List<string> delimittedLine1 = loadCombination1.Split(',').ToList();
            List<string> delimittedLine2 = loadCombination2.Split(',').ToList();

            List<Loadcase> associatedLoadcases = new List<Loadcase>();
            List<double> loadFactors = new List<double>();

            for (int i = 1; i < delimittedLine2.Count; i += 3)
            {
                Loadcase bhomLoadcase;
                bhomLoadCaseDictionary.TryGetValue(delimittedLine2[i].Trim(), out bhomLoadcase);
                associatedLoadcases.Add(bhomLoadcase);
                loadFactors.Add(double.Parse(delimittedLine2[i + 1].Trim()));
            }

            string name = delimittedLine1[0].Split('=')[1].Trim();
            int number = 0;

            LoadCombination bhomLoadCombination = BH.Engine.Structure.Create.LoadCombination(name, number, associatedLoadcases, loadFactors);
            bhomLoadCombination.SetAdapterId(typeof(MidasCivilId), name);

            return bhomLoadCombination;
        }

        /***************************************************/

    }
}






