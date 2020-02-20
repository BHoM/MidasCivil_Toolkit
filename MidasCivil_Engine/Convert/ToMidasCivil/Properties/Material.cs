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
                    iorthotropic.DampingRatio + ",3," 
                    + iorthotropic.YoungsModulus.X + ","+ iorthotropic.YoungsModulus.Y + "," + iorthotropic.YoungsModulus.Z + ","
                    + iorthotropic.ThermalExpansionCoeff.X + "," + iorthotropic.ThermalExpansionCoeff.Y + "," + iorthotropic.ThermalExpansionCoeff.Z + ","
                    + iorthotropic.ShearModulus.X + "," + iorthotropic.ShearModulus.Y + "," + iorthotropic.ShearModulus.Z + ","
                    + iorthotropic.PoissonsRatio.X + "," + iorthotropic.PoissonsRatio.Y + "," + iorthotropic.PoissonsRatio.Z + ","
                    + iorthotropic.Density * 9.806 + "," + iorthotropic.Density
                );

            }

            return midasMaterial;
        }
    }
}