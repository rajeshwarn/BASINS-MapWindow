// ----------------------------------------------------------------------------
// MapWindow.Controls.Globals: GIS tool
// Author: Sergei Leschinski
// ----------------------------------------------------------------------------

namespace MapWindow.Controls.GisToolbox
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using MapWindow.Interfaces;

    /// <summary>
    /// Provides information about GIS tool
    /// </summary>
    public class GisTool : IGisTool
    {
        string m_name;
        string m_description;
        string m_key;
        object m_tag;
        TreeNode m_node;

        /// <summary>
        /// Creates a new instance of GIS tool class
        /// </summary>
        internal GisTool(string name, string key, string description)
        {
            m_name = name;
            m_description = description;
            m_key = key;
            m_tag = null;
            m_node = new TreeNode();
            m_node.Text = m_name;
            m_node.ImageIndex = GisToolbox.ICON_TOOL;
            m_node.Tag = this;
        }

        /// <summary>
        /// The reference to the underlying node
        /// </summary>
        internal TreeNode Node
        {
            get { return m_node;}
        }

        /// <summary>
        /// Get ot sets the description of GIS tool
        /// </summary>
        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }

        /// <summary>
        /// Gets or sets the name of tool
        /// </summary>
        public string Name
        {
            get { return m_name; }
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
            set{ m_tag = value; }
        }

        /// <summary>
        /// Gets or sets the key of the tool
        /// </summary>
        public string Key
        {
            get { return m_key; }
            set { m_key = value; }
        }
    }
}
