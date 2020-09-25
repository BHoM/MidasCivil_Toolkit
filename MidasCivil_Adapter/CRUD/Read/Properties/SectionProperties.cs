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

using BH.oM.Geometry.ShapeProfiles;
using BH.oM.Structure.SectionProperties;
using System;
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
                string type = sectionProperty.Split(',')[1].Trim();

                ISectionProperty bhomSectionProperty = null;

                if (type == "VALUE")
                {
                    string sectionProfile = sectionProperty;
                    string sectionProperties1 = sectionProperties[i + 1];
                    string sectionProperties2 = sectionProperties[i + 2];
                    string sectionProperties3 = sectionProperties[i + 3];

                    List<string> split = sectionProfile.Split(',').ToList();

                    bhomSectionProperty = Adapters.MidasCivil.Convert.ToSectionProperty(
                        split.GetRange(14, sectionProperty.Split(',').Count() - 15), sectionProperties1, sectionProperties2, sectionProperties3,
                        split[12].Trim(), m_lengthUnit);

                    bhomSectionProperty.Name = split[2].Trim();
                    bhomSectionProperty.CustomData[AdapterIdName] = split[0].Trim();

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
                        List<string> split = sectionProperty.Split(',').ToList();
                        bhomSectionProperty = Adapters.MidasCivil.Convert.ToSectionProperty(split.GetRange(14, numberColumns - 16), 
                            split[12].Trim(), m_lengthUnit);

                        bhomSectionProperty.Name = split[2].Trim();
                        bhomSectionProperty.CustomData[AdapterIdName] = split[0].Trim();
                    }
                }
                else if (type == "TAPERED")
                {
                    List<string> split = sectionProperty.Split(',').ToList();
                    List<string> profiles = sectionProperties[i + 1].Split(',').ToList();
                    string shape = split[14].Trim();
                    string interpolationOrder = Math.Max(System.Convert.ToInt32(split[15].Trim()), System.Convert.ToInt32(split[16].Trim())).ToString();

                    bhomSectionProperty = Adapters.MidasCivil.Convert.ToSectionProperty(profiles, "TAPERED" + "-" + shape + "-" + interpolationOrder, m_lengthUnit);

                    bhomSectionProperty.CustomData[AdapterIdName] = split[0].Trim();
                    bhomSectionProperty.Name = split[2].Trim();

                    i = i + 1;
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
