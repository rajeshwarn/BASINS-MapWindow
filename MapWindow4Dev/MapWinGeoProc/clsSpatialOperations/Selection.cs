// ********************************************************************************************************
// <copyright file="Selection.cs" company="MapWindow.org">
//     Copyright (c) MapWindow Development team. All rights reserved.
// </copyright>
// Description: Internal class, provides methods for selecting lines/points/polygons that intersect with any given polygon.
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
// 12/06/05 ah - Angela Hillier - added selection methods 							
// 07/04/07 sb - Simon Batson   - added ExportShapesWithPolygons to perform Identity processing.
// 12/29/10 pm - Paul Meems     - Start making this class StyleCop and ReSharper compliant
// ********************************************************************************************************

namespace MapWinGeoProc
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using MapWindow.Interfaces.Geometries;
    using NTS.Topology.Geometries;
    using MapWinGIS;

    /// <summary>
    /// Provides functions for selecting lines/points/polygons from
    /// a shapefile that intersect with any given polygon.
    /// </summary>
    public class Selection
    {
        private static string gErrorMsg = "";

        #region ExportSelectedShapes
        // Chris Michaelis August 15 2006
        /// <summary>
        /// Exports the shapes that are selected in the MapWindow view to a new shapefile.
        /// </summary>
        /// <param name="mapWin">A reference to the running MapWindow.</param>
        /// <param name="exportToSFPath">The full path to where the result shapefile should be saved.</param>
        /// <returns>False if an error occurs, true otherwise.</returns>
        public static bool ExportSelectedMWViewShapes(MapWindow.Interfaces.IMapWin mapWin, string exportToSFPath)
        {
            return ExportSelectedMWViewShapes(mapWin, exportToSFPath, false);
        }

        /// <summary>
        /// Exports the shapes that are selected in the MapWindow view to a new shapefile.
        /// </summary>
        /// <param name="mapWin">A reference to the running MapWindow.</param>
        /// <param name="exportToSFPath">The full path to where the result shapefile should be saved.</param>
        /// <param name="addToMap">Indicates that the output should be added to the map view immediately.</param>
        /// <returns>False if an error occurs, true otherwise.</returns>
        public static bool ExportSelectedMWViewShapes(MapWindow.Interfaces.IMapWin mapWin, string exportToSFPath, bool addToMap)
        {
            MapWinUtility.Logger.Dbg("ExportSelectedMWViewShapes(MapWin: IMapWin,\n" +
                                     "                           ExportToSFPath: " + exportToSFPath + ",\n" +
                                     "                           AddToMap: " + addToMap + ")");

            if (mapWin.Layers.NumLayers == 0)
            {
                gErrorMsg = "Please select a layer first.";
                Error.SetErrorMsg(gErrorMsg);
                MapWinUtility.Logger.Dbg(gErrorMsg);
                return false;
            }

            if (mapWin.View.SelectedShapes.NumSelected == 0)
            {
                gErrorMsg = "There are no selected features to export. Please select a feature first.";
                Error.SetErrorMsg(gErrorMsg);
                MapWinUtility.Logger.Dbg(gErrorMsg);
                return false;
            }

            var sf = new Shapefile();
            var tollSF = new Shapefile();
            var fld = new Field();
            var status = sf.Open(mapWin.Layers[mapWin.Layers.CurrentLayer].FileName, null);
            
            if (!status)
            {
                gErrorMsg = sf.get_ErrorMsg(sf.LastErrorCode);
                Error.SetErrorMsg(gErrorMsg);
                MapWinUtility.Logger.Dbg(gErrorMsg);
                return false;
            }

            if (System.IO.File.Exists(exportToSFPath))
            {
                try
                {
                    DataManagement.DeleteShapefile(ref exportToSFPath);
                }
                catch
                {
                    gErrorMsg = "The destination file already exists, but could not be deleted. Please check to make sure the file isn't in use.";
                    Error.SetErrorMsg(gErrorMsg);
                    MapWinUtility.Logger.Dbg(gErrorMsg);
                    return false;
                }
            }

            status = tollSF.CreateNew(exportToSFPath, sf.ShapefileType);

            if (status == false)
            {
                gErrorMsg = tollSF.get_ErrorMsg(tollSF.LastErrorCode);
                Error.SetErrorMsg(gErrorMsg);
                MapWinUtility.Logger.Dbg(gErrorMsg);
                return false;
            }

            try
            {
                tollSF.Projection = sf.Projection;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

            status = tollSF.StartEditingShapes(true, null);
            if (status == false)
            {
                gErrorMsg = tollSF.get_ErrorMsg(tollSF.LastErrorCode);
                Error.SetErrorMsg(gErrorMsg);
                MapWinUtility.Logger.Dbg(gErrorMsg);
                return false;
            }

            fld.Name = "MWShapeID";
            fld.Type = FieldType.INTEGER_FIELD;
            fld.Width = 12;
            var segments = 0;

            for (var j = 0; j <= sf.NumFields - 1; j++)
            {
                tollSF.EditInsertField(sf.get_Field(j), ref j, null);
            }

            mapWin.View.MapCursor = tkCursor.crsrWait;
            try
            {
                MapWindow.Interfaces.SelectInfo info = mapWin.View.SelectedShapes;
                for (var i = 0; i <= info.NumSelected - 1; i++)
                {
                    var seg = sf.get_Shape(info[i].ShapeIndex);
                    status = tollSF.EditInsertShape(seg, ref segments);
                    if (status == false)
                    {
                        gErrorMsg = tollSF.get_ErrorMsg(tollSF.LastErrorCode);
                        Error.SetErrorMsg(gErrorMsg);
                        MapWinUtility.Logger.Dbg(gErrorMsg);
                        return false;
                    }

                    for (var h = 0; h <= sf.NumFields - 1; h++)
                    {
                        tollSF.EditCellValue(h, i, sf.get_CellValue(h, info[i].ShapeIndex));
                    }

                    segments = segments + 1;
                }

                sf.Close();
                tollSF.StopEditingShapes(true, true, null);
            }
            catch (Exception ex)
            {
                gErrorMsg = ex.Message;
                Error.SetErrorMsg(gErrorMsg);
                MapWinUtility.Logger.Dbg(gErrorMsg);
            }
            mapWin.View.MapCursor = tkCursor.crsrArrow;
            tollSF.Close();
            if (addToMap)
            {
                mapWin.View.LockMap();
                var thelayer = mapWin.Layers.Add(exportToSFPath, System.IO.Path.GetFileNameWithoutExtension(exportToSFPath), true);
                thelayer.ClearLabels();
                mapWin.View.UnlockMap();
            }
            MapWinUtility.Logger.Dbg("Finished ExportSelectedMWViewShapes");
            return true;
        }

        /// <summary>
        /// This is used by the Identity process to export all shapes intesecting all polygons passed in.
        /// Each identity polygon is used to select and clip input shapes. The new clipped shape is written
        /// to the result shapefile. The attribute fields from both input and identity shapefiles are copied
        /// to the result shapefile.
        /// This process uses a QuadTree index to speed up the selection of overlapping geometries.
        /// </summary>
        /// <param name="inputSF">The shapefile, of any geometry type, to be clipped and exported.</param>
        /// <param name="identitySF">The polygon shapefile used to clip the inputSF.</param>
        /// <param name="resultSF">The result shapefile that will contain the results.</param>
        /// <returns>False if an error occurs, true otherwise.</returns>
        public static bool ExportShapesWithPolygons(ref Shapefile inputSF, ref Shapefile identitySF, ref Shapefile resultSF)
        {
            try
            {
                // Boundary intersection test variables

                // Build Quadtree index for inputSF
                var myQuadTree = new NTS.Topology.Index.Quadtree.Quadtree();

                Shape currGeom;
                for (var i = 0; i < inputSF.NumShapes; i++)
                {
                    currGeom = inputSF.get_Shape(i);
                    double xMin1;
                    double xMax1;
                    double yMin1;
                    double yMax1;
                    double zMin1;
                    double zMax1;
                    currGeom.Extents.GetBounds(out xMin1, out yMin1, out zMin1, out xMax1, out yMax1, out zMax1);
                    var myItemEnv = new Envelope(xMin1, xMax1, yMin1, yMax1);
                    myQuadTree.Insert(myItemEnv, i);

                } // end of looping through lines

                // Copy inputSf and identitySF fields to resultSF, renaming duplicate fields
                if (Globals.CopyFields(ref inputSF, ref resultSF) == false)
                {
                    return false;
                }

                if (Globals.CopyFields(ref identitySF, ref resultSF, true) == false)
                {
                    return false;
                }

                var inputNumFields = inputSF.NumFields;
                var identityNumFields = identitySF.NumFields;
                Geometry inputGeom;
                IGeometry intersectGeom;
                Shape identityShape;
                IList results;
                var inputShapesForIdentityShape = 0;

                // Loop through identitySF and get inputSF geometries that intersect
                for (var identityIndex = 0; identityIndex < identitySF.NumShapes; identityIndex++)
                {
                    identityShape = identitySF.get_Shape(identityIndex);
                    var queryPoly = NTS_Adapter.ShapeToGeometry(identityShape);
                    double xMin2;
                    double xMax2;
                    double yMin2;
                    double yMax2;
                    double zMin2;
                    double zMax2;
                    identityShape.Extents.GetBounds(out xMin2, out yMin2, out zMin2, out xMax2, out yMax2, out zMax2);
                    var myQueryEnv = new Envelope(xMin2, xMax2, yMin2, yMax2);

                    // use quadtree index to find geometries that may intersect
                    results = myQuadTree.Query(myQueryEnv);
                    for (var i = 0; i < results.Count; i++)
                    {
                        var intersectIndex = Convert.ToInt32(results[i]);
                        currGeom = inputSF.get_Shape(intersectIndex);   // Get input geometry
                        inputGeom = NTS_Adapter.ShapeToGeometry(currGeom);   // Convert to NTS Geometry
                        for (var inputGoemIndex = 0; inputGoemIndex < inputGeom.NumGeometries; inputGoemIndex++)    //use each part of the geometry
                        {
                            if (inputGeom.GetGeometryN(inputGoemIndex).Intersects(queryPoly))   // check for intersection
                            {
                                // TODO Remove NTS
                                intersectGeom = NTS.Topology.Operation.Overlay.OverlayOp.Overlay(queryPoly, inputGeom.GetGeometryN(inputGoemIndex), NTS.Topology.Operation.Overlay.SpatialFunctions.Intersection); // create intersect geometry
                                if (!intersectGeom.IsEmpty)
                                {
                                    for (var geomIndex = 0; geomIndex < intersectGeom.NumGeometries; geomIndex++)   // process each part of intersect result
                                    {
                                        if (inputGeom.GetGeometryN(inputGoemIndex).GetType().Name == intersectGeom.GetGeometryN(geomIndex).GetType().Name) // only used geometries of the same type as the input.
                                        {
                                            // Write shape geometry
                                            var intersectShape = NTS_Adapter.GeometryToShape(intersectGeom.GetGeometryN(geomIndex));
                                            var shpIndex = resultSF.NumShapes;
                                            if (resultSF.EditInsertShape(intersectShape, ref shpIndex) == false)
                                            {
                                                gErrorMsg = string.Format("Problem inserting shape into result file: {0}, Input Id: {1}, IdentityId: {2}", resultSF.get_ErrorMsg(resultSF.LastErrorCode), intersectIndex, identityIndex);
                                                Debug.WriteLine(gErrorMsg);
                                                Error.SetErrorMsg(gErrorMsg);
                                                return false;
                                            }

                                            inputShapesForIdentityShape++;
                                            
                                            // add the table values from input SF
                                            for (var j = 0; j <= inputNumFields - 1; j++)
                                            {
                                                if (resultSF.EditCellValue(j, shpIndex, inputSF.get_CellValue(j, intersectIndex)) == false)
                                                {
                                                    gErrorMsg = "Problem inserting value into DBF table: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
                                                    Debug.WriteLine(gErrorMsg);
                                                    Error.SetErrorMsg(gErrorMsg);
                                                    return false;
                                                }
                                            } // end of looping through table

                                            // add the table values from identity SF
                                            for (var j = 0; j <= identityNumFields - 1; j++)
                                            {
                                                if (resultSF.EditCellValue(j + inputNumFields, shpIndex, identitySF.get_CellValue(j, identityIndex)) == false)
                                                {
                                                    gErrorMsg = "Problem inserting value into DBF table: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
                                                    Debug.WriteLine(gErrorMsg);
                                                    Error.SetErrorMsg(gErrorMsg);
                                                    return false;
                                                }
                                            } // end of looping through table
                                        }
                                    }
                                }
                            }
                        }
                    }
                    var progressmessage = string.Format("{3}: Identity Index:{0}, Quadtree Results:{1}, Shapes added:{2}", identityIndex, results.Count, inputShapesForIdentityShape, DateTime.Now.ToShortTimeString());
                    results.Clear();
                    Debug.WriteLine(progressmessage);
                    inputShapesForIdentityShape = 0;
                    resultSF.StopEditingShapes(true, true, null);
                    resultSF.StartEditingShapes(true, null);
                }
                return (resultSF.NumShapes > 0);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
                return false;
            }
        }
        #endregion

        #region SelectPolygonsWithPolygon()
        /// <summary>
        /// Exports the shapes from the inputSF which fall within the given polygon, saving to the resultSF provided.
        /// </summary>
        /// <returns>False if an error occurs, true otherwise.</returns>
        public static bool SelectPolygonsWithPolygon(ref Shapefile inputSF, ref Shape polygon, ref Shapefile resultSF)
        {
            return SelectPolygonsWithPolygon(ref inputSF, ref polygon, ref resultSF, false);
        }

        /// <summary>
        /// Exports the shapes from the inputSF which fall within the given polygon, saving to the resultSF provided.
        /// </summary>
        /// <returns>False if an error occurs, true otherwise.</returns>
        public static bool SelectPolygonsWithPolygon(ref Shapefile inputSF, ref Shape polygon, ref Shapefile resultSF, bool skipMwShapeID)
        {
            MapWinUtility.Logger.Dbg("SelectPolygonsWithPolygon(inputSF: " + Macro.ParamName(inputSF) + ",\n" +
                                     "                          polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                          resultSF: ref " + Macro.ParamName(resultSF) + ",\n" +
                                     "                          SkipMWShapeID: " + skipMwShapeID + ")");

            var numShapes = inputSF.NumShapes;

            if (!Globals.CopyFields(ref inputSF, ref resultSF))
            {
                Globals.WriteErrorMessage("Error in copy fields");
                return false;
            }

            for (var i = 0; i < numShapes; i++)
            {
                var currShape = inputSF.get_Shape(i);
                var boundsIntersect = Globals.CheckBounds(ref currShape, ref polygon);
                if (boundsIntersect)
                {
                    //check that actual intersection occurs
                    var intersectShp = SpatialOperations.Intersection(currShape, polygon);

                    if (intersectShp == null)
                    {
                        // No intersection was found
                        continue;
                    }

                    // Paul Meems: Related to bug 1068. Added check for correct shape:
                    if (!intersectShp.IsValid)
                    {
                        Globals.WriteErrorMessage("SpatialOperations.Intersection returned an invalid shape: " + intersectShp.IsValidReason);
                        continue;
                    }

                    if (intersectShp.numPoints == 0)
                    {
                        Globals.WriteErrorMessage("SpatialOperations.Intersection returned a shape with no points");
                        continue;
                    }

                    // Shape has at least a small portion inside polygon
                    var shpIndex = resultSF.NumShapes;
                    if (!resultSF.EditInsertShape(currShape, ref shpIndex))
                    {
                        Globals.WriteErrorMessage("Problem inserting shape into result file: " +
                                                  resultSF.get_ErrorMsg(resultSF.LastErrorCode));
                        return false;
                    }

                    // add the table values
                    var numFields = resultSF.NumFields;
                    for (var j = 0; j < numFields; j++)
                    {
                        if (!resultSF.EditCellValue(j, shpIndex, inputSF.get_CellValue(j, i)))
                        {
                            Globals.WriteErrorMessage("Problem inserting value into DBF table: " +
                                                      resultSF.get_ErrorMsg(resultSF.LastErrorCode));
                            return false;
                        }
                    }
                } // end of checking bounds
            } // end of looping through shapes

            // Paul Meems: Added check for null
            if (resultSF != null && resultSF.NumShapes > 0)
            {
                if (resultSF.NumFields == 0 || !skipMwShapeID)
                {
                    // add the ID field and values
                    if (!Globals.DoInsertIDs(ref resultSF))
                    {
                        return false;
                    }
                }
            }

            MapWinUtility.Logger.Dbg("Finished SelectPolygonsWithPolygon");
            return true;
        }

        /// <summary>
        /// Returns the shape indexes of the polygons falling in the specified polygon.
        /// </summary>
        /// <returns>True if some are found.</returns>
        public static bool SelectPolygonsWithPolygon(ref Shapefile inputSF, ref Shape polygon, ref ArrayList results)
        {
            MapWinUtility.Logger.Dbg("SelectPolygonsWithPolygon(inputSF: " + Macro.ParamName(inputSF) + ", \n" +
                                     "                          polygon: " + Macro.ParamName(polygon) + ", \n" +
                                     "                          results: " + results + ")");
            var numShapes = inputSF.NumShapes;
            results = new ArrayList();

            for (var i = 0; i <= numShapes - 1; i++)
            {
                var currShape = inputSF.get_Shape(i);
                var boundsIntersect = Globals.CheckBounds(ref currShape, ref polygon);
                if (boundsIntersect)
                {
                    //check that actual intersection occurs
                    var intersectShp = SpatialOperations.Intersection(currShape, polygon);

                    // Paul Meems: Related to bug 1068. Added check for correct shape:
                    if (intersectShp == null)
                    {
                        // No intersection was found
                        continue;
                    }

                    // Paul Meems: Related to bug 1068. Added check for correct shape:
                    if (!intersectShp.IsValid)
                    {
                        Globals.WriteErrorMessage("SpatialOperations.Intersection returned an invalid shape: " + intersectShp.IsValidReason);
                        continue;
                    }

                    if (intersectShp.numPoints == 0)
                    {
                        Globals.WriteErrorMessage("SpatialOperations.Intersection returned a shape with no points");
                        continue;
                    }

                    // shape has at least a small portion inside polygon
                    results.Add(i);

                } // end of checking bounds
            } // end of looping through shapes
            MapWinUtility.Logger.Dbg("Finished SelectPolygonWithPolygon");

            if (results.Count == 0)
            {
                Globals.WriteErrorMessage("SelectPolygonsWithPolygon did not find any shapes");
            }

            return (results.Count > 0);
        }
        #endregion

        #region SelectPointsWithPolygon()
        /// <summary>
        /// Exports the shapes from the inputSF which fall within the given polygon, saving to the resultSF provided.
        /// </summary>
        /// <returns>False if an error occurs, true otherwise.</returns>
        public static bool SelectPointsWithPolygon(ref Shapefile inputSF, ref Shape polygon, ref Shapefile resultSF)
        {
            return SelectPointsWithPolygon(ref inputSF, ref polygon, ref resultSF, false);
        }
        /// <summary>
        /// Exports the shapes from the inputSF which fall within the given polygon, saving to the resultSF provided.
        /// </summary>
        /// <returns>False if an error occurs, true otherwise.</returns>
        public static bool SelectPointsWithPolygon(ref Shapefile inputSF, ref Shape polygon, ref Shapefile resultSF, bool skipMWShapeID)
        {
            MapWinGIS.Utils utils = new UtilsClass();
            var numPoints = inputSF.NumShapes;

            if (Globals.CopyFields(ref inputSF, ref resultSF) == false)
            {
                return false;
            }

            for (var i = 0; i <= numPoints - 1; i++)
            {
                var currPt = inputSF.QuickPoint(i, 0);
                if (utils.PointInPolygon(polygon, currPt))
                {
                    var shpIndex = resultSF.NumShapes;
                    if (resultSF.EditInsertShape(inputSF.get_Shape(i), ref shpIndex) == false)
                    {
                        gErrorMsg = "Problem inserting shape into result file: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
                        Debug.WriteLine(gErrorMsg);
                        Error.SetErrorMsg(gErrorMsg);
                        return false;
                    }

                    // add the table values
                    var numFields = resultSF.NumFields;
                    for (var j = 0; j <= numFields - 1; j++)
                    {
                        if (resultSF.EditCellValue(j, shpIndex, inputSF.get_CellValue(j, i)) == false)
                        {
                            gErrorMsg = "Problem inserting value into DBF table: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
                            Debug.WriteLine(gErrorMsg);
                            Error.SetErrorMsg(gErrorMsg);
                            return false;
                        }
                    } // end of looping through table
                } // end of checking if point is inside polygon				
            } // end of looping through points

            if (resultSF.NumShapes > 0)
            {
                if (resultSF.NumFields == 0 || !skipMWShapeID)
                {
                    // add the ID field and values
                    if (Globals.DoInsertIDs(ref resultSF) == false)
                    {
                        return false;
                    }
                }
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Returns the shape indexes of the shapes falling within the specified polygon.
        /// </summary>
        /// <returns>False if an error occurs, true otherwise.</returns>
        public static bool SelectPointsWithPolygon(ref Shapefile inputSF, ref Shape polygon, ref ArrayList results)
        {
            results = new ArrayList();
            MapWinGIS.Utils utils = new UtilsClass();
            var numPoints = inputSF.NumShapes;

            for (var i = 0; i <= numPoints - 1; i++)
            {
                var currPt = inputSF.QuickPoint(i, 0);
                if (utils.PointInPolygon(polygon, currPt))
                {
                    results.Add(i);
                } // end of checking if point is inside polygon				
            } // end of looping through points

            return (results.Count > 0);
        }

        #endregion

        #region SelectLinesWithPolygon()
        /// <summary>
        /// Exports the shapes from the inputSF which fall within the given polygon, saving to the resultSF provided.
        /// </summary>
        /// <returns>False if an error occurs, true otherwise.</returns>
        public static bool SelectLinesWithPolygon(ref Shapefile inputSF, ref Shape polygon, ref Shapefile resultSF)
        {
            return SelectLinesWithPolygon(ref inputSF, ref polygon, ref resultSF, false);
        }

        /// <summary>
        /// Exports the shapes from the inputSF which fall within the given polygon, saving to the resultSF provided.
        /// </summary>
        /// <param name="inputSF">the input shapefile</param>
        /// <param name="polygon">The shape to select with</param>
        /// <param name="resultSF">The resulting shapefile</param>
        /// <param name="skipMWShapeID">True to skip creating the MWShapeID field</param>
        /// <returns>False if an error occurs, true otherwise.</returns>
        public static bool SelectLinesWithPolygon(ref Shapefile inputSF, ref Shape polygon, ref Shapefile resultSF, bool skipMWShapeID)
        {
            var numLines = inputSF.NumShapes;

            // Boundary intersection test variables
            double xMin2;
            double xMax2;
            double yMin2;
            double yMax2;
            double zMin2;
            double zMax2;
            
            // Get the masking polygon's boundaries only once:
            polygon.Extents.GetBounds(out xMin2, out yMin2, out zMin2, out xMax2, out yMax2, out zMax2);

            if (Globals.CopyFields(ref inputSF, ref resultSF) == false)
            {
                return false;
            }

            Shape currLine;
            for (var i = 0; i <= numLines - 1; i++)
            {
                currLine = inputSF.get_Shape(i);
                double xMin1;
                double xMax1;
                double yMin1;
                double yMax1;
                double zMin1;
                double zMax1;
                currLine.Extents.GetBounds(out xMin1, out yMin1, out zMin1, out xMax1, out yMax1, out zMax1);

                // Are the boundaries intersecting?
                if (!(xMin1 > xMax2 || xMax1 < xMin2 || yMin1 > yMax2 || yMax1 < yMin2))
                {
                    // lines are nasty, just because the boundaries intersect it
                    // doesn't mean the line enters the polygon
                    // do a quick point check before doing a more thorough investigation
                    var numPoints = currLine.numPoints;
                    var pointInside = false;
                    for (var j = 0; j <= numPoints - 1; j++)
                    {
                        var currPt = currLine.get_Point(j);
                        if (polygon.PointInThisPoly(currPt))
                        {
                            pointInside = true;
                            break;
                        }
                    }

                    int shpIndex;
                    if (pointInside)
                    {
                        // we know part of the line is inside the polygon so add line to result file
                        shpIndex = resultSF.NumShapes;
                        if (resultSF.EditInsertShape(currLine, ref shpIndex) == false)
                        {
                            gErrorMsg = "Problem inserting shape into result file: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
                            Debug.WriteLine(gErrorMsg);
                            Error.SetErrorMsg(gErrorMsg);
                            return false;
                        }

                        // add the table values
                        var numFields = resultSF.NumFields;
                        for (var j = 0; j <= numFields - 1; j++)
                        {
                            if (resultSF.EditCellValue(j, shpIndex, inputSF.get_CellValue(j, i)) == false)
                            {
                                gErrorMsg = "Problem inserting value into DBF table: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
                                Debug.WriteLine(gErrorMsg);
                                Error.SetErrorMsg(gErrorMsg);
                                return false;
                            }
                        } // end of looping through table
                    }
                    else
                    {
                        // Avoid using a temp file to test each individual file;
                        // instead, just see if the line crosses the polygon
                        if (LineCrossesPoly(ref currLine, ref polygon))
                        {
                            // part of the line lies within the polygon, add to result file
                            shpIndex = resultSF.NumShapes;
                            if (resultSF.EditInsertShape(currLine, ref shpIndex) == false)
                            {
                                gErrorMsg = "Problem inserting shape into result file: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
                                Debug.WriteLine(gErrorMsg);
                                Error.SetErrorMsg(gErrorMsg);
                                return false;
                            }
                            
                            // add the table values
                            var numFields = resultSF.NumFields;
                            for (var j = 0; j <= numFields - 1; j++)
                            {
                                if (resultSF.EditCellValue(j, shpIndex, inputSF.get_CellValue(j, i)) == false)
                                {
                                    gErrorMsg = "Problem inserting value into DBF table: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
                                    Debug.WriteLine(gErrorMsg);
                                    Error.SetErrorMsg(gErrorMsg);
                                    return false;
                                }
                            } // end of looping through table
                        } // end of successful cross
                    } // end of else no points were found inside polygon
                } // end of checking bounds
            } // end of looping through lines

            if (resultSF.NumShapes > 0)
            {
                if (resultSF.NumFields == 0 || !skipMWShapeID)
                {
                    // add the ID field and values
                    if (Globals.DoInsertIDs(ref resultSF) == false)
                    {
                        return false;
                    }
                }

                return true;
            }
            
            return false;
        }

        /// <summary>
        ///  Returns the indexes of shapes that fall within the specified polygon.
        /// </summary>
        /// <param name="inputSF">The input shapefile</param>
        /// <param name="polygon">The polygon to select with</param>
        /// <param name="results">A list of shape indexes</param>
        /// <returns>False if an error occurs, true otherwise.</returns>
        public static bool SelectLinesWithPolygon(ref Shapefile inputSF, ref Shape polygon, ref ArrayList results)
        {
            var numLines = inputSF.NumShapes;
            results = new ArrayList();

            // Boundary intersection test variables
            double xMin2;
            double xMax2;
            double yMin2;
            double yMax2;
            double zMin2;
            double zMax2;
            
            // Get the masking polygon's boundaries only once:
            polygon.Extents.GetBounds(out xMin2, out yMin2, out zMin2, out xMax2, out yMax2, out zMax2);

            Shape currLine;
            for (var i = 0; i <= numLines - 1; i++)
            {
                currLine = inputSF.get_Shape(i);
                double xMin1;
                double xMax1;
                double yMin1;
                double yMax1;
                double zMin1;
                double zMax1;
                currLine.Extents.GetBounds(out xMin1, out yMin1, out zMin1, out xMax1, out yMax1, out zMax1);

                // Are the boundaries intersecting?
                if (!(xMin1 > xMax2 || xMax1 < xMin2 || yMin1 > yMax2 || yMax1 < yMin2))
                {
                    // lines are nasty, just because the boundaries intersect it
                    // doesn't mean the line enters the polygon
                    // do a quick point check before doing a more thorough investigation
                    var numPoints = currLine.numPoints;
                    var pointInside = false;
                    for (var j = 0; j <= numPoints - 1; j++)
                    {
                        var currPt = currLine.get_Point(j);
                        if (polygon.PointInThisPoly(currPt))
                        {
                            pointInside = true;
                            break;
                        }
                    }

                    if (pointInside)
                    {
                        results.Add(i);
                    }
                    else
                    {
                        // Avoid using a temp file to test each individual file;
                        // instead, just see if the line crosses the polygon
                        if (LineCrossesPoly(ref currLine, ref polygon))
                        {
                            // part of the line lies within the polygon, add to result file
                            results.Add(i);
                        } // end of successful cross
                    } // end of else no points were found inside polygon
                } // end of checking bounds
            } // end of looping through lines

            return results.Count > 0;
        }

        /// <summary>
        /// Calculates if the line crossed the polygon
        /// </summary>
        /// <param name="line">The line that might cross</param>
        /// <param name="polygon">The polygon to test</param>
        /// <returns>True if it crosses</returns>
        /// <remarks>TODO: Rewrite this method using faster GEOS calls</remarks>
        private static bool LineCrossesPoly(ref Shape line, ref Shape polygon)
        {
            var numLineSegs = line.numPoints - 1;
            var numPolyPts = polygon.numPoints;
            int[] intersectsPerSeg;
            var intersectPts = new MapWinGIS.Point[numLineSegs][];
            var polyIntLocs = new int[numLineSegs][];

            for (var i = 0; i <= numLineSegs - 1; i++)
            {
                intersectPts[i] = new MapWinGIS.Point[numPolyPts];
                polyIntLocs[i] = new int[numPolyPts];
            }

            var numIntersects = Globals.CalcSiDeterm(ref line, ref polygon, out intersectsPerSeg, out intersectPts, out polyIntLocs);

            return numIntersects != 0;
        }
        #endregion
    }
}
