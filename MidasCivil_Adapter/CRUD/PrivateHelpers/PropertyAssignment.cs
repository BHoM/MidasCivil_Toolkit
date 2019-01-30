using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public void PropertyAssignment(string bhomID, string propertyName, string section)
        {
            string path = directory + "\\" + section + ".txt";

            List<string> propertyText = File.ReadAllLines(path).ToList();

            int index = propertyText.FindIndex(x => x.Contains(propertyName));

            string constraint = propertyText[index];

            string [] split = constraint.Split(',');

            string assignmentList = split[0];

            if(!(string.IsNullOrWhiteSpace(assignmentList)))
            {
                List<string> assignmentRanges = new List<string>();
                if (assignmentList.Contains(" "))
                {
                    assignmentRanges = assignmentList.Split(' ').
                        Select(x => x.Trim()).
                        Where(x => !string.IsNullOrEmpty(x)).
                        ToList();
                }
                List<int> assignments = GetAssignmentsAsList(assignmentRanges);
                assignments.Add(int.Parse(bhomID));

                split[0] = CreateAssignmentString(assignments);
            }
            else
            {
                split[0] = bhomID;
            }

            string updatedProperty = split[0];

            for(int i = 1; i < split.Count(); i++)
            {
                updatedProperty = updatedProperty + "," + split[i];
            }

            propertyText[index] = updatedProperty;

            using (StreamWriter sectionText = File.CreateText(path))
            {
                foreach(string property in propertyText)
                {
                    sectionText.WriteLine(property);
                }
                sectionText.Close();
            }

        }
    }
}
