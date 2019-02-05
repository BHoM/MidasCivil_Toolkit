using BH.oM.Structure.Elements;
using System.Collections.Generic;
using System.Linq;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static Bar ToBHoMBar(this string bar, Dictionary<string,Node> bhomNodes)
        {
            List<string> delimitted = bar.Split(',').ToList();
            Node startNode = null;
            Node endNode = null;

            bhomNodes.TryGetValue(delimitted[4].Replace(" ", ""), out startNode);
            bhomNodes.TryGetValue(delimitted[5].Replace(" ", ""), out endNode);

            Bar bhomBar = Structure.Create.Bar(startNode,endNode);

            bhomBar.CustomData[AdapterId] = delimitted[0].Replace(" ", "");

            return bhomBar;
        }
    }
}

