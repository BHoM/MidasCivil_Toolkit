using BH.oM.Structure.Elements;
using BH.oM.Structure.Loads;
using BH.oM.Structure.Properties.Surface;
using BH.oM.Structure.Properties.Section;
using BH.oM.Structure.Properties.Constraint;
using BH.oM.Common.Materials;
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
            if (type == typeof(Material))
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
            if (type == typeof(PointForce))
                success = DeletePointForces(ids);
            if (type == typeof(GravityLoad))
                success = DeleteGravityLoads(ids);
            if (type == typeof(BarUniformlyDistributedLoad))
                success = DeleteBarUniformlyDistributedLoads(ids);
            if (type == typeof(BarVaryingDistributedLoad))
                success = DeleteBarVaryingDistributedLoads(ids);
            if (type == typeof(BarPointLoad))
                success = DeleteBarPointLoads(ids);
            if (type == typeof(AreaUniformalyDistributedLoad))
                success = DeleteAreaUniformlyDistributedLoads(ids);

            return 0;
        }

        /***************************************************/
    }
}
