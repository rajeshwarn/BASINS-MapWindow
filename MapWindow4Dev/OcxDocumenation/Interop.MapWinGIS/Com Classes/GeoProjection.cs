
namespace MapWinGIS
{
    using System;

    class IGeoProjection
    {
        #region IGeoProjection Members

        public bool CopyFrom(GeoProjection sourceProj)
        {
            throw new NotImplementedException();
        }

        public string ExportToProj4()
        {
            throw new NotImplementedException();
        }

        public string ExportToWKT()
        {
            throw new NotImplementedException();
        }

        public string GeogCSName
        {
            get { throw new NotImplementedException(); }
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

        public bool ImportFromAutoDetect(string proj)
        {
            throw new NotImplementedException();
        }

        public bool ImportFromEPSG(int projCode)
        {
            throw new NotImplementedException();
        }

        public bool ImportFromESRI(string proj)
        {
            throw new NotImplementedException();
        }

        public bool ImportFromProj4(string proj)
        {
            throw new NotImplementedException();
        }

        public bool ImportFromWKT(string proj)
        {
            throw new NotImplementedException();
        }

        public double InverseFlattening
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsEmpty
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsGeographic
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsLocal
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsProjected
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

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public string ProjectionName
        {
            get { throw new NotImplementedException(); }
        }

        public bool ReadFromFile(string Filename)
        {
            throw new NotImplementedException();
        }

        public double SemiMajor
        {
            get { throw new NotImplementedException(); }
        }

        public double SemiMinor
        {
            get { throw new NotImplementedException(); }
        }

        public void SetGeographicCS(tkCoordinateSystem coordinateSystem)
        {
            throw new NotImplementedException();
        }

        public void SetNad83Projection(tkNad83Projection Projection)
        {
            throw new NotImplementedException();
        }

        public void SetWellKnownGeogCS(tkCoordinateSystem newVal)
        {
            throw new NotImplementedException();
        }

        public void SetWgs84Projection(tkWgs84Projection Projection)
        {
            throw new NotImplementedException();
        }

        public bool WriteToFile(string Filename)
        {
            throw new NotImplementedException();
        }

        public string get_ErrorMsg(int ErrorCode)
        {
            throw new NotImplementedException();
        }

        public bool get_GeogCSParam(tkGeogCSParameter Name, ref double pVal)
        {
            throw new NotImplementedException();
        }

        public bool get_IsSame(GeoProjection proj)
        {
            throw new NotImplementedException();
        }

        public bool get_IsSameExt(GeoProjection proj, Extents bounds, int numSamplingPoints)
        {
            throw new NotImplementedException();
        }

        public bool get_IsSameGeogCS(GeoProjection proj)
        {
            throw new NotImplementedException();
        }

        public bool get_ProjectionParam(tkProjectionParameter Name, ref double Value)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
