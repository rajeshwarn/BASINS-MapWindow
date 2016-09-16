//********************************************************************************************************
//File name: Buffer.cs
//Description: Internal class for buffering points, lines, polygons and shapefiles.
//********************************************************************************************************
//The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
//you may not use this file except in compliance with the License. You may obtain a copy of the License at 
//http://www.mozilla.org/MPL/ 
//Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
//ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
//limitations under the License. 
//
//The Original Code is MapWindow Open Source. 
//
//Contributor(s): (Open source contributors should list themselves and their modifications here). 
//3-30-06 Angela Hillier wrote original BufferPoint(), BufferLine(), BufferPolygon(),
//			BufferPointSF(), BufferLineSF(), BufferPolygonSF() and all supporting private functions.
//********************************************************************************************************
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MapWinGeoProc
{
	/// <summary>
	/// Summary description for BufferPoly.
	/// </summary>
	internal class Buffer
	{
		private static string gErrorMsg = "";

		#region BufferPoint()
		//Angela Hillier 3/15/06
		/// <summary>
		/// Creates a circle (buffer) around a point.
		/// </summary>
		/// <param name="point">The point to be buffered.</param>
		/// <param name="radius">The distance (in units) from the point that the circle's boundary should exist at.</param>
		/// <param name="numQuadrants">The smoothness of the circle. Smaller value = smoother circle.</param>
		/// <param name="resultShp">The resulting buffer.</param>
		/// <returns>False if an error was encountered, true otherwise.</returns>
		public static bool BufferPoint(ref MapWinGIS.Point point, double radius, int numQuadrants, out MapWinGIS.Shape resultShp)
		{
            MapWinUtility.Logger.Dbg("BufferPoint(point: " + Macro.ParamName(point) + ",\n" +
                                     "            radius: " + radius.ToString() + ",\n" +
                                     "            numQuadrants: " + numQuadrants.ToString() + ",\n" +
                                     "            resultShp: (out))");
			MapWinGIS.Shape circle = new MapWinGIS.ShapeClass();
			circle.Create(MapWinGIS.ShpfileType.SHP_POLYGON);
			int ptIndex = 0;

			if(radius <= 0)
			{
				gErrorMsg = "Negative radius not allowed when buffering a point.";
				Error.SetErrorMsg(gErrorMsg);
				Debug.WriteLine(gErrorMsg);
				resultShp = circle;
                MapWinUtility.Logger.Dbg(gErrorMsg);
				return false;
			}
			if(numQuadrants < 1)
			{
				numQuadrants = 8;
			}

			//Create the points that will go inside the circle.
			//Use numQuadrants to define how smooth the circle will be.
			for(int i = 360; i > 0; i -= numQuadrants)
			{
				MapWinGIS.Point circlePt = new MapWinGIS.PointClass();
				circlePt.x = (Math.Cos(i * Math.PI/180) * radius) + point.x;
				circlePt.y = (Math.Sin(i * Math.PI/180) * radius) + point.y;
				ptIndex = circle.numPoints;
				circle.InsertPoint(circlePt, ref ptIndex);
			}
			//add the first point to the end of the circle shape to close off the polygon
			ptIndex = circle.numPoints;
			circle.InsertPoint(circle.get_Point(0), ref ptIndex);
			resultShp = circle;
            MapWinUtility.Logger.Dbg("Finished BufferPoint");
			return true;
		}
		#endregion

		#region BufferLine()
		//Angela Hillier 3/17/06
		/// <summary>
		/// Creates a buffer around a line shape.
		/// </summary>
		/// <param name="line">The line to be buffered.</param>
		/// <param name="distance">Distance (in units) from the line at which the buffer should be created.</param>
		/// <param name="buffSide">Which side of the line to buffer. 0 = both, 1 = left, 2 = right.</param>
		/// <param name="capStyle">Edge treatement. 0 = pointed, 1 = rounded.</param>
		/// <param name="endCapStyle">End treatment. 0 = pointed, 1 = rounded, 2 = closed (for polygons only!)</param>
		/// <param name="numQuadrants">Smoothness of rounded caps. Smaller value = smoother circle.</param>
		/// <param name="resultShp">The resulting polygon buffer.</param>
		/// <returns>False if an error was encountered, true otherwise.</returns>
		public static bool BufferLine(ref MapWinGIS.Shape line, double distance, Enumerations.Buffer_LineSide buffSide, Enumerations.Buffer_CapStyle capStyle, Enumerations.Buffer_EndCapStyle endCapStyle, int numQuadrants, out MapWinGIS.Shape resultShp)
		{
            MapWinUtility.Logger.Dbg("BufferLine(line: " + Macro.ParamName(line) + ",\n" + 
                                     "distance: " + distance.ToString() + "\n," + 
                                     "buffSide: " + buffSide.ToString() + "\n," + 
                                     "capStyle: " + capStyle.ToString() + "\n," + 
                                     "endCapStyle: " + endCapStyle.ToString() + "\n," +
                                     "numQuadrants: " + numQuadrants.ToString() + "\n," +
                                     "resultShp: (out))");

			MapWinGIS.Shape buffResult = new MapWinGIS.ShapeClass();
			buffResult.Create(MapWinGIS.ShpfileType.SHP_POLYGON);

			if(distance <= 0)
			{
				gErrorMsg = "Negative or zero distance not allowed when buffering a line.";
				Error.SetErrorMsg(gErrorMsg);
				Debug.WriteLine(gErrorMsg);
				resultShp = buffResult;
                MapWinUtility.Logger.Dbg(gErrorMsg);
				return false;
			}
			if(numQuadrants < 1)
			{
				numQuadrants = 8;
			}
            MapWinUtility.Logger.Dbg("Before RemoveDuplicatePts: numPoints: " + line.numPoints.ToString());
			Globals.RemoveDuplicatePts(ref line);
            MapWinUtility.Logger.Dbg("After RemoveDuplicatePts: numPoints: " + line.numPoints.ToString());
			int numPts = line.numPoints;
			int numEdges = numPts-1;
			int ptIndex = 0;
			MapWinGIS.Point capPt1 = new MapWinGIS.PointClass();
			MapWinGIS.Point capPt2 = new MapWinGIS.PointClass();
			MapWinGIS.Shape capRnd1 = new MapWinGIS.ShapeClass();
			capRnd1.Create(MapWinGIS.ShpfileType.SHP_POLYLINE);
			MapWinGIS.Shape capRnd2 = new MapWinGIS.ShapeClass();
			capRnd2.Create(MapWinGIS.ShpfileType.SHP_POLYLINE);
			MapWinGIS.Shape[] bufferShapes = new MapWinGIS.Shape[numEdges];
			MapWinGIS.Point currPt = new MapWinGIS.PointClass();
			MapWinGIS.Point nextPt = new MapWinGIS.PointClass();
		
			for(int i = 0; i <= numPts-2; i++)
			{
				bufferShapes[i] = new MapWinGIS.ShapeClass();
				bufferShapes[i].Create(MapWinGIS.ShpfileType.SHP_POLYGON);

				currPt = new MapWinGIS.PointClass();
				currPt = line.get_Point(i);
				nextPt = new MapWinGIS.PointClass();
				nextPt = line.get_Point(i+1);

				double deltaX = nextPt.x - currPt.x;
				double deltaY = nextPt.y - currPt.y;

				#region Create Parallel Line Segments

				MapWinGIS.Point pt0 = new MapWinGIS.PointClass();
				MapWinGIS.Point pt1 = new MapWinGIS.PointClass();
				MapWinGIS.Point pt2 = new MapWinGIS.PointClass();
				MapWinGIS.Point pt3 = new MapWinGIS.PointClass();
				
				double leftTheta = FindPerpendicularDegree(Enumerations.Buffer_LineSide.Left, deltaX, deltaY);
				double leftXChange = distance * Math.Cos(leftTheta * Math.PI/180);
				double leftYChange = distance * Math.Sin(leftTheta * Math.PI/180);
				
				double rightTheta = FindPerpendicularDegree(Enumerations.Buffer_LineSide.Right, deltaX, deltaY);
				double rightXChange = distance * Math.Cos(rightTheta * Math.PI/180);
				double rightYChange = distance * Math.Sin(rightTheta * Math.PI/180);

				if(buffSide == Enumerations.Buffer_LineSide.Left)//buffer left side
				{
					pt0.x = currPt.x;
					pt0.y = currPt.y;
					pt1.x = currPt.x + leftXChange;
					pt1.y = currPt.y + leftYChange;
					pt2.x = nextPt.x + leftXChange;
					pt2.y = nextPt.y + leftYChange;
					pt3.x = nextPt.x;
					pt3.y = nextPt.y;
				}
				else if(buffSide == Enumerations.Buffer_LineSide.Right)//buffer right side
				{
					pt0.x = currPt.x + rightXChange;
					pt0.y = currPt.y + rightYChange;
					pt1.x = currPt.x;
					pt1.y = currPt.y;
					pt2.x = nextPt.x;
					pt2.y = nextPt.y;
					pt3.x = nextPt.x + rightXChange;
					pt3.y = nextPt.y + rightYChange;
				}
				else //buffer both sides
				{
					//Left side is represented by pt1 and pt2;
					//Right side is represent by pt0 and pt3;
					//This guarantees that buffer polygons are clockwise in direction.
					pt0.x = currPt.x + rightXChange;
					pt0.y = currPt.y + rightYChange;
					pt1.x = currPt.x + leftXChange;
					pt1.y = currPt.y + leftYChange;
					pt2.x = nextPt.x + leftXChange;
					pt2.y = nextPt.y + leftYChange;
					pt3.x = nextPt.x + rightXChange;
					pt3.y = nextPt.y + rightYChange;
				}			

				ptIndex = 0;
				bufferShapes[i].InsertPoint(pt0, ref ptIndex);
				ptIndex++;
				bufferShapes[i].InsertPoint(pt1, ref ptIndex);
				ptIndex++;
				bufferShapes[i].InsertPoint(pt2, ref ptIndex);
				ptIndex++;
				bufferShapes[i].InsertPoint(pt3, ref ptIndex);
				ptIndex++;
				bufferShapes[i].InsertPoint(pt0, ref ptIndex);
				#endregion

				#region Create End Caps
				if(i == 0 || i == numPts-2)
				{
					if(endCapStyle == Enumerations.Buffer_EndCapStyle.Pointed)//find points for pointed end caps
					{
						double capXchange, capYchange;
						if(i == 0)
						{
							if(buffSide != Enumerations.Buffer_LineSide.Both)//only buffering one side
							{
								MapWinGIS.Point midPt = new MapWinGIS.PointClass();
								MapWinGIS.Point firstPt = new MapWinGIS.PointClass();
								firstPt = bufferShapes[0].get_Point(0);
								MapWinGIS.Point secondPt = new MapWinGIS.PointClass();
								secondPt = bufferShapes[0].get_Point(1);
								midPt.x = ((secondPt.x - firstPt.x)/2) + firstPt.x;
								midPt.y = ((secondPt.y - firstPt.y)/2) + firstPt.y;
								FindCapChange(0, rightTheta, (distance/2), deltaX, deltaY, out capXchange, out capYchange);
								capPt1.x = midPt.x + capXchange;
								capPt1.y = midPt.y + capYchange;
							}
							else //full buffer
							{
								FindCapChange(0, rightTheta, distance, deltaX, deltaY, out capXchange, out capYchange);
								capPt1.x = currPt.x + capXchange;
								capPt1.y = currPt.y + capYchange;
							}
						}
						if(i == numPts-2)
						{
							if(buffSide != Enumerations.Buffer_LineSide.Both) //only buffering one side
							{
								MapWinGIS.Point midPt = new MapWinGIS.PointClass();							
								MapWinGIS.Point firstPt = new MapWinGIS.PointClass();
								firstPt = bufferShapes[numPts-2].get_Point(2);
								MapWinGIS.Point secondPt = new MapWinGIS.PointClass();
								secondPt = bufferShapes[numPts-2].get_Point(3);
								midPt.x = ((secondPt.x - firstPt.x)/2) + firstPt.x;
								midPt.y = ((secondPt.y - firstPt.y)/2) + firstPt.y;
								FindCapChange(1, rightTheta, (distance/2), deltaX, deltaY, out capXchange, out capYchange);
								capPt2.x = midPt.x + capXchange;
								capPt2.y = midPt.y + capYchange;
							}
							else//full buffer
							{
								FindCapChange(1, rightTheta, distance, deltaX, deltaY, out capXchange, out capYchange);
								capPt2.x = nextPt.x + capXchange;
								capPt2.y = nextPt.y + capYchange;
							}
						}
					}//end of forming pointed caps
					else if(endCapStyle == Enumerations.Buffer_EndCapStyle.Rounded)//form half-circles for rounded end caps
					{
						double startDegree, endDegree;
						if(i == 0)
						{
							startDegree = rightTheta;
							endDegree = startDegree - 180;

							if(buffSide != Enumerations.Buffer_LineSide.Both)//only buffering one side
							{
								Debug.WriteLine("StartDegree = " + startDegree + " EndDegree = " + endDegree);
								MapWinGIS.Point midPt = new MapWinGIS.PointClass();
								MapWinGIS.Point firstPt = new MapWinGIS.PointClass();
								firstPt = bufferShapes[0].get_Point(0);
								MapWinGIS.Point secondPt = new MapWinGIS.PointClass();
								secondPt = bufferShapes[0].get_Point(1);
								midPt.x = ((secondPt.x - firstPt.x)/2) + firstPt.x;
								midPt.y = ((secondPt.y - firstPt.y)/2) + firstPt.y;
							
								for(double j = startDegree; j > endDegree; j-= numQuadrants)
								{
									MapWinGIS.Point circlePt = new MapWinGIS.PointClass();
									circlePt.x = (Math.Cos(j * Math.PI/180) * (distance/2)) + midPt.x;
									circlePt.y = (Math.Sin(j * Math.PI/180) * (distance/2)) + midPt.y;
									ptIndex = capRnd1.numPoints;
									capRnd1.InsertPoint(circlePt, ref ptIndex);
								}
							}
							else//full buffer
							{
								for(double j = startDegree; j > endDegree; j-= numQuadrants)
								{
									MapWinGIS.Point circlePt = new MapWinGIS.PointClass();
									circlePt.x = (Math.Cos(j * Math.PI/180) * distance) + currPt.x;
									circlePt.y = (Math.Sin(j * Math.PI/180) * distance) + currPt.y;
									ptIndex = capRnd1.numPoints;
									capRnd1.InsertPoint(circlePt, ref ptIndex);
								}
							}
						}//end of first cap
						if(i == numPts-2)
						{
							startDegree = leftTheta;
							endDegree = startDegree - 180;

							if(buffSide != Enumerations.Buffer_LineSide.Both)//only buffering one side
							{
								Debug.WriteLine("StartDegree = " + startDegree + " EndDegree = " + endDegree);
								MapWinGIS.Point midPt = new MapWinGIS.PointClass();							
								MapWinGIS.Point firstPt = new MapWinGIS.PointClass();
								firstPt = bufferShapes[numPts-2].get_Point(2);
								MapWinGIS.Point secondPt = new MapWinGIS.PointClass();
								secondPt = bufferShapes[numPts-2].get_Point(3);
								midPt.x = ((secondPt.x - firstPt.x)/2) + firstPt.x;
								midPt.y = ((secondPt.y - firstPt.y)/2) + firstPt.y;

								for(double j = startDegree; j > endDegree; j-= numQuadrants)
								{
									MapWinGIS.Point circlePt = new MapWinGIS.PointClass();
									circlePt.x = (Math.Cos(j * Math.PI/180) * (distance/2)) + midPt.x;
									circlePt.y = (Math.Sin(j * Math.PI/180) * (distance/2)) + midPt.y;
									ptIndex = capRnd2.numPoints;
									capRnd2.InsertPoint(circlePt, ref ptIndex);
								}
							}
							else//full buffer
							{
								for(double j = startDegree; j > endDegree; j-= numQuadrants)
								{
									MapWinGIS.Point circlePt = new MapWinGIS.PointClass();
									circlePt.x = (Math.Cos(j * Math.PI/180) * distance) + nextPt.x;
									circlePt.y = (Math.Sin(j * Math.PI/180) * distance) + nextPt.y;
									ptIndex = capRnd2.numPoints;
									capRnd2.InsertPoint(circlePt, ref ptIndex);
								}
							}
						}//end of second cap
					}//end of forming rounded caps
				} 
				#endregion

				#region Add Connecting Points
				if(i > 0)
				{
					//check the slopes to see if we need to add any points...
					MapWinGIS.Point prevPt = new MapWinGIS.PointClass();
					prevPt = line.get_Point(i-1);
					double ln1Xdiff = currPt.x - prevPt.x;
					double ln1Ydiff = currPt.y - prevPt.y;
					double ln2Xdiff = nextPt.x - currPt.x;
					double ln2Ydiff = nextPt.y - currPt.y;
					double slope1 = 0;
					if(ln1Xdiff != 0)
					{
						slope1 = ln1Ydiff / ln1Xdiff;
					}
					double slope2 = 0;
					if(ln2Xdiff != 0)
					{
						slope2 = ln2Ydiff / ln2Xdiff;
					}
					if((ln1Xdiff == 0 && ln2Xdiff == 0)||(ln1Ydiff == 0 && ln2Ydiff == 0)) 
					{	
						//both are vertical or both are horizontal
						//no gap exists
					}
					else if(Math.Abs(slope1) == Math.Abs(slope2) && slope1 != 0)
					{
						//both have the same slope (note: one cannot be vertical and the other horizontal)
						//no gap exists
					}
					else if(slope1 == 0 && slope2 == 0)
					{
						double startDegree = 0;
						double endDegree = 0;
						
						if(ln1Xdiff == 0 && ln2Ydiff == 0)//vertical followed by horizontal
						{
							if(ln1Ydiff > 0 && ln2Xdiff > 0)//+,+ gap on left
							{
								startDegree = 180;
								endDegree = 90;
							}
							else if(ln1Ydiff > 0 && ln2Xdiff < 0)//+,- gap on right
							{
								startDegree = 0;
								endDegree = 90;
							}
							else if(ln1Ydiff < 0 && ln2Xdiff < 0) //-,- gap on left
							{
								startDegree = 360;
								endDegree = 270;
							}
							else// -,+ gap on right
							{
								startDegree = 180;
								endDegree = 270;
							}                            
						}
						if(ln1Ydiff == 0 && ln2Xdiff == 0)//horizontal followed by vertical
						{
							if(ln1Xdiff > 0 && ln2Ydiff > 0)//+,+ gap on right
							{
								startDegree = 270;
								endDegree = 360;
							}
							else if(ln1Xdiff > 0 && ln2Ydiff < 0) //+,- gap on left
							{
								startDegree = 90;
								endDegree = 0;
							}
							else if(ln1Xdiff < 0 && ln2Ydiff < 0)//-,- gap on right
							{
								startDegree = 90;
								endDegree = 180;
							}
							else//-,+ gap on left
							{
								startDegree = 270;
								endDegree = 180;
							}
						}
						if(capStyle == Enumerations.Buffer_CapStyle.Rounded)//rounded end caps
						{
							if(startDegree > endDegree) //filling left gap
							{
								if(buffSide == Enumerations.Buffer_LineSide.Both || buffSide == Enumerations.Buffer_LineSide.Left)
								{
									startDegree -= numQuadrants;
									ptIndex = 3;
									for(double j = startDegree; j > endDegree; j -= numQuadrants)
									{
										MapWinGIS.Point circlePt = new MapWinGIS.PointClass();
										circlePt.x = (Math.Cos(j * Math.PI/180) * distance) + currPt.x;
										circlePt.y = (Math.Sin(j * Math.PI/180) * distance) + currPt.y;
										bufferShapes[i-1].InsertPoint(circlePt, ref ptIndex);
										ptIndex++;
									}							
									bufferShapes[i-1].InsertPoint(bufferShapes[i].get_Point(1), ref ptIndex);
								}
							}
							else //filling right gap
							{
								if(buffSide == Enumerations.Buffer_LineSide.Both || buffSide == Enumerations.Buffer_LineSide.Right)
								{
									endDegree -= numQuadrants;
									ptIndex = 3;
									bufferShapes[i-1].InsertPoint(bufferShapes[i].get_Point(0), ref ptIndex);
									ptIndex++;
									for(double j = endDegree; j > startDegree; j -= numQuadrants)
									{
										MapWinGIS.Point circlePt = new MapWinGIS.PointClass();
										circlePt.x = (Math.Cos(j * Math.PI/180) * distance) + currPt.x;
										circlePt.y = (Math.Sin(j * Math.PI/180) * distance) + currPt.y;
										bufferShapes[i-1].InsertPoint(circlePt, ref ptIndex);
										ptIndex++;
									}
								}
							}
						}
						else//pointed end caps
						{
							MapWinGIS.Point leftIntPt = new MapWinGIS.PointClass();
							MapWinGIS.Point rightIntPt = new MapWinGIS.PointClass();

							Globals.Line line1 = new Globals.Line(bufferShapes[i-1].get_Point(1), bufferShapes[i-1].get_Point(2));
							Globals.Line line2 = new Globals.Line(bufferShapes[i].get_Point(2), bufferShapes[i].get_Point(1));
							bool foundPt = Globals.LinesIntersect2D(line1, true, line2, true, out leftIntPt);
							if(foundPt == false)
							{
								Debug.WriteLine("Intersect point not found.");
							}
					
							line1 = new Globals.Line(bufferShapes[i-1].get_Point(0), bufferShapes[i-1].get_Point(3));
							line2 = new Globals.Line(bufferShapes[i].get_Point(3), bufferShapes[i].get_Point(0));
							foundPt = Globals.LinesIntersect2D(line1, true, line2, true, out rightIntPt);
							if(foundPt == false)
							{
								Debug.WriteLine("Intersect point not found.");
							}

							ptIndex = 2;
							bufferShapes[i-1].DeletePoint(ptIndex);
							bufferShapes[i-1].InsertPoint(leftIntPt, ref ptIndex);
							ptIndex = 1;
							bufferShapes[i].DeletePoint(ptIndex);
							bufferShapes[i].InsertPoint(leftIntPt, ref ptIndex);
					
							ptIndex = 3;
							bufferShapes[i-1].DeletePoint(ptIndex);
							bufferShapes[i-1].InsertPoint(rightIntPt, ref ptIndex);
							ptIndex = 0;
							bufferShapes[i].DeletePoint(ptIndex);
							bufferShapes[i].InsertPoint(rightIntPt, ref ptIndex);
							ptIndex = 4;
							bufferShapes[i].DeletePoint(ptIndex);
							bufferShapes[i].InsertPoint(rightIntPt, ref ptIndex);							
						}
					}
					else//the segments have different slopes, expect an intersect point
					{
						MapWinGIS.Point leftIntPt = new MapWinGIS.PointClass();
						
						if(buffSide == Enumerations.Buffer_LineSide.Right)//right side of line is being buffered, already know left intersect
						{
							leftIntPt = currPt; 
						}
						else//the left side is being buffered
						{
							Globals.Line left1 = new Globals.Line(bufferShapes[i-1].get_Point(1), bufferShapes[i-1].get_Point(2));
							Globals.Line left2 = new Globals.Line(bufferShapes[i].get_Point(2), bufferShapes[i].get_Point(1));
						
							bool found = Globals.LinesIntersect2D(left1, true, left2, true, out leftIntPt);

							if(found == false)
							{
								left1 = new Globals.Line(bufferShapes[i-1].get_Point(2), bufferShapes[i-1].get_Point(1));
								found = Globals.LinesIntersect2D(left1, true, left2, true, out leftIntPt);
								if(found == false)
								{
									left2 = new Globals.Line(bufferShapes[i].get_Point(1), bufferShapes[i].get_Point(2));
									found = Globals.LinesIntersect2D(left1, true, left2, true, out leftIntPt);
									if(found == false)
									{
										left1 = new Globals.Line(bufferShapes[i-1].get_Point(1), bufferShapes[i-1].get_Point(2));
										found = Globals.LinesIntersect2D(left1, true, left2, true, out leftIntPt);
										if(found == false)
										{
											Debug.WriteLine("Left intersect not found.");
										}
									}
								}
							}
						}
						
						MapWinGIS.Point rightIntPt = new MapWinGIS.PointClass();
						if(buffSide == Enumerations.Buffer_LineSide.Left)//left side of line is being buffered, already know right intersect
						{
							rightIntPt = currPt;
						}
						else//the right side is being buffered
						{		
							Globals.Line right1 = new Globals.Line(bufferShapes[i-1].get_Point(0), bufferShapes[i-1].get_Point(3));
							Globals.Line right2 = new Globals.Line(bufferShapes[i].get_Point(3), bufferShapes[i].get_Point(0));

							bool found = Globals.LinesIntersect2D(right1, true, right2, true, out rightIntPt);
							if(found == false)
							{
								right1 = new Globals.Line(bufferShapes[i-1].get_Point(3), bufferShapes[i-1].get_Point(0));
								found = Globals.LinesIntersect2D(right1, true, right2, true, out rightIntPt);
								if(found == false)
								{
									right2 = new Globals.Line(bufferShapes[i].get_Point(0), bufferShapes[i].get_Point(3));
									found = Globals.LinesIntersect2D(right1, true, right2, true, out rightIntPt);
									if(found == false)
									{
										right1 = new Globals.Line(bufferShapes[i-1].get_Point(0), bufferShapes[i-1].get_Point(3));
										found = Globals.LinesIntersect2D(right1, true, right2, true, out rightIntPt);
										if(found == false)
										{
											Debug.WriteLine("Right intersect not found.");
										}
									}
								}
							}
						}

						if(capStyle == Enumerations.Buffer_CapStyle.Rounded)//round edges
						{
							//find which edge is the 'outer' edge
							double left1Distance = Globals.PtDistance(bufferShapes[i-1].get_Point(1), bufferShapes[i-1].get_Point(2));
							double leftIntDistance = Globals.PtDistance(bufferShapes[i-1].get_Point(1), leftIntPt);
							if(leftIntDistance > left1Distance)//gap exists, left side is our outer edge
							{
								double startDegree, intDegree, endDegree;
								//start vertex
								double xDiff1 = bufferShapes[i-1].get_Point(2).x - currPt.x;
								double yDiff1 = bufferShapes[i-1].get_Point(2).y - currPt.y;
								//intersect vertex
								double intXDiff = leftIntPt.x - currPt.x;
								double intYDiff = leftIntPt.y - currPt.y;
								//end vertex
								double xDiff2 = bufferShapes[i].get_Point(1).x - currPt.x;
								double yDiff2 = bufferShapes[i].get_Point(1).y - currPt.y;

								FindConnectingDegrees(Enumerations.Buffer_LineSide.Left, xDiff1, yDiff1, intXDiff, intYDiff, xDiff2, yDiff2, out startDegree, out intDegree, out endDegree);

								Debug.WriteLine("StartDegree = " + startDegree + "IntDegree = " + intDegree + " EndDegree = " + endDegree);

								startDegree -= numQuadrants;
								ptIndex = 3;
								for(double j = startDegree; j > endDegree; j -= numQuadrants)
								{
									MapWinGIS.Point circlePt = new MapWinGIS.PointClass();
									circlePt.x = (Math.Cos(j * Math.PI/180) * distance) + currPt.x;
									circlePt.y = (Math.Sin(j * Math.PI/180) * distance) + currPt.y;
									bufferShapes[i-1].InsertPoint(circlePt, ref ptIndex);
									ptIndex++;
								}							
								bufferShapes[i-1].InsertPoint(bufferShapes[i].get_Point(1), ref ptIndex);
							}
							else
							{
								double right1Distance = Globals.PtDistance(bufferShapes[i-1].get_Point(0), bufferShapes[i-1].get_Point(3));
								double rightIntDistance = Globals.PtDistance(bufferShapes[i-1].get_Point(0), rightIntPt);
								if(rightIntDistance > right1Distance)//gap exists on right side
								{
									double startDegree, intDegree, endDegree;
									//start vertex
									double xDiff1 = bufferShapes[i-1].get_Point(3).x - currPt.x;
									double yDiff1 = bufferShapes[i-1].get_Point(3).y - currPt.y;
									//intersect vertex
									double intXDiff = rightIntPt.x - currPt.x;
									double intYDiff = rightIntPt.y - currPt.y;
									//end vertex
									double xDiff2 = bufferShapes[i].get_Point(0).x - currPt.x;
									double yDiff2 = bufferShapes[i].get_Point(0).y - currPt.y;

									FindConnectingDegrees(Enumerations.Buffer_LineSide.Right, xDiff1, yDiff1, intXDiff, intYDiff, xDiff2, yDiff2, out startDegree, out intDegree, out endDegree);
									//Debug.WriteLine("StartDegree = " + startDegree + "IntDegree = " + intDegree + " EndDegree = " + endDegree);
									
									endDegree -= numQuadrants;
									ptIndex = 3;
									bufferShapes[i-1].InsertPoint(bufferShapes[i].get_Point(0), ref ptIndex);
									ptIndex++;
									for(double j = endDegree; j > startDegree; j -= numQuadrants)
									{
										MapWinGIS.Point circlePt = new MapWinGIS.PointClass();
										circlePt.x = (Math.Cos(j * Math.PI/180) * distance) + currPt.x;
										circlePt.y = (Math.Sin(j * Math.PI/180) * distance) + currPt.y;
										bufferShapes[i-1].InsertPoint(circlePt, ref ptIndex);
										ptIndex++;
									}
									
								}
								else//just add the intersect points
								{
									ptIndex = 2;
									bufferShapes[i-1].DeletePoint(ptIndex);
									bufferShapes[i-1].InsertPoint(leftIntPt, ref ptIndex);
									ptIndex = 1;
									bufferShapes[i].DeletePoint(ptIndex);
									bufferShapes[i].InsertPoint(leftIntPt, ref ptIndex);
					
									ptIndex = 3;
									bufferShapes[i-1].DeletePoint(ptIndex);
									bufferShapes[i-1].InsertPoint(rightIntPt, ref ptIndex);
									ptIndex = 0;
									bufferShapes[i].DeletePoint(ptIndex);
									bufferShapes[i].InsertPoint(rightIntPt, ref ptIndex);
									ptIndex = 4;
									bufferShapes[i].DeletePoint(ptIndex);
									bufferShapes[i].InsertPoint(rightIntPt, ref ptIndex);
								}
							}//end of filling right-sided gap
						}//end of capStyle == round
						else //pointed intersect
						{
							ptIndex = 2;
							bufferShapes[i-1].DeletePoint(ptIndex);
							bufferShapes[i-1].InsertPoint(leftIntPt, ref ptIndex);
							ptIndex = 1;
							bufferShapes[i].DeletePoint(ptIndex);
							bufferShapes[i].InsertPoint(leftIntPt, ref ptIndex);
					
							ptIndex = 3;
							bufferShapes[i-1].DeletePoint(ptIndex);
							bufferShapes[i-1].InsertPoint(rightIntPt, ref ptIndex);
							ptIndex = 0;
							bufferShapes[i].DeletePoint(ptIndex);
							bufferShapes[i].InsertPoint(rightIntPt, ref ptIndex);
							ptIndex = 4;
							bufferShapes[i].DeletePoint(ptIndex);
							bufferShapes[i].InsertPoint(rightIntPt, ref ptIndex);
						}//end of capSytle == pointed
					}//end of else slopes are different
				}
				#endregion
			}

			#region Add End Caps
			if(endCapStyle == Enumerations.Buffer_EndCapStyle.Pointed) //pointed caps
			{
				ptIndex = 1;
				bufferShapes[0].InsertPoint(capPt1, ref ptIndex);
		
				ptIndex = bufferShapes[numPts-2].numPoints-2;
				bufferShapes[numPts-2].InsertPoint(capPt2, ref ptIndex);
			}
			else if(endCapStyle == Enumerations.Buffer_EndCapStyle.Rounded)//rounded caps
			{
				ptIndex = 1;
				int numCapPts = capRnd1.numPoints;
				for(int j = 1; j <= numCapPts-2; j++)
				{
					bufferShapes[0].InsertPoint(capRnd1.get_Point(j), ref ptIndex);
					ptIndex++;
				}
				ptIndex = bufferShapes[numPts-2].numPoints-2;
				numCapPts = capRnd2.numPoints;
				for(int j = 1; j <= numCapPts-2; j++)
				{
					bufferShapes[numPts-2].InsertPoint(capRnd2.get_Point(j), ref ptIndex);
					ptIndex++;
				}
			}
			else if(endCapStyle == Enumerations.Buffer_EndCapStyle.ClosePolygon)//original line was a polygon, close the end points
			{
				MapWinGIS.Shape lastBuff = new MapWinGIS.ShapeClass();
				lastBuff.Create(MapWinGIS.ShpfileType.SHP_POLYGON);
				lastBuff = bufferShapes[numPts-2];
				MapWinGIS.Shape firstBuff = new MapWinGIS.ShapeClass();
				firstBuff.Create(MapWinGIS.ShpfileType.SHP_POLYGON);
				firstBuff = bufferShapes[0];
				ClosePolyBuffer(ref lastBuff, ref firstBuff, distance, buffSide, capStyle, numQuadrants);
				bufferShapes[numPts-2] = lastBuff;
				bufferShapes[0] = firstBuff;
			}
			//else blunt caps
			#endregion

			#region Unite Buffer Shapes
            // Paul Meems 22 July 2009: Added some debugging lines for bug #1314
			if(bufferShapes.Length > 1)
			{
				buffResult = SpatialOperations.Union(bufferShapes[0], bufferShapes[1]);
                if (buffResult == null || buffResult.ShapeType == MapWinGIS.ShpfileType.SHP_NULLSHAPE)
                {
                    MapWinUtility.Logger.Dbg("ERROR: SpatialOperations.Union returned a NULLSHAPE!");
                    resultShp = null;
                    return false;
                }
                MapWinUtility.Logger.Dbg("Before edge loop. numEdges: " + numEdges.ToString());
				for(int i = 2; i <= numEdges-1; i++)
				{
                    //MapWinUtility.Logger.Dbg("In edge loop. buffResult.numPoints: " + buffResult.numPoints.ToString());
					buffResult = SpatialOperations.Union(buffResult, bufferShapes[i]);
                    if (buffResult == null || buffResult.ShapeType == MapWinGIS.ShpfileType.SHP_NULLSHAPE)
                    {
                        MapWinUtility.Logger.Dbg("ERROR: SpatialOperations.Union returned a NULLSHAPE!");
                        resultShp = null;
                        return false;
                    }
                }
			}
			else
			{
				buffResult = bufferShapes[0];
			}

			#endregion

            if (buffResult == null || buffResult.ShapeType == MapWinGIS.ShpfileType.SHP_NULLSHAPE)
            {
                MapWinUtility.Logger.Dbg("ERROR: SpatialOperations.Union returned a NULLSHAPE!");
                resultShp = null;
                return false;
            }
            resultShp = buffResult;
            MapWinUtility.Logger.Dbg("Finished BufferLine");
			return true;
        }

        #endregion

        ///// <summary>
        ///// This is a patch by Ted Dunsford on 3/3/2007 in an effort to fix a bug that crops up under certain situations.
        ///// Angela's code doesn't handle multi-part polylines.  However, This patch currently only fixes the case where
        ///// the caps are rounded and the desired buffer is two sided.  This is not elegant, but it should make buffering
        ///// lines much more doable.
        ///// </summary>
        ///// <param name="mwPolyline">MapWinGIS.Shape representing the polyline to buffer</param>
        ///// <param name="Distance">Double, the distance from the polyline for the buffer to be drawn.</param>
        ///// <param name="numCapPoints">The caps on the ends will be drawn with numCapPoints.  The more points, the smoother the curve. </param>
        ///// <param name="Side">Determines whether to buffer the left, right or both sides of the vectors</param>
        ///// <param name="ICallBack">A MapWinGIS.ICallBack for status (Buffering can take a while)</param>
        ///// <returns>A MapWinGIS.Shape object representing the buffer.</returns>
        //public static MapWinGIS.Shape Buffer_Segments(MapWinGIS.Shape mwPolyline, double Distance, int numCapPoints, Topology2D.Enums.BufferSide Side, MapWinGIS.ICallback ICallBack)
        //{
        //    if (mwPolyline.ShapeType != MapWinGIS.ShpfileType.SHP_POLYLINE &&
        //       mwPolyline.ShapeType != MapWinGIS.ShpfileType.SHP_POLYLINEM &&
        //       mwPolyline.ShapeType != MapWinGIS.ShpfileType.SHP_POLYLINEZ)
        //    {
        //        throw new ArgumentException("mwPolyline must be one of the Polyline shape types.");
        //    }
           
        //    MapWinGIS.Shape shpOut;
        //    MapWinGIS.Shape shpFinal = null;
        //    Topology2D.Polygon Buff = null;
        //    if (mwPolyline.NumParts < 2)
        //    {
        //        for (int iPoint = 0; iPoint < mwPolyline.numPoints - 1; iPoint++)
        //        {
        //            MapWinGIS.Point Pta = mwPolyline.get_Point(iPoint);
        //            MapWinGIS.Point Ptb = mwPolyline.get_Point(iPoint + 1);
        //            Topology2D.LineSegment Seg = new Topology2D.LineSegment(Pta, Ptb);
        //            Buff = (Seg.Buffer(Distance, numCapPoints, Side) as Topology2D.Polygon);
        //            shpOut = new MapWinGIS.Shape();
        //            shpOut.ShapeType = MapWinGIS.ShpfileType.SHP_POLYGON;
        //            for (int pt = 0; pt < Buff.Points.Count; pt++)
        //            {
        //               MapWinGIS.Point Pnt = new MapWinGIS.Point();
        //               Pnt.x = Buff.Points[pt].X;
        //               Pnt.y = Buff.Points[pt].Y;
        //               shpOut.InsertPoint(Pnt, ref pt);
        //            }
        //            if (shpFinal == null)
        //            {
        //               shpFinal = shpOut;
        //            }
        //            else
        //            {
        //               shpFinal = MapWinGeoProc.SpatialOperations.Union(shpFinal, shpOut);
        //               if (ICallBack != null)
        //               {
        //                   ICallBack.Progress("Status", (int)(iPoint * 100 / mwPolyline.numPoints), "Buffering...");
        //               }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        for (int iPart = 0; iPart < mwPolyline.NumParts - 1; iPart++)
        //        {
        //            for (int iPoint = mwPolyline.get_Part(iPart); iPoint < mwPolyline.get_Part(iPart + 1) - 1; iPoint++)
        //            {
        //                MapWinGIS.Point Pta = mwPolyline.get_Point(iPoint);
        //                MapWinGIS.Point Ptb = mwPolyline.get_Point(iPoint + 1);
        //                Topology2D.LineSegment Seg = new Topology2D.LineSegment(Pta, Ptb);
        //                Buff = (Seg.Buffer(Distance, numCapPoints, Side) as Topology2D.Polygon);
        //                shpOut = new MapWinGIS.Shape();
        //                shpOut.ShapeType = MapWinGIS.ShpfileType.SHP_POLYGON;
        //                for (int pt = 0; pt < Buff.Points.Count; pt++)
        //                {
        //                    MapWinGIS.Point Pnt = new MapWinGIS.Point();
        //                    Pnt.x = Buff.Points[pt].X;
        //                    Pnt.y = Buff.Points[pt].Y;
        //                    shpOut.InsertPoint(Pnt, ref pt);
        //                }
        //                if (shpFinal == null)
        //                {
        //                    shpFinal = shpOut;
        //                }
        //                else
        //                {
        //                    shpFinal = MapWinGeoProc.SpatialOperations.Union(shpFinal, shpOut);
        //                 }
        //                 if (ICallBack != null)
        //                 {
        //                     ICallBack.Progress("Status", (int)(iPoint * 100 / mwPolyline.numPoints), "Buffering...");
        //                 }
        //            }
        //        }  
        //    }
        //    for (int iPoint = mwPolyline.get_Part(mwPolyline.NumParts - 1); iPoint < mwPolyline.numPoints - 1; iPoint++)
        //    {
        //        MapWinGIS.Point Pta = mwPolyline.get_Point(iPoint);
        //        MapWinGIS.Point Ptb = mwPolyline.get_Point(iPoint + 1);
        //        Topology2D.LineSegment Seg = new Topology2D.LineSegment(Pta, Ptb);
        //        Buff = (Seg.Buffer(Distance, numCapPoints, Side) as Topology2D.Polygon);
        //        shpOut = new MapWinGIS.Shape();
        //        shpOut.ShapeType = MapWinGIS.ShpfileType.SHP_POLYGON;
        //        for (int pt = 0; pt < Buff.Points.Count; pt++)
        //        {
        //            MapWinGIS.Point Pnt = new MapWinGIS.Point();
        //            Pnt.x = Buff.Points[pt].X;
        //            Pnt.y = Buff.Points[pt].Y;
        //            shpOut.InsertPoint(Pnt, ref pt);
        //        }
        //        if (shpFinal == null)
        //        {
        //            shpFinal = shpOut;
        //        }
        //        else
        //        {
        //            shpFinal = MapWinGeoProc.SpatialOperations.Union(shpFinal, shpOut);
        //        }
        //        if (ICallBack != null)
        //        {
        //            ICallBack.Progress("Status", (int)(iPoint * 100 / mwPolyline.numPoints), "Buffering...");
        //        }
        //    }
        //    return shpFinal;
        //}


		

		#region private FindCapChange()
		private static void FindCapChange(int capNum, double rightTheta, double distance, double deltaX, double deltaY, out double capXchange, out double capYchange)
		{
			#region Horizontal segment
			if(deltaY == 0)//horizontal segment
			{
				capYchange = 0;
				if(deltaX > 0)//+x
				{
					if(capNum == 0)
					{	
						capXchange = -distance;						
					}
					else
					{
						capXchange = distance;
					}
				}
				else//-x
				{
					if(capNum == 0)
					{
						capXchange = distance;
					}
					else
					{
						capXchange = -distance;
					}
				}
			}
				#endregion
	
				#region Vertical segment
			else if(deltaX == 0)//vertical segment
			{
				capXchange = 0;

				if(deltaY > 0)// +y
				{
					if(capNum == 0)
					{
						capYchange = -distance;
					}
					else
					{
						capYchange = distance;
					}
				}
				else// -y
				{
					if(capNum == 0)
					{
						capYchange = distance;
					}
					else
					{
						capYchange = -distance;
					}
				}
			}
				#endregion

				#region Sloped segment
			else
			{
				double alpha = rightTheta - 90;
				double xChange = distance * Math.Cos(alpha * (Math.PI/180));
				double yChange = distance * Math.Sin(alpha * (Math.PI/180));

				//				if(deltaX > 0)
				//				{
				if(capNum == 0)
				{
					capXchange = xChange;
					capYchange = yChange;
				}
				else
				{
					capXchange = -xChange;
					capYchange = -yChange;
				}
				//				}				
			}
			#endregion
		}
		#endregion

		#region private FindPerpendicularDegree()
		private static double FindPerpendicularDegree(Enumerations.Buffer_LineSide bufferSide, double deltaX, double deltaY)
		{
			double theta;

			if(bufferSide == Enumerations.Buffer_LineSide.Left) //Left side
			{
				if(deltaX == 0) //vertical segment, so perpedicular is horizontal
				{
					if(deltaY > 0)
					{
						theta = 180;
					}
					else
					{
						theta = 0;
					}
				}
				else if(deltaY == 0) //horizontal segment, so perpendicular is vertical
				{
					if(deltaX > 0)
					{
						theta = 90;
					}
					else
					{
						theta = 270;
					}
				}
				else //sloped segment
				{
					double alpha = Math.Atan(deltaY/deltaX)*(180/Math.PI);
					if(deltaX < 0)
					{
						alpha += 180; //put into correct quadrant
					}
					else if(deltaX > 0 && alpha < 0)
					{
						alpha += 360; //let's just deal with positive angles...
					}
					theta = alpha + 90;
				}
			}//end of finding theta for left side
			else //bufferSide = right
			{
				if(deltaX == 0) //vertical segment, so perpedicular is horizontal
				{
					if(deltaY > 0)
					{
						theta = 0;
					}
					else
					{
						theta = 180;
					}
				}
				else if(deltaY == 0) //horizontal segment, so perpendicular is vertical
				{
					if(deltaX > 0)
					{
						theta = 270;
					}
					else
					{
						theta = 90;
					}
				}
				else //sloped segment
				{
					double alpha = Math.Atan(deltaY/deltaX)*(180/Math.PI);
					if(deltaX < 0)
					{
						alpha += 180; //put into correct quadrant
					}
					else if(deltaX > 0 && alpha < 0)
					{
						alpha += 360; //let's just deal with positive angles...
					}
					theta = alpha - 90;					
				}
			}
			return theta;
		}
		#endregion

		#region private FindConnectingDegrees()
		private static void FindConnectingDegrees(Enumerations.Buffer_LineSide bufferSide, double xDiff1, double yDiff1, double intXDiff, double intYDiff, double xDiff2, double yDiff2, out double startDegree, out double intDegree, out double endDegree)
		{
			if(bufferSide == Enumerations.Buffer_LineSide.Left)//left side
			{
				//For a left sided edge: startDegree > intDegree > endDegree
				#region START degree
				if(xDiff1 == 0)//vertical segment
				{
					if(yDiff1 > 0)//+y
					{
						startDegree = 90;
					}
					else
					{
						startDegree = 270;
					}	
				}
				else if(yDiff1 == 0)//horizontal
				{
					if(xDiff1 < 0)//-x
					{
						startDegree = 180;
					}
					else
					{
						startDegree = 360;
					}
				}
				else //slope
				{
					startDegree = Math.Atan(yDiff1/xDiff1)*(180/Math.PI);
					if(xDiff1 < 0)//put into correct quadrant
					{
						startDegree += 180;
					}
					else if(xDiff1 > 0 && startDegree <= 0)
					{
						startDegree += 360; //want startDegree to be largest degree
					}
				}
				#endregion

				#region INTERSECT degree
				if(intXDiff == 0)//vertical
				{
					if(startDegree < 90)
					{
						intDegree = -90;
					}
					else if(startDegree > 270)
					{
						intDegree = 270;
					}
					else
					{
						intDegree = 90;
					}
				}
				else if(intYDiff == 0)//horizontal
				{
					if(startDegree < 180)
					{
						intDegree = 0;
					}
					else
					{
						intDegree = 180;
					}
				}
				else //slope
				{
					intDegree = Math.Atan(intYDiff/intXDiff)*(180/Math.PI);	
					if(intXDiff < 0)//put into correct quadrant
					{
						intDegree += 180;
					}
					else if(intXDiff > 0 && intDegree < 0 && startDegree > 270)
					{									
						intDegree += 360;
					}
				}
				if(intDegree > startDegree)
				{
					intDegree -= 360;
				}
				#endregion

				#region END degree
				if(xDiff2 == 0)//vertical
				{
					if(intDegree < 90)
					{
						endDegree = -90;
					}
					else if(intDegree > 270)
					{
						endDegree = 270;
					}
					else
					{
						endDegree = 90;
					}
				}
				else if(yDiff2 == 0)//horizontal
				{
					if(intDegree < 180)
					{
						endDegree = 0;
					}
					else
					{
						endDegree = 180;
					}
				}
				else //slope
				{
					endDegree = Math.Atan(yDiff2/xDiff2)*(180/Math.PI);
					if(xDiff2 < 0)//put into correct quadrant
					{
						endDegree += 180;
					}
					else if(xDiff2 > 0 && endDegree < 0 && intDegree > 270)
					{
						endDegree += 360;
					}
				}
				if(endDegree > intDegree)
				{
					endDegree -= 360;
				}
				#endregion
			}
			else//right side
			{
				//For a right sided edge: startDegree < intersectDegree < endDegree
				#region END degree
				if(xDiff2 == 0)//vertical
				{
					if(yDiff2 > 0)
					{
						endDegree = 90;
					}
					else
					{
						endDegree = 270;
					}
				}
				else if(yDiff2 == 0)//horizontal
				{
					if(xDiff2 > 0)
					{
						endDegree = 360;
					}
					else
					{
						endDegree = 180;
					}
				}
				else //slope
				{
					endDegree = Math.Atan(yDiff2/xDiff2)*(180/Math.PI);
					if(xDiff2 < 0)
					{
						endDegree += 180; //put in correct quadrant
					}
					else if(xDiff2 > 0 && endDegree <= 0)
					{
						endDegree += 360; //always want this to be the largest degree
					}
				}
				#endregion

				#region INTERSECT degree
				if(intXDiff == 0)//vertical segment
				{
					if(endDegree < 90)
					{
						intDegree = -90;
					}
					else if(endDegree > 270)
					{
						intDegree = 270;
					}
					else
					{
						intDegree = 90;
					}
				}
				else if(intYDiff == 0)//horizontal
				{
					if(endDegree < 180)
					{
						intDegree = 0;
					}
					else
					{
						intDegree = 180;
					}
				}
				else //slope
				{
					intDegree = Math.Atan(intYDiff/intXDiff)*(180/Math.PI);
					if(intXDiff < 0)
					{
						intDegree += 180; //transfer to correct quadrant
					}
					else if(intXDiff > 0 && intDegree < 0 && endDegree > 270)
					{
						intDegree += 360;
					}
				}
				if(intDegree > endDegree)
				{
					intDegree -= 360;
				}
				#endregion

				#region START degree
				if(xDiff1 == 0)//vertical
				{
					if(intDegree < 90)
					{
						startDegree = -90;
					}
					else if(intDegree > 270)
					{
						startDegree = 270;
					}
					else
					{
						startDegree = 90;
					}
				}
				else if(yDiff1 == 0)//horizontal
				{
					if(endDegree < 180)
					{
						startDegree = 0;
					}
					else
					{
						startDegree = 180;
					}
				}
				else //slope
				{
					startDegree = Math.Atan(yDiff1/xDiff1)*(180/Math.PI);
					if(xDiff1 < 0)
					{
						startDegree += 180;
					}
					else if(xDiff1 > 0 && startDegree < 0 && intDegree > 270)
					{
						startDegree += 360;
					}
				}
				if(startDegree > intDegree)
				{
					startDegree -= 360;
				}
				#endregion
			}
		}
		#endregion

		#region BufferPolygon()
		//Angela Hillier 3/28/06
		/// <summary>
		/// Creates a buffer shape around the polygon.
		/// </summary>
		/// <param name="polygon">The polygon to be buffered.</param>
		/// <param name="distance">Positive or negative value for buffer distance.</param>
		/// <param name="holeTreatment">How holes are to be treated. 0 == ignore, 1 == opposite, 2 == same</param>
		/// <param name="capStyle">Edge treatment. 0 = pointed, 1 = rounded.</param>
		/// <param name="numQuadrants">Smoothness of curves.</param>
		/// <param name="resultShp">The resulting buffer shape.</param>
		/// <returns>False if an error was encountered, true otherwise.</returns>
		public static bool BufferPolygon(ref MapWinGIS.Shape polygon, double distance, Enumerations.Buffer_HoleTreatment holeTreatment, Enumerations.Buffer_CapStyle capStyle, int numQuadrants, out MapWinGIS.Shape resultShp)
		{
			MapWinGIS.Shape buffer = new MapWinGIS.ShapeClass();
			buffer.Create(polygon.ShapeType);

			if(distance == 0)
			{
				gErrorMsg = "Buffer distance cannot equal zero.";
				Debug.WriteLine(gErrorMsg);
				Error.SetErrorMsg(gErrorMsg);
				resultShp = buffer;
				return false;
			}
			if (numQuadrants < 1)
			{
				numQuadrants = 8;
			}
			int numParts = polygon.NumParts;
			if(numParts == 0)
			{
				numParts = 1;
			}
			if(numParts > 1)//multiPart polygon
			{
				//Be sure that all parts of the polygon have the correct orientation
				Globals.FixMultiPartPoly(ref polygon);

				double holeDistance = distance;
				if(holeTreatment == Enumerations.Buffer_HoleTreatment.Opposite)//opposite buffer effect
				{
					holeDistance = -distance;
				}
		
				MapWinGIS.Shapefile buffParts = new MapWinGIS.ShapefileClass();
				string tempFile = System.IO.Path.GetTempPath() + "tempSF.shp";
                // Paul Meems 22 july 2009: Deleteshape is done in Globals.PrepareResultSF:
                //DataManagement.DeleteShapefile(ref tempFile);
				//CDM 8/4/2006 buffParts.CreateNew(tempFile, polygon.ShapeType);
                Globals.PrepareResultSF(ref tempFile, ref buffParts, polygon.ShapeType);
                // Paul Meems 22 july 2009:
                // After a shapefile is created, the attribute table and shapefile are automatically in editing mode
				//buffParts.StartEditingShapes(true, null);
				
				MapWinGIS.Shape[] partsArray = new MapWinGIS.Shape[numParts];
				int numValidParts = 0;
				Globals.SeparateParts(ref polygon, out partsArray);
				int shpIndex = 0;
				
				for(int i = 0; i <= numParts-1; i++)
				{
					MapWinGIS.Shape buffPart = new MapWinGIS.ShapeClass();
					buffPart.Create(polygon.ShapeType);

					bool isClockwise = Globals.IsClockwise(ref partsArray[i]);
					if(!isClockwise)//we have a hole
					{
						if(holeTreatment != Enumerations.Buffer_HoleTreatment.Ignore) //we can't ignore the hole
						{
							if(holeTreatment == Enumerations.Buffer_HoleTreatment.Original)
							{
								shpIndex = buffParts.NumShapes;
								buffParts.EditInsertShape(partsArray[i], ref shpIndex);
							}
							else
							{
								if(BufferSimplePoly(ref partsArray[i], holeDistance, capStyle, numQuadrants, out buffPart) == false)
								{
									resultShp = buffer;
									return false;
								}

								int numBuffPoints = buffPart.numPoints;
								if(numBuffPoints > 0)
								{
									int numBuffParts = buffPart.NumParts;
									if(numBuffParts == 0)
									{
										numBuffParts = 1;
									}
									MapWinGIS.Shape[] parts = new MapWinGIS.Shape[numBuffParts];
									Globals.SeparateParts(ref buffPart, out parts);
									for(int j = 0; j <= numBuffParts-1; j++)
									{
										//BufferSimplePoly() reversed the shape, be sure that all HOLES are counterclockwise
										if(Globals.IsClockwise(ref parts[j]))
										{
											Globals.ReverseSimplePoly(ref parts[j]);
										}
										shpIndex = buffParts.NumShapes;
										buffParts.EditInsertShape(parts[j], ref shpIndex);
									}
								}//end of if numBuffPts > 0
							}//end of else holeTreatment != 3
						}//end of if holeTreatment != 0
					}//end of dealing with holes
					else //it's not a hole
					{
						if (BufferSimplePoly(ref partsArray[i], distance, capStyle, numQuadrants, out buffPart) == false)
						{
							resultShp = buffer;
							return false;
						}
						if (buffPart.numPoints > 0)
						{
							int numBuffParts = buffPart.NumParts;
							if(numBuffParts == 0)
							{
								numBuffParts = 1;
							}
							MapWinGIS.Shape[] parts = new MapWinGIS.Shape[numBuffParts];
							Globals.SeparateParts(ref buffPart, out parts);
							for(int j = 0; j <= numBuffParts-1; j++)
							{
								shpIndex = buffParts.NumShapes;
								buffParts.EditInsertShape(parts[j], ref shpIndex);
							}
						}
					}
				}//end of buffering parts

				numValidParts = buffParts.NumShapes;
				//combine overlapping shapes
				if(numValidParts > 1)
				{
					CombineOverlappingParts(ref buffParts);
				}
				numValidParts = buffParts.NumShapes;
				if(numValidParts > 1)
				{
					MapWinGIS.Shape[] allParts = new MapWinGIS.Shape[numValidParts];
					for(int j = 0; j <= numValidParts-1; j++)
					{
						allParts[j] = new MapWinGIS.ShapeClass();
						allParts[j].Create(polygon.ShapeType);
						allParts[j] = buffParts.get_Shape(j);
					}
					Globals.CombineParts(ref allParts, out buffer);
					Globals.FixMultiPartPoly(ref buffer);//make sure all holes are counter-clockwise
				}
				else
				{
					buffer = buffParts.get_Shape(0);
					if(buffer.numPoints == 0)
					{
						gErrorMsg = "No points in resulting buffer shape.";
						Error.SetErrorMsg(gErrorMsg);
						Debug.WriteLine(gErrorMsg);
						resultShp = buffer;
						return false;
					}
				}			
			}//Done with mulitPart polygon
			
			else //simple polygon
			{
				if(BufferSimplePoly(ref polygon, distance, capStyle, numQuadrants, out buffer) == false)
				{
					resultShp = buffer;
					return false;
				}
			}//end of dealing with simple polygon

			resultShp = buffer;
			return true;
		}
		#endregion
		
		#region private BufferSimplePoly()
		//Angela Hillier 3/18/06
		/// <summary>
		/// Creates a buffer around a single-part polygon.
		/// </summary>
		/// <param name="polygon">The simple polygon to be buffered.</param>
		/// <param name="distance">The distance from the polygon's border that the buffer should be created at.</param>
		/// <param name="capStyle">Edge treatement. 0 = pointed, 1 = rounded.</param>
		/// <param name="numQuadrants">Smoothness of rounded egdes. Smaller number = smoother circle.</param>
		/// <param name="resultShp">The resulting buffer.</param>
		/// <returns>False if an error was encountered, true otherwise.</returns>
		private static bool BufferSimplePoly(ref MapWinGIS.Shape polygon, double distance, Enumerations.Buffer_CapStyle capStyle, int numQuadrants, out MapWinGIS.Shape resultShp)
		{
            MapWinUtility.Logger.Dbg("NumPoints: " + polygon.numPoints.ToString());
			MapWinGIS.Shape buffer = new MapWinGIS.ShapeClass();
			buffer.Create(polygon.ShapeType);

			bool negBuffer = false;
			int numPts = polygon.numPoints;
			int ptIndex = 0;

			bool isClockwise = Globals.IsClockwise(ref polygon);
			if(!isClockwise)
			{
				//reverse direction, we only want to deal with clockwise shapes
				Globals.ReverseSimplePoly(ref polygon);
			}
			if(distance < 0)
			{
				distance = -distance; //we must work with positive distance values
				negBuffer = true;
			}

			MapWinGIS.Shape polyLine = new MapWinGIS.ShapeClass();
			polyLine.Create(MapWinGIS.ShpfileType.SHP_POLYLINE);
			//turn the polygon into a line
			for(int i = 0; i <= numPts-1; i++)
			{
				polyLine.InsertPoint(polygon.get_Point(i), ref ptIndex);
				ptIndex++;
			}
			if(negBuffer == false) //we'll be making the polygon larger
			{
				//form an outer shell by buffering the left side of the line
				//BufferLine(ref polyLine, distance, 1, 1, 2, 8, out buffer);
                if (!BufferLine(ref polyLine, distance, Enumerations.Buffer_LineSide.Both, capStyle, Enumerations.Buffer_EndCapStyle.ClosePolygon, 8, out buffer))
                {
                    resultShp = null;
                    return false;
                }
				//fill it with the original polygon
				resultShp = SpatialOperations.Union(polygon, buffer);
				//resultShp = buffer;
			}
			else
			{
				//form an inner shell by buffering the right side of the line
				//BufferLine(ref polyLine, distance, 2, 1, 2, 8, out buffer);
                if (!BufferLine(ref polyLine, distance, Enumerations.Buffer_LineSide.Both, capStyle, Enumerations.Buffer_EndCapStyle.ClosePolygon, 8, out buffer))
                {
                    resultShp = null;
                    return false;
                }
				resultShp = SpatialOperations.Difference(polygon, buffer);
				//what we need: an option in BufferLine() for side buffering so we only buffer the right side.
				//resultShp = buffer;
			}
			
			return true;
		}
		#endregion

		#region private ClosePolyBuffer()
		//Angela Hillier 3/20/06
		/// <summary>
		/// Creates an end-cap for a polygon that has been processed through BufferLine() so that
		/// the first and last points will be connected correctly.
		/// </summary>
		/// <param name="lastBuff">The last buffer shape that was created for the polygon line.</param>
		/// <param name="firstBuff">The first buffer shape that was created for the polygon line.</param>
		/// <param name="distance">The distance that is being used for buffer creation.</param>
		/// <param name="buffSide">The side of the line that is being buffered. 0 = both, 1 = left, 2 = right</param>
		/// <param name="capStyle">Edge treatement. 0 = pointed, 1 = rounded.</param>
		/// <param name="numQuadrants">Smoothness of rounded edges. Smaller values = smoother circle.</param>
		private static void ClosePolyBuffer(ref MapWinGIS.Shape lastBuff, ref MapWinGIS.Shape firstBuff, double distance, Enumerations.Buffer_LineSide buffSide, Enumerations.Buffer_CapStyle capStyle, int numQuadrants)
		{
			//check the slopes to see if we need to add any points...				
			int lastPts = lastBuff.numPoints;
			int firstPts = firstBuff.numPoints;
			int ptIndex = 0;
			MapWinGIS.Point currPt = new MapWinGIS.PointClass();
			if(buffSide == 0)
			{
				currPt.x = (lastBuff.get_Point(2).x - lastBuff.get_Point(3).x)/2 + lastBuff.get_Point(3).x;
				currPt.y = (lastBuff.get_Point(2).y - lastBuff.get_Point(3).y)/2 + lastBuff.get_Point(3).y;
			}
			else if(buffSide == Enumerations.Buffer_LineSide.Left)//buffering left
			{
				currPt = firstBuff.get_Point(0);
			}
			else//buffering right side
			{
				currPt = firstBuff.get_Point(1);
			}

			MapWinGIS.Point ln1pt0 = new MapWinGIS.PointClass();
			MapWinGIS.Point ln1pt1 = new MapWinGIS.PointClass();
			MapWinGIS.Point ln2pt0 = new MapWinGIS.PointClass();
			MapWinGIS.Point ln2pt1 = new MapWinGIS.PointClass();
			ln1pt0 = lastBuff.get_Point(1);
			ln1pt1 = lastBuff.get_Point(2);
			ln2pt0 = firstBuff.get_Point(1);
			ln2pt1 = firstBuff.get_Point(2);
			double ln1Xdiff = ln1pt1.x - ln1pt0.x;
			double ln1Ydiff = ln1pt1.y - ln1pt0.y;
			double ln2Xdiff = ln2pt1.x - ln2pt0.x;
			double ln2Ydiff = ln2pt1.y - ln2pt0.y;
			if((ln1Xdiff == 0 && ln2Xdiff == 0)||(ln1Ydiff == 0 && ln2Ydiff == 0)||(ln1Xdiff == ln2Xdiff && ln1Ydiff == ln2Ydiff))
			{	
				//both are vertical, horizontal, or of equal slope
			}
			else//the segments have different slopes, expect an intersect point
			{
				Globals.Line left1 = new Globals.Line(lastBuff.get_Point(1), lastBuff.get_Point(2));
				Globals.Line left2 = new Globals.Line(firstBuff.get_Point(2), firstBuff.get_Point(1));
				MapWinGIS.Point leftIntPt = new MapWinGIS.PointClass();
				bool found = Globals.LinesIntersect2D(left1, true, left2, true, out leftIntPt);
				if(found == false)
				{
					left1 = new Globals.Line(lastBuff.get_Point(2), lastBuff.get_Point(1));
					found = Globals.LinesIntersect2D(left1, true, left2, true, out leftIntPt);
				}

				Globals.Line right1 = new Globals.Line(lastBuff.get_Point(0), lastBuff.get_Point(lastPts-2));
				Globals.Line right2 = new Globals.Line(firstBuff.get_Point(firstPts-2), firstBuff.get_Point(0));
				MapWinGIS.Point rightIntPt = new MapWinGIS.PointClass();
				found = Globals.LinesIntersect2D(right1, true, right2, true, out rightIntPt);
				if(found == false)
				{
					right1 = new Globals.Line(lastBuff.get_Point(lastPts-2), firstBuff.get_Point(0));
					found = Globals.LinesIntersect2D(right1, true, right2, true, out rightIntPt);
				}						

				if(capStyle == Enumerations.Buffer_CapStyle.Rounded)//round edges
				{
					//find which edge is the 'outer' edge
					double left1Distance = Globals.PtDistance(lastBuff.get_Point(1), lastBuff.get_Point(2));
					double leftIntDistance = Globals.PtDistance(lastBuff.get_Point(1), leftIntPt);
					if(leftIntDistance > left1Distance)//gap exists, left side is our outer edge
					{
						double startDegree, intDegree, endDegree;
						//start vertex
						double xDiff1 = lastBuff.get_Point(2).x - currPt.x;
						double yDiff1 = lastBuff.get_Point(2).y - currPt.y;
						//intersect vertex
						double intXDiff = leftIntPt.x - currPt.x;
						double intYDiff = leftIntPt.y - currPt.y;
						//end vertex
						double xDiff2 = firstBuff.get_Point(1).x - currPt.x;
						double yDiff2 = firstBuff.get_Point(1).y - currPt.y;

						FindConnectingDegrees(Enumerations.Buffer_LineSide.Left, xDiff1, yDiff1, intXDiff, intYDiff, xDiff2, yDiff2, out startDegree, out intDegree, out endDegree);

						Debug.WriteLine("StartDegree = " + startDegree + "IntDegree = " + intDegree + " EndDegree = " + endDegree);

						startDegree -= numQuadrants;
						ptIndex = 3;
						for(double j = startDegree; j > endDegree; j -= numQuadrants)
						{
							MapWinGIS.Point circlePt = new MapWinGIS.PointClass();
							circlePt.x = (Math.Cos(j * Math.PI/180) * distance) + currPt.x;
							circlePt.y = (Math.Sin(j * Math.PI/180) * distance) + currPt.y;
							lastBuff.InsertPoint(circlePt, ref ptIndex);
							ptIndex++;
						}							
						lastBuff.InsertPoint(firstBuff.get_Point(1), ref ptIndex);
					}
					else
					{
						double right1Distance = Globals.PtDistance(lastBuff.get_Point(0), lastBuff.get_Point(3));
						double rightIntDistance = Globals.PtDistance(lastBuff.get_Point(0), rightIntPt);
						if(rightIntDistance > right1Distance)//gap exists on right side
						{
							double startDegree, intDegree, endDegree;
							//start vertex
							double xDiff1 = lastBuff.get_Point(lastPts-2).x - currPt.x;
							double yDiff1 = lastBuff.get_Point(lastPts-2).y - currPt.y;
							//intersect vertex
							double intXDiff = rightIntPt.x - currPt.x;
							double intYDiff = rightIntPt.y - currPt.y;
							//end vertex
							double xDiff2 = firstBuff.get_Point(0).x - currPt.x;
							double yDiff2 = firstBuff.get_Point(0).y - currPt.y;

							FindConnectingDegrees(Enumerations.Buffer_LineSide.Right, xDiff1, yDiff1, intXDiff, intYDiff, xDiff2, yDiff2, out startDegree, out intDegree, out endDegree);
							//Debug.WriteLine("StartDegree = " + startDegree + "IntDegree = " + intDegree + " EndDegree = " + endDegree);
									
							endDegree -= numQuadrants;
							ptIndex = 3;
							lastBuff.InsertPoint(firstBuff.get_Point(0), ref ptIndex);
							ptIndex++;
							for(double j = endDegree; j > startDegree; j -= numQuadrants)
							{
								MapWinGIS.Point circlePt = new MapWinGIS.PointClass();
								circlePt.x = (Math.Cos(j * Math.PI/180) * distance) + currPt.x;
								circlePt.y = (Math.Sin(j * Math.PI/180) * distance) + currPt.y;
								lastBuff.InsertPoint(circlePt, ref ptIndex);
								ptIndex++;
							}
									
						}
						else//just add the intersect points
						{
							ptIndex = 2;
							lastBuff.DeletePoint(ptIndex);
							lastBuff.InsertPoint(leftIntPt, ref ptIndex);
							ptIndex = 1;
							firstBuff.DeletePoint(ptIndex);
							firstBuff.InsertPoint(leftIntPt, ref ptIndex);
					
							ptIndex = 3;
							lastBuff.DeletePoint(ptIndex);
							lastBuff.InsertPoint(rightIntPt, ref ptIndex);
							ptIndex = 0;
							firstBuff.DeletePoint(ptIndex);
							firstBuff.InsertPoint(rightIntPt, ref ptIndex);
							ptIndex = firstPts-1;
							firstBuff.DeletePoint(ptIndex);
							firstBuff.InsertPoint(rightIntPt, ref ptIndex);
						}
					}//end of filling right-sided gap
				}//end of capStyle == round
				else //pointed intersect
				{
					ptIndex = 2;
					lastBuff.DeletePoint(ptIndex);
					lastBuff.InsertPoint(leftIntPt, ref ptIndex);
					ptIndex = 1;
					firstBuff.DeletePoint(ptIndex);
					firstBuff.InsertPoint(leftIntPt, ref ptIndex);
					
					ptIndex = 3;
					lastBuff.DeletePoint(ptIndex);
					lastBuff.InsertPoint(rightIntPt, ref ptIndex);
					ptIndex = 0;
					firstBuff.DeletePoint(ptIndex);
					firstBuff.InsertPoint(rightIntPt, ref ptIndex);
					ptIndex = firstPts-1;
					firstBuff.DeletePoint(ptIndex);
					firstBuff.InsertPoint(rightIntPt, ref ptIndex);
				}//end of capSytle == pointed
			}//end of else slopes are different	

		}
		#endregion

		#region private CombineOverlappingParts()
		//Angela Hillier 3/28/06
		/// <summary>
		/// This assumes that every shape in the input shapefile are going to be combined later
		/// as a multiPart shape. It checks to see if any of the parts overlap, if they do, and if
		/// they have the same orientation (hole or island) then the parts will be combined.
		/// </summary>
		/// <param name="shpFile">The shapefile containing the separated parts.</param>
		private static void CombineOverlappingParts(ref MapWinGIS.Shapefile shpFile)
		{
			int shpIndex = 0;
			for(int i = 0; i <= shpFile.NumShapes-1; i++)
			{
				int numShapes = shpFile.NumShapes;
				int numToDelete = 0;
				bool[] deleteMe = new bool[numShapes];

				for(int j = 0; j <= numShapes-1; j++)
				{
					deleteMe[j] = false;
					MapWinGIS.Shape currShp = new MapWinGIS.ShapeClass();
					currShp.Create(shpFile.ShapefileType);
					currShp = shpFile.get_Shape(i);
					bool currIsClockwise = Globals.IsClockwise(ref currShp);

					if(j != i)
					{
						MapWinGIS.Shape shp = new MapWinGIS.ShapeClass();
						shp.Create(shpFile.ShapefileType);
						shp = shpFile.get_Shape(j);
						bool shpIsClockwise = Globals.IsClockwise(ref shp);

						if(currIsClockwise == shpIsClockwise)
						{
							if((Globals.CheckBounds(ref currShp, ref shp) == true) && (Globals.ExtentsInside(shp.Extents, currShp.Extents) == false) && (Globals.ExtentsInside(currShp.Extents, shp.Extents) == false))//shapes overlap
							{
								//try combining them
								MapWinGIS.Shape intShp = new MapWinGIS.ShapeClass();
								intShp = SpatialOperations.Intersection(currShp, shp);
                                // Paul Meems: Related to bug 1068. Added check for correct shape:
                                if (intShp != null)
                                {
                                    if (intShp.numPoints != 0 && !intShp.IsValid)
                                    {
									    MapWinGIS.Shape resultShp = new MapWinGIS.ShapeClass();
									    resultShp = SpatialOperations.Union(currShp, shp);
                                        // Paul Meems: Added check for null:
                                        if (resultShp != null)
                                        {
                                            if (resultShp.numPoints != 0 && !resultShp.IsValid)
                                            {
                                                shpIndex = i;
                                                shpFile.EditDeleteShape(shpIndex);//delete original shape
                                                shpFile.EditInsertShape(resultShp, ref shpIndex);//replace with union result
                                                deleteMe[j] = true;
                                                numToDelete += 1;
                                            }
                                            else
                                            {
                                                gErrorMsg = "SpatialOperations.Union returned an invalid shape";
                                                Debug.WriteLine(gErrorMsg);
                                                Error.SetErrorMsg(gErrorMsg);
                                                MapWinUtility.Logger.Dbg(gErrorMsg);
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
							}//end of checking bounds
						}//end of checking orientation					
					}//end of j != i
				}//end of j comparison loop
				if(numToDelete > 0)
				{
					for(int j = numShapes-1; j >= 0; j--)
					{
						if(deleteMe[j] == true)
						{
							shpIndex = j;
							shpFile.EditDeleteShape(shpIndex);//delete the other shape
						}
					}
				}//end of deleting combo shapes
			}
		}
		#endregion

		#region BufferPointSF()
		//Angela Hillier 3/29/06
		/// <summary>
		/// Buffers all points in the point shapefile.
		/// </summary>
		/// <param name="pointSFPath">Full path to the shapefile of points.</param>
		/// <param name="resultSFPath">Full path to where the resulting buffer shapefile should be saved.</param>
		/// <param name="distance">Distance from point that buffer should be created.</param>
		/// <param name="uniteOverlaps">True if overlapping buffer shapes should be combined.</param>
		/// <param name="numQuadrants">Defines the smoothness of the circle.</param>
		/// <returns>False if an error was encountered, true otherwise.</returns>
		public static bool BufferPointSF(ref string pointSFPath, ref string resultSFPath, double distance, bool uniteOverlaps, int numQuadrants)
		{
			MapWinGIS.Shapefile pointSF = new MapWinGIS.ShapefileClass();
			pointSF.Open(pointSFPath, null);
			int numPoints = pointSF.NumShapes;
			MapWinGIS.Shapefile resultSF = new MapWinGIS.ShapefileClass();
			DataManagement.DeleteShapefile(ref resultSFPath);
			//CDM 8/4/2006 resultSF.CreateNew(resultSFPath, MapWinGIS.ShpfileType.SHP_POLYGON);
            // Skip ID field creation -- InsertFieldIDs will do this
            Globals.PrepareResultSF(ref resultSFPath, ref resultSF, MapWinGIS.ShpfileType.SHP_POLYGON, true);

			resultSF.StartEditingShapes(true, null);
			int shpIndex = 0;

            // Chris M 12/4/2006 -- preserve the original fields
            // if we're not dissolving overlaps.
            if (uniteOverlaps == false) PreserveAttributeDefns(ref pointSF, ref resultSF);

			for(int i = 0; i <= numPoints-1; i++)
			{
				MapWinGIS.Point point = new MapWinGIS.PointClass();
				point = pointSF.get_Shape(i).get_Point(0);
				MapWinGIS.Shape resultShp = new MapWinGIS.ShapeClass();
				resultShp.Create(MapWinGIS.ShpfileType.SHP_POLYGON);

				if(BufferPoint(ref point, distance, numQuadrants, out resultShp) == false)
				{
					return false;
				}
				else
				{
					if(resultShp.numPoints > 0)
					{
						shpIndex = resultSF.NumShapes;
						resultSF.EditInsertShape(resultShp, ref shpIndex);

                        // Chris M 12/4/2006 -- preserve the original fields
                        // if we're not dissolving overlaps.
                        if (uniteOverlaps == false) PreserveAttributeVals(ref pointSF, ref resultSF, i, shpIndex);
					}
				}
			}//end of buffering points

			if(uniteOverlaps == true && resultSF.NumShapes > 1)
			{
				UniteShapes(ref resultSF);
			}
            // Chris M 12/4/2006 -- preserve the original fields
            // if we're not dissolving overlaps. Therefore, only
            // insert ID fields if uniting overlaps.
            if (uniteOverlaps == true)
            {
                if (InsertFieldIDs(ref resultSF) == false)
                {
                    return false;
                }
            }
			if(resultSF.StopEditingShapes(true, true, null) == false)
			{
				gErrorMsg = "Problem with saving result file: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
				Debug.WriteLine(gErrorMsg);
				MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
				return false;
			}
			resultSF.SaveAs(resultSFPath, null);
			resultSF.Close();
			pointSF.Close();
			return true;
		}
		#endregion

		#region BufferLineSF()
		//Angela Hillier 3/29/06
		/// <summary>
		/// Buffers all lines in the input line shapefile.
		/// </summary>
		/// <param name="lineSFPath">Full path to the line shapefile.</param>
		/// <param name="resultSFPath">Full path to the resulting buffer shapefile.</param>
		/// <param name="distance">Distance (in units) from line at which the associated buffer should be created.</param>
		/// <param name="uniteOverlaps">True if overlapping shapes should be combined.</param>
		/// <param name="buffSide">Specify which side of the line to buffer. 0 = both, 1 = left, 2 = right</param>
		/// <param name="capStyle">Edge treatement. 0 = pointed, 1 = rounded.</param>
		/// <param name="endCapStyle">End treatment. 0 = pointed, 1 = rounded.</param>
		/// <param name="numQuadrants">Smoothness of rounded caps. Smaller values = smoother circle.</param>
		/// <returns>False if an error was encountered, true otherwise.</returns>
		public static bool BufferLineSF(ref string lineSFPath, ref string resultSFPath, double distance, bool uniteOverlaps, Enumerations.Buffer_LineSide buffSide, Enumerations.Buffer_CapStyle capStyle, Enumerations.Buffer_EndCapStyle endCapStyle, int numQuadrants)
		{
			MapWinGIS.Shapefile lineSF = new MapWinGIS.ShapefileClass();
			lineSF.Open(lineSFPath, null);
			int numPoints = lineSF.NumShapes;
			MapWinGIS.Shapefile resultSF = new MapWinGIS.ShapefileClass();
			DataManagement.DeleteShapefile(ref resultSFPath);
			//CDM 8/4/2006 resultSF.CreateNew(resultSFPath, MapWinGIS.ShpfileType.SHP_POLYGON);
            // Skip ID field creation -- InsertFieldIDs will do this
            Globals.PrepareResultSF(ref resultSFPath, ref resultSF, MapWinGIS.ShpfileType.SHP_POLYGON, true);
			resultSF.StartEditingShapes(true, null);
			int shpIndex = 0;

            // Chris M 12/4/2006 -- preserve the original fields
            // if we're not dissolving overlaps.
            if (uniteOverlaps == false) PreserveAttributeDefns(ref lineSF, ref resultSF);

			for(int i = 0; i <= numPoints-1; i++)
			{
				MapWinGIS.Shape line = new MapWinGIS.ShapeClass();
				line.Create(lineSF.ShapefileType);
				line = lineSF.get_Shape(i);
				MapWinGIS.Shape resultShp = new MapWinGIS.ShapeClass();
				resultShp.Create(MapWinGIS.ShpfileType.SHP_POLYGON);

				if(BufferLine(ref line, distance, buffSide, capStyle, endCapStyle, numQuadrants, out resultShp) == false)
				{
					return false;
				}
				else
				{
					if(resultShp.numPoints > 0)
					{
						shpIndex = resultSF.NumShapes;
						resultSF.EditInsertShape(resultShp, ref shpIndex);

                        // Chris M 12/4/2006 -- preserve the original fields
                        // if we're not dissolving overlaps.
                        if (uniteOverlaps == false) PreserveAttributeVals(ref lineSF, ref resultSF, i, shpIndex);
					}
				}
			}//end of buffering lines

			if(uniteOverlaps == true && resultSF.NumShapes > 1)
			{
				UniteShapes(ref resultSF);
			}
            // Chris M 12/4/2006 -- preserve the original fields
            // if we're not dissolving overlaps. Therefore, only
            // insert ID fields if uniting overlaps.
            if (uniteOverlaps == true)
            {
                if (InsertFieldIDs(ref resultSF) == false)
                {
                    return false;
                }
            }
			if(resultSF.StopEditingShapes(true, true, null) == false)
			{
				gErrorMsg = "Problem with saving result file: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
				Debug.WriteLine(gErrorMsg);
				MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
				return false;
			}
			resultSF.SaveAs(resultSFPath, null);
			resultSF.Close();
			lineSF.Close();
			return true;
		}
		#endregion

		#region BufferPolygonSF()
		//Angela Hillier 3/29/06
		/// <summary>
		/// Buffers all polygons in the input polygon shapefile.
		/// </summary>
		/// <param name="polySFPath">Full path to the polygon shapefile.</param>
		/// <param name="resultSFPath">Full path to the resulting buffer shapefile.</param>
		/// <param name="distance">Distance (in units) from polygon border at which the associated buffer should be created.</param>
		/// <param name="uniteOverlaps">True if overlapping shapes should be combined.</param>
		/// <param name="holeTreatment">How holes are to be treated. 0 == ignore, 1 == opposite, 2 == same</param>
		/// <param name="capStyle">Edge treatment. 0 = pointed, 1 = rounded.</param>
		/// <param name="numQuadrants">Smoothness of rounded edge. Smaller number = smoother circle.</param>
		/// <returns>False if an error was encountered, true otherwise.</returns>
		public static bool BufferPolygonSF(ref string polySFPath, ref string resultSFPath, double distance, bool uniteOverlaps, Enumerations.Buffer_HoleTreatment holeTreatment, Enumerations.Buffer_CapStyle capStyle, int numQuadrants)
		{
			MapWinGIS.Shapefile polySF = new MapWinGIS.ShapefileClass();
			polySF.Open(polySFPath, null);
			int numPoints = polySF.NumShapes;
			MapWinGIS.Shapefile resultSF = new MapWinGIS.ShapefileClass();
			DataManagement.DeleteShapefile(ref resultSFPath);
            //CDM 8/4/2006 resultSF.CreateNew(resultSFPath, MapWinGIS.ShpfileType.SHP_POLYGON);
            // Skip ID field creation -- InsertFieldIDs will do this
            Globals.PrepareResultSF(ref resultSFPath, ref resultSF, MapWinGIS.ShpfileType.SHP_POLYGON, true);
			resultSF.StartEditingShapes(true, null);
			int shpIndex = 0;

            // Chris M 12/4/2006 -- preserve the original fields
            // if we're not dissolving overlaps.
            if (uniteOverlaps == false) PreserveAttributeDefns(ref polySF, ref resultSF);

			for(int i = 0; i <= numPoints-1; i++)
			{
				MapWinGIS.Shape poly = new MapWinGIS.ShapeClass();
				poly.Create(polySF.ShapefileType);
				poly = polySF.get_Shape(i);
				MapWinGIS.Shape resultShp = new MapWinGIS.ShapeClass();
				resultShp.Create(MapWinGIS.ShpfileType.SHP_POLYGON);

				if(BufferPolygon(ref poly, distance, holeTreatment, capStyle, numQuadrants, out resultShp) == false)
				{
					return false;
				}
				else
				{
					if(resultShp.numPoints > 0)
					{
						shpIndex = resultSF.NumShapes;
						resultSF.EditInsertShape(resultShp, ref shpIndex);

                        // Chris M 12/4/2006 -- preserve the original fields
                        // if we're not dissolving overlaps.
                        if (uniteOverlaps == false) PreserveAttributeVals(ref polySF, ref resultSF, i, shpIndex);
					}
				}
			}//end of buffering lines

			if(uniteOverlaps == true && resultSF.NumShapes > 1)
			{
				UniteShapes(ref resultSF);
			}
            // Chris M 12/4/2006 -- preserve the original fields
            // if we're not dissolving overlaps. Therefore, only
            // insert ID fields if uniting overlaps.
            if (uniteOverlaps == true)
            {
                if (InsertFieldIDs(ref resultSF) == false)
                {
                    return false;
                }
            }
			if(resultSF.StopEditingShapes(true, true, null) == false)
			{
				gErrorMsg = "Problem with saving result file: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
				Debug.WriteLine(gErrorMsg);
				MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
				return false;
			}
			resultSF.SaveAs(resultSFPath, null);
			resultSF.Close();
			polySF.Close();
			return true;
		}
		#endregion

		#region UniteShapes
		//Angela Hillier
		/// <summary>
		/// Combines all overlapping shapes in the shapefile.
		/// </summary>
		/// <param name="shpFile">The shapefile to be modified.</param>
		private static void UniteShapes(ref MapWinGIS.Shapefile shpFile)
		{
			int shpIndex = 0;

			//note: shpFile.NumShapes will be changing during looping as
			//some shapes will be combined and others deleted. Therefore
			//DO NOT change the second loop expression to a constant.
			for(int i = 0; i <= shpFile.NumShapes-1; i++)
			{
				MapWinGIS.Shape currShp = new MapWinGIS.ShapeClass();
				currShp.Create(shpFile.ShapefileType);
				currShp = shpFile.get_Shape(i);

				//same note: shpFile.NumShapes will be changing during looping.
				for(int j = 0; j <= shpFile.NumShapes-1; j++)
				{
					MapWinGIS.Shape shp = new MapWinGIS.ShapeClass();
					shp.Create(shpFile.ShapefileType);
					shp = shpFile.get_Shape(j);

					if(j != i)
					{
						if(Globals.CheckBounds(ref currShp, ref shp) == true)
						{
							//try combining them
							MapWinGIS.Shape intShp = new MapWinGIS.ShapeClass();
							intShp = SpatialOperations.Intersection(currShp, shp);
                            // Paul Meems: Related to bug 1068. Added check for correct shape:
                            if (intShp != null)
                            {
                                if (intShp.numPoints != 0 && !intShp.IsValid)
                                {
								    MapWinGIS.Shape resultShp = new MapWinGIS.ShapeClass();
								    resultShp = SpatialOperations.Union(currShp, shp);
                                    // Paul Meems: Related to bug 1068. Added check for correct shape:
                                    if (resultShp != null)
                                    {
                                        if (resultShp.numPoints != 0 && !resultShp.IsValid)
                                        {
									        shpIndex = i;
									        shpFile.EditDeleteShape(shpIndex);//delete first shape in combo
									        shpFile.EditInsertShape(resultShp, ref shpIndex);//replace with union result
									        shpIndex = j;
									        shpFile.EditDeleteShape(shpIndex);//delete second shape in combo
									        //make the current shape the result shape for future comparisons.
									        currShp = resultShp;
									        j=0;//must retest this larger shape against all remaining shapes in the SF
                                        }
                                        else
                                        {
                                            gErrorMsg = "SpatialOperations.Union returned an invalid shape";
                                            Debug.WriteLine(gErrorMsg);
                                            Error.SetErrorMsg(gErrorMsg);
                                            MapWinUtility.Logger.Dbg(gErrorMsg);
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
							}//end of intersect exists
						}//end of bounds intersect
					}//end of if j!= i
				}//end of j comparison loop
			}
		}
		#endregion

		#region private InsertFieldIDs()
		//Angela Hillier
		/// <summary>
		/// Inserts the 'ID' attribute into the DBF table along 
		/// with ID values for each shape in the shapefile.
		/// </summary>
		/// <param name="resultSF">The shapefile that needs an ID field.</param>
		/// <returns>False if an error was encountered, true otherwise.</returns>
		private static bool InsertFieldIDs(ref MapWinGIS.Shapefile resultSF)
		{
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
			return true;
		}
		#endregion

        private static void PreserveAttributeDefns(ref MapWinGIS.Shapefile sf, ref MapWinGIS.Shapefile resultSF)
        {
            for (int i = 0; i <= sf.NumFields - 1; i++)
            {
                MapWinGIS.Field fld = new MapWinGIS.Field();
                fld.Key = sf.get_Field(i).Key;
                fld.Name = sf.get_Field(i).Name;
                fld.Precision = sf.get_Field(i).Precision;
                fld.Type = sf.get_Field(i).Type;
                fld.Width = sf.get_Field(i).Width;
                int k = i;
                resultSF.EditInsertField(fld, ref k, null);
            }
        }

        private static void PreserveAttributeVals(ref MapWinGIS.Shapefile sf, ref MapWinGIS.Shapefile resultSF, long origIdx, long destIdx)
        {
            for (int i = 0; i <= sf.NumFields - 1; i++)
            {
                resultSF.EditCellValue(i, (int)destIdx, sf.get_CellValue(i, (int)origIdx));
            }
        }

	}
}
