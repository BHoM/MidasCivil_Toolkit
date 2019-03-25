using BH.oM.Structure.Loads;
using System.Collections.Generic;

namespace BH.Engine.MidasCivil
{
    public static partial class Convert
    {
        public static string ToMCPointForce(this PointForce pointForce, string assignedNode)
        {
            string midasPointForce = assignedNode + "," + pointForce.Force.X.ToString() +
                                                    "," + pointForce.Force.Y.ToString() +
                                                    "," + pointForce.Force.Z.ToString() +
                                                    "," + pointForce.Moment.X.ToString() +
                                                    "," + pointForce.Moment.Y.ToString() +
                                                    "," + pointForce.Moment.Z.ToString() +
                                                    "," + pointForce.Name;
            return midasPointForce;
        }
    }
}