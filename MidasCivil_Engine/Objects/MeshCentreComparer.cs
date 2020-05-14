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
using BH.oM.Geometry;
using BH.Engine.Structure;
using System.Collections.Generic;

namespace BH.Engine.Adapters.MidasCivil.Comparer
{
    public class MeshCentreComparer : IEqualityComparer<FEMesh>
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public MeshCentreComparer()
        {
            nodeComparer = new NodeDistanceComparer();
        }

        public MeshCentreComparer(int decimals)
        {
            nodeComparer = new NodeDistanceComparer(decimals);
        }

        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public bool Equals(FEMesh mesh1, FEMesh mesh2)
        {
            if (ReferenceEquals(mesh1, mesh2)) return true;

            if (ReferenceEquals(mesh1, null) || ReferenceEquals(mesh2, null))
                return false;

            if (mesh1.BHoM_Guid == mesh2.BHoM_Guid)
                return true;

            Panel panel1 = Compute.FEMeshToPanel(mesh1);
            Panel panel2 = Compute.FEMeshToPanel(mesh2);
            List<Point> controlPoints1 = BH.Engine.Structure.Query.ControlPoints(panel1, true);
            List<Point> controlPoints2 = BH.Engine.Structure.Query.ControlPoints(panel2, true);
            Point centrePoint1 = BH.Engine.Geometry.Query.Average(controlPoints1);
            Point centrePoint2 = BH.Engine.Geometry.Query.Average(controlPoints2);

            if (nodeComparer.Equals(new Node() { Position = centrePoint1 }, new Node() { Position = centrePoint1 }))
                return nodeComparer.Equals(new Node() { Position = centrePoint1 }, new Node() { Position = centrePoint1 });

            return false;
        }

        /***************************************************/

        public int GetHashCode(FEMesh mesh)
        {
            return mesh.GetHashCode();
        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private NodeDistanceComparer nodeComparer;

        /***************************************************/

    }
}

