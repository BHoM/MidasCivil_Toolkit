/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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
using BH.Engine.Base;
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

        public static double ForcePerLengthToSI(this double forcePerLength, string forceUnit, string lengthUnit)
        {
            switch (forceUnit)
            {
                case "N":
                    switch (lengthUnit)
                    {
                        case "M":
                            break;
                        case "CM":
                            return forcePerLength.FromNewtonPerCentimetre();
                        case "MM":
                            return forcePerLength.FromNewtonPerMillimetre();
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
                            return forcePerLength.FromKilonewtonPerMetre();
                        case "CM":
                            return forcePerLength.FromKilonewtonPerCentimetre();
                        case "MM":
                            return forcePerLength.FromKilonewtonPerMillimetre();
                        case "FT":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        case "IN":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            return forcePerLength.FromKilonewton();
                    }
                case "KGF":
                    switch (lengthUnit)
                    {
                        case "M":
                            return forcePerLength.FromKilogramForcePerMetre();
                        case "CM":
                            return forcePerLength.FromKilogramForcePerCentimetre();
                        case "MM":
                            return forcePerLength.FromKilogramForcePerMillimetre();
                        case "FT":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        case "IN":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            return forcePerLength.FromKilogramForce();
                    }
                case "TONF":
                    switch (lengthUnit)
                    {
                        case "M":
                            return forcePerLength.FromTonneForce();
                        case "CM":
                            return forcePerLength.FromTonneForcePerCentimetre();
                        case "MM":
                            return forcePerLength.FromTonneForcePerMillimetre();
                        case "FT":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        case "IN":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            return forcePerLength.FromTonneForce();
                    }
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
                            return forcePerLength.FromPoundForcePerFoot();
                        case "IN":
                            return forcePerLength.FromPoundForcePerInch();
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            return forcePerLength.FromPoundForce();
                    }
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
                            return forcePerLength.FromKilopoundForcePerFoot();
                        case "IN":
                            return forcePerLength.FromKilopoundForcePerInch();
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            return forcePerLength.FromKilopoundForce();
                    }
                default:
                    Compute.RecordWarning("No force unit detected, MidasCivil force unit assumed to be set to Newtons. Therefore no unit conversion will occur. ");
                    break;
            }

            return forcePerLength;
        }

        /***************************************************/

    }
}



