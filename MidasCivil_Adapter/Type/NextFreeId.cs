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

using System;
using System.Collections.Generic;
using BH.oM.Structure.Elements;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.MaterialFragments;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        /***************************************************/
        /**** Adapter overload method                   ****/
        /***************************************************/

        protected override object NextFreeId(Type type, bool refresh = false)
        {
            //Method that returns the next free index for a specific object type.
            //Software dependent which type of index to return. Could be int, string, Guid or whatever the specific software is using
            //At the point of index assignment, the objects have not yet been created in the target software. 
            //The if statement below is designed to grab the first free index for the first object being created and after that increment.

            //Change from object to what the specific software is using
            int index = 1;

            if (!refresh && m_indexDict.TryGetValue(type, out index))
            {
                index++;
                m_indexDict[type] = index;
            }
            else
            {
                if (type == typeof(Node))
                {
                    string section = "NODE";

                    if (ExistsSection(section))
                    {
                        List<Node> nodes = ReadNodes();
                        List<int> nodeID = new List<int>();
                        nodes.ForEach(x => nodeID.Add(Convert.ToInt32(x.CustomData[AdapterIdName].ToString())));
                        nodeID.Sort();
                        nodeID.Reverse();

                        if (!(nodes == null))
                        {
                            index = nodeID[0] + 1;
                        }
                        else
                        {
                            index = 1;
                        }
                    }
                    else
                    {
                        index = 1;
                    }

                }

                if (type == typeof(Bar))
                {
                    string section = "ELEMENT";

                    if (ExistsSection(section))
                    {
                        index = GetMaxId(section) + 1;
                    }
                    else
                    {
                        index = 1;
                    }

                }

                if (type == typeof(FEMesh))
                {
                    string section = "ELEMENT";

                    if (ExistsSection(section))
                    {
                        index = GetMaxId(section) + 1;
                    }
                    else
                    {
                        index = 1;
                    }
                }

                if (type == typeof(ConstantThickness))
                {
                    string section = "THICKNESS";

                    if (ExistsSection(section))
                    {

                        index = GetMaxId(section) +1;
                    }
                    else
                    {
                        index = 1;
                    }
                }

                if(typeof(ISectionProperty).IsAssignableFrom(type))
                {
                    string section = "SECTION";

                    if (ExistsSection(section))
                    {
                        index = GetMaxId(section) + 1;
                    }
                    else
                    {
                        index = 1;
                    }
                }

                if (typeof(IMaterialFragment).IsAssignableFrom(type))
                {
                    string section = "MATERIAL";

                    if (ExistsSection(section))
                    {
                        index = GetMaxId(section) + 1;
                    }
                    else
                    {
                        index = 1;
                    }
                }

            }

                m_indexDict[type] = index;
                return index;
        }


        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        //Change from object to the index type used by the specific software
        private Dictionary<Type, int> m_indexDict = new Dictionary<Type, int>();


        /***************************************************/
    }
}
