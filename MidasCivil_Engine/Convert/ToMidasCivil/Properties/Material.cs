using System.Collections.Generic;
using BH.oM.Structure.MaterialFragments;
using BH.Engine.Structure;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCMaterial(this IMaterialFragment material)
        {
            string type = "";
            if(!(material.IMaterialType() == MaterialType.Steel || material.IMaterialType() == MaterialType.Concrete))
            {
                type = "USER";
            }
            else if(material.IMaterialType() == MaterialType.Steel)
            {
                type = "STEEL";
            }
            else if(material.IMaterialType() == MaterialType.Concrete)
            {
                type = "CONC";
            }

            string midasMaterial = "";

            if(material is IIsotropic)
            {
                IIsotropic isotropic = material as IIsotropic;
                midasMaterial = (
                    isotropic.CustomData[AdapterIdName].ToString() + "," + type + "," +
                    isotropic.Name + ",0,0,,C,NO," +
                    isotropic.DampingRatio + ",2," + isotropic.YoungsModulus + "," +
                    isotropic.PoissonsRatio + "," + isotropic.ThermalExpansionCoeff + "," +
                    isotropic.Density*9.806 + "," + isotropic.Density
                );
            }
            else if(material is IOrthotropic)
            {
                IOrthotropic iorthotropic = material as IOrthotropic;
                midasMaterial = (
                     iorthotropic.CustomData[AdapterIdName].ToString() + "," + type + "," +
                    iorthotropic.Name + ",0,0,,C,NO," +
                    iorthotropic.DampingRatio + ",2," + iorthotropic.YoungsModulus + "," +
                    iorthotropic.PoissonsRatio + "," + iorthotropic.ThermalExpansionCoeff + "," +
                    iorthotropic.Density * 9.806 + "," + iorthotropic.Density
                );

            }

            //material.CustomData[AdapterIdName].ToString();

            return midasMaterial;
        }
    }
}