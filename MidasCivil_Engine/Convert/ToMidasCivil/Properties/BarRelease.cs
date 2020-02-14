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

using BH.oM.Structure.Constraints;
using System.Collections.Generic;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static List<string> ToMCBarRelease(this BarRelease barRelease)
        {
            List<string> midasRelease = new List<string>();

            string startFixity = boolToConstraint(barRelease.StartRelease.TranslationX) +
                                    boolToConstraint(barRelease.StartRelease.TranslationY) +
                                    boolToConstraint(barRelease.StartRelease.TranslationZ) +
                                    boolToConstraint(barRelease.StartRelease.RotationX) +
                                    boolToConstraint(barRelease.StartRelease.RotationY) +
                                    boolToConstraint(barRelease.StartRelease.RotationZ);

            string endFixity = boolToConstraint(barRelease.EndRelease.TranslationX) +
                                    boolToConstraint(barRelease.EndRelease.TranslationY) +
                                    boolToConstraint(barRelease.EndRelease.TranslationZ) +
                                    boolToConstraint(barRelease.EndRelease.RotationX) +
                                    boolToConstraint(barRelease.EndRelease.RotationY) +
                                    boolToConstraint(barRelease.EndRelease.RotationZ);

            midasRelease.Add(",NO," + startFixity + ",0,0,0,0,0,0");
            midasRelease.Add(endFixity + ",0,0,0,0,0,0," + barRelease.Name);

            return midasRelease;
        }

        public static string boolToConstraint(DOFType fixity)
        {
            string converted = "0";

            if (fixity == DOFType.Free)
            {
                converted = "1";
            }

            return converted;
        }
    }
}