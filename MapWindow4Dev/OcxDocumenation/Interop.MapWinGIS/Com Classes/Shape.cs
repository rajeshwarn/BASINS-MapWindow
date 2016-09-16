
namespace MapWinGIS
{
    using System;

    class IShape
    {
        #region IShape Members

        public double Area
        {
            get { throw new NotImplementedException(); }
        }

        public Shape Boundry()
        {
            throw new NotImplementedException();
        }

        public Shape Buffer(double Distance, int nQuadSegments)
        {
            throw new NotImplementedException();
        }

        public Point Center
        {
            get { throw new NotImplementedException(); }
        }

        public Point Centroid
        {
            get { throw new NotImplementedException(); }
        }

        public Shape Clip(Shape Shape, tkClipOperation Operation)
        {
            throw new NotImplementedException();
        }

        public Shape Clone()
        {
            throw new NotImplementedException();
        }

        public bool Contains(Shape Shape)
        {
            throw new NotImplementedException();
        }

        public Shape ConvexHull()
        {
            throw new NotImplementedException();
        }

        public bool Create(ShpfileType ShpType)
        {
            throw new NotImplementedException();
        }

        public bool CreateFromString(string Serialized)
        {
            throw new NotImplementedException();
        }

        public bool Crosses(Shape Shape)
        {
            throw new NotImplementedException();
        }

        public bool DeletePart(int PartIndex)
        {
            throw new NotImplementedException();
        }

        public bool DeletePoint(int PointIndex)
        {
            throw new NotImplementedException();
        }

        public bool Disjoint(Shape Shape)
        {
            throw new NotImplementedException();
        }

        public double Distance(Shape Shape)
        {
            throw new NotImplementedException();
        }

        public bool Equals(Shape Shape)
        {
            throw new NotImplementedException();
        }

        public bool Explode(ref object Results)
        {
            throw new NotImplementedException();
        }

        public bool ExportToBinary(ref object bytesArray)
        {
            throw new NotImplementedException();
        }

        public Extents Extents
        {
            get { throw new NotImplementedException(); }
        }

        public void FixUp(out Shape retval)
        {
            throw new NotImplementedException();
        }

        public bool GetIntersection(Shape Shape, out object Results)
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

        public bool ImportFromBinary(object bytesArray)
        {
            throw new NotImplementedException();
        }

        public bool InsertPart(int PointIndex, ref int PartIndex)
        {
            throw new NotImplementedException();
        }

        public bool InsertPoint(Point NewPoint, ref int PointIndex)
        {
            throw new NotImplementedException();
        }

        public Point InteriorPoint
        {
            get { throw new NotImplementedException(); }
        }

        public bool Intersects(Shape Shape)
        {
            throw new NotImplementedException();
        }

        public bool IsValid
        {
            get { throw new NotImplementedException(); }
        }

        public string IsValidReason
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

        public double Length
        {
            get { throw new NotImplementedException(); }
        }

        public int NumParts
        {
            get { throw new NotImplementedException(); }
        }

        public bool Overlaps(Shape Shape)
        {
            throw new NotImplementedException();
        }

        public double Perimeter
        {
            get { throw new NotImplementedException(); }
        }

        public bool PointInThisPoly(Point pt)
        {
            throw new NotImplementedException();
        }

        public bool Relates(Shape Shape, tkSpatialRelation Relation)
        {
            throw new NotImplementedException();
        }

        public bool ReversePointsOrder(int PartIndex)
        {
            throw new NotImplementedException();
        }

        public string SerializeToString()
        {
            throw new NotImplementedException();
        }

        public ShpfileType ShapeType
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

        public bool Touches(Shape Shape)
        {
            throw new NotImplementedException();
        }

        public bool Within(Shape Shape)
        {
            throw new NotImplementedException();
        }

        public int get_EndOfPart(int PartIndex)
        {
            throw new NotImplementedException();
        }

        public string get_ErrorMsg(int ErrorCode)
        {
            throw new NotImplementedException();
        }

        public int get_Part(int PartIndex)
        {
            throw new NotImplementedException();
        }

        public Shape get_PartAsShape(int PartIndex)
        {
            throw new NotImplementedException();
        }

        public bool get_PartIsClockWise(int PartIndex)
        {
            throw new NotImplementedException();
        }

        public Point get_Point(int PointIndex)
        {
            throw new NotImplementedException();
        }

        public bool get_XY(int PointIndex, ref double x, ref double y)
        {
            throw new NotImplementedException();
        }

        public int numPoints
        {
            get { throw new NotImplementedException(); }
        }

        public bool put_XY(int PointIndex, double x, double y)
        {
            throw new NotImplementedException();
        }

        public void set_Part(int PartIndex, int pVal)
        {
            throw new NotImplementedException();
        }

        public void set_Point(int PointIndex, Point pVal)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
