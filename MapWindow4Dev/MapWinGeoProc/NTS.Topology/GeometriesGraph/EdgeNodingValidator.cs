using System;
using System.Collections;
using System.Text;

using MapWinGeoProc.NTS.Topology.Geometries;
using MapWinGeoProc.NTS.Topology.Algorithm;
using MapWinGeoProc.NTS.Topology.Noding;

namespace MapWinGeoProc.NTS.Topology.GeometriesGraph
{
    /// <summary>
    /// Validates that a collection of SegmentStrings is correctly noded.
    /// Throws an appropriate exception if an noding error is found.
    /// </summary>
    public class EdgeNodingValidator
    {        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="edges"></param>
        /// <returns></returns>
        private static IList ToSegmentStrings(IList edges)
        {
            // convert Edges to SegmentStrings
            IList segStrings = new ArrayList();
            for (IEnumerator i = edges.GetEnumerator(); i.MoveNext(); )
            {
                Edge e = (Edge)i.Current;
                segStrings.Add(new SegmentString(e.Coordinates, e));
            }
            return segStrings;
        }

        private NodingValidator nv;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="edges"></param>
        public EdgeNodingValidator(IList edges)
        {
            nv = new NodingValidator(ToSegmentStrings(edges));
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void checkValid()
        {
            nv.CheckValid();
        }
    }
}
