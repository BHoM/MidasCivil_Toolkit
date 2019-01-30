using System;
using System.Collections.Generic;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public List<int> GetAssignmentsAsList(List<string> assignments)
        {
            List<int> propertyAssignment = new List<int>();

            foreach (string assignment in assignments)
            {
                if (assignment.Contains("by"))
                {
                    propertyAssignment.AddRange(RangeBySplit(assignment, "to", "by"));
                }
                else if (assignment.Contains("to"))
                {
                    propertyAssignment.AddRange(RangeBySplit(assignment, "to"));
                }
                else
                {
                    int id = Convert.ToInt32(assignment);
                    propertyAssignment.Add(id);
                }
            }

            return propertyAssignment;

        }
    }
}