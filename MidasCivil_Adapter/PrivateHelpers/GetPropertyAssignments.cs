/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private Dictionary<string, List<int>> GetPropertyAssignments(string section, string namePrefix)
        {
            List<string> sectionText = GetSectionText(section);

            Dictionary<string, List<int>> propertyAssignments = new Dictionary<string, List<int>>();

            for (int i = 0; i < sectionText.Count(); i++)
            {
                List<string> splitSection = sectionText[i].Split(',').ToList();
                string geometryList = splitSection[0];

                List<string> geometryAssignments = new List<string>();

                if (geometryList.Contains(" "))
                    geometryAssignments = geometryList.Split(' ').
                        Select(x => x.Trim()).
                        Where(x => !string.IsNullOrEmpty(x)).
                        ToList();
                else
                    geometryAssignments.Add(geometryList);

                List<int> propertyAssignment = MidasCivilAdapter.GetAssignmentIds(geometryAssignments);


                switch (section)
                {
                    case "CONSTRAINT":
                        if (splitSection[2] == "")
                            propertyAssignments.Add(splitSection[1], propertyAssignment);
                        else
                            propertyAssignments.Add(splitSection[2], propertyAssignment);

                        break;
                    case "SPRING":
                        switch (m_midasCivilVersion)
                        {
                            case "8.8.1":
                            case "8.7.5":
                            case "8.7.0":
                            case "8.6.5":
                                if (splitSection[15] == "")
                                {
                                    string name = "Fx=" + splitSection[2] + "Fy=" + splitSection[3] + "Fz=" + splitSection[4] + "Rx=" + splitSection[5] +
                                       "Ry=" + splitSection[6] + "Rz=" + splitSection[7];
                                    propertyAssignments.Add(name, propertyAssignment);
                                }
                                else
                                    propertyAssignments.Add(splitSection[15], propertyAssignment);
                                break;
                            default:
                                if (splitSection[21] == "")
                                {
                                    string name = "Fx=" + splitSection[8] + "Fy=" + splitSection[9] + "Fz=" + splitSection[10] + "Rx=" + splitSection[11] +
                                        "Ry=" + splitSection[12] + "Rz=" + splitSection[13];
                                    propertyAssignments.Add(name, propertyAssignment);
                                }

                                else
                                    propertyAssignments.Add(splitSection[21], propertyAssignment);
                                break;
                        }
                        break;
                }
            }

            return propertyAssignments;
        }

        /***************************************************/

    }
}





