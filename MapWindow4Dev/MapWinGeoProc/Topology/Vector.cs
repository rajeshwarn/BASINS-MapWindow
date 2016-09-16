using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Topology
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
        public double X; // cartesian 
        /// <summary>
        /// The Y component of the vector
        /// </summary>
        public double Y; // 
        /// <summary>
        /// The Z component of the vector
        /// </summary>
        public double Z; //


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
            Z = Point.Z;
        }
        /// <summary>
        /// Creates a new vector from a line segment, assuming that the direction is from the start point to the end point
        /// </summary>
        /// <param name="LineLineSegment">A Topology.LineSegment object to turn into a vector</param>
        public Vector(LineSegment LineLineSegment)
        {

            X = LineLineSegment.X2 - LineLineSegment.X1;
            Y = LineLineSegment.Y2 - LineLineSegment.Y1;
            Z = LineLineSegment.Z2 - LineLineSegment.Z1;
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
            Z = EndPoint.Z - StartPoint.Z;
        }
        /// <summary>
        /// Creates a mathematical vector from X1, Y1 to X2, Y2
        /// </summary>
        /// <param name="x1">Double, The X coordinate of the start point for the vector</param>
        /// <param name="y1">Double, The Y coordinate of the start point for the vector </param>
        /// <param name="z1">Double, the Z coordinate of the start point for the vector</param>
        /// <param name="x2">Double, The X coordinate of the end point for the vector</param>
        /// <param name="y2">Double, The Y coordinate of the end point for the vector</param>
        /// <param name="z2">Double, the Z coordinate of the end point for the vector</param>
        public Vector(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            X = x2 - x1;
            Y = y2 - y1;
            Z = z2 - z1;
        }
        /// <summary>
        /// Creates a mathemtacal vector from the origin to the x, y, z coordinates
        /// </summary>
        /// <param name="x">Double, the X coordinate from the origin</param>
        /// <param name="y">Double, the Y coordinate from the origin</param>
        /// <param name="z">Double, the Z coordinate from the origin</param>
        public Vector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Creates a mathematical vector from the origin with the new magnitude and directions specified
        /// </summary>
        /// <param name="newMagnitude">Double, the length of the vector</param>
        /// <param name="Theta">The angle in the x-y plane</param>
        /// <param name="Phi">The angle in the z direction</param>
        public Vector(double newMagnitude, Angle Theta, Angle Phi)
        {
            X = newMagnitude * Angle.Cos(Theta) * Angle.Cos(Phi);
            Y = newMagnitude * Angle.Sin(Theta) * Angle.Cos(Phi);
            Z = newMagnitude * Angle.Sin(Phi);
        }

        /// <summary>
        /// Creates a mathematical vector in the X-Y plane with angle Theta
        /// </summary>
        /// <param name="newMagnitude">Double, The magnitude of the vector</param>
        /// <param name="Theta">Angle, The direction measured counterclockwise from Positive X Axis </param>
        public Vector(double newMagnitude, Angle Theta)
        {
            X = newMagnitude * Angle.Cos(Theta);
            Y = newMagnitude * Angle.Sin(Theta);
            Z = 0;
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
                return Math.Sqrt((X * X) + (Y * Y) + (Z * Z));
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
                Z = Z * Ratio;
            }
        }
        /// <summary>
        /// Returns the magnitude of the projection of the vector onto the X-Y plane
        /// Setting this magnitude will not affect Z, which should be adjusted separately
        /// </summary>
        public double Magnitude2D
        {
            get
            {
                return Math.Sqrt((X * X) + (Y * Y));
            }
            set
            {
                // preserves the angles, but adjusts the magnitude
                // if existing magnitude is 0, assume X direction.
                double mag = Magnitude2D;
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
        /// Obtains the angle above the X-Y plane.  Positive towards positive Z.
        /// Values are in radians from -Pi/2 to Pi/2
        /// Setting this value when no magnitude exists results in a unit vector with angle phi in the X direction.
        /// </summary>
        public double Phi
        {
            get
            {
                double mag = Magnitude;
                if (mag == 0) return 0;
                return Math.Asin(Z / mag);
            }
            set
            {
                // Preserves the existing magnitude unless it is 0
                double mag = Magnitude;
                if (mag == 0)
                {
                    // Create a unit vector in X and Z
                    X = Math.Cos(value);
                    Z = Math.Sin(value);
                    return;
                }
                Z = mag * Math.Sin(value);
                Magnitude2D = mag * Math.Cos(value);
            }
        }
        /// <summary>
        /// Represents the angle in the X-Y plane.  0 along the positive X axis, and increasing counterclockwise
        /// Values are in Radians.  Setting this value when no X-Y magnitude exists results in a unit vector
        /// between X and Y, but does not affect Z, so you may have something other than a unit vector in 3-D.
        /// Set theta before phi in order to obtain a unit vector in 3-D space.
        /// </summary>
        public double Theta
        {
            get
            {
                double m_Theta;
                double mag = Magnitude2D;
                if (mag == 0) return 0;
                m_Theta = Math.Atan(Y / X);
                if (X < 0)
                {
                    m_Theta = Math.PI - m_Theta;
                }
                if (X > 0 && Y < 0)
                {
                    // Turn -Pi/2 -> 0 into 3Pi/2 -> 2Pi
                    m_Theta = Math.PI * 2 + m_Theta;
                }
                return m_Theta;
            }
            set
            {
                double mag = Magnitude2D;
                if (mag == 0)
                {
                    X = Math.Cos(value);
                    Y = Math.Sin(value);
                    return;
                }
                X = mag * Math.Cos(value);
                Y = mag * Math.Sin(value);
            }

        }
        
        #endregion

        #region Methods

        #region Non Static Methods
        /// <summary>
        /// Returns the square of the distance of the vector without taking the square root
        /// This is the same as doting the vector with itself
        /// </summary>
        /// <returns>Double, the square of the distance between the vectors</returns>
        public double Norm2()
        {
            return X * X + Y * Y + Z*Z;
        }

        #region -------------------- NORMALIZE ----------------------------
        /// <summary>
        /// Normalizes the vector.
        /// </summary>
        public void Normalize()
        {
            // Chris M 2/4/2007
            double length = Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));
            if (length <= 0)
                return;

            X = X / length;
            Y = Y / length;
            Z = Z / length;
        }
        #endregion

        #region -------------------- CROSS --------------------------------
        /// <summary>
        /// Returns the cross product of this vector with the specified vector V
        /// </summary>
        /// <param name="V">The vector to perform a cross product against</param>
        /// <returns>A vector result from the inner product</returns>
        public Vector Cross(Vector V)
        {
            Vector Result = new Vector();
            Result.X = (Y * V.Z - Z * V.Y);
            Result.Y = (Z * V.Z - X * V.Z);
            Result.Z = (X * V.Y - Y * V.X);
            return Result;
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
            Result.Z = Z / Scalar;
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
            if (V.Z > 0) Result.Z = Z / V.Z;
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
            return X * V.X + Y * V.Y + Z * V.Z;
        }
        #endregion

        #region -------------------- IS EQUAL TO---------------------------
        /// <summary>
        /// Compares the values of each element, and if all the elements are equal, returns true.
        /// </summary>
        /// <param name="V">The vector to compare against this vector.</param>
        /// <returns>Boolean, true if all the elements have the same value.</returns>
        public bool Intersects(Vector V)
        {
            if ((X == V.X) && (Y == V.Y) && (Z == V.Z)) return true;
            return false;
        }
        /// <summary>
        /// Override  for definition of equality for vectors
        /// </summary>
        /// <param name="V">A vector to compare with</param>
        /// <returns>true if the X, Y, and Z coordinates are all equal</returns>
        public bool Equals(Vector V)
        {
            if ((X == V.X) && (Y == V.Y) && (Z == V.Z)) return true;
            return false;
        }
        /// <summary>
        /// Checks first to make sure that both objects are vectors.  If they are, 
        /// then it checks to determine whether or not the X, Y and Z values are equal.
        /// </summary>
        /// <param name="Vect">The object to test against</param>
        /// <returns></returns>
        public override bool Equals(object Vect)
        {
            if (Vect.GetType() == typeof(Vector))
            {
                Vector V = (Vector)Vect;
                if ((X == V.X) && (Y == V.Y) && (Z == V.Z)) return true;
                return false;
            }
            return false;
        }
        /// <summary>
        /// Returns the hash code.. or something
        /// </summary>
        /// <returns>A hash code I guess</returns>
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
            Result.Z = Z - V.Z;
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
            Result.Z = Z + V.Z;
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
            Result.Z = Z * Scalar;
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
            Result.Z = Z * V.Z;
            return Result;
        }
        #endregion

        #region -------------------- TO POINT -----------------------------
        /// <summary>
        /// A MapWinGIS.Point Representing the tip of the vector,
        /// where the beginning of the vector is at the origin 
        /// /// </summary>
        public MapWinGIS.Point ToMWPoint()
        {
            MapWinGIS.Point pt = new MapWinGIS.Point();
            pt.x = X;
            pt.y = Y;
            pt.Z = Z;
            return pt;
        }
        /// <summary>
        /// Assuming the vector starts at the origin of 0,0,0, this function returns
        /// a Point representing the tip of the vector.
        /// </summary>
        public Point ToPoint()
        {
            return new Point(X, Y, Z);
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
            result.Z = U.Z + V.Z;
            return result;
        }
       
        #endregion

        #region -------------------- CROSS PRODUCT ------------------------
        /// <summary>
        /// Returns the Cross Product of two vectors U and V
        /// </summary>
        /// <param name="U">Vector, the first input vector</param>
        /// <param name="V">Vector, the second input vector</param>
        /// <returns>A Vector containing the cross product of U and V</returns>
        public static Vector CrossProduct(Vector U, Vector V)
        {
            Vector Result = new Vector();
            Result.X = (U.Y * V.Z - U.Z * V.Y);
            Result.Y = (U.Z * V.X - U.X * V.Z);
            Result.Z = (U.X * V.Y - U.Y * V.X);
            return Result;
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
        public static Vector DivideVectors(Vector U, Vector V)
        {
            Vector result = new Vector();
            if (V.X > 0) result.X = U.X / V.X;
            if (V.Y > 0) result.Y = U.Y / V.Y;
            if (V.Z > 0) result.Z = U.Z / V.Z;
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
            result.Z = U.Z / Scalar;
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
            return (U.X * V.X) + (U.Y * V.Y) + (U.Z * V.Z);
        }
        /// <summary>
        /// Non-static version of taking the square distance for a vector
        /// </summary>
        /// <param name="U">The vector to find the square of the distance of</param>
        /// <returns>Double, the square of the distance</returns>
        public static double Norm2(Vector U)
        {
            double n2 = U.X * U.X + U.Y * U.Y + U.Z * U.Z;
            return n2;
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
            result.Z = U.Z * Scalar;
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
            result.Z = U.Z - V.Z;
            return result;
        }

       
        #endregion

        #endregion

        #region Static Operators

        #region -------------------- ADD VECTORS -------------------------
        /// <summary>
        /// Adds the vectors U and V using vector addition, which adds the corresponding components
        /// </summary>
        /// <param name="U">One vector to be added</param>
        /// <param name="V">A second vector to be added</param>
        /// <returns>The sum of the vectors</returns>
        public static Vector operator +(Vector U, Vector V)
        {
            Vector result;
            result.X = U.X + V.X;
            result.Y = U.Y + V.Y;
            result.Z = U.Z + V.Z;
            return result;
        }
       

        /// <summary>
        /// Tests equality
        /// </summary>
        /// <param name="U">A Vector to test</param>
        /// <param name="V">A Vector to test</param>
        /// <returns>Returns true if X, Y and Z are equal</returns>
        public static bool operator ==(Vector U, Vector V)
        {
            if (U.X == V.X && U.Y == V.Y && U.Z == V.Z) return true;
            return false;
        }
        /// <summary>
        /// Tests equality
        /// </summary>
        /// <param name="U">A Vector to test</param>
        /// <param name="V">A Vector to test</param>
        /// <returns>Returns true if X, Y and Z are equal</returns>
        public static bool operator !=(Vector U, Vector V)
        {
            if (U.X == V.X && U.Y == V.Y && U.Z == V.Z) return false;
            return true;
        }


        #endregion

        #region -------------------- CROSS PRODUCT ------------------------
        /// <summary>
        /// Returns the Cross Product of two vectors U and V
        /// </summary>
        /// <param name="U">Vector, the first input vector</param>
        /// <param name="V">Vector, the second input vector</param>
        /// <returns>A Vector containing the cross product of U and V</returns>
        public static Vector operator ^(Vector U, Vector V)
        {
            Vector Result = new Vector();
            Result.X = (U.Y * V.Z - U.Z * V.Y);
            Result.Y = (U.Z * V.X - U.X * V.Z);
            Result.Z = (U.X * V.Y - U.Y * V.X);
            return Result;
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
            if (V.Z > 0) result.Z = U.Z / V.Z;
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
            result.Z = U.Z / Scalar;
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
            return (U.X * V.X) + (U.Y * V.Y) + (U.Z * V.Z);
        }
        #endregion

        #region -------------------- MULTIPLY VECTORS --------------------
        /// <summary>
        /// Multiplies the vectors U and V using vector multiplication,
        /// which adds the corresponding components
        /// </summary>
        /// <param name="Scalar">A scalar to multpy to the vector</param>
        /// <param name="V">A vector to be multiplied</param>
        /// <returns>The scalar product for the vectors</returns>
        public static Vector operator *(double Scalar, Vector V)
        {
            Vector result;
            result.X = Scalar * V.X;
            result.Y = Scalar * V.Y;
            result.Z = Scalar * V.Z;
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
            result.Z = U.Z * Scalar;
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
            result.Z = U.Z - V.Z;
            return result;
        }

     

        #endregion

        #endregion

        #endregion

    }
}