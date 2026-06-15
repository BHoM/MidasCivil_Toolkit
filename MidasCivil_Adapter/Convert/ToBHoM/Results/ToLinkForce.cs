/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2026, the respective contributors. All rights reserved.
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

using BH.oM.Structure.Results;
using System.Collections.Generic;

namespace BH.Adapter.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static LinkForce ToLinkForce(List<object> item)
        {
            //TODO: resolve below identifiers extractable through the API
            int mode = -1;

            LinkForce linkForce = new LinkForce(
                System.Convert.ToInt32(item[1].ToString()),     
                item[4].ToString(),                             
                mode,
                System.Convert.ToDouble(item[5].ToString()),    
                System.Convert.ToDouble(item[6].ToString()),    
                System.Convert.ToDouble(item[7].ToString()),    
                System.Convert.ToDouble(item[8].ToString()),    
                System.Convert.ToDouble(item[9].ToString()),    
                - System.Convert.ToDouble(item[10].ToString()),   //Moments reversed to follow structural convension
                - System.Convert.ToDouble(item[11].ToString())    
                );
            return linkForce;
        }

    }
}







