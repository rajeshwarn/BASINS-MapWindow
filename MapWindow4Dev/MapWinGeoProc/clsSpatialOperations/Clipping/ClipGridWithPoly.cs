// ********************************************************************************************************
// <copyright file="ClipGridWithPoly.cs" company="MapWindow.org">
//     Copyright (c) MapWindow Development team. All rights reserved.
// </copyright>
// Description: Internal class for grid clipping.
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
// 09/01/05 - Angela Hillier - Created grid clipping method.
// 02/10/06 - Angela Hillier - updated with memory and speed optimizations
// 04/14/06 - Mark Gray      - speed optimizations and change to using new PointInPoly
// 12/21/10 - Paul Meems     - Making this class StyleCop and ReSharper compliant
// ********************************************************************************************************

namespace MapWinGeoProc
{
    using System;
    using System.Runtime.InteropServices;
    using MapWinGIS;

    /// <summary>
    /// Clips a grid so that it contains only data that falls within the polygon.
    /// </summary>
    internal class ClipGridWithPoly
    {
        #region ClipGridWithPolygon()
        /// <summary>
        /// Creates a new grid containing data from the input grid that
        /// falls within the polygon boundary.
        /// </summary>
        /// <param name="inputGridfile">Full path to the input grid file.</param>
        /// <param name="poly">The 2D polygon used for clipping the input grid.</param>
        /// <param name="resultGridfile">Full path to where the resulting grid will be saved.</param>
        /// <param name="clipToExtents">True if clipping to polygon extents rather than actual polygon shape.</param>
        /// <returns>True if clipping was successful, false if an error occurs.</returns>
        /// <remarks>TODO: Check if it also works with multipart shapes and/or shapes with holes</remarks>
        internal static bool ClipGridWithPolygon(ref string inputGridfile, ref Shape poly, ref string resultGridfile, bool clipToExtents)
        {
            MapWinUtility.Logger.Dbg("ClipGridWithPolygon(inputGridfile: " + inputGridfile + ",\n" +
                                     "                    poly: " + Macro.ParamName(poly) + ",\n" +
                                     "                    resultGridfile: " + resultGridfile + ",\n" +
                                     "                    clipToExtents: " + clipToExtents);
            Error.ClearErrorLog();
            const bool InRam = true;

            // Open grid to get info and traverse through points
            Grid grid = new GridClass();

            if (!grid.Open(inputGridfile, GridDataType.UnknownDataType, InRam, GridFileType.UseExtension, null))
            {
                Globals.WriteErrorMessage("Error occurred while trying to open grid: " + grid.get_ErrorMsg(grid.LastErrorCode));
                return false;
            }
            
            var numCols = grid.Header.NumberCols;
            var numRows = grid.Header.NumberRows;
            var cellWidth = grid.Header.dX;
            var cellHeight = grid.Header.dY;
            var xllCenter = grid.Header.XllCenter;
            var yllCenter = grid.Header.YllCenter;

            // Find the grid extents
            var minX = xllCenter - (cellWidth / 2);
            var maxX = xllCenter + (cellWidth * (numCols - 1)) + (cellWidth / 2);
            var minY = yllCenter - (cellHeight / 2);
            var maxY = yllCenter + (cellHeight * (numRows - 1)) + (cellHeight / 2);

            var polyMinX = poly.Extents.xMin;
            var polyMaxX = poly.Extents.xMax;
            var polyMinY = poly.Extents.yMin;
            var polyMaxY = poly.Extents.yMax;

            var boundsIntersect = Globals.CheckBounds(minX, maxX, minY, maxY, polyMinX, polyMaxX, polyMinY, polyMaxY);

            if (!boundsIntersect)
            {
                grid.Close();
                Globals.WriteErrorMessage("Polygon and Grid boundaries do not overlap.");
                return false;
            }

            double newXll, newYll, firstXPt, firstYPt;
            int newNumCols, newNumRows, firstCol, firstRow;

            // Check if polygon extents are completely inside of grid extents
            if ((polyMinX >= minX && polyMinX <= maxX) && (polyMaxX >= minX && polyMaxX <= maxX)
                && (polyMinY >= minY && polyMinY <= maxY) && (polyMaxY >= minY && polyMaxY <= maxY))
            {
                MapWinUtility.Logger.Dbg("Poly extents are inside of grid extents.");
                minX = polyMinX;
                minY = polyMinY;
                maxX = polyMaxX;
                maxY = polyMaxY;

                // Find the new number of cols, rows, Xll and Yll values.
                int lastCol, lastRow;

                // Paul Meems, 7 Oct 2011 - Fix for #1987
                // Globals.ProjToCell(minX, maxY, out firstCol, out firstRow, xllCenter, yllCenter, cellWidth, cellHeight, numRows);
                grid.ProjToCell(minX, maxY, out firstCol, out firstRow);

                // Paul Meems, 7 Oct 2011 - Fix for #1987
                // Globals.ProjToCell(maxX, minY, out lastCol, out lastRow, xllCenter, yllCenter, cellWidth, cellHeight, numRows);
                grid.ProjToCell(maxX, minY, out lastCol, out lastRow);

                newNumCols = (lastCol - firstCol) + 1;
                newNumRows = (lastRow - firstRow) + 1;
                MapWinUtility.Logger.Dbg("New numRows = " + newNumRows + " New numCols = " + newNumCols);
                Globals.CellToProj(firstCol, lastRow, out newXll, out newYll, xllCenter, yllCenter, cellWidth, cellHeight, numRows);
                Globals.CellToProj(firstCol, firstRow, out firstXPt, out firstYPt, xllCenter, yllCenter, cellWidth, cellHeight, numRows);
            }
            else if ((minX >= polyMinX && minX <= polyMaxX) && (maxX >= polyMinX && maxX <= polyMaxX)
                && (minY >= polyMinY && minY <= polyMaxY) && (maxY >= polyMinY && maxY <= polyMaxY))
            {
                // Check if grid extents are completely inside of polygon extents
                // Note: there is really no purpose in this, as the new grid is the same
                // as the orgiginal grid....but we'll do it anyway.
                MapWinUtility.Logger.Dbg("Grid extents are inside of polygon extents.");

                // Keep min and max values the same....no need to change them.
                newNumCols = numCols;
                newNumRows = numRows;
                newXll = xllCenter;
                newYll = yllCenter;
                firstCol = 0;
                firstRow = 0;
                Globals.CellToProj(0, 0, out firstXPt, out firstYPt, xllCenter, yllCenter, cellWidth, cellHeight, numRows);
            }
            else
            {
                // Part of polygon lies outside of the grid, find intersecting boundary shape
                MapWinUtility.Logger.Dbg("Grid extents and polygon extents overlap.");

                // Create a new shape out of the grid extents
                Shape gridEnvelope = new ShapeClass();
                gridEnvelope.Create(ShpfileType.SHP_POLYGON);
                
                Point pt = new PointClass { x = minX, y = maxY };
                var pointIndex = 0;
                gridEnvelope.InsertPoint(pt, ref pointIndex);
                
                pt = new PointClass { x = maxX, y = maxY };
                pointIndex = 1;
                gridEnvelope.InsertPoint(pt, ref pointIndex);
                
                pt = new PointClass { x = maxX, y = minY };
                pointIndex = 2;
                gridEnvelope.InsertPoint(pt, ref pointIndex);
                
                pt = new PointClass { x = minX, y = minY };
                pointIndex = 3;
                gridEnvelope.InsertPoint(pt, ref pointIndex);
                
                pt = new PointClass { x = minX, y = maxY };
                pointIndex = 4;
                gridEnvelope.InsertPoint(pt, ref pointIndex);

                // Create the final bounding envelope which is
                // the intersection of the polygon and grid envelope:
                Shape envelope = new ShapeClass();
                envelope.Create(ShpfileType.SHP_POLYGON);
                envelope = SpatialOperations.Intersection(gridEnvelope, poly);

                // Paul Meems: Related to bug 1068. Added check for correct shape:
                if (envelope == null || envelope.numPoints == 0 || !envelope.IsValid)
                {
                    Globals.WriteErrorMessage("Problem creating the bounding envelope. Aborting ClipGrid().");
                    return false;
                }

                // Calculate how many rows and columns will exist within the new grid
                // that is: how many rows/cols fit within the bounding envelope.
                minX = envelope.Extents.xMin;
                minY = envelope.Extents.yMin;
                maxX = envelope.Extents.xMax;
                maxY = envelope.Extents.yMax;

                newNumCols = (int)(((maxX - minX) / cellWidth) + 0.5);
                newNumRows = (int)(((maxY - minY) / cellHeight) + 0.5);
                newXll = minX + (cellWidth / 2);
                newYll = minY + (cellHeight / 2);
                firstXPt = newXll;
                firstYPt = newYll + (cellHeight * (newNumRows - 1));

                // Paul Meems, 7 Oct 2011 - Fix for #1987
                // Globals.ProjToCell(firstXPt, firstYPt, out firstCol, out firstRow, xllCenter, yllCenter, cellWidth, cellHeight, numRows);
                grid.ProjToCell(firstXPt, firstYPt, out firstCol, out firstRow);
                
                // Done using COM objects, release them
                //while (Marshal.ReleaseComObject(pt) != 0)
                //{
                //}

                //while (Marshal.ReleaseComObject(gridEnvelope) != 0)
                //{
                //}

                //while (Marshal.ReleaseComObject(envelope) != 0)
                //{
                //}
            }

            // The right way to copy a grid header:
            GridHeader resultHeader = new GridHeaderClass();
            resultHeader.CopyFrom(grid.Header);
            resultHeader.NumberCols = newNumCols;
            resultHeader.NumberRows = newNumRows;
            resultHeader.XllCenter = newXll;
            resultHeader.YllCenter = newYll;

            // create the new grid object
            Grid resultGrid = new GridClass();
            
            DataManagement.DeleteGrid(ref resultGridfile);

            if (resultGrid.CreateNew(resultGridfile, resultHeader, grid.DataType, grid.Header.NodataValue, InRam, GridFileType.UseExtension, null) == false)
            {
                Globals.WriteErrorMessage("Problem creating the result grid: " +
                                          resultGrid.get_ErrorMsg(resultGrid.LastErrorCode));
                return false;
            }

            // close the grids, we need to use the wrapper class now due to memory issues
            resultGrid.Save(resultGridfile, GridFileType.UseExtension, null);
            resultGrid.Close();
            //while (Marshal.ReleaseComObject(resultGrid) != 0)
            //{
            //}
            
            grid.Close();
            //while (Marshal.ReleaseComObject(grid) != 0)
            //{
            //}

            // Fill the result grid with values from the original grid
            try
            {
                MapWinUtility.Logger.Dbg("newNumRows = " + newNumRows + " newNumCols = " + newNumCols);

                var rowClearCount = Globals.DetermineRowClearCount(newNumRows, newNumCols);

                MapWinUtility.Logger.Dbg("Clearing COM resources every " + rowClearCount + " rows.");

                if (FillGrid(ref inputGridfile, ref resultGridfile, ref poly, newNumRows, newNumCols, rowClearCount, firstCol, firstRow, firstXPt, firstYPt, clipToExtents) == false)
                {
                    MapWinUtility.Logger.Dbg("Error running FillGrid");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Globals.WriteErrorMessage("Exception in ClipGridWithPolygon: " + ex.Message);
                return false;
            }

            DataManagement.CopyGridLegend(inputGridfile, resultGridfile);
            MapWinUtility.Logger.Dbg("Finished ClipGridWithPolygon");
            return true;
        }
        #endregion

        #region private FillGrid() -- used by ClipGridWithPoly()
        /// <summary>
        /// Fills the result grid with values from the input grid.
        /// </summary>
        /// <param name="inputGridfile">The path to the input grid.</param>
        /// <param name="resultGridfile">The path to the result grid.</param>
        /// <param name="poly">The polygon used in clipping.</param>
        /// <param name="newNumRows">The number of rows in the result grid.</param>
        /// <param name="newNumCols">The number of cols in the result grid.</param>
        /// <param name="rowClearCount">The number of rows that should be filled before
        /// the unmanaged resources are disposed of.</param>
        /// <param name="firstCol">The first column of the original grid that corresponds to the first column of the result grid.</param>
        /// <param name="firstRow">The first row of the original grid that corresponds to the first row of the result grid.</param>
        /// <param name="firstXPt">The X value (minX) of the first point in the result grid.</param>
        /// <param name="firstYPt">The Y value (maxY) of the first point in the result grid.(</param>
        /// <param name="clipToExtents">True if clipping to extents rather than polygon shape.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        private static bool FillGrid(ref string inputGridfile, ref string resultGridfile, ref Shape poly, int newNumRows, int newNumCols, int rowClearCount, int firstCol, int firstRow, double firstXPt, double firstYPt, bool clipToExtents)
        {
            const bool InRam = true;
            var status = true;
            var startRow = 0;
            var lastRow = newNumRows - 1;
            var inputGridWrapper = new GridWrapper();
            var resultGridWrapper = new GridWrapper();

            // variables associated with input grid:
            var oldNumCols = 0;
            double cellWidth = 0;
            double cellHeight = 0;

            Globals.Vertex[][] polyVertArray;
            Globals.ConvertPolyToVertexArray(ref poly, out polyVertArray);

            for (var row = startRow; row <= lastRow; row++)
            {
                double yPt;
                if (row == startRow)
                {
                    inputGridWrapper = new GridWrapper();
                    if (inputGridWrapper.Open(inputGridfile, InRam) == false)
                    {
                        status = false;
                        break;
                    }

                    resultGridWrapper = new GridWrapper();
                    if (resultGridWrapper.Open(resultGridfile, InRam) == false)
                    {
                        status = false;
                        break;
                    }

                    if (row == 0)
                    {
                        // initialize variables
                        cellWidth = inputGridWrapper.GetCellWidth();
                        cellHeight = inputGridWrapper.GetCellHeight();
                        oldNumCols = inputGridWrapper.GetNumCols();
                    }

                    yPt = firstYPt - (cellHeight * row);

                    if (FillRow(inputGridWrapper, resultGridWrapper, ref polyVertArray, row, firstXPt, yPt, firstCol, firstRow, cellWidth, newNumCols, oldNumCols, clipToExtents) == false)
                    {
                        status = false;
                        break;
                    }
                }
                else if (row == startRow + rowClearCount)
                {
                    // dispose of COM objects every "rowClearCount" iterations to give memory a chance to be released by GC
                    // TODO: Calculate how often we should dispose of resources (expensive) based on
                    // amount of RAM available and size of grid being created.
                    if (resultGridWrapper != null)
                    {
                        if (!resultGridWrapper.Save(resultGridfile))
                        {
                            status = false;
                            break;
                        }
                    }

                    startRow += rowClearCount;
                    if (inputGridWrapper != null)
                    {
                        inputGridWrapper.Dispose();
                        inputGridWrapper = null;
                    }
                    
                    if (resultGridWrapper != null)
                    {
                        resultGridWrapper.Dispose();
                        resultGridWrapper = null;
                    }
                    
                    row = startRow - 1;
                    continue;
                }
                else
                {
                    yPt = firstYPt - (cellHeight * row);

                    if (FillRow(inputGridWrapper, resultGridWrapper, ref polyVertArray, row, firstXPt, yPt, firstCol, firstRow, cellWidth, newNumCols, oldNumCols, clipToExtents) == false)
                    {
                        status = false;
                        break;
                    }
                }
            } // end of looping through rows

            if (inputGridWrapper != null)
            {
                inputGridWrapper.Dispose();
            }

            if (resultGridWrapper != null)
            {
                if (!resultGridWrapper.Save(resultGridfile))
                {
                    status = false;
                }

                resultGridWrapper.Dispose();
            }

            return status;
        }
        #endregion

        #region private FillRow() -- used by FillGrid()
        /// <summary>
        /// Fills a single row in the result grid with values from the input grid
        /// that fall within the polygon shape (or extents, if clipToExtents == true).
        /// </summary>
        /// <param name="inputGridWrapper">The wrapper object for the input grid.</param>
        /// <param name="resultGridWrapper">The wrapper object for the result grid.</param>
        /// <param name="polyVertArray">Array[numParts][numPointsPerPart] containing the polygon vertices.</param>
        /// <param name="row">The row to be filled.</param>
        /// <param name="firstXPt">The starting x position of the initial row.</param>
        /// <param name="yPt">The starting y position of the row.</param>
        /// <param name="firstCol">The first column of the input grid that will be used for filling.</param>
        /// <param name="firstRow">The row of the input grid that corresponds to the current row of the result grid.</param>
        /// <param name="cellWidth">dX value of the result grid.</param>
        /// <param name="newNumCols">The number of columns in the result grid.</param>
        /// <param name="oldNumCols">The number of columns in the input grid.</param>
        /// <param name="clipToExtents">True if clipping to polygon extents rather than actual polygon shape.</param>
        /// <returns>True on success</returns>
        /// <remarks>Some may wonder why I am calling this as a separate function, after several time tests, 
        /// it was found that it is actually faster to fill the columns in an outside function than by putting 
        /// this same code inside of the fill row function. Also, the Garbage Collector is faster at releasing 
        /// resources by having this as a separate routine. -- Angela Hillier 2/14/06</remarks>
        private static bool FillRow(GridWrapper inputGridWrapper, GridWrapper resultGridWrapper, ref Globals.Vertex[][] polyVertArray, int row, double firstXPt, double yPt, int firstCol, int firstRow, double cellWidth, int newNumCols, int oldNumCols, bool clipToExtents)
        {
            var oldRow = firstRow + row;
            var noDataValue = float.Parse(inputGridWrapper.GetNodataValue().ToString());

            var oldRowArray = new float[oldNumCols];
            if (!inputGridWrapper.GetRow(oldRow, ref oldRowArray))
            {
                Globals.WriteErrorMessage("Failed to get row " + oldRow + " from input grid.");
                return false;
            }

            var newRowArray = new float[newNumCols];
            Array.Copy(oldRowArray, firstCol, newRowArray, 0, newNumCols);
            if (clipToExtents == false)
            {
                var lastNewCol = newNumCols - 1;
                for (var col = 0; col <= lastNewCol; col++)
                {
                    var xPt = firstXPt + (cellWidth * col);
                    if (!Utils.PointInPoly(ref polyVertArray, xPt, yPt))
                    {
                        newRowArray[col] = noDataValue;
                    }
                }
            }

            resultGridWrapper.SetRow(row, ref newRowArray);

            // This produces excessive output in real runs -> MapWinUtility.Logger.Dbg("Finished FillRow");
            return true;
        }
        #endregion
    }
}