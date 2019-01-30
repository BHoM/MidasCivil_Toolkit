using System.Collections.Generic;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public string CreateAssignmentString(List<int> assignmentIndexes)
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
