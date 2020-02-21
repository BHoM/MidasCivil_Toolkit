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

namespace BH.Engine.MidasCivil.Comparer
{
    public class BarMidPointComparer : IEqualityComparer<Bar>
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public BarMidPointComparer()
        {
            nodeComparer = new NodeDistanceComparer();
        }

        /***************************************************/

        public BarMidPointComparer(int decimals)
        {
            nodeComparer = new NodeDistanceComparer(decimals);
        }


        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public bool Equals(Bar bar1, Bar bar2)
        {
            if (ReferenceEquals(bar1, bar2)) return true;

            if (ReferenceEquals(bar1, null) || ReferenceEquals(bar2, null))
                return false;

            if (bar1.BHoM_Guid == bar2.BHoM_Guid)
                return true;

            Point centrePoint1 = BH.Engine.Geometry.Query.IPointAtParameter(bar1.Centreline(), 0.5);
            Point centrePoint2 = BH.Engine.Geometry.Query.IPointAtParameter(bar2.Centreline(), 0.5);

            if (nodeComparer.Equals(BH.Engine.MidasCivil.Convert.ToNode(centrePoint1), BH.Engine.MidasCivil.Convert.ToNode(centrePoint2)))
            {
                return nodeComparer.Equals(BH.Engine.MidasCivil.Convert.ToNode(centrePoint1), BH.Engine.MidasCivil.Convert.ToNode(centrePoint2));
            }

            return false;
        }

        /***************************************************/

        public int GetHashCode(Bar bar)
        {
            //Check whether the object is null
            if (ReferenceEquals(bar, null)) return 0;

            return bar.StartNode.GetHashCode() ^ bar.EndNode.GetHashCode();
        }


        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private NodeDistanceComparer nodeComparer;

        /***************************************************/

    }
}

