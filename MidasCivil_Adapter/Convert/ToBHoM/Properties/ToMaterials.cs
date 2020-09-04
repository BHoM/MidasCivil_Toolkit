/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */
using BH.oM.Structure.MaterialFragments;
using BH.oM.Geometry;
using System.Linq;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static IMaterialFragment ToMaterial(this string material, string forceUnit, string lengthUnit, string temperatureUnit)
        {
            string[] delimited = material.Split(',');
            string type = delimited[1].Trim();
            string name = delimited[2].Trim();
            IMaterialFragment bhomMaterial = null;

            bhomMaterial = (IMaterialFragment)Engine.Library.Query.Match("Materials", name);

            double density = 0;

            if (delimited.Count() == 15)
            {
                density = double.Parse(delimited[14].Trim()).DensityToSI(forceUnit, lengthUnit);
                if (density == 0)
                {
                    density = (double.Parse(delimited[13].Trim()) / 9.806).DensityToSI(forceUnit, lengthUnit);
                }
            }
            else if (delimited.Count() == 24)
            {
                density = double.Parse(delimited[23].Trim()).DensityToSI(forceUnit, lengthUnit);
                if (density == 0)
                {
                    density = (double.Parse(delimited[22].Trim()) / 9.806).DensityToSI(forceUnit, lengthUnit);
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
                                YoungsModulus = double.Parse(delimited[10].Trim()).PressureToSI(forceUnit, lengthUnit),
                                PoissonsRatio = double.Parse(delimited[11].Trim()),
                                ThermalExpansionCoeff = double.Parse(delimited[12].Trim()).InverseDeltaTemperatureToSI(temperatureUnit),
                                Density = density,
                                DampingRatio = double.Parse(delimited[8].Trim())
                            };
                            Engine.Reflection.Compute.RecordWarning("Material " + name + " is a USER defined material and will default to a Generic Isotropic material");
                        }
                        else if ((delimited[9].Trim()) == "3")
                        {
                            bhomMaterial = new GenericOrthotropicMaterial()
                            {
                                Name = name,
                                YoungsModulus = new Vector()
                                {
                                    X = double.Parse(delimited[10].Trim()).PressureToSI(forceUnit, lengthUnit),
                                    Y = double.Parse(delimited[11].Trim()).PressureToSI(forceUnit, lengthUnit),
                                    Z = double.Parse(delimited[12].Trim()).PressureToSI(forceUnit, lengthUnit)
                                },
                                PoissonsRatio = new Vector() { X = double.Parse(delimited[19].Trim()), Y = double.Parse(delimited[20].Trim()), Z = double.Parse(delimited[21].Trim()) },
                                ThermalExpansionCoeff = new Vector()
                                {
                                    X = double.Parse(delimited[13].Trim()).InverseDeltaTemperatureToSI(temperatureUnit),
                                    Y = double.Parse(delimited[14].Trim()).InverseDeltaTemperatureToSI(temperatureUnit),
                                    Z = double.Parse(delimited[15].Trim()).InverseDeltaTemperatureToSI(temperatureUnit)
                                },
                                ShearModulus = new Vector()
                                {
                                    X = double.Parse(delimited[16].Trim()).PressureToSI(forceUnit, lengthUnit),
                                    Y = double.Parse(delimited[17].Trim()).PressureToSI(forceUnit, lengthUnit),
                                    Z = double.Parse(delimited[18].Trim()).PressureToSI(forceUnit, lengthUnit)
                                },
                                Density = density,

                                DampingRatio = double.Parse(delimited[8].Trim())

                            };
                            Engine.Reflection.Compute.RecordWarning("Material " + name + " is a USER defined material and will default to a Generic Orthotropic material");
                        }
                        break;
                    case "STEEL":
                        if (delimited.Count() == 15)
                        {
                            bhomMaterial = (IMaterialFragment)Engine.Structure.Create.Steel(
                                name,
                                double.Parse(delimited[10].Trim()).PressureToSI(forceUnit, lengthUnit),
                                double.Parse(delimited[11].Trim()),
                                double.Parse(delimited[12].Trim()).InverseDeltaTemperatureToSI(temperatureUnit),
                                density,
                                double.Parse(delimited[8].Trim()), 0, 0
                            );
                        }
                        else
                        {
                            Engine.Reflection.Compute.RecordWarning("Material not found in BHoM Library: S355 Steel properties assumed");
                            bhomMaterial = (IMaterialFragment)BH.Engine.Library.Query.Match("Materials", "S355");
                        }
                        break;

                    case "CONC":
                        if (delimited.Count() == 15)
                        {
                            bhomMaterial = (IMaterialFragment)Engine.Structure.Create.Concrete(
                                name,
                                double.Parse(delimited[10].Trim()).PressureToSI(forceUnit, lengthUnit),
                                double.Parse(delimited[11].Trim()),
                                double.Parse(delimited[12].Trim()).InverseDeltaTemperatureToSI(temperatureUnit),
                                density,
                                double.Parse(delimited[8].Trim()), 0, 0
                            );

                        }
                        else
                        {
                            Engine.Reflection.Compute.RecordWarning("Material not found in BHoM Library.");
                        }
                        break;
                    case "SRC":
                        Engine.Reflection.Compute.RecordError("BHoM does not support Reinforced Concrete Sections");
                        break;
                }
            }

            bhomMaterial.CustomData[AdapterIdName] = delimited[0].Trim();
            return bhomMaterial;
        }

        /***************************************************/

    }
}

