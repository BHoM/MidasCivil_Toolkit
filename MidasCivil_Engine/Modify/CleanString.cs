using System.Collections.Generic;

namespace BH.Engine.MidasCivil
{
    public partial class Modify
    {
        public static void CleanString(ref List<string> sectionText)
        {
            List<string> cleanString = new List<string>();

            if (sectionText[0].Contains("SELFWEIGHT"))
            {
                cleanString.Add(sectionText[0].Split('*')[1]);
            }

            for (int i = 0; i < sectionText.Count; i++)
            {
                if (!(sectionText[i].Contains(";")) && !(sectionText[i].Contains("*")) && !(string.IsNullOrEmpty(sectionText[i])))
                {
                    if (sectionText[i].Contains("\\"))
                    {
                        string combined = sectionText[i].Replace("\\", "");
                        bool finished = false;
                        while (!finished)
                        {
                            i++;
                            if (!sectionText[i].Contains("\\"))
                            {
                                combined = combined + sectionText[i];
                                finished = true;
                            }
                            else
                            {
                                combined = combined + sectionText[i].Replace("\\", "");
                            }
                        }
                        cleanString.Add(combined);
                    }
                    else
                    {
                        cleanString.Add(sectionText[i]);
                    }
                }
            }

            sectionText = cleanString;
        }
    }
}
