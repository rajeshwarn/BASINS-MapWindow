using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using MapWinGeoProc;
using MapWindow.Interfaces.Geometries;
using MapWindow.Interfaces.Raster;
using MapWinGeoProc.NTS.Topology.Geometries;
namespace MapWinGeoProc.Compatibility
{
    /// <summary>
    /// Wrap any calls to this in a try block to catch exceptions.
    /// Errors occuring in Methods without a boolean return type
    /// will throw exceptions if there is an error.
    /// </summary>
    /// <remarks>
    /// When MapWinExceptions are generated, they automatically register
    /// the contents of the error message with the logger.  Knowing that
    /// Raster object was involved may be useful, so that part is logged
    /// here.  The exception should be wrapped in a try-block so that
    /// the exception can be logged from the calling function.
    /// </remarks>
    public class Raster : MapWindow.Interfaces.Raster.IRaster, IDisposable
    {
        private MapWinGIS.Grid m_Grid; // Internal reference to a MapWinGIS Grid object

        private MapWinGIS.ICallback m_ICallBack; // Internal reference to a MapWinGIS ICallback

        private bool m_Disposed; // For handling disposal 

        /// <summary>
        /// Creates a new instance of the Raster wrapper for the grid
        /// </summary>
        Raster()
        {
            m_Grid = new MapWinGIS.Grid();
        }

        /// <summary>
        /// Creates a Raster from a specific instance of an m_Grid
        /// </summary>
        /// <param name="MapWinGIS_Grid">MapWinGIS.Grid to create a Raster from</param>
        Raster(MapWinGIS.Grid MapWinGIS_Grid)
        {
            m_Grid = MapWinGIS_Grid;
        }

        /// <summary>
        /// The default indexing of a raster specifies the column and row
        /// values for accessing a particular value.
        /// </summary>
        /// <param name="Column"></param>
        /// <param name="Row"></param>
        /// <returns></returns>
        public object this[int Column, int Row]
        {
            get
            {
                return m_Grid.get_Value(Column, Row);
            }
            set
            {
                m_Grid.set_Value(Column, Row, value);
            }
        }

        #region IRaster Members
        /// <summary>
        /// Not Implemented
        /// </summary>
        public int BandCount
        {
            get
            {
                MapWinUtility.Logger.Dbg("The BandCount member of the Raster class was called but is not implemented.");
                throw new NotImplementedException("The method or operation is not implemented.");
            }
        }
        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <returns>Throws NotImplementedException</returns>
        public bool CanCreate
        {
            get
            {
                MapWinUtility.Logger.Dbg("The CanCreate member of the Raster class was called but is not implemented.");
                throw new NotImplementedException("The method or operation is not implemented.");
            }
        }
        /// <summary>
        /// Given a Row and Column, it figures out the georeferenced X, Y location
        /// corresponding to the center of that grid cell.
        /// </summary>
        /// <param name="Column">Integer, Column of the grid cell to get the coordinates of</param>
        /// <param name="Row">Integer, Row of the grid cell to get the coordinates of</param>
        /// <param name="x">The double value of the horizontal coordinate</param>
        /// <param name="y">The double value of the vertical coordinate</param>
        public void CellToProj(int Column, int Row, ref double x, ref double y)
        {
            m_Grid.CellToProj(Column, Row, out x, out y);
        }
        /// <summary>
        /// Finds the X-coordinate or Longitude corresponding to the specified Column.
        /// This uses XllCenter + dX * Column.
        /// </summary>
        /// <param name="Column">The Integer column to find the X-coordinate of</param>
        public double CellToProjX(int Column)
        {
            return m_Grid.Header.XllCenter + m_Grid.Header.dX * Column;
        }
        /// <summary>
        /// Finds the Y-coordiante or Longitude corresponding to the specified Row.
        /// This uses YllCenter + dY * (NumRows - Row);
        /// </summary>
        /// <param name="Row">The integer column to find the Y-coordinate of</param>
        public double CellToProjY(int Row)
        {
            return m_Grid.Header.YllCenter + m_Grid.Header.dY * (m_Grid.Header.NumberCols - Row);
        }
        /// <summary>
        /// This calls the MapWinGIS.Clear(value) method.
        /// </summary>
        /// <param name="value"></param>
        public void Clear(double value)
        {
            if (m_Grid.Clear(value) == false)
            {
                MapWinUtility.Logger.Dbg("Error occured in MapWinGIS.Grid.Clear()");
                throw new MapWinGeoProc.MapWinException(m_Grid.LastErrorCode);
            }
        }
        /// <summary>
        /// Wraps MapWinGIS.Close()
        /// </summary>
        public void Close()
        {
            if (m_Grid.Close() == false)
            {
                MapWinUtility.Logger.Dbg("Error closing grid: " + m_Grid.get_ErrorMsg(m_Grid.LastErrorCode));
                throw new MapWinGeoProc.MapWinException(m_Grid.LastErrorCode);
            }
        }

        /// <summary>
        /// Creates a new instance of a MapWinGIS Grid according to the parameters required by IRaster
        /// </summary>
        /// <param name="Filename">The string full path filename of the grid</param>
        /// <param name="newFileType">A MapWindow.Interfaces.Type.GridFileType specifying a file format</param>
        /// <param name="dX">Double, specifies the cell width of a single "pixel" for the grid</param>
        /// <param name="dY">Double, specifies the cell height of a single pixel of the grid</param>
        /// <param name="xllCenter">Double, The longitude/X-coordinate of the lower left pixel in the grid</param>
        /// <param name="yllCenter">Double, The latitude/Y-coordinate of the lower left pixel of the grid</param>
        /// <param name="noDataVal">Double, The value to use as a no-data value in the grid</param>
        /// <param name="projection">String: the proj-4 string to use to describe the grid projection</param>
        /// <param name="nCols">Int, the number of columns in the grid</param>
        /// <param name="nrows">Int, the number of rows in the grid</param>
        /// <param name="DataType">A MapWindow.Interfaces.Type.GridDataType that specifies the numeric data format</param>
        /// <param name="CreateINRam">Boolean, if true, the entire element will be created in ram</param>
        /// <param name="initialValue">The intial value for the grid</param>
        /// <param name="applyinitialValue">I'm not sure this is an option in the old grid</param>
        public void CreateNew(string Filename, GridFileType newFileType, double dX, double dY, double xllCenter, double yllCenter, double noDataVal, string projection, int nCols, int nrows, GridDataType DataType, bool CreateINRam, double initialValue, bool applyinitialValue)
        {
            MapWinGIS.GridHeader gh = new MapWinGIS.GridHeader();
            gh.dX = dX;
            gh.dY = dY;
            gh.XllCenter = xllCenter;
            gh.YllCenter = yllCenter;
            gh.Projection = projection;
            gh.NumberCols = nCols;
            gh.NumberRows = nrows;
            gh.NodataValue = noDataVal;

            if (m_Grid.CreateNew(Filename, gh, MapWinGeoProc.Compatibility.Convert.mwGridDataType(DataType),
                    initialValue, CreateINRam, MapWinGeoProc.Compatibility.Convert.mwGridFileType(newFileType), null) == false)
            {
                MapWinUtility.Logger.Dbg("Error calling CreateNew in MapWinGIS.Grid");
                throw new MapWinException(m_Grid.LastErrorCode);
            }

        }
        /// <summary>
        /// Returns the m_Grid.DataType, converted into a new 
        /// MapWindow.Interfaces.Types.GridDataType
        /// </summary>
        public GridDataType DataType
        {
            get
            {
                return Convert.GetGridDataType(m_Grid.DataType);
            }
        }

        /// <summary>
        /// Returns the MapWinGIS.CdlgFilter
        /// </summary>
        public string DialogFilter
        {
            get
            {
                return m_Grid.CdlgFilter;
            }
        }

        /// <summary>
        /// Releases com objects
        /// </summary>
        public void Dispose()
        {
            this.Dispose(m_Disposed);
        }

        /// <summary>
        /// Gets or sets the Grid.Header.Dx
        /// </summary>
        public double Dx
        {
            get
            {
                return m_Grid.Header.dX;
            }
            set
            {
                m_Grid.Header.dX = value;
            }
        }
        /// <summary>
        /// Gets or sets the Grid.Header.Dx
        /// </summary>
        public double Dy
        {
            get
            {
                return m_Grid.Header.dY;
            }
            set
            {
                m_Grid.Header.dY = value;
            }
        }
        /// <summary>
        /// Gets an Envelope 
        /// </summary>
        public IEnvelope Envelope
        {
            get
            {
                double Left = m_Grid.Header.XllCenter - m_Grid.Header.dX / 2;
                double Width = m_Grid.Header.NumberCols * m_Grid.Header.dX;
                double Right = Left + Width;



                double Bottom = m_Grid.Header.YllCenter - m_Grid.Header.dY / 2;
                double Height = m_Grid.Header.NumberRows * m_Grid.Header.dY;
                double Top = Bottom + Height;

                // the Envelope will automatically make the larger values the maximum
                NTS.Topology.Geometries.Envelope mEnvelope = new Envelope();
                mEnvelope.MinX = Left;
                mEnvelope.MaxX = Right;
                mEnvelope.MinY = Bottom;
                mEnvelope.MaxY = Top;
                return mEnvelope;
            }
        }
        /// <summary>
        /// I don't really know that we want to code this here
        /// </summary>
        /// <returns></returns>
        public bool Equals()
        {
            throw new NotImplementedException("The Equals method is not implemented.");
        }
        /// <summary>
        /// Gets the filename currently associated with the MapWinGIS.Grid if any.
        /// </summary>
        public string Filename
        {
            get
            {
                return m_Grid.Filename;
            }
        }

        /// <summary>
        /// Returns a value like float Values[Y][X] where the block describes the window being specified
        /// </summary>
        /// <param name="StartRow">The 0 based row index corresponding to the Top (0,0 is Top, Left)</param>
        /// <param name="EndRow">The 0 based row index corresponding to the Bottom (0,0 is Top, Left)</param>
        /// <param name="StartCol">The 0 based column index corresponding to the Left (0,0 is Top, Left)</param>
        /// <param name="EndCol">The 0 based column index corresponding to the Right (0,0 is Top, Left)</param>
        /// <exception cref="System.ApplicationException">Returns an exception if there is a failure</exception>
        /// <remarks>The real return is in the Vals array that was dimensioned before calling this function.</remarks>
        public float[][] GetFloatWindow(int StartRow, int EndRow, int StartCol, int EndCol)
        {
            int X, Y;
            float[][] Vals = new float[(EndRow - StartRow) + 1][];
            float[] RowVals = new float[m_Grid.Header.NumberCols];
            Y = 0;
            for (int row = StartRow; row <= EndRow; row++)
            {
                X = 0;
                for (int col = StartCol; col < EndCol; col++)
                {
                    X++;
                    if (m_Grid.GetRow(Y, ref RowVals[0]) == false)
                    {
                        MapWinUtility.Logger.Dbg("MapWinGIS.Grid.GetRow returned an error.");
                        throw new MapWinException(m_Grid.LastErrorCode);

                    }

                    Vals[Y][X] = RowVals[col];
                }
                Y++;
            }
            return Vals;


        }

        /// <summary>
        /// Converts all the values into a jagged array.
        /// The coordinates are stored [Y][X] in the resulting array.
        /// The index values range from 0 to NumCols - 1 for X and 0 to NumRows - 1 for Y
        /// </summary>
        /// <returns>A jagged array of float values organized as array[Y][X]</returns>
        public float[][] GetFloatValues()
        {
            MapWinUtility.Logger.Dbg("GetFloatValues()");
            bool Result;
            int Prog = 0;
            int OldProg = 0;
            int NumRows, NumCols;
            NumRows = m_Grid.Header.NumberRows;
            NumCols = m_Grid.Header.NumberCols;
            float[][] Vals = new float[NumRows][];

            for (int Y = 0; Y < NumRows; Y++)
            {
                Vals[Y] = new float[NumCols];
                Result = m_Grid.GetRow(Y, ref Vals[Y][0]);
                if (Result == false)
                {
                    MapWinUtility.Logger.Dbg("MapWinGIS.Grid.GetRow(" + Y.ToString() + ") threw an error.");
                    throw new MapWinException(m_Grid.LastErrorCode);
                }
                Prog = (Y * 100) / NumRows;
                if (Prog > OldProg)
                {
                    MapWinUtility.Logger.Progress("Getting Float Values..." + Prog.ToString() + "% Complete", Prog, OldProg);
                    if (m_ICallBack != null) m_ICallBack.Progress("Progress", Prog, "Getting Float Values..." + Prog.ToString() + "% Complete");
                    OldProg = Prog;
                }

            }
            if (m_ICallBack != null) m_ICallBack.Progress("Progress", 0, "");
            MapWinUtility.Logger.Progress("", 0, 0);
            MapWinUtility.Logger.Dbg("Finished GetFloatValues.");
            return Vals;
        }
        /// <summary>
        /// Returns a one dimensional array of float values corresponding to a single row.
        /// The array should be dimensioned to the Number of columns in the grid.
        /// </summary>
        /// <param name="Row">The row to retrieve values from.</param>
        /// <param name="Vals">Float, the 0 index of an array of floats dimensioned with NumColumns elements</param>
        /// <returns>bool, in this case always true.  If an exception is thrown then return isn't used</returns>
        public bool GetRow(int Row, ref float Vals)
        {
            if (m_Grid.GetRow(Row, ref Vals) == false)
            {
                MapWinUtility.Logger.Dbg("MapWinGIS.Grid.GetRow(Row: " + Row.ToString() + ", Vals) threw an error.");
                throw new MapWinException(m_Grid.LastErrorCode);
            }
            return true;
        }

        /// <summary>
        /// This is another version of GetRow that dimensions the values for you and returns
        /// the appropriate result.
        /// </summary>
        /// <param name="Row">The row to retrieve values from</param>
        /// <returns>A one dimensional array of floats based on the specified window</returns>
        /// <remarks>This will throw exceptions if there is a problem, but logging every call would be too noisy</remarks>
        public float[] GetRow(int Row)
        {
            float[] vals = new float[m_Grid.Header.NumberCols];
            if (m_Grid.GetRow(Row, ref vals[0]) == false)
            {
                MapWinUtility.Logger.Dbg("MapWinGIS.Grid.GetRow(Row: " + Row.ToString() + " threw an error");
                throw new MapWinException(m_Grid.LastErrorCode);
            }
            return vals;
        }

        /// <summary>
        /// Returns a value for a specific row and column.
        /// This is VERY slow through all the interfaces and COM so
        /// use the others where possible.  This is ok if you just
        /// need to get an idea for the pixels along a line or something.
        /// </summary>
        /// <param name="Column">The grid column (horizontal index) where to obtain a value</param>
        /// <param name="Row">The grid row (vertical index) where to obtain a value</param>
        /// <returns></returns>
        public object GetValue(int Column, int Row)
        {
            return m_Grid.get_Value(Column, Row);
        }

        /// <summary>
        /// Returns a value for a specific row and column.
        /// This is VERY slow through all the interfaces and COM so
        /// use the others where possible.  This is ok if you just
        /// need to get an idea for the pixels along a line or something.
        /// </summary>
        /// <param name="X">The X coordinate of the geographic location to find the nearest cell value</param>
        /// <param name="Y">The Y coordinate of the geographic lcoation to find the nearest cell value</param>
        /// <returns>A late bound object corresponding to the appropriate type</returns>
        public object GetValue(double X, double Y)
        {
            int Row, Column;
            m_Grid.ProjToCell(X, Y, out Column, out Row);
            return m_Grid.get_Value(Column, Row);
        }



        /// <summary>
        /// Not Implemented
        /// </summary>
        public bool HasTransparency
        {
            get { throw new NotImplementedException("The method or operation is not implemented."); }
        }
        /// <summary>
        /// I am not sure if this is supposed to be the NumRows... but that is what I am returning here
        /// </summary>
        public int Height
        {
            get
            {
                return m_Grid.Header.NumberRows;
            }
        }

        /// <summary>
        /// Gets or Sets a MapWinGIS.ICallback to be used for future function calls that take an ICallback parameter.
        /// </summary>
        MapWinGIS.ICallback ICallBack
        {
            get
            {
                return m_ICallBack;
            }
            set
            {
                m_ICallBack = value;
            }
        }

        /// <summary>
        /// Gets a Boolean that is true if the grid is being stored in ram.
        /// </summary>
        public bool IsInRam
        {
            get
            {
                return m_Grid.InRam;
            }
        }
        /// <summary>
        /// Gets the string of the last error message that occured within the grid
        /// </summary>
        public string LastError
        {
            get
            {
                return m_Grid.get_ErrorMsg(m_Grid.LastErrorCode);
            }
        }
        /// <summary>
        /// Returns the maximum value from the grid.
        /// </summary>
        public double Maximum
        {
            get
            {
                return System.Convert.ToDouble(m_Grid.Maximum);
            }
        }
        /// <summary>
        /// Returns the minimum value from the grid
        /// </summary>
        public double Minimum
        {
            get
            {
                return System.Convert.ToDouble(m_Grid.Minimum);
            }
        }
        /// <summary>
        /// Gets or Sets the value that the grid should use to store no-data
        /// </summary>
        public double NoDataValue
        {
            get
            {
                return System.Convert.ToDouble(m_Grid.Header.NodataValue);
            }
            set
            {
                m_Grid.Header.NodataValue = value;
            }
        }
        /// <summary>
        /// Attempts to open a file using the specified parameters.  Given the popularity of the
        /// float data type, the grid will make an effort to open everything as a float data type.
        /// </summary>
        /// <param name="Filename">The Filename of the grid to open</param>
        /// <param name="InRam">Specifies whether or not the grid is in ram.</param>
        /// <param name="FileType">The file format as specified by a MapWindow.Interfaces.Types.GridFileType</param>
        /// <exception cref="System.ApplicationException">Returns an exception if there is a failure</exception>
        public void Open(string Filename, bool InRam, GridFileType FileType)
        {
            if (m_Grid.Open(Filename, MapWinGIS.GridDataType.FloatDataType, InRam, Convert.mwGridFileType(FileType), m_ICallBack) == false)
            {
                throw new MapWinException(m_Grid.LastErrorCode);
            }

        }
        /// <summary>
        /// Not Implemented (MapWindow5.0 capability)
        /// </summary>
        /// <param name="BandNumber"></param>
        /// <returns></returns>
        public void OpenBand(int BandNumber)
        {
            throw new NotImplementedException("The OpenBand method is not implemented.");
        }
        /// <summary>
        /// Not Implemented (MapWindow 5.0 Capability)
        /// </summary>
        public int OpenedBand
        {
            get { throw new NotImplementedException("The OpenedBand property is not implemented."); }
        }
        /// <summary>
        /// Fills the two referenced parameters (column, row) with a closest match
        /// to the double coordinates specified (x, y)
        /// </summary>
        /// <param name="x">The X-coordinate or longitude to find the nearest cell to</param>
        /// <param name="y">The Y-coordinate or latitude to find the nearest cell to</param>
        /// <param name="column">The Integer index of the grid column</param>
        /// <param name="row">The Integer index of the grid row</param>
        public void ProjToCell(double x, double y, ref int column, ref int row)
        {
            m_Grid.ProjToCell(x, y, out column, out row);
        }

        /// <summary>
        /// This returns an integer Column that corresponds to the X-coordinate
        /// This uses (X - XLLcenter)/dx;
        /// </summary>
        /// <param name="X">Double, the X-coordinate to find the column for</param>
        /// <returns>Integer, the index of the column that overlaps with the X coordinate</returns>
        /// <remarks>Uses Math.Floor((X - Left) / dX)</remarks>
        public int ProjToCellX(double X)
        {
            double Left = m_Grid.Header.XllCenter - m_Grid.Header.dX / 2;
            return (int)Math.Floor((X - Left) / m_Grid.Header.dX);
        }

        /// <summary>
        /// This returns an integer Row that corresponds to the Y-coordinate.
        /// </summary>
        /// <param name="Y">Double, the Y-coordinate to find the row for</param>
        /// <returns>The integer row corresponding to the specified Y coordinate</returns>
        /// <remarks>Uses Numrows - Floor( (Y - Bottom) / DY )</remarks>
        public int ProjToCellY(double Y)
        {
            double Bottom = m_Grid.Header.YllCenter - Math.Abs(m_Grid.Header.dY) / 2;
            return m_Grid.Header.NumberRows - (int)Math.Floor((Y - Bottom) / m_Grid.Header.dY);
        }


        /// <summary>
        /// Gets or Sets the Proj-4 string to use with this grid.
        /// I think the grid still has to have Save() called to actually write the file.
        /// </summary>
        public string Projection
        {
            get
            {
                return m_Grid.Header.Projection;
            }
            set
            {
                m_Grid.Header.Projection = value;
            }
        }
        /// <summary>
        /// An Accessor for the MapWinGIS.Grid PutFloatWindow method.
        /// </summary>
        /// <param name="Vals">A Jagged Array like float Values[Y][X] </param>
        /// <param name="StartRow">The Integer index of the Top row, inclusive</param>
        /// <param name="EndRow">The Integer index of the Bottom row, inclusive</param>
        /// <param name="StartCol">The Integer index of the Left column, inclusive</param>
        /// <param name="EndCol">The Integer index of the Right column, inclusive</param>
        /// <exception cref="System.ApplicationException">Returns an exception if there is a failure</exception>
        public void PutFloatWindow(float[][] Vals, int StartRow, int EndRow, int StartCol, int EndCol)
        {
            int X, Y = 0;
            float[] RowVals = new float[m_Grid.Header.NumberCols];
            for (int row = StartRow; row <= EndRow; row++)
            {
                if (m_Grid.GetRow(row, ref RowVals[0]) == false)
                {
                    MapWinUtility.Logger.Dbg("MapWinGIS.Grid.GetRow threw an exception on row " + row.ToString());
                    throw new MapWinException(m_Grid.LastErrorCode);
                }
                X = 0;
                for (int col = StartCol; col < EndCol; col++)
                {
                    RowVals[col] = Vals[Y][X];
                    X++;
                }
                if (m_Grid.PutRow(row, ref RowVals[0]) == false)
                {
                    MapWinUtility.Logger.Dbg("MapWinGIS.Grid.PutRow threw an exception on row " + row.ToString());
                    throw new MapWinException(m_Grid.LastErrorCode);
                }
            }
        }
        /// <summary>
        /// This version takes the first value in the float array, instead of the array.
        /// This one has no checking beforehand to ensure that your array length matches
        /// the row size and will cause an error and a false value if there is a problem.
        /// </summary>
        /// <param name="row">The Integer index of the row to save the values to</param>
        /// <param name="Vals">A referenced float value for the 0 index member of the array.</param>
        /// <returns>Bool, false if there was an error.</returns>
        public bool PutRow(int row, ref float Vals)
        {
            if (m_Grid.PutRow(row, ref Vals) == false)
            {
                MapWinUtility.Logger.Dbg("MapWinGIS.Grid.PutRow(row: " + row.ToString() + ", Vals) threw an exception: ");
                throw new MapWinException(m_Grid.LastErrorCode);

            }
            return true;
        }

        /// <summary>
        /// This is just a variant of the other PutRow that accepts the 
        /// </summary>
        /// <param name="row">The Integer row index to put values in</param>
        /// <param name="Vals">The float values to be stored in the grid</param>
        /// <remarks>This throws exceptions rather than returning boolean values.</remarks>
        public void PutRow(int row, float[] Vals)
        {
            if (m_Grid.Header.NumberCols != Vals.GetUpperBound(0) + 1)
            {
                MapWinUtility.Logger.Dbg("Application Exception: The Number of columns specified in the array doesn't match the number of columns in the grid.");
                throw new ApplicationException("The Number of columns specified in the row doesn't match the number of columns in the grid.");
            }
            if (m_Grid.PutRow(row, ref Vals[0]) == false)
            {
                MapWinUtility.Logger.Dbg("Application Exception: " + m_Grid.get_ErrorMsg(m_Grid.LastErrorCode));
                throw new ApplicationException(m_Grid.get_ErrorMsg(m_Grid.LastErrorCode));
            }
        }



        /// <summary>
        /// This wraps the set_Value function in the grid object.
        /// This is very slow, so only use this for cases where only a few points are needed.
        /// </summary>
        /// <param name="Column">The grid column for the submitted value</param>
        /// <param name="Row">The grid row for the submitted value</param>
        /// <param name="Value">The value being specified</param>
        public void PutValue(int Column, int Row, object Value)
        {
            m_Grid.set_Value(Column, Row, Value);
        }

        /// <summary>
        /// Returns a value for a specific row and column.
        /// This is VERY slow through all the interfaces and COM so
        /// use the others where possible.  This is ok if you just
        /// need to get an idea for the pixels along a line or something.
        /// </summary>
        /// <param name="X">The X coordinate of the geographic location to find the nearest cell value</param>
        /// <param name="Y">The Y coordinate of the geographic lcoation to find the nearest cell value</param>
        /// <param name="Value">A late bound object corresponding to the new value to set</param>
        public void PutValue(double X, double Y, object Value)
        {
            int Row, Column;
            m_Grid.ProjToCell(X, Y, out Column, out Row);
            m_Grid.set_Value(Column, Row, Value);

        }


        /// <summary>
        /// Given a jagged float array with Y major organization, assigns the values to the grid.
        /// </summary>
        /// <param name="Vals">An Array of floats like Vals[ NumRows ][ NumCols ]</param>
        public void PutValues(float[][] Vals)
        {
            int Prog = 0;
            int OldProg = 0;
            int NumRows = Vals.GetUpperBound(0) + 1;
            if (NumRows != m_Grid.Header.NumberRows)
            {
                MapWinUtility.Logger.Dbg("Application Exception: The specified array had the wrong number of rows.");
                throw new ApplicationException("The specified array had the wrong number of rows.");
            }
            for (int Y = 0; Y < NumRows; Y++)
            {
                int NumCols = Vals[Y].GetUpperBound(0) + 1;
                if (NumCols != m_Grid.Header.NumberCols)
                {
                    MapWinUtility.Logger.Dbg("Application Exception: The array had the wrong number of values in Row " + NumRows + ".");
                    throw new ApplicationException("The array had the wrong number of values in Row " + NumRows + ".");
                }
                if (m_Grid.PutRow(Y, ref Vals[Y][0]) == false)
                {
                    MapWinUtility.Logger.Dbg("MapWinGIS.Grid.PutRow(Row: " + Y.ToString() + ", Vals) threw an Exception.");
                    throw new MapWinException(m_Grid.LastErrorCode);
                }
                Prog = (Y * 100) / NumRows;
                if (Prog > OldProg)
                {
                    if (m_ICallBack != null) m_ICallBack.Progress("Progress", Prog, "Saving Values to Grid..." + Prog.ToString() + "% Complete");
                    MapWinUtility.Logger.Progress("Saving Values to Grid..." + Prog.ToString() + "% Complete", Prog, OldProg);
                    OldProg = Prog;
                }
            }
            MapWinUtility.Logger.Dbg("Finished PutValues.");
            MapWinUtility.Logger.Progress("", 0, 0);
            if (m_ICallBack != null) m_ICallBack.Progress("Progress", 0, "");

        }
        /// <summary>
        /// Saves the grid using all the current settings, using the extension to determine file type.
        /// </summary>
        public void Save()
        {
            if (m_Grid.Save(m_Grid.Filename, MapWinGIS.GridFileType.UseExtension, m_ICallBack) == false)
            {
                MapWinUtility.Logger.Dbg("MapWinGIS.Grid.Save threw an Exception.");
                throw new MapWinException(m_Grid.LastErrorCode);
            }
        }

        /// <summary>
        /// Saves the grid using the specified parameters
        /// </summary>
        /// <param name="Filename">The name of the file to save this raster to</param>
        /// <param name="newFileFormat">The file format to use when saving the grid</param>
        /// <returns>bool - always true - (throws exceptions)</returns>
        public bool Save(string Filename, GridFileType newFileFormat)
        {
            if (m_Grid.Save(Filename, Convert.mwGridFileType(newFileFormat), m_ICallBack) == false)
            {
                MapWinUtility.Logger.Dbg("MapWinGIs.Grid.Save threw an Exception.");
                throw new MapWinException(m_Grid.LastErrorCode);
            }
            return true;
        }
        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="MinThresholdValue"></param>
        /// <param name="MaxThresholdValue"></param>
        /// <returns></returns>
        public bool SetInvalidValuesToNodata(double MinThresholdValue, double MaxThresholdValue)
        {
            throw new NotImplementedException("The SetInvalidValuesToNodata method is not implemented.");
        }
        /// <summary>
        /// Not Implemented
        /// </summary>
        public int Width
        {
            get { throw new NotImplementedException("The method or operation is not implemented."); }
        }
        /// <summary>
        /// Gets or Sets the double value representing the X-coordinate, or longitude, 
        /// for the center of the cell in the lower left corner of the grid.
        /// </summary>
        public double XllCenter
        {
            get
            {
                return m_Grid.Header.XllCenter;
            }
            set
            {
                m_Grid.Header.XllCenter = value;
            }
        }
        /// <summary>
        /// Gets or Sets the double value representing the Y-coordinate, or latitude, 
        /// for the center of the cell in the lower left corner of the grid.
        /// </summary>
        public double YllCenter
        {
            get
            {

                return m_Grid.Header.YllCenter;
            }
            set
            {
                m_Grid.Header.YllCenter = value;
            }
        }

        #endregion

        #region IDisposable Members
        // This section is copied from Angela's GridWrapper class, which this will be replacing
        void IDisposable.Dispose()
        {
            //Trace.WriteLine("GridWrapper: Dispose");
            // dispose of the managed and unmanaged resources
            Dispose(true);

            // tell the GC that the Finalize process no longer needs
            // to be run for this object.
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Releases com objects
        /// </summary>
        /// <param name="disposeManagedResources"></param>
        protected virtual void Dispose(bool disposeManagedResources)
        {
            // process only if mananged and unmanaged resources have
            // not been disposed of.
            if (!m_Disposed)
            {
                //Trace.WriteLine("GridWrapper: Resources not disposed");
                if (disposeManagedResources)
                {
                }

                // dispose unmanaged resources
                //Trace.WriteLine("GridWrapper: Disposing unmanaged resouces");
                if (m_Grid != null)
                {
                    //Trace.WriteLine("GridWrapper: Disposing of grid COM object");
                    m_Grid.Close();
                    //while(Marshal.ReleaseComObject(m_Grid) != 0);
                    m_Grid = null;
                }

                m_Disposed = true;
            }
        }

        // Use C# destructor syntax for finalization code.
        /// <summary>
        /// Deconstructor (Same as calling Dispose)
        /// </summary>
        ~Raster()
        {
            //Trace.WriteLine("GridWrapper: Destructor");
            // call Dispose with false.  Since we're in the
            // destructor call, the managed resources will be
            // disposed of anyways.
            Dispose(false);
        }
        #endregion

        /// <summary>
        /// Returns the actual grid being used by this Raster class
        /// </summary>
        /// <returns></returns>
        MapWinGIS.Grid ToGrid()
        {
            return m_Grid;
        }

    }
}
