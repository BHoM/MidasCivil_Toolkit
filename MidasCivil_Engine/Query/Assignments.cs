using System;
using System.Collections.Generic;

namespace BH.Engine.MidasCivil
{
    public partial class Query
    {
        public static List<int> Assignments(List<string> assignments)
        {
            List<int> propertyAssignment = new List<int>();

            foreach (string assignment in assignments)
            {
                if (assignment.Contains("by"))
                {
                    propertyAssignment.AddRange(Engine.MidasCivil.Compute.RangeBySplit(assignment, "to", "by"));
                }
                else if (assignment.Contains("to"))
                {
                    propertyAssignment.AddRange(Engine.MidasCivil.Compute.RangeBySplit(assignment, "to"));
                }
                else
                {
                    int id = System.Convert.ToInt32(assignment);
                    propertyAssignment.Add(id);
                }
            }

            return propertyAssignment;
        }
    }
}