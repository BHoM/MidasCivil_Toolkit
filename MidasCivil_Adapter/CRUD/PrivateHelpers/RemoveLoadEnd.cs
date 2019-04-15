using System.IO;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private void RemoveLoadEnd(string path)
        {
            string[] loads = File.ReadAllLines(path);

            for (int i = 0; i < loads.Length; i++)
            {
                if (loads[i].Contains("; End of data"))
                    loads[i] = "";
            }

            loads = loads.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            File.Delete(path);
            File.AppendAllLines(path, loads);
        }
    }
}
