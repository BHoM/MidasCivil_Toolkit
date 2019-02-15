using System;
using System.Collections.Generic;
using BH.oM.Common.Materials;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<Material> ReadMaterials(List<string> ids = null)
        {
            List<Material> bhomMaterials = new List<Material>();

            List<string> materialText = GetSectionText("MATERIAL",directory);

            foreach (string material in materialText)
            {
                Material bhomMaterial = Engine.MidasCivil.Convert.ToBHoMMaterial(material);
                bhomMaterials.Add(bhomMaterial);
            }

            return bhomMaterials;
        }
    }
}
