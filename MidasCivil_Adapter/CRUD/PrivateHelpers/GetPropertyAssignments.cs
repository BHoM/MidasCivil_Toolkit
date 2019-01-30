using System;
using System.Collections.Generic;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public Dictionary<string,List<int>> GetPropertyAssignments(string section, string namePrefix)
        {
            List<string> sectionText = GetSectionText(section);

            Dictionary<string, List<int>> propertyAssignments = new Dictionary<string, List<int>>();

            for (int i = 0; i < sectionText.Count(); i++)
            {
                string splitSection = sectionText[i].Split(',')[0];

                List<string> geometryAssignments = new List<string>();

                if(splitSection.Contains(" "))
                {
                    geometryAssignments= splitSection.Split(' ').
                        Select(x=>x.Trim()).
                        Where(x => !string.IsNullOrEmpty(x)).
                        ToList();
                }

                List<int> propertyAssignment = GetAssignmentsAsList(geometryAssignments);

                propertyAssignments.Add(namePrefix + "_" + (i+1), propertyAssignment);
            }

            return propertyAssignments;
        }
    }
}
