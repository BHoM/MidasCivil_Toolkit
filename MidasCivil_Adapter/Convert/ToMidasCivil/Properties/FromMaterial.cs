/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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

using BH.oM.Adapters.MidasCivil;
using BH.Engine.Adapter;
using BH.oM.Structure.MaterialFragments;
using BH.Engine.Structure;
using System.Linq;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static string FromMaterial(this IMaterialFragment material, string forceUnit, string lengthUnit, string temperatureUnit, int materialCharacterLimit)
        {
            string type = "";
            if (!(material.IMaterialType() == MaterialType.Steel || material.IMaterialType() == MaterialType.Concrete))
            {
                type = "USER";
            }
            else if (material.IMaterialType() == MaterialType.Steel)
            {
                type = "STEEL";
            }
            else if (material.IMaterialType() == MaterialType.Concrete)
            {
                type = "CONC";
            }

            string midasMaterial = "";

            if (material is IIsotropic)
            {
                IIsotropic isotropic = material as IIsotropic;
                midasMaterial = (
                    isotropic.AdapterId<string>(typeof(MidasCivilId)) + "," + type + "," +
                    new string(isotropic.DescriptionOrName().Replace(",", "").Take(materialCharacterLimit).ToArray()) + ",0,0,,C,NO," +
                    isotropic.DampingRatio + ",2," + isotropic.YoungsModulus.PressureFromSI(forceUnit, lengthUnit) + "," +
                    isotropic.PoissonsRatio + "," + isotropic.ThermalExpansionCoeff.InverseDeltaTemperatureFromSI(temperatureUnit) + "," +
                    isotropic.Density.DensityFromSI(forceUnit, lengthUnit) * 9.806 + "," + isotropic.Density.DensityFromSI(forceUnit, lengthUnit)
                );
            }
            else if (material is IOrthotropic)
            {
                IOrthotropic iorthotropic = material as IOrthotropic;
                midasMaterial = (
                     iorthotropic.AdapterId<string>(typeof(MidasCivilId)) + "," + type + "," +
                    new string(iorthotropic.DescriptionOrName().Replace(",", "").Take(materialCharacterLimit).ToArray()) + ",0,0,,C,NO," +
                    iorthotropic.DampingRatio + ",3,"
                    + iorthotropic.YoungsModulus.X.PressureFromSI(forceUnit, lengthUnit) + "," +
                        iorthotropic.YoungsModulus.Y.PressureFromSI(forceUnit, lengthUnit) + "," +
                        iorthotropic.YoungsModulus.Z.PressureFromSI(forceUnit, lengthUnit) + ","
                    + iorthotropic.ThermalExpansionCoeff.X.InverseDeltaTemperatureFromSI(temperatureUnit) + "," +
                        iorthotropic.ThermalExpansionCoeff.Y.InverseDeltaTemperatureFromSI(temperatureUnit) + "," +
                        iorthotropic.ThermalExpansionCoeff.Z.InverseDeltaTemperatureFromSI(temperatureUnit) + ","
                    + iorthotropic.ShearModulus.X.PressureFromSI(forceUnit, lengthUnit) + "," +
                        iorthotropic.ShearModulus.Y.PressureFromSI(forceUnit, lengthUnit) + "," +
                        iorthotropic.ShearModulus.Z.PressureFromSI(forceUnit, lengthUnit) + ","
                    + iorthotropic.PoissonsRatio.X + "," + iorthotropic.PoissonsRatio.Y + "," + iorthotropic.PoissonsRatio.Z + ","
                    + iorthotropic.Density.DensityFromSI(forceUnit, lengthUnit) * 9.806 + "," + iorthotropic.Density.DensityFromSI(forceUnit, lengthUnit)
                );
            }
            return midasMaterial;
        }

        /***************************************************/

    }
}
