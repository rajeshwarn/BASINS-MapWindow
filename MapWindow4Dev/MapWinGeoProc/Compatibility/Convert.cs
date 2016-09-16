using System;
using System.Collections.Generic;
using System.Text;
using MapWindow.Interfaces.Raster;
namespace MapWinGeoProc.Compatibility
{
    /// <summary>
    /// Converts types form MapWinGIS format to MapWindow.Interfaces format
    /// </summary>
    public static class Convert
    {
        /// <summary>
        /// Returns a MapWindow.Interfaces.Types.GridDataType that corresponds to a MapWinGIs.GridDataType
        /// </summary>
        /// <param name="MapWinGIS_GridDataType">A MapWinGIS.GridDataType</param>
        /// <returns>A MapWindow.Interfaces.Types.GridDataType</returns>
        public static GridDataType GetGridDataType(MapWinGIS.GridDataType MapWinGIS_GridDataType)
        {
            GridDataType retVal = GridDataType.UNKNOWN_TYPE;
            switch (MapWinGIS_GridDataType)
            {
                case MapWinGIS.GridDataType.DoubleDataType:
                    retVal = GridDataType.DOUBLE_TYPE;
                    break;
                case MapWinGIS.GridDataType.FloatDataType:
                    retVal = GridDataType.FLOAT_TYPE;
                    break;
                case MapWinGIS.GridDataType.InvalidDataType:
                    retVal = GridDataType.INVALID_DATA_TYPE;
                    break;
                case MapWinGIS.GridDataType.LongDataType:
                    retVal = GridDataType.LONG_TYPE;
                    break;
                case MapWinGIS.GridDataType.ShortDataType:
                    retVal = GridDataType.SHORT_TYPE;
                    break;
                case MapWinGIS.GridDataType.UnknownDataType:
                    retVal = GridDataType.UNKNOWN_TYPE;
                    break;
            }
            return retVal;
        }

        /// <summary>
        /// Returns a new MapWindow.Interfaces.Types.GridFileType that corresponds to the
        /// older MapWinGIS.GridFileType
        /// </summary>
        /// <param name="MapWinGIS_GridFileType">A MapWinGIS.GridFileType</param>
        /// <returns>A MapWindow.Interfaces.Types.GridFileType</returns>
        public static GridFileType GetGridFileType(MapWinGIS.GridFileType MapWinGIS_GridFileType)
        {
            GridFileType retVal = GridFileType.USE_EXTENSION;
            switch(MapWinGIS_GridFileType)
            {
                case MapWinGIS.GridFileType.Ascii:
                    retVal = GridFileType.ASCII_GRID;
                    break;
                case MapWinGIS.GridFileType.Bil:
                    retVal = GridFileType.BIL;
                    break;
                case MapWinGIS.GridFileType.Binary:
                    retVal = GridFileType.BINARY_GRID;
                    break;
                case MapWinGIS.GridFileType.DTed:
                    retVal = GridFileType.DTED;
                    break;
                case MapWinGIS.GridFileType.Ecw:
                    retVal = GridFileType.ECW;
                    break;
                case MapWinGIS.GridFileType.Esri:
                    retVal = GridFileType.ESRI_GRID;
                    break;
                case MapWinGIS.GridFileType.Flt:
                    retVal = GridFileType.FLT;
                    break;
                case MapWinGIS.GridFileType.GeoTiff:
                    retVal = GridFileType.GEOTIFF_GRID;
                    break;
                case MapWinGIS.GridFileType.InvalidGridFileType:
                    retVal = GridFileType.INVALID_GRID_TYPE;
                    break;
                case MapWinGIS.GridFileType.MrSid:
                    retVal = GridFileType.MRSID;
                    break;
                case MapWinGIS.GridFileType.PAux:
                    retVal = GridFileType.PAUX;
                    break;
                case MapWinGIS.GridFileType.PCIDsk:
                    retVal = GridFileType.PCIDSK;
                    break;
                case MapWinGIS.GridFileType.Sdts:
                    retVal = GridFileType.SDTS_GRID;
                    break;
                case MapWinGIS.GridFileType.UseExtension:
                    retVal = GridFileType.USE_EXTENSION;
                    break;
                default:
                    retVal = GridFileType.USE_EXTENSION;
                    break;
            }
            return retVal;
        }

        /// <summary>
        /// Converts a new MapWindow.Interfaces.Types.GridDataType to an older MapWinGIS.GridDataType format
        /// </summary>
        /// <param name="Types_GridDataType">A Mapwindow.Interfaces.Types.GridDataType</param>
        /// <returns>A MapWinGIS.GridDataType</returns>
        public static MapWinGIS.GridDataType mwGridDataType(GridDataType Types_GridDataType)
        {
            MapWinGIS.GridDataType retVal = MapWinGIS.GridDataType.UnknownDataType;
            switch(Types_GridDataType)
            {
                case GridDataType.DOUBLE_TYPE:
                    retVal = MapWinGIS.GridDataType.DoubleDataType;
                    break;
                case GridDataType.FLOAT_TYPE:
                    retVal = MapWinGIS.GridDataType.FloatDataType;
                    break;
                case GridDataType.INVALID_DATA_TYPE:
                    retVal = MapWinGIS.GridDataType.InvalidDataType;
                    break;
                case GridDataType.LONG_TYPE:
                    retVal = MapWinGIS.GridDataType.LongDataType;
                    break;
                case GridDataType.SHORT_TYPE:
                    retVal = MapWinGIS.GridDataType.ShortDataType;
                    break;
                case GridDataType.UNKNOWN_TYPE:
                    retVal = MapWinGIS.GridDataType.UnknownDataType;
                    break;
            }
            return retVal;
        }
        
        /// <summary>
        /// Converts a newer Mapwindow.Interfaces.Types.GridFileType to an older
        /// MapWinGIS.GridFileType
        /// </summary>
        /// <param name="Types_GridFileType">A MapWindow.Interfaces.Types.GridFileType</param>
        /// <returns>A MapWinGIS.GridFiletype</returns>
        public static MapWinGIS.GridFileType mwGridFileType(GridFileType Types_GridFileType)
        {
            MapWinGIS.GridFileType retVal = MapWinGIS.GridFileType.UseExtension;
            switch (Types_GridFileType)
            {
                case GridFileType.ASCII_GRID:
                    retVal = MapWinGIS.GridFileType.Ascii;
                    break;
                case GridFileType.BIL:
                    retVal = MapWinGIS.GridFileType.Bil;
                    break;
                case GridFileType.BINARY_GRID:
                    retVal = MapWinGIS.GridFileType.Binary;
                    break;
                case GridFileType.DTED:
                    retVal = MapWinGIS.GridFileType.DTed;
                    break;
                case GridFileType.ECW:
                    retVal = MapWinGIS.GridFileType.Ecw;
                    break;
                case GridFileType.ESRI_GRID:
                    retVal = MapWinGIS.GridFileType.Esri;
                    break;
                case GridFileType.FLT:
                    retVal = MapWinGIS.GridFileType.Flt;
                    break;
                case GridFileType.GEOTIFF_GRID:
                    retVal = MapWinGIS.GridFileType.GeoTiff;
                    break;
                case GridFileType.INVALID_GRID_TYPE:
                    retVal = MapWinGIS.GridFileType.InvalidGridFileType;
                    break;
                case GridFileType.MRSID:
                    retVal = MapWinGIS.GridFileType.MrSid;
                    break;
                case GridFileType.PAUX:
                    retVal = MapWinGIS.GridFileType.PAux;
                    break;
                case GridFileType.PCIDSK:
                    retVal = MapWinGIS.GridFileType.PCIDsk;
                    break;
                case GridFileType.SDTS_GRID:
                    retVal = MapWinGIS.GridFileType.Sdts;
                    break;
                case GridFileType.USE_EXTENSION:
                    retVal = MapWinGIS.GridFileType.UseExtension;
                    break;
            }
            return retVal;
        }
    }
}
