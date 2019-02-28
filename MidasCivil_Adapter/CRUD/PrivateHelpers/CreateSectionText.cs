using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private string CreateSectionFile(string section)
        {
            string newFolder = directory + "\\TextFiles\\";
            System.IO.Directory.CreateDirectory(newFolder);
            string path =  newFolder + "\\" + section + ".txt";

            if (section.Contains("\\"))
            {
                string [] delimitted = section.Split('\\');
                section = delimitted[delimitted.Count() - 1];
            }

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
                if(!(readSection[0].Contains("*"+section)))
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