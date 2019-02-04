using BH.oM.Structure.Elements;
using BH.Engine.Geometry;
using BH.oM.Geometry;
using System.Collections.Generic;
using System.Linq;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static FEMesh ToBHoMFEMesh(this string feMesh, Dictionary<string,Node> bhomNodes)
        {
            List<string> delimitted = feMesh.Split(',').ToList();
            Node n1 = null;
            Node n2 = null;
            Node n3 = null;
            Node n4 = null;
            FEMesh bhomFEMesh;

            bhomNodes.TryGetValue(delimitted[4], out n1);
            bhomNodes.TryGetValue(delimitted[5], out n2);
            bhomNodes.TryGetValue(delimitted[6], out n3);

            Point p1 = Engine.Structure.Query.Position(n1);
            Point p2 = Engine.Structure.Query.Position(n2);
            Point p3 = Engine.Structure.Query.Position(n3);

            if (System.Convert.ToInt32(delimitted[7])!=0)
            {
                bhomNodes.TryGetValue(delimitted[7], out n4);
                Point p4 = Engine.Structure.Query.Position(n4);
                List<Point> meshPoints = new List<Point> { p1, p2, p3, p4 };
                List<Face> meshFace = new List<Face> { Geometry.Create.Face(1, 2, 3, 4) };
                Mesh bhomMesh = Geometry.Create.Mesh(meshPoints,meshFace);
                bhomFEMesh = Structure.Create.FEMesh(bhomMesh);
            }
            else
            {
                List<Point> meshPoints = new List<Point> { p1, p2, p3 };
                List<Face> meshFace = new List<Face> { Geometry.Create.Face(1, 2, 3) };
                Mesh bhomMesh = Geometry.Create.Mesh(meshPoints, meshFace);
                bhomFEMesh = Structure.Create.FEMesh(bhomMesh);
            }

            bhomFEMesh.CustomData[AdapterId] = delimitted[0];

            return bhomFEMesh;
        }
    }
}