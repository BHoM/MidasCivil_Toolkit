using BH.oM.Structure.Loads;
using BH.oM.Geometry;
using System.Collections.Generic;
using System.Linq;
using System;
using BH.oM.Structure.Elements;
using BH.Engine.Geometry;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCAreaUniformlyDistributedLoad(this AreaUniformalyDistributedLoad FEMeshLoad, string assignedFEMesh)
        {
            string midasFEMeshLoad = null;

            string direction = MCDirectionConverter(FEMeshLoad.Pressure);
            midasFEMeshLoad = assignedFEMesh + ", PRES, PLATE, FACE, " + MCAxisConverter(FEMeshLoad.Axis) + direction +
                                                                    ", 0, 0, 0, " + MCProjectionConverter(FEMeshLoad.Projected) +
                                                                    ", " + MCVectorConverter(FEMeshLoad.Pressure, direction) +
                                                                    ", 0, 0, 0, 0, " + FEMeshLoad.Name;

            return midasFEMeshLoad;
        }

        public static List<FEMesh> ConvertPanelPlanar(List<PanelPlanar> panels)
        {
            List<FEMesh> FEMeshes = new List<FEMesh>();

            foreach (PanelPlanar panel in panels)
            {
                List<Edge> panelEdges = new List<Edge>();
                panelEdges.AddRange(panel.ExternalEdges);
                List<Edge> distinctEdges = GetDistinctEdges(panelEdges);
                List<Point> points = new List<Point>();

                foreach (Edge edge in distinctEdges)
                {
                    points.Add(edge.Curve.IStartPoint());
                }

                List<Face> faces = new List<Face>();

                if (distinctEdges.Count == 3)
                    faces.Add(BH.Engine.Geometry.Create.Face(1, 2, 3));
                else
                    faces.Add(BH.Engine.Geometry.Create.Face(1, 2, 3, 4));

                Mesh mesh = BH.Engine.Geometry.Create.Mesh(points, faces);
                FEMeshes.Add(BH.Engine.Structure.Create.FEMesh(mesh));
            }

            for (int i = 0; i < FEMeshes.Count; i++)
            {
                FEMeshes[i].CustomData[AdapterId] = panels[i].CustomData[AdapterId];
            }

            return FEMeshes;
        }

        public static List<Edge> GetDistinctEdges(IEnumerable<Edge> edges)
        {
            List<Edge> distinctEdges = edges.GroupBy(m => new
            {
                X = Math.Round(m.Curve.IPointAtParameter(0.5).X, 3),
                Y = Math.Round(m.Curve.IPointAtParameter(0.5).Y, 3),
                Z = Math.Round(m.Curve.IPointAtParameter(0.5).Z, 3)
            })
        .Select(x => x.First())
        .ToList();

            return distinctEdges;
        }
    }
}