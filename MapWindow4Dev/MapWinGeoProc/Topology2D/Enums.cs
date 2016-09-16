using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Topology2D
{
    /// <summary>
    /// Contains enumerations for Topology operations
    /// </summary>
    public static class Enums
    {
        /// <summary>
        /// Specifies which side of a vector to buffer.
        /// </summary>
        public enum BufferSide
        {
            /// <summary>
            /// The left side of the vector
            /// </summary>
            Left,
            /// <summary>
            /// The right side of the vector
            /// </summary>
            Right,
            /// <summary>
            /// Both sides of the vector
            /// </summary>
            Both
        }
    }
}
