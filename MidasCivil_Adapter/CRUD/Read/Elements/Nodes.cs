﻿/*
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
using System.Collections.Generic;
using System.Linq;


namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<Node> ReadNodes(List<string> ids = null)
        {
            List<Node> bhomNodes = new List<Node>();

            List<string> nodesText = GetSectionText("NODE");
            List<string> supportText = GetSectionText("CONSTRAINT");
            List<string> springText = GetSectionText("SPRING");

            Dictionary<string, List<int>> nodeGroups = ReadTags("GROUP", 1);

            List<Constraint6DOF> supportsList = Read6DOFConstraints();
            Dictionary<string, Constraint6DOF> supports = supportsList.ToDictionary(x => x.Name.ToString());

            Dictionary<string, List<int>> supportAssignments = GetPropertyAssignments("CONSTRAINT","Support");
            Dictionary<string, List<int>> springAssignments = GetPropertyAssignments("SPRING", "Spring");

            foreach (string node in nodesText)
            {
                Node bhomNode = Adapter.External.MidasCivil.Convert.ToNode(node, supports, supportAssignments, springAssignments);
                int bhomID = System.Convert.ToInt32(bhomNode.CustomData[AdapterIdName]);
                bhomNode.Tags = GetGroupAssignments(nodeGroups, bhomID);
                bhomNodes.Add(bhomNode);
            }

            return bhomNodes;
        }

    }
}
