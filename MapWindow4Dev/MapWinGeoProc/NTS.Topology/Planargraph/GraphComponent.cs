using System;
using System.Collections;

namespace MapWinGeoProc.NTS.Topology.Planargraph
{
    /// <summary>
    /// The base class for all graph component classes.
    /// Maintains flags of use in generic graph algorithms.
    /// Provides two flags:
    /// marked - typically this is used to indicate a state that persists
    /// for the course of the graph's lifetime.  For instance, it can be
    /// used to indicate that a component has been logically deleted from the graph.
    /// visited - this is used to indicate that a component has been processed
    /// or visited by an single graph algorithm.  For instance, a breadth-first traversal of the
    /// graph might use this to indicate that a node has already been traversed.
    /// The visited flag may be set and cleared many times during the lifetime of a graph.
    /// </summary>
    public abstract class GraphComponent
    {

        #region Static

        /// <summary>
        /// Sets the <see cref="GraphComponent.Visited" /> state 
        /// for all <see cref="GraphComponent" />s in an <see cref="IEnumerator" />.
        /// </summary>
        /// <param name="i">A <see cref="IEnumerator" /> to scan.</param>
        /// <param name="visited">The state to set the <see cref="GraphComponent.Visited" /> flag to.</param>
        public static void SetVisited(IEnumerator i, bool visited)
        {
            while (i.MoveNext())
            {
                GraphComponent comp = (GraphComponent) i.Current;
                comp.Visited = visited;
            }
        }

        /// <summary>
        /// Sets the <see cref="GraphComponent.Marked" /> state 
        /// for all <see cref="GraphComponent" />s in an <see cref="IEnumerator" />.
        /// </summary>
        /// <param name="i">A <see cref="IEnumerator" /> to scan.</param>
        /// <param name="marked">The state to set the <see cref="GraphComponent.Marked" /> flag to.</param>
        public static void SetMarked(IEnumerator i, bool marked)
        {
            while (i.MoveNext())
            {
                GraphComponent comp = (GraphComponent)i.Current;
                comp.Marked = marked;
            }
        }

        /// <summary>
        /// Finds the first <see cref="GraphComponent" /> 
        /// in a <see cref="IEnumerator" /> set
        /// which has the specified <see cref="GraphComponent.Visited" /> state.
        /// </summary>
        /// <param name="i">A <see cref="IEnumerator" /> to scan.</param>
        /// <param name="visitedState">The <see cref="GraphComponent.Visited" /> state to test.</param>
        /// <returns>The first <see cref="GraphComponent" /> found, or <c>null</c> if none found.</returns>
        public static GraphComponent GetComponentWithVisitedState(IEnumerator i, bool visitedState)
        {
            while (i.MoveNext())
            {
                GraphComponent comp = (GraphComponent)i.Current;
                if (comp.IsVisited == visitedState)
                    return comp;
            }
            return null;
        }
        
        #endregion

        /// <summary>
        /// 
        /// </summary>
        protected bool isMarked = false;

        /// <summary>
        /// 
        /// </summary>
        protected bool isVisited = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:GraphComponent"/> class.
        /// </summary>
        public GraphComponent() { }

        /// <summary>
        /// Tests if a component has been visited during the course of a graph algorithm.
        /// </summary>              
        public virtual bool IsVisited
        {
            get
            {
                return Visited;
            }
        }

        /// <summary> 
        /// Gets/Sets the visited flag for this component.
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
        /// Tests if a component has been marked at some point during the processing
        /// involving this graph.
        /// </summary>
        public virtual bool IsMarked
        {
            get
            {
                return Marked;
            }
        }

        /// <summary>
        /// Gets/Sets the marked flag for this component.
        /// </summary>
        public virtual bool Marked
        {
            get
            {
                return isMarked;
            }
            set
            {
                isMarked = value;
            }
        }

        /// <summary>
        /// Tests whether this component has been removed from its containing graph.
        /// </summary>
        public abstract bool IsRemoved { get; }

        /*
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is GraphComponent))
                return false;
            if (!base.Equals(obj))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;
             
            GraphComponent other = obj as GraphComponent;
            if (IsMarked != other.IsMarked)
                return false;
            if (IsVisited != other.IsVisited)
                return false;
            if (IsRemoved != other.IsRemoved)
                return false;           
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int result = 29 * IsMarked.GetHashCode();
            result += 14 + 29 * IsVisited.GetHashCode();
            result += 14 + 29 * IsRemoved.GetHashCode();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static bool operator ==(GraphComponent o1, GraphComponent o2)
        {
            return Object.Equals(o1, o2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static bool operator !=(GraphComponent o1, GraphComponent o2)
        {
            return !(o1 == o2);
        }
        
        */

    }
}
