using System;
using System.Collections;
using System.Text;

using MapWinGeoProc.NTS.Topology.Geometries;

using MapWindow.Interfaces.Geometries;


namespace MapWinGeoProc.NTS.Topology.Utilities
{
    /// <summary>
    /// A <c>CoordinateFilter</c> that counts the total number of coordinates
    /// in a <c>Geometry</c>.
    /// </summary>
    public class CoordinateCountFilter : ICoordinateFilter 
    {
        private int n = 0;

        /// <summary>
        /// 
        /// </summary>
        public CoordinateCountFilter() { }

        /// <summary>
        /// Returns the result of the filtering.
        /// </summary>
        public virtual int Count 
        {
            get
            {
                return n;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coord"></param>
        public virtual void Filter(ICoordinate coord) 
        {
            n++;
        }
    }
}
