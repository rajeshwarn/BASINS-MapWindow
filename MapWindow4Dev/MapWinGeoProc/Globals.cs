// ********************************************************************************************************
// <copyright file="Globals.cs" company="MapWindow.org">
//     Copyright (c) MapWindow Development team. All rights reserved.
// </copyright>
// Description: This class provides methods for checking bounds, calculating intersect points, deleting shapefiles, etc
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
// 10/17/05 - Angela Hillier - Created original class and functions.
// 04/14/06 - Mark Gray      - new algorithm for DetermineRowClearCount, optimizations in TrimGrid
// 07/04/07 - Simon Batson   - Refactored CopyFields to include renaming of duplicate fieldnames
//                           - Declared FindField as static so it could be used in CopyFields
// 10/23/09 - Paul Meems     - Changes Globals Class from internal to public
// 12/21/10 - Paul Meems     - Made this class StyleCop and ReSharper compliant
// 12/29/10 - Paul Meems     - Finished making StyleCop and ReSharper compliant
//                           - TODO: Check and clean up the code
// ********************************************************************************************************

namespace MapWinGeoProc
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Runtime.InteropServices;
    using MapWinGIS;
    using MapWinUtility;

    /// <summary>
    /// Internal class that provides methods common to several members of MapWinGeoProc.
    /// </summary>
    public class Globals
    {
        #region RemoveDuplicatePts()

        /// <summary>
        /// Remove duplicate points
        /// </summary>
        /// <param name="shape">the shape to use</param>
        public static void RemoveDuplicatePts(ref Shape shape)
        {
            var numPts = shape.numPoints;
            var pointIndex = 0;
            var sfType = shape.ShapeType;
            Shape resultShp = new ShapeClass();
            resultShp.Create(sfType);
            Point prevPt = new PointClass();

            for (var i = 0; i <= numPts - 2; i++)
            {
                var currPt = shape.get_Point(i);
                if (i == 0)
                {
                    resultShp.InsertPoint(currPt, ref pointIndex);
                    prevPt = currPt;
                }
                else
                {
                    if (prevPt.x == currPt.x && prevPt.y == currPt.y)
                    {
                        // don't add this point to the result shape, it is a duplicate point!
                    }
                    else
                    {
                        pointIndex = resultShp.numPoints;
                        resultShp.InsertPoint(currPt, ref pointIndex);
                        prevPt = currPt;
                    }
                }
            }

            pointIndex = resultShp.numPoints;
            resultShp.InsertPoint(shape.get_Point(numPts - 1), ref pointIndex);
            shape = resultShp;
        }

        #endregion

        #region RemoveColinearPts()

        /// <summary>
        /// Remove colinear points
        /// </summary>
        /// <param name="shape">The shape to use</param>
        public static void RemoveColinearPts(ref Shape shape)
        {
            var numPts = shape.numPoints;
            var pointIndex = 0;
            var sfType = shape.ShapeType;
            Shape resultShp = new ShapeClass();
            resultShp.Create(sfType);

            for (var i = 0; i <= numPts - 2; i++)
            {
                Point prevPt = new PointClass();
                var currPt = shape.get_Point(i);
                var nextPt = shape.get_Point(i + 1);
                var diffX = nextPt.x - currPt.x;
                var diffY = nextPt.y - currPt.y;
                if (diffX != 0)
                {
                    Math.Abs(diffY / diffX);
                }

                if (i == 0)
                {
                    resultShp.InsertPoint(currPt, ref pointIndex);

                    // PM Is this working? Is not just the pointer copied?
                    // It is not used either:
                    // prevPt = currPt;
                    pointIndex++;
                }
                else
                {
                    var diffX1 = currPt.x - prevPt.x;
                    var diffY1 = currPt.y - prevPt.y;
                    var diffX2 = nextPt.x - currPt.x;
                    var diffY2 = nextPt.y - currPt.y;
                    var slope1 = diffY1 / diffX1;
                    var slope2 = diffY2 / diffX2;

                    if (diffX1 == 0 && diffX2 == 0) 
                    {
                        // both are vertical
                    }
                    else if (diffY1 == 0 && diffY2 == 0) 
                    {
                        // both are horizontal
                    }
                    else if ((diffX1 == 0 && diffY2 == 0) || (diffY1 == 0 && diffX2 == 0))
                    {
                        // One is horizontal, the other vertical
                        if (currPt.x != prevPt.x || currPt.y != prevPt.y)
                        {
                            resultShp.InsertPoint(currPt, ref pointIndex);

                            // PM Is this working? Is not just the pointer copied?
                            // It is not used anyway:
                            // prevPt = currPt;
                            pointIndex++;
                        }
                    }
                    else if (Math.Abs(slope1) != Math.Abs(slope2)) 
                    {
                        // the segments do not have the same slope
                        if (currPt.x != prevPt.x || currPt.y != prevPt.y)
                        {
                            resultShp.InsertPoint(currPt, ref pointIndex);

                            // PM Is this working? Is not just the pointer copied?
                            // It is not used anyway:
                            // prevPt = currPt;
                            pointIndex++;
                        }
                    }
                }

                if (i + 1 == numPts - 1) 
                {
                    // on last segment
                    resultShp.InsertPoint(nextPt, ref pointIndex);
                }
            }

            // PM Is this working? Is not just the pointer copied?
            shape = resultShp;
        }

        #endregion

        #region ReverseSimplePoly()

        /// <summary>
        /// Reverses the orientation of a single-part polgyon.
        /// </summary>
        /// <param name="polygon">Single-part polygon.</param>
        public static void ReverseSimplePoly(ref Shape polygon)
        {
            // Paul Meems 22 December 2010: Added check for parts:
            if (polygon.NumParts > 1)
            {
                throw new Exception("Error in ReverseSimplePoly. The input shape has multiple parts");
            }

            Shape reverseShp = new ShapeClass();
            reverseShp.Create(polygon.ShapeType);
            var numPoints = polygon.numPoints;
            for (var i = numPoints - 1; i >= 0; i--)
            {
                var pointIndex = reverseShp.numPoints;
                reverseShp.InsertPoint(polygon.get_Point(i), ref pointIndex);
            }

            polygon = reverseShp;
        }

        #endregion

        #region FixMultiPartPoly()

        /// <summary>
        /// Determines which shapes are holes and which shapes are islands in
        /// a multi-part polygon and fixes their orientations accordingly.
        /// </summary>
        /// <param name="polygon">The multi-part polygon whose parts need to be checked.</param>
        public static void FixMultiPartPoly(ref Shape polygon)
        {
            // Paul Meems 22 July 2009: Added a try-catch, all for bug #1068
            try
            {
                var numParts = polygon.NumParts;
                if (numParts == 0)
                {
                    numParts = 1;
                }

                if (numParts == 1) 
                {
                    // This isn't really a multiPart polygon, but there is some checking we can do...

                    // just make sure it's clockwise
                    if (IsClockwise(ref polygon))
                    {
                        return;
                    }

                    ReverseSimplePoly(ref polygon);
                    return;
                }

                Shape[] parts;
                SeparateParts(ref polygon, out parts);

                for (var i = 0; i <= numParts - 1; i++)
                {
                    var currIsClockwise = IsClockwise(ref parts[i]);
                    var reverse = false;
                    var partIsHole = false;

                    // Decide if the current part is an island or a hole.
                    // Properties of Holes:
                    // 1) Extents are inside the extents of another part.
                    // 2) All points are inside the above part.
                    for (var j = 0; j <= numParts - 1; j++)
                    {
                        if (j != i)
                        {
                            if (ExtentsInside(parts[j].Extents, parts[i].Extents))
                            {
                                // found a potential hole, do further checking
                                var pt = parts[i].get_Point(0);
                                var insidePoint = Utils.PointInPolygon(ref parts[j], ref pt);
                                //while (Marshal.ReleaseComObject(pt) != 0)
                                //{
                                //}

                                if (insidePoint)
                                {
                                    // part is a hole
                                    partIsHole = true;
                                    break;
                                }
                            }
                        }
                    }

                    // done checking current part against all other parts
                    if (partIsHole && currIsClockwise)
                    {
                        // Hole, make sure it's in counter-clockwise order
                        reverse = true;
                    }
                    else if (!partIsHole && !currIsClockwise)
                    {
                        // Island, make sure it's in clockwise order
                        reverse = true;
                    }

                    if (reverse)
                    {
                        ReverseSimplePoly(ref parts[i]);
                    }
                }

                // done looping through parts and correcting orientation (if necessary)
                Shape resultShp = new ShapeClass();
                resultShp.Create(polygon.ShapeType);
                CombineParts(ref parts, out resultShp);
                polygon = resultShp;
            }
            catch (Exception ex)
            {
                WriteErrorMessage("MapWinGeoProc:Globals:FixMultiPartPoly:Exception: " + ex);
                throw;
            }
        }

        #endregion

        #region CombineParts()

        /// <summary>
        /// Takes an array of simple polygons and combines them into one multi-part shape.
        /// </summary>
        /// <param name="parts">The array of polygons.</param>
        /// <param name="resultShp">The resulting multi-part shape.</param>
        public static void CombineParts(ref Shape[] parts, out Shape resultShp)
        {
            var numParts = parts.Length;
            resultShp = new ShapeClass();
            resultShp.Create(parts[0].ShapeType);
            for (var i = 0; i <= numParts - 1; i++)
            {
                var pointIndex = resultShp.numPoints;
                resultShp.InsertPart(pointIndex, ref i);

                var numPts = parts[i].numPoints;
                for (var j = 0; j <= numPts - 1; j++)
                {
                    pointIndex = resultShp.numPoints;
                    resultShp.InsertPoint(parts[i].get_Point(j), ref pointIndex);
                }
            }

            // TODO Check if shape is valid
        }

        #endregion

        #region ExtentsInside()

        /// <summary>
        /// Finds if the testExtents lie completely inside of the 'outerExtents.'
        /// </summary>
        /// <param name="outerExtents">The extents belonging to the surrounding shape.</param>
        /// <param name="testExtents">The extents belonging to the inner shape.</param>
        /// <returns>True if the test extents are completely inside of the outer extents, false otherwise.</returns>
        public static bool ExtentsInside(Extents outerExtents, Extents testExtents)
        {
            if ((testExtents.xMin >= outerExtents.xMin && testExtents.xMin <= outerExtents.xMax) &&
                (testExtents.xMax >= outerExtents.xMin && testExtents.xMax <= outerExtents.xMax)
                && (testExtents.yMin >= outerExtents.yMin && testExtents.yMin <= outerExtents.yMax) &&
                (testExtents.yMax >= outerExtents.yMin && testExtents.yMax <= outerExtents.yMax))
            {
                // testExtents are inside of outerExtents
                return true;
            }

            return false;
        }

        #endregion

        #region SeparateParts()

        /// <summary>
        /// Separates all parts of a multi-part polygon.
        /// </summary>
        /// <param name="poly">The mulit-part polygon to be separated.</param>
        /// <param name="polyParts">The array that will hold each separated polygon part.</param>
        public static void SeparateParts(ref Shape poly, out Shape[] polyParts)
        {
            var numParts = poly.NumParts;
            if (numParts == 0)
            {
                numParts = 1;
            }

            var numPoints = poly.numPoints;
            var parts = new Shape[numParts];

            if (numParts > 1)
            {
                // separate parts of polygon
                for (var i = 0; i <= numParts - 1; i++)
                {
                    parts[i] = new ShapeClass();
                    parts[i].Create(poly.ShapeType);

                    var begPart = poly.get_Part(i);
                    int endPart;
                    if (i < numParts - 1)
                    {
                        endPart = poly.get_Part(i + 1);
                    }
                    else
                    {
                        endPart = numPoints;
                    }

                    for (var j = begPart; j <= endPart - 1; j++)
                    {
                        var pointIndex = parts[i].numPoints;
                        parts[i].InsertPoint(poly.get_Point(j), ref pointIndex);
                    }
                }

                polyParts = parts;
            }
            else
            {
                parts[0] = new ShapeClass();
                parts[0].Create(poly.ShapeType);
                parts[0] = poly;
                polyParts = parts;
            }
        }

        #endregion

        #region ConvertPolyToVertexArray()

        /// <summary>
        /// Takes a MapWinGIS polygon shape and stores all x/y coordinates into a vertex array.
        /// </summary>
        /// <param name="poly">The polygon to be converted.</param>
        /// <param name="polyVertArray">The array[numParts][] that will contain the polygon vertices.</param>
        public static void ConvertPolyToVertexArray(ref Shape poly, out Vertex[][] polyVertArray)
        {
            var numParts = poly.NumParts;
            if (numParts == 0)
            {
                numParts = 1;
            }

            var numPoints = poly.numPoints;
            var vertArray = new Vertex[numParts][];

            if (numParts > 1)
            {
                // separate parts of polygon
                for (var i = 0; i <= numParts - 1; i++)
                {
                    var begPart = poly.get_Part(i);
                    int endPart;
                    if (i < numParts - 1)
                    {
                        endPart = poly.get_Part(i + 1);
                    }
                    else
                    {
                        endPart = numPoints;
                    }

                    var numPtsInPart = endPart - begPart;
                    vertArray[i] = new Vertex[numPtsInPart];
                    var pointIndex = begPart;
                    for (var j = 0; j <= numPtsInPart - 2; j++)
                    {
                        vertArray[i][j] = new Vertex(poly.get_Point(pointIndex).x, poly.get_Point(pointIndex).y);
                        pointIndex++;
                    }

                    // be sure to 'close' the polygon in the vertex array!
                    vertArray[i][numPtsInPart - 1] = vertArray[i][0];
                }
            }
            else
            {
                // all points in polygon go into same vertex array
                vertArray[0] = new Vertex[numPoints];
                for (var i = 0; i <= numPoints - 1; i++)
                {
                    vertArray[0][i] = new Vertex(poly.get_Point(i).x, poly.get_Point(i).y);
                }
            }

            polyVertArray = vertArray;
        }

        #endregion

        #region PrepareResultSF()

        /// <summary>
        /// Deletes any file located at resultSFPath and creates a new one (resultSF).
        /// </summary>
        /// <param name="resultSFPath">Full path to where the result file should be saved.</param>
        /// <param name="resultSF">The result shapefile.</param>
        /// <param name="sfType">The MapWinGIS shapefile type.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool PrepareResultSF(ref string resultSFPath, ref Shapefile resultSF, ShpfileType sfType)
        {
            return PrepareResultSF(ref resultSFPath, ref resultSF, sfType, false);
        }

        /// <summary>
        /// Deletes any file located at resultSFPath and creates a new one (resultSF).
        /// </summary>
        /// <param name="resultSFPath">Full path to where the result file should be saved.</param>
        /// <param name="resultSF">The result shapefile.</param>
        /// <param name="sfType">The MapWinGIS shapefile type.</param>
        /// <param name="skipMwShapeIDField">Do not create the MWShapeID field.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool PrepareResultSF(
            ref string resultSFPath, 
            ref Shapefile resultSF, 
            ShpfileType sfType,
            bool skipMwShapeIDField)
        {
            DataManagement.DeleteShapefile(ref resultSFPath);

            // create the result shapeFile
            if (resultSF.CreateNew(resultSFPath, sfType) == false)
            {
                WriteErrorMessage("Problem creating the result shapeFile: " +
                                  resultSF.get_ErrorMsg(resultSF.LastErrorCode));
                return false;
            }

            if (!skipMwShapeIDField)
            {
                // Insert the ID field so that it's a valid file:
                var newfld = new Field
                                 {
                                     Key = "MWShapeID",
                                     Name = "MWShapeID",
                                     Precision = 10,
                                     Type = FieldType.INTEGER_FIELD
                                 };
                var fldIdx = resultSF.NumFields;
                resultSF.EditInsertField(newfld, ref fldIdx, null);
            }

            return true;
        }

        #endregion

        #region CheckBounds()

        /// <summary>
        /// Checks if two shape boundaries overlap.
        /// </summary>
        /// <param name="shp1">First shape.</param>
        /// <param name="shp2">Second shape.</param>
        /// <returns>True if the boundaries overlap, false otherwise.</returns>
        /// <remarks>Angela Hillier 10/05</remarks>
        public static bool CheckBounds(ref Shape shp1, ref Shape shp2)
        {
            double minX1, maxX1, minY1, maxY1, minZ1, maxZ1, minX2, maxX2, minY2, maxY2, minZ2, maxZ2;
            shp1.Extents.GetBounds(out minX1, out minY1, out minZ1, out maxX1, out maxY1, out maxZ1);
            shp2.Extents.GetBounds(out minX2, out minY2, out minZ2, out maxX2, out maxY2, out maxZ2);

            if (minX1 > maxX2 || maxX1 < minX2 || minY1 > maxY2 || maxY1 < minY2)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if two shape boundaries overlap.
        /// </summary>
        /// <param name="minX1">Minimum x-value for bounding box 1.</param>
        /// <param name="maxX1">Maximum x-value for bounding box 1.</param>
        /// <param name="minY1">Minimum y-value for bounding box 1.</param>
        /// <param name="maxY1">Maximum y-value for bounding box 1.</param>
        /// <param name="minX2">Minimum x-value for bounding box 2.</param>
        /// <param name="maxX2">Maximum x-value for bounding box 2.</param>
        /// <param name="minY2">Minimum y-value for bounding box 2.</param>
        /// <param name="maxY2">Maximum y-value for bounding box 2.</param>
        /// <returns>True if overlap</returns>
        public static bool CheckBounds(
            double minX1, 
            double maxX1, 
            double minY1, 
            double maxY1,
            double minX2, 
            double maxX2, 
            double minY2, 
            double maxY2)
        {
            if (minX1 > maxX2 || maxX1 < minX2 || minY1 > maxY2 || maxY1 < minY2)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region SimpleIntersect()

        /// <summary>
        /// Finds if two INFINITE lines will intersect, and if so, computes the intersect point.
        /// </summary>
        /// <param name="line1">The first infinite line</param>
        /// <param name="line2">The second infinite line</param>
        /// <param name="intersectPoint">The point of intersection</param>
        /// <returns>True if one intersect point exists, false otherwise.</returns>
        public static bool SimpleIntersect(Line line1, Line line2, out Point intersectPoint)
        {
            Point intPoint = new PointClass();

            // put line in the General form: y = a + bx
            var diffX1 = line1.p1.x - line1.p0.x;
            var diffY1 = line1.p1.y - line1.p0.y;
            double b1 = 0;
            if (diffX1 != 0)
            {
                b1 = diffY1 / diffX1;
            }

            // solve for a1 (a1 = y1 - b1*x1)
            var a1 = line1.p0.y - (b1 * line1.p0.x);

            var diffX2 = line2.p1.x - line2.p0.x;
            var diffY2 = line2.p1.y - line2.p0.y;
            double b2 = 0;
            if (diffX2 != 0)
            {
                b2 = diffY2 / diffX2;
            }

            // solve for a2 (a2 = y2 - b2*x2)
            var a2 = line2.p0.y - (b2 * line2.p0.x);

            if (b1 == b2)
            {
                // lines have the same slope (parallel or coincident)
                intersectPoint = intPoint;
                return false;
            }

            // in general, the two lines (y = a1 + b1x and y = a2 + b2x) 
            // intersect at xi = - (a1 - a2) / (b1 - b2) yi = a1 + b1xi 
            intPoint.x = -(a1 - a2) / (b1 - b2);
            intPoint.y = a1 + (b1 * intPoint.x);
            intersectPoint = intPoint;
            return true;
        }

        #endregion

        #region LinesIntersect2D()

        /// <summary>
        /// Calculates the intersection point between two lines.
        /// </summary>
        /// <param name="lineSegment1">First line segment.</param>
        /// <param name="lineSegment1IsRay">True if line 1 is a ray, false if it's a finite segment.</param>
        /// <param name="lineSegment2">Second line segment.</param>
        /// <param name="lineSegment2IsRay">True if line 2 is a ray, false if it's a finite segment.</param>
        /// <param name="intersectPoint">The intersection point between segments 1 and 2.</param>
        /// <returns>True if a valid intersect point exists, false if the lines do not cross.</returns>
        /// <remarks>Ideas for this function were found here http://softsurfer.com/Archive/algorithm_0104/algorithm_0104B.htm
        /// and may fall under the following copyright: Copyright 2001, softSurfer (www.softsurfer.com)
        /// This code may be freely used and modified for any purpose providing that this copyright notice is included with it.
        /// SoftSurfer makes no warranty for this code, and cannot be held liable for any real or imagined damage 
        /// resulting from its use. Users of this code must verify correctness for their application. 
        /// However, the code below is original and written by Angela Hillier.</remarks>
        public static bool LinesIntersect2D(Line lineSegment1, bool lineSegment1IsRay, Line lineSegment2, bool lineSegment2IsRay, out Point intersectPoint)
        {
            bool intersect;
            Point pt = new PointClass();

            // find direction vector <u> for S1
            Vector u = new VectorClass(); // direction vector for S1
            var diffX = lineSegment1.p1.x - lineSegment1.p0.x;
            var diffY = lineSegment1.p1.y - lineSegment1.p0.y;
            var magnitude = Math.Sqrt((diffX * diffX) + (diffY * diffY));
            u.i = diffX / magnitude;
            u.j = diffY / magnitude;

            // find direction vector <v> for S2
            Vector v = new VectorClass(); // direction vector for S2
            diffX = lineSegment2.p1.x - lineSegment2.p0.x;
            diffY = lineSegment2.p1.y - lineSegment2.p0.y;
            magnitude = Math.Sqrt((diffX * diffX) + (diffY * diffY));
            v.i = diffX / magnitude;
            v.j = diffY / magnitude;

            // now the lines can be put into their parametric form:
            // S1 = P(s) = P0 + s(P1-P0) = P0 + s<u>, where P0 is a point on the line and <u> is the direction vector
            // S2 = Q(t) = Q0 + t(Q1-Q0) = Q0 + t<w> where Q0 is a point on the line and <w> is the direction vector
            // These lines are parallel when and only when their directions are collinear
            // that is, when the perpendicular product of the two vectors <u> and <w> equals zero
            var perp = PerpProduct(u, v);
            if (perp == 0)
            {
                // The lines are parallel, 
                // if they are not coincident, they will never intersect
                // if coincident, they share more than one point.
                // We only want to return a single point of intersection for now
                // but later it might be a good to extend this code to return
                // the segment of intersection for coincident lines.
                intersect = false;
            }
            else
            {
                // lines are not parallel so might intersect at a unique point
                // To determine sI, we have the vector equality 
                // P(s)-Q0 = <w> + s<u>  where <w>=P0-Q0
                Vector w = new VectorClass();
                diffX = lineSegment1.p0.x - lineSegment2.p0.x;
                diffY = lineSegment1.p0.y - lineSegment2.p0.y;

                // note: w is not a direction (unit) vector, so do not divide by magnitude
                w.i = diffX;
                w.j = diffY;

                // At the intersection point, the vector P(s)-Q0 is perpendicular 
                // to v^, and this is equivalent to the perp product condition that v^·(w+su) = 0
                // after solving, we can now get s and t at the point of intersection:
                // double sI = (((v.j * w.i) - (v.i* w.j))/((v.i*u.j) - (v.j*u.i)));
                // double tI = (((u.i * w.j) - (u.j * w.i))/((u.i*v.j) - (u.j*v.i)));
                var sI = PerpProduct(v, w) / perp;
                var tI = PerpProduct(u, w) / perp;

                // if the first line is a ray but the second is not
                // intersection only occurs when sI >= 0
                if (lineSegment1IsRay && sI >= 0 && lineSegment2IsRay == false)
                {
                    // our second line is finite, 
                    // find its bounds and check if the intersection pt 
                    // is valid.
                    double minSegX, minSegY, maxSegX, maxSegY;
                    if (lineSegment2.p0.x < lineSegment2.p1.x)
                    {
                        minSegX = lineSegment2.p0.x;
                        maxSegX = lineSegment2.p1.x;
                    }
                    else
                    {
                        minSegX = lineSegment2.p1.x;
                        maxSegX = lineSegment2.p0.x;
                    }

                    if (lineSegment2.p0.y < lineSegment2.p1.y)
                    {
                        minSegY = lineSegment2.p0.y;
                        maxSegY = lineSegment2.p1.y;
                    }
                    else
                    {
                        minSegY = lineSegment2.p1.y;
                        maxSegY = lineSegment2.p0.y;
                    }

                    // compute the intersect point
                    // I = S2.Q0 + tI * v
                    var valueX = lineSegment2.p0.x + (tI * v.i);
                    var valueY = lineSegment2.p0.y + (tI * v.j);

                    // if the intersect point is valid, save the point to iPoint
                    if (valueX <= maxSegX && valueX >= minSegX && valueY <= maxSegY && valueY >= minSegY)
                    {
                        pt.x = valueX;
                        pt.y = valueY;
                        intersect = true;
                    }
                    else
                    {
                        intersect = false;
                    }
                }
                else
                {
                    // if our second line is a ray but the first is not
                    // intersection only occurs when tI >= 0
                    if (lineSegment2IsRay && tI >= 0 && lineSegment1IsRay == false)
                    {
                        // our first line is finite, 
                        // find its bounds and check if the intersection point 
                        // is valid.
                        double minSegX, minSegY, maxSegX, maxSegY;
                        if (lineSegment1.p0.x < lineSegment1.p1.x)
                        {
                            minSegX = lineSegment1.p0.x;
                            maxSegX = lineSegment1.p1.x;
                        }
                        else
                        {
                            minSegX = lineSegment1.p1.x;
                            maxSegX = lineSegment1.p0.x;
                        }

                        if (lineSegment1.p0.y < lineSegment1.p1.y)
                        {
                            minSegY = lineSegment1.p0.y;
                            maxSegY = lineSegment1.p1.y;
                        }
                        else
                        {
                            minSegY = lineSegment1.p1.y;
                            maxSegY = lineSegment1.p0.y;
                        }

                        // compute the intersect point
                        // I = S1.Q0 + sI * u
                        var valueX = lineSegment1.p0.x + (sI * u.i);
                        var valueY = lineSegment1.p0.y + (sI * u.j);

                        // if the intersect point is valid, save the point to iPoint
                        if (valueX <= maxSegX && valueX >= minSegX && valueY <= maxSegY && valueY >= minSegY)
                        {
                            pt.x = valueX;
                            pt.y = valueY;
                            intersect = true;
                        }
                        else
                        {
                            intersect = false;
                        }
                    }
                    else
                    {
                        // if both lines are rays they should cross as long
                        // as both tI and sI are >= 0
                        if (lineSegment1IsRay && lineSegment2IsRay && tI >= 0 && sI >= 0)
                        {
                            // compute the intersect point
                            // I = S1.Q0 + sI * u
                            pt.x = lineSegment1.p0.x + (sI * u.i);
                            pt.y = lineSegment1.p0.y + (sI * u.j);
                            intersect = true;
                        }
                        else
                        {
                            // if both lines are segments,
                            // they intersect when tI and sI >= 0 and the intersect point is within
                            // the segment boundaries.
                            if (lineSegment1IsRay == false && lineSegment2IsRay == false && tI >= 0 && sI >= 0)
                            {
                                // compute the intersect point
                                // I = S1.Q0 + sI * u
                                var valueX = lineSegment1.p0.x + (sI * u.i);
                                var valueY = lineSegment1.p0.y + (sI * u.j);

                                // both of the lines are finite, find each of their
                                // bounds and check if the intersection point 
                                // falls within both.

                                // Find bounds for segment #1:
                                double minX1, minY1, maxX1, maxY1;
                                if (lineSegment1.p0.x < lineSegment1.p1.x)
                                {
                                    minX1 = lineSegment1.p0.x;
                                    maxX1 = lineSegment1.p1.x;
                                }
                                else
                                {
                                    minX1 = lineSegment1.p1.x;
                                    maxX1 = lineSegment1.p0.x;
                                }

                                if (lineSegment1.p0.y < lineSegment1.p1.y)
                                {
                                    minY1 = lineSegment1.p0.y;
                                    maxY1 = lineSegment1.p1.y;
                                }
                                else
                                {
                                    minY1 = lineSegment1.p1.y;
                                    maxY1 = lineSegment1.p0.y;
                                }

                                // Find segment #2's bounds
                                double minX2, minY2, maxX2, maxY2;
                                if (lineSegment2.p0.x < lineSegment2.p1.x)
                                {
                                    minX2 = lineSegment2.p0.x;
                                    maxX2 = lineSegment2.p1.x;
                                }
                                else
                                {
                                    minX2 = lineSegment2.p1.x;
                                    maxX2 = lineSegment2.p0.x;
                                }

                                if (lineSegment2.p0.y < lineSegment2.p1.y)
                                {
                                    minY2 = lineSegment2.p0.y;
                                    maxY2 = lineSegment2.p1.y;
                                }
                                else
                                {
                                    minY2 = lineSegment2.p1.y;
                                    maxY2 = lineSegment2.p0.y;
                                }

                                // check the intersect point against both segment boundaries
                                if ((valueX <= maxX1 && valueX >= minX1 && valueY <= maxY1 && valueY >= minY1)
                                    && (valueX <= maxX2 && valueX >= minX2 && valueY <= maxY2 && valueY >= minY2))
                                {
                                    // intersect point lies within the boundaries of both segments
                                    // save the point to iPoint
                                    pt.x = valueX;
                                    pt.y = valueY;
                                    intersect = true;
                                }
                                else
                                {
                                    intersect = false;
                                }
                            }
                            else
                            {
                                intersect = false;
                            }
                        }
                    }
                }
            }

            intersectPoint = pt;
            return intersect;
        }

        #endregion

        #region FindAndSortValidIntersects()

        /// <summary>
        /// Sorts all valid intersects in the array intersectPts.
        /// </summary>
        /// <param name="numIntersects">Expected number of valid intersects.</param>
        /// <param name="intersectPts">Array of all possible intersect points.</param>
        /// <param name="validIntersects">Array that will contain only the valid intersect points in sorted order.</param>
        /// <param name="startPt">The reference point to sort the valid intersect points by.</param>
        /// <remarks>Angela Hillier 10/05</remarks>
        public static void FindAndSortValidIntersects(int numIntersects, ref Point[] intersectPts, ref Point[] validIntersects, ref Point startPt)
        {
            for (var i = 0; i <= numIntersects - 1; i++)
            {
                validIntersects[i] = intersectPts[i];
            }

            SortPointsArray(ref startPt, ref validIntersects);
        }

        /// <summary>
        /// Sorts all valid intersects in the array intersectPts, along with corresponding polygon locations in array polyLoc.
        /// </summary>
        /// <param name="numIntersects">Expected number of valid intersects.</param>
        /// <param name="intersectPts">Array of all possible intersect points.</param>
        /// <param name="validIntersects">Array that will contain only the valid intersect points in sorted order.</param>
        /// <param name="startPt">The reference point to sort the valid intersect points by.</param>
        /// <param name="polyLoc">Array with corresponding indicies to where an intersect pt occurs in polygon.</param>
        public static void FindAndSortValidIntersects(int numIntersects, ref Point[] intersectPts, ref Point[] validIntersects, ref Point startPt, ref int[] polyLoc)
        {
            for (var i = 0; i < numIntersects; i++)
            {
                validIntersects[i] = intersectPts[i];
            }

            SortIntersectAndLocationArrays(ref startPt, ref validIntersects, ref polyLoc);
        }

        #endregion

        #region PtDistance() -- used by SortPointsArray()

        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        /// <param name="pt0">The first point.</param>
        /// <param name="pt1">The second point.</param>
        /// <returns>The distance between pt0 and pt1.</returns>
        /// <remarks>Angela Hillier 10/05</remarks>
        public static double PtDistance(ref Point pt0, ref Point pt1)
        {
            var diffX = pt1.x - pt0.x;
            var diffY = pt1.y - pt0.y;
            return Math.Sqrt((diffX * diffX) + (diffY * diffY));
        }

        /// <summary>
        /// Calculates the distance between two points
        /// </summary>
        /// <param name="pt0">The first point</param>
        /// <param name="pt1">The second point</param>
        /// <returns>The distance</returns>
        public static double PtDistance(Point pt0, Point pt1)
        {
            var diffX = pt1.x - pt0.x;
            var diffY = pt1.y - pt0.y;
            return Math.Sqrt((diffX * diffX) + (diffY * diffY));
        }

        #endregion

        #region DetermineRowClearCount()

        /// <summary>
        /// Determines how many rows can be filled before unmanaged resources should
        /// be released.
        /// </summary>
        /// <param name="newNumRows">The total number of rows in the grid that's being filled.</param>
        /// <param name="newNumCols">The total number of cols in the grid that's being filled.</param>
        /// <returns>The number of rows that can be filled before COM objects need to be released.</returns>
        public static int DetermineRowClearCount(int newNumRows, int newNumCols)
        {
            // TODO: Determine optimal time based on available memory, newNumRows
            // and newNumCols to dispose of COM objects.
            var rowClearCount = 500;
            if (newNumCols > 100)
            {
                rowClearCount = 4000000 / newNumCols;
            }

            if (rowClearCount < 1)
            {
                rowClearCount = 1;
            }

            return rowClearCount;
        }

        #endregion

        #region TrimGrid

        /// <summary>
        /// Removes rows and columns that contain only NoData values from the edges of the grid.
        /// </summary>
        /// <param name="inputGridfilePath">The full path to the input grid.</param>
        /// <param name="resultGridfilePath">The full path to where the result grid should be saved.</param>
        /// <returns>False if an error occurs, true otherwise.</returns>
        public static bool TrimGrid(ref string inputGridfilePath, ref string resultGridfilePath)
        {
            using (var inputGridWrapper = new GridWrapper())
            {
                if (inputGridWrapper.Open(inputGridfilePath, true) == false)
                {
                    inputGridWrapper.Dispose();
                    return false;
                }

                var noData = (double)inputGridWrapper.GetNodataValue();
                var numRows = inputGridWrapper.GetNumRows();
                var numCols = inputGridWrapper.GetNumCols();
                var firstRowLoc = 0;
                var lastRowLoc = numRows - 1;
                var firstColLoc = 0;
                var lastColLoc = numCols - 1;
                var found = false;

                if (numRows <= 0 || numCols <= 0)
                {
                    WriteErrorMessage("The grid has 0 rows or columns. Aborting TrimGrid()");
                    inputGridWrapper.Dispose();
                    return false;
                }

                // Find which row contains the first valid Y points (yMax).
                for (var row = 0; row <= numRows - 1; row++)
                {
                    for (var col = 0; col <= numCols - 1; col++)
                    {
                        if (Convert.ToDouble(inputGridWrapper.GetValue(col, row)) == noData)
                        {
                            continue;
                        }

                        firstRowLoc = row;
                        found = true;
                        break;
                    }

                    if (found)
                    {
                        break;
                    }
                }

                // Find which row contains the last valid Y points (yMin)
                found = false;
                for (var row = numRows - 1; row >= 0; row--)
                {
                    for (var col = 0; col <= numCols - 1; col++)
                    {
                        if (Convert.ToDouble(inputGridWrapper.GetValue(col, row)) == noData)
                        {
                            continue;
                        }

                        lastRowLoc = row;
                        found = true;
                        break;
                    }

                    if (found)
                    {
                        break;
                    }
                }

                // find the column containing the first valid point (xMin)
                found = false;
                for (var col = 0; col <= numCols - 1; col++)
                {
                    for (var row = 0; row <= numRows - 1; row++)
                    {
                        if (Convert.ToDouble(inputGridWrapper.GetValue(col, row)) == noData)
                        {
                            continue;
                        }

                        firstColLoc = col;
                        found = true;
                        break;
                    }

                    if (found)
                    {
                        break;
                    }
                }

                // find the column containing the last valid point (xMax)
                found = false;
                for (var col = numCols - 1; col >= 0; col--)
                {
                    for (var row = 0; row <= numRows - 1; row++)
                    {
                        if (Convert.ToDouble(inputGridWrapper.GetValue(col, row)) == noData)
                        {
                            continue;
                        }

                        lastColLoc = col;
                        found = true;
                        break;
                    }

                    if (found)
                    {
                        break;
                    }
                }

                // if there are no rows or cols in need of trimming, don't trim, just save to the result path!
                if (firstColLoc == 0 && firstRowLoc == 0 && lastColLoc == numCols - 1 && lastRowLoc == numRows - 1)
                {
                    inputGridWrapper.Save(resultGridfilePath);
                    inputGridWrapper.Close();
                    inputGridWrapper.Dispose();
                    return true;
                }

                // now make a new grid with the adjusted dimensions

                // No... bad idea --> rHeader = inputGW.GetHeader();
                var gridHeader = inputGridWrapper.GetHeaderCopy();

                var newColNum = (numCols - firstColLoc) - (numCols - lastColLoc) + 1;
                var newRowNum = (numRows - firstRowLoc) - (numRows - lastRowLoc) + 1;
                gridHeader.NumberCols = newColNum;
                gridHeader.NumberRows = newRowNum;
                var cellSize = gridHeader.dX;
                gridHeader.XllCenter += firstColLoc * cellSize;
                gridHeader.YllCenter += (numRows - 1 - lastRowLoc) * cellSize;
                gridHeader.NodataValue = inputGridWrapper.GetNodataValue();

                // create the result grid
                Grid resultGrid = new GridClass();
                DataManagement.DeleteGrid(ref resultGridfilePath);
                if (!
                    resultGrid.CreateNew(
                    resultGridfilePath,
                    gridHeader,
                    GridDataType.UnknownDataType,
                    inputGridWrapper.GetNodataValue(),
                    true,
                    GridFileType.UseExtension, 
                    null))
                {
                    WriteErrorMessage("Error creating result file while trimming grid: " +
                                      resultGrid.get_ErrorMsg(resultGrid.LastErrorCode));

                    // free unmanaged resources
                    inputGridWrapper.Dispose();
                    //while (Marshal.ReleaseComObject(resultGrid) != 0)
                    //{
                    //}

                    //while (Marshal.ReleaseComObject(gridHeader) != 0)
                    //{
                    //}

                    return false;
                }

                resultGrid.Save(resultGridfilePath, GridFileType.UseExtension, null);
                resultGrid.Close();
                //while (Marshal.ReleaseComObject(resultGrid) != 0)
                //{
                //}

                //while (Marshal.ReleaseComObject(gridHeader) != 0)
                //{
                //}

                using (var resultGridWrapper = new GridWrapper())
                {
                    if (resultGridWrapper.Open(resultGridfilePath, true) == false)
                    {
                        resultGridWrapper.Dispose();
                        inputGridWrapper.Dispose();
                        return false;
                    }

                    // fill in the new grid with values from original grid
                    var rowPosition = firstRowLoc;
                    var rowVals = new float[numCols];
                    var rowCopy = new float[newColNum];
                    for (var row = 0; row < newRowNum; row++)
                    {
                        inputGridWrapper.GetRow(rowPosition, ref rowVals);
                        Array.Copy(rowVals, firstColLoc, rowCopy, 0, newColNum);
                        resultGridWrapper.SetRow(row, ref rowCopy);
                        rowPosition++;
                    }

                    resultGridWrapper.Save(resultGridfilePath);
                } // end of using resultGW, will be disposed of by GC
            } // end of using inputGW, will be disposed of by GC
            return true;
        }

        /// <summary>
        /// Removes excess rows and columns contianing only "NoData" values from the grid's edges.
        /// </summary>
        /// <param name="grid">Grid in need of trimming.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool TrimGrid(ref Grid grid)
        {
            var tempPath = Path.GetTempPath();
            var noData = Convert.ToDouble(grid.Header.NodataValue);
            var numRows = grid.Header.NumberRows;
            var numCols = grid.Header.NumberCols;
            var firstRowLoc = 0;
            var lastRowLoc = numRows - 1;
            var firstColLoc = 0;
            var lastColLoc = numCols - 1;
            var found = false;

            if (numCols <= 0 || numRows <= 0)
            {
                WriteErrorMessage("The grid has 0 rows or columns. Aborting TrimGrid()");
                return false;
            }

            // Find which row contains the first valid Y points (yMax).
            for (var row = 0; row <= numRows - 1; row++)
            {
                for (var col = 0; col <= numCols - 1; col++)
                {
                    if (Convert.ToDouble(grid.get_Value(col, row)) == noData)
                    {
                        continue;
                    }

                    firstRowLoc = row;
                    found = true;
                    break;
                }

                if (found)
                {
                    break;
                }
            }

            // Find which row contains the last valid Y points (yMin)
            found = false;
            for (var row = numRows - 1; row >= 0; row--)
            {
                for (var col = 0; col <= numCols - 1; col++)
                {
                    if (Convert.ToDouble(grid.get_Value(col, row)) == noData)
                    {
                        continue;
                    }

                    lastRowLoc = row;
                    found = true;
                    break;
                }

                if (found)
                {
                    break;
                }
            }

            // find the column containing the first valid point (xMin)
            found = false;
            for (var col = 0; col <= numCols - 1; col++)
            {
                for (var row = 0; row <= numRows - 1; row++)
                {
                    if (Convert.ToDouble(grid.get_Value(col, row)) == noData)
                    {
                        continue;
                    }

                    firstColLoc = col;
                    found = true;
                    break;
                }

                if (found)
                {
                    break;
                }
            }

            // find the column containing the last valid point (xMax)
            found = false;
            for (var col = numCols - 1; col >= 0; col--)
            {
                for (var row = 0; row <= numRows - 1; row++)
                {
                    if (Convert.ToDouble(grid.get_Value(col, row)) == noData)
                    {
                        continue;
                    }

                    lastColLoc = col;
                    found = true;
                    break;
                }

                if (found)
                {
                    break;
                }
            }

            // if there are no rows or cols in need of trimming, don't trim, just save to the result path!
            if (firstColLoc == 0 && firstRowLoc == 0 && lastColLoc == numCols - 1 && lastRowLoc == numRows - 1)
            {
                // no need to change the original grid.
                return true;
            }

            // now make a new grid with the adjusted dimensions
            GridHeader gridHeader = new GridHeaderClass();
            var newColNum = lastColLoc - firstColLoc + 1;
            var newRowNum = lastRowLoc - firstRowLoc + 1;
            gridHeader.NumberCols = newColNum;
            gridHeader.NumberRows = newRowNum;
            var cellSize = grid.Header.dX;
            gridHeader.XllCenter = grid.Header.XllCenter + (firstColLoc * cellSize);
            gridHeader.YllCenter = grid.Header.YllCenter + ((numRows - 1 - lastRowLoc) * cellSize);
            gridHeader.dX = grid.Header.dX;
            gridHeader.dY = grid.Header.dY;
            gridHeader.Projection = grid.Header.Projection;

            // create a new grid
            Grid result = new GridClass();
            var resultGrid = tempPath + "tempGrid.bgd";
            if (File.Exists(resultGrid) == false)
            {
                if (!
                    result.CreateNew(
                    resultGrid,
                    gridHeader,
                    grid.DataType,
                    grid.Header.NodataValue,
                    true,
                    GridFileType.UseExtension,
                    null))
                {
                    WriteErrorMessage("Problem creating the result grid: " + result.get_ErrorMsg(result.LastErrorCode));
                    return false;
                }
            }
            else
            {
                // delete the old grid files of the same name
                var length = resultGrid.IndexOf(".", 0);
                var gridFileName = resultGrid.Substring(0, length);
                File.Delete(resultGrid);
                File.Delete(gridFileName + ".bmp");
                File.Delete(gridFileName + ".bpw");
                File.Delete(gridFileName + ".mwleg");

                if (!result.CreateNew(resultGrid, gridHeader, grid.DataType, grid.Header.NodataValue, true, GridFileType.UseExtension, null))
                {
                    WriteErrorMessage("Problem creating the result grid: " + result.get_ErrorMsg(result.LastErrorCode));
                    return false;
                }
            }

            // fill in the new grid with values from original grid
            var rowPosition = lastRowLoc;
            var colPosition = firstColLoc;
            for (var row = newRowNum - 1; row >= 0; row--)
            {
                for (var col = 0; col <= newColNum - 1; col++)
                {
                    result.set_Value(col, row, grid.get_Value(colPosition, rowPosition));

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
            }

            // assign the new grid to the original reference grid:
            grid = result;
            return true;
        }

        #endregion

        #region ProjToCell()

        /// <summary>
        /// Takes an x/y coordinate and finds what row/col the point exists in within the grid.
        /// </summary>
        /// <param name="x">X value of the coordinate point.</param>
        /// <param name="y">Y value of the coordinate point.</param>
        /// <param name="col">The resulting column.</param>
        /// <param name="row">The resulting row.</param>
        /// <param name="xllCenter">The X value of the center point of the lower-left corner of the grid.</param>
        /// <param name="yllCenter">The Y value of the center point of the lower-left corner of the grid.</param>
        /// <param name="dX">The width of a cell in the grid.</param>
        /// <param name="dY">The height of a cell in the grid.</param>
        /// <param name="numRows">The total number of rows in the grid.</param>
        public static void ProjToCell(double x, double y, out int col, out int row, double xllCenter, double yllCenter, double dX, double dY, int numRows)
        {
            if (dX != 0.0 && dY != 0.0)
            {
            	// cwg 10/10/2011 Fix for #1997
            	// code for col was rounding before division
            	col = (int)(Math.Round((x - xllCenter) / dX));
                row = (int)(numRows - Math.Round((y - yllCenter) / dY) - 1);
            }
            else
            {
                col = -1;
                row = -1;
            }
        }

        #endregion

        #region CellToProj()

        /// <summary>
        /// Finds the center point of a grid cell that corresponds to the row/col provided.
        /// </summary>
        /// <param name="col">The column to be considered.</param>
        /// <param name="row">The row to be considered.</param>
        /// <param name="x">The resulting X value of the point that is in the center of the grid's cell.</param>
        /// <param name="y">The resulting Y value of the point that is in the center of the grid's cell.</param>
        /// <param name="xllCenter">The X value of the center point of the lower-left corner of the grid.</param>
        /// <param name="yllCenter">The Y value of the center point of the lower-left corner of the grid.</param>
        /// <param name="dX">The width of a cell in the grid.</param>
        /// <param name="dY">The height of a cell in the grid.</param>
        /// <param name="numRows">The total number of rows in the grid.</param>
        public static void CellToProj(int col, int row, out double x, out double y, double xllCenter, double yllCenter, double dX, double dY, int numRows)
        {
            x = xllCenter + (col * dX);
            y = yllCenter + ((numRows - row - 1) * dY);
        }

        #endregion

        #region CopyFields()

        /// <summary>
        /// Copies all fields from the inputSF .dbf table to the resultSF table.
        /// </summary>
        /// <param name="inputSF">The shapefile with fields to be copied.</param>
        /// <param name="resultSF">The result shapefile that will inherit the fields.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        /// <remarks>Refactored by Simon Batson to allow renaming of duplicate fields</remarks>
        public static bool CopyFields(ref Shapefile inputSF, ref Shapefile resultSF)
        {
            return CopyFields(ref inputSF, ref resultSF, false);
        }

        /// <summary>
        /// Copies all fields from the inputSF .dbf table to the resultSF table.
        /// </summary>
        /// <param name="inputSF">The shapefile with fields to be copied.</param>
        /// <param name="resultSF">The result shapefile that will inherit the fields.</param>
        /// <param name="renameDuplicates">Instructs function to rename duplicate fields, used when merging two shape files.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool CopyFields(ref Shapefile inputSF, ref Shapefile resultSF, bool renameDuplicates)
        {
            var numFields = inputSF.NumFields;
            for (var i = 0; i <= numFields - 1; i++)
            {
                Field field = new FieldClass { Name = inputSF.get_Field(i).Name, Type = inputSF.get_Field(i).Type };
                if (field.Type == FieldType.STRING_FIELD)
                {
                    field.Width = inputSF.get_Field(i).Width;
                }
                else if (field.Type == FieldType.DOUBLE_FIELD)
                {
                    field.Precision = inputSF.get_Field(i).Precision;
                }

                var fieldIndex = resultSF.NumFields;

                if (!renameDuplicates)
                {
                    if (FindField(field.Name, ref resultSF) > -1)
                    {
                        if (field.Name.Length < 10)
                        {
                            field.Name = field.Name + "1";
                        }
                        else
                        {
                            field.Name = field.Name.Substring(0, 9) + "1";
                        }
                    }
                }

                if (resultSF.EditInsertField(field, ref fieldIndex, null) == false)
                {
                    WriteErrorMessage("Problem inserting field into result DBF table: " +
                                      resultSF.get_ErrorMsg(resultSF.LastErrorCode));
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region FindField()

        /// <summary>
        /// Find the specified field in a shapefile. CDM 1/17/2007 
        /// </summary>
        /// <param name="fieldName">Name of the field</param>
        /// <param name="sf">Shapefile with the field</param>
        /// <returns>Field index</returns>
        public static int FindField(string fieldName, ref Shapefile sf)
        {
            if (sf == null)
            {
                return -1;
            }

            for (var i = 0; i < sf.NumFields; i++)
            {
                if (sf.get_Field(i).Name.ToLower().Trim() == fieldName.ToLower().Trim())
                {
                    return i;
                }
            }

            return -1;
        }

        #endregion

        #region InsertIDs()

        /// <summary>
        /// Inserts the field 'ID' into the .dbf table along with the corresponding
        /// value for each shape in the input shapefile.
        /// </summary>
        /// <param name="sf">The input shapefile.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool DoInsertIDs(ref Shapefile sf)
        {
            // add ID field to the .dbf table
            Field idField = new FieldClass { Name = "MWShapeID", Type = FieldType.INTEGER_FIELD };
            var fieldIndex = 0;
            if (sf.EditInsertField(idField, ref fieldIndex, null) == false)
            {
                WriteErrorMessage("Problem inserting field into .dbf table: " + sf.get_ErrorMsg(sf.LastErrorCode));
                return false;
            }

            // Add id values to the dbf table
            var numIDs = sf.NumShapes;
            for (var i = 0; i <= numIDs - 1; i++)
            {
                if (sf.EditCellValue(0, i, i) == false)
                {
                    WriteErrorMessage("Problem inserting value into .dbf table for shape " + i + ": " +
                                      sf.get_ErrorMsg(sf.LastErrorCode));
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region IsClockwise()

        /// <summary>
        /// Calculates the Area to find if a simple convex or
        /// concave polyon is ordered clockwise or counter-clockwise.
        /// </summary>
        /// <param name="poly">Polygon to check for clockwiseness.</param>
        /// <returns>True if polygon is clockwise, false otherwise.</returns>
        public static bool IsClockwise(ref Shape poly)
        {
            var numPoints = poly.numPoints;
            double area = 0;

            // loop through all edges of the polygon
            for (var i = 0; i <= numPoints - 2; i++)
            {
                var oneX = poly.get_Point(i).x;
                var oneY = poly.get_Point(i).y;
                var twoX = poly.get_Point(i + 1).x;
                var twoY = poly.get_Point(i + 1).y;

                var trapArea = (oneX * twoY) - (twoX * oneY);
                area += trapArea;
            }

            if (area > 0)
            {
                // polygon vertices are ordered counter-clockwise about the normal
                return false;
            }

            return true;
        }

        /// <summary>
        /// Calculates the Area to find if a simple convex or
        /// concave polyon is ordered clockwise or counter-clockwise.
        /// </summary>
        /// <param name="v">Array of vertices from the polygon whose orientation
        /// is being determined.</param>
        /// <returns>True if polygon vertices are oriented clockwise about 
        /// the normal, false otherwise.</returns>
        internal static bool IsClockwise(ref Vertex[] v)
        {
            var numPoints = v.Length;
            double area = 0;

            // loop through all edges of the polygon
            for (var i = 0; i <= numPoints - 2; i++)
            {
                var oneX = v[i].x;
                var oneY = v[i].y;
                var twoX = v[i + 1].x;
                var twoY = v[i + 1].y;

                var trapArea = (oneX * twoY) - (twoX * oneY);
                area += trapArea;
            }

            if (area > 0)
            {
                // if area is positive, then
                // vertices are ordered counter-clockwise about the normal
                return false;
            }

            // vertices are ordered clockwise about the normal
            return true;
        }

        #endregion

        #region internal CalcSiDeterm()

        /// <summary>
        /// Calculates the determinants for line segments with a polygon
        /// to see if any intersections will occur. -- Angela Hillier 10/05
        /// </summary>
        /// <param name="lineSF">A line shapefile of 2pt segments.</param>
        /// <param name="polygon">The polygon to test for intersections with.</param>
        /// <param name="intersectsPerLineSeg">Out parameter: an array of numbers representing how many
        /// intersections will occur for each line segment.</param>
        /// <param name="intersectionPts">Out parameter: an array of points, if the point is != (0,0) then
        /// it represents a valid intersection point at the corresponding location in the polygon.</param>
        /// <param name="polyIntersectLocs">Out paramter: an array of indicies corresponding to where in the polygon
        /// an intersection point is found.</param>
        /// <returns>The total number of intersections found for the line shapefile.</returns>
        /// <remarks>Ideas for this algorithm were taken by a similar function found in 
        /// John Theal's 2002 class: CPolygonClip. Which was found here http://www.codeproject.com/cpp/2dpolyclip.asp. 
        /// It has been changed to work with MapWinGIS shapes, as well as to calculate the accurate number of 
        /// intersection points for finite segments (original version treated the line segments as rays).</remarks>
        internal static int CalcSiDeterm(
            ref Shapefile lineSF,
            ref Shape polygon,
            out int[] intersectsPerLineSeg,
            out Point[][] intersectionPts,
            out int[][] polyIntersectLocs)
        {
            var numSignChanges = 0; // tracks number of determinant sign changes
            var numLines = lineSF.NumShapes;
            var numVerticies = polygon.numPoints;
            var detSigns = new int[numLines][];
            var signChanges = new bool[numLines][]; // keeps track of where sign changes occur
            var changeLocations = new int[numLines][];
            var intersectsPerLine = new int[numLines];
            var intersectPts = new Point[numLines][];

            for (var lineNo = 0; lineNo <= numLines - 1; lineNo++)
            {
                var line = lineSF.get_Shape(lineNo);
                var numChangesPerLine = 0;
                detSigns[lineNo] = new int[numVerticies];
                signChanges[lineNo] = new bool[numVerticies];
                intersectPts[lineNo] = new Point[numVerticies];
                changeLocations[lineNo] = new int[numVerticies];

                for (var vertNo = 0; vertNo <= numVerticies - 1; vertNo++)
                {
                    intersectPts[lineNo][vertNo] = new PointClass();

                    // Calculate the determinant (3x3 square matrix)
                    var sI = TurboDeterm(
                        polygon.get_Point(vertNo).x,
                        line.get_Point(0).x,
                        line.get_Point(1).x,
                        polygon.get_Point(vertNo).y,
                        line.get_Point(0).y,
                        line.get_Point(1).y);

                    // Check the determinant result
                    switch (vertNo)
                    {
                        case 0:
                            if (sI == 0)
                            {
                                detSigns[lineNo][vertNo] = 0; // we have hit a vertex
                            }
                            else if (sI > 0)
                            {
                                detSigns[lineNo][vertNo] = 1; // +'ve
                            }
                            else if (sI < 0)
                            {
                                detSigns[lineNo][vertNo] = -1; // -'ve
                            }

                            signChanges[lineNo][0] = false; // First element will NEVER be a sign change
                            break;
                        default:
                            if (sI == 0)
                            {
                                detSigns[lineNo][vertNo] = 0;
                            }
                            else if (sI > 0)
                            {
                                detSigns[lineNo][vertNo] = 1;
                            }
                            else if (sI < 0)
                            {
                                detSigns[lineNo][vertNo] = -1;
                            }

                            // Check for sign change
                            if (detSigns[lineNo][vertNo - 1] != detSigns[lineNo][vertNo])
                            {
                                // calculate the actual intercept point
                                var polyTestLine = new Line(polygon.get_Point(vertNo - 1), polygon.get_Point(vertNo));
                                var lineSeg = new Line(line.get_Point(0), line.get_Point(1));
                                Point intersectPt;
                                var validIntersect = LinesIntersect2D(
                                    lineSeg,
                                    false,
                                    polyTestLine,
                                    false,
                                    out intersectPt);

                                if (validIntersect)
                                {
                                    signChanges[lineNo][vertNo] = true;
                                    numSignChanges += 1;
                                    numChangesPerLine += 1;

                                    intersectsPerLine[lineNo] = numChangesPerLine;

                                    // we want to store the valid intersect pts at the
                                    // beginning of the array so we don't have to search for them
                                    intersectPts[lineNo][numChangesPerLine - 1] = intersectPt;

                                    // keep track of where the intersect occurs in reference to polygon
                                    // intersect pt occurs between vertNo-1 and vertNo
                                    changeLocations[lineNo][numChangesPerLine - 1] = vertNo;
                                }
                                else
                                {
                                    signChanges[lineNo][vertNo] = false;
                                }
                            }
                            else
                            {
                                signChanges[lineNo][vertNo] = false;
                            }

                            break;
                    } // end of switch
                }
            }

            polyIntersectLocs = changeLocations;
            intersectionPts = intersectPts;
            intersectsPerLineSeg = intersectsPerLine;
            return numSignChanges;
        }

        /// <summary>
        /// Calculates the determinator
        /// </summary>
        /// <param name="line">The line shape</param>
        /// <param name="polygon">The polygon shape</param>
        /// <param name="intersectsPerLineSeg">The intersections per line segment</param>
        /// <param name="intersectionPts">The intersection points</param>
        /// <param name="polyIntersectLocs">The locations of the intersections on the polygon</param>
        /// <returns>The number of intersections</returns>
        internal static int CalcSiDeterm(
            ref Shape line,
            ref Shape polygon,
            out int[] intersectsPerLineSeg,
            out Point[][] intersectionPts,
            out int[][] polyIntersectLocs)
        {
            var numSignChanges = 0; // tracks number of determinant sign changes
            const int NumLines = 1;
            var numVerticies = polygon.numPoints;
            var detSigns = new int[NumLines][];
            var signChanges = new bool[NumLines][]; // keeps track of where sign changes occur
            var changeLocations = new int[NumLines][];
            var intersectsPerLine = new int[NumLines];
            var intersectPts = new Point[NumLines][];

            for (var lineNo = 0; lineNo <= NumLines - 1; lineNo++)
            {
                var numChangesPerLine = 0;
                detSigns[lineNo] = new int[numVerticies];
                signChanges[lineNo] = new bool[numVerticies];
                intersectPts[lineNo] = new Point[numVerticies];
                changeLocations[lineNo] = new int[numVerticies];

                for (var vertNo = 0; vertNo <= numVerticies - 1; vertNo++)
                {
                    intersectPts[lineNo][vertNo] = new PointClass();

                    // Calculate the determinant (3x3 square matrix)
                    var sI = TurboDeterm(
                        polygon.get_Point(vertNo).x,
                        line.get_Point(0).x,
                        line.get_Point(1).x,
                        polygon.get_Point(vertNo).y,
                        line.get_Point(0).y,
                        line.get_Point(1).y);

                    // Check the determinant result
                    switch (vertNo)
                    {
                        case 0:
                            if (sI == 0)
                            {
                                detSigns[lineNo][vertNo] = 0; // we have hit a vertex
                            }
                            else if (sI > 0)
                            {
                                detSigns[lineNo][vertNo] = 1; // +'ve
                            }
                            else if (sI < 0)
                            {
                                detSigns[lineNo][vertNo] = -1; // -'ve
                            }

                            signChanges[lineNo][0] = false; // First element will NEVER be a sign change
                            break;
                        default:
                            if (sI == 0)
                            {
                                detSigns[lineNo][vertNo] = 0;
                            }
                            else if (sI > 0)
                            {
                                detSigns[lineNo][vertNo] = 1;
                            }
                            else if (sI < 0)
                            {
                                detSigns[lineNo][vertNo] = -1;
                            }

                            // Check for sign change
                            if (detSigns[lineNo][vertNo - 1] != detSigns[lineNo][vertNo])
                            {
                                // calculate the actual intercept point
                                var polyTestLine = new Line(polygon.get_Point(vertNo - 1), polygon.get_Point(vertNo));
                                var lineSeg = new Line(line.get_Point(0), line.get_Point(1));
                                Point intersectPt;
                                var validIntersect = LinesIntersect2D(
                                    lineSeg,
                                    false,
                                    polyTestLine,
                                    false,
                                    out intersectPt);

                                if (validIntersect)
                                {
                                    signChanges[lineNo][vertNo] = true;
                                    numSignChanges += 1;
                                    numChangesPerLine += 1;
                                    intersectsPerLine[lineNo] = numChangesPerLine;

                                    // we want to store the valid intersect pts at the
                                    // beginning of the array so we don't have to search for them
                                    intersectPts[lineNo][numChangesPerLine - 1] = intersectPt;

                                    // keep track of where the intersect occurs in reference to polygon
                                    // intersect pt occurs between vertNo-1 and vertNo
                                    changeLocations[lineNo][numChangesPerLine - 1] = vertNo;
                                }
                                else
                                {
                                    signChanges[lineNo][vertNo] = false;
                                }
                            }
                            else
                            {
                                signChanges[lineNo][vertNo] = false;
                            }

                            break;
                    } // end of switch
                }
            }

            polyIntersectLocs = changeLocations;
            intersectionPts = intersectPts;
            intersectsPerLineSeg = intersectsPerLine;
            return numSignChanges;
        }

        /// <summary>
        /// Calculate of the line and the polygon intersect
        /// </summary>
        /// <param name="lineSF">The line shapefile</param>
        /// <param name="polygon">the polygon</param>
        /// <returns>The number of intersections</returns>
        /// <remarks>Overloaded version</remarks>
        internal static int CalcSiDeterm(ref Shapefile lineSF, ref Shape polygon)
        {
            var numSignChanges = 0; // tracks number of determinant sign changes
            var numLines = lineSF.NumShapes;
            var numVerticies = polygon.numPoints;
            var detSigns = new int[numLines][];
            var signChanges = new bool[numLines][]; // keeps track of where sign changes occur
            var changeLocations = new int[numLines][];
            var intersectsPerLine = new int[numLines];
            var intersectPts = new Point[numLines][];

            for (var lineNo = 0; lineNo <= numLines - 1; lineNo++)
            {
                var line = lineSF.get_Shape(lineNo);
                var numChangesPerLine = 0;
                detSigns[lineNo] = new int[numVerticies];
                signChanges[lineNo] = new bool[numVerticies];
                intersectPts[lineNo] = new Point[numVerticies];
                changeLocations[lineNo] = new int[numVerticies];

                for (var vertNo = 0; vertNo <= numVerticies - 1; vertNo++)
                {
                    intersectPts[lineNo][vertNo] = new PointClass();

                    // Calculate the determinant (3x3 square matrix)
                    var sI = TurboDeterm(
                        polygon.get_Point(vertNo).x,
                        line.get_Point(0).x,
                        line.get_Point(1).x,
                        polygon.get_Point(vertNo).y,
                        line.get_Point(0).y,
                        line.get_Point(1).y);

                    // Check the determinant result
                    switch (vertNo)
                    {
                        case 0:
                            if (sI == 0)
                            {
                                detSigns[lineNo][vertNo] = 0; // we have hit a vertex
                            }
                            else if (sI > 0)
                            {
                                detSigns[lineNo][vertNo] = 1; // +'ve
                            }
                            else if (sI < 0)
                            {
                                detSigns[lineNo][vertNo] = -1; // -'ve
                            }

                            signChanges[lineNo][0] = false; // First element will NEVER be a sign change
                            break;
                        default:
                            if (sI == 0)
                            {
                                detSigns[lineNo][vertNo] = 0;
                            }
                            else if (sI > 0)
                            {
                                detSigns[lineNo][vertNo] = 1;
                            }
                            else if (sI < 0)
                            {
                                detSigns[lineNo][vertNo] = -1;
                            }

                            // Check for sign change
                            if (detSigns[lineNo][vertNo - 1] != detSigns[lineNo][vertNo])
                            {
                                // calculate the actual intercept point
                                var polyTestLine = new Line(polygon.get_Point(vertNo - 1), polygon.get_Point(vertNo));
                                var lineSeg = new Line(line.get_Point(0), line.get_Point(1));
                                Point intersectPt;
                                var validIntersect = LinesIntersect2D(
                                    lineSeg,
                                    false,
                                    polyTestLine,
                                    false,
                                    out intersectPt);

                                if (validIntersect)
                                {
                                    signChanges[lineNo][vertNo] = true;
                                    numSignChanges += 1;
                                    numChangesPerLine += 1;
                                    intersectsPerLine[lineNo] = numChangesPerLine;

                                    // we want to store the valid intersect pts at the
                                    // beginning of the array so we don't have to search for them
                                    intersectPts[lineNo][numChangesPerLine - 1] = intersectPt;

                                    // keep track of where the intersect occurs in reference to polygon
                                    // intersect pt occurs between vertNo-1 and vertNo
                                    changeLocations[lineNo][numChangesPerLine - 1] = vertNo;
                                }
                                else
                                {
                                    signChanges[lineNo][vertNo] = false;
                                }
                            }
                            else
                            {
                                signChanges[lineNo][vertNo] = false;
                            }

                            break;
                    } // end of switch
                }
            }

            return numSignChanges;
        }

        #endregion

        /// <summary>
        /// Write the error message to several locations
        /// </summary>
        /// <param name="errorMsg">The error message</param>
        internal static void WriteErrorMessage(string errorMsg)
        {
            Debug.WriteLine(errorMsg);
            Error.SetErrorMsg(errorMsg);
            Logger.Dbg(errorMsg);
        }

        #region private PerpProduct() -- used by LinesIntersect2D()

        /// <summary>
        /// Computes the perpendicular product of two 2D vectors.
        /// </summary>
        /// <param name="v1">The first vector</param>
        /// <param name="v2">The second vector</param>
        /// <returns>Perpedicular product; if == 0, v1 and v2 are parallel.</returns>
        private static double PerpProduct(IVector v1, IVector v2)
        {
            return (v1.i * v2.j) - (v1.j * v2.i);
        }

        #endregion

        #region private TurboDeterm() -- used by CalcSiDeterm()

        /// <summary>
        /// Calculates the determinant of a 3X3 matrix, where the first two rows
        /// represent the x,y values of two lines, and the third row is (1 1 1).
        /// </summary>
        /// <param name="elem11">The first element of the first row in the matrix.</param>
        /// <param name="elem12">The second element of the first row in the matrix.</param>
        /// <param name="elem13">The third element of the first row in the matrix.</param>
        /// <param name="elem21">The first element of the second row in the matrix.</param>
        /// <param name="elem22">The second element of the second row in the matrix.</param>
        /// <param name="elem23">The third element of the second row in the matrix.</param>
        /// <returns>The determinant of the matrix.</returns>
        /// <remarks>Original function found in John Theal's 2002 class: CPolygonClip. 
        /// http://www.codeproject.com/cpp/2dpolyclip.asp</remarks>
        private static double TurboDeterm(double elem11, double elem12, double elem13, double elem21, double elem22, double elem23)
        {
            // The third row of the 3x3 matrix is (1,1,1)
            return (elem11 * (elem22 - elem23))
                   - (elem12 * (elem21 - elem23))
                   + (elem13 * (elem21 - elem22));
        }

        #endregion

        #region private SortPointsArray()

        /// <summary>
        /// Given a reference point to the line, and an array of points that
        /// lie along the line, this method sorts the array of points from the point
        /// closest to the reference pt to the pt farthest away.
        /// </summary>
        /// <param name="startPt">Point in line segment used as reference.</param>
        /// <param name="intersectPts">Array of points that lie on the same line as startPt.</param>
        private static void SortPointsArray(ref Point startPt, ref Point[] intersectPts)
        {
            double dist1;
            var numIntersectPts = intersectPts.Length;

            // if 0 or 1 the points don't need to be sorted
            if (numIntersectPts == 2)
            {
                // do a brute sort
                // just compare distances of each pt to the start pt.
                dist1 = PtDistance(ref startPt, ref intersectPts[0]);
                var dist2 = PtDistance(ref startPt, ref intersectPts[1]);
                if (dist1 > dist2)
                {
                    // need to swap locations
                    var tempPt = intersectPts[0];
                    intersectPts[0] = intersectPts[1];
                    intersectPts[1] = tempPt;
                }
            }
            else if (numIntersectPts > 2 /*&& numintersectPts <= 10*/)
            {
                // use insertion sort for small arrays
                for (var i = 0; i <= numIntersectPts - 1; i++)
                {
                    var compPt1 = intersectPts[i];
                    dist1 = PtDistance(ref startPt, ref compPt1);
                    var c = i;
                    Point compPt2;
                    if (c != 0)
                    {
                        compPt2 = intersectPts[c - 1];
                    }
                    else
                    {
                        compPt2 = intersectPts[0];
                    }

                    while (c > 0 && PtDistance(ref startPt, ref compPt2) > dist1)
                    {
                        intersectPts[c] = intersectPts[c - 1];
                        c--;
                        if (c != 0)
                        {
                            compPt2 = intersectPts[c - 1];
                        }
                    }

                    if (c != i)
                    {
                        intersectPts[c] = compPt1;
                    }
                }
            }

            // else if(numIntersectPts > 10)
            // {
            // TO DO: write a quick-sort function to aid in time
            // haven't done this because it is rare to have
            // a large number of intersect pts for a small line segment
            // quick-sort performs poorly on small lists, that's why insertion
            // sort is used above.
            // }
        }

        #endregion

        #region private SortIntersectAndLocationArrays()

        /// <summary>
        /// Given a reference point to the line, and an array of points that
        /// lie along the line, this method sorts the array of points from the point
        /// closest to the reference pt to the pt farthest away. It also sorts the corresponding
        /// polygon location array so that the indicies refer to the correct intersection point.
        /// </summary>
        /// <param name="startPt">Point in line segment used as reference.</param>
        /// <param name="intersectPts">Array of points that lie on the same line as startPt.</param>
        /// <param name="polyLoc">Array indexing where in polygon an intersect occurs.</param>
        private static void SortIntersectAndLocationArrays(ref Point startPt, ref Point[] intersectPts, ref int[] polyLoc)
        {
            var numIntersectPts = intersectPts.Length;

            // if 0 or 1 the points don't need to be sorted
            if (numIntersectPts == 2)
            {
                // do a brute sort
                // just compare distances of each pt to the start pt.
                var dist1 = PtDistance(ref startPt, ref intersectPts[0]);
                var dist2 = PtDistance(ref startPt, ref intersectPts[1]);
                if (dist1 > dist2)
                {
                    // need to swap locations
                    var tempPt = intersectPts[0];
                    intersectPts[0] = intersectPts[1];
                    intersectPts[1] = tempPt;

                    // move poly location so it corresponds to correct intersect point
                    var tempLoc = polyLoc[0];
                    polyLoc[0] = polyLoc[1];
                    polyLoc[1] = tempLoc;
                }
            }
            else if (numIntersectPts > 2 /*&& numintersectPts <= 10*/)
            {
                // use insertion sort for small arrays
                for (var i = 0; i <= numIntersectPts - 1; i++)
                {
                    var compPt1 = intersectPts[i];
                    var tempLoc1 = polyLoc[i];
                    PtDistance(ref startPt, ref compPt1);
                    var c = i;

                    /*
                     * PM: This has no meaning?
                    Point compPt2;
                    if (c != 0)
                    {
                        compPt2 = intersectPts[c - 1];
                    }
                    else
                    {
                        compPt2 = intersectPts[0];
                    }
                    */

                    intersectPts[c] = intersectPts[c - 1];
                    polyLoc[c] = polyLoc[c - 1];
                    c--;

                    /*
                     * PM: This has no meaning?
                    if (c != 0)
                    {
                        compPt2 = intersectPts[c - 1];
                    }
                    */

                    if (c != i)
                    {
                        intersectPts[c] = compPt1;
                        polyLoc[c] = tempLoc1;
                    }
                }
            }

            // else if(numIntersectPts > 10)
            // {
            // TO DO: write a quick-sort function to aid in time
            // haven't done this because it is rare to have
            // a large number of intersect pts for a small line segment
            // quick-sort performs poorly on small lists, that's why insertion
            // sort is used above.
            // }
        }

        #endregion

        #region Vertex struct

        /// <summary>
        /// A coordinate point (x, y)
        /// </summary>
        public struct Vertex
        {
            // ReSharper disable InconsistentNaming

            /// <summary>X coordinate</summary>
            [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter", Justification = "Old public variable, must retain for backward compatibility")]
            public double x;

            /// <summary>Y coordinate</summary>
            [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter", Justification = "Old public variable, must retain for backward compatibility")]
            public double y;

            // ReSharper restore InconsistentNaming

            /// <summary>
            /// Initializes a new instance of the Vertex struct.
            /// </summary>
            /// <param name="x">X coordinate</param>
            /// <param name="y">Y coordinate</param>
            public Vertex(double x, double y)
            {
                this.x = x;
                this.y = y;
            }

            /// <summary>
            /// Initializes a new instance of the Vertex struct.
            /// </summary>
            /// <param name="pt">The point to use</param>
            public Vertex(ref Point pt)
            {
                this.x = pt.x;
                this.y = pt.y;
            }
        }

        #endregion

        #region Line struct

        /// <summary>
        /// Defines a 2pt line segment that begins at point p0 and ends at point p1.
        /// </summary>
        public struct Line
        {
            // ReSharper disable InconsistentNaming

            /// <summary>The start point</summary>
            [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter", Justification = "Old public variable, must retain for backward compatibility")]
            public Point p0;

            /// <summary>The end point</summary>
            [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1307:AccessibleFieldsMustBeginWithUpperCaseLetter", Justification = "Old public variable, must retain for backward compatibility")]
            public Point p1;

            // ReSharper restore InconsistentNaming

            /// <summary>
            /// Initializes a new instance of the Line struct.
            /// </summary>
            /// <param name="initialPoint">The start point</param>
            /// <param name="terminalPoint">The end point</param>
            public Line(Point initialPoint, Point terminalPoint)
            {
                this.p0 = new PointClass();
                this.p1 = new PointClass();
                this.p0 = initialPoint;
                this.p1 = terminalPoint;
            }
        }

        #endregion
    }
}