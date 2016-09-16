//File name: ClipPolySFWithPoly.cs
//Description: Internal class, provides functions for clipping a polygon shapefile with an outside polygon.
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
//10-17-05 ah - Angela Hillier - Created original class and functions. 							
//********************************************************************************************************
using System;
using System.Diagnostics;

namespace MapWinGeoProc
{
	/// <summary>
	/// Functions for clipping a polygon shapefile with an outside polygon.
	/// </summary>
	internal class ClipPolySFWithPoly
	{
		private static string gErrorMsg = "";

		#region ClipPolygonSFWithPolygon() -- save-to-disk and save-to-memory versions
		
		#region Save-to-Disk version
		/// <summary>
		/// Returns all portions of the shapefile polygons that fall within the clipper polygon.
		/// </summary>
		/// <param name="polySFPath">The full path to the shapefile of polygons to be clipped.</param>
		/// <param name="polygon">The polygon used for clipping the shapefile.</param>
		/// <param name="resultSFPath">The full path to the result file for where the clipped polygons should be saved.</param>
		/// <param name="copyAttributes">True if copying attrs</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
		public static bool ClipPolygonSFWithPolygon(ref string polySFPath, ref MapWinGIS.Shape polygon, ref string resultSFPath, bool copyAttributes)
		{
            MapWinUtility.Logger.Dbg("ClipPolygonSFWithPolygon(polySFPath: " + polySFPath + "," + Environment.NewLine +
                                     "                         polygon: + " + Macro.ParamName(polygon) + "," + Environment.NewLine +
                                     "                         resultsSFPath: " + resultSFPath + "," + Environment.NewLine +
                                     "                         copyAttributes: " + copyAttributes + Environment.NewLine);

			MapWinGIS.Shapefile resultSF = new MapWinGIS.ShapefileClass();
			MapWinGIS.Shapefile polySF = new MapWinGIS.ShapefileClass();
            if (!polySF.Open(polySFPath, null))
            {
                gErrorMsg = "Error opening shapefile: " + polySFPath;
                Debug.WriteLine(gErrorMsg);
                Error.SetErrorMsg(gErrorMsg);
                MapWinUtility.Logger.Dbg(gErrorMsg);
                return false;
            }
			MapWinGIS.ShpfileType sfType = polySF.ShapefileType;
			int shapeIndex = 0;//insert new shapes at the beginning of the shapefile
            string tmpName;

            if (Globals.PrepareResultSF(ref resultSFPath, ref resultSF, sfType, copyAttributes) == false)
			{
                polySF.Close();
				return false;
			}

            if (copyAttributes)
            {
                MapWinGIS.Field tmpField = null, currField = null;
                for (int f = 0; f <= polySF.NumFields - 1; f++)
                {
                    tmpField = new MapWinGIS.Field();
                    currField = polySF.get_Field(f);
                    tmpName = currField.Name;
                    if (tmpName.Contains("MWShapeID"))
                    {
                        tmpField.Name = "Last_" + tmpName;
                    }
                    else
                    {
                        tmpField.Name = tmpName;
                    }

                    tmpField.Width = currField.Width;
                    tmpField.Type = currField.Type;
                    tmpField.Precision = currField.Precision;
                    tmpField.Key = currField.Key;
                    if (!resultSF.EditInsertField(tmpField, ref f, null))
                    {
                        gErrorMsg = "Error in adding field to shapefile: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
                        Debug.WriteLine(gErrorMsg);
                        Error.SetErrorMsg(gErrorMsg);
                        MapWinUtility.Logger.Dbg(gErrorMsg);
                        return false;
                    }
                }
            }

			if(sfType == MapWinGIS.ShpfileType.SHP_POLYGON || sfType == MapWinGIS.ShpfileType.SHP_POLYGONM || sfType == MapWinGIS.ShpfileType.SHP_POLYGONZ)
			{
				MapWinGIS.Shape resultShape = new MapWinGIS.ShapeClass();
				MapWinGIS.Extents shpExtents = new MapWinGIS.ExtentsClass();			
				int numShapes = polySF.NumShapes;
				bool boundsIntersect = false;	
	
				for(int i = 0; i <= numShapes-1; i++)
				{	
					MapWinGIS.Shape currPoly = new MapWinGIS.ShapeClass();
					currPoly = polySF.get_Shape(i);
					//check to see if bounds intersect before sending shape to GPC clip function
					boundsIntersect = Globals.CheckBounds(ref currPoly, ref polygon);

					if(boundsIntersect)
					{
						//find the shape resulting from intersection
						resultShape = SpatialOperations.Intersection(currPoly, polygon);
                        if (resultShape != null)
                        {
                            if (resultShape.numPoints != 0 && resultShape.IsValid)
                            {
                                //save the new shape to the result shapefile
                                if (!resultSF.EditInsertShape(resultShape, ref shapeIndex))
                                {
                                    gErrorMsg = "Problem inserting shape: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
                                    Debug.WriteLine(gErrorMsg);
                                    Error.SetErrorMsg(gErrorMsg);
                                    MapWinUtility.Logger.Dbg(gErrorMsg);
                                    return false;
                                }
                                if (copyAttributes)
                                {
                                    for (int f = 0; f <= polySF.NumFields - 1; f++)
                                    {
                                        bool tmpbool = resultSF.EditCellValue(f, shapeIndex, polySF.get_CellValue(f, i));
                                    }
                                }
                            }
                            else
                            {
                                gErrorMsg = "SpatialOperations.Intersection returned an invalid shape";
                                Debug.WriteLine(gErrorMsg);
                                Error.SetErrorMsg(gErrorMsg);
                                MapWinUtility.Logger.Dbg(gErrorMsg);
                            }
                        }
					}
				}
			}
            if (copyAttributes)
            {
                MapWinGIS.Field ID = new MapWinGIS.FieldClass();
                ID.Name = "MWShapeID";
                ID.Type = MapWinGIS.FieldType.INTEGER_FIELD;
                int fieldIndex = 0;
                if (resultSF.EditInsertField(ID, ref fieldIndex, null) == false)
                {
                    gErrorMsg = "Problem inserting field into .dbf table: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
                    Debug.WriteLine(gErrorMsg);
                    Error.SetErrorMsg(gErrorMsg);
                    MapWinUtility.Logger.Dbg(gErrorMsg);
                    return false;
                }
            }
            int numIDs = resultSF.NumShapes;
            for (int i = 0; i <= numIDs - 1; i++)
            {
                if (resultSF.EditCellValue(0, i, i) == false)
                {
                    gErrorMsg = "Problem inserting value into .dbf table for shape " + i + ": " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
                    Debug.WriteLine(gErrorMsg);
                    Error.SetErrorMsg(gErrorMsg);
                    MapWinUtility.Logger.Dbg(gErrorMsg);
                    return false;
                }
            }
			if(resultSF.StopEditingShapes(true, true, null)==false)
			{
				gErrorMsg = "Problem with StopEditingShapes: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
				Debug.WriteLine(gErrorMsg);
				Error.SetErrorMsg(gErrorMsg);
                MapWinUtility.Logger.Dbg(gErrorMsg);
				return false;
			}
			resultSF.Close();
			polySF.Close();
            MapWinUtility.Logger.Dbg("Finished ClipPolygonSFWithPolygon");
			return true;
		}
		#endregion

		#region Save-to-Memory version
		/// <summary>
		/// Returns the portions of the polygons in polySF that lie within polygon as a 
		/// new shapefile of polygons: resultPolySF.
		/// </summary>
		/// <param name="polySF">The shapefile of polygons that are to be clipped.</param>
		/// <param name="polygon">The polygon used for clipping.</param>
		/// <param name="resultPolySF">The result shapefile for the resulting polygons to be saved (in-memory).</param>
        /// <param name="copyAttributes">True if copying attrs</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool ClipPolygonSFWithPolygon(ref MapWinGIS.Shapefile polySF, ref MapWinGIS.Shape polygon, out MapWinGIS.Shapefile resultPolySF, bool copyAttributes)
		{
            MapWinUtility.Logger.Dbg("ClipPolygonSFWithPolygon(polySF: " + Macro.ParamName(polySF) + ",\n" +
                                     "                         polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                         resultPolySF: out,\n" +
                                     "                         copyAttributes: " + copyAttributes + "\n");

		    MapWinGIS.Shapefile resultSF = new MapWinGIS.ShapefileClass();

		    MapWinGIS.ShpfileType sfType = polySF.ShapefileType;
			string tempPath = System.IO.Path.GetTempPath() + "resultSF.shp";
            // Is also done in PrepareResultSF: 
            // DataManagement.DeleteShapefile(ref tempPath);
            string tmpName;
            Globals.PrepareResultSF(ref tempPath, ref resultSF, sfType, copyAttributes);

            if (copyAttributes)
            {
                MapWinGIS.Field tmpField, currField;
                for (int f = 0; f <= polySF.NumFields - 1; f++)
                {
                    tmpField = new MapWinGIS.Field();
                    currField = polySF.get_Field(f);
                    tmpName = currField.Name;
                    if (tmpName.Contains("MWShapeID"))
                    {
                        tmpField.Name = "Last_" + tmpName;
                    }
                    else
                    {
                        tmpField.Name = tmpName;
                    }
                    tmpField.Width = currField.Width;
                    tmpField.Type = currField.Type;
                    tmpField.Precision = currField.Precision;
                    tmpField.Key = currField.Key;
                    resultSF.EditInsertField(tmpField, ref f, null);
                }
            }

			int shapeIndex = 0;//insert new shapes at the beginning of the shapefile

			if(polySF.NumShapes != 0 && polygon.numPoints != 0 && (sfType == MapWinGIS.ShpfileType.SHP_POLYGON || sfType == MapWinGIS.ShpfileType.SHP_POLYGONM || sfType == MapWinGIS.ShpfileType.SHP_POLYGONZ))
			{
				MapWinGIS.Shape resultShape = new MapWinGIS.ShapeClass();		
				int numShapes = polySF.NumShapes;
				bool boundsIntersect = false;	
	
				for(int i = 0; i <= numShapes-1; i++)
				{	
					MapWinGIS.Shape currPoly = new MapWinGIS.ShapeClass();
					currPoly = polySF.get_Shape(i);
					//check to see if bounds intersect before sending shape to GPC clip function
					boundsIntersect = Globals.CheckBounds(ref currPoly, ref polygon); 

					if(boundsIntersect == true)
					{
						//find the shape resulting intersection
						resultShape = SpatialOperations.Intersection(polySF.get_Shape(i), polygon);
                        // Paul Meems: For bug 1507 Added check for correct shape:
                        if (resultShape != null)
                        {
                            if (resultShape.numPoints != 0 && resultShape.IsValid)
                            {
                                //save the new shape to the result shapefile
                                if (resultSF.EditInsertShape(resultShape, ref shapeIndex) == false)
                                {
                                    gErrorMsg = "Problem inserting shape: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
                                    Debug.WriteLine(gErrorMsg);
                                    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                                    resultPolySF = resultSF;
                                    return false;
                                }
                                if (copyAttributes)
                                {
                                    for (int f = 0; f <= polySF.NumFields - 1; f++)
                                    {
                                        bool tmpbool = resultSF.EditCellValue(f, shapeIndex, polySF.get_CellValue(f, i));
                                    }
                                }
                            }
                            else
                            {
                                gErrorMsg = "SpatialOperations.Intersection returned an invalid shape";
                                Debug.WriteLine(gErrorMsg);
                                Error.SetErrorMsg(gErrorMsg);
                                MapWinUtility.Logger.Dbg(gErrorMsg);
                            }
						}
					}
				}
			}

			resultPolySF = resultSF;

            // Paul Meems 11 March 2011 - Give the result sf the same projections as the input:
		    resultPolySF.Projection = polySF.Projection;
			return true;			
		}
		#endregion

		#endregion

	}
}

