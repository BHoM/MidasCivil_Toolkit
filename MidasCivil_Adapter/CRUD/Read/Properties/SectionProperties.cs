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

                ISectionProperty bhomSectionProperty = null;

                if (type == "VALUE")
                {
                    bhomSectionProperty = Engine.MidasCivil.Convert.ToBHoMSectionProperty(
                        sectionProperty);

//; iSEC, TYPE, SNAME, [OFFSET], bSD, bWE, SHAPE, BLT, D1, ..., D8, iCEL              ; 1st line - VALUE
//;       AREA, ASy, ASz, Ixx, Iyy, Izz                                               ; 2nd line
//; CyP, CyM, CzP, CzM, QyB, QzB, PERI_OUT, PERI_IN, Cy, Cz                     ; 3rd line
//; Y1, Y2, Y3, Y4, Z1, Z2, Z3, Z4, Zyy, Zzz                                    ; 4th line

                    i = i + 4;
                }
                else if (type == "DBUSER")
                {

                }
                else
                {
                    Engine.Reflection.Compute.RecordError(type + " not supported in the MidasCivil_Toolkit");
                }


                bhomSectionProperties.Add(bhomSectionProperty);
            }

            return bhomSectionProperties;
        }
    }
}
