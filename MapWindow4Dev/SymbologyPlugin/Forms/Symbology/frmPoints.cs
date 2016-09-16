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
    using System.IO;
    using mwSymbology.Classes;

    public partial class frmPoints : Form
    {
        // the drawing options being edited
        ShapeDrawingOptions _options = null;
        
        // the tab that was used on the last call
        static int _tabIndex = 0;

        // supresses events while loading form
        bool _noEvents = false;

        // the layer that holdes point shapefile
        LegendControl.Layer _layer = null;

        // serialized state of the options
        string _initState = "";

        // reference to MapWindow application
        MapWindow.Interfaces.IMapWin _mapWin = null;

        #region Initialization
        /// <summary>
        /// Creates a new instance of frmPoints class
        /// </summary>
        public frmPoints(ShapeDrawingOptions options, LegendControl.Layer layer, MapWindow.Interfaces.IMapWin mapWin, bool ApplyDisabled)
        {
            InitializeComponent();
            if (options == null)
            {
                throw new Exception("frmPoints: Unexpected null parameter");
            }

            btnApply.Visible = !ApplyDisabled;

            // setting values to the controls
            _options = options;
            _layer = layer;
            _initState = _options.Serialize();
            _mapWin = mapWin;
            _noEvents = true;

            clpFillColor.SelectedColorChanged += new EventHandler(clpFillColor_SelectedColorChanged);
            cboIconCollection.SelectedIndexChanged += new EventHandler(cboIconCollection_SelectedIndexChanged);
            cboFillType.SelectedIndexChanged += new EventHandler(cboFillType_SelectedIndexChanged);

            icbPointShape.ComboStyle = ImageComboStyle.PointShape;
            icbLineType.ComboStyle = ImageComboStyle.LineStyle;
            icbLineWidth.ComboStyle = ImageComboStyle.LineWidth;
            icbHatchStyle.ComboStyle = ImageComboStyle.HatchStyle;

            pnlFillPicture.Parent = groupBox3;    // options
            pnlFillPicture.Top = pnlFillHatch.Top;
            pnlFillPicture.Left = pnlFillHatch.Left;

            pnlFillGradient.Parent = groupBox3;    // options
            pnlFillGradient.Top = pnlFillHatch.Top;
            pnlFillGradient.Left = pnlFillHatch.Left;

            cboFillType.Items.Clear();
            cboFillType.Items.Add("Simple");
            cboFillType.Items.Add("Hatch");
            cboFillType.Items.Add("Gradient");

            cboGradientType.Items.Clear();
            cboGradientType.Items.Add("Linear");
            cboGradientType.Items.Add("Rectangular");
            cboGradientType.Items.Add("Circle");

            // character control
            cboFontName.SelectedIndexChanged += new EventHandler(cboFontName_SelectedIndexChanged);
            RefreshFontList(null, null);
            characterControl1.SelectedCharacterCode = (byte)_options.PointCharacter;

            // icon control
            RefreshIconCombo();
            if (layer != null)
            {
                SymbologySettings settings = SymbologyPlugin.get_LayerSettings(_layer.Handle, mapWin);
                
                iconControl1.SelectedIndex = settings.IconIndex;
                chkScaleIcons.Checked = settings.ScaleIcons;
                string name = settings.IconCollection.ToLower();
                for (int i = 0; i < cboIconCollection.Items.Count; i++)
                {
                    if (cboIconCollection.Items[i].ToString().ToLower() == name)
                    {
                        cboIconCollection.SelectedIndex = i;
                        break;
                    }
                }
            }

            Options2GUI();
            _noEvents = false;

            // -----------------------------------------------------
            // adding event handlers
            // -----------------------------------------------------
            udRotation.ValueChanged += new EventHandler(GUI2Options);
            udPointNumSides.ValueChanged += new EventHandler(GUI2Options);
            udSideRatio.ValueChanged += new EventHandler(GUI2Options);
            udSize.ValueChanged += new EventHandler(GUI2Options);
            chkShowAllFonts.CheckedChanged += new EventHandler(RefreshFontList);

            // line
            chkOutlineVisible.CheckedChanged += new EventHandler(GUI2Options);
            icbLineType.SelectedIndexChanged += new EventHandler(GUI2Options);
            icbLineWidth.SelectedIndexChanged += new EventHandler(GUI2Options);
            clpOutline.SelectedColorChanged += new EventHandler(clpOutline_SelectedColorChanged);
            
            chkFillVisible.CheckedChanged += new EventHandler(GUI2Options);
            
            iconControl1.SelectionChanged += new mwSymbology.Controls.ListControl.SelectionChangedDel(iconControl1_SelectionChanged);
            chkScaleIcons.CheckedChanged += new EventHandler(GUI2Options);

            // character
            characterControl1.SelectionChanged += new mwSymbology.Controls.ListControl.SelectionChangedDel(characterControl1_SelectionChanged);
            symbolControl1.SelectionChanged += new mwSymbology.Controls.ListControl.SelectionChangedDel(symbolControl1_SelectionChanged);

            // hatch
            icbHatchStyle.SelectedIndexChanged += new EventHandler(GUI2Options);
            chkFillBgTransparent.CheckedChanged += new EventHandler(GUI2Options);
            clpHatchBack.SelectedColorChanged += new EventHandler(GUI2Options);

            // gradient
            clpGradient2.SelectedColorChanged += new EventHandler(GUI2Options);
            udGradientRotation.ValueChanged += new EventHandler(GUI2Options);
            cboGradientType.SelectedIndexChanged += new EventHandler(GUI2Options);

            DrawPreview();

            tabControl1.SelectedIndex = _tabIndex;
        }
        #endregion

        #region ChangingFill
        /// <summary>
        /// Toggles fill type oprions
        /// </summary>
        void cboFillType_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlFillGradient.Visible = false;
            pnlFillHatch.Visible = false;
            pnlFillPicture.Visible = false;
            lblNoOptions.Visible = false;

            if (cboFillType.SelectedIndex == (int)tkFillType.ftHatch)
            {
                pnlFillHatch.Visible = true;
                _options.FillType = tkFillType.ftHatch;
            }
            else if (cboFillType.SelectedIndex == (int)tkFillType.ftGradient)
            {
                pnlFillGradient.Visible = true;
                _options.FillType = tkFillType.ftGradient;
            }
            else if (cboFillType.SelectedIndex == (int)tkFillType.ftPicture)
            {
                pnlFillPicture.Visible = true;
                _options.FillType = tkFillType.ftPicture;
            }
            else
            {
                lblNoOptions.Visible = true;
                _options.FillType = tkFillType.ftStandard;
            }

            if (!_noEvents)
                btnApply.Enabled = true;
            DrawPreview();
        }
        #endregion

        #region Icons
        /// <summary>
        /// Fills the image combo with the names of icons collectins (folders) 
        /// </summary>
        private void RefreshIconCombo()
        {
            cboIconCollection.Items.Clear();
            
            string path = SymbologyPlugin.GetIconsPath();
            if (!System.IO.Directory.Exists(path))
            {
                cboIconCollection.Enabled = false;
                chkScaleIcons.Enabled = false;
                return;
            }

            string[] directories = Directory.GetDirectories(path);
            if (directories.Length <= 0)
            {
                // TODO: report error 
            }
            else
            {
                for (int i = 0; i < directories.Length; i++)
                {
                    string[] files = Directory.GetFiles(directories[i]);
                    if (files.Length > 0)
                    {
                        for (int j = 0; j < files.Length; j++)
                        {
                            string ext = Path.GetExtension(files[j]).ToLower();
                            if (ext == ".bmp" || ext == ".png")
                            {
                                cboIconCollection.Items.Add(directories[i].Substring(path.Length));
                                break;
                            }
                        }
                    }
                }
                if (cboIconCollection.Items.Count <= 0)
                {
                    cboIconCollection.Enabled = false;
                }
                else
                {
                    cboIconCollection.SelectedIndex = 0;
                }
            }           
        }

        /// <summary>
        /// Updates the preview with newly selected icon
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

                _options.PointType = tkPointSymbolType.ptSymbolPicture;
                _options.Picture = img;

                if (chkScaleIcons.Checked)
                {
                    _options.PictureScaleX = (double)udSize.Value / (double)img.Width;
                    _options.PictureScaleY = (double)udSize.Value / (double)img.Height;
                }

                DrawPreview();
            }
            else
            {
                string errString = string.Empty;
                if (img.LastErrorCode != 0)
                {
                    errString = ": " + img.get_ErrorMsg(img.LastErrorCode);
                }
                MessageBox.Show("Failed to open image: " + errString);
            }
            
            if (!_noEvents)
                btnApply.Enabled = true;
        }

        /// <summary>
        /// Building new list of icons from the changed path
        /// </summary>
        void cboIconCollection_SelectedIndexChanged(object sender, EventArgs e)
        {
            string path = SymbologyPlugin.GetIconsPath();
            path += cboIconCollection.Text;

            if (Directory.Exists(path))
            {
                iconControl1.CellWidth = 32;
                iconControl1.CellHeight = 32;

                // let's try to determine real size by first file
                try
                {
                    string[] files = Directory.GetFiles(path);
                    foreach (string name in files)
                    {
                        string ext = Path.GetExtension(name);
                        if (ext == ".bmp" || ext == ".png")
                        {
                            Bitmap bmp = new Bitmap(name);
                            if (bmp.Width <= 16 || bmp.Height <= 16)
                            {
                                // do nothing - use 32
                            }
                            else if (bmp.Width < 48 && bmp.Height < 48)
                            {
                                iconControl1.CellWidth = bmp.Height < bmp.Width ? bmp.Height + 16 : bmp.Width + 16;
                                iconControl1.CellHeight = iconControl1.CellWidth;
                            }
                            else
                            {
                                iconControl1.CellWidth = 48 + 16;
                                iconControl1.CellHeight = iconControl1.CellWidth;
                            }
                            break;
                        }
                    }
                }
                catch
                {
                    // do nothing
                }
            }
            
            
            iconControl1.FilePath = path;
            lblCopyright.Text = "";

            string filename = path + @"\copyright.txt";
            if (File.Exists(filename))
            {
                StreamReader reader = null;
                try
                {
                    reader = new StreamReader(filename);
                    lblCopyright.Text = reader.ReadLine();
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                }
            }
        }
        #endregion

        #region FontCharacters
        /// <summary>
        /// Refreshes the list of fonts
        /// </summary>
        private void RefreshFontList(object sender, EventArgs e)
        {
            cboFontName.Items.Clear();
            if (!chkShowAllFonts.Checked)
            {
                foreach (FontFamily family in FontFamily.Families)
                {
                    string name = family.Name.ToLower();

                    if (name == "webdings" ||
                        name == "wingdings" ||
                        name == "wingdings 2" ||
                        name == "wingdings 3" ||
                        name == "times new roman")
                    {
                        cboFontName.Items.Add(family.Name);
                    }
                }
            }
            else
            {
                foreach (FontFamily family in FontFamily.Families)
                {
                    cboFontName.Items.Add(family.Name);
                }
            }

            string fontName = _options.FontName.ToLower();
            for (int i = 0; i < cboFontName.Items.Count; i++)
            {
                if (cboFontName.Items[i].ToString().ToLower() == fontName)
                {
                    cboFontName.SelectedIndex = i;
                    break;
                }
            }
            if (cboFontName.SelectedIndex == -1)
            {
                cboFontName.SelectedItem = "Arial";
            }
        }

        /// <summary>
        /// Changing the font in the font control
        /// </summary>
        private void cboFontName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_noEvents)
                btnApply.Enabled = true;
            characterControl1.SetFontName(cboFontName.Text);
            _options.FontName = cboFontName.Text;
            DrawPreview();
        }

        /// <summary>
        /// Updates the preview with the newly selected character
        /// </summary>
        void characterControl1_SelectionChanged()
        {
            if (!_noEvents)
                btnApply.Enabled = true;
            _options.PointType = tkPointSymbolType.ptSymbolFontCharacter;
            _options.PointCharacter = Convert.ToInt16(characterControl1.SelectedCharacterCode);
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
            _options.DrawPoint(ptr, 0.0f, 0.0f, rect.Width, rect.Height, MapWinUtility.Colors.ColorToUInteger(this.BackColor));

            g.ReleaseHdc();
            pctPreview.Image = bmp;
        }
        #endregion

        /// <summary>
        /// Changes the chosen point symbol
        /// </summary>
        private void symbolControl1_SelectionChanged()
        {
            tkDefaultPointSymbol symbol = (tkDefaultPointSymbol)symbolControl1.SelectedIndex;
            _options.SetDefaultPointSymbol(symbol);
            if (!_noEvents)
                btnApply.Enabled = true;
            
            Options2GUI();
            DrawPreview();
        }

        #region Properties Exchange
        /// <summary>
        /// Sets the values entered by user to the class
        /// </summary>
        private void GUI2Options(object sender, EventArgs e)
        {
            if (_noEvents)
            {
                return;
            }
            
            _options.PointSize = (float)udSize.Value;

            if (_options.Picture != null && chkScaleIcons.Checked)
            {
                _options.PictureScaleX = (double)udSize.Value / (double)_options.Picture.Width;
                _options.PictureScaleY = (double)udSize.Value / (double)_options.Picture.Height;
            }
            else
            {
                _options.PictureScaleX = 1.0;
                _options.PictureScaleY = 1.0;
            }

            _options.PointRotation = (double)udRotation.Value;
            _options.FillColor = MapWinUtility.Colors.ColorToUInteger(clpFillColor.Color);
            
            
            _options.PointShape = (tkPointShapeType)icbPointShape.SelectedIndex;
            _options.PointSidesCount = (int)udPointNumSides.Value;
            _options.PointSidesRatio = (float)udSideRatio.Value / 10;
        
            _options.LineStipple = (tkDashStyle)icbLineType.SelectedIndex;
            _options.LineWidth = (float)icbLineWidth.SelectedIndex + 1;
            _options.LineVisible = chkOutlineVisible.Checked;
            _options.FillVisible = chkFillVisible.Checked;
            _options.FillType = (tkFillType)cboFillType.SelectedIndex;

            // hatch
            _options.FillHatchStyle = (tkGDIPlusHatchStyle)icbHatchStyle.SelectedIndex;
            _options.FillBgTransparent = chkFillBgTransparent.Checked;
            _options.FillBgColor = MapWinUtility.Colors.ColorToUInteger(clpHatchBack.Color);

            // gradient
            _options.FillGradientType = (tkGradientType)cboGradientType.SelectedIndex;
            _options.FillColor2 = MapWinUtility.Colors.ColorToUInteger(clpGradient2.Color);
            _options.FillRotation = (double)udGradientRotation.Value;

            _options.FillTransparency = (float)transparencyControl1.Value;
            _options.LineTransparency = (float)transparencyControl1.Value;

            if (!_noEvents)
                btnApply.Enabled = true;

            DrawPreview();
        }

        /// <summary>
        /// Loads the values of the class instance to the controls
        /// </summary>
        private void Options2GUI()
        {
            _noEvents = true;
            udSize.SetValue(_options.PointSize);
            udRotation.SetValue(_options.PointRotation);
            clpFillColor.Color = MapWinUtility.Colors.IntegerToColor(_options.FillColor);

            // point
            icbPointShape.SelectedIndex = (int)_options.PointShape;
            udPointNumSides.SetValue(_options.PointSidesCount);
            udSideRatio.SetValue(_options.PointSidesRatio * 10.0);
            
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

            transparencyControl1.Value = (byte)_options.FillTransparency;

            _noEvents = false;
        }
        #endregion

        #region Colors
        /// <summary>
        /// Updates all the controls with the selected fill color
        /// </summary>
        private void clpFillColor_SelectedColorChanged(object sender, EventArgs e)
        {
            _options.FillColor = MapWinUtility.Colors.ColorToUInteger(clpFillColor.Color);
            symbolControl1.ForeColor = clpFillColor.Color;
            characterControl1.ForeColor = clpFillColor.Color;
            icbPointShape.Color1 = clpFillColor.Color;
            if (!_noEvents)
                btnApply.Enabled = true;
            DrawPreview();
        }

        /// <summary>
        ///  Updates all the control witht the selected outline color
        /// </summary>
        void clpOutline_SelectedColorChanged(object sender, EventArgs e)
        {
            _options.LineColor = MapWinUtility.Colors.ColorToUInteger(clpOutline.Color);
            
            // TODO: implement
            //symbolControl1.ForeColor = clpFillColor.Color;
            //characterControl1.ForeColor = clpFillColor.Color;
            //icbPointShape.Color1 = clpFillColor.Color;

            if (!_noEvents)
                btnApply.Enabled = true;
            DrawPreview();
        }
        #endregion

        /// <summary>
        /// Saves the selected page
        /// </summary>
        private void btnOk_Click(object sender, EventArgs e)
        {
            _tabIndex = tabControl1.SelectedIndex;

            SymbologySettings settings = SymbologyPlugin.get_LayerSettings(_layer.Handle, _mapWin);
            
            settings.IconCollection = cboIconCollection.Text;
            settings.ScaleIcons = chkScaleIcons.Checked;
            settings.IconIndex = iconControl1.SelectedIndex;
            SymbologyPlugin.SaveLayerSettings(_layer.Handle, settings, _mapWin);

            if (_options.Serialize() != _initState)
            {
                _mapWin.Project.Modified = true;
            }

            // saves options for default loading behavior
            SymbologyPlugin.SaveLayerOptions(_mapWin, _layer.Handle);
        }

        /// <summary>
        /// Changes the transparency
        /// </summary>
        private void transparencyControl1_ValueChanged(object sender, byte value)
        {
            GUI2Options(null, null);
        }

        /// <summary>
        /// Saves options and redraws map without closing the form
        /// </summary>
        private void btnApply_Click(object sender, EventArgs e)
        {
            _mapWin.View.ForceFullRedraw();
            _mapWin.Project.Modified = true;
            btnApply.Enabled = false;
            _initState = _options.Serialize();
        }

        /// <summary>
        /// Reverts changes and closes the form
        /// </summary>
        private void frmPoints_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel)
            {
                _tabIndex = tabControl1.SelectedIndex;
                _options.Deserialize(_initState);
            }
        }
    }
}
