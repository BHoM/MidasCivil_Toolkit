using System;
using System.Collections.Generic;
using BH.oM.Common.Materials;
using System.IO;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static void ToMCMaterial(this Material material, List<string> units, string path)
        {
            using (StreamWriter materialText = File.AppendText(path))
            {
                materialText.WriteLine(
                    material.CustomData[AdapterId].ToString() + "," + "USER," +
                    material.Type.ToString() + " " + material.Name + ",0,0,,C,NO," +
                    material.DampingRatio + ",2," + unitConverter("kN/m2",units[0],material.YoungsModulus) + "," +
                    material.PoissonsRatio + "," + unitConverter("kN/m2", units[0], material.CoeffThermalExpansion) + "," +
                    unitConverter("kN/m2", units[0], material.Density) + "," + "0"
                );
            }
        }
    }
}