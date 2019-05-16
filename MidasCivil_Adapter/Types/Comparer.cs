using BH.Engine.Base.Objects;
using BH.oM.Structure.Elements;
using BH.oM.Structure.Constraints;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Structure.SectionProperties;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Structure.Loads;
using BH.oM.Geometry;
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
            {typeof(Bar), new BH.Engine.Structure.BarEndNodesDistanceComparer(3) },
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

        public class MeshCentreComparer : IEqualityComparer<FEMesh>
        {
            public bool Equals(FEMesh mesh1, FEMesh mesh2)
            {
                Panel panel1 = BH.Engine.MidasCivil.Convert.ConvertFEMesh(mesh1);
                Panel panel2 = BH.Engine.MidasCivil.Convert.ConvertFEMesh(mesh2);
                List<Point> controlPoints1 = BH.Engine.Structure.Query.ControlPoints(panel1, true);
                List<Point> controlPoints2 = BH.Engine.Structure.Query.ControlPoints(panel2, true);
                Point centrePoint1 = BH.Engine.Geometry.Query.Average(controlPoints1);
                Point centrePoint2 = BH.Engine.Geometry.Query.Average(controlPoints2);

                IEqualityComparer<Node> comparer =  new BH.Engine.Structure.NodeDistanceComparer();
                return comparer.Equals(BH.Engine.MidasCivil.Convert.PointToNode(centrePoint1), BH.Engine.MidasCivil.Convert.PointToNode(centrePoint2));
            }
            public int GetHashCode(FEMesh mesh)
            {
                return mesh.GetHashCode();
            }
        }
    }
}