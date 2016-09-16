// ********************************************************************************************************
// <copyright file="SpatialOperations.cs" company="MapWindow.org">
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
// 09-01-05 ah - Angela Hillier - Created clipping methods and support functions.
// 12-06-05 ah - Angela Hillier - added selection method
// 01-12-06 ah - Angela Hilleir - provided initial API and parameter descriptions for unimplemented functions	
// 02-27-06 ah - Angela Hillier - added MergeShapes()
// 03-29-06 ah - Angela Hillier - added Buffer routines 							
// 04-14-06 mg - Mark Gray      - simplified and moved error checking in ClipGridWithPolygon wrappers
// 06-04-06 ah - Angela Hillier - altered ClipPolygonWithLine and ClipLineWithPolygon to handle multi-part polygons.					
// 08-06-06 aa - Allen Anselmo  - altered clips to add a copyAttribute parameter
// 07-04-07 sb - Simon Batson   - added Identity processing
// 12/29/10 pm - Paul Meems     - Start making this class StyleCop and ReSharper compliant
// ********************************************************************************************************

namespace MapWinGeoProc
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using MapWindow.Interfaces.Geometries;
    using MapWinGIS;

    /// <summary>
    /// An enumeration which is intended to eventually contain a list of spatial join types (only nearest is supported)
    /// </summary>
    public enum SpatialJoinTypes
    {
        /// <summary>
        /// An enum element representing the Nearest topology operation
        /// </summary>
        Nearest
    };

    /// <summary>
    /// Contains public methods for users to clip grids and shapes with.
    /// </summary>
    public class SpatialOperations
    {
        // All methods are static so that no instance of the class is required.
        // If changes are made to a function, be sure to make similar changes in the overloaded versions.

        private static string gErrorMsg = "";

        #region ClipGridWithPolygon()
        /// <summary>
        /// Creates a new grid containing data from the input grid that
        /// falls within the polygon shape.
        /// </summary>
        /// <param name="inputGridPath">Full path to the input grid file.</param>
        /// <param name="polygon">The 2D polygon used for clipping the input grid.</param>
        /// <param name="resultGridPath">Full path to where the resulting grid will be saved.</param>
        /// <returns>True if clipping was successful, false otherwise.</returns>
        public static bool ClipGridWithPolygon(ref string inputGridPath, ref Shape polygon, ref string resultGridPath)
        {
            if (inputGridPath == null)
            {
                throw new ArgumentNullException("inputGridPath");
            }

            MapWinUtility.Logger.Dbg("ClipGridWithPolygon(intputGFPath: " + inputGridPath + ",\n" +
                                     "                    polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                    resultGridPath: " + resultGridPath + ")");

            return ClipGridWithPolygon(ref inputGridPath, ref polygon, ref resultGridPath, false);
        }

        /// <summary>
        /// Creates a new grid containing data from the input grid that
        /// falls within the polygon shape or within the polygon extents.
        /// </summary>
        /// <param name="inputGridPath">Full path to the input grid file.</param>
        /// <param name="polygon">The 2D polygon used for clipping the input grid.</param>
        /// <param name="resultGridPath">Full path to where the resulting grid will be saved.</param>
        /// <param name="clipToExtents">True if clipping to polygon extents rather than actual polygon shape.</param>
        /// <returns>True if clipping was successful, false otherwise.</returns>
        public static bool ClipGridWithPolygon(ref string inputGridPath, ref Shape polygon, ref string resultGridPath, bool clipToExtents)
        {
            MapWinUtility.Logger.Dbg("ClipGridWithPolygon(inputGFPath: " + inputGridPath + ",\n" +
                                     "                    polygon: " + polygon + ",\n" +
                                     "                    resultGridPath: " + resultGridPath + ",\n" +
                                     "                    clipToExtents: " + clipToExtents);
            Error.ClearErrorLog();

            // PM: Check input:
            if (string.IsNullOrEmpty(inputGridPath))
            {
                Globals.WriteErrorMessage("Parameter inputGridPath is not correct");
                return false;
            }

            if (!File.Exists(inputGridPath))
            {
                Globals.WriteErrorMessage(string.Format("The file {0} does not exists", inputGridPath));
                return false;
            }

            if (string.IsNullOrEmpty(resultGridPath))
            {
                Globals.WriteErrorMessage("Parameter resultGridPath is not correct");
                return false;
            }

            if (!polygon.IsValid)
            {
                Globals.WriteErrorMessage("Parameter polygon is not valid: " + polygon.IsValidReason);
                return false;
            }

            if (polygon.numPoints == 0)
            {
                Globals.WriteErrorMessage("Parameter polygon has no points");
                return false;
            }

            MapWinUtility.Logger.Dbg("Finished ClipGridWithPolygon");
            return ClipGridWithPoly.ClipGridWithPolygon(ref inputGridPath, ref polygon, ref resultGridPath, clipToExtents);
        }
        #endregion

        #region SelectWithPolygon()
        /// <summary>
        /// Selects all points/lines/polygons in the input shapefile that intersect the polygon shape.
        /// Returns the selected shapes in a new shapefile.
        /// </summary>
        /// <param name="inputSFPath">The full path to the shapefile containing shapes for selection.</param>
        /// <param name="polygon">The polygon used for selecting shapes from the shapefile.</param>
        /// <param name="resultSFPath">The full path to where the selected shapes should be saved.</param>
        /// <returns>False if an error was encountered or no shapes selected, true otherwise.</returns>
        public static bool SelectWithPolygon(ref string inputSFPath, ref Shape polygon, ref string resultSFPath)
        {
            MapWinUtility.Logger.Dbg("SelectWithPolygon(inputSFPath: " + inputSFPath + ",\r\n" +
                                     "                  polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                  resultSFPath: " + resultSFPath + ")");
            return SelectWithPolygon(ref inputSFPath, ref polygon, ref resultSFPath, false);

        }
        /// <summary>
        /// Selects all points/lines/polygons in the input shapefile that intersect the polygon shape.
        /// Returns the selected shapes in a new shapefile.
        /// </summary>
        /// <param name="inputSFPath">The full path to the shapefile containing shapes for selection.</param>
        /// <param name="polygon">The polygon used for selecting shapes from the shapefile.</param>
        /// <param name="resultSFPath">The full path to where the selected shapes should be saved.</param>
        /// <param name="skipMwShapeID">Indicates whether to skip creating an MWShapeID field in the result.</param>
        /// <returns>False if an error was encountered or no shapes selected, true otherwise.</returns>
        public static bool SelectWithPolygon(ref string inputSFPath, ref Shape polygon, ref string resultSFPath, bool skipMwShapeID)
        {
            MapWinUtility.Logger.Dbg("SelectWithPolygon(inputSFPath: " + inputSFPath + ",\n" +
                                     "                  polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                  resultSFPath: " + resultSFPath + ",\n" +
                                     "                  SkipMWShapeID: " + skipMwShapeID + ")");
            Error.ClearErrorLog();

            // Do some checking:
            if (string.IsNullOrEmpty(inputSFPath))
            {
                Globals.WriteErrorMessage("Parameter inputSFPath is empty");
                return false;
            }

            if (!File.Exists(inputSFPath))
            {
                Globals.WriteErrorMessage("Parameter inputSFPath does not exist: " + inputSFPath);
                return false;
            }

            if (!polygon.IsValid)
            {
                Globals.WriteErrorMessage("Parameter polygon is invalid: " + polygon.IsValidReason);
                return false;
            }

            if (string.IsNullOrEmpty(resultSFPath))
            {
                Globals.WriteErrorMessage("Parameter resultSFPath is empty");
                return false;
            }

            // Open input shapefile:
            Shapefile inputSF = new ShapefileClass();
            inputSF.Open(inputSFPath, null);
            var sfType = inputSF.ShapefileType;
            Shapefile resultSF = new ShapefileClass();

            if (sfType == ShpfileType.SHP_NULLSHAPE)
            {
                Globals.WriteErrorMessage("The shapefile is NULL, it must contain at least one shape.");
                return false;
            }

            // Create the result file
            if (!Globals.PrepareResultSF(ref resultSFPath, ref resultSF, sfType, true))
            {
                // I think this already logged the error in PrepareResultSF
                resultSF.Close();
                inputSF.Close();
                return false;
            }

            switch (sfType)
            {
                case ShpfileType.SHP_POLYGONZ:
                case ShpfileType.SHP_POLYGONM:
                case ShpfileType.SHP_POLYGON:
                    if (!Selection.SelectPolygonsWithPolygon(ref inputSF, ref polygon, ref resultSF, skipMwShapeID))
                    {
                        Globals.WriteErrorMessage("Error! Selection.SelectPolygonsWithPolygon returned: " + Error.GetLastErrorMsg());
                        resultSF.Close();
                        inputSF.Close();
                        return false;
                    }
                    break;
                case ShpfileType.SHP_POINTZ:
                case ShpfileType.SHP_POINTM:
                case ShpfileType.SHP_POINT:
                    if (!Selection.SelectPointsWithPolygon(ref inputSF, ref polygon, ref resultSF, skipMwShapeID))
                    {
                        Globals.WriteErrorMessage("Selection.SelectPointsWithPolygon returned: " + Error.GetLastErrorMsg());
                        resultSF.Close();
                        inputSF.Close();
                        return false;
                    }
                    break;
                case ShpfileType.SHP_POLYLINEZ:
                case ShpfileType.SHP_POLYLINEM:
                case ShpfileType.SHP_POLYLINE:
                    if (!Selection.SelectLinesWithPolygon(ref inputSF, ref polygon, ref resultSF, skipMwShapeID))
                    {
                        Globals.WriteErrorMessage("Selection.SelectLinesWithPolygon returned: " + Error.GetLastErrorMsg());
                        resultSF.Close();
                        inputSF.Close();
                        return false;
                    }
                    break;
                default:
                    Globals.WriteErrorMessage("Not a valid shapefile type: " + sfType);
                    resultSF.Close();
                    inputSF.Close();
                    return false;
            }


            //save the result file
            if (resultSF.NumShapes > 0)
            {
                // PM Add projection of source shapefile:
                resultSF.Projection = inputSF.Projection;
                resultSF.StopEditingShapes(true, true, null);

                // TODO (PM): Is it necessary to call SaveAs after calling StopEditingShapes?
                resultSF.SaveAs(resultSFPath, null);
                resultSF.Close();
                inputSF.Close();
                MapWinUtility.Logger.Dbg("Finished SelectWithPolygon");
                return true;
            }

            // Something went wrong:
            Globals.WriteErrorMessage("The shapefile is NULL, it must contain at least one shape.");
            return false;
        }


        /// <summary>
        /// Selects all points/lines/polygons in the input shapefile that intersect the polygon shape.
        /// Returns the indexes of the shapes that are found to fall in or cross the polygon.
        /// </summary>
        /// <param name="inputSFPath">The full path to the shapefile containing shapes for selection.</param>
        /// <param name="polygon">The polygon used for selecting shapes from the shapefile.</param>
        /// <param name="results">An arraylist of the resulting shape indices.</param>
        /// <returns>False if no shapes were found, true otherwise.</returns>
        public static bool SelectWithPolygon(ref string inputSFPath, ref Shape polygon, ref System.Collections.ArrayList results)
        {
            MapWinUtility.Logger.Dbg("SelectWithPolygon(inputSFPath: " + inputSFPath + ",\n" +
                                     "                  polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                  results: out)");
            Error.ClearErrorLog();

            // Do some checking:
            if (string.IsNullOrEmpty(inputSFPath))
            {
                Globals.WriteErrorMessage("Parameter inputSFPath is empty");
                return false;
            }

            if (!File.Exists(inputSFPath))
            {
                Globals.WriteErrorMessage("Parameter inputSFPath does not exist: " + inputSFPath);
                return false;
            }

            if (!polygon.IsValid)
            {
                Globals.WriteErrorMessage("Parameter polygon is invalid: " + polygon.IsValidReason);
                return false;
            }

            // Open input shapefile:
            Shapefile inputSF = new ShapefileClass();
            inputSF.Open(inputSFPath, null);
            var sfType = inputSF.ShapefileType;
            Shapefile resultSF = new ShapefileClass();
            if (sfType == ShpfileType.SHP_NULLSHAPE)
            {
                Globals.WriteErrorMessage("The shapefile is NULL, it must contain at least one shape.");
                return false;
            }

            switch (sfType)
            {
                case ShpfileType.SHP_POLYGONZ:
                case ShpfileType.SHP_POLYGONM:
                case ShpfileType.SHP_POLYGON:
                    if (!Selection.SelectPolygonsWithPolygon(ref inputSF, ref polygon, ref results))
                    {
                        Globals.WriteErrorMessage("Selection.SelectPolygonsWithPolygon returned: " + Error.GetLastErrorMsg());
                        resultSF.Close();
                        inputSF.Close();
                        return false;
                    }
                    break;
                case ShpfileType.SHP_POINTZ:
                case ShpfileType.SHP_POINTM:
                case ShpfileType.SHP_POINT:
                    if (!Selection.SelectPointsWithPolygon(ref inputSF, ref polygon, ref results))
                    {
                        Globals.WriteErrorMessage("Selection.SelectPointsWithPolygon returned: " + Error.GetLastErrorMsg());
                        resultSF.Close();
                        inputSF.Close();
                        return false;
                    }
                    break;
                case ShpfileType.SHP_POLYLINEZ:
                case ShpfileType.SHP_POLYLINEM:
                case ShpfileType.SHP_POLYLINE:
                    if (!Selection.SelectLinesWithPolygon(ref inputSF, ref polygon, ref results))
                    {
                        Globals.WriteErrorMessage("Selection.SelectLinesWithPolygon returned: " + Error.GetLastErrorMsg());
                        resultSF.Close();
                        inputSF.Close();
                        return false;
                    }
                    break;
                default:
                    Globals.WriteErrorMessage("Not a valid shapefile type.");
                    resultSF.Close();
                    inputSF.Close();
                    return false;
            }

            MapWinUtility.Logger.Dbg("Finished SelectWithPolygon");
            return true;
        }

        #endregion

        #region ClipShapesWithPolygon(): save-to-disk versions
        /// <summary>
        /// Saves all points/lines/polygons within the polygon's border to a new shapefile.
        /// </summary>
        /// <param name="inputSFPath">Full path to the input shapefile that will be clipped.</param>
        /// <param name="polygon">2D polygon used for clipping objects in inputSF.</param>
        /// <param name="resultSFPath">Full path to where the resulting shapefile should be saved.</param>
        /// <returns>True if clipping was successful, false if an error was encountered.</returns>
        public static bool ClipShapesWithPolygon(ref string inputSFPath, ref Shape polygon, ref string resultSFPath)
        {
            MapWinUtility.Logger.Dbg("ClipShapesWithPolygon(inputSFPath: " + inputSFPath + ",\n" +
                                     "                      polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                      resultsSFPath: " + resultSFPath + ",\n");
            Error.ClearErrorLog();
            Shapefile sf = new ShapefileClass();
            sf.Open(inputSFPath, null);
            var sfType = sf.ShapefileType;
            sf.Close();
            if (sfType != ShpfileType.SHP_NULLSHAPE)
            {
                if (sfType == ShpfileType.SHP_POLYGON || sfType == ShpfileType.SHP_POLYGONM || sfType == ShpfileType.SHP_POLYGONZ)
                {
                    return ClipPolySFWithPoly.ClipPolygonSFWithPolygon(ref inputSFPath, ref polygon, ref resultSFPath, false);
                }
                else if (sfType == ShpfileType.SHP_POINT || sfType == ShpfileType.SHP_POINTM || sfType == ShpfileType.SHP_POINTZ)
                {
                    return ClipPointSFWithPoly.ClipPointSFWithPolygon(ref inputSFPath, ref polygon, ref resultSFPath, false);
                }
                else if (sfType == ShpfileType.SHP_POLYLINE || sfType == ShpfileType.SHP_POLYLINEM || sfType == ShpfileType.SHP_POLYLINEZ)
                {
                    //always call the "accurate" version for this overloaded method
                    if (polygon.NumParts > 1)//multi-part polygon
                    {
                        return ClipLineSFWithPoly.ClipLineSFWithMultiPartPolygon(ref inputSFPath, ref polygon, ref resultSFPath, false, false);
                    }
                    else //single-part polygon
                    {
                        return ClipLineSFWithPoly.Accurate_ClipLineSFWithPolygon(ref inputSFPath, ref polygon, ref resultSFPath, false);
                    }
                }
                else
                {
                    gErrorMsg = "Not a valid shapefile type.";
                    Debug.WriteLine(gErrorMsg);
                    Error.SetErrorMsg(gErrorMsg);
                    MapWinUtility.Logger.Dbg(gErrorMsg);
                    return false;
                }
            }
            else
            {
                gErrorMsg = "The shapefile is NULL, it must contain at least one shape.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                MapWinUtility.Logger.Dbg(gErrorMsg);
                return false;
            }

        }

        /// <summary>Saves all points/lines/polygons within the polygon's border to a new shapefile.</summary>
        /// <param name="inputSFPath">Full path to the input shapefile that will be clipped.</param>
        /// <param name="polygon">2D polygon used for clipping objects in inputSF.</param>
        /// <param name="resultSFPath">Full path to where the resulting shapefile should be saved.</param>
        /// <param name="speedOptimized">True if speed of computation is more important than accuracy when dealing with line shapefiles.</param>
        /// <returns>True if clipping was successful, false if an error was encountered.</returns>
        public static bool ClipShapesWithPolygon(ref string inputSFPath, ref Shape polygon, ref string resultSFPath, bool speedOptimized)
        {
            MapWinUtility.Logger.Dbg("ClipShapesWithPolygon(inputSFPath: " + inputSFPath + ",\n" +
                                     "                      polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                      resultSFPath: " + resultSFPath + ",\n" +
                                     "                      sppedOptimized: " + speedOptimized + ")");

            Error.ClearErrorLog();
            Shapefile sf = new ShapefileClass();
            sf.Open(inputSFPath, null);
            var sfType = sf.ShapefileType;
            sf.Close();
            if (sfType != ShpfileType.SHP_NULLSHAPE)
            {
                if (sfType == ShpfileType.SHP_POLYGON || sfType == ShpfileType.SHP_POLYGONM || sfType == ShpfileType.SHP_POLYGONZ)
                {
                    return ClipPolySFWithPoly.ClipPolygonSFWithPolygon(ref inputSFPath, ref polygon, ref resultSFPath, false);
                }
                else if (sfType == ShpfileType.SHP_POINT || sfType == ShpfileType.SHP_POINTM || sfType == ShpfileType.SHP_POINTZ)
                {
                    return ClipPointSFWithPoly.ClipPointSFWithPolygon(ref inputSFPath, ref polygon, ref resultSFPath, false);
                }
                else if (sfType == ShpfileType.SHP_POLYLINE || sfType == ShpfileType.SHP_POLYLINEM || sfType == ShpfileType.SHP_POLYLINEZ)
                {
                    if (polygon.NumParts > 1) //multi-part polygon
                    {
                        return ClipLineSFWithPoly.ClipLineSFWithMultiPartPolygon(ref inputSFPath, ref polygon, ref resultSFPath, speedOptimized, false);
                    }
                    else //single-part polygon
                    {
                        if (speedOptimized == true)
                        {
                            return ClipLineSFWithPoly.Fast_ClipLineSFWithPolygon(ref inputSFPath, ref polygon, ref resultSFPath, false);
                        }
                        else
                        {
                            return ClipLineSFWithPoly.Accurate_ClipLineSFWithPolygon(ref inputSFPath, ref polygon, ref resultSFPath, false);
                        }
                    }
                }
                else
                {
                    gErrorMsg = "Not a valid shapefile type.";
                    Debug.WriteLine(gErrorMsg);
                    Error.SetErrorMsg(gErrorMsg);
                    MapWinUtility.Logger.Dbg(gErrorMsg);
                    return false;
                }
            }
            else
            {
                gErrorMsg = "The shapefile is NULL, it must contain at least one shape.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                MapWinUtility.Logger.Dbg(gErrorMsg);
                return false;
            }
        }


        /// <summary>Saves all points/lines/polygons within the polygon's border to a new shapefile.</summary>
        /// <param name="inputSFPath">Full path to the input shapefile that will be clipped.</param>
        /// <param name="polygon">2D polygon used for clipping objects in inputSF.</param>
        /// <param name="resultSFPath">Full path to where the resulting shapefile should be saved.</param>
        /// <param name="speedOptimized">True if speed of computation is more important than accuracy when dealing with line shapefiles.</param>
        /// <param name="copyAttributes">True if copying Attributes</param>
        /// <returns>True if clipping was successful, false if an error was encountered.</returns>
        public static bool ClipShapesWithPolygon(ref string inputSFPath, ref Shape polygon, ref string resultSFPath, bool speedOptimized, bool copyAttributes)
        {
            MapWinUtility.Logger.Dbg("ClipShapesWithPolygon(inputSFPath: " + inputSFPath + ",\n" +
                                     "                      polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                      resultsSFPath: " + resultSFPath + ",\n" +
                                     "                      speedOptimized: " + speedOptimized.ToString() + ",\n" +
                                     "                      copyAttributes: " + copyAttributes.ToString() + ")");
            Error.ClearErrorLog();
            Shapefile sf = new ShapefileClass();
            if (!sf.Open(inputSFPath, null))
            {
                gErrorMsg = "Cannot open shapefile: " + inputSFPath;
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                MapWinUtility.Logger.Dbg(gErrorMsg);
                return false;
            }
            var sfType = sf.ShapefileType;
            sf.Close();
            if (sfType != ShpfileType.SHP_NULLSHAPE)
            {
                if (sfType == ShpfileType.SHP_POLYGON || sfType == ShpfileType.SHP_POLYGONM || sfType == ShpfileType.SHP_POLYGONZ)
                {
                    return ClipPolySFWithPoly.ClipPolygonSFWithPolygon(ref inputSFPath, ref polygon, ref resultSFPath, copyAttributes);
                }
                else if (sfType == ShpfileType.SHP_POINT || sfType == ShpfileType.SHP_POINTM || sfType == ShpfileType.SHP_POINTZ)
                {
                    return ClipPointSFWithPoly.ClipPointSFWithPolygon(ref inputSFPath, ref polygon, ref resultSFPath, copyAttributes);
                }
                else if (sfType == ShpfileType.SHP_POLYLINE || sfType == ShpfileType.SHP_POLYLINEM || sfType == ShpfileType.SHP_POLYLINEZ)
                {
                    if (polygon.NumParts > 1) //multi-part polygon
                    {
                        return ClipLineSFWithPoly.ClipLineSFWithMultiPartPolygon(ref inputSFPath, ref polygon, ref resultSFPath, speedOptimized, copyAttributes);
                    }
                    else //single-part polygon
                    {
                        if (speedOptimized == true)
                        {
                            return ClipLineSFWithPoly.Fast_ClipLineSFWithPolygon(ref inputSFPath, ref polygon, ref resultSFPath, copyAttributes);
                        }
                        else
                        {
                            return ClipLineSFWithPoly.Accurate_ClipLineSFWithPolygon(ref inputSFPath, ref polygon, ref resultSFPath, copyAttributes);
                        }
                    }
                }
                else
                {
                    gErrorMsg = "Not a valid shapefile type.";
                    Debug.WriteLine(gErrorMsg);
                    Error.SetErrorMsg(gErrorMsg);
                    MapWinUtility.Logger.Dbg(gErrorMsg);
                    return false;
                }
            }
            else
            {
                gErrorMsg = "The shapefile is NULL, it must contain at least one shape.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                MapWinUtility.Logger.Dbg(gErrorMsg);
                return false;
            }
        }

        #endregion

        #region ClipShapesWithPolygon(): save-to-memory versions
        /// <summary>Returns all points/lines/polygons inside the polygon's border as a new shapefile (in memory).</summary>
        /// <param name="inputSF">Input shapefile of lines, points, or polygons in need of clipping.</param>
        /// <param name="polygon">2D polygon shape used for clipping the shapefile.</param>
        /// <param name="resultSF">Output polygon shapefile which will contain all shapes that fall inside the boundaries of poly.</param>
        /// <returns>True if clipping was successful, false if an error occured. </returns> 
        public static bool ClipShapesWithPolygon(ref Shapefile inputSF, ref Shape polygon, out Shapefile resultSF)
        {
            MapWinUtility.Logger.Dbg("ClipShapesWithPolygon(inputSF: " + Macro.ParamName(inputSF) + ",\n" +
                                     "                      polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                      resultSF: out)");
            MapWinGeoProc.Error.ClearErrorLog();
            var sfType = inputSF.ShapefileType;
            Shapefile resultFile = new ShapefileClass();
            bool status;

            if (sfType != ShpfileType.SHP_NULLSHAPE)
            {
                if (sfType == ShpfileType.SHP_POLYGON || sfType == ShpfileType.SHP_POLYGONM || sfType == ShpfileType.SHP_POLYGONZ)
                {
                    status = ClipPolySFWithPoly.ClipPolygonSFWithPolygon(ref inputSF, ref polygon, out resultFile, false);
                }
                else if (sfType == ShpfileType.SHP_POINT || sfType == ShpfileType.SHP_POINTM || sfType == ShpfileType.SHP_POINTZ)
                {
                    status = ClipPointSFWithPoly.ClipPointSFWithPolygon(ref inputSF, ref polygon, out resultFile, false);
                }
                else if (sfType == ShpfileType.SHP_POLYLINE || sfType == ShpfileType.SHP_POLYLINEM || sfType == ShpfileType.SHP_POLYLINEZ)
                {
                    if (polygon.NumParts > 1)
                    {
                        status = ClipLineSFWithPoly.ClipLineSFWithMultiPartPolygon(ref inputSF, ref polygon, out resultFile, false, false);
                    }
                    else
                    {
                        status = ClipLineSFWithPoly.Accurate_ClipLineSFWithPolygon(ref inputSF, ref polygon, out resultFile, false);
                    }
                }
                else
                {
                    gErrorMsg = "Not a valid Shapefile type.";
                    Debug.WriteLine(gErrorMsg);
                    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                    resultSF = resultFile;
                    MapWinUtility.Logger.Dbg(gErrorMsg);
                    return false;
                }
            }
            else
            {
                gErrorMsg = "NullShape is not a valid type.";
                Debug.WriteLine(gErrorMsg);
                MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                resultSF = resultFile;
                MapWinUtility.Logger.Dbg(gErrorMsg);
                return false;
            }
            resultSF = resultFile;
            MapWinUtility.Logger.Dbg("Finsihed ClipShapesWithPolygon");
            return status;
        }

        //Overloaded version for fast processing of line shapefiles
        /// <summary>Returns all points/lines/polygons inside the polygon's border as a new shapefile (in memory).</summary>
        /// <param name="inputSF">Input shapefile of lines, points, or polygons in need of clipping.</param>
        /// <param name="polygon">2D polygon shape used for clipping the shapefile.</param>
        /// <param name="resultSF">Output polygon shapefile which will contain all shapes that fall inside the boundaries of poly.</param>
        /// <param name="speedOptimized">True if speed of computation is more important than accuracy when dealing with line shapefiles.</param>
        /// <returns>True if clipping was successful, false if an error occured. </returns> 
        public static bool ClipShapesWithPolygon(ref Shapefile inputSF, ref Shape polygon, out Shapefile resultSF, bool speedOptimized)
        {
            MapWinUtility.Logger.Dbg("ClipShapesWithPolygon(inputSF: " + Macro.ParamName(inputSF) + ",\n" +
                                     "                      polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                      resultSF: out,\n" +
                                     "                      speedOptimized: " + speedOptimized.ToString() + ")");
            Error.ClearErrorLog();
            var sfType = inputSF.ShapefileType;
            Shapefile resultFile = new ShapefileClass();
            bool status;

            if (sfType != ShpfileType.SHP_NULLSHAPE)
            {
                if (sfType == ShpfileType.SHP_POLYGON || sfType == ShpfileType.SHP_POLYGONM || sfType == ShpfileType.SHP_POLYGONZ)
                {
                    status = ClipPolySFWithPoly.ClipPolygonSFWithPolygon(ref inputSF, ref polygon, out resultFile, false);
                }
                else if (sfType == ShpfileType.SHP_POINT || sfType == ShpfileType.SHP_POINTM || sfType == ShpfileType.SHP_POINTZ)
                {
                    status = ClipPointSFWithPoly.ClipPointSFWithPolygon(ref inputSF, ref polygon, out resultFile, false);
                }
                else if (sfType == ShpfileType.SHP_POLYLINE || sfType == ShpfileType.SHP_POLYLINEM || sfType == ShpfileType.SHP_POLYLINEZ)
                {
                    if (polygon.NumParts > 1)
                    {
                        status = ClipLineSFWithPoly.ClipLineSFWithMultiPartPolygon(ref inputSF, ref polygon, out resultFile, speedOptimized, false);
                    }
                    else
                    {
                        if (speedOptimized)
                        {
                            status = ClipLineSFWithPoly.Fast_ClipLineSFWithPolygon(ref inputSF, ref polygon, out resultFile, false);
                        }
                        else
                        {
                            status = ClipLineSFWithPoly.Accurate_ClipLineSFWithPolygon(ref inputSF, ref polygon, out resultFile, false);
                        }
                    }
                }
                else
                {
                    gErrorMsg = "Not a valid Shapefile type.";
                    Debug.WriteLine(gErrorMsg);
                    Error.SetErrorMsg(gErrorMsg);
                    resultSF = resultFile;
                    MapWinUtility.Logger.Dbg(gErrorMsg);
                    return false;
                }
            }
            else
            {
                gErrorMsg = "NullShape is not a valid type.";
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                resultSF = resultFile;
                MapWinUtility.Logger.Dbg(gErrorMsg);
                return false;
            }

            resultSF = resultFile;
            MapWinUtility.Logger.Dbg("Finished CLipShapesWithPolygon");
            return status;
        }

        //Overloaded version for fast processing of line shapefiles and copy attributes overload
        /// <summary>Returns all points/lines/polygons inside the polygon's border as a new shapefile (in memory).</summary>
        /// <param name="inputSF">Input shapefile of lines, points, or polygons in need of clipping.</param>
        /// <param name="polygon">2D polygon shape used for clipping the shapefile.</param>
        /// <param name="resultSF">Output polygon shapefile which will contain all shapes that fall inside the boundaries of poly.</param>
        /// <param name="speedOptimized">True if speed of computation is more important than accuracy when dealing with line shapefiles.</param>
        /// <param name="copyAttributes">True if copying attributes over</param>
        /// <returns>True if clipping was successful, false if an error occured. </returns> 
        public static bool ClipShapesWithPolygon(ref Shapefile inputSF, ref Shape polygon, out Shapefile resultSF, bool speedOptimized, bool copyAttributes)
        {
            MapWinUtility.Logger.Dbg("ClipShapesWithPolygon(inputSF: " + Macro.ParamName(inputSF) + ",\n" +
                                     "                      polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                      resultSF: out,\n" +
                                     "                      speedOptimized: " + speedOptimized + ",\n" +
                                     "                      copyAttributes: " + copyAttributes + ",\n");
            MapWinGeoProc.Error.ClearErrorLog();
            var sfType = inputSF.ShapefileType;
            Shapefile resultFile = new ShapefileClass();
            bool status;

            if (sfType != ShpfileType.SHP_NULLSHAPE)
            {
                if (sfType == ShpfileType.SHP_POLYGON || sfType == ShpfileType.SHP_POLYGONM || sfType == ShpfileType.SHP_POLYGONZ)
                {
                    status = ClipPolySFWithPoly.ClipPolygonSFWithPolygon(ref inputSF, ref polygon, out resultFile, copyAttributes);
                }
                else if (sfType == ShpfileType.SHP_POINT || sfType == ShpfileType.SHP_POINTM || sfType == ShpfileType.SHP_POINTZ)
                {
                    status = ClipPointSFWithPoly.ClipPointSFWithPolygon(ref inputSF, ref polygon, out resultFile, copyAttributes);
                }
                else if (sfType == ShpfileType.SHP_POLYLINE || sfType == ShpfileType.SHP_POLYLINEM || sfType == ShpfileType.SHP_POLYLINEZ)
                {
                    if (polygon.NumParts > 1)
                    {
                        status = ClipLineSFWithPoly.ClipLineSFWithMultiPartPolygon(ref inputSF, ref polygon, out resultFile, speedOptimized, copyAttributes);
                    }
                    else
                    {
                        if (speedOptimized)
                        {
                            status = ClipLineSFWithPoly.Fast_ClipLineSFWithPolygon(ref inputSF, ref polygon, out resultFile, copyAttributes);
                        }
                        else
                        {
                            status = ClipLineSFWithPoly.Accurate_ClipLineSFWithPolygon(ref inputSF, ref polygon, out resultFile, copyAttributes);
                        }
                    }
                }
                else
                {
                    gErrorMsg = "Not a valid Shapefile type.";
                    Debug.WriteLine(gErrorMsg);
                    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                    resultSF = resultFile;
                    MapWinUtility.Logger.Dbg(gErrorMsg);
                    return false;
                }
            }
            else
            {
                gErrorMsg = "NullShape is not a valid type.";
                Debug.WriteLine(gErrorMsg);
                MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                resultSF = resultFile;
                MapWinUtility.Logger.Dbg(gErrorMsg);
                return false;
            }
            resultSF = resultFile;
            MapWinUtility.Logger.Dbg("Finished ClipShapesWithPolygon");
            return status;
        }

        #endregion

        #region ClipPolygonSFWithLineSF
        /// <summary>
        /// In Development: Prone to errors! 
        /// For clipping multiple polygons with multiple lines.
        /// </summary>
        /// <param name="polySF">The polygon shapefile.</param>
        /// <param name="lineSF">The line shapefile.</param>
        /// <param name="resultSF">The result shapefile containing the polygon sections.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool ClipPolygonSFWithLineSF(ref Shapefile polySF, ref Shapefile lineSF, out Shapefile resultSF)
        {
            MapWinUtility.Logger.Dbg("ClipPolygonSFWithLineSF(polySF: " + polySF.ToString() + ",\n" +
                                     "                        lineSF: " + lineSF.ToString() + ",\n" +
                                     "                        resultSF: out");
            Error.ClearErrorLog();
            return ClipPolyWithLine.ClipPolySFWithLineSF(ref polySF, ref lineSF, out resultSF);
        }
        #endregion

        #region ClipPolygonWithLine()
        /// <summary>
        /// Divides a polygon into multiple sections depending on where a line crosses it. Saves the resulting
        /// polygon sections to a new polygon shapefile.
        /// </summary>
        /// <param name="polygon">The polygon to be divided.</param>
        /// <param name="line">The line that will be used to divide the polgyon.</param>
        /// <param name="resultSFPath">The full path to where the resulting polygons should be saved.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool ClipPolygonWithLine(ref Shape polygon, ref Shape line, ref string resultSFPath)
        {
            MapWinUtility.Logger.Dbg("ClipPolygonWithLine(polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                    line: " + Macro.ParamName(line) + ",\n" +
                                     "                    resultsSFPath: " + resultSFPath + ")");
            //save-to-disk, accurate form
            Error.ClearErrorLog();
            //always call the accurate version for this overload
            if (polygon.NumParts > 1)
            {
                return ClipPolyWithLine.ClipMultiPartPolyWithLine(ref polygon, ref line, ref resultSFPath, false);
            }
            else
            {
                return ClipPolyWithLine.Accurate_ClipPolygonWithLine(ref polygon, ref line, ref resultSFPath);
            }
        }
        /// <summary>
        /// Divides a polygon into multiple sections depending on where a line crosses it. Saves the resulting
        /// polygon sections to a new polygon shapefile.
        /// </summary>
        /// <param name="polygon">The polygon to be divided.</param>
        /// <param name="line">The line that will be used to divide the polgyon.</param>
        /// <param name="resultSF">The in-memory shapefile where resulting polygons should be saved.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool ClipPolygonWithLine(ref Shape polygon, ref Shape line, out Shapefile resultSF)
        {
            MapWinUtility.Logger.Dbg("ClipPolygonWithLine(polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                    line: " + Macro.ParamName(line) + ",\n" +
                                     "                    resultSF: out)");
            //in-memory, accurate form
            Error.ClearErrorLog();
            //always call accurate verion
            if (polygon.NumParts > 1)
            {
                return ClipPolyWithLine.ClipMultiPartPolyWithLine(ref polygon, ref line, out resultSF, false);
            }
            else
            {
                return ClipPolyWithLine.Accurate_ClipPolygonWithLine(ref polygon, ref line, out resultSF);
            }
        }

        /// <summary>
        /// Divides a polygon into multiple sections depending on where a line crosses it. Saves the resulting
        /// polygon sections to a new polygon shapefile.
        /// </summary>
        /// <param name="polygon">The polygon to be divided.</param>
        /// <param name="line">The line that will be used to divide the polgyon.</param>
        /// <param name="resultSFPath">The full path to where the resulting polygons should be saved.</param>
        /// <param name="speedOptimized">True if a line follows the assumption that only one intersection
        /// occurs for any 2pt segment. And only 2pt segments that have a point inside->outside or outside->inside
        /// will be tested for interesection.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool ClipPolygonWithLine(ref Shape polygon, ref Shape line, ref string resultSFPath, bool speedOptimized)
        {
            MapWinUtility.Logger.Dbg("CLipPolygonWithLIne(polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                    line: " + resultSFPath + ",\n" +
                                     "                    resultSFPath: " + resultSFPath + ",\n" +
                                     "                    speedOptimized: " + speedOptimized.ToString() + ")");
            //save-to-disk form
            Error.ClearErrorLog();
            if (polygon.NumParts > 1)
            {
                return ClipPolyWithLine.ClipMultiPartPolyWithLine(ref polygon, ref line, ref resultSFPath, speedOptimized);
            }
            else
            {
                if (speedOptimized == true)
                {
                    return ClipPolyWithLine.Fast_ClipPolygonWithLine(ref polygon, ref line, ref resultSFPath);
                }
                else
                {
                    return ClipPolyWithLine.Accurate_ClipPolygonWithLine(ref polygon, ref line, ref resultSFPath);
                }
            }
        }
        /// <summary>
        /// Divides a polygon into multiple sections depending on where a line crosses it. Saves the resulting
        /// polygon sections to a new polygon shapefile.
        /// </summary>
        /// <param name="polygon">The polygon to be divided.</param>
        /// <param name="line">The line that will be used to divide the polgyon.</param>
        /// <param name="resultSF">The in-memory shapefile where resulting polygons should be saved.</param>
        /// <param name="speedOptimized">True if a line follows the assumption that only one intersection
        /// occurs for any 2pt segment. And only 2pt segments that have a point inside->outside or outside->inside
        /// will be tested for interesection.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool ClipPolygonWithLine(ref Shape polygon, ref Shape line, out Shapefile resultSF, bool speedOptimized)
        {
            MapWinUtility.Logger.Dbg("ClipPolygonWithLine(polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                    line: " + Macro.ParamName(line) + ",\n" +
                                     "                    resultSF: out,\n" +
                                     "                    speedOptimized: " + speedOptimized.ToString() + ")");
            //in-memory form
            Error.ClearErrorLog();
            if (polygon.NumParts > 1)
            {
                return ClipPolyWithLine.ClipMultiPartPolyWithLine(ref polygon, ref line, out resultSF, speedOptimized);
            }
            if (speedOptimized == true)
            {
                return ClipPolyWithLine.Fast_ClipPolygonWithLine(ref polygon, ref line, out resultSF);
            }
            else
            {
                return ClipPolyWithLine.Accurate_ClipPolygonWithLine(ref polygon, ref line, out resultSF);
            }
        }
        #endregion

        #region Difference()
        /// <summary>Returns a new polygon representing the point set difference of poly1 with poly2.</summary>
        /// <param name="poly1">The 2D polygon in need of clipping.</param>
        /// <param name="poly2">The 2D polygon used for clipping poly1.</param>
        /// <returns>Shape containing all points in poly1 that are not in poly2.</returns> 
        public static Shape Difference(Shape poly1, Shape poly2)
        {
            MapWinUtility.Logger.Dbg("Difference(poly1: " + Macro.ParamName(poly1) + ",\n" +
                                     "           poly2: " + Macro.ParamName(poly2) + ")");
            Shape resultShp = new ShapeClass();
            MapWinGIS.Utils utils = new UtilsClass();
            resultShp = utils.ClipPolygon(PolygonOperation.DIFFERENCE_OPERATION, poly1, poly2);
            //Fix any orientation errors that may have arisen through GPC call
            if (resultShp != null && resultShp.NumParts > 1)
            {
                Globals.FixMultiPartPoly(ref resultShp);
            }
            MapWinUtility.Logger.Dbg("Finished Difference");
            return resultShp;
        }
        #endregion

        #region Union()
        /// <summary>Returns a new polygon representing the point set combination of polygon1 with polygon2.</summary>
        /// <param name="poly1">The 2D polygon in need of clipping.</param>
        /// <param name="poly2">The 2D polygon used for clipping poly1.</param>
        /// <returns>Shape containing all points in poly1 and poly2.</returns> 
        public static Shape Union(Shape poly1, Shape poly2)
        {
            MapWinUtility.Logger.Dbg("Union(poly1: " + Macro.ParamName(poly1) + "," + Environment.NewLine +
                                     "      poly2: " + Macro.ParamName(poly2) + ")");


            // There were potentially errors pertaining to the clockwise/counter-clockwise state
            // during the union method from utils.ClipPolygon.  This should provide an alternative that
            // should may be slightly slower, but might be more accurate. 
            // code added by Ted Dunsford 2/28/09

            // Paul Meems 22 july 2009: Sometime poly1 is a nullshape. Bug #1314
            if (poly1 == null || poly1.ShapeType == ShpfileType.SHP_NULLSHAPE)
            {
                MapWinUtility.Logger.Dbg("Error:Poly1 is not correct!");
                return null;
            }

            if (poly2 == null || poly2.ShapeType == ShpfileType.SHP_NULLSHAPE)
            {
                MapWinUtility.Logger.Dbg("Error:Poly2 is not correct!");
                return null;
            }

            // MapWinUtility.Logger.Dbg("Before NTS_Adapter.ShapeToGeometry. poly1: " + poly1.ShapeType.ToString());
            IGeometry p1 = NTS_Adapter.ShapeToGeometry(poly1);
            // MapWinUtility.Logger.Dbg("After NTS_Adapter.GeometryToShape. p1: " + p1.ToString());

            // MapWinUtility.Logger.Dbg("Before NTS_Adapter.ShapeToGeometry. poly2: " + poly2.ShapeType.ToString());
            IGeometry p2 = NTS_Adapter.ShapeToGeometry(poly2);
            // MapWinUtility.Logger.Dbg("After NTS_Adapter.GeometryToShape. p2: " + p2.ToString());

            var union = p1.Union(p2);

            // MapWinUtility.Logger.Dbg("Before NTS_Adapter.GeometryToShape. Union: " + union.ToString());
            var resultShp = NTS_Adapter.GeometryToShape(union);
            // MapWinUtility.Logger.Dbg("After NTS_Adapter.GeometryToShape. resultShp: " + resultShp.ShapeType.ToString());

            MapWinUtility.Logger.Dbg("Finished Union");
            return resultShp;
        }
        #endregion

        #region Intersection()
        /// <summary>Returns a new polygon representing the point set intersection of polygon1 with polygon2.</summary>
        /// <param name="poly1">The 2D polygon in need of clipping.</param>
        /// <param name="poly2">The 2D polygon used for clipping poly1.</param>
        /// <returns>Shape containing all points in poly1 that are also in poly2. Or NULL if no intersection have been found.</returns> 
        public static Shape Intersection(Shape poly1, Shape poly2)
        {
            // Paul Meems 22 July 2009: Added a try-catch, all for bug #1068
            try
            {
                //MapWinUtility.Logger.Dbg("Intersection(poly1: " + Macro.ParamName(poly1) + "," + Environment.NewLine +
                //                         "             poly2: " + Macro.ParamName(poly2) + ")");                
                MapWinGIS.Utils utils = new UtilsClass();
                //MapWinUtility.Logger.Dbg("Before utils.ClipPolygon");
                var resultShp = utils.ClipPolygon(PolygonOperation.INTERSECTION_OPERATION, poly1, poly2);
                //MapWinUtility.Logger.Dbg("After utils.ClipPolygon");
                //Fix any orientation errors that may have arisen through GPC call
                if (resultShp != null && resultShp.NumParts > 1)
                {
                    Globals.FixMultiPartPoly(ref resultShp);
                }
                //MapWinUtility.Logger.Dbg("Finished Intersection");

                // Paul Meems 28 sept. 2010, possible fix for issue #1671
                // return resultShp;
                if (resultShp != null)
                {
                    return resultShp.IsValid ? resultShp : resultShp.Buffer(0, 0);
                }

                return null;
            }
            catch (Exception ex)
            {
                MapWinUtility.Logger.Dbg(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Tests to see if there is an intersection between two polygons
        /// </summary>
        /// <param name="poly1">A Shape to test</param>
        /// <param name="poly2">A Second Shape to compare the first shape against</param>
        /// <returns>Boolean, true if the polygons intersect</returns>
        public static bool Intersects(Shape poly1, Shape poly2)
        {
            MapWinUtility.Logger.Dbg("Intersects(poly1: " + Macro.ParamName(poly1) + "," + Environment.NewLine +
                                     "           poly2: " + Macro.ParamName(poly2) + ")");
            Shape resultShp = new ShapeClass();
            MapWinGIS.Utils utils = new UtilsClass();
            resultShp = utils.ClipPolygon(PolygonOperation.INTERSECTION_OPERATION, poly1, poly2);
            MapWinUtility.Logger.Dbg("Finished Intersects");
            //If the shape is null, no intersections occured
            if (resultShp != null && resultShp.numPoints > 0)
                return true;
            return false;
        }
        #endregion

        #region SymmetricDifference()
        /// <summary>Returns a new polygon representing the point set exclusive OR of polygon1 with polygon2.</summary>
        /// <param name="poly1">The 2D polygon in need of clipping.</param>
        /// <param name="poly2">The 2D polygon used for clipping poly1.</param>
        /// <returns>Shape containing all points in poly1 and poly2, but not both.</returns> 
        public static Shape SymmetricDifference(Shape poly1, Shape poly2)
        {
            MapWinUtility.Logger.Dbg("SymmetricDifference(poly1: " + Macro.ParamName(poly1) + ",\n" +
                                     "                    poly2: " + Macro.ParamName(poly2) + ")");
            Shape resultShp = new ShapeClass();
            MapWinGIS.Utils utils = new UtilsClass();

            resultShp = utils.ClipPolygon(PolygonOperation.EXCLUSIVEOR_OPERATION, poly1, poly2);
            //Fix any orientation errors that may have arisen through GPC call
            if (resultShp != null && resultShp.NumParts > 1)
            {
                Globals.FixMultiPartPoly(ref resultShp);
            }
            MapWinUtility.Logger.Dbg("Finished SymmetricDifference");
            return resultShp;
        }
        #endregion

        #region MergeShapes()
        /// <summary>
        /// Merges two lines (at matching end points) or two polygons (along common border) to make one result shape.
        /// </summary>
        /// <param name="shp1">First shape.</param>
        /// <param name="shp2">Second shape.</param>
        /// <param name="resultShp">The result of merging shp1 and shp2.</param>
        /// <returns>True if shapes were merged, false otherwise.</returns>
        public static bool MergeShapes(ref Shape shp1, ref Shape shp2, out Shape resultShp)
        {
            MapWinUtility.Logger.Dbg("MergeShapes(shp1: " + Macro.ParamName(shp1) + ",\n" +
                                     "            shp2: " + Macro.ParamName(shp2) + ",\n" +
                                     "            resultShp: out)");
            Shapefile tempSF = new ShapefileClass();
            var tempPath = System.IO.Path.GetTempPath() + "tempSF.shp";
            DataManagement.DeleteShapefile(ref tempPath);
            //CDM 8/4/2006 tempSF.CreateNew(tempPath, shp1.ShapeType);
            Globals.PrepareResultSF(ref tempPath, ref tempSF, shp1.ShapeType);
            tempSF.StartEditingShapes(true, null);
            var shpIndex = 0;
            tempSF.EditInsertShape(shp1, ref shpIndex);
            shpIndex = 1;
            tempSF.EditInsertShape(shp2, ref shpIndex);
            MapWinUtility.Logger.Dbg("Finished MergeShapes");
            return Merge.MergeShapes(ref tempSF, 0, 1, out resultShp);
        }
        /// <summary>
        /// Merges two shapes within the same shapefile together.
        /// If lines, merging occurs at endpoints. If polygons, union is taken.
        /// </summary>
        /// <param name="sf">The shapefile containing the shapes to be merged.</param>
        /// <param name="indexOne">The index to the first shape.</param>
        /// <param name="indexTwo">The index to the second shape.</param>
        /// <param name="resultShp">The merged result shape.</param>
        /// <returns>True if the shapes were mereged, false otherwise.</returns>
        public static bool MergeShapes(ref Shapefile sf, int indexOne, int indexTwo, out Shape resultShp)
        {
            MapWinUtility.Logger.Dbg("MergeShapes(sf: " + Macro.ParamName(sf) + ",\n" +
                                     "            indexOne: " + indexOne.ToString() + ",\n" +
                                     "            indexTwo: " + indexTwo.ToString() + ",\n" +
                                     "            resultShp: out)");
            return Merge.MergeShapes(ref sf, indexOne, indexTwo, out resultShp);
        }
        #endregion

        #region BufferShape()
        //Angela Hillier 3/29/06
        /// <summary>
        /// Creates a buffer around the input shape at the distance specified.
        /// </summary>
        /// <param name="shape">The shape to be buffered.</param>
        /// <param name="distance">Distance from shape's border that the buffer should be created.</param>
        /// <param name="resultShp">The resulting buffer shape.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool BufferShape(ref Shape shape, double distance, out Shape resultShp)
        {
            MapWinUtility.Logger.Dbg("BufferShape(shape: " + Macro.ParamName(shape) + ",\n" +
                                     "            distance: " + distance.ToString() + ",\n" +
                                     "            resultShp: out)");
            Error.ClearErrorLog();
            Shape result = new ShapeClass();
            if (shape == null)
            {
                gErrorMsg = "Invalid shape, aborting BufferShape().";
                Error.SetErrorMsg(gErrorMsg);
                Debug.WriteLine(gErrorMsg);
                resultShp = result;
                MapWinUtility.Logger.Dbg(gErrorMsg);
                return false;
            }
            var sfType = shape.ShapeType;
            if (sfType == ShpfileType.SHP_POINT || sfType == ShpfileType.SHP_POINTM || sfType == ShpfileType.SHP_POINTZ)
            {
                Point point = new PointClass();
                point = shape.get_Point(0);
                return Buffer.BufferPoint(ref point, distance, 8, out resultShp);
            }
            else if (sfType == ShpfileType.SHP_POLYLINE || sfType == ShpfileType.SHP_POLYLINEM || sfType == ShpfileType.SHP_POLYLINEZ)
            {
                //buffer both sides with rounded caps
                return Buffer.BufferLine(ref shape, distance, Enumerations.Buffer_LineSide.Both, Enumerations.Buffer_CapStyle.Rounded, Enumerations.Buffer_EndCapStyle.Rounded, 8, out resultShp);
            }
            else if (sfType == ShpfileType.SHP_POLYGON || sfType == ShpfileType.SHP_POLYGONM || sfType == ShpfileType.SHP_POLYGONZ)
            {
                //holeTreatment == opposite. If polygon grows, hole shrinks.
                //use rounded caps
                return Buffer.BufferPolygon(ref shape, distance, Enumerations.Buffer_HoleTreatment.Opposite, Enumerations.Buffer_CapStyle.Rounded, 8, out resultShp);
            }
            else
            {
                gErrorMsg = "The shape does not have a recognizable type. Aborting BufferShape().";
                Error.SetErrorMsg(gErrorMsg);
                Debug.WriteLine(gErrorMsg);
                resultShp = result;
                MapWinUtility.Logger.Dbg(gErrorMsg);
                return false;
            }
        }
        #endregion

        #region BufferSF()
        //Angela Hillier 3/29/06
        /// <summary>
        /// Buffers all shapes in the input shapefile.
        /// </summary>
        /// <param name="inputSFPath">Full path to the input shapefile.</param>
        /// <param name="resultSFPath">Full path to the resulting buffer shapefile.</param>
        /// <param name="distance">Distance from a shape's border at which the corresponding buffer should be created.</param>
        /// <param name="uniteOverlaps">True if overlapping shapes should be combined.</param>
        /// <returns>False if an error occured, true otherwise.</returns>
        public static bool BufferSF(ref string inputSFPath, ref string resultSFPath, double distance, bool uniteOverlaps)
        {
            MapWinUtility.Logger.Dbg("BufferSF(inputSFPath: " + inputSFPath + ",\n" +
                                     "         resultSFPath: " + resultSFPath + ",\n" +
                                     "         distance: " + distance.ToString() + ",\n" +
                                     "         uniteOverlaps: " + uniteOverlaps.ToString());
            Error.ClearErrorLog();
            Shapefile inputSF = new ShapefileClass();
            inputSF.Open(inputSFPath, null);
            var sfType = inputSF.ShapefileType;
            inputSF.Close();
            if (sfType == ShpfileType.SHP_POINT || sfType == ShpfileType.SHP_POINTM || sfType == ShpfileType.SHP_POINTZ)
            {
                return Buffer.BufferPointSF(ref inputSFPath, ref resultSFPath, distance, uniteOverlaps, 8);
            }
            else if (sfType == ShpfileType.SHP_POLYLINE || sfType == ShpfileType.SHP_POLYLINEM || sfType == ShpfileType.SHP_POLYLINEZ)
            {
                return Buffer.BufferLineSF(ref inputSFPath, ref resultSFPath, distance, uniteOverlaps, Enumerations.Buffer_LineSide.Both, Enumerations.Buffer_CapStyle.Rounded, Enumerations.Buffer_EndCapStyle.Rounded, 8);
            }
            else if (sfType == ShpfileType.SHP_POLYGON || sfType == ShpfileType.SHP_POLYGONM || sfType == ShpfileType.SHP_POLYGONZ)
            {
                return Buffer.BufferPolygonSF(ref inputSFPath, ref resultSFPath, distance, uniteOverlaps, Enumerations.Buffer_HoleTreatment.Opposite, Enumerations.Buffer_CapStyle.Rounded, 8);
            }
            else
            {
                gErrorMsg = "The input shapefile does not have a recognizable type. Aborting BufferSF().";
                Error.SetErrorMsg(gErrorMsg);
                Debug.WriteLine(gErrorMsg);
                MapWinUtility.Logger.Dbg(gErrorMsg);
                return false;
            }
        }
        #endregion

        #region BufferLine()

        //  This function is obsolete and buggy, use BufferSegments instead
        //
        //Angela Hillier 3/29/06
        /// <summary>
        /// Creates a polygon buffer around the line at the distance specified.
        /// </summary>
        /// <param name="line">The line to be buffered.</param>
        /// <param name="distance">The distance from the line that the buffer should be created.
        /// Only positive values are allowed.</param>
        /// <param name="side">The side of the line that should be buffered.</param>
        /// <param name="capStyle">Edge treatment - pointed or rounded caps.</param>
        /// <param name="resultShp">The resulting buffer shape.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool BufferLine(ref Shape line, double distance, Enumerations.Buffer_LineSide side, Enumerations.Buffer_CapStyle capStyle, out Shape resultShp)
        {
            MapWinUtility.Logger.Dbg("BufferLine(line: " + Macro.ParamName(line) + ",\n" +
                                     "           side: " + side.ToString() + ",\n" +
                                     "           capStyle: " + capStyle.ToString() + ",\n" +
                                     "           resultShp: out)");
            Error.ClearErrorLog();

            Enumerations.Buffer_EndCapStyle endCapStyle;
            if ((int)capStyle == 0)
            {
                endCapStyle = Enumerations.Buffer_EndCapStyle.Pointed;
            }
            else
            {
                endCapStyle = Enumerations.Buffer_EndCapStyle.Rounded;
            }

            var sfType = new ShpfileType();
            sfType = line.ShapeType;
            //line, distance, buffSide (0 = both, 1 = left, 2 = right), capStyle (0 = pointed, 1 = round),
            //endCapStyle(0 = pointed, 1 = round), numQuadrants, shape
            if (sfType == ShpfileType.SHP_POLYLINE || sfType == ShpfileType.SHP_POLYLINEM || sfType == ShpfileType.SHP_POLYLINEZ)
            {
                return Buffer.BufferLine(ref line, distance, side, capStyle, endCapStyle, 8, out resultShp);
            }
            else
            {
                gErrorMsg = "Shape is invalid. Must be of type LINE.";
                Error.SetErrorMsg(gErrorMsg);
                Debug.WriteLine(gErrorMsg);
                Shape result = new ShapeClass();
                resultShp = result;
                MapWinUtility.Logger.Dbg(gErrorMsg);
                return false;
            }

        }
        #endregion

        #region BufferSegments()



        #endregion

        #region BufferLineSF()
        //Angela Hillier 3/29/06
        /// <summary>
        /// Creates a buffer around each line in the input shapefile.
        /// </summary>
        /// <param name="inputSFPath">Full path to the line shapefile.</param>
        /// <param name="resultSFPath">Full path to the resulting buffer shapefile. </param>
        /// <param name="distance">Distance from line at which buffer shape should be created.</param>
        /// <param name="uniteOverlaps">True if overlapping shapes should be combined.</param>
        /// <param name="side">Specifies which side of the line should be buffered.</param>
        /// <param name="capStyle">Edge treatment (Pointed or Rounded caps);</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool BufferLineSF(ref string inputSFPath, ref string resultSFPath, double distance, bool uniteOverlaps, Enumerations.Buffer_LineSide side, Enumerations.Buffer_CapStyle capStyle)
        {
            MapWinUtility.Logger.Dbg("BufferLineSF(inputSFPath: " + inputSFPath + ",\n" +
                                     "             resultSFPath: " + resultSFPath + ",\n" +
                                     "             distance: " + distance.ToString() + ",\n" +
                                     "             uniteOverlaps: " + uniteOverlaps.ToString() + ",\n" +
                                     "             side: " + side.ToString() + ",\n" +
                                     "             capStyle: " + capStyle.ToString() + ")");
            Error.ClearErrorLog();

            Enumerations.Buffer_EndCapStyle endCapStyle;
            if ((int)capStyle == 0)
            {
                endCapStyle = Enumerations.Buffer_EndCapStyle.Pointed;
            }
            else
            {
                endCapStyle = Enumerations.Buffer_EndCapStyle.Rounded;
            }

            Shapefile inputSF = new ShapefileClass();
            inputSF.Open(inputSFPath, null);
            var sfType = inputSF.ShapefileType;
            inputSF.Close();
            if (sfType == ShpfileType.SHP_POLYLINE || sfType == ShpfileType.SHP_POLYLINEM || sfType == ShpfileType.SHP_POLYLINEZ)
            {
                return Buffer.BufferLineSF(ref inputSFPath, ref resultSFPath, distance, uniteOverlaps, side, capStyle, endCapStyle, 8);
            }
            else
            {
                gErrorMsg = "Not a valid type. The input shapefile should be of type LINE.";
                Error.SetErrorMsg(gErrorMsg);
                Debug.WriteLine(gErrorMsg);
                MapWinUtility.Logger.Dbg(gErrorMsg);
                return false;
            }
        }
        #endregion

        #region BufferPolygon()
        //Angela Hillier 3/29/06
        /// <summary>
        /// Creates a buffer around (or inside of) the input polygon at the distance specified.
        /// </summary>
        /// <param name="polygon">The polygon shape to be buffered.</param>
        /// <param name="distance">The distance from the polygon's border that the buffer should be created.
        /// Positive or Negative values are allowed.</param>
        /// <param name="holeTreatment">For multiPart polygons with holes, specifies what should be done with holes.
        /// 0 = ignore (no holes will be included in result buffer), 1 = opposite (if distance is positive, hole will shrink),
        /// 2 = same (if distance is positive, hole will grow), 3 = keep original (the hole will not be buffered but will still exist inside of the buffer shape).</param>
        /// <param name="capStyle">Edge treatment. 0 = pointed, 1 = rounded.</param>
        /// <param name="resultShp">The resulting buffer shape.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool BufferPolygon(ref Shape polygon, double distance, Enumerations.Buffer_HoleTreatment holeTreatment, Enumerations.Buffer_CapStyle capStyle, out Shape resultShp)
        {
            MapWinUtility.Logger.Dbg("BufferPolygon(polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "              distance: " + distance.ToString() + ",\n" +
                                     "              holeTreatment: " + holeTreatment.ToString() + ",\n" +
                                     "              capStyle: " + capStyle.ToString() + ",\n" +
                                     "              resultShp: out)");
            Error.ClearErrorLog();

            var sfType = new ShpfileType();
            sfType = polygon.ShapeType;
            if (sfType == ShpfileType.SHP_POLYGON || sfType == ShpfileType.SHP_POLYGONM || sfType == ShpfileType.SHP_POLYGONZ)
            {
                return Buffer.BufferPolygon(ref polygon, distance, holeTreatment, capStyle, 8, out resultShp);
            }
            else
            {
                gErrorMsg = "Shape is invalid. Must be of type POLYGON.";
                Error.SetErrorMsg(gErrorMsg);
                Debug.WriteLine(gErrorMsg);
                Shape result = new ShapeClass();
                resultShp = result;
                MapWinUtility.Logger.Dbg(gErrorMsg);
                return false;
            }
        }
        #endregion

        #region BufferPolygonSF()
        //Angela Hillier 3/29/06
        /// <summary>
        /// Creates a buffer around each polygon in the input shapefile.
        /// </summary>
        /// <param name="inputSFPath">Full path to the polygon shapefile.</param>
        /// <param name="resultSFPath">Full path to the resulting buffer shapefile.</param>
        /// <param name="distance">Distance from polygon border at which the corresponding buffer should be created.</param>
        /// <param name="uniteOverlaps">True if overlapping buffers should be combined.</param>
        /// <param name="holeTreatment">For multiPart polygons with holes, specifies what should be done with holes.
        /// Ignore (no holes will be included in result buffer), Opposite (if distance is positive, hole will shrink),
        /// Same (if distance is positive, hole will grow), Original (the hole will not be buffered but will still exist inside of the buffer shape).</param>
        /// <param name="capStyle">Edge treatment (Pointed or Rounded caps).</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool BufferPolygonSF(ref string inputSFPath, ref string resultSFPath, double distance, bool uniteOverlaps, Enumerations.Buffer_HoleTreatment holeTreatment, Enumerations.Buffer_CapStyle capStyle)
        {
            MapWinUtility.Logger.Dbg("BufferPolygonSF(inputSFPath: " + inputSFPath + ",\n" +
                                     "                resultsSFPath: " + resultSFPath + ",\n" +
                                     "                distance: " + distance.ToString() + ",\n" +
                                     "                uniteOverlaps: " + uniteOverlaps.ToString() + ",\n" +
                                     "                holeTreatment: " + holeTreatment.ToString() + ",\n" +
                                     "                capStyle: " + capStyle.ToString() + ")");
            Error.ClearErrorLog();

            Shapefile inputSF = new ShapefileClass();
            inputSF.Open(inputSFPath, null);
            var sfType = inputSF.ShapefileType;
            inputSF.Close();
            if (sfType == ShpfileType.SHP_POLYGON || sfType == ShpfileType.SHP_POLYGONM || sfType == ShpfileType.SHP_POLYGONZ)
            {
                return Buffer.BufferPolygonSF(ref inputSFPath, ref resultSFPath, distance, uniteOverlaps, holeTreatment, capStyle, 8);
            }
            else
            {
                gErrorMsg = "Not a valid type. The input shapefile should be of type POLYGON.";
                Error.SetErrorMsg(gErrorMsg);
                Debug.WriteLine(gErrorMsg);
                MapWinUtility.Logger.Dbg(gErrorMsg);
                return false;
            }
        }
        #endregion

        #region Erase()
        /// <summary>
        /// Removes portions of the input shapefile that fall within the polygon's border.
        /// </summary>
        /// <param name="inputSFPath">The full path to the input shapefile.</param>
        /// <param name="polygon">The overlay polygon.</param>
        /// <param name="resultSFPath">The full path to the resulting shapefile.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool Erase(ref string inputSFPath, ref Shape polygon, ref string resultSFPath)
        {
            MapWinUtility.Logger.Dbg("Erase(inputSFPath: " + inputSFPath + ",\n" +
                                     "      polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "      resultSFPath: " + resultSFPath + ")");
            return Erase(ref inputSFPath, ref polygon, ref resultSFPath, false, false);
        }

        /// <summary>
        /// Removes portions of the input shapefile that fall within the polygon's border.
        /// </summary>
        /// <param name="inputSFPath">The full path to the input shapefile.</param>
        /// <param name="polygon">The overlay polygon.</param>
        /// <param name="resultSFPath">The full path to the resulting shapefile.</param>
        /// <param name="SkipMWShapeID">Indicates whether to skip creating an MWShapeID field in the result.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        /// 
        public static bool Erase(ref string inputSFPath, ref Shape polygon, ref string resultSFPath, bool SkipMWShapeID)
        {
            MapWinUtility.Logger.Dbg("Erase(inputSFPath: " + inputSFPath + ",\n" +
                                        "      polygon: " + Macro.ParamName(polygon) + ",\n" +
                                        "      resultSFPath: " + resultSFPath + ",\n" +
                                        "      SkipMWShapeID: " + SkipMWShapeID.ToString() + ")");

            return Erase(ref inputSFPath, ref polygon, ref resultSFPath, SkipMWShapeID, false);
        }

        /// <summary>
        /// Removes portions of the input shapefile that fall within the polygon's border.
        /// </summary>
        /// <param name="inputSFPath">The full path to the input shapefile.</param>
        /// <param name="polygon">The overlay polygon.</param>
        /// <param name="resultSFPath">The full path to the resulting shapefile.</param>
        /// <param name="SkipMWShapeID">Indicates whether to skip creating an MWShapeID field in the result.</param>
        /// <param name="CopyAttributes">Indicates whether to copy attributes or not.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        /// 
        public static bool Erase(ref string inputSFPath, ref Shape polygon, ref string resultSFPath, bool SkipMWShapeID, bool CopyAttributes)
        {
            MapWinUtility.Logger.Dbg("Erase(intputSFPath: " + inputSFPath + ",\n" +
                                     "      polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "      resultSFPath: " + resultSFPath + ",\n" +
                                     "      SkipWMShapeID: " + SkipMWShapeID.ToString() + ",\n" +
                                     "      CopyAttributes: " + CopyAttributes.ToString() + ")");
            Error.ClearErrorLog();
            var status = false;

            if (CopyAttributes)
            {   //we want to skip the MWShapeID and assume there will be one on the input shapefile
                SkipMWShapeID = true;
            }

            Shapefile inputSF = new ShapefileClass();
            inputSF.Open(inputSFPath, null);
            if (inputSF.NumShapes > 0 && polygon.numPoints > 0)
            {
                var sfType = new ShpfileType();
                sfType = inputSF.ShapefileType;
                Shapefile resultSF = new ShapefileClass();
                //create the result shapefile if it does not already exist
                if (Globals.PrepareResultSF(ref resultSFPath, ref resultSF, sfType, true) == false)
                {
                    return false;
                }

                if (sfType == ShpfileType.SHP_POLYGON || sfType == ShpfileType.SHP_POLYGONM || sfType == ShpfileType.SHP_POLYGONZ)
                {
                    status = MapWinGeoProc.Erase.ErasePolySFWithPoly(ref inputSF, ref polygon, ref resultSF, CopyAttributes);
                }
                else if (sfType == ShpfileType.SHP_POINT || sfType == ShpfileType.SHP_POINTM || sfType == ShpfileType.SHP_POINTZ)
                {
                    status = MapWinGeoProc.Erase.ErasePointSFWithPoly(ref inputSF, ref polygon, ref resultSF, CopyAttributes);
                }
                else if (sfType == ShpfileType.SHP_POLYLINE || sfType == ShpfileType.SHP_POLYLINEM || sfType == ShpfileType.SHP_POLYLINEZ)
                {
                    status = MapWinGeoProc.Erase.EraseLineSFWithPoly(ref inputSF, ref polygon, ref resultSF, CopyAttributes);
                }
                else
                {
                    status = false;
                    gErrorMsg = "The shapefile type is invalid.";
                    Error.SetErrorMsg(gErrorMsg);
                    Debug.WriteLine(gErrorMsg);
                    MapWinUtility.Logger.Dbg(gErrorMsg);
                }
                if (status == true && !SkipMWShapeID)
                {
                    //insert shape ID's into the result table
                    Globals.DoInsertIDs(ref resultSF);
                }
                resultSF.StopEditingShapes(true, true, null);
                resultSF.Close();
            }
            else
            {
                gErrorMsg = "Either the inputSF or the polygon shape is invalid.";
                Error.SetErrorMsg(gErrorMsg);
                Debug.WriteLine(gErrorMsg);
                status = false;
                MapWinUtility.Logger.Dbg(gErrorMsg);
            }
            inputSF.Close();
            MapWinUtility.Logger.Dbg("Finished Erase");
            return status;
        }

        /// <summary>
        /// Removes portions of the input shapefile that fall within the erase polygons.
        /// </summary>
        /// <param name="inputSFPath">Full path to input shapefile.</param>
        /// <param name="polySFPath">Full path to the erase polygon shapefile.</param>
        /// <param name="resultSFPath">Full path to where the result shapefile should be saved.</param>
        /// <returns>Fals if an error was encountered, true otherwise.</returns>
        public static bool Erase(ref string inputSFPath, ref string polySFPath, ref string resultSFPath)
        {
            MapWinUtility.Logger.Dbg("Erase(inputSFPath: " + inputSFPath + ",\n" +
                                     "      polySFPath: " + polySFPath + ",\n" +
                                     "      resultSFPath: " + resultSFPath + ")");
            return Erase(ref inputSFPath, ref polySFPath, ref resultSFPath, false);
        }
        /// <summary>
        /// Removes portions of the input shapefile that fall within the erase polygons.
        /// </summary>
        /// <param name="inputSFPath">Full path to input shapefile.</param>
        /// <param name="polySFPath">Full path to the erase polygon shapefile.</param>
        /// <param name="resultSFPath">Full path to where the result shapefile should be saved.</param>
        /// <param name="SkipMWShapeID">Indicates whether to skip creating an MWShapeID field in the result.</param>
        /// <returns>Fals if an error was encountered, true otherwise.</returns>
        public static bool Erase(ref string inputSFPath, ref string polySFPath, ref string resultSFPath, bool SkipMWShapeID)
        {
            MapWinUtility.Logger.Dbg("Erase(inputSFPath: " + inputSFPath + ",\n" +
                                     "      polySFPath: " + polySFPath + ",\n" +
                                     "      resultSFPath: " + resultSFPath + ",\n" +
                                     "      SkipMWShapeID: " + SkipMWShapeID.ToString() + ")");
            Error.ClearErrorLog();
            var status = false;
            Shapefile inputSF = new ShapefileClass();
            inputSF.Open(inputSFPath, null);
            Shapefile polySF = new ShapefileClass();
            polySF.Open(polySFPath, null);

            if (inputSF.NumShapes > 0 && polySF.NumShapes > 0)
            {
                var sfType = new ShpfileType();
                sfType = inputSF.ShapefileType;
                Shapefile resultSF = new ShapefileClass();
                //create the result shapefile if it does not already exist
                if (Globals.PrepareResultSF(ref resultSFPath, ref resultSF, sfType, true) == false)
                {
                    return false;
                }

                if (sfType == ShpfileType.SHP_POLYGON || sfType == ShpfileType.SHP_POLYGONM || sfType == ShpfileType.SHP_POLYGONZ)
                {
                    status = MapWinGeoProc.Erase.ErasePolySFWithPolySF(ref inputSF, ref polySF, ref resultSF);
                }
                else if (sfType == ShpfileType.SHP_POINT || sfType == ShpfileType.SHP_POINTM || sfType == ShpfileType.SHP_POINTZ)
                {
                    status = MapWinGeoProc.Erase.ErasePointSFWithPolySF(ref inputSF, ref polySF, ref resultSF);
                }
                else if (sfType == ShpfileType.SHP_POLYLINE || sfType == ShpfileType.SHP_POLYLINEM || sfType == ShpfileType.SHP_POLYLINEZ)
                {
                    status = MapWinGeoProc.Erase.EraseLineSFWithPolySF(ref inputSF, ref polySF, ref resultSF);
                }
                else
                {
                    status = false;
                    gErrorMsg = "The shapefile type is invalid.";
                    Error.SetErrorMsg(gErrorMsg);
                    Debug.WriteLine(gErrorMsg);
                    MapWinUtility.Logger.Dbg(gErrorMsg);
                }
                if (status == true && !SkipMWShapeID)
                {
                    //insert shape ID's into the result table
                    Globals.DoInsertIDs(ref resultSF);
                }
                resultSF.StopEditingShapes(true, true, null);
                resultSF.Close();
            }
            else
            {
                gErrorMsg = "Either the inputSF or the polygon shape is invalid.";
                Error.SetErrorMsg(gErrorMsg);
                Debug.WriteLine(gErrorMsg);
                status = false;
                MapWinUtility.Logger.Dbg(gErrorMsg);
            }
            inputSF.Close();
            polySF.Close();
            return status;
        }
        #endregion

        #region Identity
        /// <summary>
        /// Computes a geometric intersection of the input shapes and the indentity shapes.
        /// The input shapes, or portions thereof that overlap the identity shapes, will get
        /// the attributes of those identity shapes.
        /// </summary>
        /// <param name="inputSFPath">The full path to the input shapefile.</param>
        /// <param name="identitySFPath">The full path to the identity shapefile.</param>
        /// <param name="resultSFPath">The full path to the result shapefile.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool Identity(ref string inputSFPath, ref string identitySFPath, ref string resultSFPath)
        {
            Shapefile inputSF = new ShapefileClass();
            inputSF.Open(inputSFPath, null);
            var inputSFType = inputSF.ShapefileType;

            Shapefile identitySF = new ShapefileClass();
            identitySF.Open(identitySFPath, null);

            Shapefile resultSF = new ShapefileClass();
            resultSF.CreateNew(resultSFPath, inputSFType);
            resultSF.Projection = inputSF.Projection;

            var status = MapWinGeoProc.Selection.ExportShapesWithPolygons(ref inputSF, ref identitySF, ref resultSF);

            inputSF.Close();
            identitySF.Close();
            resultSF.StopEditingShapes(true, true, null);
            resultSF.Close();
            return status;
        }

        #region DeleteShapeFile()
        //this function is used to delete temporary shapefiles that were
        //saved to disk. There are three files for every one shapefile, so
        //all three must be deleted.
        private void DeleteShapeFile(string shapeFilePath)
        {
            // Paul Meems 22 july 2009:
            // It's better to use global function:
            /*
            System.IO.File.Delete(shapeFilePath + ".shp");
            System.IO.File.Delete(shapeFilePath + ".dbf");
            System.IO.File.Delete(shapeFilePath + ".shx");
            */
            DataManagement.DeleteShapefile(ref shapeFilePath);
        }
        #endregion

        #endregion Identity

        #region GetShapeNearestToPoint
        /// <summary>
        /// Function which will output the index and distance to the nearest shape in the provided shapefile to the provided point. -1 will be returned if nothing is found
        /// </summary>
        /// <param name="InputSF"></param>
        /// <param name="InputPoint"></param>
        /// <param name="OutIndex"></param>
        /// <param name="OutDistance"></param>
        public static void GetShapeNearestToPoint(ref Shapefile InputSF, ref Point InputPoint, out int OutIndex, out double OutDistance)
        {
            var lim = InputSF.Extents;
            object retShapes = null;
            InputSF.SelectShapes(lim, 0.0, SelectMode.INTERSECTION, ref retShapes);
            var shapes = (int[])retShapes;
            doGetShapeNearestToPoint(ref InputSF, ref InputPoint, ref shapes, out OutIndex, out OutDistance);
        }

        /// <summary>
        /// Function which will output the index and distance to the nearest shape in the provided shapefile to the provided point, limited by the extents box inputted such that any values outside of it will not be included. -1 will be returned if nothing is found
        /// </summary>
        /// <param name="InputSF"></param>
        /// <param name="InputPoint"></param>
        /// <param name="LimitBox"></param>
        /// <param name="OutIndex"></param>
        /// <param name="OutDistance"></param>
        public static void GetShapeNearestToPoint(ref Shapefile InputSF, ref Point InputPoint, ref Extents LimitBox, out int OutIndex, out double OutDistance)
        {
            object retShapes = null;
            InputSF.SelectShapes(LimitBox, 0.0, SelectMode.INTERSECTION, ref retShapes);
            var shapes = (int[])retShapes;
            doGetShapeNearestToPoint(ref InputSF, ref InputPoint, ref shapes, out OutIndex, out OutDistance);
        }

        /// <summary>
        /// Function which will output the index and distance to the nearest shape in the provided shapefile to the provided point, limited by an extents generated by the size inputted such that any values outside of it will not be included. -1 will be returned if nothing is found
        /// </summary>
        /// <param name="InputSF"></param>
        /// <param name="InputPoint"></param>
        /// <param name="ExtentSize"></param>
        /// <param name="OutIndex"></param>
        /// <param name="OutDistance"></param>
        public static void GetShapeNearestToPoint(ref Shapefile InputSF, ref Point InputPoint, double ExtentSize, out int OutIndex, out double OutDistance)
        {
            var ext = new Extents();
            ext.SetBounds(InputPoint.x, InputPoint.y, 0, InputPoint.x, InputPoint.y, 0);
            object retShapes = null;
            InputSF.SelectShapes(ext, ExtentSize * 2, SelectMode.INTERSECTION, ref retShapes);
            var shapes = (int[])retShapes;

            doGetShapeNearestToPoint(ref InputSF, ref InputPoint, ref shapes, out OutIndex, out OutDistance);
        }

        /// <summary>
        /// Function which will output the index and distance to the nearest shape in the provided shapefile to the provided point, limited by index list inputted such that any values outside of it will not be included. -1 will be returned if nothing is found
        /// </summary>
        /// <param name="InputSF"></param>
        /// <param name="InputPoint"></param>
        /// <param name="IndexList"></param>
        /// <param name="OutIndex"></param>
        /// <param name="OutDistance"></param>
        public static void GetShapeNearestToPoint(ref Shapefile InputSF, ref Point InputPoint, ref int[] IndexList, out int OutIndex, out double OutDistance)
        {
            doGetShapeNearestToPoint(ref InputSF, ref InputPoint, ref IndexList, out OutIndex, out OutDistance);
        }

        private static void doGetShapeNearestToPoint(ref Shapefile InputSF, ref Point InputPoint, ref int[] IndexList, out int OutIndex, out double OutDistance)
        {
            Shape tmpShape;
            var minIndex = -1;
            double tmpDist;
            var minDist = 10000000.0;
            var proj = InputSF.Projection;
            string units;
            if (proj != null)
            {
                units = proj.Substring(proj.IndexOf("units=") + 6);
                units = units.Substring(0, units.IndexOf("+")).Trim();
            }
            else
            {
                var tmpX = InputSF.Extents.xMax;
                var tmpstr = Math.Floor(tmpX).ToString();

                if (tmpstr.Length > 4)
                {
                    units = "";
                }
                else
                {
                    units = "lat/long";
                }
            }

            for (var i = 0; i < IndexList.Length; i++)
            {
                tmpShape = InputSF.get_Shape(IndexList[i]);
                for (var j = 0; j < tmpShape.numPoints; j++)
                {
                    tmpDist = Distance(InputPoint.x, InputPoint.y, tmpShape.get_Point(j).x, tmpShape.get_Point(j).y, units);
                    if (tmpDist < minDist)
                    {
                        minIndex = IndexList[i];
                        minDist = tmpDist;
                    }
                }
            }

            OutIndex = minIndex;
            OutDistance = minDist;
        }

        /// <summary>
        /// Gives the distance between two projected points
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="Units"></param>
        /// <returns></returns>
        public static double Distance(double x1, double y1, double x2, double y2, string Units)
        {
            if ((Units.ToLower().Trim() == "lat/long"))
            {
                return LLDistance(y1, x1, y2, x2);
            }
            else
            {
                return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
            }
        }

        /// <summary>
        /// Gives the distance between two lat/long points
        /// </summary>
        /// <param name="Lat1"></param>
        /// <param name="Long1"></param>
        /// <param name="Lat2"></param>
        /// <param name="Long2"></param>
        /// <returns></returns>
        public static double LLDistance(double Lat1, double Long1, double Lat2, double Long2)
        {
            var DegToRadians = (2 * Math.PI) / 360;
            double RadiusEarth = 3963; //in miles

            var result = RadiusEarth * Math.Acos(Math.Sin(Lat1 * DegToRadians)
                * Math.Sin(Lat2 * DegToRadians)
                + Math.Cos(Lat1 * DegToRadians)
                * Math.Cos(Lat2 * DegToRadians)
                * Math.Cos(Long2 * DegToRadians - Long1 * DegToRadians));
            result = result * 1609.344;
            return result;
        }
        #endregion

        #region Spatial Join
        /// <summary>
        /// A spatial join function which will append the attributes of the Join shapefile to the attributes of the Target shapefile according to a Join type (Only nearest supported so far).
        /// </summary>
        /// <param name="TargetSFPath"></param>
        /// <param name="JoinSFPath"></param>
        /// <param name="OutputSFPath"></param>
        /// <param name="JoinType"></param>
        public static void SpatialJoin(string TargetSFPath, string JoinSFPath, string OutputSFPath, SpatialJoinTypes JoinType)
        {
            doSpatialJoinPreProc(TargetSFPath, JoinSFPath, OutputSFPath, JoinType, -1);
        }

        /// <summary>
        /// A spatial join function which will append the attributes of the Join shapefile to the attributes of the Target shapefile according to a Join type (Only nearest supported so far). The nearest operation will be limited by the search radius provided.
        /// </summary>
        /// <param name="TargetSFPath"></param>
        /// <param name="JoinSFPath"></param>
        /// <param name="OutputSFPath"></param>
        /// <param name="JoinType"></param>
        /// <param name="SearchRadius"></param>
        public static void SpatialJoin(string TargetSFPath, string JoinSFPath, string OutputSFPath, SpatialJoinTypes JoinType, double SearchRadius)
        {
            doSpatialJoinPreProc(TargetSFPath, JoinSFPath, OutputSFPath, JoinType, SearchRadius);
        }

        private static void doSpatialJoinPreProc(string TargetSFPath, string JoinSFPath, string OutputSFPath, SpatialJoinTypes JoinType, double SearchRadius)
        {
            var JoinSF = new Shapefile();
            var OutputSF = new Shapefile();
            if (File.Exists(OutputSFPath))
            {
                DataManagement.DeleteShapefile(ref OutputSFPath);
            }
            OutputSF.Open(TargetSFPath, null);
            OutputSF.SaveAs(OutputSFPath, null);
            JoinSF.Open(JoinSFPath, null);

            if (JoinType == SpatialJoinTypes.Nearest)
            {
                doSpatialJoinNearest(ref JoinSF, ref OutputSF, JoinType, SearchRadius);
            }

            JoinSF.Close();
            OutputSF.Close();
        }

        private static void doSpatialJoinNearest(ref Shapefile JoinSF, ref Shapefile OutputSF, SpatialJoinTypes JoinType, double SearchRadius)
        {
            Shape baseShape;
            Point basePoint;
            var newFieldIdxStart = OutputSF.NumFields;
            int fieldIdx;
            int outIndex;
            double outDist;

            OutputSF.StartEditingShapes(true, null);

            for (var i = 0; i < JoinSF.NumFields; i++)
            {
                var newField = new Field();
                var currField = JoinSF.get_Field(i);
                if ((currField.Name == "ID") || (currField.Name == "MWShapeID"))
                {
                    newField.Name = currField.Name + "1";
                }
                else
                {
                    newField.Name = currField.Name;
                }
                newField.Type = currField.Type;
                newField.Precision = currField.Precision;
                newField.Width = currField.Width;
                fieldIdx = OutputSF.NumFields;
                OutputSF.EditInsertField(newField, ref fieldIdx, null);
            }

            for (var i = 0; i < OutputSF.NumShapes; i++)
            {
                baseShape = OutputSF.get_Shape(i);
                basePoint = new Point();
                basePoint.x = baseShape.get_Point(0).x;
                basePoint.y = baseShape.get_Point(0).y;

                for (var j = 0; j < JoinSF.NumShapes; j++)
                {
                    if (SearchRadius != -1.0)
                    {
                        GetShapeNearestToPoint(ref JoinSF, ref basePoint, SearchRadius, out outIndex, out outDist);
                    }
                    else
                    {
                        GetShapeNearestToPoint(ref JoinSF, ref basePoint, out outIndex, out outDist);
                    }
                    if (outIndex != -1)
                    {
                        for (var z = 0; z < JoinSF.NumFields; z++)
                        {
                            OutputSF.EditCellValue(newFieldIdxStart + z, i, JoinSF.get_CellValue(z, outIndex));
                        }
                    }
                }
            }
            OutputSF.StopEditingShapes(true, true, null);
        }

        #endregion

        /// <summary>
        /// Computes the convex hull of a polygon. 
        /// The convex hull is the smallest convex polygon that contains all the points in the input polgyon. 
        /// Uses the Graham Scan algorithm. 
        /// </summary>
        /// <param name="polygon">The input polygon shape.</param>
        /// <param name="resultShp">The resulting convex hull of the input polygon.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        /// <remarks>Implemented by Paul Meems on August 10, 2011 </remarks>
        public static bool ConvexHull(ref Shape polygon, ref Shape resultShp)
        {
            try
            {
                resultShp = polygon.ConvexHull();
            }
            catch (Exception exception)
            {
                throw new Exception("Error in ConvexHull: " + polygon.get_ErrorMsg(polygon.LastErrorCode), exception);
            }
            
            return true;
        }

        /// <summary>Creates all centroids of all shapes of the shapefile</summary>
        /// <param name="sf">The input shapefile</param>
        /// <param name="newFilename">The name of the centroid file</param>
        /// <param name="allParts">Create centroids for all parts of a shape</param>
        /// <param name="useCenter">Use center or centroid algoritm. With centroid point can be outside the shape</param>
        /// <returns>The shapefile with the centroids</returns>
        /// <exception cref="Exception"></exception>
        /// <remarks>Implemented by Paul Meems on August 10, 2011 </remarks>
        public static Shapefile CreateCentroids(Shapefile sf, string newFilename, bool allParts, bool useCenter)
        {
            if (sf.ShapefileType != ShpfileType.SHP_POLYGON)
            {
                return null;
            }

            var sfCentroid = new Shapefile();
            DataManagement.DeleteShapefile(ref newFilename);
            if (!sfCentroid.CreateNewWithShapeID(newFilename, ShpfileType.SHP_POINT))
            {
                throw new Exception("Could not create new shapefile: " + sfCentroid.get_ErrorMsg(sfCentroid.LastErrorCode));
            }

            if (sfCentroid == null)
            {
                throw new Exception("Shapefile is null");
            }

            var numShapes = 0;
            var fldIndexParentID = sfCentroid.NumFields;
            if (!sfCentroid.EditInsertField(new Field { Name = "ParentID", Type = FieldType.INTEGER_FIELD, Width = 10 }, ref fldIndexParentID, null))
            {
                throw new Exception("Error in sf.EditInsertField: " + sfCentroid.get_ErrorMsg(sfCentroid.LastErrorCode));
            }

            var fldIndexX = sfCentroid.NumFields;
            if (!sfCentroid.EditInsertField(new Field { Name = "X", Type = FieldType.DOUBLE_FIELD, Width = 20, Precision = 6 }, ref fldIndexX, null))
            {
                throw new Exception("Error in sf.EditInsertField: " + sfCentroid.get_ErrorMsg(sfCentroid.LastErrorCode));
            }

            var fldIndexY = sfCentroid.NumFields;
            if (!sfCentroid.EditInsertField(new Field { Name = "Y", Type = FieldType.DOUBLE_FIELD, Width = 20, Precision = 6 }, ref fldIndexY, null))
            {
                throw new Exception("Error in sf.EditInsertField: " + sfCentroid.get_ErrorMsg(sfCentroid.LastErrorCode));
            }

            for (var i = 0; i < sf.NumShapes; i++)
            {
                var shpOrg = sf.get_Shape(i);
                var centroidPoints = new List<Point>();

                // TODO: Handle multi-parts:
                if (allParts && shpOrg.NumParts > 1)
                {
                    for (var j = 0; j < shpOrg.NumParts; j++)
                    {
                        var part = shpOrg.get_PartAsShape(j);
                        var centroid = useCenter ? part.Center : part.Centroid;
                        centroidPoints.Add(centroid);
                    }
                }
                else
                {
                    var centroid = useCenter ? shpOrg.Center : shpOrg.Centroid;
                    centroidPoints.Add(centroid);
                }

                // Add all centroid points as shape to the new shapefile:
                foreach (var centroidPoint in centroidPoints)
                {
                    // Create shape:
                    var shp = new Shape();
                    if (!shp.Create(ShpfileType.SHP_POINT))
                    {
                        throw new Exception("Could not create shape: " + shp.get_ErrorMsg(shp.LastErrorCode));
                    }

                    // Insert the point into the shape
                    var numPoint = 0;
                    if (!shp.InsertPoint(centroidPoint, ref numPoint))
                    {
                        throw new Exception(
                            string.Format(
                                "Error! In shape.InsertPoint[{0}]: {1}", numPoint, shp.get_ErrorMsg(shp.LastErrorCode)));
                    }

                    // Add centroid:
                    sfCentroid.EditInsertShape(shp, ref numShapes);

                    // Add ParentID:
                    sfCentroid.EditCellValue(fldIndexParentID, numShapes, i);

                    // Add X coordinate:
                    sfCentroid.EditCellValue(fldIndexX, numShapes, centroidPoint.x);

                    // Add Y coordinate:
                    sfCentroid.EditCellValue(fldIndexY, numShapes, centroidPoint.y);

                    numShapes++;
                }
            }

            // Set projection:
            sfCentroid.Projection = sf.Projection;
            sfCentroid.GeoProjection = sf.GeoProjection;

            // Save centroid file:
            sfCentroid.StopEditingShapes(true, true, null);

            return sfCentroid;
        }

        #region Functions Not Implemented

        /// <summary>
        /// Selects all shapes in the input shapefile based on a field value.
        /// </summary>
        /// <param name="inputSFPath">The full path to the input shapefile.</param>
        /// <param name="fieldID">The table ID of the field used for selection.</param>
        /// <param name="testValLoc">The location of a value used for comparison purposes.</param>
        /// <param name="compType">The type of comparison to be performed (==, >=, !=, etc)</param>
        /// <param name="resultSFPath">The full path to the result shapefile.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool SelectByAttribute(ref string inputSFPath, int fieldID, int testValLoc, int compType, ref string resultSFPath)
        {
            //TODO: Implement this function
            Error.ClearErrorLog();
            gErrorMsg = "This function is not yet implemented.";
            Error.SetErrorMsg(gErrorMsg);
            Debug.WriteLine(gErrorMsg);

            return false;
        }

        #endregion

    }
}
