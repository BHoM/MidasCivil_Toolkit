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
                        bhomMaterial = (IMaterialFragment)Engine.Structure.Create.Steel(
                             name,
                             double.Parse(delimited[10].Trim()),
                             double.Parse(delimited[11].Trim()),
                             double.Parse(delimited[12].Trim()),
                             density,
                             double.Parse(delimited[8].Trim()), 0, 0
                         );
                        Engine.Reflection.Compute.RecordWarning("Material " + name + " is a USER defined material and will default to a steel material");
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
