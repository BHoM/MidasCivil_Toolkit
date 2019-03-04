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

                List<string> midasLoadGroup = new List<string>();
                midasLoadGroup.Add(Engine.MidasCivil.Convert.ToMCLoadGroup(gravityLoad));

                midasGravityLoads.Add(Engine.MidasCivil.Convert.ToMCGravityLoad(gravityLoad));

                File.AppendAllLines(loadGroupPath, midasLoadGroup);
                File.WriteAllText(gravityLoadPath, string.Empty);
                File.AppendAllLines(gravityLoadPath, midasGravityLoads);
            }

            return true;
        }

    }
}
