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

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static ISectionProperty ToSectionProperty(this string sectionProperty, string lengthUnit)
        {
            string[] split = sectionProperty.Split(',');
            string shape = split[12].Trim();

            GenericSection bhomSection = null;

            if (shape == "SB")
            {
                //    2, DBUSER    , USER-RECTANGLE    , CC, 0, 0, 0, 0, 0, 0, YES, NO, SB , 2, 0.5, 0.5, 0, 0, 0, 0, 0, 0, 0, 0
                bhomSection = Create.GenericSectionFromProfile(Create.RectangleProfile
                    (System.Convert.ToDouble(split[14]).LengthToSI(lengthUnit), System.Convert.ToDouble(split[15]).LengthToSI(lengthUnit), 0), null);

            }
            else if (shape == "B")
            {
                //    4, DBUSER    , USER-BOX          , CC, 0, 0, 0, 0, 0, 0, YES, NO, B  , 2, 0.5, 0.2, 0.01, 0.02, 0.19, 0.02, 0, 0, 0, 0
                bhomSection = Create.GenericSectionFromProfile(Convert.ToProfile(sectionProperty, lengthUnit), null
                    );


            }
            else if (shape == "P")
            {
                //    6, DBUSER    , CHS 114.3x9.83    , CC, 0, 0, 0, 0, 0, 0, YES, NO, P  , 1, BS, CHS 114.3x9.83
                bhomSection = Create.GenericSectionFromProfile(
                    Create.TubeProfile(System.Convert.ToDouble(split[14]).LengthToSI(lengthUnit), System.Convert.ToDouble(split[15]).LengthToSI(lengthUnit))
                    , null);

            }
            else if (shape == "SR")
            {
                //14, DBUSER    , USER - CIRCLE       , CC, 0, 0, 0, 0, 0, 0, YES, NO, SR , 2, 0.5, 0, 0, 0, 0, 0, 0, 0, 0, 0
                bhomSection = Create.GenericSectionFromProfile(
                    Create.CircleProfile(System.Convert.ToDouble(split[14]).LengthToSI(lengthUnit)), null);

            }
            else if (shape == "H")
            {
                bhomSection = Create.GenericSectionFromProfile(Engine.Geometry.Create.FabricatedISectionProfile
                    (System.Convert.ToDouble(split[14]).LengthToSI(lengthUnit), System.Convert.ToDouble(split[15]).LengthToSI(lengthUnit),
                    System.Convert.ToDouble(split[18]).LengthToSI(lengthUnit), System.Convert.ToDouble(split[16]).LengthToSI(lengthUnit),
                    System.Convert.ToDouble(split[17]).LengthToSI(lengthUnit), System.Convert.ToDouble(split[19]).LengthToSI(lengthUnit), 0), null);

                //    8, DBUSER    , USER-ISECTION     , CC, 0, 0, 0, 0, 0, 0, YES, NO, H  , 2, 1, 0.3, 0.03, 0.025, 0.5, 0.02, 0.01, 0.01, 0, 0
            }
            else if (shape == "T")
            {
                bhomSection = Create.GenericSectionFromProfile(Create.TSectionProfile
                    (System.Convert.ToDouble(split[14]).LengthToSI(lengthUnit), System.Convert.ToDouble(split[15]).LengthToSI(lengthUnit),
                    System.Convert.ToDouble(split[16]).LengthToSI(lengthUnit), System.Convert.ToDouble(split[17]).LengthToSI(lengthUnit), 0, 0), null);

                //   10, DBUSER    , USER-TSECTION     , CC, 0, 0, 0, 0, 0, 0, YES, NO, T  , 2, 0.3, 0.2, 0.02, 0.05, 0, 0, 0, 0, 0, 0
            }
            else if (shape == "C")
            {
                //   12, DBUSER    , USER-CHANNEL      , CC, 0, 0, 0, 0, 0, 0, YES, NO, C  , 2, 0.9, 0.5, 0.02, 0.02, 0.5, 0.02, 0, 0, 0, 0

                bhomSection = Create.GenericSectionFromProfile(ToProfile(sectionProperty, lengthUnit), null);


                Engine.Reflection.Compute.RecordWarning(bhomSection.SectionProfile.GetType().ToString() +
                    " has identical flanges. Therefore, the top flange width and thickness have been used from MidasCivil.");
            }
            else if (shape == "L")
            {
                //    1, DBUSER    , USERANGLE         , CC, 0, 0, 0, 0, 0, 0, YES, NO, L  , 2, 0.5, 0.25, 0.01, 0.03, 0, 0, 0, 0, 0, 0
                bhomSection = Create.GenericSectionFromProfile(
                        ToProfile(sectionProperty, lengthUnit), null);
            }
            else
            {
                Engine.Reflection.Compute.RecordError("Section not yet supported in MidasCivil_Toolkit ");
            }

            bhomSection.Name = split[2];
            bhomSection.CustomData[AdapterIdName] = split[0].Replace(" ", "");

            return bhomSection;
        }

        /***************************************************/

        public static ISectionProperty ToSectionProperty(this string sectionProfile, string sectionProperty1,
            string sectionProperty2, string sectionProperty3, string lengthUnit)
        {
            IProfile bhomProfile = ToProfile(sectionProfile, lengthUnit);

            string[] split0 = sectionProfile.Split(',');
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

            GenericSection bhomSection = new GenericSection(
                bhomProfile, area, rgy, rgz, j, iy, iz, iw,
                wely, welz, wply, wplz, centreZ, centreY, zt, zb, yt, yb, asy, asz);



            bhomSection.Name = split0[2];
            bhomSection.CustomData[AdapterIdName] = split0[0].Trim();

            return bhomSection;
        }

        /***************************************************/

    }
}