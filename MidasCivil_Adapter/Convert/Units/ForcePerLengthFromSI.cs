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

using BH.Adapter.MidasCivil;
using BH.oM.Geometry;
using BH.Engine.Reflection;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.Elements;
using BH.Engine.Units;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static double ForcePerLengthFromSI(this double forcePerLength, string forceUnit, string lengthUnit)
        {
            switch (forceUnit)
            {
                case "N":
                    switch (lengthUnit)
                    {
                        case "M":
                            break;
                        case "CM":
                            forcePerLength.ToNewtonPerCentimetre();
                            break;
                        case "MM":
                            forcePerLength.ToNewtonPerMillimetre();
                            break;
                        case "FT":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        case "IN":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil pressure unit assumed to be set to Newtons per square metre.Therefore no unit conversion will occur.");
                            break;
                    }
                    break;
                case "KN":
                    switch (lengthUnit)
                    {
                        case "M":
                            forcePerLength.ToKilonewtonPerMetre();
                            break;
                        case "CM":
                            forcePerLength.ToKilonewtonPerCentimetre();
                            break;
                        case "MM":
                            forcePerLength.ToKilonewtonPerMillimetre();
                            break;
                        case "FT":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        case "IN":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            forcePerLength.ToKilonewton();
                            break;
                    }
                    break;
                case "KGF":
                    switch (lengthUnit)
                    {
                        case "M":
                            forcePerLength.ToKilogramForcePerMetre();
                            break;
                        case "CM":
                            forcePerLength.ToKilogramForcePerCentimetre();
                            break;
                        case "MM":
                            forcePerLength.ToKilogramForcePerMillimetre();
                            break;
                        case "FT":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        case "IN":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            forcePerLength.ToKilogramForce();
                            break;
                    }
                    break;
                case "TONF":
                    switch (lengthUnit)
                    {
                        case "M":
                            forcePerLength.ToTonneForce();
                            break;
                        case "CM":
                            forcePerLength.ToTonneForcePerCentimetre();
                            break;
                        case "MM":
                            forcePerLength.ToTonneForcePerMillimetre();
                            break;
                        case "FT":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        case "IN":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            forcePerLength.ToTonneForce();
                            break;
                    }
                    break;
                case "LBF":
                    switch (lengthUnit)
                    {
                        case "M":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        case "CM":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        case "MM":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        case "FT":
                            forcePerLength.ToPoundForcePerFoot();
                            break;
                        case "IN":
                            forcePerLength.ToPoundForcePerInch();
                            break;
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            forcePerLength.ToPoundForce();
                            break;
                    }
                    break;
                case "KIPS":
                    switch (lengthUnit)
                    {
                        case "M":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        case "CM":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        case "MM":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        case "FT":
                            forcePerLength.ToKilopoundForcePerFoot();
                            break;
                        case "IN":
                            forcePerLength.ToKilopoundForcePerInch();
                            break;
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            forcePerLength.ToKilopoundForce();
                            break;
                    }
                    break;
                default:
                    Compute.RecordWarning("No force unit detected, MidasCivil force unit assumed to be set to Newtons. Therefore no unit conversion will occur. ");
                    break;
            }

            return forcePerLength;
        }

        /***************************************************/

    }
}
