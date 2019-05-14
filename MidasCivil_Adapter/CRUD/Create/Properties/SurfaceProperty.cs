using System.IO;
using System.Collections.Generic;
using BH.oM.Structure.SurfaceProperties;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool CreateCollection(IEnumerable<ISurfaceProperty> surfaceProperties)
        {
            string path = CreateSectionFile("THICKNESS");
            List<string> midasSectionProperties = new List<string>();

            foreach (ISurfaceProperty surfaceProperty in surfaceProperties)
            {
                midasSectionProperties.Add(Engine.MidasCivil.Convert.ToMCSurfaceProperty(surfaceProperty));
            }

            File.AppendAllLines(path, midasSectionProperties);

            return true;
        }

    }
}
