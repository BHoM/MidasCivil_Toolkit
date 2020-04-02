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
using BH.oM.Structure.Elements;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.MaterialFragments;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<Bar> ReadBars(List<string> ids = null)
        {
            List<Bar> bhomBars = new List<Bar>();
            List<string> beamElements = new List<string> { "BEAM", "TRUSS", "TENSTR", "COMPTR" };
            List<string> barText = new List<string>();

            List<string> elementsText = GetSectionText("ELEMENT");
            Dictionary<string, List<int>> elementGroups = ReadTags("GROUP", 2);

            foreach (string element in elementsText)
            {
                foreach (string type in beamElements)
                {
                    if (element.Contains(type))
                        barText.Add(element);
                }
            }

            IEnumerable<Node> bhomNodesList = ReadNodes();
            Dictionary<string, Node> bhomNodes = bhomNodesList.ToDictionary(
                x => x.CustomData[AdapterIdName].ToString());

            IEnumerable<BarRelease> bhomBarReleaseList = ReadBarReleases();
            Dictionary<string, BarRelease> bhomBarReleases = bhomBarReleaseList.ToDictionary(x => x.CustomData[AdapterIdName].ToString());

            IEnumerable<ISectionProperty> bhomSectionPropertyList = ReadSectionProperties();
            Dictionary<string, ISectionProperty> bhomSectionProperties = bhomSectionPropertyList.ToDictionary(
                x => x.CustomData[AdapterIdName].ToString());

            IEnumerable<IMaterialFragment> bhomMaterialList = ReadMaterials();
            Dictionary<string, IMaterialFragment> bhomMaterials = bhomMaterialList.ToDictionary(x => x.CustomData[AdapterIdName].ToString());

            Dictionary<string, List<int>> barReleaseAssignments = GetBarReleaseAssignments("FRAME-RLS", "barRelease");

            List<List<string>> materialSectionCombos = barText.Select(x => x.Split(',').ToList()[2].Trim() + "," + x.Split(',').ToList()[3].Trim())
                .Distinct()
                .Select(x => x.Split(',').ToList())
                .ToList();

            Dictionary<string, ISectionProperty> materialSections = SectionMaterialCombinations(materialSectionCombos, bhomMaterials, bhomSectionProperties);

            foreach (string bar in barText)
            {
                Bar bhomBar = Engine.External.MidasCivil.Convert.ToBar(bar, bhomNodes, materialSections, bhomBarReleases, barReleaseAssignments);
                int bhomID = System.Convert.ToInt32(bhomBar.CustomData[AdapterIdName]);
                bhomBar.Tags = GetGroupAssignments(elementGroups, bhomID);
                bhomBars.Add(bhomBar);
            }

            return bhomBars;
        }

    }
}
