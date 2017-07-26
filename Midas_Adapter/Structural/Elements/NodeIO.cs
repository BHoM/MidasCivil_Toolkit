using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHoME = BHoM.Structural.Elements;
using BHoMP = BHoM.Structural.Properties;
using BHoM.Structural.Interface;
using BHG = BHoM.Generic;

namespace Midas_Adapter.Structural.Elements
{
    public static class NodeIO
    {


        public static bool CreateNodes2(string pathName, List<BHoME.Node> nodes, out List<string> ids)
        {
            //TODO: Check if pathName\Nodes.txt exist, if not create.

            List<BHoME.Node> existingNodes = new List<BHoME.Node>();

            if (File.Exists(pathName + "\\Nodes.txt"))
            {
                List<string> existStr = new List<string>();

                foreach (string text in File.ReadAllLines(pathName+ "\\Nodes.txt"))
                {
                    existStr.Add(text);
                }

                for (int i = 2; i < existStr.Count; i++)
                {
                    string[] words = System.Text.RegularExpressions.Regex.Split(existStr[i], ", ");
                    BHoME.Node node = new BHoME.Node(Convert.ToDouble(words[1]), Convert.ToDouble(words[2]), Convert.ToDouble(words[3]), words[0]);
                    node.CustomData["Midas_Id"] = words[0];
                    existingNodes.Add(node);
                }
            }

            //TODO: CELL SIZE!
            BHG.PointMatrix<BHoME.Node> pointMatrix = new BHG.PointMatrix<BHoME.Node>(0.0001);
            foreach (BHoME.Node n in existingNodes)
            {
                pointMatrix.AddPoint(n.Point, n);
            }

            //TODO: Check highest index in existing nodes
            int num = 1;
            ids = new List<string>();

            foreach (BHoME.Node n in nodes)
            {
                //TODO: Tolerance
                BHG.PointMatrix<BHoME.Node>.CompositeValue existingNode = pointMatrix.GetClosestPoint(n.Point, 0.0001);

                //NO:
                //Add the node n to the pointmatrix
                if (existingNode.Data == null)
                {
                    n.CustomData["Midas_Id"] = num;
                    pointMatrix.AddPoint(n.Point, n);
                    
                    
                    //TODO:
                    // Do what you have been doing below with string things
                    string midasNodes = num + ", " + n.X + ", " + n.Y + ", " + n.Z;
                    ids.Add(num.ToString());
                    object value = null;

                    num++;
                }

                //YES:
                //Tag the node n custom data with that nodes ID
                //n.CustomData["Midas_ID"]= existinNode.["Midas_ID"];
                else
                {
                    n.CustomData["Midas_Id"] = existingNode.Data.CustomData["Midas_Id"];
                    ids.Add(n.CustomData["Midas_Id"].ToString());
                }                               
            }


        }



        public static bool CreateNodes(string fileName, List<BHoME.Node> nodes, out List<string> ids)
        {

            //TODO: Check if pathName\Nodes.txt exist, if not create.

            string texts = "*NODE    ; Nodes" + Environment.NewLine + "; iNO, X, Y, Z" + Environment.NewLine;

            ids = new List<string>();
            List<string> midasNodesList = new List<string>();
            int num = 0;

            for (int i = 0; i < nodes.Count; i++)
            {
                num = i + 1;
                ids.Add(num.ToString());

                string midasNodes = num + ", " + nodes[i].X + ", " + nodes[i].Y + ", " + nodes[i].Z;
                midasNodesList.Add(midasNodes);
            }

            foreach (string node in midasNodesList)
            {
                texts += node + Environment.NewLine;
            }
            File.WriteAllText(fileName, texts);

            return true;
        }       
    }
}
