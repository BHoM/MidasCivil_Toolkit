using BH.oM.Structure.Properties.Section;
using System.Collections.Generic;
using System.Linq;
using BH.Engine.Reflection;


namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<ISectionProperty> ReadSectionProperties(List<string> ids = null)
        {
            List<ISectionProperty> bhomSectionProperties = new List<ISectionProperty>();

            List<string> sectionProperties = GetSectionText("SECTION");

            for (int i = 0; i < sectionProperties.Count-1; i++)
            {
                string sectionProperty = sectionProperties[i];
                string type = sectionProperty.Split(',')[1].Replace(" ","");

                ISectionProperty bhomSectionProperty = null;

                if (type == "VALUE")
                {
                    bhomSectionProperty = Engine.MidasCivil.Convert.ToBHoMSectionProperty(
                        sectionProperty);

                    string profile = sectionProperty;
                    string sectionProperties1 = sectionProperties[i + 1];
                    string sectionProperties2 = sectionProperties[i + 2];
                    string sectionProperties3 = sectionProperties[i + 3];

                    //; iSEC, TYPE, SNAME, [OFFSET], bSD, bWE, SHAPE, BLT, D1, ..., D8, iCEL              ; 1st line - VALUE
                    //;       AREA, ASy, ASz, Ixx, Iyy, Izz                                               ; 2nd line
                    //; CyP, CyM, CzP, CzM, QyB, QzB, PERI_OUT, PERI_IN, Cy, Cz                     ; 3rd line
                    //; Y1, Y2, Y3, Y4, Z1, Z2, Z3, Z4, Zyy, Zzz                                    ; 4th line

                    i = i + 4;
                }
                else if (type == "DBUSER")
                {
                    int numberColumns = sectionProperty.Split(',').Count();

                    if(numberColumns == 16)
                    {
                        Engine.Reflection.Compute.RecordError("Library sections are not yet supported in the MidasCivil_Toolkit");
                    }
                    else
                    {
                        bhomSectionProperty = Engine.MidasCivil.Convert.ToBHoMSectionProperty(
                            sectionProperty);
                    }

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
