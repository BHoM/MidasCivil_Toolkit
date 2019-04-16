using System.IO;
using System;
using BH.oM.Structure.Elements;
using System.Collections.Generic;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCRigidLink(this RigidLink link)
        {
            string midasLink;

            string masterNode = link.MasterNode.CustomData[AdapterId].ToString();
            string slaveNodes = "";

            foreach (Node node in link.SlaveNodes)
            {
                slaveNodes = slaveNodes + " " + node.CustomData[AdapterId].ToString();
            }

            string fixity = boolToFixity(link.Constraint.XtoX) +
                            boolToFixity(link.Constraint.YtoY) +
                            boolToFixity(link.Constraint.XXtoXX) +
                            boolToFixity(link.Constraint.ZtoZ) +
                            boolToFixity(link.Constraint.YYtoYY) +
                            boolToFixity(link.Constraint.ZZtoZZ);

            midasLink = masterNode + "," + fixity + "," + slaveNodes + "," + link.Name;

            return midasLink;
        }

        public static string boolToFixity(bool fixity)
        {
            string converted = "0";

            if (fixity)
            {
                converted = "1";
            }

            return converted;
        }
    }
}