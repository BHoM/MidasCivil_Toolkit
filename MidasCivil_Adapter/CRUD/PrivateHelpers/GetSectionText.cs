using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        public List<string> GetSectionText(string section)
        {

            string path = directory + "\\TextFiles\\" + section + ".txt";
            List<string> sectionText = new List<string>();

            if (File.Exists(path))
            {
                sectionText = File.ReadAllLines(path).ToList();
                Engine.MidasCivil.Modify.CleanString(ref sectionText);
            }
            return sectionText;
        }
    }
}
