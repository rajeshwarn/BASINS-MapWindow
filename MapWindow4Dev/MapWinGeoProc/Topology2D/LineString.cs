using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Topology2D
{
    /// <summary>
    /// This represents a two dimensional collection of points that represent connected segments.
    /// </summary>
    public class LineString : Geometry
    {
        #region variables
        /// <summary>
        /// List of points.  For normal use, please use Add_Point or Insert_Point to correctly
        /// handle extents and prevent duplicate entries
        /// </summary>
        private List<Coordinate> m_Coordinates;

        private bool extentsValid; // Keeps track of whether we need to recalibrate our extents

        private double m_Length; // A cache for the length so we don't recalculate every time
        private bool lengthValid; // Keeps track of whether we already calculated our length

        private double m_MaxRadius; // radius of a sphere completely surrounding the polyline
        private bool maxradiusValid; // keeps track of whether we have one of these cached

        private Point m_Center; // The point in the geometric center of the extents rectangle
        private bool centerValid; // the center might be used a lot, so cache it here.

        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new, empty instance of the Polyline class.
        /// </summary>
        public LineString()
        {
            base.Envelope = new Envelope();
            extentsValid = false; // When adding all the points at once, we have no problem.

            m_Length = 0;
            lengthValid = false; // Calculate this only if it is needed

            m_Center = new Point();
            centerValid = false; // since extents were ok already, we can quickly find the center

            m_MaxRadius = 0;
            maxradiusValid = false; // Calculate this only if it is needed

            m_Coordinates = new List<Coordinate>();
           
        }
        /// <summary>
        /// This will use the factory in order to create a linestring from a shape
        /// </summary>
        /// <param name="MapWinGIS_Shape">A MapWinGIS.Shape</param>
        public LineString(object MapWinGIS_Shape)
        {
            Coordinates = GeometryFactory.CreateLineString(MapWinGIS_Shape).Coordinates;
        }
        #endregion

        #region Properties

        #region ------------------------ CENTER -----------------------------------

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
                    Envelope ext = this.Envelope; // checks if extents already exist.  Creates them otherwise.
                    m_Center = new Point((base.Envelope.xMin + base.Envelope.xMax) / 2, (base.Envelope.yMin + base.Envelope.yMax) / 2);
                    centerValid = true;
                }
                return m_Center;
            }
            set
            {
                Vector trans = new Vector(Center, value);
                for (int I = 0; I < Coordinates.Count; I++)
                {
                    Coordinates[I] = trans.Plus(Coordinates[I].ToVector()).ToCoordinate();
                }
                m_Center = value;
                centerValid = true;
            }
        }
        #endregion


        /// <summary>
        /// The extents that form a bounding box around the polyline.
        /// </summary>
        public override Envelope Envelope
        {
            get
            {
                if (extentsValid == false)
                {
                    base.Envelope.xMax = double.NegativeInfinity;
                    base.Envelope.yMax = double.NegativeInfinity;
                    base.Envelope.xMin = double.PositiveInfinity;
                    base.Envelope.yMin = double.PositiveInfinity;
                    for (int I = 0; I < m_Coordinates.Count; I++)
                    {
                        if (m_Coordinates[I].X < base.Envelope.xMin) base.Envelope.xMin = m_Coordinates[I].X;
                        if (m_Coordinates[I].X > base.Envelope.xMax) base.Envelope.xMax = m_Coordinates[I].X;
                        if (m_Coordinates[I].Y < base.Envelope.yMin) base.Envelope.yMin = m_Coordinates[I].Y;
                        if (m_Coordinates[I].Y > base.Envelope.yMax) base.Envelope.yMax = m_Coordinates[I].Y;
                    }
                    extentsValid = true;
                }
                return base.Envelope;
            }
            set
            {
                base.Envelope = value;
            }
        }
        /// <summary>
        /// Gets a double specifying the length along the polyline
        /// </summary>
        public double Length
        {
            get
            {
                if (lengthValid == false)
                {
                    for (int I = 0; I < m_Coordinates.Count - 1; I++)
                    {
                        m_Length += new Vector(m_Coordinates[I], m_Coordinates[I + 1]).Magnitude;
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
                    for (int I = 0; I < m_Coordinates.Count; I++)
                    {
                        // take advantage of the check against the center
                        Vector dist = new Vector(m_Coordinates[I], Center.ToCoordinate());
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
                len = new Vector(m_Coordinates[0], m_Coordinates[m_Coordinates.Count - 1]).Magnitude;
                return len;
            }
        }
        /// <summary>
        /// Returns the dimension of the geometry.  In this case, LineStrings are 1 dimensional
        /// so this property returns 1.
        /// </summary>
        public override int Dimension
        {
            get { return 1; }
        }


        #region --------------------------- POINTS -----------------------------------
        /// <summary>
        /// A list of points within this polygon.  The last point is NOT the same
        /// as the first, so when adding points just add the last one.
        /// </summary>
        public List<Coordinate> Coordinates
        {
            get
            {
                return m_Coordinates;
            }
            set
            {
                m_Coordinates = value;
                Invalidate();
            }

        }

        #endregion

        #endregion


        #region Geometry Overrides

        #region ------------------------ BOOLEAN RELATIONS ------------------------------
        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override bool Contains(Geometry geom)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override bool Crosses(Geometry geom)
        {
            throw new Exception("The method or operation is not implemented.");
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
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public override bool Intersects(Geometry geom)
        {
            throw new Exception("The method or operation is not implemented.");
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
        /// Not Implemented Yet
        /// </summary>
        /// <param name="Distance">The orthogonal distance of the buffer to this linestring</param>
        /// <returns>A Polygon representing a buffer around this Linestring</returns>
        public override Geometry Buffer(double Distance)
        {
            return null;
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
        /// <returns></returns>
        public override Geometry Intersection(Geometry geom)
        {
            throw new Exception("The method or operation is not implemented.");
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
        /// <param name="geom"></param>
        /// <returns></returns>
        public override LineSegment ShortestPathTo(Geometry geom)
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
        /// Currently only supports point and linestring unions.
        /// </summary>
        /// <param name="geom">A geometry</param>
        /// <returns>A linestring if the ends are connected, or a multilinestring if not</returns>
        public override Geometry Union(Geometry geom)
        {
            if (geom.GetType() == typeof(Point))
            {
                this.Add_Point(geom as Topology2D.Point);
                return this;
            }
            if (geom.GetType() == typeof(LineString))
            {
                LineString LS = geom as LineString;
                if (LS.Coordinates[0] == Coordinates[0])
                {
                    LS.Coordinates.Reverse();
                    List<Coordinate> mergedCoords = new List<Coordinate>();
                    mergedCoords = LS.Coordinates;
                    mergedCoords.AddRange(Coordinates);
                    LineString outString = new LineString();
                    outString.Coordinates = mergedCoords;
                    LS.Coordinates.Reverse();
                    return outString;
                }
                else if (LS.Coordinates[LS.Coordinates.Count - 1] == Coordinates[0])
                {
                    List<Coordinate> mergedCoords = new List<Coordinate>();
                    mergedCoords = LS.Coordinates;
                    mergedCoords.AddRange(Coordinates);
                    LineString outString = new LineString();
                    outString.Coordinates = mergedCoords;
                    return outString;
                }
                else if (LS.Coordinates[0] == Coordinates[Coordinates.Count - 1])
                {
                    List<Coordinate> mergedCoords = new List<Coordinate>();
                    mergedCoords = Coordinates;
                    mergedCoords.AddRange(LS.Coordinates);
                    LineString outString = new LineString();
                    outString.Coordinates = mergedCoords;
                    return outString;
                }
                else if (LS.Coordinates[LS.Coordinates.Count - 1] == Coordinates[Coordinates.Count - 1])
                {
                    Coordinates.Reverse();
                    List<Coordinate> mergedCoords = new List<Coordinate>();
                    mergedCoords = Coordinates;
                    mergedCoords.AddRange(LS.Coordinates);
                    LineString outString = new LineString();
                    outString.Coordinates = mergedCoords;
                    Coordinates.Reverse();
                    return outString;
                }
                // We failed to join the linestrings to create a single linestring
                return null;
            }
            throw new ApplicationException("The specified geometry was not a point or linestring, and isn't supported yet.");
        }

        #endregion



        #region Methods

        #region -------------------- COPY ---------------------------------
        /// <summary>
        /// Returns a new instance of the Polyline class with the same values as this object.
        /// </summary>
        /// <returns>Topology2D.Polyline with identical points.</returns>
        public LineString Copy()
        {
            LineString NewPoly = new LineString();
            for (int I = 0; I < m_Coordinates.Count; I++)
            {
                // Copy each point so that nothing references the same data
                NewPoly.Coordinates.Add(m_Coordinates[I].Copy());
            }
            return NewPoly;
        }
        #endregion

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
            Coordinate c = new Coordinate();
            c.X = newPoint.X;
            c.Y = newPoint.Y;
            m_Coordinates.Add(c);
            Invalidate();
        }
        /// <summary>
        /// Adds a coordinate to the list of coordinates and
        /// invalidates the dependent properties
        /// </summary>
        /// <param name="newCoord">The new Topology.2D coordinate to add</param>
        public void Add_Coordinate(Coordinate newCoord)
        {
            m_Coordinates.Add(newCoord);
            Invalidate();
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
            m_Coordinates.Insert(Index, newPoint.ToCoordinate());
            Invalidate();
        }

        /// <summary>
        /// Inserts a point in the list.  Remember that this list does not need to
        /// end with the first point.  Invalidates derived properties so that they
        /// will be recalculated when queried.
        /// </summary>
        /// <param name="Index">The index specifying where to insert the point</param>
        /// <param name="newCoord">The Point you want to insert into this polyline</param>
        public void Insert_Coordinate(int Index, Coordinate newCoord)
        {
            m_Coordinates.Insert(Index, newCoord);
            Invalidate();
        }
       
        /// <summary>
        /// Appends the the givin line segment to the end of the polyline.
        /// We assume that if the first point in the segment is the same as the
        /// endpoint of the polygon, we don't need to add the first point.
        /// </summary>
        /// <param name="newLineSegment">LineSegment, the line segment to append</param>
        public void Add_LineSegment(LineSegment newLineSegment)
        {
            if (newLineSegment.StartPoint != m_Coordinates[m_Coordinates.Count - 1])
            {
                Add_Point(newLineSegment.StartPoint);
            }
            Add_Point(newLineSegment.EndPoint);
        }
        /// <summary>
        /// Inserts a segment into a polyline.
        /// This will check the points of the segment and only add the points if they are 
        /// not identical to what will become the adjacent points.
        /// </summary>
        /// <param name="Index">the position to add the segment in the list of points</param>
        /// <param name="newLineSegment">The LineSegment to insert</param>
        public void Insert_LineSegment(int Index, LineSegment newLineSegment)
        {
            if (Index >= m_Coordinates.Count)
            {
                Add_LineSegment(newLineSegment);
            }
            if (newLineSegment.StartPoint != m_Coordinates[Index - 1])
            {
                Insert_Point(Index, newLineSegment.StartPoint);
            }
            // check the second point against the point above this
            Index++; // we added a point so the index location is increased
            if (newLineSegment.StartPoint != m_Coordinates[Index])
            {
                Insert_Point(Index, newLineSegment.EndPoint);
            }
        }

        #endregion


        #region ----------------------- TO SEGMENTS ------------------------------------

        /// <summary>
        /// Will create a list of segment objects for the entire polygon
        /// </summary>
        /// <returns>A List of LineSegments representing the entire polygon</returns>
        public List<LineSegment> ToLineSegments()
        {
            List<LineSegment> SegList = new List<LineSegment>();
            for (int iPoint = 0; iPoint < m_Coordinates.Count - 1; iPoint++)
            {
                SegList.Add(new LineSegment(m_Coordinates[iPoint], m_Coordinates[iPoint + 1]));
            }
            SegList.Add(new LineSegment(m_Coordinates[m_Coordinates.Count - 1], m_Coordinates[0]));
            return SegList;
        }

        /// <summary>
        /// Returns a list of segments from the Polyline that are within the
        /// submitted extents.
        /// </summary>
        /// <param name="Ext">Envelope to check for an intersection with.</param>
        /// <returns>Returns a list of possible Envelope within range.</returns>
        public List<LineSegment> LineSegmentsWithin(Envelope Ext)
        {
            List<LineSegment> SegList = new List<LineSegment>();
            LineSegment seg;
            for (int iPoint = 0; iPoint < m_Coordinates.Count - 1; iPoint++)
            {
                seg = new LineSegment(m_Coordinates[iPoint], m_Coordinates[iPoint + 1]);
                if (Ext.Intersects(seg)) SegList.Add(seg);
            }
            return SegList;
        }

        #endregion

        /// <summary>
        /// Returns a Douglas-Peuker simplification of the line string.
        /// http://softsurfer.com/Archive/algorithm_0205/algorithm_0205.htm
        /// D. H. Douglass and T. K. Peucker.  "Algorithms for the reduction of the nuber 
        ///     of points required to represent a line or its caricature."
        ///     The Canadian Cartographer 10(2):112-122 1973
        /// Warning... simplified is not the same as being Simple (Aka non-intersecting)
        /// Simplified in this case just means made up of only characteristic points
        /// </summary>
        /// <returns>Line String (A Simplified version of the linestring</returns>
        public LineString Simplified_DP(float tolerance)
        {
            if (m_Coordinates.Count == 0) return null;
            bool[] Markers;
            List<int> JList = new List<int>();
            List<int> KList = new List<int>();
            Markers = new bool[m_Coordinates.Count];
            // testing the assumption that booleans are false by default
            //for (int i = 0; i < m_Coordinates.Count; i++)
            //{
            //    Markers[i] = false;
            //}
            JList.Add(0);
            Markers[0] = true;
            KList.Add(m_Coordinates.Count-1);
            Markers[m_Coordinates.Count - 1] = true;
            while (JList.Count > 0)
            {
                int I, J, K;
                J = JList[0];
                K = KList[0];
                // Remove the current span
                JList.RemoveAt(0);
                KList.RemoveAt(0);

                I = Furthest_Point(tolerance, J, K);
                if (I != -1)
                {
                    Markers[I] = true;

                    // Add the first span
                    JList.Add(J);
                    KList.Add(I);

                    // add the Second span
                    JList.Add(I);
                    KList.Add(K);
                }
                
            }
            LineString SimpleLine = new LineString();
            for (int pt = 0; pt < m_Coordinates.Count; pt++)
            {
                if (Markers[pt] == true)
                {
                    SimpleLine.Add_Coordinate(m_Coordinates[pt]);
                }
            }
            return SimpleLine;
        }

        // Part of the Douglas-Peuker simplification.  For any stretch between points,
        // it finds the point furthest from the segment created by directly connecting
        // point j and point k.
        private int Furthest_Point(float tol, int j, int k)
        {
            if (j + 1 >= k) return -1; // There is nothing to simplify
            // check for adequate approximation by segment S from v[j] to v[k]
            int maxi = j;          // index of vertex farthest from S
            double maxd2 = 0;         // distance squared of farthest vertex
            double tol2 = tol * tol;  // tolerance squared
            LineSegment S = new LineSegment(this.m_Coordinates[j], this.m_Coordinates[k]);  // segment from v[j] to v[k]

            for (int i = j + 1; i < k; i++)
            {
                // get the square of the shortest distance from each point to segment S
                double dist = S.SqrDistance(m_Coordinates[i].ToPoint());

                // test with current max distance squared
                if (dist <= maxd2)
                    continue;
                // v[i] is a new max vertex
                maxi = i;
                maxd2 = dist;
            }
            if (maxd2 > tol2)        // error is worse than the tolerance
            {
                return maxi;
            }
            // else the approximation is OK, so ignore intermediate vertices
            return -1;
        }

        

        #region Static Methods

        
       

        /// <summary>
        /// Creates a list of segments from this polyline that are within the specified distance to a point
        /// </summary>
        /// <param name="Point">The point that represents the test location</param>
        /// <param name="Distance">Double, the maximum distance for selection</param>
        /// <returns>A List of LineSegments that are within Distance of the specified Point.</returns>
        public List<LineSegment> LineSegmentsWithinDistanceOf(Point Point, double Distance)
        {
            List<LineSegment> closeSegs = new List<LineSegment>();
            bool PreviousPointIncluded = true;
            for (int iPoint = 0; iPoint < m_Coordinates.Count; iPoint++)
            {
                if (PreviousPointIncluded == false)
                {
                    closeSegs.Add(new LineSegment(m_Coordinates[iPoint - 1], m_Coordinates[iPoint]));
                }
                if (iPoint < m_Coordinates.Count - 1)
                {
                    if (new Vector(Point, m_Coordinates[iPoint].ToPoint()).Magnitude < Distance)
                    {
                        closeSegs.Add(new LineSegment(m_Coordinates[iPoint], m_Coordinates[iPoint + 1]));
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
