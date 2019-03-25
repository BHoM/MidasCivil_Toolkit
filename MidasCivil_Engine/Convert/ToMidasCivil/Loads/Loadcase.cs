using BH.oM.Structure.Loads;
using System.Collections.Generic;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCLoadCase(this Loadcase loadcase)
        {
            LoadNature bhomNature = loadcase.Nature;
            string midasNature = "D";
            bhomLoadNatureConverter(bhomNature, ref midasNature);

            string midasLoadcase = loadcase.Name + " " + "," + midasNature + ",";

            return midasLoadcase;
        }

        private static void bhomLoadNatureConverter(LoadNature bhomNature, ref string nature)
        {
            Dictionary<LoadNature, string> converter = new Dictionary<LoadNature, string>
        {
            {LoadNature.Dead,"D"},
            {LoadNature.Live,"L"},
            {LoadNature.Wind,"W"},
            {LoadNature.Temperature,"T"},
            {LoadNature.SuperDead,"DC"},
            {LoadNature.Prestress,"PS"},
            {LoadNature.Snow,"S"},
            {LoadNature.Seismic,"E"},
            {LoadNature.Accidental,"CO"},
            {LoadNature.Notional,"USER"},
            {LoadNature.Other,"USER"}
        };
            converter.TryGetValue(bhomNature, out nature);
        }
    }
}