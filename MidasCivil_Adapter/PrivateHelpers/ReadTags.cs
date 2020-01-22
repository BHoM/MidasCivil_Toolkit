using System.Collections.Generic;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public Dictionary<string,List<int>> ReadTags(string section, int position)
        {
            List<string> sectionText = GetSectionText(section);

            Dictionary<string, List<int>> itemAssignments = new Dictionary<string, List<int>>();

            for (int i = 0; i < sectionText.Count(); i++)
            {
                string name = sectionText[i].Split(',')[0].Trim();
                string items = sectionText[i].Split(',')[position];

                List<int> itemAssignment = new List<int>();

                if (items.Contains(" ") || string.IsNullOrWhiteSpace(items))
                {
                    List<string> assignments = items.Split(' ').
                        Select(x=>x.Trim()).
                        Where(x => !string.IsNullOrEmpty(x)).
                        ToList();
                    itemAssignment = Engine.MidasCivil.Query.Assignments(assignments);
                }
                else
                {
                    itemAssignment.Add(int.Parse(items));
                }

                itemAssignments.Add(name, itemAssignment);
            }

            return itemAssignments;
        }

    }
}
