using BH.oM.Structure.Elements;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Structure.SectionProperties;
using System.Collections.Generic;
using System.Linq;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static Bar ToBHoMBar(this string bar, Dictionary<string, Node> bhomNodes,
            Dictionary<string, ISectionProperty> bhomSectionProperties, Dictionary<string, IMaterialFragment> bhomMaterials,
            Dictionary<string, BarRelease> barReleases, Dictionary<string, List<int>> barReleaseAssignments)
        {
            List<string> delimitted = bar.Split(',').ToList();
            Node startNode = null;
            Node endNode = null;
            BarFEAType feaType = BarFEAType.Axial;
            ISectionProperty sectionProperty = null;
            IMaterialFragment material = null;

            bhomNodes.TryGetValue(delimitted[4].Trim(), out startNode);
            bhomNodes.TryGetValue(delimitted[5].Trim(), out endNode);

            if (!(bhomSectionProperties.Count() == 0))
            {
                bhomSectionProperties.TryGetValue(delimitted[3].Trim(), out sectionProperty);
                if (!(bhomMaterials.Count() == 0))
                {
                    bhomMaterials.TryGetValue(delimitted[2].Trim(), out material);
                    sectionProperty.Material = material;
                }
            }

            switch (delimitted[1].Trim())
            {
                case "TRUSS":
                    break;

                case "BEAM":
                    feaType = BarFEAType.Flexural;
                    break;

                case "TENSTR":
                    feaType = BarFEAType.TensionOnly;
                    break;

                case "COMPTR":
                    feaType = BarFEAType.CompressionOnly;
                    break;
            }

            int bhomID = System.Convert.ToInt32(delimitted[0].Trim());

            string barReleaseName = "";

            foreach (KeyValuePair<string, List<int>> barReleaseAssignment in barReleaseAssignments)
            {
                if (barReleaseAssignment.Value.Contains(bhomID))
                {
                    barReleaseName = barReleaseAssignment.Key;
                    break;
                }
            }

            BarRelease barRelease = null;
            if (!(barReleaseName == ""))
            {
                barReleases.TryGetValue(barReleaseName, out barRelease);
            }

            double orientationAngle = double.Parse(delimitted[6].Trim());

            Bar bhomBar = Structure.Create.Bar(startNode, endNode, sectionProperty, orientationAngle, barRelease, feaType);
            bhomBar.CustomData[AdapterId] = bhomID;

            return bhomBar;
        }
    }
}

