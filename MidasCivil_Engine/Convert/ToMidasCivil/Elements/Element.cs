using System.IO;
using System;
using BH.oM.Structure.Elements;
using System.Collections.Generic;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCElement(this Bar bar)
        {
            string midasElement;
            string feaType = "TRUSS";
            switch (bar.FEAType)
            {
                case BarFEAType.Axial:
                    break;

                case BarFEAType.Flexural:
                    feaType = "BEAM";
                    break;

                case BarFEAType.TensionOnly:
                    feaType = "TENSTR";
                    break;

                case BarFEAType.CompressionOnly:
                    feaType = "COMPTR";
                    break;
            }

            string startNodeID = bar.StartNode.CustomData[AdapterId].ToString();
            string endNodeID = bar.EndNode.CustomData[AdapterId].ToString();
            string materialID = "1";
            string sectionID = "1";

            if (bar.SectionProperty != null)
            {
                sectionID = bar.SectionProperty.CustomData[AdapterId].ToString();
                if (bar.SectionProperty.Material != null)
                {
                    materialID = bar.SectionProperty.Material.CustomData[AdapterId].ToString();
                }
            }

            if (bar.FEAType == BarFEAType.Axial || bar.FEAType == BarFEAType.Flexural)
            {
                midasElement = (bar.CustomData[AdapterId].ToString() + "," + feaType +
                                      "," + materialID + "," + sectionID + "," +
                                      startNodeID + "," + endNodeID + "," +
                                      bar.OrientationAngle.ToString() + ",0,0");
            }
            else
            {
                midasElement = (bar.CustomData[AdapterId].ToString() + "," + feaType +
                                  "," + materialID + "," + sectionID + "," +
                                  startNodeID + "," + endNodeID + "," +
                                  bar.OrientationAngle.ToString() + ",1,0,0,NO");
            }

            return midasElement;
        }

        public static string ToMCElement(this FEMesh feMesh)
        {
            string midasElement;
            List<int> nodeIndices = feMesh.MeshFaces[0].NodeListIndices;

            if (feMesh.Nodes.Count == 4)
            {
                midasElement = (feMesh.CustomData[AdapterId].ToString() + ",PLATE,1, 1," +
                  feMesh.Nodes[nodeIndices[0]].CustomData[AdapterId].ToString() + "," +
                  feMesh.Nodes[nodeIndices[1]].CustomData[AdapterId].ToString() + "," +
                  feMesh.Nodes[nodeIndices[2]].CustomData[AdapterId].ToString() + "," +
                  feMesh.Nodes[nodeIndices[3]].CustomData[AdapterId].ToString() + ",1,0");
            }
            else
            {
                midasElement = (feMesh.CustomData[AdapterId].ToString() + ",PLATE,1,1, " +
                 feMesh.Nodes[nodeIndices[0]].CustomData[AdapterId].ToString() + "," +
                 feMesh.Nodes[nodeIndices[1]].CustomData[AdapterId].ToString() + "," +
                 feMesh.Nodes[nodeIndices[2]].CustomData[AdapterId].ToString() + ",0,1,0");
            }

            return midasElement;
        }
    }
}