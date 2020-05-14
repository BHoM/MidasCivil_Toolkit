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
using System.Collections.Generic;
using BH.oM.Structure.MaterialFragments;
using BH.Engine.Structure;
namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static string FromFEMesh(this FEMesh feMesh)
        {
            string midasElement = "";
            List<int> nodeIndices = feMesh.Faces[0].NodeListIndices;
            string sectionPropertyId = "1";
            string materialId = "1";

            if (!(feMesh.Property == null))
            {
                sectionPropertyId = feMesh.Property.CustomData[AdapterIdName].ToString();

                if (!(feMesh.Property.Material == null))
                {
                    materialId = feMesh.Property.Material.CustomData[AdapterIdName].ToString();
                }
            }
            if (feMesh.Nodes.Count > 4)
            {
                BH.Engine.Reflection.Compute.RecordError("Cannot push mesh with more than 4 nodes");
            }

            if (feMesh.Nodes.Count == 4)
            {
                midasElement = (feMesh.CustomData[AdapterIdName].ToString() + ",PLATE," +
                materialId + "," +
                sectionPropertyId + "," +
              feMesh.Nodes[nodeIndices[0]].CustomData[AdapterIdName].ToString() + "," +
              feMesh.Nodes[nodeIndices[1]].CustomData[AdapterIdName].ToString() + "," +
              feMesh.Nodes[nodeIndices[2]].CustomData[AdapterIdName].ToString() + "," +
              feMesh.Nodes[nodeIndices[3]].CustomData[AdapterIdName].ToString() + ",1,0");
            }
            else
            {
                midasElement = (feMesh.CustomData[AdapterIdName].ToString() + ",PLATE," +
                materialId + "," +
                sectionPropertyId + "," +
             feMesh.Nodes[nodeIndices[0]].CustomData[AdapterIdName].ToString() + "," +
             feMesh.Nodes[nodeIndices[1]].CustomData[AdapterIdName].ToString() + "," +
             feMesh.Nodes[nodeIndices[2]].CustomData[AdapterIdName].ToString() + ",0,1,0");
            }
            return midasElement;
        }

        /***************************************************/
    }
}