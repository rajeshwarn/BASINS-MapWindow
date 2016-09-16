using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Topology
{
    /// <summary>
    /// A polyline (under construction)
    /// </summary>
    public class PolyLine
    {
        #region variables
        private Extents m_Extents; // A bounding box for our shape
        private bool extentsValid; // Keeps track of whether we need to recalibrate our extents

        private double m_Length; // A cache for the length so we don't recalculate every time
        private bool lengthValid; // Keeps track of whether we already calculated our length

        private double m_MaxRadius; // radius of a sphere completely surrounding the polyline
        private bool maxradiusValid; // keeps track of whether we have one of these cached

        private Point m_Center; // The point in the geometric center of the extents rectangle
        private bool centerValid; // the center might be used a lot, so cache it here.


       
        private List<Point> m_Points;

        #endregion

        #region Constructors

        /// <summary>
        /// initialises a polyline with no points.
        /// </summary>
        public PolyLine()
        {
            m_Extents = new Extents();
            extentsValid = false; // When adding all the points at once, we have no problem.

            m_Length = 0;
            lengthValid = false; // Calculate this only if it is needed

            m_Center = new Point();
            centerValid = false; // since extents were ok already, we can quickly find the center

            m_MaxRadius = 0;
            maxradiusValid = false; // Calculate this only if it is needed 

            m_Points = new List<Point>();
        }
        /// <summary>
        /// Creates a new instance of the polyline class
        /// </summary>
        /// <param name="mwShape">A MapWinGIS.Shape to derive the polyline from</param>
        /// <remarks>Assumes shape is one part.  To Split Multipart shapes, use Split.</remarks>
        public PolyLine(MapWinGIS.Shape mwShape)
        {
            if (Adapter.GetCategory(mwShape) != ShapeCategories.Line)
                throw new ArgumentException("The Split method only takes Polyline shape types.");

            m_Extents = new Extents(mwShape.Extents);
            extentsValid = true; // When adding all the points at once, we have no problem.

            m_Length = 0;
            lengthValid = false; // Calculate this only if it is needed

            m_Center = new Point((m_Extents.xMin + m_Extents.xMax) / 2, (m_Extents.yMin + m_Extents.yMax) / 2, (m_Extents.zMin + m_Extents.zMax) / 2);
            centerValid = true; // since extents were ok already, we can quickly find the center

            m_MaxRadius = 0;
            maxradiusValid = false; // Calculate this only if it is needed 

            m_Points = new List<Point>();
            for (int I = 0; I < mwShape.NumParts; I++)
            {
                Add_Point(mwShape.get_Point(I));
            }
        }
        #endregion

        #region Properties

        #region --------------------------- CENTER -----------------------------------

        /// <summary>
        /// Returns a Point representing the midpoint between all the extents.
        /// This is not the same as the centroid, which is wieghted by the
        /// enclosed area.  Setting this will translate your entire polyline,
        /// so be careful.  The center will automatically correct itself if points are added.
        /// </summary>
        public Point Center
        {
            get
            {
                if (centerValid == false)
                {
                    Extents ext = this.Extents; // checks if extents already exist.  Creates them otherwise.
                    m_Center = new Point((m_Extents.xMin + m_Extents.xMax) / 2, (m_Extents.yMin + m_Extents.yMax) / 2);
                    centerValid = true;
                }
                return m_Center;
            }
            set
            {
                Vector trans = new Vector(Center, value);
                for (int I = 0; I < Points.Count; I++)
                {
                    Points[I] = trans.Plus(Points[I].ToVector()).ToPoint();
                }
                m_Center = value;
                centerValid = true;
            }
        }
        #endregion

        #region --------------------------- EXTENTS ----------------------------------
        /// <summary>
        /// The extents that form a bounding box around the polyline.
        /// </summary>
        public Extents Extents
        {
            get
            {
                if (extentsValid == false)
                {
                    m_Extents.xMax = double.NegativeInfinity;
                    m_Extents.yMax = double.NegativeInfinity;
                    m_Extents.xMin = double.PositiveInfinity;
                    m_Extents.yMin = double.PositiveInfinity;
                    for (int I = 0; I < Points.Count; I++)
                    {
                        if (Points[I].X < m_Extents.xMin) m_Extents.xMin = Points[I].X;
                        if (Points[I].X > m_Extents.xMax) m_Extents.xMax = Points[I].X;
                        if (Points[I].Y < m_Extents.yMin) m_Extents.yMin = Points[I].Y;
                        if (Points[I].Y > m_Extents.yMax) m_Extents.yMax = Points[I].Y;
                    }
                    extentsValid = true;
                }
                return m_Extents;
            }
            set
            {
                m_Extents = value;
            }
        }
        #endregion

        /// <summary>
        /// Gets a double specifying the length along the polyline
        /// </summary>
        public double Length
        {
            get
            {
                if (lengthValid == false)
                {
                    for (int I = 0; I < Points.Count - 1; I++)
                    {
                        m_Length += new Vector(Points[I], Points[I + 1]).Magnitude;
                    }
                    lengthValid = true;
                }
                return m_Length;
            }
        }
        /// <summary>
        /// Gets a double specifying the maximum distance any segment gets from the center
        /// where the center is defined as the middle of the extents.  Since it is not
        /// possible for the segments to be further away than the endpoints, the max radius
        /// must be defined by one of the vertecies.
        /// </summary>
        public double MaxRadius
        {
            get
            {
                if (maxradiusValid == false)
                {
                    m_MaxRadius = 0.0;
                    for (int I = 0; I < Points.Count; I++)
                    {
                        // take advantage of the check against the center
                        Vector dist = new Vector(Points[I], Center);
                        double mag = dist.Magnitude;
                        if (mag > m_MaxRadius)
                        {
                            m_MaxRadius = mag;
                        }
                    }
                    maxradiusValid = true;
                }
                return m_MaxRadius;
            }
        }
        /// <summary>
        /// Gets a double representing the distance of the first point to the last point
        /// </summary>
        public double NetDistance
        {
            get
            {
                double len;
                len = new Vector(Points[0], Points[Points.Count - 1]).Magnitude;
                return len;
            }
        }

        #region --------------------------- POINTS -----------------------------------
        /// <summary>
        /// A list of points within this polygon.  The last point is NOT the same
        /// as the first, so when adding points just add the last one.
        /// </summary>
        public List<Point> Points
        {
            get
            {
                return m_Points;
            }
            set
            {
                m_Points = value;
                Invalidate();
            }

        }

        #endregion


        #endregion

        #region Methods

        #region ----------------------- POINT HANDLING ---------------------------------

        /// <summary>
        /// Many properties are derived from the points.  Generally, these are calculated
        /// once and cached.  Most changes that can be made externally will automatically
        /// cause an invalidation to occur, so adding points, for instance, will force
        /// the perimeter to be recalculated.  This forces such an invalidation so that the
        /// next time derived properties are queried, they will be recalculated.
        /// </summary>
        public void Invalidate()
        {
           
            extentsValid = true;
            centerValid = true;
            lengthValid = false;
            maxradiusValid = false;
        }

        /// <summary>
        /// Adds a point to the list of points and adjusts the extents
        /// Invalidates derived properties so that they will be recalculated when
        /// queried.
        /// </summary>
        /// <param name="newPoint">A Point to add to the Polygon</param>
        public void Add_Point(Point newPoint)
        {
            m_Points.Add(newPoint);
            Invalidate();
        }
        /// <summary>
        /// Adds a MapWinGIS.Point to the list of points and adjusts the extents
        /// Invalidates derived properties so that they will be recalculated when
        /// queried.
        /// </summary>
        /// <param name="mwPoint">A MapWinGIS.Point to append to the list</param>
        public void Add_Point(MapWinGIS.Point mwPoint)
        {
            Point newPoint = new Point(mwPoint);
            Add_Point(newPoint);
        }
        /// <summary>
        /// Inserts a point in the list.  Remember that this list does not need to
        /// end with the first point.  Invalidates derived properties so that they
        /// will be recalculated when queried.
        /// </summary>
        /// <param name="Index">The index specifying where to insert the point</param>
        /// <param name="newPoint">The Point you want to insert into this polyline</param>
        public void Insert_Point(int Index, Point newPoint)
        {
            m_Points.Insert(Index, newPoint);
            Invalidate();
        }
        /// <summary>
        /// Inserts a point in the list.  Remember that this list does not need to
        /// end with the first point.  Invalidates derived properties so that they
        /// will be recalculated when queried.
        /// </summary>
        /// <param name="Index">The index specifying where to insert the point</param>
        /// <param name="mwPoint">The MapWinGIS.Point you want to insert into this Polygon</param>
        public void Insert_Point(int Index, MapWinGIS.Point mwPoint)
        {
            Point newPoint = new Point(mwPoint);
            m_Points.Insert(Index, newPoint);
            Invalidate();
        }
        /// <summary>
        /// Appends the the givin line segment to the end of the polyline.
        /// We assume that if the first point in the segment is the same as the
        /// endpoint of the polygon, we don't need to add the first point.
        /// </summary>
        /// <param name="newSegment">Segment, the line segment to append</param>
        public void Add_Segment(Segment newSegment)
        {
            if (newSegment.StartPoint.IsIdenticalTo(m_Points[m_Points.Count - 1]) == false)
            {
                Add_Point(newSegment.StartPoint);
            }
            Add_Point(newSegment.EndPoint);
        }
        /// <summary>
        /// Inserts a segment into a polyline.
        /// This will check the points of the segment and only add the points if they are 
        /// not identical to what will become the adjacent points.
        /// </summary>
        /// <param name="Index">the position to add the segment in the list of points</param>
        /// <param name="newSegment">The Segment to insert</param>
        public void Insert_Segment(int Index, Segment newSegment)
        {
            if (Index >= Points.Count)
            {
                Add_Segment(newSegment);
            }
            if (newSegment.StartPoint.IsIdenticalTo(m_Points[Index - 1]) == false)
            {
                Insert_Point(Index, newSegment.StartPoint);
            }
            // check the second point against the point above this
            Index++; // we added a point so the index location is increased
            if (newSegment.StartPoint.IsIdenticalTo(m_Points[Index]) == false)
            {
                Insert_Point(Index, newSegment.EndPoint);
            }
        }

#endregion

        #region ----------------------- TO SEGMENTS ------------------------------------

        /// <summary>
        /// Will create a list of segment objects for the entire polygon
        /// </summary>
        /// <returns>A List of Segments representing the entire polygon</returns>
        public List<Segment> ToSegments()
        {
            List<Segment> SegList = new List<Segment>();
            for (int iPoint = 0; iPoint < Points.Count - 1; iPoint++)
            {
                SegList.Add(new Segment(Points[iPoint], Points[iPoint + 1]));
            }
            return SegList;
        }

        /// <summary>
        /// Returns a list of segments from the Polyline that are within the
        /// submitted extents.
        /// </summary>
        /// <param name="Ext">Extents to check for an intersection with.</param>
        /// <returns>Returns a list of possible Extents within range.</returns>
        public List<Segment> SegmentsWithin(Extents Ext)
        {
            List<Segment> SegList = new List<Segment>();
            Segment seg;
            for (int iPoint = 0; iPoint < Points.Count - 1; iPoint++)
            {
                seg = new Segment(Points[iPoint], Points[iPoint + 1]);
                if (Ext.IntersectWith(seg)) SegList.Add(seg);
            }
            return SegList;
        }

        #endregion


        #region Static Methods

        /// <summary>
        /// If a Shape has multiple parts, this will create a separate polyline for each part.
        /// </summary>
        /// <param name="mwShape">A MapWinGIS.Shape that should be a PolyLine shape type</param>
        /// <returns>A List of Polylines derived from the various shapes</returns>
        public static List<PolyLine> mwShape_To_PolyLines(MapWinGIS.Shape mwShape)
        {
            if (Adapter.GetCategory(mwShape) != ShapeCategories.Line)
                throw new ArgumentException("The Split method only takes Polyline shape types.");
            List<PolyLine> newLines = new List<PolyLine>();
            if (mwShape.NumParts <= 1)
            {
                PolyLine Part = new PolyLine();
                for (int I = 0; I < mwShape.numPoints; I++)
                {
                    Part.Add_Point(mwShape.get_Point(I));
                }
                newLines.Add(Part);
                return newLines;
            }
            int PartIndex = 0;
            for (int P = 0; P < mwShape.NumParts; P++)
            {
                PolyLine Part = new PolyLine();
                int Pnext = mwShape.get_Part(P);
                for (int I = PartIndex; I < Pnext; I++)
                {
                    Part.Add_Point(mwShape.get_Point(I));
                }
                newLines.Add(Part);
            }
            return newLines;
        }
        /// <summary>
        /// Converts a list of polylines into a single multi-part shape
        /// </summary>
        /// <param name="Polylines"></param>
        /// <returns></returns>
        public static MapWinGIS.Shape PolyLines_To_mwShape(List<PolyLine> Polylines)
        {
            MapWinGIS.Shape mwShape = new MapWinGIS.Shape();
            bool res;
            res = mwShape.Create(MapWinGIS.ShpfileType.SHP_POLYLINEZ);
            if (res != true) throw new ApplicationException(mwShape.get_ErrorMsg(mwShape.LastErrorCode));
            int idx = 0;
            for (int iPart = 0; iPart < Polylines.Count; iPart++)
            {
                PolyLine Part = Polylines[iPart];
                // we only cal set_part if we have multiple polylines
                if (Polylines.Count > 0) mwShape.set_Part(iPart, idx);
                for (int iPoint = 0; iPoint < Part.Points.Count; iPoint++)
                {
                    mwShape.InsertPoint(Part.Points[iPoint].To_mwPoint(), ref idx);
                    idx++;
                }
            }
            return mwShape;
        } // End PolyLines_To_mwShape

        /// <summary>
        /// Creates a list of segments from this polyline that are within the specified distance to a point
        /// </summary>
        /// <param name="Point">The point that represents the test location</param>
        /// <param name="Distance">Double, the maximum distance for selection</param>
        /// <returns>A List of Segments that are within Distance of the specified Point.</returns>
        public List<Segment> SegmentsWithinDistanceOf(Point Point, double Distance)
        {
            List<Segment> closeSegs = new List<Segment>();
            bool PreviousPointIncluded = true;
            for (int iPoint = 0; iPoint < Points.Count; iPoint++)
            {
                if (PreviousPointIncluded == false)
                {
                    closeSegs.Add(new Segment(Points[iPoint - 1], Points[iPoint]));
                }
                if (iPoint < Points.Count - 1)
                {
                    if (new Vector(Point, Points[iPoint]).Magnitude < Distance)
                    {
                        closeSegs.Add(new Segment(Points[iPoint], Points[iPoint + 1]));
                        PreviousPointIncluded = true;
                    }
                    else
                    {
                        PreviousPointIncluded = false;
                    }
                }
            }
            return closeSegs;
        }



        #endregion


        #endregion
    }
}