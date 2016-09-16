// ********************************************************************************************************
// <copyright file="ImageAnalysis.cs" company="MapWindow.org">
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
// 12/28/10 - Paul Meems     - Made this class StyleCop and ReSharper compliant and cleaned up the code
// ********************************************************************************************************

namespace MapWinGeoProc
{
    #region Usings

    using System;
    using System.IO;
    using System.Windows.Forms;
    using Dialogs;
    using MapWinGIS;
    using MapWinUtility;

    #endregion
    
    /// <summary>
    ///   Contains static functions for image transforms, especially filters
    /// </summary>
    public static class ImageAnalysis
    {
        #region "Apply Filter"

        #region ------- APPLY FILTER --- OVERLOADS

        #region ----------- FILE BASED

        /// <summary>
        ///   Will convolve the 2-D filter with the source image, producing the output image.
        ///   This overload assumes that you are working with files.
        /// </summary>
        /// <param name = "sourceFile">A string representing the image file to open.</param>
        /// <param name = "destFile">A string representing the image file to save to.</param>
        /// <param name = "filter">A 2D array of floats, row major.  Filter must be smaller than image.</param>
        /// <param name = "showProgressDialog">Boolean, true to have the function automatically show a dialog.</param>
        /// <param name = "callBack">A MapWinGIS.ICallback for handling errors and progress messages</param>
        /// <returns>Boolean, false if the process was canceled.</returns>
        public static bool ApplyFilter(
            string sourceFile, string destFile, float[,] filter, bool showProgressDialog, ICallback callBack)
        {
            Logger.Dbg(
                "ApplyFilter(SourceFile: " + sourceFile + ",\n" +
                "            DestFile: " + destFile + ",\n" +
                "            filter: [" + filter.GetUpperBound(0) + ", " + filter.GetUpperBound(1) + "],\n" +
                "            ShowProgressDialog: " + showProgressDialog + ",\n" +
                "            ICallback)");
            
            // Argument checks
            if (sourceFile == null)
            {
                Logger.Dbg("Argument Exception: SourceFile cannot be null.");
                throw new ArgumentException("SourceFile cannot be null.");
            }

            if (File.Exists(sourceFile) == false)
            {
                Logger.Dbg("Argument Exception: SourceFile not found.");
                throw new ArgumentException("SourceFile not found.");
            }

            if (destFile == null)
            {
                Logger.Dbg("Argument Exception: DestFile cannot be null.");
                throw new ArgumentException("DestFile cannot be null.");
            }

            if (File.Exists(sourceFile))
            {
                File.Delete(destFile);
            }

            if (filter.GetUpperBound(0) == 0 || filter.GetUpperBound(1) == 0)
            {
                Logger.Dbg("Argument Exception: Filter must have values.");
                throw new ArgumentException("Filter must have values.");
            }

            // Check image object
            var sourceImage = new Image();
            var res = sourceImage.Open(sourceFile, ImageType.USE_FILE_EXTENSION, true, null);
            if (res == false)
            {
                Logger.Dbg(
                    "Application Exception: " + "Attempting to open " + sourceFile + " produced the following error:\n" +
                    sourceImage.get_ErrorMsg(sourceImage.LastErrorCode));
                throw new ApplicationException(
                    "Attempting to open " + sourceFile + " produced the following error:\n" +
                    sourceImage.get_ErrorMsg(sourceImage.LastErrorCode));
            }

            // Try to create the output image
            var destImage = new Image();
            res = destImage.CreateNew(sourceImage.Width, sourceImage.Height);
            if (res == false)
            {
                Logger.Dbg(
                    "Application Exception: " + "Attempting to create " + destFile + " produced the following error:\n" +
                    destImage.get_ErrorMsg(destImage.LastErrorCode));
                throw new ApplicationException(
                    "Attempting to create " + destFile + " produced the following error:\n" +
                    destImage.get_ErrorMsg(destImage.LastErrorCode));
            }

            destImage.Save(destFile, false, ImageType.USE_FILE_EXTENSION, callBack);
            res = DoApplyFilter(sourceImage, ref destImage, filter, showProgressDialog, callBack);
            destImage.Close();
            sourceImage.Close();
            Logger.Dbg("Finished ApplyFilter");
            return res;
        }

        ///<summary>
        ///  Will convolve the 2-D filter with the source image, producing the output image.
        ///  This overload assumes that you are working with files.
        ///</summary>
        ///<param name = "sourceFile">A string representing the image file to open.</param>
        ///<param name = "destFile">A string representing the image file to save to.</param>
        ///<param name = "filter">A 2D array of floats, row major.  Filter must be smaller than image.</param>
        ///<returns>Boolean, false if the process was canceled.</returns>
        public static bool ApplyFilter(string sourceFile, string destFile, float[,] filter)
        {
            Logger.Dbg(
                "ApplyFilter(SourceFile: " + sourceFile + ",\n" +
                "            DestFile: " + destFile + ",\n" +
                "            filter: [" + filter.GetUpperBound(0) + ", " + filter.GetUpperBound(1) + "])");
            // The default for show dialog is true, the default for icallback is null.
            Logger.Dbg("Finsihed ApplyFilter");
            return ApplyFilter(sourceFile, destFile, filter, true, null);
        }

        #endregion

        #region ----------- OBJECT BASED

        /// <summary>
        ///   Will convolve the SourceImage specified using the filter specified, returning the result in a new image
        ///   as the ref parameter DestImage.
        /// </summary>
        /// <param name = "sourceImage">A MapWinGIS.Image object to be processed</param>
        /// <param name = "destImage">The output MapWinGIS.Image object from this process</param>
        /// <param name = "filter">The 2D float array of filter coefficients to use (Row Major)</param>
        /// <param name = "showProgressDialog">If true, will show progress in a typical dialog form</param>
        /// <param name = "callBack">If specified, will return data as a MapWinGIS.ICallBack object</param>
        /// <returns>Boolean, false if the process was canceled.</returns>
        public static bool ApplyFilter(
            Image sourceImage, ref Image destImage, float[,] filter, bool showProgressDialog, ICallback callBack)
        {
            // Report unnamed as differently from null
            var sourceFile = "null";
            var destFile = "null";

            if (sourceImage != null)
            {
                sourceFile = sourceImage.Filename ?? "Unnamed";
            }

            if (destImage != null)
            {
                if (sourceImage != null)
                {
                    destFile = sourceImage.Filename;
                }

                if (destFile == null)
                {
                    destFile = "Unnamed";
                }
            }
            Logger.Dbg(
                "ApplyFilter(SourceImage: " + sourceFile + ",\n" +
                "            DestImage: " + destFile + ",\n" +
                "            filter: [" + filter.GetUpperBound(0) + ", " + filter.GetUpperBound(1) + "],\n" +
                "            ShowProgressDialog: " + showProgressDialog + ",\n" +
                "            ICallback)");

            return DoApplyFilter(sourceImage, ref destImage, filter, showProgressDialog, callBack);
        }

        /// <summary>
        ///   Will convolve the SourceImage specified using the filter specified, returning the result in a new image
        ///   as the ref parameter DestImage.
        /// </summary>
        /// <param name = "sourceImage">A MapWinGIS.Image object to be processed</param>
        /// <param name = "destImage">The output MapWinGIS.Image object from this process</param>
        /// <param name = "filter">The 2D float array of filter coefficients to use (Row Major)</param>
        /// <returns>Boolean, false if the process was canceled.</returns>
        public static bool ApplyFilter(Image sourceImage, ref Image destImage, float[,] filter)
        {
            // Report unnamed as differently from null
            var sourceFile = "null";
            var destFile = "null";

            if (sourceImage != null)
            {
                sourceFile = sourceImage.Filename ?? "Unnamed";
            }

            if (destImage != null)
            {
                if (sourceImage != null)
                {
                    destFile = sourceImage.Filename;
                }

                if (destFile == null)
                {
                    destFile = "Unnamed";
                }
            }

            Logger.Dbg(
                "ApplyFilter(SourceImage: " + sourceFile + ",\n" +
                "            DestImage: " + destFile + ",\n" +
                "            filter: [" + filter.GetUpperBound(0) + ", " + filter.GetUpperBound(1) + "])");

            return DoApplyFilter(sourceImage, ref destImage, filter, true, null);
        }

        #endregion

        #endregion

        // False if the user canceled, true otherwise
        private static bool DoApplyFilter(
            IImage sourceImage, ref Image destImage, float[,] filter, bool showProgressDialog, ICallback callBack)
        {
            var prog = 0;
            var oldProg = 0;

            if (sourceImage == null)
            {
                Logger.Dbg("Argument Exception: SourceImage cannot be null.");
                throw new ArgumentException("SourceImage cannot be null.");
            }

            if (destImage == null)
            {
                Logger.Dbg("Argument Exception: DestImage cannot be null.");
                throw new ArgumentException("DestImage cannot be null.");
            }

            Logger.Dbg(
                "Do_ApplyFilter(SourceImage: " + sourceImage.Filename + ",\n" +
                "            DestImage: " + destImage.Filename + ",\n" +
                "            filter: [" + filter.GetUpperBound(0) + ", " + filter.GetUpperBound(1) + "],\n" +
                "            ShowProgressDialog: " + showProgressDialog + ",\n" +
                "            ICallback)");

            var myProgress = new ProgressDialog();

            if (filter.GetUpperBound(0) == 0 || filter.GetUpperBound(1) == 0)
            {
                Logger.Dbg("Argument Exception: Filter must have values.");
                throw new ArgumentException("Filter must have values.");
            }

            // Ensure the filter is smaller than the image.
            if (filter.GetUpperBound(0) > sourceImage.Height || filter.GetUpperBound(1) > sourceImage.Width)
            {
                throw new ArgumentException(
                    "The filter is too large for this image.  In order for convolution to work, the image must be larger than the filter.");
            }

            // We are going to assume mirror handling of edges
            var locHandler = new ExtHandler(sourceImage.Height, sourceImage.Width);

            // convolve
            int xcor = 0, ycor = 0; // Corrected X and Y locations to take into account mirror
            var fH = filter.GetUpperBound(0) / 2;
            var fW = filter.GetUpperBound(1) / 2;

            if (callBack == null)
            {
                myProgress.Show();
                myProgress.WriteMessage("Applying Filter...");
                Logger.Progress("Applying Filter...", prog, oldProg);
            }

            for (var row = 0; row < sourceImage.Height; row++)
            {
                for (var col = 0; col < sourceImage.Width; col++)
                {
                    float fR = 0;
                    float fG = 0;
                    float fB = 0;

                    int r;
                    int g;
                    int b;
                    int color;
                    for (var y = 0; y <= filter.GetUpperBound(0); y++)
                    {
                        for (var x = 0; x <= filter.GetUpperBound(1); x++)
                        {
                            // Read the color for this spot
                            locHandler.CorrectLocation(col + x - fW, row + y - fH, ref xcor, ref ycor);
                            color = sourceImage.get_Value(ycor, xcor);
                            r = color % 256;
                            g = (color / 256) % 256;
                            b = (color / (256 * 256));

                            // convolve the values with the filter and add them to the accumulators
                            fR += filter[y, x] * r;
                            fG += filter[y, x] * g;
                            fB += filter[y, x] * b;
                        }
                    }

                    // After convolution, write the combined value to the file
                    r = (int)fR;
                    if (r > 255)
                    {
                        r = 255;
                    }

                    g = (int)fG;
                    if (g > 255)
                    {
                        g = 255;
                    }

                    b = (int)fB;
                    if (b > 255)
                    {
                        b = 255;
                    }

                    color = (256 * 256) * b + 256 * g + r;
                    destImage.set_Value(row, col, color);
                }

                prog = (100 * row) / sourceImage.Height;
                if (prog <= oldProg)
                {
                    continue;
                }

                if (callBack != null)
                {
                    callBack.Progress("Status", prog, "Filtering Image...");
                }

                if (showProgressDialog)
                {
                    myProgress.Progress = prog;

                    if (myProgress.IsCanceled)
                    {
                        Logger.Message(
                            "Apply Filter was canceled.",
                            "Process Canceled",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            DialogResult.OK);
                        return false;
                    }
                }

                Logger.Progress("Filtering Image..." + prog + "%Complete", prog, oldProg);
                oldProg = prog;
            }

            myProgress.Hide();
            destImage.dX = sourceImage.dX;
            destImage.dY = sourceImage.dY;
            destImage.SetProjection(sourceImage.GetProjection());
            destImage.Save(destImage.Filename, true, ImageType.USE_FILE_EXTENSION, null);
            Logger.Dbg("Finished ApplyFilter");
            return true;
        }

        // Ensures that we use mirror edges when a filter extends beyond the end of the image
        private class ExtHandler
        {
            private readonly int height;
            private readonly int width;

            public ExtHandler(int height, int width)
            {
                this.height = height;
                this.width = width;
            }

            public void CorrectLocation(int x, int y, ref int xcor, ref int ycor)
            {
                xcor = x;
                ycor = y;
                if (x > this.width - 1)
                {
                    xcor = this.width - (x - (this.width - 1));
                }
                if (y > this.height - 1)
                {
                    ycor = this.height - (y - (this.height - 1));
                }
                if (x < 0)
                {
                    xcor = Math.Abs(x) - 1;
                }
                if (y < 0)
                {
                    ycor = Math.Abs(y) - 1;
                }
            }
        }

        #endregion

        #region "Difference"

        /// <summary>
        ///   Calculates the difference values and stores them in the Dest grid.  This only works if the grids
        ///   have the same number of rows and columns.
        /// </summary>
        /// <param name = "source1">MapWinGIS.Grid representing one source grid</param>
        /// <param name = "source2">MapWinGIS.Grid to compare Source1 against</param>
        /// <param name = "dest">MapWinGIS.Grid where the output is to be saved</param>
        /// <param name = "callBack">A MapWinGIS.ICallBack</param>
        /// <remarks>
        ///   Uses ArgumentExceptions if the grids are different sizes
        /// </remarks>
        public static void Difference(Grid source1, Grid source2, Grid dest, ICallback callBack)
        {
            // Log entrance as best as possible while preventing errors from a null reference
            var source1File = "null";
            var source2File = "null";
            var destFile = "null";
            if (source1 != null)
            {
                source1File = source1.Filename ?? "Unnamed";
            }

            if (source2 != null)
            {
                source2File = source2.Filename ?? "Unnamed";
            }

            if (dest != null)
            {
                destFile = dest.Filename ?? "Unnamed";
            }

            Logger.Dbg(
                "Difference(Source1: " + source1File + ",\n" +
                "           Source2: " + source2File + ",\n" +
                "           Dest: " + destFile + ",\n" +
                "           ICallback");

            if (source1 == null)
            {
                Logger.Dbg("Argument Exception: Source1 cannot be null.");
                throw new ArgumentException("Source1 cannot be null.");
            }

            if (source2 == null)
            {
                Logger.Dbg("Argument Exception: Source2 cannot be null.");
                throw new ArgumentException("Source2 cannot be null.");
            }

            if (dest == null)
            {
                Logger.Dbg("Argument Exception: Dest cannot be null.");
                throw new ArgumentException("Dest cannot be null.");
            }

            var nX = source1.Header.NumberCols;
            var nY = source1.Header.NumberRows;
            if (source2.Header.NumberRows != nY)
            {
                Logger.Dbg("Argument Exception: The grids are not the same height.");
                throw new ArgumentException("The grids are not the same height.");
            }

            if (source2.Header.NumberCols != nX)
            {
                Logger.Dbg("Argument Exception: The grids are not the same width!");
                throw new ArgumentException("The grids are not the same width!");
            }

            if (dest.Header.NumberRows != nY)
            {
                Logger.Dbg("Argument Exception: The output grid is not the same height!");
                throw new ArgumentException("The output grid is not the same height!");
            }

            if (dest.Header.NumberCols != nX)
            {
                Logger.Dbg("Argument Exception: The output grid is not the same width!");
                throw new ArgumentException("The output grid is not the same width!");
            }

            var oldProg = 0;
            var prog = 0;
            if (callBack != null)
            {
                callBack.Progress("Status", 0, "Difference...0% Complete");
            }

            Logger.Progress("Difference...0% Complete", prog, oldProg);
            if (source1.DataType != GridDataType.FloatDataType)
            {
                switch (source1.DataType)
                {
                    case GridDataType.DoubleDataType:
                        for (var y = 0; y < nY; y++)
                        {
                            for (var x = 0; x < nX; x++)
                            {
                                var val = (double)source1.get_Value(x, y) - (double)source2.get_Value(x, y);
                                dest.set_Value(x, y, val);
                            }

                            if (callBack == null)
                            {
                                continue;
                            }

                            prog = (y * 100 / nY);
                            if (prog <= oldProg)
                            {
                                continue;
                            }

                            callBack.Progress("Status", prog, "Difference..." + prog + "% Complete");
                            Logger.Progress("Difference..." + prog + "% Complete", prog, oldProg);
                            oldProg = prog;
                        }
                        break;
                    case GridDataType.UnknownDataType:
                        for (var y = 0; y < nY; y++)
                        {
                            for (var x = 0; x < nX; x++)
                            {
                                var val = (double)source1.get_Value(x, y) - (double)source2.get_Value(x, y);
                                dest.set_Value(x, y, val);
                            }

                            if (callBack == null)
                            {
                                continue;
                            }

                            prog = (y * 100 / nY);
                            if (prog <= oldProg)
                            {
                                continue;
                            }

                            callBack.Progress("Status", prog, "Difference..." + prog + "% Complete");
                            Logger.Progress("Difference..." + prog + "% Complete", prog, oldProg);
                            oldProg = prog;
                        }
                        break;
                    case GridDataType.LongDataType:
                        for (var y = 0; y < nY; y++)
                        {
                            for (var x = 0; x < nX; x++)
                            {
                                var val = (long)source1.get_Value(x, y) - (long)source2.get_Value(x, y);
                                dest.set_Value(x, y, val);
                            }

                            if (callBack == null)
                            {
                                continue;
                            }

                            prog = (y * 100 / nY);
                            if (prog <= oldProg)
                            {
                                continue;
                            }

                            Logger.Progress("Difference..." + prog + "% Complete", prog, oldProg);
                            callBack.Progress("Status", prog, "Difference..." + prog + "% Complete");
                            oldProg = prog;
                        }
                        break;
                    case GridDataType.ShortDataType:
                        for (var y = 0; y < nY; y++)
                        {
                            for (var x = 0; x < nX; x++)
                            {
                                var val = (int)source1.get_Value(x, y) - (int)source2.get_Value(x, y);
                                dest.set_Value(x, y, val);
                            }

                            if (callBack == null)
                            {
                                continue;
                            }

                            prog = (y * 100 / nY);
                            if (prog <= oldProg)
                            {
                                continue;
                            }

                            Logger.Progress("Difference..." + prog + "% Complete", prog, oldProg);
                            callBack.Progress("Status", prog, "Difference..." + prog + "% Complete");
                            oldProg = prog;
                        }
                        break;
                    default:
                        Logger.Progress("The Datatype was not a valid numeric type.", prog, oldProg);
                        throw new ArgumentException("The Datatype was not a valid numeric type.");
                }
            }
            else
            {
                for (var y = 0; y < nY; y++)
                {
                    var vals1 = new float[nX];
                    var vals2 = new float[nX];
                    var diff = new float[nX];
                    source1.GetRow(y, ref vals1[0]);
                    source2.GetRow(y, ref vals2[0]);
                    for (var x = 0; x < nX; x++)
                    {
                        diff[x] = vals1[x] - vals2[x];
                    }

                    dest.PutRow(y, ref diff[0]);
                    
                    if (callBack == null)
                    {
                        continue;
                    }

                    prog = (y * 100 / nY);
                    if (prog <= oldProg)
                    {
                        continue;
                    }

                    Logger.Progress("Difference..." + prog + "% Complete", prog, oldProg);
                    callBack.Progress("Status", prog, "Difference..." + prog + "% Complete");
                    oldProg = prog;
                }
            }

            Logger.Dbg("Finished Difference");
            if (callBack != null)
            {
                callBack.Progress("Status", 0, "Done.");
            }
        }

        /// <summary>
        ///   This overload calculates the difference between files.  THe number of rows and columns should be the same.
        /// </summary>
        /// <param name = "sourceFile1">String filename of one grid to compare</param>
        /// <param name = "sourceFile2">String filename of another grid to compare</param>
        /// <param name = "destFile">String filename of the output difference file</param>
        /// <param name = "overwrite">Boolean, true if you wish to overwrite an existing output 
        ///   file and delete the associated .bmp file.  False raises a messagebox if the files exist.</param>
        /// <param name = "callBack">A MapWinGIS.ICallBack for status messages</param>
        public static void Difference(
            string sourceFile1, string sourceFile2, string destFile, bool overwrite, ICallback callBack)
        {
            var source1 = new Grid();
            var source2 = new Grid();
            var dest = new Grid();

            // Open the source grids
            if (callBack != null)
            {
                callBack.Progress("Status", 0, "Opening Files...");
            }
            
            var res = source1.Open(sourceFile1, GridDataType.UnknownDataType, true, GridFileType.UseExtension, callBack);
            if (res == false)
            {
                Logger.Dbg("Argument Exception: " + source1.get_ErrorMsg(source1.LastErrorCode));
                throw new ArgumentException(source1.get_ErrorMsg(source1.LastErrorCode));
            }

            res = source2.Open(sourceFile2, GridDataType.UnknownDataType, true, GridFileType.UseExtension, callBack);
            if (res == false)
            {
                throw new ArgumentException(source2.get_ErrorMsg(source2.LastErrorCode));
            }

            // Delete any existing files for our output grid
            if (File.Exists(destFile))
            {
                var bmp = Path.ChangeExtension(destFile, "bmp");
                var bpw = Path.ChangeExtension(destFile, "bpw");
                var prj = Path.ChangeExtension(destFile, "prj");
                var mwleg = Path.ChangeExtension(destFile, "mwleg");
                if (overwrite == false)
                {
                    if (File.Exists(bmp) || File.Exists(bpw) ||
                        File.Exists(prj) || File.Exists(mwleg))
                    {
                        if (
                            Logger.Message(
                                "The output file exists, or associated files of the same name exist.  Do you wish to delete the existing files?\n",
                                "Output Files Exist",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning,
                                DialogResult.No) == DialogResult.No)
                        {
                            return;
                        }

                        // This ensures mapwindow will recognize the new image as a new file.
                        if (File.Exists(bmp))
                        {
                            File.Delete(bmp);
                        }

                        if (File.Exists(bpw))
                        {
                            File.Delete(bpw);
                        }

                        if (File.Exists(prj))
                        {
                            File.Delete(prj);
                        }

                        if (File.Exists(mwleg))
                        {
                            File.Delete(mwleg);
                        }
                    }
                    else
                    {
                        if (Logger.Message(
                            "The output file already exists.  Do you wish to delete it?",
                            "Destination File Already Exists",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning,
                            DialogResult.No)
                            == DialogResult.No)
                        {
                            return;
                        }
                    }
                }
                else
                {
                    if (File.Exists(bmp))
                    {
                        File.Delete(bmp);
                    }

                    if (File.Exists(bpw))
                    {
                        File.Delete(bpw);
                    }

                    if (File.Exists(prj))
                    {
                        File.Delete(prj);
                    }

                    if (File.Exists(mwleg))
                    {
                        File.Delete(mwleg);
                    }
                }

                File.Delete(destFile);
            }

            // Create a new output grid
            var newHeader = new GridHeader();
            newHeader.CopyFrom(source1.Header);
            if (callBack != null)
            {
                Logger.Dbg("Creating Output File...");
                callBack.Progress("Status", 0, "Creating Output File...");
            }

            res = dest.CreateNew(destFile, newHeader, source1.DataType, 0, true, GridFileType.UseExtension, callBack);
            if (res == false)
            {
                Logger.Dbg("Argument Exception: " + dest.get_ErrorMsg(dest.LastErrorCode));
                throw new ArgumentException(dest.get_ErrorMsg(dest.LastErrorCode));
            }

            // Calculate the differences
            Difference(source1, source2, dest, callBack);

            // Close Source grids
            source1.Close();
            source2.Close();

            // Save and close the output grid
            res = dest.Save(destFile, GridFileType.UseExtension, callBack);
            if (res == false)
            {
                Logger.Dbg("Application Exception: " + dest.get_ErrorMsg(dest.LastErrorCode));
                throw new ArgumentException(dest.get_ErrorMsg(dest.LastErrorCode));
            }

            dest.Close();
            Logger.Dbg("Finished Difference");
        }

        #endregion

        // This is not a full fledged map algebra function!  This cannot deal with
        // images of different sizes or cell spacings.  This is only a quick function
        // that can tell if, for instance, a pitfilled image is different from the
        // version created in Arcview.  It simply takes each cell and finds the difference
        // in values.
    }
}