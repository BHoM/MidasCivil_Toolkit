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

        public static double DensityFromSI(this double density, string forceUnit, string lengthUnit)
        {
            switch (forceUnit)
            {
                case "N":
                    switch (lengthUnit)
                    {
                        case "M":
                            break;
                        case "CM":
                            density.ToKilogramPerCubicCentimetre();
                            break;
                        case "MM":
                            density.ToKilogramPerCubicMillimetre();
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
                            density.ToTonnePerCubicMetre();
                            break;
                        case "CM":
                            density.ToTonnePerCubicCentimetre();
                            break;
                        case "MM":
                            density.ToTonnePerCubicMillimetre();
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
                        //The FromCubicUnitLength is used because the volume quantity is on the denominator
                        //Recall density is mass/length^3
                        case "M":
                            density.ToKilogramForce();
                            break;
                        case "CM":
                            density.ToKilogramForce().FromCubicCentimetre();
                            break;
                        case "MM":
                            density.ToKilogramForce().FromCubicMillimetre();
                            break;
                        case "FT":
                            density.ToKilogramForce().FromFootPerSecondSquared().FromCubicFoot();
                            break;
                        case "IN":
                            density.ToKilogramForce().FromFootPerSecondSquared().FromCubicInch();
                            break;
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            density.ToKilogramForce();
                            break;
                    }
                    break;
                case "TONF":
                    switch (lengthUnit)
                    {
                        //The FromCubicUnitLength is used because the volume quantity is on the denominator
                        //Recall density is mass/length^3
                        case "M":
                            density.ToTonneForce();
                            break;
                        case "CM":
                            density.ToTonneForce().FromCubicCentimetre();
                            break;
                        case "MM":
                            density.ToTonneForce().FromCubicMillimetre();
                            break;
                        case "FT":
                            density.ToTonneForce().FromFootPerSecondSquared().FromCubicFoot();
                            break;
                        case "IN":
                            density.ToTonneForce().FromFootPerSecondSquared().FromCubicInch();
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
                        //The FromCubicUnitLength is used because the volume quantity is on the denominator
                        //Recall density is mass/length^3
                        case "M":
                            density.ToPoundForce();
                            break;
                        case "CM":
                            density.ToPoundForce().FromCubicCentimetre();
                            break;
                        case "MM":
                            density.ToPoundForce().FromCubicMillimetre();
                            break;
                        case "FT":
                            density.ToPoundForce().FromFootPerSecondSquared().FromCubicFoot();
                            break;
                        case "IN":
                            density.ToPoundForce().FromFootPerSecondSquared().FromCubicInch();
                            break;
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            density.ToPoundForce();
                            break;
                    }
                    break;
                case "KIPS":
                    switch (lengthUnit)
                    {
                        //The FromCubicUnitLength is used because the volume quantity is on the denominator
                        //Recall density is mass/length^3
                        case "M":
                            density.ToKilopoundForce();
                            break;
                        case "CM":
                            density.ToKilopoundForce().FromCubicCentimetre();
                            break;
                        case "MM":
                            density.ToKilopoundForce().FromCubicMillimetre();
                            break;
                        case "FT":
                            density.ToKilopoundForce().FromFootPerSecondSquared().FromCubicFoot();
                            break;
                        case "IN":
                            density.ToKilopoundForce().FromFootPerSecondSquared().FromCubicInch();
                            break;
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            density.ToKilopoundForce();
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
