// ********************************************************************************************************
// <copyright file="PointExporter.cs" company="MapWindow.org">
//     Copyright (c) MapWindow Development team. All rights reserved.
// </copyright>
// Description: Exports an array of [x,y,z] coordinates to a shapefile or a text file
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
// 01/22/08 - Jiri Kadlec - Provided initial implementation
// 12/21/10 - Paul Meems  - Made this class StyleCop and ReSharper compliant and cleaned up the code
// ********************************************************************************************************

namespace MapWinGeoProc
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using MapWinGIS;

    /// <summary>
    /// This class converts an array of [x,y,z] coordinates to a shapefile or text file
    /// </summary>
    public class PointExporter
    {
        /// <summary>The list of points as coordinates</summary>
        private readonly List<Coordinates> points;

        /// <summary>
        /// Initializes a new instance of the PointExporter class.
        /// </summary>
        /// <param name="points">List of coordinates</param>
        public PointExporter(List<Coordinates> points)
        {
            this.points = points;
        }

        /// <summary>
        /// Initializes a new instance of the PointExporter class.
        /// </summary>
        /// <param name="points">Enumerable of coordinates</param>
        public PointExporter(IEnumerable<Coordinates> points)
        {
            this.points = new List<Coordinates>(points);
        }

        /// <summary>
        /// Initializes a new instance of the PointExporter class.
        /// </summary>
        /// <param name="points">The points to export</param>
        public PointExporter(IList<Point> points)
        {
            var numPoints = points.Count;
            this.points = new List<Coordinates>(numPoints);
            for (var i = 0; i < numPoints; ++i)
            {
                this.points[i].X = points[i].x;
                this.points[i].Y = points[i].y;
                this.points[i].Z = points[i].Z;
            }
        }
        
        /// <summary>
        /// Converts an array of x,y,z points to a text file.
        /// The separator can be specified (use comma or semicolon for CSV files)
        /// </summary>
        /// <param name="textFileName">name of output text file</param>
        /// <param name="separator">separator character (" ", "," ";" etc.)</param>
        public void ToTextFile(string textFileName, string separator)
        {
            using (var w = new System.IO.StreamWriter(textFileName))
            {
                w.WriteLine("X" + separator + "Y" + separator + "Z");
                foreach (var pt in this.points)
                {
                    w.WriteLine(PointToText(pt, separator));
                }

                w.Flush();
            }
        }

        /// <summary>
        /// Converts a list of 3d-points to a point shapefile with z-value field.
        /// This function creates a new shapefile. The shapefile has two fields:
        /// a 'MWShapeId' field and a field which contains the z-value.
        /// </summary>
        /// <param name="shpFileName">Name of the resulting point shapefile</param>
        /// <param name="fieldNameZ">Name of the z-field in the shapefile</param>
        public void ToShapefile(string shpFileName, string fieldNameZ)
        {
            var newSF = new Shapefile();
            try
            {
                const int NumDecimals = 3;
                var fieldIndices = new Hashtable();

                const ShpfileType Sftype = ShpfileType.SHP_POINT;
                var fldIdex = 0;

                // if shapefile exists - open it and clear all shapes
                if (System.IO.File.Exists(shpFileName))
                {
                    newSF.Open(shpFileName, null);
                    newSF.StartEditingShapes(true, null);
                    newSF.EditClear();
                }
                else 
                {
                    // Create a new shapefile
                    if (!newSF.CreateNewWithShapeID(shpFileName, Sftype))
                    {
                        throw new InvalidOperationException("Error creating shapefile " +
                                                            newSF.get_ErrorMsg(newSF.LastErrorCode));
                    }

                    newSF.StartEditingShapes(true, null);
                }

                // Check existing fields:
                for (var i = 0; i < newSF.NumFields; ++i)
                {
                    var fl = newSF.get_Field(i);
                    if (fl.Name == "MWShapeID")
                    {
                        fieldIndices.Add("MWShapeID", i);
                    }

                    if (fl.Name == fieldNameZ)
                    {
                        fieldIndices.Add(fieldNameZ, i);
                    }
                }

                // Add the fields:
                if (!fieldIndices.ContainsKey("MWShapeID"))
                {
                    // First an ID field
                    var idFld = new Field { Name = "MWShapeID", Type = FieldType.INTEGER_FIELD, Width = 10 };
                    fldIdex = newSF.NumFields;
                    
                    if (!newSF.EditInsertField(idFld, ref fldIdex, null))
                    {
                        throw new InvalidOperationException("Error inserting field " + 
                            newSF.get_ErrorMsg(newSF.LastErrorCode));
                    }

                    fieldIndices.Add("MWShapeID", fldIdex);
                }

                if (!fieldIndices.ContainsKey(fieldNameZ))
                {
                    // Second add a Z-field
                    var fieldZ = new Field { Name = "Z", Type = FieldType.DOUBLE_FIELD, Width = 10, Precision = NumDecimals };
                    fldIdex = newSF.NumFields;

                    if (!newSF.EditInsertField(fieldZ, ref fldIdex, null))
                    {
                        throw new InvalidOperationException("Error inserting field " + 
                            newSF.get_ErrorMsg(newSF.LastErrorCode));
                    }

                    fieldIndices.Add("Z", fldIdex);
                }

                // Create and add the shape:
                foreach (var pt in this.points)
                {
                    // First, add a point shape (geometry)
                    var newShp = new Shape();
                    if (!newShp.Create(ShpfileType.SHP_POINT))
                    {
                        throw new InvalidOperationException("Error creating shape " +
                            newShp.get_ErrorMsg(newShp.LastErrorCode));
                    }

                    var newPt = new Point { x = pt.X, y = pt.Y, Z = pt.Z };
                    var pointIndex = 0;
                    if (!newShp.InsertPoint(newPt, ref pointIndex))
                    {
                        throw new InvalidOperationException("Error insert point " +
                            newShp.get_ErrorMsg(newShp.LastErrorCode));
                    }

                    var shpIdx = newSF.NumShapes;
                    if (!newSF.EditInsertShape(newShp, ref shpIdx))
                    {
                        throw new InvalidOperationException("Error EditInsertShape " +
                            newSF.get_ErrorMsg(newSF.LastErrorCode));
                    }

                    // Second add the z-value
                    if (!newSF.EditCellValue(fldIdex, shpIdx, Math.Round(pt.Z, NumDecimals)))
                    {
                        throw new InvalidOperationException("Error EditCellValue " +
                            newSF.get_ErrorMsg(newSF.LastErrorCode));
                    }
                }
            }
            finally
            {
                // Finally stop editing and close the shapefile
                if (!newSF.StopEditingShapes(true, true, null))
                {
                    throw new InvalidOperationException("Error in StopEditingShapes" +
                            newSF.get_ErrorMsg(newSF.LastErrorCode));
                }

                if (!newSF.Close())
                {
                    throw new InvalidOperationException("Error closing shapefile " + 
                            newSF.get_ErrorMsg(newSF.LastErrorCode));
                }
            }
        }

        /// <summary>
        /// Converts an x, y, z point to a formatted text string
        /// </summary>
        /// <param name="point">An [x,y,z] point</param>
        /// <param name="separator">the separator character (typically space, comma, semicolon)</param>
        /// <returns>A formatted string with coordinates divided by the separator character</returns>
        private static string PointToText(Coordinates point, string separator)
        {
            return string.Format(
                "{0}{1}{2}{3}{4}", 
                point.X.ToString("0.00", CultureInfo.InvariantCulture), 
                separator,
                point.Y.ToString("0.00", CultureInfo.InvariantCulture),
                separator,
                point.Z.ToString("0.00", CultureInfo.InvariantCulture));
        }
    }
}
