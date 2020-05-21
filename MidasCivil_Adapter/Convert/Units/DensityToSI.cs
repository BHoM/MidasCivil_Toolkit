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

        public static double DensityToSI(this double density, string forceUnit, string lengthUnit)
        {
            switch (forceUnit)
            {
                case "N":
                    switch (lengthUnit)
                    {
                        case "M":
                            break;
                        case "CM":
                            density.FromKilogramPerCubicCentimetre();
                            break;
                        case "MM":
                            density.FromKilogramPerCubicMillimetre();
                            break;
                        case "FT":
                            throw new Exception("No conversion method found for"+forceUnit+ " " +lengthUnit);
                        case "IN":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil moment unit assumed to be set to Newton-metres.Therefore no unit conversion will occur.");
                            break;
                    }
                    break;
                case "KN":
                    switch (lengthUnit)
                    {
                        case "M":
                            density.FromTonnePerCubicMetre();
                            break;
                        case "CM":
                            density.FromTonnePerCubicCentimetre();
                            break;
                        case "MM":
                            density.FromTonnePerCubicMillimetre();
                            break;
                        case "FT":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        case "IN":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            density.ToKilonewton();
                            break;
                    }
                    break;
                case "KGF":
                    switch (lengthUnit)
                    {
                        //The ToCubicLengthUnit is used because the volume quantity is on the denominator
                        //Recall density is mass/length^3
                        case "M":
                            density.FromKilogramForce();
                            break;
                        case "CM":
                            density.FromKilogramForce().ToCubicCentimetre();
                            break;
                        case "MM":
                            density.FromKilogramForce().ToCubicMillimetre();
                            break;
                        case "FT":
                            density.FromKilogramForce().ToFootPerSecondSquared().ToCubicFoot();
                            break;
                        case "IN":
                            density.FromKilogramForce().ToFootPerSecondSquared().ToCubicInch();
                            break;
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            density.FromKilogramForce();
                            break;
                    }
                    break;
                case "TONF":
                    switch (lengthUnit)
                    {
                        //The ToCubicLengthUnit is used because the volume quantity is on the denominator
                        //Recall density is mass/length^3
                        case "M":
                            density.FromTonneForce();
                            break;
                        case "CM":
                            density.FromTonneForce().ToCubicCentimetre();
                            break;
                        case "MM":
                            density.FromTonneForce().ToCubicMillimetre();
                            break;
                        case "FT":
                            density.FromTonneForce().ToFootPerSecondSquared().ToCubicFoot();
                            break;
                        case "IN":
                            density.FromTonneForce().ToFootPerSecondSquared().ToCubicInch();
                            break;
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            density.FromTonneForce();
                            break;
                    }
                    break;
                case "LBF":
                    switch (lengthUnit)
                    {
                        //The ToCubicLengthUnit is used because the volume quantity is on the denominator
                        //Recall density is mass/length^3
                        case "M":
                            density.FromPoundForce();
                            break;
                        case "CM":
                            density.FromPoundForce().ToCubicCentimetre();
                            break;
                        case "MM":
                            density.FromPoundForce().ToCubicMillimetre();
                            break;
                        case "FT":
                            density.FromPoundForce().ToFootPerSecondSquared().ToCubicFoot();
                            break;
                        case "IN":
                            density.FromPoundForce().ToFootPerSecondSquared().ToCubicInch();
                            break;
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            density.FromPoundForce();
                            break;
                    }
                    break;
                case "KIPS":
                    switch (lengthUnit)
                    {
                        //The ToCubicLengthUnit is used because the volume quantity is on the denominator
                        //Recall density is mass/length^3
                        case "M":
                            density.FromKilopoundForce();
                            break;
                        case "CM":
                            density.FromKilopoundForce().ToCubicCentimetre();
                            break;
                        case "MM":
                            density.FromKilopoundForce().ToCubicMillimetre();
                            break;
                        case "FT":
                            density.FromKilopoundForce().ToFootPerSecondSquared().ToCubicFoot();
                            break;
                        case "IN":
                            density.FromKilopoundForce().ToFootPerSecondSquared().ToCubicInch();
                            break;
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            density.FromKilopoundForce();
                            break;
                    }
                    break;
                default:
                    Compute.RecordWarning("No force unit detected, MidasCivil force unit assumed to be set to Newtons. Therefore no unit conversion will occur. ");
                    break;
            }

            return density;
        }

        /***************************************************/

    }
}
