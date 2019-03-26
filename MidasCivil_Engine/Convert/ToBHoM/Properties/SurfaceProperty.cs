using BH.oM.Structure.Properties.Surface;
using BH.Engine.Structure;
using System;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static ISurfaceProperty ToBHoMSurfaceProperty(this string surfaceProperty)
        {
            string[] split = surfaceProperty.Split(',');

            ISurfaceProperty constantThickness = Engine.Structure.Create.ConstantThickness(System.Convert.ToDouble(split[3]));

            constantThickness.Name = "t = " + split[3];
            constantThickness.CustomData[AdapterId] = System.Convert.ToInt32(split[0]);

            Engine.Reflection.Compute.RecordWarning("SurfaceProperty objects do not have offsets implemented so this information will be lost");

            return constantThickness;
        }

    }
}
