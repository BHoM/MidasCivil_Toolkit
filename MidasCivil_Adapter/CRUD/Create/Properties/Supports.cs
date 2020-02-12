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

using System.Collections.Generic;
using System.IO;
using BH.oM.Structure.Constraints;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool CreateCollection(IEnumerable<Constraint6DOF> supports)
        {
            string boundaryGroupPath = CreateSectionFile("BNDR-GROUP");
            string supportPath = CreateSectionFile("CONSTRAINT");
            string springPath = CreateSectionFile("SPRING");

            List<string> midasSprings = new List<string>();
            List<string> midasSupports = new List<string>();

            foreach (Constraint6DOF constraint6DOF in supports)
            {
                string midasBoundaryGroup = Engine.MidasCivil.Convert.ToMCBoundaryGroup(constraint6DOF.Name);
                CompareGroup(midasBoundaryGroup, boundaryGroupPath);
            }

            foreach (Constraint6DOF constraint6DOF in supports)
            {
                if (Engine.MidasCivil.Compute.GetStiffnessVectorModulus(constraint6DOF) > 0)
                {
                    midasSprings.Add(Engine.MidasCivil.Convert.ToMCSpring(constraint6DOF, midasCivilVersion));
                    
                }
                else
                {
                    midasSupports.Add(Engine.MidasCivil.Convert.ToMCSupport(constraint6DOF));
                }
            }

            File.AppendAllLines(supportPath, midasSupports);
            File.AppendAllLines(springPath, midasSprings);

            return true;
        }

    }
}