using System;
using System.Collections;
using System.Text;
using MapWinGeoProc.NTS.Topology.Operation;
using MapWinGeoProc.NTS.Topology.GeometriesGraph;


using MapWindow.Interfaces.Geometries;

namespace MapWinGeoProc.NTS.Topology.Geometries
{
    /// <summary>
    /// Basic implementation of <c>MultiLineString</c>.
    /// </summary>    
    public class MultiLineString : GeometryCollection, IMultiLineString
    {
        /// <summary>
        /// Represents an empty <c>MultiLineString</c>.
        /// </summary>
        public static new readonly IMultiLineString Empty = new GeometryFactory().CreateMultiLineString(null);

        /// <summary>
        /// Constructs a <c>MultiLineString</c>.
        /// </summary>
        /// <param name="lineStrings">
        /// The <c>LineString</c>s for this <c>MultiLineString</c>,
        /// or <c>null</c> or an empty array to create the empty
        /// point. Elements may be empty <c>LineString</c>s,
        /// but not <c>null</c>s.
        /// </param>
        /// <param name="factory"></param>
        public MultiLineString(ILineStringBase[] lineStrings, IGeometryFactory factory) : base(lineStrings, factory) { }        

        /// <summary>
        /// Constructs a <c>MultiLineString</c>.
        /// </summary>
        /// <param name="lineStrings">
        /// The <c>LineString</c>s for this <c>MultiLineString</c>,
        /// or <c>null</c> or an empty array to create the empty
        /// point. Elements may be empty <c>LineString</c>s,
        /// but not <c>null</c>s.
        /// </param>
        /// <remarks>
        /// For create this <see cref="Geometry"/> is used a standard <see cref="GeometryFactory"/> 
        /// with <see cref="PrecisionModel" /> <c> == </c> <see cref="PrecisionModels.Floating"/>.
        /// </remarks>
        public MultiLineString(ILineStringBase[] lineStrings) : this(lineStrings, DefaultFactory) { }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public override Dimensions Dimension
        {
            get
            {
                return Dimensions.Curve;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public override Dimensions BoundaryDimension
        {
            get
            {
                if (IsClosed)
                    return Dimensions.False;
                return Dimensions.Point;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        [Obsolete("Use reflection! GetType().Name")]
        public override string GeometryType
        {
            get
            {
                return "MultiLineString";
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is closed.
        /// </summary>
        /// <value><c>true</c> if this instance is closed; otherwise, <c>false</c>.</value>
        public virtual bool IsClosed
        {
            get
            {
                if (IsEmpty) 
                    return false;
                for (int i = 0; i < geometries.Length; i++)
                    if (!((LineString)geometries[i]).IsClosed)
                        return false;                
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public override bool IsSimple
        {
            get
            {
                return (new IsSimpleOp()).IsSimple((MultiLineString)this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public override IGeometry Boundary
        {
            get
            {
                if(IsEmpty)
                    return Factory.CreateGeometryCollection(null);
                GeometryGraph g = new GeometryGraph(0, this);
                Coordinate[] pts = g.GetBoundaryPoints();
                return Factory.CreateMultiPoint(pts);
            }
        }

        /// <summary>
        /// Creates a <see cref="MultiLineString" /> in the reverse order to this object.
        /// Both the order of the component LineStrings
        /// and the order of their coordinate sequences are reversed.
        /// </summary>
        /// <returns>a <see cref="MultiLineString" /> in the reverse order.</returns>
        public virtual IMultiLineString Reverse()
        {
            int nLines = geometries.Length;
            ILineString[] revLines = new LineString[nLines];
            for (int i = 0; i < geometries.Length; i++)
                revLines[nLines - 1 - i] = new LineString(((ILineString)geometries[i]).Reverse());            
            return Factory.CreateMultiLineString(revLines);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public override bool EqualsExact(IGeometry other, double tolerance) 
        {
            if (!IsEquivalentClass(other)) 
                return false;            
            return base.EqualsExact(other, tolerance);
        }

        
    }
}
