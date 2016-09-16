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
        /// Initializes the state of dynamic visibility controls
        /// </summary>
        private void InitVisibilityTab()
        {
            scaleLayer.Locked = true;

            MapWindow.Interfaces.Layer layer = m_layer;
            scaleLayer.MaximumScale = layer.MaxVisibleScale;
            scaleLayer.MinimimScale = layer.MinVisibleScale;
            scaleLayer.UseDynamicVisibility = layer.UseDynamicVisibility;

            AxMapWinGIS.AxMap map = (AxMapWinGIS.AxMap)m_mapWin.GetOCX;
            scaleLayer.CurrentScale = map.CurrentScale;
            
            ShpfileType type = SymbologyPlugin.ShapefileType2D(m_shapefile.ShapefileType);
            uint color = (type == ShpfileType.SHP_POLYLINE)? m_shapefile.DefaultDrawingOptions.LineColor : m_shapefile.DefaultDrawingOptions.FillColor;
            scaleLayer.FillColor = MapWinUtility.Colors.IntegerToColor(color);

            scaleLayer.StateChanged += new mwSymbology.Controls.StateChanged(scaleLayer_StateChanged);

            scaleLayer.Locked = false;
        }

        /// <summary>
        /// Handles the changes in the dynamic visibility state of the layer
        /// </summary>
        private void scaleLayer_StateChanged()
        {
            if (_noEvents)
                return;

            MapWindow.Interfaces.Layer lyr = m_layer;
            lyr.MaxVisibleScale = scaleLayer.MaximumScale;
            lyr.MinVisibleScale = scaleLayer.MinimimScale;
            lyr.UseDynamicVisibility = scaleLayer.UseDynamicVisibility;
            RedrawMap();
            Application.DoEvents();
        }
    }
}
