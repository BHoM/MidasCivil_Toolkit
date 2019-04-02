using System;
using BH.oM.Structure.Properties.Section.ShapeProfiles;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static IProfile ToProfile(string sectionProfile)
        {
            string[] split = sectionProfile.Split(',');
            string shape = split[12].Replace(" ", "");

            IProfile bhomProfile = null;
            if (shape == "SB")
            {
                bhomProfile = Engine.Structure.Create.RectangleProfile(
                    System.Convert.ToDouble(split[14]), System.Convert.ToDouble(split[15]),0);
            }
            else if (shape == "B")
            {
                double width = System.Convert.ToDouble(split[15]);
                double webSpacing = System.Convert.ToDouble(split[18]);
                double webThickness = System.Convert.ToDouble(split[16]);
                double corbel;

                if (webSpacing==0)
                    corbel = 0;
                else
                    corbel = width / 2 - webSpacing / 2 - webThickness / 2;

                bhomProfile = Engine.Structure.Create.GeneralisedFabricatedBoxProfile(
                        System.Convert.ToDouble(split[14]), width, webThickness,
                        System.Convert.ToDouble(split[17]), System.Convert.ToDouble(split[19]),
                        corbel, corbel
                );
            }
            else if (shape == "P")
            {
                bhomProfile = Engine.Structure.Create.TubeProfile(System.Convert.ToDouble(split[14]), System.Convert.ToDouble(split[15]));
            }
            else if (shape == "SR")
            {
                bhomProfile = Engine.Structure.Create.CircleProfile(
                     System.Convert.ToDouble(split[14]));
            }
            else if (shape == "H")
            {
                bhomProfile = Engine.Structure.Create.FabricatedISectionProfile(
                    System.Convert.ToDouble(split[14]), System.Convert.ToDouble(split[15]), System.Convert.ToDouble(split[18]), 
                    System.Convert.ToDouble(split[16]), System.Convert.ToDouble(split[17]), System.Convert.ToDouble(split[19]),0);
            }
            else if (shape == "T")
            {
                bhomProfile = Engine.Structure.Create.TSectionProfile(
                    System.Convert.ToDouble(split[14]), System.Convert.ToDouble(split[15]),
                    System.Convert.ToDouble(split[16]), System.Convert.ToDouble(split[17]),
                    0,0
                    );
            }
            else if (shape == "C")
            {
                bhomProfile = Engine.Structure.Create.ChannelProfile(
                        System.Convert.ToDouble(split[14]), System.Convert.ToDouble(split[15]),
                        System.Convert.ToDouble(split[16]), System.Convert.ToDouble(split[17]),
                        System.Convert.ToDouble(split[20]), System.Convert.ToDouble(split[21]));
            }
            else if (shape == "L")
            {
                bhomProfile = Engine.Structure.Create.AngleProfile(
                        System.Convert.ToDouble(split[14]), System.Convert.ToDouble(split[15]),
                        System.Convert.ToDouble(split[16]), System.Convert.ToDouble(split[17]),
                        0,0,false,true);
            }

            return bhomProfile;
            /***************************************************/
        }
    }
}
