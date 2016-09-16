using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Topology2D
{
    /// <summary>
    /// This class enables developers to create the various geometries.
    /// Accessors for these are generally also provided in the constructors.
    /// The only references with regards to topology2D are here
    /// </summary>
    public static class GeometryFactory
    {
        #region TO TOPOLOGY

        #region COORDINATE
        /// <summary>
        /// MapWinGIS referencing is only done in the geometry factory.
        /// </summary>
        /// <param name="mwPoint"></param>
        /// <returns>A Coordinate created from mwPoint</returns>
        public static Coordinate CreateCoordinate(MapWinGIS.Point mwPoint)
        {
            return new Coordinate(mwPoint.x, mwPoint.y);
        }
        /// <summary>
        /// Creates a point from a MapWinGIS.Point passed as a generic object
        /// </summary>
        /// <param name="objPoint">A MapWinGIS.Point</param>
        /// <returns>A Coordinate generated from the MapWinGIS.Point</returns>
        public static Coordinate CreateCoordinate(object objPoint)
        {
            if (objPoint.GetType() != typeof(MapWinGIS.Point))
            {
                return null;
            }
            MapWinGIS.Point mwPoint = objPoint as MapWinGIS.Point;
            return new Coordinate(mwPoint.x, mwPoint.y);
        }

        #endregion

        #region  POINT
        /// <summary>
        /// MapWinGIS referencing is only done in the geometry factory.
        /// </summary>
        /// <param name="mwPoint"></param>
        /// <returns>A new Point object</returns>
        public static Point CreatePoint(MapWinGIS.Point mwPoint)
        {
            return new Point(mwPoint.x, mwPoint.y);
        }
        /// <summary>
        /// Creates a point from a MapWinGIS.Point passed as a generic object
        /// </summary>
        /// <param name="objPoint">A MapWinGIS.Point</param>
        /// <returns>A Point generated fromt he MapWinGIS.Point</returns>
        public static Point CreatePoint(object objPoint)
        {
            if (objPoint.GetType() != typeof(MapWinGIS.Point))
            {
                return null;
            }
            MapWinGIS.Point mwPoint = objPoint as MapWinGIS.Point;
            return new Point(mwPoint.x, mwPoint.y);
        }
        #endregion

        #region SEGMENT
        #endregion

        #region LINE STRING
        /// <summary>
        /// Creates a new instance of the polyline class
        /// </summary>
        /// <param name="MapWinGIS_Shape">A MapWinGIS.Shape to derive the polyline from</param>
        /// <remarks>Assumes shape is one part.  To Split Multipart shapes, use Split.</remarks>
        public static LineString CreateLineString(object MapWinGIS_Shape)
        {

            if (MapWinGIS_Shape.GetType() != typeof(MapWinGIS.ShapeClass)) return null;
            MapWinGIS.Shape mwShape = MapWinGIS_Shape as MapWinGIS.Shape;

            LineString LS = new LineString();

            LS.Envelope = new Envelope(mwShape.Extents);
            
            LS.Coordinates = new List<Coordinate>();
            for (int I = 0; I < mwShape.numPoints; I++)
            {
                MapWinGIS.Point mwPoint = mwShape.get_Point(I);
                LS.Coordinates.Add(new Coordinate(mwPoint.x, mwPoint.y));
            }
            return LS;
        }
        #endregion

        #region LINEAR RING
        #endregion

        #region MULTI-LINE STRING

        /// <summary>
        /// Creates a new instance of a MultLineSTring from a Late-Bound object
        /// </summary>
        /// <param name="MapWinGIS_Shape">Uses object so we can localized MapWinGIS Dependency</param>
        /// <returns>A MultLineString</returns>
        public static MultiLineString CreateMultiLineString(object MapWinGIS_Shape)
        {
            if(MapWinGIS_Shape.GetType() != typeof(MapWinGIS.ShapeClass))return null;
            MapWinGIS.Shape mwShape = MapWinGIS_Shape as MapWinGIS.Shape;
            return CreateMultiLineString(mwShape);
        }
        /// <summary>
        /// Returns a MultiLineString geometry collection derived from the mwShape
        /// </summary>
        /// <param name="mwShape">The shape to convert into a multi-line string</param>
        public static MultiLineString CreateMultiLineString(MapWinGIS.Shape mwShape)
        {
            MultiLineString MLS = new MultiLineString();
            // Variables
            int numParts; // The number of parts in the shape
            int numPoints; // The number of points in the shape
            LineString LS;
            // Parameter checking
            if (mwShape == null)
            {
                throw new ArgumentException("mwShape should either not be null, or not be specified.");
            }
            if (mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPATCH ||
                mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPOINT ||
                mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPOINTM ||
                mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPOINTZ ||
                mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_NULLSHAPE ||
                mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POINT ||
                mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POINTM ||
                mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POINTZ)
            {
                throw new ArgumentException("Argument mwShape shapetype must be a polyline or polygon.");
            }

            MLS.GeometryN = new List<Geometry>();
            numParts = mwShape.NumParts;
            numPoints = mwShape.numPoints;
            // if NumParts = 0, treat as though the whole shape was a single part
            int prt = 0;                 // prt is the part index
            int pt = 0;                  // pt is the point index
            while(prt < numParts-1)
            {
                int maxpt = mwShape.get_Part(prt+1);  
                LS = new LineString();
                while(pt < maxpt)
                {
                    Coordinate Coord = GeometryFactory.CreateCoordinate(mwShape.get_Point(pt));
                    LS.Coordinates.Add(Coord);
                    pt++;
                }
                MLS.GeometryN.Add(LS);
                prt++;
            }
            LS = new LineString();
            while (pt < numPoints)
            {
                Coordinate Coord = GeometryFactory.CreateCoordinate(mwShape.get_Point(pt));
                LS.Coordinates.Add(Coord);
                pt++;
            }
            MLS.GeometryN.Add(LS);
            return MLS;
        }


        #endregion

        #region POLYGON
        #endregion

        #region MULTI-POLYGON
        #endregion

        #region ENVELOPE
        #endregion

        #endregion

        #region TO MapWinGIS

        #region POINT

        /// <summary>
        /// This creates a strongly typed MapWinGIS Point from a coordinate
        /// </summary>
        /// <param name="Coord">The coordinates to turn into points</param>
        /// <returns>MapWinGIS.Point</returns>
        public static MapWinGIS.Point mwPointFromCoord(Coordinate Coord)
        {
            MapWinGIS.Point mwPoint = new MapWinGIS.Point();
            mwPoint.x = Coord.X;
            mwPoint.y = Coord.Y;
            return mwPoint;
        }

        /// <summary>
        /// This creates a strongly typed MapWinGIS Point from a Topology.Point
        /// </summary>
        /// <param name="Loc">The coordinates to turn into points</param>
        /// <returns>MapWinGIS.Point</returns>
        public static MapWinGIS.Point mwPointFromPoint(Point Loc)
        {
            MapWinGIS.Point mwPoint = new MapWinGIS.Point();
            mwPoint.x = Loc.X;
            mwPoint.y = Loc.Y;
            return mwPoint;
        }

        /// <summary>
        /// This will return a strongly typed MapWinGIS Vector
        /// </summary>
        /// <param name="Vec">Vector</param>
        /// <returns>MapWinGIS.Point</returns>
        public static MapWinGIS.Point mwPointFromVector(Vector Vec)
        {
            MapWinGIS.Point mwPoint = new MapWinGIS.Point();
            mwPoint.x = Vec.X;
            mwPoint.y = Vec.Y;
            return mwPoint;
        }

        #endregion

        #region MULTI LINE STRING

        /// <summary>
        /// Returns a SHP_POLYLINE type MapWinGIS Multi-part PolyLine representing this multiLineString
        /// </summary>
        /// <returns>MapWinGIS Shape</returns>
        public static MapWinGIS.Shape mwShapeFromMultiLineString(MultiLineString MLS)
        {
            return mwShapeFromMultiLineString(MLS, MapWinGIS.ShpfileType.SHP_POLYLINE);
        }

        /// <summary>
        /// Returns a MapWinGIS Multi-part PolyLine representing this multiLineString
        /// </summary>
        /// <returns>MapWinGIS Shape</returns>
        public static MapWinGIS.Shape mwShapeFromMultiLineString(MultiLineString MLS, MapWinGIS.ShpfileType sfType)
        {
            MapWinGIS.Shape mwShape = new MapWinGIS.Shape();
            mwShape.Create(sfType);
            int prt = 0; // Part Index
            int pt = 0; // Point Index
            int numParts = MLS.NumGeometries;
            int maxpt;

            while (prt < numParts)
            {
                LineString LS = MLS.GeometryN[prt] as LineString;
                maxpt = LS.Coordinates.Count;
                mwShape.InsertPart(pt, ref prt);
                for (int I = 0; I < maxpt; I++)
                {
                    MapWinGIS.Point mwPoint = new MapWinGIS.Point();
                    mwPoint.x = LS.Coordinates[I].X;
                    mwPoint.y = LS.Coordinates[I].Y;
                    bool result = mwShape.InsertPoint(mwPoint, ref pt);
                    if (result == false)
                    {
                        throw new ApplicationException(mwShape.get_ErrorMsg(mwShape.LastErrorCode));
                    }
                    pt++;
                }
                prt++;
            }
            return mwShape;
        }

        #endregion



        #endregion
    }
}
