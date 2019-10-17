using BH.oM.Structure.SurfaceProperties;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static ISurfaceProperty ToBHoMSurfaceProperty(this string surfaceProperty, string version)
        {
            string[] split = surfaceProperty.Split(',');

            ISurfaceProperty constantThickness = null;

            switch(version)
            {
                case "8.8.5":
                    constantThickness = Engine.Structure.Create.ConstantThickness(System.Convert.ToDouble(split[4].Trim()),null,split[1]);
                    break;
                default:
                    constantThickness = Engine.Structure.Create.ConstantThickness(System.Convert.ToDouble(split[3].Trim()));
                    constantThickness.Name = "t = " + split[3].Trim();
                    break;
            }

            constantThickness.CustomData[AdapterId] = split[0].Trim();

            if (split[5].Trim() == "YES")
                Engine.Reflection.Compute.RecordWarning("SurfaceProperty objects do not have offsets implemented so this information will be lost");

            return constantThickness;
        }

    }
}
