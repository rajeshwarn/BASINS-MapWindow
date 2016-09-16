
namespace MapWinGIS
{
    using System;

    class IESRIGridManager
    {
        #region IESRIGridManager Members

        public bool CanUseESRIGrids()
        {
            throw new NotImplementedException();
        }

        public bool DeleteESRIGrids(string Filename)
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

        public bool IsESRIGrid(string Filename)
        {
            throw new NotImplementedException();
        }

        public int LastErrorCode
        {
            get { throw new NotImplementedException(); }
        }

        public string get_ErrorMsg(int ErrorCode)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
