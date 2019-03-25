using BH.oM.Structure.Loads;
using BH.oM.Geometry;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
                List<string> midasLoadGroup = new List<string>();
                midasLoadGroup.Add(Engine.MidasCivil.Convert.ToMCLoadGroup(barVaryingDistributedLoad));

                List<string> assignedBars = barVaryingDistributedLoad.Objects.Elements.Select(x => x.CustomData[AdapterId].ToString()).ToList();

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
                            barVaryingDistributedLoad.ForceA = createSingleComponentVector(i, startLoadVectors[i]);
                            barVaryingDistributedLoad.ForceB = createSingleComponentVector(i, endLoadVectors[i]);

                            foreach (string assignedBar in assignedBars)
                            {
                                midasBarLoads.Add(Engine.MidasCivil.Convert.ToMCBarVaryingDistributedLoad(barVaryingDistributedLoad, assignedBar, "Force"));
                            }
                        }
                        else
                        {
                            barVaryingDistributedLoad.MomentA = createSingleComponentVector(i-3, startLoadVectors[i]);
                            barVaryingDistributedLoad.MomentB = createSingleComponentVector(i-3, endLoadVectors[i]);

                            foreach (string assignedBar in assignedBars)
                            {
                                midasBarLoads.Add(Engine.MidasCivil.Convert.ToMCBarVaryingDistributedLoad(barVaryingDistributedLoad, assignedBar, "Moment"));
                            }
                        }

                    }
                }

                File.AppendAllLines(loadGroupPath, midasLoadGroup);
                File.AppendAllLines(barLoadPath, midasBarLoads);
            }



            return true;
        }

        public static Vector createSingleComponentVector(int index, double value)
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
