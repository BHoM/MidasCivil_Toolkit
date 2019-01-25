using System;
using System.Collections.Generic;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public List<int> RangeBySplit(string text, string split)
        {
            string[] splitStringTo = text.Split(new[] { split }, StringSplitOptions.RemoveEmptyEntries);
            int start = System.Convert.ToInt32(splitStringTo[0]);
            int end = System.Convert.ToInt32(splitStringTo[1]);

            List<int> range = new List<int>(Enumerable.Range(start, end - start + 1));

            return range;
        }

        public List<int> RangeBySplit(string text, string split1, string split2)
        {
            string[] splitStringTo = text.Split(new[] { split1 }, StringSplitOptions.RemoveEmptyEntries);
            int start = System.Convert.ToInt32(splitStringTo[0]);
            string[] splitStringBy = splitStringTo[1].Split(new[] { split2 }, StringSplitOptions.RemoveEmptyEntries);
            int increment = System.Convert.ToInt32(splitStringBy[1]);
            int end = System.Convert.ToInt32(splitStringBy[0]);

            List<int> range = new List<int>(Enumerable.Range(start, end - start + 1).Where((x, i) => i % increment == 0));

            return range;
        }

    }
}
