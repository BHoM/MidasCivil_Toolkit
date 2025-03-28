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
using BH.oM.Structure.Loads;
using BH.oM.Geometry;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using BH.Engine.Base;
using BH.Engine.Spatial;
using BH.Engine.Geometry;
using BH.Adapter.Adapters.MidasCivil;
using System.Data;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private bool CreateCollection(IEnumerable<BarVaryingDistributedLoad> barVaryingDistributedLoads)
        {
            string loadGroupPath = CreateSectionFile("LOAD-GROUP");

            foreach (BarVaryingDistributedLoad barVaryingDistributedLoad in barVaryingDistributedLoads)
            {
                BarVaryingDistributedLoad load = barVaryingDistributedLoad.ShallowClone();

                if (load.StartPosition >= load.EndPosition)
                {
                    Engine.Base.Compute.RecordError("MidasCivil only supports start positions less than end positions for BarVaryingDistributedLoads.");
                    continue;
                }

                List<double> startPosition = new List<double>();
                List<double> endPosition = new List<double>();


                foreach (Bar bar in load.Objects.Elements)
                {
                    if (load.RelativePositions == false)
                    {
                        double length = bar.Length();

                        startPosition.Add(barVaryingDistributedLoad.StartPosition / length);
                        endPosition.Add(barVaryingDistributedLoad.EndPosition / length);

                        if (load.StartPosition / length > 1 || load.EndPosition / length > 1)
                            Engine.Base.Compute.RecordError("The load start or end position is outside one or more bars");
                    }
                    else
                    {
                        startPosition.Add(barVaryingDistributedLoad.StartPosition);
                        endPosition.Add(barVaryingDistributedLoad.EndPosition);
                    }
                }


                List<string> midasBarLoads = new List<string>();
                string barLoadPath = CreateSectionFile(load.Loadcase.Name + "\\BEAMLOAD");
                string midasLoadGroup = Adapters.MidasCivil.Convert.FromLoadGroup(load);

                List<string> assignedBars = load.Objects.Elements.Select(x => x.AdapterId<string>(typeof(MidasCivilId))).ToList();


                List<double> startLoadVectors = new List<double> { load.ForceAtStart.X,
                                                              load.ForceAtStart.Y,
                                                              load.ForceAtStart.Z,
                                                              load.MomentAtStart.X,
                                                              load.MomentAtStart.Y,
                                                              load.MomentAtStart.Z};

                List<double> endLoadVectors = new List<double> { load.ForceAtEnd.X,
                                                              load.ForceAtEnd.Y,
                                                              load.ForceAtEnd.Z,
                                                              load.MomentAtEnd.X,
                                                              load.MomentAtEnd.Y,
                                                              load.MomentAtEnd.Z};

                Vector zeroVector = new Vector { X = 0, Y = 0, Z = 0 };

                for (int i = 0; i < 6; i++)
                {
                    load.ForceAtStart = zeroVector;
                    load.MomentAtStart = zeroVector;
                    load.ForceAtEnd = zeroVector;
                    load.MomentAtEnd = zeroVector;

                    if (!(startLoadVectors[i] == 0 && endLoadVectors[i] == 0))
                    {
                        if (i < 3)
                        {
                            load.ForceAtStart = CreateSingleComponentVector(i, startLoadVectors[i]);
                            load.ForceAtEnd = CreateSingleComponentVector(i, endLoadVectors[i]);

                            for (int j = 0; j < assignedBars.Count; j++)
                            {
                                midasBarLoads.Add(Adapters.MidasCivil.Convert.FromBarVaryingDistributedLoad(load,
                                    assignedBars[j], "Force", m_forceUnit, m_lengthUnit, startPosition[j], endPosition[j]));
                            }
                        }
                        else
                        {
                            load.MomentAtStart = CreateSingleComponentVector(i - 3, startLoadVectors[i]);
                            load.MomentAtEnd = CreateSingleComponentVector(i - 3, endLoadVectors[i]);

                            for (int j = 0; j < assignedBars.Count; j++)
                            {
                                midasBarLoads.Add(Adapters.MidasCivil.Convert.FromBarVaryingDistributedLoad(load,
                                    assignedBars[j], "Moment", m_forceUnit, m_lengthUnit, startPosition[j], endPosition[j]));
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

        private static Vector CreateSingleComponentVector(int index, double value)
        {
            Vector vector = new Vector { X = 0, Y = 0, Z = 0 };

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

        /***************************************************/

    }
}





