using BH.oM.Structure.Loads;
using BH.oM.Geometry;
using System.Collections.Generic;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCBarPointLoad(this BarPointLoad barLoad, string assignedBar, string loadType)
        {
            string midasBarLoad = null;
            if (loadType == "Force")
            {
                string direction = MCDirectionConverter(barLoad.Force);
                midasBarLoad = assignedBar + ",BEAM,CONLOAD," + MCAxisConverter(barLoad.Axis) + direction +
                                                                    "," + MCProjectionConverter(barLoad.Projected) +
                                                                    ",NO,aDir[1], , , ," + barLoad.DistanceFromA.ToString() + "," +
                                                                    MCVectorConverter(barLoad.Force, direction) +
                                                                    ",0,0,0,0,0,0," + barLoad.Name + ",NO,0,0,NO";
            }
            else
            {
                string direction = MCDirectionConverter(barLoad.Moment);
                midasBarLoad = assignedBar + ",BEAM,CONMOMENT," + MCAxisConverter(barLoad.Axis) + direction +
                                                                    "," + MCProjectionConverter(barLoad.Projected) +
                                                                    ",NO,aDir[1], , , ," + barLoad.DistanceFromA.ToString() + "," +
                                                                    MCVectorConverter(barLoad.Moment, direction) +
                                                                    ",0,0,0,0,0,0," + barLoad.Name + ",NO,0,0,NO";
            }

            return midasBarLoad;
        }
    }
}