/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2026, the respective contributors. All rights reserved.
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
using BH.Engine.Adapter;
using BH.oM.Adapters.MidasCivil;
using BH.oM.Base;

namespace BH.Adapter.Adapters.MidasCivil
{
    public static partial class Convert
    {
        public static IFragment ToTimeHistorySettings(Dictionary<string, object> common)
        {
            /***************************************************/
            /**** Public Methods                            ****/
            /***************************************************/
            LinearType LinearType = new LinearType();
            switch (common["iATYPE"]?.ToString())
            {
                case "1":
                    LinearType = LinearType.Linear;
                    break;
                case "2":
                    LinearType = LinearType.NonLinear;
                    break;
            }

            IntegrationMethod integrationMethod = new IntegrationMethod();
            switch (common["iAMETHOD"]?.ToString())
            {
                case "1":
                    integrationMethod = IntegrationMethod.Modal;
                    break;
                case "2":
                    integrationMethod = IntegrationMethod.DirectIntegration;
                    break;
                case "3":
                    integrationMethod = IntegrationMethod.Static;
                    break;
            }

            DampingMethod dampingMethod = new DampingMethod();
            switch (common["iMDTYPE"]?.ToString())
            {
                case "1":
                    dampingMethod = DampingMethod.Modal;
                    break;
                case "2":
                    dampingMethod = DampingMethod.MassStiffnessProportional;
                    break;
                case "3":
                    dampingMethod = DampingMethod.StrainEnergyProportional;
                    break;
                case "4":
                    dampingMethod = DampingMethod.Rayleigh;
                    break;
            }

            TimeHistoryType timeHistoryType = new TimeHistoryType();
            switch (common["iTHTYPE"]?.ToString())
            {
                case "1":
                    timeHistoryType = TimeHistoryType.Transient;
                    break;
                case "2":
                    timeHistoryType = TimeHistoryType.Periodic;
                    break;
            }
            DynamicLoadType dynamicLoadType = new DynamicLoadType();
            switch (common["INITMETHOD"]?.ToString())
            {
                case "1":
                    dynamicLoadType = DynamicLoadType.InitialLoad;
                    break;
                case "2":
                    dynamicLoadType = DynamicLoadType.SequentialLoad;
                    break;
            }

            TimeHistorySettings timeHistorySettings = new TimeHistorySettings
            {
                LinearType = LinearType,
                IntegrationMethod = integrationMethod,
                DampingMethod = dampingMethod,
                TimeHistoryType = timeHistoryType,
                DynamicLoadType = dynamicLoadType
            };
            return timeHistorySettings;
        }

        /***************************************************/

    }
}






