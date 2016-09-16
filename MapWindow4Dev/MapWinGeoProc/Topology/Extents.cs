using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Topology
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
        /// <summary>
        /// Double, the smallest Z value
        /// </summary>
        public double zMin;
        /// <summary>
        /// Double, the largest Z value
        /// </summary>
        public double zMax;

        

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
            zMin = 0;
            zMax = 0;
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
            zMax = 0;
            zMin = 0;
        }
        /// <summary>
        /// Creates a new instance of Extents from double values
        /// </summary>
        /// <param name="MinX">Double, The minimum X value</param>
        /// <param name="MaxX">Double, The largest X value</param>
        /// <param name="MinY">Double, The minimum Y value</param>
        /// <param name="MaxY">Double, the largest Y value</param>
        /// <param name="MinZ">Double, the minimum Z value</param>
        /// <param name="MaxZ">Double, the largest Z value</param>
        public Extents(double MinX, double MaxX, double MinY, double MaxY, double MinZ, double MaxZ)
        {
            xMin = MinX;
            yMin = MinY;
            xMax = MaxX;
            yMax = MaxY;
            zMin = MinZ;
            zMax = MaxZ;
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
            zMin = Bounds.zMin;
            zMax = Bounds.zMax;
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
        /// Gets or sets the depth.  Setting this will keep the zMin the same and adjust zMax.
        /// </summary>
        public double Depth
        {
            get
            {
                return zMax - zMin;
            }
            set
            {
                if (value < 0) throw new ArgumentException("Depth cannot be negative.");
                zMax = yMin + value;
            }
        }
        /// <summary>
        /// Gets or sets the center.  Setting this will translate the entire box.
        /// </summary>
        public Point Center
        {
            get
            {
                return new Point((xMax + xMin) / 2, (yMax + yMin / 2), (zMax + zMin) / 2);
            }
            set
            {
                Vector trans = new Vector(Center, value);
                xMin = xMin + trans.X;
                yMin = yMin + trans.Y;
                zMin = yMin + trans.Z;
                xMax = xMax + trans.X;
                yMax = yMax + trans.Y;
                zMax = zMax + trans.Z;
            }
        }
        #endregion

        #region Methods

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
            if (Point.Z < zMin || Point.Z > zMax) return false;
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
            if (Point.Z < zMin || Point.Z > zMax) return false;
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
            if (Seg.Z1 > zMax && Seg.Z2 > zMax) return false;
            if (Seg.X1 < xMin && Seg.X2 < xMin) return false;
            if (Seg.Y1 < yMin && Seg.X2 < yMin) return false;
            if (Seg.Z1 < zMin && Seg.Z2 < zMin) return false;

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
                if (TestPoint.Y >= yMin && TestPoint.Y <= yMax && TestPoint.Z >= zMin && TestPoint.Z <= zMax) return true;
            }
            // Test if segment contacts bounds at Xmin
            if (Seg.LocationByX(xMin, out TestPoint) == true)
            {
                // we obtained a valid point where X = Xmin, test if it is in bounds
                if (TestPoint.Y >= yMin && TestPoint.Y <= yMax && TestPoint.Z >= zMin && TestPoint.Z <= zMax) return true;
            }
            // Test if segment contacts bounds at Ymax
            if (Seg.LocationByY(yMax, out TestPoint) == true)
            {
                // we obtained a valid point where Y = Ymax, test if it is in bounds
                if (TestPoint.X >= xMin && TestPoint.X <= xMax && TestPoint.Z >= zMin && TestPoint.Z <= zMax) return true;
            }
            // Test if segment contacts bounds at Ymin
            if (Seg.LocationByY(yMin, out TestPoint) == true)
            {
                // we obtained a valid point where Y = Ymin, test if it is in bounds
                if (TestPoint.X >= xMin && TestPoint.X <= xMax && TestPoint.Z >= zMin && TestPoint.Z <= zMax) return true;
            }
            // Test if segment contacts bounds at Zmax
            if (Seg.LocationByZ(zMax, out TestPoint) == true)
            {
                // we obtained a valid point where Z = Zmax, test if it is in bounds
                if (TestPoint.Y >= yMin && TestPoint.Y <= yMax && TestPoint.X >= xMin && TestPoint.X <= xMax) return true;
            }
            // Test if segment contacts bounds at Zmin
            if (Seg.LocationByZ(zMin, out TestPoint) == true)
            {
                // we obtained a valid point where Z = Zmin, test if it is in bounds
                if (TestPoint.Y >= yMin && TestPoint.Y <= yMax && TestPoint.X >= xMin && TestPoint.X <= xMax) return true;
            }

            // Since our segment did not intersect with any of the sides, we can conclude that
            // it doesn't intersect anywhere with our bounds.
            return false;
        }
        #endregion


        /// <summary>
        /// Returns a RectD that has the intersection area for both rectangles
        /// </summary>
        /// <param name="Rectangle2">A Extents that you wish to intersect with this rectangle</param>
        /// <returns>The Extents of the intersection area</returns>
        public Extents Intersection(Extents Rectangle2)
        {
            Extents intersect = new Extents();
            // check to see if we have an intersection first
            if (xMin > Rectangle2.xMax) return null;
            if (Rectangle2.xMin > xMax) return null;
            if (yMin > Rectangle2.yMax) return null;
            if (Rectangle2.yMin > yMax) return null;
            if (zMin > Rectangle2.zMax) return null;
            if (Rectangle2.zMin > zMax) return null;

            // We intersect, so go ahead and find the values
            intersect.xMax = Math.Min(xMax, Rectangle2.xMax);
            intersect.xMin = Math.Max(xMin, Rectangle2.xMin);
            intersect.yMax = Math.Min(yMax, Rectangle2.yMax);
            intersect.yMin = Math.Max(yMin, Rectangle2.yMin);
            intersect.zMax = Math.Min(zMax, Rectangle2.zMax);
            intersect.zMin = Math.Max(zMin, Rectangle2.zMin);
            return intersect;
        }
        /// <summary>
        /// Checks each value in the point to see if they are the same
        /// </summary>
        /// <param name="Ext">The Extents to compare with this point</param>
        /// <returns>Boolean, true if the points are the same</returns>
        public bool AreIdenticalTo(Extents Ext)
        {
            if ((xMin == Ext.xMin) && (yMin == Ext.yMin) && (zMin == Ext.zMin)
             && (xMax == Ext.xMax) && (yMax == Ext.yMax) && (zMax == Ext.zMax)) return true;
            return false;
        }
        /// <summary>
        /// Checks each value in the point to see if they are the same
        /// </summary>
        /// <param name="Ext">The Extents to compare with this point</param>
        /// <returns>Boolean, true if the points are the same</returns>
        public bool AreIdenticalTo(MapWinGIS.Extents Ext)
        {
            if ((xMin == Ext.xMin) && (yMin == Ext.yMin) && (zMin == Ext.zMin)
             && (xMax == Ext.xMax) && (yMax == Ext.yMax) && (zMax == Ext.zMax)) return true;
            return false;
        }
       
        #region ------------------------ Conversion To -------------------------
        /// <summary>
        /// Converts this rectangleD to a MapWinGIS.Extents object
        /// </summary>
        /// <returns>A MapWinGIS.Extents object with the same measurements as Extents</returns>
        public MapWinGIS.Extents ToMapWinExtents()
        {
            MapWinGIS.Extents mwExtents = new MapWinGIS.Extents();
            mwExtents.SetBounds(xMin, yMin, zMin, xMax, yMax, zMax);
            return mwExtents;
        }
        /// <summary>
        /// Creates a list of segments that define the boundaries of the extents.
        /// </summary>
        /// <returns>List of Segments</returns>
        public List<Segment> ToSegments()
        {
            List<Segment> SegList = new List<Segment>();
            // Xmin face
            SegList.Add(new Segment(xMin, yMin, zMin, xMin, yMin, zMax));
            SegList.Add(new Segment(xMin, yMax, zMin, xMin, yMax, zMax));
            SegList.Add(new Segment(xMin, yMin, zMin, xMin, yMax, zMin));
            SegList.Add(new Segment(xMin, yMin, zMax, xMin, yMax, zMax));

            // Xmax face
            SegList.Add(new Segment(xMax, yMin, zMin, xMin, yMin, zMax));
            SegList.Add(new Segment(xMax, yMax, zMin, xMin, yMax, zMax));
            SegList.Add(new Segment(xMax, yMin, zMin, xMin, yMax, zMin));
            SegList.Add(new Segment(xMax, yMin, zMax, xMin, yMax, zMax));

            // Connect xMin to Xmax
            SegList.Add(new Segment(xMin, yMin, zMin, xMax, yMin, zMin));
            SegList.Add(new Segment(xMin, yMax, zMin, xMax, yMax, zMin));
            SegList.Add(new Segment(xMin, yMin, zMax, xMax, yMin, zMax));
            SegList.Add(new Segment(xMin, yMax, zMax, xMax, yMax, zMax));
            return SegList;
        }
        #endregion

        #endregion
    }
}