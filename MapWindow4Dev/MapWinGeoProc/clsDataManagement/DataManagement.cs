// ********************************************************************************************************
// <copyright file="DataManagement.cs" company="MapWindow.org">
//     Copyright (c) MapWindow Development team. All rights reserved.
// </copyright>
// Description: Converts a shapefile Z into a regular shapefile without Z values.
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
// 01/12/06 - Angela Hillier - Provided initial API and parameter descriptions
// 06/03/06 - Angela Hillier - Added copy, delete, and rename functions
// 12/21/10 - Paul Meems     - Made this class StyleCop and ReSharper compliant and cleaned up the code
// ********************************************************************************************************

namespace MapWinGeoProc
{
    using System;
    using System.IO;
    using MapWinUtility;

    /// <summary>
    /// The DataManagement namespace will contain basic file handling routines 
    /// such as copy and delete, as well as some more complex methods for appending and merging..
    /// </summary>
    public class DataManagement
    {
        #region RenameGrid()
        /// <summary>
        /// Rename a grid (or move it's path) and all associated files.
        /// </summary>
        /// <param name="oldGridPath">The full path to the original grid file (including extension).</param>
        /// <param name="newGridPath">The full path to the new grid file (including extension).</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool RenameGrid(ref string oldGridPath, ref string newGridPath)
        {
            Logger.Dbg("RenameGrid(oldGridPath: " + oldGridPath + ",\n" +
                                     "           newGridPath: " + newGridPath + ")");
            if (CopyGrid(ref oldGridPath, ref newGridPath))
            {
                if (DeleteGrid(ref oldGridPath))
                {
                    Logger.Dbg("Finished RenameGrid");
                    return true;
                }

                Logger.Dbg("DeleteGrid returned false; Rename " + oldGridPath + " failed.");
                return false;
            }

            Logger.Dbg("CopyGrid returned false; Rename " + oldGridPath + " failed.");
            return false;
        }
        #endregion

        #region RenameShapefile()
        /// <summary>
        /// Rename a shapefile and all associated files.
        /// </summary>
        /// <param name="oldShapefilePath">Full path to the original shapefile (including the .shp extension).</param>
        /// <param name="newShapefilePath">New path to the shapefile (including the .shp extension).</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool RenameShapefile(ref string oldShapefilePath, ref string newShapefilePath)
        {
            Logger.Dbg("RenameShapefile(oldShapefilePath: " + oldShapefilePath + ",\n" +
                                     "                newSahpefilePath: " + newShapefilePath + ")");
            if (CopyShapefile(ref oldShapefilePath, ref newShapefilePath))
            {
                if (DeleteShapefile(ref oldShapefilePath))
                {
                    Logger.Dbg("Finished RenameShapefile");
                    return true;
                }

                Logger.Dbg("DeleteShapefile returned false; RenameShapefile failed.");
                return false;
            }

            Logger.Dbg("CopyShapefile returned false; RenameShapefile failed.");
            return false;
        }
        #endregion

        #region CopyGrid()
        /// <summary>
        /// Copies a grid and all associated files from one destination path to another.
        /// </summary>
        /// <param name="oldGridPath">The original path to the grid (including extension).</param>
        /// <param name="newGridPath">The path to where the grid copy should be (including extension).</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        /// <remarks>Won't work for Grid formats that are directory names like an ESRI grid format yet</remarks>
        public static bool CopyGrid(ref string oldGridPath, ref string newGridPath)
        {
            Logger.Dbg("CopyGrid(oldGridPath" + oldGridPath + ",\n" +
                                     "         newGridPath" + newGridPath + ")");
            if (string.IsNullOrEmpty(oldGridPath))
            {
                Globals.WriteErrorMessage("Argument Exception: oldGridPath cannot be null.");
                return false;
            }

            if (string.IsNullOrEmpty(newGridPath))
            {
                Globals.WriteErrorMessage("Argument Exception: newGridPath cannot be null.");
                return false;
            }

            if (!File.Exists(oldGridPath))
            {
                if (Path.GetExtension(oldGridPath) == string.Empty)
                {
                    if (Directory.Exists(oldGridPath))
                    {
                        // TODO: ESRI GRID format handling.
                        Globals.WriteErrorMessage(
                            "Argument Exception: ESRI grids that are directories are not currently supported.");
                        return false;
                    }
                }
                else
                {
                    Globals.WriteErrorMessage("Input grid does not exists: " + oldGridPath);
                }
            }

            DeleteGrid(ref newGridPath);
            
            // Copy the files that make up a grid file:

            // .grd
            TryCopy(oldGridPath, newGridPath);

            // .bmp
            TryCopy(Path.ChangeExtension(oldGridPath, ".bmp"), Path.ChangeExtension(newGridPath, ".bmp"));

            // .bpw
            TryCopy(Path.ChangeExtension(oldGridPath, ".bpw"), Path.ChangeExtension(newGridPath, ".bpw"));

            // .mwleg
            TryCopy(Path.ChangeExtension(oldGridPath, ".mwleg"), Path.ChangeExtension(newGridPath, ".mwleg"));

            // .prj
            TryCopy(Path.ChangeExtension(oldGridPath, ".prj"), Path.ChangeExtension(newGridPath, ".prj"));

            // .aux
            TryCopy(Path.ChangeExtension(oldGridPath, ".aux"), Path.ChangeExtension(newGridPath, ".aux"));

            // .rrd
            TryCopy(Path.ChangeExtension(oldGridPath, ".rrd"), Path.ChangeExtension(newGridPath, ".rrd"));

            Logger.Dbg("Finished CopyGrid");
            return true;
        }
        #endregion

        #region CopyShapefile()
        /// <summary>
        /// Copies a shapefile and all associated files.
        /// </summary>
        /// <param name="oldShapefilePath">Full path to the original shapefile (including .shp extension).</param>
        /// <param name="newShapefilePath">Full path to where the copy should be saved (including .shp extension).</param>
        /// <returns>False if an error was encoutered, true otherwise.</returns>
        public static bool CopyShapefile(ref string oldShapefilePath, ref string newShapefilePath)
        {
            Logger.Dbg("CopyShapefile(oldShapefilePath" + oldShapefilePath + ",\n" +
                                     "         newShapefilePath" + newShapefilePath + ")");
            if (string.IsNullOrEmpty(oldShapefilePath))
            {
                Globals.WriteErrorMessage("Argument Exception: oldShapefilePath cannot be null.");
                return false;
            }

            if (string.IsNullOrEmpty(newShapefilePath))
            {
                Globals.WriteErrorMessage("Argument Exception: newShapefilePath cannot be null.");
                return false;
            }

            if (!File.Exists(oldShapefilePath))
            {
                Globals.WriteErrorMessage("Input shapefile does not exists: " + oldShapefilePath);
                return false;
            }

            DeleteShapefile(ref newShapefilePath);

            // Copy the files that make up a shape file:

            // .shp
            TryCopy(oldShapefilePath, newShapefilePath);

            // .shx
            TryCopy(Path.ChangeExtension(oldShapefilePath, ".shx"), Path.ChangeExtension(newShapefilePath, ".shx"));

            // .dbf
            TryCopy(Path.ChangeExtension(oldShapefilePath, ".dbf"), Path.ChangeExtension(newShapefilePath, ".dbf"));

            // .spx
            TryCopy(Path.ChangeExtension(oldShapefilePath, ".spx"), Path.ChangeExtension(newShapefilePath, ".spx"));

            // .prj
            TryCopy(Path.ChangeExtension(oldShapefilePath, ".prj"), Path.ChangeExtension(newShapefilePath, ".prj"));

            // .sbn
            TryCopy(Path.ChangeExtension(oldShapefilePath, ".prj"), Path.ChangeExtension(newShapefilePath, ".sbn"));

            // .xml
            TryCopy(Path.ChangeExtension(oldShapefilePath, ".xml"), Path.ChangeExtension(newShapefilePath, ".xml"));

            // .shp.xml
            TryCopy(Path.ChangeExtension(oldShapefilePath, ".shp.xml"), Path.ChangeExtension(newShapefilePath, ".shp.xml"));

            Logger.Dbg("Finished CopyShapefile");

            return true;
        }
        #endregion


        /// <summary>
        /// Attempt to delete a file, logging an error if the file does not exist.
        /// </summary>
        /// <param name="file"></param>
        /// <returns>False if the file does not exist</returns>
        public static bool TryDelete(string file)
        {
            return TryDelete(file, true);
        }

        /// <summary>
        /// Attempts to delete a file.
        /// </summary>
        /// <param name="file">The filename to delete</param>
        /// <param name="mustExist">Show error if the file must exist</param>
        /// <returns>Boolean, false if the file is null or the directory doesn't exist or the file doesn't exist and mustExist is true</returns>
        public static bool TryDelete(string file, bool mustExist)
        {
            Logger.Dbg("TryDelete(file: " + file + ")");
            if (string.IsNullOrEmpty(file))
            {
                Globals.WriteErrorMessage("File cannot be null.\nTryDelete = false");
                return false;
            }

            var directory = Path.GetDirectoryName(file);
            if (string.IsNullOrEmpty(directory))
            {
                Globals.WriteErrorMessage("Directory cannot be null.\nTryDelete = false");
                return false;
            }

            if (!Directory.Exists(directory))
            {
                // Paul Meems, 7 Oct 2011
                if (mustExist)
                {
                    Globals.WriteErrorMessage(
                        "Specified Directory: " + directory + " did not exist.\nTryDelete = false");
                    return false;
                }

                Logger.Dbg("Specified Directory: " + directory + " did not exist.");
                return true;
            }

            if (!File.Exists(file))
            {
                // Paul Meems, 7 Oct 2011
                if (mustExist)
                {
                    Globals.WriteErrorMessage("Specified File: " + file + " did not exist.\nTryDelete = false");
                    return false;
                }

                Logger.Dbg("Specified File: " + file + " did not exist.");
                return true;
            }

            try
            {
                File.Delete(file);
            }
            catch (Exception ex)
            {
                Globals.WriteErrorMessage("Exception trying to delete " + file + ":\n" + ex.Message + ".\nTryDelete = false");
                return false;
            }

            Logger.Dbg("Finished TryDelete.");
            return true;
        }

        /// <summary>
        /// Attempts to copy a file
        /// </summary>
        /// <param name="oldName">The old filename</param>
        /// <param name="newName">The new filename</param>
        /// <returns>True on success</returns>
        public static bool TryCopy(string oldName, string newName)
        {
            try
            {
                if (File.Exists(oldName))
                {
                    File.Copy(oldName, newName, true);
                    var fl = new FileInfo(newName);
                    fl.Attributes = fl.Attributes & (FileAttributes.Archive & FileAttributes.ReadOnly);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Globals.WriteErrorMessage("Exception thrown while copying " + oldName + " to " + newName + ":\n" + ex.Message);
            }
            
            return false;
        }

        /// <summary>
        /// Copies the .mwleg file from the input grid to the output grid.
        /// </summary>       
        /// /// <param name="inputGf">The name of the input grid</param>
        /// <param name="resultGf">The name of the output grid</param>
        /// <returns>true on success</returns>
        public static bool CopyGridLegend(string inputGf, string resultGf)
        {
            Logger.Dbg("CopyGridLegend(inputGF: " + inputGf + ", resultGF:" + resultGf);
            var legendFile = Path.ChangeExtension(inputGf, ".mwleg");
            if (File.Exists(legendFile))
            {
                return TryCopy(legendFile, Path.ChangeExtension(resultGf, ".mwleg"));
            }

            // Paul Meems Nov. 8 2010
            // Fill the error message when the file doesn't exist:
            Globals.WriteErrorMessage("mwleg file doesn't exist: " + legendFile);
            return false;
        }

        #region DeleteGrid()
        /// <summary>
        /// Deletes the grid and associated  files (.bmp, .bpw, .mwleg, .prj).
        /// </summary>
        /// <param name="gridPath">Full path to the grid file, including extension.</param>
        /// <returns>Boolean, False if gridPath is null, the directory doesn't exist, or any of the files exist but could not be deleted.</returns>
        public static bool DeleteGrid(ref string gridPath)
        {
            Logger.Dbg("In DeleteGrid(gridPath" + gridPath + ",\n");
            if (string.IsNullOrEmpty(gridPath))
            {
                Globals.WriteErrorMessage("gridPath cannot be null.");
                return false;
            }

            var dir = Path.GetDirectoryName(gridPath);
            if (dir != null)
            {
                if (!Directory.Exists(dir))
                {
                    Globals.WriteErrorMessage("The specified directory: " + dir + " does not exist.");
                    return false;
                }
            }

            // .grd, .asc, or .tif
            TryDelete(gridPath, false);

            // .bmp
            TryDelete(Path.ChangeExtension(gridPath, ".bmp"), false);

            // .bpw
            TryDelete(Path.ChangeExtension(gridPath, ".bpw"), false);

            // .mwleg
            TryDelete(Path.ChangeExtension(gridPath, ".mwleg"), false);

            // .prj
            TryDelete(Path.ChangeExtension(gridPath, ".prj"), false);

            // .aux
            TryDelete(Path.ChangeExtension(gridPath, ".aux"), false);

            // .rrd
            TryDelete(Path.ChangeExtension(gridPath, ".rrd"), false);

            return true;
        }
        #endregion

        #region DeleteShapefile()
        /// <summary>
        /// Deletes shapefile and associated files (.shx, .dbf, .prj).
        /// </summary>
        /// <param name="shapefilePath">Full path to shapefile, including .shp extension</param>
        /// <returns>True on success</returns>
        public static bool DeleteShapefile(ref string shapefilePath)
        {
            Logger.Dbg("In DeleteShapefile(shapefilePath: " + shapefilePath + ")");
            if (string.IsNullOrEmpty(shapefilePath))
            {
                Globals.WriteErrorMessage("Shapefile path cannot be null.");
                return false;
            }

            var dir = Path.GetDirectoryName(shapefilePath);

            if (dir != null)
            {
                if (!Directory.Exists(dir))
                {
                    Globals.WriteErrorMessage("The specified directory: " + dir + " does not exist.");
                    return false;
                }
            }

            // .shp
            TryDelete(shapefilePath, false);

            // .shx
            TryDelete(Path.ChangeExtension(shapefilePath, ".shx"), false);

            // .dbf
            TryDelete(Path.ChangeExtension(shapefilePath, ".dbf"), false);

            // .prj
            TryDelete(Path.ChangeExtension(shapefilePath, ".prj"), false);

            // .spx
            TryDelete(Path.ChangeExtension(shapefilePath, ".spx"), false);

            // .sbn
            TryDelete(Path.ChangeExtension(shapefilePath, ".sbn"), false);

            // .mwsr
            TryDelete(Path.ChangeExtension(shapefilePath, ".mwsr"), false);

            // .mwsymb
            TryDelete(Path.ChangeExtension(shapefilePath, ".mwsymb"), false);

            // .lbl
            TryDelete(Path.ChangeExtension(shapefilePath, ".lbl"), false);

            // .xml
            TryDelete(Path.ChangeExtension(shapefilePath, ".xml"), false);

            // .shp.xml
            TryDelete(Path.ChangeExtension(shapefilePath, ".shp.xml"), false);

            Logger.Dbg("Finished DeleteShapefile");
            return true;
        }
        #endregion

        #region ChangeGridFormat

        /// <summary>
        /// Change the grid format?
        /// </summary>
        /// <param name="origFilename">Original grid filename</param>
        /// <param name="newFilename">Output grid filename</param>
        /// <param name="newFileType">Specifies the original file format of the grid</param>
        /// <param name="newFileFormat">Specifies the new file format</param>
        /// <param name="multFactor">Like Extrusion, this multiplies the Z value</param>
        /// <returns>True on success</returns>
        public static bool ChangeGridFormat(string origFilename, string newFilename, MapWinGIS.GridFileType newFileType, MapWinGIS.GridDataType newFileFormat, float multFactor)
        {
            var hasErrors = false;

            var grid = new MapWinGIS.Grid();
            grid.Open(origFilename, MapWinGIS.GridDataType.UnknownDataType, true, MapWinGIS.GridFileType.UseExtension, null);

            Logger.Dbg("Writing Grid to New Format");

            // If we're multiplying by a factor, must
            // create the new grid and actually do it ourselves.
            // Otherwise, can save directly
            // Jiri Kadlec 1-28-2009 we still neet to create a new grid when the data or file type is different.
            if (multFactor == 1 && newFileFormat == grid.DataType)
            {
                Logger.Dbg("Saving directly to new format");
                if (!grid.Save(newFilename, newFileType, null))
                {
                    Globals.WriteErrorMessage("Error in saving grid: " + grid.get_ErrorMsg(grid.LastErrorCode));
                    hasErrors = true;
                }
            }
            else
            {
                Logger.Dbg("Saving to new format with mult. factor: " + multFactor);

                var hdr = new MapWinGIS.GridHeader();
                hdr.CopyFrom(grid.Header);

                var newgrid = new MapWinGIS.Grid();
                if (!newgrid.CreateNew(newFilename, hdr, newFileFormat, hdr.NodataValue, true, newFileType, null))
                {
                    Globals.WriteErrorMessage("Unable to create new grid: " + newgrid.get_ErrorMsg(newgrid.LastErrorCode));
                    return false;
                }
                
                var ncols = grid.Header.NumberCols;
                var nrows = grid.Header.NumberRows;
                var oneRow = new float[ncols + 1];
                for (var i = 0; i < nrows; i++)
                {
                    grid.GetFloatWindow(i, i, 0, ncols, ref oneRow[0]);
                    // CWG 2/2/2011 little point doing this if multFactor is 1
                    if (multFactor != 1)
                    {
                    	for (var z = 0; z < ncols; z++)
                    	{
                    		oneRow[z] *= multFactor;
                    	}
                    }

                    newgrid.PutFloatWindow(i, i, 0, ncols, ref oneRow[0]);
                }

                if (!newgrid.Save(newFilename, newFileType, null))
                {
                    Globals.WriteErrorMessage("Error in saving new grid: " + newgrid.get_ErrorMsg(newgrid.LastErrorCode));
                    hasErrors = true;
                }

                if (!newgrid.Close())
                {
                    Globals.WriteErrorMessage("Error in closing new grid: " + newgrid.get_ErrorMsg(newgrid.LastErrorCode));
                    hasErrors = true;
                }
            }

            return !hasErrors;
        }
        #endregion

        /// <summary>
        /// Not Implemented
        /// Appends one shapefile to another. No intersection/union operation is performed
        /// on overlapping shapes. The input shapefile is overwritten.
        /// </summary>
        /// <param name="inputSFPath">Full path to the input shapefile.</param>
        /// <param name="appendSFPath">Full path to the shapefile that needs to be appended to the input shapefile.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        /// <remarks>This function is not yet implemented.</remarks>
        public static bool AppendShapefile(ref string inputSFPath, ref string appendSFPath)
        {
            // TODO: Implement this function
            Error.ClearErrorLog();
            Globals.WriteErrorMessage("This function is not yet implemented.");

            // PM Added NotImplementedException:
            throw new NotImplementedException();

            // return false;
        }

        /// <summary>
        /// Not Implemented
        /// Removes shapes from the shapefile that contain a specified attribute.
        /// </summary>
        /// <param name="inputSFPath">The full path to the input shapefile.</param>
        /// <param name="resultSFPath">The full path to the resulting shapefile.</param>
        /// <param name="fieldID">The ID value for which field in the input shapefile will be considered.</param>
        /// <param name="attributeLoc">The location of an attribute value to compare against for removing shapes.</param>
        /// <param name="compOperation">The comparison method to use (==, !=, >=, etc).</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        /// <remarks>This function is not yet implemented.</remarks>
        public static bool DissolveShapefile(ref string inputSFPath, ref string resultSFPath, int fieldID, int attributeLoc, int compOperation)
        {
            // TODO: Implement this function
            Error.ClearErrorLog();
            Globals.WriteErrorMessage("This function is not yet implemented.");

            // PM Added NotImplementedException:
            throw new NotImplementedException();

            // return false;
        }

        /// <summary>
        /// Not Implemented
        /// Combines two grids into one.
        /// </summary>
        /// <param name="grid1Path">Full path to the first grid.</param>
        /// <param name="grid2Path">Full path to the second grid.</param>
        /// <param name="resultGridPath">Full path to the result grid.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        /// <remarks>This function is not yet implemented.</remarks>
        public static bool MergeGrids(ref string grid1Path, ref string grid2Path, ref string resultGridPath)
        {
            // TODO: Implement this function
            Error.ClearErrorLog();
            Globals.WriteErrorMessage("This function is not yet implemented.");

            // PM Added NotImplementedException:
            throw new NotImplementedException();

            // return false;
        }

        /// <summary>
        /// Not Implemented
        /// Combines two shapefiles into one.
        /// </summary>
        /// <param name="sfPath1">Full path to the first shapefile.</param>
        /// <param name="sfPath2">Full path to the second shapefile.</param>
        /// <param name="resultSFPath">Full path to the result shapefile.</param>
        /// <param name="mergeOperation">Indicates whether Union or Intersect should be performed on overlapping shapes.</param>
        /// <param name="tableOperation">Indicates how table data should be combined.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        /// <remarks>This function is not yet implemented.</remarks>
        public static bool MergeShapefiles(ref string sfPath1, ref string sfPath2, ref string resultSFPath, int mergeOperation, int tableOperation)
        {
            // TODO: Implement this function
            Error.ClearErrorLog();
            Globals.WriteErrorMessage("This function is not yet implemented.");

            // PM Added NotImplementedException:
            throw new NotImplementedException();

            // return false;
        }

        /// <summary>
        /// Uses dialogs to obtain input and output information for processing files
        /// </summary>
        public static void MergeShapefiles()
        {
            Logger.Dbg("MergeShapefiles()");
            var merger = new clsMergeShapefiles();
            merger.DoMergeShapefiles();
            Logger.Dbg("Exit MergeShapefiles()");
        }
    }
}
