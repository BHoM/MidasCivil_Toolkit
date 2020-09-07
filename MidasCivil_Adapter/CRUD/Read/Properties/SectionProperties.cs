/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using BH.oM.Structure.SectionProperties;
using System.Collections.Generic;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<ISectionProperty> ReadSectionProperties(List<string> ids = null)
        {
            List<ISectionProperty> bhomSectionProperties = new List<ISectionProperty>();

            List<string> sectionProperties = GetSectionText("SECTION");

            for (int i = 0; i < sectionProperties.Count; i++)
            {
                string sectionProperty = sectionProperties[i];
                string type = sectionProperty.Split(',')[1].Replace(" ", "");

                ISectionProperty bhomSectionProperty = null;

                if (type == "VALUE")
                {
                    string sectionProfile = sectionProperty;
                    string sectionProperties1 = sectionProperties[i + 1];
                    string sectionProperties2 = sectionProperties[i + 2];
                    string sectionProperties3 = sectionProperties[i + 3];

                    bhomSectionProperty = Adapters.MidasCivil.Convert.ToSectionProperty(
                        sectionProfile, sectionProperties1, sectionProperties2, sectionProperties3, m_lengthUnit);

                    i = i + 3;
                }
                else if (type == "DBUSER")
                {
                    int numberColumns = sectionProperty.Split(',').Count();

                    if (numberColumns == 16)
                    {
                        Engine.Reflection.Compute.RecordWarning("Library sections are not yet supported in the MidasCivil_Toolkit");
                    }
                    else
                    {
                        bhomSectionProperty = Adapters.MidasCivil.Convert.ToSectionProperty(
                            sectionProperty, m_lengthUnit);
                    }

                }
                else
                {
                    Engine.Reflection.Compute.RecordWarning(type + " not supported in the MidasCivil_Toolkit");
                }

                if (bhomSectionProperty != null)
                    bhomSectionProperties.Add(bhomSectionProperty);
            }

            return bhomSectionProperties;
        }

        /***************************************************/

    }
}
