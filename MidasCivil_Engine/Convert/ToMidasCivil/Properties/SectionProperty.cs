using System;
using BH.oM.Structure.SectionProperties;
using BH.oM.Geometry.ShapeProfiles;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCSectionProperty(this ISectionProperty sectionProperty)
        {
            string midasSectionProperty = sectionProperty.CustomData[AdapterId] + ",DBUSER," +
                sectionProperty.Name + ",CC, 0, 0, 0, 0, 0, 0, YES, NO," +
                CreateSection(sectionProperty as dynamic);

            return midasSectionProperty;
        }

        private static string CreateSection(SteelSection sectionProperty)
        {
            string midasSectionProperty = CreateProfile(sectionProperty.SectionProfile as dynamic);

            return midasSectionProperty;
        }

        private static string CreateSection(ConcreteSection sectionProperty)
        {
            Engine.Reflection.Compute.RecordError("ConcreteSection not supported in MidasCivil_Toolkit");
            return null;
        }

        private static string CreateSection(ExplicitSection sectionProperty)
        {
            Engine.Reflection.Compute.RecordError("ExplicitSection not supported in MidasCivil_Toolkit");
            return null;
        }

        private static string CreateProfile(RectangleProfile profile)
        {
            string midasSectionProperty = "SB, 2," +
                profile.Height + "," + profile.Width +
                " ,0, 0, 0, 0, 0, 0, 0, 0";
            return midasSectionProperty;
        }

        private static string CreateProfile(BoxProfile profile)
        {
            double webSpacing = profile.Width - profile.Thickness;

            string midasSectionProperty = "B, BUILT," +
                profile.Height + "," + profile.Width + "," + profile.Thickness + "," +
                profile.Thickness + "," + webSpacing + "," + profile.Thickness +
                ", 0, 0, 0, 0";
            return midasSectionProperty;
        }

        private static string CreateProfile(FabricatedBoxProfile profile)
        {
            double webSpacing = profile.Width - profile.WebThickness;

            string midasSectionProperty = "B, 2," +
                profile.Height + "," + profile.Width + "," + profile.WebThickness + "," +
                profile.TopFlangeThickness + "," + webSpacing + "," + profile.BotFlangeThickness +
                ", 0, 0, 0, 0";
            return midasSectionProperty;
        }

        private static string CreateProfile(CircleProfile profile)
        {
            string midasSectionProperty = "SR, 2," +
                profile.Diameter +
                ", 0, 0, 0, 0, 0, 0, 0, 0, 0";
            return midasSectionProperty;
        }

        private static string CreateProfile(TubeProfile profile)
        {
            string midasSectionProperty = "P  , 2," +
                profile.Diameter + "," + profile.Thickness +
                ", 0, 0, 0, 0, 0, 0, 0, 0";
            return midasSectionProperty;
        }

        private static string CreateProfile(ISectionProfile profile)
        {
            string midasSectionProperty = "H, 2, " + profile.Height + "," + profile.Width + "," +
                profile.WebThickness + "," + profile.FlangeThickness + "," + profile.Width + "," +
                profile.FlangeThickness + "," + profile.RootRadius + "," + profile.ToeRadius + 
                ", 0, 0";
            return midasSectionProperty;
        }

        private static string CreateProfile(TSectionProfile profile)
        {
            string midasSectionProperty = "T, 2," + profile.Height + "," + profile.Width + "," + 
                profile.WebThickness + "," + profile.FlangeThickness + 
                ",0, 0, 0, 0, 0, 0";
            return midasSectionProperty;
        }

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

        private static string CreateProfile(AngleProfile profile)
        {
            string midasSectionProperty = "L, 2," + profile.Height + "," + profile.Width + "," + 
                profile.WebThickness + "," + profile.FlangeThickness + 
                ", 0, 0, 0, 0, 0, 0";

            return midasSectionProperty;
        }

        private static string CreateProfile(ChannelProfile profile)
        {
            string midasSectionProperty = "C  , 2," + profile.Height + "," + profile.FlangeWidth + "," + profile.WebThickness + "," +
                profile.FlangeThickness + "," + profile.FlangeWidth + "," + profile.FlangeThickness + ", 0, 0, 0, 0";
            return midasSectionProperty;
        }


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

        private static string CreateProfile(ZSectionProfile profile)
        {
            Engine.Reflection.Compute.RecordError(
                profile.GetType().ToString() + "not supported in MidasCivil_Toolkit"
                );
            return null;
        }

        private static string CreateProfile(FreeFormProfile profile)
        {
            Engine.Reflection.Compute.RecordError(
                profile.GetType().ToString() + "not supported in MidasCivil_Toolkit"
                );
            return null;
        }

        private static string CreateProfile(KiteProfile profile)
        {
            Engine.Reflection.Compute.RecordError(
                profile.GetType().ToString() + "not supported in MidasCivil_Toolkit"
                );
            return null;
        }
    }
}