using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Pitfill
{
    /// <summary>
    /// Very large images are divided up and tracked by a collection of bricked windows called a framework.
    /// The framework is divided up into individual frames.  Like a frame, this object doesn't actually contain
    /// any of the data itself.
    /// </summary>
    public class Frame
    {
        #region Class Variables
        
        // Frame Extents and Location
        private int m_X; // The Horizontal position of the frame in the framework
        private int m_Y; // The Vertical position of frame inside framework
        private System.Drawing.Rectangle m_Rect; // the bounds of the frame relative to the whole grid
        private Framework m_Parent;
        private int m_Rank; // The distance from the nearest edge... lower rank gets evaluated first
        private StatusType m_Status;
        private bool[] m_HasDependencies;
        const int UP = 1;
        const int DOWN = 2;
        const int LEFT = 3;
        const int RIGHT = 4;
        
        #endregion
        /// <summary>
        /// The completion status of a frame.
        /// </summary>
        public enum StatusType
        {
            /// <summary>
            /// No analysis has been done on this frame yet.
            /// </summary>
            tkNotEvaluated = 0,
            /// <summary>
            /// The frame has been analyzed and has not been tagged for re-analysis.
            /// </summary>
            tkEvaluatedNoDependencies = 1,
            /// <summary>
            /// The frame has been analyzed, but needs to be re-analyzed.
            /// </summary>
            tkEvaluatedNeedsRevisiting = 2,
            /// <summary>
            /// The frame is currently being analyzed.
            /// </summary>
            tkBeingEvaluated1 = 3
        }
        /// <summary>
        /// A structure to hold X, Y values
        /// </summary>
        public struct Loc
        {
            /// <summary>
            /// Integer, The X coordinate or horizontal value of the Location
            /// </summary>
            public int X;
            /// <summary>
            /// Integer, the Y coordinate or vertical value of the location.
            /// </summary>
            public int Y;
            /// <summary>
            /// Creates a new instance of a Location structure.
            /// </summary>
            /// <param name="X_Value">integer, The X coordinate</param>
            /// <param name="Y_Value">integer, The Y coordinate</param>
            public Loc(int X_Value, int Y_Value)
            {
                X = X_Value;
                Y = Y_Value;
            }
        }


        #region Constructor
        /// <summary>
        /// Initializes a new frame using specific parameters
        /// </summary>
        /// <param name="X_value">The horizontal integer index of the frame.</param>
        /// <param name="Y_value">The vertical integer index of the frame.</param>
        /// <param name="ParentFramework">The Framework collection that will contain the new frame.</param>
        public Frame(int X_value, int Y_value, Framework ParentFramework)
        {
            m_Parent = ParentFramework;
            m_X = X_value;
            m_Y = Y_value;
            m_Status = 0;
            m_HasDependencies = new bool[5];
         
            for (int I = 1; I < 5; I++)
            {
                m_HasDependencies[I] = false;
            } 

            // ------- Set up rectangle ----------

            // To overlap by 1, each frame shifts 1 less than the entire width
            int XShift = m_Parent.FrameWidth - 1; 
            int YShift = m_Parent.FrameHeight - 1;

            // The rightmost frame is narrower, but most frames are the same width
            int width = m_Parent.FrameWidth;
            if (m_X == m_Parent.NumFramesWide - 1)
            {
                width = m_Parent.numCols - XShift * m_X;
            }
            // the bottom frame is shorter, but most frames are the same height
            int height = m_Parent.FrameHeight;
            if (m_Y == m_Parent.NumFramesTall - 1)
            {
                height = m_Parent.numRows - YShift * m_Y;
            }
            
            m_Rect = new System.Drawing.Rectangle(m_X * XShift, m_Y * YShift, width, height);
           
            // ---------- Set up Rank ---------------
            int LowestHorizontalRank = Math.Min(m_X, (Parent.NumFramesWide - 1) - m_X);
            int LowestVerticalRank = Math.Min(m_Y, (Parent.NumFramesTall - 1) - m_Y);
            m_Rank = Math.Min(LowestHorizontalRank, LowestVerticalRank);
            m_HasDependencies = new bool[5];
            if (m_X == 0) m_HasDependencies[LEFT] = true;
            if (m_Y == 0) m_HasDependencies[UP] = true;
            if (m_X == m_Parent.NumFramesWide - 1) m_HasDependencies[RIGHT] = true;
            if (m_Y == m_Parent.NumFramesTall - 1) m_HasDependencies[DOWN] = true;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the framework collection that this frame belongs to
        /// </summary>
        public Framework Parent
        {
            get
            {
                return m_Parent;
            }
        }
        /// <summary>
        /// Gets the horizontal index of the frame from the left
        /// </summary>
        public int X
        {
            get
            {
                return m_X;
            }
        }
        /// <summary>
        /// Gets the vertical index of the frame from the top
        /// </summary>
        public int Y
        {
            get
            {
                return m_Y;
            }
        }
        /// <summary>
        /// Gets the current status of this Frame:
        /// </summary>
        public StatusType Status
        {
            get
            {
                return m_Status;
            }
            set
            {
                m_Status = value;
            }
        }
        /// <summary>
        /// Gets or Sets the rank value.  A frame on the edge of an image
        /// has a rank of 0.  Otherwise it is the minimum number of frames
        /// required to reach the edge of the image.
        /// </summary>
        public int Rank
        {
            get
            {
                return m_Rank;
            }
        }
        /// <summary>
        /// Gets the boolean value indicating whether any dependencies exist for this frame.
        /// </summary>
        public bool HasDependencies
        {
            get
            {
                for (int I = 1; I < 5; I++)
                {
                    if (m_HasDependencies[I] == true) return true;
                }
                return false;
            }
        }
        
            #region Rectangle Properties
            /// <summary>
            /// Gets a System.Drawing.Rectangle representing the row and column placement and extent of the frame
            /// </summary>
            public System.Drawing.Rectangle Rectangle
        {
            get
            {
                return m_Rect;
            }
        }
        /// <summary>
        /// Gets an integer representing the first grid column included in the frame
        /// </summary>
        public int Left
        {
            get
            {
                return m_Rect.Left;
            }
        }
        /// <summary>
        /// Gets an integer representing the first grid row included in the frame
        /// </summary>
        public int Top
        {
            get
            {
                return m_Rect.Top;
            }   
        }
        /// <summary>
        /// Gets an integer representing the cell width of the frame
        /// </summary>
        public int Width
        {
            get
            {
                return m_Rect.Width;
            }
        }
        /// <summary>
        /// Gets an integer representing the cell height of the frame
        /// </summary>
        public int Height
        {
            get
            {
                return m_Rect.Height;
            }
        }
        #endregion

        #endregion

        #region Dependency Functions
        /// <summary>
        /// This will return a list of locations relative to this frame where values have 
        /// been altered and we need to run the "Fill Dependencies" algorithm again.
        /// </summary>
        /// <returns>A List of Frame.Loc structures</returns>
        public List<Loc> get_Dependencies()
        {
            List<Loc> Deps = new List<Loc>();
            // ---------- UP ------------------------
            if (m_HasDependencies[UP] == true)
            {
                byte[] buffer = Read_Horizontal(UP);
                for (int col = 0; col < m_Rect.Width; col++)
                {
                    if (buffer[col] == (byte)1)
                    {
                        Deps.Add(new Loc(col, 0));
                    }
                }
            }
            // ------------------- DOWN ------------------
            if (m_HasDependencies[DOWN] == true)
            {
                byte[] buffer = Read_Horizontal(DOWN);
                for (int col = 0; col < m_Rect.Width; col++)
                {
                    if (buffer[col] == (byte)2)
                    {
                        Deps.Add(new Loc(col, m_Rect.Height-1));
                    }
                }
            }
            // ------------------- LEFT ---------------------
            if (m_HasDependencies[LEFT] == true)
            {
                byte[] buffer = Read_Vertical(LEFT);
                for (int row = 0; row < m_Rect.Height; row++)
                {
                    if (buffer[row] == (byte)3)
                    {
                        Deps.Add(new Loc(0, row));
                    }
                }
            }
            // ------------------- RIGHT ---------------------
            if (m_HasDependencies[RIGHT] == true)
            {
                byte[] buffer = Read_Vertical(RIGHT);
                for (int row = 0; row < m_Rect.Height; row++)
                {
                    if (buffer[row] == (byte)4)
                    {
                        Deps.Add(new Loc(m_Rect.Width-1, row));
                    }
                }
            }
            return Deps;
        }
        /// <summary>
        /// Writes the dependencies to a file.
        /// </summary>
        /// <param name="Values">byte[] direction of specific dependencies relative to the frame
        /// - 1 North, 2 South, 3 West, 4 East</param>
        /// <param name="Direction">Integer direction of values relative to the frame
        /// - 1 North, 2 South, 3 West, 4 East</param>
        public void Write_Dependencies(byte[] Values, int Direction)
        {
            if (Direction == UP || Direction == DOWN) Write_Horizontal(Values, Direction);
            if (Direction == LEFT || Direction == RIGHT) Write_Vertical(Values, Direction);
        }
        /// <summary>
        /// If dependencies exist in a direction, this is where we tell the neighboring
        /// frames that they have new dependencies to take into consideration.
        /// </summary>
        /// <param name="DepsExist">bool array, true if dependencies exist in that direction</param>
        public void Update_Neighbor_Status(bool[] DepsExist)
        {
            if(DepsExist[UP] == true && Y > 0)
            {
                m_Parent.set_Status(X, Y - 1, DOWN, StatusType.tkEvaluatedNeedsRevisiting);
            }
            if (DepsExist[DOWN] == true && Y < m_Parent.NumFramesTall - 1 )
            {
                m_Parent.set_Status(X, Y + 1, UP, StatusType.tkEvaluatedNeedsRevisiting);
            }
            if (DepsExist[LEFT] == true && X > 0)
            {
                m_Parent.set_Status(X - 1, Y, RIGHT, StatusType.tkEvaluatedNeedsRevisiting);
            }
            if (DepsExist[RIGHT] == true && X < m_Parent.NumFramesWide - 1)
            {
                m_Parent.set_Status(X + 1, Y, LEFT, StatusType.tkEvaluatedNeedsRevisiting);
            }
        }
        /// <summary>
        /// Sets the boolean characteristic of having dependencies in a specific direction to 
        /// the value specified by val.
        /// </summary>
        /// <param name="Direction">The direction relative to this frame
        /// -1 North, 2 South, 3 West, 4 East</param>
        /// <param name="val">Boolean, true if the frame in Direction depends on changes made in this frame.</param>
        public void Set_HasDependencies(int Direction, bool val)
        {
            m_HasDependencies[Direction] = val;
        }
        /// <summary>
        /// Immediately after evaluating a frame, it won't have any dependencies so we can clear them
        /// </summary>
        public void Clear_Dependencies()
        {
            for (int I = 1; I < 5; I++)
            {
                m_HasDependencies[I] = false;
            }
        }
        #endregion

        #region Dependency File Handling Functions
        // Accesses the file of dependencies for vertically stacked blocks
        private byte[] Read_Horizontal(int Direction)
        {
            int FileRow = m_Y;
            if (Direction == DOWN) FileRow += 1;
            long Offset = FileRow * Parent.numCols + m_Rect.Left;
            int BufferLength = m_Rect.Width;
            byte[] Buffer = new byte[BufferLength];
            System.IO.FileStream fs = new System.IO.FileStream(Parent.TempFileHZ, System.IO.FileMode.Open);
            try
            {
                System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                fs.Seek(Offset, System.IO.SeekOrigin.Begin);
                fs.Read(Buffer, 0, BufferLength);
            }
            finally
            {
                fs.Close();
            }
            return Buffer;
        }

        // File is set up sideways.  Columns become 
        private byte[] Read_Vertical(int Direction)
        {
            int FileRow = m_X;
            if (Direction == RIGHT) FileRow+=1;
            long Offset = FileRow * Parent.numRows + m_Rect.Top;
            int BufferLength = m_Rect.Height;
            byte[] Buffer = new byte[BufferLength];
            System.IO.FileStream fs = new System.IO.FileStream(Parent.TempFileVT, System.IO.FileMode.Open);
            try
            {
                System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                fs.Seek(Offset, System.IO.SeekOrigin.Begin);
                fs.Read(Buffer, 0, BufferLength);
            }
            finally
            {
                fs.Close();
            }
            return Buffer;
        }

        // Writes an array of bytes to the horizontal temp file
        private void Write_Horizontal(byte[] Values, int Direction)
        {
            System.IO.FileStream fs = new System.IO.FileStream(Parent.TempFileHZ, System.IO.FileMode.Open);
            int FileRow = m_Y;
            if (Direction == DOWN) FileRow += 1;
            long Offset = FileRow * Parent.numCols + m_Rect.Left;
            try
            {
                System.IO.BinaryWriter br = new System.IO.BinaryWriter(fs);
                fs.Seek(Offset, System.IO.SeekOrigin.Begin);
                fs.Write(Values, 0, m_Rect.Width);
            }
            finally
            {
                fs.Close();
            }
            return;
        }//End Write_Horizontal

        // File is set up sideways.
        // Saves the dependencies that represent columns shared between adjacent blocks.
        private void Write_Vertical(byte[] Values, int Direction)
        {
            int FileRow = m_X;
            if (Direction == RIGHT) FileRow += 1;
            long Offset = FileRow * Parent.numRows + m_Rect.Top;
            System.IO.FileStream fs = new System.IO.FileStream(Parent.TempFileVT, System.IO.FileMode.Open);
            try
            {
                System.IO.BinaryWriter br = new System.IO.BinaryWriter(fs);
                fs.Seek(Offset, System.IO.SeekOrigin.Begin);
                fs.Write(Values, 0, m_Rect.Height);
            }
            finally
            {
                fs.Close();
            }
            return;
        }//End Write_Vertical
        #endregion

        #region Array - Grid Handling

        /// <summary>
        /// Converts a region of a grid into a 2D array of float values
        /// </summary>
        /// <param name="mwSourceGrid">The grid to read</param>
        /// <param name="ICallBack">A MapWinGIS.ICallback for status messages</param>
        /// <returns>float[][] the values from the grid</returns>
        public float[][] GetArrayFromWindow(MapWinGIS.Grid mwSourceGrid, MapWinGIS.ICallback ICallBack)
        {
            if (mwSourceGrid == null)
            {
                if (ICallBack != null) ICallBack.Error("Error", "SourceGrid cannot be null.");
                throw new ArgumentException("SourceGrid cannot be null.");
            }

            int numRows = this.Rectangle.Height;
            int numCols = this.Rectangle.Width;

            // Populate the Array by directly reading values from the grid
            float[][] SourceArray = new float[numRows][];
            float[] Temp = new float[numRows * numCols];

            // Read from the ocx once, rather than calling several times
            if (ICallBack != null) ICallBack.Progress("Status", 0, " Loading Values");
            mwSourceGrid.GetFloatWindow(this.Rectangle.Y, this.Rectangle.Bottom - 1, this.Rectangle.X, this.Rectangle.Right - 1, ref Temp[0]);
            if (ICallBack != null) ICallBack.Progress("Status", 0, "Copying Values");
            for (int row = 0; row < numRows; row++)
            {
                SourceArray[row] = new float[numCols];
                // 
                for (int col = 0; col < numCols; col++)
                {
                    // copy the values into a 2D style array because it improves
                    // handling speeds for some reason
                    SourceArray[row][col] = Temp[row * numCols + col];
                }
            }
            if (ICallBack != null) ICallBack.Progress("Status", 0, "Finished copying values from grid.");
            return SourceArray;

        }// End GetArrayFromWindow

        /// <summary>
        /// Returns jagged array where the borders are read from the destination file but the central
        /// portion is set to be the maximum float value.
        /// </summary>
        /// <param name="mwSourceGrid">A MapWinGIS.Grid to read the frame borders that are on the very outside of the image</param>
        /// <param name="mwDestGrid">A MapWinGIS.Grid to read the frame borders on the interior edges already processed </param>
        /// <param name="ICallBack">A MapWinGIS.ICallback for messages (optional)</param>
        /// <returns>A float[][] array representing the destination values for an entire frame</returns>
        public float[][] GetBorders(MapWinGIS.Grid mwSourceGrid, MapWinGIS.Grid mwDestGrid, MapWinGIS.ICallback ICallBack)
        {
            if (mwSourceGrid == null || mwDestGrid == null)
            {
                if (ICallBack != null) ICallBack.Error("Error", "SourceGrid cannot be null.");
                throw new ArgumentException("SourceGrid cannot be null.");
            }

            int numRows = this.Rectangle.Height;
            int numCols = this.Rectangle.Width;

            // Populate the Array by directly reading values from the grid
            float[][] SourceArray = new float[numRows][];
            
            
            // Read from the ocx once, rather than calling several times
           
           
           
            // Initialize the array to max values
            for (int row = 0; row < numRows; row++)
            {
                SourceArray[row] = new float[numCols];
                for (int col = 0; col < numCols; col++)
                {
                   SourceArray[row][col] = float.MaxValue;    
                }
            }

            // If we need a dependency in a given direction, read it from the file.
            if (Y == 0)
            {
                mwSourceGrid.GetFloatWindow(this.Rectangle.Y, this.Rectangle.Y, this.Rectangle.X, this.Rectangle.Right - 1, ref SourceArray[0][0]);
            }
            else if (Parent.First_Time(X, Y - 1)== false)
            {
                mwDestGrid.GetFloatWindow(this.Rectangle.Y, this.Rectangle.Y, this.Rectangle.X, this.Rectangle.Right - 1, ref SourceArray[0][0]);
            }

            if (Y == Parent.NumFramesTall - 1)
            {
                mwSourceGrid.GetFloatWindow(this.Rectangle.Bottom - 1, this.Rectangle.Bottom - 1, this.Rectangle.X, this.Rectangle.Right - 1, ref  SourceArray[numRows - 1][0]);
            }
            else if (Parent.First_Time(X, Y + 1)==false)
            {
               mwDestGrid.GetFloatWindow(this.Rectangle.Bottom - 1, this.Rectangle.Bottom - 1, this.Rectangle.X, this.Rectangle.Right - 1, ref  SourceArray[numRows - 1][0]);
            }

            if (X==0)
            {
                float[] Temp = new float[numRows];
                mwSourceGrid.GetFloatWindow(this.Rectangle.Y, this.Rectangle.Bottom - 1, this.Rectangle.X, this.Rectangle.X, ref Temp[0]);
                for (int row = 0; row < numRows; row++)
                {
                    SourceArray[row][0] = Temp[row];
                }
            }
            else if (Parent.First_Time(X - 1, Y)==false)
            {
                float[] Temp = new float[numRows];
                mwDestGrid.GetFloatWindow(this.Rectangle.Y, this.Rectangle.Bottom - 1, this.Rectangle.X, this.Rectangle.X, ref Temp[0]);
                for (int row = 0; row < numRows; row++)
                {
                    SourceArray[row][0] = Temp[row];
                }
            }

            if (X == Parent.NumFramesWide - 1)
            {
                float[] Temp = new float[numRows];
                mwSourceGrid.GetFloatWindow(this.Rectangle.Y, this.Rectangle.Bottom - 1, this.Rectangle.Right - 1, this.Rectangle.Right - 1, ref Temp[0]);
                for (int row = 0; row < numRows; row++)
                {
                    SourceArray[row][numCols-1] = Temp[row];
                }
            }
            else if (Parent.First_Time(X + 1, Y)==false)
            {
                float[] Temp = new float[numRows];
                mwDestGrid.GetFloatWindow(this.Rectangle.Y, this.Rectangle.Bottom - 1, this.Rectangle.Right - 1, this.Rectangle.Right - 1, ref Temp[0]);
                for (int row = 0; row < numRows; row++)
                {
                    SourceArray[row][numCols - 1] = Temp[row];
                }
            }
            if (ICallBack != null) ICallBack.Progress("Status", 0, "Finished copying values from grid.");
            return SourceArray;

        }

        /// <summary>
        /// This function copies a two dimensional array of float values to the appropriate space in the grid.
        /// </summary>
        /// <param name="OutputArray">The two dimensional array of floats to save to the grid</param>
        /// <param name="mwGrid">The MapWinGIS.grid to save the float values to</param>
        /// <param name="ICallBack">A MapWinGIS.ICallback for status messages</param>
        public void SaveArrayToWindow(float[][] OutputArray, MapWinGIS.Grid mwGrid, MapWinGIS.ICallback ICallBack)
        {

            // Write values to the grid
            int numCols = this.Rectangle.Width;
            int numRows = this.Rectangle.Height;
            float[] Temp = new float[numCols * numRows];
            //Organize the OutputArray into a single window
            if (ICallBack != null) ICallBack.Progress("Status", 0, "Copying value to 1-D array.");
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    Temp[numCols * row + col] = OutputArray[row][col];
                }
            }
            if (ICallBack != null) ICallBack.Progress("Status", 0, "Saving window to grid");
            mwGrid.PutFloatWindow(this.Rectangle.Y, this.Rectangle.Bottom - 1, this.Rectangle.X, this.Rectangle.Right - 1, ref Temp[0]);
            return;
        }
        #endregion
    }
}
