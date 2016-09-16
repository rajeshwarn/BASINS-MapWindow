// ********************************************************************************************************
// <copyright file="GridWrapper.cs" company="MapWindow.org">
//     Copyright (c) MapWindow Development team. All rights reserved.
// </copyright>
// Description: Internal class, wrapper for the MapWinGIS COM grid class.
// ********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either express or implied. See the License for the specific language governing rights and 
// limitations under the License. 
//
// The Original Code is MapWindow Open Source.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// 01/20/06 - Angela Hillier - created wrapper for heavy use of the COM grid object to insure proper memory disposal.
// 12/29/10 - Paul Meems     - Making this class StyleCop and ReSharper compliant
// ********************************************************************************************************

namespace MapWinGeoProc
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Wrapper for the MapWinGIS COM grid class.
    /// </summary>
    internal class GridWrapper : IDisposable
    {
        /// <summary>External unmanaged resource.</summary>
        private MapWinGIS.Grid grid;

        /// <summary>Track whether Dispose has been called.</summary>
        private bool disposed;

        #region Constructor and destructor
        /// <summary>
        /// Initializes a new instance of the GridWrapper class.
        /// </summary>
        public GridWrapper()
        {
            this.grid = new MapWinGIS.GridClass();
        }

        /// <summary>
        /// Finalizes an instance of the GridWrapper class.
        /// </summary>
        ~GridWrapper()
        {
            // call Dispose with false.  Since we're in the
            // destructor call, the managed resources will be
            // disposed of anyways.
            this.Dispose(false);
        }
        #endregion

        #region public methods
        /// <summary>
        /// Use C# destructor syntax for finalization code.
        /// </summary>
        public void Dispose()
        {
            // dispose of the managed and unmanaged resources
            this.Dispose(true);

            // tell the GC that the Finalize process no longer needs
            // to be run for this object.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Opens a grid file
        /// </summary>
        /// <param name="gridFile">The grid file to open</param>
        /// <param name="inRam">Load in memory or not</param>
        /// <returns>True on success</returns>
        public bool Open(string gridFile, bool inRam)
        {
            if (!this.grid.Open(gridFile, MapWinGIS.GridDataType.UnknownDataType, inRam, MapWinGIS.GridFileType.UseExtension, null))
            {
                Globals.WriteErrorMessage("Problem opening grid: " + this.grid.get_ErrorMsg(this.grid.LastErrorCode));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Saves the grid file
        /// </summary>
        /// <param name="gridFile">The name of the grid</param>
        /// <returns>True on success</returns>
        public bool Save(string gridFile)
        {
            if (!this.grid.Save(gridFile, MapWinGIS.GridFileType.UseExtension, null))
            {
                Globals.WriteErrorMessage("Problem saving grid: " + this.grid.get_ErrorMsg(this.grid.LastErrorCode));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Closes the grid
        /// </summary>
        /// <returns>True on success</returns>
        public bool Close()
        {
            if (!this.grid.Close())
            {
                Globals.WriteErrorMessage("Problem saving grid: " + this.grid.get_ErrorMsg(this.grid.LastErrorCode));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the cell height
        /// </summary>
        /// <returns>The cell height</returns>
        public double GetCellHeight()
        {
            return this.grid.Header.dY;
        }

        /// <summary>
        /// Gets the cell width
        /// </summary>
        /// <returns>The cell width</returns>
        public double GetCellWidth()
        {
            return this.grid.Header.dX;
        }

        /// <summary>
        /// Gets the number of columns
        /// </summary>
        /// <returns>The number of columns</returns>
        public int GetNumCols()
        {
            return this.grid.Header.NumberCols;
        }

        /// <summary>
        /// Gets the number of rows
        /// </summary>
        /// <returns>The number of rows</returns>
        public int GetNumRows()
        {
            return this.grid.Header.NumberRows;
        }

        /// <summary>
        /// Reads the NoData value from the grid
        /// </summary>
        /// <returns>The NoData value</returns>
        public object GetNodataValue()
        {
            return this.grid.Header.NodataValue;
        }

        /// <summary>
        /// Copy the grid header
        /// </summary>
        /// <returns>The grid header</returns>
        public MapWinGIS.GridHeader GetHeaderCopy()
        {
            var newHdr = new MapWinGIS.GridHeader();
            newHdr.CopyFrom(this.grid.Header);
            return newHdr;
        }

        /// <summary>
        /// A reference -- i.e., the original may be changed if this header is changed
        /// </summary>
        /// <returns>The grid header</returns>
        public MapWinGIS.GridHeader GetHeaderReference()
        {
            return this.grid.Header;
        }
        
        /// <summary>
        /// Converts the projection to the cell location
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <param name="col">The column number</param>
        /// <param name="row">The row number</param>
        public void ProjToCell(double x, double y, out int col, out int row)
        {
            this.grid.ProjToCell(x, y, out col, out row);
        }
        
        /// <summary>
        /// Converts the cell location to the projection
        /// </summary>
        /// <param name="col">The column number</param>
        /// <param name="row">The row number</param>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        public void CellToProj(int col, int row, out double x, out double y)
        {
            this.grid.CellToProj(col, row, out x, out y);
        }

        /// <summary>
        /// Gets the value
        /// </summary>
        /// <param name="col">The column number</param>
        /// <param name="row">The row number</param>
        /// <returns>The value of the grid</returns>
        public object GetValue(int col, int row)
        {
            return this.grid.get_Value(col, row);
        }

        /// <summary>
        /// Sets the value
        /// </summary>
        /// <param name="col">The column number</param>
        /// <param name="row">The row number</param>
        /// <param name="value">The value of the grid</param>
        public void SetValue(int col, int row, object value)
        {
            this.grid.set_Value(col, row, value);
        }

        /// <summary>
        /// Gets the row
        /// </summary>
        /// <param name="row">The row number</param>
        /// <param name="valArray">The row value</param>
        /// <returns>True on success</returns>
        public bool GetRow(int row, ref float[] valArray)
        {
            // Talk to Chris about why this returns false....
            this.grid.GetRow(row, ref valArray[0]);
            return true;
        }

        /// <summary>
        /// Sets the row
        /// </summary>
        /// <param name="row">The row number</param>
        /// <param name="valArray">The row value</param>
        public void SetRow(int row, ref float[] valArray)
        {
            this.grid.PutRow(row, ref valArray[0]);
        }

        #endregion

        #region obsolete methods

        // ReSharper disable InconsistentNaming
        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "This method is obsolete")]
        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "This method is obsolete")]
        [Obsolete("This method is obsolete; use GetValue() instead")]
        public object get_Value(int col, int row)
        {
            return this.GetValue(col, row);
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "This method is obsolete")]
        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "This method is obsolete")]
        [Obsolete("This method is obsolete; use SetValue() instead")]
        public void set_Value(int col, int row, object value)
        {
            this.SetValue(col, row, value);
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "This method is obsolete")]
        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "This method is obsolete")]
        [Obsolete("This method is obsolete; use GetRow() instead")]
        public bool get_Row(int row, ref float[] valArray)
        {
            return this.GetRow(row, ref valArray);
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "This method is obsolete")]
        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "This method is obsolete")]
        [Obsolete("This method is obsolete; use SetRow() instead")]
        public void set_Row(int row, ref float[] valArray)
        {
            this.SetRow(row, ref valArray);
        }

        // ReSharper restore InconsistentNaming
        #endregion

        #region protected methods
        /// <summary>
        /// Use C# destructor syntax for finalization code.
        /// </summary>
        /// <param name="disposeManagedResources">Dispose managed resources</param>
        protected virtual void Dispose(bool disposeManagedResources)
        {
            // process only if mananged and unmanaged resources have
            // not been disposed of.
            if (!this.disposed)
            {
                if (disposeManagedResources)
                {
                    // dispose managed resources
                }

                // dispose unmanaged resources
                if (this.grid != null)
                {
                    this.grid.Close();
                    //while (Marshal.ReleaseComObject(this.grid) != 0)
                    //{
                    //}

                    this.grid = null;
                }

                this.disposed = true;
            }
        }
        #endregion
    }
}
