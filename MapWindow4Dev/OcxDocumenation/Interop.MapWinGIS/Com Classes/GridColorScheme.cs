
namespace MapWinGIS
{
    using System;

    class IGridColorScheme
    {
        #region IGridColorScheme Members

        public double AmbientIntensity
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

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void DeleteBreak(int Index)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(string newVal)
        {
            throw new NotImplementedException();
        }

        public Vector GetLightSource()
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

        public void InsertAt(int Position, GridColorBreak Break)
        {
            throw new NotImplementedException();
        }

        public void InsertBreak(GridColorBreak BrkInfo)
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

        public double LightSourceAzimuth
        {
            get { throw new NotImplementedException(); }
        }

        public double LightSourceElevation
        {
            get { throw new NotImplementedException(); }
        }

        public double LightSourceIntensity
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

        public uint NoDataColor
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

        public int NumBreaks
        {
            get { throw new NotImplementedException(); }
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }

        public void SetLightSource(double Azimuth, double Elevation)
        {
            throw new NotImplementedException();
        }

        public void UsePredefined(double LowValue, double HighValue, PredefinedColorScheme Preset)
        {
            throw new NotImplementedException();
        }

        public GridColorBreak get_Break(int Index)
        {
            throw new NotImplementedException();
        }

        public string get_ErrorMsg(int ErrorCode)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
