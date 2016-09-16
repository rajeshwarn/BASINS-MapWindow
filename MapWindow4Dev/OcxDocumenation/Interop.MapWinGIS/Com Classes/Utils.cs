
namespace MapWinGIS
{
    using System;

    class IUtils
    {
        #region IUtils Members

        public Shape ClipPolygon(PolygonOperation op, Shape SubjectPolygon, Shape ClipPolygon)
        {
            throw new NotImplementedException();
        }

        public uint ColorByName(tkMapColor Name)
        {
            throw new NotImplementedException();
        }

        public bool GenerateContour(string pszSrcFilename, string pszDstFilename, double dfInterval, double dfNoData, bool Is3D, object dblFLArray, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public bool GenerateHillShade(string bstrGridFilename, string bstrShadeFilename, float Z, float scale, float az, float alt)
        {
            throw new NotImplementedException();
        }

        public ICallback GlobalCallback
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool GridInterpolateNoData(Grid Grd, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public Grid GridMerge(object Grids, string MergeFilename, bool InRam, GridFileType GrdFileType, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public bool GridReplace(Grid Grd, object OldValue, object newValue, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public Grid GridToGrid(Grid Grid, GridDataType OutDataType, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public Image GridToImage(Grid Grid, GridColorScheme cScheme, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public Shapefile GridToShapefile(Grid Grid, Grid ConnectionGrid, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public string Key
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int LastErrorCode
        {
            get { throw new NotImplementedException(); }
        }

        public bool MergeImages(Array InputNames, string OutputName)
        {
            throw new NotImplementedException();
        }

        public Shapefile OGRLayerToShapefile(string Filename, ShpfileType ShpType, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public bool PointInPolygon(Shape Shp, Point TestPoint)
        {
            throw new NotImplementedException();
        }

        public bool RemoveColinearPoints(Shapefile Shapes, double LinearTolerance, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public Shapefile ReprojectShapefile(Shapefile sf, GeoProjection source, GeoProjection target)
        {
            throw new NotImplementedException();
        }

        public Shape ShapeMerge(Shapefile Shapes, int IndexOne, int IndexTwo, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public Shapefile ShapeToShapeZ(Shapefile Shapefile, Grid Grid, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public Grid ShapefileToGrid(Shapefile Shpfile, bool UseShapefileBounds, GridHeader GrdHeader, double Cellsize, bool UseShapeNumber, short SingleValue)
        {
            throw new NotImplementedException();
        }

        public Shapefile TinToShapefile(Tin Tin, ShpfileType Type, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public bool TranslateRaster(string bstrSrcFilename, string bstrDstFilename, string bstrOptions, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public double get_Area(Shape Shape)
        {
            throw new NotImplementedException();
        }

        public string get_ErrorMsg(int ErrorCode)
        {
            throw new NotImplementedException();
        }

        public double get_Length(Shape Shape)
        {
            throw new NotImplementedException();
        }

        public double get_Perimeter(Shape Shape)
        {
            throw new NotImplementedException();
        }

        public stdole.IPictureDisp hBitmapToPicture(int hBitmap)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
