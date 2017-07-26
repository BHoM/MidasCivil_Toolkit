using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHE = BHoM.Structural.Elements;

namespace Midas_Adapter.Structural.Elements
{
    public static class BarIO
    {
        public static bool CreateBars(string pathName, List<BHE.Bar> bars, out List<string> ids)
        {
            //Check if path\Bars.txt exist, otherwhise create



            //List<string> nodeIds = new List<string>();

            List<BHE.Node> nodes = bars.SelectMany(x => new List<BHE.Node> { x.StartNode, x.EndNode }).Distinct().ToList();

            List<string> nodeIds;
            NodeIO.CreateNodes2(pathName, nodes, out nodeIds);

            //Loop through and create the bar strings

            ids = new List<string>();


            
        }
    }
}