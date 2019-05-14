using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.MaterialFragments;
using System;
using System.Collections.Generic;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        /***************************************************/
        /**** Adapter overload method                   ****/
        /***************************************************/

        protected override int Delete(Type type, IEnumerable<object> ids)
        {
            int success = 0;

            if (type == typeof(Node))
                success = DeleteNodes(ids);
            if (type == typeof(Constraint6DOF))
                success = DeleteConstraints(ids);
            if (type == typeof(RigidLink))
                success = DeleteRigidLinks(ids);
            if (type == typeof(BarRelease))
                success = DeleteBarReleases(ids);
            if (type == typeof(IMaterialFragment))
                success = DeleteMaterials(ids);
            if (type == typeof(Bar))
                success = DeleteElements(ids);
            if (type == typeof(Loadcase))
                success = DeleteLoadcases(ids);
            if (type == typeof(LoadCombination))
                success = DeleteLoadCombinations(ids);
            if (type == typeof(ConstantThickness))
                success = DeleteSurfaceProperties(ids);
            if (type == typeof(SteelSection))
                success = DeleteSectionProperties(ids);
            if (type == typeof(PointLoad))
                success = DeletePointLoads(ids);
            if (type == typeof(GravityLoad))
                success = DeleteGravityLoads(ids);
            if (type == typeof(BarUniformlyDistributedLoad))
                success = DeleteBarUniformlyDistributedLoads(ids);
            if (type == typeof(BarVaryingDistributedLoad))
                success = DeleteBarVaryingDistributedLoads(ids);
            if (type == typeof(BarPointLoad))
                success = DeleteBarPointLoads(ids);
            if (type == typeof(AreaUniformlyDistributedLoad))
                success = DeleteAreaUniformlyDistributedLoads(ids);
            if (type == typeof(AreaTemperatureLoad))
                success = DeleteAreaTemperatureLoads(ids);
            if (type == typeof(BarTemperatureLoad))
                success = DeleteBarTemperatureLoads(ids);

            return 0;
        }

        /***************************************************/
    }
}
