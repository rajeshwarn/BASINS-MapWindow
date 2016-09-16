//********************************************************************************************************
//File name: Erase.cs
//Description: Internal class for removing portions of point, line, or polygon shapefiles
//that fall within given erase polygons.
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
//4-19-06 ah - Angela Hillier - Added functions to erase portions of point, line, and polygon shapefiles. 							
//********************************************************************************************************
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MapWinGeoProc
{
	/// <summary>
	/// The erase functions remove portions of the input shapefile that fall within the erase polygon(s).
	/// </summary>
	internal class Erase
	{
		private static string gErrorMsg = "";

		#region ErasePointSFWithPoly()
		/// <summary>
		/// Removes points from the point shapefile that lie within the polygon.
		/// </summary>
		/// <param name="pointSF">The point shapefile.</param>
		/// <param name="polygon">The erase polygon.</param>
		/// <param name="resultSF">The resulting file with points removed.</param>
        /// <param name="CopyAttributes">Indicates whether to copy attributes</param>
		/// <returns>False if an error was encountered, true otherwise.</returns>
		public static bool ErasePointSFWithPoly(ref MapWinGIS.Shapefile pointSF, ref MapWinGIS.Shape polygon, ref MapWinGIS.Shapefile resultSF, bool CopyAttributes)
		{
            MapWinUtility.Logger.Dbg("ErasePointSFWithPoly(pointSF: " + Macro.ParamName(pointSF) + "\n, " +
                                     "                     polygon: " + Macro.ParamName(polygon) + "\n, " +
                                     "                     resultSF: " + Macro.ParamName(resultSF) + "\n, " +
                                     "                     CopyAttributes: " + CopyAttributes.ToString() + ")");
			
            if(pointSF == null || polygon == null ||resultSF == null)
			{
				gErrorMsg = "One of the input parameters is null.";
				Error.SetErrorMsg(gErrorMsg);
				Debug.WriteLine(gErrorMsg);
                MapWinUtility.Logger.Dbg(gErrorMsg);
				return false;
			}

            if (CopyAttributes)
            {
                string tmpName;
                MapWinGIS.Field tmpField, currField;
                for (int f = 0; f <= pointSF.NumFields - 1; f++)
                {
                    tmpField = new MapWinGIS.Field();
                    currField = pointSF.get_Field(f);
                    tmpName = currField.Name;
                    tmpField.Name = tmpName;

                    tmpField.Width = currField.Width;
                    tmpField.Type = currField.Type;
                    tmpField.Precision = currField.Precision;
                    tmpField.Key = currField.Key;
                    resultSF.EditInsertField(tmpField, ref f, null);
                }
            }

			int numPts = pointSF.NumShapes;
			int numParts = polygon.NumParts;
			if(numParts == 0)
			{
				numParts = 1;
			}
			int shpIndex = 0;
			Globals.Vertex[][] vertArray = new Globals.Vertex[numParts][];
			Globals.ConvertPolyToVertexArray(ref polygon, out vertArray);

			for(int i = 0; i <= numPts-1; i++)
			{
				MapWinGIS.Point currPt = new MapWinGIS.PointClass();
				currPt = pointSF.QuickPoint(i, 0);
				double currX = currPt.x;
				double currY = currPt.y;
				if(Utils.PointInPoly(ref vertArray, currX, currY) == false)
				{
					shpIndex = resultSF.NumShapes;
					if(resultSF.EditInsertShape(pointSF.get_Shape(i), ref shpIndex) == false)
					{
						gErrorMsg = "ErasePointSF: problem inserting shape into result file: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
						Debug.WriteLine(gErrorMsg);
						Error.SetErrorMsg(gErrorMsg);
                        MapWinUtility.Logger.Dbg(gErrorMsg);
						return false;
					}
                    if (CopyAttributes)
                    {
                        for (int f = 0; f <= pointSF.NumFields - 1; f++)
                        {
                            bool tmpbool = resultSF.EditCellValue(f, shpIndex, pointSF.get_CellValue(f, i));
                        }
                    }
				}		
			}
            MapWinUtility.Logger.Dbg("Finished ErasePointSFWithPoly");
			return true;
		}
       
		#endregion

		#region ErasePointSFWithPolySF()
		/// <summary>
		/// Removes points from the point shapefile that lie within any shapes in the polygon shapefile.
		/// </summary>
		/// <param name="pointSF">The point shapefile.</param>
		/// <param name="polygonSF">The shapefile containing the erase polygons.</param>
		/// <param name="resultSF">The resulting file with points removed.</param>
		/// <returns>False if an error was encountered, true otherwise.</returns>
		public static bool ErasePointSFWithPolySF(ref MapWinGIS.Shapefile pointSF, ref MapWinGIS.Shapefile polygonSF, ref MapWinGIS.Shapefile resultSF)
		{
            MapWinUtility.Logger.Dbg("ErasePointSFWithPolySF(pointSF : " + Macro.ParamName(pointSF) + ",\n" +
                                     "                       polygonSF: " + Macro.ParamName(polygonSF) + ",\n" +
                                     "                       resultSF: " + resultSF.ToString());
			if(pointSF == null || polygonSF == null || resultSF == null)
			{
				gErrorMsg = "One of the input parameters is null.";
				Error.SetErrorMsg(gErrorMsg);
				Debug.WriteLine(gErrorMsg);
                MapWinUtility.Logger.Dbg(gErrorMsg);
				return false;
			}
			int shpIndex = 0;
			int numPts = pointSF.NumShapes;
			for(int i = 0; i <= numPts-1; i++)
			{
				shpIndex = resultSF.NumShapes;
				resultSF.EditInsertShape(pointSF.get_Shape(i), ref shpIndex);
			}
			
			int numPolygons = polygonSF.NumShapes;
			for(int i = 0; i <= numPolygons-1; i++)
			{
				MapWinGIS.Shape currPoly = new MapWinGIS.ShapeClass();
				currPoly.Create(polygonSF.ShapefileType);
				currPoly = polygonSF.get_Shape(i);
				int numParts = currPoly.NumParts;
				if(numParts == 0)
				{
					numParts = 1;
				}
				Globals.Vertex[][] polyVertArray = new Globals.Vertex[numParts][];
				Globals.ConvertPolyToVertexArray(ref currPoly, out polyVertArray);

				numPts = resultSF.NumShapes;
				for(int j = 0; j <= numPts-1; j++)
				{
					double x = resultSF.QuickPoint(j, 0).x;
					double y = resultSF.QuickPoint(j, 0).y;
					if(Utils.PointInPoly(ref polyVertArray, x, y) == true)
					{
						//remove the point.
						resultSF.EditDeleteShape(j);
						numPts--;
						j--;
					}			
				}				
			}
            MapWinUtility.Logger.Dbg("Finsihed ErasePointSFWithPolySF");
            return true;
           
		}
		#endregion

		#region ErasePolySFWithPoly()
		/// <summary>
		/// Erases the portions of the polygon shapefile that are within the polygon shape.
		/// </summary>
		/// <param name="polySF">The polygon shapefile.</param>
		/// <param name="polygon">The erase polygon.</param>
		/// <param name="resultSF">The resulting shapefile, with portions removed.</param>
        /// <param name="CopyAttributes">Indicates whether to copy attributes or not.</param>
		/// <returns>False if an error was encountered, true otherwise.</returns>
		public static bool ErasePolySFWithPoly(ref MapWinGIS.Shapefile polySF, ref MapWinGIS.Shape polygon, ref MapWinGIS.Shapefile resultSF, bool CopyAttributes)
		{
            MapWinUtility.Logger.Dbg("ErasePolySFWithPoly(polySF: " + Macro.ParamName(polySF) + ",\n" +
                                     "                    polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                    resultSF: " + Macro.ParamName(resultSF) + "\n" +
                                     "                    CopyAttributes: " + CopyAttributes.ToString());
			if(polySF == null || polygon == null || resultSF == null)
			{
				gErrorMsg = "One of the input parameters is null.";
				Error.SetErrorMsg(gErrorMsg);
				Debug.WriteLine(gErrorMsg);
                MapWinUtility.Logger.Dbg(gErrorMsg);
				return false;
			}

            if (CopyAttributes)
            {
                string tmpName;
                MapWinGIS.Field tmpField, currField;
                for (int f = 0; f <= polySF.NumFields - 1; f++)
                {
                    tmpField = new MapWinGIS.Field();
                    currField = polySF.get_Field(f);
                    tmpName = currField.Name;
                    tmpField.Name = tmpName;

                    tmpField.Width = currField.Width;
                    tmpField.Type = currField.Type;
                    tmpField.Precision = currField.Precision;
                    tmpField.Key = currField.Key;
                    resultSF.EditInsertField(tmpField, ref f, null);
                }        
            }
			
			int numShapes = polySF.NumShapes;
			int shpIndex = 0;
			for(int i = 0; i <= numShapes-1; i++)
			{
				MapWinGIS.Shape currShape = new MapWinGIS.Shape();
                MapWinGIS.Shape resultShp = new MapWinGIS.Shape();

				currShape = polySF.get_Shape(i);

				//if bounds intersect, then check if all polygon points are inside the currShape
				if(Globals.CheckBounds(ref currShape, ref polygon))
				{
					int numPts = polygon.numPoints;
					bool allInside = true;
					int numParts = currShape.NumParts;
					if(numParts == 0)
					{
						numParts = 1;
					}
					Globals.Vertex[][] vertArray = new Globals.Vertex[numParts][];
					Globals.ConvertPolyToVertexArray(ref currShape, out vertArray);
					for(int j = 0; j <= numPts-1; j++)
					{
						double x = polygon.get_Point(j).x;
						double y = polygon.get_Point(j).y;
						if(Utils.PointInPoly(ref vertArray, x, y) == false)
						{
							allInside = false;
							break;
						}
					}

					if(allInside == true)
					{
						resultShp = new MapWinGIS.ShapeClass();
						resultShp.Create(polygon.ShapeType);
						//we want the symmetric difference of these two shapes
						//which should leave us with a hole where the erase polygon was in the currShape
						resultShp = SpatialOperations.SymmetricDifference(polygon, currShape);
					}
					else
					{
						//erase overlapping section and add result to the file.
						MapWinGIS.Shape intersect = new MapWinGIS.ShapeClass();
						intersect.ShapeType = polygon.ShapeType;
						intersect = SpatialOperations.Intersection(polygon, currShape);
                        // Paul Meems: Related to bug 1068. Added check for correct shape:
                        if (intersect != null)
                        {
                            if (intersect.numPoints != 0 && !intersect.IsValid)
                            {
                                //there might be parts in the difference result that do not belong,
                                //perform an intersection operation with currShape to remove them.
                                MapWinGIS.Shape diff = new MapWinGIS.ShapeClass();
                                diff.ShapeType = polygon.ShapeType;
                                //diff = SpatialOperations.SymmetricDifference(intersect, currShape);
                                diff = SpatialOperations.Difference(currShape, polygon);
                                // Paul Meems: Related to bug 1068. Added check for correct shape:
                                if (diff != null)
                                {
                                    if (diff.numPoints != 0 && !diff.IsValid)
                                    {
                                        resultShp = diff;
                                    }
                                    else
                                    {
                                        gErrorMsg = "SpatialOperations.Difference returned an invalid shape";
                                        Debug.WriteLine(gErrorMsg);
                                        Error.SetErrorMsg(gErrorMsg);
                                        MapWinUtility.Logger.Dbg(gErrorMsg);
                                    }
                                }//difference operation successful
                            }
                            else
                            {
                                gErrorMsg = "SpatialOperations.Intersection returned an invalid shape";
                                Debug.WriteLine(gErrorMsg);
                                Error.SetErrorMsg(gErrorMsg);
                                MapWinUtility.Logger.Dbg(gErrorMsg);
                            }
						}//intersect operation successful
						else
						{
							//no intersection, shapes do not collide
							resultShp = currShape;
						}
					}//all points of erase polygon are not inside currShape
							
					if(resultShp.numPoints > 0)
					{
						shpIndex = resultSF.NumShapes;
						if(resultSF.EditInsertShape(resultShp, ref shpIndex)== false)
						{
							gErrorMsg = "ErasePolySF: problem inserting shape into result file: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
							Debug.WriteLine(gErrorMsg);
							Error.SetErrorMsg(gErrorMsg);
                            MapWinUtility.Logger.Dbg(gErrorMsg);
							return false;
						}
                        if (CopyAttributes)
                        {
                            for (int f = 0; f <= polySF.NumFields - 1; f++)
                            {
                                bool tmpbool = resultSF.EditCellValue(f, shpIndex, polySF.get_CellValue(f, i));
                            }
                        }
					}
				}//end of if bounds intersect
				else
				{
					//the erase object does not intersect with the current polygon,
					//add current polygon to resultSF in unchanged form
					shpIndex = resultSF.NumShapes;
					if(resultSF.EditInsertShape(currShape, ref shpIndex) == false)
					{
						gErrorMsg = "ErasePolySF: problem inserting shape into result file: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
						Debug.WriteLine(gErrorMsg);
						Error.SetErrorMsg(gErrorMsg);
                        MapWinUtility.Logger.Dbg(gErrorMsg);
						return false;
					}
                    if (CopyAttributes)
                    {
                        for (int f = 0; f <= polySF.NumFields - 1; f++)
                        {
                            bool tmpbool = resultSF.EditCellValue(f, shpIndex , polySF.get_CellValue(f, i));
                        }
                    }
				}
			}//end of looping through shapes in shapefile
            MapWinUtility.Logger.Dbg("Finished ErasePolySFWithPoly");
			return true;
		}
		#endregion

		#region ErasePolySFWithPolySF()
		/// <summary>
		/// Removes portions of the input polygon shapefile that are within the erase polygons.
		/// </summary>
		/// <param name="inputSF">The input polygon shapefile.</param>
		/// <param name="eraseSF">The erase polygon shapefile.</param>
		/// <param name="resultSF">The resulting shapefile, with portions removed.</param>
		/// <returns>False if an error was encountered, true otherwise.</returns>
		public static bool ErasePolySFWithPolySF(ref MapWinGIS.Shapefile inputSF, ref MapWinGIS.Shapefile eraseSF, ref MapWinGIS.Shapefile resultSF)
		{
            MapWinUtility.Logger.Dbg("ErasePolySFWithPolySF(inputSF: " + Macro.ParamName(inputSF) + ",\n" +
                                     "                      eraseSF: " + Macro.ParamName(eraseSF) + ",\n" +
                                     "                      resultSF: " + Macro.ParamName(resultSF) + ",\n");
			if(inputSF == null || eraseSF == null || resultSF == null)
			{
				gErrorMsg = "One of the input parameters is null.";
				Error.SetErrorMsg(gErrorMsg);
				Debug.WriteLine(gErrorMsg);
                MapWinUtility.Logger.Dbg(gErrorMsg);
				return false;
			}
			int numInputs = inputSF.NumShapes;
			int shpIndex = 0;
			//create the result shapefile out of the original inputSF
			for(int i = 0; i <= numInputs-1; i++)
			{
				shpIndex = resultSF.NumShapes;
				resultSF.EditInsertShape(inputSF.get_Shape(i), ref shpIndex);
			}
			
			int numErase = eraseSF.NumShapes;
			for(int i = 0; i <= numErase-1; i++)
			{
				MapWinGIS.Shape eraseShape = new MapWinGIS.ShapeClass();
				eraseShape = eraseSF.get_Shape(i);

				MapWinGIS.Shape resultShp = new MapWinGIS.ShapeClass();
				for(int j= 0; j <= numInputs-1; j++)
				{
					MapWinGIS.Shape currShape = new MapWinGIS.ShapeClass();
					currShape = resultSF.get_Shape(j);

					//if bounds intersect, then check if all polygon points are inside the currShape
					if(Globals.CheckBounds(ref currShape, ref eraseShape))
					{
						int numPts = eraseShape.numPoints;
						bool allInside = true;
						int numParts = eraseShape.NumParts;
						if(numParts == 0)
						{
							numParts = 1;
						}
						Globals.Vertex[][] vertArray = new Globals.Vertex[numParts][];
						Globals.ConvertPolyToVertexArray(ref currShape, out vertArray);
						for(int k = 0; k <= numPts-1; k++)
						{
							double x = eraseShape.get_Point(k).x;
							double y = eraseShape.get_Point(k).y;
							if(Utils.PointInPoly(ref vertArray, x, y) == false)
							{
								allInside = false;
								break;
							}
						}
						
						if(allInside == true)
						{
							resultShp = new MapWinGIS.ShapeClass();
							resultShp.Create(inputSF.ShapefileType);
							//we want the symmetric difference of these two shapes
							//which should leave us with a hole where the erase polygon was in the currShape
							resultShp = SpatialOperations.SymmetricDifference(eraseShape, currShape);
						}
						else
						{
							//erase overlapping section and add result to the file.
							MapWinGIS.Shape intersect = new MapWinGIS.ShapeClass();
							intersect.ShapeType = inputSF.ShapefileType;
							intersect = SpatialOperations.Intersection(eraseShape, currShape);
                            // Paul Meems: Related to bug 1068. Added check for correct shape:
                            if (intersect != null)
                            {                                
                                if (intersect.numPoints != 0 && !intersect.IsValid)
                                {
                                    MapWinGIS.Shape diff = new MapWinGIS.ShapeClass();
                                    diff.ShapeType = eraseShape.ShapeType;
                                    diff = SpatialOperations.Difference(currShape, eraseShape);
                                    // Paul Meems: Related to bug 1068. Added check for correct shape:
                                    if (diff != null)
                                    {
                                        if (diff.numPoints != 0 && !diff.IsValid)
                                        {
                                            resultShp = new MapWinGIS.ShapeClass();
                                            resultShp.Create(inputSF.ShapefileType);
                                            resultShp = diff;
                                        }
                                        else
                                        {
                                            gErrorMsg = "SpatialOperations.Difference returned an invalid shape";
                                            Debug.WriteLine(gErrorMsg);
                                            Error.SetErrorMsg(gErrorMsg);
                                            MapWinUtility.Logger.Dbg(gErrorMsg);
                                        }
                                    }//difference operation successful
                                }
                                else
                                {
                                    gErrorMsg = "SpatialOperations.Intersection returned an invalid shape";
                                    Debug.WriteLine(gErrorMsg);
                                    Error.SetErrorMsg(gErrorMsg);
                                    MapWinUtility.Logger.Dbg(gErrorMsg);
                                }                            
							}//intersect operation successful
							else
							{
								//no intersection, shapes do not collide
								resultShp = currShape;
							}
						}//all points of erase polygon are not inside currShape

                        // Paul Meems: Added check for null:
                        if (resultShp != null && resultShp.numPoints > 0)
						{
							shpIndex = j;
							resultSF.EditDeleteShape(shpIndex);
							resultSF.EditInsertShape(resultShp, ref shpIndex);
						}
					}//end of bounds intersect
				}//end of looping through input polygons
			}//end of looping through erase polygons
            MapWinUtility.Logger.Dbg("Finsihed ErasePolySFWithPolySF");
			return true;
		}
		#endregion

		#region EraseLineSFWithPoly()
		/// <summary>
		/// Removes portions of the lineSF that fall within the erase polygon
		/// </summary>
		/// <param name="lineSF">The shapefile of lines to be erased.</param>
		/// <param name="erasePoly">The polygon to be used for erasing portion of the line shapefile.</param>
		/// <param name="resultSF">The resulting line shapefile with portions removed.</param>
        /// <param name="CopyAttributes">Indicates whether to copy attributes</param>
		/// <returns>False if an error was encountered, true otherwise.</returns>
		public static bool EraseLineSFWithPoly(ref MapWinGIS.Shapefile lineSF, ref MapWinGIS.Shape erasePoly, ref MapWinGIS.Shapefile resultSF, bool CopyAttributes)
		{
            MapWinUtility.Logger.Dbg("EraseLineSFWithPoly(lineSF: " + Macro.ParamName(lineSF) + ",\n" +
                                     "                    erasePoly: " + Macro.ParamName(erasePoly) + ",\n" + 
                                     "                    resultSF: " + Macro.ParamName(resultSF) + ",\n" +
                                     "                    CopyAttributes: " + CopyAttributes.ToString() + ")");
			if(lineSF == null || erasePoly == null || resultSF == null)
			{
				gErrorMsg = "One of the input parameters is null.";
				Error.SetErrorMsg(gErrorMsg);
				Debug.WriteLine(gErrorMsg);
                MapWinUtility.Logger.Dbg(gErrorMsg);
				return false;
			}

            if (CopyAttributes)
            {
                string tmpName;
                MapWinGIS.Field tmpField, currField;
                for (int f = 0; f <= lineSF.NumFields - 1; f++)
                {
                    tmpField = new MapWinGIS.Field();
                    currField = lineSF.get_Field(f);
                    tmpName = currField.Name;
                    tmpField.Name = tmpName;

                    tmpField.Width = currField.Width;
                    tmpField.Type = currField.Type;
                    tmpField.Precision = currField.Precision;
                    tmpField.Key = currField.Key;
                    resultSF.EditInsertField(tmpField, ref f, null);
                }
            }
            
            int shpIndex = 0;
			int numLines = lineSF.NumShapes;
			for(int i = 0; i <= numLines-1; i++)
			{
				MapWinGIS.Shape currLine = new MapWinGIS.ShapeClass();
				currLine.Create(lineSF.ShapefileType);
				currLine = lineSF.get_Shape(i);

				MapWinGIS.Shape lineEnvelope = new MapWinGIS.ShapeClass();
				lineEnvelope.Create(MapWinGIS.ShpfileType.SHP_POLYGON);
				//create lineExtents' points out of the line extent points
				MapWinGIS.Point lTop, rTop, rBottom, lBottom;
				lTop = new MapWinGIS.PointClass();
				lTop.x = currLine.Extents.xMin;
				lTop.y = currLine.Extents.yMax;
				rTop = new MapWinGIS.PointClass();
				rTop.x = currLine.Extents.xMax;
				rTop.y = currLine.Extents.yMax;
				rBottom = new MapWinGIS.PointClass();
				rBottom.x = currLine.Extents.xMax;
				rBottom.y = currLine.Extents.yMin;
				lBottom = new MapWinGIS.PointClass();
				lBottom.x = currLine.Extents.xMin;
				lBottom.y = currLine.Extents.yMin;
				//now add the extent points to the new polygon shape: lineEnvelope
				int ptIndex = 0;
				lineEnvelope.InsertPoint(lTop, ref ptIndex);
				ptIndex++;
				lineEnvelope.InsertPoint(rTop, ref ptIndex);
				ptIndex++;
				lineEnvelope.InsertPoint(rBottom, ref ptIndex);
				ptIndex++;
				lineEnvelope.InsertPoint(lBottom, ref ptIndex);
				ptIndex++;
				lineEnvelope.InsertPoint(lTop, ref ptIndex);
				//remove COM points from memory
                //while(Marshal.ReleaseComObject(lTop) != 0);
                //while(Marshal.ReleaseComObject(rTop) != 0);
                //while(Marshal.ReleaseComObject(rBottom) != 0);
                //while(Marshal.ReleaseComObject(lBottom) != 0);

				//Check if line extents and polygon extents overlap
				if(Globals.CheckBounds(ref lineEnvelope, ref erasePoly))
				{
					//make the envelope polygon slightly larger
					MapWinGIS.Shape lgEnvelope = new MapWinGIS.ShapeClass();
					lgEnvelope.Create(MapWinGIS.ShpfileType.SHP_POLYGON);
                    // Paul Meems: Added check if BufferPolygon returned false
                    if (!SpatialOperations.BufferPolygon(ref lineEnvelope, 0.5, Enumerations.Buffer_HoleTreatment.Ignore, Enumerations.Buffer_CapStyle.Pointed, out lgEnvelope))
                    {
                        return false;
                    }

					//take the difference of the envelope polygon with the erase polygon.
					MapWinGIS.Shape diff = new MapWinGIS.ShapeClass();
					diff.Create(MapWinGIS.ShpfileType.SHP_POLYGON);
					diff = SpatialOperations.Difference(lgEnvelope, erasePoly);
                    // Paul Meems: Related to bug 1068. Added check for correct shape:
                    if (diff != null)
                    {
                        if (diff.numPoints != 0 && !diff.IsValid)
                        {
						    //the difference shape represents the line envelope
						    //minus the area of the erase polygon.
						    MapWinGIS.Shapefile inputLine = new MapWinGIS.ShapefileClass();
						    string tempPath = System.IO.Path.GetTempPath() + "tempInputLine.shp";
						    //CDM 8/4/2006 inputLine.CreateNew(tempPath, lineSF.ShapefileType);
                            Globals.PrepareResultSF(ref tempPath, ref inputLine, lineSF.ShapefileType);
						    shpIndex = 0;
						    inputLine.EditInsertShape(currLine, ref shpIndex);

						    int numParts = diff.NumParts;
						    if(numParts == 0)
						    {
							    numParts = 1;
						    }

						    if(numParts > 1)
						    {
							    //separate and test each part individually
							    MapWinGIS.Shape[] diffParts = new MapWinGIS.Shape[numParts];
							    Globals.SeparateParts(ref diff, out diffParts);
							    for(int j = 0; j <= numParts-1; j++)
							    {
								    //don't check inside of holes
								    if(Globals.IsClockwise(ref diffParts[j]))
								    {
									    MapWinGIS.Shapefile tempLineResult = new MapWinGIS.ShapefileClass();
									    string tempLineFile = System.IO.Path.GetTempPath() + "tempLines.shp";
									    DataManagement.DeleteShapefile(ref tempLineFile);
									    //CDM 8/4/2006 tempLineResult.CreateNew(tempLineFile, lineSF.ShapefileType);
                                        Globals.PrepareResultSF(ref tempLineFile, ref tempLineResult, lineSF.ShapefileType);
									    tempLineResult.StartEditingShapes(true, null);

                                        // Paul Meems: Added check if returned false:
                                        if (!SpatialOperations.ClipShapesWithPolygon(ref inputLine, ref diffParts[j], out tempLineResult, false))
                                        {
                                            return false;
                                        }
    					
									    int numResults = tempLineResult.NumShapes;
									    if(numResults > 0)
									    {
										    //add results to the final result file.
										    for(int k = 0; k <= numResults-1; k++)
										    {
											    shpIndex = resultSF.NumShapes;
											    resultSF.EditInsertShape(tempLineResult.get_Shape(k), ref shpIndex);
                                                if (CopyAttributes)
                                                {
                                                    for (int f = 0; f <= lineSF.NumFields - 1; f++)
                                                    {
                                                        bool tmpbool = resultSF.EditCellValue(f, shpIndex, lineSF.get_CellValue(f, i));
                                                    }
                                                }
										    }							
									    }//clipping successful
								    }//done checking islands
							    }//done looping through parts of the difference shape
						    }
						    else
						    {
							    MapWinGIS.Shapefile tempLineResult = new MapWinGIS.ShapefileClass();
							    string tempLineFile = System.IO.Path.GetTempPath() + "tempLines.shp";
							    DataManagement.DeleteShapefile(ref tempLineFile);
							    //CDM 8/4/2006 tempLineResult.CreateNew(tempLineFile, lineSF.ShapefileType);
                                Globals.PrepareResultSF(ref tempLineFile, ref tempLineResult, lineSF.ShapefileType);
							    tempLineResult.StartEditingShapes(true, null);
                                
                                // Paul Meems: Added check if returned false:
                                if (!SpatialOperations.ClipShapesWithPolygon(ref inputLine, ref diff, out tempLineResult, false))
                                {
                                    return false;
                                }
    					
							    int numResults = tempLineResult.NumShapes;
							    if(numResults > 0)
							    {
								    //add results to the final result file.
								    for(int k = 0; k <= numResults-1; k++)
								    {
									    shpIndex = resultSF.NumShapes;
									    resultSF.EditInsertShape(tempLineResult.get_Shape(k), ref shpIndex);
                                        if (CopyAttributes)
                                        {
                                            for (int f = 0; f <= lineSF.NumFields - 1; f++)
                                            {
                                                bool tmpbool = resultSF.EditCellValue(f, shpIndex, lineSF.get_CellValue(f, i));
                                            }
                                        }
								    }							
							    }//clipping successful
						    }
                        }
                        else
                        {
                            gErrorMsg = "SpatialOperations.Intersection returned an invalid shape";
                            Debug.WriteLine(gErrorMsg);
                            Error.SetErrorMsg(gErrorMsg);
                            MapWinUtility.Logger.Dbg(gErrorMsg);
                        }
                    }//difference operation successful
				}//bounds overlapped
				else
				{
					shpIndex = resultSF.NumShapes;
					resultSF.EditInsertShape(currLine, ref shpIndex);
                    if (CopyAttributes)
                    {
                        for (int f = 0; f <= lineSF.NumFields - 1; f++)
                        {
                            bool tmpbool = resultSF.EditCellValue(f, shpIndex, lineSF.get_CellValue(f, i));
                        }
                    }
				}

			}//end of looping through lines in the input shapefile
            MapWinUtility.Logger.Dbg("Finished EraseLineSFWithPoly");
			return true;
		}
		#endregion

		#region EraseLineSFWithPolySF()
		/// <summary>
		/// Removes portions of the input line shapefile that fall within the polygons of the erase polygon shapefile.
		/// </summary>
		/// <param name="inputSF">The line shapefile to erase.</param>
		/// <param name="eraseSF">The polygon shapefile that will be used to erase portions of the line shapefile.</param>
		/// <param name="resultSF">The result shapefile.</param>
		/// <returns>False if an error was encountered, true otherwise.</returns>
		public static bool EraseLineSFWithPolySF(ref MapWinGIS.Shapefile inputSF, ref MapWinGIS.Shapefile eraseSF, ref MapWinGIS.Shapefile resultSF)
		{
            MapWinUtility.Logger.Dbg("EraseLineSFWithPolySF(inputSF: " + Macro.ParamName(inputSF) + "\n" +
                                     "                      eraseSF: " + Macro.ParamName(eraseSF) + "\n" +
                                     "                      resultSF: " + Macro.ParamName(resultSF) + ")");

			if(inputSF == null || eraseSF == null || resultSF == null)
			{
				gErrorMsg = "One of the input parameters is null.";
				Error.SetErrorMsg(gErrorMsg);
				Debug.WriteLine(gErrorMsg);
                MapWinUtility.Logger.Dbg(gErrorMsg);
				return false;
			}
			bool status = true;
			MapWinGIS.Shapefile tempInput = new MapWinGIS.ShapefileClass();
			string tempFile = System.IO.Path.GetTempPath() + "tempLineResult.shp";
			//CDM 8/4/2006 tempInput.CreateNew(tempFile, inputSF.ShapefileType);
            Globals.PrepareResultSF(ref tempFile, ref tempInput, inputSF.ShapefileType);
			tempInput.StartEditingShapes(true, null);
			int shpIndex = 0;
			int numInputs = inputSF.NumShapes;
			for(int i = 0; i <= numInputs-1; i++)
			{
				tempInput.EditInsertShape(inputSF.get_Shape(i), ref shpIndex);
				shpIndex++;
			}
			
			int numErase = eraseSF.NumShapes;
			for(int i = 0; i <= numErase-1; i++)
			{
				Debug.WriteLine("Num shapes in tempInput = " + tempInput.NumShapes);
				MapWinGIS.Shape eraseShp = new MapWinGIS.ShapeClass();
				eraseShp = eraseSF.get_Shape(i);
				resultSF.EditClear();
				status = EraseLineSFWithPoly(ref tempInput, ref eraseShp, ref resultSF, false);
				if(i != numErase-1)
				{
					int numResults = resultSF.NumShapes;
					if(numResults > 0)
					{
						tempInput.EditClear();
						shpIndex = 0;
						for(int j = 0; j <= numResults-1; j++)
						{
							tempInput.EditInsertShape(resultSF.get_Shape(j), ref shpIndex);
							shpIndex++;
						}
					}
				}
			}
            MapWinUtility.Logger.Dbg("Finished EraseLineSFWithPolySF"); 
			return status;
		}
		#endregion

	}
}

