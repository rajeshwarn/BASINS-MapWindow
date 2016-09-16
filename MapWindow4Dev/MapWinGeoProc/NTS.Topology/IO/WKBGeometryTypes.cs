using System;
using System.Collections;
using System.Text;

namespace MapWinGeoProc.NTS.Topology.IO
{
    /// <summary>
    /// Byte order
    /// </summary>
    public enum ByteOrder
    {
        /// <summary>
        /// LittleIndian
        /// </summary>
        BigIndian = 0x00,

        /// <summary>
        /// BigIndian
        /// </summary>
        LittleIndian = 0x01,
    }

    /// <summary>
    /// WKB Geometry Types
    /// </summary>
    public enum WKBGeometryTypes
    {
        /// <summary>
        /// Point.
        /// </summary>
        WKBPoint = 1,

        /// <summary>
        /// LineString.
        /// </summary>
        WKBLineString = 2,

        /// <summary>
        /// Polygon.
        /// </summary>
        WKBPolygon = 3,

        /// <summary>
        /// MultiPoint.
        /// </summary>
        WKBMultiPoint = 4,

        /// <summary>
        /// MultiLineString.
        /// </summary>
        WKBMultiLineString = 5,

        /// <summary>
        /// MultiPolygon.
        /// </summary>
        WKBMultiPolygon = 6,

        /// <summary>
        /// GeometryCollection.
        /// </summary>
        WKBGeometryCollection = 7
    };
}
