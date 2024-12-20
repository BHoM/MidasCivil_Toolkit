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

using BH.oM.Adapters.MidasCivil;
using BH.Engine.Adapter;
using BH.oM.Structure.Elements;
using System.Collections.Generic;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static string FromBar(this Bar bar)
        {
            string midasElement;
            string feaType = "TRUSS";
            switch (bar.FEAType)
            {
                case BarFEAType.Axial:
                    break;

                case BarFEAType.Flexural:
                    feaType = "BEAM";
                    break;

                case BarFEAType.TensionOnly:
                    feaType = "TENSTR";
                    break;

                case BarFEAType.CompressionOnly:
                    feaType = "COMPTR";
                    break;
            }

            string startNodeID = bar.Start.AdapterId<string>(typeof(MidasCivilId));
            string endNodeID = bar.End.AdapterId<string>(typeof(MidasCivilId));
            string materialID = "1";
            string sectionID = "1";

            if (bar.SectionProperty != null)
            {
                sectionID = bar.SectionProperty.AdapterId<string>(typeof(MidasCivilId));
                if (bar.SectionProperty.Material != null)
                {
                    materialID = bar.SectionProperty.Material.AdapterId<string>(typeof(MidasCivilId));
                }
            }

            if (bar.FEAType == BarFEAType.Axial || bar.FEAType == BarFEAType.Flexural)
            {
                midasElement = (bar.AdapterId<string>(typeof(MidasCivilId)) + "," + feaType +
                                      "," + materialID + "," + sectionID + "," +
                                      startNodeID + "," + endNodeID + "," +
                                      (bar.OrientationAngle * 180 / System.Math.PI).ToString() + ",0,0");
            }
            else
            {
                midasElement = (bar.AdapterId<string>(typeof(MidasCivilId)) + "," + feaType +
                                  "," + materialID + "," + sectionID + "," +
                                  startNodeID + "," + endNodeID + "," +
                                  (bar.OrientationAngle * 180 / System.Math.PI).ToString() + ",1,0,0,NO");
            }

            return midasElement;
        }

        /***************************************************/

    }
}




