using System;
using System.Collections;
using System.Text;

using MapWinGeoProc.NTS.Topology.Geometries;
using MapWindow.Interfaces.Geometries;
namespace MapWinGeoProc.NTS.Topology.Operation.Valid
{
    /// <summary> 
    /// Implements the appropriate checks for repeated points
    /// (consecutive identical coordinates) as defined in the
    /// NTS spec.
    /// </summary>
    public class RepeatedPointTester
    {

        // save the repeated coord found (if any)
        private ICoordinate repeatedCoord;

        /// <summary>
        /// 
        /// </summary>
        public RepeatedPointTester() { }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICoordinate Coordinate
        {
            get
            {
                return repeatedCoord;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public virtual bool HasRepeatedPoint(IGeometry g)
        {
            if (g.IsEmpty)  return false;
            if (g is Point) return false;
            else if (g is MultiPoint) return false;
            // LineString also handles LinearRings
            else if (g is LineString) 
                return HasRepeatedPoint(((ILineString) g).Coordinates);
            else if (g is Polygon)
                return HasRepeatedPoint((IPolygon) g);
            else if (g is GeometryCollection) 
                return HasRepeatedPoint((IGeometryCollection) g);
            else  throw new NotSupportedException(g.GetType().FullName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public virtual bool HasRepeatedPoint(ICoordinate[] coord)
        {
            for (int i = 1; i < coord.Length; i++)
            {
                if (coord[i - 1].Equals(coord[i]))
                {
                    repeatedCoord = coord[i];
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool HasRepeatedPoint(IPolygon p)
        {
            if (HasRepeatedPoint(p.ExteriorRing.Coordinates))
                return true;
            for (int i = 0; i < p.NumInteriorRings; i++)
                if (HasRepeatedPoint(p.GetInteriorRingN(i).Coordinates)) 
                    return true;            
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gc"></param>
        /// <returns></returns>
        private bool HasRepeatedPoint(IGeometryCollection gc)
        {
            for (int i = 0; i < gc.NumGeometries; i++)
            {
                IGeometry g = gc.GetGeometryN(i);
                if (HasRepeatedPoint(g)) 
                    return true;
            }
            return false;
        }
    }
}
