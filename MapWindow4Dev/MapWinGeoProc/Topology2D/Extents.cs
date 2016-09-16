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
    public class Extents
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
        public Extents()
        {
            xMin = 0;
            xMax = 0;
            yMin = 0;
            yMax = 0;
        }
        /// <summary>
        /// Creates a new instance of Extents from double values
        /// </summary>
        /// <param name="MinX">Double, The minimum X value</param>
        /// <param name="MaxX">Double, The largest X value</param>
        /// <param name="MinY">Double, The minimum Y value</param>
        /// <param name="MaxY">Double, the largest Y value</param>
        public Extents(double MinX, double MaxX, double MinY, double MaxY)
        {
            xMin = MinX;
            yMin = MinY;
            xMax = MaxX;
            yMax = MaxY;
        }

        /// <summary>
        /// Creates a new instance of Extents from double values in the MapWinGIS.Extents object
        /// </summary>
        /// <param name="Bounds">A MapWinGIS.Extents object to define a rectangle</param>
        public Extents(MapWinGIS.Extents Bounds)
        {
            xMin = Bounds.xMin;
            xMax = Bounds.xMax;
            yMin = Bounds.yMin;
            yMax = Bounds.yMax;
        }
        /// <summary>
        /// Calculates a new extents object that is just large enough to contain the entire segment.
        /// </summary>
        /// <param name="seg">A Segment to use as the "diagonal" of this extents object.</param>
        public Extents(Segment seg)
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

        #endregion

        #region Methods

       
        #region ----------------------- CLOSEST POINT ON --------------------------

        /// <summary>
        /// In the 2D case, the shortest path to the specified segment is on one of the segments here.
        /// This determines the shortest path and picks shortest distance.  Then it returns the point
        /// on the specified segment that is closest to this extent.
        /// </summary>
        /// <param name="Seg">A Segment to find the closest point on.</param>
        /// <returns>Topology2D.Point specifying the closest point on the specified segment.
        /// This returns null if there is an intersection.</returns>
        public Point ClosestPointOn(Segment Seg)
        {
            Segment Ret;
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
        /// <param name="Ext">Extents to compare to this object.</param>
        /// <returns>A point representing the closest point on the specified extents.  Returns null
        /// if the two extents intersect.</returns>
        public Point ClosestPointOn(Extents Ext)
        {
            Segment Ret;
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
        /// <returns>The Topology2D.Point representing the closest point on this Extents to the specified point.</returns>
        public Point ClosestPointTo(Point Pnt)
        {
            Segment Seg = ShortestPathTo(Pnt);
            if (Seg == null) return null;
            return Seg.StartPoint;
        }
        
        /// <summary>
        /// In the 2D case, the shortest path to the specified segment is on one of the segments here.
        /// This determines the shortest path and picks shortest distance.  Then it returns the point
        /// on the this extents that is closest to the specified segment.
        /// </summary>
        /// <param name="Seg">A Segment to compare to.</param>
        /// <returns>Topology2D.Point specifying the closest point on this extents to the specified segment.
        /// This returns null if there is an intersection.</returns>
        public Point ClosestPointTo(Segment Seg)
        {
            Segment Ret = ShortestPathTo(Seg);
            if (Ret == null) return null;
            return Ret.StartPoint;
        }

        /// <summary>
        /// If the two extents intersect, this returns null.
        /// This checks all the possible shortest paths between the 4 Borders of both extents.
        /// This finds the shortest one and then returns the location of the point on this object
        /// that is closest to the specified extents.
        /// </summary>
        /// <param name="Ext">Extents to compare to this object.</param>
        /// <returns>A point representing the closest point on this object to the specified extents.
        /// Returns null if the two extents intersect.</returns>
        public Point ClosestPointTo(Extents Ext)
        {
            Segment Seg = ShortestPathTo(Ext);
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
            // Extents are easy because we know how they are set up in 2D space
            if (IntersectWith(Pnt) == false) return false;
            if (Pnt.X == xMin || Pnt.X == xMax) return false;
            if (Pnt.Y == yMin || Pnt.Y == yMax) return false;
            return true;
        }
        /// <summary>
        /// Ensures that the segment is completely within the extents without touching the border.
        /// </summary>
        /// <param name="Seg">The Topology2D.Segment to compare against</param>
        /// <returns>Boolean, true if both points of the segment are completely within the extents</returns>
        public bool CompletelyContains(Segment Seg)
        {
            // Rather coolly, if both points are inside a rectangle, than no part of the middle can touch or be
            // outside
            if (CompletelyContains(Seg.StartPoint) && CompletelyContains(Seg.EndPoint)) return true;
            return false;
        }
        /// <summary>
        /// checks to ensure that this Extents completely contains the specified extents.
        /// </summary>
        /// <param name="Ext">The extents to compare to this extents</param>
        /// <returns>Boolean, true if the specified extents is within this extents without touching the borders</returns>
        public bool CompletelyContains(Extents Ext)
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

        #region ----------------------- CONTAINS ----------------------------------

        /// <summary>
        /// This is here for completeness, but is synonomous with IntersectsWith
        /// in the case of a point.
        /// </summary>
        /// <param name="Pnt">The Topology2D Point to test if it lies within these extents.</param>
        /// <returns>Boolean, true if the point is inside the extents or touches the border.</returns>
        public bool Contains(Point Pnt)
        {
            return IntersectWith(Pnt);
        }
        /// <summary>
        /// This will check to ensure that no part of the segment lies outside the domain of the rectangle
        /// 
        /// </summary>
        /// <param name="Seg">The Topology2D Segment to test</param>
        /// <returns>Boolean, False if any part of the segment lies outside the extents.</returns>
        public bool Contains(Segment Seg)
        {
            // Because of the nifty nature of rectangles, it is impossible to have any part
            // of a segment outside of the rectangle area if both endpoints are inside.
            if (IntersectWith(Seg.StartPoint) == false) return false;
            if (IntersectWith(Seg.EndPoint) == false) return false;
            return true;
        }

        /// <summary>
        /// Checks against the specified extents to ensure that the entire scope of the specified
        /// extents are within these extents.  This can be done by checking the four endpoints.
        /// </summary>
        /// <param name="Ext">The Extents object to compare against.</param>
        /// <returns>Boolean, True as long as every corner intersects with this extent.</returns>
        public bool Contains(Extents Ext)
        {
            if (IntersectWith(new Point(Ext.xMin, Ext.yMin)) == false) return false;
            if (IntersectWith(new Point(Ext.xMin, Ext.yMax)) == false) return false;
            if (IntersectWith(new Point(Ext.xMax, Ext.yMin)) == false) return false;
            if (IntersectWith(new Point(Ext.xMax, Ext.yMax)) == false) return false;
            return true;
        }

        #endregion

        #region ----------------------- COPY --------------------------------------

        /// <summary>
        /// Returns a new instance of the Extents class with the same values as this object.
        /// </summary>
        /// <returns>Topology2D.Extents with identical properties.</returns>
        public Extents Copy()
        {
            Extents NewExt = new Extents(xMin, xMax, yMin, yMax);
            return NewExt;
        }

        #endregion

        #region ----------------------- DISTANCE TO -------------------------------

        /// <summary>
        /// Finds the shortest path to the specified point, and returns the length.
        /// </summary>
        /// <param name="Pnt">The Topology2D.Point to test agianst.</param>
        /// <returns>Double, the length of the shortest path.  0 if intersects.</returns>
        public double DistanceTo(Point Pnt)
        {
            if (IntersectWith(Pnt) == true) return 0.0;
            return ShortestPathTo(Pnt).Length;
        }
        /// <summary>
        /// Finds the path between the two closest points between this object and the segment.
        /// Returns the length.  Returns 0.0 if the two intersect.
        /// </summary>
        /// <param name="Seg">The Topology2D.Segment to compare to.</param>
        /// <returns>Double, the shortest possible distance between this extents and the segment.</returns>
        public double DistanceTo(Segment Seg)
        {
            if (IntersectWith(Seg) == true) return 0.0;
            return ShortestPathTo(Seg).Length;
        }

        /// <summary>
        /// Finds the shortest path between the two closest points on the two extents objects.
        /// Returns 0.0 if the two intersect.
        /// </summary>
        /// <param name="Ext">The Topology2D.Extents to compare to.</param>
        /// <returns>Double, the magnitude of shortest path between the extents.</returns>
        public double DistanceTo(Extents Ext)
        {
            if (IntersectWith(Ext) == true) return 0.0;
            return ShortestPathTo(Ext).Length;
        }

        #endregion

        #region ----------------------- INTERSECT WITH ----------------------------
        /// <summary>
        /// Determines whether a point is inside or on the bounds of the rectangle
        /// </summary>
        /// <param name="Point">A MapWinGIS.Point parameter</param>
        /// <returns>True if the point is inside or on the bounds of the rectangle</returns>
        public bool IntersectWith(MapWinGIS.Point Point)
        {
            if (Point.x < xMin || Point.x > xMax) return false;
            if (Point.y < yMin || Point.y > yMax) return false;

            return true;
        }
        /// <summary>
        /// Determines whether a point is inside or on the bounds of the rectangle
        /// </summary>
        /// <param name="Point">A Topology.Point parameter</param>
        /// <returns>True if the point is inside or on the bounds of the rectangle</returns>
        public bool IntersectWith(Point Point)
        {
            if (Point.X < xMin || Point.X > xMax) return false;
            if (Point.Y < yMin || Point.Y > yMax) return false;

            return true;
        }
        /// <summary>
        /// Returns true if any part of the segment has coordinates within the extents
        /// </summary>
        /// <param name="Seg">The line segment to test</param>
        /// <returns>Boolean, true if any part of the segment is within the extents</returns>
        public bool IntersectWith(Segment Seg)
        {
            // First rule out any with both points outside in the same direction
            if (Seg.X1 > xMax && Seg.X2 > xMax) return false;
            if (Seg.Y1 > yMax && Seg.X2 > yMax) return false;

            if (Seg.X1 < xMin && Seg.X2 < xMin) return false;
            if (Seg.Y1 < yMin && Seg.X2 < yMin) return false;

            // Next include any with at least one point inside the cube
            if (IntersectWith(Seg.StartPoint)) return true;
            if (IntersectWith(Seg.EndPoint)) return true;


            // Hopefully most cases will be solved before this, but some wierd cases require special
            // handling like segments passing clean through the box.
            Point TestPoint;

            // Test if segment contacts bounds at Xmax
            if (Seg.LocationByX(xMax, out TestPoint) == true)
            {
                // we obtained a valid point where X = Xmax, test if it is in bounds
                if (TestPoint.Y >= yMin && TestPoint.Y <= yMax) return true;
            }
            // Test if segment contacts bounds at Xmin
            if (Seg.LocationByX(xMin, out TestPoint) == true)
            {
                // we obtained a valid point where X = Xmin, test if it is in bounds
                if (TestPoint.Y >= yMin && TestPoint.Y <= yMax) return true;
            }
            // Test if segment contacts bounds at Ymax
            if (Seg.LocationByY(yMax, out TestPoint) == true)
            {
                // we obtained a valid point where Y = Ymax, test if it is in bounds
                if (TestPoint.X >= xMin && TestPoint.X <= xMax) return true;
            }
            // Test if segment contacts bounds at Ymin
            if (Seg.LocationByY(yMin, out TestPoint) == true)
            {
                // we obtained a valid point where Y = Ymin, test if it is in bounds
                if (TestPoint.X >= xMin && TestPoint.X <= xMax) return true;
            }


            // Since our segment did not intersect with any of the sides, we can conclude that
            // it doesn't intersect anywhere with our bounds.
            return false;
        }

        /// <summary>
        /// Returns true if any part of the two extents overlap or touch.
        /// </summary>
        /// <param name="Ext">An extents object to test</param>
        /// <returns>Boolean, true if the rectangles overlap, false otherwise.</returns>
        public bool IntersectWith(Extents Ext)
        {
            // First rule out any with both points outside in the same direction
            Extents res = Intersection(Ext);
            if (res == null) return false;
            return true;
        }
        #endregion

        #region ----------------------- INTERSECTION ------------------------------
        /// <summary>
        /// The purpose of this is to reduce a potentially much larger extents down
        /// to a more manageable extents by making it just big enough to contain the complete
        /// intersection with a segment.
        /// </summary>
        /// <param name="Seg">Segment to look for an intersection with</param>
        /// <returns>An Extents object that is the intersection between this extents and the segment.
        /// This should be smaller than the AOI of the extents and the segment's extents.</returns>
        public Extents IntersectionArea(Segment Seg)
        {
            Extents retExt = new Extents(Seg);
            if (retExt == null) return null;

            // First rule out any with both points outside in the same direction
            if (Seg.X1 > xMax && Seg.X2 > xMax) return null;
            if (Seg.Y1 > yMax && Seg.X2 > yMax) return null;

            if (Seg.X1 < xMin && Seg.X2 < xMin) return null;
            if (Seg.Y1 < yMin && Seg.X2 < yMin) return null;

            // If both points of the segment are inside the extent area, just return the extents
            // of the segment.
            bool p2int;
            if (IntersectWith(Seg.StartPoint) == true)
            {
                // If both points are inside, the intersection area is the same as the segment extents
                p2int = IntersectWith(Seg.EndPoint);   
                if(p2int)return retExt;

                double slope = (Seg.Y2 - Seg.X2) / (Seg.X2 - Seg.Y2);
                // P1 is inside, but P2 is outside, so find the crossing point.
                
                if (Seg.X2 > xMax) retExt.xMax = xMax;
                if (Seg.Y2 < yMin) retExt.yMin = yMin;
                
            } 
            return retExt;
            
        }
        
        /// <summary>
        /// Returns a segment representing the intersection of the submitted segment and this
        /// extents rectangle.
        /// </summary>
        /// <param name="Seg">The segment that you want to compare to the borders of this extent</param>
        /// <returns>Segment: null = no intersections, otherwise the part of Seg that intersects with this object.</returns>
        public Segment Intersection(Segment Seg)
        {
            Segment Overlap;
            List<Point> CrossingPoints = new List<Point>();
            Overlap = null;
             // First rule out any with both points outside in the same direction
            if (Seg.X1 > xMax && Seg.X2 > xMax) return null;
            if (Seg.Y1 > yMax && Seg.X2 > yMax) return null;

            if (Seg.X1 < xMin && Seg.X2 < xMin) return null;
            if (Seg.Y1 < yMin && Seg.X2 < yMin) return null;
            
            List<Segment> mySegs = ToSegments();
            for (int I = 0; I < mySegs.Count; I++)
            {
                Overlap = mySegs[I].Intersection(Seg);
                if (Overlap == null) continue;
                if (Overlap.StartPoint == Overlap.EndPoint)
                {
                    CrossingPoints.Add(Overlap.StartPoint);
                }
                else
                {
                    // If the overlap is congruant with this segment, then no other border segments
                    // can influence our intersection.
                    return Overlap;
                }
            }
            if (CrossingPoints.Count == 0) return null;
            // at this point we have a list of crossing points, but we are unsure about what is inside
            // or outside of our point.
            // either 1 or 2 crossing points exist.  If 2 exist, we can just connect them.
            if (CrossingPoints.Count == 2)
            {
                return new Segment(CrossingPoints[0], CrossingPoints[1]);
            }
            // One crossing point exists, so we have to figure out which point is inside 
            if (IntersectWith(Seg.StartPoint) == true)
            {
                return new Segment(Seg.StartPoint, CrossingPoints[0]);
            }
            else
            {
                return new Segment(Seg.EndPoint, CrossingPoints[0]);
            }
        }

        /// <summary>
        /// Returns a RectD that has the intersection area for both rectangles
        /// </summary>
        /// <param name="Rectangle2">A Extents that you wish to intersect with this rectangle</param>
        /// <returns>The Extents of the intersection area</returns>
        public Extents Intersection(Extents Rectangle2)
        {
            Extents intersect = new Extents();
            // check to see if we have an intersection first
            if (xMin > Rectangle2.xMax) return intersect;
            if (Rectangle2.xMin > xMax) return intersect;
            if (yMin > Rectangle2.yMax) return intersect;
            if (Rectangle2.yMin > yMax) return intersect;


            // We intersect, so go ahead and find the values
            intersect.xMax = Math.Min(xMax, Rectangle2.xMax);
            intersect.xMin = Math.Max(xMin, Rectangle2.xMin);
            intersect.yMax = Math.Min(yMax, Rectangle2.yMax);
            intersect.yMin = Math.Max(yMin, Rectangle2.yMin);

            return intersect;
        }
        #endregion

        #region ----------------------- IS COMPLETELY WITHIN ----------------------

        /// <summary>
        /// For completeness.  Simply tests if the specified extents contain these 
        /// extents completely so that no borders are touching.
        /// </summary>
        /// <param name="Ext">The extents to compare against</param>
        /// <returns>Boolean, true if all the points of this extents lie within the other extents.</returns>
        bool IsCompletelyWithin(Extents Ext)
        {
            return Ext.CompletelyContains(this);
        }

        #endregion

        #region ----------------------- IS CONTAINED BY ---------------------------
        /// <summary>
        /// For completeness.  Simply tests if the Extents specified contains these extents.
        /// </summary>
        /// <param name="Ext">The extents to compare against these extents</param>
        /// <returns>Boolean, true if every corner of this rectangle intersects with the specified rectangle.</returns>
        bool IsContainedBy(Extents Ext)
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
        bool IsCrossedByTheOutlineOf(Extents Ext)
        {
            // the two extents have to intersect for crossing to be possible, which takes longer to check.
            if (IntersectWith(Ext) == false) return false;
            List<Segment> MySegs = ToSegments();
            List<Segment> Segs = Ext.ToSegments();
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
        bool IsIdenticalTo(Extents Ext)
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
        /// <param name="Distance">Double, the maximum allowed distance for the shortest path between objects.</param>
        /// <returns>Boolean, true if the actual distance is less than or equal to the specified value.</returns>
        bool IsWithinADistanceOf(Point Pnt, double Distance)
        {
            if (DistanceTo(Pnt) <= Distance) return true;
            return false;
        }
        /// <summary>
        /// Checks to see if the shortest distance from this extents to the specified
        /// Segment is less than or equal to the specified distance.
        /// </summary>
        /// <param name="Seg">A Topology2D.Segment object to judge the distance to.</param>
        /// <param name="Distance">Double, the maximum allowed distance for the shortest path between objects.</param>
        /// <returns>Boolean, true if the actual distance is less than or equal to the specified value.</returns>
        bool IsWithinADistanceOf(Segment Seg, double Distance)
        {
            if (DistanceTo(Seg) <= Distance) return true;
            return false;
        }
        /// <summary>
        /// Checks to see if the shortest distance from this extents to the specified
        /// Extents is less than or equal to the specified distance.
        /// </summary>
        /// <param name="Ext">The Extents object to compare with.</param>
        /// <param name="Distance">The maximum distance allowed</param>
        /// <returns>Boolean, true if the actual distance is less than or equal to the specified value.</returns>
        bool IsWithinADistanceOf(Extents Ext, double Distance)
        {
            if (DistanceTo(Ext) <= Distance) return true;
            return false;

        }
        #endregion

        #region ----------------------- SHORTEST PATH TO --------------------------

        /// <summary>
        /// Determines the pathway from the border of this extents object to the 
        /// specified point that is the shortest.  The startpoint will be on this extents
        /// object, while the endpoint will be on the specified point.
        /// </summary>
        /// <param name="Pnt">The point to find a path to.</param>
        /// <returns>Topology2D.Segment that is the shortest path between the extent borders and the specified point.
        /// Returns null if the point intersects with these extents.</returns>
        public Segment ShortestPathTo(Point Pnt)
        {
            if (IntersectWith(Pnt)) return null;
            List<Segment> MySegs = ToSegments();
            double Dist = double.PositiveInfinity;
            Segment ShortestPath = new Segment();

            for (int I = 0; I < MySegs.Count; I++)
            {
                Segment path = MySegs[I].ShortestPathTo(Pnt);
                if (path.Length < Dist)
                {
                    Dist = path.Length;
                    ShortestPath = path;
                }
            }
            return ShortestPath;
        }
        /// <summary>
        /// Determines the shortest possible path from any of the border segments of this extent to the
        /// specified segment.  Returns null if the two intersect.
        /// </summary>
        /// <param name="Seg">The Segment to compare to this extent.</param>
        /// <returns>Topology2D.Segment with a startpoint somewhere on this extent and an endpoint on 
        /// the specified segment that represents the shortest path.</returns>
        public Segment ShortestPathTo(Segment Seg)
        {
            if (IntersectWith(Seg)) return null;
            List<Segment> MySegs = ToSegments();
            double Dist = double.PositiveInfinity;
            Segment ShortestPath = new Segment();

            for (int I = 0; I < MySegs.Count; I++)
            {
                Segment path = MySegs[I].ShortestPathTo(Seg);
                if (path.Length < Dist)
                {
                    Dist = path.Length;
                    ShortestPath = path;
                }
            }
            return ShortestPath;
        }
        /// <summary>
        /// Finds the shortest path starting at this extents and ending at the specified one.
        /// </summary>
        /// <param name="Ext">The Topology2D.Extents object to compare to.</param>
        /// <returns>Either a Topology2D.Segment with a startpoint on this object's borders
        /// and an endpoint on the specified extents, or null if the two intersect.</returns>
        public Segment ShortestPathTo(Extents Ext)
        {
            if (IntersectWith(Ext) == true) return null;
    
            
            List<Segment> Segs = Ext.ToSegments();
            double Dist = double.PositiveInfinity;
            Segment ShortestPath = new Segment();

            for (int I = 0; I < Segs.Count; I++)
            {
                Segment path = ShortestPathTo(Segs[I]);
                if (path.Length < Dist)
                {
                    Dist = path.Length;
                    ShortestPath = path;
                }
            }
            return ShortestPath;
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
            if (Pgn.IntersectsWith(this) == false) return false;
            List<Segment> MySegs = SegmentsWithin(Pgn.Extents);
            for (int I = 0; I < MySegs.Count; I++)
            {
                if (MySegs[I].TouchesTheBoundaryOf(Pgn)) return true;
            }
            List<Segment> Segs = Pgn.SegmentsWithin(this);
            for (int I = 0; I < Segs.Count; I++)
            {
                if (Segs[I].TouchesTheBoundaryOf(this)) return true;
            }
            return false;
        }
        /// <summary>
        /// Tests to see if any of the boundaries are the same or touch.
        /// </summary>
        /// <param name="Ext">The Extents object to compare with.</param>
        /// <returns>True if any of the extents are the same.</returns>
        bool TouchesTheBoundaryOf(Extents Ext)
        {
            if (Ext.IntersectWith(this) == false) return false;
            List<Segment> MySegs = SegmentsWithin(Ext);
            for (int I = 0; I < MySegs.Count; I++)
            {
                if (MySegs[I].TouchesTheBoundaryOf(Ext)) return true;
            }
            List<Segment> Segs = Ext.SegmentsWithin(this);
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
        /// <param name="Ext">The Extents to compare with this point</param>
        /// <returns>Boolean, true if the points are the same</returns>
        public bool AreIdenticalTo(Extents Ext)
        {
            if ((xMin == Ext.xMin) && (yMin == Ext.yMin)
             && (xMax == Ext.xMax) && (yMax == Ext.yMax)) return true;
            return false;
        }
        /// <summary>
        /// Checks each value in the point to see if they are the same
        /// </summary>
        /// <param name="Ext">The Extents to compare with this point</param>
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
        /// <returns>A MapWinGIS.Extents object with the same measurements as Extents</returns>
        public MapWinGIS.Extents ToMapWinExtents()
        {
            MapWinGIS.Extents mwExtents = new MapWinGIS.Extents();
            mwExtents.SetBounds(xMin, yMin, 0, xMax, yMax, 0);
            return mwExtents;
        }

        /// <summary>
        /// Creates a list of segments that define the boundaries of the extents.
        /// </summary>
        /// <returns>List of Segments</returns>
        public List<Segment> ToSegments()
        {
            List<Segment> SegList = new List<Segment>();
            
            SegList.Add(new Segment(xMin, yMin, xMin, yMax));
            SegList.Add(new Segment(xMax, yMin, xMax, yMax));
            SegList.Add(new Segment(xMin, yMin, xMax, yMin));
            SegList.Add(new Segment(xMin, yMax, xMax, yMax));
           
            return SegList;
        }
        /// <summary>
        /// If this extents were a rectangle, this would return the borders of the
        /// rectangle that fall within the specified extents.
        /// </summary>
        /// <param name="Ext">An Extents object specifying the region to return segments from.</param>
        /// <returns>A List of Segments containing any segments that have any intersection with Ext</returns>
        public List<Segment> SegmentsWithin(Extents Ext)
        {
            List<Segment> SegList = new List<Segment>();
            Segment seg = new Segment(xMin, yMin, xMin, yMax);
            if (Ext.IntersectWith(seg)) SegList.Add(seg);
            seg = new Segment(xMax, yMin, xMax, yMax);
            if (Ext.IntersectWith(seg)) SegList.Add(seg);
            seg = new Segment(xMin, yMin, xMax, yMin);
            if (Ext.IntersectWith(seg)) SegList.Add(seg);
            seg = new Segment(xMin, yMax, xMax, yMax);
            if (Ext.IntersectWith(seg)) SegList.Add(seg);
            return SegList;
        }

        #endregion
        
        #endregion
    }
}
