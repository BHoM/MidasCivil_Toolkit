using BH.oM.Structure.Loads;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCBarTemperatureLoad(this BarTemperatureLoad barLoad, string assignedBar)
        {
            string midasBarLoad = null;

            midasBarLoad = assignedBar + "," + barLoad.TemperatureChange.ToString() + "," + barLoad.Name;

            return midasBarLoad;
        }
    }
}