using BH.oM.Structure.Loads;
using BH.oM.Geometry;
using System.Collections.Generic;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCBarVaryingDistributedLoad(this BarVaryingDistributedLoad barLoad, string assignedBar, string loadType)
        {
            string midasBarLoad = null;
            if (loadType == "Force")
            {
                string direction = MCDirectionConverter(barLoad.ForceA);
                midasBarLoad = assignedBar + ",BEAM,UNILOAD," + MCAxisConverter(barLoad.Axis) + direction +
                                                                    "," + MCProjectionConverter(barLoad.Projected) +
                                                                    ",NO,aDir[1], , , ," + barLoad.DistanceFromA + "," +
                                                                    MCVectorConverter(barLoad.ForceA, direction) + "," +
                                                                    barLoad.DistanceFromB + "," 
                                                                    + MCVectorConverter(barLoad.ForceB, direction) +
                                                                    ",0,0,0,0, ,NO,0,0,NO";
            }
            else
            {
                string direction = MCDirectionConverter(barLoad.MomentA);
                midasBarLoad = assignedBar + ",BEAM,UNIMOMENT," + MCAxisConverter(barLoad.Axis) + direction +
                                                                    "," + MCProjectionConverter(barLoad.Projected) +
                                                                    ",NO,aDir[1], , , ," + barLoad.DistanceFromA + "," +
                                                                    MCVectorConverter(barLoad.MomentA, direction) + "," +
                                                                    barLoad.DistanceFromB + ","
                                                                    + MCVectorConverter(barLoad.MomentB, direction) +
                                                                    ",0,0,0,0, ,NO,0,0,NO";
            }

            return midasBarLoad;
        }
    }
}