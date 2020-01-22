using BH.oM.Structure.Elements;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCRigidLink(this RigidLink link)
        {
            string midasLink = "";

            string masterNode = link.MasterNode.CustomData[AdapterIdName].ToString();
            string slaveNodes = "";

            foreach (Node slaveNode in link.SlaveNodes)
            {
                slaveNodes = slaveNodes + " " + slaveNode.CustomData[AdapterIdName].ToString();
            }

            string fixity = boolToFixity(link.Constraint.XtoX) +
                            boolToFixity(link.Constraint.YtoY) +
                            boolToFixity(link.Constraint.ZtoZ) +
                            boolToFixity(link.Constraint.XXtoXX) +
                            boolToFixity(link.Constraint.YYtoYY) +
                            boolToFixity(link.Constraint.ZZtoZZ);

            midasLink = "1, " + masterNode + "," + fixity + "," + slaveNodes + "," + link.Name;

            return midasLink;
        }

        private static string boolToFixity(bool fixity)
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