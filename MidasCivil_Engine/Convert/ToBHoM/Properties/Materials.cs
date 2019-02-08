using System;
using BH.oM.Common.Materials;
using System.Collections.Generic;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static Material ToBHoMMaterial(this string material)
        {
            string[] delimited = material.Split(',');
            string type = delimited[1].Replace(" ", "");
            string name = delimited[2].Replace(" ", "");
            Material bhomMaterial = null;

            if (type == "USER")
            {
                bhomMaterial = new Material
                {
                    Name = name,
                    YoungsModulus = double.Parse(delimited[10].Replace(" ", "")),
                    PoissonsRatio = double.Parse(delimited[11].Replace(" ", "")),
                    CoeffThermalExpansion = double.Parse(delimited[12].Replace(" ", "")),
                    Density = double.Parse(delimited[13].Replace(" ", "")),
                    DampingRatio = double.Parse(delimited[8].Replace(" ", ""))
                };
            }
            else
            {
                bhomMaterial = (Material)BH.Engine.Library.Query.Match("Materials", name);

                if (bhomMaterial == null)
                {
                    switch (type)
                    {
                        case "STEEL":
                            Reflection.Compute.RecordWarning("Material not found in BHoM Library: S355 Steel properties assumed");
                            bhomMaterial = (Material) BH.Engine.Library.Query.Match("Materials", "S355");
                            break;

                        case "CONC":
                            Reflection.Compute.RecordWarning("Material not found in BHoM Library: C30/37 Concrete properties assumed");
                            bhomMaterial = (Material)BH.Engine.Library.Query.Match("Materials", "C30/37");
                            break;

                        case "SRC":
                            Reflection.Compute.RecordError("BHoM does not support Reinforced Concrete Sections");
                            break;
                    }
                }
            }

            bhomMaterial.CustomData[AdapterId] = delimited[0].Replace(" ", "");
            return bhomMaterial;

        }
    }
}
