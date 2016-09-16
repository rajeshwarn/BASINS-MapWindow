using System;
using System.Collections;
using System.Text;

using MapWinGeoProc.NTS.Topology.Geometries;
using MapWinGeoProc.NTS.Topology.GeometriesGraph;
using MapWinGeoProc.NTS.Topology.Algorithm;
using MapWinGeoProc.NTS.Topology.Utilities;
using MapWindow.Interfaces.Geometries;


namespace MapWinGeoProc.NTS.Topology.Operation.Valid
{
    /// <summary>
    /// Tests whether any of a set of <c>LinearRing</c>s are
    /// nested inside another ring in the set, using a simple O(n^2)
    /// comparison.
    /// </summary>
    public class SimpleNestedRingTester
    {        
        private GeometryGraph graph;  // used to find non-node vertices
        private IList rings = new ArrayList();
        private Coordinate nestedPt;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph"></param>
        public SimpleNestedRingTester(GeometryGraph graph)
        {
            this.graph = graph;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ring"></param>
        public virtual void Add(LinearRing ring)
        {
            rings.Add(ring);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Coordinate NestedPoint
        {
            get
            {
                return nestedPt;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool IsNonNested()
        {
            for (int i = 0; i < rings.Count; i++) 
            {
                LinearRing innerRing = (LinearRing) rings[i];
                ICoordinate[] innerRingPts = innerRing.Coordinates;

                for (int j = 0; j < rings.Count; j++) 
                {
                    LinearRing searchRing = (LinearRing) rings[j];
                    ICoordinate[] searchRingPts = searchRing.Coordinates;

                    if (innerRing == searchRing) continue;

                    if (!innerRing.EnvelopeInternal.Intersects(searchRing.EnvelopeInternal)) continue;

                    Coordinate innerRingPt = IsValidOp.FindPointNotNode(innerRingPts, searchRing, graph);
                    Assert.IsTrue(innerRingPt != null, "Unable to find a ring point not a node of the search ring");

                    bool isInside = CGAlgorithms.IsPointInRing(innerRingPt, searchRingPts);
                    if (isInside)
                    {
                        nestedPt = innerRingPt;
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
