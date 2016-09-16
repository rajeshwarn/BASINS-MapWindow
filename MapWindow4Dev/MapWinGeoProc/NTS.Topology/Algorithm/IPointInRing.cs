using System;
using MapWinGeoProc.NTS.Topology.Geometries;

namespace MapWinGeoProc.NTS.Topology.Algorithm
{
    /// <summary> 
    /// An interface for classes which test whether a <c>Coordinate</c> lies inside a ring.
    /// </summary>
    public interface IPointInRing
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        bool IsInside(Coordinate pt);
    }
}
