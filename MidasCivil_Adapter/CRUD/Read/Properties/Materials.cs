using System.Collections.Generic;
using BH.oM.Structure.MaterialFragments;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<IMaterialFragment> ReadMaterials(List<string> ids = null)
        {
            List<IMaterialFragment> bhomMaterials = new List<IMaterialFragment>();

            List<string> materialText = GetSectionText("MATERIAL");

            foreach (string material in materialText)
            {
                IMaterialFragment bhomMaterial = Engine.MidasCivil.Convert.ToBHoMMaterial(material);
                bhomMaterials.Add(bhomMaterial);
            }

            return bhomMaterials;
        }

    }
}
