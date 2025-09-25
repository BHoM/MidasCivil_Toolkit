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

using BH.oM.Adapters.MidasCivil;
using BH.Engine.Adapter;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Constraints;
using System;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static string FromRigidLink(this RigidLink link, string version)
        {
            LinkConstraint con = link.Constraint;

            if (con.XtoYY || con.XtoZZ || con.YtoXX || con.YtoZZ || con.ZtoXX || con.ZtoYY)
                Engine.Base.Compute.RecordError("Imposed rotations due to translations are not supported in this Adapter.");

            string midasLink = "";

            string primaryId = link.PrimaryNode.AdapterId<string>(typeof(MidasCivilId));
            string secondaryId = "";

            foreach (Node secondaryNode in link.SecondaryNodes)
            {
                secondaryId = secondaryId + " " + secondaryNode.AdapterId<string>(typeof(MidasCivilId));
            }

            string fixity = BoolToFixity(con.XtoX) +
                            BoolToFixity(con.YtoY) +
                            BoolToFixity(con.ZtoZ) +
                            BoolToFixity(con.XXtoXX) +
                            BoolToFixity(con.YYtoYY) +
                            BoolToFixity(con.ZZtoZZ);

            switch (version)
            {
                case "9.0.5":
                case "9.1.0":
                case "9.4.0":
                case "9.4.5":
                case "9.5.0":
                case "9.5.0.nx":
                case "9.5.5.nx":
                    midasLink = primaryId + "," + fixity + "," + secondaryId + "," + link.Name;
                    break;
                default:
                    midasLink = "1, " + primaryId + "," + fixity + "," + secondaryId + "," + link.Name;
                    break;
            }

            return midasLink;
        }

        private static string BoolToFixity(bool fixity)
        {
            string converted = "0";

            if (fixity)
            {
                converted = "1";
            }

            return converted;
        }

        /***************************************************/

    }
}




