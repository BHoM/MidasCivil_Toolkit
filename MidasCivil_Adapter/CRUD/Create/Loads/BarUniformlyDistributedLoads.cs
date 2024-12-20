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
using BH.oM.Structure.Loads;
using BH.oM.Geometry;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using BH.Engine.Base;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private bool CreateCollection(IEnumerable<BarUniformlyDistributedLoad> barUniformlyDistributedLoads)
        {
            string loadGroupPath = CreateSectionFile("LOAD-GROUP");

            foreach (BarUniformlyDistributedLoad barUniformlyDistributedLoad in barUniformlyDistributedLoads)
            {
                BarUniformlyDistributedLoad load = barUniformlyDistributedLoad.ShallowClone();
                List<string> midasBarLoads = new List<string>();
                string barLoadPath = CreateSectionFile(load.Loadcase.Name + "\\BEAMLOAD");
                string midasLoadGroup = Adapters.MidasCivil.Convert.FromLoadGroup(barUniformlyDistributedLoad);

                List<string> assignedBars = load.Objects.Elements.Select(x => x.AdapterId<string>(typeof(MidasCivilId))).ToList();

                List<double> loadVectors = new List<double> { load.Force.X,
                                                              load.Force.Y,
                                                              load.Force.Z,
                                                              load.Moment.X,
                                                              load.Moment.Y,
                                                              load.Moment.Z};

                Vector zeroVector = new Vector { X = 0, Y = 0, Z = 0 };

                for (int i = 0; i < 6; i++)
                {
                    load.Force = zeroVector;
                    load.Moment = zeroVector;

                    if (loadVectors[i] != 0)
                    {
                        if (i < 3)
                        {
                            load.Force = CreateSingleComponentVector(i, loadVectors[i]);

                            foreach (string assignedBar in assignedBars)
                            {
                                midasBarLoads.Add(Adapters.MidasCivil.Convert.FromBarUniformlyDistributedLoad(load, assignedBar, "Force", m_forceUnit, m_lengthUnit));
                            }
                        }
                        else
                        {
                            load.Moment = CreateSingleComponentVector(i - 3, loadVectors[i]);

                            foreach (string assignedBar in assignedBars)
                            {
                                midasBarLoads.Add(Adapters.MidasCivil.Convert.FromBarUniformlyDistributedLoad(load, assignedBar, "Moment", m_forceUnit, m_lengthUnit));
                            }
                        }

                    }
                }

                CompareLoadGroup(midasLoadGroup, loadGroupPath);
                RemoveEndOfDataString(barLoadPath);
                File.AppendAllLines(barLoadPath, midasBarLoads);
            }
            return true;
        }

        /***************************************************/

    }
}





