/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static string FromRigidLink(this RigidLink link)
        {
            string midasLink = "";

            string primaryId = link.PrimaryNode.AdapterId<string>(typeof(MidasCivilId));
            string secondaryId = "";

            foreach (Node secondaryNode in link.SecondaryNodes)
            {
                secondaryId = secondaryId + " " + secondaryNode.AdapterId<string>(typeof(MidasCivilId));
            }

            string fixity = BoolToFixity(link.Constraint.XtoX) +
                            BoolToFixity(link.Constraint.YtoY) +
                            BoolToFixity(link.Constraint.ZtoZ) +
                            BoolToFixity(link.Constraint.XXtoXX) +
                            BoolToFixity(link.Constraint.YYtoYY) +
                            BoolToFixity(link.Constraint.ZZtoZZ);

            midasLink = "1, " + primaryId + "," + fixity + "," + secondaryId + "," + link.Name;

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
