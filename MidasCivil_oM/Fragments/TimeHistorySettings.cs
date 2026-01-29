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

using BH.oM.Base;
using System.ComponentModel;
using System.Collections.Generic;

namespace BH.oM.Adapters.MidasCivil
{
    public class TimeHistorySettings : BHoMObject, IFragment
    {
        /***************************************************/
        /**** Public Properties                         ****/
        /***************************************************/

        [Description("Indicates whether the time history analysis is performed linear or nonlinear.")]
        public virtual LinearType LinearType { get; set; }
        [Description("Indicates how Midas performs the time history calculation.")]
        public virtual IntegrationMethod IntegrationMethod { get; set; }
        [Description("Specifies how the damping matrix is calculated in the analysis.")]
        public virtual DampingMethod DampingMethod { get; set; }
        [Description("Describes the type of time‑dependent loading used in the analysis.")]
        public virtual TimeHistoryType TimeHistoryType { get; set; }
        [Description("Defines how the starting state for the time history analysis is determined. \n" +
                    "It can either begin from the model’s initial, unloaded state (InitialLoad) \n" +
                    "or continue from the final state of another load case (SequentialOrder).")]
        public virtual DynamicLoadType DynamicLoadType { get; set; }

        /***************************************************/
    }
}





