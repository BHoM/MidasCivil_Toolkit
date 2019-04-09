using BH.oM.Structure.Properties.Surface;
using System.Collections.Generic;


namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<ISurfaceProperty> ReadSurfaceProperties(List<string> ids = null)
        {
            List<ISurfaceProperty> bhomSurfaceProperties = new List<ISurfaceProperty>();

            List<string> surfaceProperties = GetSectionText("THICKNESS");
            for (int i = 0; i < surfaceProperties.Count; i++)
            {
                string surfaceProperty = surfaceProperties[i];
                string type = surfaceProperty.Split(',')[1].Replace(" ", "");

                ISurfaceProperty bhomSurfaceProperty = null;

                if (type == "STIFFENED")
                {
                    Engine.Reflection.Compute.RecordWarning("Stiffened sections not supported in MidasCivil_Toolkit");

                    i = i + 2;
                }
                else if (type == "VALUE")
                {
                    bhomSurfaceProperty = Engine.MidasCivil.Convert.ToBHoMSurfaceProperty(surfaceProperty);
                    bhomSurfaceProperties.Add(bhomSurfaceProperty);
                }
            }

            return bhomSurfaceProperties;
        }

    }
}
