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
using System.Collections.Generic;
namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/
        private static int i = 0;
        public static string FromFEMesh(this FEMesh feMesh)
        {

            string midasElement = "";
            List<int> nodeIndices = feMesh.Faces[0].NodeListIndices;
            string sectionPropertyId = "1";
            string materialId = "1";

            if (feMesh.Faces.Count == 1)
            {
                if (!(feMesh.Property == null))
                {
                    sectionPropertyId = feMesh.Property.AdapterId<string>(typeof(MidasCivilId));

                    if (!(feMesh.Property.Material == null))
                    {
                        materialId = feMesh.Property.Material.AdapterId<string>(typeof(MidasCivilId));
                    }
                }
                if (feMesh.Nodes.Count < 3)
                {
                    BH.Engine.Reflection.Compute.RecordError("Cannot push mesh with less than 3 nodes");
                }

                if (feMesh.Faces[0].NodeListIndices.Count == 4)
                {
                    midasElement = (feMesh.AdapterId<string>(typeof(MidasCivilId)) + ",PLATE," +
                    materialId + "," +
                    sectionPropertyId + "," +
                  feMesh.Nodes[nodeIndices[0]].AdapterId<string>(typeof(MidasCivilId)) + "," +
                  feMesh.Nodes[nodeIndices[1]].AdapterId<string>(typeof(MidasCivilId)) + "," +
                  feMesh.Nodes[nodeIndices[2]].AdapterId<string>(typeof(MidasCivilId)) + "," +
                  feMesh.Nodes[nodeIndices[3]].AdapterId<string>(typeof(MidasCivilId)) + ",1,0");
                }
                else if (feMesh.Faces[0].NodeListIndices.Count == 3)
                {
                    midasElement = (feMesh.AdapterId<string>(typeof(MidasCivilId)) + ",PLATE," +
                    materialId + "," +
                    sectionPropertyId + "," +
                 feMesh.Nodes[nodeIndices[0]].AdapterId<string>(typeof(MidasCivilId)) + "," +
                 feMesh.Nodes[nodeIndices[1]].AdapterId<string>(typeof(MidasCivilId)) + "," +
                 feMesh.Nodes[nodeIndices[2]].AdapterId<string>(typeof(MidasCivilId)) + ",0,1,0");
                }
            }
            else if (feMesh.Faces.Count > 1)
            {
                i++;
                foreach (FEMeshFace meshFace in feMesh.Faces)
                {
                    int Node0 = meshFace.NodeListIndices[0];
                    int Node1 = meshFace.NodeListIndices[1];
                    int Node2 = meshFace.NodeListIndices[2];
           
                    nodeIndices = meshFace.NodeListIndices;
                    if (nodeIndices.Count == 4)
                    {
                        int Node3 = meshFace.NodeListIndices[3];
                        string j = i.ToString();//Will end up with random plate numbers
                        FEMesh discreteMesh = new FEMesh();
                        discreteMesh.Nodes.AddRange(feMesh.Nodes);//Unfortunatley need all the nodes because the node indices need to match
                        FEMeshFace disFace = new FEMeshFace();
                        disFace.NodeListIndices.Add(Node0);
                        disFace.NodeListIndices.Add(Node1);
                        disFace.NodeListIndices.Add(Node2);
                        disFace.NodeListIndices.Add(Node3);
                        discreteMesh.Faces.Add(disFace);

                        //discreteMesh.Property.Material.SetAdapterId<string>(typeof(MidasCivilId), feMesh.Property.Material.AdapterId<string>(typeof(MidasCivilId)));

                        discreteMesh.SetAdapterId<string>(typeof(MidasCivilId), j);
                        midasElement += "\n" + FromFEMesh(discreteMesh);
                    }
                    else if (nodeIndices.Count == 3)
                    {
                        string j = i.ToString();
                        FEMesh discreteMesh = new FEMesh();
                        discreteMesh.Nodes.AddRange(feMesh.Nodes);
                        FEMeshFace disFace = new FEMeshFace();
                        disFace.NodeListIndices.Add(Node0);
                        disFace.NodeListIndices.Add(Node1);
                        disFace.NodeListIndices.Add(Node2);
                        discreteMesh.Faces.Add(disFace);

                        //discreteMesh.Property.Material.SetAdapterId<string>(typeof(MidasCivilId), feMesh.Property.Material.AdapterId<string>(typeof(MidasCivilId)));

                        discreteMesh.SetAdapterId<string>(typeof(MidasCivilId), j);
                        midasElement += "\n" + FromFEMesh(discreteMesh);
                    }
                    i++;

                }
            }
            return midasElement;
        }

        /***************************************************/
    }
}
