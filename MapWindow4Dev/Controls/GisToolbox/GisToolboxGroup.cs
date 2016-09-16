// ----------------------------------------------------------------------------
// MapWindow.Controls.Globals: GIS toolbox
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
    
    /// <summary>
    /// Provides information about GIS tool
    /// </summary>
    public class GisToolboxGroup : IGisToolboxGroup
    {
        string m_name;
        string m_description;
        object m_tag;
        TreeNode m_node;

        /// <summary>
        /// Creates a new instance of GIS tool class
        /// </summary>
        internal GisToolboxGroup(string name, string description)
        {
            m_name = name;
            m_description = description;
            m_tag = null;

            m_node = new TreeNode();
            m_node.Text = name;
            m_node.ImageIndex = GisToolbox.ICON_FOLDER;
            m_node.Expand();

            m_node.Tag = this;
        }

        /// <summary>
        /// Source tree node for the group
        /// </summary>
        internal TreeNode Node
        {
            get
            { 
                return m_node; 
            }
        }

        /// <summary>
        /// Get ot sets the description of GIS tool
        /// </summary>
        public string Description
        {
            get{ return m_description; }
            set{ m_description = value; }
        }

        /// <summary>
        /// Gets or sets the name of tool
        /// </summary>
        public string Name
        {
            get{ return m_name; }
            set
            {
                m_name = value;
                if (m_node != null)
                    m_node.Text = m_name;
            }
        }

        /// <summary>
        /// Gets or sets the tag associated with tool
        /// </summary>
        public object Tag
        {
            get{ return m_tag; }
            set{
                m_tag = value;
            }
        }

        /// <summary>
        /// Gets list of child groups fro current group
        /// </summary>
        public IGisToolboxGroups SubGroups
        {
            get 
            {
                if (m_node == null)
                    throw new NullReferenceException();
                else
                    return new GisToolboxGroups(m_node.Nodes) ;
            }
        }

        /// <summary>
        /// Gets list of tools for current group
        /// </summary>
        public IGisTools Tools
        {
            get
            {
                return new GisTools(m_node.Nodes);
            }
        }
        
        /// <summary>
        /// Gets or sets the expanded state of group in treeview
        /// </summary>
        public bool Expanded
        {
            get
            {
                return m_node.IsExpanded;
            }
            set
            {
                if (value)
                    m_node.Expand();
                else
                    m_node.Collapse();
            }
        }
    }
}
