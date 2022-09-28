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


using BH.oM.Adapters.MidasCivil;
using BH.Engine.Adapter;
using BH.Engine.Structure;
using BH.oM.Structure.SurfaceProperties;
using System.Linq;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static string FromSurfaceProperty(this ISurfaceProperty surfaceProperty, string version, string lengthUnit, int groupCharacterLimit)
        {
            string midasSurfaceProperty = CreateSurfaceProfile(surfaceProperty as dynamic, version, lengthUnit, groupCharacterLimit);

            return midasSurfaceProperty;
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static string CreateSurfaceProfile(ConstantThickness bhomSurfaceProperty, string version, string lengthUnit, int groupCharacterLimit)
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
                    case "9.1.0":
                    case "9.0.5":
                    case "9.0.0":
                    case "8.9.5":
                    case "8.9.0":
                        midasSurfaceProperty =
                            bhomSurfaceProperty.AdapterId<string>(typeof(MidasCivilId)) + ",VALUE,1,Yes," +
                            bhomSurfaceProperty.Thickness.LengthFromSI(lengthUnit) + ",0,No,0,0";
                        break;
                    case "8.8.5":
                        midasSurfaceProperty =
                        bhomSurfaceProperty.AdapterId<string>(typeof(MidasCivilId)) + "," + new string(bhomSurfaceProperty.DescriptionOrName().Replace(",", "").Take(groupCharacterLimit).ToArray())
                        + ",VALUE,Yes," + bhomSurfaceProperty.Thickness.LengthFromSI(lengthUnit) + ",0,No,0,0";
                        break;
                    default:
                        midasSurfaceProperty =
                        bhomSurfaceProperty.AdapterId<string>(typeof(MidasCivilId)) + ",VALUE,Yes," +
                        bhomSurfaceProperty.Thickness.LengthFromSI(lengthUnit) + ",0,No,0,0";
                        break;
                }

                return midasSurfaceProperty;
            }
        }

        /***************************************************/

        private static string CreateSurfaceProfile(LoadingPanelProperty bhomSurfaceProperty, string lengthUnit)
        {
            Engine.Base.Compute.RecordError("LoadingPanelProperty not supported in MidasCivil_Toolkit");
            return null;
        }

        /***************************************************/

        private static string CreateSurfaceProfile(Ribbed bhomSurfaceProperty, string lengthUnit)
        {
            Engine.Base.Compute.RecordError("Ribbed not supported in MidasCivil_Toolkit");
            return null;
        }

        /***************************************************/

        private static string CreateSurfaceProfile(Waffle bhomSurfaceProperty, string lengthUnit)
        {
            Engine.Base.Compute.RecordError("Waffle not supported in MidasCivil_Toolkit");
            return null;
        }

        /***************************************************/

    }
}



