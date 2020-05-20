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

        public static double MomentFromSI(this double moment, string forceUnit, string lengthUnit)
        {
            switch (forceUnit)
            {
                case "N":
                    switch (lengthUnit)
                    {
                        case "M":
                            break;
                        case "CM":
                            moment.ToNewtonCentimetre();
                            break;
                        case "MM":
                            moment.ToNewtonMillimetre();
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
                            moment.ToKilonewtonCentimetre();
                            break;
                        case "CM":
                            moment.ToKilonewtonCentimetre();
                            break;
                        case "MM":
                            moment.ToKilonewtonMillimetre();
                            break;
                        case "FT":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        case "IN":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            moment.ToKilonewton();
                            break;
                    }
                    break;
                case "KGF":
                    switch (lengthUnit)
                    {
                        case "M":
                            break;
                        case "CM":
                            moment.ToKilogramForceCentimetre();
                            break;
                        case "MM":
                            moment.ToKilogramForceMillimetre();
                            break;
                        case "FT":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        case "IN":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            moment.ToKilogramForce();
                            break;
                    }
                    break;
                case "TONF":
                    switch (lengthUnit)
                    {
                        case "M":
                            moment.ToTonneForceMetre();
                            break;
                        case "CM":
                            moment.ToTonneForceCentimetre();
                            break;
                        case "MM":
                            moment.ToTonneForceMillimetre();
                            break;
                        case "FT":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        case "IN":
                            throw new Exception("No conversion method found for" + forceUnit + " " + lengthUnit);
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            moment.ToTonneForce();
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
                            moment.ToPoundForceFoot();
                            break;
                        case "IN":
                            moment.ToPoundForceInch();
                            break;
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            moment.ToPoundForce();
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
                            moment.ToKilopoundForceFoot();
                            break;
                        case "IN":
                            moment.ToKilopoundForceInch();
                            break;
                        default:
                            Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres.");
                            moment.ToKilopoundForce();
                            break;
                    }
                    break;
                default:
                    Compute.RecordWarning("No force unit detected, MidasCivil force unit assumed to be set to Newtons. Therefore no unit conversion will occur. ");
                    break;
            }

            return moment;
        }

        /***************************************************/

    }
}
