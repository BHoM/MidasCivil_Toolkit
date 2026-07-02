/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2026, the respective contributors. All rights reserved.
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
using BH.oM.Structure.Results;
using System.Linq;
using System.Collections.Generic;
using BH.Adapter.Adapters.MidasCivil;
using System;

namespace BH.Adapter.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static BarStress ToBarStress(this List<string> delimitted, string forceUnit, string lengthUnit)
        {
            double position = GetBarResultPosition(delimitted[7]);
            //TODO: resolve below identifiers extractable through the API
            int mode = -1;
            double timeStep = 0;
            int divisions = 0;

            BarStress barstress = new BarStress(
                System.Convert.ToInt32(delimitted[2]),
                delimitted[3],
                mode,
                timeStep,
                position,
                divisions,
                System.Convert.ToDouble(delimitted[10]).ForceToSI(forceUnit),
                System.Convert.ToDouble(delimitted[11]).ForceToSI(forceUnit),
                System.Convert.ToDouble(delimitted[12]).ForceToSI(forceUnit),
                System.Convert.ToDouble(delimitted[14]).MomentToSI(forceUnit, lengthUnit),
                System.Convert.ToDouble(delimitted[13]).MomentToSI(forceUnit, lengthUnit),
                System.Convert.ToDouble(delimitted[16]).MomentToSI(forceUnit, lengthUnit),
                System.Convert.ToDouble(delimitted[15]).MomentToSI(forceUnit, lengthUnit),
                0,
                0
                );
				
            return barstress;
        }

        /***************************************************/

        public static BarStress ToBarStress(List<object> item)
        {
            double position = GetBarResultPosition(item[3].ToString());

            List<double> combBends = new List<double> { 
                System.Convert.ToDouble(item[11].ToString()), 
                System.Convert.ToDouble(item[12].ToString()), 
                System.Convert.ToDouble(item[13].ToString()), 
                System.Convert.ToDouble(item[14].ToString()) 
            };
            
            double combBendPos = combBends.Max();
            double combBendNeg = combBends.Min();

            //TODO: resolve below identifiers extractable through the API
            int mode = -1;
            double timeStep = 0;
            int divisions = 0;

            BarStress barstress = new BarStress(
                System.Convert.ToInt32(item[1].ToString()),
                item[2].ToString(),
                mode,
                timeStep,
                position,
                divisions,
                System.Convert.ToDouble(item[4].ToString()),
                System.Convert.ToDouble(item[5].ToString()),
                System.Convert.ToDouble(item[6].ToString()),
                System.Convert.ToDouble(item[7].ToString()),
                System.Convert.ToDouble(item[8].ToString()),
                System.Convert.ToDouble(item[9].ToString()),
                System.Convert.ToDouble(item[10].ToString()),
                combBendPos,
                combBendNeg
                );

            return barstress;
        }

        /***************************************************/
    }
}








