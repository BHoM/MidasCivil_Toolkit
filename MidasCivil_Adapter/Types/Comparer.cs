using BH.Engine.Base.Objects;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.Loads;
using BH.Engine.MidasCivil.Comparer;
using System;
using System.Collections.Generic;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        /***************************************************/
        /**** BHoM Adapter Interface                    ****/
        /***************************************************/

        //Standard implementation of the comparer class.
        //Compares nodes by distance (down to 3 decimal places -> mm)
        //Compares Materials, SectionProprties, LinkConstraints, and Property2D by name
        //Add/remove any type in the dictionary below that you want (or not) a specific comparison method for

        protected override IEqualityComparer<T> Comparer<T>()
        {
            Type type = typeof(T);

            if (m_Comparers.ContainsKey(type))
            {
                return m_Comparers[type] as IEqualityComparer<T>;
            }
            else
            {
                return EqualityComparer<T>.Default;
            }

        }

        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private static Dictionary<Type, object> m_Comparers = new Dictionary<Type, object>
        {
            {typeof(Node), new BH.Engine.Structure.NodeDistanceComparer(3) },   //The 3 in here sets how many decimal places to look at for node merging. 3 decimal places gives mm precision
            {typeof(Bar), new BarMidPointComparer(3) },
            {typeof(FEMesh), new MeshCentreComparer() },
            {typeof(Constraint6DOF), new BHoMObjectNameComparer() },
            {typeof(RigidLink), new BHoMObjectNameComparer() },
            {typeof(BarRelease), new BHoMObjectNameComparer() },
            {typeof(SteelSection), new BHoMObjectNameComparer() },
            {typeof(ConstantThickness), new BHoMObjectNameComparer() },
            {typeof(ISectionProperty), new BHoMObjectNameComparer() },
            {typeof(IMaterialFragment), new BHoMObjectNameComparer() },
            {typeof(LinkConstraint), new BHoMObjectNameComparer() },
            {typeof(ISurfaceProperty), new BHoMObjectNameComparer() },
            {typeof(Loadcase), new BHoMObjectNameComparer() },
            {typeof(PointLoad), new BHoMObjectNameComparer() },
            {typeof(GravityLoad), new BHoMObjectNameComparer() },
            {typeof(BarUniformlyDistributedLoad), new BHoMObjectNameComparer() },
            {typeof(BarVaryingDistributedLoad), new BHoMObjectNameComparer() },
            {typeof(BarPointLoad), new BHoMObjectNameComparer() },
            {typeof(AreaUniformlyDistributedLoad), new BHoMObjectNameComparer() },
            {typeof(AreaTemperatureLoad), new BHoMObjectNameComparer() },
            {typeof(BarTemperatureLoad), new BHoMObjectNameComparer() },
            {typeof(LoadCombination), new BHoMObjectNameComparer() },
        };

        /***************************************************/

    }
}