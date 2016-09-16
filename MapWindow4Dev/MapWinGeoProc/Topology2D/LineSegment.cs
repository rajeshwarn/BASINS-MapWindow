using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Topology2D
{

    /// <summary>
    /// Stores a set of double values that represents a line segment.
    /// </summary>
    public class LineSegment : Geometry
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
        public LineSegment()
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
        public LineSegment(Vector V)
        {
            X1 = 0;
            Y1 = 0;

            X2 = V.X;
            Y2 = V.Y;

        }

        /// <summary>
        /// Creates a new instance of a segment using a list of two coordinates
        /// </summary>
        /// <param name="Coords">A list of coordinates to use when building the segment</param>
        public LineSegment(List<Coordinate> Coords)
        {
            if (Coords == null) return;
            if (Coords.Count != 2)
            {
                throw new ArgumentException("LineSegment requires 2 points.");
            }
            StartPoint = new Point(Coords[0]);
            EndPoint = new Point(Coords[1]);
        }
        /// <summary>
        /// Stores a set of double values that represents a line segment.
        /// </summary>
        /// <param name="StartPoint">A MapWinGIS.Point representing the beginning of the line segment</param>
        /// <param name="EndPoint">A MapWinGIS.Point representing the end of the line segment</param>
        public LineSegment(MapWinGIS.Point StartPoint, MapWinGIS.Point EndPoint)
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
        public LineSegment(Point StartPoint, Point EndPoint)
        {
            X1 = StartPoint.X;
            Y1 = StartPoint.Y;

            X2 = EndPoint.X;
            Y2 = EndPoint.Y;
        }

        /// <summary>
        /// Stores values representing a line segment
        /// </summary>
        /// <param name="Start">A Segment representing the start location</param>
        /// <param name="End">A Segment representing the end location</param>
        public LineSegment(Coordinate Start, Coordinate End)
        {
            X1 = Start.X;
            Y1 = Start.Y;
            X2 = End.X;
            X2 = End.Y;
        }
        /// <summary>
        /// Creates a new segment from a point and a vector.  The point represents the start point
        /// or tail of the vector, and the vector is added to that point to find the second point.
        /// </summary>
        /// <param name="StartPoint">Point representing one of the points of the segment</param>
        /// <param name="StartToEnd">Vector representing the path of the segment</param>
        public LineSegment(Point StartPoint, Vector StartToEnd)
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
        public LineSegment(double x1, double y1, double x2, double y2)
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
        /// Gets the Envelope of this segment.
        /// </summary>
        public override Envelope Envelope
        {
            get
            {
                // just create this on the fly.
                return new Envelope(this);
            }
        }
        /// <summary>
        /// Returns the dimension.  In this case, line segments have 1 dimension, so
        /// the dimension returns 1.
        /// </summary>
        public override int Dimension
        {
            get { return 1; }
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


        #region Geometry Overrides

        #region ------------------------ BOOLEAN RELATIONS ------------------------------
        /// <summary>
        /// If geom is a point, this will return true if geom is on this line.
        /// In order for a LineSegment to be within this
        /// segment, its intersection must be equal to the supplied geom.
        /// </summary>
        /// <param name="geom">Any geometry</param>
        /// <returns>Boolean, true if the geometry is contained by this LineSegment</returns>
        public override bool Contains(Geometry geom)
        {
            
            if (geom.GetType() == typeof(Point))
            {
                if (Intersection(geom) == geom) return true;
            }
            if (geom.GetType() == typeof(LineSegment))
            {
                if (Intersection(geom) == geom) return true;
            }
            // defer to code in higher level code;
            return geom.Within(this);
        }

        /// <summary>
        /// Can't be true for points.
        /// True if the segments intersect in a point, but the endpoints don't intersect.
        /// </summary>
        /// <param name="geom">Any geometry.</param>
        /// <returns>Boolean, true if the geometries cross.</returns>
        public override bool Crosses(Geometry geom)
        {
            if (geom.GetType() == typeof(Point)) return false;
            if (geom.GetType() == typeof(LineSegment))
            {
                Geometry intsct = Intersection(geom);
                // False if they don't intersect
                if (intsct == null) return false;
                // False if they overlap
                if (intsct.Dimension == 1) return false;
                // Check points
                Point Pnt = (Point)intsct;
                if (Pnt == StartPoint) return false;
                if (Pnt == EndPoint) return false;
                LineSegment Seg = (LineSegment)geom;
                if (Pnt == Seg.StartPoint) return false;
                if (Pnt == Seg.EndPoint) return false;
            }
            // Defer to the more complex geometry for calculations
            return geom.Crosses(this);
        }

        /// <summary>
        /// Tests this segment with a geometry to see if they are disjoint.
        /// </summary>
        /// <param name="geom">Any class derived from the geometry class.</param>
        /// <returns>Boolean, true if the two geometries are disjoint.</returns>
        public override bool Disjoint(Geometry geom)
        {
            if (Intersects(geom) == true) return false;
            return true;
        }

        /// <summary>
        /// This form of equals satisfies the geometry requirements.
        /// There is also an overload that compares to any object.
        /// They will only be found equal, however if they are LineSegments.
        /// </summary>
        /// <param name="geom">Any geometry.</param>
        /// <returns>Boolean, true if the LineSegments are the same (endpoints can be flipped.)</returns>
        public override bool Equals(Geometry geom)
        {
            if (this == geom) return true;
            return false;
        }

        /// <summary>
        /// Determines whether or not this LineSegment intersects with the specified geometry.
        /// </summary>
        /// <param name="geom">Any of the derivative classes of Topology2D.Geometry</param>
        /// <returns>Boolean, true if an intersection occurs with this LineSegment</returns>
        public override bool Intersects(Geometry geom)
        {
            Type tp = geom.GetType();
            if (tp == typeof(Point))
            {
                //http://mathworld.wolfram.com/Collinear.html

                // If the point is outside the extents of the segment, return false
                Point Pnt = (Point)geom;
                if (Pnt.X < X1 && Pnt.X < X2) return false;
                if (Pnt.X > X1 && Pnt.X > X2) return false;
                if (Pnt.Y < Y1 && Pnt.Y < Y2) return false;
                if (Pnt.Y > Y1 && Pnt.Y > Y2) return false;

                // If the point is not collinear, return false
                //http://mathworld.wolfram.com/Collinear.html
                // |(V2 - V1) x (V1 - V3)| = 0 for collinear points
                Vector V1 = new Vector(X1, Y1);
                Vector V2 = new Vector(X2, Y2);
                Vector V3 = new Vector(Pnt.X, Pnt.Y);
                Vector d1 = V1.Minus(V2);
                Vector d2 = V1.Minus(V3);
                double res = d1.Cross(d2);
                if (res == 0) return true;
                return false;
            }
            if (tp == typeof(LineSegment))
            {
                if (Intersection(geom) == null) return false;
            }
            // Defer to code stored in higher level geometries
            return geom.Intersects(this);
        }

        /// <summary>
        /// This is a special form of intersection for Segments.  Two segments overlap
        /// if they intersect to form a line segment that is not equal to either of the 
        /// contributing segments.  Returns false for points.  Defers to the code found in
        /// more complex objects for other intersections.
        /// </summary>
        /// <param name="geom">Any geometry.</param>
        /// <returns>Boolean, true if the two geometries overlap.</returns>
        public override bool Overlaps(Geometry geom)
        {
            Type tp = geom.GetType();
            if(tp == typeof(Point))return false;
            if(tp == typeof(LineSegment))
            {
                Geometry Intsct = Intersection(geom);
                if (Intsct == null) return false;
                if (Intsct.Dimension == 0) return false;
                LineSegment Seg = (LineSegment)Intsct;
                if (Seg == this) return false; // this segment is contained.. so it isn't an overlap.
                if (Seg == geom) return false; // geom is contained, so it isn't an overlap.
            }
            // Defer to code found in more comlex shapes for more complex things.
            return geom.Overlaps(this);
        }

        /// <summary>
        /// Tests whether this LineSegment touches the specified geometry.
        /// </summary>
        /// <param name="geom">Any geometry</param>
        /// <returns>Boolean, true if the geometries touch.</returns>
        public override bool Touches(Geometry geom)
        {
            // Defer to the code found in more complex shapes
            Type tp = geom.GetType();
            
            if (tp == typeof(Point))
            {
                // I am considering the line connecting the endpoints as "Interior" which 
                // can't intersect with the point in order for Touches to be true.
                // therefore, in order for touches to be true, the endpoints must intersect.
                Point Pnt = (Point)geom;
                if (Pnt == StartPoint || Pnt == EndPoint) return true;
                return false;
            }

            if (tp == typeof(LineSegment))
            {
                Geometry Intsct = Intersection(geom);
                if (Intsct == null) return false;  // They don't touch if they don't intersect
                
                // They "touch" only if the only point of intersection is an endpoint.
                // An endpoint with an interior is ok too.
                if (Intsct.Dimension == 1) return false;
                Point Pnt = (Point)Intsct;
                if (Pnt == StartPoint) return true;
                if (Pnt == EndPoint) return true;
                LineSegment Seg = (LineSegment)geom;
                if (Pnt == Seg.StartPoint) return true;
                if (Pnt == Seg.EndPoint) return true;
                return false;
            }

            // Defer to the other objects in cases where higher order is of importance.
            return geom.Touches(this);
            
        }

        /// <summary>
        /// Tests to see if this segment falls "within" another geometry.
        /// False for points.  True for segments where the intersection
        /// results in a segment that is the same as this segment.
        /// </summary>
        /// <param name="geom">The geometry to test.</param>
        /// <returns>Boolean, true if this geometry is within the specified geometry.</returns>
        public override bool Within(Geometry geom)
        {
            Type tp = geom.GetType();
            if (tp == typeof(Point)) return false;
            if (tp == typeof(LineSegment))
            {
                Geometry Intsct = Intersection(geom);
                if (Intsct == null) return false;
                if (Intsct.Dimension == 0) return false;
                LineSegment Seg = (LineSegment)Intsct;
                if (Seg == (LineSegment)geom) return true;
                return false;
            }
            return geom.Contains(this);
        }
        #endregion
        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <returns></returns>
        public override byte[] AsBinary()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <returns></returns>
        public override string AsText()
        {
            return "LINESTRING (" + X1 + " " + Y1 + ", " + X2 + " " + Y2 + ")";
        }
        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <returns></returns>
        public override Geometry Boundary()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// Returns a polygon of Distance around this segment.
        /// </summary>
        /// <param name="Distance">The distance in map units of the radius of the buffer</param>
        /// <returns>Usually a Topology2D.Polygon representing a capsule like shape with end caps of radius r.
        /// Or a copy of this Topology2D.Point if the Radius is 0</returns>
        public override Geometry Buffer(double Distance)
        {
            // Defaults to using 4 additional points on the rounded endcaps
            return Buffer(Distance, 4);
        }

        /// <summary>
        /// Returns a polygonal circle of Radius around this point.
        /// </summary>
        /// <param name="Distance">The distance in map units of the radius of the buffer</param>
        /// <param name="numPoints">Controls the number of points used in each cap. 
        /// More points will be smoother.  0 points creates a "flat" end cap.</param>
        /// <returns>Topology2D.Polygon representing a capsule like shape with end caps of radius r.
        /// Or a copy of this Topology2D.Point if the Radius is 0</returns>
        public Geometry Buffer(double Distance, int numPoints)
        {
            if (numPoints < 0)
            {
                throw new ArgumentException("An invalid number of points was passed.  numPoints must be >= 0");
            }
            // A zero radius compels us just to return the point sent to us
            if (Distance == 0)
            {
                return this.Copy();
            }

            Vector Dir = new Vector(this);
            Angle ang = Dir.Theta;
            ang.Degrees += 90;
            
            Polygon Buff = new Polygon();
            // Radius can be negative... we just take the absolute value.
            if (Distance < 0) Distance = -Distance;
            double dAng = Math.PI / (numPoints+1); // add one here or else a cap point will be redundant with an "edge" point
          
            Buff.Shell.Coordinates.Add(
                new Coordinate(StartPoint.X + Distance * Math.Cos(ang.Radians),
                               StartPoint.Y + Distance * Math.Sin(ang.Radians)));
            

            Buff.Shell.Coordinates.Add(
                new Coordinate(EndPoint.X + Distance * Math.Cos(ang.Radians),
                          EndPoint.Y + Distance * Math.Sin(ang.Radians)));

            // Cap on EndPoint side of segment
            
            for (int pt = 0; pt < numPoints; pt++)
            {
                ang.Radians -= dAng; // Counter Clockwise for consistency (Non-Hole) direction
                Buff.Shell.Coordinates.Add(new Coordinate(
                    EndPoint.X + Distance * Math.Cos(ang.Radians),
                    EndPoint.Y + Distance * Math.Sin(ang.Radians)));
            }
            ang.Radians -= dAng;

            // An edge
            Buff.Shell.Coordinates.Add(new Coordinate(
                EndPoint.X + Distance * Math.Cos(ang.Radians),
                EndPoint.Y + Distance * Math.Sin(ang.Radians)));

            Buff.Shell.Coordinates.Add(new Coordinate(
                StartPoint.X + Distance * Math.Cos(ang.Radians),
                StartPoint.Y + Distance * Math.Sin(ang.Radians)));

            // Cap on StartPoint side of segment
            for (int pt = 0; pt < numPoints; pt++)
            {
                ang.Radians -= dAng;
                Buff.Shell.Coordinates.Add(new Coordinate(
                    StartPoint.X + Distance * Math.Cos(ang.Radians),
                    StartPoint.Y + Distance * Math.Sin(ang.Radians)));
            }
            
            return Buff;
        }
        /// <summary>
        /// Returns a polygonal circle of Radius around this point.
        /// </summary>
        /// <param name="Distance">The distance in map units of the radius of the buffer</param>
        /// <param name="numPoints">Controls the number of points used in each cap. 
        /// More points will be smoother.  0 points creates a "flat" end cap.</param>
        /// <returns>Topology2D.Polygon representing a capsule like shape with end caps of radius r.
        /// <param name="Side">Side: Left or Right.  Specifies to only buffer one side of the segment.</param>
        /// Or a copy of this Topology2D.Point if the Radius is 0</returns>
        /// <remarks>This will not "clip" the buffers properly for a linestring.
        /// Use the linestring Buffer function for that.</remarks>
        public Geometry Buffer(double Distance, int numPoints, Topology2D.Enums.BufferSide Side)
        {
            //
            //          For left buffer
            //
            //           ___________________________
            //          /                            \
            //          ---o---------------------->---
            //            BufferStart            BufferEnd
        


            if (numPoints < 0)
            {
                throw new ArgumentException("An invalid number of points was passed.  numPoints must be >= 0");
            }
            // A zero radius compels us just to return the point sent to us
            if (Distance == 0)
            {
                return this.Copy();
            }
            if (Side == Enums.BufferSide.Both)
            {
                // Handling for both sides is different enough that I have a different function for it.
                return Buffer(Distance, numPoints);
            }
            Vector Dir = new Vector(this);
            Angle ang = Dir.Theta;
            Point BufferStart = new Point();
            Point BufferEnd = new Point();
            
            int RelPoints = numPoints;
            if (Side == Enums.BufferSide.Left)
            {
                ang.Degrees += 180;
                BufferStart = this.StartPoint;
                BufferEnd = this.EndPoint;
            }
            else
            {
                BufferStart = this.EndPoint;
                BufferEnd = this.StartPoint;
            }

            Polygon Buff = new Polygon();
            // Radius can be negative... we just take the absolute value.
            if (Distance < 0) Distance = -Distance;
            double dAng = Math.PI / (2*(numPoints + 1)); // add one here or else a cap point will be redundant with an "edge" point


           
            //Buff.Add_Point(new Point(BufferStart.X + Distance * Math.Cos(ang.Radians),
                             //       BufferStart.Y + Distance * Math.Sin(ang.Radians)));

            // Cap on EndPoint side of segment

            for (int pt = 0; pt <= numPoints; pt++)
            {
                
                Buff.Shell.Coordinates.Add(new Coordinate(
                    BufferStart.X + Distance * Math.Cos(ang.Radians),
                    BufferStart.Y + Distance * Math.Sin(ang.Radians)));

                ang.Radians -= dAng; // Clockwise for consistency (Non-Hole) direction
            }
            

            // An edge
            Buff.Shell.Coordinates.Add(new Coordinate(
                BufferStart.X + Distance * Math.Cos(ang.Radians),
                BufferStart.Y + Distance * Math.Sin(ang.Radians)));

            Buff.Shell.Coordinates.Add(new Coordinate(
                BufferEnd.X + Distance * Math.Cos(ang.Radians),
                BufferEnd.Y + Distance * Math.Sin(ang.Radians)));
            
           
            for (int pt = 0; pt < numPoints; pt++)
            {
                ang.Radians -= dAng;
                Buff.Shell.Coordinates.Add(new Coordinate(
                    BufferEnd.X + Distance * Math.Cos(ang.Radians),
                    BufferEnd.Y + Distance * Math.Sin(ang.Radians)));
               
            }

            // To prevent rounding errors, try setting the far side point using theta directly
            ang = Dir.Theta;
            if (Side == Enums.BufferSide.Right)
            {
                ang.Degrees += 180;
            }
            Buff.Shell.Coordinates.Add(new Coordinate(
                BufferEnd.X + Distance * Math.Cos(ang.Radians),
                BufferEnd.Y + Distance * Math.Sin(ang.Radians)));
            
            // finalize the buffer with the line on top of the original line segment
            ang.Degrees += 180;
            Buff.Shell.Coordinates.Add(new Coordinate(
                BufferStart.X + Distance * Math.Cos(ang.Radians),
                BufferStart.Y + Distance * Math.Sin(ang.Radians)));

            return Buff;
        }

       
        

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <returns></returns>
        public override Geometry ConvexHull()
        {
            throw new Exception("The method or operation is not implemented.");
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
        /// Type
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override double Distance(Geometry geom)
        {
            Type tp = geom.GetType();
            if (tp == typeof(Point))
            {
                Vector Dist = new Vector(ShortestPathTo(geom));
                return Dist.Magnitude;
            }
            if (tp == typeof(LineSegment))
            {
                Vector Dist = new Vector(ShortestPathTo(geom));
                return Dist.Magnitude;
            }
            return geom.Distance(this);
        }

        /// <summary>
        /// This function returns the Square of the shortest distance 
        /// without taking any square roots
        /// </summary>
        /// <param name="geom">A simple geometry to determine the distance to</param>
        /// <returns>Double, the square of the shortest distance to the specified geometry</returns>
        public double SqrDistance(Geometry geom)
        {
            Type tp = geom.GetType();
            if (tp == typeof(Point))
            {
                Vector Dist = new Vector(ShortestPathTo(geom));
                return Dist.Norm2();
            }
            if (tp == typeof(LineSegment))
            {
                Vector Dist = new Vector(ShortestPathTo(geom));
                return Dist.Norm2();
            }
            // I haven't coded more advanced versions yet
            throw new ApplicationException("This feature is not supported yet for advanced shapes.");
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="geom"></param>
        /// <returns>Geometry, null if no intersection occurs.</returns>
        public override Geometry Intersection(Geometry geom)
        {

            Type tp = geom.GetType();
            if (tp == typeof(Point))
            {
                // If the point intersects, then the point IS the intersection.
                if (Intersects(geom) == true) return geom;
                return null;
            }
            if (tp == typeof(LineSegment))
            {
                // http://www.geometryalgorithms.com/Archive/algorithm_0104/algorithm_0104B.htm#intersect2D_SegSeg()

                LineSegment Seg = (LineSegment)geom;
                LineSegment Overlap = new LineSegment();

                // intersect2D_2LineSegments(): the intersection of 2 finite 2D segments
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
                    if (du == 0 && dv == 0)
                    {           // both segments are points
                        if (StartPoint.Intersects(Seg.StartPoint) == false) return null; // they are distinct points
                        return StartPoint.Copy(); // they are the same point
                    }
                    if (du == 0)
                    {                    // S1 is a single point
                        if (StartPoint.Intersects(Seg) == false) return null;  // but is not in S2
                        return StartPoint.Copy();
                       
                    }
                    if (dv == 0)
                    {         // S2 a single point
                        if (Seg.StartPoint.Intersects(this) == false) return null; // but is not in S1
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
                        double t = t0;
                        t0 = t1;
                        t1 = t;    // swap if not
                    }
                    if (t0 > 1 || t1 < 0)
                    {
                        return null;     // NO overlap
                    }
                    if (t0 < 0) t0 = 0; // clip to min 0
                    if (t1 > 1) t1 = 1; // clip to max 1

                    if (t0 == t1)
                    {                 // intersect is a point
                        return Seg.StartPoint.ToVector().Plus(v.Times(t0)).ToPoint();
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
                if (sI < 0 || sI > 1) return null;      // no intersect with S1


                // get the intersect parameter for S2
                double tI = u.Cross(w) / D;
                if (tI < 0 || tI > 1) return null;      // no intersect with S2
                // compute S1 intersect point
                // *I0 = S1.P0 + sI * u; 
                return StartPoint.ToVector().Plus(u.Times(sI)).ToPoint();
            }
            return null;
        }


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
        /// Not Implemented
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override Geometry SymDifference(Geometry geom)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString();
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



        #region Methods

        #region -------------------- COPY ---------------------------------

        /// <summary>
        /// Returns a new instance of the Topology2D.LineSegment class with the same values as this object.
        /// </summary>
        /// <returns>Topology2D.LineSegment with identical properties.</returns>
        public LineSegment Copy()
        {
            LineSegment NewSeg = new LineSegment(X1, Y1, X2, Y2);
            return NewSeg;
        }

        #endregion

        #region -------------------- REVERSE -------------------------------

        /// <summary>
        /// Flips the direction of this segment so that the startpoint becomes the endpoint.
        /// </summary>
        /// <returns>A LineSegment with the startpoint and endpoint reversed.</returns>
        public LineSegment Reverse()
        {
            return new LineSegment(EndPoint, StartPoint);
        }

        #endregion

        #region ------------------- CLOSEST POINT TO -------------------------
        /// <summary>
        /// Returns a Point representing the closest point.
        /// </summary>
        /// <param name="geom">The target point we want to be close to.</param>
        /// <returns>The point on this segment that is closest to the given geometry.  Null if
        /// the two features intersect.</returns>
        public Point ClosestPointTo(Geometry geom)
        {
            LineSegment Seg = ShortestPathTo(geom);
            if (Seg == null) return null;
            return Seg.StartPoint;
        }

       

        /// <summary>
        /// Returns a LineSegment starting with the closest point on this segment to the specified point
        /// and terminating at the specified point.  Returns null if the two geometries intersect.
        /// </summary>
        /// <param name="geom">The geometry to compare to this segment.</param>
        /// <returns>Topology2D LineSegment from this segemnt to the specified point deliniating the shortest path.</returns>
        public override LineSegment ShortestPathTo(Geometry geom)
        {
            Type tp = geom.GetType();
            if (tp == typeof(Point))
            {
                // If the points defining this segment are the same, we treat the segment as a point
                // special handling to avoid 0 in denominator later
                Point Pnt = (Point)geom;
                if (Pnt == this) return null; // In the case of intersection, we return null.
                if (X2 == X1 && Y2 == Y1)
                {
                    // The shortest path too is just a segment defined by connecting the points
                    return new LineSegment(StartPoint, (Point)geom);
                }
                //http://softsurfer.com/Archive/algorithm_0102/algorithm_0102.htm

                Vector Vseg = new Vector(this); // vector from p1 to p2 in the segment
                Vector W = new Vector(new Point(X1, Y1), Pnt); // vector from p1 to Point
                double c1 = W.Dot(Vseg); // the dot product represents the projection onto the line
                if (c1 < 0)
                {
                    // The closest point on the segment to Point is p1
                    return new LineSegment(StartPoint, (Point)geom);
                }
                double c2 = Vseg.Magnitude;
                if (c2 <= c1)
                {
                    // The closest point on the segment to Point is p2
                    return new LineSegment(EndPoint, (Point)geom);
                }
                // The closest point on the segment is perpendicular to the point, 
                // but somewhere on the segment between P1 and P2 
                double b = c1 / c2;
                Vector Pb = Vseg.Times(b);
                Pb = Pb.Plus(new Vector(X1, Y1));
                return new LineSegment(Pb.ToPoint(), (Point)geom);
            }
            if (tp == typeof(LineSegment))
            {
                // Don't bother with this math if they intersect.
                if (Intersects(geom) == true) return null;
                LineSegment Seg = (LineSegment)geom;
                LineSegment Path = new LineSegment();
                //http://www.geometryalgorithms.com/Archive/algorithm_0106/algorithm_0106.htm
                const double SMALL_NUM = 0.00000001;
                Vector u = new Vector(this); // LineSegment 1
                Vector v = new Vector(Seg); // LineSegment 2
                Vector w = new Vector(this.StartPoint, Seg.StartPoint);
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
                Path.EndPoint = Seg.StartPoint.ToVector().Plus(dV).ToPoint();
                return Path;

            }
            return geom.ShortestPathTo(this).Reverse();
        }
       
       


        /// <summary>
        /// This retrieves the shortest path, but only returns the point that represents
        /// the closest point to this segment that is located on the segment specified.
        /// </summary>
        /// <param name="geom">The geometry containing the point to find.</param>
        /// <returns>MapWinGeoProc.Topology2D.Point the closest point to this segment on 
        /// the specified geometry.</returns>
        public Point ClosestPointOn(Geometry geom)
        {
            if (Intersects(geom)) return null;

            LineSegment seg = ShortestPathTo(geom);
            return seg.EndPoint;
        }
        
        #endregion

        

        #region ------------------- INTERSECTS WITH --------------------------
        /// <summary>
        /// Determines whether the point falls on a line segment
        /// </summary>
        /// <param name="Point">The MapWinGIS.Point to test agianst this segment</param>
        /// <returns>Boolean, true if the point is on the segment</returns>
        public bool Intersects(MapWinGIS.Point Point)
        {
            return Intersects(new Point(Point));
        }

        /// <summary>
        /// Determines whether the point falls on a line segment
        /// </summary>
        /// <param name="Point">The Point to test agianst this segment</param>
        /// <returns>Boolean, true if the point is on the segment</returns>
        public bool Intersects(Point Point)
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
            if (Pgn.Envelope.Intersects(this) == false) return false;

            // -- TO DO -- 

            //if (Pgn.Intersects(this) == false) return false;

            return true;
        }

        #endregion

        #region ------------------- IS PARALLEL TO ---------------------------
        /// <summary>
        /// Returns true if the line segments are parallel
        /// </summary>
        /// <param name="Seg">The Topology.SegementD LineSegment to compare to </param>
        /// <returns>Boolean, true if the segments are parallel</returns>
        public bool IsParallelTo(LineSegment Seg)
        {
            Vector u = new Vector(this);
            Vector v = new Vector(Seg);
            // if Abs(u.v) is = |u||v|, the segments are parallel
            if (Math.Abs(u.Dot(v)) == u.Magnitude * v.Magnitude) return true;
            return false;
        }
        #endregion

        #region ------------------- IS PERPENDICULAR TO ----------------------

        /// <summary>
        /// Returns true if the line segments are perpendicular
        /// </summary>
        /// <param name="Seg">The LineSegment line segment to compare this segment with</param>
        /// <returns>Boolean, true if the segments are perpendicular</returns>
        public bool IsPerpendicularTo(LineSegment Seg)
        {
            Vector u = new Vector(this);
            Vector v = new Vector(Seg);
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
        /// <param name="Dist">The double distance the point can be from the line and still be considered contained</param>
        /// <returns>Boolean, true if the point is within the buffer distance of the line segment</returns>
        public bool IsWithinADistanceOf(Point Point, double Dist)
        {
            if (Distance(Point) < Dist)return true;
            return false;
            
        }// End Contains Point

        /// <summary>
        /// Determines whether this segment is within the buffer Distance specified of the line segment specified
        /// </summary>
        /// <param name="Seg">The LineSegment to compare to this segment</param>
        /// <param name="Dist">Double, the maximum distance between segments to allow</param>
        /// <returns>Boolean, true if this segment is within the specified distance of the other segment.</returns>
        public bool IsWithinADistanceOf(LineSegment Seg, double Dist)
        {
            if (Distance(Seg) < Dist)return true;
            return false;
        }
        #endregion

        #region ------------------- SHARES A LINE SEGMENT WITH ---------------

        /// <summary>
        /// Since these are already segments, we just determine if the intersection is a valid line segment.
        /// If the segments intersect as a point, or don't intersect, this will return false.
        /// </summary>
        /// <param name="Seg">Topology2D.LineSegment to compare with.</param>
        /// <returns>Boolean, true if the two lines intersect in a segment.</returns>
        public bool SharesALineLineSegmentWith(LineSegment Seg)
        {
            Geometry ret = Intersection(Seg);
            if(ret == null)return false;
            if (ret.Dimension == 0) return false;
            return true;
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
        public bool TouchesTheBoundaryOf(Envelope Ext)
        {
            if (Ext.Intersects(this) == false) return false;
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
            List<LineSegment> Segs = null;
            if (Pgn.Envelope.Intersects(StartPoint) == true)
            {
                Segs = Pgn.Shell.LineSegmentsWithin(this.Envelope);
                for (int I = 0; I < Segs.Count; I++)
                {
                    if (StartPoint.Intersects(Segs[I])) return true;
                }
            }
            else
            {
                if (Pgn.Envelope.Intersects(EndPoint) == false) return false;
            }
            if (Segs == null)
            {
                Segs = Pgn.Shell.LineSegmentsWithin(this.Envelope);
            }
            for (int I = 0; I < Segs.Count; I++)
            {
                if (EndPoint.Intersects(Segs[I])) return true;
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

        #region  --------------------------- Opperators ------------------------
        /// <summary>
        /// Equality operartor is equal if the endpoints of the segment line up.
        /// </summary>
        /// <param name="A">A LineSegment</param>
        /// <param name="objB">Any Object</param>
        /// <returns>Boolean, true if the line segments are equal</returns>
        static public bool operator ==(LineSegment A, Object objB)
        {
            if (objB == null)
            {
                if ((object)A == null)
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
                if (objB.GetType() != typeof(LineSegment)) return false;
                LineSegment B = (LineSegment)objB;
                if (A.StartPoint == B.StartPoint && A.EndPoint == B.EndPoint) return true;
                if (A.StartPoint == B.EndPoint && A.EndPoint == B.StartPoint) return true;
            }
            return false;
        }
        
        /// <summary>
        /// InEquality operartor is false if the endpoints of the segment line up.
        /// </summary>
        /// <param name="A">A LineSegment</param>
        /// <param name="objB">Any Object</param>
        /// <returns>Boolean, true if the line segments are equal</returns>
        static public bool operator !=(LineSegment A, Object objB)
        {

            if (objB.GetType() != typeof(LineSegment)) return true;
            LineSegment B = (LineSegment)objB;
            if (A.StartPoint == B.StartPoint && A.EndPoint == B.EndPoint) return false;
            if (A.StartPoint == B.EndPoint && A.EndPoint == B.StartPoint) return false;
            return true;
        }
       
        
        #endregion
    }
}