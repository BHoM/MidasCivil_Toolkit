using BH.oM.Structure.Elements;
using BH.oM.Structure.Properties.Section;
using System.Collections.Generic;
using System.Linq;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static Bar ToBHoMBar(this string bar, Dictionary<string, Node> bhomNodes,
            Dictionary<string, ISectionProperty> bhomSectionProperties)
        {
            List<string> delimitted = bar.Split(',').ToList();
            Node startNode = null;
            Node endNode = null;
            BarFEAType feaType = BarFEAType.Axial;
            ISectionProperty sectionProperty = null;

            bhomNodes.TryGetValue(delimitted[4].Replace(" ", ""), out startNode);
            bhomNodes.TryGetValue(delimitted[5].Replace(" ", ""), out endNode);
            bhomSectionProperties.TryGetValue(delimitted[3].Replace(" ", ""),out sectionProperty);

            switch (delimitted[1].Replace(" ", ""))
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

            double orientationAngle = int.Parse(delimitted[6].Replace(" ", ""));

            Bar bhomBar = Structure.Create.Bar(startNode, endNode, sectionProperty, orientationAngle, null, feaType);

            bhomBar.CustomData[AdapterId] = delimitted[0].Replace(" ", "");

            return bhomBar;
        }
    }
}

