/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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
    public class ArrayComparer : IEqualityComparer<double[]>
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public bool Equals(double[] x, double[] y)
        {
            if (x.Length != y.Length)
                return false;

            for (int i = 0; i < x.Length; ++i)
                if (!x[i].Equals(y[i]))
                    return false;

            return true;
        }

        /***************************************************/

        public int GetHashCode(double[] obj)
        {
            return string.Join("", obj).GetHashCode();
        }

        /***************************************************/

    }
}


