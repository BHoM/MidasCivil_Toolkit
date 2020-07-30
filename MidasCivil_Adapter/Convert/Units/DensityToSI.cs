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
                            return density.FromKilogramPerCubicCentimetre();
                        case "MM":
                            return density.FromKilogramPerCubicMillimetre();
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
                            return density.FromTonnePerCubicMetre();
                        case "CM":
                            return density.FromTonnePerCubicCentimetre();
                        case "MM":
                            return density.FromTonnePerCubicMillimetre();
                        case "FT":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        case "IN":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            return density.ToKilonewton();
                    }
                case "KGF":
                    switch (lengthUnit)
                    {
                        //The ToCubicLengthUnit is used because the volume quantity is on the denominator
                        //Recall density is mass/length^3
                        case "M":
                            return density.FromKilogramForce();
                        case "CM":
                            return density.FromKilogramForce().ToCubicCentimetre();
                        case "MM":
                            return density.FromKilogramForce().ToCubicMillimetre();
                        case "FT":
                            return density.FromKilogramForce().ToFootPerSecondSquared().ToCubicFoot();
                        case "IN":
                            return density.FromKilogramForce().ToFootPerSecondSquared().ToCubicInch();
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            return density.FromKilogramForce();
                    }
                case "TONF":
                    switch (lengthUnit)
                    {
                        //The ToCubicLengthUnit is used because the volume quantity is on the denominator
                        //Recall density is mass/length^3
                        case "M":
                            return density.FromTonneForce();
                        case "CM":
                            return density.FromTonneForce().ToCubicCentimetre();
                        case "MM":
                            return density.FromTonneForce().ToCubicMillimetre();
                        case "FT":
                            return density.FromTonneForce().ToFootPerSecondSquared().ToCubicFoot();
                        case "IN":
                            return density.FromTonneForce().ToFootPerSecondSquared().ToCubicInch();
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            return density.FromTonneForce();
                    }
                case "LBF":
                    switch (lengthUnit)
                    {
                        //The ToCubicLengthUnit is used because the volume quantity is on the denominator
                        //Recall density is mass/length^3
                        case "M":
                            return density.FromPoundForce();
                        case "CM":
                            return density.FromPoundForce().ToCubicCentimetre();
                        case "MM":
                            return density.FromPoundForce().ToCubicMillimetre();
                        case "FT":
                            return density.FromPoundForce().ToFootPerSecondSquared().ToCubicFoot();
                        case "IN":
                            return density.FromPoundForce().ToFootPerSecondSquared().ToCubicInch();
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            return density.FromPoundForce();
                    }
                case "KIPS":
                    switch (lengthUnit)
                    {
                        //The ToCubicLengthUnit is used because the volume quantity is on the denominator
                        //Recall density is mass/length^3
                        case "M":
                            return density.FromKilopoundForce();
                        case "CM":
                            return density.FromKilopoundForce().ToCubicCentimetre();
                        case "MM":
                            return density.FromKilopoundForce().ToCubicMillimetre();
                        case "FT":
                            return density.FromKilopoundForce().ToFootPerSecondSquared().ToCubicFoot();
                        case "IN":
                            return density.FromKilopoundForce().ToFootPerSecondSquared().ToCubicInch();
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            return density.FromKilopoundForce();
                    }
                default:
                    Compute.RecordWarning("No force unit detected, MidasCivil force unit assumed to be set to Newtons. Therefore no unit conversion will occur. ");
                    break;
            }

            return density;
        }

        /***************************************************/

    }
}
