using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Topology2D
{
    /// <summary>
    /// This represents the root Geometry class for 2D space
    /// </summary>
    public abstract class Geometry
    {
        #region Variables
        
        Envelope m_Envelope;
        string m_GeometryType;
        int m_SRID;
        bool m_IsEmpty;
        bool m_IsSimple;

        #endregion

        #region Contructor
        /// <summary>
        /// Creates a new instance of the geometry class
        /// </summary>
        public Geometry()
        {
            m_IsEmpty = true;
            m_GeometryType = "Geometry";
            m_SRID = 1;
            m_IsSimple = true;
        }
        #endregion


        #region Basic Methods

        /// <summary>
        /// The inherent dimension of this geometric object, which must be less than or equal to the 
        /// coordinate dimension.
        /// </summary>
        /// <returns>Integer</returns>
        public abstract int Dimension
        {
            get;
        }
       
       
       
        /// <summary>
        /// returns the name of the instantiable subtype of Geometry of which this geometric object is a
        /// instantiable member.  The name of the subtype of Geobetry is returned as a string.
        /// </summary>
        public string GeometryType
        {
            get
            {
                return m_GeometryType;
            }
            set
            {
                m_GeometryType = value;
            }
        }

        /// <summary>
        /// Returns the Spatial Reference System ID for this geometric object
        /// </summary>
        /// <returns></returns>
        public int SRID
        {
            get
            {
                return m_SRID;
            }
            set
            {
                m_SRID = value;
            }
        }
       
        /// <summary>
        /// The minimum bounding box for this geometry, returned as envelope.
        /// </summary>
        /// <returns>Topology2D.Envelope</returns>
        public virtual Envelope Envelope
        {
            get
            {
                return m_Envelope;
            }
            set
            {
                if (m_Envelope == null)
                {
                    m_Envelope = new Envelope();
                }
                m_Envelope.xMax = value.xMax;
                m_Envelope.xMin = value.xMin;
                m_Envelope.yMax = value.yMax;
                m_Envelope.yMin = value.yMin;
            }
            
        }
        /// <summary>
        /// Exports this geometric object to a specific Well-known text representation of the geometry
        /// </summary>
        /// <returns>String, text representation of this geometry</returns>
        public abstract String AsText();
      

        /// <summary>
        /// Exports this geometric object to a specific well-known binary representation of geometry
        /// </summary>
        /// <returns></returns>
        public abstract byte[] AsBinary();
       

        /// <summary>
        /// If this geometric object is the empty geometry, this returns true.  If true, then this
        /// geometric object represents the empty point set 0 for the coordinate space.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return m_IsEmpty;
            }
            set
            {
                m_IsEmpty = value;
            }
        }
        /// <summary>
        /// Returns true if this geometric object has no anomalous geometric points, such as self
        /// intersection or self tangency.  The description of each instantiable geometric class will
        /// include the specific conditions that cause an instance of that class to be classified as
        /// not simple.
        /// </summary>
        /// <returns>Boolean, true as long as the shape is simple.</returns>
        public bool IsSimple
        {
            get
            {
                return m_IsSimple;
            }
            set
            {
                m_IsSimple = value;
            }
        }
        /// <summary>
        /// Returns the closure of the combinatorial boundary of this geometric object.  Because the
        /// result of this function is a closure, and hence topologically closed, the resulting boundary
        /// can be represented using representational geometry primitives.
        /// </summary>
        /// <returns></returns>
        public abstract Geometry Boundary();
       

        #endregion

        #region  Methods for testing spatial relations between geometric objects

        /// <summary>
        /// Returns true if theis geometric object is spatially equal to another geometry
        /// </summary>
        /// <param name="geom">The specific case to test against this object.</param>
        /// <returns>Boolean, true if the two geometries are equal</returns>
        public abstract bool Equals(Geometry geom);
        

        /// <summary>
        /// Returns true if this geometric object is spatially disjoint from another geometry.
        /// </summary>
        /// <param name="geom">The specific case to test against this object.</param>
        /// <returns>Boolean, true if the two geometries are disjoint</returns>
        public abstract bool Disjoint(Geometry geom);
       

        /// <summary>
        /// Returns true if this geometric object intersects the specified object spatially.
        /// </summary>
        /// <param name="geom">The specific instance to test against this object.</param>
        /// <returns>Boolean, true if the two geometries intersect</returns>
        public abstract bool Intersects(Geometry geom);

        /// <summary>
        /// Returns true if this geometric object spatially touches the specified object.
        /// </summary>
        /// <param name="geom">The specific instance to test against this object.</param>
        /// <returns>Boolean, true if the two geometries touch</returns>
        public abstract bool Touches(Geometry geom);

        /// <summary>
        /// Returns true if this geometric object spatially crosses the specified object.
        /// </summary>
        /// <param name="geom">The specific instance to test against this object.</param>
        /// <returns>Boolean, true if the two geometries cross</returns>
        public abstract bool Crosses(Geometry geom);

        /// <summary>
        /// Returns true if this geometric object is spatially within the specified object.
        /// </summary>
        /// <param name="geom">The specific instance to test against this object.</param>
        /// <returns>Boolean, true if this geometry is within the specified geometry</returns>
        public abstract bool Within(Geometry geom);

        /// <summary>
        /// Returns true if this geometric object is contained by the specified object.
        /// </summary>
        /// <param name="geom">The specific instance to test against this object.</param>
        /// <returns>Boolean, true if the specified geometry contains this geometry</returns>
        public abstract bool Contains(Geometry geom);

        /// <summary>
        /// Returns true if this geometric object spatially crosses the specific object.
        /// </summary>
        /// <param name="geom">The specific instance to test against this object.</param>
        /// <returns>Boolean, true if the two geometries cross</returns>
        public abstract bool Overlaps(Geometry geom);

        /// <summary>
        /// Returns true if this geometric object is spatially related to the specified geometry
        /// by testing for intersections between the interior, boundary and exterior of the two
        /// geometric objects as specified by the values in the intersectionPatternMatrix
        /// </summary>
        /// <param name="geom">The specific instance to test against this object.</param>
        /// <param name="IntersectionPatternMatrix">The IntersectionPatternMatrix</param>
        /// <returns>Boolean, true if the two geometries cross</returns>
        public abstract bool Relate(Geometry geom, string IntersectionPatternMatrix);

        /// <summary>
        /// Returns the shortest distance between any two Points in the two geometric objects as 
        /// calculated in the spatial reference system of this geometric object.
        /// {Note, these points may be interpolated, rather than existing as a vertex}
        /// </summary>
        /// <param name="geom">The specific instance to test against this object.</param>
        /// <returns>Double, the distance between two points.</returns>
        public abstract double Distance(Geometry geom);
        

        /// <summary>
        /// Returns a geometric object that represents all Points whose distance from
        /// this geometrric object is less than or equal to distance.  Calculations are in the
        /// spatial reference system of this geometric object.
        /// </summary>
        /// <param name="Distance">The double value representing the buffer distance to use.</param>
        /// <returns><c>Topology2D.Geometry</c></returns>
        public abstract Geometry Buffer(double Distance);

        /// <summary>
        /// returns a geometric object that represents the convex hull of this geometric object
        /// </summary>
        /// <returns><c>Topology2D.Geometry</c></returns>
        public abstract Geometry ConvexHull();

        /// <summary>
        /// Returns a geometric object that represetns the intersection of this geometric object
        /// with the specified geometric object.
        /// </summary>
        /// <param name="geom">The geometric object to intersect with this geometric object</param>
        /// <returns><c>Geometry</c> representing the intersection</returns>
        public abstract Geometry Intersection(Geometry geom);

        /// <summary>
        /// Returns a geometric object that represents the union of this geometric object with 
        /// the specified geometry.
        /// </summary>
        /// <param name="geom">The specific geometry to form a union with this object</param>
        /// <returns><c>Geometry</c> representing the union of this object with the geom</returns>
        public abstract Geometry Union(Geometry geom);

        /// <summary>
        /// Returns a geometric object that represetns the difference between this object and 
        /// the specified geometry.
        /// </summary>
        /// <param name="geom">A geometric object to compare with this object.</param>
        /// <returns><c>Topology2D.Geometry</c> representing the difference.</returns>
        public abstract Geometry Difference(Geometry geom);

        /// <summary>
        /// Returns a geometric ojbect that represents the point set symetric difference of this geometric
        /// object with another geometry.
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public abstract Geometry SymDifference(Geometry geom);

        /// <summary>
        /// Returns null if the two geometries intersect.  Otherwise, returns a segment representing the
        /// shortest path between the two geometries.
        /// </summary>
        /// <param name="geom">The geometries to test</param>
        /// <returns>LineSegment or null in the case of intersection</returns>
        public abstract LineSegment ShortestPathTo(Geometry geom);

        #endregion

    }
}
