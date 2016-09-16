
namespace MapWinGIS
{
    using System;

    class IImage
    {
        #region IImage Members

        public bool AllowHillshade
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

        public int BufferSize
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

        public void BufferToProjection(int BufferX, int BufferY, out double projX, out double projY)
        {
            throw new NotImplementedException();
        }

        public bool BuildOverviews(tkGDALResamplingMethod ResamplingMethod, int NumOverviews, Array OverviewList)
        {
            throw new NotImplementedException();
        }

        public bool CanUseGrouping
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

        public string CdlgFilter
        {
            get { throw new NotImplementedException(); }
        }

        public bool Clear(uint CanvasColor, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public bool ClearGDALCache
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
        
        /// <summary>
        /// Closes the image
        /// </summary>
        /// <returns></returns>
        public bool Close()
        {
            throw new NotImplementedException();
        }

        public bool CreateNew(int NewWidth, int NewHeight)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(string newVal)
        {
            throw new NotImplementedException();
        }

        public tkInterpolationMode DownsamplingMode
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

        public int DrawingMethod
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

        public Extents Extents
        {
            get { throw new NotImplementedException(); }
        }

        public int FileHandle
        {
            get { throw new NotImplementedException(); }
        }

        public string Filename
        {
            get { throw new NotImplementedException(); }
        }

        public bool GetImageBitsDC(int hdc)
        {
            throw new NotImplementedException();
        }

        public int GetOriginalHeight()
        {
            throw new NotImplementedException();
        }

        public int GetOriginalWidth()
        {
            throw new NotImplementedException();
        }

        public double GetOriginalXllCenter()
        {
            throw new NotImplementedException();
        }

        public double GetOriginalYllCenter()
        {
            throw new NotImplementedException();
        }

        public double GetOriginal_dX()
        {
            throw new NotImplementedException();
        }

        public double GetOriginal_dY()
        {
            throw new NotImplementedException();
        }

        public string GetProjection()
        {
            throw new NotImplementedException();
        }

        public bool GetRow(int Row, ref int Vals)
        {
            throw new NotImplementedException();
        }

        public int GetUniqueColors(double MaxBufferSizeMB, out object Colors, out object Frequencies)
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

        public bool HasColorTable
        {
            get { throw new NotImplementedException(); }
        }

        public int Height
        {
            get { throw new NotImplementedException(); }
        }

        public PredefinedColorScheme ImageColorScheme
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

        public void ImageToProjection(int ImageX, int ImageY, out double projX, out double projY)
        {
            throw new NotImplementedException();
        }

        public ImageType ImageType
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsInRam
        {
            get { throw new NotImplementedException(); }
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

        public Labels Labels
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

        public bool LoadBuffer(double maxBufferSize)
        {
            throw new NotImplementedException();
        }

        public int NoBands
        {
            get { throw new NotImplementedException(); }
        }

        public int NumOverviews
        {
            get { throw new NotImplementedException(); }
        }

        public bool Open(string ImageFileName, ImageType fileType, bool InRam, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public double OriginalDX
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

        public double OriginalDY
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

        public int OriginalHeight
        {
            get { throw new NotImplementedException(); }
        }

        public int OriginalWidth
        {
            get { throw new NotImplementedException(); }
        }

        public double OriginalXllCenter
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

        public double OriginalYllCenter
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

        public string PaletteInterpretation
        {
            get { throw new NotImplementedException(); }
        }

        public stdole.IPictureDisp Picture
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

        public void ProjectionToBuffer(double projX, double projY, out int BufferX, out int BufferY)
        {
            throw new NotImplementedException();
        }

        public void ProjectionToImage(double projX, double projY, out int ImageX, out int ImageY)
        {
            throw new NotImplementedException();
        }

        public bool Resource(string newImgPath)
        {
            throw new NotImplementedException();
        }

        public bool Save(string ImageFileName, bool WriteWorldFile, ImageType fileType, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public string Serialize(bool SerializePixels)
        {
            throw new NotImplementedException();
        }

        public bool SetImageBitsDC(int hdc)
        {
            throw new NotImplementedException();
        }

        public void SetNoDataValue(double Value, ref bool Result)
        {
            throw new NotImplementedException();
        }

        public bool SetProjection(string Proj4)
        {
            throw new NotImplementedException();
        }

        public bool SetToGrey
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

        public void SetVisibleExtents(double newMinX, double newMinY, double newMaxX, double newMaxY, int newPixelsInView, float transPercent)
        {
            throw new NotImplementedException();
        }

        public tkImageSourceType SourceType
        {
            get { throw new NotImplementedException(); }
        }

        public uint TransparencyColor
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

        public uint TransparencyColor2
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

        public double TransparencyPercent
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

        public tkInterpolationMode UpsamplingMode
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

        public bool UseHistogram
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

        public bool UseTransparencyColor
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

        public int Width
        {
            get { throw new NotImplementedException(); }
        }

        public double XllCenter
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

        public double YllCenter
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

        public bool _pushSchemetkRaster(GridColorScheme cScheme)
        {
            throw new NotImplementedException();
        }

        public double dX
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

        public double dY
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

        public string get_ErrorMsg(int ErrorCode)
        {
            throw new NotImplementedException();
        }

        public int get_Value(int Row, int col)
        {
            throw new NotImplementedException();
        }

        public void set_Value(int Row, int col, int pVal)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
