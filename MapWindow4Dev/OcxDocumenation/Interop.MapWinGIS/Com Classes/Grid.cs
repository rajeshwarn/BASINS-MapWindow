
namespace MapWinGIS
{
    using System;

    class IGrid
    {
        #region IGrid Members

        public bool AssignNewProjection(string Projection)
        {
            throw new NotImplementedException();
        }

        public string CdlgFilter
        {
            get { throw new NotImplementedException(); }
        }

        public void CellToProj(int Column, int Row, out double x, out double y)
        {
            throw new NotImplementedException();
        }

        public bool Clear(object ClearValue)
        {
            throw new NotImplementedException();
        }

        public bool Close()
        {
            throw new NotImplementedException();
        }

        public bool CreateNew(string Filename, GridHeader Header, GridDataType DataType, object InitialValue, bool InRam, GridFileType fileType, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public GridDataType DataType
        {
            get { throw new NotImplementedException(); }
        }

        public string Filename
        {
            get { throw new NotImplementedException(); }
        }

        public bool GetFloatWindow(int StartRow, int EndRow, int StartCol, int EndCol, ref float Vals)
        {
            throw new NotImplementedException();
        }

        public bool GetRow(int Row, ref float Vals)
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

        public GridHeader Header
        {
            get { throw new NotImplementedException(); }
        }

        public bool InRam
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

        public int LastErrorCode
        {
            get { throw new NotImplementedException(); }
        }

        public object Maximum
        {
            get { throw new NotImplementedException(); }
        }

        public object Minimum
        {
            get { throw new NotImplementedException(); }
        }

        public bool Open(string Filename, GridDataType DataType, bool InRam, GridFileType fileType, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public void ProjToCell(double x, double y, out int Column, out int Row)
        {
            throw new NotImplementedException();
        }

        public bool PutFloatWindow(int StartRow, int EndRow, int StartCol, int EndCol, ref float Vals)
        {
            throw new NotImplementedException();
        }

        public bool PutRow(int Row, ref float Vals)
        {
            throw new NotImplementedException();
        }

        public GridColorScheme RasterColorTableColoringScheme
        {
            get { throw new NotImplementedException(); }
        }

        public bool Resource(string newSrcPath)
        {
            throw new NotImplementedException();
        }

        public bool Save(string Filename, GridFileType GridFileType, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public bool SetInvalidValuesToNodata(double MinThresholdValue, double MaxThresholdValue)
        {
            throw new NotImplementedException();
        }

        public string get_ErrorMsg(int ErrorCode)
        {
            throw new NotImplementedException();
        }

        public object get_Value(int Column, int Row)
        {
            throw new NotImplementedException();
        }

        public void set_Value(int Column, int Row, object pVal)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
