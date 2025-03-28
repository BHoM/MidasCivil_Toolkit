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

using BH.oM.Adapters.MidasCivil;
using BH.Engine.Adapter;
using BH.oM.Structure.SurfaceProperties;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        public static ISurfaceProperty ToSurfaceProperty(this string surfaceProperty, string version, string lengthUnit)
        {
            /***************************************************/
            /**** Public Methods                            ****/
            /***************************************************/

            string[] split = surfaceProperty.Split(',');

            ISurfaceProperty constantThickness = null;

            switch (version)
            {
                case "8.8.5":
                    constantThickness = new ConstantThickness
                    {
                        Thickness = System.Convert.ToDouble(split[4].Trim()).LengthToSI(lengthUnit),
                        Name = split[1]
                    };
                    break;
                case "8.8.1":
                case "8.7.5":
                case "8.7.0":
                case "8.6.5":
                    constantThickness = new ConstantThickness
                    {
                        Thickness = System.Convert.ToDouble(split[3].Trim()).LengthToSI(lengthUnit),
                        Name = "t = " + split[3].Trim()
                    };
                    break;
                default:
                    constantThickness = new ConstantThickness
                    {
                        Thickness = System.Convert.ToDouble(split[4].Trim()).LengthToSI(lengthUnit),
                    };

                    if (split[2].Trim() != "1")
                        constantThickness.Name = split[2].Trim();
                    else
                        constantThickness.Name = "t = " + split[4].Trim();

                    break;
            }

            constantThickness.SetAdapterId(typeof(MidasCivilId), split[0].Trim());

            if (split[5].Trim() == "YES")
                Engine.Base.Compute.RecordWarning("SurfaceProperty objects do not have offsets implemented so this information will be lost");

            return constantThickness;
        }

        /***************************************************/

    }
}





