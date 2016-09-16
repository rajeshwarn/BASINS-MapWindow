using System;
using System.Collections;
using System.Text;

using MapWinGeoProc.NTS.Topology.Algorithm;
using MapWinGeoProc.NTS.Topology.Geometries;
using MapWinGeoProc.NTS.Topology.GeometriesGraph;
using MapWinGeoProc.NTS.Topology.Index.Quadtree;
using MapWinGeoProc.NTS.Topology.Utilities;
using MapWindow.Interfaces.Geometries;
namespace MapWinGeoProc.NTS.Topology.Operation.Valid
{
    /// <summary>
    /// Tests whether any of a set of <c>LinearRing</c>s are
    /// nested inside another ring in the set, using a <c>Quadtree</c>
    /// index to speed up the comparisons.
    /// </summary>
    public class QuadtreeNestedRingTester
    {
        private GeometryGraph graph;  // used to find non-node vertices
        private IList rings = new ArrayList();
        private Envelope totalEnv = new Envelope();
        private Quadtree quadtree;
        private Coordinate nestedPt;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph"></param>
        public QuadtreeNestedRingTester(GeometryGraph graph)
        {
            this.graph = graph;
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
        /// <param name="ring"></param>
        public virtual void Add(LinearRing ring)
        {
            rings.Add(ring);
            totalEnv.ExpandToInclude(ring.EnvelopeInternal);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool IsNonNested()
        {
            BuildQuadtree();

            for (int i = 0; i < rings.Count; i++)
            {
                LinearRing innerRing = (LinearRing)rings[i];
                ICoordinate[] innerRingPts = innerRing.Coordinates;

                IList results = quadtree.Query(innerRing.EnvelopeInternal);
                for (int j = 0; j < results.Count; j++)
                {
                    LinearRing searchRing = (LinearRing)results[j];
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

        /// <summary>
        /// 
        /// </summary>
        private void BuildQuadtree()
        {
            quadtree = new Quadtree();

            for (int i = 0; i < rings.Count; i++)
            {
                LinearRing ring = (LinearRing)rings[i];
                IEnvelope env = ring.EnvelopeInternal;
                quadtree.Insert(env, ring);
            }
        }
    }
}
