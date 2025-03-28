/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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

using System.IO;
using System.Collections.Generic;
using BH.oM.Structure.SectionProperties;
using BH.oM.Spatial.ShapeProfiles;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private bool CreateCollection(IEnumerable<ISectionProperty> sectionProperties)
        {
            string sectionPath = CreateSectionFile("SECTION");
            string pscSectionPath = CreateSectionFile("SECT-PSCVALUE");
            List<string> midasSectionProperties = new List<string>();
            List<string> midasPSCSectionProperties = new List<string>();

            foreach (ISectionProperty sectionProperty in sectionProperties)
            {
                List<string> midasSectionProperty = Adapters.MidasCivil.Convert.FromSectionProperty(sectionProperty, m_lengthUnit, m_sectionPropertyCharacterLimit);
                if (midasSectionProperty != null)
                {
                    if (sectionProperty is SteelSection)
                    {
                        SteelSection steelSection = (SteelSection)sectionProperty;
                        if (steelSection.SectionProfile is FreeFormProfile)
                            midasPSCSectionProperties.AddRange(midasSectionProperty);
                        else
                            midasSectionProperties.AddRange(midasSectionProperty);
                    }
                    else if (sectionProperty is ConcreteSection)
                    {
                        ConcreteSection concreteSection = (ConcreteSection)sectionProperty;
                        if (concreteSection.SectionProfile is FreeFormProfile)
                            midasPSCSectionProperties.AddRange(midasSectionProperty);
                        else
                            midasSectionProperties.AddRange(midasSectionProperty);
                    }
                    else if (sectionProperty is TimberSection)
                    {
                        TimberSection timberSection = (TimberSection)sectionProperty;
                        if (timberSection.SectionProfile is FreeFormProfile)
                            midasPSCSectionProperties.AddRange(midasSectionProperty);
                        else
                            midasSectionProperties.AddRange(midasSectionProperty);
                    }
                    else if (sectionProperty is AluminiumSection)
                    {
                        AluminiumSection aluminiumSection = (AluminiumSection)sectionProperty;
                        if (aluminiumSection.SectionProfile is FreeFormProfile)
                            midasPSCSectionProperties.AddRange(midasSectionProperty);
                        else
                            midasSectionProperties.AddRange(midasSectionProperty);
                    }
                    else
                    {
                        GenericSection steelSection = (GenericSection)sectionProperty;
                        if (steelSection.SectionProfile is FreeFormProfile)
                            midasPSCSectionProperties.AddRange(midasSectionProperty);
                        else
                            midasSectionProperties.AddRange(midasSectionProperty);
                    }
                }
            }

            File.AppendAllLines(sectionPath, midasSectionProperties);
            File.AppendAllLines(pscSectionPath, midasPSCSectionProperties);

            return true;
        }

        /***************************************************/

    }
}




