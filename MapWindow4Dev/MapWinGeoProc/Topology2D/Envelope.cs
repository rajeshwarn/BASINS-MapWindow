using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Topology2D
{
    /// <summary>
    /// A primitive rectangle class for holding double values
    /// Since we define it in terms of X, Y and Z, this is an 
    /// axis-aligned bounding box (AABB). 
    /// </summary>
    public class Envelope : Geometry
    {
        #region Variables
        /// <summary>
        /// Double, The minimum X value
        /// </summary>
        public double xMin;
        /// <summary>
        /// Double, The largest X value
        /// </summary>
        public double xMax;
        /// <summary>
        /// Double, The minimum Y value
        /// </summary>
        public double yMin;
        /// <summary>
        /// Double, the largest Y value
        /// </summary>
        public double yMax;

        
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new, blank extents object with all the extents = 0.
        /// </summary>
        public Envelope()
        {
            xMin = 0;
            xMax = 0;
            yMin = 0;
            yMax = 0;
        }
        /// <summary>
        /// Creates a new instance of Envelope from double values
        /// </summary>
        /// <param name="MinX">Double, The minimum X value</param>
        /// <param name="MaxX">Double, The largest X value</param>
        /// <param name="MinY">Double, The minimum Y value</param>
        /// <param name="MaxY">Double, the largest Y value</param>
        public Envelope(double MinX, double MaxX, double MinY, double MaxY)
        {
            xMin = MinX;
            yMin = MinY;
            xMax = MaxX;
            yMax = MaxY;
        }

        /// <summary>
        /// Creates a new instance of Envelope from double values in the MapWinGIS.Envelope object
        /// </summary>
        /// <param name="Bounds">A MapWinGIS.Extents object to define a rectangle</param>
        public Envelope(MapWinGIS.Extents Bounds)
        {
            xMin = Bounds.xMin;
            xMax = Bounds.xMax;
            yMin = Bounds.yMin;
            yMax = Bounds.yMax;
        }
        /// <summary>
        /// Calculates a new extents object that is just large enough to contain the entire segment.
        /// </summary>
        /// <param name="seg">A LineSegment to use as the "diagonal" of this extents object.</param>
        public Envelope(LineSegment seg)
        {
            xMin = Math.Min(seg.X1, seg.X2);
            xMax = Math.Max(seg.X1, seg.X2);
            yMin = Math.Min(seg.Y1, seg.Y2);
            yMax = Math.Max(seg.Y1, seg.Y2);
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the width.  Setting this will keep xMin the same and adjust xMax.
        /// </summary>
        public double Width
        {
            get
            {
                return xMax - xMin;
            }
            set
            {
                if (value < 0) throw new ArgumentException("Width cannot be negative.");
                xMax = xMin + value;
            }
        }
        /// <summary>
        /// Gets or sets the height.  Setting this will keep the yMin the same and adjust yMax.
        /// </summary>
        public double Height
        {
            get
            {
                return yMax - yMin;
            }
            set
            {
                if (value < 0) throw new ArgumentException("Height cannot be negative.");
                yMax = yMin + value;
            }
        }
        /// <summary>
        /// Gets or sets the center.  Setting this will translate the entire box.
        /// </summary>
        public Point Center
        {
            get
            {
                return new Point((xMax + xMin) / 2, (yMax + yMin / 2));
            }
            set
            {
                Vector trans = new Vector(Center, value);
                xMin = xMin + trans.X;
                yMin = yMin + trans.Y;
                xMax = xMax + trans.X;
                yMax = yMax + trans.Y;
            }
        }
        /// <summary>
        /// Integer, Specifies the number of spatial dimensions for this object.
        /// A value of 2 represents 2D space, 3 represents 3D space.
        /// </summary>
        public override int Dimension
        {
            get
            { 
                return 2;
            }
        }

        #endregion

        

        #region Geometry Overrides

        #region ------------------------ BOOLEAN RELATIONS ------------------------------
        /// <summary>
        /// Tests to see if the specified geometry.
        /// To contain, it means that no part of the specified geometry can fall outside
        /// of the given position.
        /// </summary>
        /// <param name="geom">Any geometry to test to see if it is contained in the extents.</param>
        /// <returns>Boolean, true if the specified geometry is contained in the extents.</returns>
        public override bool Contains(Geometry geom)
        {
            Type tp = geom.GetType();
            if(tp == typeof(Point))
            {
                if(Intersects(geom)==true)return true;
            }
            if (tp == typeof(LineSegment))
            {
                LineSegment Seg = (LineSegment)geom;
                if (Intersects(Seg.StartPoint) || Intersects(Seg.EndPoint))
                {
                    // if both points intersect, then the whole segment must be contained.
                    return true;
                }
                return false;
            }
            if (tp == typeof(Envelope))
            {
                Envelope Env = (Envelope)geom;
                if (Env.xMax <= xMax && Env.xMin > xMin && Env.yMax <= yMax && Env.yMin >= yMin)
                {
                    return true;
                }
            }
            return geom.Within(this);
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override bool Crosses(Geometry geom)
        {
            Type tp = geom.GetType();
            if (tp == typeof(Point))
            {
                // Points cannot match the definition of crossing
                return false;
            }
            
            if (tp == typeof(LineSegment))
            {
                Geometry Intsct = Intersection(geom);
                // We don't intersect at all
                if (Intsct == null) return false;
                // If the intersection is the same length as the line segment, then
                // the line segment is contained and didn't cross or it would be shorter.
                if (Intsct == (LineSegment)geom) return false;
                // If the intersection is a single point, then we are touching, not crossing.
                if (Intsct.Dimension == 0) return false;
                return true;
            }
            if (tp == typeof(Envelope))
            {
                Geometry Intsct = Intersection(geom);
                // We don't intersect at all so we can't cross
                if (Intsct == null) return false;
                // If the intersection is a line or a point, we touch
                if (Intsct.Dimension == 0 || Intsct.Dimension == 1) return false;
                // if the intersection is the same as the original envelope, it is contained, not crossing
                if (Intsct == (Envelope)geom) return false;
            }
            
            return geom.Crosses(this);
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override bool Disjoint(Geometry geom)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override bool Equals(Geometry geom)
        {
            // We will allow a four part rectangle polygon with the same points be judged 
            // for equality against the corners of this envelope.
            Type tp = geom.GetType();
            if(tp != typeof(Envelope) && tp != typeof(Polygon))return false;
            if (tp == typeof(Envelope))
            {
                Envelope env = (Envelope)geom;
                if (env.xMin != xMin) return false;
                if (env.yMin != yMin) return false;
                if (env.xMax != xMax) return false;
                if (env.yMax != yMax) return false;
                return true;
            }
            if(tp == typeof(Polygon))
            {
                // In the new system, the first and last point of the polygon are equal.
                // Therefore a rectangle that is closed has 5 points, not 4.
                Polygon Pgn = geom as Polygon;
                if (Pgn.Shell.Coordinates.Count != 5) return false;
                // ensure that the polygon is closed
                if (Pgn.Shell.Coordinates[4] != Pgn.Shell.Coordinates[0]) return false;
                // Don't bother with point[4] because it is a duplicate
                List<Point> Corners = GetCorners();
                int MatchCount = 0;
                for (int I = 0; I < 4; I++)
                {
                    for (int J = 0; J < 4; J++)
                    {
                        if (Pgn.Shell.Coordinates[I] != Corners[J]) continue;
                        MatchCount++;
                        break;
                    }
                }
                // four matching corners mean we can consider this polygon to be the same
                // as this envelope
                if (MatchCount == 4) return true;
                return false;
            }
            return false;
        }

        /// <summary>
        /// Checks to see if this envelope intersects with the specified object.
        /// This will be true if the object is inside this envelope or touches etc.
        /// </summary>
        /// <param name="geom">THe geometry to test for an intersection.</param>
        /// <returns>Boolean, true if there is an intersection.</returns>
        public override bool Intersects(Geometry geom)
        {
            Type tp = geom.GetType();
            if (tp == typeof(Point))
            {
                Point Pnt = (Point)geom;
                if (Pnt.X < xMin || Pnt.X > xMax) return false;
                if (Pnt.Y < yMin || Pnt.Y > yMax) return false;
            }
            if (tp == typeof(LineSegment))
            {
                if (Intersection(geom) == null) return false;
                return true;
            }
            if (tp == typeof(Envelope))
            {
                Envelope rect = (Envelope)geom;

                // check to see if we have an intersection first
                if (xMin > rect.xMax) return false;
                if (rect.xMin > xMax) return false;
                if (yMin > rect.yMax) return false;
                if (rect.yMin > yMax) return false;
                return true;
            }
            return geom.Intersects(this);
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override bool Overlaps(Geometry geom)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override bool Touches(Geometry geom)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override bool Within(Geometry geom)
        {
            throw new Exception("The method or operation is not implemented.");
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
            throw new Exception("The method or operation is not implemented.");
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
        /// Not Implemented
        /// </summary>
        /// <param name="Distance"></param>
        /// <returns></returns>
        public override Geometry Buffer(double Distance)
        {
            throw new Exception("The method or operation is not implemented.");
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
        /// Not Implemented
       /// </summary>
       /// <param name="geom"></param>
       /// <returns></returns>
        public override double Distance(Geometry geom)
        {
            throw new Exception("The method or operation is not implemented.");
        }
       
        /// <summary>
        /// Checks to see if Any object qualifies for geometric equality
        /// but will return false if the unspecified type is not a geometry.
        /// </summary>
        /// <param name="obj">Any object</param>
        /// <returns>Boolean, true if the specified object is goemetrically equivalent to this one.</returns>
        public override bool Equals(object obj)
        {
            Type tp = obj.GetType();
            if (tp == typeof(Envelope) || tp == typeof(Polygon))
            {
                return Equals((Geometry)obj);
            }
            return false;
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
        /// Returns a geometry representing the region where both shapes intersect.
        /// </summary>
        /// <param name="geom">Any geometry to intersect with this Envelope.</param>
        /// <returns>The geometry that best describes the intersection.  Null if they don't intersect.</returns>
        public override Geometry Intersection(Geometry geom)
        {
            // ----------------------------- POINT -------------------------------------
            Type tp = geom.GetType();
            if (tp == typeof(Point))
            {
                Point Pnt = (Point)geom;
                if (Pnt.X < xMin || Pnt.X > xMax) return Pnt;
                if (Pnt.Y < yMin || Pnt.Y > yMax) return Pnt;
                return null;
            }
            // ---------------------------- LINE SEGMENT -------------------------------
            if (tp == typeof(LineSegment))
            {
                LineSegment Seg = (LineSegment)geom;
                Geometry Overlap;
                List<Point> CrossingPoints = new List<Point>();
                Overlap = null;
                // First rule out any with both points outside in the same direction
                if (Seg.X1 > xMax && Seg.X2 > xMax) return null;
                if (Seg.Y1 > yMax && Seg.X2 > yMax) return null;

                if (Seg.X1 < xMin && Seg.X2 < xMin) return null;
                if (Seg.Y1 < yMin && Seg.X2 < yMin) return null;

                List<LineSegment> mySegs = ToLineSegments();
                for (int I = 0; I < mySegs.Count; I++)
                {
                    Overlap = mySegs[I].Intersection(Seg);
                    if (Overlap == null) continue;
                    if (Overlap.Dimension == 0)
                    {
                        CrossingPoints.Add((Point)Overlap);
                    }
                    else
                    {
                        // If the overlap is congruant with this segment, then no other border segments
                        // can influence our intersection.
                        return Overlap;
                    }
                }

                if (CrossingPoints.Count == 0)
                {
                    // We already ruled out being outside, so it must completely inside
                    return Seg;
                } 
                // at this point we have a list of crossing points, but we are unsure about what is inside
                // or outside of our point.
                // either 1 or 2 crossing points exist.  If 2 exist, we can just connect them.
                if (CrossingPoints.Count == 2)
                {
                    return new LineSegment(CrossingPoints[0], CrossingPoints[1]);
                }
                // One crossing point exists, so we have to figure out which point is inside 
                if (Intersects(Seg.StartPoint) == true)
                {
                    return new LineSegment(Seg.StartPoint, CrossingPoints[0]);
                }
                else
                {
                    return new LineSegment(Seg.EndPoint, CrossingPoints[0]);
                }
            }
            // ------------------------------ ENVELOPE ---------------------------------------
            if (tp == typeof(Envelope))
            {
                Envelope rect = (Envelope)geom;
                
                // check to see if we have an intersection first
                if (xMin > rect.xMax) return null;
                if (rect.xMin > xMax) return null;
                if (yMin > rect.yMax) return null;
                if (rect.yMin > yMax) return null;


                // We intersect, so go ahead and find the values

                Envelope intersect = new Envelope();
                intersect.xMax = Math.Min(xMax, rect.xMax);
                intersect.xMin = Math.Max(xMin, rect.xMin);
                intersect.yMax = Math.Min(yMax, rect.yMax);
                intersect.yMin = Math.Max(yMin, rect.yMin);

                if (intersect.xMax == intersect.xMin)
                {
                    // reduce our dimensionality to a segment
                    if (intersect.yMax == intersect.yMin)
                    {

                    }
                }

                return intersect;
            }
            return geom.Intersection(this);

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

        #region ----------------------- CLOSEST POINT ON --------------------------

        /// <summary>
        /// In the 2D case, the shortest path to the specified segment is on one of the segments here.
        /// This determines the shortest path and picks shortest distance.  Then it returns the point
        /// on the specified segment that is closest to this extent.
        /// </summary>
        /// <param name="Seg">A LineSegment to find the closest point on.</param>
        /// <returns>Topology2D.Point specifying the closest point on the specified segment.
        /// This returns null if there is an intersection.</returns>
        public Point ClosestPointOn(LineSegment Seg)
        {
            LineSegment Ret;
            Ret = ShortestPathTo(Seg);
            if (Ret == null) return null;
            return Ret.EndPoint;
        }

        /// <summary>
        /// If the two extents intersect, this returns null.
        /// This checks all the possible shortest paths between the 4 Borders of both extents.
        /// This finds the shortest one and then reutrns the location of the point on the specified
        /// extents rectangle that is closest to this one.
        /// </summary>
        /// <param name="Ext">Envelope to compare to this object.</param>
        /// <returns>A point representing the closest point on the specified extents.  Returns null
        /// if the two extents intersect.</returns>
        public Point ClosestPointOn(Envelope Ext)
        {
            LineSegment Ret;
            Ret = ShortestPathTo(Ext);
            if (Ret == null) return null;
            return Ret.EndPoint;
        }
        #region ----------------------- CLOSEST POINT TO --------------------------

        /// <summary>
        /// Finds the shortest pathway between the borders of this extent and the specified point.
        /// Returns the point on this extents that is closest to the specified point.
        /// </summary>
        /// <param name="Pnt">The point to compare against.</param>
        /// <returns>The Topology2D.Point representing the closest point on this Envelope to the specified point.</returns>
        public Point ClosestPointTo(Point Pnt)
        {
            LineSegment Seg = ShortestPathTo(Pnt);
            if (Seg == null) return null;
            return Seg.StartPoint;
        }
        
        /// <summary>
        /// In the 2D case, the shortest path to the specified segment is on one of the segments here.
        /// This determines the shortest path and picks shortest distance.  Then it returns the point
        /// on the this extents that is closest to the specified segment.
        /// </summary>
        /// <param name="Seg">A LineSegment to compare to.</param>
        /// <returns>Topology2D.Point specifying the closest point on this extents to the specified segment.
        /// This returns null if there is an intersection.</returns>
        public Point ClosestPointTo(LineSegment Seg)
        {
            LineSegment Ret = ShortestPathTo(Seg);
            if (Ret == null) return null;
            return Ret.StartPoint;
        }

        /// <summary>
        /// If the two extents intersect, this returns null.
        /// This checks all the possible shortest paths between the 4 Borders of both extents.
        /// This finds the shortest one and then returns the location of the point on this object
        /// that is closest to the specified extents.
        /// </summary>
        /// <param name="Ext">Envelope to compare to this object.</param>
        /// <returns>A point representing the closest point on this object to the specified extents.
        /// Returns null if the two extents intersect.</returns>
        public Point ClosestPointTo(Envelope Ext)
        {
            LineSegment Seg = ShortestPathTo(Ext);
            if (Seg == null) return null;
            return Seg.StartPoint;
        }
        #endregion


        #endregion

        #region ----------------------- COMPLETELY CONTAINS -----------------------

        /// <summary>
        /// Ensures that the point is not only within the extents, but also doesn't
        /// touch any of the borders.
        /// </summary>
        /// <param name="Pnt">A Topology2D point to test.</param>
        /// <returns>Boolean, true if the point is entirely within the extents, without touching the edges.</returns>
        public bool CompletelyContains(Point Pnt)
        {
            // Envelope are easy because we know how they are set up in 2D space
            if (Intersects(Pnt) == false) return false;
            if (Pnt.X == xMin || Pnt.X == xMax) return false;
            if (Pnt.Y == yMin || Pnt.Y == yMax) return false;
            return true;
        }
        /// <summary>
        /// Ensures that the segment is completely within the extents without touching the border.
        /// </summary>
        /// <param name="Seg">The Topology2D.LineSegment to compare against</param>
        /// <returns>Boolean, true if both points of the segment are completely within the extents</returns>
        public bool CompletelyContains(LineSegment Seg)
        {
            // Rather coolly, if both points are inside a rectangle, than no part of the middle can touch or be
            // outside
            if (CompletelyContains(Seg.StartPoint) && CompletelyContains(Seg.EndPoint)) return true;
            return false;
        }
        /// <summary>
        /// checks to ensure that this Envelope completely contains the specified extents.
        /// </summary>
        /// <param name="Ext">The extents to compare to this extents</param>
        /// <returns>Boolean, true if the specified extents is within this extents without touching the borders</returns>
        public bool CompletelyContains(Envelope Ext)
        {
            // Interestingly, we can check the four corners.  If all of them are within this rectangle,
            // then we are completely contained.
            if (CompletelyContains(new Point(Ext.xMin, Ext.yMin)) == false) return false;
            if (CompletelyContains(new Point(Ext.xMin, Ext.yMax)) == false) return false;
            if (CompletelyContains(new Point(Ext.xMax, Ext.yMin)) == false) return false;
            if (CompletelyContains(new Point(Ext.xMax, Ext.yMax)) == false) return false;
            return true;
        }

        #endregion

        #region ----------------------- COPY --------------------------------------

        /// <summary>
        /// Returns a new instance of the Envelope class with the same values as this object.
        /// </summary>
        /// <returns>Topology2D.Envelope with identical properties.</returns>
        public Envelope Copy()
        {
            Envelope NewExt = new Envelope(xMin, xMax, yMin, yMax);
            return NewExt;
        }

        #endregion

        /// <summary>
        /// returns the four corners for this envelope in a list of points.
        /// We might do geometry collection eventually, but for now a list will do.
        /// </summary>
        /// <returns>List of points.  </returns>
        public List<Point> GetCorners()
        {
            List<Point> Crn = new List<Point>();
            Crn.Add(new Point(xMin, yMin));
            Crn.Add(new Point(xMax, yMin));
            Crn.Add(new Point(xMax, yMax));
            Crn.Add(new Point(xMin, yMax));
            return Crn;
        }


        #region ----------------------- DISTANCE TO -------------------------------

        /// <summary>
        /// Finds the shortest path to the specified point, and returns the length.
        /// </summary>
        /// <param name="Pnt">The Topology2D.Point to test agianst.</param>
        /// <returns>Double, the length of the shortest path.  0 if intersects.</returns>
        public double Distance(Point Pnt)
        {
            if (Intersects(Pnt) == true) return 0.0;
            return ShortestPathTo(Pnt).Length;
        }
        /// <summary>
        /// Finds the path between the two closest points between this object and the segment.
        /// Returns the length.  Returns 0.0 if the two intersect.
        /// </summary>
        /// <param name="Seg">The Topology2D.LineSegment to compare to.</param>
        /// <returns>Double, the shortest possible distance between this extents and the segment.</returns>
        public double Distance(LineSegment Seg)
        {
            if (Intersects(Seg) == true) return 0.0;
            return ShortestPathTo(Seg).Length;
        }

        /// <summary>
        /// Finds the shortest path between the two closest points on the two extents objects.
        /// Returns 0.0 if the two intersect.
        /// </summary>
        /// <param name="Ext">The Topology2D.Envelope to compare to.</param>
        /// <returns>Double, the magnitude of shortest path between the extents.</returns>
        public double Distance(Envelope Ext)
        {
            if (Intersects(Ext) == true) return 0.0;
            return ShortestPathTo(Ext).Length;
        }

        #endregion

        #region ----------------------- INTERSECTS WITH ----------------------------
        /// <summary>
        /// Determines whether a point is inside or on the bounds of the rectangle
        /// </summary>
        /// <param name="Point">A MapWinGIS.Point parameter</param>
        /// <returns>True if the point is inside or on the bounds of the rectangle</returns>
        public bool Intersects(MapWinGIS.Point Point)
        {
            if (Point.x < xMin || Point.x > xMax) return false;
            if (Point.y < yMin || Point.y > yMax) return false;

            return true;
        }
        #endregion

        #region ----------------------- INTERSECTION AREA -------------------------
        /// <summary>
        /// The purpose of this is to reduce a potentially much larger extents down
        /// to a more manageable extents by making it just big enough to contain the complete
        /// intersection with a segment.
        /// </summary>
        /// <param name="Seg">LineSegment to look for an intersection with</param>
        /// <returns>An Envelope object that is the intersection between this extents and the segment.
        /// This should be smaller than the AOI of the extents and the segment's extents.</returns>
        public Envelope IntersectionArea(LineSegment Seg)
        {
            Envelope retExt = new Envelope(Seg);
            if (retExt == null) return null;

            // First rule out any with both points outside in the same direction
            if (Seg.X1 > xMax && Seg.X2 > xMax) return null;
            if (Seg.Y1 > yMax && Seg.X2 > yMax) return null;

            if (Seg.X1 < xMin && Seg.X2 < xMin) return null;
            if (Seg.Y1 < yMin && Seg.X2 < yMin) return null;

            // If both points of the segment are inside the extent area, just return the extents
            // of the segment.
            bool p2int;
            if (Intersects(Seg.StartPoint) == true)
            {
                // If both points are inside, the intersection area is the same as the segment extents
                p2int = Intersects(Seg.EndPoint);   
                if(p2int)return retExt;

                double slope = (Seg.Y2 - Seg.X2) / (Seg.X2 - Seg.Y2);
                // P1 is inside, but P2 is outside, so find the crossing point.
                
                if (Seg.X2 > xMax) retExt.xMax = xMax;
                if (Seg.Y2 < yMin) retExt.yMin = yMin;
                
            } 
            return retExt;
            
        }
        
       
        
        #endregion

        #region ----------------------- IS COMPLETELY WITHIN ----------------------

        /// <summary>
        /// For completeness.  Simply tests if the specified extents contain these 
        /// extents completely so that no borders are touching.
        /// </summary>
        /// <param name="Ext">The extents to compare against</param>
        /// <returns>Boolean, true if all the points of this extents lie within the other extents.</returns>
        bool IsCompletelyWithin(Envelope Ext)
        {
            return Ext.CompletelyContains(this);
        }

        #endregion

        #region ----------------------- IS CONTAINED BY ---------------------------
        /// <summary>
        /// For completeness.  Simply tests if the Envelope specified contains these extents.
        /// </summary>
        /// <param name="Ext">The extents to compare against these extents</param>
        /// <returns>Boolean, true if every corner of this rectangle intersects with the specified rectangle.</returns>
        bool Within(Envelope Ext)
        {
            return Ext.Contains(this);
        }
        #endregion

        #region ----------------------- IS CROSSED BY THE OUTLINE OF --------------
        /// <summary>
        /// This specifically checks the borders to see if any of the segments
        /// completely cross over each other.  Touching or overlapping won't
        /// evaluate as true. 
        /// </summary>
        /// <param name="Ext">The extents object to compare with.</param>
        /// <returns>Boolean, true if any of the segments from this </returns>
        bool IsCrossedByTheOutlineOf(Envelope Ext)
        {
            // the two extents have to intersect for crossing to be possible, which takes longer to check.
            if (Intersects(Ext) == false) return false;
            List<LineSegment> MySegs = ToLineSegments();
            List<LineSegment> Segs = Ext.ToLineSegments();
            for (int I = 0; I < MySegs.Count; I++)
            {
                for (int J = 0; J < Segs.Count; J++)
                {
                    if (MySegs[I].Crosses(Segs[J])) return true;
                }
            }
            return false;
        }
        #endregion

        #region ----------------------- IS IDENTICAL TO ---------------------------

        /// <summary>
        /// Checks against another extents to determine if all the extent values are the same
        /// </summary>
        /// <param name="Ext">The extent to compare to</param>
        /// <returns>Boolean, true if all four extents are the same</returns>
        bool IsIdenticalTo(Envelope Ext)
        {
            if (Ext.xMax != xMax) return false;
            if (Ext.xMin != xMin) return false;
            if (Ext.yMin != yMin) return false;
            if (Ext.yMax != yMax) return false;
            return true;
        }

        #endregion

        #region ----------------------- IS WITHIN A DISTANCE OF -------------------

        /// <summary>
        /// Checks to see if the shortest distance from this extents to the specified
        /// point is less than or equal to the specified distance.
        /// </summary>
        /// <param name="Pnt">A Topology2D.Point object to judge the distance to.</param>
        /// <param name="Dist">Double, the maximum allowed distance for the shortest path between objects.</param>
        /// <returns>Boolean, true if the actual distance is less than or equal to the specified value.</returns>
        bool IsWithinADistanceOf(Point Pnt, double Dist)
        {
            if (Distance(Pnt) <= Dist) return true;
            return false;
        }
        /// <summary>
        /// Checks to see if the shortest distance from this extents to the specified
        /// LineSegment is less than or equal to the specified distance.
        /// </summary>
        /// <param name="Seg">A Topology2D.LineSegment object to judge the distance to.</param>
        /// <param name="Dist">Double, the maximum allowed distance for the shortest path between objects.</param>
        /// <returns>Boolean, true if the actual distance is less than or equal to the specified value.</returns>
        bool IsWithinADistanceOf(LineSegment Seg, double Dist)
        {
            if (Distance(Seg) <= Dist) return true;
            return false;
        }
        /// <summary>
        /// Checks to see if the shortest distance from this extents to the specified
        /// Envelope is less than or equal to the specified distance.
        /// </summary>
        /// <param name="Ext">The Envelope object to compare with.</param>
        /// <param name="Dist">The maximum distance allowed</param>
        /// <returns>Boolean, true if the actual distance is less than or equal to the specified value.</returns>
        bool IsWithinADistanceOf(Envelope Ext, double Dist)
        {
            if (Distance(Ext) <= Dist) return true;
            return false;

        }
        #endregion

        #region ----------------------- SHORTEST PATH TO --------------------------

        /// <summary>
        /// Determines the pathway from the border of this extents object to the 
        /// specified geometry that is the shortest.  The startpoint will be on this extents
        /// object, while the endpoint will be on the specified point.
        /// </summary>
        /// <param name="geom">The geometry to find a path to.</param>
        /// <returns>Topology2D.LineSegment that is the shortest path between the extent borders and the specified point.
        /// Returns null if the point intersects with these extents.</returns>
        public override LineSegment ShortestPathTo(Geometry geom)
        {
            Type tp = geom.GetType();
            // -------------------------- POINT -------------------------------
            if (tp == typeof(Point))
            {
                Point Pnt = (Point)geom;
                if (Intersects(Pnt)) return null;
                List<LineSegment> MySegs = ToLineSegments();
                double Dist = double.PositiveInfinity;
                LineSegment ShortestPath = new LineSegment();

                for (int I = 0; I < MySegs.Count; I++)
                {
                    LineSegment path = MySegs[I].ShortestPathTo(Pnt);
                    if (path.Length < Dist)
                    {
                        Dist = path.Length;
                        ShortestPath = path;
                    }
                }
                return ShortestPath;
            }
            // -------------------- LINE SEGMENT --------------------------------
            if (tp == typeof(LineSegment))
            {
                LineSegment Seg = (LineSegment)geom;
                if (Intersects(Seg)) return null;
                List<LineSegment> MySegs = ToLineSegments();
                double Dist = double.PositiveInfinity;
                LineSegment ShortestPath = new LineSegment();

                for (int I = 0; I < MySegs.Count; I++)
                {
                    LineSegment path = MySegs[I].ShortestPathTo(Seg);
                    if (path.Length < Dist)
                    {
                        Dist = path.Length;
                        ShortestPath = path;
                    }
                }
                return ShortestPath;

            }
            // ------------------------ ENVELOPE ---------------------------------
            if (tp == typeof(Envelope))
            {
                Envelope Env = (Envelope)geom;
                if (Intersects(Env) == true) return null;


                List<LineSegment> Segs = Env.ToLineSegments();
                double Dist = double.PositiveInfinity;
                LineSegment ShortestPath = new LineSegment();

                for (int I = 0; I < Segs.Count; I++)
                {
                    LineSegment path = ShortestPathTo(Segs[I]);
                    if (path.Length < Dist)
                    {
                        Dist = path.Length;
                        ShortestPath = path;
                    }
                }
                return ShortestPath;
            }
            return geom.ShortestPathTo(this).Reverse();
        }
        
       
        #endregion

        #region ----------------------- TOUCHES THE BOUNDARY OF -------------------
        /// <summary>
        /// checks to see if any of the segments that make either the boundaries of this extents
        /// or the boundaries of the polygon have an endpoint that intersects with the opposite.
        /// </summary>
        /// <param name="Pgn">The polygon to check against.</param>
        /// <returns>Boolean, true if any segments from either the extent or polygon boundaries has an endpoint that intersects with the other.</returns>
        bool TouchesTheBoundaryOf(Polygon Pgn)
        {
            if (Pgn.Intersects(this) == false) return false;
            List<LineSegment> MySegs = LineSegmentsWithin(Pgn.Envelope);
            for (int I = 0; I < MySegs.Count; I++)
            {
                if (MySegs[I].TouchesTheBoundaryOf(Pgn)) return true;
            }
            List<LineSegment> Segs = Pgn.Shell.LineSegmentsWithin(this);
            for (int I = 0; I < Segs.Count; I++)
            {
                if (Segs[I].TouchesTheBoundaryOf(this)) return true;
            }
            return false;
        }
        /// <summary>
        /// Tests to see if any of the boundaries are the same or touch.
        /// </summary>
        /// <param name="Ext">The Envelope object to compare with.</param>
        /// <returns>True if any of the extents are the same.</returns>
        bool TouchesTheBoundaryOf(Envelope Ext)
        {
            if (Ext.Intersects(this) == false) return false;
            List<LineSegment> MySegs = LineSegmentsWithin(Ext);
            for (int I = 0; I < MySegs.Count; I++)
            {
                if (MySegs[I].TouchesTheBoundaryOf(Ext)) return true;
            }
            List<LineSegment> Segs = Ext.LineSegmentsWithin(this);
            for (int J = 0; J < Segs.Count; J++)
            {
                if (Segs[J].TouchesTheBoundaryOf(this)) return true;
            }
            return false;
        }


        #endregion

        #region ----------------------- ARE IDENTICAL TO --------------------------
        /// <summary>
        /// Checks each value in the point to see if they are the same
        /// </summary>
        /// <param name="Ext">The Envelope to compare with this point</param>
        /// <returns>Boolean, true if the points are the same</returns>
        public bool AreIdenticalTo(Envelope Ext)
        {
            if ((xMin == Ext.xMin) && (yMin == Ext.yMin)
             && (xMax == Ext.xMax) && (yMax == Ext.yMax)) return true;
            return false;
        }
        /// <summary>
        /// Checks each value in the point to see if they are the same
        /// </summary>
        /// <param name="Ext">The Envelope to compare with this point</param>
        /// <returns>Boolean, true if the points are the same</returns>
        public bool AreIdenticalTo(MapWinGIS.Extents Ext)
        {
            if ((xMin == Ext.xMin) && (yMin == Ext.yMin)
             && (xMax == Ext.xMax) && (yMax == Ext.yMax)) return true;
            return false;
        }
        #endregion

        #region ----------------------- CONVERSION TO -----------------------------
        /// <summary>
        /// Converts this rectangleD to a MapWinGIS.Extents object
        /// </summary>
        /// <returns>A MapWinGIS.Extents object with the same measurements as Envelope</returns>
        public MapWinGIS.Extents ToMapWinEnvelope()
        {
            MapWinGIS.Extents mwEnvelope = new MapWinGIS.Extents();
            mwEnvelope.SetBounds(xMin, yMin, 0, xMax, yMax, 0);
            return mwEnvelope;
        }

        /// <summary>
        /// Creates a list of segments that define the boundaries of the extents.
        /// </summary>
        /// <returns>List of LineSegments</returns>
        public List<LineSegment> ToLineSegments()
        {
            List<LineSegment> SegList = new List<LineSegment>();
            
            SegList.Add(new LineSegment(xMin, yMin, xMin, yMax));
            SegList.Add(new LineSegment(xMax, yMin, xMax, yMax));
            SegList.Add(new LineSegment(xMin, yMin, xMax, yMin));
            SegList.Add(new LineSegment(xMin, yMax, xMax, yMax));
           
            return SegList;
        }
        /// <summary>
        /// If this extents were a rectangle, this would return the borders of the
        /// rectangle that fall within the specified extents.
        /// </summary>
        /// <param name="Ext">An Envelope object specifying the region to return segments from.</param>
        /// <returns>A List of LineSegments containing any segments that have any intersection with Ext</returns>
        public List<LineSegment> LineSegmentsWithin(Envelope Ext)
        {
            List<LineSegment> SegList = new List<LineSegment>();
            LineSegment seg = new LineSegment(xMin, yMin, xMin, yMax);
            if (Ext.Intersects(seg)) SegList.Add(seg);
            seg = new LineSegment(xMax, yMin, xMax, yMax);
            if (Ext.Intersects(seg)) SegList.Add(seg);
            seg = new LineSegment(xMin, yMin, xMax, yMin);
            if (Ext.Intersects(seg)) SegList.Add(seg);
            seg = new LineSegment(xMin, yMax, xMax, yMax);
            if (Ext.Intersects(seg)) SegList.Add(seg);
            return SegList;
        }

        #endregion
        
        #endregion

        #region Operators

        #region -------------------- EQUAL -------------------------------
        /// <summary>
        /// Returns true if X and Y coordinates are equal for extremes of both envelopes
        /// </summary>
        /// <param name="U">An Envelope to compare</param>
        /// <param name="objV">Any object to compare with the Envelope</param>
        /// <returns>Boolean, true if the object is an equivalent envelope or rectangular polygon</returns>
        public static bool operator ==(Envelope U, Object objV)
        {
            object obU = U as object;

            if (objV == null)
            {
                if (obU == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
           
            Type tp = objV.GetType();
            if(tp == typeof(Polygon))
            {
                return U.Equals((Polygon)objV);
            }
            if(tp == typeof(Envelope))
            {
                return U.Equals((Envelope)objV);
            }
            return false;
        }
        /// <summary>
        /// Returns true if X and Y coordinates are different for extremes of both envelopes
        /// </summary>
        /// <param name="U">An Envelope to compare</param>
        /// <param name="objV">An object to compare</param>
        /// <returns>Boolean, false if the object is an equivalent envelope or rectangular polygon</returns>
        public static bool operator !=(Envelope U, Object objV)
        {
            Type tp = objV.GetType();
            if (tp == typeof(Polygon))
            {
                return !(U.Equals((Polygon)objV));
            }
            if (tp == typeof(Envelope))
            {
                return !(U.Equals((Envelope)objV));
            }
            return true;
        }

        #endregion

        #endregion
    }
}
