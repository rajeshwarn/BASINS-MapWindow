using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Pitfill
{
    /// <summary>
    /// This class is a collection object for all the frames that cover a specific grid.
    /// It contains information about the positions of frames as well as functions for
    /// iterating through frames.
    /// </summary>
    public class Framework
    {
        #region Class Variables
        // ------- Frame related Variables
        int m_numCols;
        int m_numRows;
        int m_FrameWidth;
        int m_FrameHeight;
        int m_NumFramesWide;
        int m_NumFramesTall;
        int m_MaxRank;
        bool m_FisrtTimeThrough;
        
        const int UP = 1;
        const int DOWN = 2;
        const int LEFT = 3;
        const int RIGHT = 4;
        Frame[,] Frames;
        bool[,] m_FirstTime;
        List<Frame> m_FrameList;
        int m_FrameIndex;
        // ------- Dependency related Variables
        string m_DependenciesFile_HZ;
        string m_DependenciesFile_VT;
        
        #endregion

        #region Constructor
        /// <summary>
        /// Constructs a new Framework to keep track of the multiple frames that make up an image.
        /// </summary>
        /// <param name="NumberCols">The number of rows in the entire image</param>
        /// <param name="NumberRows">the number of columns in the entire image</param>
        /// <param name="FrameWidth">The maximum number of columns for each frame</param>
        /// <param name="FrameHeight">The maximum number of rows for each frame</param>
        public Framework(int NumberCols, int NumberRows, int FrameWidth, int FrameHeight)
        {
            m_FisrtTimeThrough = true;
            m_numCols = NumberCols;
            m_numRows = NumberRows;
            m_FrameWidth = FrameWidth;
            m_FrameHeight = FrameHeight;
            m_NumFramesWide = (int)Math.Ceiling((decimal)m_numCols/(decimal)m_FrameWidth);
            m_NumFramesTall = (int)Math.Ceiling((decimal)m_numRows/(decimal)m_FrameHeight);
            Frames = new Frame[m_NumFramesWide, m_NumFramesTall];
            m_FirstTime = new bool[m_NumFramesWide, m_NumFramesTall];
            m_MaxRank = 0;
            m_FrameList = new List<Frame>();
            for (int Y = 0; Y < m_NumFramesTall; Y++)
            {
                for (int X = 0; X < m_NumFramesWide; X++)
                {
                    m_FirstTime[X, Y] = true;
                    Frames[X, Y] = new Frame(X, Y, this);
                    if (Frames[X, Y].Rank > m_MaxRank) m_MaxRank = Frames[X, Y].Rank;
                    // Create a sequential list of frames based on rank
                    bool added_Value = false;
                    for (int T = 0; T < m_FrameList.Count; T++)
                    {
                        if (m_FrameList[T].Rank > Frames[X, Y].Rank)
                        {
                            //insert into the field based on rank
                            m_FrameList.Insert(T, Frames[X, Y]);
                            added_Value = true;
                            break;
                        }
                    }
                    if (added_Value == false)
                    {
                        m_FrameList.Add(Frames[X, Y]);
                    }
                }
            }

        }

        #endregion

        #region Public Properties
       
        /// <summary>
        /// The number of rowas in the entire grid
        /// </summary>
        public int numRows
        {
            get
            {
                return m_numRows;
            }
        }
        /// <summary>
        /// The number of columns in the entire grid
        /// </summary>
        public int numCols
        {
            get
            {
                return m_numCols;
            }
        }
        /// <summary>
        /// The maximum number of columms to devote to an individual frame
        /// </summary>
        public int FrameWidth
        {
            get
            {
                return m_FrameWidth;
            }
        }
        /// <summary>
        /// The maximum number of rows to devote to an individual frame
        /// </summary>
        public int FrameHeight
        {
            get
            {
                return m_FrameHeight;
            }
        }
        /// <summary>
        /// The entire grid is divided into this number of frames vertically.
        /// </summary>
        public int NumFramesTall
        {
            get
            {
                return m_NumFramesTall;
            }
        }
        /// <summary>
        /// The entire grid is divided into this number of frames horizontally.
        /// </summary>
        public int NumFramesWide
        {
            get
            {
                return m_NumFramesWide;
            }
        }
        /// <summary>
        /// This is the temporary file to write the horizontal dependencies to.
        /// </summary>
        public string TempFileHZ
        {
            get
            {
                return m_DependenciesFile_HZ;
            }
        }
        /// <summary>
        /// This is the temporary file to write vertical dependencies to.
        /// </summary>
        public string TempFileVT
        {
            get
            {
                return m_DependenciesFile_VT;
            }
        }
        /// <summary>
        /// This is the function to obtain a specific frame based on its location in the framework.
        /// </summary>
        /// <param name="X">The horrizontal offset in frames (not pixels)</param>
        /// <param name="Y">The vertical offset in frames (not pixels)</param>
        /// <returns>A Pitfill.Frame</returns>
        public Frame get_Frame(int X, int Y)
        {
            return Frames[X, Y];
        }

        /// <summary>
        /// First time through
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public bool First_Time(int X, int Y)
        {
            return m_FirstTime[X, Y];
        }
        /// <summary>
        /// Sets first time through
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="value"></param>
        public void set_First_Time(int X, int Y, bool value)
        {
            m_FirstTime[X, Y] = value;
        }
        /// <summary>
        /// Returns the next frame in the list, or the first frame once
        /// the end of the list is reached.
        /// </summary>
        /// <returns>the next frame.</returns>
        public Frame Next_Frame()
        {
            m_FrameIndex++;
            if(m_FrameIndex == m_FrameList.Count)
            {
                m_FrameIndex = 0;
                m_FisrtTimeThrough = false;
            }
            return m_FrameList[m_FrameIndex];
            
        }
        /// <summary>
        /// Returns the first frame in the list
        /// </summary>
        /// <returns>The first frame in the list.</returns>
        public Frame First_Frame()
        {
            m_FrameIndex = 0;
            return m_FrameList[0];
        }
        /// <summary>
        /// Assigns a particular status to a frame based on its location in the framework.
        /// The Direction allows the appropriate "HasDependencies" 
        /// </summary>
        /// <param name="X">The horizontal offset in frames (not pixels).</param>
        /// <param name="Y">The vertical offset in frames (not pixels)</param>
        /// <param name="Direction">Sets the direction for HasDependencies to be changed.</param>
        /// <param name="Status">The new status of the frame</param>
        /// <remarks>This is generally not used to </remarks>
        public void set_Status(int X, int Y, int Direction, Frame.StatusType Status)
        {
            Frames[X, Y].Status = Status;
            if (Status == Frame.StatusType.tkEvaluatedNeedsRevisiting)
            {
                Frames[X, Y].Set_HasDependencies(Direction, true);
            }
            else if (Status == Frame.StatusType.tkEvaluatedNoDependencies)
            {
                Frames[X, Y].Set_HasDependencies(Direction, false);
            }
        }

        /// <summary>
        /// Gets a boolean value indicating whether any frames exist in the framework
        /// </summary>
        public bool HasFrames
        {
            get
            {
                if (Frames == null) return false;
                return true;
            }
          
        }
        /// <summary>
        /// Gets a boolean value indicating whether or not any dependencies exist
        /// </summary>
        public bool HasDependencies
        {
            get
            {
                if (m_FisrtTimeThrough == true) return true;
                for (int Y = 0; Y < NumFramesTall; Y++)
                {
                    for (int X = 0; X < NumFramesWide; X++)
                    {
                        if (Frames[X, Y].HasDependencies) return true;
                    }
                }
                // only return false if NONE of the frames has any dependencies left.
                return false;
            }
        }
        #endregion

        #region DependencyFilehandling
        /// <summary>
        /// Will create a pair of temporary binary files with names similar to the destination file.
        /// The file named HZ stores dependencies in the rows that are shared by vertically stacked frames
        /// The file named VT stores dependencies in the columns shared by adjacent frames
        /// </summary>
        /// <param name="DestFile">The string filename of the filled output grid being created.</param>
        public void InitDependencyFiles(string DestFile)
        {
            m_DependenciesFile_HZ = DestFile.Substring(0, DestFile.Length - 4);
            m_DependenciesFile_HZ += "HZ.bin";
            m_DependenciesFile_VT = DestFile.Substring(0, DestFile.Length - 4);
            m_DependenciesFile_VT += "VT.bin";
            //Initially, no dependencies exist, so we can create our output files with 0 values.
            Create_Horizontal_File();
            Create_Vertical_File();
        }
        // Initializes a file showing where a frame depends on a frame above or below it
        private void Create_Horizontal_File()
        {
            if (System.IO.File.Exists(m_DependenciesFile_HZ)) System.IO.File.Delete(m_DependenciesFile_HZ);
            System.IO.FileStream fs = new System.IO.FileStream(m_DependenciesFile_HZ, System.IO.FileMode.CreateNew);
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs);
            for (int row = 0; row < NumFramesTall+1; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    // Our initial top grids depend on the top row
                    if(row==0)
                    {
                        bw.Write((byte)1);
                        continue;
                    }
                    // Our intial bottom grids depend on the bottom row
                    if (row == NumFramesTall)
                    {
                        bw.Write((byte)2);
                        continue;
                    }
                    // Initially no other dependencies exist for this file
                    bw.Write((byte)0);
                }
            }
            bw.Close();
            fs.Close();
        }
        // Initializes a file showing where a frame depends on the frame to the left or right of it
        private void Create_Vertical_File()
        {
            if (System.IO.File.Exists(m_DependenciesFile_VT)) System.IO.File.Delete(m_DependenciesFile_VT);
            System.IO.FileStream fs = new System.IO.FileStream(m_DependenciesFile_VT, System.IO.FileMode.CreateNew);
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs);
            // Effectively we are writing the file sideways for speed. Columns end up written to the file as rows.
            for (int row = 0; row < NumFramesWide + 1; row++)
            {
                for (int col = 0; col < numRows; col++) 
                {
                   
                     // Our initial Left grids depend on the Left Column, which is the first row in the temp file
                    if(row==0)
                    {
                        bw.Write((byte)3);
                        continue;
                    }
                    // Our intial right grids depend on the right column, which is the last row in the temp file
                    if (row == NumFramesWide)
                    {
                        bw.Write((byte)4);
                        continue;
                    }
                    // Initially no other dependencies exist for this file
                    bw.Write((byte)0);
                }
            }
            bw.Close();
            fs.Close();
        }
        /// <summary>
        /// Remove temporary binary files for storing dependencies
        /// </summary>
        public void DeleteFiles()
        {
            System.IO.File.Delete(m_DependenciesFile_HZ);
            System.IO.File.Delete(m_DependenciesFile_VT);
        }
        #endregion


    }
}
