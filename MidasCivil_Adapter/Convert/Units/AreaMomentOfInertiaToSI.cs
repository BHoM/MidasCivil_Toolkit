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

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static double AreaMomentOfInertiaToSI(this double areaMomentOfInertia, string lengthUnit)
        {
            switch (lengthUnit)
            {
                case "M":
                    break;
                case "CM":
                    return areaMomentOfInertia.FromCentimetreToTheFourth();
                case "MM":
                    return areaMomentOfInertia.FromMillimetreToTheFourth();
                case "FT":
                    return areaMomentOfInertia.FromFootToTheFourth();
                case "IN":
                    return areaMomentOfInertia.FromInchToTheFourth();
                default:
                    Compute.RecordWarning("No length unit detected, MidasCivil length unit assumed to be set to metres. Therefore no unit conversion will occur.");
                    break;
            }

            return areaMomentOfInertia;
        }

        /***************************************************/

    }
}



