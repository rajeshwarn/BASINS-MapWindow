
namespace MapWinGIS
{
    using System;

    class IShapeNetwork
    {
        #region IShapeNetwork Members

        public int Build(Shapefile Shapefile, int ShapeIndex, int FinalPointIndex, double Tolerance, AmbiguityResolution ar, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public bool Close()
        {
            throw new NotImplementedException();
        }

        public Shape CurrentShape
        {
            get { throw new NotImplementedException(); }
        }

        public int CurrentShapeIndex
        {
            get { throw new NotImplementedException(); }
        }

        public bool DeleteShape(int ShapeIndex)
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

        public bool MoveDown()
        {
            throw new NotImplementedException();
        }

        public bool MoveTo(int ShapeIndex)
        {
            throw new NotImplementedException();
        }

        public bool MoveToOutlet()
        {
            throw new NotImplementedException();
        }

        public bool MoveUp(int UpIndex)
        {
            throw new NotImplementedException();
        }

        public int NetworkSize
        {
            get { throw new NotImplementedException(); }
        }

        public int NumDirectUps
        {
            get { throw new NotImplementedException(); }
        }

        public bool Open(Shapefile sf, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public int ParentIndex
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

        public Grid RasterizeD8(bool UseNetworkBounds, GridHeader Header, double Cellsize, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public Shapefile Shapefile
        {
            get { throw new NotImplementedException(); }
        }

        public int get_AmbigShapeIndex(int Index)
        {
            throw new NotImplementedException();
        }

        public double get_DistanceToOutlet(int PointIndex)
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
