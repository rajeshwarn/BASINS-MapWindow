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
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;
    using MapWinGIS;
    using mwSymbology.Classes;
    using System.Drawing.Drawing2D;
    using System.Drawing;

    partial class frmSymbologyMain
    {
        /// <summary>
        /// Initializes labels tab
        /// </summary>
        private void InitLabelsTab()
        {
            MapWinGIS.Labels lb = m_shapefile.Labels;
            chkShowLabels.Checked = lb.Visible;
            
            chkLabelFrame.Checked = lb.FrameVisible;
            clpLabelFrame.Color = MapWinUtility.Colors.IntegerToColor(lb.FrameBackColor);
            udLabelFontSize.SetValue(lb.FontSize);
        }

        /// <summary>
        /// Generating of labels
        /// </summary>
        private void btnGenerateLabels_Click(object sender, EventArgs e)
        {
            LegendControl.Layer layer = m_mapWin.View.LegendControl.Layers.ItemByHandle(m_layerHandle);
            if (layer == null)
                return;

            //frmGenerateLabels form = new frmGenerateLabels(m_mapWin, m_shapefile, layer, LabelAction.ChangeAll);

            frmLabelStyle form = new frmLabelStyle(sfPreview, m_mapWin, m_layerHandle);
            if (form.ShowDialog(this) == DialogResult.OK)
            {           
                m_shapefile.Labels.Visible = true;
                DrawLabelsPreview();
                RefreshControlsState(null, null);
                RedrawMap();
            }
            form.Dispose();
        }

        /// <summary>
        /// Updates preview of the labels
        /// </summary>
        private void DrawLabelsPreview()
        {
            LabelUtilities.DrawPreview(m_shapefile.Labels.Options, m_shapefile, pctLabelPreview, false, Color.White);
        }

        /// <summary>
        /// Changing the default style of labels
        /// </summary>
        private void btnLabelsAppearance_Click(object sender, EventArgs e)
        {
            frmLabelStyle styleForm = new frmLabelStyle(m_shapefile, m_mapWin, m_layerHandle );
            styleForm.ShowDialog();

            // updating controls (even if cancel was hit, a user could have applied the options)
            MapWinGIS.LabelCategory options = m_shapefile.Labels.Options;
            udLabelFontSize.Value = options.FontSize;
            clpLabelFrame.Color = MapWinUtility.Colors.IntegerToColor(options.FrameBackColor);
            chkLabelFrame.Checked = options.FrameVisible;
            chkShowLabels.Checked = options.Visible;

            RefreshControlsState(null, null);

            RedrawMap();

            // refreshing preview
            DrawLabelsPreview();
            
            styleForm.Dispose();
        }

        /// <summary>
        /// Deletes all the labels
        /// </summary>
        private void btnLabelsClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to delete labels?", m_mapWin.ApplicationInfo.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                m_shapefile.Labels.Clear();
                m_shapefile.Labels.Expression = "";
                DrawLabelsPreview();
                RefreshControlsState(null, null);
                RedrawMap();
            }
        }

        /// /// <summary>
        /// Changes label font size. We shall make changes for the categories as well in case a user wants to
        /// </summary>
        private void udLabelFontSize_ValueChanged(object sender, EventArgs e)
        {
            if (_noEvents)
                return;

            m_shapefile.Labels.FontSize = (int)udLabelFontSize.Value;
            DrawLabelsPreview();
            RedrawMap();
        }

        /// <summary>
        /// Changes label frame color. We shall make changes for the categories as well in case a user wants to
        /// </summary>
        private void clpLabelFrame_SelectedColorChanged(object sender, EventArgs e)
        {
            if (_noEvents)
                return;
           
            m_shapefile.Labels.FrameBackColor = MapWinUtility.Colors.ColorToUInteger(clpLabelFrame.Color);
            DrawLabelsPreview();
            RedrawMap();
        }

        /// <summary>
        /// Changes label frame visiblity. We shall make changes for the categories as well in case a user wants to
        /// </summary>
        private void chkLabelFrame_CheckedChanged(object sender, EventArgs e)
        {
            if (_noEvents)
                return;

            m_shapefile.Labels.FrameVisible = chkLabelFrame.Checked;
            DrawLabelsPreview();
            RedrawMap();
        }

        /// <summary>
        /// Sets the properties of the labels based upon user input
        /// </summary>
        private void UpdateLabels()
        {
            MapWinGIS.Labels lb = m_shapefile.Labels;
            lb.Visible = chkShowLabels.Checked;

            // categories will have the same alignment
            for (int i = 0; i < m_shapefile.Labels.NumCategories; i++)
            {
                MapWinGIS.LabelCategory cat = m_shapefile.Labels.get_Category(i);
                cat.Alignment = lb.Alignment;
                cat.OffsetX = lb.OffsetX;
                cat.OffsetY = lb.OffsetY;
            }

            DrawLabelsPreview();
        }
    }
}
