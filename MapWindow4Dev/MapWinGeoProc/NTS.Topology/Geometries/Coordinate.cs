using System;
using System.Runtime.Serialization;

using MapWindow.Interfaces.Geometries;


namespace MapWinGeoProc.NTS.Topology.Geometries
{

    /// <summary>
    /// A lightweight class used to store coordinates
    /// on the 2-dimensional Cartesian plane.
    /// It is distinct from <c>Point</c>, which is a subclass of <c>Geometry</c>.
    /// Unlike objects of type <c>Point</c> (which contain additional
    /// information such as an envelope, a precision model, and spatial reference
    /// system information), a <c>Coordinate</c> only contains ordinate values
    /// and accessor methods.
    /// <c>Coordinate</c>s are two-dimensional points, with an additional
    /// z-ordinate. NTS does not support any operations on the z-ordinate except
    /// the basic accessor functions. Constructed coordinates will have a
    /// z-ordinate of <c>NaN</c>.  The standard comparison functions will ignore
    /// the z-ordinate.
    /// </summary>
    [Serializable]
    public class Coordinate : ICoordinate
    {
        private double x = Double.NaN;
        private double y = Double.NaN;
        private double z = Double.NaN;

        /// <summary>
        /// X coordinate.
        /// </summary>
        public virtual double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        /// <summary>
        /// Y coordinate.
        /// </summary>
        public virtual double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        /// <summary>
        /// Z coordinate.
        /// </summary>
        public virtual double Z
        {
            get
            {
                return z;
            }
            set
            {
                z = value;
            }
        }

        /// <summary>
        /// Constructs a <c>Coordinate</c> at (x,y,z).
        /// </summary>
        /// <param name="x">X value.</param>
        /// <param name="y">Y value.</param>
        /// <param name="z">Z value.</param>
        public Coordinate(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Creates a Coordinate from any ICoordinate Interface
        /// </summary>
        /// <param name="Icoord">The Vector.IPoint interface to construct a coordinate from</param>
        public Coordinate(ICoordinate Icoord)
        {
            this.X = Icoord.X;
            this.y = Icoord.Y;
            this.Z = Icoord.Z;
        }


        /// <summary>
        ///  Constructs a <c>Coordinate</c> at (0,0,NaN).
        /// </summary>
        public Coordinate() : this(0.0, 0.0, Double.NaN) { }
        
        ///// <summary>
        ///// Constructs a <c>Coordinate</c> having the same (x,y,z) values as
        ///// <c>other</c>.
        ///// </summary>
        ///// <param name="c"><c>Coordinate</c> to copy.</param>
        //public Coordinate(Coordinate c) : this(c.x, c.y, c.z) { }

        /// <summary>
        /// Constructs a <c>Coordinate</c> at (x,y,NaN).
        /// </summary>
        /// <param name="x">X value.</param>
        /// <param name="y">Y value.</param>
        public Coordinate(double x, double y) : this(x, y, Double.NaN) { }

        /// <summary>
        /// Gets/Sets <c>Coordinate</c>s (x,y,z) values.
        /// </summary>
        public virtual Coordinate CoordinateValue
        {
            get
            {
                return this;
            }
            set
            {
                x = value.x;
                y = value.y;
                z = value.z;
            }
        }

        /// <summary>
        /// Returns whether the planar projections of the two <i>Coordinate</i>s are equal.
        ///</summary>
        /// <param name="other"><i>ICoordinate</i> with which to do the 2D comparison.</param>
        /// <returns>
        /// <c>true</c> if the x- and y-coordinates are equal;
        /// the Z coordinates do not have to be equal.
        /// </returns>
        public bool Equals2D(ICoordinate other)
        {
            if (x != other.X)
                return false;
            if (y != other.Y)
                return false;
            return true;
            
        }



        /// <summary>
        /// Returns <c>true</c> if <c>other</c> has the same values for the x and y ordinates.
        /// Since Coordinates are 2.5D, this routine ignores the z value when making the comparison.
        /// </summary>
        /// <param name="other"><c>Coordinate</c> with which to do the comparison.</param>
        /// <returns><c>true</c> if <c>other</c> is a <c>Coordinate</c> with the same values for the x and y ordinates.</returns>
        public override bool Equals(object other)
        {
            if (other == null)
                return false;
            if (!(other is ICoordinate))
                return false;
            return Equals2D((ICoordinate) other);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool operator ==(Coordinate obj1, ICoordinate obj2)
        {
            return Object.Equals(obj1, obj2);
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool operator !=(Coordinate obj1, ICoordinate obj2)
        {
            return !(obj1 == obj2);
        }
       
        /// <summary>
        /// Compares this object with the specified object for order.
        /// Since Coordinates are 2.5D, this routine ignores the z value when making the comparison.
        /// Returns
        ///    -1 : this.x lowerthan other.x || ((this.x == other.x) AND (this.y lowerthan other.y))
        ///    0  : this.x == other.x AND this.y = other.y 
        ///    1  : this.x greaterthan other.x || ((this.x == other.x) AND (this.y greaterthan other.y)) 
        /// </summary>
        /// <param name="o"><c>Coordinate</c> with which this <c>Coordinate</c> is being compared.</param>
        /// <returns>
        /// A negative integer, zero, or a positive integer as this <c>Coordinate</c>
        ///         is less than, equal to, or greater than the specified <c>Coordinate</c>.
        /// </returns>
        public virtual int CompareTo(object o)
        {
            ICoordinate other = (ICoordinate) o;
            if (x < other.X)
                return -1;
            if (x > other.X)
                return 1;
            if (y < other.Y)
                return -1;
            if (y > other.Y)
                return 1;
            return 0;
        }

        /// <summary>
        /// Returns true if other has the same values for x, y and z.
        /// </summary>
        /// <param name="other"><i>ICoordinate</i> with which to do the 3D comparison.</param>
        /// <returns><c>true</c> if <c>other</c> is a <c>ICoordinate</c> with the same values for x, y and z.</returns>
        public bool Equals3D(ICoordinate other)
        {
            return (x == other.X) && (y == other.X) && ((z == other.Z)
            || (Double.IsNaN(z) && Double.IsNaN(other.Z)));
        }


        /// <summary>
        /// Returns a <c>string</c> of the form <I>(x,y,z)</I> .
        /// </summary>
        /// <returns><c>string</c> of the form <I>(x,y,z)</I></returns>
        public override string ToString()
        {
            return "(" + x + ", " + y + ", " + z + ")";
        }

        /// <summary>
        /// Create a new object as copy of this instance.
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            return new Coordinate(this.X, this.Y, this.Z);
        }

        /// <summary>
        /// Returns Euclidean 2D distance from ICoordinate p.
        /// </summary>
        /// <param name="p"><i>ICoordinate</i> with which to do the distance comparison.</param>
        /// <returns>Double, the distance between the two locations.</returns>
        public double Distance(ICoordinate p)
        {
            double dx = x - p.X;
            double dy = y - p.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
        /// <summary>
        /// Return HashCode.
        /// </summary>
        public override int GetHashCode()
        {
            int result = 17;            
            result = 37 * result + GetHashCode(X);
            result = 37 * result + GetHashCode(Y);
            return result;
        }

        /// <summary>
        /// Return HashCode.
        /// </summary>
        /// <param name="x">Value from HashCode computation.</param>
        public static int GetHashCode(double x)
        {
            long f = BitConverter.DoubleToInt64Bits(x);
            return (int)(f^(f>>32));
        }

        /* BEGIN ADDED BY MPAUL42: monoGIS team */

        /// <summary>
        /// Overloaded + operator.
        /// </summary>
        public static Coordinate operator +(Coordinate coord1, ICoordinate coord2)
        {
            // returns Coordinate as a specific implementatino of ICoordinate
            return new Coordinate(coord1.X + coord2.X, coord1.Y + coord2.Y, coord1.Z + coord2.Z);
        }

       

        /// <summary>
        /// Overloaded + operator.
        /// </summary>
        public static Coordinate operator +(Coordinate coord1, double d)
        {
            return new Coordinate(coord1.X + d, coord1.Y + d, coord1.Z + d);
        }

        /// <summary>
        /// Overloaded + operator.
        /// </summary>
        public static Coordinate operator +(double d, Coordinate coord1)
        {
            return coord1 + d;
        }

        /// <summary>
        /// Overloaded * operator.
        /// </summary>
        public static Coordinate operator *(Coordinate coord1, ICoordinate coord2)
        {
            return new Coordinate(coord1.X * coord2.X, coord1.Y * coord2.Y, coord1.Z * coord2.Z);
        }
       

        /// <summary>
        /// Overloaded * operator.
        /// </summary>
        public static Coordinate operator *(Coordinate coord1, double d)
        {
            return new Coordinate(coord1.X * d, coord1.Y * d, coord1.Z * d);
        }

        /// <summary>
        /// Overloaded * operator.
        /// </summary>
        public static Coordinate operator *(double d, Coordinate coord1)
        {
            return coord1 * d;
        }

        /// <summary>
        /// Overloaded - operator.
        /// </summary>
        public static Coordinate operator -(Coordinate coord1, ICoordinate coord2)
        {
            return new Coordinate(coord1.X - coord2.X, coord1.Y - coord2.Y, coord1.Z - coord2.Z);
        }
       

        /// <summary>
        /// Overloaded - operator.
        /// </summary>
        public static Coordinate operator -(Coordinate coord1, double d)
        {
            return new Coordinate(coord1.X - d, coord1.Y - d, coord1.Z - d);
        }

        /// <summary>
        /// Overloaded - operator.
        /// </summary>
        public static Coordinate operator -(double d, Coordinate coord1)
        {
            return coord1 - d;
        }

        /// <summary>
        /// Overloaded / operator.
        /// </summary>
        public static Coordinate operator /(Coordinate coord1, ICoordinate coord2)
        {
            return new Coordinate(coord1.X / coord2.X, coord1.Y / coord2.Y, coord1.Z / coord2.Z);
        }
        
     
        /// <summary>
        /// Overloaded / operator.
        /// </summary>
        public static Coordinate operator /(Coordinate coord1, double d)
        {
            return new Coordinate(coord1.X / d, coord1.Y / d, coord1.Z / d);
        }  

        /// <summary>
        /// Overloaded / operator.
        /// </summary>
        public static Coordinate operator /(double d, Coordinate coord1)
        {
            return coord1 / d;
        }

        /* END ADDED BY MPAUL42: monoGIS team */


        
    }
} 
