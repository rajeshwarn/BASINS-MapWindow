//********************************************************************************************************
//7-04-07 sb - Simon Batson - Fixed error in MULTI LINE STRING processing
//********************************************************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using MapWinGeoProc.NTS.Topology.Geometries;
using MapWindow.Interfaces.Geometries;
namespace MapWinGeoProc
{
    /// <summary>
    /// This is a standard adapter that can convert between MapWinGIS.Shapes and Geometries
    /// </summary>
    /// <remarks>NOTE!  You will lose any Z or M information by using these tools!</remarks>
    public static class NTS_Adapter
    {
        /// <summary>
        /// Converts a MapWinGIS.Shape of any type to the associated geometry.
        /// Clockwise/Counter clockwise arrangements are important for polygons.
        /// Any hole that isn't correctly counter-clockwise will be treated as a polygon.
        /// Any clockwise "hole" that is not contained by at least one polygon will be
        /// reversed to clockwise and returned as a polygon.
        /// NULLSHAPE || MULTIPATCH --> throws exception.
        /// POINT || POINTM || POINTZ --> Geometries.Point
        /// MULTIPOINT || MULTIPOINTM || MULTIPOINTZ --> Geometries.MultiPoint
        /// {1 part} POLYLINE || POLYLINEM || POLYLINEZ --> Geometires.LineString
        /// {n parts} POLYLINE || POLYLINEM || POLYLINEZ --> Geometires.MultiLineString
        /// {1 shell} POLYGON || POLYGONM || POLYGONZ --> Geometires.Polygon
        /// {n shells} POLYGON || POLYGONM || POLYGONZ --> Geometries.MultiPolygon
        /// </summary>
        /// <param name="mwShape">A MapWinGIS.Shape to convert to a Geometry</param>
        /// <returns>Geometry representing the shape specified by mwShape.</returns>
        /// 
        public static Geometry ShapeToGeometry(MapWinGIS.Shape mwShape)
        {
            #region -------------------------- UNSUPPORTED -------------------------------------

            MapWinGIS.Point mwPoint; // a generic MapWinGIS style point

            // ------------ NOT SUPPORTED
            if (mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_NULLSHAPE ||
               mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPATCH)
            {
                throw new ArgumentException("Shape type is not supported.");
            }
            #endregion

            #region -------------------------- POINT -------------------------------------------
            
            // ------------- POINT
            if (mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POINT ||
               mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POINTM ||
               mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POINTZ)
            {
                // The shape represents a single point, so return a single point
                Coordinate Coord = new Coordinate(mwShape.get_Point(0).x, mwShape.get_Point(0).y);
                return new Point(Coord);
            }
            
            #endregion

            #region -------------------------- MULTI POINT -------------------------------------
           
            if (mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPOINT ||
               mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPOINTM ||
               mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPOINTZ)
            {
                Point[] Coords = new Point[mwShape.numPoints];
                for (int pt = 0; pt < mwShape.numPoints; pt++)
                {
                    mwPoint = mwShape.get_Point(pt);
                    Coords[pt] = new Point(mwPoint.x, mwPoint.y);
                }
                return new MultiPoint(Coords);
            }
            #endregion

            #region -------------------------- LINE STRINGS ------------------------------------
            

            if (mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYLINE ||
                mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYLINEM ||
                mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYLINEZ)
            {
               
                // ------------- LINE STRING
                if (mwShape.NumParts < 2)
                {
                    Coordinate[] Coords = new Coordinate[mwShape.numPoints];
                    for (int pt = 0; pt < mwShape.numPoints; pt++)
                    {
                        mwPoint = mwShape.get_Point(pt);
                        Coords[pt] = new Coordinate(mwPoint.x, mwPoint.y);
                    }
                    return new LineString(Coords);

                }
               
                // ------------- MULTI LINE STRING
                else
                {
                    int pt = 0;
                    LineString[] Lines = new LineString[mwShape.NumParts];
                    for (int prt = 0; prt < mwShape.NumParts; prt++)
                    {
                        int Start = mwShape.get_Part(prt);
                        int End;
                        if(prt == mwShape.NumParts -1)
                        {
                            End = mwShape.numPoints;
                        }
                        else
                        {
                            End = mwShape.get_Part(prt+1);
                        }
                        Coordinate[] Coords = new Coordinate[End - Start];
                        for (int PrtPoint = 0; PrtPoint < End - Start; PrtPoint++)
                        {
                            // PrtPoint will be tracking the index for the specific linestring
                            mwPoint = mwShape.get_Point(pt);
                            Coords[PrtPoint] = new Coordinate(mwPoint.x, mwPoint.y);
                            pt++;
                        }
                        // Create a new linestring using only the coordinates for a specific part
                        Lines[prt] = new LineString(Coords);
                    }
                    return new MultiLineString(Lines);
                }

            }
            #endregion

            #region -------------------------- POLYGONS ----------------------------------------
            
            if (mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGON ||
               mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGONM ||
               mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGONZ)
            {
                // Polygons are complex and not as straight forward as linear rings.
                // A multi-part shape might just be a single polygon with a hole.
                
                List<NRings> Rings = new List<NRings>();
                List<LinearRing> Holes = new List<LinearRing>();
                int pt = 0;
                int prt = 0;
                List<LinearRing> RawRings = new List<LinearRing>();
                do
                {
                    int Start = mwShape.get_Part(prt);
                    int End;
                    if (prt >= mwShape.NumParts - 1)
                    {
                        End = mwShape.numPoints;
                    }
                    else
                    {
                        End = mwShape.get_Part(prt + 1);
                    }
                    // BUG #1350: Include part if not empty
                    if (End > Start)
                    {
                        int Count = End - Start + 1;
                        Coordinate[] Coords = new Coordinate[Count];
                        double area = 0;
                        for (int PrtPoint = 0; PrtPoint < Count - 1; PrtPoint++)
                        {
                            // PrtPoint will be tracking the index for the specific linestring
                            mwPoint = mwShape.get_Point(pt);
                            Coords[PrtPoint] = new Coordinate(mwPoint.x, mwPoint.y);
                            if (PrtPoint > 0)
                            {
                                area += Coords[PrtPoint].X * Coords[PrtPoint - 1].Y - Coords[PrtPoint - 1].X * Coords[PrtPoint].Y;
                            }
                            pt++;
                        }
                        mwPoint = mwShape.get_Point(Start); // close the loop
                        Coords[Count - 1] = new Coordinate(mwPoint.x, mwPoint.y);
                        area += Coords[Count - 2].X * Coords[Count - 1].Y - Coords[Count - 1].X * Coords[Count - 2].Y;

                        // if the area is negative, then the shape is counter-clockwise and therefore a hole
                        if (area < 0)
                        {
                            Holes.Add(new LinearRing(Coords));
                        }
                        else
                        {
                            Rings.Add(new NRings(Coords));
                        }
                    }                   
                    prt++;
                } while (prt < mwShape.NumParts);
                // Add the holes to every ring that can hold it.
                for (int iHole = 0; iHole < Holes.Count; iHole++)
                {
                    bool HolePunched = false;
                    for (int iRing = 0; iRing < Rings.Count; iRing++)
                    {
                        if (Rings[iRing].Ring.Contains(Holes[iHole]))
                        {
                            Rings[iRing].Holes.Add(Holes[iHole]);
                            HolePunched = true;
                        }
                    }
                    // If we couldn't punch the hole anywhere, then we will turn it into a polygon
                    if (HolePunched == false)
                    {
                        ILineString LS = Holes[iHole].Reverse(); //Switch to clockwise
                        Rings.Add(new NRings(LS.Coordinates));
                    }
                }

                // Create polygons from each of the rings and add them to a multipolygon
                if (Rings.Count == 1)
                {
                    if (Rings[0].Holes.Count > 0)
                    {
                        LinearRing[] HoleArray = new LinearRing[Rings[0].Holes.Count];
                        for (int iHole = 0; iHole < Rings[0].Holes.Count; iHole++)
                        {
                            HoleArray[iHole] = Rings[0].Holes[iHole];
                        }
                        return new Polygon(Rings[0].Ring, HoleArray);
                    }
                    else
                    {
                        return new Polygon(Rings[0].Ring);
                    }
                }
                Polygon[] Polygons = new Polygon[Rings.Count];
                for (int iRing = 0; iRing < Rings.Count; iRing++)
                {
                    if (Rings[iRing].Holes.Count > 0)
                    {
                        LinearRing[] HoleArray = new LinearRing[Rings[iRing].Holes.Count];
                        for (int iHole = 0; iHole < Rings[iRing].Holes.Count; iHole++)
                        {
                            HoleArray[iHole] = Rings[0].Holes[iHole];
                        }
                        Polygons[iRing] = new Polygon(Rings[iRing].Ring, HoleArray);
                    }
                    else
                    {
                        Polygons[iRing] = new Polygon(Rings[iRing].Ring);
                    }
                }
                return new MultiPolygon(Polygons);
            }
            #endregion

            // There shouldn't be any way to get here.
            throw new ApplicationException("The shapetype specified is invalid, or new and not handled yet.");

           
        }

        /// <summary>
        /// Determines whether a closed list of coordinates (last point = first) is clockwise
        /// </summary>
        /// <param name="Coords">A list of coordinates like those used in Geometries</param>
        /// <returns>Boolean, true if the list is clockwise</returns>
        public static bool IsClockwise(ICoordinate[] Coords)
        {
            double area = MapWinGeoProc.NTS.Topology.Algorithm.CGAlgorithms.SignedArea(Coords);

            // A clockwise shape using the formula above will have a positive area and not be a hole
            if (area > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// This one way function will convert most geometries into a valid MapWinGIS Shape object.
        /// </summary>
        /// <param name="geom">Point, MultiPoint, LineString, MultiLineString, LinearRing, Polygon, MultiPolygon</param>
        /// <returns>A MapWinGIS.Shape that will hold the specified geometries.</returns>
        /// <remarks>Shapes are not TypeZ or TypeM!  Adding them to the wrong shapefile can cause problems.</remarks>
        public static MapWinGIS.Shape GeometryToShape(IGeometry geom)
        {
            // We can just call AppendGeometryToShape from here after using a null parameter.
            MapWinGIS.Shape mwShape = null;
            AppendGeometryToShape(geom, ref mwShape);
            return mwShape;
        }
        /// <summary>
        /// This function lets the user set up whether the shape should be regular, type M or type Z.
        /// This will not interfere with existing shapes in the geometry, but will simply
        /// add the specified geometry to the end of the existing information.  Sending a null shape
        /// will create a new shape of the appropriate kind.
        /// </summary>
        /// <param name="geom">A Geometries.Geometry specifying what to append to the shape.
        /// Simply returns mwShape unchanged if geom is null.</param>
        /// <param name="mwShape">An existing shape with a shapetype already specified.</param>
        public static void AppendGeometryToShape(IGeometry geom, ref MapWinGIS.Shape mwShape)
        {
            // we don't need to throw an error if geom is null, there is simply nothing to add.
            if (geom == null) return; 
            
            // If we have to set up a new shape object, we will also have to specify a shape type.
            bool SpecifyShapeType = false;
            if (mwShape == null)
            {
                mwShape = new MapWinGIS.Shape();
                SpecifyShapeType = true;
            }
            MapWinGIS.Point mwPoint;
            int rfPt = mwShape.numPoints; // Flexible Point Index 
            int rfPrt = mwShape.NumParts; // Flexible Part Index
            if (rfPt > 0 && rfPrt == 0)
            {
                //Change single part to a multipart shapefile
                mwShape.InsertPart(0, ref rfPrt);
                rfPrt++;
            }

            #region -------------------------- POINT -------------------------------------------

            if (geom.GetType() == typeof(Point))
            {
                if (SpecifyShapeType == true) mwShape.ShapeType = MapWinGIS.ShpfileType.SHP_POINT;
                // Only allow appending to a point shape type if there isn't a point defined yet.
                if ((mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPOINT ||
                   mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPOINTM ||
                   mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPOINTZ) || 
                   ((mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POINT ||
                   mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POINTM ||
                   mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POINTZ) && rfPt == 0))
                {
                    Point newPoint = geom as Point;
                    mwPoint = new MapWinGIS.Point();
                    mwPoint.x = newPoint.X;
                    mwPoint.y = newPoint.Y;
                    mwShape.InsertPoint(mwPoint, ref rfPt);
                    return;
                }
                else
                {
                    if ((mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POINT ||
                         mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POINTM ||
                         mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POINTZ))
                    {
                        throw new ArgumentException("The Shape type of mwShape must be MultiPoint, MultiPointM, or MultiPointZ in order to Append a Point Geometry to a shape that already contains a point.");
                    }
                    else
                    {
                        throw new ArgumentException("mwShape had a shape type that did not correspond to Point or MultiPoint.  Point geometry not added.");
                    }
                }
                   
            }

            #endregion

            #region -------------------------- MULTI POINT--------------------------------------

            if (geom.GetType() == typeof(MultiPoint))
            {
                if (SpecifyShapeType == true) mwShape.ShapeType = MapWinGIS.ShpfileType.SHP_MULTIPOINT;
                
                if (mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPOINT ||
                    mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPOINTM ||
                    mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPOINTZ)
                {
                    MultiPoint newPoints = geom as MultiPoint;
                    for (int iPoint = 0; iPoint < newPoints.Count; iPoint++)
                    {
                        Point CurPoint = newPoints.Geometries[iPoint] as Point;
                        mwPoint = new MapWinGIS.Point();
                        mwPoint.x = CurPoint.X;
                        mwPoint.y = CurPoint.Y;
                        mwShape.InsertPoint(mwPoint, ref rfPt);
                        rfPt++;  // I don't think we can have multi-parts in a multipoint shape
                    }
                    return;
                }
                else
                {
                    throw new ArgumentException("The shape type must be one of the multipoint types to add a multipoint geometry.");
                }
            }

            #endregion 

            #region -------------------------- LINE STRING -------------------------------------

            if (geom.GetType() == typeof(LineString) || geom.GetType() == typeof(LinearRing))
            {
                if (SpecifyShapeType == true) mwShape.ShapeType = MapWinGIS.ShpfileType.SHP_POLYLINE;
                
                if (mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYLINE ||
                    mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYLINEM ||
                    mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYLINEZ)
                {

                    LineString newPoints = geom as LineString; // Polymorphism should allow linear rings to be line strings
                    for (int iPoint = 0; iPoint < newPoints.Count; iPoint++)
                    {
                        ICoordinate CurPoint = newPoints.Coordinates[iPoint];
                        mwPoint = new MapWinGIS.Point();
                        mwPoint.x = CurPoint.X;
                        mwPoint.y = CurPoint.Y;
                        mwShape.InsertPoint(mwPoint, ref rfPt);
                        rfPt++;  // I don't think we can have multi-parts in a multipoint shape
                    }
                    return;
                }
                else
                {
                    throw new ArgumentException("The shape type must be one of the polyline types to add a LineString geometry.");
                }
                
            }
            #endregion

            #region -------------------------- MULTI LINESTRING --------------------------------

            if (geom.GetType() == typeof(MultiLineString))
            {
                if (SpecifyShapeType == true) mwShape.ShapeType = MapWinGIS.ShpfileType.SHP_POLYLINE;
             
                MultiLineString MLS = geom as MultiLineString;

                if (mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYLINE ||
                    mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYLINEM ||
                    mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYLINEZ)
                {
                    for (int iGeom = 0; iGeom < MLS.NumGeometries; iGeom++)
                    {
                        LineString LS = MLS.Geometries[iGeom] as LineString;
                        AppendGeometryToShape(LS, ref mwShape); // prevents a lot of code duplication
                    }
                    return;

                }
                else
                {
                    throw new ArgumentException("Cannot append a MultiLineString geometry to a " + mwShape.ShapeType.ToString() + " shape.");
                }

            }

            #endregion

            #region -------------------------- POLYGON -----------------------------------------
            if (geom.GetType() == typeof(Polygon))
            {
                Polygon newPolygon = geom as Polygon;
                if (SpecifyShapeType == true) mwShape.ShapeType = MapWinGIS.ShpfileType.SHP_POLYGON;


                if (mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGON ||
                    mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGONM ||
                    mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGONZ)
                {
                    mwShape.InsertPart(rfPt, ref rfPrt);
                    rfPrt++;
                    ILinearRing Shell = newPolygon.Shell;
                    for (int iPoint = 0; iPoint < Shell.NumPoints; iPoint++)
                    {
                        ICoordinate Coord = Shell.Coordinates[iPoint];
                        mwPoint = new MapWinGIS.Point();
                        mwPoint.x = Coord.X;
                        mwPoint.y = Coord.Y;
                        mwShape.InsertPoint(mwPoint, ref rfPt);
                        rfPt++;
                    }

                    // The same hole may appear multiple times in overlapping polygons.
                    // Create a list of holes in order to test clockwise.
                    if (newPolygon.NumInteriorRings > 0)
                    {
                        List<ILinearRing> UniqueHoles = new List<ILinearRing>();
                        List<ILinearRing> Holes = GetHoles(mwShape); //holes in the existing shape

                        for (int iHole = 0; iHole < newPolygon.NumInteriorRings; iHole++)
                        {
                            if (Holes.Count > 0)
                            {
                                for (int jHole = 0; jHole < Holes.Count; jHole++)
                                {
                                    if (newPolygon.Holes[iHole] == Holes[jHole])
                                    {
                                        continue;
                                    }
                                    // if we get here, the new hole is unique and should be added
                                    UniqueHoles.Add(newPolygon.Holes[iHole]);
                                    // Also add it to the list to compare against other future holes
                                    Holes.Add(newPolygon.Holes[iHole]); // Holes.Count is re-evaluated each time so this should be ok
                                }
                            }
                            //else
                            //{
                            //    //make sure holes are added to a shape with no holes
                            //    UniqueHoles.Add(newPolygon.Holes[iHole]);
                            //    Holes.Add(newPolygon.Holes[iHole]);
                            //}
                        }

                        for (int iHole = 0; iHole < UniqueHoles.Count; iHole++)
                        {
                            mwShape.InsertPart(rfPt, ref rfPrt);
                            rfPrt++;
                            ICoordinate[] Hole;
                            if (IsClockwise(UniqueHoles[iHole].Coordinates))
                            {
                                // Holes should be counter clockwise
                                Hole = UniqueHoles[iHole].Reverse().Coordinates;
                            }
                            else
                            {
                                Hole = UniqueHoles[iHole].Coordinates;
                            }

                            int Count = Hole.GetUpperBound(0) + 1;
                            for (int iPoint = 0; iPoint < Count; iPoint++)
                            {
                                ICoordinate Coord = Hole[iPoint];
                                mwPoint = new MapWinGIS.Point();
                                mwPoint.x = Coord.X;
                                mwPoint.y = Coord.Y;
                                mwShape.InsertPoint(mwPoint, ref rfPt);
                                rfPt++;
                            }
                        }
                    }

                }
            }

            #endregion

            #region -------------------------- MULTI POLYGON------------------------------------

            // The reason not to just Call Add Polygon recursively with this is that we only
            // want to test for unique holes once.  Doing so each and every time we add a polygon
            // would be a major inefficiency, plus the overhead from converting back and forth
            // to a shape object.

            if (geom.GetType() == typeof(MultiPolygon))
            {
                MultiPolygon MPG = geom as MultiPolygon;

                if (SpecifyShapeType == true) mwShape.ShapeType = MapWinGIS.ShpfileType.SHP_POLYGON;

                if (mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGON ||
                    mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGONM ||
                    mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGONZ)
                {
                    // By doing this one time up here, we prevent a lot of duplication
                    List<ILinearRing> Holes = GetHoles(mwShape);
                    
                    for (int iPolygon = 0; iPolygon < MPG.NumGeometries; iPolygon++)
                    {
                        Polygon newPolygon = MPG.Geometries[iPolygon] as Polygon;
                        mwShape.InsertPart(rfPt, ref rfPrt);
                        rfPrt++;
                        ILinearRing Shell = newPolygon.Shell;
                        for (int iPoint = 0; iPoint < Shell.NumPoints; iPoint++)
                        {
                            ICoordinate Coord = Shell.Coordinates[iPoint];
                            mwPoint = new MapWinGIS.Point();
                            mwPoint.x = Coord.X;
                            mwPoint.y = Coord.Y;
                            mwShape.InsertPoint(mwPoint, ref rfPt);
                            rfPt++;
                        }

                        // The same hole may appear multiple times in overlapping polygons.
                        // Create a list of holes in order to test clockwise.
                        if (newPolygon.NumInteriorRings > 0)
                        {
                            List<ILinearRing> UniqueHoles = new List<ILinearRing>();


                            for (int iHole = 0; iHole < newPolygon.NumInteriorRings; iHole++)
                            {
                                for (int jHole = 0; jHole < Holes.Count; jHole++)
                                {
                                    if (newPolygon.Holes[iHole] == Holes[jHole])
                                    {
                                        continue;
                                    }
                                    // if we get here, the new hole is unique and should be added
                                    UniqueHoles.Add(newPolygon.Holes[iHole]);
                                    // Also add it to the list to compare against other future holes
                                    Holes.Add(newPolygon.Holes[iHole]); // Holes.Count is re-evaluated each time so this should be ok
                                }
                            }

                            for (int iHole = 0; iHole < UniqueHoles.Count; iHole++)
                            {
                                mwShape.InsertPart(rfPt, ref rfPrt);
                                rfPrt++;
                                ICoordinate[] Hole;
                                if (IsClockwise(UniqueHoles[iHole].Coordinates))
                                {
                                    // Holes should be counter clockwise
                                    Hole = UniqueHoles[iHole].Reverse().Coordinates;
                                }
                                else
                                {
                                    Hole = UniqueHoles[iHole].Coordinates;
                                }

                                int Count = Hole.GetUpperBound(0) + 1;
                                for (int iPoint = 0; iPoint < Count; iPoint++)
                                {
                                    ICoordinate Coord = Hole[iPoint];
                                    mwPoint = new MapWinGIS.Point();
                                    mwPoint.x = Coord.X;
                                    mwPoint.y = Coord.Y;
                                    mwShape.InsertPoint(mwPoint, ref rfPt);
                                    rfPt++;
                                }
                            }
                        }
                    }

                }
            }

            #endregion

          
        }

        /// <summary>
        /// This function scans a Polygon/PolygonM/PolygonZ to determine if any of the 
        /// existing polygons are counter-clockwise.  If they are, then it creates a linear ring
        /// from the shape.
        /// </summary>
        /// <param name="mwShape">A MapWinGIS.Shape Polygon/PolygonM/PolygonZ that might have holes.</param>
        /// <returns>A List of Linear Rings, each item representing one of the holes from the polygon.</returns>
        public static List<ILinearRing> GetHoles(MapWinGIS.Shape mwShape)
        {
            List<ILinearRing> Holes = new List<ILinearRing>();
            if (mwShape.numPoints > 0)
            {
                MapWinGIS.Point mwPoint = new MapWinGIS.Point();
                int pt = 0;
                int prt = 0;

                do
                {
                    int Start = mwShape.get_Part(prt);
                    int End;
                    if (prt >= mwShape.NumParts - 1)
                    {
                        End = mwShape.numPoints;
                    }
                    else
                    {
                        End = mwShape.get_Part(prt + 1);
                    }
                    int Count = End - Start + 1;
                    Coordinate[] Coords = new Coordinate[Count];
                    double area = 0;
                    for (int PrtPoint = 0; PrtPoint < Count - 1; PrtPoint++)
                    {
                        // PrtPoint will be tracking the index for the specific linestring
                        mwPoint = mwShape.get_Point(pt);
                        Coords[PrtPoint] = new Coordinate(mwPoint.x, mwPoint.y);
                        if (PrtPoint > 0)
                        {
                            area += Coords[PrtPoint].X * Coords[PrtPoint - 1].Y - Coords[PrtPoint - 1].X * Coords[PrtPoint].Y;
                        }
                        pt++;
                    }
                    mwPoint = mwShape.get_Point(Start); // close the loop
                    Coords[Count - 1] = new Coordinate(mwPoint.x, mwPoint.y);
                    area += Coords[Count - 2].X * Coords[Count - 1].Y - Coords[Count - 1].X * Coords[Count - 2].Y;

                    // if the area is negative, then the shape is counter-clockwise and therefore a hole
                    if (area < 0)
                    {
                        Holes.Add(new LinearRing(Coords));
                    }
                    prt++;
                }
                while (prt < mwShape.NumParts);
            }
            return Holes;
        }


    }
    /// <summary>
    /// Organizes linear rings into polygons
    /// </summary>
    public class NRings
    {
        /// <summary>
        /// The outer shell of the proposed ring
        /// </summary>
        public LinearRing Ring;
        /// <summary>
        /// A list of the linear rings that should be holes contained by this outer shell
        /// </summary>
        public List<LinearRing> Holes;

        /// <summary>
        /// A list version of the polygon for adding and removing holes easilly
        /// </summary>
        /// <param name="Coords">A list of coordinates used to define the outer shell of the new NRings list</param>
        public NRings(ICoordinate[] Coords)
        {
            Ring = new LinearRing(Coords);
            Holes = new List<LinearRing>();
        }
      
    }
   
}
