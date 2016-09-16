using System;
using System.Collections;
using System.Text;

using MapWinGeoProc.NTS.Topology.Geometries;
using MapWinGeoProc.NTS.Topology.Utilities;
using MapWindow.Interfaces.Geometries;
namespace MapWinGeoProc.NTS.Topology.GeometriesGraph
{
    /// <summary>
    /// A GraphComponent is the parent class for the objects'
    /// that form a graph.  Each GraphComponent can carry a
    /// Label.
    /// </summary>
    abstract public class GraphComponent
    {        
        /// <summary>
        /// 
        /// </summary>
        protected Label label;

        // isInResult indicates if this component has already been included in the result
        private bool isInResult = false;

        private bool isCovered = false;
        private bool isCoveredSet = false;
        private bool isVisited = false;

        /// <summary>
        /// 
        /// </summary>
        public GraphComponent() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        public GraphComponent(Label label)
        {
            this.label = label;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Label Label
        {
            get
            {
                return label;
            }
            set
            {
                label = value;
            }
        }     
                
        /// <summary>
        /// 
        /// </summary>
        public virtual bool InResult
        { 
            get
            {
                return isInResult;
            }
            set
            {
                isInResult = value;
            }           
        }

        /// <summary> 
        /// IsInResult indicates if this component has already been included in the result.
        /// </summary>
        public virtual bool IsInResult
        {
            get
            {
                return InResult;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool Covered
        {
            get
            {
                return this.isCovered;
            }
            set
            {
                isCovered = value;
                isCoveredSet = true;                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsCovered
        {
            get
            {
                return Covered;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsCoveredSet 
        {
            get
            {
                return isCoveredSet;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool Visited
        {
            get
            {
                return isVisited;
            }
            set
            {
                isVisited = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsVisited
        {
            get
            {
                return isVisited;
            }
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// A coordinate in this component (or null, if there are none).
        /// </returns>
        abstract public ICoordinate Coordinate { get; }

        /// <summary>
        /// Compute the contribution to an IM for this component.
        /// </summary>
        abstract public void ComputeIM(IntersectionMatrix im);

        /// <summary>
        /// An isolated component is one that does not intersect or touch any other
        /// component.  This is the case if the label has valid locations for
        /// only a single Geometry.
        /// </summary>
        /// <returns><c>true</c> if this component is isolated.</returns>
        abstract public bool IsIsolated { get; }

        /// <summary>
        /// Update the IM with the contribution for this component.
        /// A component only contributes if it has a labelling for both parent geometries.
        /// </summary>
        /// <param name="im"></param>
        public virtual void UpdateIM(IntersectionMatrix im)
        {
            Assert.IsTrue(label.GeometryCount >= 2, "found partial label");
            ComputeIM(im);
        }
    }
}
