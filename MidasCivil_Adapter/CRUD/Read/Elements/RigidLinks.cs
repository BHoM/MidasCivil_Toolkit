using BH.oM.Structure.Elements;
using BH.oM.Structure.Constraints;
using System.Collections.Generic;
using System.Linq;


namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<RigidLink> ReadRigidLinks(List<string> ids = null)
        {
            List<RigidLink> bhomRigidLinks = new List<RigidLink>();

            List<string> linkText = GetSectionText("RIGIDLINK");
            List<Node> nodes = ReadNodes();
            Dictionary<string, Node> nodeDictionary = nodes.ToDictionary(x => x.CustomData[AdapterId].ToString());

            int count = 0;

            foreach (string link in linkText)
            {
                RigidLink bhomRigidLink = Engine.MidasCivil.Convert.ToBHoMRigidLink(link, nodeDictionary, count);
                bhomRigidLinks.Add(bhomRigidLink);

                if (string.IsNullOrWhiteSpace(link.Split(',')[3].Replace(" ", "")))
                    count++;
            }

            return bhomRigidLinks;
        }

    }
}
