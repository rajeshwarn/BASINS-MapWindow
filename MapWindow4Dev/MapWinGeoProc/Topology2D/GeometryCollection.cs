using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Topology2D
{
    /// <summary>
    /// An abstract class for groups of simpler geometric shapes
    /// </summary>
    public abstract class GeometryCollection
    {
        /// <summary>
        /// A list of geometries stored in this geometry collection
        /// </summary>
        public List<Geometry> GeometryN;

        /// <summary>
        /// Returns the number of geometries in the collection
        /// </summary>
        public virtual int NumGeometries
        {
            get
            {
                return GeometryN.Count;
            }
        }
        

    }
}
