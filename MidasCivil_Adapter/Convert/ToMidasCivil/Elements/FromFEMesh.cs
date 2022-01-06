/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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
using System.Collections.Generic;
using System;
using BH.Adapter.MidasCivil;
using System.Text;


namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/
        public static string FromFEMesh(this FEMesh feMesh, ref int index)
        {
            StringBuilder midasElements = new StringBuilder();
            string midasElement = "";
            string sectionPropertyId = "1";
            string materialId = "1";

            if (!(feMesh.Property == null))
            {
                sectionPropertyId = feMesh.Property.AdapterId<string>(typeof(MidasCivilId));

                if (!(feMesh.Property.Material == null))
                {
                    materialId = feMesh.Property.Material.AdapterId<string>(typeof(MidasCivilId));
                }
            }

            foreach (FEMeshFace meshFace in feMesh.Faces)
            {
                int node0 = meshFace.NodeListIndices[0];
                int node1 = meshFace.NodeListIndices[1];
                int node2 = meshFace.NodeListIndices[2];

                if (meshFace.NodeListIndices.Count < 3)
                {
                    BH.Engine.Reflection.Compute.RecordError("Cannot push a FEMesh with less than three nodes");
                }

                if (meshFace.NodeListIndices.Count == 4)
                {
                    int node3 = meshFace.NodeListIndices[3];
                    midasElement = (index + ",PLATE," +
                    materialId + "," +
                    sectionPropertyId + "," +
                    feMesh.Nodes[node0].AdapterId<string>(typeof(MidasCivilId)) + "," +
                    feMesh.Nodes[node1].AdapterId<string>(typeof(MidasCivilId)) + "," +
                    feMesh.Nodes[node2].AdapterId<string>(typeof(MidasCivilId)) + "," +
                    feMesh.Nodes[node3].AdapterId<string>(typeof(MidasCivilId)) + ",1,0");
                }
                else if (meshFace.NodeListIndices.Count == 3)
                {
                    midasElement = (index + ",PLATE," +
                    materialId + "," +
                    sectionPropertyId + "," +
                    feMesh.Nodes[node0].AdapterId<string>(typeof(MidasCivilId)) + "," +
                    feMesh.Nodes[node1].AdapterId<string>(typeof(MidasCivilId)) + "," +
                    feMesh.Nodes[node2].AdapterId<string>(typeof(MidasCivilId)) + ",0,1,0");
                }
                meshFace.SetAdapterId(typeof(MidasCivilId), index);
                midasElements.AppendLine(midasElement);
                index++;

            }
            return midasElements.ToString();
        }

        /***************************************************/
    }
}

