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
            List<string> beamElements = new List<string> { "BEAM", "TRUSS", "TENSTR", "COMPTR" };
            List<string> barText = new List<string>();

            List<string> elementsText = GetSectionText("ELEMENT");

            foreach (string element in elementsText)
            {
                foreach(string type in beamElements)
                {
                    if (element.Contains(type))
                        barText.Add(element);
                }
            }

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
