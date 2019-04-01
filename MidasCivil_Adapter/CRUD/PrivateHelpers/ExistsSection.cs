using System.IO;


namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool ExistsSection(string section)
        {
            string path = directory + "\\TextFiles\\" + section + ".txt";

            if (File.Exists(path))
            {
                return true;
            }

            return false;

        }
    }
}