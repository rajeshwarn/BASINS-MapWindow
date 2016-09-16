using System;
using System.Collections.Generic;
using System.Text;
namespace MapWinGeoProc.Topology2D
{
    /// <summary>
    /// Contains a magnitude and direction
    /// Supports more fundamental calculations than LineSegment, rather than topological functions
    /// </summary>
    public struct Vector
    {
        #region Variables
        /// <summary>
        /// The X component of the vector
        /// </summary>
        public double X;
        /// <summary>
        /// The Y component of the vector
        /// </summary>
        public double Y;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new vector from the Point, assuming the tail of the vector is the origin
        /// </summary>
        /// <param name="Point">The Point to create a vector from</param>
        public Vector(Point Point)
        {
            X = Point.X;
            Y = Point.Y;
        }

        /// <summary>
        /// Creates a new vector from the specified coordinate
        /// </summary>
        /// <param name="Coord"></param>
        public Vector(Coordinate Coord)
        {
            X = Coord.X;
            Y = Coord.Y;
        }
        /// <summary>
        /// Creates a new vector from a line segment, assuming that the direction is from the start point to the end point
        /// </summary>
        /// <param name="LineLineSegment">A Topology.LineSegment object to turn into a vector</param>
        public Vector(LineSegment LineLineSegment)
        {

            X = LineLineSegment.X2 - LineLineSegment.X1;
            Y = LineLineSegment.Y2 - LineLineSegment.Y1;
        }
        /// <summary>
        /// Creates a new vector from two points, assuming that the direction is from the start point to the end point
        /// and uses the StartPoint as the origin.
        /// </summary>
        /// <param name="StartPoint">The beginning of the vector</param>
        /// <param name="EndPoint">The end of the vector</param>
        public Vector(Point StartPoint, Point EndPoint)
        {
            X = EndPoint.X - StartPoint.X;
            Y = EndPoint.Y - StartPoint.Y;
        }
        /// <summary>
        /// Creates a vector using two coordinates
        /// </summary>
        /// <param name="Tail">Coordinate where the vector starts</param>
        /// <param name="Tip">coordinate where the vector ends</param>
        public Vector(Coordinate Tail, Coordinate Tip)
        {
            X = Tip.X - Tail.X;
            Y = Tip.Y - Tail.Y;
        }
        /// <summary>
        /// Creates a mathematical vector from X1, Y1 to X2, Y2
        /// </summary>
        /// <param name="x1">Double, The X coordinate of the start point for the vector</param>
        /// <param name="y1">Double, The Y coordinate of the start point for the vector </param>
        /// <param name="x2">Double, The X coordinate of the end point for the vector</param>
        /// <param name="y2">Double, The Y coordinate of the end point for the vector</param>
        public Vector(double x1, double y1, double x2, double y2)
        {
            X = x2 - x1;
            Y = y2 - y1;
        }
        /// <summary>
        /// Creates a mathemtacal vector from the origin to the x, y, z coordinates
        /// </summary>
        /// <param name="x">Double, the X coordinate from the origin</param>
        /// <param name="y">Double, the Y coordinate from the origin</param>   
        public Vector(double x, double y)
        {
            X = x;
            Y = y;

        }

        /// <summary>
        /// Creates a mathematical vector from the origin with the new magnitude and directions specified
        /// </summary>
        /// <param name="newMagnitude">Double, the length of the vector</param>
        /// <param name="Theta">The angle in the x-y plane</param>
        public Vector(double newMagnitude, Angle Theta)
        {
            X = newMagnitude * Math.Cos(Theta.Radians);
            Y = newMagnitude * Math.Sin(Theta.Radians);
        }

        #endregion

        #region Properties


        /// <summary>
        /// The Euclidean distance from the origin to the tip of the 3 dimensional vector
        /// Setting the magntiude won't change the direction.
        /// </summary>
        public double Magnitude
        {
            get
            {
                return Math.Sqrt((X * X) + (Y * Y));
            }
            set
            {
                // preserves the angles, but adjusts the magnitude
                // if existing magnitude is 0, assume X direction.
                double mag = Magnitude;
                if (mag == 0)
                {
                    X = value;
                    return;
                }
                double Ratio = value / mag;
                X = X * Ratio;
                Y = Y * Ratio;
            }
        }

        /// <summary>
        /// Represents the angle in the X-Y plane.  0 along the positive X axis, and increasing counterclockwise
        /// Values are in Radians.  Setting this value when no X-Y magnitude exists results in a unit vector
        /// between X and Y.
        /// </summary>
        public Angle Theta
        {
            get
            {
                double m_Theta;
                double mag = Magnitude;
                if (mag == 0) return new Angle(0);
                if (X == 0 && Y > 0) return new Angle(Math.PI / 2);
                if (X == 0 && Y < 0) return new Angle(-Math.PI / 2);
                m_Theta = Math.Atan(Y / X);
                if (X < 0)
                {
                    m_Theta = Math.PI + m_Theta;
                }
                if (X > 0 && Y < 0)
                {
                    // Turn -Pi/2 -> 0 into 3Pi/2 -> 2Pi
                    m_Theta = Math.PI * 2 + m_Theta;
                }
                return new Angle(m_Theta);
            }
            set
            {
                double mag = Magnitude;
                if (mag == 0)
                {
                    X = Math.Cos(value.Radians);
                    Y = Math.Sin(value.Radians);
                    return;
                }
                X = mag * Math.Cos(value.Radians);
                Y = mag * Math.Sin(value.Radians);
            }

        }
        #endregion

        #region Methods

        #region Non Static Methods
        #region -------------------- COPY ---------------------------------

        /// <summary>
        /// Returns a new instance of the vector class with the same values as this object.
        /// </summary>
        /// <returns>Vector with identical properties.</returns>
        public Vector Copy()
        {
            Vector NewVector = new Vector(X, Y);
            return NewVector;
        }

        #endregion

        #region -------------------- CROSS --------------------------------
        /// <summary>
        /// Returns the cross product of this vector with the specified vector V.
        /// Given X and Y vectors, this creates a vector in the Z direction, so
        /// simply interpret the double value to be positive in the positive Z direction.
        /// </summary>
        /// <param name="V">The vector to perform a cross product against</param>
        /// <returns>A vector result from the inner product</returns>
        public double Cross(Vector V)
        {
            double result;
            result = (X * V.Y - Y * V.X);
            return result;
        }
        #endregion

        #region -------------------- DIVIDED BY ---------------------------
        /// <summary>
        /// Returns the quotient of this vector with Scalar
        /// </summary>
        /// <param name="Scalar">Double, a value to divide all the members of this vector by</param>
        /// <returns>A vector where each element has been divided by the scalar</returns>
        public Vector DividedBy(double Scalar)
        {
            if (Scalar == 0) throw new ArgumentException("Divisor cannot be 0");
            Vector Result = new Vector();
            Result.X = X / Scalar;
            Result.Y = Y / Scalar;
            return Result;
        }
        /// <summary>
        /// Divides each of the elements of this vector by the elements of V
        /// </summary>
        /// <param name="V">Vector, the vector to divide this vector by</param>
        /// <returns>A vector result from the division</returns>
        /// <remarks>To prevent division by 0, members of V that are 0 will produce an output
        /// vector with 0 in that column</remarks>
        public Vector DividedBy(Vector V)
        {
            Vector Result = new Vector();
            if (V.X > 0) Result.X = X / V.X;
            if (V.Y > 0) Result.Y = Y / V.Y;
            return Result;
        }
        #endregion

        #region -------------------- DOT ---------------------------------
        /// <summary>
        /// Returns the dot product of this vector with V2
        /// </summary>
        /// <param name="V">The vector to perform an inner product against</param>
        /// <returns>A Double result from the inner product</returns>
        public double Dot(Vector V)
        {
            return X * V.X + Y * V.Y;
        }
        #endregion
        /// <summary>
        /// Returns the square of the distance of the vector without taking the square root
        /// This is the same as doting the vector with itself
        /// </summary>
        /// <returns>Double, the square of the distance between the vectors</returns>
        public double Norm2()
        {
            return X * X + Y * Y;
        }

        #region -------------------- IS EQUAL TO---------------------------
       
        /// <summary>
        /// This function has been overriden to compare the vectors in the mathematical sense.
        /// If the components of the vector are equal, then this will return true.
        /// </summary>
        /// <param name="obj">An object to compare with this object</param>
        /// <returns>Boolean, true if the X and Y components are equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Vector))
            {
                Vector V = (Vector)obj;
                if ((X == V.X) && (Y == V.Y)) return true;
                return false;
            }
            return false;
        }
        /// <summary>
        /// Returns a hash code, whatever that is.
        /// </summary>
        /// <returns>A hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region -------------------- MINUS --------------------------------
        /// <summary>
        /// Subtracts each element of V from each element of this vector
        /// </summary>
        /// <param name="V">Vector, the vector to subtract from this vector</param>
        /// <returns>A vector result from the subtraction</returns>
        public Vector Minus(Vector V)
        {
            Vector Result = new Vector();
            Result.X = X - V.X;
            Result.Y = Y - V.Y;
            return Result;
        }
       
        #endregion

        #region -------------------- PLUS ---------------------------------
        /// <summary>
        /// Adds each of the elements of V to the elements of this vector
        /// </summary>
        /// <param name="V">Vector, the vector to add to this vector</param>
        /// <returns>A vector result from the addition</returns>
        public Vector Plus(Vector V)
        {
            Vector Result = new Vector();
            Result.X = X + V.X;
            Result.Y = Y + V.Y;
            return Result;
        }
       
        #endregion

        #region -------------------- TIMES --------------------------------
        /// <summary>
        /// Returns the scalar product of this vector against a scalar
        /// </summary>
        /// <param name="Scalar">Double, a value to multiply against all the members of this vector</param>
        /// <returns>A vector multiplied by the scalar</returns>
        public Vector Times(double Scalar)
        {
            Vector Result = new Vector();
            Result.X = X * Scalar;
            Result.Y = Y * Scalar;
            return Result;
        }
        /// <summary>
        /// Multiplies each of the elements of V to the elements of this vector
        /// </summary>
        /// <param name="V">Vector, the vector to multply with this vector</param>
        /// <returns>A vector result from the multiplication</returns>
        public Vector Times(Vector V)
        {
            Vector Result = new Vector();
            Result.X = X * V.X;
            Result.Y = Y * V.Y;
            return Result;
        }
        #endregion

        #region -------------------- TO POINT -----------------------------
        /// <summary>
        /// A MapWinGIS.Point Representing the tip of the vector,
        /// where the beginning of the vector is at the origin 
        /// </summary>
        /// <remarks>This is passed as an object to isolate MapWinGIS refs</remarks>
        public object To_MapWinGIS_Point()
        {
            return GeometryFactory.mwPointFromVector(this);
        }
        /// <summary>
        /// Assuming the vector starts at the origin of 0,0,0, this function returns
        /// a Point representing the tip of the vector.
        /// </summary>
        public Point ToPoint()
        {
            return new Point(X, Y);
        }
        #endregion

        #region -------------------- TO COORDINATE --------------------
        /// <summary>
        /// A MapWinGIS.Point Representing the tip of the vector,
        /// where the beginning of the vector is at the origin 
        /// /// </summary>
        public Coordinate ToCoordinate()
        {
            Coordinate pt = new Coordinate(X, Y);
            return pt;
        }
       
        #endregion



        #region -------------------- TO SEGMENT ---------------------------

        /// <summary>
        /// Returns a new segment from this vector, where the StartPoint is 0, 0, 0
        /// and the End Point is the tip of this vector
        /// </summary>
        /// <returns></returns>
        public LineSegment ToLineSegment()
        {
            return new LineSegment(this);
        }

        #endregion

        #endregion

        #region Static Methods

        /// <summary>
        /// Non-static version of taking the square distance for a vector
        /// </summary>
        /// <param name="U">The vector to find the square of the distance of</param>
        /// <returns>Double, the square of the distance</returns>
        public static double Norm2(Vector U)
        {
            double n2 = U.X * U.X + U.Y * U.Y;
            return n2;
        }

        #region -------------------- ADD VECTORS -------------------------
        /// <summary>
        /// Adds the vectors U and V using vector addition, which adds the corresponding components
        /// </summary>
        /// <param name="U">One vector to be added</param>
        /// <param name="V">A second vector to be added</param>
        /// <returns></returns>
        public static Vector AddVectors(Vector U, Vector V)
        {
            Vector result;
            result.X = U.X + V.X;
            result.Y = U.Y + V.Y;
            return result;
        }
        
        #endregion

        #region -------------------- CROSS PRODUCT ------------------------
        /// <summary>
        /// Returns the Cross Product of two vectors U and V
        /// </summary>
        /// <param name="U">Vector, the first input vector</param>
        /// <param name="V">Vector, the second input vector</param>
        /// <returns>A double containing the cross product of U and V</returns>
        public static double CrossProduct(Vector U, Vector V)
        {
            return (U.X * V.Y - U.Y * V.X);
        }
        #endregion

        #region -------------------- DIVIDE VECTORS ----------------------
        /// <summary>
        /// Divides the components of vector U by the respective components
        /// of vector V and returns the resulting vector. 
        /// </summary>
        /// <param name="U">Vector Dividend (Numbers to be divided)</param>
        /// <param name="V">Vector Divisor (Numbers to divide by)</param>
        /// <returns>A Vector quotient of U and V</returns>
        /// <remarks>To prevent divide by 0, if a 0 is in V, it will return 0 in the result</remarks>
        public static Vector DivideVectors(Vector U, Vector V)
        {
            Vector result = new Vector();
            if (V.X > 0) result.X = U.X / V.X;
            if (V.Y > 0) result.Y = U.Y / V.Y;

            return result;
        }
        /// <summary>
        /// Multiplies each component of vector U by the Scalar value
        /// </summary>
        /// <param name="U">A vector representing the vector to be multiplied</param>
        /// <param name="Scalar">Double, the scalar value to mulitiply the vector components by</param>
        /// <returns>A Vector representing the vector product of vector U and the Scalar</returns>
        public static Vector DivideVectors(Vector U, double Scalar)
        {
            Vector result;
            if (Scalar == 0) throw new ArgumentException("Divisor cannot be 0.");
            result.X = U.X / Scalar;
            result.Y = U.Y / Scalar;

            return result;
        }
        #endregion

        #region -------------------- DOT PRODUCT --------------------------
        /// <summary>
        /// Returns the Inner Product also known as the dot product of two vectors, U and V
        /// </summary>
        /// <param name="U">The input vector</param>
        /// <param name="V">The vector to take the inner product against U</param>
        /// <returns>a Double containing the dot product of U and V</returns>
        public static double DotProduct(Vector U, Vector V)
        {
            return (U.X * V.X) + (U.Y * V.Y);
        }
        #endregion

        #region -------------------- MULTIPLY VECTORS --------------------
        
        /// <summary>
        /// Multiplies each component of vector U by the Scalar value
        /// </summary>
        /// <param name="U">A vector representing the vector to be multiplied</param>
        /// <param name="Scalar">Double, the scalar value to mulitiply the vector components by</param>
        /// <returns>A Vector representing the vector product of vector U and the Scalar</returns>
        public static Vector MultiplyVectors(Vector U, double Scalar)
        {
            Vector result;
            result.X = U.X * Scalar;
            result.Y = U.Y * Scalar;

            return result;
        }
        #endregion

        #region -------------------- SUBTRACT VECTORS --------------------

        /// <summary>
        /// Subtracts Vector V from Vector U
        /// </summary>
        /// <param name="U">A Vector to subtract from</param>
        /// <param name="V">A Vector to subtract</param>
        /// <returns>The Vector difference U - V</returns>
        public static Vector SubtractVectors(Vector U, Vector V)
        {
            Vector result;
            result.X = U.X - V.X;
            result.Y = U.Y - V.Y;

            return result;
        }

        #endregion

        #region Operators

        #region -------------------- EQUAL -------------------------------
        /// <summary>
        /// Returns true if X and Y coordinates are equal
        /// </summary>
        /// <param name="U">A vector to compare</param>
        /// <param name="V">A second vector to compare</param>
        /// <returns>Boolean, true if the vectors are the same</returns>
        public static bool operator ==(Vector U, Vector V)
        {
            if (U.X == V.X && U.Y == V.Y) return true;
            return false;
        }
        /// <summary>
        /// Returns true if X and Y coordinates are different
        /// </summary>
        /// <param name="U">A vector to compare</param>
        /// <param name="V">A second vector to compare</param>
        /// <returns>Boolean, true if the vectors are the different</returns>
        public static bool operator !=(Vector U, Vector V)
        {
            if (U.X == V.X && U.Y == V.Y) return false;
            return true;
        }

        #endregion 

        #region -------------------- ADD VECTORS -------------------------
        /// <summary>
        /// Adds the vectors U and V using vector addition, which adds the corresponding components
        /// </summary>
        /// <param name="U">One vector to be added</param>
        /// <param name="V">A second vector to be added</param>
        /// <returns></returns>
        public static Vector operator +(Vector U, Vector V)
        {
            Vector result;
            result.X = U.X + V.X;
            result.Y = U.Y + V.Y;
            return result;
        }
       
        #endregion

        #region -------------------- CROSS PRODUCT ------------------------
        /// <summary>
        /// Returns the Cross Product of two vectors U and V
        /// </summary>
        /// <param name="U">Vector, the first input vector</param>
        /// <param name="V">Vector, the second input vector</param>
        /// <returns>A double containing the cross product of U and V</returns>
        public static double operator ^(Vector U, Vector V)
        {
            return (U.X * V.Y - U.Y * V.X);
        }
        #endregion

        #region -------------------- DIVIDE VECTORS ----------------------
        /// <summary>
        /// Divides the components of vector U by the respective components
        /// ov vector V and returns the resulting vector. 
        /// </summary>
        /// <param name="U">Vector Dividend (Numbers to be divided)</param>
        /// <param name="V">Vector Divisor (Numbers to divide by)</param>
        /// <returns>A Vector quotient of U and V</returns>
        /// <remarks>To prevent divide by 0, if a 0 is in V, it will return 0 in the result</remarks>
        public static Vector operator /(Vector U, Vector V)
        {
            Vector result = new Vector();
            if (V.X > 0) result.X = U.X / V.X;
            if (V.Y > 0) result.Y = U.Y / V.Y;

            return result;
        }
        /// <summary>
        /// Multiplies each component of vector U by the Scalar value
        /// </summary>
        /// <param name="U">A vector representing the vector to be multiplied</param>
        /// <param name="Scalar">Double, the scalar value to mulitiply the vector components by</param>
        /// <returns>A Vector representing the vector product of vector U and the Scalar</returns>
        public static Vector operator /(Vector U, double Scalar)
        {
            Vector result;
            if (Scalar == 0) throw new ArgumentException("Divisor cannot be 0.");
            result.X = U.X / Scalar;
            result.Y = U.Y / Scalar;

            return result;
        }
        #endregion

        #region -------------------- DOT PRODUCT --------------------------
        /// <summary>
        /// Returns the Inner Product also known as the dot product of two vectors, U and V
        /// </summary>
        /// <param name="U">The input vector</param>
        /// <param name="V">The vector to take the inner product against U</param>
        /// <returns>a Double containing the dot product of U and V</returns>
        public static double operator *(Vector U, Vector V)
        {
            return (U.X * V.X) + (U.Y * V.Y);
        }
        #endregion

        #region -------------------- MULTIPLY VECTORS --------------------
        /// <summary>
        /// Multiplies the vectors U and V using vector multiplication,
        /// which adds the corresponding components
        /// </summary>
        /// <param name="Scalar">One vector to be multiplied</param>
        /// <param name="V">A second vector to be multiplied</param>
        /// <returns>A Vector product of U and V</returns>
        public static Vector operator *(double Scalar, Vector V)
        {
            Vector result;
            result.X = Scalar * V.X;
            result.Y = Scalar * V.Y;

            return result;
        }
        /// <summary>
        /// Multiplies each component of vector U by the Scalar value
        /// </summary>
        /// <param name="U">A vector representing the vector to be multiplied</param>
        /// <param name="Scalar">Double, the scalar value to mulitiply the vector components by</param>
        /// <returns>A Vector representing the vector product of vector U and the Scalar</returns>
        public static Vector operator *(Vector U, double Scalar)
        {
            Vector result;
            result.X = U.X * Scalar;
            result.Y = U.Y * Scalar;

            return result;
        }
        #endregion

        #region -------------------- SUBTRACT VECTORS --------------------

        /// <summary>
        /// Subtracts Vector V from Vector U
        /// </summary>
        /// <param name="U">A Vector to subtract from</param>
        /// <param name="V">A Vector to subtract</param>
        /// <returns>The Vector difference U - V</returns>
        public static Vector operator -(Vector U, Vector V)
        {
            Vector result;
            result.X = U.X - V.X;
            result.Y = U.Y - V.Y;

            return result;
        }

        #endregion

        #endregion

        #endregion

        #endregion

    }

}