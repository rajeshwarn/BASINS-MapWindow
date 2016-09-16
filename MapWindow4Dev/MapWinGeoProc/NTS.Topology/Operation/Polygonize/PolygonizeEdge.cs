using System;
using System.Collections;
using System.Text;

using MapWinGeoProc.NTS.Topology.Geometries;
using MapWinGeoProc.NTS.Topology.Planargraph;

namespace MapWinGeoProc.NTS.Topology.Operation.Polygonize
{
    /// <summary>
    /// An edge of a polygonization graph.
    /// </summary>
    public class PolygonizeEdge : Edge
    {
        private LineString line;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        public PolygonizeEdge(LineString line)
        {
            this.line = line;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual LineString Line
        {
            get
            {
                return line;
            }
        }
    }
}
