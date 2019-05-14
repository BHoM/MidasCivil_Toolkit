using BH.oM.Structure.Properties.Constraint;
using System.Collections.Generic;
using System.Linq;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static BarRelease ToBHoMBarRelease(string release, int count)
        {
            List<string> delimitted = release.Split(',').ToList();

            string releaseName = "";

            string startFixity = delimitted[1].Replace(" ", "");
            string endFixity = delimitted[8].Replace(" ", "");

            List<bool> bhomStartFixity = new List<bool>();
            List<bool> bhomEndFixity = new List<bool>();

            for (int i=0; i<6; i++)
            {
                bhomStartFixity.Add(Engine.MidasCivil.Convert.Fixity(startFixity.Substring(i, 1)));
                bhomEndFixity.Add(Engine.MidasCivil.Convert.Fixity(endFixity.Substring(i, 1)));
            }

            Constraint6DOF startConstraint = BH.Engine.Structure.Create.Constraint6DOF(bhomStartFixity[0], bhomStartFixity[1], bhomStartFixity[2],
                                                                                       bhomStartFixity[3], bhomStartFixity[4], bhomStartFixity[5], "StartConstraint");
            Constraint6DOF endConstraint = BH.Engine.Structure.Create.Constraint6DOF(bhomEndFixity[0], bhomEndFixity[1], bhomEndFixity[2],
                                                                                     bhomEndFixity[3], bhomEndFixity[4], bhomEndFixity[5], "EndConstraint");

            if (!string.IsNullOrWhiteSpace(delimitted[15]))
            {
                releaseName = delimitted[15].Replace(" ", "");
            }
            else
            {
                releaseName = "BR" + count.ToString();
            }

            BarRelease bhomBarRelease = Engine.Structure.Create.BarRelease(startConstraint, endConstraint, releaseName);
            bhomBarRelease.CustomData[AdapterId] = bhomBarRelease.Name;

            return bhomBarRelease;
        }

    }
}