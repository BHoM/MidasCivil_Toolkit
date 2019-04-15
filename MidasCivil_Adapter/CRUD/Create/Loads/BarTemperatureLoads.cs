using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;
using System.Collections.Generic;
using System.IO;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public bool CreateCollection(IEnumerable<BarTemperatureLoad> barTemperatureLoads)
        {
            string loadGroupPath = CreateSectionFile("LOAD-GROUP");

            foreach (BarTemperatureLoad barTemperatureLoad in barTemperatureLoads)
            {
                List<string> midasTemperatureLoads = new List<string>();
                string barLoadPath = CreateSectionFile(barTemperatureLoad.Loadcase.Name + "\\ELTEMPER");
                string midasLoadGroup = Engine.MidasCivil.Convert.ToMCLoadGroup(barTemperatureLoad);

                List<Bar> assignedElements = barTemperatureLoad.Objects.Elements;

                List<string> assignedBars = new List<string>();

                foreach (Bar bar in assignedElements)
                {
                    assignedBars.Add(bar.CustomData[AdapterId].ToString());
                }

                foreach (string assignedBar in assignedBars)
                {
                    midasTemperatureLoads.Add(Engine.MidasCivil.Convert.ToMCBarTemperatureLoad(barTemperatureLoad, assignedBar));
                }

                CompareLoadGroup(midasLoadGroup, loadGroupPath);
                RemoveLoadEnd(barLoadPath);
                File.AppendAllLines(barLoadPath, midasTemperatureLoads);
            }

            return true;
        }

    }
}
