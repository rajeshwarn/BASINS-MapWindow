using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Topology2D
{
    /// <summary>
    /// A structure representing a single closed polygon.  This would represent a single part
    /// of a polygon shape.  No holes.  Unlike a shapefile, this polygon does not list the
    /// first point again at the end.  (This prevents confusion when adding points to the
    /// list.)
    /// </summary>
    public class Polygon : Geometry
    {
        #region variables
        /// <summary>
        /// List of points.  For normal use, please use Add_Point or Insert_Point to correctly
        /// handle extents and prevent duplicate entries
        /// </summary>
        private LinearRing m_Shell;
        private List<LinearRing> m_Holes;
       
        
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new empty instance of a polygon class.
        /// </summary>
        public Polygon()
        {
            Shell.Envelope = new Envelope();
            Shell.Coordinates = new List<Coordinate>();
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// Returns the dimension of the geometry.  In this case, polygons are 2 dimensional,
        /// so we return a 2.
        /// </summary>
        public override int Dimension
        {
            get { return 2; }
        }
        #endregion

       

        #region --------------------------- ENVELOPE ----------------------------------
        /// <summary>
        /// The extents that form a bounding box around the polyline.
        /// </summary>
        public override Envelope Envelope
        {
            get
            {
                return Shell.Envelope;
            }
            set
            {
                base.Envelope = value;
            }
        }

        #endregion

       
        /// <summary>
        /// A list of points that make up the outer shell of the polygon
        /// </summary>
        public LinearRing Shell
        {
            get
            {
                return m_Shell;
            }
            set
            {
                m_Shell = value;
            }

        }
        /// <summary>
        /// This is a collection of linear rings defining holes in the polygon
        /// </summary>
        public List<LinearRing> Holes
        {
            get
            {
                return m_Holes;
            }
            set
            {
                m_Holes = value;
            }
        }
        /// <summary>
        /// Returns the number of interior linear rings that make up "holes" in the polygon.
        /// </summary>
        public int NumHoles
        {
            get
            {
                return m_Holes.Count;
            }

        }

      



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
        /// Returns the LinearRing that is the shell
        /// </summary>
        /// <returns>LinearRing</returns>
        public override Geometry Boundary()
        {
            return Shell;
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
        public override LineSegment ShortestPathTo(Geometry geom)
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
        /// Returns a new instance of the Polygon class with the same values as this object.
        /// </summary>
        /// <returns>Topology2D.Polygon with identical points.</returns>
        public Polygon Copy()
        {
            Polygon NewPoly = new Polygon();
            if (NumHoles > 0)
            {
                NewPoly.Holes = new List<LinearRing>();
                // TO DO: ADD LINEAR RING COPY
            }
            NewPoly.Shell = new LinearRing();
            return NewPoly;
        }
        #endregion

        #region ----------------------- INTERSECTS WITH --------------------------------
        /// <summary>
        /// Only returns true if the point is in the same plane as the polygon
        /// and the polygon encloses the point.  Touching a node is considered intersecting.
        /// </summary>
        /// <param name="Point"></param>
        /// <returns>Boolean, true if the sum of all the angles = 360</returns>
        /// <remarks>The code for cross-type intersect is in the larger structure</remarks>
        public bool Intersects(Point Point)
        {
            throw new ApplicationException("Not yet implemented");
        }

       
       

        /// <summary>
        /// Returns true if any segment from the polyline touches any segment from the borders of this polygon
        /// or in the case where no segments touch, the first point is intersects with this polygon.
        /// </summary>
        /// <param name="Poly">The Polyline to test.</param>
        /// <returns>Boolean, true if the specified polyline intersects with this polygon.</returns>
        public bool Intersects(LineString Poly)
        {
            throw new Exception("Not implemented yet");
        }

        /// <summary>
        /// Returns true if any of the border segments touch or no segments touch but one point
        /// from either polygon is inside the other polygon.  Makes sure rectangular extents
        /// overlap first.
        /// </summary>
        /// <param name="Poly">A Polygon object to compare to this polygon.</param>
        /// <returns>Boolean, true if the the polygons intersect.</returns>
        public bool Intersects(Polygon Poly)
        {
            throw new Exception("Not implemented yet");
        }

        #endregion

        #region ----------------------- WITHIN A DISTANCE OF ---------------------------



        #endregion

        #region ----------------------- TOUCHES THE BOUNDARY OF ------------------------

        /// <summary>
        /// Tests to see if the distance between any segments is 0.  Will test extents
        /// first, since if the extents don't overlap, then neither do any segments.
        /// 
        /// </summary>
        /// <param name="Ext">The extents to compare with </param>
        /// <returns>True if the borders of this polygon touch any of the four segments that
        /// define the outer boundaries of the specified extents object Ext.</returns>
        public bool TouchesTheBoundaryOf(Envelope Ext)
        {
            // If the extents don't overlap, we can't touch
            if (Ext.Intersects(Envelope) == false) return false;

            List<LineSegment> SegList = Ext.ToLineSegments();
            for (int I = 0; I < SegList.Count; I++)
            {
                List<LineSegment> mySegs = Shell.LineSegmentsWithin(Ext);
                for (int J = 0; J < mySegs.Count; J++)
                {
                    if (SegList[I].Touches(mySegs[J])) return true;
                }
            }
            // No segments touch, so the boundaries don't touch.
            return false;
        }
        /// <summary>
        /// Tests to see if the extents overlap.  If they do, it will check
        /// the segments from the overlapping region for an intersection.
        /// </summary>
        /// <param name="Poly">The polygon to test against this polygon.</param>
        /// <returns>Boolean, true if there are two segments that touch.</returns>
        public bool TouchesTheBoundaryOf(Polygon Poly)
        {
            // If the extents don't overlap, we can't touch
            Envelope AOI = (Envelope)Poly.Envelope.Intersection(Envelope);
            if (AOI == null) return false;

            List<LineSegment> SegList = Poly.Shell.LineSegmentsWithin(AOI);
            
            for (int I = 0; I < SegList.Count; I++)
            {
                List<LineSegment> mySegs = Shell.LineSegmentsWithin(AOI);
                for (int J = 0; J < mySegs.Count; J++)
                {
                    if (SegList[I].Touches(mySegs[J])) return true;
                }
            }
            // No segments touch, so the boundaries don't touch.
            return false;
        }


        #endregion

        #region ----------------------- CONTAINS ---------------------------------------
        #endregion

        #region ----------------------- TO SEGMENTS ------------------------------------

        
        #endregion

        #region Static Methods

       
        #endregion


        #endregion
    }
}
