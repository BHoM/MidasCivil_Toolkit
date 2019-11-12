using System.Linq;
using System.Collections.Generic;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Constraints;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static RigidLink ToBHoMRigidLink(string rigidLink, Dictionary<string,Node> nodes, int count)
        {
            string[] delimitted = rigidLink.Split(',');
            List<Node> slaveNodes = new List<Node>();

            string master = delimitted[0].Trim();
            string fixity = delimitted[1].Replace(" ","");
            List<string> slaves = delimitted[2].Split(' ').Where(m => !string.IsNullOrWhiteSpace(m)).ToList();
            List<int> assignments = Engine.MidasCivil.Query.Assignments(slaves);

            bool x = Engine.MidasCivil.Convert.Fixity(fixity.Substring(0, 1));
            bool y = Engine.MidasCivil.Convert.Fixity(fixity.Substring(1, 1));
            bool z = Engine.MidasCivil.Convert.Fixity(fixity.Substring(2, 1));
            bool xx = Engine.MidasCivil.Convert.Fixity(fixity.Substring(3, 1));
            bool yy = Engine.MidasCivil.Convert.Fixity(fixity.Substring(4, 1));
            bool zz = Engine.MidasCivil.Convert.Fixity(fixity.Substring(5, 1));

            LinkConstraint constraint = new LinkConstraint { XtoX = x, YtoY = y, ZtoZ = z, XXtoXX = xx, YYtoYY = yy, ZZtoZZ = zz };

            Node masterNode;
            nodes.TryGetValue(master, out masterNode);

            foreach (int assignment in assignments)
            {
                Node bhomSlave;
                nodes.TryGetValue(assignment.ToString(), out bhomSlave);
                slaveNodes.Add(bhomSlave);
            }

            string name = "";

            if (string.IsNullOrWhiteSpace(delimitted[3]))
            {
                name = "RL" + count;
            }
            else
            {
                name = delimitted[3].Trim();
            }

            RigidLink bhomRigidLink = Engine.Structure.Create.RigidLink(masterNode, slaveNodes, constraint);
            bhomRigidLink.Name = name;
            bhomRigidLink.CustomData[AdapterId] = name;

            return bhomRigidLink;
        }

    }
}

