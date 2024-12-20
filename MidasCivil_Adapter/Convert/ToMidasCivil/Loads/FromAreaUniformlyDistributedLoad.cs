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

using BH.oM.Structure.Loads;
using BH.oM.Geometry;
using System.Collections.Generic;
using System.Linq;
using System;
using BH.oM.Structure.Elements;
using BH.Engine.Geometry;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static string FromAreaUniformlyDistributedLoad(this AreaUniformlyDistributedLoad femeshLoad, string assignedFEMesh, string version,
            string forceUnit, string lengthUnit)
        {
            string direction = FromVector(femeshLoad.Pressure);
            string midasFEMeshLoad = "";

            switch (version)
            {
                case "9.4.0":
                case "9.1.0":
                case "9.4.5":
                case "9.5.0":
                    midasFEMeshLoad = assignedFEMesh + ", PRES, PLATE, FACE, " + FromLoadAxis(femeshLoad.Axis) + direction +
                                ", 0, 0, 0, " + FromLoadProjection(femeshLoad.Projected) + ", " +
                                FromVectorDirection(femeshLoad.Pressure, direction).PressureFromSI(forceUnit, lengthUnit).ToString() +
                                ", 0, 0, 0, 0, " + femeshLoad.Name + ",0";
                    break;
                default:
                    midasFEMeshLoad = assignedFEMesh + ", PRES, PLATE, FACE, " + FromLoadAxis(femeshLoad.Axis) + direction +
                    ", 0, 0, 0, " + FromLoadProjection(femeshLoad.Projected) + ", " +
                    FromVectorDirection(femeshLoad.Pressure, direction).PressureFromSI(forceUnit, lengthUnit).ToString() +
                    ", 0, 0, 0, 0, " + femeshLoad.Name;
                    break;
            }



            return midasFEMeshLoad;
        }

        /***************************************************/

    }
}




