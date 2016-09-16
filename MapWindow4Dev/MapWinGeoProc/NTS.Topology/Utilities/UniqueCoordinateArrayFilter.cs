using System;
using System.Collections;
using System.Text;

using Iesi.Collections;

using MapWindow.Interfaces.Geometries;
using MapWinGeoProc.NTS.Topology.Geometries;

namespace MapWinGeoProc.NTS.Topology.Utilities
{
    /// <summary>
    /// A <c>CoordinateFilter</c> that builds a set of <c>Coordinate</c>s.
    /// The set of coordinates contains no duplicate points.
    /// </summary>
    public class UniqueCoordinateArrayFilter : ICoordinateFilter 
    {
        private ISet table = new SortedSet();
        private ArrayList list = new ArrayList();

        /// <summary>
        /// 
        /// </summary>
        public UniqueCoordinateArrayFilter() { }

        /// <summary>
        /// Returns the gathered <c>Coordinate</c>s.
        /// </summary>
        public virtual Coordinate[] Coordinates
        {
            get
            {
                Coordinate[] coordinates = new Coordinate[list.Count];
                return (Coordinate[])list.ToArray(typeof(Coordinate));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coord"></param>
        public virtual void Filter(ICoordinate coord) 
        {
            if (!table.Contains(coord)) 
            {
                list.Add(coord);
                table.Add(coord);
            }
        }
    }
}
