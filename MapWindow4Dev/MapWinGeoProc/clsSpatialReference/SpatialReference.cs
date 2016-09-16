//********************************************************************************************************
//File name: SpatialReference.cs
//Description: Public class, changes grid projections. Depends on the external proj4 (proj.dll) library.
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
//8-5-05 Angela Hillier wrote ProjectShapefile() and ProjectGrid() functions and their support methods
//1-13-06 Angela Hillier - added FreeProjPointers() to free memory before exiting functions that call pj_init_plus().
//3-30-06 Angela Hillier - fixed the public function ProjectPoint() so that it would handle long/lat coordinates properly.
//4-14-06 Mark Gray
//          ProjectGrid: replaced MapWinGIS.Point objects with double variables
//                       replaced if statements with Math.Max and Math.Min
//                       combined two "if(trimResult == true)" statements
//          FillGrid: simplified for loop to avoid math in upper bound and "if(col > 0)"
//          ProjectShapefile: simplified by using new ConvertAndProjectPoint
//          ConvertAndProjectPoint: inlined private "ProjectPoint(ref double x, ref double y)"
//4-19-06 Angela Hillier - updated ProjectGrid() functions so that the destination projection string is saved in the result grid's header.
//10-2-07 Dan Ames - updated ProjectImage functions to work with the Bug 429 sample TerraServer grids.
//3-28-08 Jiri Kadlec - fixed function ProjectShapefile() to work with uppercase filenames (like shapefile.SHP)
//********************************************************************************************************
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;


namespace MapWinGeoProc
{
    /// <summary>
    /// Projects all points in a grid or shapefile from the original
    /// coordinate system to a new coordinate system using Proj.4 (proj.dll).
    /// </summary>
    public class SpatialReference
    {
        //Because Proj.4 is not a COM component, we must declare each function
        //that we will be using from the external library.
        //proj.dll must be included in the bin directory of the calling program
        //the nad initialization folder should also be present on the computer.
        [DllImport("proj.dll")]
        private static extern int pj_init_plus(string init); //init should be passed as a c-style string
        [DllImport("proj.dll")]
        private static extern void pj_free(int pointer);
        [DllImport("proj.dll")]
        private static extern int pj_transform(int srcPrj, int destPrj, int nPoints, int offset, ref double x, ref double y, ref double Z);
        [DllImport("proj.dll")]
        private static extern int pj_is_latlong(int coordPointer);

        private const double RAD_TO_DEG = 57.29577951308232;
        private const double DEG_TO_RAD = 0.0174532925199432958;

        private static int srcPrj;  //Pointer to source projection structure in proj.dll
        private static int destPrj; //Pointer to destination projection structure in proj.dll

        private static bool lockPrj = false; //Set to 1 when srcPrj and destPrj are in use

        private static bool convertToRadians; //True if we need to convert to radians before sending to Proj.4
        private static bool convertToDegrees; //True if we need to convert to degrees after leaving Proj.4

        private static string gErrorMsg = "";

        #region ProjectGrid(): save to file version

        /// <summary>
        /// Transforms a grid from one coordinate system to another.
        /// </summary>
        /// <param name="sourceProj">Proj.4 initialization string for input coordinate system.</param>
        /// <param name="destProj">Proj.4 initialization string for output coordinate system.</param>
        /// <param name="inputGrid">The full path to the input grid file.</param>
        /// <param name="resultGrid">The full path to where the result grid file will be saved.</param>
        /// <param name="trimResult">True if user wants excess rows/columns of NoData removed from result. Ensures minimum file size.</param>
        /// <returns>True if projection was successful, false if an error occurs.</returns>
        public static bool ProjectGrid(ref string sourceProj, ref string destProj, ref string inputGrid, ref string resultGrid, bool trimResult)
        {
            MapWinUtility.Logger.Dbg("ProjectGrid(sourceProj: " + sourceProj + ",\n" +
                                     "            destProj: " + destProj + ",\n" +
                                     "            inputGrid: " + inputGrid + ",\n" +
                                     "            resultGrid: " + resultGrid + ",\n" +
                                     "            trimResult: " + trimResult.ToString() + ")");
            return ProjectGridFile(ref sourceProj, ref destProj, ref inputGrid, ref resultGrid, trimResult, null);

        }

        /// <summary>
        /// Transforms a grid from one coordinate system to another.
        /// </summary>
        /// <param name="sourceProj">Proj.4 initialization string for input coordinate system.</param>
        /// <param name="destProj">Proj.4 initialization string for output coordinate system.</param>
        /// <param name="inputGrid">The full path to the input grid file.</param>
        /// <param name="resultGrid">The full path to where the result grid file will be saved.</param>
        /// <param name="trimResult">True if user wants excess rows/columns of NoData removed from result. Ensures minimum file size.</param>
        /// <param name="ICallBack">A MapWinGIS ICallBack for status messages</param>
        /// <returns>True if projection was successful, false if an error occurs.</returns>
        public static bool ProjectGrid(ref string sourceProj, ref string destProj, ref string inputGrid, ref string resultGrid, bool trimResult, MapWinGIS.ICallback ICallBack)
        {
            MapWinUtility.Logger.Dbg("ProjectGrid(sourceProj: " + sourceProj + ",\n" +
                                     "            destProj: " + destProj + ",\n" +
                                     "            inputGrid: " + inputGrid + ",\n" +
                                     "            resultGrid: " + resultGrid + ",\n" +
                                     "            trimResult: " + trimResult.ToString() + ",\n" +
                                     "            ICallback)");
            return ProjectGridFile(ref sourceProj, ref destProj, ref inputGrid, ref resultGrid, trimResult, ICallBack);
        }

        /// <summary>
        /// Reproject the location of labels in the specified file.
        /// </summary>
        /// <param name="src">Source filename (.lbl file)</param>
        /// <param name="dst">Destination filename (.lbl file)</param>
        /// <param name="srcproj">Source projection (proj4 format)</param>
        /// <param name="dstproj">Destination projection (proj4 format)</param>
        public static void ReprojectLabels(string src, string dst, string srcproj, string dstproj)
        {
            // Take every label in src lbl file in srcproj projection, and reproject its location to dstproj and write to dst prj file.
            // Chris Michaelis Jan 21 2009
            System.IO.TextReader fi = new System.IO.StreamReader(src);
            System.IO.TextWriter fo = new System.IO.StreamWriter(dst);
            var xReg = new System.Text.RegularExpressions.Regex("X=\"[0-9]\\.[0-9]\"");
            var yReg = new System.Text.RegularExpressions.Regex("Y=\"[0-9]\\.[0-9]\"");

            var s = fi.ReadLine();
            while (s != null)
            {
                if (s.Contains("X=\"") & s.Contains("Y=\""))
                {
                    double x = 0;
                    double y = 0;
                    double.TryParse(xReg.Matches(s)[0].Value.ToString().Replace("X=\"", "").Replace("\"", ""), out x);
                    double.TryParse(yReg.Matches(s)[0].Value.ToString().Replace("Y=\"", "").Replace("\"", ""), out y);
                    MapWinGeoProc.SpatialReference.ProjectPoint(ref x, ref y, srcproj, dstproj);
                    s = s.Replace(xReg.Matches(s)[0].Value.ToString(), "X=\"" + x.ToString() + "\"");
                    s = s.Replace(yReg.Matches(s)[0].Value.ToString(), "Y=\"" + y.ToString() + "\"");
                    fo.WriteLine(s);
                }
                else
                {
                    //Whatever it is, we don't need to mess with it :)
                    fo.WriteLine(s);
                }
                s = fi.ReadLine();
            }
            fo.Close();
            fi.Close();
        }

        private static bool ProjectGridFile(ref string sourceProj, ref string destProj, ref string inputGrid, ref string resultGrid, bool trimResult, MapWinGIS.ICallback ICallBack)
        {
            MapWinUtility.Logger.Dbg("ProjectGridFile(sourceProj: " + sourceProj + ",\n" +
                                     "            destProj: " + destProj + ",\n" +
                                     "            inputGrid: " + inputGrid + ",\n" +
                                     "            resultGrid: " + resultGrid + ",\n" +
                                     "            trimResult: " + trimResult + ",\n" +
                                     "            ICallback)");

            Error.ClearErrorLog();

            if (sourceProj.Equals(string.Empty))
            {
                gErrorMsg = "The source projection is missing.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                MapWinUtility.Logger.Dbg("Bad Argument: " + gErrorMsg + "\nReturning false.");
                return false;
            }

            if (destProj.Equals(string.Empty))
            {
                gErrorMsg = "The destination projection is missing.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                MapWinUtility.Logger.Dbg("Bad Argument: " + gErrorMsg + "\nReturning false.");
                return false;
            }

            if (File.Exists(inputGrid) == false)
            {
                gErrorMsg = "The path to the input grid is not valid.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                MapWinUtility.Logger.Dbg("Bad Argument: " + gErrorMsg + "\nReturning false.");
                return false;
            }

            if (resultGrid.Equals(string.Empty))
            {
                gErrorMsg = "The path for the result grid is missing.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                MapWinUtility.Logger.Dbg("Bad Argument: " + gErrorMsg + "\nReturning false.");
                return false;
            }
                       
            const bool InRam = true;
            const double DefaultSize = 30;
            bool status;
            var unitsChanged = false; // Flag to set if distances in old and new grids are incomparable
            
            // Added Chris George 14 October 2007
            var toSmaller = false;

            try
            {
                if (InitializeGlobalVariables(ref sourceProj, ref destProj) == false)
                {
                    FreeProjPointers();
                    return false;
                }
                lockPrj = true;
                var fileInfo = new FileInfo(inputGrid);
                var fileExt = fileInfo.Extension;
                Debug.WriteLine("inputGrid extension: " + fileExt);
                var tempPath = Path.GetTempPath() + "tempGrid" + fileExt;
                Debug.WriteLine("tempPath = " + tempPath);
                var tempGridFilePath = tempPath;

                MapWinGIS.Grid grid = new MapWinGIS.GridClass();
                if (!grid.Open(inputGrid, MapWinGIS.GridDataType.UnknownDataType, InRam, MapWinGIS.GridFileType.UseExtension, null))
                {
                    gErrorMsg = "Problem opening the input Grid: " + grid.get_ErrorMsg(grid.LastErrorCode);
                    Debug.WriteLine(gErrorMsg);
                    Error.SetErrorMsg(gErrorMsg);
                    MapWinUtility.Logger.Dbg("Bad Argument: " + gErrorMsg + "\nReturning false.");
                    lockPrj = false;
                    FreeProjPointers();
                    return false;
                }

                var numCols = grid.Header.NumberCols;
                var numRows = grid.Header.NumberRows;

                double lTptX, lTptY;
                double rTptX, rTptY;
                double rBptX, rBptY;
                double lBptX, lBptY;

                var oldXll = grid.Header.XllCenter;
                var oldYll = grid.Header.YllCenter;
                var oldSize = grid.Header.dX;
                var oldDx = grid.Header.dX;
                var oldDy = grid.Header.dY;
                if (oldSize > grid.Header.dY)
                {
                    oldSize = grid.Header.dY;
                }

                double x, y;
                double oldX1;
                double oldX2;

                // Convert the top left corner point
                Globals.CellToProj(0, 0, out x, out y, oldXll, oldYll, oldDx, oldDy, numRows);
                oldX1 = x;
                status = ConvertAndProjectPoint(ref x, ref y);
                if (status)
                {
                    lTptX = x;
                    lTptY = y;

                    // Convert the right top corner point
                    Globals.CellToProj(numCols - 1, 0, out x, out y, oldXll, oldYll, oldDx, oldDy, numRows);
                    oldX2 = x;
                    status = ConvertAndProjectPoint(ref x, ref y);
                    if (status)
                    {
                        rTptX = x;
                        rTptY = y;
                        
                        // Convert the right bottom corner point"
                        Globals.CellToProj(numCols - 1, numRows - 1, out x, out y, oldXll, oldYll, oldDx, oldDy, numRows);
                        status = ConvertAndProjectPoint(ref x, ref y);
                        if (status)
                        {
                            rBptX = x;
                            rBptY = y;

                            // Convert the left bottom corner point:
                            Globals.CellToProj(0, numRows - 1, out x, out y, oldXll, oldYll, oldDx, oldDy, numRows);
                            status = ConvertAndProjectPoint(ref x, ref y);
                            if (status)
                            {
                                lBptX = x;
                                lBptY = y;
                            }
                            else
                            {
                                gErrorMsg = "Problem with projecting left bottom point to new coordinates.";
                                Debug.WriteLine(gErrorMsg);
                                Error.SetErrorMsg(gErrorMsg);
                                MapWinUtility.Logger.Dbg("Application Error: " + gErrorMsg + "\nReturning false.");
                                lockPrj = false;
                                FreeProjPointers();
                                grid.Close();
                                return false;
                            }
                        }
                        else
                        {
                            gErrorMsg = "Problem with projecting right bottom point to new coordinates.";
                            Debug.WriteLine(gErrorMsg);
                            Error.SetErrorMsg(gErrorMsg);
                            MapWinUtility.Logger.Dbg("Application Error: " + gErrorMsg + "\nReturning false.");
                            lockPrj = false;
                            FreeProjPointers();
                            grid.Close();
                            return false;
                        }
                    }
                    else
                    {
                        gErrorMsg = "Problem with projecting right top point to new coordinates.";
                        Debug.WriteLine(gErrorMsg);
                        Error.SetErrorMsg(gErrorMsg);
                        MapWinUtility.Logger.Dbg("Application Error: " + gErrorMsg + "\nReturning false.");
                        lockPrj = false;
                        FreeProjPointers();
                        grid.Close();
                        return false;
                    }
                }
                else
                {
                    gErrorMsg = "Problem with projecting left top point to new coordinates.";
                    Debug.WriteLine(gErrorMsg);
                    MapWinUtility.Logger.Dbg("Application Error: " + gErrorMsg + "\nReturning false.");
                    Error.SetErrorMsg(gErrorMsg);
                    lockPrj = false;
                    FreeProjPointers();
                    grid.Close();
                    return false;
                }

                // The code below uses Distance calculations on the new and old grids
                // which assumes the units of the two grids are the same.
                // This is not the case, for example, if projecting from longlat to utm or vice versa.
                // In that case, calculate the new cell size so as to be approximately the same as
                // the original.
                // This is a temporary fix to deal with longlat - utm conversions only:
                // needs a more general algorithm for getting units of the two projections.
                // Chris George 14 October 2007

                const string Longlat = "+proj=longlat";
                const string Utm = "+proj=utm";
                if ((sourceProj.Contains(Longlat) && destProj.Contains(Utm)) ||
                    (sourceProj.Contains(Utm) && destProj.Contains(Longlat)))
                {
                    unitsChanged = true;
                }
                else
                {
                    // also assume units have changed if one grid is latlong and the other is not
                    // which applies for example in latlong to Albers Equal Area (see bug #1477)
                    // Chris George 10 November 2009
                    if (pj_is_latlong(srcPrj) != pj_is_latlong(destPrj))
                    {
                        unitsChanged = true;
                    }
                }

                // find if we are going to a larger projection or a smaller one
                // if going to a smaller one, be sure to calculate the cell size
                // so that data will not be lost. If going to a larger one, use the
                // default cellSize or the one of the current grid (if larger than default).
                double minX, minY, maxX, maxY, cellSize;

                // Don't compare incomparable distances
                if (!unitsChanged)
                {
                    var oldDist = Distance(oldX1, oldX2);
                    var newDist = Distance(lTptX, rTptX);
                    toSmaller = oldDist > newDist;
                }

                if (unitsChanged || toSmaller)
                {
                    // Find the minimum x and y distances
                    minX = Distance(rTptX, lTptX);
                    maxX = Distance(rBptX, lBptX);
                    if (maxX < minX)
                    {
                        minX = maxX;
                    }

                    minY = Distance(lTptY, lBptY);
                    maxY = Distance(rTptY, rBptY);
                    if (maxY < minY)
                    {
                        minY = maxY;
                    }

                    // Calculate the new width and height
                    double width, height;
                    if (numCols == 0)
                    {
                        width = 0;
                    }
                    else
                    {
                        width = minX / numCols;
                    }

                    if (numRows == 0)
                    {
                        height = 0;
                    }
                    else
                    {
                        height = minY / numRows;
                    }

                    // if within reason, try to match the cell size to the original grid's
                    var cellWidth = width;
                    var cellHeight = height;
                    if (!unitsChanged)
                    {
                        var dX = grid.Header.dX;
                        var dY = grid.Header.dY;
                        var diff = Distance(dX, width);
                        if (diff > 0 && diff < 1)
                        {
                            cellWidth = dX;
                        }
                        else
                        {
                            cellWidth = width;
                        }

                        diff = Distance(dY, height);
                        if (diff > 0 && diff < 1)
                        {
                            cellHeight = dY;
                        }
                        else
                        {
                            cellHeight = height;
                        }
                    }

                    // we want our cells to be square, so set to the minimum size
                    cellSize = cellWidth < cellHeight ? cellWidth : cellHeight;

                    //Debug.WriteLine("new cellSize = " + cellSize);
                }
                else
                {
                    // toSmaller == false
                    // shift the old cell size to an appropriate value
                    if (oldSize < 1)
                    {
                        var newSize = oldSize;
                        
                        // shift to a cell size > 10
                        while (newSize < 10)
                        {
                            newSize = newSize * 10;
                        }

                        var diff = Distance(newSize, DefaultSize);
                        if (diff > 0 && diff < 1)
                        {
                            newSize = DefaultSize;
                        }

                        cellSize = newSize;
                    }
                    else
                    {
                        // either use the default size or the original grid's cellSize;
                        cellSize = oldSize > DefaultSize ? oldSize : DefaultSize;
                    }
                }

                Debug.WriteLine("new cellSize = " + cellSize);

                // find the maximum extents for the new grid
                // this assumes the extents will be within the four corner points
                maxX = Math.Max(rTptX, rBptX);
                maxY = Math.Max(lTptY, rTptY);
                minX = Math.Min(lTptX, lBptX);
                minY = Math.Min(lBptY, rBptY);

                Debug.WriteLine("Extents: (" + minX + ", " + maxY + ") (" + maxX + ", " + maxY + ") (" + maxX + ", " + minY + ") (" + minX + ", " + minY + ")");

                // calculate the number of cells needed to build the grid
                // converting to int rounds down, so add 0.5
                var newNumCols = (int)((Distance(maxX, minX) / cellSize) + 0.5);
                var newNumRows = (int)((Distance(maxY, minY) / cellSize) + 0.5);
                
                // The point:(minX, minY) represents the left-bottom corner of the grid
                // shift accordingly to get the center point of the cell.
                var newXll = minX + (cellSize / 2);
                var newYll = minY + (cellSize / 2);

                Debug.WriteLine("new col number =" + newNumCols + " new row number = " + newNumRows);

                if (newNumCols == 0 || newNumRows == 0)
                {
                    gErrorMsg = "The result grid would have zero rows or columns! Ensure the reprojection operation is valid.";
                    Debug.WriteLine(gErrorMsg);
                    Error.SetErrorMsg(gErrorMsg);
                    MapWinUtility.Logger.Dbg("Application Error: " + gErrorMsg + "\nReturning false.");
                    FreeProjPointers();
                    lockPrj = false;
                    grid.Close();
                    return false;
                }

                // create a header for the new grid
                // Chris M 12/13/2006 for BugZilla 377
                // Below code:
                // The grid header cannot be copied right across like that! The first line creates 
                // a new grid header; the second line deletes the newly created grid header and 
                // copies a reference to the original grid's header. Both grids then are using
                // the same header; and when the last lines set the XllCenter and YllCenter,
                // BOTH grids are updated with that information! A classic example of pointers gone wrong.
                MapWinGIS.GridHeader header = new MapWinGIS.GridHeaderClass
                    {
                        NumberCols = newNumCols,
                        NumberRows = newNumRows,
                        dX = cellSize,
                        dY = cellSize,
                        NodataValue = grid.Header.NodataValue,
                        XllCenter = newXll,
                        YllCenter = newYll,
                        Projection = destProj
                    };

                // Now that we have a header, create the new grid
                MapWinGIS.Grid resultGf = new MapWinGIS.GridClass();

                if (trimResult)
                {
                    // this grid is temporary
                    DataManagement.DeleteGrid(ref tempGridFilePath);
                    if (resultGf.CreateNew(tempGridFilePath, header, grid.DataType, grid.Header.NodataValue, true, MapWinGIS.GridFileType.UseExtension, null) == false)
                    {
                        gErrorMsg = "Problem creating the result Grid: " + resultGf.get_ErrorMsg(resultGf.LastErrorCode);
                        Debug.WriteLine(gErrorMsg);
                        Error.SetErrorMsg(gErrorMsg);
                        MapWinUtility.Logger.Dbg("Application Error: " + gErrorMsg + "\nReturning false.");
                        lockPrj = false;
                        FreeProjPointers();
                        grid.Close();
                        return false;
                    }

                    resultGf.Save(tempGridFilePath, MapWinGIS.GridFileType.UseExtension, null);
                }
                else
                {
                    // this grid is final
                    DataManagement.DeleteGrid(ref resultGrid);
                    if (resultGf.CreateNew(resultGrid, header, grid.DataType, grid.Header.NodataValue, true, MapWinGIS.GridFileType.UseExtension, null) == false)
                    {
                        gErrorMsg = "Problem creating the result grid: " + resultGf.get_ErrorMsg(resultGf.LastErrorCode);
                        Debug.WriteLine(gErrorMsg);
                        Error.SetErrorMsg(gErrorMsg);
                        MapWinUtility.Logger.Dbg("Application Error: " + gErrorMsg + "\nReturning false.");
                        lockPrj = false;
                        FreeProjPointers();
                        grid.Close();
                        return false;
                    }

                    resultGf.Save(resultGrid, MapWinGIS.GridFileType.UseExtension, null);
                }

                // close down grids and release unmanaged resources
                // need to work through the gridWrapper class now due to issues with memory
                //while (Marshal.ReleaseComObject(header) != 0)
                //{
                //}

                grid.Close();
                //while (Marshal.ReleaseComObject(grid) != 0)
                //{
                //}

                //grid = null;
                resultGf.Close();
                //while (Marshal.ReleaseComObject(resultGf) != 0)
                //{
                //}

                //resultGf = null;

                // must switch projections, we need to "back project" into the old grid
                SwitchProjections();

                // FILL IN RESULT GRID
                var rowClearCount = Globals.DetermineRowClearCount(newNumRows, newNumCols);
                Debug.WriteLine("Clearing COM resources every " + rowClearCount + " rows.");

                if (trimResult)
                {
                    if (FillGrid(
                        ref inputGrid,
                        ref tempGridFilePath,
                        newNumRows,
                        newNumCols,
                        rowClearCount,
                        newXll,
                        newYll,
                        cellSize,
                        oldXll,
                        oldYll,
                        oldDx,
                        oldDy,
                        ICallBack) == false)
                    {
                        return false;
                    }

                    // Now trim any excess rows and columns that contain values of only NoData
                    if (Globals.TrimGrid(ref tempGridFilePath, ref resultGrid) == false)
                    {
                        gErrorMsg = "Error trimming result grid.";
                        MapWinUtility.Logger.Dbg("Application Error: " + gErrorMsg + "\nReturning false.");
                        Debug.WriteLine(gErrorMsg);
                        Error.SetErrorMsg(gErrorMsg);
                        status = false;
                    }

                    DataManagement.DeleteGrid(ref tempGridFilePath);
                }
                else
                {
                    if (FillGrid(
                        ref inputGrid,
                        ref resultGrid,
                        newNumRows,
                        newNumCols,
                        rowClearCount,
                        newXll,
                        newYll,
                        cellSize,
                        oldXll,
                        oldYll,
                        oldDx,
                        oldDy,
                        ICallBack) == false)
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                if (e.Message.Equals("latitude or longitude exceeded limits"))
                {
                    gErrorMsg = "Could not project grid point. Perhaps it was already projected?";
                    Debug.WriteLine(gErrorMsg);
                    Error.SetErrorMsg(gErrorMsg);
                    MapWinUtility.Logger.Dbg("Application Error: " + gErrorMsg + "\nReturning false.");
                }
                else
                {
                    gErrorMsg = e.Message + e;
                    Debug.WriteLine(gErrorMsg);
                    Error.SetErrorMsg(gErrorMsg);
                }
                status = false;
            }

            // free memory
            lockPrj = false;
            FreeProjPointers();

            try
            {
                DataManagement.CopyGridLegend(inputGrid, resultGrid);
            }
            catch
            {
            }

            return status;
        }
        #endregion

        #region private FillGrid() -- used by ProjectGrid
        /// <summary>
        /// Fills the result grid with values from the input grid.
        /// </summary>
        /// <param name="inputGF">The path to the input grid.</param>
        /// <param name="resultGF">The path to the result grid.</param>
        /// <param name="newNumRows">The number of rows in the result grid.</param>
        /// <param name="newNumCols">The number of cols in the result grid.</param>
        /// <param name="rowClearCount">The number of rows that should be filled before
        /// <param name="newXll">The Xll center of the result grid.</param>
        /// <param name="newYll">The Yll center of the result grid.</param>
        /// <param name="cellSize">The calculated size of a cell in the result grid.</param>
        /// <param name="oldXll">The Xll center of the input grid.</param>
        /// <param name="oldYll">The Yll center of the input grid.</param>
        /// <param name="oldDx">The dX value of the input grid.</param>
        /// <param name="oldDy">The dY value of the input grid.</param>
        /// <param name="ICallBack">A MapWinGIS ICallback for status messages</param>
        /// the unmanaged resources are disposed of.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        private static bool FillGrid(ref string inputGF, ref string resultGF, int newNumRows, int newNumCols, int rowClearCount, double newXll, double newYll, double cellSize,
            double oldXll, double oldYll, double oldDx, double oldDy, MapWinGIS.ICallback ICallBack)
        {
            MapWinUtility.Logger.Dbg("FillGrid(inputGF: " + inputGF + ",\n" +
                                     "         resultGF: " + resultGF + ",\n" +
                                     "         newNumRows: " + newNumRows.ToString() + ",\n" +
                                     "         newNumCols: " + newNumCols.ToString() + ",\n" +
                                     "         rowClearCount: " + rowClearCount.ToString() + ",\n" +
                                     "         newXll: " + newXll.ToString() + ",\n" +
                                     "         newYll: " + newYll.ToString() + ",\n" +
                                     "         cellSize: " + cellSize.ToString() + ",\n" +
                                     "         oldXll: " + oldXll.ToString() + ",\n" +
                                     "         oldYll: " + oldYll.ToString() + ",\n" +
                                     "         oldDx: " + oldDx.ToString() + ",\n" +
                                     "         oldDy: " + oldDy.ToString() + ",\n" +
                                     "         ICallback)");
            bool status = true;
            int startRow = newNumRows - 1;
            var inputGW = new GridWrapper();
            var resultGW = new GridWrapper();
            double colPt = newXll;
            double rowPt = newYll;
            
            // variables associated with input grid
            object noDataValue = null;
            int oldNumRows = 0;
            int oldNumCols = 0;
            int OldProg = -1;
            for (var row = startRow; row >= 0; row--)
            {
                if (row == startRow)
                {
                    inputGW = new GridWrapper();
                    if (inputGW.Open(inputGF, true) == false)
                    {
                        status = false;
                        break;
                    }

                    resultGW = new GridWrapper();
                    if (resultGW.Open(resultGF, true) == false)
                    {
                        status = false;
                        break;
                    }

                }
                if (row == newNumRows - 1)
                {
                    //initialize variables
                    noDataValue = inputGW.GetNodataValue();
                    oldNumCols = inputGW.GetNumCols();
                    oldNumRows = inputGW.GetNumRows();
                }
                //dispose of COM objects every "rowClearCount" iterations to give memory a chance to be released by GC
                else if (row == startRow - rowClearCount)
                {
                    if (resultGW.Save(resultGF) == false)
                    {
                        status = false;
                        break;
                    }
                    startRow -= rowClearCount;
                    inputGW.Dispose();
                    inputGW = null;
                    resultGW.Dispose();
                    resultGW = null;
                    row = startRow + 1;
                    continue;
                }

                colPt = newXll;
                if (row != newNumRows - 1)
                {
                    //point to next row
                    rowPt += cellSize;
                }

                double x, y;
                int oldCol, oldRow;

                for (int col = 0; col < newNumCols; col++)
                {
                    x = colPt;
                    y = rowPt;

                    //status = true;
                    //x = oldXll;
                    //y = oldYll;

                    status = ConvertAndProjectPoint(ref x, ref y);
                    if (status)
                    {
                        //grid.ProjToCell(x, y, out oldCol, out oldRow);
                        //Globals.ProjToCell(x, y, out oldCol, out oldRow, oldXll, oldYll, oldDx, oldDy, oldNumRows);

                        if (oldDx != 0.0 && oldDy != 0.0)
                        {
                            // cwg 10/10/2011 Fix for #1997
                            // code for col was rounding before division
                            oldCol = (int)(Math.Round((x - oldXll) / oldDx));
                            oldRow = (int)(oldNumRows - Math.Round((y - oldYll) / oldDy) - 1);
                        }
                        else
                        {
                            oldCol = -1;
                            oldRow = -1;
                        }

                        if (oldCol <= -1 || oldRow <= -1 || oldCol >= oldNumCols || oldRow >= oldNumRows)
                        {
                            // new point lies outside the bounds of the old grid, set value to NoData
                            resultGW.SetValue(col, row, noDataValue);
                        }
                        else
                        {
                            resultGW.SetValue(col, row, inputGW.GetValue(oldCol, oldRow));
                        }
                    }
                    else
                    {
                        // Remmarked by Ted on 8/9/06 because if one point fails I don't see the benefit
                        // of exiting the entire row, only to resume on the next row.  Either exit the
                        // function or pick up at the next point.
                        //break;
                    }
                    colPt += cellSize;
                }//end of looping through columns

                if (ICallBack != null)
                {
                    var Prog = (int)(100 * (startRow - row) / startRow);
                    if (Prog > OldProg)
                    {
                        if (Prog > 100) Prog = 100;
                        ICallBack.Progress("ProjectGrid", Prog, "Projecting Grid..." + Prog + "%");
                        OldProg = Prog;
                    }
                }
                else
                {
                    MapWinUtility.Logger.Progress(++OldProg, newNumRows);
                }
            }//end of looping through rows

            if (inputGW != null)
            {
                inputGW.Dispose();
                inputGW = null;
            }
            if (resultGW != null)
            {
                if (resultGW.Save(resultGF) == false)
                {
                    status = false;
                }
                resultGW.Dispose();
                resultGW = null;
            }

            return status;
        }
        #endregion

        #region ProjectGrid(): in memory version
        /// <summary>
        /// Transforms a grid from one coordinate system to another.
        /// For large grids (>50MB) use the save-to-file version of this function. 
        /// </summary>
        /// <param name="sourceProj">Proj.4 initialization string for input coordinate system.</param>
        /// <param name="destProj">Proj.4 initialization string for output coordinate system.</param>
        /// <param name="inputGrid">The reference to the in-memory input grid.</param>
        /// <param name="resultGrid">The output parameter resultGrid which will reside in memory.</param>
        /// <param name="trimResult">True if user wants excess rows/columns of NoData removed from result. Ensures minimum file size.</param>
        /// <returns>True if projection was successful, false if an error occurs.</returns>
        public static bool ProjectGrid(ref string sourceProj, ref string destProj, ref MapWinGIS.Grid inputGrid, out MapWinGIS.Grid resultGrid, bool trimResult)
        {
            MapWinUtility.Logger.Dbg("ProjectGrid(sourceProj: " + sourceProj + ",\n" +
                                     "destProj: " + destProj + "\n," +
                                     "inputGrid: " + Macro.ParamName(inputGrid) + "\n" +
                                     "resultGrid: [out]\n" +
                                     "trimResult: + " + trimResult.ToString() + ")");
            return memory_ProjectGrid(ref sourceProj, ref destProj, ref inputGrid, out resultGrid, trimResult, null);
        }
        /// <summary>
        /// Transforms a grid from one coordinate system to another.
        /// For large grids (>50MB) use the save-to-file version of this function. 
        /// </summary>
        /// <param name="sourceProj">Proj.4 initialization string for input coordinate system.</param>
        /// <param name="destProj">Proj.4 initialization string for output coordinate system.</param>
        /// <param name="inputGrid">The reference to the in-memory input grid.</param>
        /// <param name="resultGrid">The output parameter resultGrid which will reside in memory.</param>
        /// <param name="trimResult">True if user wants excess rows/columns of NoData removed from result. Ensures minimum file size.</param>
        /// <param name="ICallBack">A MapWinGIS.ICallback for progress messages </param>
        /// <returns>True if projection was successful, false if an error occurs.</returns>
        public static bool ProjectGrid(ref string sourceProj, ref string destProj, ref MapWinGIS.Grid inputGrid, out MapWinGIS.Grid resultGrid, bool trimResult, MapWinGIS.ICallback ICallBack)
        {
            return memory_ProjectGrid(ref sourceProj, ref destProj, ref inputGrid, out resultGrid, trimResult, ICallBack);
        }

        /// <summary>
        /// In memory version
        /// </summary>
        /// <param name="sourceProj">Source Projection </param>
        /// <param name="destProj">Destination Projection</param>
        /// <param name="inputGrid">MapWinGIS.Grid to project</param>
        /// <param name="resultGrid">MapWinGIS.Grid after projection</param>
        /// <param name="trimResult">Boolean True if the result should be trimmed</param>
        /// <param name="ICallBack">A MapWinGIS.ICallback for status and error messages</param>
        /// <returns></returns>
        public static bool memory_ProjectGrid(ref string sourceProj, ref string destProj, ref MapWinGIS.Grid inputGrid, out MapWinGIS.Grid resultGrid, bool trimResult, MapWinGIS.ICallback ICallBack)
        {
            MapWinGeoProc.Error.ClearErrorLog();
            MapWinGIS.Grid rGrid = new MapWinGIS.GridClass();

            var extStart = inputGrid.Filename.IndexOf(".", 0);
            var fileEXT = inputGrid.Filename.Substring(extStart);
            Debug.WriteLine("inputGrid extension: " + fileEXT);
            var tempPath = System.IO.Path.GetTempPath();
            var resultGridFile = tempPath + "tempGF1" + fileEXT;//@"C:\temp\resultGrid.bgd";

            if (sourceProj.Equals(""))
            {
                gErrorMsg = "The source projection is missing.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                resultGrid = rGrid;
                return false;
            }
            if (destProj.Equals(""))
            {
                gErrorMsg = "The destination projection is missing.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                resultGrid = rGrid;
                return false;
            }

            var status = true;
            var toSmaller = false;
            var unitsChanged = false; // Flag to set if distances in old and new grids are incomparable
            // Added Chris George 14 October 2007
            double defaultSize = 30;

            try
            {
                if (InitializeGlobalVariables(ref sourceProj, ref destProj) == false)
                {
                    resultGrid = rGrid;
                    FreeProjPointers();
                    return false;
                }
                else
                {
                    lockPrj = true;
                    var numCols = inputGrid.Header.NumberCols;
                    var numRows = inputGrid.Header.NumberRows;

                    MapWinGIS.Point LTpt = new MapWinGIS.PointClass();
                    MapWinGIS.Point RTpt = new MapWinGIS.PointClass();
                    MapWinGIS.Point RBpt = new MapWinGIS.PointClass();
                    MapWinGIS.Point LBpt = new MapWinGIS.PointClass();

                    var oldXll = inputGrid.Header.XllCenter;
                    var oldYll = inputGrid.Header.YllCenter;
                    var oldSize = inputGrid.Header.dX;
                    if (oldSize > inputGrid.Header.dY)
                    {
                        oldSize = inputGrid.Header.dY;
                    }
                    double x, y;
                    double oldX1 = 0;
                    double oldX2 = 0;

                    //convert the top left corner point
                    inputGrid.CellToProj(0, 0, out x, out y);
                    //					x = oldXll - oldSize/2;
                    //					y = oldYll + oldSize * (numRows-1) + oldSize/2;
                    oldX1 = x;
                    status = ConvertAndProjectPoint(ref x, ref y);
                    if (status == true)
                    {
                        LTpt.x = x;
                        LTpt.y = y;
                        inputGrid.CellToProj(numCols - 1, 0, out x, out y);
                        //convert the right top corner point
                        //						x = oldXll + oldSize * (numCols-1) + oldSize/2;
                        //						y = oldYll + oldSize * (numRows-1) + oldSize/2;
                        oldX2 = x;
                        status = ConvertAndProjectPoint(ref x, ref y);
                        if (status == true)
                        {
                            RTpt.x = x;
                            RTpt.y = y;
                            //convert the right bottom corner point
                            //							x = oldXll + oldSize * (numCols-1) + oldSize/2;
                            //							y = oldYll - oldSize/2;
                            inputGrid.CellToProj(numCols - 1, numRows - 1, out x, out y);
                            status = ConvertAndProjectPoint(ref x, ref y);
                            if (status == true)
                            {
                                RBpt.x = x;
                                RBpt.y = y;
                                //convert the left bottom corner point
                                //								x = oldXll - oldSize/2;
                                //								y = oldYll - oldSize/2;
                                inputGrid.CellToProj(0, numRows - 1, out x, out y);
                                status = ConvertAndProjectPoint(ref x, ref y);
                                if (status == true)
                                {
                                    LBpt.x = x;
                                    LBpt.y = y;
                                }
                                else
                                {
                                    gErrorMsg = "Problem with projecting left bottom point to new coordinates.";
                                    Debug.WriteLine(gErrorMsg);
                                    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                                    resultGrid = rGrid;
                                    lockPrj = false;
                                    FreeProjPointers();
                                    return false;
                                }
                            }
                            else
                            {
                                gErrorMsg = "Problem with projecting right bottom point to new coordinates.";
                                Debug.WriteLine(gErrorMsg);
                                MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                                resultGrid = rGrid;
                                lockPrj = false;
                                FreeProjPointers();
                                return false;
                            }
                        }
                        else
                        {
                            gErrorMsg = "Problem with projecting right top point to new coordinates.";
                            Debug.WriteLine(gErrorMsg);
                            MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                            resultGrid = rGrid;
                            lockPrj = false;
                            FreeProjPointers();
                            return false;
                        }
                    }
                    else
                    {
                        gErrorMsg = "Problem with projecting left top point to new coordinates.";
                        Debug.WriteLine(gErrorMsg);
                        MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                        resultGrid = rGrid;
                        lockPrj = false;
                        FreeProjPointers();
                        return false;
                    }

                    // The code below uses Distance calculations on the new and old grids
                    // which assumes the units of the two grids are the same.
                    // This is not the case, for example, if projecting from longlat to utm or vice versa.
                    // In that case, calculate the new cell size so as to be approximately the same as 
                    // the original.
                    // This is a temporary fix to deal with longlat - utm conversions only:
                    // needs a more general algorithm for getting units of the two projections.
                    // Chris George 14 October 2007

                    var longlat = "+proj=longlat";
                    var utm = "+proj=utm";
                    if ((sourceProj.Contains(longlat) && destProj.Contains(utm)) ||
                        (sourceProj.Contains(utm) && destProj.Contains(longlat)))
                    {
                        unitsChanged = true;
                    }
                    else
                    {
                        // also assume units have changed if one grid is latlong and the other is not
                        // which applies for example in latlong to Albers Equal Area (see bug #1477)
                        // Chris George 10 November 2009
                        if (pj_is_latlong(srcPrj) != pj_is_latlong(destPrj))
                            unitsChanged = true;
                    }

                    //find if we are going to a larger projection or a smaller one
                    //if going to a smaller one, be sure to calculate the cell size
                    //so that data will not be lost. If going to a larger one, use the
                    //default cellSize or the one of the current grid (if larger than default).
                    double minX, minY, maxX, maxY, cellSize;
                    // Don't compare incomparable distances
                    if (!unitsChanged)
                    {
                        var oldDist = Distance(oldX1, oldX2);
                        var newDist = Distance(LTpt.x, RTpt.x);
                        if (oldDist > newDist)
                        {
                            toSmaller = true;
                        }
                        else
                        {
                            toSmaller = false;
                        }
                    }

                    if (unitsChanged || (toSmaller == true))
                    {
                        //find the minimum x and y distances
                        minX = Distance(RTpt.x, LTpt.x);
                        maxX = Distance(RBpt.x, LBpt.x);
                        if (maxX < minX)
                        {
                            minX = maxX;
                        }
                        minY = Distance(LTpt.y, LBpt.y);
                        maxY = Distance(RTpt.y, RBpt.y);
                        if (maxY < minY)
                        {
                            minY = maxY;
                        }

                        //calculate the new width and height
                        double width, height;
                        if (numCols == 0)
                        {
                            width = 0;
                        }
                        else
                        {
                            width = (minX / numCols);
                        }
                        if (numRows == 0)
                        {
                            height = 0;
                        }
                        else
                        {
                            height = (minY / numRows);
                        }

                        //if within reason, try to match the cell size to the original grid's
                        var cellWidth = width;
                        var cellHeight = height;
                        if (!unitsChanged)
                        {
                            var dX = inputGrid.Header.dX;
                            var dY = inputGrid.Header.dY;
                            var diff = Distance(dX, width);
                            if (diff > 0 && diff < 1)
                            {
                                cellWidth = dX;
                            }
                            else
                            {
                                cellWidth = width;
                            }
                            diff = Distance(dY, height);
                            if (diff > 0 && diff < 1)
                            {
                                cellHeight = dY;
                            }
                            else
                            {
                                cellHeight = height;
                            }
                        }

                        //we want our cells to be square, so set to the minimum size
                        if (cellWidth < cellHeight)
                        {
                            cellSize = cellWidth;
                        }
                        else
                        {
                            cellSize = cellHeight;
                        }

                    }
                    else //we need to use a larger (or equal) cell size
                    {
                        //shift the old cell size to an appropriate value
                        if (oldSize < 1)
                        {
                            var newSize = oldSize;
                            //shift to a cell size > 10
                            while (newSize < 10)
                            {
                                newSize = newSize * 10;
                            }
                            var diff = Distance(newSize, defaultSize);
                            if (diff > 0 && diff < 1)
                            {
                                newSize = defaultSize;
                            }
                            cellSize = newSize;
                        }
                        //either use the default size or the original grid's cellSize;
                        else
                        {
                            if (oldSize > defaultSize)
                            {
                                cellSize = oldSize;
                            }
                            else
                            {
                                cellSize = defaultSize;
                            }
                        }
                    }

                    //Debug.WriteLine("new cellSize = " + cellSize);				

                    //Debug.WriteLine("new cellWidth = " + cellWidth + " new cellHeight = " + cellHeight);

                    //find the maximum extents for the new grid
                    //this assumes the extents will be within the four corner points
                    maxX = RTpt.x;
                    if (RBpt.x > maxX)
                    {
                        maxX = RBpt.x;
                    }
                    maxY = LTpt.y;
                    if (RTpt.y > maxY)
                    {
                        maxY = RTpt.y;
                    }
                    minX = LTpt.x;
                    if (LBpt.x < minX)
                    {
                        minX = LBpt.x;
                    }
                    minY = LBpt.y;
                    if (RBpt.y < minY)
                    {
                        minY = RBpt.y;
                    }
                    //set the extent points
                    LTpt.x = minX;
                    LTpt.y = maxY;
                    RTpt.x = maxX;
                    RTpt.y = maxY;
                    RBpt.x = maxX;
                    RBpt.y = minY;
                    LBpt.x = minX;
                    LBpt.y = minY;

                    //Debug.WriteLine("Extents: (" + minX + ", " + maxY + ") (" + maxX + ", " + maxY + ") (" + maxX + ", " + minY + ") (" + minX + ", " + minY + ")");

                    //calculate the number of cells needed to build the grid
                    //converting to int rounds down, so add 0.5
                    var colNum = (int)((Distance(maxX, minX) / cellSize) + 0.5);
                    var rowNum = (int)((Distance(maxY, minY) / cellSize) + 0.5);

                    //Debug.WriteLine("new col number =" + colNum + " new row number = " + rowNum);

                    //create a header for the new grid
                    // Chris M 12/13/2006 for BugZilla 377
                    // Below code:
                    // The grid header cannot be copied right across like that! The first line creates 
                    // a new grid header; the second line deletes the newly created grid header and 
                    // copies a reference to the original grid's header. Both grids then are using
                    // the same header; and when the last lines set the XllCenter and YllCenter,
                    // BOTH grids are updated with that information! A classic example of pointers gone wrong.

                    MapWinGIS.GridHeader header = new MapWinGIS.GridHeaderClass();
                    // No! --> header = inputGrid.Header;
                    // No reason to CopyFrom either, since entirely new values will be set. Why was it
                    // copied in the first place?
                    header.NumberCols = colNum;
                    header.NumberRows = rowNum;
                    header.dX = cellSize;
                    header.dY = cellSize;
                    //LBpt represents left-bottom corner of the grid
                    //shift accordingly to get the center.
                    header.XllCenter = LBpt.x + cellSize / 2;
                    header.YllCenter = LBpt.y + cellSize / 2;
                    header.Projection = destProj;

                    //Now that we have a header, create the new grid
                    MapWinGIS.Grid resultGF = new MapWinGIS.GridClass();
                    if (trimResult == true)//this grid is temporary and will only exist in memory
                    {
                        var tempGrid = tempPath + "tempGF2" + fileEXT;//@"C:\Temp\tempGrid.bgd";
                        DataManagement.DeleteGrid(ref tempGrid);
                        if (resultGF.CreateNew(tempGrid, header, inputGrid.DataType, inputGrid.Header.NodataValue, true, MapWinGIS.GridFileType.UseExtension, null) == false)
                        {
                            gErrorMsg = "Problem creating the result Grid: " + resultGF.get_ErrorMsg(resultGF.LastErrorCode);
                            Debug.WriteLine(gErrorMsg);
                            MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                            resultGrid = rGrid;
                            lockPrj = false;
                            FreeProjPointers();
                            return false;
                        }
                    }
                    else //this grid will be the final version and will be our 'out' grid
                    {
                        //delete the old grid files of the same name
                        DataManagement.DeleteGrid(ref resultGridFile);
                        if (resultGF.CreateNew(resultGridFile, header, inputGrid.DataType, inputGrid.Header.NodataValue, true, MapWinGIS.GridFileType.UseExtension, null) == false)
                        {
                            gErrorMsg = "Problem creating the result Grid: " + resultGF.get_ErrorMsg(resultGF.LastErrorCode);
                            Debug.WriteLine(gErrorMsg);
                            MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                            resultGrid = rGrid;
                            lockPrj = false;
                            FreeProjPointers();
                            return false;
                        }
                    }

                    //must switch projections, we need to "back project" into the old grid
                    int tempPrj;
                    tempPrj = srcPrj;
                    srcPrj = destPrj;
                    destPrj = tempPrj;
                    tempPrj = 0;
                    if (pj_is_latlong(destPrj) != 0)
                    {
                        //Debug.WriteLine("destination is latlong");
                        convertToDegrees = true;
                        //need to convert from radians to degrees after transforming
                    }
                    else
                    {
                        convertToDegrees = false;
                    }
                    if (pj_is_latlong(srcPrj) != 0)
                    {
                        //Debug.WriteLine("Source is latlong");
                        convertToRadians = true;
                        //need to convert degrees to radians before transforming
                    }
                    else
                    {
                        convertToRadians = false;
                    }

                    //Fill in new grid starting form left bottom corner
                    //and working upwards
                    //Note: Do not start from the top corner (col=0,row=0) and go down, 
                    //it doesn't work for certain projections (WGS84).
                    int oldCol, oldRow;
                    var cPt = resultGF.Header.XllCenter;
                    var rPt = resultGF.Header.YllCenter;
                    var OldProg = -1;
                    for (var row = rowNum - 1; row >= 0; row--)
                    {
                        cPt = resultGF.Header.XllCenter;
                        if (row != rowNum - 1)
                        {
                            rPt = rPt + cellSize;
                        }

                        for (var col = 0; col <= colNum - 1; col++)
                        {
                            if (col > 0)
                            {
                                cPt = cPt + cellSize;
                            }
                            x = cPt;
                            y = rPt;
                            status = ConvertAndProjectPoint(ref x, ref y);
                            if (status == true)
                            {
                                inputGrid.ProjToCell(x, y, out oldCol, out oldRow);
                                if (oldCol <= -1 || oldRow <= -1 || oldCol >= numCols || oldRow >= numRows)
                                {
                                    //new point lies outside the bounds of the old grid, set value to NoData
                                    resultGF.set_Value(col, row, inputGrid.Header.NodataValue);
                                }
                                else
                                {
                                    resultGF.set_Value(col, row, inputGrid.get_Value(oldCol, oldRow));
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (status == false)
                        {
                            break;
                        }
                        if (ICallBack != null)
                        {
                            var Prog = 100 * (rowNum - row) / rowNum;
                            if (Prog > OldProg)
                            {
                                if (Prog > 100) Prog = 100;
                                ICallBack.Progress("ProjectGrid", Prog, "Projecting Grid..." + Prog + "% Complete");
                                OldProg = Prog;
                            }

                        }
                    }

                    if (trimResult == true)
                    {
                        //Now trim any excess rows and columns that contain values of only NoData
                        var noData = Convert.ToDouble(resultGF.Header.NodataValue);
                        var firstRowLoc = 0;
                        var lastRowLoc = rowNum - 1;
                        var firstColLoc = 0;
                        var lastColLoc = colNum - 1;
                        var found = false;
                        //Find which row contains the first valid Y points (yMax).
                        for (var row = 0; row <= rowNum - 1; row++)
                        {
                            for (var col = 0; col <= colNum - 1; col++)
                            {
                                if (Convert.ToDouble(resultGF.get_Value(col, row)) == noData)
                                {
                                    continue;
                                }
                                else
                                {
                                    firstRowLoc = row;
                                    found = true;
                                    break;
                                }
                            }
                            if (found == true)
                            {
                                break;
                            }
                        }
                        //Find which row contains the last valid Y points (yMin)
                        found = false;
                        for (var row = rowNum - 1; row >= 0; row--)
                        {
                            for (var col = 0; col <= colNum - 1; col++)
                            {
                                if (Convert.ToDouble(resultGF.get_Value(col, row)) == noData)
                                {
                                    continue;
                                }
                                else
                                {
                                    lastRowLoc = row;
                                    found = true;
                                    break;
                                }
                            }
                            if (found == true)
                            {
                                break;
                            }
                        }
                        //find the column containing the first valid point (xMin)
                        found = false;
                        for (var col = 0; col <= colNum - 1; col++)
                        {
                            for (var row = 0; row <= rowNum - 1; row++)
                            {
                                if (Convert.ToDouble(resultGF.get_Value(col, row)) == noData)
                                {
                                    continue;
                                }
                                else
                                {
                                    firstColLoc = col;
                                    found = true;
                                    break;
                                }
                            }
                            if (found == true)
                            {
                                break;
                            }
                        }
                        //find the column containing the last valid point (xMax)
                        found = false;
                        for (var col = colNum - 1; col >= 0; col--)
                        {
                            for (var row = 0; row <= rowNum - 1; row++)
                            {
                                if (Convert.ToDouble(resultGF.get_Value(col, row)) == noData)
                                {
                                    continue;
                                }
                                else
                                {
                                    lastColLoc = col;
                                    found = true;
                                    break;
                                }
                            }
                            if (found == true)
                            {
                                break;
                            }
                        }
                        //now make a new grid with the adjusted dimensions
                        // Chris M 12/13/2006 for BugZilla 377
                        // Below code:
                        // The grid header cannot be copied right across like that! The first line creates 
                        // a new grid header; the second line deletes the newly created grid header and 
                        // copies a reference to the original grid's header. Both grids then are using
                        // the same header; and when the last lines set the XllCenter and YllCenter,
                        // BOTH grids are updated with that information! A classic example of pointers gone wrong.

                        MapWinGIS.GridHeader rHeader = new MapWinGIS.GridHeaderClass();
                        // Invalid. Don't do this. --> rHeader = resultGF.Header;
                        var newColNum = (colNum - firstColLoc) - (colNum - lastColLoc) + 1;
                        var newRowNum = (rowNum - firstRowLoc) - (rowNum - lastRowLoc) + 1;
                        rHeader.NumberCols = newColNum;
                        rHeader.NumberRows = newRowNum;
                        //resultGF.CellToProj(firstColLoc, lastRowLoc, out x, out y);
                        rHeader.XllCenter = resultGF.Header.XllCenter + (firstColLoc * cellSize);
                        rHeader.YllCenter = resultGF.Header.YllCenter + ((rowNum - 1 - lastRowLoc) * cellSize);
                        //Debug.WriteLine("new XllCenter: " + x + " new YllCenter: " + y );
                        //Debug.WriteLine("Should have: " + newColNum + " cols and " + newRowNum + " rows.");
                        rHeader.dX = resultGF.Header.dX;
                        rHeader.dY = resultGF.Header.dY;
                        rHeader.Projection = resultGF.Header.Projection;

                        //create the final grid
                        MapWinGIS.Grid result = new MapWinGIS.GridClass();
                        //delete the old grid files of the same name
                        DataManagement.DeleteGrid(ref resultGridFile);
                        if (result.CreateNew(resultGridFile, rHeader, inputGrid.DataType, inputGrid.Header.NodataValue, true, MapWinGIS.GridFileType.UseExtension, null) == false)
                        {
                            gErrorMsg = "Problem creating the result grid: " + result.get_ErrorMsg(result.LastErrorCode);
                            Debug.WriteLine(gErrorMsg);
                            MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                            resultGrid = rGrid;
                            lockPrj = false;
                            FreeProjPointers();
                            return false;
                        }

                        //fill in the new grid with values from resultGF (our temporary grid)
                        var rowPosition = lastRowLoc;
                        var colPosition = firstColLoc;
                        for (var row = newRowNum - 1; row >= 0; row--)
                        {
                            for (var col = 0; col <= newColNum - 1; col++)
                            {
                                result.set_Value(col, row, resultGF.get_Value(colPosition, rowPosition));

                                if (col == newColNum - 1)
                                {
                                    colPosition = firstColLoc;
                                }
                                else
                                {
                                    colPosition++;
                                }
                            }
                            rowPosition--;
                        }//end of for loop
                        rGrid = result;
                    }//end of if trimResult == true
                    resultGrid = rGrid;
                }//end of else sourceProj != 0
            }//end of try
            catch (Exception e)
            {
                if (e.Message.Equals("latitude or longitude exceeded limits"))
                {
                    gErrorMsg = "Could not project grid point. Perhaps it was already projected?";
                    Debug.WriteLine(gErrorMsg);
                    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                }
                else
                {
                    gErrorMsg = e.Message + e.ToString();
                    Debug.WriteLine(gErrorMsg);
                    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                }
                status = false;
                resultGrid = rGrid;
            }
            //free memory
            lockPrj = false;
            FreeProjPointers();

            try
            {
                DataManagement.CopyGridLegend(inputGrid.Filename, resultGrid.Filename);
            }
            catch
            { }

            return status;
        }
        #endregion

        /// <summary>
        /// Transforms a grid from one coordinate system to another.
        /// 
        /// </summary>
        /// <param name="sourceProj">Proj.4 initialization string for input coordinate system.</param>
        /// <param name="destProj">Proj.4 initialization string for output coordinate system.</param>
        /// <param name="inputImage">The full path to the input image file.</param>
        /// <param name="resultImage">The full path to where the result image file will be saved.</param>
        /// <param name="ICallBack">An ICallback interface for progress and error messages</param>
        /// <remarks>throws exceptions in the case of errors</remarks>
        public static void ProjectImage(string sourceProj, string destProj, string inputImage, string resultImage, MapWinGIS.ICallback ICallBack)
        {

            bool res;
            MapWinGIS.Point inLL, inLR, inUL, inUR;  // Lower Left, Lower Right, Upper Left, Upper Right
            MapWinGIS.Point outLL, outLR, outUL, outUR; // Lower Left, Lower Right, Upper Left, Upper Right

            var Height = 0;
            var Width = 0;
            // Check to make sure that the strings being passed are valid strings

            // -------------------- PARAMETER TESTING ----------------------------------------
            if (sourceProj == "") throw new ArgumentException("Source projection was missing.");
            if (destProj == "") throw new ArgumentException("Destination projection was missing.");
            if (System.IO.File.Exists(inputImage) == false) throw new ArgumentException("Input Image file was not found.");
            if (resultImage == "") throw new ArgumentException("Result Image file was not specified.");
            var SourceImage = new MapWinGIS.Image();
            res = SourceImage.Open(inputImage, MapWinGIS.ImageType.USE_FILE_EXTENSION, false, null);
            if (res == false) throw new ArgumentException(SourceImage.get_ErrorMsg(SourceImage.LastErrorCode));

            // SourceImage.get_OriginalHeight(ref Height);  OriginalHeight does not seem to be valid - 0, negative, or very large
            if (Height == 0) Height = SourceImage.OriginalWidth;
            
            // SourceImage.get_OriginalWidth(ref Width);    OriginalWidth does not seem to be valid - 0, negative, or very large
            if (Width == 0) Width = SourceImage.OriginalHeight;

            if (SourceImage.SourceType == MapWinGIS.tkImageSourceType.istGDALBased)
            {
                if (Width * Height > 10000 * 10000)   // there is little chance to reproject an image of the larger size
                {
                    SourceImage.Close();
                    throw new ArgumentException("The input image is too large.");
                }
                else
                {
                    SourceImage.LoadBuffer(1000);    // size MB
                }
            }

            // -------------------- Projection Handling through proj4 ------------------------
            if (SourceImage.GetProjection() == "")
            {
                res = SourceImage.SetProjection(sourceProj);
                if (res == false) throw new ArgumentException(SourceImage.get_ErrorMsg(SourceImage.LastErrorCode));
            }
            MapWinGeoProc.Error.ClearErrorLog();
            if (InitializeGlobalVariables(ref sourceProj, ref destProj) == false)
            {
                FreeProjPointers();
                throw new ArgumentException(MapWinGeoProc.Error.GetLastErrorMsg());
            }
            // Define all four corners for our projective transform
            //      UL--------UR
            //      |          |
            //      LL--------LR
            //
            inLL = new MapWinGIS.Point();
            inLL.x = SourceImage.XllCenter;
            inLL.y = SourceImage.YllCenter;
            inLR = new MapWinGIS.Point();
            inLR.y = SourceImage.YllCenter;
            inLR.x = SourceImage.XllCenter + SourceImage.dX * SourceImage.OriginalHeight;
            inUL = new MapWinGIS.Point();
            inUL.x = SourceImage.XllCenter;
            inUL.y = SourceImage.YllCenter + SourceImage.dY * SourceImage.OriginalWidth;
            inUR = new MapWinGIS.Point();
            inUR.x = SourceImage.XllCenter + SourceImage.dX * SourceImage.OriginalWidth;
            inUR.y = SourceImage.YllCenter + SourceImage.dY * SourceImage.OriginalHeight;

            // These use Angela's code with global variables defined in InitializeGlobalVariables
            outLL = ConvertAndProjectPoint(inLL);
            outLR = ConvertAndProjectPoint(inLR);
            outUL = ConvertAndProjectPoint(inUL);
            outUR = ConvertAndProjectPoint(inUR);

            // ---------------------- WORLD FILE ONLY --------------------------------------------
            // First of all, we may not have to modify the image.  If this is true, we can re-project quite quickly.
            double dx, dy;
            double cx, cy;
            double test;
            dx = (outUR.x - outLL.x);
            dy = (outUR.y - outLL.y);
            cx = dx / Width; // estimation of cellwidth, assuming no skew or rotation
            cy = dx / Height;
            test = outLL.x + dx;
            // in the interest in keeping speed, if the difference is less than two pixels, just change the worldfile
            // dpa 10/2/2007 - remove this section.  Good idea, but caused problems not projecting several different images.
            /*
            if (test - outLR.x < 2 * cx)
            {
                test = outLL.y + dy;
                if (test - outUL.x < 2 * cy)
                {
                    // No image modification is necessary, so just rewrite the file
                    SourceImage.dX = cx;
                    SourceImage.dY = cy;
                    SourceImage.XllCenter = outLL.x;
                    SourceImage.YllCenter = outLL.y;

                   

                    // Copy the initial file
                   
                    // Opening as JPG in MapWinGIS causes write protection errors when saving to other formats

                    string tempname = null;
                    if (System.IO.Path.GetExtension(resultImage) == ".jpg")
                    {
                        SourceImage.Close();
                        tempname = System.IO.Path.ChangeExtension(resultImage, ".bmp");
                        if (System.IO.File.Exists(tempname)) System.IO.File.Delete(tempname);
                        System.Drawing.Image bob = new System.Drawing.Bitmap(inputImage);
                        bob.Save(tempname, System.Drawing.Imaging.ImageFormat.Bmp);
                        bob.Dispose();
                        bob = null;
                        res = SourceImage.Open(tempname, MapWinGIS.ImageType.BITMAP_FILE, true, null);
                        if (res == false) throw new ArgumentException(SourceImage.get_ErrorMsg(SourceImage.LastErrorCode));
                        
                    }
                    if (System.IO.File.Exists(resultImage)) System.IO.File.Delete(resultImage);
                    res = SourceImage.Save(resultImage, true, MapWinGIS.ImageType.USE_FILE_EXTENSION, null);                    
                    if (res == false) throw new ApplicationException(SourceImage.get_ErrorMsg(SourceImage.LastErrorCode));
                    
                    //res = DestImage.Save(resultImage, true, MapWinGIS.ImageType.USE_FILE_EXTENSION, null);
                    //if (res == false) throw new ApplicationException(DestImage.get_ErrorMsg(DestImage.LastErrorCode));
                    if (ICallBack != null) ICallBack.Progress("Status", 0, "Done.");
                    SourceImage.Close();
                    SourceImage = null;
                    GC.Collect();
                    return;
                }
            }
            */

            // ------------------------ Affine -------------------------------------------------

            var ext = System.IO.Path.GetExtension(resultImage);
            // This speed-up is only applicable to file types supported by 
            // System.Drawing.Image 
            if (ext == ".gif" || ext == ".jgp" || ext == ".bmp" || ext == ".png" || ext == ".tif")
            {

                var Aff = new MapWinGeoProc.Transforms.Affine();
                Aff.Derive_Coefficients(Width, Height, outUL, outUR, outLL, outLR);
                if (Aff.Error() < 2)
                {
                    // We couldn't just change the world file, but a linear transform will work
                    SourceImage.Close();
                    Aff.TransformImage(inputImage, resultImage, true);
                    string tempname = null;
                    if (System.IO.Path.GetExtension(resultImage) == ".jpg")
                    {
                        SourceImage.Close();
                        tempname = System.IO.Path.ChangeExtension(resultImage, ".bmp");
                        if (System.IO.File.Exists(tempname)) System.IO.File.Delete(tempname);
                        System.Drawing.Image bob = new System.Drawing.Bitmap(inputImage);
                        bob.Save(tempname, System.Drawing.Imaging.ImageFormat.Bmp);
                        bob.Dispose();
                        bob = null;
                        res = SourceImage.Open(tempname, MapWinGIS.ImageType.BITMAP_FILE, true, null);
                        if (res == false) throw new ArgumentException(SourceImage.get_ErrorMsg(SourceImage.LastErrorCode));

                    }
                    else
                    {
                        SourceImage.Open(resultImage, MapWinGIS.ImageType.USE_FILE_EXTENSION, true, ICallBack);
                    }
                    //SourceImage.SetProjection(destProj);
                    res = SourceImage.Save(resultImage, true, MapWinGIS.ImageType.USE_FILE_EXTENSION, null);
                    if (res == false) throw new ArgumentException(SourceImage.get_ErrorMsg(SourceImage.LastErrorCode));
                    SourceImage.Close();
                    if (ICallBack != null) ICallBack.Progress("Status", 0, "Done.");
                    return;
                }
            }

            // --------------------- Projective --------------------------------------------

            var Proj = new Transforms.Projective();

            // solves the coefficients of the projective tansform and saves them in the class
            Proj.Derive_Coefficients(Width, Height, outLL, outUL, outUR, outLR);

            // Actually applies a projective transform to an image
            Proj.ProjectImage(SourceImage, resultImage, ICallBack);

            // repopen the image to save projection (filename is used by image.SetProjection method)
            // TODO: optimize
            MapWinGIS.Image destImage = new MapWinGIS.Image();
            if (destImage.Open(resultImage, MapWinGIS.ImageType.USE_FILE_EXTENSION, false, ICallBack))
            {
                destImage.SetProjection(destProj);
                destImage.Close();
            }
        }

        /// <summary>
        /// Arcview will create worldfiles with rotational/skew terms that are not explicitly stored
        /// in the image, but rather are stored as coefficients in the world file.  This function
        /// creates an output image that is rectfied so that it has 0 for the rotational terms, 
        /// but should have the same positioning as the original image.
        /// </summary>
        /// <param name="inputImage">String sourcefile to be rectified.</param>
        /// <param name="resultImage">(Optional) ouput filename.  Default is inputImage + "_Rect"</param>
        /// <param name="ICallBack">A MapWinGIS.ICallback interface object for progress messages.</param>
        public static void RectifyToWorldFile(string inputImage, string resultImage, MapWinGIS.ICallback ICallBack)
        {
            bool res;
            string worldfile;
            if (inputImage == null) throw new ArgumentException("inputImage can't be null.");
            if (resultImage == null) resultImage = System.IO.Path.ChangeExtension(inputImage, "_Rect" + System.IO.Path.GetExtension(inputImage));
            var SourceImage = new MapWinGIS.Image();
            res = SourceImage.Open(inputImage, MapWinGIS.ImageType.USE_FILE_EXTENSION, true, ICallBack);
            if (res == false) throw new ApplicationException(SourceImage.get_ErrorMsg(SourceImage.LastErrorCode));
            worldfile = System.IO.Path.ChangeExtension(inputImage, ".wld");
            if (File.Exists(worldfile) == false)
            {
                var ext = System.IO.Path.GetExtension(inputImage);
                worldfile = inputImage + "w";
                if (File.Exists(worldfile) == false)
                {
                    worldfile = System.IO.Path.ChangeExtension(inputImage, "." + ext.Substring(1, 1) + ext.Substring(3, 1) + "w");
                    if (File.Exists(worldfile) == false)
                    {
                        throw new ArgumentException("Cannot find a worldfile for inputImage.");
                    }
                }
            }
            var sr = new System.IO.StreamReader(worldfile);
            double a, b, c, d, e, f;
            a = double.Parse(sr.ReadLine());
            d = double.Parse(sr.ReadLine());
            b = double.Parse(sr.ReadLine());
            e = double.Parse(sr.ReadLine());
            c = double.Parse(sr.ReadLine());
            f = double.Parse(sr.ReadLine());
            sr.Close();

            var outLL = new MapWinGIS.Point();
            var outUL = new MapWinGIS.Point();
            var outUR = new MapWinGIS.Point();
            var outLR = new MapWinGIS.Point();
            outUL.x = c;
            outUL.y = f;
            outLL.x = b * SourceImage.Height + c;
            outLL.y = e * SourceImage.Height + f;
            outUR.x = a * SourceImage.Width + c;
            outUR.y = d * SourceImage.Width + f;
            outLR.x = a * SourceImage.Width + b * SourceImage.Height + c;
            outLR.y = d * SourceImage.Width + e * SourceImage.Height + f;
            var proj = new MapWinGeoProc.Transforms.Projective();
            proj.Derive_Coefficients(SourceImage.Width, SourceImage.Height, outLL, outUL, outUR, outLR);
            proj.ProjectImage(SourceImage, resultImage, ICallBack);
        }

        #region GetUTMZone
        // 3/14/2006 Chris Michaelis
        /// <summary>
        /// Returns an appropriate UTM zone given a longitude in degrees.
        /// </summary>
        /// <param name="Degrees_Longitude">Degrees of longitude that you need a UTM zone for.</param>
        /// <returns>Returns an integer for the appropriate UTM zone; e.g. "12" for UTM Zone 12</returns>
        public static int GetUTMZone(double Degrees_Longitude)
        {
            // The original from Dan: ((longitude + 180) \ 6)+1
            // C# doesn't have a \ as an "integer divide", so just
            // cast degrees longitude to an integer first to force integer divison.
            return ((int)(Degrees_Longitude + 180) / 6) + 1;
        }
        #endregion

        #region ProjectShapefile(): save to file version with callback
        /// <summary>
        /// Transforms a shapefile from one coordinate system to another. 
        /// This version saves the new result shapefile to disk.
        /// </summary>
        /// <param name="sourceProj">Proj.4 initialization string for input coordinate system.</param>
        /// <param name="destProj">Proj.4 initialization string for output coordinate system.</param>
        /// <param name="inputSF">The full path to the input shapefile that will be projected.</param>
        /// <param name="resultSF">The full path to where the projected result shapefile will be saved.</param>
        /// <param name="ICallBack">callback object</param>
        /// <returns>True if projection was successful, false if an error occurs.</returns>
        public static bool ProjectShapefile(ref string sourceProj, ref string destProj, ref string inputSF, ref string resultSF, MapWinGIS.ICallback ICallBack)
        {
            MapWinGeoProc.Error.ClearErrorLog();

            // 3.Mar.2008 Jiri Kadlec - convert shapefile names to lowercase 
            // to make the function work with *.SHP filenames
            inputSF = inputSF.ToLower();
            resultSF = resultSF.ToLower();

            if (sourceProj.Equals(""))
            {
                gErrorMsg = "The source projection is missing.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                return false;
            }
            if (destProj.Equals(""))
            {
                gErrorMsg = "The destination projection is missing.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                return false;
            }
            if (File.Exists(inputSF) == false)
            {
                gErrorMsg = "The path to the input shapefile is not valid.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                return false;
            }
            if (resultSF.Equals(""))
            {
                gErrorMsg = "The path for the result shapefile is missing.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                return false;
            }

            // 3/28/2008 Jiri Kadlec - simplified the code
            // get result and input file name without the '.shp' extension
            var inputFile = MapWinUtility.modFile.FilenameNoExt(inputSF);
            var resultFile = MapWinUtility.modFile.FilenameNoExt(resultSF);

            var status = true;
            MapWinGIS.Point resultPoint = new MapWinGIS.PointClass();
            MapWinGIS.Shapefile sf = new MapWinGIS.ShapefileClass();
            sf.Open(inputSF, null);
            var numShapes = sf.NumShapes;
            var sfType = sf.ShapefileType;
            MapWinGIS.Shapefile result = new MapWinGIS.ShapefileClass();

            if (ICallBack != null) ICallBack.Progress("Status", 0, "");

            //Delete the result shapefile if it already exists
            if (Globals.PrepareResultSF(ref resultSF, ref result, sfType) == false)
            {
                if (sf != null)
                    sf.Close();
                return false;
            }

            try
            {

                if (InitializeGlobalVariables(ref sourceProj, ref destProj) == false)
                {
                    FreeProjPointers();
                    if (sf != null)
                        sf.Close();
                    if (result != null)
                        result.Close();
                    return false;
                }
                else
                {
                    int numPoints, shpIndex, ptIndex;
                    MapWinGIS.Shape shp, resultShp;
                    MapWinGIS.Point pt;

                    for (var i = 0; i <= numShapes - 1; i++)
                    {
                        shpIndex = result.NumShapes;
                        shp = new MapWinGIS.ShapeClass();
                        resultShp = new MapWinGIS.ShapeClass();
                        resultShp.ShapeType = sfType;

                        shp = sf.get_Shape(i);
                        var numParts = shp.NumParts;
                        numPoints = shp.numPoints;
                        double x, y;

                        //Deal with multi-part shapes
                        if (numParts > 1)
                        {
                            int begPart;
                            int endPart;
                            int partIndex;

                            for (var j = 0; j <= numParts - 1; j++)
                            {
                                begPart = shp.get_Part(j);
                                if (j < numParts - 1)
                                {
                                    endPart = shp.get_Part(j + 1);
                                }
                                else
                                {
                                    endPart = shp.numPoints;
                                }

                                partIndex = j;
                                resultShp.InsertPart(begPart, ref partIndex);

                                //project each point of the current part
                                for (var k = begPart; k <= endPart - 1; k++)
                                {
                                    ptIndex = resultShp.numPoints;
                                    pt = new MapWinGIS.PointClass();
                                    x = shp.get_Point(k).x;
                                    y = shp.get_Point(k).y;
                                    if (ConvertAndProjectPoint(ref x, ref y) == true)
                                    {
                                        pt.x = x;
                                        pt.y = y;
                                        resultShp.InsertPoint(pt, ref ptIndex);
                                        status = true;
                                    }
                                    else
                                    {
                                        gErrorMsg = "Error occured while projecting point " + k + " of part " + j + " of shape " + i;
                                        Debug.WriteLine(gErrorMsg);
                                        MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                                        status = false;
                                    }
                                }//end of looping through points in part
                            }//end of looping through parts of a multi-part shape
                        }//end of dealing with multi-part shapes

                        else //not a multi-part shape
                        {

                            for (var j = 0; j <= numPoints - 1; j++)
                            {
                                ptIndex = resultShp.numPoints;
                                pt = new MapWinGIS.PointClass();
                                x = shp.get_Point(j).x;
                                y = shp.get_Point(j).y;
                                if (ConvertAndProjectPoint(ref x, ref y) == true)
                                {
                                    pt.x = x;
                                    pt.y = y;
                                    resultShp.InsertPoint(pt, ref ptIndex);
                                    status = true;
                                }
                                else
                                {
                                    gErrorMsg = "Error occured while projecting point " + j + " of shape " + i;
                                    Debug.WriteLine(gErrorMsg);
                                    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                                    status = false;
                                }
                            }//end of looping through points
                        }//end of dealing with non-multi-part shapes

                        if (result.EditInsertShape(resultShp, ref shpIndex) == false)
                        {
                            gErrorMsg = "Problem editing result shapes: " + result.get_ErrorMsg(result.LastErrorCode);
                            Debug.WriteLine(gErrorMsg);
                            MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                            FreeProjPointers();
                            if (sf != null)
                                sf.Close();
                            if (result != null)
                                result.Close();
                            return false;
                        }
                        if (ICallBack != null) ICallBack.Progress("Status", 100 * i / numShapes, "");
                    }//end of looping through shapes

                    if (result.StopEditingShapes(true, true, null) == false)
                    {
                        gErrorMsg = "Problem with result.StopEditingShapes: " + result.get_ErrorMsg(result.LastErrorCode);
                        Debug.WriteLine(gErrorMsg);
                        MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                        FreeProjPointers();
                        if (sf != null)
                            sf.Close();
                        if (result != null)
                            result.Close();

                        return false;
                    }
                    //copy the .dbf table
                    var inputDBF = inputFile + ".dbf";
                    var resultDBF = resultFile + ".dbf";
                    CopyDBF(ref inputDBF, ref resultDBF);
                    if (ICallBack != null) ICallBack.Progress("Status", 100, "");
                }
            }
            catch (Exception e)
            {
                if (e.Message.Equals("latitude or longitude exceeded limits"))
                {
                    gErrorMsg = "Could not project '" + sf.Filename + "'. Perhaps it was already projected?";
                    Debug.WriteLine(gErrorMsg);
                    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                }
                else
                {
                    gErrorMsg = e.Message + e.ToString();
                    Debug.WriteLine(gErrorMsg);
                    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                }
                status = false;
            }

            result.Projection = destProj;

            //free memory
            FreeProjPointers();

            if (sf != null)
                sf.Close();

            if (result != null)
                result.Close();

            return status;

        }
        #endregion

        #region ProjectShapefile(): save to file version
        /// <summary>
        /// Transforms a shapefile from one coordinate system to another. 
        /// This version saves the new result shapefile to disk.
        /// </summary>
        /// <param name="sourceProj">Proj.4 initialization string for input coordinate system.</param>
        /// <param name="destProj">Proj.4 initialization string for output coordinate system.</param>
        /// <param name="inputSF">The full path to the input shapefile that will be projected.</param>
        /// <param name="resultSF">The full path to where the projected result shapefile will be saved.</param>
        /// <returns>True if projection was successful, false if an error occurs.</returns>
        public static bool ProjectShapefile(ref string sourceProj, ref string destProj, ref string inputSF, ref string resultSF)
        {
            MapWinGeoProc.Error.ClearErrorLog();

            // 3.Mar.2008 Jiri Kadlec - convert shapefile names to lowercase 
            // to make the function work with *.SHP filenames
            inputSF = inputSF.ToLower();
            resultSF = resultSF.ToLower();

            if (sourceProj.Equals(string.Empty))
            {
                gErrorMsg = "The source projection is missing.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                return false;
            }

            if (destProj.Equals(string.Empty))
            {
                gErrorMsg = "The destination projection is missing.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                return false;
            }

            // Paul Meems 
            // 17 August 2009 Added:
            if (destProj.Equals(sourceProj))
            {
                gErrorMsg = "The destination projection is identical to the source projection. No reprojection has been done.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                return false;
            }

            if (File.Exists(inputSF) == false)
            {
                gErrorMsg = "The path to the input shapefile is not valid.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                return false;
            }

            if (resultSF.Equals(string.Empty))
            {
                gErrorMsg = "The path for the result shapefile is missing.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                return false;
            }

            // 3/28/2008 Jiri Kadlec - simplified the code
            // get result and input file name without the '.shp' extension
            var inputFile = MapWinUtility.modFile.FilenameNoExt(inputSF);
            var resultFile = MapWinUtility.modFile.FilenameNoExt(resultSF);

            var status = true;
            MapWinGIS.Point resultPoint = new MapWinGIS.PointClass();
            MapWinGIS.Shapefile sf = new MapWinGIS.ShapefileClass();
            sf.Open(inputSF, null);
            var numShapes = sf.NumShapes;
            var sfType = sf.ShapefileType;
            MapWinGIS.Shapefile result = new MapWinGIS.ShapefileClass();

            //Delete the result shapefile if it already exists
            if (Globals.PrepareResultSF(ref resultSF, ref result, sfType) == false)
            {
                if (sf != null)
                    sf.Close();
                return false;
            }

            try
            {
                if (InitializeGlobalVariables(ref sourceProj, ref destProj) == false)
                {
                    FreeProjPointers();
                    if (sf != null)
                        sf.Close();
                    if (result != null)
                        result.Close();
                    return false;
                }

                int numPoints, shpIndex, ptIndex;
                MapWinGIS.Shape shp, resultShp;
                MapWinGIS.Point pt;

                for (var i = 0; i <= numShapes - 1; i++)
                {
                    shpIndex = result.NumShapes;
                    shp = new MapWinGIS.ShapeClass();
                    resultShp = new MapWinGIS.ShapeClass();
                    resultShp.ShapeType = sfType;

                    shp = sf.get_Shape(i);
                    var numParts = shp.NumParts;
                    numPoints = shp.numPoints;
                    double x, y;

                    // Deal with multi-part shapes
                    if (numParts > 1)
                    {
                        int begPart;
                        int endPart;
                        int partIndex;

                        for (var j = 0; j <= numParts - 1; j++)
                        {
                            begPart = shp.get_Part(j);
                            if (j < numParts - 1)
                            {
                                endPart = shp.get_Part(j + 1);
                            }
                            else
                            {
                                endPart = shp.numPoints;
                            }

                            partIndex = j;
                            resultShp.InsertPart(begPart, ref partIndex);

                            // project each point of the current part
                            for (var k = begPart; k <= endPart - 1; k++)
                            {
                                ptIndex = resultShp.numPoints;
                                pt = new MapWinGIS.PointClass();
                                x = shp.get_Point(k).x;
                                y = shp.get_Point(k).y;
                                if (ConvertAndProjectPoint(ref x, ref y) == true)
                                {
                                    pt.x = x;
                                    pt.y = y;
                                    resultShp.InsertPoint(pt, ref ptIndex);
                                    status = true;
                                }
                                else
                                {
                                    gErrorMsg = "Error occured while projecting point " + k + " of part " + j + " of shape " + i;
                                    Debug.WriteLine(gErrorMsg);
                                    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                                    status = false;
                                }
                            }//end of looping through points in part
                        }//end of looping through parts of a multi-part shape
                    }//end of dealing with multi-part shapes

                    else //not a multi-part shape
                    {

                        for (var j = 0; j <= numPoints - 1; j++)
                        {
                            ptIndex = resultShp.numPoints;
                            pt = new MapWinGIS.PointClass();
                            x = shp.get_Point(j).x;
                            y = shp.get_Point(j).y;
                            if (ConvertAndProjectPoint(ref x, ref y) == true)
                            {
                                pt.x = x;
                                pt.y = y;
                                resultShp.InsertPoint(pt, ref ptIndex);
                                status = true;
                            }
                            else
                            {
                                gErrorMsg = "Error occured while projecting point " + j + " of shape " + i;
                                Debug.WriteLine(gErrorMsg);
                                MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                                status = false;
                            }
                        }//end of looping through points
                    }//end of dealing with non-multi-part shapes

                    if (result.EditInsertShape(resultShp, ref shpIndex) == false)
                    {
                        gErrorMsg = "Problem editing result shapes: " + result.get_ErrorMsg(result.LastErrorCode);
                        Debug.WriteLine(gErrorMsg);
                        MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                        FreeProjPointers();
                        if (sf != null)
                            sf.Close();
                        if (result != null)
                            result.Close();
                        return false;
                    }
                }//end of looping through shapes

                if (result.StopEditingShapes(true, true, null) == false)
                {
                    gErrorMsg = "Problem with result.StopEditingShapes: " + result.get_ErrorMsg(result.LastErrorCode);
                    Debug.WriteLine(gErrorMsg);
                    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                    FreeProjPointers();
                    if (sf != null)
                        sf.Close();
                    if (result != null)
                        result.Close();

                    return false;
                }
                //copy the .dbf table
                var inputDBF = inputFile + ".dbf";
                var resultDBF = resultFile + ".dbf";
                CopyDBF(ref inputDBF, ref resultDBF);
            }
            catch (Exception e)
            {
                if (e.Message.Equals("latitude or longitude exceeded limits"))
                {
                    gErrorMsg = "Could not project '" + sf.Filename + "'. Perhaps it was already projected?";
                    Debug.WriteLine(gErrorMsg);
                    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                }
                else
                {
                    gErrorMsg = e.Message + e.ToString();
                    Debug.WriteLine(gErrorMsg);
                    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                }
                status = false;
            }

            result.Projection = destProj;

            //free memory
            FreeProjPointers();

            if (sf != null)
                sf.Close();

            if (result != null)
                result.Close();

            return status;

        }
        #endregion

        #region ProjectShapefile -- Additional Overloads without input projections
        /// <summary>
        /// Transforms a shapefile from one coordinate system to another. 
        /// This version saves the new result shapefile to disk.
        /// The source projection is inferred from the input shapefile. If the
        /// .prj file is missing, the function will return false.
        /// </summary>
        /// <param name="destProj">Proj.4 initialization string for output coordinate system.</param>
        /// <param name="inputSF">The full path to the input shapefile that will be projected.</param>
        /// <param name="resultSF">The full path to where the projected result shapefile will be saved.</param>
        /// <returns>True if projection was successful, false if an error occurs.</returns>
        public static bool ProjectShapefile(ref string destProj, ref string inputSF, ref string resultSF)
        {
            if (!System.IO.File.Exists(inputSF)) return false;
            var inputf = new MapWinGIS.Shapefile();
            inputf.Open(inputSF, null);
            if (inputSF == null) return false;
            var prj = inputf.Projection;
            inputf.Close();
            if (prj.Trim() == "") return false;

            return ProjectShapefile(ref prj, ref destProj, ref inputSF, ref resultSF);
        }

        /// <summary>
        /// Transforms a shapefile from one coordinate system to another. 
        /// This version saves the new result shapefile to disk.
        /// The source projection is inferred from the input shapefile. If the
        /// .prj file is missing, the function will return false.
        /// </summary>
        /// <param name="destProj">Proj.4 initialization string for output coordinate system.</param>
        /// <param name="inputSF">The input shapefile that will be projected.</param>
        /// <param name="resultSF">The result shapefile.</param>
        /// <returns>True if projection was successful, false if an error occurs.</returns>
        public static bool ProjectShapefile(ref string destProj, ref MapWinGIS.Shapefile inputSF, out MapWinGIS.Shapefile resultSF)
        {
            if (inputSF == null)
            {
                resultSF = null;
                return false;
            }
            var prj = inputSF.Projection;
            if (prj.Trim() == "")
            {
                resultSF = null;
                return false;
            }

            return ProjectShapefile(ref prj, ref destProj, ref inputSF, out resultSF);
        }
        #endregion

        #region ProjectShapefile(): over-write original file version
        /// <summary>
        /// Transforms a shapefile from one coordinate system to another.
        /// This version over-writes the original shapefile.
        /// </summary>
        /// <param name="sourceProj">Proj.4 initialization string for input coordinate system.</param>
        /// <param name="destProj">Proj.4 initialization string for output coordinate system.</param>
        /// <param name="shpFile">The full path to the shapefile that will be projected and over-written.</param>
        /// <returns>True if projection was successful, false if an error occurs.</returns>
        public static bool ProjectShapefile(ref string sourceProj, ref string destProj, ref MapWinGIS.Shapefile shpFile)
        {
            MapWinGeoProc.Error.ClearErrorLog();
            if (sourceProj.Equals(""))
            {
                gErrorMsg = "The source projection is missing.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                return false;
            }
            if (destProj.Equals(""))
            {
                gErrorMsg = "The destination projection is missing.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                return false;
            }
            if (shpFile.NumShapes == 0 || shpFile.ShapefileType == MapWinGIS.ShpfileType.SHP_NULLSHAPE)
            {
                gErrorMsg = "The shapefile is not valid, either it contains 0 shapes or is of type NULLSHAPE.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                return false;
            }

            var status = true;
            var numShapes = shpFile.NumShapes;

            try
            {
                if (InitializeGlobalVariables(ref sourceProj, ref destProj) == false)
                {
                    FreeProjPointers();
                    return false;
                }
                else
                {
                    int numPoints;
                    MapWinGIS.Shape shp;

                    if (shpFile.StartEditingShapes(true, null) == false)
                    {
                        gErrorMsg = "Problem with StartEditingShapes: " + shpFile.get_ErrorMsg(shpFile.LastErrorCode);
                        Debug.WriteLine(gErrorMsg);
                        MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                        FreeProjPointers();
                        return false;
                    }

                    for (var i = 0; i <= numShapes - 1; i++)
                    {
                        shp = new MapWinGIS.ShapeClass();
                        shp = shpFile.get_Shape(i);
                        var numParts = shp.NumParts;
                        numPoints = shp.numPoints;
                        double x, y;

                        //no need to check for multi-part polygons in this version
                        //While all points are over-written, the parts will 
                        //remain in tact.
                        for (var j = 0; j <= numPoints - 1; j++)
                        {
                            x = shp.get_Point(j).x;
                            y = shp.get_Point(j).y;
                            if (ConvertAndProjectPoint(ref x, ref y) == true)
                            {
                                //replace old point with projected point in shapefile
                                shp.get_Point(j).x = x;
                                shp.get_Point(j).y = y;
                            }
                            else
                            {
                                gErrorMsg = "Error occured while projecting point " + j + " of shape " + i;
                                Debug.WriteLine(gErrorMsg);
                                MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                                status = false;
                            }
                        }//end of looping through points
                    }//end of looping through shapes
                    if (shpFile.StopEditingShapes(true, true, null) == false)
                    {
                        gErrorMsg = "Problem with StopEditingShapes: " + shpFile.get_ErrorMsg(shpFile.LastErrorCode);
                        Debug.WriteLine(gErrorMsg);
                        MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                        FreeProjPointers();
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                if (e.Message.Equals("latitude or longitude exceeded limits"))
                {
                    gErrorMsg = "Could not project '" + shpFile.Filename + "'. Perhaps it was already projected?";
                    Debug.WriteLine(gErrorMsg);
                    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                }
                else
                {
                    gErrorMsg = e.Message + e.ToString();
                    Debug.WriteLine(gErrorMsg);
                    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                }
                status = false;
            }

            shpFile.Projection = destProj;

            //free memory
            FreeProjPointers();

            return status;
        }
        #endregion

        #region ProjectShapefile(): in-memory version
        /// <summary>
        /// Transforms a shapefile from one coordinate system to another. 
        /// This version saves the result shapefile in memory.
        /// </summary>
        /// <param name="sourceProj">Proj.4 initialization string for input coordinate system.</param>
        /// <param name="destProj">Proj.4 initialization string for output coordinate system.</param>
        /// <param name="inputSF">Reference to input shapefile loaded in memory.</param>
        /// <param name="resultSF">Out parameter for projected shapefile results residing in memory.</param>
        /// <returns>True if projection was successful, false if an error occurs.</returns>
        public static bool ProjectShapefile(ref string sourceProj, ref string destProj, ref MapWinGIS.Shapefile inputSF, out MapWinGIS.Shapefile resultSF)
        {
            MapWinGeoProc.Error.ClearErrorLog();
            MapWinGIS.Shapefile result = new MapWinGIS.ShapefileClass();

            if (sourceProj.Equals(""))
            {
                gErrorMsg = "The source projection is missing.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                resultSF = result;
                return false;
            }
            if (destProj.Equals(""))
            {
                gErrorMsg = "The destination projection is missing.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                resultSF = result;
                return false;
            }
            if (inputSF.NumShapes == 0 || inputSF.ShapefileType == MapWinGIS.ShpfileType.SHP_NULLSHAPE)
            {
                gErrorMsg = "The input shapefile is invalid, either it contains 0 shapes or is of type NULLSHAPE.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                resultSF = result;
                return false;
            }

            var status = true;
            MapWinGIS.Point resultPoint = new MapWinGIS.PointClass();
            var numShapes = inputSF.NumShapes;
            var sfType = inputSF.ShapefileType;

            //create the result shapeFile
            // Paul Meems 17 August 2009
            // Fix for bug #1010
            //string tempPath = System.IO.Path.GetTempPath();
            //Debug.WriteLine("tempPath = " + tempPath);            
            //string resultFile = tempPath + "tempSF.shp";
            var resultFile = System.IO.Path.ChangeExtension(System.IO.Path.GetTempFileName(), ".shp");
            Debug.WriteLine("resultFile = " + resultFile);

            // Do not create shape id here (last true arg) -- it will be copied from existing table if it exists.
            // (Don't go changing tables on simple reproject operations if possible)
            if (Globals.PrepareResultSF(ref resultFile, ref result, sfType, true) == false)
            {
                resultSF = result;
                return false;
            }

            //we need to copy the DBF table into memory & associate it with the result shapefile
            var numFields = inputSF.NumFields;
            var fieldIndex = 0;
            for (var i = 0; i <= numFields - 1; i++)
            {
                fieldIndex = result.NumFields;
                if (result.EditInsertField(inputSF.get_Field(i), ref fieldIndex, null) == false)
                {
                    gErrorMsg = "Problem inserting field into dbf table: " + result.get_ErrorMsg(result.LastErrorCode);
                    Debug.WriteLine(gErrorMsg);
                    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                    resultSF = result;
                    return false;
                }
            }

            try
            {
                if (InitializeGlobalVariables(ref sourceProj, ref destProj) == false)
                {
                    resultSF = result;
                    status = false;
                }
                else
                {
                    int numPoints, shpIndex, ptIndex;
                    MapWinGIS.Shape shp, resultShp;
                    MapWinGIS.Point pt;

                    for (var i = 0; i <= numShapes - 1; i++)
                    {
                        shpIndex = result.NumShapes;
                        shp = new MapWinGIS.ShapeClass();
                        resultShp = new MapWinGIS.ShapeClass();
                        resultShp.ShapeType = sfType;
                        shp = inputSF.get_Shape(i);
                        numPoints = shp.numPoints;
                        var numParts = shp.NumParts;
                        double x, y;

                        //Deal with multi-part shapes
                        if (numParts > 1)
                        {
                            int begPart;
                            int endPart;
                            int partIndex;

                            for (var j = 0; j <= numParts - 1; j++)
                            {
                                begPart = shp.get_Part(j);
                                if (j < numParts - 1)
                                {
                                    endPart = shp.get_Part(j + 1);
                                }
                                else
                                {
                                    endPart = shp.numPoints;
                                }

                                partIndex = j;
                                resultShp.InsertPart(begPart, ref partIndex);

                                //project each point of the current part
                                for (var k = begPart; k <= endPart - 1; k++)
                                {
                                    ptIndex = resultShp.numPoints;
                                    pt = new MapWinGIS.PointClass();
                                    x = shp.get_Point(k).x;
                                    y = shp.get_Point(k).y;
                                    if (ConvertAndProjectPoint(ref x, ref y) == true)
                                    {
                                        pt.x = x;
                                        pt.y = y;
                                        resultShp.InsertPoint(pt, ref ptIndex);
                                        status = true;
                                    }
                                    else
                                    {
                                        gErrorMsg = "Error occured while projecting point " + k + " of part " + j + " of shape " + i;
                                        Debug.WriteLine(gErrorMsg);
                                        MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                                        status = false;
                                    }
                                }//end of looping through points in part
                            }//end of looping through parts of a multi-part shape
                        }//end of dealing with multi-part shape

                        else
                        {
                            for (var j = 0; j <= numPoints - 1; j++)
                            {
                                ptIndex = resultShp.numPoints;
                                pt = new MapWinGIS.PointClass();
                                x = shp.get_Point(j).x;
                                y = shp.get_Point(j).y;
                                if (ConvertAndProjectPoint(ref x, ref y) == true)
                                {
                                    pt.x = x;
                                    pt.y = y;
                                    resultShp.InsertPoint(pt, ref ptIndex);
                                    status = true;
                                }
                                else
                                {
                                    gErrorMsg = "Error occured while projecting point " + j + " of shape " + i;
                                    Debug.WriteLine(gErrorMsg);
                                    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                                    resultSF = result;
                                    status = false;
                                }
                            }//end of looping through points
                        }//end of dealing with non-multi-part shape

                        if (result.EditInsertShape(resultShp, ref shpIndex) == false)
                        {
                            gErrorMsg = "Problem with editing shapes: " + result.get_ErrorMsg(result.LastErrorCode);
                            Debug.WriteLine(gErrorMsg);
                            MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                            resultSF = result;
                            FreeProjPointers();
                            return false;
                        }
                        for (var j = 0; j <= numFields - 1; j++)
                        {
                            if (result.EditCellValue(j, shpIndex, inputSF.get_CellValue(j, shpIndex)) == false)
                            {
                                gErrorMsg = "Problem inserting the cell value into the dbf table: " + result.get_ErrorMsg(result.LastErrorCode);
                                Debug.WriteLine(gErrorMsg);
                                MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                                resultSF = result;
                                FreeProjPointers();
                                return false;
                            }
                        }
                    }//end of looping through shapes
                    if (result.StopEditingShapes(true, true, null) == false)
                    {
                        gErrorMsg = "Problem with StopEditingShapes: " + result.get_ErrorMsg(result.LastErrorCode);
                        Debug.WriteLine(gErrorMsg);
                        MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                        resultSF = result;
                        FreeProjPointers();
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                if (e.Message.Equals("latitude or longitude exceeded limits"))
                {
                    gErrorMsg = "Could not project '" + inputSF.Filename + "'. Perhaps it was already projected?";
                    Debug.WriteLine(gErrorMsg);
                    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                }
                else
                {
                    gErrorMsg = e.Message + e.ToString();
                    Debug.WriteLine(gErrorMsg);
                    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                }
                status = false;
            }
            //free memory
            FreeProjPointers();

            result.Projection = destProj;
            resultSF = result;

            return status;
        }
        #endregion

        #region private Distance()
        /// <summary>
        /// Computes the distance between two values.
        /// </summary>
        /// <param name="v1">The first value.</param>
        /// <param name="v2">The second value.</param>
        /// <returns>The distance between the first and second value.</returns>	
        private static double Distance(double v1, double v2)
        {
            //Rob Cairns 9 November 2009. Using Chris George's suggestion for Bug 1477
            return Math.Abs(v2 - v1);

            //if(v1 < 0)//v1 is negative
            //{
            //    if(v2 < 0)
            //    {
            //        //both are negative
            //        if(v1 < v2)
            //        {
            //            return v2-v1;
            //        }
            //        else
            //        {
            //            return v1-v2;
            //        }
            //    }
            //    else
            //    {
            //        //v2 is positive
            //        if(v2 + v1 > 0)
            //        {
            //            return v2 + v1;
            //        }
            //        else
            //        {
            //            return -1*v1 + v2;
            //        }
            //    }
            //}
            //else
            //{
            //    //v1 is positive
            //    if(v2 < 0)
            //    {
            //        if(v1 + v2 > 0)
            //        {
            //            return v1 + v2;
            //        }
            //        else
            //        {
            //            return -1*v2 + v1;
            //        }
            //    }
            //    else
            //    {
            //        //both are positive
            //        if(v1 > v2)
            //        {
            //            return v1 - v2;
            //        }
            //        else
            //        {
            //            return v2 - v1;
            //        }
            //    }
            //}

        }
        #endregion

        #region private ConvertAndProjectPoint()
        /// <summary>
        /// Projects a 2D point from one coordinate system to another
        /// using Proj.4 function: pj_transform.
        /// This function assumes that the caller has already checked to see
        /// if conversion to radians or degrees is necessary, then performs
        /// the necessary conversion before or after projecting the point.
        /// </summary>
        /// <param name="x">X value of point to project.</param>
        /// <param name="y">Y value of point to project.</param>
        /// <returns>True if projection was successful, false if an error was encountered.</returns>
        private static bool ConvertAndProjectPoint(ref double x, ref double y)
        {
            var status = false;
            if (convertToRadians == true)
            {
                x *= DEG_TO_RAD;
                y *= DEG_TO_RAD;
            }

            double Z = 0;
            var retcode = pj_transform(srcPrj, destPrj, 1, 0, ref x, ref y, ref Z);

            if (retcode != 0)
            {
                gErrorMsg = "Error occured while converting and projecting point. " + GetErrorMsg(retcode);
                Debug.WriteLine(gErrorMsg);
                MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
            }
            else
            {
                if (convertToDegrees == true)
                {
                    x *= RAD_TO_DEG;
                    y *= RAD_TO_DEG;
                }
                status = true;
            }
            return status;
        }
        /// <summary>
        /// Projects a 2D point from one coordinate system to another
        /// using Proj.4 function: pj_transform.
        /// This function assumes that the caller has already checked to see
        /// if conversion to radians or degrees is necessary, then performs
        /// the necessary conversion before or after projecting the point.
        /// Call InitializeGlobals before this.
        /// </summary>
        /// <param name="InputPoint">A MapWinGIS.Point specifying the point to transform.</param>
        /// <returns>A MapWinGIS.Point containing the projected information</returns>
        /// <remarks>Will throw an Application exception if there is an error.</remarks>
        private static MapWinGIS.Point ConvertAndProjectPoint(MapWinGIS.Point InputPoint)
        {
            double x, y;
            double Z = 0;
            x = InputPoint.x;
            y = InputPoint.y;
            var OutputPoint = new MapWinGIS.Point();
            if (convertToRadians == true)
            {
                x *= DEG_TO_RAD;
                y *= DEG_TO_RAD;
            }


            var retcode = pj_transform(srcPrj, destPrj, 1, 0, ref x, ref y, ref Z);

            if (retcode != 0)
            {
                throw new ApplicationException("Error occured while converting and projecting point. " + GetErrorMsg(retcode));
            }
            else
            {
                if (convertToDegrees == true)
                {
                    x *= RAD_TO_DEG;
                    y *= RAD_TO_DEG;
                }
            }
            OutputPoint.x = x;
            OutputPoint.y = y;
            return OutputPoint;
        }
        #endregion

        #region ProjectPoint()
        /// <summary>
        /// Projects a 2D point from one coordinate system to another
        /// using Proj.4 function: pj_transform.
        /// </summary>
        /// <param name="x">X value of point to project.</param>
        /// <param name="y">Y value of point to project.</param>
        /// <param name="srcPrj4String">Source projection string (proj4 format)</param>
        /// <param name="destPrj4String">Destination projection string (proj4 format)</param>
        /// <returns>True if projection was successful, false if an error was encountered.</returns>
        public static bool ProjectPoint(ref double x, ref double y, string srcPrj4String, string destPrj4String)
        {
            Error.ClearErrorLog();
            if (srcPrj4String.Equals(""))
            {
                gErrorMsg = "The source projection is missing.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                return false;
            }
            if (destPrj4String.Equals(""))
            {
                gErrorMsg = "The destination projection is missing.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                return false;
            }
            if (InitializeGlobalVariables(ref srcPrj4String, ref destPrj4String) == false)
            {
                FreeProjPointers();
                return false;
            }
            if (ConvertAndProjectPoint(ref x, ref y) == false)
            {
                FreeProjPointers();
                return false;
            }

            FreeProjPointers();

            return true;
        }
        #endregion

        #region private FreeProjPointers()
        /// <summary>
        /// Releases memory used by the srcPrj and destPrj structures.
        /// </summary>
        private static void FreeProjPointers()
        {
            if (lockPrj) return;

            //free memory
            if (srcPrj != 0)
            {
                try { pj_free(srcPrj); }
                catch { }
            }
            srcPrj = 0;

            if (destPrj != 0)
            {
                try { pj_free(destPrj); }
                catch { }
            }
            destPrj = 0;
        }
        #endregion

        #region private GetErrorMsg()
        /// <summary>
        /// Returns the error message associated with prj_transform.
        /// </summary>
        /// <param name="retcode">The return value of prj_transform.</param>
        /// <returns>The message string corresponding to a known error.</returns>
        private static string GetErrorMsg(int retcode)
        {
            var msg = "prj_transform error: ";
            if (retcode != 0)
            {
                switch (retcode)
                {
                    case -1:
                        msg = "no arguments in initialization list";
                        break;
                    case -2:
                        msg = "no options found in 'init' file";
                        break;
                    case -3:
                        msg = "no colon in init= string";
                        break;
                    case -4:
                        msg = "projection not named";
                        break;
                    case -5:
                        msg = "unknown projection id";
                        break;
                    case -6:
                        msg = "effective eccentricity = 1";
                        break;
                    case -7:
                        msg = "unknown unit conversion id";
                        break;
                    case -8:
                        msg = "invalid boolean param argument";
                        break;
                    case -9:
                        msg = "unknown elliptical parameter name";
                        break;
                    case -10:
                        msg = "reciprocal flattening (1/f) = 0";
                        break;
                    case -11:
                        msg = "|radius reference latitude| > 90";
                        break;
                    case -12:
                        msg = "squared eccentricity < 0";
                        break;
                    case -13:
                        msg = "major axis or radius = 0 or not given";
                        break;
                    case -14:
                        msg = "latitude or longitude exceeded limits";
                        break;
                    case -15:
                        msg = "invalid x or y";
                        break;
                    case -16:
                        msg = "improperly formed DMS value";
                        break;
                    case -17:
                        msg = "non-convergent inverse meridinal dist";
                        break;
                    case -18:
                        msg = "non-convergent inverse phi2";
                        break;
                    case -19:
                        msg = "acos/asin: |arg| >1.+1e-14";
                        break;
                    case -20:
                        msg = "tolerance condition error";
                        break;
                    case -21:
                        msg = "conic lat_1 = -lat_2";
                        break;
                    case -22:
                        msg = "lat_1 >= 90";
                        break;
                    case -23:
                        msg = "lat_1 = 0";
                        break;
                    case -24:
                        msg = "lat_ts >= 90";
                        break;
                    case -25:
                        msg = "no distance between control points";
                        break;
                    case -26:
                        msg = "projection not selected to be rotated";
                        break;
                    case -27:
                        msg = "! <= 0 or M <= 0";
                        break;
                    case -28:
                        msg = "lsat not in 1-5 range";
                        break;
                    case -29:
                        msg = "path not in range";
                        break;
                    case -30:
                        msg = "h <= 0";
                        break;
                    case -31:
                        msg = "k <= 0";
                        break;
                    case -32:
                        msg = "lat_0 = 0 or 90 or alpha = 90";
                        break;
                    case -33:
                        msg = "lat_1=lat_2 or lat_1=0 or lat_2 = 90";
                        break;
                    case -34:
                        msg = "elliptical usage required";
                        break;
                    case -35:
                        msg = "invalid UTM zone number";
                        break;
                    case -36:
                        msg = "arg(s) our of range for Tcheby eval";
                        break;
                    case -37:
                        msg = "failed to find projection to be rotated";
                        break;
                    case -38:
                        msg = "failed to load NAD27-83 correction file";
                        break;
                    case -39:
                        msg = "both n & m must be specified and > 0";
                        break;
                    case -40:
                        msg = "n <= 0, n >1, or not specified";
                        break;
                    case -41:
                        msg = "lat_1 or lat_2 no specified";
                        break;
                    case -42:
                        msg = "|lat_1| == |lat_2|";
                        break;
                    case -43:
                        msg = "lat_0 is pi/2 from mean lat";
                        break;
                    case -44:
                        msg = "unparseable coordinate system definition";
                        break;
                    default:
                        msg = "unknown error #" + retcode;
                        break;
                }
                return msg;
            }
            else
            {
                return "no error";
            }
        }
        #endregion

        #region private CopyDBF()
        private static void CopyDBF(ref string inputDBF, ref string outputDBF)
        {
            System.IO.File.Copy(inputDBF, outputDBF, true);
        }
        #endregion

        #region private InitializeGlobalVariables()
        /// <summary>
        /// Sets the global projection variables: srcPrj, destPrj, convertToRadians, and convertToDegrees.
        /// </summary>
        /// <param name="sourceProj">The proj4 source projection.</param>
        /// <param name="destProj">The proj4 destination projection.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        private static bool InitializeGlobalVariables(ref string sourceProj, ref string destProj)
        {
            var status = true;
            if (lockPrj)
                return false;

            try
            {
                var projCmdLine = destProj;
                //Debug.WriteLine("projectionDest = " + projCmdLine);

                destPrj = pj_init_plus(projCmdLine);
                if (destPrj == 0)
                {
                    gErrorMsg = "Could not initialize proj.dll for output: At least one parameter in the destination projection is incorrect.";
                    Debug.WriteLine(gErrorMsg);
                    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                    FreeProjPointers();
                    return false;
                }

                if (pj_is_latlong(destPrj) != 0)
                {
                    //Debug.WriteLine("destination is latlong");
                    convertToDegrees = true;
                    //need to convert from radians to degrees after transforming
                }
                else
                {
                    convertToDegrees = false;
                }

                projCmdLine = sourceProj;

                //Debug.WriteLine("projectionSource = " + projCmdLine);

                srcPrj = pj_init_plus(projCmdLine);
                if (srcPrj == 0)
                {
                    gErrorMsg = "Could not initialize proj.dll for output: At least one parameter in the source projection is incorrect.";
                    Debug.WriteLine(gErrorMsg);
                    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                    FreeProjPointers();
                    return false;
                }
                //Debug.WriteLine(srcPrj);
                if (pj_is_latlong(srcPrj) != 0)
                {
                    //Debug.WriteLine("Source is latlong");
                    convertToRadians = true;
                    //Debug.WriteLine(srcPrj);
                    //need to convert degrees to radians before transorming
                }
                else
                {
                    convertToRadians = false;
                }
            }
            catch (Exception e)
            {
                gErrorMsg = e.Message + e.ToString();
                Debug.WriteLine(gErrorMsg);
                MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                status = false;
            }
            return status;
        }
        #endregion

        #region private SwitchProjections() -- used by ProjectGrid
        private static void SwitchProjections()
        {
            int tempPrj;
            tempPrj = srcPrj;
            srcPrj = destPrj;
            destPrj = tempPrj;
            tempPrj = 0;
            if (pj_is_latlong(destPrj) != 0)
            {
                //Debug.WriteLine("destination is latlong");
                convertToDegrees = true;
                //need to convert from radians to degrees after transforming
            }
            else
            {
                convertToDegrees = false;
            }
            if (pj_is_latlong(srcPrj) != 0)
            {
                //Debug.WriteLine("Source is latlong");
                convertToRadians = true;
                //need to convert degrees to radians before transforming
            }
            else
            {
                convertToRadians = false;
            }
        }
        #endregion

    }
}