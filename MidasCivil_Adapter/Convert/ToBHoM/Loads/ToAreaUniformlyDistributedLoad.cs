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
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;
using BH.oM.Geometry;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        public static AreaUniformlyDistributedLoad ToAreaUniformlyDistributedLoad(string areaUniformlyDistributedLoad, List<string> associatedFEMeshes,
            string loadcase, Dictionary<string, Loadcase> loadcaseDictionary, Dictionary<string, FEMesh> femeshDictionary, int count, string forceUnit, string lengthUnit)
        {
            string[] delimitted = areaUniformlyDistributedLoad.Split(',');
            List<FEMesh> bhomAssociatedFEMeshes = new List<FEMesh>();

            Loadcase bhomLoadcase;
            loadcaseDictionary.TryGetValue(loadcase, out bhomLoadcase);

            foreach (string associatedFEMesh in associatedFEMeshes)
            {
                FEMesh bhomAssociatedFEMesh;
                femeshDictionary.TryGetValue(associatedFEMesh, out bhomAssociatedFEMesh);
                bhomAssociatedFEMeshes.Add(bhomAssociatedFEMesh);
            }

            string loadAxis = delimitted[3].Trim().Substring(0, 1);
            string direction = delimitted[3].Trim().Substring(1, 1);
            string projection = delimitted[7].Trim();

            LoadAxis axis = LoadAxis.Global;
            bool loadProjection = false;

            if (loadAxis == "L")
            {
                axis = LoadAxis.Local;
            }

            if (projection == "YES")
            {
                loadProjection = true;
            }

            double XLoad = 0;
            double YLoad = 0;
            double ZLoad = 0;
            double force = double.Parse(delimitted[8].Trim()).PressureToSI(forceUnit, lengthUnit);

            switch (direction)
            {
                case "X":
                    XLoad = force;
                    break;
                case "Y":
                    YLoad = force;
                    break;
                case "Z":
                    ZLoad = force;
                    break;
            }

            Vector loadVector = new Vector
            {
                X = XLoad,
                Y = YLoad,
                Z = ZLoad
            };

            string name;

            if (string.IsNullOrWhiteSpace(delimitted[13]))
            {
                name = "AL" + count;
            }
            else
            {
                name = delimitted[13].Trim();
            }

            AreaUniformlyDistributedLoad bhomAreaUniformlyDistributedLoad = Engine.Structure.Create.AreaUniformlyDistributedLoad(bhomLoadcase, loadVector,
                bhomAssociatedFEMeshes, axis, loadProjection, name);
            bhomAreaUniformlyDistributedLoad.CustomData[AdapterIdName] = bhomAreaUniformlyDistributedLoad.Name;

            return bhomAreaUniformlyDistributedLoad;
        }

    }
}

