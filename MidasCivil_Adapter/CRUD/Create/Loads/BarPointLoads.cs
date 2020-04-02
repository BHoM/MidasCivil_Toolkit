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
        public bool CreateCollection(IEnumerable<BarPointLoad> barPointLoads)
        {
            string loadGroupPath = CreateSectionFile("LOAD-GROUP");

            foreach (BarPointLoad barPointLoad in barPointLoads)
            {
                List<string> midasBarLoads = new List<string>();
                string barLoadPath = CreateSectionFile(barPointLoad.Loadcase.Name + "\\BEAMLOAD");
                string midasLoadGroup = Engine.External.MidasCivil.Convert.FromLoadGroup(barPointLoad);

                List<string> assignedBars = barPointLoad.Objects.Elements.Select(x => x.CustomData[AdapterIdName].ToString()).ToList();

                List<double> loadVectors = new List<double> { barPointLoad.Force.X,
                                                              barPointLoad.Force.Y,
                                                              barPointLoad.Force.Z,
                                                              barPointLoad.Moment.X,
                                                              barPointLoad.Moment.Y,
                                                              barPointLoad.Moment.Z};

                Vector zeroVector = new Vector { X = 0, Y = 0, Z = 0 };

                for (int i = 0; i < 6; i++)
                {
                    barPointLoad.Force = zeroVector;
                    barPointLoad.Moment = zeroVector;

                    if (loadVectors[i] != 0)
                    {
                        if (i < 3)
                        {
                            barPointLoad.Force = CreateSingleComponentVector(i, loadVectors[i]);

                            foreach (string assignedBar in assignedBars)
                            {
                                midasBarLoads.Add(Engine.External.MidasCivil.Convert.FromBarPointLoad(barPointLoad, assignedBar, "Force"));
                            }
                        }
                        else
                        {
                            barPointLoad.Moment = CreateSingleComponentVector(i - 3, loadVectors[i]);

                            foreach (string assignedBar in assignedBars)
                            {
                                midasBarLoads.Add(Engine.External.MidasCivil.Convert.FromBarPointLoad(barPointLoad, assignedBar, "Moment"));
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

    }
}
