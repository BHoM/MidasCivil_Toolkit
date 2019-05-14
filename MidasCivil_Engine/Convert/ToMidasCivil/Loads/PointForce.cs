using BH.oM.Structure.Loads;
using System.Collections.Generic;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCPointLoad(this PointLoad PointLoad, string assignedNode)
        {
            string midasPointLoad = assignedNode + "," + PointLoad.Force.X.ToString() +
                                                    "," + PointLoad.Force.Y.ToString() +
                                                    "," + PointLoad.Force.Z.ToString() +
                                                    "," + PointLoad.Moment.X.ToString() +
                                                    "," + PointLoad.Moment.Y.ToString() +
                                                    "," + PointLoad.Moment.Z.ToString() +
                                                    "," + PointLoad.Name;
            return midasPointLoad;
        }
    }
}