using System.Collections.Generic;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public void SetSectionText()
        {
            List<int> sectionIndexes = midasText.Select((value, index) => new { value, index })
                .Where(x => x.ToString().Contains("*"))
                .Select(x => x.index)
                .ToList();

            List<int> loadcaseStarts = midasText.Select((value, index) => new { value, index })
                .Where(x => x.ToString().Contains("*USE-STLD"))
                .Select(x => x.index)
                .ToList();

            List<int> loadcaseEnds = midasText.Select((value, index) => new { value, index })
                .Where(x => x.ToString().Contains("; End of data for load case"))
                .Select(x => x.index)
                .ToList();

            List<int> loadcaseRange = new List<int>();

            for (int j = 0; j < loadcaseStarts.Count(); j++)
            {
                loadcaseRange.AddRange(Enumerable.Range(
                    loadcaseStarts[j] +1, loadcaseEnds[j] - loadcaseStarts[j] + 1));
            }

            List<int> loadSectionIndexes = loadcaseRange.Where(x => sectionIndexes.Contains(x)).ToList();

            for (int i = 0; i < sectionIndexes.Count() - 1; i++)
            {
                int sectionStart = sectionIndexes[i];
                int sectionEnd = sectionIndexes[i + 1] - 1;

                string sectionHeader = midasText[sectionStart];

                if (!(sectionHeader[0] == '*') || loadSectionIndexes.Contains(sectionStart))
                {
                    continue;
                }

                string sectionName = Engine.MidasCivil.Query.SectionName(sectionHeader);
                List<string> sectionText = midasText.GetRange(sectionStart, sectionEnd - sectionStart);

                if (loadcaseStarts.Contains(sectionStart))
                {
                    string loadcaseName = sectionHeader.Split(',')[1];
                    string path = directory + "\\TextFiles\\" + loadcaseName;
                    System.IO.Directory.CreateDirectory(path);
                    WriteSectionText(sectionText, sectionName, path);

                    int loadcaseEnd = loadcaseEnds[loadcaseStarts.IndexOf(sectionStart)];

                    for(int k = i+1; sectionIndexes[k] < loadcaseEnd; k++)
                    {
                        int loadStart = sectionIndexes[k];
                        int loadEnd = sectionIndexes[k + 1] - 1;
                        string loadHeader = midasText[loadStart];
                        List<string> loadText = midasText.GetRange(loadStart, loadEnd - loadStart);
                        if (!(loadHeader[0] == '*'))
                        {
                            continue;
                        }
                        string loadName = Engine.MidasCivil.Query.SectionName(midasText[loadStart]);
                        WriteSectionText(loadText, loadName, path);
                    }
                }
                else
                {
                    CreateSectionFile(sectionName);
                    WriteSectionText(sectionText, sectionName);
                }
            }
        }
    }
}