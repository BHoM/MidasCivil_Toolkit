using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using BH.Engine.Structure;
using System.Collections.Generic;

namespace BH.Engine.MidasCivil.Comparer
{
    public class MeshCentreComparer : IEqualityComparer<FEMesh>
    {
        public MeshCentreComparer()
        {
            nodeComparer = new NodeDistanceComparer();
        }

        public MeshCentreComparer(int decimals)
        {
            nodeComparer = new NodeDistanceComparer(decimals);
        }
        public bool Equals(FEMesh mesh1, FEMesh mesh2)
        {
            if (ReferenceEquals(mesh1, mesh2)) return true;

            if (ReferenceEquals(mesh1, null) || ReferenceEquals(mesh2, null))
                return false;

            if (mesh1.BHoM_Guid == mesh2.BHoM_Guid)
                return true;

            Panel panel1 = BH.Engine.MidasCivil.Convert.ConvertFEMesh(mesh1);
            Panel panel2 = BH.Engine.MidasCivil.Convert.ConvertFEMesh(mesh2);
            List<Point> controlPoints1 = BH.Engine.Structure.Query.ControlPoints(panel1, true);
            List<Point> controlPoints2 = BH.Engine.Structure.Query.ControlPoints(panel2, true);
            Point centrePoint1 = BH.Engine.Geometry.Query.Average(controlPoints1);
            Point centrePoint2 = BH.Engine.Geometry.Query.Average(controlPoints2);

            if (nodeComparer.Equals(BH.Engine.MidasCivil.Convert.PointToNode(centrePoint1), BH.Engine.MidasCivil.Convert.PointToNode(centrePoint2)))
                return nodeComparer.Equals(BH.Engine.MidasCivil.Convert.PointToNode(centrePoint1), BH.Engine.MidasCivil.Convert.PointToNode(centrePoint2));

            return false;
        }
        public int GetHashCode(FEMesh mesh)
        {
            return mesh.GetHashCode();
        }

        private NodeDistanceComparer nodeComparer;
    }
}

