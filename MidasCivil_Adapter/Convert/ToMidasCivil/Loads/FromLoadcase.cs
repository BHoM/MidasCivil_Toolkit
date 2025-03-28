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

using BH.oM.Structure.Loads;
using System.Collections.Generic;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static string FromLoadcase(this Loadcase loadcase)
        {
            LoadNature bhomNature = loadcase.Nature;
            string midasNature = "D";
            BhomLoadNatureConverter(bhomNature, ref midasNature);

            string midasLoadcase = loadcase.Name + " " + "," + midasNature + ",";

            return midasLoadcase;
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static void BhomLoadNatureConverter(LoadNature bhomNature, ref string nature)
        {
            Dictionary<LoadNature, string> converter = new Dictionary<LoadNature, string>
        {
            {LoadNature.Dead,"D"},
            {LoadNature.Live,"L"},
            {LoadNature.Wind,"W"},
            {LoadNature.Temperature,"T"},
            {LoadNature.SuperDead,"DC"},
            {LoadNature.Prestress,"PS"},
            {LoadNature.Snow,"S"},
            {LoadNature.Seismic,"E"},
            {LoadNature.Accidental,"CO"},
            {LoadNature.Notional,"USER"},
            {LoadNature.Other,"USER"}
        };
            converter.TryGetValue(bhomNature, out nature);
        }

        /***************************************************/

    }
}




