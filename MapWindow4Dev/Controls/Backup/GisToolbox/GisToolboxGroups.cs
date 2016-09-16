// ----------------------------------------------------------------------------
// MapWindow.Controls.Globals: IGisToolboxGroups
// Author: Sergei Leschinski
// ----------------------------------------------------------------------------

namespace MapWindow.Controls.GisToolbox
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using MapWindow.Interfaces;
    using System.Windows.Forms;
    using System.Collections;

    /// <summary>
    /// Provies access to the list of groups of group toolbox
    /// </summary>
    public class GisToolboxGroups: IGisToolboxGroups
    {
        // list of nodes in underlying treeview
        private TreeNodeCollection m_nodes = null;

        /// <summary>
        /// Creates a new instance of GisToolboxGroups class. Public constructor must not be available
        /// </summary>
        /// <param name="nodes"></param>
        internal GisToolboxGroups(TreeNodeCollection nodes)
        {
            if (nodes == null)
                throw new NullReferenceException();
            m_nodes = nodes;
        }
    
        #region ICollection<IGisToolboxGroup> Members
        /// <summary>
        /// Adds new tool to the group
        /// </summary>
        public void Add(IGisToolboxGroup item)
        {
            if (this.Equals(item))
                throw new InvalidOperationException();
            
            GisToolboxGroup group = item as GisToolboxGroup;
            if (group == null)
                throw new InvalidCastException();

            int i = 0;
            for (; i < m_nodes.Count; i++)
            {
                if (m_nodes[i].Tag as IGisToolboxGroup == null)
                    break;
            }

            TreeNode node = ((GisToolboxGroup)item).Node;

            if (i < m_nodes.Count)
                m_nodes.Insert(i, node);
            else
                m_nodes.Add(node);
        }
        
        
        /// <summary>
        /// Clears all the groups
        /// </summary>
        public void Clear()
        {
            for (int i = m_nodes.Count - 1; i >= 0; i--)
            {
                IGisToolboxGroup group = m_nodes[i].Tag as IGisToolboxGroup;
                if (group != null)
                    m_nodes.RemoveAt(i);
            }
        }

        /// <summary>
        /// Informs whether list of groups contain particular group
        /// </summary>
        public bool Contains(IGisToolboxGroup item)
        {
            if (item == null)
                return false;

            for (int i = 0; i < m_nodes.Count; i++ )
            {
                IGisToolboxGroup group = m_nodes[i].Tag as IGisToolboxGroup;
                if (group == item)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Copies list of group to the array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(IGisToolboxGroup[] array, int arrayIndex)
        {
            if (object.ReferenceEquals(array, null))
                throw new ArgumentNullException("Null array reference");
            
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("Index is out of range");

            if (array.Rank > 1)
                throw new ArgumentException("Array is multi-dimensional");

            IEnumerable<TreeNode> nodes =  m_nodes.Cast<TreeNode>().Where(node => (node.Tag as IGisToolboxGroup != null));

            foreach (TreeNode node in nodes)
            {
                array.SetValue(node.Tag as IGisToolboxGroup, arrayIndex);
                arrayIndex++;
            }
        }

        /// <summary>
        /// Returns number of groups in the list
        /// </summary>
        public int Count
        {
            get 
            {
                return m_nodes.Cast<TreeNode>().Where(node => (node.Tag as IGisToolboxGroup != null)).Count();
            }
        }

        /// <summary>
        /// Get the read only flag for the collection
        /// </summary>
        public bool IsReadOnly
        {
            get {return m_nodes.IsReadOnly;}
        }

        /// <summary>
        /// Removes group at specified position
        /// </summary>
        public bool Remove(IGisToolboxGroup item)
        {
            foreach (TreeNode node in m_nodes)
            {
                if (node.Tag as IGisToolboxGroup == item)
                {
                    m_nodes.Remove(node);
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region IEnumerable Members
        /// <summary>
        /// Get typed enumerator for collection
        /// </summary>
        public IEnumerator<IGisToolboxGroup> GetEnumerator()
        {
            return new GroupEnumerator(m_nodes);
        }
        
        /// <summary>
        /// Get untyped enumeration for collection
        /// </summary>
        System.Collections.IEnumerator IEnumerable.GetEnumerator()
        {
            return new GroupEnumerator(m_nodes);
        }

        #endregion

        #region Enumerator
        /// <summary>
        /// Enumerator for groups collection
        /// </summary>
        private class GroupEnumerator : System.Collections.Generic.IEnumerator<IGisToolboxGroup>, System.Collections.IEnumerator
        {
            TreeNodeCollection m_nodes = null;
            int m_index = -1;

            /// <summary>
            /// Creates new instance of GroupEnumerator class
            /// </summary>
            /// <param name="nodes">The list of nodes in the underlying treeview</param>
            internal GroupEnumerator(TreeNodeCollection nodes)
            {
                if (nodes == null)
                    throw new NullReferenceException();
                m_nodes = nodes;
            }

            /// <summary>
            /// Gest the current item in the collection
            /// </summary>
            object IEnumerator.Current
            {
                get { return m_nodes[m_index].Tag;}
            }

            /// <summary>
            /// Get the current item
            /// </summary>
            public IGisToolboxGroup Current
            {
                get { return (IGisToolboxGroup)m_nodes[m_index].Tag; }
            }

            /// <summary>
            /// Moves to the next item
            /// </summary>
            public bool MoveNext()
            {
                do
                {
                    m_index++;
                    if (m_index == m_nodes.Count)
                        return false;
                    if (m_nodes[m_index].Tag as GisToolboxGroup != null)
                        return true;
                } while (true);
            }

            /// <summary>
            /// Sets enumrator in the beginning of collection
            /// </summary>
            public void Reset()
            {
                m_index = -1;
            }

            /// <summary>
            /// Disposes the items
            /// </summary>
            public void Dispose()
            {
                // do nothing
            }
        }
        #endregion
    }
}
