
namespace MapWinGIS
{
    using System;

    class ITin
    {
        #region ITin Members

        public string CdlgFilter
        {
            get { throw new NotImplementedException(); }
        }

        public bool Close()
        {
            throw new NotImplementedException();
        }

        public bool CreateNew(Grid Grid, double Deviation, SplitMethod SplitTest, double STParam, int MeshDivisions, int MaximumTriangles, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public bool CreateTinFromPoints(Array Points)
        {
            throw new NotImplementedException();
        }

        public string Filename
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

        public void Max(out double x, out double y, out double Z)
        {
            throw new NotImplementedException();
        }

        public void Min(out double x, out double y, out double Z)
        {
            throw new NotImplementedException();
        }

        public int NumTriangles
        {
            get { throw new NotImplementedException(); }
        }

        public int NumVertices
        {
            get { throw new NotImplementedException(); }
        }

        public bool Open(string TinFile, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public bool Save(string TinFilename, ICallback cBack)
        {
            throw new NotImplementedException();
        }

        public bool Select(ref int TriangleHint, double x, double y, out double Z)
        {
            throw new NotImplementedException();
        }

        public void Triangle(int TriIndex, out int vtx1Index, out int vtx2Index, out int vtx3Index)
        {
            throw new NotImplementedException();
        }

        public void TriangleNeighbors(int TriIndex, ref int triIndex1, ref int triIndex2, ref int triIndex3)
        {
            throw new NotImplementedException();
        }

        public void Vertex(int VtxIndex, out double x, out double y, out double Z)
        {
            throw new NotImplementedException();
        }

        public string get_ErrorMsg(int ErrorCode)
        {
            throw new NotImplementedException();
        }

        public bool get_IsNDTriangle(int TriIndex)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
