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

            string startNodeID = bar.StartNode.CustomData[AdapterIdName].ToString();
            string endNodeID = bar.EndNode.CustomData[AdapterIdName].ToString();
            string materialID = "1";
            string sectionID = "1";

            if (bar.SectionProperty != null)
            {
                sectionID = bar.SectionProperty.CustomData[AdapterIdName].ToString();
                if (bar.SectionProperty.Material != null)
                {
                    materialID = bar.SectionProperty.Material.CustomData[AdapterIdName].ToString();
                }
            }

            if (bar.FEAType == BarFEAType.Axial || bar.FEAType == BarFEAType.Flexural)
            {
                midasElement = (bar.CustomData[AdapterIdName].ToString() + "," + feaType +
                                      "," + materialID + "," + sectionID + "," +
                                      startNodeID + "," + endNodeID + "," +
                                      (bar.OrientationAngle*180/System.Math.PI).ToString() + ",0,0");
            }
            else
            {
                midasElement = (bar.CustomData[AdapterIdName].ToString() + "," + feaType +
                                  "," + materialID + "," + sectionID + "," +
                                  startNodeID + "," + endNodeID + "," +
                                  (bar.OrientationAngle * 180 / System.Math.PI).ToString() + ",1,0,0,NO");
            }

            return midasElement;
        }

        public static string ToMCElement(this FEMesh feMesh)
        {
            string midasElement = "";
            List<int> nodeIndices = feMesh.Faces[0].NodeListIndices;

            if(feMesh.Nodes.Count > 4)
            {
                BH.Engine.Reflection.Compute.RecordError("Cannot push mesh with more than 4 nodes");
            }
            else if (feMesh.Nodes.Count == 4)
            {
                midasElement = (feMesh.CustomData[AdapterIdName].ToString() + ",PLATE," +
                    feMesh.Property.CustomData[AdapterIdName].ToString() + "," +
                    feMesh.Property.Material.CustomData[AdapterIdName] + "," +
                  feMesh.Nodes[nodeIndices[0]].CustomData[AdapterIdName].ToString() + "," +
                  feMesh.Nodes[nodeIndices[1]].CustomData[AdapterIdName].ToString() + "," +
                  feMesh.Nodes[nodeIndices[2]].CustomData[AdapterIdName].ToString() + "," +
                  feMesh.Nodes[nodeIndices[3]].CustomData[AdapterIdName].ToString() + ",1,0");
            }
            else
            {
                midasElement = (feMesh.CustomData[AdapterIdName].ToString() + ",PLATE," +
                    feMesh.Property.CustomData[AdapterIdName].ToString() + "," +
                    feMesh.Property.Material.CustomData[AdapterIdName] + "," +
                 feMesh.Nodes[nodeIndices[0]].CustomData[AdapterIdName].ToString() + "," +
                 feMesh.Nodes[nodeIndices[1]].CustomData[AdapterIdName].ToString() + "," +
                 feMesh.Nodes[nodeIndices[2]].CustomData[AdapterIdName].ToString() + ",0,1,0");
            }

            return midasElement;
        }
    }
}