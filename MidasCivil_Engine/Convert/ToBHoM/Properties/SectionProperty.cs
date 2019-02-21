using BH.oM.Structure.Properties.Section;
using BH.oM.Structure.Properties.Section.ShapeProfiles;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static ISectionProperty ToBHoMSectionProperty(this string sectionProperty)
        {
            string[] split = sectionProperty.Split(',');
            string shape = split[12].Replace(" ","");

            SteelSection bhomSection = null;

            if (shape == "SB")
            {
                //    2, DBUSER    , USER-RECTANGLE    , CC, 0, 0, 0, 0, 0, 0, YES, NO, SB , 2, 0.5, 0.5, 0, 0, 0, 0, 0, 0, 0, 0
                bhomSection = Engine.Structure.Create.SteelRectangleSection(
                    System.Convert.ToDouble(split[14]), System.Convert.ToDouble(split[15]));
            }
            else if (shape == "B")
            {
                //    4, DBUSER    , USER-BOX          , CC, 0, 0, 0, 0, 0, 0, YES, NO, B  , 2, 0.5, 0.2, 0.01, 0.02, 0.19, 0.02, 0, 0, 0, 0

                double width = System.Convert.ToDouble(split[15]);
                double webSpacing = System.Convert.ToDouble(split[18]);
                double webThickness = System.Convert.ToDouble(split[16]);
                double corbel = width / 2 - webSpacing / 2 + webThickness / 2;

                bhomSection = Engine.Structure.Create.SteelSectionFromProfile(
                    Engine.Structure.Create.GeneralisedFabricatedBoxProfile(
                        System.Convert.ToDouble(split[14]), width, webThickness,
                        System.Convert.ToDouble(split[17]), System.Convert.ToDouble(split[19]),
                        corbel, corbel
                    ));
            }
            else if (shape == "P")
            {
                //    6, DBUSER    , CHS 114.3x9.83    , CC, 0, 0, 0, 0, 0, 0, YES, NO, P  , 1, BS, CHS 114.3x9.83
                bhomSection = Engine.Structure.Create.SteelTubeSection(
                    System.Convert.ToDouble(split[14]), System.Convert.ToDouble(split[15]));
            }
            else if (shape == "SR")
            {
                //14, DBUSER    , USER - CIRCLE       , CC, 0, 0, 0, 0, 0, 0, YES, NO, SR , 2, 0.5, 0, 0, 0, 0, 0, 0, 0, 0, 0
                bhomSection = Engine.Structure.Create.SteelCircularSection(
                    System.Convert.ToDouble(split[14]));
            }
            else if (shape == "H")
            {
                bhomSection = Engine.Structure.Create.SteelFabricatedISection(
                    System.Convert.ToDouble(split[14]), System.Convert.ToDouble(split[16]), 
                    System.Convert.ToDouble(split[15]), System.Convert.ToDouble(split[17]),
                    System.Convert.ToDouble(split[18]), System.Convert.ToDouble(split[18]),
                    0);
                //    8, DBUSER    , USER-ISECTION     , CC, 0, 0, 0, 0, 0, 0, YES, NO, H  , 2, 1, 0.3, 0.03, 0.025, 0.5, 0.02, 0.01, 0.01, 0, 0
            }
            else if (shape == "T")
            {
                bhomSection = Engine.Structure.Create.SteelTSection(
                    System.Convert.ToDouble(split[14]), System.Convert.ToDouble(split[16]),
                    System.Convert.ToDouble(split[15]), System.Convert.ToDouble(split[17]));
                //   10, DBUSER    , USER-TSECTION     , CC, 0, 0, 0, 0, 0, 0, YES, NO, T  , 2, 0.3, 0.2, 0.02, 0.05, 0, 0, 0, 0, 0, 0
            }
            else if (shape == "C")
            {
                //   12, DBUSER    , USER-CHANNEL      , CC, 0, 0, 0, 0, 0, 0, YES, NO, C  , 2, 0.9, 0.5, 0.02, 0.02, 0.5, 0.02, 0, 0, 0, 0
                bhomSection = Engine.Structure.Create.SteelSectionFromProfile(
                    Engine.Structure.Create.ChannelProfile(
                        System.Convert.ToDouble(split[14]), System.Convert.ToDouble(split[15]),
                        System.Convert.ToDouble(split[16]), System.Convert.ToDouble(split[17]),
                        System.Convert.ToDouble(split[20]), System.Convert.ToDouble(split[21])));

                Engine.Reflection.Compute.RecordWarning(bhomSection.SectionProfile.GetType().ToString()+
                    " has identical flanges. Therefore, the top flange width and thickness have been used from MidasCivil.");
            }
            else
            {
                Engine.Reflection.Compute.RecordError("Section not yet supported in MidasCivil_Toolkit ");
            }

            return bhomSection;
        }
    }
}