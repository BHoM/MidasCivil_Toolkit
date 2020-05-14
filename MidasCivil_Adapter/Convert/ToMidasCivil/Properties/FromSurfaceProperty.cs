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

using System;
using BH.oM.Structure.SurfaceProperties;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static string FromSurfaceProperty(this ISurfaceProperty surfaceProperty, string version)
        {
            string midasSurfaceProperty = CreateSurfaceProfile(surfaceProperty as dynamic, version);

            return midasSurfaceProperty;
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static string CreateSurfaceProfile(ConstantThickness bhomSurfaceProperty, string version)
        {
            if (bhomSurfaceProperty.Thickness == 0)
            {
                return null;
            }
            else
            {
                string midasSurfaceProperty = "";
                switch (version)
                {

                    case "8.8.5":
                        midasSurfaceProperty =
                        bhomSurfaceProperty.CustomData[AdapterIdName].ToString() + "," + bhomSurfaceProperty.Name + ",VALUE,Yes," + bhomSurfaceProperty.Thickness + ",0,Yes,0,0";
                        break;
                    default:
                        midasSurfaceProperty =
                        bhomSurfaceProperty.CustomData[AdapterIdName].ToString() + ",VALUE,Yes," + bhomSurfaceProperty.Thickness + ",0,Yes,0,0";
                        break;
                }

                return midasSurfaceProperty;
            }
        }

        /***************************************************/

        private static string CreateSurfaceProfile(LoadingPanelProperty bhomSurfaceProperty)
        {
            Engine.Reflection.Compute.RecordError("LoadingPanelProperty not supported in MidasCivil_Toolkit");
            return null;
        }

        /***************************************************/

        private static string CreateSurfaceProfile(Ribbed bhomSurfaceProperty)
        {
            Engine.Reflection.Compute.RecordError("Ribbed not supported in MidasCivil_Toolkit");
            return null;
        }

        /***************************************************/

        private static string CreateSurfaceProfile(Waffle bhomSurfaceProperty)
        {
            Engine.Reflection.Compute.RecordError("Waffle not supported in MidasCivil_Toolkit");
            return null;
        }

        /***************************************************/

    }
}
