using BH.oM.Structure.SurfaceProperties;
using BH.Engine.Structure;
using System;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static ISurfaceProperty ToBHoMSurfaceProperty(this string surfaceProperty)
        {
            string[] split = surfaceProperty.Split(',');

            ISurfaceProperty constantThickness = Engine.Structure.Create.ConstantThickness(System.Convert.ToDouble(split[3].Trim()));

            constantThickness.Name = "t = " + split[3].Trim();
            constantThickness.CustomData[AdapterId] = split[0].Trim();

            if (split[5].Trim() == "YES")
                Engine.Reflection.Compute.RecordWarning("SurfaceProperty objects do not have offsets implemented so this information will be lost");

            return constantThickness;
        }

    }
}
