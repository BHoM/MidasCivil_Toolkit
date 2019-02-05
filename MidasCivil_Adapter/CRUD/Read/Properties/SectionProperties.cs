using BH.oM.Structure.Properties.Section;
using System.Collections.Generic;
using BH.Engine.Reflection;


namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<ISectionProperty> ReadSectionProperties(List<string> ids = null)
        {
            List<ISectionProperty> bhomSectionProperties = new List<ISectionProperty>();

            List<string> sectionProperties = GetSectionText("SECTION");

            for (int i = 0; i < sectionProperties.Count; i++)
            {
                string sectionProperty = sectionProperties[i];
                string type = sectionProperty.Split(',')[1];

                if (type == "VALUE")
                {
                    ISectionProperty bhomSectionProperty = Engine.MidasCivil.Convert.ToBHoMSectionProperty(
                        sectionProperty);
                }
                else if (type == "DBUSER")
                {

                }
                else
                {
                    Engine.Reflection.Compute.RecordError(type + " not supported in the MidasCivil_Toolkit")
                }


                bhomSectionProperties.Add(bhomSectionProperty);
            }

            return bhomSectionProperties;
        }
    }
}
