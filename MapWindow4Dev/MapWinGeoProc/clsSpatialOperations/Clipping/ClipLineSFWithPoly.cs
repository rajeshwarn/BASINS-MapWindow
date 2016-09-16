//********************************************************************************************************
//File name: ClipLinesWithPoly.cs
//Description: Internal class provides functions for clipping lines with polygons.
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
//06-03-06 ah - Angela Hillier - Added ClipLineSFWithMultiPartPoly to handle multipart polygons.							
//********************************************************************************************************
using System;
using System.Diagnostics;

namespace MapWinGeoProc
{
	/// <summary>
	///ClipLines defines internal functions for clipping lines with polygons.
	/// </summary>
	internal class ClipLineSFWithPoly
	{
		private static string gErrorMsg = "";
		
		#region ClipLineSFWithMultiPartPolygon - save to disk version
        /// <summary>
		/// Clips all lines in the shapefile using a multi-part polygon.
		/// </summary>
		/// <param name="lineSFPath">The full path to the line shapefile.</param>
		/// <param name="polygon">The multi-part polygon used for clipping.</param>
		/// <param name="resultSFPath">The full path to where the result file should be saved.</param>
		/// <param name="speedOptimized">True if fast clipping is desired (not advisable).</param>
        /// <param name="copyAttributes">True if copying Attributes</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool ClipLineSFWithMultiPartPolygon(ref string lineSFPath, ref MapWinGIS.Shape polygon, ref string resultSFPath, bool speedOptimized, bool copyAttributes)
        {
            MapWinUtility.Logger.Dbg("ClipLineSFWithMultiPartPolygon(lineSFPath: " + lineSFPath + ",\n" +
                                     "                               polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                               resultSFPath: " + resultSFPath + ",\n" +
                                     "                               speedOptimized: " + speedOptimized.ToString() + ",\n" + 
                                     "                               copyAttributes: " + copyAttributes.ToString() + ")");
            return ClipLineSFWithMultiPartPolygon(ref lineSFPath, ref polygon, ref resultSFPath, speedOptimized, copyAttributes, false);
        }
		/// <summary>
		/// Clips all lines in the shapefile using a multi-part polygon.
		/// </summary>
		/// <param name="lineSFPath">The full path to the line shapefile.</param>
		/// <param name="polygon">The multi-part polygon used for clipping.</param>
		/// <param name="resultSFPath">The full path to where the result file should be saved.</param>
		/// <param name="speedOptimized">True if fast clipping is desired (not advisable).</param>
        /// <param name="copyAttributes">True if copying Attributes</param>
        /// <param name="SkipMWShapeID">Indicates whether to skip creating an MWShapeID field in the result.</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool ClipLineSFWithMultiPartPolygon(ref string lineSFPath, ref MapWinGIS.Shape polygon, ref string resultSFPath, bool speedOptimized, bool copyAttributes, bool SkipMWShapeID)
		{
            MapWinUtility.Logger.Dbg("ClipLineSFWithMultiPartPolygon(lineSFPath: " + lineSFPath + ",\n" +
                                     "                               polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                               resultSFPath: " + resultSFPath + ",\n" +
                                     "                               speedOptimized: " + speedOptimized.ToString() + ",\n" +
                                     "                               copyAttributes: " + copyAttributes.ToString() + ",\n" +
                                     "                               skipMWShapeID: " + SkipMWShapeID.ToString() + ")");
			int numParts = polygon.NumParts;
			if (numParts == 0)
			{
				numParts = 1;
			}
			if(numParts > 1)
			{
				//fix multi-part polygon to insure all parts have correct orientation
				Globals.FixMultiPartPoly(ref polygon);
				
				//separate parts of the polygon
				MapWinGIS.Shape[] polyParts = new MapWinGIS.Shape[numParts];
				Globals.SeparateParts(ref polygon, out polyParts);
				
				//clip line shapefile with non-hole parts and save to a temporary result file (modLineSF)
				MapWinGIS.Shapefile lineSF = new MapWinGIS.ShapefileClass();
				lineSF.Open(lineSFPath, null);
				MapWinGIS.ShpfileType sfType = new MapWinGIS.ShpfileType();
				sfType = lineSF.ShapefileType;
				lineSF.Close();
				int shpIndex = 0;
				bool[] notHole = new bool[numParts];
				string tempPath = System.IO.Path.GetTempPath() + "tempPartSF.shp";
				DataManagement.DeleteShapefile(ref tempPath);
				
				string modLineSFPath = System.IO.Path.GetTempPath() + "modifiedLineSF.shp";
				DataManagement.DeleteShapefile(ref modLineSFPath);
				MapWinGIS.Shapefile modLineSF = new MapWinGIS.ShapefileClass();
				//CDM 8/4/2006 modLineSF.CreateNew(modLineSFPath, sfType);
                Globals.PrepareResultSF(ref modLineSFPath, ref modLineSF, sfType, copyAttributes);
				modLineSF.StartEditingShapes(true, null);
				
				string holeSFPath = System.IO.Path.GetTempPath() + "tempHoleSF.shp";
				DataManagement.DeleteShapefile(ref holeSFPath);
				MapWinGIS.Shapefile holeSF = new MapWinGIS.ShapefileClass();
				//CDM 8/4/2006 holeSF.CreateNew(holeSFPath, polygon.ShapeType);
                Globals.PrepareResultSF(ref holeSFPath, ref holeSF, polygon.ShapeType, copyAttributes);
				holeSF.StartEditingShapes(true, null);

				string trialPath = lineSFPath;
				
				for(int i = 0; i <= numParts-1; i++)
				{
					MapWinGIS.Shape currPart = new MapWinGIS.ShapeClass();
					currPart.Create(MapWinGIS.ShpfileType.SHP_POLYGON);
					currPart = polyParts[i];
					notHole[i] = Globals.IsClockwise(ref currPart); //holes are counter-clockwise
					if(notHole[i] == true)
					{
						if(speedOptimized)
						{
                            Fast_ClipLineSFWithPolygon(ref lineSFPath, ref currPart, ref tempPath, copyAttributes);
						}
						else
						{
                            Accurate_ClipLineSFWithPolygon(ref lineSFPath, ref currPart, ref tempPath, copyAttributes);
						}
						MapWinGIS.Shapefile tempSF = new MapWinGIS.ShapefileClass();
						tempSF.Open(tempPath, null);
						int numShapes = tempSF.NumShapes;
						if(numShapes > 0)
						{
							for(int j = 0; j <= numShapes-1; j++)
							{
								shpIndex = modLineSF.NumShapes;
								modLineSF.EditInsertShape(tempSF.get_Shape(j), ref shpIndex);
							}
							tempSF.Close();
						}						
					}
					else
					{
						//add the shape to our hole shapefile
						shpIndex = holeSF.NumShapes;
						holeSF.EditInsertShape(currPart, ref shpIndex);
						Debug.WriteLine("Found a hole.");
					}
				}

				//erase lines in the modified line file that fall inside of holes
				if(holeSF.NumShapes > 0 && modLineSF.NumShapes > 0)
				{
					Debug.WriteLine("number of holes = " + holeSF.NumShapes);
					MapWinGIS.Shapefile resultSF = new MapWinGIS.ShapefileClass();
					DataManagement.DeleteShapefile(ref resultSFPath);
					//CDM 8/4/2006 resultSF.CreateNew(resultSFPath, sfType);
                    Globals.PrepareResultSF(ref resultSFPath, ref resultSF, sfType, copyAttributes);
					resultSF.StartEditingShapes(true, null);

					Erase.EraseLineSFWithPolySF(ref modLineSF, ref holeSF, ref resultSF);

					if(resultSF.NumShapes > 0)
					{
                        if (!SkipMWShapeID) Globals.DoInsertIDs(ref resultSF);
						resultSF.SaveAs(resultSFPath, null);
						resultSF.StopEditingShapes(true, true, null);
						resultSF.Close();
					}
					else
					{
						gErrorMsg = "No results found when clipping lineSF with multi-part polygon.";
						Error.SetErrorMsg(gErrorMsg);
						Debug.WriteLine(gErrorMsg);
                        MapWinUtility.Logger.Dbg(gErrorMsg);
						return false;
					}
				}
				else
				{
					if(modLineSF.NumShapes > 0)
					{
                        if (!SkipMWShapeID) Globals.DoInsertIDs(ref modLineSF);
						modLineSF.SaveAs(resultSFPath, null);
						modLineSF.StopEditingShapes(true, true, null);
						modLineSF.Close();
					}
					else
					{
						gErrorMsg = "No results found when clipping lineSF with multi-part polygon.";
						Error.SetErrorMsg(gErrorMsg);
						Debug.WriteLine(gErrorMsg);
                        MapWinUtility.Logger.Dbg(gErrorMsg);
						return false;
					}
				}
				return true;				
			}
			else
			{
				if(speedOptimized)
				{
                    return Fast_ClipLineSFWithPolygon(ref lineSFPath, ref polygon, ref resultSFPath, copyAttributes);
				}
				else
				{
                    return Accurate_ClipLineSFWithPolygon(ref lineSFPath, ref polygon, ref resultSFPath, copyAttributes);
				}
			}
		}
		#endregion
		
		#region ClipLineSFWithMultiPartPolygon - save to memory version
        /// <summary>
		/// Clips all lines in the shapefile using a multi-part polygon.
		/// </summary>
		/// <param name="lineSF">The line shapefile.</param>
		/// <param name="polygon">The multi-part polygon used for clipping.</param>
		/// <param name="resultSF">The result file.</param>
		/// <param name="speedOptimized">True if fast clipping is desired (not advisable).</param>
        /// <param name="copyAttributes">True if copying Attributes</param>
		/// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool ClipLineSFWithMultiPartPolygon(ref MapWinGIS.Shapefile lineSF, ref MapWinGIS.Shape polygon, out MapWinGIS.Shapefile resultSF, bool speedOptimized, bool copyAttributes)
        {
            MapWinUtility.Logger.Dbg("ClipLineSFWithMultiPartPolygon(lineSF: " + Macro.ParamName(lineSF) + ",\n" +
                                     "                               polygon: " + Macro.ParamName(polygon) + ",\n" + 
                                     "                               resultSF: out,\n" + 
                                     "                               speedOptimized: " + speedOptimized.ToString() + ",\n" + 
                                     "                               copyAttributes: " + copyAttributes.ToString() + ")");
            return ClipLineSFWithMultiPartPolygon(ref lineSF, ref polygon, out resultSF, speedOptimized, copyAttributes, false);
        }
		/// <summary>
		/// Clips all lines in the shapefile using a multi-part polygon.
		/// </summary>
		/// <param name="lineSF">The line shapefile.</param>
		/// <param name="polygon">The multi-part polygon used for clipping.</param>
		/// <param name="resultSF">The result file.</param>
		/// <param name="speedOptimized">True if fast clipping is desired (not advisable).</param>
        /// <param name="copyAttributes">True if copying Attributes</param>
        /// <param name="SkipMWShapeID">Indicates whether to skip creating an MWShapeID field in the result.</param>
		/// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool ClipLineSFWithMultiPartPolygon(ref MapWinGIS.Shapefile lineSF, ref MapWinGIS.Shape polygon, out MapWinGIS.Shapefile resultSF, bool speedOptimized, bool copyAttributes, bool SkipMWShapeID)
		{
		    MapWinGIS.Shapefile resultFile = new MapWinGIS.ShapefileClass();

		    int numParts = polygon.NumParts;
			if (numParts == 0)
			{
				numParts = 1;
			}
			if(numParts > 1)
			{
				//fix multi-part polygon to insure all parts have correct orientation
				Globals.FixMultiPartPoly(ref polygon);
				
				//separate parts of the polygon
				MapWinGIS.Shape[] polyParts = new MapWinGIS.Shape[numParts];
				Globals.SeparateParts(ref polygon, out polyParts);
				
				//clip line shapefile with non-hole parts and save to a temporary result file (modLineSF)
				MapWinGIS.ShpfileType sfType = new MapWinGIS.ShpfileType();
				sfType = lineSF.ShapefileType;
				int shpIndex = 0;
				bool[] notHole = new bool[numParts];
				string tempPath = System.IO.Path.GetTempPath() + "tempPartSF.shp";
				DataManagement.DeleteShapefile(ref tempPath);
				MapWinGIS.Shapefile tempSF = new MapWinGIS.ShapefileClass();
				//CDM 8/4/2006 tempSF.CreateNew(tempPath, sfType);
                Globals.PrepareResultSF(ref tempPath, ref tempSF, sfType, copyAttributes);
				tempSF.StartEditingShapes(true, null);
				
				string modLineSFPath = System.IO.Path.GetTempPath() + "modifiedLineSF.shp";
				DataManagement.DeleteShapefile(ref modLineSFPath);
				MapWinGIS.Shapefile modLineSF = new MapWinGIS.ShapefileClass();
				//CDM 8/4/2006 modLineSF.CreateNew(modLineSFPath, sfType);
                Globals.PrepareResultSF(ref modLineSFPath, ref modLineSF, sfType, copyAttributes);
				modLineSF.StartEditingShapes(true, null);
				
				string holeSFPath = System.IO.Path.GetTempPath() + "tempHoleSF.shp";
				DataManagement.DeleteShapefile(ref holeSFPath);
				MapWinGIS.Shapefile holeSF = new MapWinGIS.ShapefileClass();
				//CDM 8/4/2006 holeSF.CreateNew(holeSFPath, polygon.ShapeType);
                Globals.PrepareResultSF(ref holeSFPath, ref holeSF, polygon.ShapeType, copyAttributes);
				holeSF.StartEditingShapes(true, null);
				
				for(int i = 0; i <= numParts-1; i++)
				{
					MapWinGIS.Shape currPart = new MapWinGIS.ShapeClass();
					currPart.Create(MapWinGIS.ShpfileType.SHP_POLYGON);
					currPart = polyParts[i];
					notHole[i] = Globals.IsClockwise(ref currPart); //holes are counter-clockwise
					if(notHole[i] == true)
					{
						if(speedOptimized)
						{
                            Fast_ClipLineSFWithPolygon(ref lineSF, ref currPart, out tempSF, false);
						}
						else
						{
                            Accurate_ClipLineSFWithPolygon(ref lineSF, ref currPart, out tempSF, false);
						}
						
						int numShapes = tempSF.NumShapes;
						if(numShapes > 0)
						{
							for(int j = 0; j <= numShapes-1; j++)
							{
								shpIndex = modLineSF.NumShapes;
								modLineSF.EditInsertShape(tempSF.get_Shape(j), ref shpIndex);
							}
							
							tempSF.EditClear();
						}						
					}
					else
					{
						//add the shape to our hole shapefile
						shpIndex = holeSF.NumShapes;
						holeSF.EditInsertShape(currPart, ref shpIndex);
					}
				}

				//erase lines in the modified line file that fall inside of holes
				if(holeSF.NumShapes > 0 && modLineSF.NumShapes > 0)
				{
					MapWinGIS.Shapefile resultLines = new MapWinGIS.ShapefileClass();
					string resultSFPath = System.IO.Path.GetTempPath() + "tempEraseLines.shp";
					DataManagement.DeleteShapefile(ref resultSFPath);
					//CDM 8/4/2006 resultLines.CreateNew(resultSFPath, sfType);
                    Globals.PrepareResultSF(ref resultSFPath, ref resultLines, sfType, copyAttributes);
					resultLines.StartEditingShapes(true, null);

					Erase.EraseLineSFWithPolySF(ref modLineSF, ref holeSF, ref resultLines);

					if(resultLines.NumShapes > 0)
					{
                        if (!SkipMWShapeID) Globals.DoInsertIDs(ref resultLines);
						resultSF = resultLines;
					}
					else
					{
						gErrorMsg = "No results found when clipping lineSF with multi-part polygon.";
						Error.SetErrorMsg(gErrorMsg);
						Debug.WriteLine(gErrorMsg);
						resultSF = resultFile;
                        MapWinUtility.Logger.Dbg(gErrorMsg);
						return false;
					}
				}
				else
				{
					if(modLineSF.NumShapes > 0)
					{
                        if (!SkipMWShapeID) Globals.DoInsertIDs(ref modLineSF);
						resultSF = modLineSF;
					}
					else
					{
						gErrorMsg = "No results found when clipping lineSF with multi-part polygon.";
						Error.SetErrorMsg(gErrorMsg);
						Debug.WriteLine(gErrorMsg);
						resultSF = resultFile;
                        MapWinUtility.Logger.Dbg(gErrorMsg);
						return false;
					}
				}
				return true;				
			}
		    
            if(speedOptimized)
		    {
		        return Fast_ClipLineSFWithPolygon(ref lineSF, ref polygon, out resultSF, false);
		    }
		    
            return Accurate_ClipLineSFWithPolygon(ref lineSF, ref polygon, out resultSF, false);
		}
		#endregion


		#region public Fast_ClipLineSFWithPolygon() -- save-to-disk and save-to-memory versions
		//Angela Hillier 10/05: Save-to-Disk and Save-to-Memory versions. If a change is
		//made in one version, be sure to make similar upgrades to the other version!
		#region Save-to-Disk version
		/// <summary>
		/// Finds all portions of the lines that lie within the polygon and returns them in
		/// the shapefile resultSF.
		/// </summary>
		/// <param name="lineSFPath">Full path to the line shapefile.</param>
		/// <param name="polygon">The polygon that will be checked for intersections.</param>
		/// <param name="resultSFPath">Full path to the result shapefile.</param>
        /// <param name="copyAttributes">True if copying Attributes</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool Fast_ClipLineSFWithPolygon(ref string lineSFPath, ref MapWinGIS.Shape polygon, ref string resultSFPath, bool copyAttributes)
		{
            MapWinUtility.Logger.Dbg("Fast_ClipLineSFWithPolygon(lineSFPath: " + lineSFPath + ",\n" +
                                     "                           polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                           resultSFPath: " + resultSFPath + ",\n" +
                                     "                           copyAttributes: " + copyAttributes.ToString() + ")");
			MapWinGIS.Shapefile resultSF = new MapWinGIS.ShapefileClass();
			MapWinGIS.Shapefile lineSF = new MapWinGIS.ShapefileClass();
			//MapWinGIS.Utils utils = new MapWinGIS.UtilsClass();
			lineSF.Open(lineSFPath, null);
			MapWinGIS.ShpfileType sfType = lineSF.ShapefileType;
			int shpIndex = 0; //add lines at beginning of result shapefile
			bool boundsIntersect = false;
            string tmpName;

			//make sure we are dealing with a valid shapefile type
			if(sfType == MapWinGIS.ShpfileType.SHP_POLYLINE || sfType == MapWinGIS.ShpfileType.SHP_POLYLINEM || sfType == MapWinGIS.ShpfileType.SHP_POLYLINEZ)
			{
				//delete the shapeFile of the same name then re-create it.
                if (Globals.PrepareResultSF(ref resultSFPath, ref resultSF, sfType, copyAttributes) == false)
				{
					return false;
				}

                if (copyAttributes)
                {
                    MapWinGIS.Field tmpField, currField;
                    for (int f = 0; f <= lineSF.NumFields - 1; f++)
                    {
                        tmpField = new MapWinGIS.Field();
                        currField = lineSF.get_Field(f);
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

				int numLines = lineSF.NumShapes;
				int numPolygonPts = polygon.numPoints;

				for(int i = 0; i <= numLines-1; i++)
				{
					MapWinGIS.Shape lineShp = new MapWinGIS.ShapeClass();
					lineShp = lineSF.get_Shape(i);
					//check to see if line and polygon bounds intersect
					boundsIntersect = Globals.CheckBounds(ref lineShp, ref polygon);

					if(boundsIntersect == false)
					{
						continue;
					}
					else
					{
						//line might intersect polygon
						int numPoints = lineShp.numPoints;
						bool[] ptsInside = new bool[numPoints];
						MapWinGIS.Point currPt = new MapWinGIS.PointClass();
						MapWinGIS.Point nextPt = new MapWinGIS.PointClass();
						int numInside = 0;
						int numOutside = 0;

						int numParts = polygon.NumParts;
						if(numParts == 0)
						{
							numParts = 1;
						}
						Globals.Vertex[][] polyVertArray = new Globals.Vertex[numParts][];
						Globals.ConvertPolyToVertexArray(ref polygon, out polyVertArray);

						//check each point in the line to see if the entire line is either
						//inside of the polygon or outside of it (we know it's inside polygon bounding box).
						for(int j = 0; j <= numPoints-1; j++)
						{
							currPt = lineShp.get_Point(j);
		
							//if(utils.PointInPolygon(polygon, currPt) == true)
							if(Utils.PointInPoly(ref polyVertArray, ref currPt) == true)
							{
								ptsInside[j] = true;
								numInside += 1;
							}
							else
							{
								ptsInside[j] = false;
								numOutside += 1;
							}
						}
						//case: all points are inside polygon - add entire line to result file
						if(numInside == numPoints)
						{
							shpIndex = resultSF.NumShapes;
							if(resultSF.EditInsertShape(lineShp, ref shpIndex) == false)
							{	
								gErrorMsg = "Problem processing inside points.";
								Debug.WriteLine(gErrorMsg);
								MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                                MapWinUtility.Logger.Dbg(gErrorMsg);
								return false;
							}
                            if (copyAttributes)
                            {
                                for (int f = 0; f <= lineSF.NumFields - 1; f++)
                                {
                                    bool tmpbool = resultSF.EditCellValue(f, shpIndex, lineSF.get_CellValue(f, i));
                                }
                            }
						}
							//case: all points are outside of the polygon - don't add any to result file
						else if(numOutside == numPoints)
						{	
						}
						
							//case: part of line is inside and part is outside - find inside segments.
						else
						{
                            shpIndex = resultSF.NumShapes;
							if(Fast_ProcessPartInAndOutPoints(ref ptsInside, ref lineShp, ref polygon, ref resultSF) == false)
							{
								gErrorMsg = "Problem calculating intersect points.";
								Debug.WriteLine(gErrorMsg);
								MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                                MapWinUtility.Logger.Dbg(gErrorMsg);
								return false;
							}
                            if (copyAttributes)
                            {
                                for (int f = 0; f <= lineSF.NumFields - 1; f++)
                                {
                                    bool tmpbool = resultSF.EditCellValue(f, shpIndex, lineSF.get_CellValue(f, i));
                                }
                            }
						}			

					}//end of else (boundsIntersect == true)
				}//end of looping through each line in the shapefile

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
				if(resultSF.StopEditingShapes(true, true, null) == false)
				{
					gErrorMsg = "Problem with saving result file: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
					Debug.WriteLine(gErrorMsg);
					MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                    MapWinUtility.Logger.Dbg(gErrorMsg);
					return false;
				}
				resultSF.SaveAs(resultSFPath, null);
				resultSF.Close();
				lineSF.Close();
			}//end of checking if shapefile is valid
			else
			{
				lineSF.Close();
				gErrorMsg = "Invalid shapefile type. Should be of type Line.";
				Debug.WriteLine(gErrorMsg);
				MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                MapWinUtility.Logger.Dbg(gErrorMsg);
				return false;
			}
            MapWinUtility.Logger.Dbg("Finished Fast_ClipLineSFWithPolygon");
			return true;
		}
		#endregion

		#region Save-to-Memory version
		/// <summary>
		/// All portions of lineSF within the polygon are found and saved in memory to resultLineSF.
		/// </summary>
		/// <param name="lineSF">The shapefile of lines to be tested.</param>
		/// <param name="polygon">The polygon that may contain none, a portion, or all of the lines.</param>
		/// <param name="resultLineSF">The resulting line portions found within the polygon's borders.</param>
        /// <param name="copyAttributes">True if copying Attributes</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool Fast_ClipLineSFWithPolygon(ref MapWinGIS.Shapefile lineSF, ref MapWinGIS.Shape polygon, out MapWinGIS.Shapefile resultLineSF, bool copyAttributes)
		{
            MapWinUtility.Logger.Dbg("Fast_ClipLineSFWithPolyon(lineSF: " + Macro.ParamName(lineSF) + ",\n" +
                                     "                          polygon: " + Macro.ParamName(polygon) + ",\n" + 
                                     "                          resultLineSF: (out),\n" + 
                                     "                          copyAttributes: " + copyAttributes + ")");
            
            MapWinGIS.Shapefile resultSF = new MapWinGIS.ShapefileClass();

		    //MapWinGIS.Utils utils = new MapWinGIS.UtilsClass();
			MapWinGIS.ShpfileType sfType = lineSF.ShapefileType;
			int shpIndex = 0; //add lines at beginning of result shapefile
			bool boundsIntersect = false;
            string tmpName;

			//make sure we are dealing with a valid shapefile type
			if(sfType == MapWinGIS.ShpfileType.SHP_POLYLINE || sfType == MapWinGIS.ShpfileType.SHP_POLYLINEM || sfType == MapWinGIS.ShpfileType.SHP_POLYLINEZ)
			{
				//create the result shapefile
				string tempPath = System.IO.Path.GetTempPath() + "resultLineSF.shp";
				DataManagement.DeleteShapefile(ref tempPath);
                //if(resultSF.CreateNew(tempPath, sfType)==false)
                //{
                //    gErrorMsg = "Problem creating the result shapeFile: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
                //    Debug.WriteLine(gErrorMsg);
                //    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                //    resultLineSF = resultSF;
                //    return false;
                //}
                //CDM 8/4/2006
                Globals.PrepareResultSF(ref tempPath, ref resultSF, sfType, copyAttributes);

                if (copyAttributes)
                {
                    MapWinGIS.Field tmpField, currField;
                    for (int f = 0; f <= lineSF.NumFields - 1; f++)
                    {
                        tmpField = new MapWinGIS.Field();
                        currField = lineSF.get_Field(f);
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

				int numLines = lineSF.NumShapes;
				int numPolygonPts = polygon.numPoints;

				for(int i = 0; i <= numLines-1; i++)
				{
					MapWinGIS.Shape lineShp = new MapWinGIS.ShapeClass();
					lineShp = lineSF.get_Shape(i);
					//check to see if line and polygon bounds intersect
					boundsIntersect = Globals.CheckBounds(ref lineShp, ref polygon);

					if(boundsIntersect == false)
					{
						continue;
					}
					else
					{
						//line might intersect polygon
						int numPoints = lineShp.numPoints;
						bool[] ptsInside = new bool[numPoints];
						MapWinGIS.Point currPt = new MapWinGIS.PointClass();
						MapWinGIS.Point nextPt = new MapWinGIS.PointClass();
						int numInside = 0;
						int numOutside = 0;

						int numParts = polygon.NumParts;
						if(numParts == 0)
						{
							numParts = 1;
						}
						Globals.Vertex[][] polyVertArray = new Globals.Vertex[numParts][];
						Globals.ConvertPolyToVertexArray(ref polygon, out polyVertArray);

						//check each point in the line to see if the entire line is either
						//inside of the polygon or outside of it (we know it's inside polygon bounding box).
						for(int j = 0; j <= numPoints-1; j++)
						{
							currPt = lineShp.get_Point(j);
		
							//if(utils.PointInPolygon(polygon, currPt) == true)
							if(Utils.PointInPoly(ref polyVertArray, ref currPt) == true)
							{
								ptsInside[j] = true;
								numInside += 1;
							}
							else
							{
								ptsInside[j] = false;
								numOutside += 1;
							}
						}
						//case: all points are inside polygon - add entire line to result file
						if(numInside == numPoints)
						{
							shpIndex = resultSF.NumShapes;
							if(resultSF.EditInsertShape(lineShp, ref shpIndex) == false)
							{	
								gErrorMsg = "Problem processing inside points.";
								Debug.WriteLine(gErrorMsg);
								MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
								resultLineSF = resultSF;
                                MapWinUtility.Logger.Dbg(gErrorMsg);
								return false;
							}
                            if (copyAttributes)
                            {
                                for (int f = 0; f <= lineSF.NumFields - 1; f++)
                                {
                                    bool tmpbool = resultSF.EditCellValue(f, shpIndex, lineSF.get_CellValue(f, i));
                                }
                            }
						}
							//case: all points are outside of the polygon - don't add any to result file
						else if(numOutside == numPoints)
						{	
						}
						
							//case: part of line is inside and part is outside - find inside segments.
						else
						{
                            shpIndex = resultSF.NumShapes;
							if(Fast_ProcessPartInAndOutPoints(ref ptsInside, ref lineShp, ref polygon, ref resultSF) == false)
							{
								gErrorMsg = "Problem calculating intersect points.";
								Debug.WriteLine(gErrorMsg);
								MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
								resultLineSF = resultSF;
                                MapWinUtility.Logger.Dbg(gErrorMsg);
								return false;
							}
                            if (copyAttributes)
                            {
                                for (int f = 0; f <= lineSF.NumFields - 1; f++)
                                {
                                    bool tmpbool = resultSF.EditCellValue(f, shpIndex, lineSF.get_CellValue(f, i));
                                }
                            }
						}			

					}//end of else (boundsIntersect == true)
				}//end of looping through each line in the shapefile
				resultLineSF = resultSF;

                // Paul Meems 11 March 2011 - Give the result sf the same projections as the input:
			    resultLineSF.Projection = lineSF.Projection;

			}//end of checking if shapefile is valid
			else
			{
				gErrorMsg = "Invalid shapefile type. Should be of type Line.";
				Debug.WriteLine(gErrorMsg);
				MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
				resultLineSF = resultSF;
                MapWinUtility.Logger.Dbg(gErrorMsg);
				return false;
			}
			return true;
		}
		#endregion

		#endregion

		#region private Fast_ProcessPartInAndOutPoints() -- used by Fast_ClipLineSFWithPolygon()
		//Angela Hillier 10/05
		/// <summary>
		/// Divides a line containing both inside and outside polygon points into multiple 2pt segments.
		/// Calculates intersecetions and finds the portions of the line that are within the polygon.
		/// </summary>
		/// <param name="insidePts">Boolean array signifying which points lie within the polygon (true), or outside of it (false).</param>
		/// <param name="line">The line that being tested for internal segments.</param>
		/// <param name="polygon">The polygon that may contain portions of the line.</param>
		/// <param name="resultSF">The result shapefile that inside line portions will be saved to.</param>
		/// <returns>False if an error was encountered, true otherwise.</returns>
		private static bool Fast_ProcessPartInAndOutPoints(ref bool[] insidePts, ref MapWinGIS.Shape line, ref MapWinGIS.Shape polygon, ref MapWinGIS.Shapefile resultSF)
		{
			int numPolyPts = polygon.numPoints;
			int numLinePts = line.numPoints;
			MapWinGIS.Shape insideLine;
			MapWinGIS.Shape outsideLine;
			MapWinGIS.Shape intersectLine;
			MapWinGIS.Point startingIntersectPt = new MapWinGIS.PointClass();
			int addToShpIndex = 0;
			bool addStartingIntersect = false;
			bool startInside = insidePts[0];
			MapWinGIS.Shapefile tempResultSF = new MapWinGIS.ShapefileClass();
			string tempPath = System.IO.Path.GetTempPath() + "tempResultSF.shp";
			DataManagement.DeleteShapefile(ref tempPath);
			//CDM 8/4/2006 tempResultSF.CreateNew(tempPath, line.ShapeType);
            Globals.PrepareResultSF(ref tempPath, ref tempResultSF, line.ShapeType);
			int ptIndex = 0;
			int shpIndex = 0;
			int[] intersectsPerSeg = new int[1];
			int[][] polyIntLocs = new int[1][];
			MapWinGIS.Point[][] intersectPts = new MapWinGIS.Point[1][];
			
			for(int i=0; i<= numLinePts-2; i++)
			{
				insideLine = new MapWinGIS.ShapeClass();
				insideLine.ShapeType = line.ShapeType;
				outsideLine = new MapWinGIS.ShapeClass();
				outsideLine.ShapeType = line.ShapeType;
				intersectLine = new MapWinGIS.ShapeClass();
				intersectLine.ShapeType = line.ShapeType;
				MapWinGIS.Shapefile intersectLineSF = new MapWinGIS.ShapefileClass();
				string tempPath2 = System.IO.Path.GetTempPath() + "intersectSF.shp";
				DataManagement.DeleteShapefile(ref tempPath2);
				//CDM 8/4/2006 intersectLineSF.CreateNew(tempPath2, line.ShapeType);
                Globals.PrepareResultSF(ref tempPath2, ref intersectLineSF, line.ShapeType);
				intersectPts[0] = new MapWinGIS.Point[numPolyPts];
				polyIntLocs[0] = new int[numPolyPts];

				//**************Build a line of "all inside" points if appropriate***********
				if(startInside == true && insidePts[i+1] == true)
				{
					ptIndex = 0;
					insideLine.InsertPoint(line.get_Point(i), ref ptIndex);
					int c = i+1;
					while(insidePts[c] == true && c <= numLinePts-1)
					{
						ptIndex = insideLine.numPoints;
						insideLine.InsertPoint(line.get_Point(c), ref ptIndex);
						c++;
						if(c == numLinePts)
						{
							break;
						}
					}
					i = c-1;
					//now we should have a line of all inside values
					//and c represents the first outside point encountered.
					//Add insideLine to the result file.
					shpIndex = tempResultSF.NumShapes;
					if(tempResultSF.EditInsertShape(insideLine, ref shpIndex) == false)
					{
						gErrorMsg = "Problem inserting shape into result file: " + tempResultSF.get_ErrorMsg(tempResultSF.LastErrorCode);
						MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
						Debug.WriteLine(gErrorMsg);
						return false;
					}
                    
					if(addStartingIntersect == true)
					{
						ptIndex = 0;
						tempResultSF.get_Shape(addToShpIndex).InsertPoint(startingIntersectPt, ref ptIndex);
						addToShpIndex = 0;
						startingIntersectPt = new MapWinGIS.PointClass();
						addStartingIntersect = false;
					}

					if(i != numLinePts-1)
					{
						//we need to find the interesect pts existing between
						//the last inside point and the next outside point.					
						ptIndex = 0;
						intersectLine.InsertPoint(line.get_Point(c-1), ref ptIndex);
						ptIndex = 1;
						intersectLine.InsertPoint(line.get_Point(c), ref ptIndex);
						shpIndex = 0;
						intersectLineSF.EditInsertShape(intersectLine, ref shpIndex);
						int numIntersects = Globals.CalcSiDeterm(ref intersectLineSF, ref polygon, out intersectsPerSeg, out intersectPts, out polyIntLocs);
						
						if(numIntersects == 0)
						{
							//there was a problem, should have at least one intersect pt for this case
							gErrorMsg = "No intersects were found but one was expected.";
							Error.SetErrorMsg(gErrorMsg);
							Debug.WriteLine(gErrorMsg);
							return false;
						}
						else if(numIntersects == 1)
						{
							//add intersect pt to end of last shape in tempResultSF
							shpIndex = tempResultSF.NumShapes;
							ptIndex = tempResultSF.get_Shape(shpIndex-1).numPoints;
							tempResultSF.get_Shape(shpIndex-1).InsertPoint(intersectPts[0][0], ref ptIndex);
							startInside = false;
							continue;
						}
						else //more than one intersect pt exists (should always be an odd number)
						{
							gErrorMsg = "More than one intersect detected for a 2pt segment. Use Accurate version instead of fast version.";
							MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
							Debug.WriteLine(gErrorMsg);
							return false;	
						}
					}//end of if i == numLinePts-1
				}//end of building inside line
				
					//**************Build a line of "all outside" points if appropriate*******************
				else if(startInside == false && insidePts[i+1] == false)
				{
					ptIndex = 0;
					outsideLine.InsertPoint(line.get_Point(i), ref ptIndex);
					int c = i+1;
					while(insidePts[c] == false && c <= numLinePts-1)
					{
						ptIndex = outsideLine.numPoints;
						outsideLine.InsertPoint(line.get_Point(c), ref ptIndex);						
						c++;
						if(c == numLinePts)
						{
							break;
						}
					}
					i = c-1;
					//now c represents the first inside point encountered
					//Don't add any of the points in outsideLine to the result file.

					if(i != numLinePts-1)
					{
						//we need to find any interesect pts existing between
						//the last outside point and the next inside point.					
						ptIndex = 0;
						intersectLine.InsertPoint(line.get_Point(c-1), ref ptIndex);
						ptIndex = 1;
						intersectLine.InsertPoint(line.get_Point(c), ref ptIndex);
						shpIndex = 0;
						intersectLineSF.EditInsertShape(intersectLine, ref shpIndex);
					
						int numIntersects = Globals.CalcSiDeterm(ref intersectLineSF, ref polygon, out intersectsPerSeg, out intersectPts, out polyIntLocs);
					
						if(numIntersects == 0)
						{
							//there was a problem, should have at least one intersect pt for this case
							gErrorMsg = "No intersect was found but one was expected.";
							MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
							Debug.WriteLine(gErrorMsg);
							return false;
						}
					
						else if(numIntersects == 1)
						{
							//the intersect and inside points will be at
							//the beginning of a new shape in tempResultSF
							if(c != numLinePts-1)
							{
								startingIntersectPt = new MapWinGIS.PointClass();
								startingIntersectPt = intersectPts[0][0];
								addStartingIntersect = true;
								addToShpIndex = tempResultSF.NumShapes;
							}
							else //need to deal with last segment
							{
								MapWinGIS.Shape innerSeg = new MapWinGIS.ShapeClass();
								innerSeg.ShapeType = line.ShapeType;
								ptIndex = 0;
								innerSeg.InsertPoint(intersectPts[0][0], ref ptIndex);
								ptIndex = 1;
								innerSeg.InsertPoint(line.get_Point(c), ref ptIndex);
								shpIndex = tempResultSF.NumShapes;
								tempResultSF.EditInsertShape(innerSeg, ref shpIndex);
							}
							startInside = true;
							continue;
						}
					
						else //more than one intersect pt exists (should always be an odd number)
						{
							//We don't deal with this case in the 'fast' version
							gErrorMsg = "More than one intersect pt detected for a 2pt segment. Use accurate version instead.";
							MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
							Debug.WriteLine(gErrorMsg);
							return false;                            							
						}
					}
				}//end of build line of "all outside" points

					//**************Deal with inside->outside segment*********************
				else if(startInside == true && insidePts[i+1] == false)
				{
					ptIndex = 0;
					intersectLine.InsertPoint(line.get_Point(i), ref ptIndex);
					ptIndex = 1;
					intersectLine.InsertPoint(line.get_Point(i+1), ref ptIndex);
					shpIndex = 0;
					intersectLineSF.EditInsertShape(intersectLine, ref shpIndex);
					
					//calculate intersect points
					int numIntersects = Globals.CalcSiDeterm(ref intersectLineSF, ref polygon, out intersectsPerSeg, out intersectPts, out polyIntLocs);
					
					if(numIntersects == 0)
					{
						gErrorMsg = "No intersect was found but one was expected.";
						MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
						Debug.WriteLine(gErrorMsg);
						return false;
					}
					
					else if(numIntersects == 1)
					{
						shpIndex = tempResultSF.NumShapes;
						MapWinGIS.Shape insideSeg = new MapWinGIS.ShapeClass();
						insideSeg.ShapeType = line.ShapeType;
						ptIndex = 0;
						insideSeg.InsertPoint(line.get_Point(i), ref ptIndex);
						ptIndex = 1;
						insideSeg.InsertPoint(intersectPts[0][0], ref ptIndex);
						tempResultSF.EditInsertShape(insideSeg, ref shpIndex);

						//there might be an additional point waiting to be added!
						if(addStartingIntersect == true)
						{
							ptIndex = 0;
							tempResultSF.get_Shape(addToShpIndex).InsertPoint(startingIntersectPt, ref ptIndex);
							addToShpIndex = 0;
							startingIntersectPt = new MapWinGIS.PointClass();
							addStartingIntersect = false;
						}
					}
					
					else //more than one intersect exists
					{
						//This requires the accurate version
						gErrorMsg = "More than one intersect detected for a 2pt segment. Use accurate version instead.";
						MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
						Debug.WriteLine(gErrorMsg);
						return false;
					}
					startInside = false;
				} //end of else inside->outside

					//**************Deal with outside->inside segment**********************
				else if(startInside == false && insidePts[i+1] == true)
				{
					ptIndex = 0;
					intersectLine.InsertPoint(line.get_Point(i), ref ptIndex);
					ptIndex = 1;
					intersectLine.InsertPoint(line.get_Point(i+1), ref ptIndex);
					shpIndex = 0;
					intersectLineSF.EditInsertShape(intersectLine, ref shpIndex);
					
					//calculate intersect points
					int numIntersects = Globals.CalcSiDeterm(ref intersectLineSF, ref polygon, out intersectsPerSeg, out intersectPts, out polyIntLocs);
					
					if(numIntersects == 0)
					{
						gErrorMsg = "No intersect was found but one was expected.";
						MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
						Debug.WriteLine(gErrorMsg);
						return false;
					}

					else if(numIntersects == 1)
					{
						if(i == numLinePts-2)
						{
							//add last inside segment to result file
							MapWinGIS.Shape insideSeg = new MapWinGIS.ShapeClass();
							insideSeg.ShapeType = line.ShapeType;
							ptIndex = 0;
							insideSeg.InsertPoint(intersectPts[0][0], ref ptIndex);
							ptIndex = 1;
							insideSeg.InsertPoint(line.get_Point(i+1), ref ptIndex);
							
							shpIndex = tempResultSF.NumShapes;
							tempResultSF.EditInsertShape(insideSeg, ref shpIndex);			
						}
						else
						{	
							//intersect needs to be at the beginning of the next shape
							startingIntersectPt = new MapWinGIS.PointClass();
							startingIntersectPt = intersectPts[0][0];
							addStartingIntersect = true;
							addToShpIndex = tempResultSF.NumShapes;
						}
					}
					
					else //more than one intersect exists (should always be an odd #)
					{
						//Only deal with this case in accurate version.
						gErrorMsg = "More than one intersect detected for a 2pt segment. Need to use accurate version.";
						MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
						Debug.WriteLine(gErrorMsg);
						return false;						
					}	
					startInside = true;
				}//end of dealing with outside->inside segment
			}//end of looping through line points
			
			//finish by copying all of the tempResultSF shapes into resultSF
			int numShapes = tempResultSF.NumShapes;
			for(int i = 0; i <= numShapes-1; i++)
			{
				shpIndex = resultSF.NumShapes;
				resultSF.EditInsertShape(tempResultSF.get_Shape(i), ref shpIndex);
			}

			return true;
		}
		#endregion
		

		#region public Accurate_ClipLineSFWithPolygon() -- save-to-disk and save-to-memory versions
		#region Save-to-Disk version
		//Angela Hillier 10/05
		/// <summary>
		/// Given a line shapefile and a polygon, this function will return only the
		/// portions of the lines that lie within the polygon.
		/// </summary>
		/// <param name="lineSFPath">The full path to the shapefile.</param>
		/// <param name="polygon">The polygon object to be used for clipping.</param>
		/// <param name="resultSFPath">The full path to the result file where clipped lines will be saved.</param>
        /// <param name="copyAttributes">True if copying Attributes</param>
        /// <returns>False if an error occured, true otherwise.</returns>
        public static bool Accurate_ClipLineSFWithPolygon(ref string lineSFPath, ref MapWinGIS.Shape polygon, ref string resultSFPath, bool copyAttributes)
		{
            MapWinUtility.Logger.Dbg("Accurate_ClipLineSFWithPolygon(lineSFPath: " + lineSFPath + ",\n" +
                                     "                               polygon: " + Macro.ParamName(polygon) +
                                     "                               resultsSFPath: " + resultSFPath + ",\n" +
                                     "                               copyAttributes: " + copyAttributes.ToString() + ")");

			MapWinGIS.Shapefile resultSF = new MapWinGIS.ShapefileClass();
			MapWinGIS.Shapefile lineSF = new MapWinGIS.ShapefileClass();
			//MapWinGIS.Utils utils = new MapWinGIS.UtilsClass();
			lineSF.Open(lineSFPath, null);
			MapWinGIS.ShpfileType sfType = lineSF.ShapefileType;
			bool boundsIntersect = false;
            int shpIndex = 0;
            string tmpName;

			//make sure we are dealing with a valid shapefile type
			if(sfType == MapWinGIS.ShpfileType.SHP_POLYLINE || sfType == MapWinGIS.ShpfileType.SHP_POLYLINEM || sfType == MapWinGIS.ShpfileType.SHP_POLYLINEZ)
			{
				//delete the shapeFile of the same name then re-create it.
                if (Globals.PrepareResultSF(ref resultSFPath, ref resultSF,  sfType, copyAttributes) == false)
				{
					return false;
				}

                if (copyAttributes)
                {
                    MapWinGIS.Field tmpField, currField;
                    for (int f = 0; f <= lineSF.NumFields - 1; f++)
                    {
                        tmpField = new MapWinGIS.Field();
                        currField = lineSF.get_Field(f);
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

				int numLines = lineSF.NumShapes;
				int numPolygonPts = polygon.numPoints;
				for(int i = 0; i <= numLines-1; i++)
				{
					MapWinGIS.Shape lineShp = new MapWinGIS.ShapeClass();
					lineShp = lineSF.get_Shape(i);
					//check to see if line and polygon bounds intersect
					boundsIntersect = Globals.CheckBounds(ref lineShp, ref polygon);

					if(boundsIntersect == false)
					{
						continue;
					}
					else
					{
						//line might intersect polygon
						int numPoints = lineShp.numPoints;
						bool[] ptsInside = new bool[numPoints];
						MapWinGIS.Point currPt = new MapWinGIS.PointClass();
						MapWinGIS.Point nextPt = new MapWinGIS.PointClass();
						int numInside = 0;
						int numOutside = 0;

						int numParts = polygon.NumParts;
						if(numParts == 0)
						{
							numParts = 1;
						}
						Globals.Vertex[][] polyVertArray = new Globals.Vertex[numParts][];
						Globals.ConvertPolyToVertexArray(ref polygon, out polyVertArray);

						//check each point in the line to see if the entire line is either
						//inside of the polygon or outside of it (we know it's inside polygon bounding box).
						for(int j = 0; j <= numPoints-1; j++)
						{
							currPt = lineShp.get_Point(j);
		
							//if(utils.PointInPolygon(polygon, currPt) == true)
							if(Utils.PointInPoly(ref polyVertArray, ref currPt) == true)
							{
								ptsInside[j] = true;
								numInside += 1;
							}
							else
							{
								ptsInside[j] = false;
								numOutside += 1;
							}
						}
						//case: all points are inside polygon - check for possible intersections
						if(numInside == numPoints)
						{
                            shpIndex = resultSF.NumShapes;
							if(ProcessAllInsidePoints(ref lineShp, ref polygon, ref resultSF)== false)
							{	
								gErrorMsg = "Problem processing inside points: " + MapWinGeoProc.Error.GetLastErrorMsg();
								Debug.WriteLine(gErrorMsg);
								MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                                MapWinUtility.Logger.Dbg(gErrorMsg);
								return false;
							}
                            if (copyAttributes)
                            {
                                for (int f = 0; f <= lineSF.NumFields - 1; f++)
                                {
                                    bool tmpbool = resultSF.EditCellValue(f, shpIndex, lineSF.get_CellValue(f, i));
                                }
                            }
						}
							//case: all points are outside of the polygon - check for possible intersections
						else if(numOutside == numPoints)
						{
                            shpIndex = resultSF.NumShapes;
							if(ProcessAllOutsidePoints(ref lineShp, ref polygon, ref resultSF)==false)
							{
								gErrorMsg = "Problem processing outside line points: " + MapWinGeoProc.Error.GetLastErrorMsg();
								Debug.WriteLine(gErrorMsg);
								MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                                MapWinUtility.Logger.Dbg(gErrorMsg);
								return false;
							}
                            if (resultSF.NumShapes > shpIndex)
                            {
                                if (copyAttributes)
                                {
                                    for (int f = 0; f <= lineSF.NumFields - 1; f++)
                                    {
                                        bool tmpbool = resultSF.EditCellValue(f, shpIndex, lineSF.get_CellValue(f, i));
                                    }
                                }
                            }
						}
						
							//case: part of line is inside and part is outside - find inside segments.
						else
						{
                            shpIndex = resultSF.NumShapes;
							if(ProcessPartInAndOutPoints(ref ptsInside, ref lineShp, ref polygon, ref resultSF)==false)
							{
								gErrorMsg = "Problem processing part in and out line: " + MapWinGeoProc.Error.GetLastErrorMsg();
								Debug.WriteLine(gErrorMsg);
								MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                                MapWinUtility.Logger.Dbg(gErrorMsg);
								return false;
							}
                            if (copyAttributes)
                            {
                                for (int f = 0; f <= lineSF.NumFields - 1; f++)
                                {
                                    bool tmpbool = resultSF.EditCellValue(f, shpIndex, lineSF.get_CellValue(f, i));
                                }
                            }
						}			

					}//end of else (boundsIntersect == true)
				}//end of looping through each line in the shapefile

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
				
				if(resultSF.StopEditingShapes(true, true, null) == false)
				{
					gErrorMsg = "Problem with saving result file: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
					Debug.WriteLine(gErrorMsg);
					MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                    MapWinUtility.Logger.Dbg(gErrorMsg);
					return false;
				}
				resultSF.SaveAs(resultSFPath, null);
				resultSF.Close();
				lineSF.Close();
			}//end of checking if shapefile is valid
			else
			{
				lineSF.Close();
				gErrorMsg = "Invalid shapefile type. Should be of type Line.";
				Debug.WriteLine(gErrorMsg);
				MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                MapWinUtility.Logger.Dbg(gErrorMsg);
				return false;
			}
            MapWinUtility.Logger.Dbg("Finished Accurate_ClipLineSFWithPolygon");
			return true;
		}
		#endregion

		#region Save-to-Memory version
		/// <summary>
		/// Given a line shapefile and a polygon, this function will find the
		/// portions of each line that lies within the polygon and save it in memory to resultLineSF. 
		/// </summary>
		/// <param name="lineSF">The line shapefile to be clipped by the polygon.</param>
		/// <param name="polygon">The polygon that might contain portions of one or more lines.</param>
		/// <param name="resultLineSF">The shapefile that will hold the newly clipped lines.</param>
        /// <param name="copyAttributes">True if copying Attributes</param>
		/// <returns>False if an error was encountered, true otherwise.</returns>
        public static bool Accurate_ClipLineSFWithPolygon(ref MapWinGIS.Shapefile lineSF, ref MapWinGIS.Shape polygon, out MapWinGIS.Shapefile resultLineSF, bool copyAttributes)
		{
            MapWinUtility.Logger.Dbg("Accurate_ClipLineSFWithPolygon(lineSF: " + Macro.ParamName(lineSF) + ",\n" +
                                     "                               polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                               resultLineSF: (out),\n" +
                                     "                               copyAttributes: " + copyAttributes.ToString() + ")");

		    MapWinGIS.Shapefile resultSF = new MapWinGIS.ShapefileClass();

		    //MapWinGIS.Utils utils = new MapWinGIS.UtilsClass();
			MapWinGIS.ShpfileType sfType = lineSF.ShapefileType;
			bool boundsIntersect = false;
            int shpIndex = 0;
            string tmpName;

			//make sure we are dealing with a valid shapefile type
			if(sfType == MapWinGIS.ShpfileType.SHP_POLYLINE || sfType == MapWinGIS.ShpfileType.SHP_POLYLINEM || sfType == MapWinGIS.ShpfileType.SHP_POLYLINEZ)
			{
				//create the result shapeFile
				string tempPath = System.IO.Path.GetTempPath() + "resultLineSf.shp";
				DataManagement.DeleteShapefile(ref tempPath);
                //if(resultSF.CreateNew(tempPath, sfType)==false)
                //{
                //    gErrorMsg = "Problem creating the result shapeFile: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
                //    Debug.WriteLine(gErrorMsg);
                //    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                //    resultLineSF = resultSF;
                //    return false;
                //}
                //CDM 8/4/2006 
                Globals.PrepareResultSF(ref tempPath, ref resultSF, sfType, copyAttributes);

                if (copyAttributes)
                {
                    MapWinGIS.Field tmpField, currField;
                    for (int f = 0; f <= lineSF.NumFields - 1; f++)
                    {
                        tmpField = new MapWinGIS.Field();
                        currField = lineSF.get_Field(f);
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

				int numLines = lineSF.NumShapes;
				int numPolygonPts = polygon.numPoints;
				for(int i = 0; i <= numLines-1; i++)
				{
					MapWinGIS.Shape lineShp = new MapWinGIS.ShapeClass();
					lineShp = lineSF.get_Shape(i);
					//check to see if line and polygon bounds intersect
					boundsIntersect = Globals.CheckBounds(ref lineShp, ref polygon);

					if(boundsIntersect == false)
					{
						continue;
					}
					else
					{
						//line might intersect polygon
						int numPoints = lineShp.numPoints;
						bool[] ptsInside = new bool[numPoints];
						MapWinGIS.Point currPt = new MapWinGIS.PointClass();
						MapWinGIS.Point nextPt = new MapWinGIS.PointClass();
						int numInside = 0;
						int numOutside = 0;

						int numParts = polygon.NumParts;
						if(numParts == 0)
						{
							numParts = 1;
						}
						Globals.Vertex[][] polyVertArray = new Globals.Vertex[numParts][];
						Globals.ConvertPolyToVertexArray(ref polygon, out polyVertArray);

						//check each point in the line to see if the entire line is either
						//inside of the polygon or outside of it (we know it's inside polygon bounding box).
						for(int j = 0; j <= numPoints-1; j++)
						{
							currPt = lineShp.get_Point(j);
		
							//if(utils.PointInPolygon(polygon, currPt) == true)
							if(Utils.PointInPoly(ref polyVertArray, ref currPt) == true)
							{
								ptsInside[j] = true;
								numInside += 1;
							}
							else
							{
								ptsInside[j] = false;
								numOutside += 1;
							}
						}
						//case: all points are inside polygon - check for possible intersections
						if(numInside == numPoints)
						{
                            shpIndex = resultSF.NumShapes;
							if(ProcessAllInsidePoints(ref lineShp, ref polygon, ref resultSF)== false)
							{	
								gErrorMsg = "Problem processing inside points: " + MapWinGeoProc.Error.GetLastErrorMsg();
								Debug.WriteLine(gErrorMsg);
								MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
								resultLineSF = resultSF;
                                MapWinUtility.Logger.Dbg(gErrorMsg);
								return false;
							}
                            if (copyAttributes)
                            {
                                for (int f = 0; f <= lineSF.NumFields - 1; f++)
                                {
                                    bool tmpbool = resultSF.EditCellValue(f, shpIndex, lineSF.get_CellValue(f, i));
                                }
                            }
						}
							//case: all points are outside of the polygon - check for possible intersections
						else if(numOutside == numPoints)
						{
                            shpIndex = resultSF.NumShapes;
							if(ProcessAllOutsidePoints(ref lineShp, ref polygon, ref resultSF)==false)
							{
								gErrorMsg = "Problem processing outside line points: " + MapWinGeoProc.Error.GetLastErrorMsg();
								Debug.WriteLine(gErrorMsg);
								MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
								resultLineSF = resultSF;
                                MapWinUtility.Logger.Dbg(gErrorMsg);
								return false;
							}
                            if (resultSF.NumShapes > shpIndex)
                            {
                                if (copyAttributes)
                                {
                                    for (int f = 0; f <= lineSF.NumFields - 1; f++)
                                    {
                                        bool tmpbool = resultSF.EditCellValue(f, shpIndex, lineSF.get_CellValue(f, i));
                                    }
                                }
                            }
						}
						
							//case: part of line is inside and part is outside - find inside segments.
						else
						{
                            shpIndex = resultSF.NumShapes;
							if(ProcessPartInAndOutPoints(ref ptsInside, ref lineShp, ref polygon, ref resultSF)==false)
							{
								gErrorMsg = "Problem processing part in and out line: " + MapWinGeoProc.Error.GetLastErrorMsg();
								Debug.WriteLine(gErrorMsg);
								MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
								resultLineSF = resultSF;
                                MapWinUtility.Logger.Dbg(gErrorMsg);
								return false;
							}
                            if (copyAttributes)
                            {
                                for (int f = 0; f <= lineSF.NumFields - 1; f++)
                                {
                                    bool tmpbool = resultSF.EditCellValue(f, shpIndex, lineSF.get_CellValue(f, i));
                                }
                            }
						}			

					}//end of else (boundsIntersect == true)
				}//end of looping through each line in the shapefile
				
                resultLineSF = resultSF;
                // Paul Meems 11 March 2011 - Give the result sf the same projections as the input:
			    resultLineSF.Projection = lineSF.Projection;

			}//end of checking if shapefile is valid
			else
			{
				gErrorMsg = "Invalid shapefile type. Should be of type Line.";
				Debug.WriteLine(gErrorMsg);
				MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
				resultLineSF = resultSF;
                MapWinUtility.Logger.Dbg(gErrorMsg);
				return false;
			}
			return true;
		}
		#endregion
		#endregion

		#region private ProcessAllInsidePoints() -- used by Accurate_ClipLineSFWithPolygon()
		//Angela Hillier 10/05
		/// <summary>
		/// For lines where every point lies within the polygon, this function will
		/// find if a 2pt segment crosses outside of the polygon and then adds only the
		/// internal segments to the result file.
		/// </summary>
		/// <param name="line">The line whose points are all inside the polygon.</param>
		/// <param name="polygon">The polygon being checked for intersection.</param>
		/// <param name="resultSF">The file the internal segments should be saved to.</param>
		/// <returns>False if errors were encountered, true otherwise.</returns>
		private static bool ProcessAllInsidePoints(ref MapWinGIS.Shape line, ref MapWinGIS.Shape polygon, ref MapWinGIS.Shapefile resultSF)
		{
            if (line.numPoints == 0) return true;

			int numLinePts = line.numPoints;
			int numLineSegs = numLinePts-1;
			int numPolyPts = polygon.numPoints;
			int[] intersectsPerSeg = new int[numLineSegs];
			MapWinGIS.Point[][] intersectPts = new MapWinGIS.Point[numLineSegs][];
			int[][] polyIntLocs = new int[numLineSegs][];
			
			//cut line into 2pt segments and put in new shapefile.
			MapWinGIS.Shapefile lineSegSF = new MapWinGIS.ShapefileClass();
			string tempPath = System.IO.Path.GetTempPath() + "tempLineSF.shp";
			DataManagement.DeleteShapefile(ref tempPath);
			//CDM 8/4/2006 lineSegSF.CreateNew(tempPath, line.ShapeType);
            Globals.PrepareResultSF(ref tempPath, ref lineSegSF, line.ShapeType);
			int shpIndex = 0;
			MapWinGIS.Shape lineSegment;
			for(int i = 0; i <= numLineSegs-1; i++)
			{
				lineSegment = new MapWinGIS.ShapeClass();
				lineSegment.ShapeType = line.ShapeType;
				int ptIndex = 0;
				lineSegment.InsertPoint(line.get_Point(i), ref ptIndex);
				ptIndex = 1;
				lineSegment.InsertPoint(line.get_Point(i+1), ref ptIndex);
				shpIndex = lineSegSF.NumShapes;
				lineSegSF.EditInsertShape(lineSegment, ref shpIndex);			

				intersectPts[i] = new MapWinGIS.Point[numPolyPts];
				polyIntLocs[i] = new int[numPolyPts];
			}

			int numIntersects = Globals.CalcSiDeterm(ref lineSegSF, ref polygon, out intersectsPerSeg, out intersectPts, out polyIntLocs);
			
			if (numIntersects == 0)//entire line is inside the polygon
			{
				shpIndex = resultSF.NumShapes;
				resultSF.EditInsertShape(line, ref shpIndex);
				return true;
			}
			else
			{
				//intersections exist!
				MapWinGIS.Shape insideLine = new MapWinGIS.ShapeClass();
				insideLine.ShapeType = line.ShapeType;
				int ptIndex = 0;
				for(int i=0; i <= numLineSegs-1; i++)
				{
					int numSegIntersects = intersectsPerSeg[i];
					if(numSegIntersects == 0)
					{
						ptIndex = insideLine.numPoints;
						insideLine.InsertPoint(lineSegSF.get_Shape(i).get_Point(0), ref ptIndex);
						ptIndex++;
						insideLine.InsertPoint(lineSegSF.get_Shape(i).get_Point(1), ref ptIndex);
						int c = i+1;
						while(intersectsPerSeg[c] == 0 && c <= numLineSegs-1)
						{
							ptIndex = insideLine.numPoints;
							insideLine.InsertPoint(lineSegSF.get_Shape(c).get_Point(0), ref ptIndex);
							ptIndex++;
							insideLine.InsertPoint(lineSegSF.get_Shape(c).get_Point(1), ref ptIndex);
							c++;
							if(c == numLineSegs)
							{
								break;
							}
						}
						i = c-1;
						if(i == numLineSegs-1)
						{
							shpIndex = resultSF.NumShapes;
							resultSF.EditInsertShape(insideLine, ref shpIndex);
						}
					}

					else
					{	//there should always be an even # of intersects for a line of all inside pts
						//find the valid intersect points from our array
						MapWinGIS.Point[] intPts = new MapWinGIS.Point[numSegIntersects];
						MapWinGIS.Point startPt = new MapWinGIS.PointClass();
						startPt = lineSegSF.get_Shape(i).get_Point(0);
						
						Globals.FindAndSortValidIntersects(numSegIntersects, ref intersectPts[i], ref intPts, ref startPt);

						//add internal segments to the result shapefile
						for(int j =0; j<= numSegIntersects-1; j++)
						{
							if(j == 0)
							{
								//add first inside point
								ptIndex = insideLine.numPoints;
								insideLine.InsertPoint(lineSegSF.get_Shape(i).get_Point(0), ref ptIndex);
								//add first intersect to close off inside segment
								ptIndex++;
								insideLine.InsertPoint(intPts[j], ref ptIndex);
								//this finishes the line, add it to the result file
								shpIndex = resultSF.NumShapes;
								resultSF.EditInsertShape(insideLine, ref shpIndex);
								//create a new line holder
								insideLine = new MapWinGIS.ShapeClass();
								insideLine.ShapeType = line.ShapeType;
							}
							else if(j <= numSegIntersects-2)
							{
								//we're adding internal segments made from two intersection points
								ptIndex = 0;
								insideLine.InsertPoint(intPts[j], ref ptIndex);
								ptIndex = 1;
								insideLine.InsertPoint(intPts[j+1], ref ptIndex);
								//that's all there is to the inside segment, add it to the result shapefile
								shpIndex = resultSF.NumShapes;
								resultSF.EditInsertShape(insideLine, ref shpIndex);
								//clear the holder
								insideLine = new MapWinGIS.ShapeClass();
								insideLine.ShapeType = line.ShapeType;
								//increase j because we have added two intersects in this round
								j++;
							}
							else
							{
								//we're at the last intersect point
								ptIndex = 0;
								insideLine.InsertPoint(intPts[j], ref ptIndex);
								ptIndex = 1;
								//add last point of the 2pt segment to the result shape
								insideLine.InsertPoint(lineSegSF.get_Shape(i).get_Point(1), ref ptIndex);
								//this finishes off the line segment, add to result shapefile
								shpIndex = resultSF.NumShapes;
								resultSF.EditInsertShape(insideLine, ref shpIndex);
								//clear line holder
								insideLine = new MapWinGIS.ShapeClass();
								insideLine.ShapeType = line.ShapeType;
							}
						}//end of looping through segment intersect points						
					}//end of else intersects exist for current segment					
				}//end of looping through line segments	
				return true;
			}//end of else intersections exist in line			
		}
		#endregion

		#region private ProcessAllOutsidePoints() -- used by Accurate_ClipLineSFWithPolygon()
		//Angela Hillier 10/05
		/// <summary>
		/// For lines where every point lies outside the polygon, this function will
		/// find if any 2pt segment crosses within the polygon and then adds the
		/// internal segments to the result file.
		/// </summary>
		/// <param name="line">The line whose points are all outside of the polygon.</param>
		/// <param name="polygon">The polygon being checked for intersection.</param>
		/// <param name="resultSF">The file internal segments should be saved to.</param>
		/// <returns>False if errors were encountered, true otherwise.</returns>
		private static bool ProcessAllOutsidePoints(ref MapWinGIS.Shape line, ref MapWinGIS.Shape polygon, ref MapWinGIS.Shapefile resultSF)
		{
			int numLinePts = line.numPoints;
			int numLineSegs = numLinePts-1;
			int numPolyPts = polygon.numPoints;
			int[] intersectsPerSeg = new int[numLineSegs];
			MapWinGIS.Point[][] intersectPts = new MapWinGIS.Point[numLineSegs][];
			int[][] polyIntLocs = new int[numLineSegs][];
			
			//cut line into 2pt segments and put in new shapefile.
			MapWinGIS.Shapefile lineSegSF = new MapWinGIS.ShapefileClass();
			string tempPath = System.IO.Path.GetTempPath() + "tempLineSF.shp";
			DataManagement.DeleteShapefile(ref tempPath);
            //CDM 8/4/2006 lineSegSF.CreateNew(tempPath, line.ShapeType);
            Globals.PrepareResultSF(ref tempPath, ref lineSegSF, line.ShapeType);
			int shpIndex = 0;
			MapWinGIS.Shape lineSegment;
			for(int i = 0; i <= numLineSegs-1; i++)
			{
				lineSegment = new MapWinGIS.ShapeClass();
				lineSegment.ShapeType = line.ShapeType;
				int ptIndex = 0;
				lineSegment.InsertPoint(line.get_Point(i), ref ptIndex);
				ptIndex = 1;
				lineSegment.InsertPoint(line.get_Point(i+1), ref ptIndex);
				shpIndex = lineSegSF.NumShapes;
				lineSegSF.EditInsertShape(lineSegment, ref shpIndex);			

				intersectPts[i] = new MapWinGIS.Point[numPolyPts];
				polyIntLocs[i] = new int[numPolyPts];
			}

			int numIntersects = Globals.CalcSiDeterm(ref lineSegSF, ref polygon, out intersectsPerSeg, out intersectPts, out polyIntLocs);
			
			if (numIntersects == 0)//entire line is outside the polygon
			{
				// do not add any portion of the line to resultSF
				return true;
			}
			else
			{
				//intersections exist!
				MapWinGIS.Shape insideLine = new MapWinGIS.ShapeClass();
				insideLine.ShapeType = line.ShapeType;
				int ptIndex = 0;
				for(int i=0; i <= numLineSegs-1; i++)
				{
					int numSegIntersects = intersectsPerSeg[i];
					if(numSegIntersects == 0)
					{
						//do not add the segment to the result file,
						//this portion is not inside the polygon
					}
					else
					{	//there should always be an even # of intersects for a line of all outside pts
						//find the valid intersect points from our array
                        if (numSegIntersects > 1)
                        {
                            MapWinGIS.Point[] intPts = new MapWinGIS.Point[numSegIntersects];
                            MapWinGIS.Point startPt = new MapWinGIS.PointClass();
                            startPt = lineSegSF.get_Shape(i).get_Point(0);

                            Globals.FindAndSortValidIntersects(numSegIntersects, ref intersectPts[i], ref intPts, ref startPt);


                            //add only internal segments to the result shapefile
                            for (int j = 0; j <= numSegIntersects - 1; j++)
                            {
                                //no need to add first or last point of the line segment,
                                //both are outside of the polygon.
                                //We need to form the internal semgents out of two intersection points
                                ptIndex = 0;
                                insideLine.InsertPoint(intPts[j], ref ptIndex);
                                ptIndex = 1;
                                insideLine.InsertPoint(intPts[j + 1], ref ptIndex);
                                //that's all there is to the inside segment, add it to the result shapefile
                                shpIndex = resultSF.NumShapes;
                                resultSF.EditInsertShape(insideLine, ref shpIndex);
                                //clear the holder
                                insideLine = new MapWinGIS.ShapeClass();
                                insideLine.ShapeType = line.ShapeType;
                                //increase j because we have added two intersects
                                j++;
                            }//end of looping through segment intersect points	
                        }
					}//end of else intersects exist for current segment					
				}//end of looping through line segments	
				return true;
			}//end of else line intersects the polygon
		}
		#endregion

		#region private ProcessPartInAndOutPoints() -- used by Accurate_ClipLineSFWithPolygon()
		//Angela Hillier 10/05
		/// <summary>
		/// For lines where part of the line lies within the polygon
		/// and part of it lies outside, this function will save only
		/// the inside portions and intersection points to the result shapefile.
		/// </summary>
		/// <param name="insidePts">A boolean array corresponding to each point in
		/// the line signifying if the point lies within the polygon or not.</param>
		/// <param name="line">A line that has at least one part of it within the polygon.</param>
		/// <param name="polygon">The polygon containing a portion of the line.</param>
		/// <param name="resultSF">The file that will contain the portions of the line that lie within the polygon.</param>
		/// <returns>False if an error is encountered, true otherwise.</returns>
		private static bool ProcessPartInAndOutPoints(ref bool[] insidePts, ref MapWinGIS.Shape line, ref MapWinGIS.Shape polygon, ref MapWinGIS.Shapefile resultSF)
		{
			int numPolyPts = polygon.numPoints;
			int numLinePts = line.numPoints;
			MapWinGIS.Shape insideLine;
			MapWinGIS.Shape outsideLine;
			MapWinGIS.Shape intersectLine;
			MapWinGIS.Point startingIntersectPt = new MapWinGIS.PointClass();
			int addToShpIndex = 0;
			bool addStartingIntersect = false;
			bool startInside = insidePts[0];
			MapWinGIS.Shapefile tempResultSF = new MapWinGIS.ShapefileClass();
			string tempPath = System.IO.Path.GetTempPath() + "tempResultSF.shp";
			DataManagement.DeleteShapefile(ref tempPath);
			//CDM 8/4/2006 tempResultSF.CreateNew(tempPath, line.ShapeType);
            Globals.PrepareResultSF(ref tempPath, ref tempResultSF, line.ShapeType);
			int ptIndex = 0;
			int shpIndex = 0;
			int[] intersectsPerSeg = new int[1];
			MapWinGIS.Point[][] intersectPts = new MapWinGIS.Point[1][];
			int[][] polyIntLocs = new int[1][];
			
			for(int i=0; i<= numLinePts-2; i++)
			{
				insideLine = new MapWinGIS.ShapeClass();
				insideLine.ShapeType = line.ShapeType;
				outsideLine = new MapWinGIS.ShapeClass();
				outsideLine.ShapeType = line.ShapeType;
				intersectLine = new MapWinGIS.ShapeClass();
				intersectLine.ShapeType = line.ShapeType;
				MapWinGIS.Shapefile intersectLineSF = new MapWinGIS.ShapefileClass();
				string tempPath2 = System.IO.Path.GetTempPath() + "intersectSF.shp";
				DataManagement.DeleteShapefile(ref tempPath2);
				//CDM 8/4/2006 intersectLineSF.CreateNew(tempPath2, line.ShapeType);
                Globals.PrepareResultSF(ref tempPath2, ref intersectLineSF, line.ShapeType);
				intersectPts[0] = new MapWinGIS.Point[numPolyPts];
				polyIntLocs[0] = new int[numPolyPts];

				//**************Build a line of "all inside" points if appropriate***********
				if(startInside == true && insidePts[i+1] == true)
				{
					ptIndex = 0;
					insideLine.InsertPoint(line.get_Point(i), ref ptIndex);
					int c = i+1;
					while(insidePts[c] == true && c <= numLinePts-1)
					{
						ptIndex = insideLine.numPoints;
						insideLine.InsertPoint(line.get_Point(c), ref ptIndex);
						c++;
						if(c == numLinePts)
						{
							break;
						}
					}
					i = c-1;
					//now we should have a line of all inside values
					//and c represents the first outside point encountered.
					//process insideLine.
					if(ProcessAllInsidePoints(ref insideLine, ref polygon, ref tempResultSF) == false)
					{
						gErrorMsg = "Problem processing inside points.";
						MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
						Debug.WriteLine(gErrorMsg);
						return false;
					}
					if(addStartingIntersect == true)
					{
						ptIndex = 0;
						tempResultSF.get_Shape(addToShpIndex).InsertPoint(startingIntersectPt, ref ptIndex);
						addToShpIndex = 0;
						startingIntersectPt = new MapWinGIS.PointClass();
						addStartingIntersect = false;
					}

					if(i != numLinePts-1)
					{
						//we need to find the interesect pts existing between
						//the last inside point and the next outside point.					
						ptIndex = 0;
						intersectLine.InsertPoint(line.get_Point(c-1), ref ptIndex);
						ptIndex = 1;
						intersectLine.InsertPoint(line.get_Point(c), ref ptIndex);
						shpIndex = 0;
						intersectLineSF.EditInsertShape(intersectLine, ref shpIndex);
						int numIntersects = Globals.CalcSiDeterm(ref intersectLineSF, ref polygon, out intersectsPerSeg, out intersectPts, out polyIntLocs);
						
						if(numIntersects == 0)
						{
							//there was a problem, should have at least one intersect pt for this case
							return false;
						}
						else if(numIntersects == 1)
						{
							//add intersect pt to end of last shape in tempResultSF
							shpIndex = tempResultSF.NumShapes;
							ptIndex = tempResultSF.get_Shape(shpIndex-1).numPoints;
							tempResultSF.get_Shape(shpIndex-1).InsertPoint(intersectPts[0][0], ref ptIndex);
							startInside = false;
							continue;
						}
						else //more than one intersect pt exists (should always be an odd number)
						{
							//find valid intersect points and sort them
							MapWinGIS.Point startPt = new MapWinGIS.PointClass();
							startPt = intersectLine.get_Point(0);
							MapWinGIS.Point[] intersects = new MapWinGIS.Point[numIntersects];

							Globals.FindAndSortValidIntersects(numIntersects, ref intersectPts[0], ref intersects, ref startPt);
						
							//add the intersect points to tempResultSF
							for(int j=0; j <= numIntersects-1; j++)
							{
								if(j == 0)
								{
									//add it to end of last shape in tempResultFile
									shpIndex = tempResultSF.NumShapes;
									ptIndex = tempResultSF.get_Shape(shpIndex-1).numPoints;
									tempResultSF.get_Shape(shpIndex-1).InsertPoint(intersects[j], ref ptIndex);
								}
								else if(j < numIntersects-1)
								{
									MapWinGIS.Shape insideSeg = new MapWinGIS.ShapeClass();
									insideSeg.ShapeType = line.ShapeType;
									ptIndex = 0;
									insideSeg.InsertPoint(intersects[j], ref ptIndex);
									ptIndex = 1;
									insideSeg.InsertPoint(intersects[j+1], ref ptIndex);
									shpIndex = tempResultSF.NumShapes;
									tempResultSF.EditInsertShape(insideSeg, ref shpIndex);
									j++;
								}
							}//end of looping through valid intersect points
							startInside = false;
						}//end of else more than one intersect pt exists
					}//end of if i == numLinePts-1
				}//end of building inside line
				
					//**************Build a line of "all outside" points if appropriate*******************
				else if(startInside == false && insidePts[i+1] == false)
				{
					ptIndex = 0;
					outsideLine.InsertPoint(line.get_Point(i), ref ptIndex);
					int c = i+1;
					while(insidePts[c] == false && c <= numLinePts-1)
					{
						ptIndex = outsideLine.numPoints;
						outsideLine.InsertPoint(line.get_Point(c), ref ptIndex);						
						c++;
						if(c == numLinePts)
						{
							break;
						}
					}
					i = c-1;
					//now c represents the first inside point encountered
					//process outsideLine.
					if(ProcessAllOutsidePoints(ref outsideLine, ref polygon, ref tempResultSF) == false)
					{
						gErrorMsg = "Error processing outside points.";
						Debug.WriteLine(gErrorMsg);
						MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
						return false;
					}
					if(i != numLinePts-1)
					{
						//we need to find any interesect pts existing between
						//the last outside point and the next inside point.					
						ptIndex = 0;
						intersectLine.InsertPoint(line.get_Point(c-1), ref ptIndex);
						ptIndex = 1;
						intersectLine.InsertPoint(line.get_Point(c), ref ptIndex);
						shpIndex = 0;
						intersectLineSF.EditInsertShape(intersectLine, ref shpIndex);
					
						int numIntersects = Globals.CalcSiDeterm(ref intersectLineSF, ref polygon, out intersectsPerSeg, out intersectPts, out polyIntLocs);
					
						if(numIntersects == 0)
						{
							//there was a problem, should have at least one intersect pt for this case
							gErrorMsg = "No intersect was found but one was expected.";
							MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
							Debug.WriteLine(gErrorMsg);
							return false;
						}
					
						else if(numIntersects == 1)
						{
							//the intersect and inside points will be at
							//the beginning of a new shape in tempResultSF
							if(c != numLinePts-1)
							{
								startingIntersectPt = new MapWinGIS.PointClass();
								startingIntersectPt = intersectPts[0][0];
								addStartingIntersect = true;
								addToShpIndex = tempResultSF.NumShapes;
							}
							else //need to deal with last segment
							{
								MapWinGIS.Shape innerSeg = new MapWinGIS.ShapeClass();
								innerSeg.ShapeType = line.ShapeType;
								ptIndex = 0;
								innerSeg.InsertPoint(intersectPts[0][0], ref ptIndex);
								ptIndex = 1;
								innerSeg.InsertPoint(line.get_Point(c), ref ptIndex);
								shpIndex = tempResultSF.NumShapes;
								tempResultSF.EditInsertShape(innerSeg, ref shpIndex);
							}
							startInside = true;
							continue;
						}
					
						else //more than one intersect pt exists (should always be an odd number)
						{
							//find valid intersect points and sort them
							MapWinGIS.Point startPt = new MapWinGIS.PointClass();
							startPt = intersectLine.get_Point(0);
							MapWinGIS.Point[] intersects = new MapWinGIS.Point[numIntersects];
						
							Globals.FindAndSortValidIntersects(numIntersects, ref intersectPts[0], ref intersects, ref startPt);

							//add the intersect points to tempResultSF
							for(int j=0; j <= numIntersects-1; j++)
							{
								if(j < numIntersects-1)
								{
									MapWinGIS.Shape insideSeg = new MapWinGIS.ShapeClass();
									insideSeg.ShapeType = line.ShapeType;
									ptIndex = 0;
									insideSeg.InsertPoint(intersects[j], ref ptIndex);
									ptIndex = 1;
									insideSeg.InsertPoint(intersects[j+1], ref ptIndex);
									shpIndex = tempResultSF.NumShapes;
									tempResultSF.EditInsertShape(insideSeg, ref shpIndex);
									j++;
								}
								else if(j == numIntersects-1)
								{
									if(c != numLinePts-1)
									{
										//intersect needs to be at the beginning of the next shape
										startingIntersectPt = new MapWinGIS.PointClass();
										startingIntersectPt = intersects[j];
										addStartingIntersect = true;
										addToShpIndex = tempResultSF.NumShapes;
									}
									else //we need to deal with the very last segment
									{
										MapWinGIS.Shape insideSeg = new MapWinGIS.ShapeClass();
										insideSeg.ShapeType = line.ShapeType;
										ptIndex = 0;
										//add last intersect point
										insideSeg.InsertPoint(intersects[j], ref ptIndex);
										ptIndex = 1;
										//add inside point
										insideSeg.InsertPoint(line.get_Point(c), ref ptIndex);
										tempResultSF.EditInsertShape(insideSeg, ref shpIndex);
									}
								}
							}//end of looping through valid intersect points
							startInside = true;
						}//end of else more than one intersect pt exists
					}
				}//end of build line of "all outside" points

					//**************Deal with inside->outside segment*********************
				else if(startInside == true && insidePts[i+1] == false)
				{
					ptIndex = 0;
					intersectLine.InsertPoint(line.get_Point(i), ref ptIndex);
					ptIndex = 1;
					intersectLine.InsertPoint(line.get_Point(i+1), ref ptIndex);
					shpIndex = 0;
					intersectLineSF.EditInsertShape(intersectLine, ref shpIndex);
					
					//calculate intersect points
					int numIntersects = Globals.CalcSiDeterm(ref intersectLineSF, ref polygon, out intersectsPerSeg, out intersectPts, out polyIntLocs);
					
					if(numIntersects == 0)
					{
						gErrorMsg = "No intersect was found but one was expected.";
						MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
						Debug.WriteLine(gErrorMsg);
						return false;
					}
					
					else if(numIntersects == 1)
					{
						shpIndex = tempResultSF.NumShapes;
						MapWinGIS.Shape insideSeg = new MapWinGIS.ShapeClass();
						insideSeg.ShapeType = line.ShapeType;
						ptIndex = 0;
						insideSeg.InsertPoint(line.get_Point(i), ref ptIndex);
						ptIndex = 1;
						insideSeg.InsertPoint(intersectPts[0][0], ref ptIndex);
						tempResultSF.EditInsertShape(insideSeg, ref shpIndex);

						//there might be an additional point waiting to be added!
						if(addStartingIntersect == true)
						{
							ptIndex = 0;
							tempResultSF.get_Shape(addToShpIndex).InsertPoint(startingIntersectPt, ref ptIndex);
							addToShpIndex = 0;
							startingIntersectPt = new MapWinGIS.PointClass();
							addStartingIntersect = false;
						}
					}
					
					else //more than one intersect exists
					{
						//find valid intersect points and sort them
						MapWinGIS.Point startPt = new MapWinGIS.PointClass();
						startPt = line.get_Point(i);
						MapWinGIS.Point[] intersects = new MapWinGIS.Point[numIntersects];
						
						Globals.FindAndSortValidIntersects(numIntersects, ref intersectPts[0], ref intersects, ref startPt);

						for(int j=0; j <= numIntersects-1; j++)
						{
							if(j == 0)
							{
								shpIndex = tempResultSF.NumShapes;

								MapWinGIS.Shape insideSeg = new MapWinGIS.ShapeClass();
								insideSeg.ShapeType = line.ShapeType;
								ptIndex = 0;
								insideSeg.InsertPoint(line.get_Point(i), ref ptIndex);
								ptIndex = 1;
								insideSeg.InsertPoint(intersects[j], ref ptIndex);
								tempResultSF.EditInsertShape(insideSeg, ref shpIndex);
								
								//there might be an additional point waiting to be added!
								if(addStartingIntersect == true)
								{
									ptIndex = 0;
									tempResultSF.get_Shape(addToShpIndex).InsertPoint(startingIntersectPt, ref ptIndex);
									addToShpIndex = 0;
									startingIntersectPt = new MapWinGIS.PointClass();
									addStartingIntersect = false;
								}
							}
							else if(j < numIntersects-1)
							{
								MapWinGIS.Shape insideSeg = new MapWinGIS.ShapeClass();
								insideSeg.ShapeType = line.ShapeType;
								ptIndex = 0;
								insideSeg.InsertPoint(intersects[j], ref ptIndex);
								ptIndex = 1;
								insideSeg.InsertPoint(intersects[j+1], ref ptIndex);
								shpIndex = tempResultSF.NumShapes;
								tempResultSF.EditInsertShape(insideSeg, ref shpIndex);
								j++;
							}
						}//end of looping through intersects
					}//end of else more than one intersect exists
					startInside = false;
				} //end of else inside->outside

					//**************Deal with outside->inside segment**********************
				else if(startInside == false && insidePts[i+1] == true)
				{
					ptIndex = 0;
					intersectLine.InsertPoint(line.get_Point(i), ref ptIndex);
					ptIndex = 1;
					intersectLine.InsertPoint(line.get_Point(i+1), ref ptIndex);
					shpIndex = 0;
					intersectLineSF.EditInsertShape(intersectLine, ref shpIndex);
					
					//calculate intersect points
					int numIntersects = Globals.CalcSiDeterm(ref intersectLineSF, ref polygon, out intersectsPerSeg, out intersectPts, out polyIntLocs);
					
					if(numIntersects == 0)
					{
						gErrorMsg = "No intersect was found but one was expected.";
						MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
						Debug.WriteLine(gErrorMsg);
						return false;
					}

					else if(numIntersects == 1)
					{
						if(i == numLinePts-2)
						{
							//add last inside segment to result file
							MapWinGIS.Shape insideSeg = new MapWinGIS.ShapeClass();
							insideSeg.ShapeType = line.ShapeType;
							ptIndex = 0;
							insideSeg.InsertPoint(intersectPts[0][0], ref ptIndex);
							ptIndex = 1;
							insideSeg.InsertPoint(line.get_Point(i+1), ref ptIndex);
							
							shpIndex = tempResultSF.NumShapes;
							tempResultSF.EditInsertShape(insideSeg, ref shpIndex);			
						}
						else
						{	
							//intersect needs to be at the beginning of the next shape
							startingIntersectPt = new MapWinGIS.PointClass();
							startingIntersectPt = intersectPts[0][0];
							addStartingIntersect = true;
							addToShpIndex = tempResultSF.NumShapes;
						}
					}
					
					else //more than one intersect exists (should always be an odd #)
					{
						//find valid intersect points and sort them
						MapWinGIS.Point startPt = new MapWinGIS.PointClass();
						startPt = line.get_Point(i);
						MapWinGIS.Point[] intersects = new MapWinGIS.Point[numIntersects];
						
						Globals.FindAndSortValidIntersects(numIntersects, ref intersectPts[0], ref intersects, ref startPt);

						for(int j = 0; j <= numIntersects-1; j++)
						{
							if(j < numIntersects-1)
							{
								MapWinGIS.Shape insideSeg = new MapWinGIS.ShapeClass();
								insideSeg.ShapeType = line.ShapeType;
								ptIndex = 0;
								insideSeg.InsertPoint(intersects[j], ref ptIndex);
								ptIndex = 1;
								insideSeg.InsertPoint(intersects[j+1], ref ptIndex);
								shpIndex = tempResultSF.NumShapes;
								tempResultSF.EditInsertShape(insideSeg, ref shpIndex);
								j++;
							}
							else
							{
								if(i == numLinePts-2)
								{
									//add last inside segment to result file
									MapWinGIS.Shape insideSeg = new MapWinGIS.ShapeClass();
									insideSeg.ShapeType = line.ShapeType;
									ptIndex = 0;
									insideSeg.InsertPoint(intersects[j], ref ptIndex);
									ptIndex = 1;
									insideSeg.InsertPoint(line.get_Point(i+1), ref ptIndex);

									shpIndex = tempResultSF.NumShapes;
									tempResultSF.EditInsertShape(insideSeg, ref shpIndex);
								}
								else
								{	
									//intersect needs to be at the beginning of the next shape
									startingIntersectPt = new MapWinGIS.PointClass();
									startingIntersectPt = intersects[j];
									addStartingIntersect = true;
									addToShpIndex = tempResultSF.NumShapes;
								}
							}//end of adding last intersect							
						}//end of looping through intersect points
					}//end of else more than one intersect point exists				
					
					startInside = true;
				}//end of dealing with outside->inside segment
			}//end of looping through line points
			//finish by copying all of the tempResultSF shapes into resultSF
			int numShapes = tempResultSF.NumShapes;
			for(int i = 0; i <= numShapes-1; i++)
			{
				shpIndex = resultSF.NumShapes;
				resultSF.EditInsertShape(tempResultSF.get_Shape(i), ref shpIndex);
			}
			return true;
		}
		#endregion

	}
}
