using System.Collections.Generic;

namespace BH.Engine.MidasCivil
{
    public partial class Compute
    {
        public static string AssignmentString(List<int> assignmentIndexes)
        {
            string indexes = "";

            foreach (int index in assignmentIndexes)
            {
                indexes = indexes + " " + index;
            }

            return indexes;
        }
    }
}
