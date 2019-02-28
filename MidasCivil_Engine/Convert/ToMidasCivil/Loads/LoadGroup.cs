using BH.oM.Structure.Loads;
using System.Collections.Generic;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCLoadGroup(this ILoad load)
        {
            string midasLoadGroup = load.Name;
            return midasLoadGroup;
        }
    }
}