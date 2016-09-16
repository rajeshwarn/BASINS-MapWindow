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
    using System.Text;
    using System.Windows.Forms;
    using MapWinGIS;
    using mwSymbology.Classes;
    using System.Drawing.Drawing2D;
    using System.Drawing;
    using MapWindow.Controls.Projections;

    partial class frmSymbologyMain
    {
        /// <summary>
        /// Sets the state of controls on the general tab on loading
        /// </summary>
        private void InitAppearanceTab()
        {
            // default options
            MapWinGIS.ShapeDrawingOptions options = m_shapefile.DefaultDrawingOptions;

            groupPoint.Top = groupFill.Top;
            groupPoint.Left = groupFill.Left;

            groupLine.Top = groupFill.Top;
            groupLine.Left = groupFill.Left;

            groupFill.Visible = false;
            groupLine.Visible = false;
            groupPoint.Visible = false;

            icbFillStyle.ComboStyle = ImageComboStyle.HatchStyleWithNone;
            icbLineWidth.ComboStyle = ImageComboStyle.LineWidth;

            ShpfileType type = SymbologyPlugin.ShapefileType2D(m_shapefile.ShapefileType);
            if (type == ShpfileType.SHP_POINT || type == ShpfileType.SHP_MULTIPOINT)
            {
                groupPoint.Visible = true;
                clpPointFill.Color = MapWinUtility.Colors.IntegerToColor(options.FillColor);
            }
            else if ( type == ShpfileType.SHP_POLYLINE )
            {
                groupLine.Visible = true;
            }
            else if (type == ShpfileType.SHP_POLYGON )
            {
                groupFill.Visible = true;
                clpPolygonFill.Color = MapWinUtility.Colors.IntegerToColor(options.FillColor);
            }

            Appearance2Controls();
        }

        /// <summary>
        /// Updating controls
        /// </summary>
        private void Appearance2Controls()
        {
            ShapeDrawingOptions options = m_shapefile.DefaultDrawingOptions;
            clpSelection.Color = MapWinUtility.Colors.IntegerToColor(m_shapefile.SelectionColor);
            transpSelection.Value = m_shapefile.SelectionTransparency;

            ShpfileType type = SymbologyPlugin.ShapefileType2D(m_shapefile.ShapefileType);
            if (type == ShpfileType.SHP_POINT || type == ShpfileType.SHP_MULTIPOINT)
            {
                transpMain.Value  = (byte)m_shapefile.DefaultDrawingOptions.FillTransparency;
                clpPointFill.Color = MapWinUtility.Colors.IntegerToColor(options.FillColor);
                udDefaultSize.SetValue(options.PointSize);

            }
            else if (type == ShpfileType.SHP_POLYLINE)
            {
                transpMain.Value = (byte)m_shapefile.DefaultDrawingOptions.LineTransparency;
                icbLineWidth.SelectedIndex = (int)options.LineWidth - 1;
                clpDefaultOutline.Color = MapWinUtility.Colors.IntegerToColor(options.LineColor);
            }
            else if (type == ShpfileType.SHP_POLYGON)
            {
                clpPolygonFill.Color = MapWinUtility.Colors.IntegerToColor(m_shapefile.DefaultDrawingOptions.FillColor);
                icbFillStyle.SelectedIndex = options.FillType == tkFillType.ftHatch ? (int)options.FillHatchStyle : 0;
            }
        }

        /// <summary>
        /// Opens default drawing options
        /// </summary>
        private void btnDrawingOptions_Click(object sender, EventArgs e)
        {
            Form form = m_plugin.GetSymbologyForm(m_layerHandle, m_shapefile.ShapefileType, m_shapefile.DefaultDrawingOptions, true);
            form.Text = "Default drawing options";
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                //updating controls
                this.Appearance2Controls();
                this.DrawAppearancePreview();
                Application.DoEvents();
                this.RedrawMap();
                this.RefreshControlsState(null, null);
            }
            else
            {
                Application.DoEvents();
            }
        }

        /// <summary>
        /// Draws preview on the appearance tab
        /// </summary>
        private void DrawAppearancePreview()
        {
            ShpfileType shapeType = m_shapefile.ShapefileType;
            for (int i = 0; i < 2; i++)
            {
                ShapeDrawingOptions sdo = new ShapeDrawingOptions();
                PictureBox pct = new PictureBox();
                
                pct = pictureBox1;
                sdo = m_shapefile.DefaultDrawingOptions;

                if (pct.Image != null) pct.Image.Dispose();

                Rectangle rect = pct.ClientRectangle;
                Bitmap bmp = new Bitmap(rect.Width, rect.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(bmp);
                IntPtr ptr = g.GetHdc();

                if (shapeType == ShpfileType.SHP_POINT || shapeType == ShpfileType.SHP_POINTM || shapeType == ShpfileType.SHP_POINTZ ||
                    shapeType == ShpfileType.SHP_MULTIPOINT || shapeType == ShpfileType.SHP_MULTIPOINTM || shapeType == ShpfileType.SHP_MULTIPOINTZ)
                {
                    sdo.DrawPoint(ptr, 0.0f, 0.0f, rect.Width, rect.Height, MapWinUtility.Colors.ColorToUInteger(Color.White));
                }
                else if (shapeType == ShpfileType.SHP_POLYLINE || shapeType == ShpfileType.SHP_POLYLINEZ || shapeType == ShpfileType.SHP_POLYLINEM)
                {
                    if (sdo.UseLinePattern)
                    {
                        sdo.DrawLine(ptr, 20.0f, 0.0f, 0, 0, true, rect.Width - 40, rect.Height, MapWinUtility.Colors.ColorToUInteger(Color.White));
                    }
                    else
                    {
                        int w = rect.Width - 40;
                        int h = rect.Height - 40;
                        sdo.DrawLine(ptr, (rect.Width - w)/2, (rect.Height - h) / 2, w, h, true, rect.Width, rect.Height, MapWinUtility.Colors.ColorToUInteger(Color.White));
                    }
                }
                else if (shapeType == ShpfileType.SHP_POLYGON || shapeType == ShpfileType.SHP_POLYGONZ || shapeType == ShpfileType.SHP_POLYGONM)
                {
                    sdo.DrawRectangle(ptr, rect.Width / 2 - 40, rect.Height / 2 - 40, 80, 80, true, rect.Width, rect.Height, MapWinUtility.Colors.ColorToUInteger(Color.White));
                }

                g.ReleaseHdc(ptr);
                pct.Image = bmp;
            }
        }

        /// <summary>
        /// Displays form with color schemes
        /// </summary>
        private void btnColorSchemes_Click(object sender, EventArgs e)
        {
            frmColorSchemes form = new frmColorSchemes(ref m_plugin.LayerColors);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                int index = icbChartColorScheme.SelectedIndex;

                icbChartColorScheme.ColorSchemes = m_plugin.LayerColors;
                icbChartColorScheme.ComboStyle = ImageComboStyle.ColorSchemeGraduated;

                m_plugin.LayerColors.Serialize2Xml();

                if (index < icbChartColorScheme.Items.Count)
                {
                    icbChartColorScheme.SelectedIndex = index;
                }
                else if (icbChartColorScheme.Items.Count > 0)
                {
                    icbChartColorScheme.SelectedIndex = 0;
                }
            }
            form.Dispose();
        }

        /// <summary>
        /// Sets the properties of the labels based upon user input
        /// </summary>
        private void Controls2Appearance()
        {
            // default options
            MapWinGIS.ShapeDrawingOptions options = m_shapefile.DefaultDrawingOptions;
            
            ShpfileType type = SymbologyPlugin.ShapefileType2D(m_shapefile.ShapefileType);
            if (type == ShpfileType.SHP_POLYGON)
            {
                options.FillColor = MapWinUtility.Colors.ColorToUInteger(clpPolygonFill.Color);
                // hatch style is set in the corresponding event
            }
            else if (type == ShpfileType.SHP_POINT || type == ShpfileType.SHP_MULTIPOINT)
            {
                options.FillColor = MapWinUtility.Colors.ColorToUInteger(clpPointFill.Color);
                options.PointSize = (float)udDefaultSize.Value;
            }
            else if (type == ShpfileType.SHP_POLYLINE)
            {
                options.LineColor = MapWinUtility.Colors.ColorToUInteger(clpDefaultOutline.Color);
                options.LineWidth = (float)icbLineWidth.SelectedIndex + 1;

                // and pattern ones in case there is a single line pattern
                if (options.UseLinePattern)
                {
                    if (options.LinePattern.Count == 1)
                    {
                        LineSegment line = options.LinePattern.get_Line(0);
                        line.Color = options.LineColor;
                        if (line.LineType == tkLineType.lltSimple)
                        {
                            line.LineWidth = options.LineWidth;
                        }
                    }
                }
            }

            m_shapefile.SelectionColor = MapWinUtility.Colors.ColorToUInteger(clpSelection.Color);
            m_shapefile.SelectionTransparency = transpSelection.Value;
            
            DrawAppearancePreview();
        }

        /// <summary>
        /// Handles the change of transparency by user
        /// </summary>
        private void transpMain_ValueChanged(object sender, byte value)
        {
            ShpfileType type = SymbologyPlugin.ShapefileType2D(m_shapefile.ShapefileType);
            if (type == ShpfileType.SHP_POINT || type == ShpfileType.SHP_MULTIPOINT)
            {
                m_shapefile.DefaultDrawingOptions.FillTransparency = value;
                m_shapefile.DefaultDrawingOptions.LineTransparency = value;
            }
            else if (type == ShpfileType.SHP_POLYLINE)
            {
                m_shapefile.DefaultDrawingOptions.LineTransparency = value;
            }
            else if (type == ShpfileType.SHP_POLYGON)
            {
                m_shapefile.DefaultDrawingOptions.FillTransparency = value;
                m_shapefile.DefaultDrawingOptions.LineTransparency = value;
            }
            DrawAppearancePreview();
            RedrawMap();
        }

        /// <summary>
        /// Handles the changes of the selection transparency by user
        /// </summary>
        private void transpSelection_ValueChanged(object sender, byte value)
        {
            m_shapefile.SelectionTransparency = value;
            DrawAppearancePreview();
            RedrawMap();
        }

        /// <summary>
        /// Handles the changes of the fill type by user
        /// </summary>
        private void icbFillStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_noEvents)
                return;

            ShapeDrawingOptions options = m_shapefile.DefaultDrawingOptions;
            if (icbFillStyle.SelectedIndex == 0 && options.FillType == tkFillType.ftHatch)
            {
                options.FillType = tkFillType.ftStandard;
            }
            if (icbFillStyle.SelectedIndex > 0)
            {
                options.FillType = tkFillType.ftHatch;
                options.FillHatchStyle = (tkGDIPlusHatchStyle)icbFillStyle.SelectedIndex - 1;
            }
            DrawAppearancePreview();
            RedrawMap();
        }

        /// <summary>
        /// Handles the change of selection color
        /// </summary>
        private void clpSelection_SelectedColorChanged(object sender, EventArgs e)
        {
            m_shapefile.SelectionColor = MapWinUtility.Colors.ColorToUInteger(clpSelection.Color);
            transpSelection.BandColor = clpSelection.Color;
            DrawAppearancePreview();
            RedrawMap();
        }

        /// <summary>
        /// Shows properties of the projection
        /// </summary>
        private void btnProjection_Click(object sender, EventArgs e)
        {
            ProjectionDatabase db = m_mapWin.ProjectionDatabase as ProjectionDatabase;
            if (db != null)
            {
                CoordinateSystem cs = db.GetCoordinateSystem(m_mapWin.Project.GeoProjection, ProjectionSearchType.UseDialects);
                if ((cs != null))
                {
                    frmProjectionProperties form = new frmProjectionProperties(cs, db);
                    form.ShowDialog();
                    form.Dispose();
                }
                else
                {
                    MapWinGIS.GeoProjection proj = m_mapWin.Project.GeoProjection;
                    if (!proj.IsEmpty)
                    {
                        frmProjectionProperties form = new frmProjectionProperties(proj);
                        form.ShowDialog();
                        form.Dispose();
                    }
                    else
                    {
                        SymbologyPlugin.MessageBoxInformation("There is no projection to show information about");
                    }
                }
            }

        }
    }
}
