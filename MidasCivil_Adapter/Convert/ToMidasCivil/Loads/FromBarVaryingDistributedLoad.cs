/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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

using BH.oM.Structure.Loads;
using BH.oM.Geometry;
using System.Collections.Generic;
using BH.oM.Structure.Elements;
using BH.Engine.Spatial;


namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static string FromBarVaryingDistributedLoad(this BarVaryingDistributedLoad barLoad, string assignedBar, string loadType, string forceUnit, string lengthUnit, double startPosition, double endPosition)
        {

            string midasBarLoad = null;

            if (loadType == "Force")
            {
                string direction = FromVector(barLoad.ForceAtStart);
                midasBarLoad = assignedBar + ",BEAM,UNILOAD," + FromLoadAxis(barLoad.Axis) + direction +
                                                                    "," + FromLoadProjection(barLoad.Projected) +
                                                                    ",NO,aDir[1], , , ," + startPosition + "," +
                                                                    FromVectorDirection(barLoad.ForceAtStart, direction).ForcePerLengthFromSI(forceUnit, lengthUnit).ToString() + "," +
                                                                    endPosition + ","
                                                                    + FromVectorDirection(barLoad.ForceAtEnd, direction).ForcePerLengthFromSI(forceUnit, lengthUnit).ToString() +
                                                                    ",0,0,0,0," + barLoad.Name + ",NO,0,0,NO";
            }
            else
            {
                string direction = FromVector(barLoad.MomentAtStart);
                midasBarLoad = assignedBar + ",BEAM,UNIMOMENT," + FromLoadAxis(barLoad.Axis) + direction +
                                                                    "," + FromLoadProjection(barLoad.Projected) +
                                                                    ",NO,aDir[1], , , ," + startPosition + "," +
                                                                    FromVectorDirection(barLoad.MomentAtStart, direction).MomentPerLengthFromSI(forceUnit, lengthUnit).ToString() + "," +
                                                                    endPosition + ","
                                                                    + FromVectorDirection(barLoad.MomentAtEnd, direction).MomentPerLengthFromSI(forceUnit, lengthUnit).ToString() +
                                                                    ",0,0,0,0," + barLoad.Name + ",NO,0,0,NO";
            }

            return midasBarLoad;
        }

        /***************************************************/

    }
}



