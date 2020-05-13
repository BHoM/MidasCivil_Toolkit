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

namespace BH.Adapter.External.MidasCivil
{
    public static partial class Convert
    {
        public static IProfile ToProfile(string sectionProfile)
        {
            string[] split = sectionProfile.Split(',');
            string shape = split[12].Trim();

            IProfile bhomProfile = null;
            if (shape == "SB")
            {
                bhomProfile = Engine.Geometry.Create.RectangleProfile(
                    System.Convert.ToDouble(split[14]), System.Convert.ToDouble(split[15]),0);
            }
            else if (shape == "B")
            {
                double width = System.Convert.ToDouble(split[15]);
                double webSpacing = System.Convert.ToDouble(split[18]);
                double webThickness = System.Convert.ToDouble(split[16]);
                double corbel;
                if (System.Math.Abs(width / 2 - webSpacing / 2 - webThickness / 2) < oM.Geometry.Tolerance.Distance)
                {
                    corbel = 0;
                }

                else
                {
                    corbel = width / 2 - webSpacing / 2 - webThickness / 2;
                }

                bhomProfile = Engine.Geometry.Create.GeneralisedFabricatedBoxProfile(
                        System.Convert.ToDouble(split[14]), width, webThickness,
                        System.Convert.ToDouble(split[17]), System.Convert.ToDouble(split[19]),
                        corbel, corbel
                );
            }
            else if (shape == "P")
            {
                bhomProfile = Engine.Geometry.Create.TubeProfile(System.Convert.ToDouble(split[14]), System.Convert.ToDouble(split[15]));
            }
            else if (shape == "SR")
            {
                bhomProfile = Engine.Geometry.Create.CircleProfile(
                     System.Convert.ToDouble(split[14]));
            }
            else if (shape == "H")
            {
                bhomProfile = Engine.Geometry.Create.FabricatedISectionProfile(
                    System.Convert.ToDouble(split[14]), System.Convert.ToDouble(split[15]), System.Convert.ToDouble(split[18]), 
                    System.Convert.ToDouble(split[16]), System.Convert.ToDouble(split[17]), System.Convert.ToDouble(split[19]),0);
            }
            else if (shape == "T")
            {
                bhomProfile = Engine.Geometry.Create.TSectionProfile(
                    System.Convert.ToDouble(split[14]), System.Convert.ToDouble(split[15]),
                    System.Convert.ToDouble(split[16]), System.Convert.ToDouble(split[17]),
                    0,0
                    );
            }
            else if (shape == "C")
            {
                bhomProfile = Engine.Geometry.Create.ChannelProfile(
                        System.Convert.ToDouble(split[14]), System.Convert.ToDouble(split[15]),
                        System.Convert.ToDouble(split[16]), System.Convert.ToDouble(split[17]),
                        System.Convert.ToDouble(split[20]), System.Convert.ToDouble(split[21]));
            }
            else if (shape == "L")
            {
                bhomProfile = Engine.Geometry.Create.AngleProfile(
                        System.Convert.ToDouble(split[14]), System.Convert.ToDouble(split[15]),
                        System.Convert.ToDouble(split[16]), System.Convert.ToDouble(split[17]),
                        0,0,false,true);
            }

            return bhomProfile;
            /***************************************************/
        }
    }
}
