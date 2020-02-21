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

using System.Linq;
using System.Collections.Generic;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Constraints;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static RigidLink ToRigidLink(string rigidLink, Dictionary<string,Node> nodes, int count)
        {
            string[] delimitted = rigidLink.Split(',');
            List<Node> slaveNodes = new List<Node>();

            string master = delimitted[0].Trim();
            string fixity = delimitted[1].Replace(" ","");
            List<string> slaves = delimitted[2].Split(' ').Where(m => !string.IsNullOrWhiteSpace(m)).ToList();
            List<int> assignments = Engine.MidasCivil.Query.Assignments(slaves);

            bool x = Engine.MidasCivil.Compute.Fixity(fixity.Substring(0, 1));
            bool y = Engine.MidasCivil.Compute.Fixity(fixity.Substring(1, 1));
            bool z = Engine.MidasCivil.Compute.Fixity(fixity.Substring(2, 1));
            bool xx = Engine.MidasCivil.Compute.Fixity(fixity.Substring(3, 1));
            bool yy = Engine.MidasCivil.Compute.Fixity(fixity.Substring(4, 1));
            bool zz = Engine.MidasCivil.Compute.Fixity(fixity.Substring(5, 1));

            LinkConstraint constraint = new LinkConstraint { XtoX = x, YtoY = y, ZtoZ = z, XXtoXX = xx, YYtoYY = yy, ZZtoZZ = zz };

            Node masterNode;
            nodes.TryGetValue(master, out masterNode);

            foreach (int assignment in assignments)
            {
                Node bhomSlave;
                nodes.TryGetValue(assignment.ToString(), out bhomSlave);
                slaveNodes.Add(bhomSlave);
            }

            string name = "";

            if (string.IsNullOrWhiteSpace(delimitted[3]))
            {
                name = "RL" + count;
            }
            else
            {
                name = delimitted[3].Trim();
            }

            RigidLink bhomRigidLink = Engine.Structure.Create.RigidLink(masterNode, slaveNodes, constraint);
            bhomRigidLink.Name = name;
            bhomRigidLink.CustomData[AdapterIdName] = name;

            return bhomRigidLink;
        }

    }
}

