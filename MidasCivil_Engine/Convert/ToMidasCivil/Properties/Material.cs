using System.Collections.Generic;
using BH.oM.Physical.Materials;
using BH.oM.Structure.MaterialFragments;
using BH.Engine.Structure;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCMaterial(this Material material, List<string> units)
        {
            string youngsModulusUnit = "";
            string coeffThermalExpansionUnit = "";
            string densityUnit = "";

            if(units.Count > 0)
            {
                youngsModulusUnit = units[0];
                coeffThermalExpansionUnit = units[1];
                densityUnit = units[2];
            }

            string type = "";
            if(!(material.MaterialType() == MaterialType.Steel || material.MaterialType() == MaterialType.Concrete))
            {
                type = "USER";
            }
            else if(material.MaterialType() == MaterialType.Steel)
            {
                type = "STEEL";
            }
            else if(material.MaterialType() == MaterialType.Concrete)
            {
                type = "CONCRETE";
            }

            string midasMaterial = (
                    material.CustomData[AdapterId].ToString() +"," + type + "," +
                    material.Name + ",0,0,,C,NO," +
                    material.DampingRatio() + ",2," + Engine.MidasCivil.Convert.Unit("kN/m2", youngsModulusUnit, material.YoungsModulusIsotropic()) + "," +
                    material.PoissonsRatio() + "," + Engine.MidasCivil.Convert.Unit("kN/m2", coeffThermalExpansionUnit, material.ThermalExpansionCoeffIsotropic()) + "," +
                    Engine.MidasCivil.Convert.Unit("kN/m2", densityUnit, material.Density) + "," + "0"
                );

            material.CustomData[AdapterId].ToString();

            return midasMaterial;
        }
    }
}