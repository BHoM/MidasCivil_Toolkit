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

            Node n1; Node n2; Node n3;
            bhomNodes.TryGetValue(delimitted[4].Trim(), out n1);
            bhomNodes.TryGetValue(delimitted[5].Trim(), out n2);
            bhomNodes.TryGetValue(delimitted[6].Trim(), out n3);

            ISurfaceProperty bhomSurfaceProperty = null;

            if (!(bhomSurfaceProperties.Count() == 0))
            {
                bhomSurfaceProperties.TryGetValue(delimitted[3].Replace(" ",""), out bhomSurfaceProperty);

                if (!(bhomMaterials.Count() == 0))
                {
                    IMaterialFragment bhomMaterial;
                    bhomMaterials.TryGetValue(delimitted[2].Trim(), out bhomMaterial);
                    bhomSurfaceProperty.Material = bhomMaterial;
                }
            }

            List<Node> nodeList = new List<Node>()
            {
                n1, n2, n3
            };

            List<int> nodeListIndicies = Enumerable.Range(0, 3).ToList();

            if (System.Convert.ToInt32(delimitted[7].Trim()) != 0)
            {
                nodeListIndicies = Enumerable.Range(0, 4).ToList();
                Node n4;
                bhomNodes.TryGetValue(delimitted[7].Trim(), out n4);
                nodeList.Add(n4);
            }


            List<FEMeshFace> feMeshFace = new List<FEMeshFace>()
                {
                    new FEMeshFace() {NodeListIndices = nodeListIndicies}
                };

            FEMesh bhomFEMesh = new FEMesh()
            {
                Faces = feMeshFace,
                Nodes = nodeList,
                Property = bhomSurfaceProperty
            };

            bhomFEMesh.CustomData[AdapterIdName] = delimitted[0].Trim();

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

            if (mesh.CustomData.ContainsValue(AdapterIdName))
                panels[0].CustomData[AdapterIdName] = mesh.CustomData[AdapterIdName];

            return panels[0];
        }
    }

}