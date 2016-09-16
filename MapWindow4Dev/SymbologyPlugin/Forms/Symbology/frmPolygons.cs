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
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using MapWinGIS;
    using mwSymbology.Classes;
    
    public partial class frmPolygons : Form
    {
        // member variables
        private ShapeDrawingOptions _options = null;
        private bool _noEvents = false;
        private LegendControl.Layer _layer = null;
        private static int _tabPage = 0;
        private string _initState = "";
        private MapWindow.Interfaces.IMapWin _mapWin = null;

        #region Initialization
        /// <summary>
        /// Creates a new instance of frmPolygons class
        /// </summary>
        public frmPolygons(ShapeDrawingOptions options, LegendControl.Layer layer, MapWindow.Interfaces.IMapWin mapWin, bool applyDisabled)
        {
            InitializeComponent();

            if (options == null || mapWin == null)
            {
                throw new Exception("frmPolygons: unexpected null parameter");
            }
            else
            {
                _options = options;
                _layer = layer;
            }

            btnApply.Visible = !applyDisabled;

            _initState = options.Serialize();
            _mapWin = mapWin;

            _noEvents = true;
            groupPicture.Parent = tabPage2;
            groupPicture.Top = groupGradient.Top;
            groupPicture.Left = groupGradient.Left;

            groupHatch.Parent = tabPage2;
            groupHatch.Top = groupGradient.Top;
            groupHatch.Left = groupGradient.Left;

            cboFillType.Items.Clear();
            cboFillType.Items.Add("Simple");
            cboFillType.Items.Add("Hatch");
            cboFillType.Items.Add("Gradient");
            cboFillType.Items.Add("Texture");

            cboGradientType.Items.Clear();
            cboGradientType.Items.Add("Linear");
            cboGradientType.Items.Add("Rectangular");
            cboGradientType.Items.Add("Circle");

            cboGradientBounds.Items.Clear();
            cboGradientBounds.Items.Add("Whole layer");
            cboGradientBounds.Items.Add("Single shapes");

            cboVerticesType.Items.Clear();
            cboVerticesType.Items.Add("Square");
            cboVerticesType.Items.Add("Circle");
            
            icbHatchStyle.ComboStyle = ImageComboStyle.HatchStyle;
            icbLineType.ComboStyle = ImageComboStyle.LineStyle;
            icbLineWidth.ComboStyle = ImageComboStyle.LineWidth;

            // loading icons
            string path = SymbologyPlugin.GetTexturesPath();
            if (System.IO.Directory.Exists(path))
            {
                iconControl1.FilePath = path;
                iconControl1.Textures = true;

                SymbologySettings settings = SymbologyPlugin.get_LayerSettings(layer.Handle, mapWin);
                iconControl1.SelectedIndex = settings.IconIndex;
            }
            else
            {
                udScaleX.Enabled = false;
                udScaleY.Enabled = false;
            }

            _noEvents = false;

            cboFillType.SelectedIndexChanged += new EventHandler(cboFillType_SelectedIndexChanged);

            Options2GUI();

            // -----------------------------------------------------
            // adding event handlers
            // -----------------------------------------------------
            // fill
            chkFillVisible.CheckedChanged += new EventHandler(GUI2Options);
            clpFill.SelectedColorChanged += new EventHandler(GUI2Options);

            // hatch
            icbHatchStyle.SelectedIndexChanged += new EventHandler(GUI2Options);
            chkFillBgTransparent.CheckedChanged += new EventHandler(GUI2Options);
            clpHatchBack.SelectedColorChanged += new EventHandler(GUI2Options);

            // gradient
            clpGradient2.SelectedColorChanged += new EventHandler(GUI2Options);
            udGradientRotation.ValueChanged += new EventHandler(GUI2Options);
            cboGradientType.SelectedIndexChanged += new EventHandler(GUI2Options);
            cboGradientBounds.SelectedIndexChanged += new EventHandler(GUI2Options);

            // outline
            chkOutlineVisible.CheckedChanged += new EventHandler(GUI2Options);
            icbLineType.SelectedIndexChanged += new EventHandler(GUI2Options);
            icbLineWidth.SelectedIndexChanged += new EventHandler(GUI2Options);
            clpOutline.SelectedColorChanged += new EventHandler(GUI2Options);

            // vertices
            chkVerticesVisible.CheckedChanged += new EventHandler(GUI2Options);
            cboVerticesType.SelectedIndexChanged += new EventHandler(GUI2Options);
            clpVerticesColor.SelectedColorChanged += new EventHandler(GUI2Options);
            chkVerticesFillVisible.CheckedChanged += new EventHandler(GUI2Options);
            udVerticesSize.ValueChanged += new EventHandler(GUI2Options);

            udScaleX.ValueChanged += new EventHandler(GUI2Options);
            udScaleY.ValueChanged += new EventHandler(GUI2Options);

            iconControl1.SelectionChanged += new mwSymbology.Controls.ListControl.SelectionChangedDel(iconControl1_SelectionChanged);

            DrawPreview();

            tabControl1.SelectedIndex = _tabPage;
        }
        #endregion

        /// <summary>
        /// Changes the textures
        /// </summary>
        void iconControl1_SelectionChanged()
        {
            string filename = iconControl1.SelectedName;
            if (filename == string.Empty)
            {
                return;
            }

            // seeking the transarent color
            Bitmap bmp = new Bitmap(filename);
            Color clrTransparent = Color.White;
            for (int i = 0; i < bmp.Width; i++)
            {
                int j;
                for (j = 0; j < bmp.Height; j++)
                {
                    Color clr = bmp.GetPixel(i, j);
                    if (clr.A == 0)
                    {
                        clrTransparent = clr;
                        break;
                    }
                }
                if (j != bmp.Width)
                {
                    break;
                }
            }

            MapWinGIS.Image img = new MapWinGIS.Image();
            if (img.Open(filename, ImageType.USE_FILE_EXTENSION, true, null))
            {
                img.LoadBuffer(50);

                img.TransparencyColor = MapWinUtility.Colors.ColorToUInteger(clrTransparent);
                img.TransparencyColor2 = MapWinUtility.Colors.ColorToUInteger(clrTransparent);
                img.UseTransparencyColor = true;

                _options.Picture = img;

                DrawPreview();
            }
            else
            {
                string errString = string.Empty;
                if (img.LastErrorCode != 0)
                {
                    errString = ": " + img.get_ErrorMsg(img.LastErrorCode);
                }
                SymbologyPlugin.MessageBoxError("Failed to open image: " + errString);
            }
        }

        #region OptionsExchange
        /// <summary>
        /// Sets the values entered by user to the class
        /// </summary>
        private void GUI2Options(object sender, EventArgs e)
        {
            if (_noEvents)
            {
                return;
            }

            // fill
            _options.FillVisible = chkFillVisible.Checked;
            _options.FillType = (tkFillType)cboFillType.SelectedIndex;
            _options.FillColor = MapWinUtility.Colors.ColorToUInteger(clpFill.Color);

            // hatch
            _options.FillHatchStyle = (tkGDIPlusHatchStyle)icbHatchStyle.SelectedIndex;
            _options.FillBgTransparent = chkFillBgTransparent.Checked;
            _options.FillBgColor = MapWinUtility.Colors.ColorToUInteger(clpHatchBack.Color);

            // gradient
            _options.FillGradientType = (tkGradientType)cboGradientType.SelectedIndex;
            _options.FillColor2 = MapWinUtility.Colors.ColorToUInteger(clpGradient2.Color);
            _options.FillRotation = (double)udGradientRotation.Value;
            _options.FillGradientBounds = (tkGradientBounds)cboGradientBounds.SelectedIndex;

            // texture
            _options.PictureScaleX = (double)udScaleX.Value;
            _options.PictureScaleY = (double)udScaleY.Value;

            // outline
            _options.LineStipple = (tkDashStyle)icbLineType.SelectedIndex;
            _options.LineWidth = (float)icbLineWidth.SelectedIndex + 1;
            _options.LineVisible = chkOutlineVisible.Checked;
            _options.LineColor = MapWinUtility.Colors.ColorToUInteger(clpOutline.Color);

            // vertices
            _options.VerticesVisible = chkVerticesVisible.Checked;
            _options.VerticesFillVisible = chkVerticesFillVisible.Checked;
            _options.VerticesSize = (int)udVerticesSize.Value;
            _options.VerticesColor = MapWinUtility.Colors.ColorToUInteger(clpVerticesColor.Color);
            _options.VerticesType = (tkVertexType)cboVerticesType.SelectedIndex;

            // transparency
            _options.LineTransparency = (float)transpOutline.Value;
            _options.FillTransparency = (float)transpFill.Value;

            //int val;
            //if (Int32.TryParse(cboLineTransparency.Text, out val))
            //{
            //    _options.LineTransparency = (int)((double)(val) * 2.55 + 0.5);
            //}
            //if (Int32.TryParse(cboFillTransparency.Text, out val))
            //{
            //    _options.FillTransparency = (int)((double)(val) * 2.55 + 0.5);
            //}

            btnApply.Enabled = true;

            DrawPreview();
        }

        /// <summary>
        /// Loads the values of the class instance to the controls
        /// </summary>
        private void Options2GUI()
        {
            _noEvents = true;

            // options
            icbLineType.SelectedIndex = (int)_options.LineStipple;
            icbLineWidth.SelectedIndex = (int)_options.LineWidth - 1;
            cboFillType.SelectedIndex = (int)_options.FillType;
            chkOutlineVisible.Checked = _options.LineVisible;
            clpOutline.Color = MapWinUtility.Colors.IntegerToColor(_options.LineColor);
            chkFillVisible.Checked = _options.FillVisible;

            // hatch
            icbHatchStyle.SelectedIndex = (int)_options.FillHatchStyle;
            chkFillBgTransparent.Checked = _options.FillBgTransparent;
            clpHatchBack.Color = MapWinUtility.Colors.IntegerToColor(_options.FillBgColor);

            // gradient
            cboGradientType.SelectedIndex = (int)_options.FillGradientType;
            clpGradient2.Color = MapWinUtility.Colors.IntegerToColor(_options.FillColor2);
            udGradientRotation.Value = (decimal)_options.FillRotation;

            clpFill.Color = MapWinUtility.Colors.IntegerToColor(_options.FillColor);
            cboGradientBounds.SelectedIndex = (int)_options.FillGradientBounds;
            chkOutlineVisible.Checked = _options.LineVisible;

            // texture
            udScaleX.SetValue(_options.PictureScaleX);
            udScaleY.SetValue(_options.PictureScaleY);

            // vertices
            chkVerticesVisible.Checked = _options.VerticesVisible;
            chkVerticesFillVisible.Checked = _options.VerticesFillVisible;
            udVerticesSize.SetValue(_options.VerticesSize);
            clpVerticesColor.Color = MapWinUtility.Colors.IntegerToColor(_options.VerticesColor);
            cboVerticesType.SelectedIndex = (int)_options.VerticesType;

            // transparency
            //cboFillTransparency.Text = ((int)((double)(_options.FillTransparency) / 2.55 + 0.5)).ToString();
            //cboLineTransparency.Text = ((int)((double)(_options.LineTransparency) / 2.55 + 0.5)).ToString();
            transpFill.Value = (byte)_options.FillTransparency;
            transpOutline.Value = (byte)_options.LineTransparency;

            _noEvents = false;
        }
        #endregion

        #region ChangingFill
        /// <summary>
        /// Changes available fill options
        /// </summary>
        void cboFillType_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupHatch.Visible = false;
            groupPicture.Visible = false;
            groupGradient.Visible = false;
            pnlFillPicture.Visible = false;
            clpFill.Visible = true;
            label6.Visible = true;
            
            if (cboFillType.SelectedIndex == (int)tkFillType.ftHatch)
            {
                groupHatch.Visible = true;
            }
            else if (cboFillType.SelectedIndex == (int)tkFillType.ftGradient)
            {
                groupGradient.Visible = true;
            }
            else if (cboFillType.SelectedIndex == (int)tkFillType.ftPicture)
            {
                groupPicture.Visible = true;
                pnlFillPicture.Visible = true;
                clpFill.Visible = false;
                label6.Visible = false;
            }
            
            if (cboFillType.SelectedIndex >= 0)
            {
                _options.FillType = (tkFillType)cboFillType.SelectedIndex;
            }

            if (!_noEvents)
                btnApply.Enabled = true;
            DrawPreview();
        }
        #endregion

        #region Drawing
        /// <summary>
        /// Draws preview based on the chosen options
        /// </summary>
        private void DrawPreview()
        {
            if (_noEvents)
            {
                return;
            }

            if (pctPreview.Image != null)
            {
                pctPreview.Image.Dispose();
            }

            Rectangle rect = pctPreview.ClientRectangle;
            Bitmap bmp = new Bitmap(rect.Width, rect.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            IntPtr ptr = g.GetHdc();

            // creating shape to draw
            _options.DrawRectangle(ptr, 40.0f, 40.0f, rect.Width - 80, rect.Height - 80, true, rect.Width, rect.Height, MapWinUtility.Colors.ColorToUInteger(this.BackColor));

            g.ReleaseHdc();
            pctPreview.Image = bmp;
        }
        #endregion

        /// <summary>
        /// Saves the window state
        /// </summary>
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (_options.Serialize() != _initState)
            {
                _mapWin.Project.Modified = true;
            }

            SymbologySettings settings = SymbologyPlugin.get_LayerSettings(_layer.Handle, _mapWin);
            settings.IconIndex = iconControl1.SelectedIndex;
            SymbologyPlugin.SaveLayerSettings(_layer.Handle, settings, _mapWin);
            _tabPage = tabControl1.SelectedIndex;

            // saves options for default loading behavior
            SymbologyPlugin.SaveLayerOptions(_mapWin, _layer.Handle);
        }

        /// <summary>
        /// Handles the changes of the transparency by user
        /// </summary>
        /// <param name="value"></param>
        private void transpOutline_ValueChanged(object sender, byte value)
        {
            GUI2Options(sender, null);
        }

        /// <summary>
        /// Applies the changes and updates the map
        /// </summary>
        private void btnApply_Click(object sender, EventArgs e)
        {
            _mapWin.View.ForceFullRedraw();
            _mapWin.Project.Modified = true;
            _initState = _options.Serialize();
            btnApply.Enabled = false;
        }

        /// <summary>
        /// Reverts the changes if cancel was hit
        /// </summary>
        private void frmPolygons_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel)
            {
                _tabPage = tabControl1.SelectedIndex;
                _options.Deserialize(_initState);
            }
        }

        /// <summary>
        /// Allows to apply newly selected texture
        /// </summary>
        private void iconControl1_SelectionChanged_1()
        {
            if (_noEvents)
                return;
            btnApply.Enabled = true;
        }
    }
}
