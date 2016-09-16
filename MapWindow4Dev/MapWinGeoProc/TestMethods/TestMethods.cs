// ********************************************************************************************************
// <copyright file="TestMethods.cs" company="MapWindow.org">
//     Copyright (c) MapWindow Development team. All rights reserved.
// </copyright>
// ********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http:// www.mozilla.org/MPL/ 
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
// 
// The initial developer of this code is Paul Meems (MapWindow.nl)
//
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// Change Log: 
//  Date            Changed By     Notes
//  05 July 2010    Paul Meems     Inital coding
//  08 July 2010    Paul Meems     Made the code StyleCop compliant
//  30 Aug  2010    Paul Meems     Checked code with ReSharper
//  16 Sep  2010    Paul Meems     Added some new methods
//  10 Mar  2011    Paul Meems     Added some new methods and made changes recommended by ReSharper
//  03 Apr  2011    Paul Meems     Added timer to catch stalling scripts
// ********************************************************************************************************

namespace MapWinGeoProc.TestMethods
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.IO;
    using System.Security.Cryptography;
    using System.Timers;
    using System.Windows.Forms;

    using MapWindow.Interfaces;

    using MapWinGIS;

    #endregion

    /// <summary>
    ///   Class to hold the methods used in the test scripts
    /// </summary>
    public class TestMethods : ICallback
    {
        #region Fields

        /// <summary>
        ///   The reference to the MapWindow Interfaces
        /// </summary>
        private readonly IMapWin mapWin;

        /// <summary>
        ///   The name of the script, used while logging
        /// </summary>
        private readonly string scriptName = string.Empty;

        /// <summary>
        ///   Should the map be cleared
        /// </summary>
        private readonly bool startWithEmptyMap = true;

        /// <summary>
        ///   If true, close MW after each script
        /// </summary>
        private bool calledAsBatch;

        /// <summary>
        ///   The name of the logfile
        /// </summary>
        private string logFilename = string.Empty;

        /// <summary>
        ///   To count the time needed to process this script
        /// </summary>
        private Stopwatch stopWatchGlobale;

        /// <summary>
        ///   The name of the summery log
        /// </summary>
        private string summeryFilename = string.Empty;

        /// <summary>
        ///   The result of the test.
        /// </summary>
        private TestResults testResults = TestResults.Crashed;

        /*
        /// <summary>
        ///   The timer to catch time-outs 
        /// </summary>
        private System.Timers.Timer timer;

        /// <summary>
        ///   The time out value
        /// </summary>
        private int timeOutValue;
        */

        #endregion

        #region Constructor

        /// <summary>
        ///   Initializes a new instance of the TestMethods class.
        /// </summary>
        /// <param name = "scriptName">The name for the script, used when logging</param>
        /// <param name = "mapWin">The reference to the MapWindow Interfaces</param>
        /// <param name = "startWithEmptyMap">Should the map be cleared</param>
        public TestMethods(string scriptName, IMapWin mapWin, bool startWithEmptyMap)
        {
            this.scriptName = scriptName;
            this.mapWin = mapWin;
            this.startWithEmptyMap = startWithEmptyMap;

            // Init:
            this.Init();
        }

        #endregion

        #region Enumerations

        #region CompareResult enum

        /// <summary>
        ///   Enumeration for the image compare methods
        /// </summary>
        public enum CompareResult
        {
            /// <summary>
            ///   The images are equal
            /// </summary>
            CompareOk,

            /// <summary>
            ///   The pixels don't match
            /// </summary>
            PixelMismatch,

            /// <summary>
            ///   The images have different sizes
            /// </summary>
            SizeMismatch
        }

        #endregion

        #region TestResults enum

        /// <summary>
        ///   Enumeration for the test results
        /// </summary>
        public enum TestResults
        {
            /// <summary>
            ///   The test was successfull
            /// </summary>
            Successfull,

            /// <summary>
            ///   The test failed
            /// </summary>
            Failed,

            /// <summary>
            ///   MapWindow crashed
            /// </summary>
            Crashed,

            /// <summary>
            ///   The script timed out    
            /// </summary>
            TimedOut
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the folder where the test data is
        /// </summary>
        public string WorkingFolder { get; set; }

        /// <summary>
        ///   Gets or sets the folder where temporarily data should be stored
        /// </summary>
        public string TempFolder { get; set; }

        /*
        /// <summary>
        ///    Gets or sets the time out value in minutes
        /// </summary>
        public int TimeOutValue
        {
            get
            {
                return this.timeOutValue;
            }

            set
            {
                this.timeOutValue = value;
                this.timer.Interval = this.timeOutValue; // *60 * 1000;
            }
        }
        */

        #endregion

        #region Implementation of ICallback

        /// <summary>Report the progress</summary>
        /// <param name="keyOfSender">The key or sender</param>
        /// <param name="percent">The percentage</param>
        /// <param name="message">The message</param>
        public void Progress(string keyOfSender, int percent, string message)
        {
            if (percent % 10 == 0)
            {
                // Report every 10 percent:
                this.ReportMessage("Still processing " + message);
            }
        }

        /// <summary>Report the error</summary>
        /// <param name="keyOfSender">The key or sender</param>
        /// <param name="errorMsg">The error message</param>
        public void Error(string keyOfSender, string errorMsg)
        {
            this.ReportMessage(string.Format("{0} reported an error: {1}", keyOfSender, errorMsg));
        }

        #endregion

        #region Public methods

        /// <summary>Copy the grid from one location to another</summary>
        /// <param name="dir1">The original location</param>
        /// <param name="dir2">The new location</param>
        /// <param name="grid">The name of the grid</param>
        public void CopyGrid(string dir1, string dir2, string grid)
        {
            try
            {
                var source = Path.Combine(dir1, grid);
                var target = Path.Combine(dir2, grid);
                DataManagement.CopyGrid(ref source, ref target);
            }
            catch (Exception ex)
            {
                this.ReportMessage("Warning: " + ex.Message);
            }
        }

        /// <summary>
        ///   Adds the message to the message log file
        /// </summary>
        /// <param name = "msg">The message</param>
        public void ReportMessage(string msg)
        {
            this.WriteToFile(this.logFilename, msg);
        }

        /// <summary>
        ///   Adds the message to the summery log file
        /// </summary>
        /// <param name = "successful">Did the test run successful</param>
        public void AddToSummary(bool successful)
        {
            this.testResults = successful ? TestResults.Successfull : TestResults.Failed;

            var msg = "The test script " + this.testResults;

            // Get total duration of this script:
            msg += " [Time the script needed: " + this.GetDurationOfScript() + "]";
            this.WriteToFile(this.summeryFilename, msg);

            // Save also to environment var so the test tool can check for it:
            Environment.SetEnvironmentVariable(
                "MWTESTRESULT", this.scriptName + "=" + this.testResults, EnvironmentVariableTarget.User);
        }

        /// <summary>
        ///   Finally, end the test script, end all message loops and end MapWindow.
        /// </summary>
        public void EndScript()
        {
            this.EndScript(true);
        }

        /// <summary>
        ///   Finally, end the test script, end all message loops and end MapWindow.
        /// </summary>
        /// <param name = "deleteTempFiles">Delete the temp files or not</param>
        public void EndScript(bool deleteTempFiles)
        {
            /*
            // End timer:
            this.timer.Stop();
            this.timer.Close();
            this.timer.Dispose();
            */

            try
            {
                if (this.calledAsBatch)
                {
                    // Remove all layers because else we cannot delete the files:
                    this.mapWin.Layers.Clear();
                }

                if (deleteTempFiles || this.calledAsBatch)
                {
                    Directory.Delete(this.TempFolder, true);
                }
            }
            catch (Exception ex)
            {
                this.ReportMessage("ERROR! Cannot delete tempfolder: " + ex.Message);
            }

            // Finally, end the test script, end all message loops, end MapWindow.
            this.ReportMessage("Script has finished");

            if (this.calledAsBatch)
            {
                Application.ExitThread();
            }
            else
            {
                // Show the logfiles:
                Process.Start(this.logFilename);
                Process.Start(this.summeryFilename);
            }
        }

        /// <summary>
        ///   Opens a shapefile
        /// </summary>
        /// <param name = "filename">The file to open</param>
        /// <returns>The opened shapefile</returns>
        /// <exception cref="FileNotFoundException">When file does not exists</exception>
        /// <exception cref="Exception">On error opening shapefile</exception>
        public Shapefile OpenShapeFile(string filename)
        {
            return this.OpenShapeFile(filename, false);
        }

        /// <summary>
        ///   Opens a shapefile
        /// </summary>
        /// <param name = "filename">The file to open</param>
        /// <param name="inEditMode">Call StartEditingShapes or not</param>
        /// <returns>The opened shapefile</returns>
        /// <exception cref="FileNotFoundException">When file does not exists</exception>
        /// <exception cref="Exception">On error opening shapefile</exception>
        /// <exception cref="Exception">On error StartEditingShapes</exception>
        public Shapefile OpenShapeFile(string filename, bool inEditMode)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("Error in OpenShapeFile. File does not exists", filename);
            }

            var sf = new Shapefile();
            sf.Open(filename, null);
            if (sf == null)
            {
                throw new Exception("Error in opening shapefile: " + sf.get_ErrorMsg(sf.LastErrorCode));
            }

            if (inEditMode)
            {
                // Goto Edit mode:
                if (!sf.StartEditingShapes(true, null))
                {
                    throw new Exception("Error in sf.StartEditingShapes: " + sf.get_ErrorMsg(sf.LastErrorCode));
                }
            }

            return sf;
        }

        /// <summary>
        /// Add a new field to the shapefile
        /// </summary>
        /// <param name="sf">The shapefile</param>
        /// <param name="fld">The new field</param>
        /// <returns>The index of the added field</returns>
        public int AddField(ref Shapefile sf, Field fld)
        {
            var fldIndex = sf.NumFields;
            if (!sf.EditInsertField(fld, ref fldIndex, null))
            {
                throw new Exception("Error in sf.EditInsertField: " + sf.get_ErrorMsg(sf.LastErrorCode));
            }

            return fldIndex;
        }

        /// <summary>
        ///   Copies the attributes from 1 shapefile to another.
        ///   It assumes the shapefiles are cloned versions so the number and order of the fields are equal
        /// </summary>
        /// <param name = "sfFrom">The shapefile to copy from</param>
        /// <param name = "shapeIDFrom">The ID of the shape that holds the values</param>
        /// <param name = "sfTo">The shapefile to copy to</param>
        /// <param name = "shapeIDTo">The ID of the shape that will get the values</param>
        public void CopyAttributes(Shapefile sfFrom, int shapeIDFrom, Shapefile sfTo, int shapeIDTo)
        {
            // Assuming sfTo is a cloned version of sfFrom so the order of fields is equal

            // Do some small checks:
            if (sfFrom.NumFields != sfTo.NumFields)
            {
                throw new Exception("Cannot copy attributes. Shapefiles don't have the same number of fields.");
            }

            if (!sfTo.EditingShapes)
            {
                sfTo.StartEditingShapes(true, null);
            }

            for (var i = 0; i < sfFrom.NumFields; i++)
            {
                sfTo.EditCellValue(i, shapeIDTo, sfFrom.get_CellValue(i, shapeIDFrom));
            }
        }

        /// <summary>
        ///   Gets a shape object based in a shapeID
        ///   If the shape isn't valid it will try to buffer it.
        ///   Most of the times this will return a valid shape.
        /// </summary>
        /// <param name = "sf">The shapefile the shape is in</param>
        /// <param name = "shapeID">The ID of the shape to get</param>
        /// <returns>A shape object</returns>
        public MapWinGIS.Shape GetShape(Shapefile sf, int shapeID)
        {
            if (sf == null)
            {
                throw new ArgumentNullException("sf");
            }

            var shp = sf.get_Shape(shapeID);
            if (!shp.IsValid)
            {
                // try the buffer trick to make it valid:
                shp = shp.Buffer(0.0, 0);
            }

            if (!shp.IsValid)
            {
                throw new Exception("The shape is invalid and cannot be fixed.");
            }

            return shp;
        }

        /// <summary>
        /// Get the field by name
        /// </summary>
        /// <param name="sf">The shapefile with the field</param>
        /// <param name="name">The name of the field</param>
        /// <returns>The field index if found else -1</returns>
        public int GetFieldByName(ref Shapefile sf, string name)
        {
            var retVal = -1;

            for (var i = 0; i < sf.NumFields; i++)
            {
                if (sf.get_Field(i).Name != name)
                {
                    continue;
                }

                retVal = i;
                break;
            }

            return retVal;
        }

        /// <summary>
        ///   Refreshes the maps and calls DoEvents
        /// </summary>
        public void RefreshMap()
        {
            if (this.calledAsBatch)
            {
                return;
            }

            this.mapWin.Refresh();
            Application.DoEvents();
        }

        /// <summary>
        ///   Stops the stopwatch and logs the elapsed time
        /// </summary>
        /// <param name = "action">The name of the action</param>
        /// <param name = "stopWatch">The stopWatch object</param>
        /// <returns>The elapsed time in milliseconds</returns>
        public long EndStopWatch(string action, ref Stopwatch stopWatch)
        {
            Application.DoEvents();
            stopWatch.Stop();
            var elapsedTime = GetElapsedTime(stopWatch);
            this.ReportMessage("Time needed for " + action + ": " + elapsedTime);
            return stopWatch.ElapsedMilliseconds;
        }

        /// <summary>
        ///   Compares two bitmaps on size and pixels
        /// </summary>
        /// <param name = "filename1">First bitmap</param>
        /// <param name = "filename2">Second bitmap</param>
        /// <returns>Enumerator with compare results</returns>
        public CompareResult CompareBitmaps(string filename1, string filename2)
        {
            CompareResult retVal;

            if (!File.Exists(filename1))
            {
                throw new FileNotFoundException("Error in CompareBitmaps. File does not exists", filename1);
            }

            if (!File.Exists(filename2))
            {
                throw new FileNotFoundException("Error in CompareBitmaps. File does not exists", filename2);
            }

            using (var bitmap1 = (Bitmap)System.Drawing.Image.FromFile(filename1))
            {
                using (var bitmap2 = (Bitmap)System.Drawing.Image.FromFile(filename2))
                {
                    retVal = this.CompareBitmaps(bitmap1, bitmap2);
                }
            }

            return retVal;
        }

        /// <summary>
        ///   Compares two bitmaps on size and pixels
        /// </summary>
        /// <param name = "bmp1">First bitmap</param>
        /// <param name = "bmp2">Second bitmap</param>
        /// <returns>Enumerator with compare results</returns>
        public CompareResult CompareBitmaps(Bitmap bmp1, Bitmap bmp2)
        {
            var cr = CompareResult.CompareOk;

            // Test to see if we have the same size of image
            if (bmp1.Size != bmp2.Size)
            {
                cr = CompareResult.SizeMismatch;
            }
            else
            {
                // Convert each image to a byte array
                var ic = new ImageConverter();
                var byteImage1 = new byte[1];
                byteImage1 = (byte[])ic.ConvertTo(bmp1, byteImage1.GetType());
                var byteImage2 = new byte[1];
                byteImage2 = (byte[])ic.ConvertTo(bmp2, byteImage2.GetType());

                // Compute a hash for each image
                var shaM = new SHA256Managed();
                if (byteImage1 != null)
                {
                    var hash1 = shaM.ComputeHash(byteImage1);
                    if (byteImage2 != null)
                    {
                        var hash2 = shaM.ComputeHash(byteImage2);

                        // Compare the hash values
                        for (var i = 0; i < hash1.Length && i < hash2.Length && cr == CompareResult.CompareOk; i++)
                        {
                            if (hash1[i] != hash2[i])
                            {
                                cr = CompareResult.PixelMismatch;
                                break;
                            }
                        }
                    }
                }
            }

            return cr;
        }

        /// <summary>
        ///   Create a test shape
        /// </summary>
        /// <param name = "sf">The shapefile the new shape should be in</param>
        /// <param name = "offset">The location offset of the new shape</param>
        /// <param name = "length">The lenght of the line or the width of the polygon</param>
        /// <exception cref = "Exception"></exception>
        /// <returns>A shape object</returns>
        public MapWinGIS.Shape CreateTestShape(Shapefile sf, double offset, double length)
        {
            // Create a shape based on the shapefile type:
            var shpFileType = sf.ShapefileType;

            if (shpFileType == ShpfileType.SHP_NULLSHAPE)
            {
                return null;
            }

            // Create new shape:
            MapWinGIS.Shape testShape = null;

            try
            {
                // Initialize:   
                this.ReportMessage("Make test shape of type " + shpFileType);
                testShape = new MapWinGIS.Shape();
                testShape.Create(shpFileType);

                // Get the extents of the shapefile:
                double xMin, yMin, zMin, xMax, yMax, zMax;
                sf.Extents.GetBounds(out xMin, out yMin, out zMin, out xMax, out yMax, out zMax);

                // Create a collection to hold the coordinates
                IList<Coordinates> points = new List<Coordinates>();

                // Add some coordinates:
                if (shpFileType == ShpfileType.SHP_POINT)
                {
                    // Use the left bottom extents:
                    points.Add(new Coordinates(xMin + offset, yMin + offset));
                }

                if (shpFileType == ShpfileType.SHP_POLYLINE)
                {
                    // Change the coordinates
                    points.Add(new Coordinates(xMin + offset, yMin + offset));
                    points.Add(new Coordinates(xMin + offset + length, yMin + offset + (3 * length)));
                    points.Add(new Coordinates(xMin + offset + (2 * length), yMin + offset + length));
                    points.Add(new Coordinates(xMin + offset + (3 * length), yMin + offset + (2 * length)));
                }

                if (shpFileType == ShpfileType.SHP_POLYGON)
                {
                    points.Add(new Coordinates(xMin + offset + length, yMin + offset + length));
                    points.Add(new Coordinates(xMin + offset + length, yMin + offset));
                    points.Add(new Coordinates(xMin + offset, yMin + offset));
                    points.Add(new Coordinates(xMin + offset, yMin + offset + length));
                    points.Add(new Coordinates(xMin + offset + length, yMin + offset + length));
                }

                // Use the points to create the shape
                foreach (var coord in points)
                {
                    var pt = new MapWinGIS.Point { x = coord.X, y = coord.Y };
                    var numPoint = testShape.numPoints;
                    testShape.InsertPoint(pt, ref numPoint);
                }

                if (!testShape.IsValid)
                {
                    throw new Exception("Error! The created shape is invalid: " + testShape.IsValidReason);
                }
            }
            catch (Exception ex)
            {
                var msg = "Error CreateTestShape: ";
                if (testShape != null)
                {
                    msg += testShape.get_ErrorMsg(testShape.LastErrorCode);
                }

                throw new Exception(msg, ex);
            }

            this.ReportMessage("NumPoints of shape: " + testShape.numPoints);
            return testShape;
        }

        /// <summary>
        ///   Creates a snapshot and saves it to a file.
        /// </summary>
        /// <param name = "filename">The filename to save to</param>
        public void SaveSnapshot(string filename)
        {
            // Make sure the map has finished drawing:
            Application.DoEvents();

            var snapshot = this.mapWin.View.Snapshot(this.mapWin.View.Extents);
            if (!snapshot.Save(filename, false, ImageType.BITMAP_FILE, null))
            {
                throw new Exception(
                    "Error in saving snapshot [" + filename + "]: " + snapshot.get_ErrorMsg(snapshot.LastErrorCode));
            }

            snapshot.Close();
        }

        /// <summary>
        ///   This methods cleans the shapefile by splitting all multishapes into single shapes 
        ///   and fixing some invalid shapes by applying the buffer trick
        /// </summary>
        /// <param name = "sf">The shapefile to clean</param>
        /// <returns>The cleaned shapefile</returns>
        public Shapefile CleanShapefile(Shapefile sf)
        {
            // Clone the original shapefile to get the same attribute structure:
            var sfCleaned = sf.Clone();

            if (!sfCleaned.EditingShapes)
            {
                sfCleaned.StartEditingShapes(true, null);
            }

            var numShapes = sf.NumShapes;
            var numShapesCleaned = 0;
            for (var shapeIndex = 0; shapeIndex < numShapes; shapeIndex++)
            {
                var checker = false;

                // testsMethods.ReportMessage("Working on shape " + shapeIndex);
                var currentShape = this.GetShape(sf, shapeIndex);
                if (currentShape.IsValid)
                {
                    var numParts = currentShape.NumParts;
                    if (numParts == 1)
                    {
                        // testsMethods.ReportMessage("Shape is single-part");
                        sfCleaned.EditInsertShape(currentShape, ref numShapesCleaned);

                        // Add fields
                        this.CopyAttributes(sf, shapeIndex, sfCleaned, numShapesCleaned);
                        numShapesCleaned++;
                    }
                    else
                    {
                        // The first part is always clockwise and can be followed by a reversed part or another clockwise part.
                        // All parts between two clockwise parts should be added to a new shape, making sure it is not losing its hole
                        MapWinGIS.Shape shpPart;
                        for (var partIndex = 0; partIndex < numParts; partIndex++)
                        {
                            if (currentShape.get_PartIsClockWise(partIndex))
                            {
                                // Get the starting part
                                shpPart = currentShape.get_PartAsShape(partIndex);

                                // Insert the hole
                                if (partIndex < currentShape.NumParts - 1)
                                {
                                    partIndex++;
                                    if (currentShape.get_PartIsClockWise(partIndex))
                                    {
                                        partIndex--;
                                    }
                                }

                                while (!currentShape.get_PartIsClockWise(partIndex))
                                {
                                    var shpTemp = currentShape.get_PartAsShape(partIndex);

                                    if (!shpTemp.IsValid)
                                    {
                                        // Do the buffer trick:
                                        shpTemp.Buffer(0.0, 0);
                                    }

                                    if (shpTemp.IsValid)
                                    {
                                        var numPartsPartShape = shpPart.NumParts;
                                        shpPart.InsertPart(shpPart.numPoints, ref numPartsPartShape);
                                        for (var pointIndex = 0; pointIndex < shpTemp.numPoints; pointIndex++)
                                        {
                                            var pointPart = new MapWinGIS.Point();
                                            double x = 0, y = 0;
                                            shpTemp.get_XY(pointIndex, ref x, ref y);
                                            pointPart.x = x;
                                            pointPart.y = y;

                                            var numPoints = shpPart.numPoints + pointIndex;
                                            if (!shpPart.InsertPoint(pointPart, ref numPoints))
                                            {
                                                this.ReportMessage(
                                                    "ERROR in shpPart.InsertPoint: "
                                                    + shpPart.get_ErrorMsg(shpPart.LastErrorCode));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        this.ReportMessage(
                                            "Shape " + shapeIndex + " Part " + partIndex + 1 + " is not valid");
                                    }

                                    partIndex++;

                                    if (partIndex == currentShape.NumParts)
                                    {
                                        partIndex++;
                                        break;
                                    }

                                    if (currentShape.get_PartIsClockWise(partIndex))
                                    {
                                        checker = true;
                                        break;
                                    }
                                } // while

                                if (shpPart != null)
                                {
                                    if (!shpPart.IsValid)
                                    {
                                        // Do the buffer trick:
                                        shpPart.Buffer(0.0, 0);
                                    }
                                }

                                if (shpPart != null)
                                {
                                    if (shpPart.IsValid)
                                    {
                                        sfCleaned.EditInsertShape(shpPart, ref numShapesCleaned);

                                        // Add fields
                                        this.CopyAttributes(sf, shapeIndex, sfCleaned, numShapesCleaned);
                                        numShapesCleaned++;
                                    }
                                }

                                if (shapeIndex + 1 <= currentShape.NumParts)
                                {
                                    if (!currentShape.get_PartIsClockWise(partIndex) || checker)
                                    {
                                        partIndex--;

                                        // Must move back one as we have moved forward during the loop                                    
                                        checker = false;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    this.ReportMessage("Shape with ID " + shapeIndex + " is invalid:" + currentShape.IsValidReason);
                }
            }

            sfCleaned.StopEditingShapes(true, true, null);
            return sfCleaned;
        }

        /// <summary>
        ///   Write the properties to the log file
        /// </summary>
        /// <param name = "obj">The object to get the properties from</param>
        public void LogProperties(object obj)
        {
            var type = obj.GetType();
            var propertyInfo = type.GetProperties();

            this.ReportMessage("Getting properties of " + type.Name);

            foreach (var info in propertyInfo)
            {
                if (info.PropertyType.IsValueType && info.CanRead && info.GetIndexParameters().Length == 0)
                {
                    this.ReportMessage(string.Format("Value of '{0}' is {1}", info.Name, info.GetValue(obj, null)));
                }
            }
        }

        /// <summary>
        ///   Show some statistics of the image
        /// </summary>
        /// <param name = "img">The image to report</param>
        /// <returns>The image type</returns>
        public string ShowImageReport(ref MapWinGIS.Image img)
        {
            this.ReportMessage(Path.GetFileName(img.Filename) + " is loaded");
            this.ReportMessage("Image is of type: " + img.ImageType);
            this.ReportMessage("Number of bands: " + img.NoBands);
            this.ReportMessage("Number of overviews: " + img.NumOverviews);
            this.ReportMessage(
                string.Format("XllCenter: {0}, YllCenter: {1}", img.GetOriginalXllCenter(), img.GetOriginalYllCenter()));
            this.ReportMessage("Projection: " + img.GetProjection());
            this.ReportMessage(
                string.Format(
                    "GetOriginalHeight: {0}, GetOriginalWidth: {1}", img.GetOriginalHeight(), img.GetOriginalWidth()));
            this.ReportMessage("HasColorTable: " + img.HasColorTable);
            return img.ImageType.ToString();
        }

        /// <summary>
        ///   Adds a file to the map, but checks if it exists first
        /// </summary>
        /// <param name = "filename">The file to open</param>
        /// <param name = "layername">The name of the layer</param>
        /// <exception cref = "FileNotFoundException">If the file cannot be found</exception>
        /// <returns>The layer object</returns>
        public Layer AddLayer(string filename, string layername)
        {
            // First check if the file exists, else we get a nasty messagebox:
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("Error in AddLayer. Cannot find the file", filename);
            }

            return this.mapWin.Layers.Add(filename, layername);
        }

        /// <summary>
        ///   Adds a file to the map, but checks if it exists first
        /// </summary>
        /// <param name = "filename">The file to open</param>
        /// <returns>The layer object</returns>
        /// <remarks>
        ///   Overloaded method
        /// </remarks>
        public Layer AddLayer(string filename)
        {
            return this.AddLayer(filename, Path.GetFileNameWithoutExtension(filename));
        }

        /// <summary>
        ///   Create a new shapefile
        /// </summary>
        /// <param name = "fileName">The name of the shapefile</param>
        /// <param name = "shpfileType">The shapefile type</param>
        /// <returns>The new shapefile</returns>
        public Shapefile CreateShapefile(string fileName, ShpfileType shpfileType)
        {
            var sf = new Shapefile();
            DataManagement.DeleteShapefile(ref fileName);
            if (!sf.CreateNewWithShapeID(fileName, shpfileType))
            {
                throw new Exception("Could not create new shapefile: " + sf.get_ErrorMsg(sf.LastErrorCode));
            }

            if (sf == null)
            {
                throw new Exception("Shapefile is null");
            }

            return sf;
        }

        /// <summary>
        /// Creates a single part shape
        /// </summary>
        /// <param name="shpfileType">The type of the shape</param>
        /// <param name="points">The points of the shape</param>
        /// <returns>The shape object</returns>
        public MapWinGIS.Shape CreateShape(ShpfileType shpfileType, IList<MapWinGIS.Point> points)
        {
            // Create shape:
            var shape = new MapWinGIS.Shape();
            if (!shape.Create(shpfileType))
            {
                throw new Exception("Could not create shape: " + shape.get_ErrorMsg(shape.LastErrorCode));
            }

            // Insert the point into the shape
            var numPoint = 0;
            foreach (var point in points)
            {
                if (!shape.InsertPoint(point, ref numPoint))
                {
                    throw new Exception(
                        string.Format(
                            "Error! In shape.InsertPoint[{0}]: {1}", numPoint, shape.get_ErrorMsg(shape.LastErrorCode)));
                }

                numPoint++;
            }

            if (shpfileType == ShpfileType.SHP_POLYGON
                && (!this.ArePointsEqual(shape.get_Point(0), shape.get_Point(shape.numPoints - 1))))
            {
                // Close the polygon
                shape.InsertPoint(points[0], ref numPoint);
            }

            if (!shape.IsValid)
            {
                throw new Exception("Error! Created shape is not valid:" + shape.IsValidReason);
            }

            return shape;
        }

        /// <summary>
        /// Converts the selected shapes in the layer to seleced shapes in the shapefile object
        /// </summary>
        /// <param name="sf">The shapefile to select the shapes off</param>
        /// <param name="hndl">The handle of the layer</param>
        public void ConvertLayerSelectionToSfSelection(ref Shapefile sf, int hndl)
        {
            var selectInfo = this.mapWin.View.SelectedShapes;
            for (var i = 0; i < selectInfo.NumSelected; i++)
            {
                if (selectInfo.LayerHandle == hndl)
                {
                    sf.set_ShapeSelected(selectInfo[i].ShapeIndex, true);
                }
            }
        }

        /// <summary>
        ///   Add random points to the shapefile
        /// </summary>
        /// <param name = "sf">The shapefile to add the points to</param>
        /// <param name = "numPoints">The number of points to add</param>
        public void AddRandomPoints(ref Shapefile sf, int numPoints)
        {
            MapWinGIS.Shape sh;
            var pointIndex = 0;
            var rnd = new Random();

            if (!sf.StartEditingShapes(true, null))
            {
                throw new Exception("Could not StartEditingShapes: " + sf.get_ErrorMsg(sf.LastErrorCode));
            }

            this.ReportMessage("Start adding " + numPoints + " points");
            for (var i = 0; i < numPoints; i++)
            {
                var pt = new MapWinGIS.Point { x = Math.Cos(rnd.Next() * numPoints) };
                pt.y = Math.Sin(rnd.Next() * pt.x);

                sh = new MapWinGIS.Shape { ShapeType = ShpfileType.SHP_POINT };
                if (!sh.InsertPoint(pt, ref pointIndex))
                {
                    throw new Exception("Could not InsertPoint: " + sh.get_ErrorMsg(sh.LastErrorCode));
                }

                var shapeIndex = sf.NumShapes;
                if (!sf.EditInsertShape(sh, ref shapeIndex))
                {
                    throw new Exception("Could not EditInsertShape: " + sf.get_ErrorMsg(sf.LastErrorCode));
                }

                this.ReportMessage("Added shape nr: " + shapeIndex);
            }

            if (!sf.StopEditingShapes(true, true, null))
            {
                throw new Exception("Could not StopEditingShapes: " + sf.get_ErrorMsg(sf.LastErrorCode));
            }
        }

        /// <summary>
        ///   Checks if the two point are on the same location
        /// </summary>
        /// <param name = "pt1">The first MapWinGIS point</param>
        /// <param name = "pt2">The second MapWinGIS point</param>
        /// <returns>True if equal</returns>
        public bool ArePointsEqual(MapWinGIS.Point pt1, MapWinGIS.Point pt2)
        {
            return pt1.x == pt2.x && pt1.y == pt2.y;
        }

        /// <summary>
        ///   Checks if the two point are on the same location
        /// </summary>
        /// <param name = "pt1">The first Topology point</param>
        /// <param name = "pt2">The second Topology point</param>
        /// <returns>True if equal</returns>
        public bool ArePointsEqual(Topology.Point pt1, Topology.Point pt2)
        {
            return pt1.X == pt2.X && pt1.Y == pt2.Y;
        }

        /// <summary>
        /// Checks if the two envelopes are the same
        /// </summary>
        /// <param name="env1">The first envelope</param>
        /// <param name="env2">The second envelope</param>
        /// <returns>True if they are equal</returns>
        public bool AreEnvelopesEqual(Topology.Envelope env1, Topology.Envelope env2)
        {
            if (env1.Width != env2.Width || env1.Height != env2.Height)
            {
                return false;
            }

            if (env1.xMin != env2.xMin || env1.yMin != env2.yMin)
            {
                return false;
            }

            if (env1.xMax != env2.xMax || env1.yMax != env2.yMax)
            {
                return false;
            }

            // The envelopes are equal:
            return true;
        }

        /// <summary>
        /// Returns the coordinates of a MapWinGIS point as a string
        /// </summary>
        /// <param name="pt">The MapWinGIS point</param>
        /// <returns>The coordinates as a formatted string</returns>
        public string PointToString(MapWinGIS.Point pt)
        {
            return string.Format("X: {0} Y: {1} Z: {2}", pt.x, pt.y, pt.Z);
        }

        /// <summary>
        /// Returns the coordinates of a Topology point as a string
        /// </summary>
        /// <param name="pt">The Topology point</param>
        /// <returns>The coordinates as a formatted string</returns>
        public string PointToString(Topology.Point pt)
        {
            return string.Format("X: {0} Y: {1} Z: {2}", pt.X, pt.Y, pt.Z);
        }

        /// <summary>
        ///   Tests the shapefile
        /// </summary>
        /// <param name = "filename">The name of the shapefile</param>
        /// <returns>True on success</returns>
        public bool TestShapefile(string filename)
        {
            try
            {
                this.ReportMessage("Test shapefile");
                var sf = this.OpenShapeFile(filename);

                var numInvalidShapes = this.ValidateShapefile(sf, 0);
                this.ReportMessage(
                    string.Format(
                        "{0} is of type {1} and has {2} shapes. {3} are invalid.",
                        sf.Filename,
                        sf.ShapefileType,
                        sf.NumShapes,
                        numInvalidShapes));

                this.ReportMessage("Before BeginPointInShapefile");
                if (!sf.BeginPointInShapefile())
                {
                    this.ReportMessage("Error BeginPointInShapefile: " + sf.get_ErrorMsg(sf.LastErrorCode));
                    return false;
                }

                if (!sf.Close())
                {
                    this.ReportMessage("Error closing shapefile: " + sf.get_ErrorMsg(sf.LastErrorCode));
                    return false;
                }
            }
            catch (Exception ex)
            {
                this.ReportMessage("Exception in TestShapefile: " + ex.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        ///   Checks if all shapes are valid in the shapefile
        /// </summary>
        /// <param name = "sf">The shapefile</param>
        /// <param name = "fieldID">The field ID to report</param>
        /// <returns>The number of invalid shapes</returns>
        public int ValidateShapefile(IShapefile sf, int fieldID)
        {
            IShape shp;
            var retVal = 0;

            // Get the field for logging
            IField fld = sf.get_Field(fieldID);
            for (var i = 0; i < sf.NumShapes; i++)
            {
                shp = sf.get_Shape(i);
                var valid = shp.IsValid;
                if (valid)
                {
                    continue;
                }

                this.ReportMessage(
                    string.Format(
                        "Shape with {0}={1} is invalid: {2}", fld.Name, sf.get_CellValue(fieldID, i), shp.IsValidReason));
                retVal++;
            }

            return retVal;
        }

        #endregion

        #region static private menthods

        /// <summary>
        ///   Formats the elapsed time
        /// </summary>
        /// <param name = "stopWatch">The stopWatch object</param>
        /// <returns>Formatted timespan</returns>
        private static string GetElapsedTime(Stopwatch stopWatch)
        {
            var ts = stopWatch.Elapsed;
            return String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
        }

        #endregion

        #region Private methods

        /// <summary>
        ///   Initialization method
        /// </summary>
        private void Init()
        {
            // Set the initial value:
            Environment.SetEnvironmentVariable(
                "MWTESTRESULT", this.scriptName + "=" + TestResults.Crashed, EnvironmentVariableTarget.User);

            // Set variables:
            this.calledAsBatch = this.GetEnvironmentVariable("MWBATCHTEST") == "0";
            this.WorkingFolder = this.GetEnvironmentVariable("MWTESTFOLDER");
            if (string.IsNullOrEmpty(this.WorkingFolder))
            {
                this.WorkingFolder = @"C:\dev\TestingScripts\ScriptData";
            }

            // Check if this folder exists:
            this.CheckFolder(this.WorkingFolder);

            // Move one up and then down to temp folder:
            this.TempFolder = Path.Combine(
                this.WorkingFolder.Substring(0, this.WorkingFolder.LastIndexOf("\\")), "temp\\");

            // Save all temp output in own subfolder:
            this.TempFolder = Path.Combine(this.TempFolder, this.scriptName) + "\\";

            // Check if this folder exists:
            this.CheckFolder(this.TempFolder);

            // Set up logfiles locations:
            const string Tmp = "TestScriptsOutput";
            this.logFilename = Path.Combine(
                this.WorkingFolder.Substring(0, this.WorkingFolder.LastIndexOf("\\")),
                Tmp + "-" + DateTime.Now.Date.ToShortDateString().Replace("/", "-") + ".txt");
            this.summeryFilename = this.logFilename.Replace(Tmp, "TestScriptsSummery");

            this.WorkingFolder = Path.Combine(this.WorkingFolder, this.scriptName) + "\\";

            // Initialize MapWindow:            
            this.mapWin.View.CursorMode = tkCursorMode.cmSelection;
            if (this.startWithEmptyMap)
            {
                // TODO: This should be Project.New()
                // This is requesting in http://bugs.mapwindow.org/view.php?id=1768
                this.mapWin.Layers.Clear();
                this.mapWin.View.Draw.ClearDrawings();
                this.mapWin.Project.MapUnits = string.Empty;
                this.mapWin.Project.ProjectProjection = string.Empty;
                this.mapWin.Project.GeoProjection = new GeoProjection();
                this.RefreshMap();
            }

            // Start with an empty line:
            this.ReportMessage(string.Empty);
            this.ReportMessage("Start of test");

            // Init stopwatch:
            this.stopWatchGlobale = Stopwatch.StartNew();

            // TODO: Add some time-out capabilities:
            /*
            // Default time out value:
            this.TimeOutValue = 5;

            // Create a timer:
            this.timer = new System.Timers.Timer(this.TimeOutValue * 60 * 1000);
            
            // Hook up the Elapsed event for the timer.
            this.timer.Elapsed += this.OnTimedEvent;
            this.timer.Enabled = true;
            this.timer.Start();

            // If the timer is declared in a long-running method, use
            // KeepAlive to prevent garbage collection from occurring
            // before the method ends.
            GC.KeepAlive(this.timer);
            */
        }

        /*
        /// <summary>
        /// The OnTimed Event
        /// </summary>
        /// <param name="source">The source object</param>
        /// <param name="e">The ElapsedEvent arguments</param>
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            this.ReportMessage(string.Format("This scripted time-out after {0} minutes. Either change the script or the timeout value", this.timeOutValue));
            throw new TimeoutException();
        }
        */

        /// <summary>
        ///   Reads an environment variable from the registry
        /// </summary>
        /// <param name = "varName">The name of the variable to read</param>
        /// <returns>The environment variable</returns>
        private string GetEnvironmentVariable(string varName)
        {
            try
            {
                // Check if the environment variable is set:
                return Environment.GetEnvironmentVariable(varName);
            }
            catch (Exception ex)
            {
                this.ReportMessage("Error in GetEnvironmentVariable: " + ex.Message);
            }

            return string.Empty;
        }

        /// <summary>
        ///   Write the message to the file
        /// </summary>
        /// <param name = "filename">The name of the file</param>
        /// <param name = "msg">The message</param>
        private void WriteToFile(string filename, string msg)
        {
            TextWriter of1 = new StreamWriter(filename, true);
            if (msg != string.Empty)
            {
                of1.WriteLine("(" + this.scriptName + ") " + DateTime.Now + " " + msg);
            }
            else
            {
                of1.WriteLine();
            }

            of1.Close();
        }

        /// <summary>
        ///   Gets the total durations the script needed
        /// </summary>
        /// <returns>Formatted timespan</returns>
        private string GetDurationOfScript()
        {
            Application.DoEvents();
            this.stopWatchGlobale.Stop();
            return GetElapsedTime(this.stopWatchGlobale);
        }

        /// <summary>
        ///   Checks if a folder exists, if not creates it
        /// </summary>
        /// <param name = "folder">The folder to check</param>
        private void CheckFolder(string folder)
        {
            try
            {
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
            }
            catch (Exception ex)
            {
                this.ReportMessage(string.Format("Error while creating folder [{0}]: {1}", folder, ex.Message));
            }
        }

        #endregion

        #region Nested type: Coordinates

        /// <summary>
        ///   Structure to hold coordinates
        /// </summary>
        private struct Coordinates
        {
            /// <summary>
            ///   X and Y values
            /// </summary>
            public readonly double X;

            /// <summary>
            ///   X and Y values
            /// </summary>
            public readonly double Y;

            /// <summary>
            ///   Initializes a new instance of the Coordinates struct.
            /// </summary>
            /// <param name = "p1">First point</param>
            /// <param name = "p2">Second point</param>
            public Coordinates(double p1, double p2)
            {
                this.X = p1;
                this.Y = p2;
            }
        }

        #endregion
    }
}