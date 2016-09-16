
namespace MapWinGIS
{
    using System;

    class IColorScheme
    {
        #region IColorScheme Members

        public void AddBreak(double Value, uint Color)
        {
            throw new NotImplementedException();
        }

        public void Clear()
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

        public int NumBreaks
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(int Index)
        {
            throw new NotImplementedException();
        }

        public void SetColors(uint Color1, uint Color2)
        {
            throw new NotImplementedException();
        }

        public void SetColors2(tkMapColor Color1, tkMapColor Color2)
        {
            throw new NotImplementedException();
        }

        public void SetColors3(short MinRed, short MinGreen, short MinBlue, short MaxRed, short MaxGreen, short MaxBlue)
        {
            throw new NotImplementedException();
        }

        public void SetColors4(PredefinedColorScheme Scheme)
        {
            throw new NotImplementedException();
        }

        public uint get_BreakColor(int Index)
        {
            throw new NotImplementedException();
        }

        public double get_BreakValue(int Index)
        {
            throw new NotImplementedException();
        }

        public string get_ErrorMsg(int ErrorCode)
        {
            throw new NotImplementedException();
        }

        public uint get_GraduatedColor(double Value)
        {
            throw new NotImplementedException();
        }

        public uint get_RandomColor(double Value)
        {
            throw new NotImplementedException();
        }

        public void set_BreakColor(int Index, uint retval)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
