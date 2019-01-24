using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private string CreateSectionText(string section)
        {
            string path = directory + "\\" + section + ".txt";

            if (!File.Exists(path))
            {
                using (StreamWriter sectionText = File.CreateText(path))
                {
                    sectionText.WriteLine("*" + section);
                    sectionText.Close();
                }
            }
            else
            {
                List<string> readSection = File.ReadAllLines(path).ToList();
                if(!(readSection.Contains("*"+section)))
                {
                    using (StreamWriter sectionText = File.CreateText(path))
                    {
                        sectionText.WriteLine("*" + section);
                        sectionText.Close();
                    }
                }
            }

            return path;
        }
    }
}