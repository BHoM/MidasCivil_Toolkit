/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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
using BH.oM.Adapters.MidasCivil;
using BH.Engine.Adapter;
using System.Collections.Generic;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static PointLoad ToPointLoad(this string pointLoad, List<string> associatedNodes, string loadcase,
            Dictionary<string, Loadcase> loadcaseDictionary, Dictionary<string, Node> nodeDictionary, int count, string forceUnit, string lengthUnit)
        {
            string[] delimitted = pointLoad.Split(',');
            List<Node> bhomAssociatedNodes = new List<Node>();

            Loadcase bhomLoadcase;
            loadcaseDictionary.TryGetValue(loadcase, out bhomLoadcase);

            foreach (string associatedNode in associatedNodes)
            {
                Node bhomAssociatedNode;
                nodeDictionary.TryGetValue(associatedNode, out bhomAssociatedNode);
                bhomAssociatedNodes.Add(bhomAssociatedNode);
            }

            Vector forceVector = new Vector
            {
                X = double.Parse(delimitted[0].Trim()).ForceToSI(forceUnit),
                Y = double.Parse(delimitted[1].Trim()).ForceToSI(forceUnit),
                Z = double.Parse(delimitted[2].Trim()).ForceToSI(forceUnit)
            };

            Vector momentVector = new Vector
            {
                X = double.Parse(delimitted[3].Trim()).MomentToSI(forceUnit, lengthUnit),
                Y = double.Parse(delimitted[4].Trim()).MomentToSI(forceUnit, lengthUnit),
                Z = double.Parse(delimitted[5].Trim()).MomentToSI(forceUnit, lengthUnit)
            };



            string name;

            if (string.IsNullOrWhiteSpace(delimitted[6]))
            {
                name = "PF" + count;
            }
            else
            {
                name = delimitted[6].Trim();
            }

            IEnumerable<Node> nodes = bhomAssociatedNodes;

            PointLoad bhomPointLoad = Engine.Structure.Create.PointLoad(bhomLoadcase, nodes, forceVector, momentVector, LoadAxis.Global, name);
            bhomPointLoad.SetAdapterId(typeof(MidasCivilId), bhomPointLoad.Name);

            return bhomPointLoad;
        }

        /***************************************************/

    }
}






