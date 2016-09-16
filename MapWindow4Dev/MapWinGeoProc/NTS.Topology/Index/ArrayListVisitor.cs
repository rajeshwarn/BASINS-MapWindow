using System;
using System.Collections;
using System.Text;

namespace MapWinGeoProc.NTS.Topology.Index
{
    /// <summary>
    /// 
    /// </summary>
    public class ArrayListVisitor : IItemVisitor
    {
        private ArrayList items = new ArrayList();
        
        /// <summary>
        /// 
        /// </summary>
        public ArrayListVisitor() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public virtual void VisitItem(object item)
        {
            items.Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual ArrayList Items
        {
            get
            {
                return items;
            }
        }
    }
}
