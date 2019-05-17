using BH.oM.Structure.Elements;
using BH.oM.Geometry;
using BH.Engine.Structure;
using System.Collections.Generic;

namespace BH.Engine.MidasCivil.Comparer
{
    public class BarMidPointComparer : IEqualityComparer<Bar>
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public BarMidPointComparer()
        {
            nodeComparer = new NodeDistanceComparer();
        }

        /***************************************************/

        public BarMidPointComparer(int decimals)
        {
            nodeComparer = new NodeDistanceComparer(decimals);
        }


        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public bool Equals(Bar bar1, Bar bar2)
        {
            if (ReferenceEquals(bar1, bar2)) return true;

            if (ReferenceEquals(bar1, null) || ReferenceEquals(bar2, null))
                return false;

            if (bar1.BHoM_Guid == bar2.BHoM_Guid)
                return true;

            Point centrePoint1 = BH.Engine.Geometry.Query.IPointAtParameter(bar1.Centreline(), 0.5);
            Point centrePoint2 = BH.Engine.Geometry.Query.IPointAtParameter(bar1.Centreline(), 0.5);

            if (nodeComparer.Equals(BH.Engine.MidasCivil.Convert.PointToNode(centrePoint1), BH.Engine.MidasCivil.Convert.PointToNode(centrePoint2)))
            {
                return nodeComparer.Equals(BH.Engine.MidasCivil.Convert.PointToNode(centrePoint1), BH.Engine.MidasCivil.Convert.PointToNode(centrePoint2));
            }

            return false;
        }

        /***************************************************/

        public int GetHashCode(Bar bar)
        {
            //Check whether the object is null
            if (ReferenceEquals(bar, null)) return 0;

            return bar.StartNode.GetHashCode() ^ bar.EndNode.GetHashCode();
        }


        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private NodeDistanceComparer nodeComparer;

        /***************************************************/

    }
}

