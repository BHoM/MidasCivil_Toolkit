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

using System.Collections.Generic;
using BH.oM.Adapters.MidasCivil;
using BH.Engine.Adapter;
using BH.oM.Structure.Loads;
using BH.oM.Geometry;
using BH.oM.Base;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static GravityLoad ToGravityLoad(List<BHoMObject> objects, string gravityLoad, string loadcase,
            Dictionary<string, Loadcase> loadcaseDictionary, int count)
        {
            string[] delimitted = gravityLoad.Split(',');

            Loadcase bhomLoadcase;
            loadcaseDictionary.TryGetValue(loadcase, out bhomLoadcase);

            Vector direction = new Vector
            {
                X = double.Parse(delimitted[0].Trim()),
                Y = double.Parse(delimitted[1].Trim()),
                Z = double.Parse(delimitted[2].Trim())
            };
            string name;

            if (string.IsNullOrWhiteSpace(delimitted[3]))
            {
                name = "GL" + count;
            }
            else
            {
                name = delimitted[3].Trim();
            }

            GravityLoad bhomGravityLoad = Engine.Structure.Create.GravityLoad(bhomLoadcase, direction, objects, name);
            bhomGravityLoad.SetAdapterId(typeof(MidasCivilId), bhomGravityLoad.Name);

            return bhomGravityLoad;
        }

        /***************************************************/

    }
}



