// ----------------------------------------------------------------------------
// MapWindow.Controls.GisToolbox: GisTools
// Author: Sergei Leschinski
// ----------------------------------------------------------------------------

namespace MapWindow.Controls.GisToolbox
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using MapWindow.Interfaces;

    /// <summary>
    /// Provies access to the list of groups of group toolbox
    /// </summary>
    class GisTools : IGisTools
    {
        // list of nodes in underlying treeview
        private TreeNodeCollection m_nodes = null;

        /// <summary>
        /// Creates a new instance of GisToolboxGroups class. Public constructor must not be available
        /// </summary>
        /// <param name="nodes"></param>
        internal GisTools(TreeNodeCollection nodes)
        {
            if (nodes == null)
                throw new NullReferenceException();
            m_nodes = nodes;
        }

        #region ICollection<IGisTool> Members
        /// <summary>
        /// Adds new tool to the group
        /// </summary>
        public void Add(IGisTool item)
        {
            GisTool tool = item as GisTool;
            if (tool == null)
                throw new InvalidCastException("Gis tool class must be created by calling GisTool.CreateTool");
            
            m_nodes.Add(tool.Node);
        }

        /// <summary>
        /// Clears all the groups
        /// </summary>
        public void Clear()
        {
            for (int i = m_nodes.Count - 1; i >= 0; i++)
            {
                IGisTool tool = m_nodes[i].Tag as IGisTool;
                if (tool != null)
                    m_nodes.RemoveAt(i);
            }
        }

        /// <summary>
        /// Informs whether list of groups contain particular group
        /// </summary>
        public bool Contains(IGisTool item)
        {
            if (item == null)
                return false;

            for (int i = 0; i < m_nodes.Count; i++)
            {
                IGisTool tool = m_nodes[i].Tag as IGisTool;
                if (tool == item)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Copies list of group to the array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(IGisTool[] array, int arrayIndex)
        {
            if (object.ReferenceEquals(array, null))
                throw new ArgumentNullException("Null array reference");

            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("Index is out of range");

            if (array.Rank > 1)
                throw new ArgumentException("Array is multi-dimensional");

            IEnumerable<TreeNode> nodes = m_nodes.Cast<TreeNode>().Where(node => (node.Tag as IGisTool != null));

            foreach (TreeNode node in nodes)
            {
                array.SetValue(node.Tag as GisTool, arrayIndex);
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
                return m_nodes.Cast<TreeNode>().Where(node => (node.Tag as IGisTool != null)).Count();
            }
        }

        /// <summary>
        /// Get the read only flag for the collection
        /// </summary>
        public bool IsReadOnly
        {
            get { return m_nodes.IsReadOnly; }
        }

        /// <summary>
        /// Removes group at specified position
        /// </summary>
        public bool Remove(IGisTool item)
        {
            foreach (TreeNode node in m_nodes)
            {
                if (node.Tag as IGisTool == item)
                {
                    m_nodes.Remove(node);
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region IEnumerable
        /// <summary>
        /// Get typed enumerator for collection
        /// </summary>
        public IEnumerator<IGisTool> GetEnumerator()
        {
            return new ToolEnumerator(m_nodes);
        }

        /// <summary>
        /// Get untyped enumeration for collection
        /// </summary>
        System.Collections.IEnumerator IEnumerable.GetEnumerator()
        {
            return new ToolEnumerator(m_nodes);
        }
        #endregion

        #region Enumerator
        /// <summary>
        /// Enumerator for groups collection
        /// </summary>
        private class ToolEnumerator : System.Collections.Generic.IEnumerator<IGisTool>, System.Collections.IEnumerator
        {
            TreeNodeCollection m_nodes = null;
            int m_index = -1;

            /// <summary>
            /// Creates new instance of GroupEnumerator class
            /// </summary>
            /// <param name="nodes">The list of nodes in the underlying treeview</param>
            internal ToolEnumerator(TreeNodeCollection nodes)
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
                get { return m_nodes[m_index].Tag; }
            }

            /// <summary>
            /// Get the current item
            /// </summary>
            public IGisTool Current
            {
                get { return (IGisTool)m_nodes[m_index].Tag; }
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
                    if (m_nodes[m_index].Tag as IGisTool != null)
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
