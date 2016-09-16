//********************************************************************************************************
//File name: RasterCalc.cs
//Description: Public class; provides algebraic, trigonometric, boolean, and comparison operations for raster data.
//********************************************************************************************************
//The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
//you may not use this file except in compliance with the License. You may obtain a copy of the License at 
//http://www.mozilla.org/MPL/ 
//Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
//ANY KIND, either express or implied. See the License for the specific language governing rights and 
//limitations under the License. 
//
//The Original Code is MapWindow Open Source. 
//
//Contributor(s): (Open source contributors should list themselves and their modifications here). 
//1-11-06 Allen Anselmo provided the initial API and parameter descriptions. 	
//9-13-07 Jeyakanthan Veluppillai added code for the algebra and boolean functions.		
//9-17-07 Jey to add reclassify and overlay and resample functions in their regions. 				
//********************************************************************************************************
using System.Windows.Forms;
using System;
using System.Diagnostics;

namespace MapWinGeoProc
{
	/// <summary>
	/// The RasterCalc namespace provides algebraic (add, subtract, multiply, divide), 
	/// trigonometric (sin, cos, arcSin, arcos), Boolean (And, Or, Not) and 
	/// comparison (equal, less than, greater than) operations for use with raster data.
	/// </summary>
	public class RasterCalc
	{
		private static string gErrorMsg= "";

        # region Reclassify
        ///put your methods here!!!!
        # endregion

        # region Overlay
        # endregion

        # region Resample
        # endregion 

		#region Algebra
		
		/// <summary>
		/// Adds each lead cell to the corresponding cell of the tail grid or to the constant value given.
		/// </summary>
		/// <param name="strLeadGridFile">Path to the lead grid being calculated.</param>
		/// <param name="strValOrGridFile">Will be parsed as either a numeric (int or real) value to use as a constant for all grid cells of a temp data grid used for calculation or else a path to the location of a grid to be used for the tail of the calculation.</param>
		/// <param name="strResultGridFile">The path to the resulting output grid created from an add calculation.</param>
		/// <param name="cback">An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
		public static bool Add(string strLeadGridFile, string strValOrGridFile, string strResultGridFile, MapWinGIS.ICallback cback)
		{
            int m_mrow1;
            int m_mcol1;
            int m_mrow2;
            int m_mcol2;
            int row;
            int col;
            bool AddFiles; //versus adding a value to a file.
            MapWinGIS.Grid mwResult = new MapWinGIS.Grid();
            MapWinGIS.Grid mwSourceGrid1 = new MapWinGIS.Grid();
            MapWinGIS.Grid mwSourceGrid2 = new MapWinGIS.Grid();
            mwSourceGrid1.Open(strLeadGridFile);
            m_mrow1 = (mwSourceGrid1.Header.NumberRows - 1);
            m_mcol1 = (mwSourceGrid1.Header.NumberCols - 1);
            if (System.IO.File.Exists(strValOrGridFile))
            {
                AddFiles=true;
                mwSourceGrid2.Open(strValOrGridFile);
                m_mrow2 = (mwSourceGrid2.Header.NumberRows - 1);
                m_mcol2 = (mwSourceGrid2.Header.NumberCols - 1);
            }
            else
            {
                AddFiles=false;
            }

            try
            {
                if (((m_mrow1 == m_mrow2)
                            && (m_mcol1 == m_mcol2)))
                {
                    mwResult.CreateNew("", mwSourceGrid1.Header, mwSourceGrid1.DataType, 0, true, MapWinGIS.GridFileType.Ascii, null);
                    for (row = 0; (row <= m_mrow1); row++)
                    {
                        for (col = 0; (col <= m_mcol1); col++)
                        {
                            mwResult.set_Value(col, row, (double)mwSourceGrid1.get_Value(col, row) + (double)mwSourceGrid2.get_Value(col, row));
                        }
                    }
                    mwResult.Save(AddOutputPath, MapWinGIS.GridFileType.Ascii, null);
                    mwSourceGrid1.Close();
                    mwSourceGrid2.Close();
                    return true;
                }
                else
                {
                    MessageBox.Show("Please do the resampling operation and do the Addition", MessageBox.ShowStyle.Information, "Addition");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex, MessageBox.ShowStyle.Critical, "Addition");
                return false;
            }
			//TODO: Jey, Will you please implement this function?
			Error.ClearErrorLog();
			gErrorMsg = "This function is not yet implemented.";
			Error.SetErrorMsg(gErrorMsg);
			Debug.WriteLine(gErrorMsg);

			return 0;
		}

		/// <summary>
		/// Not Implemented
		/// Subtracts each lead cell from the corresponding cell of the tail grid or from the constant value given.
		/// </summary>
		/// <param name="strLeadGridFile">Path to the lead grid being calculated.</param>
		/// <param name="strValOrGridFile">Will be parsed as either a numeric (int or real) value to use as a constant for all grid cells of a temp data grid used for the calculation or else a path to the location of a grid to be used for the tail of the calculation.</param>
		/// <param name="strResultGridFile">o	The path to the resulting output grid created from a subtract calculation.</param>
		/// <param name="cback">o	An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>o	Integer representing successful creation on 0 or some error state otherwise.</returns>
        public static bool Sub(string strLeadGridFile, string strValOrGridFile, string strResultGridFile, MapWinGIS.ICallback cback)
        {
            throw new NotImplementedException("THis method is not implemented");
        }
		
        public bool Subraction(string mwGridpath1, string mwGridpath2, string outSubrationPath)
        {
            int m_mrow1;
            int m_mcol1;
            int m_mrow2;
            int m_mcol2;
            int row;
            int col;
            MapWinGIS.Grid mwResult = new MapWinGIS.Grid();
            MapWinGIS.Grid mwSourceGrid1 = new MapWinGIS.Grid();
            MapWinGIS.Grid mwSourceGrid2 = new MapWinGIS.Grid();
            mwSourceGrid1.Open(mwGridpath1);
            mwSourceGrid2.Open(mwGridpath2);
            m_mrow1 = (mwSourceGrid1.Header.NumberRows - 1);
            m_mcol1 = (mwSourceGrid1.Header.NumberCols - 1);
            m_mrow2 = (mwSourceGrid2.Header.NumberRows - 1);
            m_mcol2 = (mwSourceGrid2.Header.NumberCols - 1);
            try
            {
                if (((m_mrow1 == m_mrow2)
                            && (m_mcol1 == m_mcol2)))
                {
                    mwResult.CreateNew("", mwSourceGrid1.Header, mwSourceGrid1.DataType, 0, true, MapWinGIS.GridFileType.Ascii);
                    for (row = 0; (row <= m_mrow1); row++)
                    {
                        for (col = 0; (col <= m_mcol1); col++)
                        {
                            mwResult.Value(col, row) = (double.Parse(mwSourceGrid1.Value(col, row)) - double.Parse(mwSourceGrid2.Value(col, row)));
                        }
                    }
                    mwResult.Save(outSubrationPath, MapWinGIS.GridFileType.Ascii, null);
                    // "C:\Users\Jey\output.asc"
                    mwSourceGrid1.Close();
                    mwSourceGrid2.Close();
                    return true;
                }
                else
                {
                    MessageBox.Show("Please do the resampling operation and do the Subraction", MessageBox.ShowStyle.Information, "Subraction");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex, MessageBox.ShowStyle.Critical, "Subration");
                return false;
            }
        }

		/// <summary>
		/// Not Implemented
		/// Multiplies each lead cell with the corresponding cell of the tail grid or with the constant value given.
		/// </summary>
		/// <param name="strLeadGridFile">Path to the lead grid being calculated.</param>
		/// <param name="strValOrGridFile">Will be parsed as either a numeric (int or real) value to use as a constant for all grid cells of a temp data grid used for the calculation or else a path to the location of a grid to be used for the tail of the calculation.</param>
		/// <param name="strResultGridFile">The path to the resulting output grid created from a multiply calculation.</param>
		/// <param name="cback">An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
		public static bool Multiply(string strLeadGridFile, string strValOrGridFile, string strResultGridFile, MapWinGIS.ICallback cback)
		{
			 int m_mrow1;
            int m_mcol1;
            int m_mrow2;
            int m_mcol2;
            int row;
            int col;
            MapWinGIS.Grid mwResult = new MapWinGIS.Grid();
            MapWinGIS.Grid mwSourceGrid1 = new MapWinGIS.Grid();
            MapWinGIS.Grid mwSourceGrid2 = new MapWinGIS.Grid();
            mwSourceGrid1.Open(mwGridpath1);
            mwSourceGrid2.Open(mwGridpath2);
            m_mrow1 = (mwSourceGrid1.Header.NumberRows - 1);
            m_mcol1 = (mwSourceGrid1.Header.NumberCols - 1);
            m_mrow2 = (mwSourceGrid2.Header.NumberRows - 1);
            m_mcol2 = (mwSourceGrid2.Header.NumberCols - 1);
            try
            {
                if (((m_mrow1 == m_mrow2)
                            && (m_mcol1 == m_mcol2)))
                {
                    mwResult.CreateNew("", mwSourceGrid1.Header, mwSourceGrid1.DataType, 0, true, MapWinGIS.GridFileType.Ascii);
                    for (row = 0; (row <= m_mrow1); row++)
                    {
                        for (col = 0; (col <= m_mcol1); col++)
                        {
                            mwResult.Value(col, row) = (double.Parse(mwSourceGrid1.Value(col, row)) * double.Parse(mwSourceGrid2.Value(col, row)));
                        }
                    }
                    mwResult.Save(outMultiplicationPath, MapWinGIS.GridFileType.Ascii, null);
                    // "C:\Users\Jey\output.asc"
                    mwSourceGrid1.Close();
                    mwSourceGrid2.Close();
                    return true;
                }
                else
                {
                    MessageBox.Show("Please do the resampling operation and do the Multiplication", MessageBox.ShowStyle.Information, "Multiplication");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex, MessageBox.ShowStyle.Critical, "Multiplication");
                return false;
            }
		}

		/// <summary>
		/// Not Implemented
		/// Divides each lead cell by the corresponding cell of the tail grid or by the constant value given.
		/// </summary>
		/// <param name="strLeadGridFile">Path to the lead grid being calculated.</param>
		/// <param name="strValOrGridFile">Will be parsed as either a numeric (int or real) value to use as a constant for all grid cells of a temp data grid used for the calculation or else a path to the location of a grid to be used for the tail of the calculation.</param>
		/// <param name="strResultGridFile">The path to the resulting output grid created from a divide calculation.</param>
		/// <param name="cback">An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
		public static int Divide(string strLeadGridFile, string strValOrGridFile, string strResultGridFile, MapWinGIS.ICallback cback)
		{
			 int m_mrow1;
            int m_mcol1;
            int m_mrow2;
            int m_mcol2;
            int row;
            int col;
            MapWinGIS.Grid mwResult = new MapWinGIS.Grid();
            MapWinGIS.Grid mwSourceGrid1 = new MapWinGIS.Grid();
            MapWinGIS.Grid mwSourceGrid2 = new MapWinGIS.Grid();
            mwSourceGrid1.Open(mwGridpath1);
            mwSourceGrid2.Open(mwGridpath2);
            m_mrow1 = (mwSourceGrid1.Header.NumberRows - 1);
            m_mcol1 = (mwSourceGrid1.Header.NumberCols - 1);
            m_mrow2 = (mwSourceGrid2.Header.NumberRows - 1);
            m_mcol2 = (mwSourceGrid2.Header.NumberCols - 1);
            try
            {
                if (((m_mrow1 == m_mrow2)
                            && (m_mcol1 == m_mcol2)))
                {
                    mwResult.CreateNew("", mwSourceGrid1.Header, mwSourceGrid1.DataType, 0, true, MapWinGIS.GridFileType.Ascii);
                    for (row = 0; (row <= m_mrow1); row++)
                    {
                        for (col = 0; (col <= m_mcol1); col++)
                        {
                            mwResult.Value(col, row) = (double.Parse(mwSourceGrid1.Value(col, row)) * double.Parse(mwSourceGrid2.Value(col, row)));
                        }
                    }
                    mwResult.Save(outDivisionPath, MapWinGIS.GridFileType.Ascii, null);
                    // "C:\Users\Jey\output.asc"
                    mwSourceGrid1.Close();
                    mwSourceGrid2.Close();
                    return true;
                }
                else
                {
                    MessageBox.Show("Please do the resampling operation and do the Division", MessageBox.ShowStyle.Information, "Division");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex, MessageBox.ShowStyle.Critical, "Division");
                return false;
            }
		}

		/// <summary>
		/// Not Implemented
		/// Mods each lead cell with the corresponding cell of the tail grid or with the constant value given.
		/// </summary>
		/// <param name="strLeadGridFile">Path to the lead grid being calculated.</param>
		/// <param name="strValOrGridFile">Will be parsed as either a numeric (int or real) value to use as a constant for all grid cells of a temp data grid used for the calculation or else a path to the location of a grid to be used for the tail of the calculation.</param>
		/// <param name="strResultGridFile">The path to the resulting output grid created from a mod calculation.</param>
		/// <param name="cback">An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
		public static int Mod(string strLeadGridFile, string strValOrGridFile, string strResultGridFile, MapWinGIS.ICallback cback)
		{
			//TODO: Implement this function
			Error.ClearErrorLog();
			gErrorMsg = "This function is not yet implemented.";
			Error.SetErrorMsg(gErrorMsg);
			Debug.WriteLine(gErrorMsg);

			return 0;
		}

		/// <summary>
		/// Not Implemented
		/// Assigns each cell the corresponding cell value of the tail grid or a constant given.
		/// </summary>
		/// <param name="strResGridToAssignTo">The path to the resulting output grid created or overwritten by assignment.</param>
		/// <param name="strValOrGridFile">Will be parsed as either a numeric (int or real) value to use as a constant for all grid cells of a temp data grid used for the calculation or else a path to the location of a grid to be used for the tail of the calculation.</param>
		/// <param name="cback">An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
		public static int Assign(string strResGridToAssignTo, string strValOrGridFile, MapWinGIS.ICallback cback)
		{
			//TODO: Implement this function
			Error.ClearErrorLog();
			gErrorMsg = "This function is not yet implemented.";
			Error.SetErrorMsg(gErrorMsg);
			Debug.WriteLine(gErrorMsg);

			return 0;
		}
		
		#endregion

		#region Comparison

		/// <summary>
		/// Not Implemented
		/// Perfoms a '>' comparison on each lead cell with the corresponding cell of the tail grid or with the constant value given.
		/// </summary>
		/// <param name="strLeadGridFile">Path to the lead grid being compared.</param>
		/// <param name="strValOrGridFile">Will be parsed as either a numeric (int or real) value to use as a constant for all grid cells of a temp data grid used for the comparison or else a path to the location of a grid to be used for the tail of the comparison.</param>
		/// <param name="strResultGridFile">The path to the resulting output grid created from a 'Greater Than' comparison.</param>
		/// <param name="cback">An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
		public static int CompGT(string strLeadGridFile, string strValOrGridFile, string strResultGridFile, MapWinGIS.ICallback cback)
		{
			//TODO: Implement this function
			Error.ClearErrorLog();
			gErrorMsg = "This function is not yet implemented.";
			Error.SetErrorMsg(gErrorMsg);
			Debug.WriteLine(gErrorMsg);

			return 0;
		}

		/// <summary>
		/// Not Implemented
		/// Perfoms a '>=' comparison on each lead cell with the corresponding cell of the tail grid or with the constant value given.
		/// </summary>
		/// <param name="strLeadGridFile">Path to the lead grid being compared.</param>
		/// <param name="strValOrGridFile">Will be parsed as either a numeric (int or real) value to use as a constant for all grid cells of a temp data grid used for the comparison or else a path to the location of a grid to be used for the tail of the comparison.</param>
		/// <param name="strResultGridFile">The path to the resulting output grid created from a 'Greater Than Or Equal To' comparison.</param>
		/// <param name="cback">An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
		public static int CompGTE(string strLeadGridFile, string strValOrGridFile, string strResultGridFile, MapWinGIS.ICallback cback)
		{
			//TODO: Implement this function
			Error.ClearErrorLog();
			gErrorMsg = "This function is not yet implemented.";
			Error.SetErrorMsg(gErrorMsg);
			Debug.WriteLine(gErrorMsg);

			return 0;
		}
		
		/// <summary>
		/// Not Implemented
		/// Perfoms a 'Less Than' comparison on each lead cell with the corresponding cell of the tail grid or with the constant value given.
		/// </summary>
		/// <param name="strLeadGridFile">Path to the lead grid being compared.</param>
		/// <param name="strValOrGridFile">Will be parsed as either a numeric (int or real) value to use as a constant for all grid cells of a temp data grid used for the comparison or else a path to the location of a grid to be used for the tail of the comparison.</param>
		/// <param name="strResultGridFile">The path to the resulting output grid created from a 'Less Than' comparison.</param>
		/// <param name="cback">An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
		public static int CompLT(string strLeadGridFile, string strValOrGridFile, string strResultGridFile, MapWinGIS.ICallback cback)
		{
			//TODO: Implement this function
			Error.ClearErrorLog();
			gErrorMsg = "This function is not yet implemented.";
			Error.SetErrorMsg(gErrorMsg);
			Debug.WriteLine(gErrorMsg);

			return 0;
		}

		/// <summary>
		/// Not Implemented
		/// Perfoms a 'Less Than Or Equal To' comparison on each lead cell with the corresponding cell of the tail grid or with the constant value given.
		/// </summary>
		/// <param name="strLeadGridFile">Path to the lead grid being compared.</param>
		/// <param name="strValOrGridFile">Will be parsed as either a numeric (int or real) value to use as a constant for all grid cells of a temp data grid used for the comparison or else a path to the location of a grid to be used for the tail of the comparison.</param>
		/// <param name="strResultGridFile">The path to the resulting output grid created from a 'Less Than Or Equal To' comparison.</param>
		/// <param name="cback">An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
		public static int CompLTE(string strLeadGridFile, string strValOrGridFile, string strResultGridFile, MapWinGIS.ICallback cback)
		{
			//TODO: Implement this function
			Error.ClearErrorLog();
			gErrorMsg = "This function is not yet implemented.";
			Error.SetErrorMsg(gErrorMsg);
			Debug.WriteLine(gErrorMsg);

			return 0;
		}

		/// <summary>
		/// Not Implemented
		/// Perfoms an '==' comparison on each lead cell with the corresponding cell of the tail grid or with the constant value given.
		/// </summary>
		/// <param name="strLeadGridFile">Path to the lead grid being compared.</param>
		/// <param name="strValOrGridFile">Will be parsed as either a numeric (int or real) value to use as a constant for all grid cells of a temp data grid used for the comparison or else a path to the location of a grid to be used for the tail of the comparison.</param>
		/// <param name="strResultGridFile">The path to the resulting output grid created from an 'Equal To' comparison.</param>
		/// <param name="cback">An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
		public static int CompEQ(string strLeadGridFile, string strValOrGridFile, string strResultGridFile, MapWinGIS.ICallback cback)
		{
			//TODO: Implement this function
			Error.ClearErrorLog();
			gErrorMsg = "This function is not yet implemented.";
			Error.SetErrorMsg(gErrorMsg);
			Debug.WriteLine(gErrorMsg);

			return 0;
		}

		/// <summary>
		/// Not Implemented
		/// Perfoms a '!=' comparison on each lead cell with the corresponding cell of the tail grid or with the constant value given.
		/// </summary>
		/// <param name="strLeadGridFile">Path to the lead grid being compared.</param>
		/// <param name="strValOrGridFile">Will be parsed as either a numeric (int or real) value to use as a constant for all grid cells of a temp data grid used for the comparison or else a path to the location of a grid to be used for the tail of the comparison.</param>
		/// <param name="strResultGridFile">The path to the resulting output grid created from a 'Not Equal To' comparison.</param>
		/// <param name="cback">An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
		public static int CompNEQ(string strLeadGridFile, string strValOrGridFile, string strResultGridFile, MapWinGIS.ICallback cback)
		{
			//TODO: Implement this function
			Error.ClearErrorLog();
			gErrorMsg = "This function is not yet implemented.";
			Error.SetErrorMsg(gErrorMsg);
			Debug.WriteLine(gErrorMsg);

			return 0;
		}

		#endregion

		#region Boolean

		/// <summary>
		/// Not Implemented
		/// Places a value of 1 in all cells where the corresponding lead cell value compared to the tail cell value is TRUE by C Boolean OR, and a 0 to those that return FALSE.
		/// </summary>
		/// <param name="strLeadGridFile">Path to the lead grid file.</param>
		/// <param name="strTailGridFile">Path to the tail grid file.</param>
		/// <param name="strResultGridFile">Path to the resulting output grid file.</param>
		/// <param name="cback">An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
		public static bool BooleanAnd(string strLeadGridFile, string strTailGridFile, string strResultGridFile, MapWinGIS.ICallback cback)
		{
			 int row;
            int col;
            int m_mrow1;
            int m_mcol1;
            int m_mrow2;
            int m_mcol2;
            MapWinGIS.Grid mwResult = new MapWinGIS.Grid();
            MapWinGIS.Grid mwSourceGrid1 = new MapWinGIS.Grid();
            MapWinGIS.Grid mwSourceGrid2 = new MapWinGIS.Grid();
            object nodata1 = mwSourceGrid1.Header.NodataValue;
            object nodata2 = mwSourceGrid2.Header.NodataValue;
            mwSourceGrid1.Open(mwGridpath1);
            mwSourceGrid2.Open(mwGridpath2);
            m_mrow1 = (mwSourceGrid1.Header.NumberRows - 1);
            m_mcol1 = (mwSourceGrid1.Header.NumberCols - 1);
            m_mrow2 = (mwSourceGrid2.Header.NumberRows - 1);
            m_mcol2 = (mwSourceGrid2.Header.NumberCols - 1);
            try
            {
                if (((m_mrow1 == m_mrow2)
                            && (m_mcol1 == m_mcol2)))
                {
                    for (row = 0; (row <= m_mrow1); row++)
                    {
                        for (col = 0; (col <= m_mcol1); col++)
                        {
                            if (((mwSourceGrid1.Value(col, row) != nodata1)
                                        && (mwSourceGrid2.Value(col, row) != nodata2)))
                            {
                                mwResult.Value(col, row) = 1;
                            }
                            else
                            {
                                mwResult.Value(col, row) = 0;
                            }
                        }
                    }
                    return true;
                }
                else
                {
                    MessageBox.Show("Please do the resampling operation and do the Division", MessageBox.ShowStyle.Information, "Boolean And");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex, MessageBox.ShowStyle.Critical, "Boolean Not");
                return false;
            }
		}

		/// <summary>
		/// Not Implemented
		/// Places a value of 1 in all cells where the corresponding lead cell value compared to the tail cell value is TRUE by C Boolean AND, and a 0 to those that return FALSE.
		/// </summary>
		/// <param name="strLeadGridFile">Path to the lead grid file.</param>
		/// <param name="strTailGridFile">Path to the tail grid file.</param>
		/// <param name="strResultGridFile">Path to the resulting output grid file.</param>
		/// <param name="cback">An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
		public static bool BooleanOr(string strLeadGridFile, string strTailGridFile, string strResultGridFile, MapWinGIS.ICallback cback)
		{
		  int row;
            int col;
            int m_mrow1;
            int m_mcol1;
            int m_mrow2;
            int m_mcol2;
            MapWinGIS.Grid mwResult = new MapWinGIS.Grid();
            MapWinGIS.Grid mwSourceGrid1 = new MapWinGIS.Grid();
            MapWinGIS.Grid mwSourceGrid2 = new MapWinGIS.Grid();
            object nodata1 = mwSourceGrid1.Header.NodataValue;
            object nodata2 = mwSourceGrid2.Header.NodataValue;
            mwSourceGrid1.Open(mwGridpath1);
            mwSourceGrid2.Open(mwGridpath2);
            m_mrow1 = (mwSourceGrid1.Header.NumberRows - 1);
            m_mcol1 = (mwSourceGrid1.Header.NumberCols - 1);
            m_mrow2 = (mwSourceGrid2.Header.NumberRows - 1);
            m_mcol2 = (mwSourceGrid2.Header.NumberCols - 1);
            try
            {
                if (((m_mrow1 == m_mrow2)
                            && (m_mcol1 == m_mcol2)))
                {
                    for (row = 0; (row <= m_mrow1); row++)
                    {
                        for (col = 0; (col <= m_mcol1); col++)
                        {
                            if (((mwSourceGrid1.Value(col, row) != nodata1)
                                        || (mwSourceGrid2.Value(col, row) != nodata2)))
                            {
                                mwResult.Value(col, row) = 1;
                            }
                            else
                            {
                                mwResult.Value(col, row) = 0;
                            }
                        }
                    }
                    return true;
                }
                else
                {
                    MessageBox.Show("Please do the resampling operation and do the Division", MessageBox.ShowStyle.Information, "Boolean Or");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex, MessageBox.ShowStyle.Critical, "Boolean OR");
                return false;
            }
		}

		/// <summary>
		/// Not Implemented
		/// Places a value of 1 in all cells where the corresponding lead cell value compared to the tail cell value is TRUE by C Boolean NOT, and a 0 to those that return FALSE.
		/// </summary>
		/// <param name="strLeadGridFile">Path to the lead grid file.</param>
		/// <param name="strTailGridFile">Path to the tail grid file.</param>
		/// <param name="strResultGridFile">Path to the resulting output grid file.</param>
		/// <param name="cback">An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
		public static bool BooleanNot(string strLeadGridFile, string strTailGridFile, string strResultGridFile, MapWinGIS.ICallback cback)
		{
			            int m_mrow;
            int m_mcol;
            int row;
            int col;
            MapWinGIS.Grid mwResult = new MapWinGIS.Grid();
            MapWinGIS.Grid mwSourceGrid1 = new MapWinGIS.Grid();
            object nodata = mwSourceGrid1.Header.NodataValue;
            try
            {
                mwSourceGrid1.Open(path1);
                m_mrow = (mwSourceGrid1.Header.NumberRows - 1);
                m_mcol = (mwSourceGrid1.Header.NumberCols - 1);
                //   newgrid.CreateNew("", grid.Header, grid.DataType, 0, True, MapWinGIS.GridFileType.Ascii, Me)
                mwResult.CreateNew("", mwSourceGrid1.Header, mwSourceGrid1.DataType, 0, true, MapWinGIS.GridFileType.Ascii);
                for (row = 0; (row <= m_mrow); row++)
                {
                    for (col = 0; (col <= m_mcol); col++)
                    {
                        if ((mwSourceGrid1.Value(col, row) == nodata))
                        {
                            mwResult.Value(col, row) = 0;
                        }
                        else
                        {
                            mwResult.Value(col, row) = 1;
                        }
                    }
                }
                mwResult.Save(output, MapWinGIS.GridFileType.Ascii, null);
                //  "C:\Users\Jey\output.asc"
                mwSourceGrid1.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex, MessageBox.ShowStyle.Critical, "Boolean Not");
                return false;
            }
		}

        #endregion

		#region Trigonometry

		/// <summary>
		/// Not Implemented
		/// Performs an Absolute operation on each corresponding cell value from the input file.
		/// </summary>
		/// <param name="strGridFile">Path to the lead grid file.</param>
		/// <param name="strResultGridFile">Path to the resulting output grid file with Abs() results.</param>
		/// <param name="cback">An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
		public static int TrigAbs(string strGridFile, string strResultGridFile, MapWinGIS.ICallback cback)
		{
			//TODO: Implement this function
			Error.ClearErrorLog();
			gErrorMsg = "This function is not yet implemented.";
			Error.SetErrorMsg(gErrorMsg);
			Debug.WriteLine(gErrorMsg);

			return 0;
		}
		
		/// <summary>
		/// Not Implemented
		/// Inverts the sign of each corresponding cell value from the input file.
		/// </summary>
		/// <param name="strGridFile">Path to the lead grid file.</param>
		/// <param name="strResultGridFile">Path to the resulting output grid file with negated results.</param>
		/// <param name="cback">An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
		public static int TrigNegative(string strGridFile, string strResultGridFile, MapWinGIS.ICallback cback)
		{
			//TODO: Implement this function
			Error.ClearErrorLog();
			gErrorMsg = "This function is not yet implemented.";
			Error.SetErrorMsg(gErrorMsg);
			Debug.WriteLine(gErrorMsg);

			return 0;
		}

		/// <summary>
		/// Not Implemented
		/// Takes the Cosine of each cell value in the input file.
		/// </summary>
		/// <param name="strGridFile">Path to the lead grid file.</param>
		/// <param name="strResultGridFile">Path to the resulting output grid file with Cos() results.</param>
		/// <param name="cback">An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
		public static int TrigCos(string strGridFile, string strResultGridFile, MapWinGIS.ICallback cback)
		{
			//TODO: Implement this function
			Error.ClearErrorLog();
			gErrorMsg = "This function is not yet implemented.";
			Error.SetErrorMsg(gErrorMsg);
			Debug.WriteLine(gErrorMsg);

			return 0;
		}

		/// <summary>
		/// Not Implemented
		/// Takes the Sine of each cell value in the input file.
		/// </summary>
		/// <param name="strGridFile">Path to the lead grid file.</param>
		/// <param name="strResultGridFile">Path to the resulting output grid file with Sin() results.</param>
		/// <param name="cback">An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
		public static int TrigSin(string strGridFile, string strResultGridFile, MapWinGIS.ICallback cback)
		{
			//TODO: Implement this function
			Error.ClearErrorLog();
			gErrorMsg = "This function is not yet implemented.";
			Error.SetErrorMsg(gErrorMsg);
			Debug.WriteLine(gErrorMsg);

			return 0;
		}

		/// <summary>
		/// Not Implemented
		/// Takes the Tangent of each cell value in the input file.
		/// </summary>
		/// <param name="strGridFile">Path to the lead grid file.</param>
		/// <param name="strResultGridFile">Path to the resulting output grid file with Tan() results.</param>
		/// <param name="cback">An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
		public static int TrigTan(string strGridFile, string strResultGridFile, MapWinGIS.ICallback cback)
		{
			//TODO: Implement this function
			Error.ClearErrorLog();
			gErrorMsg = "This function is not yet implemented.";
			Error.SetErrorMsg(gErrorMsg);
			Debug.WriteLine(gErrorMsg);

			return 0;
		}

		/// <summary>
		/// Not Implemented
		/// Takes the ArcCosine of each cell value in the input file.
		/// </summary>
		/// <param name="strGridFile">Path to the lead grid file.</param>
		/// <param name="strResultGridFile">Path to the resulting output grid file with aCos() results.</param>
		/// <param name="cback">An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
		public static int TrigAcos(string strGridFile, string strResultGridFile, MapWinGIS.ICallback cback)
		{
			//TODO: Implement this function
			Error.ClearErrorLog();
			gErrorMsg = "This function is not yet implemented.";
			Error.SetErrorMsg(gErrorMsg);
			Debug.WriteLine(gErrorMsg);

			return 0;
		}

		/// <summary>
		/// Not Implemented
		/// Takes the ArcSine of each cell value in the input file.
		/// </summary>
		/// <param name="strGridFile">Path to the lead grid file.</param>
		/// <param name="strResultGridFile">Path to the resulting output grid file with aSin() results.</param>
		/// <param name="cback">An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
		public static int TrigAsin(string strGridFile, string strResultGridFile, MapWinGIS.ICallback cback)
		{
			//TODO: Implement this function
			Error.ClearErrorLog();
			gErrorMsg = "This function is not yet implemented.";
			Error.SetErrorMsg(gErrorMsg);
			Debug.WriteLine(gErrorMsg);

			return 0;
		}

		/// <summary>
		/// Not Implemented
		/// Takes the ArcTangent of each cell value in the input file.
		/// </summary>
		/// <param name="strGridFile">Path to the lead grid file.</param>
		/// <param name="strResultGridFile">Path to the resulting output grid file with aTan() results.</param>
		/// <param name="cback">An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
		public static int TrigAtan(string strGridFile, string strResultGridFile, MapWinGIS.ICallback cback)
		{
			//TODO: Implement this function
			Error.ClearErrorLog();
			gErrorMsg = "This function is not yet implemented.";
			Error.SetErrorMsg(gErrorMsg);
			Debug.WriteLine(gErrorMsg);

			return 0;
		}

		/// <summary>
		/// Not Implemented
		/// Takes the Exponent of each cell value in the input file.
		/// </summary>
		/// <param name="strGridFile">Path to the lead grid file.</param>
		/// <param name="strResultGridFile">Path to the resulting output grid file with Exp() results.</param>
		/// <param name="cback">An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
		public static int TrigExp(string strGridFile, string strResultGridFile, MapWinGIS.ICallback cback)
		{
			//TODO: Implement this function
			Error.ClearErrorLog();
			gErrorMsg = "This function is not yet implemented.";
			Error.SetErrorMsg(gErrorMsg);
			Debug.WriteLine(gErrorMsg);

			return 0;
		}

		/// <summary>
		/// Not Implemented
		/// Takes the Ceiling of each cell value in the input file.
		/// </summary>
		/// <param name="strGridFile">Path to the lead grid file.</param>
		/// <param name="strResultGridFile">Path to the resulting output grid file with Ceil() results.</param>
		/// <param name="cback">An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
		public static int TrigCeil(string strGridFile, string strResultGridFile, MapWinGIS.ICallback cback)
		{
			//TODO: Implement this function
			Error.ClearErrorLog();
			gErrorMsg = "This function is not yet implemented.";
			Error.SetErrorMsg(gErrorMsg);
			Debug.WriteLine(gErrorMsg);

			return 0;
		}

		/// <summary>
		/// Not Implemented
		/// Takes the Floor of each cell value in the input file.
		/// </summary>
		/// <param name="strGridFile">Path to the lead grid file.</param>
		/// <param name="strResultGridFile">Path to the resulting output grid file with Floor() results.</param>
		/// <param name="cback">An optional callback object which allows progress and messages to be returned during functionality.</param>
		/// <returns>Integer representing successful creation on 0 or some error state otherwise.</returns>
		public static int TrigFloor(string strGridFile, string strResultGridFile, MapWinGIS.ICallback cback)
		{
			//TODO: Implement this function
			Error.ClearErrorLog();
			gErrorMsg = "This function is not yet implemented.";
			Error.SetErrorMsg(gErrorMsg);
			Debug.WriteLine(gErrorMsg);

			return 0;
		}

		#endregion
																																			 
	}//end of class RasterCalc
}
