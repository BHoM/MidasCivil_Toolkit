using System;
using System.Collections.Generic;
using BH.oM.Structure.Loads;
using System.Linq;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static Loadcase ToBHoMLoadcase(this string loadcase, int count)
        {
            List<string> delimitted = loadcase.Split(',').ToList();
            LoadNature nature = LoadNature.Dead;
            midasLoadNatureConverter(delimitted[1].Replace(" ", ""), ref nature);

            Loadcase bhomLoadCase = new Loadcase
            {
                Name = delimitted[0].Replace(" ", ""),
                Nature = nature
            };

            return bhomLoadCase;
        }

        public static void midasLoadNatureConverter(string midasNature, ref LoadNature nature)
        {
            Dictionary<string, LoadNature> converter = new Dictionary<string, LoadNature>
        {
            {"D", LoadNature.Dead},
            {"L", LoadNature.Live},
            {"W", LoadNature.Wind},
            {"T", LoadNature.Temperature},
            {"DC", LoadNature.SuperDead},
            {"DW", LoadNature.SuperDead},
            {"PL", LoadNature.SuperDead},
            {"BL", LoadNature.SuperDead},
            {"PS", LoadNature.Prestress},
            {"S", LoadNature.Snow}
        };

            converter.TryGetValue(midasNature, out nature);
        }
    }

}

