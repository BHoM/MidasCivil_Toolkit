namespace BH.Engine.MidasCivil
{
    public partial class Convert
    {
        public static double Unit(string startUnit, string endUnit, double value)
        {
            double converted = value;

            switch (startUnit)
            {
                case "N/m2":
                    if (endUnit == "kN/m2")
                        converted = converted / 1000;
                    if (endUnit == "N/mm2")
                        converted = converted / 1000000;
                    break;

                case "kN/m2":
                    if (endUnit == "N/m2")
                        converted = converted * 1000;
                    if (endUnit == "kN/mm2")
                        converted = converted / 1000000;
                    break;

                case "N/mm2":
                    if (endUnit == "kN/mm2")
                        converted = converted / 1000;
                    if (endUnit == "N/m2")
                        converted = converted * 1000000;
                    break;

                case "1/C":
                    if (endUnit == "1/F")
                        converted = (converted * 1.8) + 32;
                    break;

                case "1/F":
                    if (endUnit == "1/C")
                        converted = (converted - 32) / 1.8;
                    break;

                case "t/m3":
                    if (endUnit == "kN/m3")
                        converted = converted*9.81;
                    if (endUnit == "kN/m3/g")
                        converted = converted * 1;
                    break;

                case "kN/m3":
                    if (endUnit == "t/m3")
                        converted = converted / 9.81;
                    break;
            }
            return converted;
        }
    }
}
