//********************************************************************************************************
//File name: Utils.cs
//Description: Public class, future home of many MapWinGIS.Utils functions;
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
//1-12-06 ah - Angela Hillier - Provided initial API and parameter descriptions
//16.Mar 2008 - jk - Jiri Kadlec - Added new version of Area() function for shapes in decimal degrees						
//********************************************************************************************************

using System;
using System.Diagnostics;
using System.Collections;

namespace MapWinGeoProc
{
    using System.Linq;

    using MapWinGIS;

    using LineSegment = MapWinGeoProc.Topology2D.LineSegment;
    using Point = MapWinGeoProc.Topology2D.Point;

    /// <summary>
    /// Utils provides a collection of methods ranging from file conversion to finding a point on a shape.
    /// </summary>
    ///
    public class Utils
    {
        /// <summary>
        /// A short structure with an integer of a part index and the first and last point in a shape.
        /// The values are inclusive, so both begPoint and endPoint are in the part.
        /// </summary>
        public struct PartBounds
        {
            /// <summary>
            /// The integer index value indicating the part in a multi-part polygon.
            /// </summary>
            public int PartIndex;
            /// <summary>
            /// The first point in the polygon that is inside the part
            /// </summary>
            public int begPoint;
            /// <summary>
            /// The last point in the polygon that is inside the part
            /// </summary>
            public int endPoint;
        }

        private static string gErrorMsg = "";

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>
        public static double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        /// <summary>
        /// Finds if a point lies on a line or not.
        /// </summary>
        /// <param name="point">The test point.</param>
        /// <param name="line">The line shape.</param>
        /// <param name="ptLocation">The index to where the point would be located
        /// on the line if it were to be added to the line.</param>
        /// <returns>False if the point is not on the line or an error was encountered, true otherwise.</returns>
        public static bool PointOnLine(ref MapWinGIS.Point point, ref MapWinGIS.Shape line, out int ptLocation)
        {
            double lineSlope = 0;
            double testSlope = 0;
            int numSegs = line.numPoints - 1;
            for (int i = 0; i <= numSegs - 1; i++)
            {
                //create a two-point line segment out of p0 and p1
                MapWinGIS.Point p0 = new MapWinGIS.PointClass();
                p0 = line.get_Point(i);
                MapWinGIS.Point p1 = new MapWinGIS.PointClass();
                p1 = line.get_Point(i + 1);

                //check if the point is within the bounds set by p0 and p1
                double xMax, yMax, xMin, yMin;
                if (p0.x > p1.x)
                {
                    xMax = p0.x;
                    xMin = p1.x;
                }
                else
                {
                    xMax = p1.x;
                    xMin = p0.x;
                }
                if (p0.y > p1.y)
                {
                    yMax = p0.y;
                    yMin = p1.y;
                }
                else
                {
                    yMax = p1.y;
                    yMin = p0.y;
                }
                if (Globals.CheckBounds(xMin, xMax, yMin, yMax, point.x, point.x, point.y, point.y))
                {
                    //calculate the slope for the 2pt line segment
                    double xDiff = p1.x - p0.x;
                    double yDiff = p1.y - p0.y;
                    lineSlope = yDiff / xDiff;

                    //consider the slope formed by p0 and the test point
                    xDiff = point.x - p0.x;
                    yDiff = point.y - p0.y;
                    testSlope = yDiff / xDiff;

                    if (lineSlope == testSlope)
                    {
                        ptLocation = i + 1;
                        return true;
                    }
                }
            }
            ptLocation = -1;
            return false;
        }

        /// <summary>
        /// Not Implemented
        /// Adds a given point to the line or polygon at the specified location.
        /// </summary>
        /// <param name="point">The point to be added.</param>
        /// <param name="shape">The line or polygon shape that will gain a new point.</param>
        /// <param name="ptLocation">The location in the polygon or line where the point should be added.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool AddPointToShape(ref MapWinGIS.Point point, ref MapWinGIS.Shape shape, int ptLocation)
        {
            //TODO: Implement this function
            Error.ClearErrorLog();
            gErrorMsg = "This function is not yet implemented.";
            Error.SetErrorMsg(gErrorMsg);
            Debug.WriteLine(gErrorMsg);

            return false;
        }

        /// <summary>
        /// Not Implemented
        /// Removes a point at pointLocation from the line or polygon shape.
        /// </summary>
        /// <param name="ptLocation">The location of the point on the line or polygon.</param>
        /// <param name="shape">The line or polygon shape that will lose a point.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool RemovePointFromShape(int ptLocation, ref MapWinGIS.Shape shape)
        {
            //TODO: Implement this function
            Error.ClearErrorLog();
            gErrorMsg = "This function is not yet implemented.";
            Error.SetErrorMsg(gErrorMsg);
            Debug.WriteLine(gErrorMsg);

            return false;
        }

        /// <summary>
        /// Finds where the test point is closest to on a line or polygon shape and 
        /// returns the corresonding point and location and distance.
        /// </summary>
        /// <param name="testPoint">The point to be checked for.</param>
        /// <param name="shape">The shape to find the nearest point on.</param>
        /// <param name="resultPoint">The resulting point that lies on the shape.</param>
        /// <param name="pointLocation">The location of where the result point should be on the shape.</param>
        /// <param name="pointDistance">The distance from the test point to the closest point.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool FindNearestPointAndLoc(ref MapWinGIS.Point testPoint, ref Shape shape, ref MapWinGIS.Point resultPoint, out int pointLocation, out double pointDistance)
        {
            var geomPoint = new Point(testPoint.x, testPoint.y);
            var geomLine = new Topology2D.LineString(shape);
            var geomSegs = geomLine.ToLineSegments();

            double shortDist = 100000;
            double shortPointX = -1;
            double shortPointY = -1;

            foreach (var currClosest in geomSegs.Select(lineSegment => geomPoint.ClosestPointOn(lineSegment)))
            {
                if (currClosest == null)
                {
                    shortDist = 0.0;
                    shortPointX = geomPoint.X;
                    shortPointY = geomPoint.Y;
                    break;
                }

                var currDist =
                    Math.Sqrt(
                        ((currClosest.X - geomPoint.X) * (currClosest.X - geomPoint.X))
                        + ((currClosest.Y - geomPoint.Y) * (currClosest.Y - geomPoint.Y)));
                if (currDist >= shortDist)
                {
                    continue;
                }

                shortDist = currDist;
                shortPointX = currClosest.X;
                shortPointY = currClosest.Y;
            }

            pointDistance = shortDist;
            pointLocation = 0;
            resultPoint.x = shortPointX;
            resultPoint.y = shortPointY;

            return true;
        }

        /// <summary>
        /// Finds where the test point is closest to on a line or polygon shape and 
        /// returns the corresonding point and location and distance.
        /// </summary>
        /// <param name="testPoint">The point to be checked for.</param>
        /// <param name="shape">The shape to find the nearest point on.</param>
        /// <param name="resultPoint">The resulting point that lies on the shape.</param>
        /// <param name="pointLocation">The location of where the result point should be on the shape.</param>
        /// <param name="pointDistance">The distance from the test point to the closest point.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool FindNearestPointAndLocOld(
            ref MapWinGIS.Point testPoint,
            ref MapWinGIS.Shape shape,
            ref MapWinGIS.Point resultPoint,
            out int pointLocation,
            out double pointDistance)
        {
            const double StepSize = 0.1;
            double shortDist = 1000000;

            double shortPointX = -1;
            double shortPointY = -1;
            var tmpLoc = 0;
            var testPointX = testPoint.x;
            var testPointY = testPoint.y;

            if ((shape.ShapeType != ShpfileType.SHP_POLYGON)
                && (shape.ShapeType != ShpfileType.SHP_POLYGONM)
                && (shape.ShapeType != ShpfileType.SHP_POLYGONZ)
                && (shape.ShapeType != ShpfileType.SHP_POLYLINE)
                && (shape.ShapeType != ShpfileType.SHP_POLYLINEM)
                && (shape.ShapeType != ShpfileType.SHP_POLYLINEZ))
            {
                Error.ClearErrorLog();
                gErrorMsg = "This function is currently only implemented for Polygon and Polyline shapefiles.";
                Error.SetErrorMsg(gErrorMsg);
                Debug.WriteLine(gErrorMsg);
                pointLocation = 0;
                pointDistance = -1;
                return false;
            }

            for (var i = 1; i < shape.numPoints; i++)
            {
                var tmpPt1 = shape.get_Point(i - 1);
                var tmpPt2 = shape.get_Point(i);
                var dX = tmpPt2.x - tmpPt1.x;
                var dY = tmpPt2.y - tmpPt1.y;
                var segLength = Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2));
                var runStep = StepSize * (tmpPt2.x - tmpPt1.x) / segLength;
                var riseStep = StepSize * (tmpPt2.y - tmpPt1.y) / segLength;
                var loopCount = int.Parse(Math.Round(segLength / StepSize).ToString());

                var currPointX = tmpPt1.x;
                var currPointY = tmpPt1.y;

                double currDist;
                for (var j = 0; j < loopCount; j++)
                {
                    // For each step between the vertices, find distance to the test point
                    currDist = Math.Sqrt(Math.Pow(testPointX - currPointX, 2) + Math.Pow(testPointY - currPointY, 2));
                    if (currDist <= shortDist)
                    {
                        shortDist = currDist;
                        shortPointX = currPointX;
                        shortPointY = currPointY;
                        tmpLoc = i;
                    }

                    currPointX = currPointX + runStep;
                    currPointY = currPointY + riseStep;
                }

                // Most lines won't be exactly divisble to the stepsize so handle the remainder of segment
                if ((segLength % StepSize) == 0)
                {
                    continue;
                }

                currDist = Math.Sqrt(Math.Pow(testPointX - tmpPt2.x, 2) + Math.Pow(testPointY - tmpPt2.y, 2));
                if (currDist > shortDist)
                {
                    continue;
                }

                shortDist = currDist;
                shortPointX = tmpPt2.x;
                shortPointY = tmpPt2.y;
                tmpLoc = i;
            }

            pointDistance = shortDist;
            pointLocation = tmpLoc;
            resultPoint.x = shortPointX;
            resultPoint.y = shortPointY;
            return true;
        }

        /// <summary>
        /// A function which snaps all points in a given shapefile path within a given threshold to lines in a polyline shapefile, tossing out any points which don't fall within the threshold
        /// </summary>
        /// <param name="pointsPath">Point shapefile path</param>
        /// <param name="linesPath">Line shapefile path</param>
        /// <param name="snapThreshold">Threshold to snap to</param>
        /// <param name="exclusionThreshold">Threshold of minimum distance between two points in the snap results grid. Suggested value of half cell size if used in delineation to avoid two outlets in the same grid cell.</param>
        /// <param name="snapResultPath">Resulting point shapefile of points that were within thresh and were snapped</param>
        /// <param name="copyAttributes">True if copying attributes over</param>
        /// <param name="callback">ICallback object for status and progress</param>
        /// <returns>True on success</returns>
        public static bool SnapPointsToLines(string pointsPath, string linesPath, double snapThreshold, double exclusionThreshold, string snapResultPath, bool copyAttributes, MapWinGIS.ICallback callback)
        {
            return DoSnapPointsToLines(pointsPath, linesPath, snapThreshold, exclusionThreshold, snapResultPath, copyAttributes, callback);
        }

        /// <summary>
        /// A function which snaps all points in a given shapefile path within a given threshold to lines in a polyline shapefile, tossing out any points which don't fall within the threshold
        /// </summary>
        /// <param name="PointsPath">Point shapefile path</param>
        /// <param name="LinesPath">Line shapefile path</param>
        /// <param name="SnapThreshold">Threshold to snap to</param>
        /// <param name="SnapResultPath">Resulting point shapefile of points that were within thresh and were snapped</param>
        /// <param name="CopyAttributes">True if copying attributes over</param>
        /// <param name="Callback">ICallback object for status and progress</param>
        /// <returns></returns>
        public static bool SnapPointsToLines(string PointsPath, string LinesPath, double SnapThreshold, string SnapResultPath, bool CopyAttributes, MapWinGIS.ICallback Callback)
        {
            return DoSnapPointsToLines(PointsPath, LinesPath, SnapThreshold, 0.0, SnapResultPath, CopyAttributes, Callback);
        }

        /// <summary>
        /// Snap points to lines method
        /// </summary>
        /// <param name="pointsPath">The points</param>
        /// <param name="linesPath">The lines</param>
        /// <param name="snapThreshold">The snap threshold</param>
        /// <param name="exclusionThreshold">The exclusion threshold </param>
        /// <param name="snapResultPath">The result path</param>
        /// <param name="copyAttributes">Copy the attributes</param>
        /// <param name="callback">The callback object</param>
        /// <returns>True on success</returns>
        private static bool DoSnapPointsToLines(
            string pointsPath,
            string linesPath,
            double snapThreshold,
            double exclusionThreshold,
            string snapResultPath,
            bool copyAttributes,
            ICallback callback)
        {
            var pt = new Shapefile();
            var ln = new Shapefile();
            var newPoint = new Shapefile();
            var resPt = new MapWinGIS.Point();
            var lowestPt = new MapWinGIS.Point();
            Shape tmpPointShape;
            double lowestDist = 100000;
            var oldperc = 0;

            if (callback != null)
            {
                callback.Progress("Status", 0, "Snapping Outlets");
            }

            pt.Open(pointsPath, callback);
            ln.Open(linesPath, callback);
            DataManagement.DeleteShapefile(ref snapResultPath);

            if (!newPoint.CreateNew(snapResultPath, ShpfileType.SHP_POINT))
            {
                return false;
            }

            // Newly created shapefiles are always in edit mode:
            /*
            if (!newPoint.StartEditingShapes(true, callback))
            {
                return false;
            }
            */

            if (copyAttributes)
            {
                Field tmpField, pointField;
                for (var f = 0; f < pt.NumFields; f++)
                {
                    tmpField = new Field();
                    pointField = pt.get_Field(f);
                    tmpField.Name = pointField.Name;
                    tmpField.Type = pointField.Type;
                    tmpField.Precision = pointField.Precision;
                    tmpField.Width = pointField.Width;
                    tmpField.Key = pointField.Key;
                    if (!newPoint.EditInsertField(tmpField, ref f, null))
                    {
                        return false;
                    }
                }
            }

            for (var i = 0; i < pt.NumShapes; i++)
            {
                if (pt.NumShapes > 0)
                {
                    if (callback != null)
                    {
                        var newperc = Convert.ToInt32((Convert.ToDouble(i) / Convert.ToDouble(pt.NumShapes)) * 100);
                        if (newperc > oldperc)
                        {
                            callback.Progress("Status", newperc, "Snapping Outlets");
                            oldperc = newperc;
                        }
                    }
                }

                var currPt = pt.get_Shape(i).get_Point(0);

                for (var j = 0; j < ln.NumShapes; j++)
                {
                    var currLn = ln.get_Shape(j);
                    int resLoc;
                    double resDist;
                    if (!FindNearestPointAndLocOld(ref currPt, ref currLn, ref resPt, out resLoc, out resDist))
                    {
                        continue;
                    }

                    if (resDist > lowestDist)
                    {
                        continue;
                    }

                    lowestDist = resDist;
                    lowestPt.x = resPt.x;
                    lowestPt.y = resPt.y;
                }

                var newPointCount = newPoint.NumShapes;
                var tmpPointCount = 0;

                if (newPointCount == 0)
                {
                    if (lowestDist <= snapThreshold)
                    {
                        tmpPointShape = new Shape();
                        tmpPointShape.Create(ShpfileType.SHP_POINT);
                        tmpPointShape.InsertPoint(lowestPt, ref tmpPointCount);
                        if (!newPoint.EditInsertShape(tmpPointShape, ref newPointCount))
                        {
                            System.Windows.Forms.MessageBox.Show(newPoint.get_ErrorMsg(newPoint.LastErrorCode));
                            return false;
                        }

                        if (copyAttributes)
                        {
                            for (var f = 0; f <= pt.NumFields - 1; f++)
                            {
                                newPoint.EditCellValue(f, newPoint.NumShapes - 1, pt.get_CellValue(f, i));
                            }
                        }
                    }
                }
                else
                {
                    var pointAlreadyAdded = false;
                    for (var j = 0; j < newPoint.NumShapes; j++)
                    {
                        var tmpPt = newPoint.get_Shape(j).get_Point(0);
                        if (IsPointInThreshold(tmpPt.x, tmpPt.y, lowestPt.x, lowestPt.y, exclusionThreshold))
                        {
                            pointAlreadyAdded = true;
                            break;
                        }
                    }

                    if (!pointAlreadyAdded)
                    {
                        if (lowestDist <= snapThreshold)
                        {
                            tmpPointShape = new Shape();
                            tmpPointShape.Create(ShpfileType.SHP_POINT);
                            tmpPointShape.InsertPoint(lowestPt, ref tmpPointCount);
                            if (!newPoint.EditInsertShape(tmpPointShape, ref newPointCount))
                            {
                                System.Windows.Forms.MessageBox.Show(newPoint.get_ErrorMsg(newPoint.LastErrorCode));
                                pt.Close();
                                ln.Close();
                                return false;
                            }

                            if (copyAttributes)
                            {
                                for (var f = 0; f < pt.NumFields; f++)
                                {
                                    newPoint.EditCellValue(f, newPoint.NumShapes - 1, pt.get_CellValue(f, i));
                                }
                            }
                        }
                    }
                }

                lowestPt = new MapWinGIS.Point();
                lowestDist = 100000;
            }

            newPoint.Projection = pt.Projection;
            pt.Close();
            ln.Close();
            newPoint.StopEditingShapes(true, true, callback);
            var numNewshapes = newPoint.NumShapes;
            newPoint.Close();
            if (numNewshapes == 0)
            {
                System.Windows.Forms.MessageBox.Show(
                    @"After snapping, no outlets were within the threshold to be snapped. Please set a higher snap threshold, draw new outlets, or uncheck the Use Custom Outlets option, rerun Network Delineation, check Use Custom Outlets, and rerun Outlets Delineation.");

                return false;
            }

            if (callback != null)
            {
                callback.Progress("Status", 0, string.Empty);
            }

            return true;
        }

        private static bool IsPointInThreshold(double startPointX, double startPointY, double checkPointX, double checkPointY, double thresh)
        {
            var minX = startPointX - thresh;
            var maxX = startPointX + thresh;
            var minY = startPointY - thresh;
            var maxY = startPointY + thresh;

            return checkPointX > minX & checkPointX < maxX & checkPointY > minY & checkPointY < maxY;
        }

        /// <summary>
        /// Function to extract the points from a given shapefile as found in an arraylist of point indexes
        /// </summary>
        /// <param name="pointShapePath">Th einput shapefile</param>
        /// <param name="extactedPointShapeResultPath">The result path</param>
        /// <param name="selectedIndexesList">The list of selected points</param>
        /// <param name="callback">The callback object</param>
        /// <returns>True on success</returns>
        public static bool ExtractSelectedPoints(string pointShapePath, string extactedPointShapeResultPath, ArrayList selectedIndexesList, MapWinGIS.ICallback callback)
        {
            var result = false;
            var points = new MapWinGIS.Shapefile();
            var pointsNew = new MapWinGIS.Shapefile();

            DataManagement.DeleteShapefile(ref extactedPointShapeResultPath);

            points.Open(pointShapePath, callback);

            if (selectedIndexesList.Count > 0)
            {
                if (selectedIndexesList.Count <= points.NumShapes)
                {
                    pointsNew.CreateNew(extactedPointShapeResultPath, MapWinGIS.ShpfileType.SHP_POINT);
                    MapWinGIS.Field tmpField;
                    MapWinGIS.Field pointField;
                    for (var f = 0; f < points.NumFields; f++)
                    {
                        tmpField = new MapWinGIS.Field();
                        pointField = points.get_Field(f);
                        tmpField.Name = pointField.Name;
                        tmpField.Type = pointField.Type;
                        tmpField.Precision = pointField.Precision;
                        tmpField.Width = pointField.Width;
                        tmpField.Key = pointField.Key;
                        pointsNew.EditInsertField(tmpField, ref f, callback);
                    }

                    var tmpPointCount = pointsNew.NumShapes;
                    foreach (var selectedIndex in selectedIndexesList)
                    {
                        var tmpShape = points.get_Shape((int)selectedIndex);
                        pointsNew.EditInsertShape(tmpShape, ref tmpPointCount);
                        for (var f = 0; f < points.NumFields; f++)
                        {
                            pointsNew.EditCellValue(f, tmpPointCount, points.get_CellValue(f, (int)selectedIndex));
                        }

                        tmpPointCount++;
                    }

                    pointsNew.Projection = points.Projection;
                    pointsNew.StopEditingShapes(true, true, callback);
                    pointsNew.Close();
                    result = true;
                }
            }

            points.Close();
            return result;
        }


        /// <summary>
        /// Not Implemented, use MapWinGIS::Utils::GridToShapefile
        /// Converts a grid to a shapefile.
        /// </summary>
        /// <param name="gridPath">Full path to the grid.</param>
        /// <param name="resultSFPath">Full path to where the resulting shapefile will be saved.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool GridToShapefile(ref string gridPath, ref string resultSFPath)
        {
            //TODO: Implement this function
            Error.ClearErrorLog();
            gErrorMsg = "This function is not yet implemented.";
            Error.SetErrorMsg(gErrorMsg);
            Debug.WriteLine(gErrorMsg);

            return false;
        }

        /// <summary>
        /// Function to convert a polyline shapefile into a points shapefile by putting all the vertices as points in the output file
        /// </summary>
        /// <param name="PolylinePath">The polyline shapefile</param>
        /// <param name="PointsPath">The result point shapefile</param>
        public static void PolylineToPoints(string PolylinePath, string PointsPath)
        {
            MapWinGIS.Shapefile lines = new MapWinGIS.Shapefile();
            MapWinGIS.Shapefile pt = new MapWinGIS.Shapefile();

            lines.Open(PolylinePath, null);
            DataManagement.DeleteShapefile(ref PointsPath); //modified by JK
            pt.CreateNew(PointsPath, MapWinGIS.ShpfileType.SHP_POINT);
            pt.StartEditingShapes(true, null);

            int numField;
            MapWinGIS.Field newField, currField;
            for (int i = 0; i < lines.NumFields; i++)
            {
                currField = lines.get_Field(i);
                newField = new MapWinGIS.Field();
                newField.Name = currField.Name;
                newField.Precision = currField.Precision;
                newField.Type = currField.Type;
                newField.Width = currField.Width;
                newField.Key = currField.Key;
                numField = pt.NumFields;
                pt.EditInsertField(newField, ref numField, null);
            }

            MapWinGIS.Shape shp, newshp;
            MapWinGIS.Point newpt, currpt;
            int numPts;
            for (int i = 0; i < lines.NumShapes; i++)
            {
                shp = lines.get_Shape(i);
                for (int j = 0; j < shp.numPoints; j++)
                {
                    newshp = new MapWinGIS.Shape();
                    newshp.Create(MapWinGIS.ShpfileType.SHP_POINT);
                    currpt = shp.get_Point(j);
                    newpt = new MapWinGIS.Point();
                    newpt.x = currpt.x;
                    newpt.y = currpt.y;
                    numPts = 0;
                    newshp.InsertPoint(newpt, ref numPts);
                    numPts = pt.NumShapes;
                    pt.EditInsertShape(newshp, ref numPts);

                    for (int k = 0; k < lines.NumFields; k++)
                    {
                        pt.EditCellValue(k, numPts, lines.get_CellValue(k, i));
                    }
                }
            }

            pt.Projection = lines.Projection;
            pt.StopEditingShapes(true, true, null);
            pt.Close();
            lines.Close();
        }


        #region ShapefileToGrid()

        /// <summary>
        /// Convert a shapefile to grid. The resulting grid has the same extents as the shapefile.
        /// The number of resulting grid columns and rows is calculated from the shapefile extents
        /// and the specified cell size.
        /// </summary>
        /// <param name="sfPath">The full path to the shapefile</param>
        /// <param name="gridPath">The full path to the result grid file</param>
        /// <param name="grFileType">Grid file type (can be Ascii, Binary or GeoTiff)</param>
        /// <param name="grDataType">Grid data type (Short, Long, Float or Double) .
        /// Please note that only 'Float' data type can be used if the file type is 'Binary'.</param>
        /// <param name="sfValueField">the name of the field in shapefile attribute
        /// table that contains data</param>
        /// <param name="gridCellSize">Cell (pixel) size of the resulting grid in shapefile projection
        /// map units</param>
        /// <param name="cback">(optional) reports progress and error messages. Set this
        /// parameter to NULL if not needed</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool ShapefileToGrid(string sfPath, string gridPath,
            MapWinGIS.GridFileType grFileType, MapWinGIS.GridDataType grDataType,
            string sfValueField, double gridCellSize, MapWinGIS.ICallback cback)
        {
            MapWinGIS.Shapefile sf = new MapWinGIS.Shapefile();
            MapWinGIS.Extents sfExtents = new MapWinGIS.Extents();

            // get the new grid extents from the shapefile
            if (sf.Open(sfPath, null))
            {
                sfExtents = sf.Extents;
                sf.Close();
                return ShapefileToGrid
                (sfPath, gridPath, grFileType, grDataType, sfValueField, gridCellSize, sfExtents, cback);
            }
            else
            {
                cback.Error("ShapefileToGrid", "ERROR opening shapefile " + sfPath);
                return false;
            }
        }

        /// <summary>
        /// Convert a shapefile to grid. The number of grid rows and columns
        /// is calculated from the specified Cellsize and gridExtents parameters.
        /// </summary>
        /// <param name="sfPath">The full path to the shapefile</param>
        /// <param name="resultGridPath">The full path to the result grid file</param>
        /// <param name="gridFileType">Grid file type (can be Ascii, Binary or GeoTiff)</param>
        /// <param name="gridDataType">Grid data type (Short, Long, Float or Double)</param>
        /// <param name="sfValueField">the name of the shapefile field that contains data</param>
        /// <param name="gridCellSize">Cell size of the grid (in shapefile map units)</param>
        /// <param name="gridExtents">Extents (min. and max. x and y) of the new grid</param>
        /// <param name="cback">(optional) reports progress and error messages. Set this
        /// parameter to NULL if not needed</param> 
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool ShapefileToGrid(string sfPath, string resultGridPath,
            MapWinGIS.GridFileType gridFileType, MapWinGIS.GridDataType gridDataType,
            string sfValueField, double gridCellSize, MapWinGIS.Extents gridExtents, MapWinGIS.ICallback cback)
        {
            // setup the grid header
            double GRID_NODATA_VALUE = -9999.0; // commonly used nodata_value for grids
            MapWinGIS.GridHeader hdr = new MapWinGIS.GridHeader();

            // setup nodata value
            hdr.NodataValue = GRID_NODATA_VALUE;

            // setup dx, dy (cell size)
            hdr.dX = gridCellSize;
            hdr.dY = gridCellSize;

            // setup xllcenter, yllcenter
            hdr.XllCenter = Math.Floor(gridExtents.xMin) + (gridCellSize / 2.0);
            hdr.YllCenter = Math.Floor(gridExtents.yMin) + (gridCellSize / 2.0);

            // setup number of rows and columns
            hdr.NumberCols = (int)Math.Ceiling((gridExtents.xMax - hdr.XllCenter) / gridCellSize) + 1;
            hdr.NumberRows = (int)Math.Ceiling((gridExtents.yMax - hdr.YllCenter) / gridCellSize) + 1;

            return ShapefileToGrid
                (sfPath, resultGridPath, gridFileType, gridDataType, sfValueField, hdr, cback);
        }

        /// <summary>Converts a shapefile to grid. The resulting grid extents and
        /// cell size are specified by the GridHeader object.
        /// </summary>
        /// <param name="SfPath">the name of the shapefile</param>
        /// <param name="GridPath">the name of the new grid. The full path
        /// should be specified. The file extension should be .asc, .bgd or .tiff</param>
        /// <param name="GrdFileType">file type of the new grid (should be set to 
        /// Ascii, Binary or GeoTiff)</param>
        /// <param name="GrdDataType">data format of the new grid</param>
        /// <param name="Fldname">the name of the field that contains data</param>
        /// <param name="GrdHd">contains information about dimension 
        /// (cell size and extents) of the new grid</param>
        /// <param name="cback">(optional) reports progress and error messages. Set this
        /// parameter to NULL if not needed</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        // 
        // 3/2/2008 added by Jiri Kadlec
        public static bool ShapefileToGrid(string SfPath, string GridPath,
            MapWinGIS.GridFileType GrdFileType, MapWinGIS.GridDataType GrdDataType,
            string Fldname, MapWinGIS.GridHeader GrdHd,
            MapWinGIS.ICallback cback)
        {
            int i;
            bool flg;
            string projStr;
            Rasterization.Rasterizer ras = new Rasterization.Rasterizer();

            //open the shapefile
            MapWinGIS.Shapefile MySf = new MapWinGIS.Shapefile();
            flg = MySf.Open(SfPath, cback);
            if (flg == false)
            {
                ras.reportError("ERROR in opening shapefile: " + SfPath, cback);
                MySf.Close();
                return false;
            }

            //get the handle for the field
            MapWinGIS.Field field;
            int FldId = -1;
            int LayFldNum = MySf.NumFields;

            i = 0;
            for (i = 0; i < LayFldNum; ++i)
            {
                field = MySf.get_Field(i);
                if (field.Name.ToLower() == Fldname.ToLower())
                {
                    FldId = i;
                    break;
                }
            }
            if (FldId < 0)
            {
                ras.reportError("The shapefile " + SfPath + " doesn't have a field " + Fldname, cback);
                MySf.Close();
                return false;
            }

            //copy shapefile projection
            projStr = MySf.Projection;
            if (!MapWinUtility.Strings.IsEmpty(projStr))
            {
                GrdHd.Projection = projStr;
            }

            //create a new grid and a new gridheader
            MapWinGIS.Grid NewGrd = new MapWinGIS.Grid();
            flg = NewGrd.CreateNew(GridPath, GrdHd, GrdDataType, GrdHd.NodataValue, false, GrdFileType, cback);
            if (flg == false)
            {
                ras.reportError("ERROR in grid initialization: " + GridPath, cback);
                NewGrd.Close();
                MySf.Close();
            }

            //verify the type of shapefile and call rasterization function
            MapWinGIS.ShpfileType SfType = new MapWinGIS.ShpfileType();
            SfType = MySf.ShapefileType;
            switch (SfType)
            {
                case MapWinGIS.ShpfileType.SHP_POLYGON:
                case MapWinGIS.ShpfileType.SHP_POLYGONM:
                case MapWinGIS.ShpfileType.SHP_POLYGONZ:
                    flg = ras.Poly2Grid(MySf, FldId, NewGrd, GrdHd, GrdDataType, cback);
                    break;
                case MapWinGIS.ShpfileType.SHP_POLYLINE:
                case MapWinGIS.ShpfileType.SHP_POLYLINEM:
                case MapWinGIS.ShpfileType.SHP_POLYLINEZ:
                    flg = ras.Line2Grid(MySf, FldId, NewGrd, GrdHd, GrdDataType, cback);
                    break;
                case MapWinGIS.ShpfileType.SHP_POINT:
                case MapWinGIS.ShpfileType.SHP_POINTM:
                case MapWinGIS.ShpfileType.SHP_POINTZ:
                    flg = ras.Point2Grid(MySf, FldId, NewGrd, GrdHd, GrdDataType, cback);
                    break;
                case MapWinGIS.ShpfileType.SHP_MULTIPOINT:
                case MapWinGIS.ShpfileType.SHP_MULTIPOINTM:
                case MapWinGIS.ShpfileType.SHP_MULTIPOINTZ:
                    flg = ras.Multipoint2Grid(MySf, FldId, NewGrd, GrdHd, GrdDataType, cback);
                    break;
                default:
                    ras.reportError("The shapefile type " + SfType.ToString() + "is not supported.", cback);
                    NewGrd.Close();
                    MySf.Close();
                    flg = false;
                    break;
            }

            //save and close the grid, close shapefile
            NewGrd.Save(GridPath, GrdFileType, cback);
            NewGrd.Close();
            MySf.Close();
            return flg;
        }

        #endregion

        #region PointInPolygon()
        /// <summary>
        /// Determines if a point lies within a polygon.
        /// </summary>
        /// <param name="polygon">The polygon shape.</param>
        /// <param name="testPoint">The point to be tested.</param>
        /// <returns>True if the point is inside, false otherwise.</returns>
        public static bool PointInPolygon(ref MapWinGIS.Shape polygon, ref MapWinGIS.Point testPoint)
        {
            Error.ClearErrorLog();
            if (polygon == null || testPoint == null)
            {
                gErrorMsg = "Unexpected null paramter.";
                Error.SetErrorMsg(gErrorMsg);
                Debug.WriteLine(gErrorMsg);
                return false;
            }
            MapWinGIS.ShpfileType shpType = polygon.ShapeType;
            if (shpType != MapWinGIS.ShpfileType.SHP_POLYGON && shpType != MapWinGIS.ShpfileType.SHP_POLYGONM && shpType != MapWinGIS.ShpfileType.SHP_POLYGONZ)
            {
                gErrorMsg = "Incompatible shape type: must be of type polygon.";
                Error.SetErrorMsg(gErrorMsg);
                Debug.WriteLine(gErrorMsg);
                return false;
            }

            //test if boundaries cross
            double minX, maxX, minY, maxY;
            minX = polygon.Extents.xMin;
            maxX = polygon.Extents.xMax;
            minY = polygon.Extents.yMin;
            maxY = polygon.Extents.yMax;
            if (Globals.CheckBounds(testPoint.x, testPoint.x, testPoint.y, testPoint.y, minX, maxX, minY, maxY) == false)
            {
                //bounds do not cross, point is not inside polygon.
                return false;
            }
            //convert the polygon shape into a vertex array
            //or, if dealing with multiple parts, multiple vertex arrays.
            int numParts = polygon.NumParts;
            Globals.Vertex[][] V = new Globals.Vertex[numParts][];
            Globals.ConvertPolyToVertexArray(ref polygon, out V);
            return PointInPoly(ref V, ref testPoint);
        }
        #endregion

        #region internal PointInPoly()
        internal static bool PointInPoly(ref Globals.Vertex[][] V, ref MapWinGIS.Point point)
        {
            return PointInPoly(ref V, point.x, point.y);
        }
        internal static bool PointInPoly(ref Globals.Vertex[][] V, double x, double y)
        {
            int numParts = V.Length;
            bool inside = true;
            int wn;

            //************* Multi-part Polygon **************************
            if (numParts > 1)
            {
                bool[] pointInPart = new bool[numParts];
                bool[] partIsHole = new bool[numParts];
                int numInside = 0;
                //int numHoles = 0;

                //counter-clockwise polygons are considered holes
                bool currIsClockwise;

                for (int i = 0; i <= numParts - 1; i++)
                {
                    currIsClockwise = Globals.IsClockwise(ref V[i]);
                    if (currIsClockwise)
                    {
                        partIsHole[i] = false;
                    }
                    else
                    {
                        partIsHole[i] = true;
                        //numHoles++;
                    }

                    wn = wn_PnPoly(ref V[i], x, y);
                    if (wn == 0)
                    {
                        pointInPart[i] = false;
                    }
                    else
                    {
                        pointInPart[i] = true;
                        numInside++;
                        //if pt is inside ANY hole, it's OUTSIDE the polygon
                        if (partIsHole[i] == true)
                        {
                            inside = false;
                            break;
                        }
                    }
                }//done looping through parts to see if point is inside any of them
                if (numInside > 0 && inside == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }//end of dealing with multi-part polygons

                //******************* Single-part Polygon **************************
            else
            {
                wn = wn_PnPoly(ref V[0], x, y);
                if (wn == 0)
                {
                    inside = false;
                }
                else
                {
                    inside = true;
                }
            }

            return inside;
        }
        #endregion

        #region private Winding Number Algorithm for PointInPoly()
        //The Winding Number algorithm can be found here:
        //http://softsurfer.com/Archive/algorithm_0103/algorithm_0103.htm
        //
        // Copyright 2001, softSurfer (www.softsurfer.com)
        // This code may be freely used and modified for any purpose
        // providing that this copyright notice is included with it.
        // SoftSurfer makes no warranty for this code, and cannot be held
        // liable for any real or imagined damage resulting from its use.
        // Users of this code must verify correctness for their application.
        //
        // Altered by Angela Hillier on 2/21/06 to work with MapWinGIS objects.

        /// <summary>
        /// Tests if a point is left|on|right of an infinite line.
        ///  See: the January 2001 Algorithm "Area of 2D and 3D Triangles and Polygons"
        ///  at http://softsurfer.com/Archive/algorithm_0101/algorithm_0101.htm
        /// </summary>
        /// <param name="x0">X value for point 0 of the line.</param>
        /// <param name="y0">Y value for point 0 of the line.</param>
        /// <param name="x1">X value for point 1 of the line.</param>
        /// <param name="y1">Y value for point 1 of the line.</param>
        /// <param name="x2">X value of the point being tested.</param>
        /// <param name="y2">Y value of the point being tested.</param>
        /// <returns>A value > 0 if the point is left of the line.
        ///			 A value = 0 if the point is on the line.
        ///			 A value less than 0 if the point right of the line.
        ///	</returns>
        private static double IsLeft(double x0, double y0, double x1, double y1, double x2, double y2)
        {
            return ((x1 - x0) * (y2 - y0) - (x2 - x0) * (y1 - y0));
        }

        /// <summary>
        /// Winding number test for point in polygon
        /// </summary>
        /// <param name="V">Array of vertices in the polygon.</param>
        /// <param name="x">X value of point being tested.</param>
        /// <param name="y">Y value of point being tested.</param>
        /// <returns>The winding number (=0 only if pt is outside poly)</returns>
        private static int wn_PnPoly(ref Globals.Vertex[] V, double x, double y)
        {
            int wn = 0; //the winding number counter
            int numPoints = V.Length;

            //loop through all edges of the polygon
            for (int i = 0; i <= numPoints - 2; i++)
            {
                if (V[i].y <= y)
                {	// start y <= y
                    if (V[i + 1].y > y)// an upward crossing
                    {
                        if (IsLeft(V[i].x, V[i].y, V[i + 1].x, V[i + 1].y, x, y) > 0)  // pt is left of edge
                        {
                            // have a valid up intersect
                            ++wn;
                        }
                    }
                }
                else
                {
                    // start y > P.y (no test needed)
                    if (V[i + 1].y <= y)     // a downward crossing
                    {
                        if (IsLeft(V[i].x, V[i].y, V[i + 1].x, V[i + 1].y, x, y) < 0)  // pt is right of edge
                        {
                            // have a valid down intersect
                            --wn;
                        }
                    }
                }
            }
            return wn;
        }
        #endregion

        #region Centroid Calculations

        /// Centroid()
        /// <summary>
        /// This function returns a MapWinGIS.Point that represents the
        /// mathematical "center of mass" for the MapWinGIS.Shape polygon.
        /// Currently, shapes with multiple parts are supported, but treated
        /// as island chains, not as shapes with holes.  Shapes with holes
        /// or shapes with self-intersecting parts will still return a value,
        /// but it is not likely to be the correct value.
        /// </summary>
        /// <param name="polygon">[In] The MapWinGIS.Shape 
        /// POLYGON/POLYGONZ/POLYGONM to find the centroid of.</param>
        /// <returns>The centroid: a point representing the center of mass of the polygon.</returns>
        /// <remarks>this version of the function has been heavilly redone
        /// from Angela's orginal code in order to allow for the centroid to be calculated
        /// for many more situations.
        /// </remarks>
        public static MapWinGIS.Point Centroid(ref MapWinGIS.Shape polygon)
        {
            string ErrorLocation = "Beginning of Function";
            MapWinGIS.Point centroid = new MapWinGIS.PointClass();
            MapWinGIS.Point groupCentroid = new MapWinGIS.PointClass();
            int numPoints = polygon.numPoints;
            int numParts = polygon.NumParts;
            double PartArea, TotalArea = 0.0;

            try
            {
                if (numParts > 1)
                {
                    // The centroid of multiple parts is the weighted average of the points
                    // Xcentroid = ((Area(i) * X(i)) + (Area(i+1) * X(i+1) ... (Area(i+n) * X(i+n)))/(TotalArea)


                    for (int PartIndex = 0; PartIndex < numParts; PartIndex++)
                    {
                        ErrorLocation = "In the parts loop, PartIndex = " + PartIndex;
                        centroid = CentroidOfPart(polygon, PartIndex);
                        PartArea = AreaOfPart(polygon, PartIndex);

                        //To Do: If a part is inside another part, then it is a hole,
                        //and should be given a negative area for this calculation.

                        TotalArea = TotalArea + PartArea;
                        groupCentroid.x += PartArea * centroid.x;
                        groupCentroid.y += PartArea * centroid.y;
                    }
                    ErrorLocation = "After the multiple parts loop.";
                    centroid.x = groupCentroid.x / TotalArea;
                    centroid.y = groupCentroid.y / TotalArea;
                }
                else
                {
                    ErrorLocation = "After the number of parts was determined to be 1.";
                    // The second parameter will be ignored since only one part exists
                    centroid = CentroidOfPart(polygon, 0);
                }

            }
            catch (Exception ex)
            {
                PassError("Centroid", ErrorLocation, ex);
            }

            return centroid;
        }

        /// CentroidOfPart()
        /// <summary>
        /// This function will calculate the centroid for a specific part of a
        /// Mapwinshape.  If the shape has only one part, the integer value
        /// PartIndex will be ignored, and the centroid for the entire shape
        /// will be calculated.
        /// </summary>
        /// <param name="polygon">MapWinGIS.Shape of type 
        /// POLYGON/POLYGONZ/POLYGONM to obtain the centroid of or containing
        /// the specific part to obtain the centroid of.</param>
        /// <param name="PartIndex">The integer index value of the part in 
        /// polygon to find the centroid of.  This value will be ignored if
        /// there is only one part in the polygon, and teh centroid for the
        /// entire shape will be returned.</param>
        /// <returns>A MapWinGIS.Point representing the position of the 
        /// two dimensional centroid of the part of polygon specified by
        /// PartIndex</returns>
        /// <remarks>
        /// Initially authored by Ted Dunsford on 6/23/2006
        /// Derived from Angela's algorithm, but heavily modified
        /// </remarks>
        public static MapWinGIS.Point CentroidOfPart(MapWinGIS.Shape polygon, int PartIndex)
        {
            string ErrorLocation = "Beginning of Function";
            MapWinGIS.Point centroid = new MapWinGIS.PointClass();
            MapWinGIS.Point currPt, nextPt;
            double area;
            int numPoints;
            int numParts;
            PartBounds part;
            double xSum = 0.0;
            double ySum = 0.0;
            double XShift = 0.0;  //Used only if shape crosses Y axis
            double YShift = 0.0;  //Used only if shape crosses X axis

            // handle invalid parameters and retrieve the begin and end points

            try
            {
                //Also tests polygon and PartIndex
                part = GetPartBounds(polygon, PartIndex);
                ErrorLocation = "After GetPartBounds";

                if (polygon.Extents.xMin < 0 && polygon.Extents.xMax > 0)
                {
                    XShift = polygon.Extents.xMax - polygon.Extents.xMin;
                }
                if (polygon.Extents.yMin < 0 && polygon.Extents.yMax > 0)
                {
                    YShift = polygon.Extents.yMax - polygon.Extents.yMin;
                }
                ErrorLocation = "After Calculating XShift and YShift";

                numPoints = polygon.numPoints;
                numParts = polygon.NumParts;
                area = AreaOfPart(polygon, PartIndex);
                ErrorLocation = "After Calculating AreaOfPart";

                for (int i = part.begPoint; i < part.endPoint; i++)
                {
                    ErrorLocation = "In centroid calculating loop, i = " + i;
                    //MapWinGIS.Point currPt = new MapWinGIS.PointClass();
                    currPt = polygon.get_Point(i);
                    //MapWinGIS.Point nextPt = new MapWinGIS.PointClass();
                    nextPt = polygon.get_Point(i + 1);
                    double cProduct = ((currPt.x + XShift) * (nextPt.y + YShift))
                                    - ((nextPt.x + XShift) * (currPt.y + YShift));

                    xSum += ((currPt.x + XShift) + (nextPt.x + XShift)) * cProduct;
                    ySum += ((currPt.y + YShift) + (nextPt.y + YShift)) * cProduct;

                }
                centroid.x = xSum / (6 * area);
                centroid.y = ySum / (6 * area);

                // corrects for shapes in quadrants other than 1 
                // or clockwise/counterclocwise sign errors
                ErrorLocation = "After centroid calculation loop.";
                if (polygon.Extents.xMax + XShift < 0.0 && centroid.x > 0.0)
                {
                    centroid.x = -1 * centroid.x;
                }
                if (polygon.Extents.xMin + XShift > 0 && centroid.x < 0)
                {
                    centroid.x = -1 * centroid.x;
                }
                if (polygon.Extents.yMax + YShift < 0 && centroid.y > 0)
                {
                    centroid.y = -1 * centroid.y;
                }
                if (polygon.Extents.yMin + YShift > 0 && centroid.y < 0)
                {
                    centroid.y = -1 * centroid.y;
                }
                ErrorLocation = "After flipping centroid across axes";
                // Adjust centroid if we calculated it using an X or Y shift

                centroid.x = centroid.x - XShift;
                centroid.y = centroid.y - YShift;

            }
            catch (Exception ex)
            {
                PassError("InternalCentroidOfPart", ErrorLocation, ex);
            }
            return centroid;
        }

        #endregion

        #region Area Calculations

        /// Area()
        /// <summary>
        /// Computes the area of a polygon. For multi-part polygons, assume holes are counter-clockwise.
        /// This function will only give correct results for shapes in equal-area projection. Don't use
        /// this function for shapes in decimal degrees (lat/long)
        /// </summary>
        /// <param name="shape">The polygon shape.</param>
        /// <returns>The area in square units, or 0.0 if it could not be found.</returns>
        /// <remarks>Revised by Ted Dunsford 6/23/2006 to loop through AreaOfPart</remarks>
        public static double Area(ref MapWinGIS.Shape shape)
        {
            // The previous area function made several calls to AreaOfPart,
            // and didn't subtract holes from area. Instead of using this duplicated
            // code, call the Statistics area version which already works properly.
            // return Statistics.Area(ref shape);

            // Paul Meems - 26 Sept 2011
            // Use the ocx version:
            return shape.Area;

            // Note that AreaOfPart still exists in this namespace; it's used from
            // the centroid functions, and as far as I can tell it works perfectly
            // as-is.
        }

        /// Area (Overloaded version)
        /// <summary>
        /// Overloaded version, computes the area of a polygon. For multi-part polygons, assume holes are 
        /// counter-clockwise. This function also works for shapes in lat/long (decimal degrees) coordinates.
        /// If the shape is in decimal degrees, resulting area will be returned in square kilometres.
        /// For other units, make sure the shape is in equal-area projection.
        /// </summary>
        /// <param name="shape">The polygon shape.</param>
        /// <param name="shapeUnits">Distance units from the shape's projection</param>
        /// <returns>The area in square kilometres (for decimal degree coordinates) or in the squared
        /// shape distance units (for metres, feet and other units)</returns>
        public static double Area(ref MapWinGIS.Shape shape, MapWindow.Interfaces.UnitOfMeasure shapeUnits)
        {
            if (shapeUnits == MapWindow.Interfaces.UnitOfMeasure.DecimalDegrees)
            {
                return Statistics.LLArea(ref shape);
            }
            else
            {
                return Statistics.Area(ref shape);
            }
        }


        /// AreaOfPart()
        /// <summary>
        /// Calculates the area of a part, without taking into consideration any other aspects of the polygon.
        /// </summary>
        /// <param name="polygon">A MapWinGIS.Shape POLYGON, POLYGONZ, or POLYGONM</param>
        /// <param name="PartIndex">The integer index of the part to obtain the area of.
        /// This value will be ignored if the shape only has one part, and the function
        /// will calculate the area of the entire shape.</param>
        /// <returns>A double value that is equal to the area of the part.</returns>
        /// <remarks>Coded by Ted Dunsford 6/23/2006, derived from Angela's Area algorithm
        /// Code reference http://astronomy.swin.edu.au/~pbourke/geometry/polyarea/
        /// Cached in MapWinGeoProc\clsUtils\Documentation\
        /// I don't think that we ever want to return a negative area from this function,
        /// even if the part is a hole, because it is being calculated outside of the 
        /// context of any other parts.  Only the collective Area function should worry about 
        /// ascribing a sign value to the individual part areas.
        ///</remarks>
        public static double AreaOfPart(MapWinGIS.Shape polygon, int PartIndex)
        {
            string ErrorLocation;
            PartBounds part;
            double Area = 0.0;
            MapWinGIS.Point pt1, pt2;
            // MapWinGIS.Point pt1 = new MapWinGIS.Point()
            // MapWinGIS.Point pt2;

            ErrorLocation = "After declaration area.";
            try
            {
                // get the extent of the part and check if parameters are ok
                part = GetPartBounds(polygon, PartIndex);
                ErrorLocation = "After GetPartBounds.";

                //calculate the area for the specific part
                for (int j = part.begPoint; j < part.endPoint; j++)
                {
                    //throw new Exception("begPoint: " + part.begPoint);
                    ErrorLocation = "In area calculation loop, j = " + j;
                    pt1 = polygon.get_Point(j);
                    pt2 = polygon.get_Point(j + 1);

                    double trapArea = ((pt1.x * pt2.y) - (pt2.x * pt1.y));
                    Area += trapArea;

                }//end of calculating individual area for the current part
            }
            catch (Exception ex)
            {
                PassError("AreaOfPart", ErrorLocation, ex);
            }
            Area = 0.5 * Math.Abs(Area);
            return Area;
        }

        #endregion


        /// bool IsSelfIntersecting()
        /// <summary>
        /// Tests a specific part to ascertain if any two segements in the part intersect each other.
        /// </summary>
        /// <param name="shape">A MapWinGIS Shapefile object.</param>
        /// <param name="PartIndex">An integer from 0 to Shape.numParts - 1 specifying the part to test.</param>
        /// <returns>A True if lines intersect, false if not.</returns>
        /// <remarks>
        /// Author: Ted Dunsford 6/21/2006
        /// Algorithm derived from: http://astronomy.swin.edu.au/~pbourke/geometry/lineline2d/
        /// Cached in MapWinGeoProc\Documentation\IntersectionPointOfTwoLines.htm
        /// </remarks>
        public static bool PartIsSelfIntersecting(MapWinGIS.Shape shape, int PartIndex)
        {
            string ErrorLocation = "At the beginning of the function.";
            PartBounds part;

            MapWinGIS.Point pt1, pt2, pt3, pt4;

            try
            {
                // test polygon and PartIndex, and find the boundaries of the part
                part = GetPartBounds(shape, PartIndex);
                ErrorLocation = "After GetPartBounds";

                for (int i = part.begPoint; i < part.endPoint - 1; i++)
                {
                    //If we have compared side 1 and 2 we don't need to compare 2 with 1 again.
                    //This also prevents checking a segment against against itself.
                    for (int j = i + 1; j < part.endPoint; j++)
                    {
                        ErrorLocation = "In intersection testing loop, i = " + i + ", j = " + j;
                        pt1 = shape.get_Point(i);
                        pt2 = shape.get_Point(i + 1);
                        pt3 = shape.get_Point(j);
                        pt4 = shape.get_Point(j + 1);

                        double ua, ub, denom;

                        //If denom is 0, then the lines are parallel and we can return false
                        denom = (pt4.y - pt3.y) * (pt2.x - pt1.x) - (pt4.x - pt3.x) * (pt2.y - pt1.y);
                        if (denom == 0) continue; //continue until we find an instance that intersects

                        //If both ua and ub are between 0 and 1, then the lines intersect.
                        ua = ((pt4.x - pt3.x) * (pt1.y - pt3.y) - (pt4.y - pt3.y) * (pt1.x - pt3.x)) / denom;
                        ub = ((pt2.x - pt2.x) * (pt1.y - pt3.y) - (pt2.y - pt1.y) * (pt1.x - pt3.x)) / denom;
                        if (ua < 1 && ua > 0 && ub < 1 && ub > 0)
                        {
                            //We found an intersection of lines.  We don't need to do anything else.
                            return true;
                        }
                    }//end of j index of line segments
                }//end of i index of line segments
            }
            catch (Exception ex)
            {
                PassError("PartIsSelfIntersecting", ErrorLocation, ex);
            }

            //We never found any intersecting lines, so return false
            return false;
        } //End of IsSelfIntersecting

        /// GetPartBounds()
        /// <summary>
        /// This function verifies that PartIndex references a valid part in 'polygon'
        /// and returns a structure containing the integer part index, beginning point
        /// index for and ending point index for the part specified by PartIndex.  It also
        /// tests polygon to make sure that it is not null and is a polygon shapetype.
        /// </summary>
        /// <param name="polygon">A MapWinGIS.Shape of POLYGON/POLYGONZ/POLYGONM type.</param>
        /// <param name="PartIndex">The integer part index to determine the first and
        /// last point index of.</param>
        /// <returns>A PartBounds structure defined in this class that contains the part
        /// index, the beginning point index and end point index included in the part.</returns>
        public static PartBounds GetPartBounds(MapWinGIS.Shape polygon, int PartIndex)
        {
            string ErrorLocation = "At the beginning of the function.";
            PartBounds mypart = new PartBounds();
            int numParts, numPoints;
            int begPoint, endPoint;
            try
            {
                // Will throw an exception if the polygon is invalid
                CheckPolygon(polygon);
                ErrorLocation = "After CheckPolygon.";

                numParts = polygon.NumParts;
                numPoints = polygon.numPoints;

                // If the numParts is 1, we will ignore the value being sent by PartIndex
                if (numParts == 1)
                {
                    mypart.PartIndex = 1;
                    mypart.begPoint = 0;
                    mypart.endPoint = numPoints - 1;
                    ErrorLocation = "After setting mypart when numParts is 0";
                    return mypart;
                }

                if (PartIndex < 0 || PartIndex > numParts - 1)
                {
                    ArgumentError("In a multipart polygon, PartIndex should range from 0 to " +
                    "polygon.numParts - 1, which was: " + (numParts - 1), "GetPartBounds");
                }

                // ------ Find the Beginning and End of the Part -------- 

                begPoint = polygon.get_Part(PartIndex);
                if (begPoint < 0 || begPoint > numPoints)
                {
                    ArgumentError("The point index returned for the beginning of the part "
                    + PartIndex + " was " + begPoint + " which is outside the valid range "
                    + "of points for the polygon.", "GetPartBounds");
                }
                if (PartIndex == numParts - 1)
                {
                    endPoint = numPoints - 1;
                }
                else
                {
                    endPoint = polygon.get_Part(PartIndex + 1) - 1;
                }
                if (endPoint < 0 || endPoint > numPoints - 1)
                {
                    ArgumentError("The point index returned for the end of the part " + PartIndex +
                    "was " + endPoint + ", which is outside the valid " +
                    "range of points for the polygon.", "GetPartBounds");
                }
                ErrorLocation = "After calculating the begin point and end point of the part.";
                // ---- assign the values to a structure for use elsewhere
                mypart.PartIndex = PartIndex;
                mypart.begPoint = begPoint;
                mypart.endPoint = endPoint;
            }
            catch (Exception ex)
            {
                PassError("GetPartBounds", ErrorLocation, ex);
            }

            return mypart;

        }//End GetPartBounds

        #region ErrorHandling

        /// CheckPolygon()
        /// <summary>
        /// Checks a shape object to ensure that it is a valid polygon.
        /// Errors will throw exceptions.
        /// </summary>
        /// <param name="polygon">MapWinGIS Shape to test.</param>
        private static void CheckPolygon(MapWinGIS.Shape polygon)
        {
            if (polygon == null)
            {
                ArgumentError("The 'polygon' parameter cannot be Null.", "CheckPolygon");
            }

            MapWinGIS.ShpfileType shpType;
            shpType = polygon.ShapeType;

            if (shpType != MapWinGIS.ShpfileType.SHP_POLYGON && shpType != MapWinGIS.ShpfileType.SHP_POLYGONM && shpType != MapWinGIS.ShpfileType.SHP_POLYGONZ)
            {
                ArgumentError("The shape must be of type POLYGON/POLYGONZ/POLYGONM", "CheckPolygon");
            }
        }

        private static void ArgumentError(string Message, string Function)
        {
            if (Message == null)
            {
                if (Function == null)
                {
                    Message = "An error occured in the MapWinGeoProc.Utils class.\n";
                }
                else
                {
                    Message = "An error occured in the " + Function +
                    " function of the MapWinGeoProc.Utils class.\n";
                }
            }
            Message = "An error occured in the " + Function + " function of the Utils Class of MapWinGeoProc.  Details: " + Message + "\n";

            Trace.WriteLine(Message);
            throw new ArgumentException(Message);
        }

        private static void PassError(string Function, string ErrorLocation, Exception ex)
        {
            string Message = "An unspecified Error occured in MapWinGeoProc.Utils.";
            if (Function == null) Function = "[Unspecified]";
            if (ex.Message.Substring(0, 23) == "An error occured in the")
            {
                //We already have our formatted error message, simply append the latest function.
                Message = ex.Message + "Called by the " + Function + " function.\n";
                Trace.WriteLine("Called by the " + Function + " function.\n" + ErrorLocation + "\n");
            }
            else
            {
                //Give all the specific details about our location.
                Message = "An error occured in the " + Function +
                " function of the MapWinGeoProc.Utils class.\n  The error was: " +
                ex.Message + "Additional Information: " + ErrorLocation + "\n";
                Trace.WriteLine(Message);
            }
            throw new Exception(Message, ex);
        }

        #endregion



        /// <summary>
        /// A simple function that takes two grid paths and an output grid path and does a simple comparison by subtracting the cell values of the second grid from the cells of the first grid and outputs the result to the output grid with 0's being no-data values so only differences will show up.
        /// </summary>
        /// <param name="grid1path"></param>
        /// <param name="grid2path"></param>
        /// <param name="outgridpath"></param>
        public static void CreateCompareGrid(string grid1path, string grid2path, string outgridpath)
        {
            MapWinUtility.Logger.Progress("Creating Compare Grid", 0, 0);
            MapWinGIS.Grid grid1 = new MapWinGIS.Grid();
            MapWinGIS.Grid grid2 = new MapWinGIS.Grid();
            MapWinGIS.Grid outgrid = new MapWinGIS.Grid();
            MapWinGIS.GridHeader outhead = new MapWinGIS.GridHeader();

            grid1.Open(grid1path, MapWinGIS.GridDataType.UnknownDataType, true, MapWinGIS.GridFileType.UseExtension, null);
            grid2.Open(grid2path, MapWinGIS.GridDataType.UnknownDataType, true, MapWinGIS.GridFileType.UseExtension, null);

            outhead.CopyFrom(grid1.Header);
            outhead.NodataValue = 0;
            DataManagement.DeleteGrid(ref outgridpath);
            outgrid.CreateNew(outgridpath, outhead, MapWinGIS.GridDataType.FloatDataType, outhead.NodataValue, true, MapWinGIS.GridFileType.UseExtension, null);
            outgrid.Header.Projection = grid1.Header.Projection;

            int nr = outhead.NumberRows;
            int nc = outhead.NumberCols;
            double dX = outhead.dX;
            double dY = outhead.dY;

            for (int row = 1; row < nr - 1; row++)
            {
                for (int col = 1; col < nc - 1; col++)
                {
                    outgrid.set_Value(col, row, Convert.ToSingle(grid1.get_Value(col, row)) - Convert.ToSingle(grid2.get_Value(col, row)));
                }
            }
            outgrid.Save(outgridpath, MapWinGIS.GridFileType.UseExtension, null);
            outgrid.Close();
            grid1.Close();
            grid2.Close();
            MapWinUtility.Logger.Progress("", 0, 0);
        }


        /// <summary>
        /// Function to test proximity of test points to a shapefile. Slow and probably better ways to do this
        /// </summary>
        /// <param name="TestPoints"></param>
        /// <param name="TestDataPath"></param>
        /// <param name="TestDataFieldNum"></param>
        /// <param name="TestDataFieldMatchVal"></param>
        /// <param name="Distance"></param>
        /// <returns></returns>
        public static bool ShapefileProximityTest(MapWinGIS.Point[] TestPoints, string TestDataPath, int TestDataFieldNum, string TestDataFieldMatchVal, double Distance)
        {
            string CachePath = "";
            MapWinGIS.Shapefile sf = new MapWinGIS.Shapefile();
            MapWinGIS.Shapefile sfpts = new MapWinGIS.Shapefile();
            int valFieldNum = 0;

            sf.Open(TestDataPath, null);

            if (sf.ShapefileType == MapWinGIS.ShpfileType.SHP_POINT)
            {
                sf.Close();
                valFieldNum = TestDataFieldNum;
            }
            else if (sf.ShapefileType == MapWinGIS.ShpfileType.SHP_POLYLINE || sf.ShapefileType == MapWinGIS.ShpfileType.SHP_POLYGON)
            {
                CachePath = TestDataPath + "_tmp.shp";
                sfpts.CreateNew(TestDataPath + "_tmp.shp", MapWinGIS.ShpfileType.SHP_POINT);
                MapWinGIS.Field idxField = new MapWinGIS.Field();
                idxField.Name = "Index";
                idxField.Type = MapWinGIS.FieldType.INTEGER_FIELD;
                int idxFieldNum = 0;
                sfpts.EditInsertField(idxField, ref idxFieldNum, null);

                MapWinGIS.Field valField = new MapWinGIS.Field();
                valField.Name = sf.get_Field(TestDataFieldNum).Name;
                valField.Type = sf.get_Field(TestDataFieldNum).Type;
                valField.Width = sf.get_Field(TestDataFieldNum).Width;
                valField.Precision = sf.get_Field(TestDataFieldNum).Precision;
                sfpts.EditInsertField(valField, ref valFieldNum, null);

                for (int i = 0; i < sf.NumShapes; i++)
                {
                    for (int j = 0; j < sf.get_Shape(i).numPoints; j++)
                    {
                        MapWinGIS.Point oldpt = sf.get_Shape(i).get_Point(j);
                        MapWinGIS.Shape newshp = new MapWinGIS.Shape();
                        MapWinGIS.Point newpt = new MapWinGIS.Point();
                        newpt.x = oldpt.x;
                        newpt.y = oldpt.y;
                        newpt.Z = oldpt.Z;
                        int ptidx = 0;
                        newshp.InsertPoint(newpt, ref ptidx);
                        int shpidx = 0;
                        sfpts.EditInsertShape(newshp, ref shpidx);
                        sfpts.EditCellValue(idxFieldNum, shpidx, i);
                        sfpts.EditCellValue(valFieldNum, shpidx, sf.get_CellValue(TestDataFieldNum, i));
                    }
                }
                sf.Close();
                sfpts.SaveAs(CachePath, null);
                sfpts.Close();
            }

            bool retValue = false;
            if (CachePath != "")
            {
                KDTreeDLL.KDTree pointsTree;
                double[][] Points;
                string[] PointVals;
                string proj, projUnits;
                MapWinGIS.Extents pointsExtents;
                Interpolation.KDTreeCachePoints(CachePath, valFieldNum, out pointsTree, out Points, out PointVals, out proj, out projUnits, out pointsExtents, null);

                for (int i = 0; i < TestPoints.Length; i++)
                {
                    double[] loVal = new double[2];
                    loVal[0] = TestPoints[i].x - Distance;
                    loVal[1] = TestPoints[i].y - Distance;
                    double[] hiVal = new double[2];
                    hiVal[0] = TestPoints[i].x + Distance;
                    hiVal[1] = TestPoints[i].y + Distance;


                    Object[] ptIndexObjs = pointsTree.range(loVal, hiVal);
                    if (ptIndexObjs.Length == 0)
                    {
                        retValue = false;
                    }
                    else
                    {
                        if (TestDataFieldMatchVal != "") //Only need to cycle if looking for a type of matched value in the neighbor list
                        {
                            for (int j = 0; j < ptIndexObjs.Length; j++)
                            {
                                int tmpIdx = (int)ptIndexObjs[j];
                                string val = PointVals[tmpIdx];
                                if (val == TestDataFieldMatchVal)
                                {
                                    retValue = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            retValue = true;
                        }
                    }
                    if (retValue == true)
                    {
                        break;
                    }
                }
            }

            if (CachePath != TestDataPath)
            {
                DataManagement.DeleteShapefile(ref CachePath);
            }

            return retValue;
        }



    }
}
