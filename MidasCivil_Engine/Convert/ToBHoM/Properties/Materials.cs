
using BH.oM.Structure.MaterialFragments;
using BH.oM.Geometry;
using System.Linq;


namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static IMaterialFragment ToBHoMMaterial(this string material)
        {
            string[] delimited = material.Split(',');
            string type = delimited[1].Trim();
            string name = delimited[2].Trim();
            string tropic = delimited[9].Trim();
            IMaterialFragment bhomMaterial = null;

            bhomMaterial = (IMaterialFragment)BH.Engine.Library.Query.Match("Materials", name);

            double density = 0;

            if (delimited.Count() == 15)
            {
                density = double.Parse(delimited[14].Trim());
                if (double.Parse(delimited[14].Trim()) == 0)
                {
                    density = double.Parse(delimited[13].Trim()) / 9.806;
                }
            }


          
            if (bhomMaterial == null)
            {
                switch (type)
                {
                    case "USER":
                        if ((delimited[9].Trim()) == "2")

                        {


                            bhomMaterial = new GenericIsotropicMaterial()
                            {

                                Name = name,
                                YoungsModulus = double.Parse(delimited[10].Trim()),
                                PoissonsRatio = double.Parse(delimited[11].Trim()),
                                ThermalExpansionCoeff = double.Parse(delimited[12].Trim()),
                                Density = density,
                                DampingRatio = double.Parse(delimited[8].Trim())
                            };
                                    Engine.Reflection.Compute.RecordWarning("Material " + name + " is a USER defined material and will default to a Generic Isotropic material");
                        }
                        else if ((delimited[9].Trim()) == "3")
                            bhomMaterial = new GenericOrthotropicMaterial()
                            {
                                Name = name,
                                YoungsModulus = new Vector() { X = double.Parse(delimited[11].Trim()), Y = double.Parse(delimited[12].Trim()), Z = double.Parse(delimited[13].Trim()) },
                                PoissonsRatio = new Vector() { X = double.Parse(delimited[20].Trim()), Y = double.Parse(delimited[21].Trim()), Z = double.Parse(delimited[22].Trim()) },
                                ThermalExpansionCoeff = new Vector() { X = double.Parse(delimited[14].Trim()), Y = double.Parse(delimited[15].Trim()), Z = double.Parse(delimited[16].Trim()) },
                                ShearModulus = new Vector() { X = double.Parse(delimited[17].Trim()), Y = double.Parse(delimited[18].Trim()), Z = double.Parse(delimited[19].Trim()) },
                                Density = density,

                                DampingRatio = double.Parse(delimited[8].Trim())

                            };
                        Engine.Reflection.Compute.RecordWarning("Material " + name + " is a USER defined material and will default to a Generic Orthotropic material");





                        break;
                                    
              
                    case "STEEL":
                        if (delimited.Count() == 15)
                        {
                            bhomMaterial = (IMaterialFragment)Engine.Structure.Create.Steel(
                                name,
                                double.Parse(delimited[10].Trim()),
                                double.Parse(delimited[11].Trim()),
                                double.Parse(delimited[12].Trim()),
                                density,
                                double.Parse(delimited[8].Trim()), 0, 0
                            );
                        }
                        else
                        {
                            Reflection.Compute.RecordWarning("Material not found in BHoM Library: S355 Steel properties assumed");
                            bhomMaterial = (IMaterialFragment)BH.Engine.Library.Query.Match("Materials", "S355");
                        }
                        break;

                    case "CONC":
                        if (delimited.Count() == 15)
                        {
                            bhomMaterial = (IMaterialFragment)Engine.Structure.Create.Concrete(
                                name,
                                double.Parse(delimited[10].Trim()),
                                double.Parse(delimited[11].Trim()),
                                double.Parse(delimited[12].Trim()),
                                density,
                                double.Parse(delimited[8].Trim()), 0, 0
                            );

                        }
                        else
                        {
                            Reflection.Compute.RecordWarning("Material not found in BHoM Library: C30/37 Concrete properties assumed");
                            bhomMaterial = (IMaterialFragment)BH.Engine.Library.Query.Match("Materials", "C30/37");
                        }
                        break;

                    case "SRC":
                        Reflection.Compute.RecordError("BHoM does not support Reinforced Concrete Sections");
                        break;
                }
            }

            bhomMaterial.CustomData[AdapterIdName] = delimited[0].Trim();
            return bhomMaterial;
        }
    }
}
