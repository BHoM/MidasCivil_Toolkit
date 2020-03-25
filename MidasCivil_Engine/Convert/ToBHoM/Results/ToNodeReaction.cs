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
        public static NodeReaction ToNodeReaction(this List<string> delimitted)
        {
            NodeReaction nodeReaction = new NodeReaction()
            {
                ObjectId = System.Convert.ToInt32(delimitted[2]),
                ResultCase = delimitted[3],
                FX = System.Convert.ToDouble(delimitted[7]),
                FY = System.Convert.ToDouble(delimitted[8]),
                FZ = System.Convert.ToDouble(delimitted[9]),
                MX = System.Convert.ToDouble(delimitted[10]),
                MY = System.Convert.ToDouble(delimitted[11]),
                MZ = System.Convert.ToDouble(delimitted[12])
            };
            return nodeReaction;
        }

    }
    public static partial class Convert
    {
        public static BarForce ToBarForce(this List<string> delimitted)
        {
            BarForce barforce = new BarForce()
            {
                ObjectId = System.Convert.ToInt32(delimitted[4]),
                ResultCase = delimitted[5],
                FX = System.Convert.ToDouble(delimitted[12]),
                FY = System.Convert.ToDouble(delimitted[13]),
                FZ = System.Convert.ToDouble(delimitted[14]),
                MX = System.Convert.ToDouble(delimitted[15]),
                MY = System.Convert.ToDouble(delimitted[16]),
                MZ = System.Convert.ToDouble(delimitted[17])
            };
            return barforce;
        }
        public static BarStress ToBarStress(this List<string> delimitted)
        {
            double position = 0;
            if (delimitted[7].Contains('['))
            {
                if (delimitted[3].Split('[')[0].Trim() == "J")
                {
                    position = 1;
                }
            }
            else if (delimitted[7].Contains('/'))
            {
                List<string> splitPosition = delimitted[7].Split('/').ToList();
                position = System.Convert.ToDouble(splitPosition[0]) / System.Convert.ToDouble(splitPosition[1]);
            }

            BarStress barstress = new BarStress()
            {
                ObjectId = System.Convert.ToInt32(delimitted[3]),
                ResultCase = delimitted[4],
                Axial = System.Convert.ToDouble(delimitted[11]),
                BendingY_Bot = System.Convert.ToDouble(delimitted[15]),
                BendingY_Top = System.Convert.ToDouble(delimitted[14]),
                BendingZ_Bot = System.Convert.ToDouble(delimitted[17]),
                BendingZ_Top = System.Convert.ToDouble(delimitted[16]),
                CombAxialBendingNeg = System.Convert.ToDouble(delimitted[16]),
                CombAxialBendingPos = System.Convert.ToDouble(0),
                ShearY = 0,
                ShearZ = 0,
                Position = position,
                Divisions = 0




            };
            return barstress;
        }

    }
}

