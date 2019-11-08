using BH.oM.Structure.Elements;
using BH.oM.Structure.MaterialFragments;
using BH.oM.Structure.SurfaceProperties;
using BH.oM.Geometry;
using System.Collections.Generic;
using System.Linq;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static FEMesh ToBHoMFEMesh(
            this string feMesh, 
            Dictionary<string, Node> bhomNodes, 
            Dictionary<string,ISurfaceProperty> bhomSurfaceProperties,
            Dictionary<string, IMaterialFragment> bhomMaterials)
        {
            List<string> delimitted = feMesh.Split(',').ToList();

            bhomNodes.TryGetValue(delimitted[4].Trim(), out Node n1);
            bhomNodes.TryGetValue(delimitted[5].Trim(), out Node n2);
            bhomNodes.TryGetValue(delimitted[6].Trim(), out Node n3);

            ISurfaceProperty bhomSurfaceProperty = null;

            if (!(bhomSurfaceProperties.Count() == 0))
            {
                bhomSurfaceProperties.TryGetValue(delimitted[3].Replace(" ",""), out bhomSurfaceProperty);

                if (!(bhomMaterials.Count() == 0))
                {
                    bhomMaterials.TryGetValue(delimitted[2].Trim(), out IMaterialFragment bhomMaterial);
                    bhomSurfaceProperty.Material = bhomMaterial;
                }
            }

            List<Node> nodeList = new List<Node>()
            {
                n1, n2, n3
            };

            FEMesh bhomFEMesh;

            if (System.Convert.ToInt32(delimitted[7].Trim()) != 0)
            {
                bhomNodes.TryGetValue(delimitted[7].Trim(), out Node n4);
                nodeList.Add(n4);
                List<int> nodeListIndicies = Enumerable.Range(0, 4).ToList();
                List<FEMeshFace> feMeshFace = new List<FEMeshFace>()
                {
                    new FEMeshFace() {NodeListIndices = nodeListIndicies}
                };

                bhomFEMesh = new FEMesh()
                {
                    Faces = feMeshFace,
                    Nodes = nodeList,
                    Property = bhomSurfaceProperty
                };

            }
            else
            {
                List<int> nodeListIndicies = Enumerable.Range(0, 4).ToList();
                List<FEMeshFace> feMeshFace = new List<FEMeshFace>()
                {
                    new FEMeshFace() {NodeListIndices = nodeListIndicies}
                };

                bhomFEMesh = new FEMesh()
                {
                    Faces = feMeshFace,
                    Nodes = nodeList,
                    Property = bhomSurfaceProperty
                };
            }

            bhomFEMesh.CustomData[AdapterId] = delimitted[0].Trim();

            return bhomFEMesh;
        }

        public static Panel ConvertFEMesh(FEMesh mesh)
        {
            List<Polyline> polylines = new List<Polyline>();

                List<Point> points = new List<Point>();

                foreach (Node node in mesh.Nodes)
                {
                    points.Add(node.Position);
                }

                points.Add(mesh.Nodes.First().Position);
                polylines.Add(BH.Engine.Geometry.Create.Polyline(points));

            List<Panel> panels = BH.Engine.Structure.Create.PanelPlanar(polylines);

            if (mesh.CustomData.ContainsValue(AdapterId))
                panels[0].CustomData[AdapterId] = mesh.CustomData[AdapterId];

            return panels[0];
        }
    }

}