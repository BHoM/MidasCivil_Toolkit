using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<Bar> ReadBars(List<string> ids = null)
        {
            List<Bar> bhomBars = new List<Bar>();

            List<string> elementsText = GetSectionText(midasText, "*ELEMENT");
            List<string> barText = elementsText.Where(x => x.Contains("BEAM")).ToList();

            IEnumerable<Node> bhomNodesList = ReadNodes();
            Dictionary<string, Node> bhomNodes = bhomNodesList.ToDictionary(
                x => x.CustomData[AdapterId].ToString());

            foreach (string bar in barText)
            {
                Bar bhomBar = Engine.MidasCivil.Convert.ToBHoMBar(bar,bhomNodes);
                bhomBars.Add(bhomBar);
            }

            return bhomBars;
        }
    }
}
