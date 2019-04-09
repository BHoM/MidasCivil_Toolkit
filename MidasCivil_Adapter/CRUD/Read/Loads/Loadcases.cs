using System.Collections.Generic;
using BH.oM.Structure.Loads;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<Loadcase> ReadLoadcases(List<string> ids = null)
        {
            List<Loadcase> bhomLoadCases = new List<Loadcase>();
            List<string> loadcaseText = GetSectionText("STLDCASE");

            foreach (string loadcase in loadcaseText)
            {
                Loadcase bhomLoadCase = Engine.MidasCivil.Convert.ToBHoMLoadcase(loadcase);
                bhomLoadCases.Add(bhomLoadCase);
            }

            return bhomLoadCases;
        }

    }
}