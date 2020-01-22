using System.IO;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private void CompareGroup(string group, string path)
        {
            string[] groups = File.ReadAllLines(path);
            bool existing = false;

            for (int i = 0; i < groups.Length; i++)
            {
                if (groups[i].Replace(" ","").Contains(group))
                {
                    existing = true;
                    break;
                }
            }

            if (!existing)
            {
                using (StreamWriter sw = new StreamWriter(path, append:true))
                {
                    sw.WriteLine(group);
                }
            }
        }
    }
}
