using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Topology2D
{
    /// <summary>
    /// A group of Polygons, like islands
    /// </summary>
    public class MultiPolygon: GeometryCollection
    {
        /// <summary>
        /// A list of polygons.
        /// </summary>
        public List<Polygon> Geometries;

        MultiPolygon()
        {
            Geometries = new List<Polygon>();
        }

        /// <summary>
        /// This will convert even the most complicated polygons into a working 
        /// collection for topology to use.  Frequently, it will be a simple polygon,
        /// but sometimes multiple parts aren't holes.
        /// </summary>
        /// <param name="mwShape"></param>
        MultiPolygon(MapWinGIS.Shape mwShape)
        {

        }

        /// <summary>
        /// Returns an integer count of the polygons
        /// </summary>
        public override int NumGeometries
        {
            get
            {
                return Geometries.Count;
            }
        }
        /// <summary>
        /// This collection is a collection of 2-D polygons
        /// </summary>
        public int Dimension
        {
            get
            {
                return 2;
            }
        }
    }
}
