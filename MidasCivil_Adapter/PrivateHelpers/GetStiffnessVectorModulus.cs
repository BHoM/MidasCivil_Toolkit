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

using BH.oM.Geometry;
using BH.oM.Structure.Constraints;
using System;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static double GetStiffnessVectorModulus(Constraint6DOF support)
        {
            Vector translationalStiffnessVector = new Vector()
            {
                X = support.TranslationalStiffnessX,
                Y = support.TranslationalStiffnessY,
                Z = support.TranslationalStiffnessZ
            };

            Vector rotationalStiffnessVector = new Vector()
            {
                X = support.RotationalStiffnessX,
                Y = support.RotationalStiffnessY,
                Z = support.RotationalStiffnessZ
            };

            return Modulus(translationalStiffnessVector) +
                Modulus(rotationalStiffnessVector);
        }

        /***************************************************/

        private static double Modulus(Vector vector)
        {
            return
                Math.Sqrt(Math.Pow(vector.X, 2) +
                Math.Pow(vector.Y, 2) +
                Math.Pow(vector.Z, 2));
        }

        /***************************************************/

    }
}




