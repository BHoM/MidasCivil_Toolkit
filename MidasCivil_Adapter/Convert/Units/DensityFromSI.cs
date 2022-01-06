/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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
                            return density.ToKilogramPerCubicCentimetre();
                        case "MM":
                            return density.ToKilogramPerCubicMillimetre();
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
                            return density.ToTonnePerCubicMetre();
                        case "CM":
                            return density.ToTonnePerCubicCentimetre();
                        case "MM":
                            return density.ToTonnePerCubicMillimetre();
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
                        //The FromCubicUnitLength is used because the volume quantity is on the denominator
                        //Recall density is mass/length^3
                        case "M":
                            return density.ToKilogramForce();
                        case "CM":
                            return density.ToKilogramForce().FromCubicCentimetre();
                        case "MM":
                            return density.ToKilogramForce().FromCubicMillimetre();
                        case "FT":
                            return density.ToKilogramForce().FromFootPerSecondSquared().FromCubicFoot();
                        case "IN":
                            return density.ToKilogramForce().FromFootPerSecondSquared().FromCubicInch();
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            return density.ToKilogramForce();
                    }
                case "TONF":
                    switch (lengthUnit)
                    {
                        //The FromCubicUnitLength is used because the volume quantity is on the denominator
                        //Recall density is mass/length^3
                        case "M":
                            return density.ToTonneForce();
                        case "CM":
                            return density.ToTonneForce().FromCubicCentimetre();
                        case "MM":
                            return density.ToTonneForce().FromCubicMillimetre();
                        case "FT":
                            return density.ToTonneForce().FromFootPerSecondSquared().FromCubicFoot();
                        case "IN":
                            return density.ToTonneForce().FromFootPerSecondSquared().FromCubicInch();
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            return density.FromTonneForce();
                    }
                case "LBF":
                    switch (lengthUnit)
                    {
                        //The FromCubicUnitLength is used because the volume quantity is on the denominator
                        //Recall density is mass/length^3
                        case "M":
                            return density.ToPoundForce();
                        case "CM":
                            return density.ToPoundForce().FromCubicCentimetre();
                        case "MM":
                            return density.ToPoundForce().FromCubicMillimetre();
                        case "FT":
                            return density.ToPoundForce().FromFootPerSecondSquared().FromCubicFoot();
                        case "IN":
                            return density.ToPoundForce().FromFootPerSecondSquared().FromCubicInch();
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            return density.ToPoundForce();
                    }
                case "KIPS":
                    switch (lengthUnit)
                    {
                        //The FromCubicUnitLength is used because the volume quantity is on the denominator
                        //Recall density is mass/length^3
                        case "M":
                            return density.ToKilopoundForce();
                        case "CM":
                            return density.ToKilopoundForce().FromCubicCentimetre();
                        case "MM":
                            return density.ToKilopoundForce().FromCubicMillimetre();
                        case "FT":
                            return density.ToKilopoundForce().FromFootPerSecondSquared().FromCubicFoot();
                        case "IN":
                            return density.ToKilopoundForce().FromFootPerSecondSquared().FromCubicInch();
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            return density.ToKilopoundForce();
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


