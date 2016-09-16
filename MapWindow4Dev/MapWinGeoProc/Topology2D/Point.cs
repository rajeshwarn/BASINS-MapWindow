using System;
using System.Collections.Generic;
using System.Text;
namespace MapWinGeoProc.Topology2D
{
    // mathematical classes
    /// <summary>
    /// In the event that we don't have MapWinGIS, this provides a useful point class with double values
    /// This will store 3D X, Y and Z values, but has no vector functions.  For that, use the vector class.
    /// </summary>
    public class Point : Geometry
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

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new blank instance of the Point class
        /// </summary>
        public Point()
        {
            X = 0.0;
            Y = 0.0;
        }
        /// <summary>
        /// Creates a point (which is a fully fleshed out geometry) from a coordinate
        /// which only has a basic pair of values.
        /// </summary>
        /// <param name="Coord">Point with the same X and Y as the coordinate</param>
        public Point(Coordinate Coord)
        {
            X = Coord.X;
            Y = Coord.Y;
        }

        /// <summary>
        /// Creates a new instance of the Point class
        /// </summary>
        /// <param name="MapWinGIS_Point">MapWinGIS.Point to convert into a Point</param>
        public Point(object MapWinGIS_Point)
        {
            // This sneaky bit of business allows the same physical call
            // but using a non-spacific object.
            Point pt = GeometryFactory.CreatePoint(MapWinGIS_Point);
            X = pt.X;
            Y = pt.Y;
        }

        /// <summary>
        /// Creates a new instance of the Point class with the values specified
        /// </summary>
        /// <param name="x">Double, The coordinate in the X direction</param>
        /// <param name="y">Double, The coordinate in the Y direction</param>

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Creates a new 3D point using the Vector U
        /// </summary>
        /// <param name="U">Vector where the X, Y, and Z values are translated into point coordinates</param>
        public Point(Vector U)
        {
            X = U.X;
            Y = U.Y;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Points are 0 dimensional.  This may help determine the dimensionality if you don't 
        /// know what type of geometry you are working with.
        /// </summary>
        public override int Dimension
        {
            get
            {
                return 0;
            }
        }

        #endregion

        #region Methods

        #region  --------------- Geometry Boolean Relations ---------------------------------
        /// <summary>
        /// This doesn't exist because a point by definition cannot contain anything.
        /// Calling this for points will always produce a value of false because 
        /// points cannot contain anything.
        /// </summary>
        /// <param name="geom">Any specific type of geometry</param>
        /// <returns>Boolean, Always False</returns>
        public override bool Contains(Geometry geom)
        {
            return false;
        }
        /// <summary>
        /// This doesn't exist because a point by definition cannot exist both on 
        /// the interior and exterior of any geometry.  Calling this for points will
        /// always produce a value of false because points cannot cross anything.
        /// </summary>
        /// <param name="geom">Geometry</param>
        /// <returns>Boolean, Always False</returns>
        public override bool Crosses(Geometry geom)
        {
            return false;
        }
        /// <summary>
        /// Tests whether or not this point is disjoint with the specified geometry.
        /// </summary>
        /// <param name="geom">Any type of geometry, like Point LineSegment, Polygon etc.</param>
        /// <returns>Boolean, true if the interior or borders of the geometry don't come in contact with point.</returns>
        public override bool Disjoint(Geometry geom)
        {
            if (geom.GetType() == typeof(Point))
            {
                Point Pnt = (Point)geom;
                if (Pnt.X == X && Pnt.Y == Y) return false;
                return true;
            }
            // For more complex shapes, use the disjoint defined in the complex shape
            return geom.Disjoint(this);

        }
        /// <summary>
        /// This function has been overriden to compare the Points in the mathematical sense.
        /// If the components of the vector are equal, then this will return true.
        /// </summary>
        /// <param name="geom">A geometry object to compare with this object</param>
        /// <returns>Boolean, true if the X and Y components are equal.</returns>
        public override bool Equals(Geometry geom)
        {
            if (geom.GetType() == typeof(Point))
            {
                Point Pnt = (Point)geom;
                if ((X == Pnt.X) && (Y == Pnt.Y)) return true;
                return false;
            }
            // if it isn't a point, it can't be equal to this geometry
            return false;
        }

        /// <summary>
        /// Checks to see if this point intersects with the specified geometry.
        /// </summary>
        /// <param name="geom">The geometry to compare with this point</param>
        /// <returns>Boolean, true if the interior or boundary of the specified geometry intersects with this point</returns>
        public override bool Intersects(Geometry geom)
        {
            // Points are hard coded
            if (geom.GetType() == typeof(Point))
            {
                Point Pnt = (Point)geom;
                if ((X == Pnt.X) && (Y == Pnt.Y)) return true;
                return false;
            }
            // other intersection possibilities are coded in the more advanced shapes
            return geom.Intersects(this);

        }

        /// <summary>
        /// Points cannot satisfy the critera for overlap, so this is always false.
        /// </summary>
        /// <param name="geom">Any geomtery</param>
        /// <returns>Boolean, Always False.</returns>
        public override bool Overlaps(Geometry geom)
        {
            return false;

        }

        /// <summary>
        /// Determines if this point touches the specified point.
        /// </summary>
        /// <param name="geom">Any geometry</param>
        /// <returns>Boolean, True if this point touches the geometry, false otherwise.</returns>
        public override bool Touches(Geometry geom)
        {
            if (geom.GetType() == typeof(Point))
            {
                Point Pnt = (Point)geom;
                if (Pnt.X == X && Pnt.Y == Y) return true;
            }
            // For higher level functions, this will be defined there
            return geom.Touches(this);
        }

        /// <summary>
        /// Determines if this point is inside another geometry.  This will return false 
        /// unless the other geometry is closed, like a Polygon or an Envelope.  This
        /// will not return true if the point lies on the border of the shape.
        /// </summary>
        /// <param name="geom">Any object derived from the geometry class.</param>
        /// <returns>Boolean, True if this point is found only on the interior of the specified shape.</returns>
        public override bool Within(Geometry geom)
        {
            Type tp = geom.GetType();
            if (tp == typeof(Point) || tp == typeof(LineSegment) || tp == typeof(LineString)) return false;
            return geom.Contains(this);
        }

        #endregion

       
       


        /// <summary>
        /// Not implemneted
        /// </summary>
        /// <returns></returns>
        public override string AsText()
        {
            return "POINT(" + X + " " + Y + ")";

        }
        /// <summary>
        /// Not implemented
        /// </summary>
        /// <returns></returns>
        public override byte[] AsBinary()
        {
            // To DO - Forest's Binary 
            throw new Exception("The method or operation is not implemented for the given value.");
        }

        /// <summary>
        /// Returns a Point equivalent to this point
        /// </summary>
        /// <returns>For points this is just the same point</returns>
        public override Geometry Boundary()
        {
            return this.Copy();
        }

       

        #region ------------  Advanced Geometry stuff
        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="geom"></param>
        /// <param name="IntersectionPatternMatrix"></param>
        /// <returns></returns>
        public override bool Relate(Geometry geom, string IntersectionPatternMatrix)
        {
            
            throw new Exception("The method or operation is not implemented.");
        }
       
        /// <summary>
        /// Returns a Polygon with the buffer distance with an assumed value of 8 for the number of points.
        /// </summary>
        /// <param name="Distance">The Radius representing the buffer distance around the point.</param>
        /// <returns>A Geometry.  Returns a Polygon unless Distance = 0, in which case it returns a copy of this point.</returns>
        public override Geometry Buffer(double Distance)
        {
            return Buffer(Distance, 8);
        }

        /// <summary>
        /// Returns a polygonal circle of Radius around this point.
        /// </summary>
        /// <param name="Distance">The distance in map units of the radius of the buffer</param>
        /// <param name="numPoints">Controls the number of points used to create the circle. 
        /// More points will be smoother.  Requires at least 3 points to be a valid polygon.</param>
        /// <returns>Topology2D.Polygon representing a circle of radius r.
        /// Or a copy of this Topology2D.Point if the Radius is 0</returns>
        public Geometry Buffer(double Distance, int numPoints)
        {
            if (numPoints < 3)
            {
                throw new ArgumentException("An invalid number of points was passed.  numPoints must be >= 3");
            }
            // A zero radius compels us just to return the point sent to us
            if (Distance == 0)
            {
                return this.Copy();
            }

            Polygon Buff = new Polygon();
            // Radius can be negative... we just take the absolute value.
            if (Distance < 0) Distance = -Distance;
            double dAng = 2 * Math.PI / numPoints;
            Angle ang = new Angle(0);
            for (int pt = 0; pt < numPoints; pt++)
            {
                ang.Radians -= dAng; // Clockwise = non-hole
                Buff.Shell.Coordinates.Add(new Coordinate(
                    this.X + Distance * Math.Cos(ang.Radians),
                    this.Y + Distance * Math.Sin(ang.Radians)));
            }

            return Buff;
        }
        /// <summary>
        /// This is undefined for points, so this will return a null value.
        /// </summary>
        /// <returns>null</returns>
        public override Geometry ConvexHull()
        {
            return null;
        }
        /// <summary>
        /// Not Implemented 
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override Geometry Difference(Geometry geom)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// Finds the shortest distance between this point and the specified geometry.
        /// </summary>
        /// <param name="geom">Any geometry that you wish to find the shortest path to.</param>
        /// <returns>Double, the distance of the shortest path between this object and the specified geometry.</returns>
        public override double Distance(Geometry geom)
        {
            if(geom.GetType() == typeof(Point))
            {
                Point Pnt = (Point)geom;
                return Math.Sqrt((X - Pnt.X) * (X - Pnt.X) + (Y - Pnt.Y) * (Y - Pnt.Y));
            }
            return geom.Distance(this);
        }

        /// <summary>
        /// This can either be null, if there is no intersection, or this point, if
        /// there is an intersection.
        /// </summary>
        /// <param name="geom">Any geometry.</param>
        /// <returns>A Point showing the location of the intersection.</returns>
        public override Geometry Intersection(Geometry geom)
        {
            // Only handle point-point intersections here
            if (geom.GetType() == typeof(Point))
            {
                Point Pnt = (Point)geom;
                if ((X == Pnt.X) && (Y == Pnt.Y)) return this;
                return null;
            }
            // The code for intersections with more advanced shapes is found in the more advanced shape
            return geom.Intersection(this);
        }
        /// <summary>
        /// The symetrical difference has no meaning for points, so this will always return null.
        /// </summary>
        /// <param name="geom">Any geometry</param>
        /// <returns>Null</returns>
        public override Geometry SymDifference(Geometry geom)
        {
            return null;
        }
        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "POINT(" + X.ToString() + " " + Y.ToString() + ")";
        }
        /// <summary>
        /// Creates a new Coordinate from this point
        /// </summary>
        /// <returns>A Coordinate</returns>
        public Coordinate ToCoordinate()
        {
            return new Coordinate(X, Y);
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override Geometry Union(Geometry geom)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion


        #region -------------------- COPY ---------------------------------

        /// <summary>
        /// Returns a new instance of the Topology2D.Point class with the same values as this object.
        /// </summary>
        /// <returns>Point with identical properties.</returns>
        public Point Copy()
        {
            Point NewPoint = new Point(X, Y);
            return NewPoint;
        }

        #endregion


        #region -------------------- Equals ---------------------------
       
       

        /// <summary>
        /// This is for the generic object case
        /// </summary>
        /// <param name="obV">Any object to compare with this object, but if the object isn't a point they will not be equal</param>
        /// <returns>Boolean, true if the object is a point and the two points have the same location.</returns>
        public override bool Equals(object obV)
        {
            // This can't ever be null
            if (obV as Object == null) return false;
            if (obV.GetType() == typeof(Coordinate))
            {
                Coordinate V = obV as Coordinate;
                if (V.X == X && V.Y == Y) return true;
                return false;
            }
            if (obV.GetType() == typeof(Point))
            {
                Point V = obV as Point;
                if (V.X == X && V.Y == Y) return true;
                return false;
            }
            if (obV.GetType() == typeof(MapWinGIS.Point))
            {
                MapWinGIS.Point V = obV as MapWinGIS.Point;
                if (V.x == X && V.y == Y) return true;
                return false;
            }
            return false;
        }

        /// <summary>
        /// Returns a hash code, whatever that is.
        /// </summary>
        /// <returns>A hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

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
            List<LineSegment> SegList = Pgn.Shell.ToLineSegments();

            // Check for an intersection with each of the segments.
            for (int iSeg = 0; iSeg < SegList.Count; iSeg++)
            {
                if (SegList[iSeg].Intersects(this)) return true;
            }
            return false;
        }
        /// <summary>
        /// Checks to see if this point lies on the boundaries of the extents specified.
        /// </summary>
        /// <param name="Ext">The extents to compare with.</param>
        /// <returns>True if the point lies anywhere on the boundaries of the extents.</returns>
        public bool TouchesTheBoundaryOf(Envelope Ext)
        {
            if (Ext.Intersects(this) == false) return false;
            if (Ext.xMax == X || Ext.xMin == X) return true;
            if (Ext.yMax == Y || Ext.yMax == Y) return true;
            return false;
        }

        #endregion

        /// <summary>
        /// Determines the shortest path to the specified geometry.  Returns a line segment with
        /// this Point as the startpoint and the closest point on the specified geometry as the
        /// endpoint.  Will return null if this point intersects with the specified geometry.
        /// </summary>
        /// <param name="geom">Line Segment</param>
        /// <returns>The shortest LineSegment from this point to the specified geometry.</returns>
        public override LineSegment ShortestPathTo(Geometry geom)
        {
            Type tp = geom.GetType();
            if (tp == typeof(Point))
            {
                return new LineSegment(this, (Point)geom);
            }
            return ShortestPathTo(this).Reverse();
        }
        #endregion

        #region Operators

        #region -------------------- EQUAL -------------------------------
        /// <summary>
        /// Returns true if X and Y coordinates are equal
        /// </summary>
        /// <param name="U">A Point to compare</param>
        /// <param name="objV">Any object to compare with the point</param>
        /// <returns>Boolean, true if the vectors are the same</returns>
        public static bool operator ==(Point U, Object  objV)
        {
            if (objV == null)
            {
                if ((object)U == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (objV.GetType() == typeof(Coordinate))
                {
                    Coordinate V = objV as Coordinate;
                    if (V.X == U.X && V.Y == U.Y) return true;
                    return false;
                }
                if (objV.GetType() == typeof(Point))
                {
                    Point V = objV as Point;
                    if (V.X == U.X && V.Y == U.Y) return true;
                    return false;
                }
                if (objV.GetType() == typeof(MapWinGIS.Point))
                {
                    MapWinGIS.Point V = objV as MapWinGIS.Point;
                    if (V.x == U.X && V.y == U.Y) return true;
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Returns true if X and Y coordinates are different
        /// </summary>
        /// <param name="U">A Point to compare</param>
        /// <param name="objV">An object to compare</param>
        /// <returns>Boolean, true if the vectors are the different</returns>
        public static bool operator !=(Point U, Object objV)
        {
            if (objV.GetType() == typeof(Coordinate))
            {
                Coordinate V = objV as Coordinate;
                if (V.X == U.X && V.Y == U.Y) return false;
                return true;
            }
            if (objV.GetType() == typeof(Point))
            {
                Point V = objV as Point;
                if (V.X == U.X && V.Y == U.Y) return false;
                return true;
            }
            if (objV.GetType() == typeof(MapWinGIS.Point))
            {
                MapWinGIS.Point V = objV as MapWinGIS.Point;
                if (V.x == U.X && V.y == U.Y) return false;
                return true;
            }
            return true;
        }
        
        #endregion 

        #endregion

    }

}
