using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc
{
    static class Macro
    {
        /// <summary>
        /// Querying a property from a null parameter will cause an error.
        /// Therefore, this checks for the null case.
        /// Returns a filename if the grid exists, and "null" otherwise.
        /// </summary>
        /// <param name="mwGrid">A MapWinGIS.Grid to obtain the filename for.</param>
        public static string ParamName(MapWinGIS.Grid mwGrid)
        {
            string filename = "null";
            if (mwGrid == null)
            {
                filename = mwGrid.Filename;
            }
            return filename;
        }

        /// <summary>
        /// Because the "MapWinUtilities" logger needs a report of parameters
        /// when object parameters might be null, this includes a null check.
        /// </summary>
        /// <param name="mwShapefile">A MapWinGIS.Shapefile that might obtain a file.</param>
        /// <returns></returns>
        public static string ParamName(MapWinGIS.Shapefile mwShapefile)
        {
            string filename = "null";
            if (mwShapefile != null)
            {
                filename = mwShapefile.Filename;
            }
            return filename;
        }

        /// <summary>
        /// Performs a null test.  If the object is null, then it returns "null".
        /// If the object is not null, it returns nonNullName.
        /// </summary>
        /// <param name="nullObject">The object to test for null</param>
        /// <param name="nonNullName">The name to use if the object is not null.</param>
        /// <returns></returns>
        public static string ParamName(object nullObject, string nonNullName)
        {
            string name = "null";
            if (nullObject != null)
            {
                name = nonNullName;
            }
            return name;
        }

        public static string ParamName(GridWrapper grid)
        {
            string name = "null";
            if (grid != null)
            {
                name = "GridWrapper";
            }
            return name;
        }

        public static string ParamName(MapWinGIS.Point point)
        {
            string name = "null";
            if (point != null)
            {
                name = "Point[" + point.x + ", " + point.y + "]";
            }
            return name;
        }

        /// <summary>
        /// Returns a name that describes a shape variable
        /// </summary>
        /// <param name="mwShape"></param>
        /// <returns></returns>
        public static string ParamName(MapWinGIS.Shape mwShape)
        {
            string name = "null";
            if (mwShape != null)
            {
                // Polygon
                if (mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGON ||
                   mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGONM ||
                   mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYGONZ)
                {
                    name = "Polygon Shape [" + mwShape.NumParts + " parts, " + mwShape.numPoints + " points]";
                }
                if (mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYLINE ||
                    mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYLINEM ||
                    mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POLYLINEZ)
                {
                    name = "Polyline Shape [" + mwShape.NumParts + " parts, " + mwShape.numPoints + " points]";
                }
                if (mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPOINT ||
                   mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPOINTM ||
                   mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPOINTZ)
                {
                    name = "MultiPoint Shape [" + mwShape.NumParts + " parts, " + mwShape.numPoints + " points]";
                }
                if (mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POINT ||
                   mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POINTM ||
                   mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_POINTZ)
                {
                    name = "Point Shape [" + mwShape.numPoints + " points]";
                }
                if (mwShape.ShapeType == MapWinGIS.ShpfileType.SHP_MULTIPATCH)
                {
                    name = "MultiPatch Shape";
                }
            }
            return name;
        }
       
 

    }
}
