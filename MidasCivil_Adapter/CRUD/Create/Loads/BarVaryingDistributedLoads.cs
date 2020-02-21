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

using BH.oM.Structure.Loads;
using BH.oM.Geometry;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public bool CreateCollection(IEnumerable<BarVaryingDistributedLoad> barVaryingDistributedLoads)
        {
            string loadGroupPath = CreateSectionFile("LOAD-GROUP");

            foreach (BarVaryingDistributedLoad barVaryingDistributedLoad in barVaryingDistributedLoads)
            {
                List<string> midasBarLoads = new List<string>();
                string barLoadPath = CreateSectionFile(barVaryingDistributedLoad.Loadcase.Name + "\\BEAMLOAD");
                string midasLoadGroup = Engine.MidasCivil.Convert.ToMCLoadGroup(barVaryingDistributedLoad);

                List<string> assignedBars = barVaryingDistributedLoad.Objects.Elements.Select(x => x.CustomData[AdapterIdName].ToString()).ToList();

                List<double> startLoadVectors = new List<double> { barVaryingDistributedLoad.ForceA.X,
                                                              barVaryingDistributedLoad.ForceA.Y,
                                                              barVaryingDistributedLoad.ForceA.Z,
                                                              barVaryingDistributedLoad.MomentA.X,
                                                              barVaryingDistributedLoad.MomentA.Y,
                                                              barVaryingDistributedLoad.MomentA.Z};

                List<double> endLoadVectors = new List<double> { barVaryingDistributedLoad.ForceB.X,
                                                              barVaryingDistributedLoad.ForceB.Y,
                                                              barVaryingDistributedLoad.ForceB.Z,
                                                              barVaryingDistributedLoad.MomentB.X,
                                                              barVaryingDistributedLoad.MomentB.Y,
                                                              barVaryingDistributedLoad.MomentB.Z};

                Vector zeroVector = new Vector { X = 0, Y = 0, Z = 0 };

                for (int i = 0; i < 6; i++)
                {
                    barVaryingDistributedLoad.ForceA = zeroVector;
                    barVaryingDistributedLoad.MomentA = zeroVector;
                    barVaryingDistributedLoad.ForceB = zeroVector;
                    barVaryingDistributedLoad.MomentB = zeroVector;

                    if (!(startLoadVectors[i] == 0 && endLoadVectors[i] == 0))
                    {
                        if (i < 3)
                        {
                            barVaryingDistributedLoad.ForceA = CreateSingleComponentVector(i, startLoadVectors[i]);
                            barVaryingDistributedLoad.ForceB = CreateSingleComponentVector(i, endLoadVectors[i]);

                            foreach (string assignedBar in assignedBars)
                            {
                                midasBarLoads.Add(Engine.MidasCivil.Convert.ToMCBarVaryingDistributedLoad(barVaryingDistributedLoad, assignedBar, "Force"));
                            }
                        }
                        else
                        {
                            barVaryingDistributedLoad.MomentA = CreateSingleComponentVector(i-3, startLoadVectors[i]);
                            barVaryingDistributedLoad.MomentB = CreateSingleComponentVector(i-3, endLoadVectors[i]);

                            foreach (string assignedBar in assignedBars)
                            {
                                midasBarLoads.Add(Engine.MidasCivil.Convert.ToMCBarVaryingDistributedLoad(barVaryingDistributedLoad, assignedBar, "Moment"));
                            }
                        }

                    }
                }

                CompareLoadGroup(midasLoadGroup, loadGroupPath);
                RemoveLoadEnd(barLoadPath);
                File.AppendAllLines(barLoadPath, midasBarLoads);
            }
            return true;
        }

        public static Vector CreateSingleComponentVector(int index, double value)
        {
            Vector vector = new Vector { X=0, Y=0, Z=0 };

            switch (index)
            {
                case (0):
                    vector.X = value;
                    break;
                case (1):
                    vector.Y = value;
                    break;
                case (2):
                    vector.Z = value;
                    break;
            }

            return vector;
        }

    }
}
