using System.Collections.Generic;
using System.IO;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private void WriteSectionText(List<string> sectionText, string section)
        {
            string path = directory + "\\TextFiles\\" + section + ".txt";

            using (StreamWriter sectionFile = File.CreateText(path))
            {
                foreach (string text in sectionText)
                {
                    sectionFile.WriteLine(text);
                }
                sectionFile.Close();
            }
        }
        private void WriteSectionText(List<string> sectionText, string section, string subDirectory)
        {
            string path = subDirectory + "\\" + section + ".txt";

            using (StreamWriter sectionFile = File.CreateText(path))
            {
                foreach (string text in sectionText)
                {
                    sectionFile.WriteLine(text);
                }
                sectionFile.Close();
            }
        }
    }
}
