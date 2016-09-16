using System;
using System.Collections;
using System.Text;
using System.Diagnostics;


using MapWindow.Interfaces.Geometries;


namespace MapWinGeoProc.NTS.Topology.Geometries
{
    /// <summary>
    /// Basic implementation of <c>Point</c>.
    /// </summary>
    [Serializable]
    public class Point : Geometry, IPoint
    {
        private static readonly Coordinate emptyCoordinate = null;

        /// <summary>
        /// Represents an empty <c>Point</c>.
        /// </summary>
        public static readonly IPoint Empty = new GeometryFactory().CreatePoint(emptyCoordinate);

        /// <summary>  
        /// The <c>Coordinate</c> wrapped by this <c>Point</c>.
        /// </summary>
        private ICoordinateSequence coordinates;        

        /// <summary>
        /// 
        /// </summary>
        public virtual ICoordinateSequence CoordinateSequence
        {
            get
            {
                return coordinates;
            }
        }             

         /// <summary>
        /// Initializes a new instance of the <see cref="T:Point"/> class.
        /// </summary>
        /// <param name="coordinate">The coordinate used for create this <see cref="Point" />.</param>
        /// <remarks>
        /// For create this <see cref="Geometry"/> is used a standard <see cref="GeometryFactory"/> 
        /// with <see cref="PrecisionModel" /> <c> == </c> <see cref="PrecisionModels.Floating"/>.
        /// </remarks>
        public Point(ICoordinate coordinate) :   
            this(new GeometryFactory().Default.CoordinateSequenceFactory.Create(new Coordinate[] { new Coordinate(coordinate) } ),
            new GeometryFactory()) { }

        /// <summary>
        /// Constructs a <c>Point</c> with the given coordinate.
        /// </summary>
        /// <param name="coordinates">
        /// Contains the single coordinate on which to base this <c>Point</c>,
        /// or <c>null</c> to create the empty point.
        /// </param>
        /// <param name="factory"></param>
        public Point(ICoordinateSequence coordinates, GeometryFactory factory) : base(factory)
        {               
            if (coordinates == null) 
                coordinates = factory.CoordinateSequenceFactory.Create(new Coordinate[] { });
            Debug.Assert(coordinates.Count <= 1);
            this.coordinates = (ICoordinateSequence)coordinates;
        }        

        /// <summary>
        /// 
        /// </summary>
        public override ICoordinate[] Coordinates 
        {
            get
            {
                return IsEmpty ? new ICoordinate[] { } : new ICoordinate[]{ this.Coordinate };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int NumPoints
        {
            get
            {
                return IsEmpty ? 0 : 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsEmpty 
        {
            get
            {
                return this.Coordinate == null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsSimple
        {
            get
            {
                return true;
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
        public virtual double X
        {
            get
            {
                if (Coordinate == null)
                    throw new ArgumentOutOfRangeException("X called on empty Point");                
                return Coordinate.X;
            }
            set
            {
                Coordinate.X = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>        
        public virtual double Y 
        {
            get
            {
                if (Coordinate == null)
                    throw new ArgumentOutOfRangeException("Y called on empty Point");                
                return Coordinate.Y;
            }
            set
            {
                Coordinate.Y = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override ICoordinate Coordinate
        {
            get
            {
                return coordinates.Count != 0 ? coordinates.GetCoordinate(0) : null;
            }
        }

       /// <summary>
       /// An enumeration to clarify that this is a Point geometry type
       /// </summary>
        public override string GeometryType 
        {
            get
            {
                return "Point";
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
        /// <returns></returns>
        protected override IEnvelope ComputeEnvelopeInternal() 
        {
            if (IsEmpty) 
                return new Envelope();            
            return new Envelope(Coordinate.X, Coordinate.X, Coordinate.Y, Coordinate.Y);
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
            if (IsEmpty && other.IsEmpty) 
                return true;
            return Equal(new Coordinate(other.Coordinate), this.Coordinate, tolerance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        public override void Apply(ICoordinateFilter filter) 
        {
            if (IsEmpty) 
                return;             
            filter.Filter(Coordinate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        public override void Apply(IGeometryFilter filter)
        {
            filter.Filter(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        public override void Apply(IGeometryComponentFilter filter) 
        {
            filter.Filter(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone() 
        {
            Point p = (Point) base.Clone();
            p.coordinates = (ICoordinateSequence)coordinates.Clone();
            return p; 
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Normalize() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override int CompareToSameClass(object other) 
        {
            Point point = (Point) other;
            return Coordinate.CompareTo(point.Coordinate);
        }

        /* BEGIN ADDED BY MPAUL42: monoGIS team */

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Point"/> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        /// /// <remarks>
        /// For create this <see cref="Geometry"/> is used a standard <see cref="GeometryFactory"/> 
        /// with <see cref="PrecisionModel" /> <c> set to </c> <see cref="PrecisionModels.Floating"/>.
        /// </remarks>
        public Point(double x, double y, double z) : 
            this(DefaultFactory.CoordinateSequenceFactory.Create(new Coordinate[] { new Coordinate(x, y, z) }), DefaultFactory) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Point"/> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// /// <remarks>
        /// For create this <see cref="Geometry"/> is used a standard <see cref="GeometryFactory"/> 
        /// with <see cref="PrecisionModel" /> <c> set to </c> <see cref="PrecisionModels.Floating"/>.
        /// </remarks>
        public Point(double x, double y)
            : this(DefaultFactory.CoordinateSequenceFactory.Create(new Coordinate[] { new Coordinate(x, y) }), DefaultFactory) { }

        /// <summary>
        /// 
        /// </summary>        
        public virtual double Z
        {
            get
            {
                if (Coordinate == null)
                    throw new ArgumentOutOfRangeException("Z called on empty Point");
                return Coordinate.Z;
            }
            set 
            { 
                Coordinate.Z = value; 
            }
        }

        /* END ADDED BY MPAUL42: monoGIS team */

       


     
        /// <summary>
        /// This is technically an envelope, but all the X and Y values are the same as 
        /// the X and Y coordinates for this point
        /// </summary>
        public new IEnvelope Envelope
        {
            get { return new Envelope(this.X, this.Y, this.X, this.Y); }
        }

        /// <summary>
        /// Tests to see if the X coordinate and Y coordinate are the same between this point and the
        /// specified Coordinate
        /// </summary>
        /// <param name="Coord">Any valid implementation of the ICoordinate Interface</param>
        /// <returns>True if the coordinates are equal, false otherwise</returns>
        public bool Equals2D(ICoordinate Coord)
        {
            if (this.X == Coord.X && this.Y == Coord.Y) return true;
            return false;
        }

        /// <summary>
        /// Tests to see if the X, Y, and Z coordinate are the same between this point and the
        /// specified Coordinate
        /// </summary>
        /// <param name="Coord">Any valid implementation of the ICoordinate Interface</param>
        /// <returns>True if the coordinates are equal, false otherwise</returns>
        public bool Equals3D(ICoordinate Coord)
        {
            if (this.X == Coord.X && this.Y == Coord.Y && this.Z == Coord.Z) return true;
            return false;
        }

        /// <summary>
        /// Calculates the vector distance.  (This is a 2D opperation)
        /// </summary>
        /// <param name="Coord">Any valid implementation of the ICoordinate Interface</param>
        /// <returns>The Euclidean distance between two points {Sqrt((X2 - X1)^2 + (Y2 - Y1)^2)</returns>
        public double Distance(ICoordinate Coord)
        {
            double dX = this.X - Coord.X;
            double dY = this.Y - Coord.Y;
            return Math.Sqrt(dX * dX - dY * dY);
        }
       
    }
}
