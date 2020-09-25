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
using System.Collections.Generic;
using BH.oM.Structure.SectionProperties;
using BH.oM.Geometry.ShapeProfiles;
using BH.Engine.Structure;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Microsoft.SqlServer.Server;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static List<string> FromSectionProperty(this ISectionProperty sectionProperty, string lengthUnit, int sectionPropertyCharacterLimit)
        {
            List<string> midasSectionProperty = new List<string>();
            midasSectionProperty = CreateSection(sectionProperty as dynamic, lengthUnit, sectionPropertyCharacterLimit);

            return midasSectionProperty is null ? null : midasSectionProperty;
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static List<string> CreateSection(SteelSection sectionProperty, string lengthUnit, int sectionPropertyCharacterLimit)
        {
            List<string> midasSectionProperty = new List<string>();
            if(sectionProperty.SectionProfile is TaperedProfile)
            {
                midasSectionProperty.Add(sectionProperty.CustomData[AdapterIdName] + ",TAPERED" +
                    new string(sectionProperty.DescriptionOrName().Replace(",", "").Take(sectionPropertyCharacterLimit).ToArray()) +
                    "CC, 0,0,0,0,0,0,0,0,YES,NO" + GetShapeCode(sectionProperty) + GetInterpolationOrder(sectionProperty) + GetInterpolationOrder(sectionProperty) + "USER");
                midasSectionProperty.Add(CreateProfile(sectionProperty.SectionProfile as dynamic, lengthUnit));
            }
            else
            {
                midasSectionProperty.AddRange(sectionProperty.CustomData[AdapterIdName] + ",DBUSER," +
                    new string(sectionProperty.DescriptionOrName().Replace(",", "").Take(sectionPropertyCharacterLimit).ToArray()) + 
                    ",CC, 0, 0, 0, 0, 0, 0, YES, NO," + CreateProfile(sectionProperty.SectionProfile as dynamic, lengthUnit));
            }

            return midasSectionProperty;
        }

        /***************************************************/

        private static List<string> CreateSection(ConcreteSection sectionProperty, string lengthUnit, int sectionPropertyCharacterLimit)
        {
            List<string> midasSectionProperty = new List<string>();
            if (sectionProperty.SectionProfile is TaperedProfile)
            {
                midasSectionProperty.AddRange(CreateProfile(sectionProperty.SectionProfile as dynamic, lengthUnit));
            }
            else
            {
                midasSectionProperty.AddRange(sectionProperty.CustomData[AdapterIdName] + ",DBUSER," +
                    new string(sectionProperty.DescriptionOrName().Replace(",", "").Take(sectionPropertyCharacterLimit).ToArray()) +
                    ",CC, 0, 0, 0, 0, 0, 0, YES, NO," + CreateProfile(sectionProperty.SectionProfile as dynamic, lengthUnit));
            }

            return midasSectionProperty;
        }

        /***************************************************/

        private static List<string> CreateSection(TimberSection sectionProperty, string lengthUnit, int sectionPropertyCharacterLimit)
        {
            List<string> midasSectionProperty = new List<string>();
            if (sectionProperty.SectionProfile is TaperedProfile)
            {
                midasSectionProperty.AddRange(CreateProfile(sectionProperty.SectionProfile as dynamic, lengthUnit));
            }
            else
            {
                midasSectionProperty.AddRange(sectionProperty.CustomData[AdapterIdName] + ",DBUSER," +
                    new string(sectionProperty.DescriptionOrName().Replace(",", "").Take(sectionPropertyCharacterLimit).ToArray()) +
                    ",CC, 0, 0, 0, 0, 0, 0, YES, NO," + CreateProfile(sectionProperty.SectionProfile as dynamic, lengthUnit));
            }

            return midasSectionProperty;
        }

        /***************************************************/

        private static List<string> CreateSection(GenericSection sectionProperty, string lengthUnit, int sectionPropertyCharacterLimit)
        {
            List<string> midasSectionProperty = new List<string>();
            if (sectionProperty.SectionProfile is TaperedProfile)
            {
                midasSectionProperty.AddRange(CreateProfile(sectionProperty.SectionProfile as dynamic, lengthUnit));
            }
            else
            {
                midasSectionProperty.AddRange(sectionProperty.CustomData[AdapterIdName] + ",DBUSER," +
                    new string(sectionProperty.DescriptionOrName().Replace(",", "").Take(sectionPropertyCharacterLimit).ToArray()) +
                    ",CC, 0, 0, 0, 0, 0, 0, YES, NO," + CreateProfile(sectionProperty.SectionProfile as dynamic, lengthUnit));
            }

            return midasSectionProperty;
        }

        /***************************************************/

        private static List<string> CreateSection(AluminiumSection sectionProperty, string lengthUnit, int sectionPropertyCharacterLimit)
        {
            List<string> midasSectionProperty = new List<string>();
            if (sectionProperty.SectionProfile is TaperedProfile)
            {
                midasSectionProperty.AddRange(CreateProfile(sectionProperty.SectionProfile as dynamic, lengthUnit));
            }
            else
            {
                midasSectionProperty.AddRange(sectionProperty.CustomData[AdapterIdName] + ",DBUSER," +
                    new string(sectionProperty.DescriptionOrName().Replace(",", "").Take(sectionPropertyCharacterLimit).ToArray()) +
                    ",CC, 0, 0, 0, 0, 0, 0, YES, NO," + CreateProfile(sectionProperty.SectionProfile as dynamic, lengthUnit));
            }

            return midasSectionProperty;
        }

        /***************************************************/

        private static string CreateSection(ExplicitSection sectionProperty, string lengthUnit, int sectionPropertyCharacterLimit)
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

        private static string CreateProfile(TaperedProfile profile, string lengthUnit)
        {
            List<IProfile> profiles = new List<IProfile>(profile.Profiles.Values);

            return CreateProfile(profiles[0] as dynamic, lengthUnit) + CreateProfile(profiles[profiles.Count-1] as dynamic, lengthUnit);
        }

        /***************************************************/

        private static string GetInterpolationOrder(ISectionProperty sectionProperty)
        {
            GenericSection genericSection = (GenericSection)sectionProperty;
            TaperedProfile taperedProfile = (TaperedProfile)genericSection.SectionProfile;
            List<int> interpolationOrders = taperedProfile.InterpolationOrder;

            return interpolationOrders.Max().ToString();
        }

        /***************************************************/
        private static string GetShapeCode(ISectionProperty sectionProperty)
        {
            GenericSection genericSection = (GenericSection)sectionProperty;
            IProfile profile = genericSection.SectionProfile;

            if (profile is GeneralisedFabricatedBoxProfile || profile is FabricatedBoxProfile || profile is BoxProfile)
                return "B";
            else if (profile is FabricatedISectionProfile || profile is ISectionProfile)
                return "I";
            else if (profile is RectangleProfile)
                return "SB";
            else if (profile is CircleProfile)
                return "SR";
            else if (profile is TubeProfile)
                return "P";
            else if (profile is TSectionProfile)
                return "T";
            else if (profile is AngleProfile)
                return "L";
            else if (profile is ChannelProfile)
                return "C";
            else
                return null;
        }

        /***************************************************/

    }
}