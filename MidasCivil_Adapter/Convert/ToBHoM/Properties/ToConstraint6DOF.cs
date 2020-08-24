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

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        public static Constraint6DOF ToConstraint6DOF(this string support, string version, string forceUnit, string lengthUnit)
        {
            List<string> delimitted = support.Split(',').ToList();
            string supportName;
            List<bool> fixity = new List<bool>();
            List<double> stiffness = new List<double>();

            int constraint = 0;
            string assignment = delimitted[1].Replace(" ", string.Empty);

            if (int.TryParse(assignment, out constraint))
            {
                foreach (char freedom in assignment)
                {
                    int freedoms = int.Parse(freedom.ToString());
                    if (freedoms == 1)
                    {
                        fixity.Add(true);
                        stiffness.Add(0.0);
                    }
                    else
                    {
                        fixity.Add(false);
                        stiffness.Add(0.0);
                    }
                }
                supportName = delimitted[2].Trim();
            }
            else
            {
                if (!(delimitted[1].Trim() == "LINEAR"))
                {
                    Engine.Reflection.Compute.RecordWarning(
                        "MidasCivil_Toolkit does not support tension/compression only springs or multi-linear springs");
                    return null;
                }
                else
                {
                    switch (version)
                    {
                        case "8.8.5":
                            for (int i = 2; i < 8; i++)
                            {
                                if (delimitted[i].Trim() == "YES")
                                {
                                    fixity.Add(true);
                                    stiffness.Add(0);
                                }
                                else if (delimitted[i].Trim() == "NO")
                                {
                                    double spring;
                                    if (i < 5)
                                    {
                                        spring = double.Parse(delimitted[i + 6]).ForcePerLengthToSI(forceUnit, lengthUnit);
                                    }
                                    else
                                    {
                                        spring = double.Parse(delimitted[i + 6]).MomentToSI(forceUnit, lengthUnit);
                                    }
                                    if (spring > 1E+017.ForcePerLengthToSI(forceUnit, lengthUnit) || spring > 1E+19.MomentToSI(forceUnit, lengthUnit))
                                    {
                                        fixity.Add(true);
                                        stiffness.Add(0);
                                    }
                                    else
                                    {
                                        fixity.Add(false);
                                        stiffness.Add(spring);
                                    }
                                }
                            }
                            supportName = delimitted[21].Trim();
                            break;

                        default:
                            for (int i = 2; i < 8; i++)
                            {
                                if (delimitted[i] == "")
                                {
                                    fixity.Add(false);
                                    stiffness.Add(0);
                                }
                                else
                                {
                                    double spring;
                                    if (i < 5)
                                    {
                                        spring = double.Parse(delimitted[i]).ForcePerLengthToSI(forceUnit, lengthUnit);
                                    }
                                    else
                                    {
                                        spring = double.Parse(delimitted[i]).MomentToSI(forceUnit, lengthUnit);
                                    }
                                    if (spring > 1E+017.ForcePerLengthFromSI(forceUnit, lengthUnit) || spring > 1E+19.MomentFromSI(forceUnit, lengthUnit))
                                    {
                                        fixity.Add(true);
                                        stiffness.Add(0);
                                    }
                                    else
                                    {
                                        fixity.Add(false);
                                        stiffness.Add(spring);
                                    }
                                }
                            }
                            supportName = delimitted[15].Trim();
                            break;
                    }

                }
            }

            Constraint6DOF bhomConstraint6DOF = Engine.Structure.Create.Constraint6DOF(supportName, fixity, stiffness);
            bhomConstraint6DOF.CustomData[AdapterIdName] = supportName;

            return bhomConstraint6DOF;

        }
    }
}