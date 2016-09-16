//File name: ClipPolyWithLine.cs
//Description: Internal class, provides functions for clipping a polygon shape with a line shape.
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
//06-03-06 ah - Angela Hillier - Added ClipMultiPartPolyWithLine to handle multipart polygons.							
//********************************************************************************************************
using System;
using System.Diagnostics;

namespace MapWinGeoProc
{
	/// <summary>
	/// Functions for clipping a polygon with a line.
	/// </summary>
	internal class ClipPolyWithLine
	{
		private static string gErrorMsg = "";

		#region ClipMultiPartPolyWithLine()
		#region save-to-disk version
        public static bool ClipMultiPartPolyWithLine(ref MapWinGIS.Shape polygon, ref MapWinGIS.Shape line, ref string resultFile, bool speedOptimized)
        {
            MapWinUtility.Logger.Dbg("ClipMultiPartPolyWithLine(polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                          line: " + Macro.ParamName(line) + ",\n" +
                                     "                          resultFile: " + resultFile + ",\n" +
                                     "                          speedOptimized: " + speedOptimized.ToString() + ")");
            return ClipMultiPartPolyWithLine(ref polygon, ref line, ref resultFile, speedOptimized, false);
        }
		public static bool ClipMultiPartPolyWithLine(ref MapWinGIS.Shape polygon, ref MapWinGIS.Shape line, ref string resultFile, bool speedOptimized, bool SkipMWShapeID)
		{
            MapWinUtility.Logger.Dbg("ClipMultiPartPolyWithLine(polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                          line: " + Macro.ParamName(line) + ",\n" +
                                     "                          resultFile: " + resultFile + ",\n" +
                                     "                          speedOptimized: " + speedOptimized.ToString() + ",\n" +
                                     "                          SkipMWShapeID: " + speedOptimized.ToString() + ")");
			int numParts = polygon.NumParts;
			if(numParts == 0)
				numParts = 1;
			if(numParts > 1)
			{
				//multiple parts
				int shpIndex = 0;
				Globals.FixMultiPartPoly(ref polygon);
				MapWinGIS.Shape[] polyParts = new MapWinGIS.Shape[numParts];
				Globals.SeparateParts(ref polygon, out polyParts);
                
				MapWinGIS.Shapefile holeSF = new MapWinGIS.ShapefileClass();
				string tempFile = System.IO.Path.GetTempPath() + "tempHoleSF.shp";
				DataManagement.DeleteShapefile(ref tempFile);
				//CDM 8/4/2006 holeSF.CreateNew(tempFile, polygon.ShapeType);
                Globals.PrepareResultSF(ref tempFile, ref holeSF, polygon.ShapeType);

				holeSF.StartEditingShapes(true, null);

				MapWinGIS.Shapefile tempResult = new MapWinGIS.ShapefileClass();
				string tempResultFile = System.IO.Path.GetTempPath() + "tempResultSF.shp";
				DataManagement.DeleteShapefile(ref tempResultFile);
				//CDM 8/4/2006 tempResult.CreateNew(tempResultFile, polygon.ShapeType);
                Globals.PrepareResultSF(ref tempResultFile, ref tempResult, polygon.ShapeType);

				tempResult.StartEditingShapes(true, null);

				MapWinGIS.Shapefile modPolySF = new MapWinGIS.ShapefileClass();
				string modPolyFile = System.IO.Path.GetTempPath() + "tempModPolySF.shp";
				DataManagement.DeleteShapefile(ref modPolyFile);
				//CDM 8/4/2006 modPolySF.CreateNew(modPolyFile, polygon.ShapeType);
                Globals.PrepareResultSF(ref modPolyFile, ref modPolySF, polygon.ShapeType);

				modPolySF.StartEditingShapes(true, null);

				MapWinGIS.Shapefile resultSF = new MapWinGIS.ShapefileClass();
				DataManagement.DeleteShapefile(ref resultFile);
				//CDM 8/4/2006 resultSF.CreateNew(resultFile, polygon.ShapeType);
                Globals.PrepareResultSF(ref resultFile, ref resultSF, polygon.ShapeType);
				resultSF.StartEditingShapes(true, null);

				for(int i = 0; i <= numParts-1; i++)
				{
					MapWinGIS.Shape currPart = new MapWinGIS.ShapeClass();
					currPart.Create(polygon.ShapeType);
					currPart = polyParts[i];
					if(Globals.IsClockwise(ref currPart))
					{
						if(speedOptimized)
							Fast_ClipPolygonWithLine(ref currPart, ref line, out tempResult);
						else
							Accurate_ClipPolygonWithLine(ref currPart, ref line, out tempResult);

						int numResults = tempResult.NumShapes;
						if(numResults > 0)
						{
							for(int j= 0; j <= numResults-1; j++)
							{
								shpIndex = modPolySF.NumShapes;
								modPolySF.EditInsertShape(tempResult.get_Shape(j), ref shpIndex);
							}
						}
					}
					else
					{
						//current part is a hole, add to holeSF
						shpIndex = holeSF.NumShapes;
						holeSF.EditInsertShape(currPart, ref shpIndex);
					}
				}

				if(holeSF.NumShapes > 0)
				{
					Erase.ErasePolySFWithPolySF(ref modPolySF, ref holeSF, ref resultSF);
				}

				if(resultSF.NumShapes > 0)
				{
					if (!SkipMWShapeID) Globals.DoInsertIDs(ref resultSF);
					resultSF.StopEditingShapes(true, true, null);
					resultSF.SaveAs(resultFile, null);
					resultSF.Close();
					return true;
				}
				else
				{
					gErrorMsg = "No shapes formed during clipping.";
					Debug.WriteLine(gErrorMsg);
					Error.SetErrorMsg(gErrorMsg);
                    MapWinUtility.Logger.Dbg(gErrorMsg);
					return false;
				}
			
			}
			else //single part polygon
			{
				if(speedOptimized)
					return Fast_ClipPolygonWithLine(ref polygon, ref line, ref resultFile);
				else
					return Accurate_ClipPolygonWithLine(ref polygon, ref line, ref resultFile);
			}			
		}
		#endregion

		#region in-memory version
        public static bool ClipMultiPartPolyWithLine(ref MapWinGIS.Shape polygon, ref MapWinGIS.Shape line, out MapWinGIS.Shapefile resultFile, bool speedOptimized)
        {
            MapWinUtility.Logger.Dbg("ClipMultiPartPolyWithLine(polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                          line: " + Macro.ParamName(line) + ",\n" +
                                     "                          resultFile: out,\n" +
                                     "                          speedOptimized: " + speedOptimized.ToString() + ")");
            return ClipMultiPartPolyWithLine(ref polygon, ref line, out resultFile, speedOptimized, false);
        }
		public static bool ClipMultiPartPolyWithLine(ref MapWinGIS.Shape polygon, ref MapWinGIS.Shape line, out MapWinGIS.Shapefile resultFile, bool speedOptimized, bool SkipMWShapeID)
		{
            MapWinUtility.Logger.Dbg("ClipMultiPartPolyWithLine(polygon: " + Macro.ParamName(polygon) + ",\n" +
                                    "                          line: " + Macro.ParamName(line) + ",\n" +
                                    "                          resultFile: out,\n" +
                                    "                          speedOptimized: " + speedOptimized.ToString() + ",\n" + 
                                    "                          SkipMWShapeID: + " + SkipMWShapeID.ToString() + ")");
			int numParts = polygon.NumParts;
			if(numParts == 0)
				numParts = 1;
			if(numParts > 1)
			{
				//multiple parts
				int shpIndex = 0;
				Globals.FixMultiPartPoly(ref polygon);
				MapWinGIS.Shape[] polyParts = new MapWinGIS.Shape[numParts];
				Globals.SeparateParts(ref polygon, out polyParts);
                
				MapWinGIS.Shapefile holeSF = new MapWinGIS.ShapefileClass();
				string tempFile = System.IO.Path.GetTempPath() + "tempHoleSF.shp";
				DataManagement.DeleteShapefile(ref tempFile);
                //CDM 8/4/2006 holeSF.CreateNew(tempFile, polygon.ShapeType);
                Globals.PrepareResultSF(ref tempFile, ref holeSF, polygon.ShapeType);

				holeSF.StartEditingShapes(true, null);

				MapWinGIS.Shapefile tempResult = new MapWinGIS.ShapefileClass();
				string tempResultFile = System.IO.Path.GetTempPath() + "tempResultSF.shp";
				DataManagement.DeleteShapefile(ref tempResultFile);
                //CDM 8/4/2006 tempResult.CreateNew(tempResultFile, polygon.ShapeType);
                Globals.PrepareResultSF(ref tempResultFile, ref tempResult, polygon.ShapeType);
				tempResult.StartEditingShapes(true, null);

				MapWinGIS.Shapefile modPolySF = new MapWinGIS.ShapefileClass();
				string modPolyFile = System.IO.Path.GetTempPath() + "tempModPolySF.shp";
				DataManagement.DeleteShapefile(ref modPolyFile);
                //CDM 8/4/2006 modPolySF.CreateNew(modPolyFile, polygon.ShapeType);
                Globals.PrepareResultSF(ref modPolyFile, ref modPolySF, polygon.ShapeType);
				modPolySF.StartEditingShapes(true, null);

				MapWinGIS.Shapefile resultSF = new MapWinGIS.ShapefileClass();
				string resultPath = System.IO.Path.GetTempPath() + "tempResultPath.shp";
				DataManagement.DeleteShapefile(ref resultPath);
				//CDM 8/4/2006 resultSF.CreateNew(resultPath, polygon.ShapeType);
                Globals.PrepareResultSF(ref resultPath, ref resultSF, polygon.ShapeType);
				resultSF.StartEditingShapes(true, null);

				for(int i = 0; i <= numParts-1; i++)
				{
					MapWinGIS.Shape currPart = new MapWinGIS.ShapeClass();
					currPart.Create(polygon.ShapeType);
					currPart = polyParts[i];
					if(Globals.IsClockwise(ref currPart))
					{
						if(speedOptimized)
							Fast_ClipPolygonWithLine(ref currPart, ref line, out tempResult);
						else
							Accurate_ClipPolygonWithLine(ref currPart, ref line, out tempResult);

						int numResults = tempResult.NumShapes;
						if(numResults > 0)
						{
							for(int j= 0; j <= numResults-1; j++)
							{
								shpIndex = modPolySF.NumShapes;
								modPolySF.EditInsertShape(tempResult.get_Shape(j), ref shpIndex);
							}
						}
					}
					else
					{
						//current part is a hole, add to holeSF
						shpIndex = holeSF.NumShapes;
						holeSF.EditInsertShape(currPart, ref shpIndex);
					}
				}

				if(holeSF.NumShapes > 0)
				{
					Erase.ErasePolySFWithPolySF(ref modPolySF, ref holeSF, ref resultSF);
				}

				if(resultSF.NumShapes > 0)
				{
					resultFile = resultSF;
					if (!SkipMWShapeID) Globals.DoInsertIDs(ref resultFile);
					return true;
				}
				else
				{
					gErrorMsg = "No shapes formed during clipping.";
					Debug.WriteLine(gErrorMsg);
					Error.SetErrorMsg(gErrorMsg);
					resultFile = resultSF;
                    MapWinUtility.Logger.Dbg(gErrorMsg);
					return false;
				}
			
			}
			else //single part polygon
			{
				if(speedOptimized)
					return Fast_ClipPolygonWithLine(ref polygon, ref line, out resultFile);
				else
					return Accurate_ClipPolygonWithLine(ref polygon, ref line, out resultFile);
			}			
		}
		#endregion
		#endregion
		#region ClipPolySFWithLineSF()
		public static bool ClipPolySFWithLineSF(ref MapWinGIS.Shapefile polySF, ref MapWinGIS.Shapefile lineSF, out MapWinGIS.Shapefile resultSF)
		{
            MapWinUtility.Logger.Dbg("ClipPolySFWithLineSF(polySF: " + Macro.ParamName(polySF) + ",\n" +
                                   "                       lineSF: " + Macro.ParamName(lineSF) + ",\n" +
                                   "                       resultSF: out)");
                                  
			int numPolygons = polySF.NumShapes;
			int numLines = lineSF.NumShapes;
			MapWinGIS.ShpfileType shpType = polySF.ShapefileType;
			MapWinGIS.Shapefile[] tempArray = new MapWinGIS.Shapefile[numPolygons];
			
			MapWinGIS.Shapefile resultFile = new MapWinGIS.ShapefileClass();
			string resultFilePath = System.IO.Path.GetTempPath() + "temporaryResults.shp";
			if(Globals.PrepareResultSF(ref resultFilePath, ref resultFile, shpType) == false)
			{
				resultSF = resultFile;
				return false;
			}
			
			MapWinGIS.ShpfileType polyType = polySF.ShapefileType;
			MapWinGIS.ShpfileType lineType = lineSF.ShapefileType;
			if(polyType != MapWinGIS.ShpfileType.SHP_POLYGON && polyType != MapWinGIS.ShpfileType.SHP_POLYGONM && polyType != MapWinGIS.ShpfileType.SHP_POLYGONZ)
			{
				gErrorMsg = "First shapefile is of wrong type: must be of type polygon";
				Debug.WriteLine(gErrorMsg);
				Error.SetErrorMsg(gErrorMsg);
				resultSF = resultFile;
                MapWinUtility.Logger.Dbg(gErrorMsg);
				return false;
			}
			if(lineType != MapWinGIS.ShpfileType.SHP_POLYLINE && lineType != MapWinGIS.ShpfileType.SHP_POLYLINEM && lineType != MapWinGIS.ShpfileType.SHP_POLYLINEZ)
			{
				gErrorMsg = "Second shapefile is of wrong type: must be of type line";
				Debug.WriteLine(gErrorMsg);
				Error.SetErrorMsg(gErrorMsg);
				resultSF = resultFile;
                MapWinUtility.Logger.Dbg(gErrorMsg);
				return false;
			}
			
			for(int i = 0; i<= numPolygons-1; i++)
			{
				int shpIndex = 0;
				MapWinGIS.Shape polygon = new MapWinGIS.ShapeClass();
				polygon.Create(polyType);
				polygon = polySF.get_Shape(i);
				
				tempArray[i] = new MapWinGIS.ShapefileClass();
				string tempSFPath = System.IO.Path.GetTempPath() + "temporarySF_" + i.ToString() + ".shp";
				if(Globals.PrepareResultSF(ref tempSFPath, ref tempArray[i], shpType) == false)
				{
					resultSF = resultFile;
					return false;
				}

				for(int j = 0; j <= numLines-1; j++)
				{
					MapWinGIS.Shape line = new MapWinGIS.ShapeClass();
					line.Create(lineType);
					line = lineSF.get_Shape(j);

					bool boundsIntersect = Globals.CheckBounds(ref polygon, ref line);
					if(boundsIntersect)
					{
						//line is valid, split current polygon
						if(tempArray[i].NumShapes == 0)//poly has not been split before
						{
							Accurate_ClipPolygonWithLine(ref polygon, ref line, out tempArray[i]);
						}
						else //poly has been split previously
						{
							int numTempPolys = tempArray[i].NumShapes;
							bool[] deletePoly = new bool[numTempPolys];
							for(int k = 0; k <= numTempPolys-1; k++)
							{
								deletePoly[k] = false;
								MapWinGIS.Shape testPoly = new MapWinGIS.ShapeClass();
								testPoly = tempArray[i].get_Shape(k);
								boundsIntersect = Globals.CheckBounds(ref testPoly, ref line);
								if(boundsIntersect)
								{
									MapWinGIS.Shapefile testSF = new MapWinGIS.ShapefileClass();
									string testSFPath = System.IO.Path.GetTempPath() + "testingSF_results.shp";
									if(Globals.PrepareResultSF(ref testSFPath, ref testSF, shpType) == false)
									{
										resultSF = resultFile;
										return false;
									}

									Accurate_ClipPolygonWithLine(ref testPoly, ref line, out testSF);
									int numTestShapes = testSF.NumShapes;
									if(numTestShapes > 0)
									{
										deletePoly[k] = true;
										//see if any of the results intersect previous shapes
										for(int newIndex = 0; newIndex <= numTestShapes-1; newIndex++)
										{
											MapWinGIS.Shape newPoly = new MapWinGIS.ShapeClass();
											newPoly = testSF.get_Shape(newIndex);
											bool intersects = false;
											for(int oldIndex = k-1; oldIndex >= 0; oldIndex--)
											{
												MapWinGIS.Shape oldPoly = new MapWinGIS.ShapeClass();
												oldPoly = tempArray[i].get_Shape(oldIndex);
												MapWinGIS.Shape intPoly = new MapWinGIS.ShapeClass();
												intPoly = SpatialOperations.Intersection(oldPoly, newPoly);
                                                // Paul Meems: Related to bug 1068. Added check for correct shape:
                                                if (intPoly != null)
                                                {
                                                    if (intPoly.numPoints != 0 && !intPoly.IsValid)
                                                    {
                                                        intersects = true;
                                                        deletePoly[oldIndex] = true;
                                                        shpIndex = tempArray[i].NumShapes;
                                                        tempArray[i].EditInsertShape(intPoly, ref shpIndex);
                                                        shpIndex++;
                                                        MapWinGIS.Shape diff1 = new MapWinGIS.ShapeClass();
                                                        MapWinGIS.Shape diff2 = new MapWinGIS.ShapeClass();
                                                        diff1 = SpatialOperations.SymmetricDifference(oldPoly, newPoly);                                                            
                                                        // Paul Meems: Related to bug 1068. Added check for correct shape:
                                                        if (diff1 != null)
                                                        {
                                                            if (diff1.numPoints != 0 && !diff1.IsValid)
                                                            {
                                                                tempArray[i].EditInsertShape(diff1, ref shpIndex);
                                                                shpIndex++;
                                                            }
                                                            else
                                                            {
                                                                gErrorMsg = "SpatialOperations.SymmetricDifference returned an invalid shape";
                                                                Debug.WriteLine(gErrorMsg);
                                                                Error.SetErrorMsg(gErrorMsg);
                                                                MapWinUtility.Logger.Dbg(gErrorMsg);
                                                            }
                                                        }
                                                        diff2 = SpatialOperations.SymmetricDifference(newPoly, oldPoly);
                                                        // Paul Meems: Related to bug 1068. Added check for correct shape:
                                                        if (diff2 != null)
                                                        {
                                                            if (diff2.numPoints != 0 && !diff2.IsValid)
                                                            {

                                                                tempArray[i].EditInsertShape(diff2, ref shpIndex);
                                                                shpIndex++;
                                                            }
                                                            else
                                                            {
                                                                gErrorMsg = "SpatialOperations.SymmetricDifference returned an invalid shape";
                                                                Debug.WriteLine(gErrorMsg);
                                                                Error.SetErrorMsg(gErrorMsg);
                                                                MapWinUtility.Logger.Dbg(gErrorMsg);
                                                            }
                                                        }
                                                    }//end of intersect exists
                                                    else
                                                    {
                                                        gErrorMsg = "SpatialOperations.Intersection returned an invalid shape";
                                                        Debug.WriteLine(gErrorMsg);
                                                        Error.SetErrorMsg(gErrorMsg);
                                                        MapWinUtility.Logger.Dbg(gErrorMsg);
                                                    }
                                                }
											}//end of comparing against original shapes
											if (intersects == false)
											{
												shpIndex = tempArray[i].NumShapes;
												tempArray[i].EditInsertShape(newPoly, ref shpIndex);                                                
											}	
										}//end of looping through new shapes and adding to tempArray
									}//end of found new splits
								}//end of checking bounds against test polygons
								//need to remove the repetitive shapes
								for(int index = numTempPolys-1; index >= 0; index--)
								{
									if(deletePoly[index] == true)
									{
										tempArray[i].EditDeleteShape(index);
									}
								}
							}//end of looping through test polygons
						}//end of dealing with previous splits
					}//end of checking bounds against original polygon
				}//end of looping through lines
				int numResults = tempArray[i].NumShapes;
				shpIndex = resultFile.NumShapes;
				for(int j = 0; j<= numResults-1; j++)
				{
					resultFile.EditInsertShape(tempArray[i].get_Shape(j), ref shpIndex);
					shpIndex++;					
				}
			}//end of looping through original polygons
			resultSF = resultFile;
			return true;
		}
		#endregion

		#region Fast_ClipPolygonWithLine()
		
		#region in-memory version
		//Angela Hillier 10/05
		/// <summary>
		/// For faster clipping of polygons with lines. Limits the finding of intersections to
		/// outside->inside or inside->outside 2pt segments. Assumes only one intersections exists
		/// per segment, that a segment of two inside points or two outside points will not intersect
		/// the polygon.
		/// </summary>
		/// <param name="polygon">The polygon that will be sectioned by the line.</param>
		/// <param name="line">The line that will clip the polygon into multiple parts.</param>
		/// <param name="resultSF">The in-memory shapefile where the polygon sections will be saved.</param>
		/// <returns>False if errors are encountered, true otherwise.</returns>
		public static bool Fast_ClipPolygonWithLine(ref MapWinGIS.Shape polygon, ref MapWinGIS.Shape line, out MapWinGIS.Shapefile resultSF)
		{
            MapWinUtility.Logger.Dbg("ClipMultiPartPolyWithLine(polygon: " + Macro.ParamName(polygon) + ",\n" +
                                   "                          line: " + Macro.ParamName(line) + ",\n" +
                                   "                          resultFile: out)");


			MapWinGeoProc.Error.ClearErrorLog();
			string resultFilePath = System.IO.Path.GetTempPath() + "tempResultSF.shp";
			MapWinGIS.Shapefile resultFile = new MapWinGIS.ShapefileClass();
			
			if(polygon != null && line != null)
			{
				MapWinGIS.ShpfileType sfType = new MapWinGIS.ShpfileType();
				sfType = polygon.ShapeType;
				//make sure we are dealing with a valid shapefile type
				if(sfType == MapWinGIS.ShpfileType.SHP_POLYGON || sfType == MapWinGIS.ShpfileType.SHP_POLYGONM || sfType == MapWinGIS.ShpfileType.SHP_POLYGONZ)
				{
					//create the result shapefile if it does not already exist
					if(Globals.PrepareResultSF(ref resultFilePath, ref resultFile, sfType)== false)
					{
						resultSF = resultFile;
						return false;
					}
					
					bool boundsIntersect = Globals.CheckBounds(ref line, ref polygon);

					if(boundsIntersect == false)
					{
						gErrorMsg = "Line does not cross polygon boundary.";
						Debug.WriteLine(gErrorMsg);
						MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
						resultSF = resultFile;
						return false;
					}
					else
					{
						//find if all of the line is inside, outside, or part in and out of polygon
						//line might intersect polygon mutliple times
						int numPoints = line.numPoints;
						bool[] ptsInside = new bool[numPoints];
						//MapWinGIS.Utils utils = new MapWinGIS.UtilsClass();
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
						for(int i = 0; i <= numPoints-1; i++)
						{
							currPt = line.get_Point(i);
		
							//if(utils.PointInPolygon(polygon, currPt) == true)
							if(Utils.PointInPoly(ref polyVertArray, ref currPt) == true)
							{
								ptsInside[i] = true;
								numInside += 1;
							}
							else
							{
								ptsInside[i] = false;
								numOutside += 1;
							}
						}

						//case: all points are inside polygon - check for possible intersections
						if(numInside == numPoints)
						{
							//assume no intersections exist in fast version
						}
							//case: all points are outside of the polygon - check for possible intersections
						else if(numOutside == numPoints)
						{	
							//assume no intersections exist in fast version
						}
							//case: part of line is inside and part is outside - find inside segments.
						else
						{
							if(Fast_ProcessPartInAndOut(ref ptsInside, ref line, ref polygon, ref resultFile)==false)
							{
								gErrorMsg = "Problem processing part in and out line: " + MapWinGeoProc.Error.GetLastErrorMsg();
								Debug.WriteLine(gErrorMsg);
								MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
								resultSF = resultFile;
								return false;
							}
						}
					}
					
					//output result file, do not save to disk.
					resultSF = resultFile;
				}
				else
				{
					gErrorMsg = "Invalid shapefile type, should be of type polygon.";
					Debug.WriteLine(gErrorMsg);
					MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
					resultSF = resultFile;
					return false;
				}
			}
			else //polygon or line is invalid
			{
				gErrorMsg = "Invalid object, cannot pass a NULL parameter.";
				Debug.WriteLine(gErrorMsg);
				MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
				resultSF = resultFile;
				return false;
			}
			return true;
		}
		#endregion
		
		#region save-to-disk version
		//Angela Hillier 10/05
		/// <summary>
		/// For faster clipping of polygons with lines. Limits the finding of intersections to
		/// outside->inside or inside->outside 2pt segments. Assumes only one intersections exists
		/// per segment, that a segment of two inside points or two outside points will not intersect
		/// the polygon.
		/// </summary>
		/// <param name="polygon">The polygon that will be sectioned by the line.</param>
		/// <param name="line">The line that will clip the polygon into multiple parts.</param>
		/// <param name="resultSFPath">The path to the file where the polygon sections will be saved.</param>
		/// <returns>False if errors are encountered, true otherwise.</returns>
		public static bool Fast_ClipPolygonWithLine(ref MapWinGIS.Shape polygon, ref MapWinGIS.Shape line, ref string resultSFPath)
		{
			MapWinGeoProc.Error.ClearErrorLog();
			if(polygon != null && line != null && resultSFPath != null)
			{
				MapWinGIS.Shapefile resultSF = new MapWinGIS.ShapefileClass();
				MapWinGIS.ShpfileType sfType = new MapWinGIS.ShpfileType();
				sfType = polygon.ShapeType;
				//make sure we are dealing with a valid shapefile type
				if(sfType == MapWinGIS.ShpfileType.SHP_POLYGON || sfType == MapWinGIS.ShpfileType.SHP_POLYGONM || sfType == MapWinGIS.ShpfileType.SHP_POLYGONZ)
				{
					//create the result shapefile if it does not already exist
					if(Globals.PrepareResultSF(ref resultSFPath, ref resultSF, sfType)== false)
					{
						return false;
					}
					
					bool boundsIntersect = Globals.CheckBounds(ref line, ref polygon);

					if(boundsIntersect == false)
					{
						gErrorMsg = "Line does not cross polygon boundary.";
						Debug.WriteLine(gErrorMsg);
						MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
						return false;
					}
					else
					{
						//find if all of the line is inside, outside, or part in and out of polygon
						//line might intersect polygon mutliple times
						int numPoints = line.numPoints;
						bool[] ptsInside = new bool[numPoints];
						MapWinGIS.Point currPt = new MapWinGIS.PointClass();
						MapWinGIS.Point nextPt = new MapWinGIS.PointClass();
						//MapWinGIS.Utils utils = new MapWinGIS.UtilsClass();
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
						for(int i = 0; i <= numPoints-1; i++)
						{
							currPt = line.get_Point(i);
		
							//if(utils.PointInPolygon(polygon, currPt) == true)
							if(Utils.PointInPoly(ref polyVertArray, ref currPt) == true)
							{
								ptsInside[i] = true;
								numInside += 1;
							}
							else
							{
								ptsInside[i] = false;
								numOutside += 1;
							}
						}

						//case: all points are inside polygon - check for possible intersections
						if(numInside == numPoints)
						{
							//assume no intersections exist in fast version
						}
							//case: all points are outside of the polygon - check for possible intersections
						else if(numOutside == numPoints)
						{	
							//assume no intersections exist in fast version
						}
							//case: part of line is inside and part is outside - find inside segments.
						else
						{
							if(Fast_ProcessPartInAndOut(ref ptsInside, ref line, ref polygon, ref resultSF)==false)
							{
								gErrorMsg = "Problem processing part in and out line: " + MapWinGeoProc.Error.GetLastErrorMsg();
								Debug.WriteLine(gErrorMsg);
								MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
								return false;
							}
						}
					}
					
					//add ID field to the .dbf table
					MapWinGIS.Field ID = new MapWinGIS.FieldClass();
                    ID.Name = "MWShapeID";
					ID.Type = MapWinGIS.FieldType.INTEGER_FIELD;
					int fieldIndex = 0;
					if(resultSF.EditInsertField(ID, ref fieldIndex, null)==false)
					{
						gErrorMsg = "Problem inserting field into .dbf table: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
						Debug.WriteLine(gErrorMsg);
						Error.SetErrorMsg(gErrorMsg);
						return false;
					}
					//add id values to the dbf table
					int numIDs = resultSF.NumShapes;
					for(int i = 0; i<= numIDs-1; i++)
					{
						if(resultSF.EditCellValue(0, i, i) == false)
						{
							gErrorMsg = "Problem inserting value into .dbf table for shape " + i + ": " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
							Debug.WriteLine(gErrorMsg);
							Error.SetErrorMsg(gErrorMsg);
							return false;
						}
					}
					//save the result file
					if(resultSF.StopEditingShapes(true, true, null)==false)
					{
						gErrorMsg = "Problem with StopEditingShapes: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
						Debug.WriteLine(gErrorMsg);
						MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
						return false;
					}
				}
				else
				{
					gErrorMsg = "Invalid shapefile type, should be of type polygon.";
					Debug.WriteLine(gErrorMsg);
					MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
					return false;
				}
			}
			else //polygon, line, or resultSFPath is invalid
			{
				gErrorMsg = "Invalid object, cannot pass a NULL parameter.";
				Debug.WriteLine(gErrorMsg);
				MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
				return false;
			}
			return true;
		}
		#endregion

		#endregion

		#region private Fast_ProcessPartInAndOut() -- used by Fast_ClipPolygonWithLine()
		/// <summary>
		/// Given a line that contains portion both inside and outside of the polygon, this
		/// function will split the polygon based only on the segments that completely bisect
		/// the polygon. It assumes: out->out, and in->in 2pt segments do not intersect the
		/// polygon, and out->in, in->out 2pt segments have only one point of intersection.
		/// </summary>
		/// <param name="insidePts">A boolean array indicating if a point is inside the polygon or not.</param>
		/// <param name="line">The line that intersects the polygon.</param>
		/// <param name="polygon">The polygon that will be split by the intersecting line.</param>
		/// <param name="resultSF">The shapefile that the polygon sections will be saved to.</param>
		/// <returns>False if errors were encountered or an assumption violated, true otherwise.</returns>
		private static bool Fast_ProcessPartInAndOut(ref bool[] insidePts, ref MapWinGIS.Shape line, ref MapWinGIS.Shape polygon, ref MapWinGIS.Shapefile resultSF)
		{
			int ptIndex = 0;
			int numLinePts = line.numPoints;
			int numLineSegs = numLinePts-1;
			int numPolyPts = polygon.numPoints;
			int[] intersectsPerSeg = new int[numLineSegs];
			MapWinGIS.Point[][] intersectPts = new MapWinGIS.Point[numLineSegs][];
			int[][] polyIntLocs = new int[numLineSegs][]; //intersection occurs between polygon point indexed by polyIntLoc[][] and the previous point.

			//cut line into 2pt segments and put in new shapefile.
			string tempPath = System.IO.Path.GetTempPath() + "tempLineSF.shp";
			DataManagement.DeleteShapefile(ref tempPath);
			MapWinGIS.Shapefile lineSegSF = new MapWinGIS.ShapefileClass();
			//CDM 8/4/2006 lineSegSF.CreateNew(tempPath, line.ShapeType);
            Globals.PrepareResultSF(ref tempPath, ref lineSegSF, line.ShapeType);
			int shpIndex = 0;
			MapWinGIS.Shape lineSegment;
			for(int i = 0; i <= numLineSegs-1; i++)
			{
				lineSegment = new MapWinGIS.ShapeClass();
				lineSegment.ShapeType = line.ShapeType;
				ptIndex = 0;
				lineSegment.InsertPoint(line.get_Point(i), ref ptIndex);
				ptIndex = 1;
				lineSegment.InsertPoint(line.get_Point(i+1), ref ptIndex);
				shpIndex = lineSegSF.NumShapes;
				lineSegSF.EditInsertShape(lineSegment, ref shpIndex);			

				intersectPts[i] = new MapWinGIS.Point[numPolyPts];
				polyIntLocs[i] = new int[numPolyPts];
			}
			
			//find number of intersections, intersection pts, and locations for each 2pt segment
			int numIntersects = Globals.CalcSiDeterm(ref lineSegSF, ref polygon, out intersectsPerSeg, out intersectPts, out polyIntLocs);

			if(numIntersects == 0)
			{
				return false;
			}

			MapWinGIS.Shape insideLine = new MapWinGIS.ShapeClass();
			insideLine.ShapeType = line.ShapeType;
			MapWinGIS.Shape intersectSeg = new MapWinGIS.ShapeClass();
			MapWinGIS.Point startIntersect = new MapWinGIS.PointClass();
			bool startIntExists = false;
			bool validInsideLine = false;
			int insideStart = 0;
			int startIntPolyLoc = 0;

			//loop through each 2pt segment
			for(int i = 0; i <= numLinePts-2; i++)
			{
				lineSegment = new MapWinGIS.ShapeClass();
				lineSegment = lineSegSF.get_Shape(i);
				int numSegIntersects = intersectsPerSeg[i];

				//****************** case: inside->inside **************************************//
				if(insidePts[i] == true && insidePts[i+1] == true)
				{					
					if(numSegIntersects == 0 && i != numLinePts-2 && i != 0)
					{
						//add points to an inside line segment
						if(startIntExists == true)
						{
							ptIndex = 0;
							insideLine.InsertPoint(startIntersect, ref ptIndex);
							startIntExists = false;
							validInsideLine = true;
							insideStart = startIntPolyLoc;
						}
						if(validInsideLine == true)
						{
							ptIndex = insideLine.numPoints;
							insideLine.InsertPoint(lineSegment.get_Point(0), ref ptIndex);
						}
					}
					else
					{
						//we do not handle multiple intersections in the fast version
						gErrorMsg = "Multiple intersections exist, use accurate version.";
						Debug.WriteLine(gErrorMsg);
						Error.SetErrorMsg(gErrorMsg);
						return false;
					}
				}//end of inside->inside
					
					//********************** case: inside->outside ****************************************
				else if(insidePts[i] == true && insidePts[i+1] == false)
				{
					if(numSegIntersects == 0)
					{
						gErrorMsg = "No intersects found but at least one was expected.";
						Error.SetErrorMsg(gErrorMsg);
						Debug.WriteLine(gErrorMsg);
						return false;
					}
					else if(numSegIntersects == 1)
					{
						if(startIntExists == true)
						{
							intersectSeg = new MapWinGIS.ShapeClass();
							intersectSeg.ShapeType = line.ShapeType;
							ptIndex = 0;
							intersectSeg.InsertPoint(startIntersect, ref ptIndex);
							ptIndex = 1;
							intersectSeg.InsertPoint(lineSegment.get_Point(0), ref ptIndex);
							ptIndex = 2;
							intersectSeg.InsertPoint(intersectPts[i][0], ref ptIndex);
							
							int firstPolyLoc = startIntPolyLoc;
							int lastPolyLoc = polyIntLocs[i][0]-1;
							if(SectionPolygonWithLine(ref intersectSeg, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
							{
								Debug.WriteLine(Error.GetLastErrorMsg());
								return false;
							}
								
							startIntExists = false; //we just used it up!
						}
						else if(insideLine.numPoints != 0 && validInsideLine == true)
						{
							ptIndex = insideLine.numPoints;
							insideLine.InsertPoint(lineSegment.get_Point(0), ref ptIndex);
							ptIndex++;
							insideLine.InsertPoint(intersectPts[i][0], ref ptIndex);

							int firstPolyLoc = insideStart;
							int lastPolyLoc = polyIntLocs[i][0]-1;
							if(SectionPolygonWithLine(ref insideLine, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
							{
								Debug.WriteLine(Error.GetLastErrorMsg());
								return false;
							}

							validInsideLine = false;
							insideLine = new MapWinGIS.ShapeClass();
							insideLine.ShapeType = line.ShapeType;
						}
					}
					else
					{ 
						//we do not handle multiple intersections in the fast version
						gErrorMsg = "Multiple intersections exist, use accurate version.";
						Debug.WriteLine(gErrorMsg);
						Error.SetErrorMsg(gErrorMsg);
						return false;					
					}
				}//end of inside->outside

					//********************** case: outside->inside ***************************************
				else if(insidePts[i] == false && insidePts[i+1] == true)
				{
					validInsideLine = false;
					startIntExists = false;

					if(numSegIntersects == 0)
					{
						gErrorMsg = "No intersects found but one was expected.";
						Error.SetErrorMsg(gErrorMsg);
						Debug.WriteLine(gErrorMsg);
						return false;
					}
					else if(numSegIntersects == 1)
					{
						startIntExists = true;
						startIntersect = new MapWinGIS.PointClass();
						startIntersect = intersectPts[i][0];
						startIntPolyLoc = polyIntLocs[i][0]-1;
					}
					else
					{
						//we do not handle multiple intersections in the fast version
						gErrorMsg = "Multiple intersections exist, use accurate version.";
						Debug.WriteLine(gErrorMsg);
						Error.SetErrorMsg(gErrorMsg);
						return false;
					}
				}//end of outside->inside

					//************************ case: outside->outside ***********************************
				else if(insidePts[i] == false && insidePts[i+1] == false)
				{
					startIntExists = false;
					validInsideLine = false;

					if(numSegIntersects == 0)
					{
						//do nothing
					}
					else
					{
						//we do not handle multiple intersections in the fast version
						gErrorMsg = "Multiple intersections exist, use accurate version.";
						Debug.WriteLine(gErrorMsg);
						Error.SetErrorMsg(gErrorMsg);
						return false;	
					}
				}//end of outside->outside				
			}//end of looping through 2pt segments
			return true;
		}
		#endregion

		#region Accurate_ClipPolygonWithLine()

		#region in-memory version
		public static bool Accurate_ClipPolygonWithLine(ref MapWinGIS.Shape polygon, ref MapWinGIS.Shape line, out MapWinGIS.Shapefile resultSF)
		{
			MapWinGeoProc.Error.ClearErrorLog();
			MapWinGIS.Shapefile resultFile = new MapWinGIS.ShapefileClass();
			string resultFilePath = System.IO.Path.GetTempPath() + "tempResultSF.shp";			
			
			if(polygon != null && line != null)
			{
				MapWinGIS.ShpfileType sfType = new MapWinGIS.ShpfileType();
				sfType = polygon.ShapeType;
				//make sure we are dealing with a valid shapefile type
				if(sfType == MapWinGIS.ShpfileType.SHP_POLYGON || sfType == MapWinGIS.ShpfileType.SHP_POLYGONM || sfType == MapWinGIS.ShpfileType.SHP_POLYGONZ)
				{
					//create the result shapefile if it does not already exist
					if(Globals.PrepareResultSF(ref resultFilePath, ref resultFile, sfType) == false)
					{
						resultSF = resultFile;
						return false;
					}
					
					bool boundsIntersect = Globals.CheckBounds(ref line, ref polygon);

					if(boundsIntersect == false)
					{
						gErrorMsg = "Line does not cross polygon boundary.";
						Debug.WriteLine(gErrorMsg);
						MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
						resultSF = resultFile;
						return false;
					}
					else
					{
						//find if all of the line is inside, outside, or part in and out of polygon
						//line might intersect polygon mutliple times
						int numPoints = line.numPoints;
						bool[] ptsInside = new bool[numPoints];
						//MapWinGIS.Utils utils = new MapWinGIS.UtilsClass();
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
						for(int i = 0; i <= numPoints-1; i++)
						{
							currPt = line.get_Point(i);
		
							//if(utils.PointInPolygon(polygon, currPt) == true)
							if(Utils.PointInPoly(ref polyVertArray, ref currPt) == true)
							{
								ptsInside[i] = true;
								numInside += 1;
							}
							else
							{
								ptsInside[i] = false;
								numOutside += 1;
							}
						}

						//case: all points are inside polygon - check for possible intersections
						if(numInside == numPoints)
						{
							if(ProcessAllInside(ref line, ref polygon, ref resultFile)== false)
							{	
								gErrorMsg = "Problem processing inside line points: " + MapWinGeoProc.Error.GetLastErrorMsg();
								Debug.WriteLine(gErrorMsg);
								MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
								resultSF = resultFile;
								return false;
							}
						}
							//case: all points are outside of the polygon - check for possible intersections
						else if(numOutside == numPoints)
						{	
							if(ProcessAllOutside(ref line, ref polygon, ref resultFile) == false)
							{
								gErrorMsg = "Problem processing outside line points: " + MapWinGeoProc.Error.GetLastErrorMsg();
								Debug.WriteLine(gErrorMsg);
								MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
								resultSF = resultFile;
								return false;
							}
						}
							//case: part of line is inside and part is outside - find inside segments.
						else
						{
							if(ProcessPartInAndOut(ref ptsInside, ref line, ref polygon, ref resultFile)==false)
							{
								gErrorMsg = "Problem processing part in and out line: " + MapWinGeoProc.Error.GetLastErrorMsg();
								Debug.WriteLine(gErrorMsg);
								MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
								resultSF = resultFile;
								return false;
							}
						}	


					}
					
					//output result file, do not save to disk.
					resultSF = resultFile;
				}
				else
				{
					gErrorMsg = "Invalid shapefile type, should be of type polygon.";
					Debug.WriteLine(gErrorMsg);
					MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
					resultSF = resultFile;
					return false;
				}
			}
			else //polygon or line is invalid
			{
				gErrorMsg = "Invalid object, cannot pass a NULL parameter.";
				Debug.WriteLine(gErrorMsg);
				MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
				resultSF = resultFile;
				return false;
			}
			return true;
		}
		#endregion

		#region save-to-disk version
		public static bool Accurate_ClipPolygonWithLine(ref MapWinGIS.Shape polygon, ref MapWinGIS.Shape line, ref string resultSFPath)
		{
			MapWinGeoProc.Error.ClearErrorLog();
			if(polygon != null && line != null && resultSFPath != null)
			{
				MapWinGIS.Shapefile resultSF = new MapWinGIS.ShapefileClass();
				MapWinGIS.ShpfileType sfType = new MapWinGIS.ShpfileType();
				sfType = polygon.ShapeType;
				//make sure we are dealing with a valid shapefile type
				if(sfType == MapWinGIS.ShpfileType.SHP_POLYGON || sfType == MapWinGIS.ShpfileType.SHP_POLYGONM || sfType == MapWinGIS.ShpfileType.SHP_POLYGONZ)
				{
					//create the result shapefile
					if(Globals.PrepareResultSF(ref resultSFPath, ref resultSF,  sfType) == false)
					{
						return false;
					}
					
					bool boundsIntersect = Globals.CheckBounds(ref line, ref polygon);

					if(boundsIntersect == false)
					{
						gErrorMsg = "Line does not cross polygon boundary.";
						Debug.WriteLine(gErrorMsg);
						MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
						return false;
					}
					else
					{
						//find if all of the line is inside, outside, or part in and out of polygon
						//line might intersect polygon mutliple times
						int numPoints = line.numPoints;
						bool[] ptsInside = new bool[numPoints];
						MapWinGIS.Point currPt = new MapWinGIS.PointClass();
						MapWinGIS.Point nextPt = new MapWinGIS.PointClass();
						//MapWinGIS.Utils utils = new MapWinGIS.UtilsClass();
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
						for(int i = 0; i <= numPoints-1; i++)
						{
							currPt = line.get_Point(i);
		
							//if(utils.PointInPolygon(polygon, currPt) == true)
							if(Utils.PointInPoly(ref polyVertArray, ref currPt) == true)
							{
								ptsInside[i] = true;
								numInside += 1;
							}
							else
							{
								ptsInside[i] = false;
								numOutside += 1;
							}
						}

						//case: all points are inside polygon - check for possible intersections
						if(numInside == numPoints)
						{
							if(ProcessAllInside(ref line, ref polygon, ref resultSF)== false)
							{	
								gErrorMsg = "Problem processing inside line points: " + MapWinGeoProc.Error.GetLastErrorMsg();
								Debug.WriteLine(gErrorMsg);
								MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
								return false;
							}
						}
							//case: all points are outside of the polygon - check for possible intersections
						else if(numOutside == numPoints)
						{	
							if(ProcessAllOutside(ref line, ref polygon, ref resultSF) == false)
							{
								gErrorMsg = "Problem processing outside line points: " + MapWinGeoProc.Error.GetLastErrorMsg();
								Debug.WriteLine(gErrorMsg);
								MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
								return false;
							}
						}
							//case: part of line is inside and part is outside - find inside segments.
						else
						{
							if(ProcessPartInAndOut(ref ptsInside, ref line, ref polygon, ref resultSF)==false)
							{
								gErrorMsg = "Problem processing part in and out line: " + MapWinGeoProc.Error.GetLastErrorMsg();
								Debug.WriteLine(gErrorMsg);
								MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
								return false;
							}
						}	


					}
					//add ID field to the .dbf table
					MapWinGIS.Field ID = new MapWinGIS.FieldClass();
                    ID.Name = "MWShapeID";
					ID.Type = MapWinGIS.FieldType.INTEGER_FIELD;
					int fieldIndex = 0;
					if(resultSF.EditInsertField(ID, ref fieldIndex, null)==false)
					{
						gErrorMsg = "Problem inserting field into .dbf table: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
						Debug.WriteLine(gErrorMsg);
						Error.SetErrorMsg(gErrorMsg);
						return false;
					}
					//add id values to the dbf table
					int numIDs = resultSF.NumShapes;
					for(int i = 0; i<= numIDs-1; i++)
					{
						if(resultSF.EditCellValue(0, i, i) == false)
						{
							gErrorMsg = "Problem inserting value into .dbf table for shape " + i + ": " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
							Debug.WriteLine(gErrorMsg);
							Error.SetErrorMsg(gErrorMsg);
							return false;
						}
					}
					//save the result file
					if(resultSF.StopEditingShapes(true, true, null)==false)
					{
						gErrorMsg = "Problem with StopEditingShapes: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
						Debug.WriteLine(gErrorMsg);
						MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
						return false;
					}
				}
				else
				{
					gErrorMsg = "Invalid shapefile type, should be of type polygon.";
					Debug.WriteLine(gErrorMsg);
					MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
					return false;
				}
			}
			else //polygon, line, or resultSFPath is invalid
			{
				gErrorMsg = "Invalid object, cannot pass a NULL parameter.";
				Debug.WriteLine(gErrorMsg);
				MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
				return false;
			}
			return true;
		}
		#endregion

		#endregion
		
		#region private ProcessPartInAndOut() -- used by Accurate_ClipPolygonWithLine()
		//Angela Hillier 10/05
		/// <summary>
		/// Given a line that contains portions both inside and outside of the polygon, this
		/// function will split the polygon based only on the segments that completely bisect
		/// the polygon. The possibility of mutliple intersections for any 2pt segment is taken
		/// into account.
		/// </summary>
		/// <param name="insidePts">A boolean array indicating if a point is inside the polygon or not.</param>
		/// <param name="line">The line that intersects the polygon.</param>
		/// <param name="polygon">The polygon that will be split by the intersecting line.</param>
		/// <param name="resultSF">The shapefile that the polygon sections will be saved to.</param>
		/// <returns>False if errors were encountered or an assumption violated, true otherwise.</returns>
		private static bool ProcessPartInAndOut(ref bool[] insidePts, ref MapWinGIS.Shape line, ref MapWinGIS.Shape polygon, ref MapWinGIS.Shapefile resultSF)
		{
			int ptIndex = 0;
			int numLinePts = line.numPoints;
			int numLineSegs = numLinePts-1;
			int numPolyPts = polygon.numPoints;
			int[] intersectsPerSeg = new int[numLineSegs];
			MapWinGIS.Point[][] intersectPts = new MapWinGIS.Point[numLineSegs][];
			int[][] polyIntLocs = new int[numLineSegs][]; //intersection occurs between polygon point indexed by polyIntLoc[][] and the previous point.

			//cut line into 2pt segments and put in new shapefile.
			string tempPath = System.IO.Path.GetTempPath() + "tempLineSF.shp";
			DataManagement.DeleteShapefile(ref tempPath);
			MapWinGIS.Shapefile lineSegSF = new MapWinGIS.ShapefileClass();
			//CDM 8/4/2006 lineSegSF.CreateNew(tempPath, line.ShapeType);
            Globals.PrepareResultSF(ref tempPath, ref lineSegSF, line.ShapeType);

			int shpIndex = 0;
			MapWinGIS.Shape lineSegment;
			for(int i = 0; i <= numLineSegs-1; i++)
			{
				lineSegment = new MapWinGIS.ShapeClass();
				lineSegment.ShapeType = line.ShapeType;
				ptIndex = 0;
				lineSegment.InsertPoint(line.get_Point(i), ref ptIndex);
				ptIndex = 1;
				lineSegment.InsertPoint(line.get_Point(i+1), ref ptIndex);
				shpIndex = lineSegSF.NumShapes;
				lineSegSF.EditInsertShape(lineSegment, ref shpIndex);			

				intersectPts[i] = new MapWinGIS.Point[numPolyPts];
				polyIntLocs[i] = new int[numPolyPts];
			}
			
			//find number of intersections, intersection pts, and locations for each 2pt segment
			int numIntersects = Globals.CalcSiDeterm(ref lineSegSF, ref polygon, out intersectsPerSeg, out intersectPts, out polyIntLocs);

			if(numIntersects == 0)
			{
				return false;
			}

			MapWinGIS.Shape insideLine = new MapWinGIS.ShapeClass();
			insideLine.ShapeType = line.ShapeType;
			MapWinGIS.Shape intersectSeg = new MapWinGIS.ShapeClass();
			MapWinGIS.Point startIntersect = new MapWinGIS.PointClass();
			bool startIntExists = false;
			bool validInsideLine = false;
			int insideStart = 0;
			int startIntPolyLoc = 0;

			//loop through each 2pt segment
			for(int i = 0; i <= numLinePts-2; i++)
			{
				lineSegment = new MapWinGIS.ShapeClass();
				lineSegment = lineSegSF.get_Shape(i);
				int numSegIntersects = intersectsPerSeg[i];

				//****************** case: inside->inside **************************************//
				if(insidePts[i] == true && insidePts[i+1] == true)
				{					
					if(numSegIntersects == 0 && i != numLinePts-2 && i != 0)
					{
						//add points to an inside line segment
						if(startIntExists == true)
						{
							ptIndex = 0;
							insideLine.InsertPoint(startIntersect, ref ptIndex);
							startIntExists = false;
							validInsideLine = true;
							insideStart = startIntPolyLoc;
						}
						if(validInsideLine == true)
						{
							ptIndex = insideLine.numPoints;
							insideLine.InsertPoint(lineSegment.get_Point(0), ref ptIndex);
						}
					}
					else
					{
						//sort the intersects and their locations
						MapWinGIS.Point[] intPts = new MapWinGIS.Point[numSegIntersects];
						MapWinGIS.Point startPt = new MapWinGIS.PointClass();
						startPt = lineSegSF.get_Shape(i).get_Point(0);
						Globals.FindAndSortValidIntersects(numSegIntersects, ref intersectPts[i], ref intPts, ref startPt, ref polyIntLocs[i]);

						for(int j= 0; j <= numSegIntersects-1; j++)
						{
							if(j == 0)
							{
								if(startIntExists == true)
								{
									//first intersect pt is an ending pt, it must be
									//combined with a starting intersect pt in order to section the polygon.
									intersectSeg = new MapWinGIS.ShapeClass();
									intersectSeg.ShapeType = line.ShapeType;
									ptIndex = 0;
									intersectSeg.InsertPoint(startIntersect, ref ptIndex);
									ptIndex = 1;
									intersectSeg.InsertPoint(lineSegment.get_Point(0), ref ptIndex);
									ptIndex = 2;
									intersectSeg.InsertPoint(intPts[0], ref ptIndex);

									int firstPolyLoc = startIntPolyLoc;
									int lastPolyLoc = polyIntLocs[i][0]-1;
									if(SectionPolygonWithLine(ref intersectSeg, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
									{
										Debug.WriteLine(Error.GetLastErrorMsg());
										return false;
									}
								
									startIntExists = false; //we used it up!
								}
								else if(insideLine.numPoints != 0 && validInsideLine == true)
								{
									ptIndex = insideLine.numPoints;
									insideLine.InsertPoint(lineSegment.get_Point(0), ref ptIndex);
									ptIndex++;
									insideLine.InsertPoint(intPts[0], ref ptIndex);

									int firstPolyLoc = insideStart;
									int lastPolyLoc = polyIntLocs[i][0]-1;
									if(SectionPolygonWithLine(ref insideLine, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
									{
										Debug.WriteLine(Error.GetLastErrorMsg());
										return false;
									}

									validInsideLine = false;
									insideLine = new MapWinGIS.ShapeClass();
									insideLine.ShapeType = line.ShapeType;
								}
							}
							else if(j == numSegIntersects-1 && i != numLinePts-2)
							{
								//last intersect pt is a starting pt, it must be
								//saved for later combination
								startIntersect = new MapWinGIS.PointClass();
								startIntersect = intPts[j];
								startIntPolyLoc = polyIntLocs[i][j]-1;
								startIntExists = true;								
							}
							else if(j!= 0 || j != numSegIntersects-1)
							{
								//a full poly section is created by two intersect points
								intersectSeg = new MapWinGIS.ShapeClass();
								intersectSeg.ShapeType = line.ShapeType;
								ptIndex = 0;
								intersectSeg.InsertPoint(intPts[j], ref ptIndex);
								ptIndex = 1;
								intersectSeg.InsertPoint(intPts[j+1], ref ptIndex);

								int firstPolyLoc = polyIntLocs[i][j]-1;
								int lastPolyLoc = polyIntLocs[i][j+1]-1;
								if(SectionPolygonWithLine(ref intersectSeg, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
								{
									Debug.WriteLine(Error.GetLastErrorMsg());
									return false;
								}
								j++;
							}
						}
					}

				}
					//********************** case: inside->outside ****************************************
				else if(insidePts[i] == true && insidePts[i+1] == false)
				{
					if(numSegIntersects == 0)
					{
						gErrorMsg = "No intersects found but at least one was expected.";
						Error.SetErrorMsg(gErrorMsg);
						Debug.WriteLine(gErrorMsg);
						return false;
					}
					else if(numSegIntersects == 1)
					{
						if(startIntExists == true)
						{
							intersectSeg = new MapWinGIS.ShapeClass();
							intersectSeg.ShapeType = line.ShapeType;
							ptIndex = 0;
							intersectSeg.InsertPoint(startIntersect, ref ptIndex);
							ptIndex = 1;
							intersectSeg.InsertPoint(lineSegment.get_Point(0), ref ptIndex);
							ptIndex = 2;
							intersectSeg.InsertPoint(intersectPts[i][0], ref ptIndex);
							
							int firstPolyLoc = startIntPolyLoc;
							int lastPolyLoc = polyIntLocs[i][0]-1;
							if(SectionPolygonWithLine(ref intersectSeg, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
							{
								Debug.WriteLine(Error.GetLastErrorMsg());
								return false;
							}
								
							startIntExists = false; //we just used it up!
						}
						else if(insideLine.numPoints != 0 && validInsideLine == true)
						{
							ptIndex = insideLine.numPoints;
							insideLine.InsertPoint(lineSegment.get_Point(0), ref ptIndex);
							ptIndex++;
							insideLine.InsertPoint(intersectPts[i][0], ref ptIndex);

							int firstPolyLoc = insideStart;
							int lastPolyLoc = polyIntLocs[i][0]-1;
							if(SectionPolygonWithLine(ref insideLine, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
							{
								Debug.WriteLine(Error.GetLastErrorMsg());
								return false;
							}

							validInsideLine = false;
							insideLine = new MapWinGIS.ShapeClass();
							insideLine.ShapeType = line.ShapeType;
						}
					}
					else
					{ 
						//sort the intersects and their locations
						MapWinGIS.Point[] intPts = new MapWinGIS.Point[numSegIntersects];
						MapWinGIS.Point startPt = new MapWinGIS.PointClass();
						startPt = lineSegSF.get_Shape(i).get_Point(0);
						Globals.FindAndSortValidIntersects(numSegIntersects, ref intersectPts[i], ref intPts, ref startPt, ref polyIntLocs[i]);

						for(int j= 0; j<= numSegIntersects-1; j++)
						{
							if(j == 0)
							{

								if(startIntExists == true)
								{
									intersectSeg = new MapWinGIS.ShapeClass();
									intersectSeg.ShapeType = line.ShapeType;
									ptIndex = 0;
									intersectSeg.InsertPoint(startIntersect, ref ptIndex);
									ptIndex = 1;
									intersectSeg.InsertPoint(lineSegment.get_Point(0), ref ptIndex);
									ptIndex = 2;
									intersectSeg.InsertPoint(intPts[0], ref ptIndex);
								
									int firstPolyLoc = startIntPolyLoc;
									int lastPolyLoc = polyIntLocs[i][0]-1;
									if(SectionPolygonWithLine(ref intersectSeg, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
									{
										Debug.WriteLine(Error.GetLastErrorMsg());
										return false;
									}
								
									startIntExists = false; //we just used it up!
								}
								else if(insideLine.numPoints != 0 && validInsideLine == true)
								{
									ptIndex = insideLine.numPoints;
									insideLine.InsertPoint(lineSegment.get_Point(0), ref ptIndex);
									ptIndex++;
									insideLine.InsertPoint(intPts[0], ref ptIndex);

									int firstPolyLoc = insideStart;
									int lastPolyLoc = polyIntLocs[i][0]-1;
									if(SectionPolygonWithLine(ref insideLine, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
									{
										Debug.WriteLine(Error.GetLastErrorMsg());
										return false;
									}

									validInsideLine = false;
									insideLine = new MapWinGIS.ShapeClass();
									insideLine.ShapeType = line.ShapeType;
								}
							}
							else
							{
								//section the polygon with the intersecting segment
								intersectSeg = new MapWinGIS.ShapeClass();
								intersectSeg.ShapeType = line.ShapeType;
								ptIndex = 0;
								intersectSeg.InsertPoint(intPts[j], ref ptIndex);
								ptIndex = 1;
								intersectSeg.InsertPoint(intPts[j+1], ref ptIndex);
								int firstPolyLoc = polyIntLocs[i][j]-1;
								int lastPolyLoc = polyIntLocs[i][j+1]-1;
								if(SectionPolygonWithLine(ref intersectSeg, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
								{
									Debug.WriteLine(Error.GetLastErrorMsg());
									return false;
								}
								j++;
							}
						}
						
					}

				}
					//********************** case: outside->inside ***************************************
				else if(insidePts[i] == false && insidePts[i+1] == true)
				{
					validInsideLine = false;
					startIntExists = false;

					if(numSegIntersects == 0)
					{
						gErrorMsg = "No intersects found but one was expected.";
						Error.SetErrorMsg(gErrorMsg);
						Debug.WriteLine(gErrorMsg);
						return false;
					}
					else if(numSegIntersects == 1)
					{
						startIntExists = true;
						startIntersect = new MapWinGIS.PointClass();
						startIntersect = intersectPts[i][0];
						startIntPolyLoc = polyIntLocs[i][0]-1;
					}
					else
					{
						//sort the intersects and their locations
						MapWinGIS.Point[] intPts = new MapWinGIS.Point[numSegIntersects];
						MapWinGIS.Point startPt = new MapWinGIS.PointClass();
						startPt = lineSegSF.get_Shape(i).get_Point(0);
						Globals.FindAndSortValidIntersects(numSegIntersects, ref intersectPts[i], ref intPts, ref startPt, ref polyIntLocs[i]);
						
						//an odd number of intersects exist, at least one full poly section
						//will be created along with one hanging intersect pt.
						for(int j=0; j<= numSegIntersects-1; j++)
						{
							if(j == numSegIntersects-1)
							{
								startIntExists = true;
								startIntersect = new MapWinGIS.PointClass();
								startIntersect = intPts[j];
								startIntPolyLoc = polyIntLocs[i][j]-1;
							}
							else
							{
								intersectSeg = new MapWinGIS.ShapeClass();
								intersectSeg.ShapeType = line.ShapeType;
								ptIndex = 0;
								intersectSeg.InsertPoint(intPts[j], ref ptIndex);
								ptIndex = 1;
								intersectSeg.InsertPoint(intPts[j+1], ref ptIndex);
								int firstPolyLoc = polyIntLocs[i][j]-1;
								int lastPolyLoc = polyIntLocs[i][j+1]-1;
								if(SectionPolygonWithLine(ref intersectSeg, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
								{
									Debug.WriteLine(Error.GetLastErrorMsg());
									return false;
								}
								j++;
							}

						}
					}

				}
					//************************ case: outside->outside ***********************************
				else if(insidePts[i] == false && insidePts[i+1] == false)
				{
					startIntExists = false;
					validInsideLine = false;

					if(numSegIntersects == 0)
					{
						//do nothing
					}
					else
					{
						//sort the intersects and their locations
						MapWinGIS.Point[] intPts = new MapWinGIS.Point[numSegIntersects];
						MapWinGIS.Point startPt = new MapWinGIS.PointClass();
						startPt = lineSegSF.get_Shape(i).get_Point(0);
						Globals.FindAndSortValidIntersects(numSegIntersects, ref intersectPts[i], ref intPts, ref startPt, ref polyIntLocs[i]);
						
						//should always be an even amount of intersections, full poly section created
						for(int j = 0; j<= numSegIntersects-1; j++)
						{
							intersectSeg = new MapWinGIS.ShapeClass();
							intersectSeg.ShapeType = line.ShapeType;
							ptIndex = 0;
							intersectSeg.InsertPoint(intPts[j], ref ptIndex);
							ptIndex = 1;
							intersectSeg.InsertPoint(intPts[j+1], ref ptIndex);
							int firstPolyLoc = polyIntLocs[i][j]-1;
							int lastPolyLoc = polyIntLocs[i][j+1]-1;
							if(SectionPolygonWithLine(ref intersectSeg, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
							{
								Debug.WriteLine(Error.GetLastErrorMsg());
								return false;
							}
							j++;
						}

					}

				}
				
			}
			return true;
		}
		#endregion

		#region private ProcessAllOutside() -- used by Accurate_ClipPolygonWithLine()
		//Angela Hillier 10/05
		/// <summary>
		/// For lines where every point lies outside the polygon, this function will
		/// find if any 2pt segment crosses through the polygon. If so, it will split
		/// the polygon into mutliple parts using the intersecting line segments.
		/// </summary>
		/// <param name="line">The line whose points are all inside the polygon.</param>
		/// <param name="polygon">The polygon being checked for intersection.</param>
		/// <param name="resultSF">The file where new polygon sections should be saved to.</param>
		/// <returns>False if errors were encountered, true otherwise.</returns>
		private static bool ProcessAllOutside(ref MapWinGIS.Shape line, ref MapWinGIS.Shape polygon, ref MapWinGIS.Shapefile resultSF)
		{
			int numLinePts = line.numPoints;
			int numLineSegs = numLinePts-1;
			int numPolyPts = polygon.numPoints;
			int[] intersectsPerSeg = new int[numLineSegs];
			MapWinGIS.Point[][] intersectPts = new MapWinGIS.Point[numLineSegs][];
			int[][] polyIntLocs = new int[numLineSegs][]; //intersection occurs between polygon point indexed by polyIntLoc[][] and the previous point.

			//cut line into 2pt segments and put in new shapefile.
			string tempPath = System.IO.Path.GetTempPath() + "tempLineSF.shp";
			DataManagement.DeleteShapefile(ref tempPath);
			MapWinGIS.Shapefile lineSegSF = new MapWinGIS.ShapefileClass();
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

			if (numIntersects == 0)
			{
				//entire line is outside the polygon, no splitting occurs
			}
			else
			{
				//intersections exist! Find out where.
				MapWinGIS.Shape intersectSeg = new MapWinGIS.ShapeClass();
				intersectSeg.ShapeType = line.ShapeType;
				int ptIndex = 0;

				for(int i=0; i <= numLineSegs-1; i++)
				{
					int numSegIntersects = intersectsPerSeg[i];
					//if there are less than 2 intersects, the line will not cross the 
					//polygon in such a way that a new polygon section can be created.
					if(numSegIntersects == 0)
					{
						//outside lines should be ignored, we only want a portions that cross
						//the polygon.
						int c = i+1;
						while(intersectsPerSeg[c] == 0 && c <= numLineSegs-1)
						{
							c++;
							if(c == numLineSegs)
							{
								break;
							}
						}
						i = c-1;
					}
					else
					{	
						//there should always be an even # of intersects for a line of all outside pts
						//find the intersecting segments that will split the polygon
						MapWinGIS.Point[] intPts = new MapWinGIS.Point[numSegIntersects];
						MapWinGIS.Point startPt = new MapWinGIS.PointClass();
						startPt = lineSegSF.get_Shape(i).get_Point(0);
						
						Globals.FindAndSortValidIntersects(numSegIntersects, ref intersectPts[i], ref intPts, ref startPt, ref polyIntLocs[i]);

						for(int j = 0; j <= numSegIntersects-1; j++)
						{
							ptIndex = 0;
							intersectSeg.InsertPoint(intPts[j], ref ptIndex);
							ptIndex = 1;
							intersectSeg.InsertPoint(intPts[j+1], ref ptIndex);
							int polyStartIndex = polyIntLocs[i][j]-1;
							int polyEndIndex = polyIntLocs[i][j+1]-1;
							if(SectionPolygonWithLine(ref intersectSeg, ref polygon, polyStartIndex, polyEndIndex, ref resultSF) == false)
							{
								gErrorMsg = "Problem sectioning polygon: " + Error.GetLastErrorMsg();
								Error.SetErrorMsg(gErrorMsg);
								Debug.WriteLine(gErrorMsg);
								return false;
							}
							intersectSeg = new MapWinGIS.ShapeClass();
							intersectSeg.ShapeType = line.ShapeType;
							j++;
						}//end of looping through intersect pts
					}//end of else intersects exist for 2pt segment	
				}//end of looping through 2pt line segments
			}//end of else intersects exist
			return true;
		}
		#endregion

		#region private ProcessAllInside() -- used by Accurate_ClipPolygonWithLine()
		//Angela Hillier 10/05
		/// <summary>
		/// For lines where every point lies within the polygon, this function will
		/// find if any 2pt segment crosses through the polygon. If so, it will split
		/// the polygon into mutliple parts using the intersecting line segments.
		/// </summary>
		/// <param name="line">The line whose points are all inside the polygon.</param>
		/// <param name="polygon">The polygon being checked for intersection.</param>
		/// <param name="resultSF">The file where new polygon sections should be saved to.</param>
		/// <returns>False if errors were encountered, true otherwise.</returns>
		private static bool ProcessAllInside(ref MapWinGIS.Shape line, ref MapWinGIS.Shape polygon, ref MapWinGIS.Shapefile resultSF)
		{
			int numLinePts = line.numPoints;
			int numLineSegs = numLinePts-1;
			int numPolyPts = polygon.numPoints;
			int[] intersectsPerSeg = new int[numLineSegs];
			MapWinGIS.Point[][] intersectPts = new MapWinGIS.Point[numLineSegs][];
			int[][] polyIntLocs = new int[numLineSegs][]; //intersection occurs between polygon point indexed by polyIntLoc[][] and the previous point.
			
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

			if (numIntersects == 0)
			{
				//entire line is inside the polygon, no splitting occurs
			}
			else
			{
				//intersections exist! Find out where.
				MapWinGIS.Shape intersectSeg = new MapWinGIS.ShapeClass();
				intersectSeg.ShapeType = line.ShapeType;
				int ptIndex = 0;
				for(int i=0; i <= numLineSegs-1; i++)
				{
					int numSegIntersects = intersectsPerSeg[i];
					//if there are less than 4 intersects, the line will not cross the 
					//polygon in such a way that a new polygon section can be created.
					if(numSegIntersects <= 2)
					{
						//inside lines should be ignored, we only want a portion that crosses
						//the polygon.
						int c = i+1;
						while(intersectsPerSeg[c] <= 2 && c <= numLineSegs-1)
						{
							c++;
							if(c == numLineSegs)
							{
								break;
							}
						}
						i = c-1;
					}
					else
					{	//there should always be an even # of intersects for a line of all inside pts
						//find intersecting segments that will split the polygon
						MapWinGIS.Point[] intPts = new MapWinGIS.Point[numSegIntersects];
						MapWinGIS.Point startPt = new MapWinGIS.PointClass();
						startPt = lineSegSF.get_Shape(i).get_Point(0);
						
						Globals.FindAndSortValidIntersects(numSegIntersects, ref intersectPts[i], ref intPts, ref startPt, ref polyIntLocs[i]);

						for(int j = 0; j <= numSegIntersects-1; j++)
						{
							if(j == 0 || j == numSegIntersects-1)
							{
								//Any segment formed from inside pt -> intersect pt
								//or intersect pt -> inside pt will NOT cross the polygon.
							}
							else
							{
								ptIndex = 0;
								intersectSeg.InsertPoint(intPts[j], ref ptIndex);
								ptIndex = 1;
								intersectSeg.InsertPoint(intPts[j+1], ref ptIndex);
								int polyStartIndex = polyIntLocs[i][j]-1;
								int polyEndIndex = polyIntLocs[i][j+1]-1;
								if(SectionPolygonWithLine(ref intersectSeg, ref polygon, polyStartIndex, polyEndIndex, ref resultSF) == false)
								{
									gErrorMsg = "Problem sectioning polygon: " + Error.GetLastErrorMsg();
									Error.SetErrorMsg(gErrorMsg);
									Debug.WriteLine(gErrorMsg);
									return false;
								}
								intersectSeg = new MapWinGIS.ShapeClass();
								intersectSeg.ShapeType = line.ShapeType;
								j++;
							}
						}//end of looping through intersect pts
					}//end of more than 2 intersects exist		
				
				}//end of looping through 2pt line segments
			}//end of else intersects exist
			return true;
		}
		#endregion

		#region private SectionPolygonWithLine()
		//Angela Hillier 10/05
		/// <summary>
		/// Sections a polygon into multiple parts depending on where line crosses it and if previous sectioning has occured.
		/// </summary>
		/// <param name="line">The line that splits the polygon. First and last points are intersect points.</param>
		/// <param name="polygon">The polygon that is to be split by the line.</param>
		/// <param name="polyStart">Index to polygon segment where the first intersect point is found.</param>
		/// <param name="polyEnd">Index to polygon segment where last intersect point is found.</param>
		/// <param name="resultSF">Reference to result shapefile where new polygon sections will be saved.</param>
		/// <returns>False if an error occurs, true otherwise.</returns>
		private static bool SectionPolygonWithLine(ref MapWinGIS.Shape line, ref MapWinGIS.Shape polygon, int polyStart, int polyEnd, ref MapWinGIS.Shapefile resultSF)
		{
			int shpIndex = 0;
			int numResults = resultSF.NumShapes;
			bool previousSplits = false;
			if(numResults!= 0)
			{
				previousSplits = true;
			}

			//we can now make two new polygons by splitting the original one with the line segment
			MapWinGIS.Shape poly1 = new MapWinGIS.ShapeClass();
			MapWinGIS.Shape poly2 = new MapWinGIS.ShapeClass();

			SplitPolyInTwo(ref line, ref polygon, polyStart, polyEnd, out poly1, out poly2);

			if(previousSplits == false)
			{
				shpIndex = 0;
				resultSF.EditInsertShape(poly1, ref shpIndex);
				shpIndex = 1;
				resultSF.EditInsertShape(poly2, ref shpIndex);
			}
			else
			{
				//this polygon underwent previous splittings, check
				//if the new results overlay the old ones before adding to resultSF
				string tempPath = System.IO.Path.GetTempPath();
				string tempFile1 = tempPath + "test1SF.shp";
				DataManagement.DeleteShapefile(ref tempFile1);
				MapWinGIS.ShpfileType sfType = resultSF.ShapefileType;
				MapWinGIS.Shapefile test1SF = new MapWinGIS.ShapefileClass();
				//CDM 8/4/2006 test1SF.CreateNew(tempFile1, sfType);
                Globals.PrepareResultSF(ref tempFile1, ref test1SF, sfType);

				string tempFile2 = tempPath + "test2SF.shp";
				DataManagement.DeleteShapefile(ref tempFile2);
				MapWinGIS.Shapefile test2SF = new MapWinGIS.ShapefileClass();
				//CDM 8/4/2006 test2SF.CreateNew(tempFile2, sfType);
                Globals.PrepareResultSF(ref tempFile2, ref test2SF, sfType);

				if(ClipPolySFWithPoly.ClipPolygonSFWithPolygon(ref resultSF, ref poly1, out test1SF, false) == false)
				{
					gErrorMsg = "Problem clipping polygon: " + Error.GetLastErrorMsg();
					Error.SetErrorMsg(gErrorMsg);
					Debug.WriteLine(gErrorMsg);
					return false;
				}
                if (ClipPolySFWithPoly.ClipPolygonSFWithPolygon(ref resultSF, ref poly2, out test2SF, false) == false)
				{
					gErrorMsg = "Problem clipping polygon: " + Error.GetLastErrorMsg();
					Error.SetErrorMsg(gErrorMsg);
					Debug.WriteLine(gErrorMsg);
					return false;
				}

				if(test1SF.NumShapes > 0 || test2SF.NumShapes > 0)
				{
					for(int j = numResults-1; j >= 0; j--)
					{
						if(resultSF.EditDeleteShape(j) == false)
						{
							gErrorMsg = "Problem deleting intermediate polygon: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
							Error.SetErrorMsg(gErrorMsg);
							Debug.WriteLine(gErrorMsg);
							return false;
						}
					}
					int numTestShapes = test1SF.NumShapes;
					int insertIndex = 0;
					MapWinGIS.Shape insertShape = new MapWinGIS.ShapeClass();
					for(int j = 0; j <= numTestShapes-1; j++)
					{
						insertShape = test1SF.get_Shape(j);
						if(insertShape.numPoints > 0)
						{
							if(resultSF.EditInsertShape(insertShape, ref insertIndex) == false)
							{
								gErrorMsg = "Problem inserting polygon into result file: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
								Error.SetErrorMsg(gErrorMsg);
								Debug.WriteLine(gErrorMsg);
								return false;
							}
						}								
					}
					numTestShapes = test2SF.NumShapes;
					for(int j = 0; j <= numTestShapes-1; j++)
					{
						insertShape = test2SF.get_Shape(j);
						if(insertShape.numPoints > 0)
						{
							if(resultSF.EditInsertShape(insertShape, ref insertIndex) == false)
							{
								gErrorMsg = "Problem inserting polygon into result file: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
								Error.SetErrorMsg(gErrorMsg);
								Debug.WriteLine(gErrorMsg);
								return false;
							}
						}
					}
				}
			}//end of checking against previous splits
			return true;
		}
		#endregion

		#region private SplitPolyInTwo() -- used by SectionPolygonWithLine()
		//Angela Hillier 10/05
		/// <summary>
		/// Splits original polygon into two portions depending on where line crosses it.
		/// </summary>
		/// <param name="line">The line the crosses the polygon. First and last points are intersects.</param>
		/// <param name="polygon">The polygon that is split by the line.</param>
		/// <param name="beginPolySeg">The section of the polygon where the first intersect point is found.</param>
		/// <param name="endPolySeg">The section of the polygon where the last intersect point is found.</param>
		/// <param name="poly1">First portion of polygon returned after splitting.</param>
		/// <param name="poly2">Second portion of polygon returned after splitting.</param>
		private static void SplitPolyInTwo(ref MapWinGIS.Shape line, ref MapWinGIS.Shape polygon, int beginPolySeg, int endPolySeg, out MapWinGIS.Shape poly1, out MapWinGIS.Shape poly2)
		{
			//function assumes first and last pts in line are the two intersection pts
			MapWinGIS.Shape firstPart = new MapWinGIS.ShapeClass();
			MapWinGIS.Shape secondPart = new MapWinGIS.ShapeClass();
			MapWinGIS.Utils utils = new MapWinGIS.UtilsClass();
			MapWinGIS.ShpfileType shpType = polygon.ShapeType;
			firstPart.ShapeType = shpType;
			secondPart.ShapeType = shpType;
			int numPolyPts = polygon.numPoints;
			int numLinePts = line.numPoints;
			int ptIndex = 0;
			bool crossZeroPt = false;
			int count = 0;

			//now, see if we'll be crossing the zero pt while building the first result poly
			if(beginPolySeg < endPolySeg+1)
			{
				crossZeroPt = true;
			}
			else
			{
				crossZeroPt = false;
			}

			//split the poly into two portions
			//begin by creating the side where the line will be inserted in the forward direction
			//add all line pts in forward direction
			for(int i = 0; i <= numLinePts-1; i++)
			{
				ptIndex = firstPart.numPoints;
				firstPart.InsertPoint(line.get_Point(i), ref ptIndex);
			}
			//add polygon pts that are clockwise of the ending line point
			if(crossZeroPt == true)
			{
				//we'll be crossing the zero point when creating a clockwise poly
				int position;
				count = (numPolyPts-1) - (endPolySeg+1);
				//add all points before the zero point and clockwise of last point in line
				for(int i = 0; i <= count-1; i++)
				{
					position = (endPolySeg+1) + i;	
					ptIndex = firstPart.numPoints;
					firstPart.InsertPoint(polygon.get_Point(position), ref ptIndex);
				}
				//add all points after the zero point and up to first line point
				for(int i = 0; i <= beginPolySeg; i++)
				{
					ptIndex = firstPart.numPoints;
					firstPart.InsertPoint(polygon.get_Point(i), ref ptIndex);
				}
			}
			else
			{
				//we don't need to worry about crossing the zero point
				for(int i = endPolySeg+1; i <= beginPolySeg; i++)
				{
					ptIndex = firstPart.numPoints;
					firstPart.InsertPoint(polygon.get_Point(i), ref ptIndex);
				}
			}
			//add beginning line point to close the new polygon
			ptIndex = firstPart.numPoints;
			firstPart.InsertPoint(line.get_Point(0), ref ptIndex);

			//create second portion by removing first from original polygon
			//secondPart = utils.ClipPolygon(MapWinGIS.PolygonOperation.DIFFERENCE_OPERATION, polygon, firstPart);
			//above method (difference) adds unnecessary points to the resulting shape, use below instead.
			//begin by creating the side where the line will be inserted in the forward direction
			//add line pts in reverse order
			for(int i = numLinePts-1; i >= 0; i--)
			{
				ptIndex = secondPart.numPoints;
				secondPart.InsertPoint(line.get_Point(i), ref ptIndex);
			}
			//add polygon pts that are clockwise of the first line point
			//This may be confusing, but if crossZeroPt was true above, then it would
			//mean that the secondPart does not require crossing over the zero pt.
			//However, if crossZeroPt was false before, then secondPart will require
			//crossing the zeroPt while adding the polygon pts to the new shape.
			if(crossZeroPt == false)
			{
				//we'll be crossing the zero point when creating the second poly
				int position;
				count = (numPolyPts-1) - (beginPolySeg+1);
				//add all points before the zero point and clockwise of first point in line
				for(int i = 0; i <= count-1; i++)
				{
					position = (beginPolySeg+1) + i;
					ptIndex = secondPart.numPoints;
					secondPart.InsertPoint(polygon.get_Point(position), ref ptIndex);
				}
				//add all points after the zero point and up to last line point
				for(int i = 0; i <= endPolySeg; i++)
				{
					ptIndex = secondPart.numPoints;
					secondPart.InsertPoint(polygon.get_Point(i), ref ptIndex);
				}
			}
			else
			{
				//we don't need to worry about crossing the zero point
				for(int i = beginPolySeg+1; i <= endPolySeg; i++)
				{
					ptIndex = secondPart.numPoints;
					secondPart.InsertPoint(polygon.get_Point(i), ref ptIndex);
				}
			}
			//add ending line point to close the new polygon
			ptIndex = secondPart.numPoints;
			secondPart.InsertPoint(line.get_Point(numLinePts-1), ref ptIndex);

			//return the two polygon portions
			poly1 = firstPart;
			poly2 = secondPart;

			//output results to screen for testing
			//Debug.WriteLine("poly1 numPoints = " + firstPart.numPoints);
			//			string poly1Points = "";
			//			for(int i = 0; i <= firstPart.numPoints-1; i++)
			//			{
			//				poly1Points += "(" + firstPart.get_Point(i).x + ", " + firstPart.get_Point(i).y + "), ";
			//			}
			//Debug.WriteLine(poly1Points);
			//Debug.WriteLine("poly2 numPoints = " + secondPart.numPoints);
			//			string poly2Points = "";
			//			for(int i = 0; i <= secondPart.numPoints-1; i++)
			//			{
			//				poly2Points += "(" + secondPart.get_Point(i).x + ", " + secondPart.get_Point(i).y + "), ";
			//			}
			//Debug.WriteLine(poly2Points);		
			
		}
		#endregion

	}
}
