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

using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Spatial.ShapeProfiles;
using BH.Engine.Structure;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static ISectionProperty ToSectionProperty(this List<string> sectionProperty, string shape, string lengthUnit)
        {
            return Create.GenericSectionFromProfile(ToProfile(sectionProperty, shape, lengthUnit), null);
        }

        /***************************************************/

        public static ISectionProperty ToSectionProperty(this List<string> sectionProfile, string sectionProperty1,
            string sectionProperty2, string sectionProperty3, string shape, string lengthUnit)
        {
            IProfile bhomProfile = ToProfile(sectionProfile, shape, lengthUnit);

            string[] split1 = sectionProperty1.Split(',');
            string[] split2 = sectionProperty2.Split(',');
            string[] split3 = sectionProperty3.Split(',');

            double area = System.Convert.ToDouble(split1[0]).AreaToSI(lengthUnit);
            double j = System.Convert.ToDouble(split1[3]).AreaMomentOfInertiaToSI(lengthUnit);
            double iz = System.Convert.ToDouble(split1[5]).AreaMomentOfInertiaToSI(lengthUnit);
            double iy = System.Convert.ToDouble(split1[4]).AreaMomentOfInertiaToSI(lengthUnit);
            double iw = bhomProfile.IWarpingConstant().AreaMomentOfInertiaToSI(lengthUnit);
            double wply = System.Convert.ToDouble(split3[8]).VolumeToSI(lengthUnit);
            double wplz = System.Convert.ToDouble(split3[9]).VolumeToSI(lengthUnit);
            double centreZ = System.Convert.ToDouble(split2[9]).LengthToSI(lengthUnit);
            double centreY = -System.Convert.ToDouble(split2[8]).LengthToSI(lengthUnit);
            double zt = System.Convert.ToDouble(split2[2]).LengthToSI(lengthUnit);
            double zb = System.Convert.ToDouble(split2[3]).LengthToSI(lengthUnit);
            double yt = System.Convert.ToDouble(split2[0]).LengthToSI(lengthUnit);
            double yb = System.Convert.ToDouble(split2[1]).LengthToSI(lengthUnit);
            double rgy = Math.Min(Math.Sqrt(area / zt), Math.Sqrt(area / zb)).LengthToSI(lengthUnit);
            double rgz = Math.Min(Math.Sqrt(area / yt), Math.Sqrt(area / yb)).LengthToSI(lengthUnit);
            double wely = Math.Min(iy / zt, iy / zb).VolumeToSI(lengthUnit);
            double welz = Math.Min(iz / yt, iz / yb).VolumeToSI(lengthUnit);
            double asy = System.Convert.ToDouble(split1[1]).AreaToSI(lengthUnit);
            double asz = System.Convert.ToDouble(split1[2]).AreaToSI(lengthUnit);

            bhomProfile = Compute.Integrate(bhomProfile, oM.Geometry.Tolerance.MicroDistance).Item1;

            return new GenericSection(
                bhomProfile, area, rgy, rgz, j, iy, iz, iw,
                wely, welz, wply, wplz, centreZ, centreY, zt, zb, yt, yb, asy, asz);
        }

        /***************************************************/

    }
}

