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
            foreach (GravityLoad gravityLoad in gravityLoads)
            {
                List<string> midasGravityLoads = new List<string>();
                string pointForcePath = CreateSectionFile(gravityLoad.Loadcase.Name + "\\SELFWEIGHT");

                midasGravityLoads.Add(Engine.MidasCivil.Convert.ToMCGravityLoad(gravityLoad));

                File.AppendAllLines(pointForcePath, midasGravityLoads);
            }

            return true;
        }

    }
}
