using BH.oM.Structure.Properties.Section;
using BH.oM.Structure.Properties.Section.ShapeProfiles;
using BH.Engine.Structure;
using System;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static ISectionProperty ToBHoMSectionProperty(this string sectionProperty)
        {
            string[] split = sectionProperty.Split(',');
            string shape = split[12].Replace(" ", "");

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
                    Engine.MidasCivil.Convert.ToProfile(sectionProperty)
                    );
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
                    System.Convert.ToDouble(split[18]), System.Convert.ToDouble(split[19]),
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
                        Engine.MidasCivil.Convert.ToProfile(sectionProperty));

                Engine.Reflection.Compute.RecordWarning(bhomSection.SectionProfile.GetType().ToString() +
                    " has identical flanges. Therefore, the top flange width and thickness have been used from MidasCivil.");
            }
            else if (shape == "L")
            {
                //    1, DBUSER    , USERANGLE         , CC, 0, 0, 0, 0, 0, 0, YES, NO, L  , 2, 0.5, 0.25, 0.01, 0.03, 0, 0, 0, 0, 0, 0
                bhomSection = Engine.Structure.Create.SteelSectionFromProfile(
                        Engine.MidasCivil.Convert.ToProfile(sectionProperty));
            }
            else
            {
                Engine.Reflection.Compute.RecordError("Section not yet supported in MidasCivil_Toolkit ");
            }

            bhomSection.Name = split[2];
            bhomSection.CustomData[AdapterId] = System.Convert.ToInt32(split[0]);

            return bhomSection;
        }

        public static ISectionProperty ToBHoMSectionProperty(this string sectionProfile, string sectionProperty1,
            string sectionProperty2, string sectionProperty3)
        {
            IProfile bhomProfile = Engine.MidasCivil.Convert.ToProfile(sectionProfile);

            string[] split0 = sectionProfile.Split(',');
            string[] split1 = sectionProperty1.Split(',');
            string[] split2 = sectionProperty2.Split(',');
            string[] split3 = sectionProperty3.Split(',');

            double area = System.Convert.ToDouble(split1[0]);

            double j = System.Convert.ToDouble(split1[3]);
            double iz = System.Convert.ToDouble(split1[5]);
            double iy = System.Convert.ToDouble(split1[4]);
            double iw = bhomProfile.IWarpingConstant();
            double wply = System.Convert.ToDouble(split3[8]);
            double wplz = System.Convert.ToDouble(split3[9]);
            double centreZ = System.Convert.ToDouble(split2[9]);
            double centreY = -System.Convert.ToDouble(split2[8]);
            double zt = System.Convert.ToDouble(split2[2]);
            double zb = System.Convert.ToDouble(split2[3]);
            double yt = System.Convert.ToDouble(split2[0]);
            double yb = System.Convert.ToDouble(split2[1]);
            double rgy = Math.Min(Math.Sqrt(area / zt), Math.Sqrt(area / zb));
            double rgz = Math.Min(Math.Sqrt(area / yt), Math.Sqrt(area / yb));
            double wely = Math.Min(iy / zt, iy / zb);
            double welz = Math.Min(iz / yt, iz / yb);
            double asy = System.Convert.ToDouble(split1[1]);
            double asz = System.Convert.ToDouble(split1[2]);

            bhomProfile = Engine.Structure.Compute.Integrate(bhomProfile, oM.Geometry.Tolerance.MicroDistance).Item1;

            SteelSection bhomSection = new SteelSection(
                bhomProfile, area, rgy, rgz, j, iy, iz, iw,
                wely, welz, wply, wplz, centreZ, centreY, zt, zb, yt, yb, asy, asz);



            bhomSection.Name = split0[2];
            bhomSection.CustomData[AdapterId] = System.Convert.ToInt32(split0[0]);

            return bhomSection;
        }
    }
}