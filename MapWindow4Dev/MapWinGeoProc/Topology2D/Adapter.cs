using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Topology2D
{
    /// <summary>
    /// Supports some functions related to polygon adjacency etc.
    /// </summary>
    public static class Adapter
    {
        

        /// <summary>
        /// reads a MapWinGIS.Shape to find the ShapeCategory
        /// </summary>
        /// <param name="Shape">A MapWinGIS.Shape object to learn the category of</param>
        /// <returns>A ShapeCategories enumeration</returns>
        public static ShapeCategories GetCategory(MapWinGIS.Shape Shape)
        {
            if (Shape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGON ||
               Shape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGONM ||
               Shape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGONZ)
            {
                return ShapeCategories.Polygon;
            }
            if (Shape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYLINE ||
                Shape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYLINEM ||
                Shape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYLINEZ)
            {
                return ShapeCategories.Line;
       
            }
            if (Shape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPATCH ||
                Shape.ShapeType == MapWinGIS.ShpfileType.SHP_NULLSHAPE)
            {
                return ShapeCategories.Invalid;
                
            }
            if (Shape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPOINT ||
                Shape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPOINTM ||
                Shape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPOINTZ)
            {
                return ShapeCategories.MultiPoint;
            }
            return ShapeCategories.Point;
            
        }
        
        #region ------------------------- SHAPES TOUCH ---------------------------------------

        /// <summary>
        /// Checks whether the borders of a polygon shape, lines of line shapes, or points of point shapes touch.
        /// </summary>
        /// <param name="Shape1">A MapWinGIS.Shape object to test</param>
        /// <param name="Shape2">A MapWinGIS.Shape object to test</param>
        /// <returns>True if the boundaries touch</returns>
        public static bool ShapeBoundariesTouch(MapWinGIS.Shape Shape1, MapWinGIS.Shape Shape2)
        {
            if (Shape1 == null || Shape1.ShapeType == MapWinGIS.ShpfileType.SHP_NULLSHAPE)
                throw new ArgumentException("Argument Shape1 cannot be null");
            if (Shape2 == null || Shape2.ShapeType == MapWinGIS.ShpfileType.SHP_NULLSHAPE) 
                throw new ArgumentException("Argument Shape2 cannot be null");
            if (Shape1.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPATCH) 
                throw new ArgumentException("Multipatch is not supported");
            if (Shape2.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPATCH) 
                throw new ArgumentException("Multipatch is not supported");

            ShapeCategories Typ1, Typ2;
            Typ1 = GetCategory(Shape1);
            Typ2 = GetCategory(Shape2);

            // for single points, simply show if these specific points overlap
            if (Typ1 == ShapeCategories.Point &&
                Typ2 == ShapeCategories.Point)
            {
                Point p1 = new Point(Shape1.get_Point(0));
                Point p2 = new Point(Shape2.get_Point(0));
                if (p1.Intersects(p2)) return true;
                return false;
            }

            // Point to Non-point
            if (Typ1 == ShapeCategories.Point)
            {
                Point p1 = new Point(Shape1.get_Point(0));

                if (Typ2 == ShapeCategories.MultiPoint)
                {
                    for (int I = 0; I < Shape2.numPoints; I++)
                    {
                        Point p2 = new Point(Shape2.get_Point(I));
                        if (p1.Intersects(p2)) return true;
                    }
                    return false;
                }
                else
                {
                    for (int I = 0; I < Shape2.numPoints - 1; I++)
                    {
                        LineSegment seg = new LineSegment(Shape2.get_Point(I), Shape2.get_Point(I + 1));
                        if (seg.Intersects(p1)) return true;
                    }
                    return false;
                }
            }

            // Point2 to non-point
            if (Typ2 == ShapeCategories.Point)
            {
                Point p2 = new Point(Shape2.get_Point(0));

                if (Typ1 == ShapeCategories.MultiPoint)
                {
                    for (int I = 0; I < Shape1.numPoints; I++)
                    {
                        Point p1 = new Point(Shape1.get_Point(I));
                        if (p2.Intersects(p1)) return true;
                    }
                    return false;
                }
                else
                {
                    for (int I = 0; I < Shape1.numPoints - 1; I++)
                    {
                        LineSegment seg = new LineSegment(Shape1.get_Point(I), Shape1.get_Point(I + 1));
                        if (seg.Intersects(p2)) return true;
                    }
                    return false;
                }
            }

            // for multipoint, test every point for intersection.
            if (Typ1 == ShapeCategories.MultiPoint &&
                Typ2 == ShapeCategories.MultiPoint)
            {
                List<int> Points1 = PointsWithinEnvelope(Shape1, Shape2.Extents);
                List<int> Points2 = PointsWithinEnvelope(Shape2, Shape1.Extents);
                for (int I = 0; I < Points1.Count; I++)
                {
                    for (int J = 0; J < Points2.Count; J++)
                    {
                        Point p1 = new Point(Shape1.get_Point(I));
                        Point p2 = new Point(Shape2.get_Point(J));
                        if (p1.Intersects(p2)) return true;
                    }
                    
                }
                return false;
            }
            // For lines and polygons simply test line segments in the area of interrest to see if they touch
            // (touching in this case just equates to having a minimum distance of 0.
            if ((Typ1 == ShapeCategories.Line || Typ1 == ShapeCategories.Polygon) &&
                (Typ2 == ShapeCategories.Line || Typ2 == ShapeCategories.Polygon))
            {
                List<LineSegment> Segs1 = LineSegmentsWithinEnvelope(Shape1, Shape2.Extents);
                List<LineSegment> Segs2 = LineSegmentsWithinEnvelope(Shape2, Shape1.Extents);
                for (int I = 0; I < Segs1.Count; I++)
                {
                    for (int J = 0; J < Segs2.Count; J++)
                    {
                        if (Segs1[I].Intersects(Segs2[J])) return true;
                        
                    }
                    
                }
                return false;
            }
           
            // multi-point to polygon
            if (Typ1 == ShapeCategories.MultiPoint)
            {
                List<int> Points1 = PointsWithinEnvelope(Shape1, Shape2.Extents);
                List<LineSegment> Segs2 = LineSegmentsWithinEnvelope(Shape2, Shape1.Extents);
                for (int I = 0; I < Points1.Count; I++)
                {
                    for (int J = 0; J < Segs2.Count; J++)
                    {
                        if (Segs2[J].Intersects(Shape1.get_Point(Points1[I]))) return true;
                        
                    }
                    
                }
                return false;
            }

            if (Typ2 == ShapeCategories.MultiPoint)
            {
                List<int> Points2 = PointsWithinEnvelope(Shape2, Shape1.Extents);
                List<LineSegment> Segs1 = LineSegmentsWithinEnvelope(Shape1, Shape2.Extents);
                for (int I = 0; I < Points2.Count; I++)
                {
                    for (int J = 0; J < Segs1.Count; J++)
                    {
                        if (Segs1[J].Intersects(Shape2.get_Point(Points2[I]))) return true;
                        
                    }
                    
                }
                return false;
            }
           


            return false;
        }
        // Only returns true if a point from shape1 is the same location as shape2
        private static bool MultiPoints_TouchTheBoundaryOf(MapWinGIS.Shape Shape1, MapWinGIS.Shape Shape2)
        {
            List<int> Points1 = new List<int>(); // Points from Shape1 where the rectangular shape extents overlap
            List<int> Points2 = new List<int>(); // Points from Shape2 where the rectangular shape extents overlap
            MapWinGIS.Point pt1, pt2;
            PointsOfInterest(Shape1, Shape2, ref Points1, ref Points2);
            for (int I = 0; I < Points1.Count; I++)
            {
                for (int J = 0; J < Points2.Count; J++)
                {
                    pt1 = Shape1.get_Point(Points1[I]);
                    pt2 = Shape2.get_Point(Points2[J]);
                    if (pt1.x == pt2.x && pt1.y == pt2.y) return true;
                }
            }
            return false;
        }
        
        private static bool LinesOrPolygons_TouchTheBoundaryOf(MapWinGIS.Shape Shape1, MapWinGIS.Shape Shape2)
        {
            List<int> Points1 = new List<int>();
            List<int> Points2 = new List<int>();
            List<LineSegment> LineSegments1 = new List<LineSegment>();
            List<LineSegment> LineSegments2 = new List<LineSegment>();
            // only worry about the points where the extents intersect
            LineSegmentsOfInterest(Shape1, Shape2, ref LineSegments1, ref LineSegments2);
          
            for (int I = 0; I < LineSegments1.Count; I++)
            {
                for (int J = 0; J < LineSegments2.Count; J++)
                {
                    if (LineSegments1[I].Intersects(LineSegments2[J])) return true;
                }
            }
            return false;
        }// End FastPolygonsIntersect

        #region --------------------------- POINTS WITHIN EXTENTS ----------------------------------

        /// <summary>
        /// Finds a list of point indecies from a MapWinGIS.Shape that are within the extents specified
        /// </summary>
        /// <param name="Shape">Any shapefile with points.</param>
        /// <param name="Envelope">A MapWinGIS.Extents object representing the area of interrest</param>
        /// <returns>Returns a list of integer point indecies in Shape that are within Envelope</returns>
        public static List<int> PointsWithinEnvelope(MapWinGIS.Shape Shape, MapWinGIS.Extents Envelope)
        {
            Envelope Rect;
            Rect = new Envelope(Envelope);
            return PointsWithinRect(Shape, Rect);
        }
        /// <summary>
        /// Finds a list of point indecies from a MapWinGIS.Shape that are within the rectangle specified
        /// </summary>
        /// <param name="Shape">A mapWinGIS.Shape object to select points from</param>
        /// <param name="Rect">A Envelope structure representing a rectangle to search</param>
        /// <returns>A List of integer values representing the index values of the points within the rectangle</returns>
        public static List<int> PointsWithinRect(MapWinGIS.Shape Shape, Envelope Rect)
        {
            List<int> ContainedPoints = new List<int>();
            Envelope RectShape, rectIntersect;
            RectShape = new Envelope(Shape.Extents);
            rectIntersect = (Envelope)RectShape.Intersection(Rect);
            MapWinGIS.Point pt = new MapWinGIS.Point();
            int numPoints = Shape.numPoints;
            for (int I = 0; I < numPoints; I++)
            {
                pt = Shape.get_Point(I);
                if (rectIntersect.Intersects(pt))
                {
                    ContainedPoints.Add(I);
                }
            }
            return ContainedPoints;
        }

        #endregion

        #region --------------------------- SEGMENTS WITHIN EXTENTS --------------------------------

        /// <summary>
        /// Finds the points within extents for polygons and polylines and automatically creates a list
        /// of appropriate line segments representing all the segments found within the extents.
        /// </summary>
        /// <param name="LineOrPolygon">A MapWinGIS.Shape that is a line or polygon shape</param>
        /// <param name="Envelope">A MapWinGIS.Extents object showing the area of interrest</param>
        /// <returns>A list of LineSegment structures that have at least one point within or on extents</returns>
        public static List<LineSegment> LineSegmentsWithinEnvelope(MapWinGIS.Shape LineOrPolygon, MapWinGIS.Extents Envelope)
        {
            Envelope rect = new Envelope(Envelope);
            return LineSegmentsWithinRect(LineOrPolygon, rect);
        }
        /// <summary>
        /// Finds the points within extents for polygons and polylines and automatically creates a list
        /// of appropriate line segments representing all the segments found within the extents.
        /// </summary>
        /// <param name="LineOrPolygon">A MapWinGIS.Shape that is a line or polygon shape</param>
        /// <param name="Rect">A Envelope defining the region of interrest</param>
        /// <returns>A list of all segments that have at least one point within or on the extents</returns>
        public static List<LineSegment> LineSegmentsWithinRect(MapWinGIS.Shape LineOrPolygon, Envelope Rect)
        {
            List<LineSegment> ContainedLineSegments = new List<LineSegment>();
            ShapeCategories Category = GetCategory(LineOrPolygon);
            if (LineOrPolygon == null) throw new ArgumentException("Argument for Line or Polygon cannot be null");
            if (Category != ShapeCategories.Polygon && Category != ShapeCategories.Line) 
                throw new ArgumentException("The input argument must be a line or polygon or else it doesn't have segments.");
            Envelope rectShape, rectIntersect;
            rectShape = new Envelope(LineOrPolygon.Extents);
            rectIntersect = (Envelope)rectShape.Intersection(Rect);
            int numPoints = LineOrPolygon.numPoints;
            bool PreviousPointIncluded = true; // the first and last points are the same, so don't look backwards from point 1
            for (int I = 0; I < numPoints-1; I++)
            {
                MapWinGIS.Point pt = LineOrPolygon.get_Point(I);
                if (rectIntersect.Intersects(pt))
                {
                    if (PreviousPointIncluded == false)
                    {
                        ContainedLineSegments.Add(new LineSegment(LineOrPolygon.get_Point(I - 1), pt));
                    }
                    // since this point is contained, we know for sure that we need the next segment
                    ContainedLineSegments.Add(new LineSegment(LineOrPolygon.get_Point(I), LineOrPolygon.get_Point(I + 1)));
                    PreviousPointIncluded = true;
                }
                else
                {
                    PreviousPointIncluded = false;
                }
            }
            return ContainedLineSegments;
        }

        #endregion


        // Returns only the point index values of the shapes in the interesection of the shape extents
        private static void PointsOfInterest(MapWinGIS.Shape Shape1, MapWinGIS.Shape Shape2, ref List<int> Points1, ref List<int> Points2)
        {
            Envelope Rect1, Rect2, rectIntersect;
            Rect1 = new Envelope(Shape1.Extents);
            Rect2 = new Envelope(Shape2.Extents);
            rectIntersect = (Envelope)Rect1.Intersection(Rect2);
            MapWinGIS.Point pt = new MapWinGIS.Point();
            // reduce our points to points of interrest
            int numPoints1 = Shape1.numPoints;
            for (int I = 0; I < numPoints1; I++)
            {
                pt = Shape1.get_Point(I);
                if (rectIntersect.Intersects(pt))
                {
                   Points1.Add(I);
                }
            }

            int numPoints2 = Shape2.numPoints;
            for (int I = 0; I < numPoints2; I++)
            {
                pt = Shape2.get_Point(I);
                if (rectIntersect.Intersects(pt))
                {
                   Points2.Add(I);
                }
                
            }
        }
      
        // Returns only the point index values of the shapes in the interesection of the shape extents
        private static void LineSegmentsOfInterest(MapWinGIS.Shape Shape1, MapWinGIS.Shape Shape2, ref List<LineSegment> LineSegments1, ref List<LineSegment> LineSegments2)
        {
            Envelope Rect1, Rect2, rectIntersect;
            Rect1 = new Envelope(Shape1.Extents);
            Rect2 = new Envelope(Shape2.Extents);
            rectIntersect = (Envelope)Rect1.Intersection(Rect2);
            MapWinGIS.Point pt = new MapWinGIS.Point();
            // reduce our points to points of interrest
            int numPoints1 = Shape1.numPoints;
            bool PreviousPointIncluded = true; // we will get the first point by wrapping from the last point
            for (int I = 0; I < numPoints1; I++)
            {
                pt = Shape1.get_Point(I);
                if (rectIntersect.Intersects(pt))
                {
                    if (PreviousPointIncluded == false)
                    {
                        // Add a segment back to the point
                        LineSegments1.Add(new LineSegment(Shape1.get_Point(I - 1), pt));
                    }
                    if (I == numPoints1 - 1)
                    {
                        LineSegments1.Add(new LineSegment(pt, Shape1.get_Point(0)));
                    }
                    else
                    {
                        LineSegments1.Add(new LineSegment(pt, Shape1.get_Point(I + 1)));
                    }
                    PreviousPointIncluded = true;
                }
                else
                {
                    PreviousPointIncluded = false;
                }
            }

            int numPoints2 = Shape2.numPoints;
            PreviousPointIncluded = true;

            for (int I = 0; I < numPoints2; I++)
            {
                pt = Shape2.get_Point(I);
                if (rectIntersect.Intersects(pt))
                {
                    if (PreviousPointIncluded == false)
                    {
                        // Add a segment back to the point
                        LineSegments2.Add(new LineSegment(Shape2.get_Point(I - 1), pt));
                    }
                    if (I == numPoints2 - 1)
                    {
                        LineSegments2.Add(new LineSegment(pt, Shape2.get_Point(0)));
                    }
                    else
                    {
                        LineSegments2.Add(new LineSegment(pt, Shape2.get_Point(I + 1)));
                    }
                    PreviousPointIncluded = true;
                }
                else
                {
                    PreviousPointIncluded = false;
                }
            }
        }

        #endregion

        #region ------------------------- EXTENTS INTERSECT ----------------------------------
        /// <summary>
        /// Determines if two shapes in the same shapefile have rectangular extents that touch or intersect.
        /// </summary>
        /// <param name="mwShapefile">A MapWinGIS.Shapefile containing both shapes to compare</param>
        /// <param name="ShapeIndex1">The integer index of the first shape</param>
        /// <param name="ShapeIndex2">The integer index of the second shape</param>
        /// <returns>Boolean, true if the extents overlap or touch</returns>
        public static bool EnvelopeIntersect(MapWinGIS.Shapefile mwShapefile, int ShapeIndex1, int ShapeIndex2)
        {
            MapWinGIS.Extents ext1;
            MapWinGIS.Extents ext2;
            ext1 = mwShapefile.QuickExtents(ShapeIndex1);
            ext2 = mwShapefile.QuickExtents(ShapeIndex2);
            if (ext1.xMin >= ext2.xMax) return false;
            if (ext1.yMin >= ext2.yMax) return false;
            if (ext2.xMin >= ext1.xMax) return false;
            if (ext2.yMin >= ext1.yMax) return false;
            return true;
        }
        /// <summary>
        /// Determines if two MapWinGIS.Shape objects have rectangular extents that touch or intersect
        /// </summary>
        /// <param name="Shape1">A MapWinGIS.Shape to test the rectangular extents of</param>
        /// <param name="Shape2">A Second MapWinGIS.Shape to test the rectangular extents of</param>
        /// <returns>Boolean, true if the extents overlap or touch.</returns>
        public static bool EnvelopeIntersect(MapWinGIS.Shape Shape1, MapWinGIS.Shape Shape2)
        {
            MapWinGIS.Extents ext1 = Shape1.Extents;
            MapWinGIS.Extents ext2 = Shape2.Extents;
            if (ext1.xMin >= ext2.xMax) return false;
            if (ext1.yMin >= ext2.yMax) return false;
            if (ext2.xMin >= ext1.xMax) return false;
            if (ext2.yMin >= ext1.yMax) return false;
            return true;
        }

        #endregion

        #region ------------------------- SHAPES WITH INTERSECTING EXTENTS -------------------
        /// <summary>
        /// Returns a System.Collections.List if integer shape indecies
        /// </summary>
        /// <param name="mwShapefile">The MapWinGIS.Shapefile to find shapes from</param>
        /// <param name="ShapeIndex">The integer shape index in shapefile of the shape to compare with</param>
        /// <returns>A System.Collections.List of integers that are the shape indecies of shapes overlapping extents</returns>
        public static List<int> ShapesWithIntersectingEnvelope(MapWinGIS.Shapefile mwShapefile, int ShapeIndex)
        {
            List<int> Shapelist = new List<int>();
            MapWinGIS.Extents ext1, ext2;
            ext1 = mwShapefile.QuickExtents(ShapeIndex);
            
            for (int shp = 0; shp < mwShapefile.NumShapes; shp++)
            {
                if (shp == ShapeIndex) continue;
                ext2 = mwShapefile.QuickExtents(shp);
                if (ext1.xMin >= ext2.xMax) continue;
                if (ext1.yMin >= ext2.yMax) continue;
                if (ext2.xMin >= ext1.xMax) continue;
                if (ext2.yMin >= ext1.yMax) continue;
                Shapelist.Add(shp);
            }
            return Shapelist;
        }
        /// <summary>
        /// Returns a System.Collections.List if integer shape indecies
        /// </summary>
        /// <param name="mwShapefile">The MapWinGIS.Shapefile to find shapes from</param>
        /// <param name="Shape">The MapWinGIS.Shape to compare with (not necessarilly in the same shapefile)</param>
        /// <returns>A System.Collections.List of integers that are the shape indecies of shapes overlapping extents</returns>
        /// <remarks>Even if Shape is in the shapefile, its index will also be returned</remarks>
        public static List<int> ShapesWithIntersectingEnvelope(MapWinGIS.Shapefile mwShapefile, MapWinGIS.Shape Shape)
        {
            List<int> Shapelist = new List<int>();
            MapWinGIS.Extents ext1, ext2;
            ext1 = Shape.Extents;

            for (int shp = 0; shp < mwShapefile.NumShapes; shp++)
            {
                ext2 = mwShapefile.QuickExtents(shp);
                if (ext1.xMin >= ext2.xMax) continue;
                if (ext1.yMin >= ext2.yMax) continue;
                if (ext2.xMin >= ext1.xMax) continue;
                if (ext2.yMin >= ext1.yMax) continue;
                Shapelist.Add(shp);
            }
            return Shapelist;
        }
        #endregion

    }//End Topology
}
