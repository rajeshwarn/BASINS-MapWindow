using System;
using System.Collections;
using System.Text;

using MapWinGeoProc.NTS.Topology.Geometries;
using MapWinGeoProc.NTS.Topology.GeometriesGraph;
using MapWindow.Interfaces.Geometries;
namespace MapWinGeoProc.NTS.Topology.GeometriesGraph.Index
{
    /// <summary>
    /// 
    /// </summary>
    public class SweepLineSegment
    {
        private Edge edge;
        private ICoordinate[] pts;
        int ptIndex;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="ptIndex"></param>
        public SweepLineSegment(Edge edge, int ptIndex)
        {
            this.edge = edge;
            this.ptIndex = ptIndex;
            pts = edge.Coordinates;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual double MinX
        {
            get
            {
                double x1 = pts[ptIndex].X;
                double x2 = pts[ptIndex + 1].X;
                return x1 < x2 ? x1 : x2;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual double MaxX
        {
            get
            {
                double x1 = pts[ptIndex].X;
                double x2 = pts[ptIndex + 1].X;
                return x1 > x2 ? x1 : x2;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ss"></param>
        /// <param name="si"></param>
        public virtual void ComputeIntersections(SweepLineSegment ss, SegmentIntersector si)
        {
            si.AddIntersections(edge, ptIndex, ss.edge, ss.ptIndex);
        }
    }
}
