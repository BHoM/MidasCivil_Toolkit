using BH.oM.Structure.Loads;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public bool CreateCollection(IEnumerable<GravityLoad> gravityLoads)
        {
            string loadGroupPath = CreateSectionFile("LOAD-GROUP");

            foreach (GravityLoad gravityLoad in gravityLoads)
            {
                List<string> midasGravityLoads = new List<string>();
                string gravityLoadPath = CreateSectionFile(gravityLoad.Loadcase.Name + "\\SELFWEIGHT");

                string midasLoadGroup = Engine.MidasCivil.Convert.ToMCLoadGroup(gravityLoad);

                midasGravityLoads.Add(Engine.MidasCivil.Convert.ToMCGravityLoad(gravityLoad));

                string[] exisitingGravityLoads = File.ReadAllLines(gravityLoadPath);
                bool containsGravity = false;

                foreach (string existingGravityLoad in exisitingGravityLoads)
                {
                    if (existingGravityLoad.Contains("SELFWEIGHT"))
                        containsGravity = true;
                }

                if (containsGravity)
                    BH.Engine.Reflection.Compute.RecordError("Midas only supports one GravityLoad per loadcase");
                else
                {
                    CompareLoadGroup(midasLoadGroup, loadGroupPath);
                    RemoveLoadEnd(gravityLoadPath);
                    File.AppendAllLines(gravityLoadPath, midasGravityLoads);
                }
            }

            return true;
        }

    }
}
