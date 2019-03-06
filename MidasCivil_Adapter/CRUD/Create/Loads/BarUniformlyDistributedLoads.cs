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
                List<BarUniformlyDistributedLoad> seperatedLoads = new List<BarUniformlyDistributedLoad>();
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
 
                List<string> directions = new List<string> { "X", "Y", "Z", "X", "Y", "Z" };

                for (int i=0; i<6; i++)
                {
                    Vector zeroVector = new Vector { X = 0, Y = 0, Z = 0 };
                    barUniformlyDistributedLoad.Force = zeroVector;
                    barUniformlyDistributedLoad.Moment = zeroVector;

                    if (loadVectors[i]!=0)
                    {
                        PropertyInfo property = barUniformlyDistributedLoad.Force.GetType().GetProperty(directions[i]);
                        property.SetValue(barUniformlyDistributedLoad.Force, System.Convert.ChangeType(loadVectors[i], property.PropertyType));

                        if (i<3)
                        {
                            foreach (string assignedBar in assignedBars)
                            {
                                midasBarLoads.Add(Engine.MidasCivil.Convert.ToMCBarUniformlyDistributedLoad(barUniformlyDistributedLoad, assignedBar, "Force"));
                            }
                        }
                        else
                        {
                            foreach (string assignedBar in assignedBars)
                            {
                                midasBarLoads.Add(Engine.MidasCivil.Convert.ToMCBarUniformlyDistributedLoad(barUniformlyDistributedLoad, assignedBar, "Moment"));
                            }
                        }

                    }
                }

                File.AppendAllLines(loadGroupPath, midasLoadGroup);
                File.AppendAllLines(barLoadPath, midasBarLoads);
            }



            return true;
        }
    }
}
