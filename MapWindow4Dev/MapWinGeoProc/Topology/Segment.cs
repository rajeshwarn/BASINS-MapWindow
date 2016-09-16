using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Topology
{
    /// <summary>
    /// Stores a set of double values that represents a line segment.
    /// </summary>
    public class Segment
    {
        #region Variables
        /// <summary>
        /// The X coordinate of the Start Point of the segment
        /// </summary>
        public double X1; // start point
        /// <summary>
        /// The Y coordinate of the Start Point of the segment
        /// </summary>
        public double Y1; // start point
        /// <summary>
        /// The Z coordinate of the Start Point of the segment
        /// </summary>
        public double Z1; // start point
        /// <summary>
        /// The X coordinate of the End Point of the segment
        /// </summary>
        public double X2; // end point
        /// <summary>
        /// The Y coordinate of the End Point of the segment
        /// </summary>
        public double Y2; // end point
        /// <summary>
        /// The Z coordinate of the End Point of the segment
        /// </summary>
        public double Z2; // end point

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new blank instance of the Segment class where all the values are 0.
        /// </summary>
        public Segment()
        {
            X1 = 0.0;
            Y1 = 0.0;
            Z1 = 0.0;
            X2 = 0.0;
            Y2 = 0.0;
            Z2 = 0.0;
        }

        /// <summary>
        /// Creates a new segment from the vector, assuming that the startpoint of the segment
        /// is the origin and the endpoint of the segment is the tail of the vector.
        /// </summary>
        /// <param name="V">The Vector to create a segment from.</param>
        public Segment(Vector V)
        {
            X1 = 0.0;
            Y1 = 0.0;
            Z1 = 0.0;
            X2 = V.X;
            Y2 = V.Y;
            Z2 = V.Z;
        }
        /// <summary>
        /// Stores a set of double values that represents a line segment.
        /// </summary>
        /// <param name="StartPoint">A MapWinGIS.Point representing the beginning of the line segment</param>
        /// <param name="EndPoint">A MapWinGIS.Point representing the end of the line segment</param>
        public Segment(MapWinGIS.Point StartPoint, MapWinGIS.Point EndPoint)
        {
            X1 = StartPoint.x;
            Y1 = StartPoint.y;
            Z1 = StartPoint.Z;
            X2 = EndPoint.x;
            Y2 = EndPoint.y;
            Z2 = EndPoint.Z;

        }
        /// <summary>
        /// Stores values representing a line segment
        /// </summary>
        /// <param name="StartPoint">A Topology.Point representing the start point of the segment</param>
        /// <param name="EndPoint">A Topology.Point representing the end point of the segment</param>
        public Segment(Point StartPoint, Point EndPoint)
        {
            X1 = StartPoint.X;
            Y1 = StartPoint.Y;
            Z1 = StartPoint.Z;
            X2 = EndPoint.X;
            Y2 = EndPoint.Y;
            Z2 = EndPoint.Z;
        }
        /// <summary>
        /// Creates a new segment from a point and a vector.  The point represents the start point
        /// or tail of the vector, and the vector is added to that point to find the second point.
        /// </summary>
        /// <param name="StartPoint">Point representing one of the points of the segment</param>
        /// <param name="StartToEnd">Vector representing the path of the segment</param>
        public Segment(Point StartPoint, Vector StartToEnd)
        {
            X1 = StartPoint.X;
            Y1 = StartPoint.Y;
            Z1 = StartPoint.Z;
            X2 = StartPoint.X + StartToEnd.X;
            Y2 = StartPoint.Y + StartToEnd.Y;
            Z2 = StartPoint.Z + StartToEnd.Z;
        }
        /// <summary>
        /// Stores a set of double values that represents a line segment.
        /// </summary>
        /// <param name="x1">A Double representing the X coordinate of the beginning of the line segment</param>
        /// <param name="y1">A Double representing the Y coordinate of the beginning of the line segment</param>
        /// <param name="z1">A Double representing the Z coordinate of the beginning of the line segment</param>
        /// <param name="x2">A Double representing the X coordinate of the end of the line segment</param>
        /// <param name="y2">A Double representing the Y coordinate of the end of the line segment</param>
        /// <param name="z2">A Double representing the Z coordinate of the end of the line segment</param>
        public Segment(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            X1 = x1;
            Y1 = y1;
            Z1 = z1;
            X2 = x2;
            Y2 = y2;
            Z2 = z2;
        }

        #endregion

        #region Properties
        /// <summary>
        /// A MapWinGIS.Point representing the beginning of the line segment
        /// </summary>
        public MapWinGIS.Point mwStartPoint
        {
            get
            {
                MapWinGIS.Point pt = new MapWinGIS.Point();
                pt.x = X1;
                pt.y = Y1;
                pt.Z = Z1;
                return pt;
            }
            set
            {
                X1 = value.x;
                Y1 = value.y;
                Z1 = value.Z;
            }
        }
        /// <summary>
        /// A Point representing the beginning of the line segment
        /// </summary>
        public Point StartPoint
        {
            get
            {
                return new Point(X1, Y1, Z1);
            }
            set
            {
                X1 = value.X;
                Y1 = value.Y;
                Z1 = value.Z;
            }
        }
        /// <summary>
        /// A MapWinGIS.Point representing the end of the line segment
        /// </summary>
        public MapWinGIS.Point mwEndPoint
        {
            get
            {
                MapWinGIS.Point pt = new MapWinGIS.Point();
                pt.x = X2;
                pt.y = Y2;
                pt.Z = Z2;
                return pt;
            }
            set
            {
                X2 = value.x;
                Y2 = value.y;
                Z2 = value.Z;
            }
        }
        /// <summary>
        /// A MapWinGIS.Point representing the end of the line segment
        /// </summary>
        public Point EndPoint
        {
            get
            {
                return new Point(X2, Y2, Z2);
            }
            set
            {
                X2 = value.X;
                Y2 = value.Y;
                Z2 = value.Z;
            }
        }

        /// <summary>
        /// A Double representing the length of the segment
        /// </summary>
        public double Length
        {
            get
            {
                return new Vector(this).Magnitude;
            }
        }

        // Gets or sets the centerpoint for this segment.  Changing the center will
        // keep the direction and size the same but translate the point.
        /// <summary>
        /// The geometric center of the line.  This is simply the average of the
        /// X, Y and Z coordinates.
        /// </summary>
        public Point Center
        {
            get
            {
                return new Point((X2 + X1) / 2, (Y1 + Y2) / 2, (Z1 + Z2) / 2);
            }
            set
            {
                // translation vector from the current center to the new center
                Vector trans = new Vector(Center, value);
                StartPoint = trans.Plus(StartPoint.ToVector()).ToPoint();
                EndPoint = trans.Plus(EndPoint.ToVector()).ToPoint();
            }
        }

        #region ----------------- INTERCEPT EQUATIONS ------------------------

        /// <summary>
        /// Given a segment, we try fix the value of one coordinate to solve
        /// for the other two.
        /// </summary>
        /// <param name="Xcoordinate">The value to use as X</param>
        /// <param name="Intercept">The location of the segment when X = Xcoordinate </param>
        /// <returns>Boolean.  False if there is no valid intersection.</returns>
        public bool LocationByX(double Xcoordinate, out Point Intercept)
        {
            Intercept = new Point();
            // if the X specified is beyond the range of X we cannot obtain a location
            if (Xcoordinate < X1 && Xcoordinate < X2) return false;
            if (Xcoordinate > X1 && Xcoordinate > X2) return false;

            // prevent division by 0 here
            if (X2 - X1 == 0) return false;

            Intercept.X = Xcoordinate;
            double mY, mZ;

            mY = (Y2 - Y1) / (X2 - X1);
            Intercept.Y = Y1 + mY * (Xcoordinate - X1);
            mZ = (Z2 - Z1) / (X2 - X1);
            Intercept.Z = Z1 + mZ * (Xcoordinate - X1);
            return true;
        }
        /// <summary>
        /// Given a segment, we try fix the value of one coordinate to solve
        /// for the other two.
        /// </summary>
        /// <param name="Ycoordinate">The value to use as Y</param>
        /// <param name="Intercept">The location of the segment when Y = Ycoordinate </param>
        /// <returns>Boolean.  False if there is no valid intersection.</returns>
        public bool LocationByY(double Ycoordinate, out Point Intercept)
        {
            Intercept = new Point();
            // if the X specified is beyond the range of X we cannot obtain a location
            if (Ycoordinate < Y1 && Ycoordinate < Y2) return false;
            if (Ycoordinate > Y1 && Ycoordinate > Y2) return false;

            // prevent division by 0 here
            if (Y2 - Y1 == 0) return false;

            Intercept.Y = Ycoordinate;
            double mX, mZ;

            mX = (X2 - X1) / (Y2 - Y1);
            Intercept.X = X1 + mX * (Ycoordinate - Y1);
            mZ = (Z2 - Z1) / (Y2 - Y1);
            Intercept.Z = Z1 + mZ * (Ycoordinate - Y1);
            return true;
        }

        /// <summary>
        /// Given a segment, we try fix the value of one coordinate to solve
        /// for the other two.
        /// </summary>
        /// <param name="Zcoordinate">The value to use as Z</param>
        /// <param name="Intercept">The location of the segment when Z = Zcoordinate </param>
        /// <returns>Boolean.  False if there is no valid intersection.</returns>
        public bool LocationByZ(double Zcoordinate, out Point Intercept)
        {
            Intercept = new Point();
            // if the X specified is beyond the range of X we cannot obtain a location
            if (Zcoordinate < Z1 && Zcoordinate < Z2) return false;
            if (Zcoordinate > Z1 && Zcoordinate > Z2) return false;

            // prevent division by 0 here
            if (Z2 - Z1 == 0) return false;

            Intercept.Z = Zcoordinate;
            double mY, mX;

            mY = (Y2 - Y1) / (Z2 - Z1);
            Intercept.Y = Y1 + mY * (Zcoordinate - Z1);
            mX = (X2 - X1) / (Z2 - Z1);
            Intercept.X = X1 + mX * (Zcoordinate - Z1);
            return true;
        }
        #endregion

        #endregion

        #region Methods

        #region ------------------- CROSSES ----------------------------------

        /// <summary>
        /// Checks to see if two segments cross.  This will not return true if
        /// the endpoints touch, or the lines are identical.  Specifically the
        /// middle portions of the lines have to contact each other.
        /// </summary>
        /// <param name="Seg">The Segment to compare this segment to.</param>
        /// <returns>True if the lines completely cross each other.</returns>
        public bool Crosses(Segment Seg)
        {
            if (Touches(Seg) == true) return false;
            if (DistanceTo(Seg) == 0) return true;
            return false;
        }

        #endregion

        #region ------------------- CLOSEST POINT TO -------------------------
        /// <summary>
        /// Returns a Point representing the closest point.
        /// </summary>
        /// <param name="Point"></param>
        /// <returns></returns>
        public Point ClosestPointTo(Point Point)
        {
            // If the points defining this segment are the same, we treat the segment as a point
            // special handling to avoid 0 in denominator later

            if (X2 == X1 && Y2 == Y1 && Z2 == Z1)
            {
                return StartPoint;
            }
            //http://softsurfer.com/Archive/algorithm_0102/algorithm_0102.htm

            Vector Vseg = new Vector(this); // vector from p1 to p2 in the segment
            Vector W = new Vector(new Point(X1, Y1, Z1), Point); // vector from p1 to Point
            double c1 = W.Dot(Vseg); // the dot product represents the projection onto the line
            if (c1 < 0)
            {
                // The closest point on the segment to Point is p1
                return StartPoint;
            }
            double c2 = Vseg.Magnitude;
            if (c2 <= c1)
            {
                // The closest point on the segment to Point is p2
                return EndPoint;
            }
            // The closest point on the segment is perpendicular to the point, 
            // but somewhere on the segment between P1 and P2 
            double b = c1 / c2;
            Vector Pb = Vseg.Times(b);
            Pb = Pb.Plus(new Vector(X1, Y1, Z1));
            return Pb.ToPoint();
        }

        #endregion

        #region ------------------- DISTANCE TO ------------------------------

        /// <summary>
        /// Calculates the shortest distance to this line segment from the specified MapWinGIS.Point
        /// </summary>
        /// <param name="Point">A MapWinGIS.Point specifing the location to find the distance to the line</param>
        /// <returns>A double value that is the shortest distance from the given Point to this line segment</returns>          
        public double DistanceTo(Point Point)
        {
            Vector Dist; // A vector representing the shortest distance
            Point pt = ClosestPointTo(Point);
            Dist = new Vector(pt, Point);
            return Dist.Magnitude;
        }

        /// <summary>
        /// Determines the shortest distance between two segments
        /// </summary>
        /// <param name="LineSegment">Segment, The line segment to test against this segment</param>
        /// <returns>Double, the shortest distance between two segments</returns>
        public double DistanceTo(Segment LineSegment)
        {
            //http://www.geometryalgorithms.com/Archive/algorithm_0106/algorithm_0106.htm
            const double SMALL_NUM = 0.00000001;
            Vector u = new Vector(this); // Segment 1
            Vector v = new Vector(LineSegment); // Segment 2
            Vector w = new Vector(this.StartPoint, LineSegment.StartPoint);
            double a = u.Dot(u);  // length of segment 1
            double b = u.Dot(v);  // length of segment 2 projected onto line 1
            double c = v.Dot(v);  // length of segment 2
            double d = u.Dot(w);  // 
            double e = v.Dot(w);
            double D = a * c - b * b;
            double sc, sN, sD = D;
            double tc, tN, tD = D;
            // compute the line parameters of the two closest points
            if (D < SMALL_NUM)  // the lines are almost parallel
            {                // force using point P0 on segment 1
                sN = 0.0;    // to prevent possible division by 0 later
                sD = 1.0;
                tN = e;
                tD = c;
            }
            else // get the closest points on the infinite lines
            {
                sN = (b * e - c * d);
                tN = (a * e - b * d);
                if (sN < 0.0) // sc < 0 => the s=0 edge is visible
                {
                    sN = 0.0;
                    tN = e;
                    tD = c;
                }
                else if (sN > sD) // sc > 1 => the s=1 edge is visible
                {
                    sN = sD;
                    tN = e + b;
                    tD = c;
                }
            }
            if (tN < 0.0) // tc < 0 => the t=0 edge is visible
            {
                tN = 0.0;
                // recompute sc for this edge
                if (-d < 0.0)
                {
                    sN = 0.0;
                }
                else if (-d > a)
                {
                    sN = sD;
                }
                else
                {
                    sN = -d;
                    sD = a;
                }
            }
            else if (tN > tD) // tc > 1 => the t = 1 edge is visible
            {
                // recompute sc for this edge
                if ((-d + b) < 0.0)
                {
                    sN = 0;
                }
                else if ((-d + b) > a)
                {
                    sN = sD;
                }
                else
                {
                    sN = (-d + b);
                    sD = a;
                }
            }
            // finally do the division to get sc and tc
            if (Math.Abs(sN) < SMALL_NUM)
            {
                sc = 0.0;
            }
            else
            {
                sc = sN / sD;
            }
            if (Math.Abs(tN) < SMALL_NUM)
            {
                tc = 0.0;
            }
            else
            {
                tc = tN / tD;
            }
            // get the difference of the two closest points
            Vector dU = u.Times(sc);
            Vector dV = v.Times(tc);
            Vector dP = (w.Plus(dU)).Minus(dV);
            // S1(sc) - S2(tc)
            return dP.Magnitude;
        }

        /// <summary>
        /// Determines the shortest distance to any of the lines in the polyline.
        /// </summary>
        /// <param name="PolyLn">The polyline to investigate</param>
        /// <returns>Double, the distance to the closest segment</returns>
        public double DistanceTo(PolyLine PolyLn)
        {
            //I'm not sure yet how to do this faster.  Extents won't really help here.
            List<Segment> Segs = PolyLn.ToSegments();
            double dist = double.PositiveInfinity;
            for (int iSeg = 0; iSeg < Segs.Count; iSeg++)
            {
                double test = Segs[iSeg].DistanceTo(this);
                if (test < dist) dist = test;
            }
            return dist;
        }

        #endregion

        #region ------------------- INTERSECTS WITH --------------------------
        /// <summary>
        /// Determines whether the point falls on a line segment
        /// </summary>
        /// <param name="Point">The MapWinGIS.Point to test agianst this segment</param>
        /// <returns>Boolean, true if the point is on the segment</returns>
        public bool IntersectsWith(MapWinGIS.Point Point)
        {
            return IntersectsWith(new Point(Point));
        }

        /// <summary>
        /// Determines whether the point falls on a line segment
        /// </summary>
        /// <param name="Point">The Point to test agianst this segment</param>
        /// <returns>Boolean, true if the point is on the segment</returns>
        public bool IntersectsWith(Point Point)
        {
            //http://mathworld.wolfram.com/Collinear.html

            // If the point is outside the extents of the segment, return false
            if (Point.X < X1 && Point.X < X2) return false;
            if (Point.X > X1 && Point.X > X2) return false;
            if (Point.Y < Y1 && Point.Y < Y2) return false;
            if (Point.Y > Y1 && Point.Y > Y2) return false;
            if (Point.Z < Z1 && Point.Z < Z2) return false;
            if (Point.Z > Z1 && Point.Z > Z2) return false;
            // If the point is not collinear, return false
            //http://mathworld.wolfram.com/Collinear.html
            // |(V2 - V1) x (V1 - V3)| = 0 for collinear points
            Vector V1 = new Vector(X1, Y1, Z1);
            Vector V2 = new Vector(X2, Y2, Z2);
            Vector V3 = new Vector(Point.X, Point.Y, Point.Z);
            Vector d1 = V1.Minus(V2);
            Vector d2 = V1.Minus(V3);
            Vector res = d1.Cross(d2);
            if (res.Magnitude == 0) return true;
            return false;
        }

        /// <summary>
        /// Will return true if the segments intersect or are coincidental.
        /// </summary>
        /// <param name="LineSegment">The line segment to compare with</param>
        /// <returns>Boolean, true if the line segments touch each other, false otherwise</returns>
        public bool IntersectsWith(Segment LineSegment)
        {
            if (DistanceTo(LineSegment) == 0) return true;
            return false;
        }

        #endregion

        #region ------------------- IS COMPLETELY WITHIN ---------------------

        /// <summary>
        /// Returns true if the entire segment is within the polygon,
        /// without touching or crossing any of the edges.
        /// </summary>
        /// <param name="Pgn">The polygon to investigate</param>
        /// <returns>Boolean, true if the segment is completely inside the polygon</returns>
        public bool IsCompletelyWithin(Polygon Pgn)
        {
            // check against the polygon extents first for speed.
            if (Pgn.Extents.IntersectWith(this) == false) return false;

            // -- TO DO -- 

            //if (Pgn.IntersectsWith(this) == false) return false;

            return true;
        }

        #endregion

        #region ------------------- IS IDENTICAL TO --------------------------

        /// <summary>
        /// Checks the points of the segemnts to see if they are the same,
        /// but is unconcerned with direction.  
        /// </summary>
        /// <param name="Seg">The Segment to compare against this segment</param>
        /// <returns>True if the segment points are the same.  Sequence doesn't matter.</returns>
        public bool IsIdenticalTo(Segment Seg)
        {
            if ((X1 == Seg.X1) && (Y1 == Seg.Y1) && (Z1 == Seg.Z1)
            && (X2 == Seg.X2) && (Y2 == Seg.Y2) && (Z2 == Seg.Z2))
            {
                return true;
            }
            if ((X1 == Seg.X2) && (Y1 == Seg.Y2) && (Z1 == Seg.Z2)
            && (X2 == Seg.X1) && (Y2 == Seg.Y1) && (Z2 == Seg.Z1))
            {
                return true;
            }
            // We are not considering segments to be directed
            return false;
        }

        #endregion

        #region ------------------- IS PARALLEL TO ---------------------------
        /// <summary>
        /// Returns true if the line segments are parallel
        /// </summary>
        /// <param name="LineSegment">The Topology.SegementD Segment to compare to </param>
        /// <returns>Boolean, true if the segments are parallel</returns>
        public bool IsParallelTo(Segment LineSegment)
        {
            Vector u = new Vector(this);
            Vector v = new Vector(LineSegment);
            // if Abs(u.v) is = |u||v|, the segments are parallel
            if (Math.Abs(u.Dot(v)) == u.Magnitude * v.Magnitude) return true;
            return false;
        }
        #endregion

        #region ------------------- IS PERPENDICULAR TO ----------------------

        /// <summary>
        /// Returns true if the line segments are perpendicular
        /// </summary>
        /// <param name="LineSegment">The Segment line segment to compare this segment with</param>
        /// <returns>Boolean, true if the segments are perpendicular</returns>
        public bool IsPerpendicularTo(Segment LineSegment)
        {
            Vector u = new Vector(this);
            Vector v = new Vector(LineSegment);
            if (u.Dot(v) == 0) return true;
            return false;
        }
        #endregion

        #region ------------------- IS WITHIN A DISTANCE OF ------------------

        /// <summary>
        /// Determines whether the point is within the buffer region of the line segment
        /// </summary>
        /// <param name="Point">A MapWinGIS.Point to check agianst this line segment</param>
        /// <param name="Distance">The double distance the point can be from the line and still be considered contained</param>
        /// <returns>Boolean, true if the point is within the buffer distance of the line segment</returns>
        public bool IsWithinADistanceOf(MapWinGIS.Point Point, double Distance)
        {
            return IsWithinADistanceOf(new Point(Point), Distance);
        }

        /// <summary>
        /// Determines whether the point is within the buffer region of the line segment
        /// </summary>
        /// <param name="Point">A Point to check agianst this line segment</param>
        /// <param name="Distance">The double distance the point can be from the line and still be considered contained</param>
        /// <returns>Boolean, true if the point is within the buffer distance of the line segment</returns>
        public bool IsWithinADistanceOf(Point Point, double Distance)
        {
            if (DistanceTo(Point) < Distance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }// End Contains Point

        #endregion

        #region ------------------- TOUCHES ----------------------------------
        /// <summary>
        /// Checks to see if two segments touch.  This is true if a vertex from
        /// one segment intersects with the other segment.  Does not return true
        /// if only the line areas touch one another.
        /// </summary>
        /// <param name="Seg">A Segment to compare to.</param>
        /// <returns>Boolean, true if an endpoint intersects with a line.</returns>
        public bool Touches(Segment Seg)
        {
            if (StartPoint.IntersectsWith(Seg)) return true;
            if (EndPoint.IntersectsWith(Seg)) return true;
            if (Seg.StartPoint.IntersectsWith(this)) return true;
            if (Seg.EndPoint.IntersectsWith(this)) return true;
            return false;
        }

        #endregion

        #region ------------------- TO VECTOR --------------------------------

        /// <summary>
        /// Creates a new vector from this segment where the StartPoint becomes
        /// the beginning of the vector and the end point is the tip of the vector.
        /// </summary>
        /// <returns>A Vector from Point 1 to Point 2 in this segment</returns>
        public Vector ToVector()
        {
            return new Vector(this);
        }

        #endregion

        #endregion

    }
}