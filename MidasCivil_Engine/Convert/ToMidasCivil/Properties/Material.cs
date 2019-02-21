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
            string midasMaterial = (
                    material.CustomData[AdapterId].ToString() + "," + "USER," +
                    material.Type.ToString() + " " + material.Name + ",0,0,,C,NO," +
                    material.DampingRatio + ",2," + Engine.MidasCivil.Convert.Unit("kN/m2",units[0],material.YoungsModulus) + "," +
                    material.PoissonsRatio + "," + Engine.MidasCivil.Convert.Unit("kN/m2", units[0], material.CoeffThermalExpansion) + "," +
                    Engine.MidasCivil.Convert.Unit("kN/m2", units[0], material.Density) + "," + "0"
                );

            return midasMaterial;
        }
    }
}