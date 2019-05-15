using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.MaterialFragments;

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
            Dictionary<string, List<int>> elementGroups = ReadTags("GROUP", 2);

            foreach (string element in elementsText)
            {
                foreach (string type in beamElements)
                {
                    if (element.Contains(type))
                        barText.Add(element);
                }
            }

            IEnumerable<Node> bhomNodesList = ReadNodes();
            Dictionary<string, Node> bhomNodes = bhomNodesList.ToDictionary(
                x => x.CustomData[AdapterId].ToString());

            IEnumerable<ISectionProperty> bhomSectionPropertyList = ReadSectionProperties();
            Dictionary<string, ISectionProperty> bhomSectionProperties = bhomSectionPropertyList.ToDictionary(
                x => x.CustomData[AdapterId].ToString());

            IEnumerable<BarRelease> bhomBarReleaseList = ReadBarReleases();
            Dictionary<string, BarRelease> bhomBarReleases = bhomBarReleaseList.ToDictionary(x => x.Name.ToString());

            IEnumerable<IMaterialFragment> bhomMaterialList = ReadMaterials();
            Dictionary<string, IMaterialFragment> bhomMaterials = bhomMaterialList.ToDictionary(x => x.Name.ToString());

            Dictionary<string, List<int>> barReleaseAssignments = GetBarReleaseAssignments("FRAME-RLS", "barRelease");

            foreach (string bar in barText)
            {
                Bar bhomBar = Engine.MidasCivil.Convert.ToBHoMBar(bar, bhomNodes, bhomSectionProperties,bhomMaterials, bhomBarReleases, barReleaseAssignments);
                int bhomID = System.Convert.ToInt32(bhomBar.CustomData[AdapterId]);
                bhomBar.Tags = GetGroupAssignments(elementGroups, bhomID);
                bhomBars.Add(bhomBar);
            }

            return bhomBars;
        }

    }
}
