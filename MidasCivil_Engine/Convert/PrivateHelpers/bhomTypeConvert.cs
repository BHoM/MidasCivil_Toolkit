using System.Collections.Generic;
using System.Linq;

namespace BH.Engine.MidasCivil
{
    public partial class Convert
    {
        public static string bhomTypeConvert(string type)
        {
            string midasVersion;

            string[] delimited = type.Split('.');
            type = delimited[delimited.Count() - 1];

            Dictionary<string, string> conversion = new Dictionary<string, string>
            {
                {"Node", "NODE" },
                {"Bar", "ELEMENT" },
                {"FEMesh", "ELEMENT" },
                {"Constraint6DOF", "CONSTRAINT" },
                {"Material", "MATERIAL" },
                {"SteelSection", "SECTION" },
                {"ConcreteSection", "SECTION" },
                {"ConstantThickness", "THICKNESS" },
                {"Loadcase", "LOADCASE" },
            };

            conversion.TryGetValue(type, out midasVersion);
            return midasVersion;
        }
    }
}
