using System;
using BH.oM.Structure.SurfaceProperties;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCSurfaceProperty(this ISurfaceProperty surfaceProperty, string version)
        {
            string midasSurfaceProperty = CreateSurfaceProfile(surfaceProperty as dynamic, version);

            return midasSurfaceProperty;
        }

        private static string CreateSurfaceProfile(ConstantThickness bhomSurfaceProperty, string version)
        {
            if (bhomSurfaceProperty.Thickness==0)
            {
                return null;
            }
            else
            {
                string midasSurfaceProperty = "";
                switch (version)
                {

                    case "8.8.5":
                        midasSurfaceProperty =
                        bhomSurfaceProperty.CustomData[AdapterId].ToString() + "," + bhomSurfaceProperty.Name + ",VALUE,Yes," + bhomSurfaceProperty.Thickness + ",0,Yes,0,0";
                        break;
                    default:
                        midasSurfaceProperty =
                        bhomSurfaceProperty.CustomData[AdapterId].ToString() + ",VALUE,Yes," + bhomSurfaceProperty.Thickness + ",0,Yes,0,0";
                        break;
                }

                return midasSurfaceProperty;
            }
        }

        private static string CreateSurfaceProfile(LoadingPanelProperty bhomSurfaceProperty)
        {
            Engine.Reflection.Compute.RecordError("LoadingPanelProperty not supported in MidasCivil_Toolkit");
            return null;
        }

        private static string CreateSurfaceProfile(Ribbed bhomSurfaceProperty)
        {
            Engine.Reflection.Compute.RecordError("Ribbed not supported in MidasCivil_Toolkit");
            return null;
        }

        private static string CreateSurfaceProfile(Waffle bhomSurfaceProperty)
        {
            Engine.Reflection.Compute.RecordError("Waffle not supported in MidasCivil_Toolkit");
            return null;
        }

    }
}
