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
using System.IO;


namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool CreateCollection(IEnumerable<Node> nodes)
        {
            string nodePath = CreateSectionFile("NODE");
            List<string> midasNodes = new List<string>();

            CreateGroups(nodes);

            foreach (Node node in nodes)
            {
                if(!(node.Support==null))
                {
                    if(MidasCivilAdapter.StiffnessVectorModulus(node.Support)>0)
                    {
                        PropertyAssignment(node.CustomData[AdapterIdName].ToString(), node.Support.Name, "SPRING");
                    }
                    else
                    {
                        PropertyAssignment(node.CustomData[AdapterIdName].ToString(), node.Support.Name, "CONSTRAINT");
                    }
                    
                }
                midasNodes.Add(Adapter.External.MidasCivil.Convert.FromNode(node));
            }

            File.AppendAllLines(nodePath, midasNodes);

                return true;
        }

    }
}