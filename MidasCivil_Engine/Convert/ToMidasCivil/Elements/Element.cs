using System.IO;
using System;
using BH.oM.Structure.Elements;
using System.Collections.Generic;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static void ToMCElement(this Bar bar, string path)
        {
            using (StreamWriter elementText = File.AppendText(path))
            {
                elementText.WriteLine(bar.CustomData[AdapterId].ToString() + ", BEAM, 1, 1, " +
                                      bar.StartNode.CustomData[AdapterId].ToString() + ", " +
                                      bar.EndNode.CustomData[AdapterId].ToString() + ", 0, 0");
                elementText.Close();
            }
           
        }

        public static void ToMCElement(this FEMesh feMesh, string path)
        {
            using (StreamWriter elementText = File.AppendText(path))
            {
                List<int> nodeIndices = feMesh.MeshFaces[0].NodeListIndices;

                if (feMesh.Nodes.Count==4)
                {
                    elementText.WriteLine(feMesh.CustomData[AdapterId].ToString() + ",PLATE,1, 1," +
                      feMesh.Nodes[nodeIndices[0]].CustomData[AdapterId].ToString() + "," +
                      feMesh.Nodes[nodeIndices[1]].CustomData[AdapterId].ToString() + "," +
                      feMesh.Nodes[nodeIndices[2]].CustomData[AdapterId].ToString() + "," +
                      feMesh.Nodes[nodeIndices[3]].CustomData[AdapterId].ToString() + ",1,0");
                }
                else
                {
                    elementText.WriteLine(feMesh.CustomData[AdapterId].ToString() + ",PLATE,1,1, " +
                    feMesh.Nodes[nodeIndices[0]].CustomData[AdapterId].ToString() + "," +
                    feMesh.Nodes[nodeIndices[1]].CustomData[AdapterId].ToString() + "," +
                    feMesh.Nodes[nodeIndices[2]].CustomData[AdapterId].ToString() + ",0,1,0");
                }

                elementText.Close();
            }
        }
    }
}