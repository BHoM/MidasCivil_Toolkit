using BH.oM.Structure.Elements;
using System.Collections.Generic;
using System.Linq;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static FEMesh ToBHoMFEMesh(this string feMesh, Dictionary<string, Node> bhomNodes)
        {
            List<string> delimitted = feMesh.Split(',').ToList();
            Node n1 = null;
            Node n2 = null;
            Node n3 = null;
            Node n4 = null;


            bhomNodes.TryGetValue(delimitted[4].Replace(" ", ""), out n1);
            bhomNodes.TryGetValue(delimitted[5].Replace(" ", ""), out n2);
            bhomNodes.TryGetValue(delimitted[6].Replace(" ", ""), out n3);

            List<Node> nodeList = new List<Node>()
            {
                n1, n2, n3
            };

            FEMesh bhomFEMesh = null;

            if (System.Convert.ToInt32(delimitted[7].Replace(" ", "")) != 0)
            {
                bhomNodes.TryGetValue(delimitted[7].Replace(" ", ""), out n4);
                nodeList.Add(n4);
                List<int> nodeListIndicies = Enumerable.Range(0, 4).ToList();
                List<FEMeshFace> feMeshFace = new List<FEMeshFace>()
                {
                    new FEMeshFace() {NodeListIndices = nodeListIndicies}
                };

                bhomFEMesh = new FEMesh()
                {
                    MeshFaces = feMeshFace,
                    Nodes = nodeList
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
                    MeshFaces = feMeshFace,
                    Nodes = nodeList
                };
            }

            bhomFEMesh.CustomData[AdapterId] = delimitted[0].Replace(" ", "");

            return bhomFEMesh;
        }
    }
}