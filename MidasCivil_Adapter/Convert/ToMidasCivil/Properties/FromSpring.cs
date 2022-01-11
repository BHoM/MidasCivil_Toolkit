/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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

using BH.Adapter.MidasCivil;
using BH.oM.Structure.Constraints;
using BH.Engine.Structure;

using System.Collections.Generic;
using System.Linq;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static string FromSpring(this Constraint6DOF constraint6DOF, string version, string forceUnit, string lengthUnit, int groupCharacterLimit)
        {
            List<double> stiffness = SpringStiffness(constraint6DOF, forceUnit, lengthUnit);

            string midasSpring = "";

            switch (version)
            {
                case "8.9.5":
                case "8.9.0":
                case "8.8.5":
                    string springFixity = SpringFixity(constraint6DOF, groupCharacterLimit);
                    midasSpring = (
                        " " + "," + "LINEAR" + "," + springFixity +
                        stiffness[0].ForcePerLengthFromSI(forceUnit, lengthUnit) + "," + stiffness[1].ForcePerLengthFromSI(forceUnit, lengthUnit) + "," +
                        stiffness[2].ForcePerLengthFromSI(forceUnit, lengthUnit) + "," + stiffness[3].MomentFromSI(forceUnit, lengthUnit) + "," +
                        stiffness[4].MomentFromSI(forceUnit, lengthUnit) + "," + stiffness[5].MomentFromSI(forceUnit, lengthUnit)
                        + "," + "NO, 0, 0, 0, 0, 0, 0," + new string(constraint6DOF.DescriptionOrName().Replace(",","").Take(groupCharacterLimit).ToArray()) + "," + "0, 0, 0, 0, 0"
                        );
                    break;
                default:
                    midasSpring = (
                        " " + "," + "LINEAR" + "," +
                        stiffness[0].ForcePerLengthFromSI(forceUnit, lengthUnit) + "," + stiffness[1].ForcePerLengthFromSI(forceUnit, lengthUnit) + "," +
                        stiffness[2].ForcePerLengthFromSI(forceUnit, lengthUnit) + "," + stiffness[3].MomentFromSI(forceUnit, lengthUnit) + "," +
                        stiffness[4].MomentFromSI(forceUnit, lengthUnit) + "," + stiffness[5].MomentFromSI(forceUnit, lengthUnit)
                        + "," + "NO, 0, 0, 0, 0, 0, 0," + new string(constraint6DOF.DescriptionOrName().Replace(",","").Take(groupCharacterLimit).ToArray()) + "," + "0, 0, 0, 0, 0"
                        );
                    break;
            }

            return midasSpring;
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static List<double> SpringStiffness(Constraint6DOF constraint6DOF, string forceUnit, string lengthUnit)
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

            for (int i = 0; i < fixities.Count; i++)
            {
                DOFType freedom = fixities[i];

                if (freedom == DOFType.Fixed)
                {
                    if (i < 3)
                    {
                        stiffness.Add(ForcePerLengthFromSI(1E17,forceUnit, lengthUnit));
                        if (!(springs[i] == ForcePerLengthFromSI(1E17, forceUnit, lengthUnit)))
                            Engine.Base.Compute.RecordWarning(
                                DOFType.Fixed + " used, this will overwrite the spring stiffness with 1E+17 N/m");
                    }
                    else
                    {
                        stiffness.Add(MomentFromSI(1E19, forceUnit, lengthUnit));
                        if (!(springs[i] == MomentFromSI(1E19, forceUnit, lengthUnit)))
                            Engine.Base.Compute.RecordWarning(
                                DOFType.Fixed + " used, this will overwrite the spring stiffness with 1E+019 Nm/rad");
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

        /***************************************************/

        private static string SpringFixity(Constraint6DOF constraint6DOF, int groupCharacterLimit)
        {
            List<DOFType> freedoms = new List<DOFType>
            {
                constraint6DOF.TranslationX, constraint6DOF.TranslationY, constraint6DOF.TranslationZ,
                constraint6DOF.RotationX, constraint6DOF.RotationY, constraint6DOF.RotationZ
            };

            string support = "";

            foreach (DOFType freedom in freedoms)
            {
                if (!(MidasCivilAdapter.GetSupportedDOFType(freedom)))
                {
                    Engine.Base.Compute.RecordWarning(
                        "Unsupported DOFType in " + new string(constraint6DOF.DescriptionOrName().Replace(",","").Take(groupCharacterLimit).ToArray()) + " assumed to be" + DOFType.Fixed);
                    support = support + "YES,";
                }

                else if (freedom == DOFType.Fixed)
                {
                    support = support + "YES,";
                }
                else
                {
                    support = support + "NO,";
                }
            }

            return support;
        }

        /***************************************************/

    }
}

