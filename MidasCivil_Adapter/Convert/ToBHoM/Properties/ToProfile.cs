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

using BH.oM.Geometry.ShapeProfiles;
using System.Collections.Generic;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static IProfile ToProfile(List<string> sectionProfile, string shape, string lengthUnit)
        {
            IProfile bhomProfile = null;
            switch (shape)
            {
                case "SB":
                    bhomProfile = Engine.Geometry.Create.RectangleProfile(
                        System.Convert.ToDouble(sectionProfile[0]).LengthToSI(lengthUnit), System.Convert.ToDouble(sectionProfile[1]).LengthToSI(lengthUnit), 0);
                    break;
                case "B":
                    double width = System.Convert.ToDouble(sectionProfile[1]).LengthToSI(lengthUnit);
                    double webSpacing = System.Convert.ToDouble(sectionProfile[4]).LengthToSI(lengthUnit);
                    double webThickness = System.Convert.ToDouble(sectionProfile[2]).LengthToSI(lengthUnit);
                    double corbel;
                    if (System.Math.Abs(width / 2 - webSpacing / 2 - webThickness / 2) < oM.Geometry.Tolerance.Distance)
                    {
                        corbel = 0;
                    }

                    else
                    {
                        corbel = (width / 2 - webSpacing / 2 - webThickness / 2).LengthToSI(lengthUnit);
                    }

                    bhomProfile = Engine.Geometry.Create.GeneralisedFabricatedBoxProfile(
                            System.Convert.ToDouble(sectionProfile[0]).LengthToSI(lengthUnit), width, webThickness,
                            System.Convert.ToDouble(sectionProfile[3]).LengthToSI(lengthUnit), System.Convert.ToDouble(sectionProfile[5]).LengthToSI(lengthUnit),
                            corbel, corbel);
                    break;
                case "P":
                    bhomProfile = Engine.Geometry.Create.TubeProfile(System.Convert.ToDouble(sectionProfile[0]).LengthToSI(lengthUnit),
                        System.Convert.ToDouble(sectionProfile[1]).LengthToSI(lengthUnit));
                    break;
                case "SR":
                    bhomProfile = Engine.Geometry.Create.CircleProfile(
                         System.Convert.ToDouble(sectionProfile[0]).LengthToSI(lengthUnit));
                    break;
                case "H":
                    bhomProfile = Engine.Geometry.Create.FabricatedISectionProfile(
                        System.Convert.ToDouble(sectionProfile[0]).LengthToSI(lengthUnit), System.Convert.ToDouble(sectionProfile[1]).LengthToSI(lengthUnit),
                        System.Convert.ToDouble(sectionProfile[4]).LengthToSI(lengthUnit), System.Convert.ToDouble(sectionProfile[2]).LengthToSI(lengthUnit),
                        System.Convert.ToDouble(sectionProfile[3]).LengthToSI(lengthUnit), System.Convert.ToDouble(sectionProfile[5]).LengthToSI(lengthUnit), 0);
                    break;
                case "T":
                    bhomProfile = Engine.Geometry.Create.TSectionProfile(
                        System.Convert.ToDouble(sectionProfile[0]).LengthToSI(lengthUnit), System.Convert.ToDouble(sectionProfile[1]).LengthToSI(lengthUnit),
                        System.Convert.ToDouble(sectionProfile[2]).LengthToSI(lengthUnit), System.Convert.ToDouble(sectionProfile[3]).LengthToSI(lengthUnit),
                        0, 0);
                    break;
                case "C":
                    bhomProfile = Engine.Geometry.Create.ChannelProfile(
                            System.Convert.ToDouble(sectionProfile[0]).LengthToSI(lengthUnit), System.Convert.ToDouble(sectionProfile[1]).LengthToSI(lengthUnit),
                            System.Convert.ToDouble(sectionProfile[2]).LengthToSI(lengthUnit), System.Convert.ToDouble(sectionProfile[3]).LengthToSI(lengthUnit),
                            System.Convert.ToDouble(sectionProfile[6]).LengthToSI(lengthUnit), System.Convert.ToDouble(sectionProfile[7]).LengthToSI(lengthUnit));
                    break;
                case "L":
                    bhomProfile = Engine.Geometry.Create.AngleProfile(
                            System.Convert.ToDouble(sectionProfile[0]).LengthToSI(lengthUnit), System.Convert.ToDouble(sectionProfile[1]).LengthToSI(lengthUnit),
                            System.Convert.ToDouble(sectionProfile[2]).LengthToSI(lengthUnit), System.Convert.ToDouble(sectionProfile[3]).LengthToSI(lengthUnit),
                            0, 0, false, true);
                    break;
            }

            if (shape.Contains("TAPERED"))
            {
                string shapeCode = shape.Split('-')[1].Trim();
                int interpolationOrder = System.Convert.ToInt32(shape.Split('-')[2].Trim());
                IProfile startProfile = Convert.ToProfile(sectionProfile.GetRange(0, sectionProfile.Count / 2), shapeCode, lengthUnit);
                IProfile endProfile = Convert.ToProfile(sectionProfile.GetRange(sectionProfile.Count / 2, sectionProfile.Count / 2), shapeCode, lengthUnit);

                bhomProfile = Engine.Geometry.Create.TaperedProfile(startProfile, endProfile, interpolationOrder);

            }

            return bhomProfile;
        }

        /***************************************************/

    }
}
