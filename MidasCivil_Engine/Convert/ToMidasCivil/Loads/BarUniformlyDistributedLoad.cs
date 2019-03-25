using BH.oM.Structure.Loads;
using BH.oM.Geometry;
using System.Collections.Generic;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCBarUniformlyDistributedLoad(this BarUniformlyDistributedLoad barLoad, string assignedBar, string loadType)
        {
            string midasBarLoad = null;
            if (loadType=="Force")
            {
                string direction = MCDirectionConverter(barLoad.Force);
                midasBarLoad = assignedBar + ",BEAM,UNILOAD," + MCAxisConverter(barLoad.Axis) + direction +
                                                                    "," + MCProjectionConverter(barLoad.Projected) +
                                                                    ",NO,aDir[1], , , ,0," +
                                                                    MCVectorConverter(barLoad.Force, direction) +
                                                                    ",1," + MCVectorConverter(barLoad.Force, direction) +
                                                                    ",0,0,0,0, ,NO,0,0,NO";
            }
            else
            {
                string direction = MCDirectionConverter(barLoad.Moment);
                midasBarLoad = assignedBar + ",BEAM,UNIMOMENT," + MCAxisConverter(barLoad.Axis) + direction +
                                                                    "," + MCProjectionConverter(barLoad.Projected) +
                                                                    ",NO,aDir[1], , , ,0," +
                                                                    MCVectorConverter(barLoad.Moment, direction) +
                                                                    ",1," + MCVectorConverter(barLoad.Moment, direction) +
                                                                    ",0,0,0,0, ,NO,0,0,NO";
            }

            return midasBarLoad;
        }

        public static string MCAxisConverter (LoadAxis bhomAxis)
        {
            string MCAxis = "G";

            if (bhomAxis == LoadAxis.Local)
            {
                MCAxis = "L";
            }

            return MCAxis;
        }

        public static string MCDirectionConverter(Vector bhomVector)
        {
            string MCDirection = "X";

            if (bhomVector.Y!=0)
            {
                MCDirection = "Y";
            }
            else if (bhomVector.Z!=0)
            {
                MCDirection = "Z";
            }

            return MCDirection;
        }

        public static string MCVectorConverter(Vector bhomVector, string direction)
        {
            string MCLoad = "0";

            if (direction=="X")
            {
                MCLoad = bhomVector.X.ToString();
            }
            else if (direction == "Y")
            {
                MCLoad = bhomVector.Y.ToString();
            }
            else
            {
                MCLoad = bhomVector.Z.ToString();
            }

            return MCLoad;
        }

        public static string MCProjectionConverter(bool bhomProjection)
        {
            string MCProjection = "NO";

            if (bhomProjection)
            {
                MCProjection = "YES";
            }

            return MCProjection;
        }
    }
}