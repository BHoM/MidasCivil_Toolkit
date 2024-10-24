/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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
using BH.oM.Adapters.MidasCivil;
using BH.Engine.Adapter;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Constraints;
using BH.Adapter.MidasCivil;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        public static RigidLink ToRigidLink(string rigidLink, Dictionary<string, Node> nodes, int count, string version)
        {
            /***************************************************/
            /**** Public Methods                            ****/
            /***************************************************/

            string[] delimitted = rigidLink.Split(',');
            List<Node> secondaryNodes = new List<Node>();

            string primaryId = "";
            string fixity = "";
            List<string> secondaryIds = new List<string>();

            string name = "";

            switch (version)
            {
                case "9.0.5":
                case "9.1.0":
                case "9.4.0":
                case "9.4.5":
                case "9.5.0":
                    primaryId = delimitted[0].Trim();
                    fixity = delimitted[1].Replace(" ", "");
                    secondaryIds = delimitted[2].Split(' ').Where(m => !string.IsNullOrWhiteSpace(m)).ToList();
                    if (string.IsNullOrWhiteSpace(delimitted[3]))
                    {
                        name = "RL" + count;
                    }
                    else
                    {
                        name = delimitted[3].Trim();
                    }
                    break;
                default:
                    primaryId = delimitted[1].Trim();
                    fixity = delimitted[2].Replace(" ", "");
                    secondaryIds = delimitted[3].Split(' ').Where(m => !string.IsNullOrWhiteSpace(m)).ToList();
                    if (string.IsNullOrWhiteSpace(delimitted[4]))
                    {
                        name = "RL" + count;
                    }
                    else
                    {
                        name = delimitted[4].Trim();
                    }
                    break;
            }

            List<int> assignments = MidasCivilAdapter.GetAssignmentIds(secondaryIds);

            bool x = FromFixity(fixity.Substring(0, 1));
            bool y = FromFixity(fixity.Substring(1, 1));
            bool z = FromFixity(fixity.Substring(2, 1));
            bool xx = FromFixity(fixity.Substring(3, 1));
            bool yy = FromFixity(fixity.Substring(4, 1));
            bool zz = FromFixity(fixity.Substring(5, 1));

            LinkConstraint constraint = new LinkConstraint { XtoX = x, YtoY = y, ZtoZ = z, XXtoXX = xx, YYtoYY = yy, ZZtoZZ = zz };

            Node primaryNode;
            nodes.TryGetValue(primaryId, out primaryNode);

            foreach (int assignment in assignments)
            {
                Node secondaryNode;
                nodes.TryGetValue(assignment.ToString(), out secondaryNode);
                secondaryNodes.Add(secondaryNode);
            }

            RigidLink bhomRigidLink = new RigidLink { PrimaryNode = primaryNode, SecondaryNodes = secondaryNodes, Constraint = constraint };
            bhomRigidLink.Name = name;
            bhomRigidLink.SetAdapterId(typeof(MidasCivilId), name);

            return bhomRigidLink;
        }

        /***************************************************/

    }
}





