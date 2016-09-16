//********************************************************************************************************
//File name: Merge.cs
//Description: Internal class, provides methods for merging two shapes of the same type into one.
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
//2-27-06 ah - Angela Hillier - Created MergeShapes() and support functions.							
//********************************************************************************************************
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using MapWindow.Interfaces.Geometries;
namespace MapWinGeoProc
{
	/// <summary>
	/// Functions for merging two shapes of the same type into one (if possible).
	/// </summary>
	internal class Merge
	{
		public static string gErrorMsg = "";

        
		#region MergeShapes()
		/// <summary>
		/// Merges two lines (at matching end points) or two polygons 
		/// (by dissolving the common border or by combining into a multi-part polygon) 
		/// to make one result shape.
		/// This version requires that both shapes be located in the same shapefile.
		/// </summary>
		/// <param name="shapes">The shapefile containing the two shapes to be merged.</param>
		/// <param name="indexOne">The index of the first shape.</param>
		/// <param name="indexTwo">The index of the second shape.</param>
		/// <param name="resultShp">The result of merging the shapes at indexOne and indexTwo together.</param>
		/// <returns>True if the shapes were merged, false otherwise.</returns>
		public static bool MergeShapes(ref MapWinGIS.Shapefile shapes, int indexOne, int indexTwo, out MapWinGIS.Shape resultShp)
		{
            MapWinUtility.Logger.Dbg("MergeShapes(shapes: " + Macro.ParamName(shapes) + ",\n" +
                                     "            indexOne: " + indexOne.ToString() + ",\n" +
                                     "            indexTwo: " + indexTwo.ToString() + ",\n" +
                                     "            resultShp: resultShp)");
			Error.ClearErrorLog();
			MapWinGIS.ShpfileType sfType = shapes.ShapefileType;
			MapWinGIS.Shape mShp = new MapWinGIS.ShapeClass();
			mShp.Create(sfType);
			bool status = false;
			
			//having a point merge function is quite pointless...but, just in case it's needed!
			if(sfType == MapWinGIS.ShpfileType.SHP_POINT || sfType == MapWinGIS.ShpfileType.SHP_POLYGONM || sfType == MapWinGIS.ShpfileType.SHP_POLYGONZ)
			{
				MapWinGIS.Point pt1 = new MapWinGIS.PointClass();
				pt1 = shapes.QuickPoint(indexOne, 0);
				MapWinGIS.Point pt2 = new MapWinGIS.PointClass();
				pt2 = shapes.QuickPoint(indexTwo, 0);

				status = MergePoints(ref pt1, ref pt2, sfType, out mShp);

                //while(Marshal.ReleaseComObject(pt1) != 0);
				pt1 = null;
                //while(Marshal.ReleaseComObject(pt2) != 0);
				pt2 = null;

				resultShp = mShp;
                MapWinUtility.Logger.Dbg("Finished MergeShapes");
				return status;
			}//end of merging points

				//merge polygons
			else if(sfType == MapWinGIS.ShpfileType.SHP_POLYGON || sfType == MapWinGIS.ShpfileType.SHP_POLYGONM || sfType == MapWinGIS.ShpfileType.SHP_POLYGONZ)
			{
				MapWinGIS.Shape poly1 = new MapWinGIS.ShapeClass();
				poly1 = shapes.get_Shape(indexOne);
				MapWinGIS.Shape poly2 = new MapWinGIS.ShapeClass();
				poly2 = shapes.get_Shape(indexTwo);

				status = MergePolygons(ref poly1, ref poly2, sfType, out mShp);

                //while(Marshal.ReleaseComObject(poly1) != 0);
				poly1 = null;
                //while(Marshal.ReleaseComObject(poly2) != 0);
				poly2 = null;

				resultShp = mShp;
                MapWinUtility.Logger.Dbg("Finished MergeShapes");
				return status;
			}//end of merging polygons

				//Merge lines by joining them at common endpoints
			else if(sfType == MapWinGIS.ShpfileType.SHP_POLYLINE || sfType == MapWinGIS.ShpfileType.SHP_POLYLINEM || sfType == MapWinGIS.ShpfileType.SHP_POLYLINEZ)
			{
				MapWinGIS.Shape line1 = new MapWinGIS.ShapeClass();
				line1 = shapes.get_Shape(indexOne);
				MapWinGIS.Shape line2 = new MapWinGIS.ShapeClass();
				line2 = shapes.get_Shape(indexTwo);

				status = MergeLines(ref line1, ref line2, sfType, out mShp);
                //if (line1 != null)
                //{
                //    while (Marshal.ReleaseComObject(line1) != 0) ;
                //}
                //line1 = null;
                //if (line2 != null)
                //{
                //    while (Marshal.ReleaseComObject(line2) != 0) ;
                //}
                //line2 = null;

				resultShp = mShp;
                MapWinUtility.Logger.Dbg("Finished MergeShapes");
				return status;

			}//end of merging lines
			else
			{
				gErrorMsg = "Unknown shapefile type, aborting call to ShapeMerge.";
				Debug.WriteLine(gErrorMsg);
				Error.SetErrorMsg(gErrorMsg);
				resultShp = mShp;
                MapWinUtility.Logger.Dbg(gErrorMsg);
				return false;
			}
		}
		#endregion

		#region private MergePoints()
		/// <summary>
		/// Will check to see if two points are identical.
		/// </summary>
		/// <param name="point1">The first point to consider.</param>
		/// <param name="point2">The second point to consider.</param>
		/// <param name="sfType">The type of shapefile the points belong to.</param>
		/// <param name="mergedShp">The result shape</param>
		/// <returns>True if the points were merged successfully, false otherwise.</returns>
		private static bool MergePoints(ref MapWinGIS.Point point1, ref MapWinGIS.Point point2, MapWinGIS.ShpfileType sfType, out MapWinGIS.Shape mergedShp)
		{
            MapWinUtility.Logger.Dbg("MergePoints(point1: " + Macro.ParamName(point1) + "\n," +
                                     "            point2: " + Macro.ParamName(point2) + "\n," + 
                                     "            sfType: " + sfType.ToString() + "\n," + 
                                     "            mergedShp: out)");

			MapWinGIS.Shape mShp = new MapWinGIS.ShapeClass();			
			if(point1.x == point2.x && point1.y == point2.y && point1.Z == point2.Z)
			{
				mShp.Create(sfType);
				int ptIndex = 0;
				mShp.InsertPoint(point1, ref ptIndex);
			}
			mergedShp = mShp;
			if(mergedShp.numPoints > 0)
			{
                MapWinUtility.Logger.Dbg("Finished MergePoints");
				return true;
			}
			else
			{
				gErrorMsg = "The points are not identical so they cannot be merged.";
				Debug.WriteLine(gErrorMsg);
				Error.SetErrorMsg(gErrorMsg);
				mergedShp = mShp;
                MapWinUtility.Logger.Dbg(gErrorMsg);
				return false;
			}
		}
		#endregion

		#region private MergeLines()
		/// <summary>
		/// Merges two lines together if they share a common end point.
		/// </summary>
		/// <param name="line1">The first line to be considered.</param>
		/// <param name="line2">The second line to be considered.</param>
		/// <param name="sfType">The type of shapefile the lines are a part of.</param>
		/// <param name="resultShp">The two lines merged into one multipart line</param>
		/// <returns>True if merging was successful, false otherwise.</returns>
		private static bool MergeLines(ref MapWinGIS.Shape line1, ref MapWinGIS.Shape line2, MapWinGIS.ShpfileType sfType, out MapWinGIS.Shape resultShp)
		{
			MapWinGIS.Shape mShp = new MapWinGIS.ShapeClass();
			mShp.Create(sfType);
            //int pt = 0;
            //int prt = 0;
            if (line1 == null && line2 == null)
            {
                resultShp = mShp;
                return false;
            }
            if (line1 == null && line2 != null)
            {
                resultShp = line2;
                return false;
            }
            if (line2 == null)
            {
                resultShp = line1;
                return false;
            }

            Topology2D.MultiLineString MLS1 = new MapWinGeoProc.Topology2D.MultiLineString(line1);
            Topology2D.MultiLineString MLS2 = new MapWinGeoProc.Topology2D.MultiLineString(line2);
            Topology2D.LineString LS2;
            // Join the linestrings from MLS1 where possible
            int j = 0;
            while ( j < MLS1.NumGeometries)
            {
                LS2 = MLS1.GeometryN[j] as Topology2D.LineString;
                Topology2D.LineString LS1;
                Topology2D.Geometry outGeom;
                for (int i = 0; i < MLS1.NumGeometries; i++)
                {
                    if (i == j) continue;
                    LS1 = MLS1.GeometryN[i] as Topology2D.LineString;
                    outGeom = LS1.Union(LS2);
                    // Attempt to join each linestring to linestrings in MLS1
                    if(outGeom != null)
                    {

                        MLS1.GeometryN[i] = outGeom;
                        MLS1.GeometryN.RemoveAt(j);
                        continue;
                        // don't increment j because we removed one instead
                    }
                }
                j++;
            }

            // Add each linestring to MLS1, merging into single linestrings where possible
            while(MLS2.NumGeometries > 0)
            {
                LS2 = MLS2.GeometryN[0] as Topology2D.LineString;
                Topology2D.LineString LS1;
                Topology2D.Geometry outGeom;
                bool merged = false;
                for (int i = 0; i < MLS1.NumGeometries; i++)
                {
                    LS1 = MLS1.GeometryN[i] as Topology2D.LineString;
                    outGeom = LS1.Union(LS2);

                    // Attempt to join each linestring to linestrings in MLS1
                    if(outGeom != null)
                    {
                        // The endpoint merge was successful
                        
                        MLS1.GeometryN[i] = outGeom;
                        merged = true;
                        break;
                    }
                    
                }
                // If they don't connect, add the linestring to the end of the list
                if(merged == false)
                {
                        // endPoint merging was not successful
                        MLS1.GeometryN.Add(LS2);
                }
                // either way, remove the line
                MLS2.GeometryN.RemoveAt(0);
            }
            resultShp = Topology2D.GeometryFactory.mwShapeFromMultiLineString(MLS1, sfType);
            return true;
		}


        #endregion

        #region private MergePolygons()
        /// <summary>
        /// Merges two polygons together. If they touch, they will be UNIONed together, 
        /// If they do not touch, they will be combined into one multi-part shape.
        /// </summary>
        /// <param name="poly1">The first polygon to be considered.</param>
        /// <param name="poly2">The second polygon to be considered.</param>
        /// <param name="sfType">The type of shapefile the polygons are a part of.</param>
        /// <param name="resultShp">The polygon shape that results after merging.</param>
        /// <returns>True if merging was successful, false otherwise.</returns>
        private static bool MergePolygons(ref MapWinGIS.Shape poly1, ref MapWinGIS.Shape poly2, MapWinGIS.ShpfileType sfType, out MapWinGIS.Shape resultShp)
		{
			MapWinGIS.Shape mShp = new MapWinGIS.ShapeClass();
			mShp.Create(sfType);
			
			if(Globals.CheckBounds(ref poly1, ref poly2))
			{
				//bounds overlap, try union
				Debug.WriteLine("Union operation is being performed.");
				mShp = SpatialOperations.Union(poly1, poly2);				
                // Paul Meems: Related to bug 1068. Added check for correct shape:
                if (mShp != null)
                {
                    if (mShp.numPoints != 0 && mShp.IsValid)
                    {
					    resultShp = mShp;
					    return true;
                    }
                    else
                    {
                        gErrorMsg = "SpatialOperations.Intersection returned an invalid shape";
                        Debug.WriteLine(gErrorMsg);
                        Error.SetErrorMsg(gErrorMsg);
                        MapWinUtility.Logger.Dbg(gErrorMsg);
                        resultShp = poly1;
                        return false;
                    }
                }
				else
				{
					//even though bounds overlap, the polygons may not touch
					//combine them into a multi-part polygon.
					bool status = CombinePolygons(ref poly1, ref poly2, out mShp);
					resultShp = mShp;
					return status;
				}
			}
			else
			{
				//Polygons don't overlap, try combining them into a multi-part polygon
                gErrorMsg = "Combining polygons into one multi-part shape.";
				Debug.WriteLine(gErrorMsg);
				bool status = CombinePolygons(ref poly1, ref poly2, out mShp);
				resultShp = mShp;
                MapWinUtility.Logger.Dbg(gErrorMsg);
				return status;
			}

		}
		#endregion

      
        #region private CombinePolygons() -- used by MergePolygons()
        /// <summary>
		/// Creates one multi-part polygon out of two input polygons.
		/// </summary>
		/// <param name="poly1">The first polygon to be combined.</param>
		/// <param name="poly2">The second polygon to be combined.</param>
		/// <param name="resultShp">The resulting multi-part polygon.</param>
		/// <returns>True if combining was successful, false otherwise.</returns>
		private static bool CombinePolygons(ref MapWinGIS.Shape poly1, ref MapWinGIS.Shape poly2, out MapWinGIS.Shape resultShp)
		{
			int p1NumParts = poly1.NumParts;
			if(p1NumParts == 0)
			{
				p1NumParts = 1;
			}
			int p2NumParts = poly2.NumParts;
			if(p2NumParts == 0)
			{
				p2NumParts = 1;
			}

			MapWinGIS.Shape multiShp = new MapWinGIS.ShapeClass();
			multiShp.Create(poly1.ShapeType);
			int partIndex = 0;
			int pointIndex = 0;
			int numPoints = poly1.numPoints;
			int begPart = 0;
			int endPart = numPoints;

			//deal with the first shape and all of its parts
			//			Globals.Vertex[][] p1VertArray = new Globals.Vertex[p1NumParts][];
			//			Globals.ConvertPolyToVertexArray(ref poly1, out p1VertArray);
			//bool firstIsClockwise = Globals.IsClockwise(ref p1VertArray[0]);

			for(int i = 0; i <= p1NumParts-1; i++)
			{
				partIndex = i;
				pointIndex = multiShp.numPoints;
				multiShp.InsertPart(pointIndex, ref partIndex);

				begPart = poly1.get_Part(i);
				if(i < p1NumParts-1)
				{
					endPart = poly1.get_Part(i+1);
				}
				else
				{
					endPart = poly1.numPoints;
				}
				//				if(firstIsClockwise)
				//				{
				//add part
				for(int j = begPart; j <= endPart-1; j++)
				{
					pointIndex = multiShp.numPoints;
					multiShp.InsertPoint(poly1.get_Point(j), ref pointIndex);
				}
				//				}
				//				else
				//				{
				//					//add part in reverse order
				//					for(int j = endPart-1; j >= begPart; j--)
				//					{
				//						pointIndex = multiShp.numPoints;
				//						multiShp.InsertPoint(poly1.get_Point(j), ref pointIndex);
				//					}
				//				}				
			}//end of adding poly1 and all of its parts to the result shape

			//deal with the second shape and all of its parts
			//			Globals.Vertex[][] p2VertArray = new Globals.Vertex[p2NumParts][];
			//			Globals.ConvertPolyToVertexArray(ref poly2, out p2VertArray);
			//			bool secondIsClockwise = Globals.IsClockwise(ref p2VertArray[0]);
			partIndex++;
			numPoints = poly2.numPoints;
			begPart = 0;
			endPart = numPoints;

			for(int i = 0; i <= p2NumParts-1; i++)
			{
				partIndex += i;
				pointIndex = multiShp.numPoints;
				multiShp.InsertPart(pointIndex, ref partIndex);

				begPart = poly2.get_Part(i);
				if(i < p2NumParts-1)
				{
					endPart = poly2.get_Part(i+1);
				}
				else
				{
					endPart = poly2.numPoints;
				}

				//				if(secondIsClockwise)
				//				{
				for(int j = begPart; j <= endPart-1; j++)
				{
					pointIndex = multiShp.numPoints;
					multiShp.InsertPoint(poly2.get_Point(j), ref pointIndex);
				}
				//				}
				//				else
				//				{
				//					for(int j = endPart-1; j >= begPart; j--)
				//					{
				//						pointIndex = multiShp.numPoints;
				//						multiShp.InsertPoint(poly2.get_Point(j), ref pointIndex);
				//					}
				//				}
			}//end of inserting parts from the second shape

			resultShp = multiShp;
			if(resultShp.numPoints > 0)
			{
				return true;
			}
			else
			{
				gErrorMsg = "Error occured while trying to combine parts. No points in result shape.";
				Debug.WriteLine(gErrorMsg);
				Error.SetErrorMsg(gErrorMsg);
                MapWinUtility.Logger.Dbg(gErrorMsg);
				return false;
			}
		}
		#endregion

	}
}
