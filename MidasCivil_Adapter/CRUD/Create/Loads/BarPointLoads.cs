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
                string midasLoadGroup = Engine.MidasCivil.Convert.ToMCLoadGroup(barPointLoad);

                List<string> assignedBars = barPointLoad.Objects.Elements.Select(x => x.CustomData[AdapterId].ToString()).ToList();

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
                            barPointLoad.Force = createSingleComponentVector(i, loadVectors[i]);

                            foreach (string assignedBar in assignedBars)
                            {
                                midasBarLoads.Add(Engine.MidasCivil.Convert.ToMCBarPointLoad(barPointLoad, assignedBar, "Force"));
                            }
                        }
                        else
                        {
                            barPointLoad.Moment = createSingleComponentVector(i - 3, loadVectors[i]);

                            foreach (string assignedBar in assignedBars)
                            {
                                midasBarLoads.Add(Engine.MidasCivil.Convert.ToMCBarPointLoad(barPointLoad, assignedBar, "Moment"));
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
