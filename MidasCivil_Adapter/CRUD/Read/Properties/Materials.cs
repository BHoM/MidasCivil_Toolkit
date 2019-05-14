using System.Collections.Generic;
using BH.oM.Physical.Materials;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<Material> ReadMaterials(List<string> ids = null)
        {
            List<Material> bhomMaterials = new List<Material>();

            List<string> materialText = GetSectionText("MATERIAL");

            foreach (string material in materialText)
            {
                Material bhomMaterial = Engine.MidasCivil.Convert.ToBHoMMaterial(material);
                bhomMaterials.Add(bhomMaterial);
            }

            return bhomMaterials;
        }

    }
}
