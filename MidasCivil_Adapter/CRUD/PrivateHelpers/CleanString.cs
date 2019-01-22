using System.Collections.Generic;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public void CleanString(ref List<string> sectionText)
        {
            List<string> cleanString = new List<string>();

            foreach (string text in sectionText)
            {
                if (!(text.Contains(";")) && !(text.Contains("*")) && !(string.IsNullOrEmpty(text)))
                {

                    cleanString.Add(text.Replace(" ",""));
                }
            }

            sectionText = cleanString;
        }
    }
}
