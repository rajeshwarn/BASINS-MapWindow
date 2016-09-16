using System;
using System.Collections;
using System.Text;
using MapWinGeoProc.NTS.Topology.Geometries;
using MapWindow.Interfaces.Geometries;

namespace MapWinGeoProc.NTS.Topology.Algorithm
{
    /// <summary> 
    /// Tests whether a <c>Coordinate</c> lies inside
    /// a ring, using a linear-time algorithm.
    /// </summary>
    public class SimplePointInRing : IPointInRing
    {
        /// <summary>
        /// 
        /// </summary>
        private ICoordinate[] pts;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ring"></param>
        public SimplePointInRing(LinearRing ring)
        {
            pts = ring.Coordinates;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public virtual bool IsInside(Coordinate pt)
        {
            return CGAlgorithms.IsPointInRing(pt, pts);
        }
    }
}
