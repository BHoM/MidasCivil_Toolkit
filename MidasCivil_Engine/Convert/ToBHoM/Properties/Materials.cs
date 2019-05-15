using BH.oM.Structure.MaterialFragments;
using System.Linq;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static IMaterialFragment ToBHoMMaterial(this string material)
        {
            string[] delimited = material.Split(',');
            string type = delimited[1].Replace(" ", "");
            string name = delimited[2].Replace(" ", "");
            IMaterialFragment bhomMaterial = null;

            if (delimited.Count() == 15)
            {
                switch (type)
                {
                    case "USER":
                        bhomMaterial = (IMaterialFragment)Engine.Structure.Create.Steel(
                            name,
                            double.Parse(delimited[10].Replace(" ", "")),
                            double.Parse(delimited[11].Replace(" ", "")),
                            double.Parse(delimited[12].Replace(" ", "")),
                            double.Parse(delimited[13].Replace(" ", "")),
                            double.Parse(delimited[8].Replace(" ", "")), 0, 0
                            );
                        Engine.Reflection.Compute.RecordWarning("Material " + name + " is a USER defined material and will default to a steel material");

                        break;
                    case "STEEL":
                        bhomMaterial = (IMaterialFragment)Engine.Structure.Create.Steel(
                            name,
                            double.Parse(delimited[10].Replace(" ", "")),
                            double.Parse(delimited[11].Replace(" ", "")),
                            double.Parse(delimited[12].Replace(" ", "")),
                            double.Parse(delimited[13].Replace(" ", "")),
                            double.Parse(delimited[8].Replace(" ", "")), 0, 0
                            );
                        break;

                    case "CONC":
                        bhomMaterial = (IMaterialFragment)Engine.Structure.Create.Concrete(
                            name,
                            double.Parse(delimited[10].Replace(" ", "")),
                            double.Parse(delimited[11].Replace(" ", "")),
                            double.Parse(delimited[12].Replace(" ", "")),
                            double.Parse(delimited[13].Replace(" ", "")),
                            double.Parse(delimited[8].Replace(" ", "")), 0, 0
                            );
                        break;

                    case "SRC":
                        Reflection.Compute.RecordError("BHoM does not support Reinforced Concrete Sections");
                        break;
                }
            }
            else
            {
                bhomMaterial = (IMaterialFragment)BH.Engine.Library.Query.Match("Materials", name);

                if (bhomMaterial == null)
                {
                    //switch (type)
                    //{
                    //    case "STEEL":
                    //        Reflection.Compute.RecordWarning("Material not found in BHoM Library: S355 Steel properties assumed");
                    //        bhomMaterial = (IMaterialFragment) BH.Engine.Library.Query.Match("Materials", "S355");
                    //        break;

                    //    case "CONC":
                    //        Reflection.Compute.RecordWarning("Material not found in BHoM Library: C30/37 Concrete properties assumed");
                    //        bhomMaterial = (IMaterialFragment)BH.Engine.Library.Query.Match("Materials", "C30/37");
                    //        break;

                    //    case "SRC":
                    //        Reflection.Compute.RecordError("BHoM does not support Reinforced Concrete Sections");
                    //        break;
                    //}
                }
            }

            bhomMaterial.CustomData[AdapterId] = delimited[0].Replace(" ", "");
            return bhomMaterial;

        }
    }
}
