using System;
using System.Linq;
using System.Collections.Generic;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties.Constraint;
using BH.oM.Geometry;
using BH.oM.Base;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static RigidLink ToBHoMRigidLink(string rigidLink, Dictionary<string,Node> nodeDictionary, int count)
        {
            string[] delimitted = rigidLink.Split(',');
            Node masterNode;
            List<Node> slaveNodes = new List<Node>();

            string master = delimitted[0].Replace(" ", "");
            string fixity = delimitted[1].Replace(" ","");
            List<string> slaves = delimitted[2].Split(' ').Where(m => !string.IsNullOrWhiteSpace(m)).ToList();
            List<int> assignments = Engine.MidasCivil.Query.Assignments(slaves);

            bool x = convertFixity(fixity.Substring(0, 1));
            bool y = convertFixity(fixity.Substring(1, 1));
            bool z = convertFixity(fixity.Substring(2, 1));
            bool xx = convertFixity(fixity.Substring(3, 1));
            bool yy = convertFixity(fixity.Substring(4, 1));
            bool zz = convertFixity(fixity.Substring(5, 1));

            LinkConstraint constraint = new LinkConstraint { XtoX = x, YtoY = y, ZtoZ = z, XXtoXX = xx, YYtoYY = yy, ZZtoZZ = zz };

            nodeDictionary.TryGetValue(master, out masterNode);

            foreach (int assignment in assignments)
            {
                Node bhomSlave;
                nodeDictionary.TryGetValue(assignment.ToString(), out bhomSlave);
                slaveNodes.Add(bhomSlave);
            }

            string name;

            if (string.IsNullOrWhiteSpace(delimitted[3]))
            {
                name = "RL" + count;
            }
            else
            {
                name = delimitted[3].Replace(" ", "");
            }

            RigidLink bhomRigidLink = Engine.Structure.Create.RigidLink(masterNode, slaveNodes, constraint);
            bhomRigidLink.Name = name;
            bhomRigidLink.CustomData[AdapterId] = name;

            return bhomRigidLink;
        }

        public static bool convertFixity(string number)
        {
            bool fixity = false;

            if (int.Parse(number)==1)
            {
                fixity = true;
            }

            return fixity;
        }

    }
}

