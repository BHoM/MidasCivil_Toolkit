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
using BH.oM.Structure.MaterialFragments;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Geometry;
using System.Collections.Generic;
using System.Linq;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static Panel ToPanel(FEMesh mesh)
        {
            List<Polyline> polylines = new List<Polyline>();

            List<Point> points = new List<Point>();

            foreach (Node node in mesh.Nodes)
            {
                points.Add(node.Position);
            }

            points.Add(mesh.Nodes.First().Position);
            polylines.Add(BH.Engine.Geometry.Create.Polyline(points));

            List<Panel> panels = BH.Engine.Structure.Create.PanelPlanar(polylines);

            if (mesh.CustomData.ContainsValue(AdapterIdName))
                panels[0].CustomData[AdapterIdName] = mesh.CustomData[AdapterIdName];

            return panels[0];
        }

        /***************************************************/

    }
}