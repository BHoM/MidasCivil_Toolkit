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

using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.MaterialFragments;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static Dictionary<string, ISectionProperty> GetSectionMaterialCombinations(
            List<List<string>> combinations, Dictionary<string, IMaterialFragment> materials,
            Dictionary<string, ISectionProperty> sectionProperties)
        {
            IMaterialFragment material;
            ISectionProperty section;
            Dictionary<string, ISectionProperty> materialSections = new Dictionary<string, ISectionProperty>();

            foreach (List<string> materialSection in combinations)
            {
                int materialId = System.Convert.ToInt32(materialSection[0].Trim());
                int sectionPropertyId = System.Convert.ToInt32(materialSection[1].Trim());

                materials.TryGetValue(materialId.ToString(), out material);
                sectionProperties.TryGetValue(sectionPropertyId.ToString(), out section);

                GenericSection genericSection = (GenericSection)section;

                switch (material.GetType().ToString().Split('.').Last())
                {
                    case "Concrete":
                        ConcreteSection concreteSection = Engine.Structure.Create.ConcreteSectionFromProfile(genericSection.SectionProfile, (Concrete)material);
                        concreteSection.Name = genericSection.Name;
                        materialSections.Add(materialId.ToString() + "," + sectionPropertyId.ToString(), concreteSection);
                        break;
                    case "Steel":
                        SteelSection steelSection = Engine.Structure.Create.SteelSectionFromProfile(genericSection.SectionProfile, (Steel)material);
                        steelSection.Name = genericSection.Name;
                        materialSections.Add(materialId.ToString() + "," + sectionPropertyId.ToString(), steelSection);
                        break;
                    case "Aluminium":
                        AluminiumSection aluminiumSection = Engine.Structure.Create.AluminiumSectionFromProfile(genericSection.SectionProfile, (Aluminium)material);
                        aluminiumSection.Name = genericSection.Name;
                        materialSections.Add(materialId.ToString() + "," + sectionPropertyId.ToString(), aluminiumSection);
                        break;
                    case "Timber":
                        TimberSection timberSection = Engine.Structure.Create.TimberSectionFromProfile(genericSection.SectionProfile, (Timber)material);
                        timberSection.Name = genericSection.Name;
                        materialSections.Add(materialId.ToString() + "," + sectionPropertyId.ToString(), timberSection);
                        break;
                    case "GenericIsotropicMaterial":
                        GenericSection genericIsoptropicSection =
                            Engine.Structure.Create.GenericSectionFromProfile(genericSection.SectionProfile, (GenericIsotropicMaterial)material);
                        genericIsoptropicSection.Name = genericSection.Name;
                        materialSections.Add(materialId.ToString() + "," + sectionPropertyId.ToString(), genericIsoptropicSection);
                        break;
                    case "GenericOrthotropicMaterial":
                        GenericSection genericOrthotropicSection =
                            Engine.Structure.Create.GenericSectionFromProfile(genericSection.SectionProfile, (GenericOrthotropicMaterial)material);
                        genericOrthotropicSection.Name = genericSection.Name;
                        materialSections.Add(materialId.ToString() + "," + sectionPropertyId.ToString(), genericOrthotropicSection);
                        break;
                    default:
                        Engine.Reflection.Compute.RecordError(material.GetType().ToString().Split('.').Last() + "not recognised");
                        break;
                }
            }

            return materialSections;
        }

        /***************************************************/

    }
}
