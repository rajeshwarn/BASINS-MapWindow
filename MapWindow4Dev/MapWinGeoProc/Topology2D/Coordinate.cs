using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Topology2D
{
    /// <summary>
    /// A basic set of coordinates with no methods 
    /// This is not an actual geometry in and of itself
    /// </summary>
    public class Coordinate
    {
        #region Variables
        /// <summary>
        /// The X, or horizontal coordinate
        /// </summary>
        public double X;
        /// <summary>
        /// The Y, or vertical coordinate
        /// </summary>
        public double Y;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a (0,0) coordinate
        /// </summary>
        public Coordinate()
        {
            X = 0.0;
            Y = 0.0;
        }
        /// <summary>
        /// Creates a new instance from a MapWinGIS.Point
        /// </summary>
        /// <param name="mwPoint">Creates a new instance from a MapWinGIS.Point</param>
        public Coordinate(object mwPoint)
        {
            Coordinate nc = GeometryFactory.CreateCoordinate(mwPoint);
            X = nc.X;
            Y = nc.Y;
        }
        /// <summary>
        /// Creates a new instance of a Coordinate using doubles
        /// </summary>
        /// <param name="Xval">The X, or horizontal location</param>
        /// <param name="Yval">The Y, or vertical location</param>
        public Coordinate(double Xval, double Yval)
        {
            X = Xval;
            Y = Yval;
        }

        /// <summary>
        /// Creates a new instance of a Coordinate using a Topology2D.Point
        /// </summary>
        /// <param name="Location">A Topology2D Point</param>
        public Coordinate(Point Location)
        {
            X = Location.X;
            Y = Location.Y;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Returns a duplicate coordinate
        /// </summary>
        /// <returns>A Coordinate with the same X and Y values</returns>
        public Coordinate Copy()
        {
            return new Coordinate(X, Y);
        }
        #endregion

        /// <summary>
        /// Exports this location as the tip of a vector starting at the origin.
        /// </summary>
        /// <returns>Vector, this coordinate is the tip of the vector.</returns>
        public Vector ToVector()
        {
            return new Vector(X, Y);
        }

        /// <summary>
        /// Returns a Point with the same X, Y values as this coordinate
        /// </summary>
        /// <returns>A Point with the same X and Y values.</returns>
        public Point ToPoint()
        {
            return new Point(X, Y);
        }
        #region Operators


        /// <summary>
        /// Tests if the locations are equal
        /// </summary>
        /// <param name="obV">Either a Point or Coordinate</param>
        /// <returns>Boolean, true if the X and Y values are equal</returns>
        public override bool Equals(Object obV)
        {
            // This can't ever be null
            if (obV as Object == null) return false;
            if (obV.GetType() == typeof(Coordinate))
            {
                Coordinate V = obV as Coordinate;
                if (V.X == X && V.Y == Y) return true;
                return false;
            }
            if (obV.GetType() == typeof(Point))
            {
                Point V = obV as Point;
                if (V.X == X && V.Y == Y) return true;
                return false;
            }
            if (obV.GetType() == typeof(MapWinGIS.Point))
            {
                MapWinGIS.Point V = obV as MapWinGIS.Point;
                if (V.x == X && V.y == Y) return true;
                return false;
            }
            return false;
        }

        /// <summary>
        /// Not sure what this is all about
        /// </summary>
        /// <returns>A hash code?</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns true if X and Y coordinates are equal
        /// </summary>
        /// <param name="U">A Point to compare</param>
        /// <param name="objV">Any object to compare with the point</param>
        /// <returns>Boolean, true if the vectors are the same</returns>
        public static bool operator ==(Coordinate U, Object objV)
        {
            if(U as Object == null && objV as Object == null)
            if (objV.GetType() == typeof(Point))
            {
                Point V = objV as Point;
                if (U.X == V.X && U.Y == V.Y) return true;
                return false;
            }
            if (objV.GetType() == typeof(Coordinate))
            {
                Coordinate V = objV as Coordinate;
                if (U.X == V.X && U.Y == V.Y) return true;
                return false;
            }
            return false;
        }
        /// <summary>
        /// Returns true if X and Y coordinates are different
        /// </summary>
        /// <param name="U">A Point to compare</param>
        /// <param name="objV">An object to compare</param>
        /// <returns>Boolean, true if the vectors are the different</returns>
        public static bool operator !=(Coordinate U, Object objV)
        {
            if (objV.GetType() == typeof(Point) || objV.GetType() == typeof(Coordinate))
            {
                Point V = objV as Point;
                if (U.X != V.X || U.Y != V.Y) return true;
                return false;
            }
            if (objV.GetType() == typeof(Coordinate))
            {
                Coordinate V = objV as Coordinate;
                if (U.X != V.X || U.Y != V.Y) return true;
                return false;
            }
            return false;
        }
        #endregion
    }
    
}
