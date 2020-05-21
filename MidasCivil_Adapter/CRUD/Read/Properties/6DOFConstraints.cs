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
using BH.oM.Structure.Constraints;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<Constraint6DOF> Read6DOFConstraints(List<string> ids = null)
        {
            List<Constraint6DOF> bhom6DOFConstraints = new List<Constraint6DOF>();

            List<string> supports = GetSectionText("CONSTRAINT");

            for (int i = 0; i < supports.Count; i++)
            {
                Constraint6DOF bhomConstraint6DOF = Adapters.MidasCivil.Convert.ToConstraint6DOF(
                    supports[i], midasCivilVersion);

                bhom6DOFConstraints.Add(bhomConstraint6DOF);
            }

            List<string> springs = GetSectionText("SPRING");

            for (int i = 0; i < springs.Count; i++)
            {
                Constraint6DOF bhomConstraint6DOF = Adapters.MidasCivil.Convert.ToConstraint6DOF(
                    springs[i], midasCivilVersion);
                if (!(bhomConstraint6DOF == null))
                {
                    bhom6DOFConstraints.Add(bhomConstraint6DOF);
                }
            }

            return bhom6DOFConstraints;
        }

        /***************************************************/

    }
}
