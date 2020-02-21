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

using System;
using System.Collections.Generic;
using BH.oM.Structure.Loads;
using System.Linq;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static Loadcase ToLoadcase(this string loadcase)
        {
            List<string> delimitted = loadcase.Split(',').ToList();
            LoadNature nature = LoadNature.Dead;
            MidasLoadNatureConverter(delimitted[1].Trim(), ref nature);

            Loadcase bhomLoadCase = new Loadcase
            {
                Name = delimitted[0].Trim(),
                Nature = nature,
                Number = 0,
            };

            bhomLoadCase.CustomData[AdapterIdName] = delimitted[0].Trim();

            return bhomLoadCase;
        }

        public static void MidasLoadNatureConverter(string midasNature, ref LoadNature nature)
        {
            Dictionary<string, LoadNature> converter = new Dictionary<string, LoadNature>
        {
            {"D", LoadNature.Dead},
            {"L", LoadNature.Live},
            {"W", LoadNature.Wind},
            {"T", LoadNature.Temperature},
            {"DC", LoadNature.SuperDead},
            {"DW", LoadNature.SuperDead},
            {"PL", LoadNature.SuperDead},
            {"BL", LoadNature.SuperDead},
            {"PS", LoadNature.Prestress},
            {"S", LoadNature.Snow}
        };

            converter.TryGetValue(midasNature, out nature);
        }
    }

}

