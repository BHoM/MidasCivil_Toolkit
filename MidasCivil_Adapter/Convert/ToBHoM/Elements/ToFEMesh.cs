/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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

using BH.oM.Adapters.MidasCivil;
using BH.Engine.Adapter;
using BH.oM.Structure.Elements;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Structure.SurfaceProperties;
using System.Collections.Generic;
using System.Linq;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static FEMesh ToFEMesh(
            this string feMesh,
            Dictionary<string, Node> bhomNodes,
            Dictionary<string, ISurfaceProperty> bhomSurfaceProperties,
            Dictionary<string, IMaterialFragment> bhomMaterials)
        {
            List<string> delimitted = feMesh.Split(',').ToList();

            Node n1; Node n2; Node n3;
            bhomNodes.TryGetValue(delimitted[4].Trim(), out n1);
            bhomNodes.TryGetValue(delimitted[5].Trim(), out n2);
            bhomNodes.TryGetValue(delimitted[6].Trim(), out n3);

            ISurfaceProperty bhomSurfaceProperty = null;

            if (!(bhomSurfaceProperties.Count() == 0))
            {
                bhomSurfaceProperties.TryGetValue(delimitted[3].Trim(), out bhomSurfaceProperty);

                if (!(bhomMaterials.Count() == 0))
                {
                    IMaterialFragment bhomMaterial;
                    bhomMaterials.TryGetValue(delimitted[2].Trim(), out bhomMaterial);
                    bhomSurfaceProperty.Material = bhomMaterial;
                }
            }

            List<Node> nodeList = new List<Node>()
            {
                n1, n2, n3
            };

            List<int> nodeListIndicies = Enumerable.Range(0, 3).ToList();

            if (System.Convert.ToInt32(delimitted[7].Trim()) != 0)
            {
                nodeListIndicies = Enumerable.Range(0, 4).ToList();
                Node n4;
                bhomNodes.TryGetValue(delimitted[7].Trim(), out n4);
                nodeList.Add(n4);
            }


            List<FEMeshFace> feMeshFace = new List<FEMeshFace>()
                {
                    new FEMeshFace() {NodeListIndices = nodeListIndicies}
                };

            FEMesh bhomFEMesh = new FEMesh()
            {
                Faces = feMeshFace,
                Nodes = nodeList,
                Property = bhomSurfaceProperty
            };

            bhomFEMesh.SetAdapterId(typeof(MidasCivilId), delimitted[0].Trim());

            return bhomFEMesh;
        }

        /***************************************************/

    }
}
