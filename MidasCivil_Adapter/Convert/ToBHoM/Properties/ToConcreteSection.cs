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

using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Geometry.ShapeProfiles;
using BH.Engine.Structure;
using System;
using System.Linq;

namespace BH.Adapter.External.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static ISectionProperty ToConcreteSection(this SteelSection steelSection)
        {
            ConcreteSection bhomSection = null;
            string sectionType = steelSection.SectionProfile.GetType().ToString().Split('.').Last();

            switch (sectionType)
            {
                case "RectangleProfile":
                    RectangleProfile rectangle = (RectangleProfile)steelSection.SectionProfile;
                    bhomSection = Engine.Structure.Create.ConcreteRectangleSection(rectangle.Height, rectangle.Width, null, steelSection.Name);
                    break;

                case "CircleProfile":
                    CircleProfile circle = (CircleProfile)steelSection.SectionProfile;
                    bhomSection = Engine.Structure.Create.ConcreteCircularSection(circle.Diameter, null, steelSection.Name);
                    break;

                case "TSectionProfile":
                    TSectionProfile tee = (TSectionProfile)steelSection.SectionProfile;
                    bhomSection = Engine.Structure.Create.ConcreteTSection(tee.Height, tee.WebThickness, tee.Width, tee.FlangeThickness, null, steelSection.Name);
                    break;
            }

            if (bhomSection == null)
            {
                Engine.Reflection.Compute.RecordError(sectionType + "Concrete section not yet supported in the BHoM");
            }

            return bhomSection;
        }

        /***************************************************/

    }
}