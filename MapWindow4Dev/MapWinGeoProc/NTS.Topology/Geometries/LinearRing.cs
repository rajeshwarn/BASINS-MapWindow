using System;
using System.Collections;
using System.Text;



using MapWindow.Interfaces.Geometries;

namespace MapWinGeoProc.NTS.Topology.Geometries
{
    /// <summary>  
    /// Basic implementation of <c>LinearRing</c>.
    /// The first and last point in the coordinate sequence must be equal.
    /// Either orientation of the ring is allowed.
    /// A valid ring must not self-intersect.
    /// </summary>
    [Serializable]
    public class LinearRing : LineString, ILinearRing
    {

        /// <summary>
        /// Constructs a <c>LinearRing</c> with the given points.
        /// </summary>
        /// <param name="points">
        /// Points forming a closed and simple linestring, or
        /// <c>null</c> or an empty array to create the empty point.
        /// This array must not contain <c>null</c> elements.
        /// </param>
        /// <param name="factory"></param>
        public LinearRing(ICoordinateSequence points, GeometryFactory factory) : base(points, factory)
        {            
            ValidateConstruction();
        }

        /// <summary>
        /// creates a LinearRing given a basic linestring 
        /// </summary>
        /// <param name="linestringbase">the base data to make into a linear ring</param>
        public LinearRing(ILineStringBase linestringbase)
            : base(linestringbase)
        {

        }

        private void ValidateConstruction() 
        {
	        if (!IsEmpty && !base.IsClosed) 
                throw new ArgumentException("points must form a closed linestring");            
            if (CoordinateSequence.Count >= 1 && CoordinateSequence.Count <= 3) 
                throw new ArgumentException("Number of points must be 0 or >3");            
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
       /// Specifies that this is a LinearRing
       /// </summary>
        public override string GeometryType
        {
            get
            {
                return "LinearRing";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsClosed
        {
            get
            {
                return true;
            }
        }

        /* BEGIN ADDED BY MPAUL42: monoGIS team */

        /// <summary>
        /// Initializes a new instance of the <see cref="T:LinearRing"/> class.
        /// </summary>
        /// <param name="points">The points used for create this instance.</param>
        /// <remarks>
        /// For create this <see cref="Geometry"/> is used a standard <see cref="GeometryFactory"/> 
        /// with <see cref="PrecisionModel" /> <c> == </c> <see cref="PrecisionModels.Floating"/>.
        /// </remarks>
        public LinearRing(Coordinate[] points) : 
            this(DefaultFactory.CoordinateSequenceFactory.Create(points), DefaultFactory) { }

        /// <summary>
        /// Creates a new instance of a Linear Ring using only ICoordinate
        /// </summary>
        /// <param name="points"></param>
        public LinearRing(ICoordinate[] points) :
            this(DefaultFactory.CoordinateSequenceFactory.Create(points), DefaultFactory) { }
        
        /* END ADDED BY MPAUL42: monoGIS team */

       
    }
}
