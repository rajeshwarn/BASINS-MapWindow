using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Topology2D
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

        public double X2; // end point
        /// <summary>
        /// The Y coordinate of the End Point of the segment
        /// </summary>
        public double Y2; // end point


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new blank instance of a segment with 0 for values.
        /// </summary>
        public Segment()
        {
            X1 = 0.0;
            Y1 = 0.0;
            X2 = 0.0;
            Y2 = 0.0;
        }

        /// <summary>
        /// Creates a new segment from the vector, assuming that the startpoint of the segment
        /// is the origin and the endpoint of the segment is the tail of the vector.
        /// </summary>
        /// <param name="V">The Vector to create a segment from.</param>
        public Segment(Vector V)
        {
            X1 = 0;
            Y1 = 0;

            X2 = V.X;
            Y2 = V.Y;

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

            X2 = EndPoint.x;
            Y2 = EndPoint.y;


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

            X2 = EndPoint.X;
            Y2 = EndPoint.Y;

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

            X2 = StartPoint.X + StartToEnd.X;
            Y2 = StartPoint.Y + StartToEnd.Y;

        }
        /// <summary>
        /// Stores a set of double values that represents a line segment.
        /// </summary>
        /// <param name="x1">A Double representing the X coordinate of the beginning of the line segment</param>
        /// <param name="y1">A Double representing the Y coordinate of the beginning of the line segment</param>
        /// <param name="x2">A Double representing the X coordinate of the end of the line segment</param>
        /// <param name="y2">A Double representing the Y coordinate of the end of the line segment</param>
        public Segment(double x1, double y1, double x2, double y2)
        {
            X1 = x1;
            Y1 = y1;

            X2 = x2;
            Y2 = y2;

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

                return pt;
            }
            set
            {
                X1 = value.x;
                Y1 = value.y;

            }
        }
        /// <summary>
        /// A Point representing the beginning of the line segment
        /// </summary>
        public Point StartPoint
        {
            get
            {
                return new Point(X1, Y1);
            }
            set
            {
                X1 = value.X;
                Y1 = value.Y;

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

                return pt;
            }
            set
            {
                X2 = value.x;
                Y2 = value.y;

            }
        }
        /// <summary>
        /// A MapWinGIS.Point representing the end of the line segment
        /// </summary>
        public Point EndPoint
        {
            get
            {
                return new Point(X2, Y2);
            }
            set
            {
                X2 = value.X;
                Y2 = value.Y;

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
                return new Point((X2 + X1) / 2, (Y1 + Y2) / 2);
            }
            set
            {
                // translation vector from the current center to the new center
                Vector trans = new Vector(Center, value);
                StartPoint = trans.Plus(StartPoint.ToVector()).ToPoint();
                EndPoint = trans.Plus(EndPoint.ToVector()).ToPoint();
            }
        }

        /// <summary>
        /// Gets the Extents of this segment.
        /// </summary>
        public Extents Extents
        {
            get
            {
                // just create this on the fly.
                return new Extents(this);
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
            double mY;

            mY = (Y2 - Y1) / (X2 - X1);
            Intercept.Y = Y1 + mY * (Xcoordinate - X1);

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
            double mX;

            mX = (X2 - X1) / (Y2 - Y1);
            Intercept.X = X1 + mX * (Ycoordinate - Y1);

            return true;
        }


        #endregion

        #endregion

        #region Methods

        #region -------------------- COPY ---------------------------------

        /// <summary>
        /// Returns a new instance of the Topology2D.Segment class with the same values as this object.
        /// </summary>
        /// <returns>Topology2D.Segment with identical properties.</returns>
        public Segment Copy()
        {
            Segment NewSeg = new Segment(X1, Y1, X2, Y2);
            return NewSeg;
        }

        #endregion

        #region ------------------- CROSSES ----------------------------------

        /// <summary>
        /// Checks to see if two segments cross.  This will not return true if
        /// the endpoints touch, or the lines overlap.  Specifically the
        /// middle portions of the lines have to contact each other without
        /// involving any of the endpoints.
        /// </summary>
        /// <param name="Seg">The Segment to compare this segment to.</param>
        /// <returns>True if the lines completely cross each other.</returns>
        public bool Crosses(Segment Seg)
        {
            Segment Intsct = Intersection(Seg);
            if (Intsct == null) return false;
            if (Intsct.StartPoint != Intsct.EndPoint) return false;
            // If the contact point is an endpoint, the two segments touch, but
            // don't actually cross.
            if (Intsct.StartPoint == Seg.StartPoint) return false;
            if (Intsct.StartPoint == Seg.EndPoint) return false;
            if (Intsct.StartPoint == StartPoint) return false;
            if (Intsct.EndPoint == EndPoint) return false;
            return true;
            
        }

        #endregion

        #region ------------------- CLOSEST POINT TO -------------------------
        /// <summary>
        /// Returns a Point representing the closest point.
        /// </summary>
        /// <param name="Point">The point we want to be close to.</param>
        /// <returns>The point on this segment that is closest to the given point.</returns>
        public Point ClosestPointTo(Point Point)
        {
            // If the points defining this segment are the same, we treat the segment as a point
            // special handling to avoid 0 in denominator later

            if (X2 == X1 && Y2 == Y1)
            {
                return StartPoint;
            }
            //http://softsurfer.com/Archive/algorithm_0102/algorithm_0102.htm

            Vector Vseg = new Vector(this); // vector from p1 to p2 in the segment
            Vector W = new Vector(new Point(X1, Y1), Point); // vector from p1 to Point
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
            Pb = Pb.Plus(new Vector(X1, Y1));
            return Pb.ToPoint();
        }

        /// <summary>
        /// Determines the shortest path between this segment and the specified segment.
        /// The point returned will be the point on this segment that is the closest point
        /// to the other segment.
        /// </summary>
        /// <param name="LineSegment">A Topology2D.Segemnt to compare to this segment.</param>
        /// <returns>MapWinGeoProc.Topology2D.Point on this segment that is closest to the specified segment.</returns>
        public Point ClosestPointTo(Segment LineSegment)
        {
            Segment seg = ShortestPathTo(LineSegment);
            return seg.StartPoint;
        }

        /// <summary>
        /// Returns a Segment starting with the closest point on this segment to the specified point
        /// and terminating at the specified point.
        /// </summary>
        /// <param name="Pnt">The point to compare to this segment.</param>
        /// <returns>Topology2D Segment from this segemnt to the specified point deliniating the shortest path.</returns>
        public Segment ShortestPathTo(Point Pnt)
        {
            Segment Seg = new Segment();
            Seg.EndPoint = Pnt.Copy();
            Seg.StartPoint = ClosestPointTo(Pnt);
            return Seg;
        }
        /// <summary>
        /// This calculates the shortest pathway between two segments and returns a segment
        /// that represents the contact between the two.  The Startpoint is the point on
        /// this segment, and the Endpoint is the point on LineSegment
        /// </summary>
        /// <param name="LineSegment">The line segment to find the distance too</param>
        /// <returns>A Line Segment from the two closest points from each segment.</returns>
        /// <remarks>This has an untested change from the distance algortthm and may not work.</remarks>
        public Segment ShortestPathTo(Segment LineSegment)
        {
            Segment Path = new Segment();
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
            // the point on u :
            Path.StartPoint = StartPoint.ToVector().Plus(dU).ToPoint();
            Path.EndPoint = LineSegment.StartPoint.ToVector().Plus(dV).ToPoint();
            return Path;
        }


        /// <summary>
        /// This retrieves the shortest path, but only returns the point that represents
        /// the closest point to this segment that is located on the segment specified.
        /// </summary>
        /// <param name="LineSegment">The linesegment containing the point to find.</param>
        /// <returns>MapWinGeoProc.Topology2D.Point the closest point to this segment on the specified segemnt.</returns>
        public Point ClosestPointOn(Segment LineSegment)
        {
            Segment seg = ShortestPathTo(LineSegment);
            return seg.EndPoint;
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

            // If the point is not collinear, return false
            //http://mathworld.wolfram.com/Collinear.html
            // |(V2 - V1) x (V1 - V3)| = 0 for collinear points
            Vector V1 = new Vector(X1, Y1);
            Vector V2 = new Vector(X2, Y2);
            Vector V3 = new Vector(Point.X, Point.Y);
            Vector d1 = V1.Minus(V2);
            Vector d2 = V1.Minus(V3);
            double res = d1.Cross(d2);
            if (res == 0) return true;
            return false;
        }

        /// <summary>
        /// Will return true if the segments intersect or are coincidental.
        /// </summary>
        /// <param name="LineSegment">The line segment to compare with</param>
        /// <returns>Boolean, true if the line segments touch each other, false otherwise</returns>
        public bool IntersectsWith(Segment LineSegment)
        {

            if (Intersection(LineSegment) == null)return false;

            return true;
        }

        #endregion

        #region ------------------- INTERSECTION -----------------------------
        /// <summary>
        /// Returns the point where two segments intersect, if any exists.
        /// </summary>
        /// <param name="Seg">The segment to compare with this segment for an intersection.</param>
        /// <returns>A Segment representing the intersection.  Null if no intersection exists.
        /// Returns a degenerate segment where Startpoint == Endpoint if they intersect at a point.</returns>
        public Segment Intersection(Segment Seg)
        {
           // http://www.geometryalgorithms.com/Archive/algorithm_0104/algorithm_0104B.htm#intersect2D_SegSeg()

            Segment Overlap = null;
            
            // intersect2D_2Segments(): the intersection of 2 finite 2D segments
            const double SMALL_NUM = 0.00000001;
            Vector u = new Vector(this);
            Vector v = new Vector(Seg);
            Vector w = new Vector(this.StartPoint, Seg.StartPoint);
            double D = u.Cross(v); // perp(u,v); in 2 D cross is just the perpendicular

            // test if they are parallel (includes either being a point)
            if (Math.Abs(D) < SMALL_NUM)
            {
                    // S1 and S2 are parallel
                if (u.Cross(v) != 0 || v.Cross(w) != 0) // they are NOT collinear
                {
                    return null;    //null case              
                }
                // they are collinear or degenerate
                // check if they are degenerate points
                double du = u.Dot(u);
                double dv = v.Dot(v);
                if (du==0 && dv==0)
                {           // both segments are points
                    if (StartPoint.IntersectsWith(Seg.StartPoint)==false)return null; // they are distinct points
                    Overlap.StartPoint = StartPoint.Copy();
                    Overlap.EndPoint = StartPoint.Copy();
                    return Overlap;  // they are the same point
                }
                if (du==0)
                {                    // S1 is a single point
                    if (StartPoint.IntersectsWith(Seg) == false)return null;  // but is not in S2
                    Overlap.StartPoint = StartPoint.Copy();
                    Overlap.EndPoint = StartPoint.Copy();
                    return Overlap;
                }
                if (dv==0) {         // S2 a single point
                    if (Seg.StartPoint.IntersectsWith(this) == false)  // but is not in S1
                    Overlap.StartPoint = Seg.StartPoint.Copy();
                    Overlap.EndPoint = Seg.StartPoint.Copy();
                    return Overlap;
                }
                // they are collinear segments - get overlap (or not)
                double t0, t1;                   // endpoints of S1 in eqn for S2
                Vector w2 = new Vector(EndPoint, Seg.StartPoint);
                if (v.X != 0)
                {
                    t0 = w.X / v.X;
                    t1 = w2.X / v.X;
                }
                else
                {
                    t0 = w.Y / v.Y;
                    t1 = w2.Y / v.Y;
                }
                if (t0 > t1)
                {                  // must have t0 smaller than t1
                    double t=t0; 
                    t0=t1;
                    t1=t;    // swap if not
                }
                if (t0 > 1 || t1 < 0) 
                {
                    return null;     // NO overlap
                }
                if(t0 < 0) t0 = 0; // clip to min 0
                if(t1 > 1) t1 = 1; // clip to max 1
            
                if (t0 == t1) 
                {                 // intersect is a point
                    Point cross =  Seg.StartPoint.ToVector().Plus(v.Times(t0)).ToPoint();
                    Overlap.StartPoint = cross.Copy();
                    Overlap.EndPoint = cross.Copy();
                    return Overlap;
                }

                // they overlap in a valid subsegment
                // *I0 = S2.P0 + t0 * v;

                Overlap.StartPoint = Seg.StartPoint.ToVector().Plus(v.Times(t0)).ToPoint();
               
                //  *I1 = S2.P0 + t1 * v;
               Overlap.EndPoint = Seg.StartPoint.ToVector().Plus(v.Times(t1)).ToPoint();
               return Overlap;
            }

            // the segments are skew and may intersect in a point
            // get the intersect parameter for S1
            double sI = v.Cross(w) / D;
            if (sI < 0 || sI > 1)  return null;      // no intersect with S1
        

            // get the intersect parameter for S2
            double tI = u.Cross(w) / D;
            if (tI < 0 || tI > 1)  return null;      // no intersect with S2
             // compute S1 intersect point
            // *I0 = S1.P0 + sI * u; 
            Point CrossingPoint = StartPoint.ToVector().Plus(u.Times(sI)).ToPoint();
            Overlap.StartPoint = CrossingPoint.Copy();
            Overlap.EndPoint = CrossingPoint.Copy();
            return Overlap;
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
            if ((X1 == Seg.X1) && (Y1 == Seg.Y1)
            && (X2 == Seg.X2) && (Y2 == Seg.Y2))
            {
                return true;
            }
            if ((X1 == Seg.X2) && (Y1 == Seg.Y2)
            && (X2 == Seg.X1) && (Y2 == Seg.Y1))
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
            if (DistanceTo(Point) < Distance)return true;
            return false;
            
        }// End Contains Point

        /// <summary>
        /// Determines whether this segment is within the buffer Distance specified of the line segment specified
        /// </summary>
        /// <param name="Seg">The Segment to compare to this segment</param>
        /// <param name="Distance">Double, the maximum distance between segments to allow</param>
        /// <returns>Boolean, true if this segment is within the specified distance of the other segment.</returns>
        public bool IsWithinADistanceOf(Segment Seg, double Distance)
        {
            if (DistanceTo(Seg) < Distance)return true;
            return false;
        }
        #endregion

        #region ------------------- SHARES A LINE SEGMENT WITH ---------------

        /// <summary>
        /// Since these are already segments, we just determine if the intersection is a valid line segment.
        /// If the segments intersect as a point, or don't intersect, this will return false.
        /// </summary>
        /// <param name="Seg">Topology2D.Segment to compare with.</param>
        /// <returns>Boolean, true if the two lines intersect in a segment.</returns>
        public bool SharesALineSegmentWith(Segment Seg)
        {
            Segment ret = Intersection(Seg);
            if(ret == null)return false;
            if (ret.StartPoint == ret.EndPoint) return false;
            return true;
        }

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

        #region ------------------- TOUCHES THE BOUNDARY OF ---------------------
        /// <summary>
        /// This returns true if either endpoint touchest any of the boundaries of the extents.
        /// This does not distinguish between whether this object is inside or outside or 
        /// overlapping with the extent borders, simply that one endpoint touches.
        /// </summary>
        /// <param name="Ext">The extents to compare to.</param>
        /// <returns></returns>
        public bool TouchesTheBoundaryOf(Extents Ext)
        {
            if (Ext.IntersectWith(this) == false) return false;
            if (Ext.xMin == X1)
            {
                if (Ext.yMin <= Y1 && Y1 <= Ext.yMax) return true;
            }
            if (Ext.xMin == X2)
            {
                if (Ext.yMin <= Y2 && Y2 <= Ext.yMax) return true;
            }
            if (Ext.yMin == Y1)
            {
                if (Ext.xMin <= X1 && X1 <= Ext.xMax) return true;
            }
            if (Ext.yMax == Y2)
            {
                if (Ext.xMin <= X2 && X2 <= Ext.xMax) return true;
            }
            return false;
        }
        /// <summary>
        /// True if either endpoint is found in direct contact with any segment that makes up
        /// the polygon borders.
        /// </summary>
        /// <param name="Pgn">The polygon to compare against</param>
        /// <returns>Boolean, true if either endpoint of this segment touches any segment of the polygon.</returns>
        public bool TouchesTheBoundaryOf(Polygon Pgn)
        {
            List<Segment> Segs = null;
            if (Pgn.Extents.IntersectWith(StartPoint) == true)
            {
                Segs = Pgn.SegmentsWithin(this.Extents);
                for (int I = 0; I < Segs.Count; I++)
                {
                    if (StartPoint.IntersectsWith(Segs[I])) return true;
                }
            }
            else
            {
                if (Pgn.Extents.IntersectWith(EndPoint) == false) return false;
            }
            if (Segs == null)
            {
                Segs = Pgn.SegmentsWithin(this.Extents);
            }
            for (int I = 0; I < Segs.Count; I++)
            {
                if (EndPoint.IntersectsWith(Segs[I])) return true;
            }
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