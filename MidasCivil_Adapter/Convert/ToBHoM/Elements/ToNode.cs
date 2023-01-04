/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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

using BH.Engine.Adapter;
using BH.oM.Adapters.MidasCivil;
using BH.oM.Geometry;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.Elements;
using System.Collections.Generic;
using System.Linq;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static Node ToNode(this string node, Dictionary<string, Constraint6DOF> supports,
            Dictionary<string, List<int>> supportAssignments, Dictionary<string, List<int>> springAssignments, string lengthUnit)
        {
            List<string> delimitted = node.Split(',').ToList();

            Node bhomNode = new Node
            {
                Position = new Point
                {
                    X = double.Parse(delimitted[1].Trim()).LengthToSI(lengthUnit),
                    Y = double.Parse(delimitted[2].Trim()).LengthToSI(lengthUnit),
                    Z = double.Parse(delimitted[3].Trim()).LengthToSI(lengthUnit)
                }
            };

            bhomNode.SetAdapterId(typeof(MidasCivilId), delimitted[0].Trim());
            int bhomID = bhomNode.AdapterId<int>(typeof(MidasCivilId));

            string supportName = "";

            foreach (KeyValuePair<string, List<int>> supportAssignment in supportAssignments)
            {
                if (supportAssignment.Value.Contains(bhomID))
                {
                    supportName = supportAssignment.Key;
                    break;
                }
            }
            if (!(springAssignments == null))
            {
                foreach (KeyValuePair<string, List<int>> springAssignment in springAssignments)
                {
                    if (springAssignment.Value.Contains(bhomID))
                    {
                        supportName = springAssignment.Key;
                        break;
                    }
                }
            }
            Constraint6DOF nodeConstraint = null;
            if (!(supportName == ""))
            {
                supports.TryGetValue(supportName, out nodeConstraint);
            }


            bhomNode.Support = nodeConstraint;


            return bhomNode;
        }

        /***************************************************/

    }
}



