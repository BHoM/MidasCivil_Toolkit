using System.Collections.Generic;
using BH.oM.Structure.MaterialFragments;
using BH.Engine.Structure;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCMaterial(this IMaterialFragment material, List<string> units)
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
                type = "CONCRETE";
            }

            string midasMaterial = "";

            if(material is IIsotropic)
            {
                IIsotropic isotropic = material as IIsotropic;
                midasMaterial = (
                    isotropic.CustomData[AdapterId].ToString() + "," + type + "," +
                    isotropic.Name + ",0,0,,C,NO," +
                    isotropic.DampingRatio + ",2," + Engine.MidasCivil.Convert.Unit("kN/m2", youngsModulusUnit, isotropic.YoungsModulus) + "," +
                    isotropic.PoissonsRatio + "," + Engine.MidasCivil.Convert.Unit("kN/m2", coeffThermalExpansionUnit, isotropic.ThermalExpansionCoeff) + "," +
                    Engine.MidasCivil.Convert.Unit("kN/m2", densityUnit, isotropic.Density) + "," + "0"
                );
            }
            else
            {
                Engine.Reflection.Compute.RecordWarning("MidasCivil_Toolkit currently suports Isotropic material only. No structural properties for material with name " + material.Name + " have been pushed");
                return null; ;
            }


            material.CustomData[AdapterId].ToString();

            return midasMaterial;
        }
    }
}