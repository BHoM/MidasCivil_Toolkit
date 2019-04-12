using System;
using System.Collections.Generic;
using BH.oM.Common.Materials;
using System.IO;

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
            if(!(material.Type == MaterialType.Steel || material.Type == MaterialType.Concrete))
            {
                type = "USER";
            }
            else if(material.Type == MaterialType.Steel)
            {
                type = "STEEL";
            }
            else if(material.Type == MaterialType.Concrete)
            {
                type = "CONCRETE";
            }

            string midasMaterial = (
                    material.CustomData[AdapterId].ToString() + "," + type + "," +
                    material.Name + ",0,0,,C,NO," +
                    material.DampingRatio + ",2," + Engine.MidasCivil.Convert.Unit("kN/m2",youngsModulusUnit,material.YoungsModulus) + "," +
                    material.PoissonsRatio + "," + Engine.MidasCivil.Convert.Unit("kN/m2", coeffThermalExpansionUnit, material.CoeffThermalExpansion) + "," +
                    Engine.MidasCivil.Convert.Unit("kN/m2", densityUnit, material.Density) + "," + "0"
                );

            return midasMaterial;
        }
    }
}