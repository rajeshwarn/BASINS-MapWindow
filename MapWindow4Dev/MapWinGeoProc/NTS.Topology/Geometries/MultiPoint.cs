using System;
using System.Collections;
using System.Text;

using MapWinGeoProc.NTS.Topology.Operation;
using MapWindow.Interfaces.Geometries;
namespace MapWinGeoProc.NTS.Topology.Geometries
{
    /// <summary>  
    /// Models a collection of <c>Point</c>s.
    /// </summary>
    [Serializable]
    public class MultiPoint : GeometryCollection, IMultiPoint
    {
        /// <summary>
        /// Represents an empty <c>MultiPoint</c>.
        /// </summary>
        public static new readonly IMultiPoint Empty = new GeometryFactory().CreateMultiPoint(new Point[] { });
        
        /// <summary>
        /// Constructs a <c>MultiPoint</c>.
        /// </summary>
        /// <param name="points">
        /// The <c>Point</c>s for this <c>MultiPoint</c>
        /// , or <c>null</c> or an empty array to create the empty point.
        /// Elements may be empty <c>Point</c>s, but not <c>null</c>s.
        /// </param>
        /// <param name="factory"></param>
        public MultiPoint(ICoordinate[] points, IGeometryFactory factory) : base(points, factory){}
        


        

        /// <summary>
        /// Constructs a <c>MultiPoint</c>.
        /// </summary>
        /// <param name="points">
        /// The <c>Point</c>s for this <c>MultiPoint</c>
        /// , or <c>null</c> or an empty array to create the empty point.
        /// Elements may be empty <c>Point</c>s, but not <c>null</c>s.
        /// </param>
        /// <remarks>
        /// For create this <see cref="Geometry"/> is used a standard <see cref="GeometryFactory"/> 
        /// with <see cref="PrecisionModel" /> <c> == </c> <see cref="PrecisionModels.Floating"/>.
        /// </remarks>
        public MultiPoint(Point[] points) : this(points, DefaultFactory) { }

        /// <summary>
        /// Creates new Multipoint using interface points
        /// </summary>
        /// <param name="points"></param>
        public MultiPoint(ICoordinate[] points) : this(CastPoints(points), DefaultFactory) { }

        /// <summary>
        /// Converts an array of point interface variables into local points.
        /// Eventually I hope to reduce the amount of "casting" necessary, in order
        /// to allow as much as possible to occur via an interface.
        /// </summary>
        /// <param name="rawPoints"></param>
        /// <returns></returns>
        public static Point[] CastPoints(ICoordinate[] rawPoints)
        {
            int Count = rawPoints.GetUpperBound(0)+1;
            Point[] outPoints = new Point[Count];
            for (int I = 0; I < Count; I++)
            {
                outPoints[I] = new Point(rawPoints[I]);
            }
            return outPoints;
        }
        /// <summary>
        /// 
        /// </summary>
        public override Dimensions Dimension 
        {
            get
            {
                return Dimensions.Point;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override Dimensions BoundaryDimension
        {
            get
            {
                return Dimensions.False;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string GeometryType
        {
            get
            {
                return "MultiPoint";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override IGeometry Boundary
        {
            get
            {
                return Factory.CreateGeometryCollection(null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsSimple
        {
            get
            {
                return (new IsSimpleOp()).IsSimple((MultiPoint)this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return true;
            }
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

        /// <summary>
        /// Returns the <c>Coordinate</c> at the given position.
        /// </summary>
        /// <param name="n">The index of the <c>Coordinate</c> to retrieve, beginning at 0.
        /// </param>
        /// <returns>The <c>n</c>th <c>Coordinate</c>.</returns>
        protected virtual ICoordinate GetCoordinate(int n) 
        {
            return ((Point)geometries[n]).Coordinate;
        }

        
    }
}
