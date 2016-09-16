
namespace MapWinGIS
{
    using System;

    class IExtents
    {
        #region IExtents Members

        public void GetBounds(out double xMin, out double yMin, out double zMin, out double xMax, out double yMax, out double zMax)
        {
            throw new NotImplementedException();
        }

        public void GetMeasureBounds(out double mMin, out double mMax)
        {
            throw new NotImplementedException();
        }

        public void SetBounds(double xMin, double yMin, double zMin, double xMax, double yMax, double zMax)
        {
            throw new NotImplementedException();
        }

        public void SetMeasureBounds(double mMin, double mMax)
        {
            throw new NotImplementedException();
        }

        public double mMax
        {
            get { throw new NotImplementedException(); }
        }

        public double mMin
        {
            get { throw new NotImplementedException(); }
        }

        public double xMax
        {
            get { throw new NotImplementedException(); }
        }

        public double xMin
        {
            get { throw new NotImplementedException(); }
        }

        public double yMax
        {
            get { throw new NotImplementedException(); }
        }

        public double yMin
        {
            get { throw new NotImplementedException(); }
        }

        public double zMax
        {
            get { throw new NotImplementedException(); }
        }

        public double zMin
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
