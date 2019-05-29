using System;
using System.Collections;
using System.Collections.Generic;
using BH.oM.Base;
using BH.oM.Structure.Elements;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Structure.Loads;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        /***************************************************/
        /**** Adapter overload method                   ****/
        /***************************************************/

        protected override IEnumerable<IBHoMObject> Read(Type type, IList ids = null)
        {
            //Choose what to pull out depending on the type. Also see example methods below for pulling out bars and dependencies
            if (type == typeof(Node))
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

            return null;
        }

    }
}
