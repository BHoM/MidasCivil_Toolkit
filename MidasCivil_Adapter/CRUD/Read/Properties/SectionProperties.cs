using BH.oM.Structure.Properties.Section;
using System.Collections.Generic;
using System.Linq;

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
                string type = sectionProperty.Split(',')[1].Replace(" ","");

                ISectionProperty bhomSectionProperty = null;

                if (type == "VALUE")
                {
                    string sectionProfile = sectionProperty;
                    string sectionProperties1 = sectionProperties[i + 1];
                    string sectionProperties2 = sectionProperties[i + 2];
                    string sectionProperties3 = sectionProperties[i + 3];

                    bhomSectionProperty = Engine.MidasCivil.Convert.ToBHoMSectionProperty(
                        sectionProfile,sectionProperties1,sectionProperties2,sectionProperties3);

                    i = i + 3;
                }
                else if (type == "DBUSER")
                {
                    int numberColumns = sectionProperty.Split(',').Count();

                    if(numberColumns == 16)
                    {
                        Engine.Reflection.Compute.RecordWarning("Library sections are not yet supported in the MidasCivil_Toolkit");
                    }
                    else
                    {
                        bhomSectionProperty = Engine.MidasCivil.Convert.ToBHoMSectionProperty(
                            sectionProperty);
                    }

                }
                else
                {
                    Engine.Reflection.Compute.RecordWarning(type + " not supported in the MidasCivil_Toolkit");
                }

                if (bhomSectionProperty!=null)
                    bhomSectionProperties.Add(bhomSectionProperty);
            }

            return bhomSectionProperties;
        }

    }
}
