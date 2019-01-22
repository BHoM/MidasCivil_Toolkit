using System.Collections.Generic;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public List<string> GetSectionText(List<string> midasText, string section)
        {
            List<int> sectionIndexes = midasText.Select((value, index) => new { value, index })
                .Where(x => x.ToString().Contains("*"))
                .Select(x => x.index)
                .ToList();

            int nodeStart = -1;
            List<string> sectionText = new List<string>();

            if (midasText.Any(section.Contains))
            {
                nodeStart = midasText.IndexOf(
                    midasText.FirstOrDefault(x => x.Contains(section)));
                int nodeEnd = sectionIndexes[sectionIndexes.IndexOf(nodeStart) + 1];
                sectionText = midasText.GetRange(nodeStart, nodeEnd - nodeStart);
            }

            CleanString(ref sectionText);

            return sectionText;
        }
    }
}
