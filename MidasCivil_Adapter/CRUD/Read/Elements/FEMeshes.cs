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

using System.Collections.Generic;
using System.Linq;
using BH.oM.Adapters.MidasCivil;
using BH.Engine.Adapter;
using BH.oM.Structure.Elements;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Structure.SurfaceProperties;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<FEMesh> ReadFEMeshes(List<string> ids = null)
        {
            List<FEMesh> bhomMeshes = new List<FEMesh>();

            List<string> elementsText = GetSectionText("ELEMENT");
            List<string> meshText = elementsText.Where(x => x.Contains("PLATE")).ToList();
            Dictionary<string, List<int>> elementGroups = GetTags("GROUP", 2);

            IEnumerable<Node> bhomNodesList = GetCachedOrRead<Node>();
            Dictionary<string, Node> bhomNodes = bhomNodesList.ToDictionary(x => x.AdapterId<string>(typeof(MidasCivilId)));

            IEnumerable<ISurfaceProperty> bhomSurfacePropertiesList = GetCachedOrRead<ISurfaceProperty>();
            Dictionary<string, ISurfaceProperty> bhomSuraceProperties = bhomSurfacePropertiesList.ToDictionary(x => x.AdapterId<string>(typeof(MidasCivilId)));

            IEnumerable<IMaterialFragment> bhomMaterialList = GetCachedOrRead<IMaterialFragment>();
            Dictionary<string, IMaterialFragment> bhomMaterials = bhomMaterialList.ToDictionary(x => x.AdapterId<string>(typeof(MidasCivilId)));

            foreach (string mesh in meshText)
            {
                FEMesh bhomMesh = Adapters.MidasCivil.Convert.ToFEMesh(mesh, bhomNodes, bhomSuraceProperties, bhomMaterials);
                int bhomID = bhomMesh.AdapterId<int>(typeof(MidasCivilId));
                bhomMesh.Tags = GetGroupAssignments(elementGroups, bhomID);
                bhomMeshes.Add(bhomMesh);
            }

            return bhomMeshes;
        }

        /***************************************************/

    }
}





