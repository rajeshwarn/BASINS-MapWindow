
namespace MapWinGIS
{
    using System;

    class ILinePattern
    {
        #region ILinePattern Members

        public void AddLine(uint Color, float Width, tkDashStyle style)
        {
            throw new NotImplementedException();
        }

        public LineSegment AddMarker(tkDefaultPointSymbol Marker)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { throw new NotImplementedException(); }
        }

        public void Deserialize(string newVal)
        {
            throw new NotImplementedException();
        }

        public bool Draw(IntPtr hdc, float x, float y, int clipWidth, int clipHeight, uint BackColor)
        {
            throw new NotImplementedException();
        }

        public bool DrawVB(int hdc, float x, float y, int clipWidth, int clipHeight, uint BackColor)
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

        public bool InsertLine(int Index, uint Color, float Width, tkDashStyle style)
        {
            throw new NotImplementedException();
        }

        public LineSegment InsertMarker(int Index, tkDefaultPointSymbol Marker)
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

        public bool RemoveItem(int Index)
        {
            throw new NotImplementedException();
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }

        public byte Transparency
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

        public LineSegment get_Line(int Index)
        {
            throw new NotImplementedException();
        }

        public void set_Line(int Index, LineSegment retval)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
