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

using BH.oM.Structure.Loads;
using BH.oM.Structure.Results;
using System.Linq;
using System.Collections.Generic;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static NodeDisplacement ToNodeDisplacement(this List<string> delimitted, string lengthUnit)
        {
            //TODO: resolve below identifiers extractable through the API
            int mode = -1;
            double timeStep = 0;

            NodeDisplacement nodeDisplacement = new NodeDisplacement(
                System.Convert.ToInt32(delimitted[2]),
                delimitted[3],
                mode,
                timeStep,
                oM.Geometry.Basis.XY,
                System.Convert.ToDouble(delimitted[7]).LengthToSI(lengthUnit),
                System.Convert.ToDouble(delimitted[8]).LengthToSI(lengthUnit),
                System.Convert.ToDouble(delimitted[9]).LengthToSI(lengthUnit),
                System.Convert.ToDouble(delimitted[10]),
                System.Convert.ToDouble(delimitted[11]),
                System.Convert.ToDouble(delimitted[12])
                );

            return nodeDisplacement;
        }

        /***************************************************/

    }
}




