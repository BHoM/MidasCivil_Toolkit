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
using BH.oM.Structure.SectionProperties;
using BH.oM.Geometry.ShapeProfiles;

namespace BH.Engine.External.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static string FromSectionProperty(this ISectionProperty sectionProperty)
        {
            if (CreateSection(sectionProperty as dynamic) == null)
            {
                return null;
            }
            else
            {
                string midasSectionProperty = sectionProperty.CustomData[AdapterIdName] + ",DBUSER," +
                 sectionProperty.Name + ",CC, 0, 0, 0, 0, 0, 0, YES, NO," +
                 CreateSection(sectionProperty as dynamic);

                string s3 = sectionProperty.Name;
                int count3 = 0;
                foreach (char c in s3)
                {
                    count3++;
                }
                if (count3 > 16)
                {
                    Engine.Reflection.Compute.RecordWarning("All names must be under 16 characters");
                }
                return midasSectionProperty;
            }

        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static string CreateSection(SteelSection sectionProperty)
        {
            string midasSectionProperty = CreateProfile(sectionProperty.SectionProfile as dynamic);

            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateSection(ConcreteSection sectionProperty)
        {
            string midasSectionProperty = CreateProfile(sectionProperty.SectionProfile as dynamic);
            return midasSectionProperty;
        }
        private static string CreateSection(TimberSection sectionProperty)
        {
            string midasSectionProperty = CreateProfile(sectionProperty.SectionProfile as dynamic);
            return midasSectionProperty;
        }
        private static string CreateSection(GenericSection sectionProperty)
        {
            string midasSectionProperty = CreateProfile(sectionProperty.SectionProfile as dynamic);
            return midasSectionProperty;
        }
        private static string CreateSection(AluminiumSection sectionProperty)
        {
            string midasSectionProperty = CreateProfile(sectionProperty.SectionProfile as dynamic);
            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateSection(ExplicitSection sectionProperty)
        {
            Engine.Reflection.Compute.RecordError("ExplicitSection not supported in MidasCivil_Toolkit");
            return null;
        }

        /***************************************************/

        private static string CreateProfile(RectangleProfile profile)
        {
            string midasSectionProperty = "SB, 2," +
                profile.Height + "," + profile.Width +
                " ,0, 0, 0, 0, 0, 0, 0, 0";
            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(BoxProfile profile)
        {
            double webSpacing = profile.Width - profile.Thickness;

            string midasSectionProperty = "B, 2," +
                profile.Height + "," + profile.Width + "," + profile.Thickness + "," +
                profile.Thickness + "," + webSpacing + "," + profile.Thickness +
                ", 0, 0, 0, 0";
            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(FabricatedBoxProfile profile)
        {
            double webSpacing = profile.Width - profile.WebThickness;

            string midasSectionProperty = "B, 2," +
                profile.Height + "," + profile.Width + "," + profile.WebThickness + "," +
                profile.TopFlangeThickness + "," + webSpacing + "," + profile.BotFlangeThickness +
                ", 0, 0, 0, 0";
            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(CircleProfile profile)
        {
            string midasSectionProperty = "SR, 2," +
                profile.Diameter +
                ", 0, 0, 0, 0, 0, 0, 0, 0, 0";
            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(TubeProfile profile)
        {
            string midasSectionProperty = "P  , 2," +
                profile.Diameter + "," + profile.Thickness +
                ", 0, 0, 0, 0, 0, 0, 0, 0";
            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(ISectionProfile profile)
        {
            string midasSectionProperty = "H, 2, " + profile.Height + "," + profile.Width + "," +
                profile.WebThickness + "," + profile.FlangeThickness + "," + profile.Width + "," +
                profile.FlangeThickness + "," + profile.RootRadius + "," + profile.ToeRadius + 
                ", 0, 0";
            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(TSectionProfile profile)
        {
            string midasSectionProperty = "T, 2," + profile.Height + "," + profile.Width + "," + 
                profile.WebThickness + "," + profile.FlangeThickness + 
                ",0, 0, 0, 0, 0, 0";
            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(FabricatedISectionProfile profile)
        {
            string midasSectionProperty = "H, 2, " + profile.Height + "," + profile.TopFlangeWidth + "," +
                profile.WebThickness + "," + profile.TopFlangeThickness + "," + profile.BotFlangeWidth + "," +
                profile.BotFlangeThickness + "," + profile.WeldSize +
                ", 0, 0, 0";

            Engine.Reflection.Compute.RecordWarning(
                "The weld size has been assumed equal to the root radius"
                );

            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(AngleProfile profile)
        {
            string midasSectionProperty = "L, 2," + profile.Height + "," + profile.Width + "," + 
                profile.WebThickness + "," + profile.FlangeThickness + 
                ", 0, 0, 0, 0, 0, 0";

            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(ChannelProfile profile)
        {
            string midasSectionProperty = "C  , 2," + profile.Height + "," + profile.FlangeWidth + "," + profile.WebThickness + "," +
                profile.FlangeThickness + "," + profile.FlangeWidth + "," + profile.FlangeThickness + ", 0, 0, 0, 0";
            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(GeneralisedFabricatedBoxProfile profile)
        {
            double webSpacing = 0;
            if (profile.TopLeftCorbelWidth!=0 || profile.TopRightCorbelWidth != 0 || profile.BotLeftCorbelWidth != 0 || profile.BotRightCorbelWidth != 0)
            {
                webSpacing = profile.Width - profile.TopLeftCorbelWidth - profile.TopRightCorbelWidth - profile.WebThickness;
            }

            string midasSectionProperty = "B, 2," +
                profile.Height + "," + profile.Width + "," + profile.WebThickness + "," +
                profile.TopFlangeThickness + "," + webSpacing + "," + profile.BotFlangeThickness +
                ", 0, 0, 0, 0";

            Engine.Reflection.Compute.RecordWarning(
                "The corbel widths have been used from the top flange only"
                );

            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(ZSectionProfile profile)
        {
            string[] profilearray = profile.GetType().ToString().Split('.');
            int profileindex = profilearray.Length;

            Engine.Reflection.Compute.RecordError(
                profilearray[profileindex-1] + " not supported in MidasCivil_Toolkit"
                );
            return null;
        }

        /***************************************************/

        private static string CreateProfile(FreeFormProfile profile)
        {
            string[] profilearray = profile.GetType().ToString().Split('.');
            int profileindex = profilearray.Length;

            Engine.Reflection.Compute.RecordError(
                profilearray[profileindex - 1] + " not supported in MidasCivil_Toolkit"
                );
            return null;
        }

        /***************************************************/

        private static string CreateProfile(KiteProfile profile)
        {
            string[] profilearray = profile.GetType().ToString().Split('.');
            int profileindex = profilearray.Length;

            Engine.Reflection.Compute.RecordError(
                profilearray[profileindex - 1] + " not supported in MidasCivil_Toolkit"
                );
            return null;
        }

        /***************************************************/

        private static string CreateProfile(GeneralisedTSectionProfile profile)
        {
            string[] profilearray = profile.GetType().ToString().Split('.');
            int profileindex = profilearray.Length;

            Engine.Reflection.Compute.RecordError(
                profilearray[profileindex - 1] + " not supported in MidasCivil_Toolkit"
                );
            return null;
        }

        /***************************************************/

    }
}