/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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

using System;
using System.Collections;
using System.Collections.Generic;
using BH.oM.Adapter;
using BH.oM.Analytical.Results;
using BH.oM.Base;
using BH.oM.Structure.Elements;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Results;
using BH.oM.Adapters.MidasCivil;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        /***************************************************/
        /**** Adapter overload method                   ****/
        /***************************************************/

        protected override IEnumerable<IBHoMObject> IRead(Type type, IList ids = null, ActionConfig actionConfig = null)
        {
            //Choose what to pull out depending on the type. Also see example methods below for pulling out bars and dependencies
            if (type == typeof(Metadata))
                return ReadMetadata(ids as dynamic);
            else if (type == typeof(Node))
                return ReadNodes(ids as dynamic);
            else if (type == typeof(Bar))
                return ReadBars(ids as dynamic);
            else if (type == typeof(FEMesh))
                return ReadFEMeshes(ids as dynamic);
            else if (typeof(ISectionProperty).IsAssignableFrom(type))
                return ReadSectionProperties(ids as dynamic);
            else if (typeof(ISurfaceProperty).IsAssignableFrom(type))
                return ReadSurfaceProperties(ids as dynamic);
            else if (typeof(IMaterialFragment).IsAssignableFrom(type))
                return ReadMaterials(ids as dynamic);
            else if (type == typeof(Constraint6DOF))
                return Read6DOFConstraints(ids as dynamic);
            else if (type == typeof(RigidLink))
                return ReadRigidLinks(ids as dynamic);
            else if (type == typeof(BarRelease))
                return ReadBarReleases(ids as dynamic);
            else if (type == typeof(Loadcase))
                return ReadLoadcases(ids as dynamic);
            else if (type == typeof(LoadCombination))
                return ReadLoadCombinations(ids as dynamic);
            else if (typeof(ILoad).IsAssignableFrom(type))
                return ReadLoad(type, ids as dynamic);
            else if (typeof(IResult).IsAssignableFrom(type))
                Modules.Structure.ErrorMessages.ReadResultsError(type);

            return null;
        }

        /***************************************************/

    }
}





