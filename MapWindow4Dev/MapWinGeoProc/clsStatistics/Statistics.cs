//********************************************************************************************************
//File name: Statistics.cs
//Description: Public class, provides methods for performing various calculations on geospatial data.
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
//1-11-06       ah - Angela Hillier - provided initial API and parameter descriptions.
//16. Mar.2008  jk - Jiri Kadlec - corrected calculation of areas for lat/long shapes, implementation of
//                   ComputeAreas() function  							
//********************************************************************************************************
using System;
using System.Diagnostics;

namespace MapWinGeoProc
{
	/// <summary>
	/// Statistics includes operations for finding things such as distance, area and length.
	/// </summary>
	public class Statistics
	{
		private static string gErrorMsg = "";

		#region Centroid()
		/// <summary>
		/// Finds the point that represents the "center of mass" for a polygon shape.
		/// </summary>
		/// <param name="polygon">The polygon shape.</param>
		/// <returns>The centroid: a point representing the center of mass of the polygon.</returns>
		public static MapWinGIS.Point Centroid(ref MapWinGIS.Shape polygon)
		{
            MapWinGIS.Point centroid = new MapWinGIS.PointClass();

            // Centroid function only works for polygons - return something useful instead of causing
            // crashes in client apps
            if (!(polygon.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGON || polygon.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGONZ || polygon.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGONM))
            {
                centroid.x = (polygon.Extents.xMax + polygon.Extents.xMin) / 2.0F;
                centroid.y = (polygon.Extents.yMax + polygon.Extents.yMin) / 2.0F;
                return centroid;
            }

			Error.ClearErrorLog();
            double area = Area(ref polygon);
			int numPoints = polygon.numPoints;
			int numParts = polygon.NumParts;
			double xSum = 0;
			double ySum = 0;
			if(numParts > 1)
			{
				//TO DO: determine how to handle this case
				//use the outermost part for finding the center????
				//what about island chains? mulitple centroids exist.
				gErrorMsg = "Sorry, centroid cannot be computed for a multi-part polygon.";
				Error.SetErrorMsg(gErrorMsg);
				Debug.WriteLine(gErrorMsg);
				return centroid;
			}
			else
			{
				for(int i =0; i<= numPoints-2; i++)
				{
					MapWinGIS.Point currPt = new MapWinGIS.PointClass();
					currPt = polygon.get_Point(i);
					MapWinGIS.Point nextPt = new MapWinGIS.PointClass();
					nextPt = polygon.get_Point(i+1);
					double cProduct = (currPt.x * nextPt.y) - (nextPt.x * currPt.y);
				
					xSum += (currPt.x + nextPt.x)* cProduct;
					ySum += (currPt.y + nextPt.y)* cProduct;
				}
			}
			centroid.x = xSum / (6*area);
			centroid.y = ySum / (6*area);

			if(centroid.x < 0 && centroid.y < 0)
			{
				if(polygon.get_Point(0).x > 0)
				{
					//translation incorrect, flip
					centroid.x = -1*centroid.x;
				}
				if(polygon.get_Point(0).y > 0)
				{
					centroid.y = -1*centroid.y;
				}
			}

			return centroid;		
		}
		#endregion

		#region Area()
		//see: http://astronomy.swin.edu.au/~pbourke/geometry/polyarea/
		//for a simple explanation on how to calculate the area of a polygon.
        // 16. Mar 2008 modified by Jiri Kadlec for shapes in lat/long coordinates.
		/// <summary>
		/// Computes the area of a polygon. For multi-part polygons, assume holes are counter-clockwise.
        /// To calculate the area correctly, the shape must have an equal-area projection. For shapes
        /// in Lat/Long coordinates use the LLArea() function.
		/// </summary>
		/// <param name="shape">The polygon shape.</param>
		/// <returns>The area in square units, or 0.0 if it could not be found.</returns>
		public static double Area(ref MapWinGIS.Shape shape)
		{
			Error.ClearErrorLog();
			double area;
			if(shape == null)
			{
				area = 0.0;
				gErrorMsg = "Unexpected null paramter: shape.";
				Error.SetErrorMsg(gErrorMsg);
				Debug.WriteLine(gErrorMsg);
				return area;
			}

			MapWinGIS.ShpfileType shpType;
			shpType = shape.ShapeType;

			if(shpType != MapWinGIS.ShpfileType.SHP_POLYGON && shpType != MapWinGIS.ShpfileType.SHP_POLYGONM && shpType != MapWinGIS.ShpfileType.SHP_POLYGONZ)
			{
				area = 0.0;
				gErrorMsg = "Incompatible shape type: must be of type polygon in order to calculate area.";
				Error.SetErrorMsg(gErrorMsg);
				Debug.WriteLine(gErrorMsg);
				return area;
			}
            
            double totalArea = 0.0;
			double indivArea = 0.0;

			int numParts = shape.NumParts;
			int numPoints = shape.numPoints;

			if(numParts > 1) //dealing with a multi-part polygon, hole areas must be subtracted from total
			{
				MapWinGIS.Shape[] allPolygons = new MapWinGIS.Shape[numParts];
				int begPart;
				int endPart;
				for(int i=0; i<= numParts-1; i++)
				{
					allPolygons[i] = new MapWinGIS.ShapeClass();
					allPolygons[i].ShapeType = shpType;

					begPart = shape.get_Part(i);
					if(i < numParts-1)
					{
						endPart = shape.get_Part(i+1);
					}
					else
						endPart = numPoints;

					int ptIndex = 0;
					for(int j = begPart; j <= endPart-1; j++)
					{
						allPolygons[i].InsertPoint(shape.get_Point(j), ref ptIndex);
						ptIndex++;
					}//end of creating separate polygons out of each part
				
					//calculate the area for each polygon part
					int numPolyPts = allPolygons[i].numPoints;
					indivArea = 0.0;
					for(int j = 0; j <= numPolyPts-2; j++)
					{
						double oneX = allPolygons[i].get_Point(j).x;
						double oneY = allPolygons[i].get_Point(j).y;
						double twoX = allPolygons[i].get_Point(j+1).x;
						double twoY = allPolygons[i].get_Point(j+1).y;

						double trapArea = ((oneX * twoY) - (twoX * oneY));
						indivArea += trapArea;
					}//end of calculating individual area for the current part
					totalArea += indivArea;
				}//end of looping through parts
				totalArea = 0.5 * Math.Abs(totalArea);
			}//end of dealing with multi-part polygons
			else
			{
				for(int i = 0; i <= numPoints-2; i++)
				{
					double oneX = shape.get_Point(i).x;
					double oneY = shape.get_Point(i).y;
					double twoX = shape.get_Point(i+1).x;
					double twoY = shape.get_Point(i+1).y;

					double trapArea = ((oneX * twoY) - (twoX * oneY));
					totalArea += trapArea;
				}
				totalArea = 0.5 * Math.Abs(totalArea);
			}

			return totalArea;
		}

        /// <summary>
        /// Calculate area of shapes in lat/long coordinates.
        /// The shape coordinates must be in decimal degrees. It is assumed that the WGS-84
        /// ellipsoid is used - this can result in small errors if the coordinate system of the
        /// shape is based on a different ellipsoid.
        /// Added by Jiri Kadlec based on the UpdateMeasurements plugin code by Paul Meems.
        /// </summary>
        /// <param name="shp">The polygon shape (must have coordinates in decimal degrees)</param>
        /// <returns>Area of shape in square kilometres</returns>
        public static double LLArea(ref MapWinGIS.Shape shp)
        {
            //for smaller shapes, an approximation is used (area correction factor for latitude).
            //for very large shapes (extent in latitude > 1 decimal degree, use the spherical polygon
            //area calculation algorithm.

            double maxLatitudeExtent = 1.0; //one decimal degree (~110 km)
            double area; //area - in square kilometres

            if (Math.Abs(shp.Extents.yMax - shp.Extents.yMin) < maxLatitudeExtent)
            {
                // use latitude corrected area approximation
                area = MapWinGeoProc.Utils.Area(ref shp);
                if (area < 0) area = -1 * area;
                area = (area / LatitudeCorrectedArea(ref shp)) * 0.01;
            }
            else
            {
                //for big shapes, use the spheric polygon area calculation (added by JK)
                area = SphericPolygonArea(ref shp);
            }

            return area;
        }

        private static double LatitudeCorrectedArea(ref MapWinGIS.Shape shp)
        {
            // area per square degree changes as a function of latitude
            // these are function parameters for a 'best-fit' equation derived from a series of known-size squares
            // that were established at different latitudes from 0 to 90

            try
            {
                double x0 = 2.5973 * Math.Pow(10, -6);
                double x1 = -1.6551 * Math.Pow(10, -7);
                double x2 = 5.8911 * Math.Pow(10, -9);
                double x3 = -9.0237 * Math.Pow(10, -11);
                double x4 = 5.5763 * Math.Pow(10, -13);

                //use the average latitude of the shape to determine the correction factor
                //1/24/2009 JK - latitude must be multiplied by (-1) for southern hemisphere.

                double latitude = (shp.Extents.yMax + shp.Extents.yMin) / 2;
                if (latitude < 0)
                {
                    latitude = latitude * -1;
                }
                double CorrectedFactor = (x0
                    + latitude * x1
                    + Math.Pow(latitude, 2) * x2
                    + Math.Pow(latitude, 3) * x3
                    + Math.Pow(latitude, 4) * x4);

                return CorrectedFactor;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in LatitudeCorrectedArea: \n" + ex.ToString());
            }
        }

        /// <summary>
        /// calculate the area of large shapes in lat/long coordinates.
        /// The coordinates must be in decimal degrees
        /// the return value is in square kilometres
        /// </summary>
        /// <param name="shape">Polygon shape (decimal degree coordinates)</param>
        /// <returns>Area of the shape in square kilometres</returns>
        private static double SphericPolygonArea(ref MapWinGIS.Shape shape)
        {
            // added by Jiri Kadlec 3/16/2008
            //
            // for each part of shape:
            // convert ellipsoid latitude to a latitude on equal-area unit sphere
            // convert the sphere coordinates from long/lat to x/y of sinusoidal equal-area projection
            // (x, y is in radians)
            // calculate area of the part
            // if it's a normal polygon, add it and if it's a hole, subtract it from total
            // area of shape
            //
            //note: this function uses the sphere approximation. It should be
            //used for large shapes with |max.latitude - min.latitude| > 1 degree.
            //please refer to: http://www.ucl.ac.uk/~ucessan/news/bob-aag2007.ppt
            // and: http://www.gmat.unsw.edu.au/wang/jgps/v5n12/v5n12p02.pdf

            //radius of equal-area sphere corresponding to WGS 84 ellipsoid
            double EARTH_RADIUS_SQUARED = Math.Pow(EqualAreaSphereRadius(), 2);

            //calculation modified from MapWinGeoProc.Statistics.Area
            double totalArea = 0.0;
            double indivArea = 0.0;

            int numParts = shape.NumParts;
            int numPoints = shape.numPoints;

            if (numParts > 1) //dealing with a multi-part polygon, hole areas must be subtracted from total
            {
                MapWinGIS.Shape[] allPolygons = new MapWinGIS.Shape[numParts];
                int begPart;
                int endPart;
                for (int i = 0; i <= numParts - 1; i++)
                {
                    allPolygons[i] = new MapWinGIS.ShapeClass();
                    allPolygons[i].ShapeType = shape.ShapeType;

                    begPart = shape.get_Part(i);
                    if (i < numParts - 1)
                    {
                        endPart = shape.get_Part(i + 1);
                    }
                    else
                        endPart = numPoints;

                    int ptIndex = 0;
                    for (int j = begPart; j <= endPart - 1; j++)
                    {
                        allPolygons[i].InsertPoint(shape.get_Point(j), ref ptIndex);
                        ptIndex++;
                    }//end of creating separate polygons out of each part

                    //calculate the area for each polygon part
                    int numPolyPts = allPolygons[i].numPoints;
                    indivArea = 0.0;
                    for (int j = 0; j <= numPolyPts - 2; j++)
                    {
                        double oneY = EqualAreaSphereLat(Utils.deg2rad(allPolygons[i].get_Point(j).y));
                        double oneX = Utils.deg2rad(allPolygons[i].get_Point(j).x) * Math.Cos(oneY);
                        double twoY = EqualAreaSphereLat(Utils.deg2rad(allPolygons[i].get_Point(j + 1).y));
                        double twoX = Utils.deg2rad(allPolygons[i].get_Point(j + 1).x) * Math.Cos(twoY);

                        double trapArea = ((oneX * twoY) - (twoX * oneY));
                        indivArea += trapArea;
                    }//end of calculating individual area for the current part
                    totalArea += indivArea;
                }//end of looping through parts
                totalArea = 0.5 * Math.Abs(totalArea) * EARTH_RADIUS_SQUARED;
            }//end of dealing with multi-part polygons
            else
            {
                for (int j = 0; j <= numPoints - 2; j++)
                {
                    double oneY = EqualAreaSphereLat(Utils.deg2rad(shape.get_Point(j).y));
                    double oneX = Utils.deg2rad(shape.get_Point(j).x) * Math.Cos(oneY);
                    double twoY = EqualAreaSphereLat(Utils.deg2rad(shape.get_Point(j + 1).y));
                    double twoX = Utils.deg2rad(shape.get_Point(j + 1).x) * Math.Cos(twoY);

                    double trapArea = ((oneX * twoY) - (twoX * oneY));
                    totalArea += trapArea;
                }
                totalArea = 0.5 * Math.Abs(totalArea) * EARTH_RADIUS_SQUARED;
            }

            return totalArea;
        }

        private static double EqualAreaSphereLat(double EllipsoidLat)
        {
            // 3/16/2008 added by Jiri Kadlec
            //convert a latitude from WGS-84 ellipsoid to an equal-area sphere latitude
            //(WGS-84 ellipsoid is assumed)
            // Please refer to: http://www.gmat.unsw.edu.au/wang/jgps/v5n12/v5n12p02.pdf
            const double ECCENTRICITY = 8.1819190842622e-2;
            const double E_2 = ECCENTRICITY * ECCENTRICITY;
            double phi = EllipsoidLat;

            double q = (1 - E_2) * (Math.Sin(phi) / (1 - E_2 * Math.Pow(Math.Sin(phi), 2.0)) -
                (1 / 2 * E_2) *
                    Math.Log((1 - ECCENTRICITY * Math.Sin(phi)) / (1 + ECCENTRICITY * Math.Sin(phi))));

            double qp = 1 + ((1 - E_2) / 2 * ECCENTRICITY) *
                Math.Log((1 + ECCENTRICITY) / (1 - ECCENTRICITY));

            double beta = Math.Asin(q / qp);
            return beta;
        }

        private static double EqualAreaSphereRadius()
        {
            // 3/16/2008 added by Jiri Kadlec
            // calculates a radius of a sphere in kilometres that has the same surface area
            // as the WGS-84 ellipsoid
            // Please refer to: http://www.gmat.unsw.edu.au/wang/jgps/v5n12/v5n12p02.pdf
            const double ECCENTRICITY = 8.1819190842622e-02;
            const double E_2 = ECCENTRICITY * ECCENTRICITY;
            const double SEMIMAJOR_AXIS = 6378.1370;

            double qp = 1 + ((1 - E_2) / (2 * ECCENTRICITY)) *
                Math.Log((1 + ECCENTRICITY) / (1 - ECCENTRICITY));
            double radius = SEMIMAJOR_AXIS * Math.Sqrt(qp / 2);
            return radius;
        }

		#endregion

		/// <summary>
		/// Not Implemented
		/// This function will calculate the area of every polygon shape in the shapefile and write the results to the corresponding .dbf table.
		/// </summary>
		/// <param name="polySF">The shapefile of polygons whose areas are to be computed.</param>
		/// <returns>False if an error was encountered, true otherwise.</returns>
		public static bool ComputeAreas(MapWinGIS.Shapefile polySF)
		{
			//TODO: Implement this function
			Error.ClearErrorLog();
			gErrorMsg = "This function is not yet implemented.";
			Error.SetErrorMsg(gErrorMsg);
			Debug.WriteLine(gErrorMsg);

			return false;
		}

		/// <summary>
		/// Not Implemented
		/// This function will calculate the length of every line shape in the shapefile and save the results to the corresponding .dbf table.
		/// </summary>
        /// <param name="sf">The shapefile of lines whose lengths are to be computed.</param>
        /// <param name="fieldIndex">The field index of the field to update with values.</param>
        /// <param name="Units">The units of the dataset (e.g., Meters, Lat/Long).</param>
		/// <returns>False if an error was encountered, true otherwise.</returns>
		public static bool ComputeLengths(MapWinGIS.Shapefile sf, int fieldIndex, string Units)
        {
            try
            {
                //Loop trough all shapes		
                for (int i = 0; i <= sf.NumShapes - 1; i++)
                {
                    double length = 0f;
                    MapWinGIS.Shape line = new MapWinGIS.Shape();
                    //Measure length of each line part
                    line = sf.get_Shape(i);
                    //-2 else out of bounds!!
                    for (int j = 0; j <= line.numPoints - 2; j++)
                    {
                        length += DistancePointToPoint(line.get_Point(j).x, line.get_Point(j).y, line.get_Point(j + 1).x, line.get_Point(j + 1).y, Units);
                    }

                    //Add length as attribute:
                    if (!sf.EditCellValue(fieldIndex, i, length)) Error.SetErrorMsg(sf.get_ErrorMsg(sf.LastErrorCode));
                }

                return false;
            }
            catch (Exception lEx)
            {
                Error.SetErrorMsg(lEx.ToString());
                return false;
            }
        }

        /// <summary>
        /// Computes distance from two latitude and longitude points.
        /// This is somewhat error prone, as latitude and longitude require many
        /// admustments to make them true representations without loss of area.
        /// This function is a very good spheroid approximation.
        /// </summary>
        /// <param name="p1lat"></param>
        /// <param name="p1lon"></param>
        /// <param name="p2lat"></param>
        /// <param name="p2lon"></param>
        /// <returns></returns>
        public static double LLDistance(double p1lat, double p1lon, double p2lat, double p2lon)
        {
            double FLATTENING = 1 / 298.257223563;
            int KILOMETER2METER = 1000;
            //This should be read from the projectfile

            double lat1 = Utils.deg2rad(p1lat);
            double lon1 = Utils.deg2rad(p1lon);
            double lat2 = Utils.deg2rad(p2lat);
            double lon2 = Utils.deg2rad(p2lon);

            double F = (lat1 + lat2) / 2.0;
            double G = (lat1 - lat2) / 2.0;
            double L = (lon1 - lon2) / 2.0;

            double sing = Math.Sin(G);
            double cosl = Math.Cos(L);
            double cosf = Math.Cos(F);
            double sinl = Math.Sin(L);
            double sinf = Math.Sin(F);
            double cosg = Math.Cos(G);

            double S = sing * sing * cosl * cosl + cosf * cosf * sinl * sinl;
            double C = cosg * cosg * cosl * cosl + sinf * sinf * sinl * sinl;
            double W = Math.Atan2(Math.Sqrt(S), Math.Sqrt(C));
            double R = Math.Sqrt((S * C)) / W;
            double H1 = (3 * R - 1.0) / (2.0 * C);
            double H2 = (3 * R + 1.0) / (2.0 * S);
            double D = 2 * W * 6378.135;
            return (D * (1 + FLATTENING * H1 * sinf * sinf * cosg * cosg - FLATTENING * H2 * cosf * cosf * sing * sing)) * KILOMETER2METER;
        }

		/// <summary>
		/// Computes the Euclidean distance between two points.
		/// </summary>
		/// <param name="x1">The first point.</param>
		/// <param name="x2">The second point.</param>
        /// <param name="y1">The first point.</param>
        /// <param name="y2">The second point.</param>
        /// <param name="Units">The units of the data (e.g., "Meters", "Lat/Long")</param>
		/// <returns>The distance between the first and second point, else 0 if an error was encountered.</returns>
		public static double DistancePointToPoint(double x1, double y1, double x2, double y2, string Units)
        {
            if ((Units.ToLower().Trim() == "lat/long"))
            {
                return LLDistance(y1, x1, y2, x2); // Jiri Kadlec - corrected order of parameters
            }
            else
            {
                return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
            }
        }

        /// <summary>
        /// Overloaded version - Computes the Euclidean distance between two points.
        /// </summary>
        /// <param name="x1">The first point.</param>
        /// <param name="x2">The second point.</param>
        /// <param name="y1">The first point.</param>
        /// <param name="y2">The second point.</param>
        /// <param name="Units">The units of the data (e.g., "Meters", "Lat/Long")</param>
        /// <returns>The distance between the first and second point, else 0 if an error was encountered.</returns>
        public static double DistancePointToPoint(double x1, double y1, double x2, double y2, MapWindow.Interfaces.UnitOfMeasure Units)
        {
            if (Units == MapWindow.Interfaces.UnitOfMeasure.DecimalDegrees)
            {
                return LLDistance(y1, x1, y2, x2); 
            }
            else
            {
                return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
            }
        }


		/// <summary>
		/// Not Implemented
		/// Computes the distance between a point and a line.
		/// </summary>
		/// <param name="pt">The point to be considered.</param>
		/// <param name="line">The line to be considered.</param>
		/// <param name="distType">Indicates whether the distance should be from the nearest, farthest, or centroid of the line.</param>
		/// <returns>The distance from the line, else 0 if an error was encountered.</returns>
		public static double DistancePointToLine(MapWinGIS.Point pt, MapWinGIS.Shape line, int distType)
		{
			//TODO: Implement this function
			Error.ClearErrorLog();
			gErrorMsg = "This function is not yet implemented.";
			Error.SetErrorMsg(gErrorMsg);
			Debug.WriteLine(gErrorMsg);

			return 0;
		}

		/// <summary>
		/// Not implemented
		/// Computes the distance between a point and a polygon.
		/// </summary>
		/// <param name="pt">The point to be considered.</param>
		/// <param name="polygon">The line to be considered.</param>
		/// <param name="distType">Indicates whether the distance should be from the nearest, farthest, or centroid of the polygon.</param>
		/// <returns></returns>
		public static double DistancePointToPolygon(MapWinGIS.Point pt, MapWinGIS.Shape polygon, int distType)
		{
			//TODO: Implement this function
			Error.ClearErrorLog();
			gErrorMsg = "This function is not yet implemented.";
			Error.SetErrorMsg(gErrorMsg);
			Debug.WriteLine(gErrorMsg);

			return 0;
		}

		#region Distance()
		//		public static double Distance(ref MapWinGIS.Shape shp1, ref MapWinGIS.Shape shp2)
		//		{
		//			MapWinGIS.ShpfileType shp1Type = shp1.ShapeType;
		//			MapWinGIS.ShpfileType shp2Type = shp2.ShapeType;
		//
		//			if(shp1Type == MapWinGIS.ShpfileType.SHP_POINT || shp1Type == MapWinGIS.ShpfileType.SHP_POINTM || shp1Type == MapWinGIS.ShpfileType.SHP_POINTZ)
		//			{
		//				//shp1 is a point type
		//				if(shp2Type == MapWinGIS.ShpfileType.SHP_POINT || shp2Type == MapWinGIS.ShpfileType.SHP_POINTM || shp2Type == MapWinGIS.ShpfileType.SHP_POINTZ)
		//				{
		//					//shp2 is a point type
		//					MapWinGIS.Point p0 = new MapWinGIS.PointClass();
		//					p0 = shp1.get_Point(0);
		//					MapWinGIS.Point p1 = new MapWinGIS.PointClass();
		//					p1 = shp2.get_Point(0);
		//					return Globals.PtDistance(ref p0, ref p1);
		//				}
		//				else if(shp2Type == MapWinGIS.ShpfileType.SHP_POLYGON || shp2Type == MapWinGIS.ShpfileType.SHP_POLYGONM || shp2Type == MapWinGIS.ShpfileType.SHP_POLYGONZ)
		//				{
		//					//shp2 is a polygon type
		//					MapWinGIS.UtilsClass utils = new MapWinGIS.UtilsClass();
		//					
		//				}
		//				else if(shp2Type == MapWinGIS.ShpfileType.SHP_POLYLINE || shp2Type == MapWinGIS.ShpfileType.SHP_POLYLINEM || shp2Type == MapWinGIS.ShpfileType.SHP_POLYLINEZ)
		//				{
		//					//shp2 is a line type
		//				}
		//				else
		//				{
		//					gErrorMsg = "shp2 is not a valid type.";
		//					Error.SetErrorMsg(gErrorMsg);
		//					Debug.WriteLine(gErrorMsg);
		//					return 0.0;
		//				}
		//
		//			}
		//			else if(shp1Type == MapWinGIS.ShpfileType.SHP_POLYGON || shp1Type == MapWinGIS.ShpfileType.SHP_POLYGONM || shp1Type == MapWinGIS.ShpfileType.SHP_POLYGONZ)
		//			{
		//				//shp1 is a polygon type
		//				if(shp2Type == MapWinGIS.ShpfileType.SHP_POINT || shp2Type == MapWinGIS.ShpfileType.SHP_POINTM || shp2Type == MapWinGIS.ShpfileType.SHP_POINTZ)
		//				{
		//					//shp2 is a point type
		//				}
		//				else if(shp2Type == MapWinGIS.ShpfileType.SHP_POLYGON || shp2Type == MapWinGIS.ShpfileType.SHP_POLYGONM || shp2Type == MapWinGIS.ShpfileType.SHP_POLYGONZ)
		//				{
		//					//shp2 is a polygon type
		//				}
		//				else if(shp2Type == MapWinGIS.ShpfileType.SHP_POLYLINE || shp2Type == MapWinGIS.ShpfileType.SHP_POLYLINEM || shp2Type == MapWinGIS.ShpfileType.SHP_POLYLINEZ)
		//				{
		//					//shp2 is a line type
		//				}
		//				else
		//				{
		//					gErrorMsg = "shp2 is not a valid type.";
		//					Error.SetErrorMsg(gErrorMsg);
		//					Debug.WriteLine(gErrorMsg);
		//					return 0.0;
		//				}
		//			}
		//			else if(shp1Type == MapWinGIS.ShpfileType.SHP_POLYLINE || shp1Type == MapWinGIS.ShpfileType.SHP_POLYLINEM || shp1Type == MapWinGIS.ShpfileType.SHP_POLYLINEZ)
		//			{
		//				//shp1 is a line type
		//				if(shp2Type == MapWinGIS.ShpfileType.SHP_POINT || shp2Type == MapWinGIS.ShpfileType.SHP_POINTM || shp2Type == MapWinGIS.ShpfileType.SHP_POINTZ)
		//				{
		//					//shp2 is a point type
		//				}
		//				else if(shp2Type == MapWinGIS.ShpfileType.SHP_POLYGON || shp2Type == MapWinGIS.ShpfileType.SHP_POLYGONM || shp2Type == MapWinGIS.ShpfileType.SHP_POLYGONZ)
		//				{
		//					//shp2 is a polygon type
		//				}
		//				else if(shp2Type == MapWinGIS.ShpfileType.SHP_POLYLINE || shp2Type == MapWinGIS.ShpfileType.SHP_POLYLINEM || shp2Type == MapWinGIS.ShpfileType.SHP_POLYLINEZ)
		//				{
		//					//shp2 is a line type
		//				}
		//				else
		//				{
		//					gErrorMsg = "shp2 is not a valid type.";
		//					Error.SetErrorMsg(gErrorMsg);
		//					Debug.WriteLine(gErrorMsg);
		//					return 0.0;
		//				}
		//			}
		//			else
		//			{
		//				gErrorMsg = "shp1 is not a valid type.";
		//				Error.SetErrorMsg(gErrorMsg);
		//				Debug.WriteLine(gErrorMsg);
		//				return 0.0;
		//			}
		//		}
		#endregion

	}
}
