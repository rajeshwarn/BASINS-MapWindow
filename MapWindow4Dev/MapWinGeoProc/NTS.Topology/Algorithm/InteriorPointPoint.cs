using System;
using System.Collections;
using System.Text;
using MapWinGeoProc.NTS.Topology.Geometries;
using MapWindow.Interfaces.Geometries;
namespace MapWinGeoProc.NTS.Topology.Algorithm
{
    /// <summary> 
    /// Computes a point in the interior of an point point.
    /// Algorithm:
    /// Find a point which is closest to the centroid of the point.
    /// </summary>
    public class InteriorPointPoint
    {
        private ICoordinate centroid;
        private double minDistance = Double.MaxValue;
        private ICoordinate interiorPoint = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        public InteriorPointPoint(Geometry g)
        {
            centroid = new Coordinate(g.Centroid);
            Add(g);
        }

        /// <summary> 
        /// Tests the point(s) defined by a Geometry for the best inside point.
        /// If a Geometry is not of dimension 0 it is not tested.
        /// </summary>
        /// <param name="geom">The point to add.</param>
        private void Add(IGeometry geom)
        {
            if(geom is Point) 
                Add(geom.Coordinate);    
            else if (geom is GeometryCollection) 
            {
                GeometryCollection gc = (GeometryCollection)geom;
                foreach (Geometry geometry in gc.Geometries)
                    Add(geometry);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        private void Add(ICoordinate point)
        {
            double dist = point.Distance(centroid);
            if (dist < minDistance)
            {
                interiorPoint = new Coordinate(point);
                minDistance = dist;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual ICoordinate InteriorPoint
        {
            get
            {
                return interiorPoint;
            }
        }
    }   
}
