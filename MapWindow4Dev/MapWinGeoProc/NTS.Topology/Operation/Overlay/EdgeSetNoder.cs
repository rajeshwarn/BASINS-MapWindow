using System;
using System.Collections;
using System.Text;

using MapWinGeoProc.NTS.Topology.Algorithm;
using MapWinGeoProc.NTS.Topology.GeometriesGraph;
using MapWinGeoProc.NTS.Topology.GeometriesGraph.Index;

namespace MapWinGeoProc.NTS.Topology.Operation.Overlay
{
    /// <summary>
    /// Nodes a set of edges.
    /// Takes one or more sets of edges and constructs a
    /// new set of edges consisting of all the split edges created by
    /// noding the input edges together.
    /// </summary>
    public class EdgeSetNoder
    {
        private LineIntersector li = null;
        private IList inputEdges = new ArrayList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="li"></param>
        public EdgeSetNoder(LineIntersector li)
        {
            this.li = li;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="edges"></param>
        public virtual void AddEdges(IList edges)
        {
            // inputEdges.addAll(edges);
            foreach (object obj in edges)
                inputEdges.Add(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual IList NodedEdges
        {
            get
            {
                EdgeSetIntersector esi = new SimpleMCSweepLineIntersector();
                SegmentIntersector si = new SegmentIntersector(li, true, false);
                esi.ComputeIntersections(inputEdges, si, true);                

                IList splitEdges = new ArrayList();
                IEnumerator i = inputEdges.GetEnumerator();
                while (i.MoveNext()) 
                {
                    Edge e = (Edge)i.Current;
                    e.EdgeIntersectionList.AddSplitEdges(splitEdges);
                }
                return splitEdges;
            }
        }
    }
}
