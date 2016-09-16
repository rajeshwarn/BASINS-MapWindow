using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Topology2D
{
    /// <summary>
    /// A grouping that categorizies the shape types
    /// </summary>
    public enum ShapeCategories
    {
        /// <summary>
        /// Point, PointM, or PointZ
        /// </summary>
        Point,
        /// <summary>
        /// MultiPoint, MultiPointM, or MultiPointZ
        /// </summary>
        MultiPoint,
        /// <summary>
        /// LineString, LineStringM, or LineStringZ
        /// </summary>
        Line,
        /// <summary>
        /// Polygon, PolygonM, or PolygonZ
        /// </summary>
        Polygon,
        /// <summary>
        /// NullShape or MultiPatch (Not Supported)
        /// </summary>
        Invalid
    }
}