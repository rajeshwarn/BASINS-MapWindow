
namespace MapWinGIS
{
    using System;

    class IShapefileColorScheme
    {
        #region IShapefileColorScheme Members

        public int Add(ShapefileColorBreak Break)
        {
            throw new NotImplementedException();
        }

        public int FieldIndex
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

        public int InsertAt(int Position, ShapefileColorBreak Break)
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

        public int LayerHandle
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

        public int NumBreaks()
        {
            throw new NotImplementedException();
        }

        public void Remove(int Index)
        {
            throw new NotImplementedException();
        }

        public ShapefileColorBreak get_ColorBreak(int Index)
        {
            throw new NotImplementedException();
        }

        public string get_ErrorMsg(int ErrorCode)
        {
            throw new NotImplementedException();
        }

        public void set_ColorBreak(int Index, ShapefileColorBreak pVal)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
