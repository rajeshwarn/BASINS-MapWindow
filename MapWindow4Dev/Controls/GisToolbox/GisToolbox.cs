// ----------------------------------------------------------------------------
// MapWindow.Controls.Globals: GIS toolbox
// Author: Sergei Leschinski
// ----------------------------------------------------------------------------

namespace MapWindow.Controls.GisToolbox
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;
    using MapWindow.Interfaces;
    
    /// <summary>
    /// Gistoolbox control
    /// </summary>
    public class GisToolbox : SplitContainer, IGisToolBox 
    {
        // icon indices
        internal const int ICON_FOLDER = 0;
        internal const int ICON_FOLDER_OPEN = 1;
        internal const int ICON_TOOL = 2;
        
        // tree view
        TreeView m_tree = null;
        
        // text box to show descriptions
        RichTextBox m_textbox = null;

        #region Initialization
        /// <summary>
        /// Creates a new instance of GIS toolbox class
        /// </summary>
        public GisToolbox()
        {
            this.BorderStyle = BorderStyle.FixedSingle;
            
            m_tree = new TreeView();
            m_tree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            m_tree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel1.Controls.Add(m_tree);

            m_textbox = new RichTextBox();
            m_textbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            m_textbox.Dock = System.Windows.Forms.DockStyle.Fill;
            m_textbox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            m_textbox.BackColor = Color.FromKnownColor(KnownColor.Control);
            m_textbox.ReadOnly = true;
            m_textbox.Text = "No tool is selected.";
            this.Panel2.Controls.Add(m_textbox);

            this.Panel1MinSize = 0;
            this.Panel2MinSize = 0;
            this.Orientation = Orientation.Horizontal;
            this.SplitterDistance = Convert.ToInt32((double)this.Height * 0.9);

		    this.InitImageList();
            
            // adding event handlers
            m_tree.BeforeExpand += m_GeoprocessingTree_BeforeExpand;
            m_tree.BeforeCollapse += m_GeoprocessingTree_BeforeCollapse;
            m_tree.AfterSelect += m_GeoprocessingTree_AfterSelect;
            m_tree.NodeMouseDoubleClick += m_tree_NodeMouseDoubleClick;
            this.ToolSelected += new ToolSelectedDelegate(GisToolbox_ToolSelected);
            this.GroupSelected += new GroupSelectedDelegate(GisToolbox_GroupSelected);
        }

        /// <summary>
        /// Shows group description
        /// </summary>
        void GisToolbox_GroupSelected(IGisToolboxGroup group, ref bool handled)
        {
            this.m_textbox.Clear();
            this.m_textbox.Text = group.Name + Environment.NewLine + Environment.NewLine + group.Description;
            this.m_textbox.Select(0, group.Name.Length);
            this.m_textbox.SelectionFont = new Font(this.Font, FontStyle.Bold);
            handled = true;
        }

        /// <summary>
        /// Shows tool description
        /// </summary>
        void GisToolbox_ToolSelected(IGisTool tool, ref bool handled)
        {
            this.m_textbox.Clear();
            this.m_textbox.Text = tool.Name + Environment.NewLine + Environment.NewLine + tool.Description;
            if (tool.Name.Length > 0)
            {
                this.m_textbox.Select(0, tool.Name.Length);
                this.m_textbox.SelectionFont = new Font(this.Font, FontStyle.Bold);
            }
            handled = true;
        }

	    /// <summary>
	    /// Initializes image list
	    /// </summary>
	    private void InitImageList()
	    {
		    ImageList imageList = new ImageList();
		    imageList.ColorDepth = ColorDepth.Depth32Bit;

            Bitmap bmp = new Bitmap(MapWindow.Controls.Properties.Resources.folder, new Size(16, 16));
		    imageList.Images.Add(bmp);

            bmp = new Bitmap(MapWindow.Controls.Properties.Resources.folder_open, new Size(16, 16));
		    imageList.Images.Add(bmp);

            bmp = new Bitmap(MapWindow.Controls.Properties.Resources.tool, new Size(16, 16));
		    imageList.Images.Add(bmp);

		    m_tree.ImageList = imageList;
        }
        #endregion

        #region IGisToolBox Members

        /// <summary>
        /// Fired when user clicks on tool to run it
        /// </summary>
        public event ToolSelectedDelegate ToolClicked;

        /// <summary>
        /// Passes the event to all listeners
        /// </summary>
        private void FireToolClicked(IGisTool tool, ref bool handled)
        {
            if (this.ToolClicked != null)
            {
                this.ToolClicked(tool, ref handled);
            }
        }

        /// <summary>
        /// Fired when tool is selected
        /// </summary>
        public event ToolSelectedDelegate ToolSelected;

        /// <summary>
        /// Passes the event to all listeners
        /// </summary>
        private void FireToolSelected(IGisTool tool, ref bool handled)
        {
            if (this.ToolSelected != null)
            {
                this.ToolSelected(tool, ref handled);
            }
        }

        /// <summary>
        /// Fired when group is selected
        /// </summary>
        public event GroupSelectedDelegate GroupSelected;

        /// <summary>
        /// Passes the event to all listeners
        /// </summary>
        private void FireGroupSelected(IGisToolboxGroup group, ref bool handled)
        {
            if (this.GroupSelected != null)
            {
                this.GroupSelected(group, ref handled);
            }
        }

        /// <summary>
        /// Returns list of groups located on the first level of toolbox.
        /// The list is gathered dynamically from nodes on each call, so better not to call it repeatedly
        /// </summary>
        public IGisToolboxGroups Groups
        {
            get 
            {
                return new GisToolboxGroups(m_tree.Nodes);
            }
        }

        /// <summary>
        /// Returns list of all tools in the toolbox
        /// </summary>
        public IEnumerable<IGisTool> Tools
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Creates a new instance of GisTool class
        /// </summary>
        public IGisTool CreateTool(string name, string key)
        {
            GisTool tool = new GisTool(name, key, "");
            return tool;
        }

        /// <summary>
        /// Creates a new instance of GisToolboxGroup class
        /// </summary>
        public IGisToolboxGroup CreateGroup(string name, string description)
        {
            return new GisToolboxGroup(name, description);
        }

        /// <summary>
        /// Expands all the groups up to the specified level
        /// </summary>
        public void ExpandGroups(int level)
        {
            expandGroups(this.Groups, level);
        }

        /// <summary>
        /// Recursively expans all the child groups up to the specified level
        /// </summary>
        private void expandGroups(IGisToolboxGroups groups, int level)
        {
            foreach (IGisToolboxGroup group in this.Groups)
            {
                group.Expanded = true;
                level--;
                if (level > 0)
                    expandGroups(group.SubGroups, level);
            }
        }
        #endregion

        #region TreeView events
        /// <summary>
        /// Sets the closed state of folder
        /// </summary>
        private void m_GeoprocessingTree_BeforeCollapse(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
        {
            if ((e.Node != null))
            {
                if (e.Node.ImageIndex == ICON_FOLDER_OPEN)
                {
                    e.Node.ImageIndex = ICON_FOLDER;
                    e.Node.SelectedImageIndex = ICON_FOLDER;
                }
            }
        }

        /// <summary>
        /// Sets the opened state of folder
        /// </summary>
        private void m_GeoprocessingTree_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
        {
            if ((e.Node != null))
            {
                if (e.Node.ImageIndex == ICON_FOLDER)
                {
                    e.Node.ImageIndex = ICON_FOLDER_OPEN;
                    e.Node.SelectedImageIndex = ICON_FOLDER_OPEN;
                }
            }
        }
        
        /// <summary>
	    /// Fires events, sets the same icons for selected mode as for regular mode
	    /// </summary>
	    private void m_GeoprocessingTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
	    {
		    if ((e.Node != null)) 
            {
			    e.Node.SelectedImageIndex = e.Node.ImageIndex;
			    if ((e.Node.Tag != null)) 
                {
				    bool handled = false;
                    if (e.Node.ImageIndex == ICON_TOOL)
                    {
                        IGisTool tool = e.Node.Tag as IGisTool;
                        if (tool != null)
                            FireToolSelected(tool, ref handled);
                    }
                    else
                    {
                        // assume that it is a folder
                        IGisToolboxGroup group = e.Node.Tag as IGisToolboxGroup;
                        if (group != null) 
						    FireGroupSelected(group, ref handled);
                    }
			    }
		    }
	    }

        /// <summary>
        /// Generates tool clicked event for plug-ins
        /// </summary>
        private void m_tree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node != null && e.Node.Tag != null)
            {
                if (e.Node.ImageIndex == ICON_TOOL)
                {
                    bool handled = false;
                    IGisTool tool = e.Node.Tag as IGisTool;
                    if (tool != null)
                        FireToolClicked(tool, ref handled);
                }
            }
        }
        #endregion
    }
}