using System.Collections.Generic;

namespace BH.Engine.MidasCivil
{
    public partial class Modify
    {
        public static void CleanString(ref List<string> sectionText)
        {
            List<string> cleanString = new List<string>();

            foreach(string text in sectionText)
            {
                if (!(text.Contains(";")) && !(text.Contains("*")) && !(string.IsNullOrEmpty(text)))
                {
                    cleanString.Add(text);
                }
            }

            sectionText = cleanString;
        }
    }
}
