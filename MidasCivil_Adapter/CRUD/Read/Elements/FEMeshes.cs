using System.Collections.Generic;
using System.Linq;
using BH.oM.Structure.Elements;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<FEMesh> ReadFEMeshses(List<string> ids = null)
        {
            List<FEMesh> bhomMeshes = new List<FEMesh>();

            List<string> elementsText = GetSectionText("ELEMENT",directory);
            List<string> meshText = elementsText.Where(x => x.Contains("PLATE")).ToList();

            IEnumerable<Node> bhomNodesList = ReadNodes();
            Dictionary<string, Node> bhomNodes = bhomNodesList.ToDictionary(
                x => x.CustomData[AdapterId].ToString());

            foreach (string mesh in meshText)
            {
                FEMesh bhomMesh = Engine.MidasCivil.Convert.ToBHoMFEMesh(mesh, bhomNodes);
                bhomMeshes.Add(bhomMesh);
            }

            return bhomMeshes;
        }
    }
}
