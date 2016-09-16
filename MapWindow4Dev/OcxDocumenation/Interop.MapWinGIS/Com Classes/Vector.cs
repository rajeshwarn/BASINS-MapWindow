
namespace MapWinGIS
{
    using System;

    class IVector
    {
        #region IVector Members

        public Vector CrossProduct(Vector V)
        {
            throw new NotImplementedException();
        }

        public double Dot(Vector V)
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

        public void Normalize()
        {
            throw new NotImplementedException();
        }

        public string get_ErrorMsg(int ErrorCode)
        {
            throw new NotImplementedException();
        }

        public double i
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

        public double j
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

        public double k
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

        #endregion
    }
}
