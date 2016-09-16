using System;
using System.Collections;
using System.Text;

using MapWinGeoProc.NTS.Topology.Geometries;
using MapWinGeoProc.NTS.Topology.GeometriesGraph;
using MapWinGeoProc.NTS.Topology.Operation;
using MapWindow.Interfaces.Geometries;

namespace MapWinGeoProc.NTS.Topology.Operation.Relate
{
    /// <summary>
    /// Implements the <c>Relate()</c> operation on <c>Geometry</c>s.
    /// </summary>
    public class RelateOp : GeometryGraphOperation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static IntersectionMatrix Relate(IGeometry a, IGeometry b)
        {
            RelateOp relOp = new RelateOp(a, b);
            IntersectionMatrix im = relOp.IntersectionMatrix;
            return im;
        }

        private RelateComputer relate = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g0"></param>
        /// <param name="g1"></param>
        public RelateOp(IGeometry g0, IGeometry g1) : base(g0, g1)
        {            
            relate = new RelateComputer(arg);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual IntersectionMatrix IntersectionMatrix
        {
            get
            {
                return relate.ComputeIM();
            }
        }
    }
}