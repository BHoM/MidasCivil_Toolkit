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

using BH.oM.Structure.Constraints;
using System.Collections.Generic;
using System.Linq;

namespace BH.Engine.External.MidasCivil
{
    public static partial class Query
    {
        public static List<double> SpringStiffness(Constraint6DOF constraint6DOF)
        {
            List<double> stiffness = new List<double>();

            List<DOFType> fixities = new List<DOFType>() {
                constraint6DOF.TranslationX, constraint6DOF.TranslationY, constraint6DOF.TranslationZ,
                constraint6DOF.RotationX, constraint6DOF.RotationY, constraint6DOF.RotationZ
            };

            List<double> springs = new List<double>() {
                constraint6DOF.TranslationalStiffnessX, constraint6DOF.TranslationalStiffnessY, constraint6DOF.TranslationalStiffnessZ,
                constraint6DOF.RotationalStiffnessX,constraint6DOF.RotationalStiffnessY,constraint6DOF.RotationalStiffnessZ
            };

            for (int i = 0; i < fixities.Count(); i++)
            {
                DOFType freedom = fixities[i];

                if (freedom == DOFType.Fixed)
                {
                    if (i < 3)
                    {
                        stiffness.Add(100000);
                        if(!(springs[i]==100000))
                        Reflection.Compute.RecordWarning(
                            DOFType.Fixed + " used, this will overwrite the spring stiffness with 100,000 kN/m");
                    }
                    else
                    {
                        stiffness.Add(1E+016);
                        if (!(springs[i] == 1E+016))
                            Reflection.Compute.RecordWarning(
                                DOFType.Fixed + " used, this will overwrite the spring stiffness with 1E+016 kNm/rad");
                    }
                }
                else if (freedom == DOFType.Free)
                {
                    stiffness.Add(0);
                }
                else
                {
                    stiffness.Add(springs[i]);
                }
            }

            return stiffness;
        }
    }
}