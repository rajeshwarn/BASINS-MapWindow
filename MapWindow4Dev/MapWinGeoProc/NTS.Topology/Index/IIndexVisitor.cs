using System;
using System.Collections;
using System.Text;

namespace MapWinGeoProc.NTS.Topology.Index
{
    /// <summary> 
    /// A visitor for nodes and items in an index.
    /// </summary> 
    public interface IIndexVisitor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        void VisitItem(object item);
    }
}
