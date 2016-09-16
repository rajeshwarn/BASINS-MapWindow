// ********************************************************************************************************
// <copyright file="mwSymbology.cs" company="MapWindow.org">
// Copyright (c) MapWindow.org. All rights reserved.
// </copyright>
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http:// Www.mozilla.org/MPL/ 
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
// 
// The Initial Developer of this version of the Original Code is Sergei Leschinski
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// Change Log: 
// Date            Changed By      Notes
// ********************************************************************************************************

namespace mwSymbology.Forms
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using System.Text.RegularExpressions;
    using mwSymbology.Classes;
    using mwSymbology;
    using System.IO;
    using System.Diagnostics;
    #endregion

    public partial class frmOptionsManager : Form
    {
        #region Declarations
        // handle of the layer
        private int m_handle = -1;

        // reference to the MW4 application        
        private MapWindow.Interfaces.IMapWin m_mapWin = null;

        // layer to set options for
        private MapWindow.Interfaces.Layer m_layer = null;

        // Suppresses events on loading
        private bool m_noEvents = true;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of the frmOptionsManager class
        /// </summary>
        public frmOptionsManager(int handle, MapWindow.Interfaces.IMapWin mapWin)
        {
            InitializeComponent();

            m_handle = handle;
            m_mapWin = mapWin;

            m_layer = m_mapWin.Layers[m_handle];
            if (m_layer == null)
                throw new NullReferenceException("Invalid layer handle");

            SymbologyPlugin.FillSymbologyList(this.listView1, m_layer.FileName, true, ref m_noEvents);

            if (this.listView1.Items.Count > 0)
                this.LoadLayer();

            this.RefreshControlsState();
        }

        /// <summary>
        /// Removes all layers on closing
        /// </summary>
        private void frmOptionsManager_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (axMap1 != null)
            {
                axMap1.RemoveAllLayers();
                axMap1 = null;
            }
            GC.Collect();
        }

        /// <summary>
        /// Allows listview selection changed event
        /// </summary>
        private void frmOptionsManager_Shown(object sender, EventArgs e)
        {
            this.listView1.SelectedIndexChanged += delegate(object sender1, EventArgs e1)
            {
                this.ShowOptions();
            };
            m_noEvents = false;
            this.ShowOptions();
        }

        /// <summary>
        /// Loads layer from datasource specifed by filename
        /// </summary>
        private void LoadLayer()
        {
            axMap1.RemoveAllLayers();

            int handle = -1;
            string ext = System.IO.Path.GetExtension(m_layer.FileName).ToLower();
            if (ext == ".shp")
            {
                MapWinGIS.Shapefile sf = new MapWinGIS.Shapefile();
                if (sf.Open(m_layer.FileName , null))
                {
                    handle = axMap1.AddLayer(sf, true);
                    sf.Labels.SavingMode = MapWinGIS.tkSavingMode.modeNone;
                    sf.Charts.SavingMode = MapWinGIS.tkSavingMode.modeNone;
                }
            }
            else
            {
                MapWinGIS.Image img = new MapWinGIS.Image();
                if (img.Open(m_layer.FileName, MapWinGIS.ImageType.USE_FILE_EXTENSION, false, null))
                {
                    handle = this.axMap1.AddLayer(img, true);
                }
            }
        }
        #endregion

        #region Buttons
        /// <summary>
        /// Saves the current state of the layer
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            frmAddOptions form = new frmAddOptions();
            
            // some values can be set there
            form.txtName.Text = "";
            form.txtDescription.Text = "";

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                string value = form.txtName.Text;
                txtDescription.Text = form.txtDescription.Text;

                if (listView1.Items.Count == 0)
                    this.LoadLayer();

                // in case file exists, let's ask the user if we are to overwrite it
                string name = m_layer.FileName + "." + value + ".mwsymb";
                if (System.IO.File.Exists(name))
                {
                    if (MessageBox.Show("Set of options with such name already exists." + Environment.NewLine +
                                    "Do you want to rewrite it?", m_mapWin.ApplicationInfo.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                }

                AxMapWinGIS.AxMap map = m_mapWin.GetOCX as AxMapWinGIS.AxMap;
                map.SaveLayerOptions(m_handle, value, true, txtDescription.Text);

                // updating the list
                SymbologyPlugin.FillSymbologyList(listView1, m_layer.FileName, true, ref m_noEvents);

                this.RefreshControlsState();
            }
            form.Dispose();
        }

        /// <summary>
        /// Renames selected set of options (including file)
        /// </summary>
        private void btnRename_Click(object sender, EventArgs e)
        {
            string name; SymbologyType type;
            if (!CurrentNameAndType(out name, out type))
                return;

            if (type == SymbologyType.Default)
                return;

            string newName = name;
            
            frmAddOptions form = new frmAddOptions();
            form.Text = "Rename options";
            form.txtName.Text = newName;
            form.txtDescription.Text = this.txtDescription.Text; 

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                newName = form.txtName.Text.Trim();
                this.txtDescription.Text = form.txtDescription.Text; 
                
                try
                {
                    string oldFileame = m_layer.FileName + "." + name + ".mwsymb";
                    System.IO.File.Delete(oldFileame);

                    AxMapWinGIS.AxMap map = m_mapWin.GetOCX as AxMapWinGIS.AxMap;
                    map.SaveLayerOptions(map.get_LayerHandle(0), newName, true, txtDescription.Text);
                }
                catch
                {
                    SymbologyPlugin.MessageBoxError("Failed to rename file");
                    return;
                }
                
                // updating the list
                listView1.SelectedItems[0].Text = newName;
            }
            form.Dispose();
        }

        /// <summary>
        /// Applies the selected options and closes the form
        /// </summary>
        private void btnApply_Click(object sender, EventArgs e)
        {
            string name; SymbologyType type;
            if (!this.CurrentNameAndType(out name, out type))
                return;

            if (type == SymbologyType.Default)
            {
                name = "";
            }

            AxMapWinGIS.AxMap map = m_mapWin.GetOCX as AxMapWinGIS.AxMap;
            if (map != null)
            {
                int handle = m_layer.Handle;
                string description = "";
                if (map.LoadLayerOptions(handle, name, ref description))
                {
                    m_mapWin.View.Redraw();
                    m_mapWin.View.LegendControl.Refresh();
                }
                else
                {
                    SymbologyPlugin.MessageBoxError("Error while loading options");
                }
            }
        }

        /// <summary>
        /// Saves current sset of options as a default one
        /// </summary>
        private void btnMakDefault_Click(object sender, EventArgs e)
        {
            string name; SymbologyType type;
            if (!this.CurrentNameAndType(out name, out type))
                return;

            string filename = m_layer.FileName + "." + name + ".mwsymb";
            
            if (type != SymbologyType.Default && System.IO.File.Exists(filename))
            {
                // let's rename the existing default set
                string oldName = m_layer.FileName + ".mwsymb";
                if (System.IO.File.Exists(oldName))
                {
                    // seak for unoccupied name
                    int index = 0;
                    string newName = m_layer.FileName + ".untitled-" + index + ".mwsymb";
                    while (System.IO.File.Exists(newName))
                    {
                        index++;
                        newName = m_layer.FileName + ".untitled-" + index + ".mwsymb";
                    }
                    try
                    {
                        System.IO.File.Move(oldName, newName);
                    }
                    catch
                    {
                        SymbologyPlugin.MessageBoxError("Failed to rename file");
                        return;
                    }
                }

                // renaming currnet file
                File.Move(filename, oldName);
                SymbologyPlugin.FillSymbologyList(listView1, m_layer.FileName, true, ref m_noEvents);
            }
        }

        /// <summary>
        /// Removes all the available options for the layer
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
                return;

            AxMapWinGIS.AxMap map = m_mapWin.GetOCX as AxMapWinGIS.AxMap;
            if (map != null)
            {
                if (MessageBox.Show("Do you want to remove all option sets for the layer?", m_mapWin.ApplicationInfo.ApplicationName,
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int errorCount = 0;
                    for (int row = 0; row < listView1.Items.Count; row++)
                    {
                        string name = listView1.Items[row].ToString();
                        SymbologyType type = (SymbologyType)listView1.Items[row].Tag;

                        if (type == SymbologyType.Default)
                            name = "";

                        if (!map.RemoveLayerOptions(m_layer.Handle, name))
                        {
                            errorCount++;
                        }
                    }

                    // redrawing
                    SymbologyPlugin.FillSymbologyList(listView1, m_layer.FileName, true, ref m_noEvents);

                    if (errorCount > 0)
                    {
                        SymbologyPlugin.MessageBoxError("Failed to remove options: " + errorCount + Environment.NewLine + "Reason: " +
                                        map.get_ErrorMsg(map.LastErrorCode));
                    }
                }
            }
            this.RefreshControlsState();
        }

        /// <summary>
        /// Removes the selected options, either .mwsymb or .mwsr
        /// </summary>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            string name; SymbologyType type;
            if (!this.CurrentNameAndType(out name, out type))
                return;

            if (MessageBox.Show("Do you want to remove the following set of options: " + name + "?", m_mapWin.ApplicationInfo.ApplicationName,
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                AxMapWinGIS.AxMap map = m_mapWin.GetOCX as AxMapWinGIS.AxMap;
                if (map != null)
                {
                    if (type == SymbologyType.Default)
                        name = "";

                    if (map.RemoveLayerOptions(m_layer.Handle, name))
                    {
                        SymbologyPlugin.FillSymbologyList(listView1, m_layer.FileName, true, ref m_noEvents);
                    }
                    else
                    {
                        SymbologyPlugin.MessageBoxError("Failed to remove options." + Environment.NewLine +
                                        "Reason: " + map.get_ErrorMsg(map.LastErrorCode));
                    }
                }
            }
            this.RefreshControlsState();
        }
        #endregion

        #region Events
        /// <summary>
        /// Returns name and type of the currently selected row
        /// </summary>
        private bool CurrentNameAndType(out string name, out SymbologyType type)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                name = "";
                type = SymbologyType.Custom;
                return false;
            }
            else
            {
                name = listView1.SelectedItems[0].Text;
                type = (SymbologyType)listView1.SelectedItems[0].Tag;
                return true;
            }
        }
        
        /// <summary>
        /// Shows rename window if applicable
        /// </summary>
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                if ((SymbologyType)listView1.SelectedItems[0].Tag == SymbologyType.Custom)
                {
                    this.btnRename_Click(null, null);
                }
            }
        }

        /// <summary>
        /// Shows options. If there is no selection, sets it first
        /// </summary>
        private void ShowOptions()
        {
            if (m_noEvents)
                return;

            if (listView1.Items.Count == 0)
            {
                axMap1.RemoveAllLayers();
                txtDescription.Text = "";
            }
            else
            {
                string name; SymbologyType type;
                if (!this.CurrentNameAndType(out name, out type))
                    return;

                if (type == SymbologyType.Default)
                {
                    name = "";
                    txtDescription.Text = SymbologyPlugin.GetSymbologyDescription(SymbologyType.Default);
                }

                // previously saved options (.mwsymb or .mwsr file)
                int handle = axMap1.get_LayerHandle(0);
                string description = "";
                if (!axMap1.LoadLayerOptions(handle, name, ref description))
                {
                    SymbologyPlugin.MessageBoxError("Error while loading options");
                }
                else
                {
                    if (name != "")
                        this.txtDescription.Text = description;
                    axMap1.Redraw();
                }
            }
            this.RefreshControlsState();
        }

        /// <summary>
        /// Updates enabled property of the controls
        /// </summary>
        private void RefreshControlsState()
        {
            bool enabled = listView1.SelectedItems.Count > 0;
            btnApply.Enabled = enabled;
            btnMakDefault.Enabled = enabled;
            btnRemove.Enabled = enabled;
            btnRename.Enabled = enabled;
            txtDescription.Enabled = true;
        }
        #endregion
    }
}
