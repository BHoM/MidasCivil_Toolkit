using System;
using System.Collections.Generic;
using BH.oM.Structure.Properties.Constraint;
using System.Linq;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private List<BarRelease> ReadBarReleases(List<string> ids = null)
        {
            List<BarRelease> bhomBarReleases = new List<BarRelease>();

            int count = 1;

            List<string> barReleaseText = GetSectionText("FRAME-RLS");

            if (barReleaseText.Count != 0)
            {
                List<string> barComparison = new List<string>();
                List<string> releasedBars = new List<string>();

                for (int i = 0; i < barReleaseText.Count; i += 2)
                {
                    List<string> delimitted = barReleaseText[i].Split(',').ToList();
                    releasedBars.Add(delimitted[0].Replace(" ", ""));
                    delimitted.RemoveAt(0);
                    barComparison.Add(String.Join(",", delimitted).Replace(" ", "") + "," + barReleaseText[i + 1].Replace(" ", ""));
                }

                List<string> distinctBarReleases = barComparison.Distinct().ToList();

                foreach (string distinctBarRelease in distinctBarReleases)
                {
                    List<int> indexMatches = barComparison.Select((barload, index) => new { barload, index })
                                               .Where(x => string.Equals(x.barload, distinctBarRelease))
                                               .Select(x => x.index)
                                               .ToList();
                    List<string> matchingBars = new List<string>();
                    indexMatches.ForEach(x => matchingBars.Add(releasedBars[x]));

                    BarRelease bhomBarRelease = Engine.MidasCivil.Convert.ToBHoMBarRelease(distinctBarRelease, count);
                    bhomBarReleases.Add(bhomBarRelease);

                    if ((distinctBarRelease.Split(',').ToList()[15].ToString() == " "))
                    {
                        count = count + 1;
                    }

                }
            }

            return bhomBarReleases;
        }

    }
}
