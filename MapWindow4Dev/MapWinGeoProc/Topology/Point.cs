using System;
using System.Collections.Generic;
using System.Text;
namespace MapWinGeoProc.Topology
{

/// <summary>
/// Point structure with double values
/// This will store 3D X, Y and Z values, but has no vector functions.  
/// For that, use the vector class.
/// </summary>
    public class Point
    {
        #region Variables
        /// <summary>
        /// The X coordinate of the point
        /// </summary>
        public double X;
        /// <summary>
        /// The Y coordinate of the point
        /// </summary>
        public double Y;
        /// <summary>
        /// The Z coordinate of the point
        /// </summary>
        public double Z;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new, blank instance of a point where all the values are 0.
        /// </summary>
        public Point()
        {
            X = 0.0;
            Y = 0.0;
            Z = 0.0;
        }

        /// <summary>
        /// Creates a new instance of the Point structure
        /// </summary>
        /// <param name="Point">MapWinGIS.Point to convert into a Point</param>
        public Point(MapWinGIS.Point Point)
        {
            X = Point.x;
            Y = Point.y;
            Z = Point.Z;

        }

        /// <summary>
        /// Creates a new instance of the Point structure with the values specified
        /// </summary>
        /// <param name="x">Double, The coordinate in the X direction</param>
        /// <param name="y">Double, The coordinate in the Y direction</param>
        /// <param name="z">Double, The coordinate in the Z direction</param>
        public Point(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;

        }
        /// <summary>
        /// Creates a new 2D instance of Point which has 0 for the Z coordinate and the X, Y values specified
        /// </summary>
        /// <param name="x">Double, The X coordinate of the point</param>
        /// <param name="y">Double, The Y coordinate of the point</param>
        public Point(double x, double y)
        {
            X = x;
            Y = y;
            Z = 0;

        }
        /// <summary>
        /// Creates a new 3D point using the Vector U
        /// </summary>
        /// <param name="U">Vector where the X, Y, and Z values are translated into point coordinates</param>
        public Point(Vector U)
        {
            X = U.X;
            Y = U.Y;
            Z = U.Z;

        }
        #endregion

        #region Methods

        #region ----------------------- CLOSEST POINT ON ----------------------------

        /// <summary>
        /// Returns a Point corresponding to the closest point on LineSegment Seg to this point
        /// </summary>
        /// <param name="Seg">Returns the closest point on Seg to this point</param>
        /// <returns>A Point representing the closest location on Seg to this point</returns>
        public Point ClosestPointOn(LineSegment Seg)
        {
            return Seg.ClosestPointTo(this);
        }

        #endregion

        #region ----------------------- DISTANCE ---------------------------------
        /// <summary>
        /// Finds the euclidean distance between the points
        /// </summary>
        /// <param name="geom">The geometry to compare with this point</param>
        /// <returns>Double, the distance between the points</returns>
        public double Distance(Point geom)
        {
            return (new Vector(this, geom)).Magnitude;
        }

        /// <summary>
        /// Returns the shortest distance to the specified segment
        /// </summary>
        /// <param name="Seg">The segment to find the distance to</param>
        /// <returns>Double, the shortest distance to the specified segment</returns>
        public double Distance(LineSegment Seg)
        {
            return Seg.Distance(this);
        }
        #endregion

        #region ----------------------- INTERSECTS WITH -----------------------------
        /// <summary>
        /// Checks each value in the point to see if they are the same
        /// </summary>
        /// <param name="Point">The MapWinGIS.Point to compare with this point</param>
        /// <returns>Boolean, true if the points are the same</returns>
        public bool Intersects(MapWinGIS.Point Point)
        {
            if ((X == Point.x) && (Y == Point.y) && (Z == Point.Z)) return true;
            return false;
        }
        /// <summary>
        /// Checks each value in the point to see if they are the same
        /// </summary>
        /// <param name="Point">The Point to compare with this point</param>
        /// <returns>Boolean, true if the points are the same</returns>
        public bool Intersects(Point Point)
        {
            if ((X == Point.X) && (Y == Point.Y) && (Z == Point.Z)) return true;
            return false;
        }
        /// <summary>
        /// Checks to see of this point touches the two points of a segment or the line between them.
        /// </summary>
        /// <param name="Sgmt">A LineSegment representing the line segment to check</param>
        /// <returns>True if the point and line segment intersect</returns>
        public bool Intersects(LineSegment Sgmt)
        {
            // prevent redundant code and simply use the segemnt
            if (Sgmt.Intersects(this)) return true;
            return false;
        }
        /// <summary>
        /// Returns true if this point falls anywhere within the specified polygon
        /// or on the borders, or on the vertecies.
        /// </summary>
        /// <param name="Pgn">A Polygon to check.</param>
        /// <returns></returns>
        public bool Intersects(Polygon Pgn)
        {
            if (Pgn.Intersects(this)) return true;
            return false;
        }

        #endregion

        #region ----------------------- IS COMPLETELY WITHIN ------------------------

        /// <summary>
        /// Tests if this point intersects the Polygon but does not touch the border of it
        /// </summary>
        /// <param name="Pgn">A Polygon to test</param>
        /// <returns>Boolean, true if the point is inside the polygon without touching the border</returns>
        public bool IsCompletelyWithin(Polygon Pgn)
        {
            // Test intersect first to rule out any outside the extents
            if (Pgn.Intersects(this) == false) return false;
            if (TouchesTheBoundaryOf(Pgn) == true) return false;
            return true;
        }

        #endregion

        #region ----------------------- IS CONTAINED BY -----------------------------

        /// <summary>
        /// Tests if this point is within or touching the boundary of the Polygon
        /// but does not cross.  (Since points have no size, this is the same as intersect.)
        /// </summary>
        /// <param name="Pgn">A Polygon to test</param>
        /// <returns>Boolean, true if the point is inside the polygon even if it touches</returns>
        public bool Within(Polygon Pgn)
        {
            // This is more for esthetic completeness and is no different than intersect for points.
            return Pgn.Intersects(this);
        }

        #endregion

        #region ----------------------- IS IDENTICAL TO -----------------------------
        /// <summary>
        /// Checks each value in the point to see if they are the same
        /// </summary>
        /// <param name="Point">The Point to compare with this point</param>
        /// <returns>Boolean, true if the points are the same</returns>
        public bool IsIdenticalTo(Point Point)
        {
            if ((X == Point.X) && (Y == Point.Y) && (Z == Point.Z)) return true;
            return false;
        }


        /// <summary>
        /// Checks each value in the point to see if they are the same
        /// </summary>
        /// <param name="Point">The MapWinGIS.Point to compare with this point</param>
        /// <returns>Boolean, true if the points are the same</returns>
        public bool IsIdenticalTo(MapWinGIS.Point Point)
        {
            if ((X == Point.x) && (Y == Point.y) && (Z == Point.Z)) return true;
            return false;
        }
        #endregion

        #region ----------------------- IS WITHIN A DISTANCE OF ---------------------
        /// <summary>
        /// Tests a point to determine if it is within the specified distance to this 
        /// point.
        /// </summary>
        /// <param name="Pnt">A Point to test against this point.</param>
        /// <param name="Dist">The distance to check against inclusive.</param>
        /// <returns>Boolean, true if the point is within the specified distance.</returns>
        public bool IsWithinADistanceOf(Point Pnt, double Dist)
        {
            if (Distance(Pnt) <= Dist) return true;
            return false;
        }
        /// <summary>
        /// Tests a segment to determine if it is within the specified distance to this point.
        /// </summary>
        /// <param name="Seg">A segment to test agianst this point.</param>
        /// <param name="Dist">The distance to return.</param>
        /// <returns>Returns true if any part of the specified segment is closer than Distance.</returns>
        public bool IsWithinADistanceOf(LineSegment Seg, double Dist)
        {
            if (Seg.Distance(this) <= Dist) return true;
            return false;
        }
        #endregion

        #region ----------------------- TO MapWinGIS POINT --------------------------

        /// <summary>
        /// Creates a MapWinGIS.Point from this point.
        /// </summary>
        /// <returns>A MapWinGIS.Point with the same X, Y and Z values</returns>
        public MapWinGIS.Point To_mwPoint()
        {
            MapWinGIS.Point mwPoint = new MapWinGIS.Point();
            mwPoint.x = X;
            mwPoint.y = Y;
            mwPoint.Z = Z;
            return mwPoint;
        }
        #endregion

        #region ----------------------- TO VECTOR -----------------------------------
        /// <summary>
        /// Converts the point into a vector by assuming that the tail of the vector is the 
        /// origin (0,0,0)
        /// </summary>
        /// <returns></returns>
        public Vector ToVector()
        {
            return new Vector(this);
        }
        #endregion

        #region ----------------------- TOUCHES THE BOUNDARY OF ---------------------

        /// <summary>
        /// Indicates that the point specifically lies on the boundary of the polygon,
        /// and is not contained by the polygon.  This only applies to area features,
        /// for line or point features use Intersect.
        /// </summary>
        /// <param name="Pgn">the Polygon to test the boundaries of.</param>
        /// <returns>True if the point lies exactly on the boundary of the polygon.</returns>
        public bool TouchesTheBoundaryOf(Polygon Pgn)
        {
            // Checking extents is much faster.  Rule out any points that you can that way.
            if (Pgn.Envelope.Intersects(this) == false) return false;

            // Get a list of all the segments in the polygon
            List<LineSegment> SegList = Pgn.ToLineSegments();

            // Check for an intersection with each of the segments.
            for (int iSeg = 0; iSeg < SegList.Count; iSeg++)
            {
                if (SegList[iSeg].Intersects(this)) return true;
            }
            return false;
        }


        #endregion

        #endregion

    }
}