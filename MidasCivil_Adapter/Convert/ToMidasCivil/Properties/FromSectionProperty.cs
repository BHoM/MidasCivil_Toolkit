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
using BH.Engine.Structure;
using System.Linq;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static string FromSectionProperty(this ISectionProperty sectionProperty, string lengthUnit, int sectionPropertyCharacterLimit)
        {
            if (CreateSection(sectionProperty as dynamic, lengthUnit) == null)
            {
                return null;
            }
            else
            {
                string midasSectionProperty = sectionProperty.CustomData[AdapterIdName] + ",DBUSER," +
                 new string(sectionProperty.DescriptionOrName().Take(sectionPropertyCharacterLimit).ToArray()) + ",CC, 0, 0, 0, 0, 0, 0, YES, NO," +
                 CreateSection(sectionProperty as dynamic, lengthUnit);

                return midasSectionProperty;
            }

        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static string CreateSection(SteelSection sectionProperty, string lengthUnit)
        {
            string midasSectionProperty = CreateProfile(sectionProperty.SectionProfile as dynamic, lengthUnit);

            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateSection(ConcreteSection sectionProperty, string lengthUnit)
        {
            string midasSectionProperty = CreateProfile(sectionProperty.SectionProfile as dynamic, lengthUnit);
            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateSection(TimberSection sectionProperty, string lengthUnit)
        {
            string midasSectionProperty = CreateProfile(sectionProperty.SectionProfile as dynamic, lengthUnit);
            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateSection(GenericSection sectionProperty, string lengthUnit)
        {
            string midasSectionProperty = CreateProfile(sectionProperty.SectionProfile as dynamic, lengthUnit);
            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateSection(AluminiumSection sectionProperty, string lengthUnit)
        {
            string midasSectionProperty = CreateProfile(sectionProperty.SectionProfile as dynamic, lengthUnit);
            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateSection(ExplicitSection sectionProperty, string lengthUnit)
        {
            Engine.Reflection.Compute.RecordError("ExplicitSection not supported in MidasCivil_Toolkit");
            return null;
        }

        /***************************************************/

        private static string CreateProfile(RectangleProfile profile, string lengthUnit)
        {
            string midasSectionProperty = "SB, 2," +
                profile.Height.LengthFromSI(lengthUnit) + "," + profile.Width.LengthFromSI(lengthUnit) +
                " ,0, 0, 0, 0, 0, 0, 0, 0";
            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(BoxProfile profile, string lengthUnit)
        {
            double webSpacing = (profile.Width - profile.Thickness).LengthFromSI(lengthUnit);

            string midasSectionProperty = "B, 2," +
                profile.Height.LengthFromSI(lengthUnit) + "," + profile.Width.LengthFromSI(lengthUnit) + "," + profile.Thickness.LengthFromSI(lengthUnit) + "," +
                profile.Thickness.LengthFromSI(lengthUnit) + "," + webSpacing + "," + profile.Thickness.LengthFromSI(lengthUnit) +
                ", 0, 0, 0, 0";
            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(FabricatedBoxProfile profile, string lengthUnit)
        {
            double webSpacing = (profile.Width - profile.WebThickness).LengthFromSI(lengthUnit);

            string midasSectionProperty = "B, 2," +
                profile.Height.LengthFromSI(lengthUnit) + "," + profile.Width.LengthFromSI(lengthUnit) + "," + profile.WebThickness.LengthFromSI(lengthUnit) + "," +
                profile.TopFlangeThickness.LengthFromSI(lengthUnit) + "," + webSpacing + "," + profile.BotFlangeThickness.LengthFromSI(lengthUnit) +
                ", 0, 0, 0, 0";
            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(CircleProfile profile, string lengthUnit)
        {
            string midasSectionProperty = "SR, 2," +
                profile.Diameter.LengthFromSI(lengthUnit) +
                ", 0, 0, 0, 0, 0, 0, 0, 0, 0";
            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(TubeProfile profile, string lengthUnit)
        {
            string midasSectionProperty = "P  , 2," +
                profile.Diameter.LengthFromSI(lengthUnit) + "," + profile.Thickness.LengthFromSI(lengthUnit) +
                ", 0, 0, 0, 0, 0, 0, 0, 0";
            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(ISectionProfile profile, string lengthUnit)
        {
            string midasSectionProperty = "H, 2, " + profile.Height.LengthFromSI(lengthUnit) + "," + profile.Width.LengthFromSI(lengthUnit) + "," +
                profile.WebThickness.LengthFromSI(lengthUnit) + "," + profile.FlangeThickness.LengthFromSI(lengthUnit) + "," + profile.Width.LengthFromSI(lengthUnit) + "," +
                profile.FlangeThickness.LengthFromSI(lengthUnit) + "," + profile.RootRadius.LengthFromSI(lengthUnit) + "," + profile.ToeRadius.LengthFromSI(lengthUnit) +
                ", 0, 0";
            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(TSectionProfile profile, string lengthUnit)
        {
            string midasSectionProperty = "T, 2," + profile.Height.LengthFromSI(lengthUnit) + "," + profile.Width.LengthFromSI(lengthUnit) + "," +
                profile.WebThickness.LengthFromSI(lengthUnit) + "," + profile.FlangeThickness.LengthFromSI(lengthUnit) +
                ",0, 0, 0, 0, 0, 0";
            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(FabricatedISectionProfile profile, string lengthUnit)
        {
            string midasSectionProperty = "H, 2, " + profile.Height.LengthFromSI(lengthUnit) + "," + profile.TopFlangeWidth.LengthFromSI(lengthUnit) + "," +
                profile.WebThickness.LengthFromSI(lengthUnit) + "," + profile.TopFlangeThickness.LengthFromSI(lengthUnit) + "," + profile.BotFlangeWidth.LengthFromSI(lengthUnit) + "," +
                profile.BotFlangeThickness.LengthFromSI(lengthUnit) + "," + profile.WeldSize.LengthFromSI(lengthUnit) +
                ", 0, 0, 0";

            Engine.Reflection.Compute.RecordWarning(
                "The weld size has been assumed equal to the root radius"
                );

            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(AngleProfile profile, string lengthUnit)
        {
            string midasSectionProperty = "L, 2," + profile.Height.LengthFromSI(lengthUnit) + "," + profile.Width.LengthFromSI(lengthUnit) + "," +
                profile.WebThickness.LengthFromSI(lengthUnit) + "," + profile.FlangeThickness.LengthFromSI(lengthUnit) +
                ", 0, 0, 0, 0, 0, 0";

            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(ChannelProfile profile, string lengthUnit)
        {
            string midasSectionProperty = "C  , 2," + profile.Height.LengthFromSI(lengthUnit) + "," + profile.FlangeWidth.LengthFromSI(lengthUnit) + "," + 
                profile.WebThickness.LengthFromSI(lengthUnit) + "," + profile.FlangeThickness.LengthFromSI(lengthUnit) + "," + 
                profile.FlangeWidth.LengthFromSI(lengthUnit) + "," + profile.FlangeThickness.LengthFromSI(lengthUnit) + ", 0, 0, 0, 0";
            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(GeneralisedFabricatedBoxProfile profile, string lengthUnit)
        {
            double webSpacing = 0;
            if (profile.TopLeftCorbelWidth != 0 || profile.TopRightCorbelWidth != 0 || profile.BotLeftCorbelWidth != 0 || profile.BotRightCorbelWidth != 0)
            {
                webSpacing = (profile.Width - profile.TopLeftCorbelWidth - profile.TopRightCorbelWidth - profile.WebThickness).LengthFromSI(lengthUnit);
            }

            string midasSectionProperty = "B, 2," +
                profile.Height.LengthFromSI(lengthUnit) + "," + profile.Width.LengthFromSI(lengthUnit) + "," + profile.WebThickness.LengthFromSI(lengthUnit) + "," +
                profile.TopFlangeThickness.LengthFromSI(lengthUnit) + "," + webSpacing + "," + profile.BotFlangeThickness.LengthFromSI(lengthUnit) +
                ", 0, 0, 0, 0";

            Engine.Reflection.Compute.RecordWarning(
                "The corbel widths have been used from the top flange only"
                );

            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateProfile(ZSectionProfile profile, string lengthUnit)
        {
            string[] profilearray = profile.GetType().ToString().Split('.');
            int profileindex = profilearray.Length;

            Engine.Reflection.Compute.RecordError(
                profilearray[profileindex - 1] + " not supported in MidasCivil_Toolkit"
                );
            return null;
        }

        /***************************************************/

        private static string CreateProfile(FreeFormProfile profile, string lengthUnit)
        {
            string[] profilearray = profile.GetType().ToString().Split('.');
            int profileindex = profilearray.Length;

            Engine.Reflection.Compute.RecordError(
                profilearray[profileindex - 1] + " not supported in MidasCivil_Toolkit"
                );
            return null;
        }

        /***************************************************/

        private static string CreateProfile(KiteProfile profile, string lengthUnit)
        {
            string[] profilearray = profile.GetType().ToString().Split('.');
            int profileindex = profilearray.Length;

            Engine.Reflection.Compute.RecordError(
                profilearray[profileindex - 1] + " not supported in MidasCivil_Toolkit"
                );
            return null;
        }

        /***************************************************/

        private static string CreateProfile(GeneralisedTSectionProfile profile, string lengthUnit)
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