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
        public bool CreateCollection(IEnumerable<BarUniformlyDistributedLoad> barUniformlyDistributedLoads)
        {
            string loadGroupPath = CreateSectionFile("LOAD-GROUP");

            foreach (BarUniformlyDistributedLoad barUniformlyDistributedLoad in barUniformlyDistributedLoads)
            {
                List<string> midasBarLoads = new List<string>();
                string barLoadPath = CreateSectionFile(barUniformlyDistributedLoad.Loadcase.Name + "\\BEAMLOAD");
                List<string> midasLoadGroup = new List<string>();
                midasLoadGroup.Add(Engine.MidasCivil.Convert.ToMCLoadGroup(barUniformlyDistributedLoad));

                List<string> assignedBars = barUniformlyDistributedLoad.Objects.Elements.Select(x => x.CustomData[AdapterId].ToString()).ToList();

                List<double> loadVectors = new List<double> { barUniformlyDistributedLoad.Force.X,
                                                              barUniformlyDistributedLoad.Force.Y,
                                                              barUniformlyDistributedLoad.Force.Z,
                                                              barUniformlyDistributedLoad.Moment.X,
                                                              barUniformlyDistributedLoad.Moment.Y,
                                                              barUniformlyDistributedLoad.Moment.Z};

                Vector zeroVector = new Vector { X = 0, Y = 0, Z = 0 };

                for (int i = 0; i < 6; i++)
                {
                    barUniformlyDistributedLoad.Force = zeroVector;
                    barUniformlyDistributedLoad.Moment = zeroVector;

                    if (loadVectors[i] != 0)
                    {
                        if (i < 3)
                        {
                            barUniformlyDistributedLoad.Force = createSingleComponentVector(i, loadVectors[i]);

                            foreach (string assignedBar in assignedBars)
                            {
                                midasBarLoads.Add(Engine.MidasCivil.Convert.ToMCBarUniformlyDistributedLoad(barUniformlyDistributedLoad, assignedBar, "Force"));
                            }
                        }
                        else
                        {
                            barUniformlyDistributedLoad.Moment = createSingleComponentVector(i - 3, loadVectors[i]);

                            foreach (string assignedBar in assignedBars)
                            {
                                midasBarLoads.Add(Engine.MidasCivil.Convert.ToMCBarUniformlyDistributedLoad(barUniformlyDistributedLoad, assignedBar, "Moment"));
                            }
                        }

                    }
                }

                string[] loads = File.ReadAllLines(barLoadPath);

                for (int i=0; i<loads.Length; i++)
                {
                    if (loads[i].Contains("; End of data"))
                        loads[i] = "";
                }

                loads = loads.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                File.Delete(barLoadPath);
                File.AppendAllLines(loadGroupPath, midasLoadGroup);
                File.AppendAllLines(barLoadPath, loads);
                File.AppendAllLines(barLoadPath, midasBarLoads);
            }



            return true;
        }
    }
}
