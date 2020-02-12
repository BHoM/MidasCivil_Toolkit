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

using BH.oM.Structure.Elements;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Structure.SectionProperties;
using System.Collections.Generic;
using System.Linq;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static Bar ToBHoMBar(this string bar, Dictionary<string, Node> bhomNodes,
            Dictionary<string, ISectionProperty> bhomSectionProperties, Dictionary<string, IMaterialFragment> bhomMaterials,
            Dictionary<string, BarRelease> barReleases, Dictionary<string, List<int>> barReleaseAssignments)
        {
            List<string> delimitted = bar.Split(',').ToList();

            BarFEAType feaType = BarFEAType.Axial;
            ISectionProperty sectionProperty = null;

            Node startNode;
            bhomNodes.TryGetValue(delimitted[4].Trim(), out startNode);
            Node endNode;
            bhomNodes.TryGetValue(delimitted[5].Trim(), out endNode);

            if (!(bhomSectionProperties.Count() == 0))
            {
                bhomSectionProperties.TryGetValue(delimitted[3].Trim(), out sectionProperty);
                if (!(bhomMaterials.Count() == 0))
                {
                    IMaterialFragment bhommMaterial;
                    bhomMaterials.TryGetValue(delimitted[2].Trim(), out bhommMaterial);

                    if (bhommMaterial.GetType().ToString().Split('.').Last() == "Concrete")
                    {
                        sectionProperty = BHoMSteeltoConcrete((SteelSection)sectionProperty);
                    }

                    sectionProperty.Material = bhommMaterial;
                }
            }

            switch (delimitted[1].Trim())
            {
                case "TRUSS":
                    break;

                case "BEAM":
                    feaType = BarFEAType.Flexural;
                    break;

                case "TENSTR":
                    feaType = BarFEAType.TensionOnly;
                    break;

                case "COMPTR":
                    feaType = BarFEAType.CompressionOnly;
                    break;
            }

            int bhomID = System.Convert.ToInt32(delimitted[0].Trim());

            string barReleaseName = "";

            foreach (KeyValuePair<string, List<int>> barReleaseAssignment in barReleaseAssignments)
            {
                if (barReleaseAssignment.Value.Contains(bhomID))
                {
                    barReleaseName = barReleaseAssignment.Key;
                    break;
                }
            }

            BarRelease barRelease = null;
            if (!(barReleaseName == ""))
            {
                barReleases.TryGetValue(barReleaseName, out barRelease);
            }

            double orientationAngle = double.Parse(delimitted[6].Trim());

            Bar bhomBar = Structure.Create.Bar(startNode, endNode, sectionProperty, orientationAngle, barRelease, feaType);
            bhomBar.CustomData[AdapterIdName] = bhomID;

            return bhomBar;
        }
    }
}

