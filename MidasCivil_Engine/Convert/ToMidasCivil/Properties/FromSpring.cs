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

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string FromSpring(this Constraint6DOF constraint6DOF, string version)
        {
            List<double> stiffness = Engine.MidasCivil.Query.SpringStiffness(constraint6DOF);

            string midasSpring = "";

            switch(version)
            {
                case "8.8.5":
                    string springFixity = Engine.MidasCivil.Query.SpringFixity(constraint6DOF);
                    midasSpring = (
                        " " + "," + "LINEAR" + "," + springFixity +
                        stiffness[0] + "," + stiffness[1] + "," + stiffness[2] + "," +
                        stiffness[3] + "," + stiffness[4] + "," + stiffness[5] + "," +
                        "NO, 0, 0, 0, 0, 0, 0," +
                        constraint6DOF.Name + "," +
                        "0, 0, 0, 0, 0"
                        );
                    break;
                default:
                    midasSpring = (
                        " " + "," + "LINEAR" + "," +
                        stiffness[0] + "," + stiffness[1] + "," + stiffness[2] + "," +
                        stiffness[3] + "," + stiffness[4] + "," + stiffness[5] + "," +
                        "NO, 0, 0, 0, 0, 0, 0," +
                        constraint6DOF.Name + "," +
                        "0, 0, 0, 0, 0"
                        );
                    break;
            }

           return midasSpring;
        }
    }
}