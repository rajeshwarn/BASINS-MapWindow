//File name: ClipPointSFWithPoly.cs
//Description: Internal class, provides functions for clipping a point shapefile with a polygon.
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
	/// Functions for clipping a point shapefile with a polygon.
	/// </summary>
	internal class ClipPointSFWithPoly
	{
		private static string gErrorMsg = "";

		#region private ClipPointSFWithPolygon()

		#region Save-to-Disk version
		/// <summary>
		/// Returns a shapefile of points from the input shapefile that fall within the polygon.
		/// </summary>
		/// <param name="pointSFPath">Full path to the point shapefile.</param>
		/// <param name="polygon">The polygon used for clipping the point shapefile.</param>
		/// <param name="resultSFPath">Full path to where the resulting point shapefile should be saved.</param>
        /// <param name="copyAttributes">True if copying attributes over</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
		public static bool ClipPointSFWithPolygon(ref string pointSFPath, ref MapWinGIS.Shape polygon, ref string resultSFPath, bool copyAttributes)
		{
            MapWinUtility.Logger.Dbg("ClipPointSFWithPolygon(pointSFPath: " + pointSFPath + ",\n" +
                                     "                       polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                       resultSFPath: " + resultSFPath.ToString() + ",\n" +
                                     "                       copyAttributes: " + copyAttributes.ToString() + ")");

			MapWinGIS.Shapefile resultSF = new MapWinGIS.ShapefileClass();
			MapWinGIS.Shapefile pointSF = new MapWinGIS.ShapefileClass();
			int shpIndex = 0; //all new shapes will be placed at the beginning of the shapefile
            string tmpName;

			if(pointSF.Open(pointSFPath, null) == false)
			{
				gErrorMsg = "Failure to open input shapefile: " + pointSF.get_ErrorMsg(pointSF.LastErrorCode);
                MapWinUtility.Logger.Dbg(gErrorMsg);
				return false;
			}
			MapWinGIS.ShpfileType sfType = pointSF.ShapefileType;
		
			//make sure we are dealing with a valid shapefile type
			if(sfType == MapWinGIS.ShpfileType.SHP_POINT || sfType == MapWinGIS.ShpfileType.SHP_POINTM || sfType == MapWinGIS.ShpfileType.SHP_POINTZ)
			{
                if (Globals.PrepareResultSF(ref resultSFPath, ref resultSF, sfType, copyAttributes) == false)
				{
					return false;
				}

                if (copyAttributes)
                {
                    MapWinGIS.Field tmpField, pointField;
                    for (int f = 0; f <= pointSF.NumFields - 1; f++)
                    {
                        tmpField = new MapWinGIS.Field();
                        pointField = pointSF.get_Field(f);
                        tmpName = pointField.Name;
                        if (tmpName.Contains("MWShapeID"))
                        {
                            tmpField.Name = "Last_" + tmpName;
                        }
                        else
                        {
                            tmpField.Name = tmpName;
                        }
                        tmpField.Width = pointField.Width;
                        tmpField.Type = pointField.Type;
                        tmpField.Precision = pointField.Precision;

                        tmpField.Key = pointField.Key;
                        if (!resultSF.EditInsertField(tmpField, ref f, null))
                        {
                            return false;
                        }
                    }
                }

				int numTargetPoints = pointSF.NumShapes;
				MapWinGIS.Point targetPoint = new MapWinGIS.PointClass();
				//MapWinGIS.Utils utils = new MapWinGIS.UtilsClass();
				int numParts = polygon.NumParts;
				if(numParts == 0)
				{
					numParts = 1;
				}
				Globals.Vertex[][] polyVertArray = new Globals.Vertex[numParts][];
				Globals.ConvertPolyToVertexArray(ref polygon, out polyVertArray);
                
                for(int i = 0; i <= numTargetPoints-1; i++)
				{
					targetPoint = pointSF.QuickPoint(i, 0);

					if(Utils.PointInPoly(ref polyVertArray, ref targetPoint) == true)
					{
						resultSF.EditInsertShape(pointSF.get_Shape(i), ref shpIndex);
                        if (copyAttributes)
                        {
                            for (int f = 0; f <= pointSF.NumFields-1;f++)
                            {
                                bool tmpbool = resultSF.EditCellValue(f, shpIndex, pointSF.get_CellValue(f, i));
                            }
                        }
					}
				}

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
                
                int numIDs = resultSF.NumShapes;
                for (int i = 0; i <= numIDs - 1; i++)
                {
                    if (resultSF.EditCellValue(fieldIndex, i, i) == false)
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
					gErrorMsg = "Problem with StopEditingShapes: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
					Debug.WriteLine(gErrorMsg);
					Error.SetErrorMsg(gErrorMsg);
                    MapWinUtility.Logger.Dbg(gErrorMsg);
					return false;
				}

                resultSF.Close();
				pointSF.Close();
                MapWinUtility.Logger.Dbg("Finished ClipPointSFWithPolygon");
			}
			else
			{
				pointSF.Close();
				gErrorMsg = "Shapefile type is incorrect. Must be of type Point.";
				Debug.WriteLine(gErrorMsg);
				MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                MapWinUtility.Logger.Dbg(gErrorMsg);
				return false;
			}
			return true;
		}
		#endregion

		#region Save-to-Memory version
		/// <summary>
		/// Returns an in-memory shapefile of points from the input shapefile that fall within the polygon.
		/// </summary>
		/// <param name="pointSF">Full path to the point shapefile.</param>
		/// <param name="polygon">The polygon used for clipping the point shapefile.</param>
		/// <param name="result">Full path to where the resulting point shapefile should be saved.</param>
        /// <param name="copyAttributes">True if copying attributes over</param>
        /// <returns>False if an error was encountered, true otherwise.</returns>
		public static bool ClipPointSFWithPolygon(ref MapWinGIS.Shapefile pointSF, ref MapWinGIS.Shape polygon, out MapWinGIS.Shapefile result, bool copyAttributes)
		{
            MapWinUtility.Logger.Dbg("ClipPointSFWithPolygon(pointSF: " + Macro.ParamName(pointSF) + ",\n" +
                                     "                       polygon: " + Macro.ParamName(polygon) + ",\n" +
                                     "                       result: out, \n" +
                                     "                       copyAttributes: " + copyAttributes + ",\n");

		    MapWinGIS.Shapefile resultSF = new MapWinGIS.ShapefileClass();

		    int shpIndex = 0; //all new shapes will be placed at the beginning of the shapefile
			MapWinGIS.ShpfileType sfType = pointSF.ShapefileType;
		
			//make sure we are dealing with a valid shapefile type
			if(sfType == MapWinGIS.ShpfileType.SHP_POINT || sfType == MapWinGIS.ShpfileType.SHP_POINTM || sfType == MapWinGIS.ShpfileType.SHP_POINTZ)
			{	
				string tempPath = System.IO.Path.GetTempPath() + "resultSF.shp";
				DataManagement.DeleteShapefile(ref tempPath);
                string tmpName;
				//create the result shapeFile if it does not already exist
                //if(resultSF.CreateNew(tempPath, sfType) == false)
                //{
                //    gErrorMsg = "Problem creating the result shapeFile: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
                //    Debug.WriteLine(gErrorMsg);
                //    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                //    result = resultSF;
                //    return false;
                //}
                //CDM 8/4/2006 resultSF.CreateNew(resultSFPath, sfType);
                Globals.PrepareResultSF(ref tempPath, ref resultSF, sfType);

                if (copyAttributes)
                {
                    MapWinGIS.Field tmpField, pointField;
                    for (int f = 0; f <= pointSF.NumFields - 1; f++)
                    {
                        tmpField = new MapWinGIS.Field();
                        pointField = pointSF.get_Field(f);
                        tmpName = pointField.Name;
                        if (tmpName.Contains("MWShapeID"))
                        {
                            tmpField.Name = "Last_" + tmpName;
                        }
                        else
                        {
                            tmpField.Name = tmpName;
                        }
                        tmpField.Width = pointField.Width;
                        tmpField.Type = pointField.Type;
                        tmpField.Precision = pointField.Precision;
                        tmpField.Key = pointField.Key;
                        resultSF.EditInsertField(tmpField, ref f, null);
                    }
                }

				int numTargetPoints = pointSF.NumShapes;
				MapWinGIS.Point targetPoint = new MapWinGIS.PointClass();
				//MapWinGIS.Utils utils = new MapWinGIS.UtilsClass();
				int numParts = polygon.NumParts;
				if(numParts == 0)
				{
					numParts = 1;
				}
				Globals.Vertex[][] polyVertArray = new Globals.Vertex[numParts][];
				Globals.ConvertPolyToVertexArray(ref polygon, out polyVertArray);

				for(int i = 0; i <= numTargetPoints-1; i++)
				{
					targetPoint = pointSF.QuickPoint(i, 0);
			
					if(Utils.PointInPoly(ref polyVertArray, ref targetPoint) == true)
					{
						resultSF.EditInsertShape(pointSF.get_Shape(i), ref shpIndex);
                        if (copyAttributes)
                        {
                            for (int f = 0; f <= pointSF.NumFields - 1; f++)
                            {
                                bool tmpbool = resultSF.EditCellValue(f, shpIndex, pointSF.get_CellValue(f, i));
                            }
                        }
					}
				}
			}			
			else
			{
				gErrorMsg = "The shapefile is of the wrong type. Should be of type Point.";
				Debug.WriteLine(gErrorMsg);
				MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
				result = resultSF;
                MapWinUtility.Logger.Dbg(gErrorMsg);
				return false;
			}

			result = resultSF;

            // Paul Meems 11 March 2011 - Give the result sf the same projections as the input:
		    result.Projection = pointSF.Projection;
            MapWinUtility.Logger.Dbg("Finished ClipPointSFWithPolygon");
			return true;
		}
		#endregion

		#endregion

	}
}
