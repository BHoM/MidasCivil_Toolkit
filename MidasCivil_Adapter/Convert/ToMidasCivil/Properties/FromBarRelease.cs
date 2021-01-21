/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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

using BH.Engine.Structure;
using BH.oM.Structure.Constraints;

using System.IO;
using System.Collections.Generic;
using System.Linq;
using BH.Adapter.MidasCivil;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static List<string> FromBarRelease(this BarRelease barRelease, int groupCharacterLimit)
        {
            List<string> midasRelease = new List<string>();

            string startFixity = FromDOFType(barRelease.StartRelease.TranslationX) +
                                    FromDOFType(barRelease.StartRelease.TranslationY) +
                                    FromDOFType(barRelease.StartRelease.TranslationZ) +
                                    FromDOFType(barRelease.StartRelease.RotationX) +
                                    FromDOFType(barRelease.StartRelease.RotationY) +
                                    FromDOFType(barRelease.StartRelease.RotationZ);

            string endFixity = FromDOFType(barRelease.EndRelease.TranslationX) +
                                    FromDOFType(barRelease.EndRelease.TranslationY) +
                                    FromDOFType(barRelease.EndRelease.TranslationZ) +
                                    FromDOFType(barRelease.EndRelease.RotationX) +
                                    FromDOFType(barRelease.EndRelease.RotationY) +
                                    FromDOFType(barRelease.EndRelease.RotationZ);

            midasRelease.Add(",NO," + startFixity + ",0,0,0,0,0,0");
            midasRelease.Add(endFixity + ",0,0,0,0,0,0," + new string(barRelease.DescriptionOrName().Replace(",","").Take(groupCharacterLimit).ToArray()));

            return midasRelease;
        }

        /***************************************************/

    }
}
