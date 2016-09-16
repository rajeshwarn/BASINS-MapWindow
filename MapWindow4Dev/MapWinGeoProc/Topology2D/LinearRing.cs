using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Topology2D
{
    /// <summary>
    /// A Line String that is closed and has the same first point and last point.
    /// </summary>
    public class LinearRing: LineString
    {
        #region Constructors

        /// <summary>
        /// Constructs a blank instance of a linear ring
        /// </summary>
        public LinearRing()
        {
            base.Coordinates = new List<Coordinate>();
        }
        /// <summary>
        /// Creates a new instance of a linear ring from a coordinate list
        /// </summary>
        /// <param name="Coords">A List of coordinates</param>
        public LinearRing(List<Coordinate> Coords)
        {
            
            base.Coordinates = Coords;
        }

        /// <summary>
        /// By definition LinearRings must be closed
        /// </summary>
        public bool IsClosed
        {
            get
            {
                return true;
            }
        }

        #endregion


        #region Geometry Overrides
        /// <summary>
        /// This shape is two dimensional
        /// </summary>
        public override int Dimension
        {
            get { return 2; }
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <returns></returns>
        public override string AsText()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <returns></returns>
        public override byte[] AsBinary()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// The boundary of a linear ring is the same as the linear ring itself.
        /// </summary>
        /// <returns></returns>
        public override Geometry Boundary()
        {
            return this;
        }
        /// <summary>
        /// Tests if the geometry is equal
        /// </summary>
        /// <param name="geom">A geometry to test for equality</param>
        /// <returns>true if the other geometry is a linear ring and has the same points</returns>
        public override bool Equals(Geometry geom)
        {
            if (this as Object == null)
            {
                if (geom as Object == null) return true;
                return false;
            }
            if(geom.GetType() != typeof(LinearRing))
            {
                return false;
            }
            LinearRing lr = geom as LinearRing;
            if (lr.NumPoints != NumPoints) return false;
            for (int I = 0; I < Coordinates.Count; I++)
            {
                if (lr.Coordinates[I] != Coordinates[I]) return false;
            }
            return true;
        }
        /// <summary>
        /// Returns the number of points in the ring
        /// </summary>
        public int NumPoints
        {
            get
            {
                return Coordinates.Count;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override bool Disjoint(Geometry geom)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override bool Intersects(Geometry geom)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override bool Touches(Geometry geom)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override bool Crosses(Geometry geom)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override bool Within(Geometry geom)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override bool Contains(Geometry geom)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override bool Overlaps(Geometry geom)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="geom"></param>
        /// <param name="IntersectionPatternMatrix"></param>
        /// <returns></returns>
        public override bool Relate(Geometry geom, string IntersectionPatternMatrix)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override double Distance(Geometry geom)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Distance"></param>
        /// <returns></returns>
        public override Geometry Buffer(double Distance)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Geometry ConvexHull()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override Geometry Intersection(Geometry geom)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override Geometry Union(Geometry geom)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override Geometry Difference(Geometry geom)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override Geometry SymDifference(Geometry geom)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override LineSegment ShortestPathTo(Geometry geom)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion
    }
}
