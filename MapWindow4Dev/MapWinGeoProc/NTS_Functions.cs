using System;
using System.Collections.Generic;
using System.Text;
using MapWinGeoProc.NTS.Topology.Geometries;
using MapWindow.Interfaces.Geometries;
namespace MapWinGeoProc
{
    /// <summary>
    /// This enables easier access to some NTS functions
    /// </summary>
    public static class NTS_Functions
    {
        /// <summary>
        /// Creates a buffer polygon that is Distance around mwShape
        /// </summary>
        /// <param name="mwShape">The polygon to buffer</param>
        /// <param name="Distance">The distance</param>
        /// <param name="QuadrantSegmants">how rounded the buffer is</param>
        /// <param name="EndCap">The type of endcap</param>
        /// <returns></returns>
        public static MapWinGIS.Shape Buffer(MapWinGIS.Shape mwShape, double Distance, int QuadrantSegmants, MapWindow.Interfaces.Geometries.BufferStyles EndCap)
        {
            IGeometry Buffer;
            Geometry geom = NTS_Adapter.ShapeToGeometry(mwShape);
            Buffer = geom.Buffer(Distance, QuadrantSegmants, EndCap);
            return NTS_Adapter.GeometryToShape(Buffer);
        }

    }
}
