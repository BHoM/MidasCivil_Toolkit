﻿/*
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

using BH.oM.Structure.Loads;
using BH.oM.Structure.Results;
using System.Linq;
using System.Collections.Generic;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static BarStress ToBarStress(this List<string> delimitted)
        {
            double position = GetBarResultPosition(delimitted[7]);

            BarStress barstress = new BarStress()
            {
                ObjectId = System.Convert.ToInt32(delimitted[2]),
                ResultCase = delimitted[3],
                Axial = System.Convert.ToDouble(delimitted[10]),
                ShearY = System.Convert.ToDouble(delimitted[11]),
                ShearZ = System.Convert.ToDouble(delimitted[12]),
                BendingY_Bot = System.Convert.ToDouble(delimitted[14]),
                BendingY_Top = System.Convert.ToDouble(delimitted[13]),
                BendingZ_Bot = System.Convert.ToDouble(delimitted[16]),
                BendingZ_Top = System.Convert.ToDouble(delimitted[15]),
                Position = position,
                Divisions = 0
            };

            return barstress;
        }

    }
}


