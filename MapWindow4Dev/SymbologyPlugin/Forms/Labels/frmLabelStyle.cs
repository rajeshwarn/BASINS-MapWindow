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
    using System.Drawing.Drawing2D;
    using MapWinUtility;
    using MapWinGIS;
    using System.Drawing.Text;
    
    /// <summary>
    /// GUI for setting options for Labels and LabelCategory classes
    /// </summary>
    public partial class frmLabelStyle : Form
    {
        #region Declarations
        //reference to the MapWindow
        private MapWindow.Interfaces.IMapWin m_mapWin = null;
        
        // The reference to the labels
        private MapWinGIS.LabelCategory m_category = null;

        // The tab number to open on the loading
        private static int tabNumber = 0;
        
        // prevents event handles to process events while on loading
        private bool m_noEvents = false;
        
        // The label string to display
        private string m_labelText = "";

        // parent shapefile
        private MapWinGIS.Shapefile m_shapefile = null;
        
        // a category is being edited, no expression is available
        private bool m_categoryEdited = false;

        // serialized initial state of the class
        private string m_initState = "";

        // a handle of the layer being edited
        private int m_handle = -1;
        #endregion

        /// <summary>
        /// Constructor for setting label expression and options
        /// </summary>
        public frmLabelStyle(MapWinGIS.Shapefile sf, MapWindow.Interfaces.IMapWin mapWin, int handle)
        {
            if (sf == null || mapWin == null)
            {
                throw new Exception("frmLabelStyle: unexpected null parameter");
            }
            m_mapWin = mapWin;
            m_shapefile = sf;
            m_handle = handle;

            InitializeComponent();

            tabControl1.TabPages.Remove(tabControl1.TabPages[6]);   // rendering

            LabelStyle style = new LabelStyle( m_shapefile.Labels.Options);
            
            // old-style labels not based on expression
            if (m_shapefile.Labels.Expression == "" && m_shapefile.Labels.Count > 0 &&
                m_shapefile.Labels.get_Label(0, 0).Text != "")
            {
                richTextBox1.Text = "<No expression>";
                listBox1.Enabled = false;
                btnPlus.Enabled = false;
                btnQuotes.Enabled = false;
                btnNewLine.Enabled = false;
                richTextBox1.Enabled = false;
            }
            else
            {
                richTextBox1.Text = mwSymbology.LabelUtilities.StripNewLineQuotes(m_shapefile.Labels.Expression);
            }

            this.Initialize(style);

            labelList1.LoadFromXML();

            tabControl1.SelectedIndex = tabNumber;
        }

        /// <summary>
        /// Constructor for editing single category
        /// </summary>
        /// <param name="lb"></param>
        public frmLabelStyle(MapWinGIS.Shapefile sf, MapWinGIS.LabelCategory lb, MapWindow.Interfaces.IMapWin mapWin) 
        {
            if (mapWin == null)
            {
                throw new Exception("frmLabelStyle: unexpected null parameter");
            }

            m_mapWin = mapWin;
            m_categoryEdited = true;
            m_shapefile = sf;

            InitializeComponent();
            this.Initialize(lb);

            tabControl1.TabPages.Remove(tabControl1.TabPages[6]);   // rendering

            // expression isn't available for the categories
            if (m_categoryEdited)
            {
                tabControl1.TabPages.Remove(tabControl1.TabPages[4]);   // visibility
                tabControl1.TabPages.Remove(tabControl1.TabPages[3]);   // position
                tabControl1.TabPages.Remove(tabControl1.TabPages[0]);   // expression
            }
            lblResult.Visible = false;
            chkGDIMode.Visible = false;
            btnApply.Visible = false;

            labelList1.LoadFromXML();

            tabControl1.SelectedIndex = tabNumber;
        }

        /// <summary>
        /// Initializes controls of the form
        /// </summary>
        private void Initialize(LabelCategory lb)
        {
            m_category = lb;

            m_noEvents = true;
            cboFontName.Items.Clear();
            foreach (FontFamily family in FontFamily.Families)
            {
                cboFontName.Items.Add(family.Name);
            }

            icbLineType.ComboStyle = ImageComboStyle.LineStyle;
            icbLineWidth.ComboStyle = ImageComboStyle.LineWidth;

            icbFrameType.Color1 = Color.LightYellow;
            icbFrameType.ComboStyle = ImageComboStyle.FrameType;
            icbFrameType.SelectedIndex = 0;

            if (!m_categoryEdited)
            {
                for (int i = 0; i < m_shapefile.NumFields; i++)
                {
                    listBox1.Items.Add(m_shapefile.get_Field(i).Name);
                }
            }

            string[] scales = { "1", "10", "100", "1000", "5000", "10000", "25000", "50000", "100000", 
                                "250000", "500000", "1000000", "10000000" };
            cboMinScale.Items.Clear();
            cboMaxScale.Items.Clear();
            cboBasicScale.Items.Clear();
            for (int i = 0; i < scales.Length; i++)
            {
                cboMinScale.Items.Add(scales[i]);
                cboMaxScale.Items.Add(scales[i]);
                cboBasicScale.Items.Add(scales[i]);
            }

            // displaying options in the GUI
            LabelStyle2GUI(m_category);

            btnTest_Click(null, null);

            txtLabelExpression.Text = m_shapefile.Labels.VisibilityExpression;

            // serialization
            if (m_categoryEdited)
            {
                m_initState = m_category.Serialize();
            }
            else
            {
                tkSavingMode mode = m_shapefile.Labels.SavingMode;
                m_shapefile.Labels.SavingMode = tkSavingMode.modeNone;
                m_initState = m_shapefile.Labels.Serialize();
                m_shapefile.Labels.SavingMode = mode;
            }

            cboLabelsVerticalPosition.Items.Clear();
            cboLabelsVerticalPosition.Items.Add("Above current layer");
            cboLabelsVerticalPosition.Items.Add("Above all layers");
            
            MapWinGIS.Labels labels = m_shapefile.Labels;

            cboLabelsVerticalPosition.SelectedIndex = (int)labels.VerticalPosition;
            chkLabelsRemoveDuplicates.Checked = labels.RemoveDuplicates;
            chkAviodCollisions.Checked = labels.AvoidCollisions;
            chkScaleLabels.Checked = labels.ScaleLabels;
            cboBasicScale.Text = labels.BasicScale.ToString();

            udLabelOffsetX.SetValue(labels.OffsetX);
            udLabelOffsetY.SetValue(labels.OffsetY);
            udLabelsBuffer.SetValue(labels.CollisionBuffer);

            // alignment
            optAlignBottomCenter.Checked = (labels.Alignment == tkLabelAlignment.laBottomCenter);
            optAlignBottomLeft.Checked = (labels.Alignment == tkLabelAlignment.laBottomLeft);
            optAlignBottomRight.Checked = (labels.Alignment == tkLabelAlignment.laBottomRight);
            optAlignCenter.Checked = (labels.Alignment == tkLabelAlignment.laCenter);
            optAlignCenterLeft.Checked = (labels.Alignment == tkLabelAlignment.laCenterLeft);
            optAlignCenterRight.Checked = (labels.Alignment == tkLabelAlignment.laCenterRight);
            optAlignTopCenter.Checked = (labels.Alignment == tkLabelAlignment.laTopCenter);
            optAlignTopLeft.Checked = (labels.Alignment == tkLabelAlignment.laTopLeft);
            optAlignTopRight.Checked = (labels.Alignment == tkLabelAlignment.laTopRight);

            ShpfileType shpType = SymbologyPlugin.ShapefileType2D(m_shapefile.ShapefileType);
            optAlignCenter.Enabled = (shpType != ShpfileType.SHP_POINT && shpType != ShpfileType.SHP_MULTIPOINT);

            btnApply.Enabled = (m_shapefile.Labels.Expression != "" && m_shapefile.Labels.Count == 0);

            //Enum.GetNames(typeof(MapWinGIS.tkTextRenderingHint));
            string[] list =  new string[]{
                                            "System default",
                                            "Glyph bitmap",
                                            "Glyph bitmap w/o hinting",
                                            "AntiAliasing",
                                            "AntiAliasing w/o hinting",
                                            "Clear type"};
            cboTextRenderingHint.DataSource = list;
            this.SetSelectedIndex(cboTextRenderingHint, (int)m_shapefile.Labels.TextRenderingHint);
            cboTextRenderingHint.SelectedIndexChanged += new EventHandler(GUI2LabelStyle);
            m_noEvents = false;

            // initial drawing
            this.DrawPreview(null, null);
        }

        
        /// <summary>
        /// Sets selected index in the combo in case it's the valid one
        /// </summary>
        private void SetSelectedIndex(ComboBox combo, int index)
        {
            if (index >= 0 && index < combo.Items.Count)
                combo.SelectedIndex = index;
        }

        /// <summary>
        /// Loads label options to the GUI controls
        /// </summary>
        private bool LabelStyle2GUI(MapWinGIS.LabelCategory lb)
        {
            if (lb == null) 
                return false;

            bool noEvents = m_noEvents;
            m_noEvents = true;

            chkVisible.Checked = m_category.Visible;

            string fontName = lb.FontName;
            int j = 0;
            foreach (FontFamily family in FontFamily.Families)
            {
                if (family.Name == fontName)
                    cboFontName.SelectedIndex = j;
                j++;
            }
            if (cboFontName.SelectedIndex == -1)
            {
                cboFontName.SelectedItem = "Arial";
            }

            // font style
            chkFontBold.Checked = lb.FontBold;
            chkFontItalic.Checked = lb.FontItalic;
            chkFontUnderline.Checked = lb.FontUnderline;
            chkFontStrikeout.Checked = lb.FontStrikeOut;

            udFontSize.Value = lb.FontSize;

            clpFont1.Color = Colors.IntegerToColor(lb.FontColor);

            udFramePaddingX.SetValue(lb.FramePaddingX);
            udFramePaddingY.SetValue(lb.FramePaddingY);

            // font outlines
            chkHaloVisible.Checked = lb.HaloVisible;
            chkShadowVisible.Checked = lb.ShadowVisible;

            clpShadow.Color = Colors.IntegerToColor(lb.ShadowColor);
            clpHalo.Color = Colors.IntegerToColor(lb.HaloColor);

            udHaloSize.SetValue(lb.HaloSize);
            udShadowOffsetX.SetValue(lb.ShadowOffsetX);
            udShadowOffsetY.SetValue(lb.ShadowOffsetY);

            // frame options
            chkUseFrame.Checked = lb.FrameVisible;
            icbFrameType.SelectedIndex = (int)lb.FrameType;

            icbLineType.SelectedIndex = (int)lb.FrameOutlineStyle;

            clpFrame1.Color = Colors.IntegerToColor(lb.FrameBackColor);
            clpFrameBorder.Color = Colors.IntegerToColor(lb.FrameOutlineColor);

            udFramePaddingX.SetValue(lb.FramePaddingX);
            udFramePaddingY.SetValue(lb.FramePaddingY);

            if (lb.FrameOutlineWidth < 1) lb.FrameOutlineWidth = 1;
            if (lb.FrameOutlineWidth > icbLineWidth.Items.Count) lb.FrameOutlineWidth = icbLineWidth.Items.Count;
            icbLineWidth.SelectedIndex = (int)lb.FrameOutlineWidth - 1;

            transparencyControl1.Value = (byte)lb.FrameTransparency;

            cboMinScale.Text = m_shapefile.Labels.MinVisibleScale.ToString();
            cboMaxScale.Text = m_shapefile.Labels.MaxVisibleScale.ToString();
            chkDynamicVisibility.Checked = m_shapefile.Labels.DynamicVisibility;

            if (!m_categoryEdited)
            {
                chkGDIMode.Checked = !m_shapefile.Labels.UseGdiPlus;
            }

            m_noEvents = noEvents;

            return true;
        }

        /// <summary>
        /// Saves the options from the GUI to labels style class
        /// </summary>
        private void GUI2LabelStyle(object sender, EventArgs e)
        {
            if (m_noEvents)
            {
                return;
            }
           
            LabelCategory lb = m_category;

            lb.Visible = chkVisible.Checked;

            // alignment
            lb.FramePaddingX = (int)udFramePaddingX.Value;
            lb.FramePaddingY = (int)udFramePaddingY.Value;

            // font 
            lb.FontBold = chkFontBold.Checked;
            lb.FontItalic = chkFontItalic.Checked;
            lb.FontUnderline = chkFontUnderline.Checked;
            lb.FontStrikeOut = chkFontStrikeout.Checked;
            lb.FontName = cboFontName.Text;
            lb.FontColor = MapWinUtility.Colors.ColorToUInteger(clpFont1.Color);
            lb.FontSize = (int)udFontSize.Value;

            // outline
            lb.HaloVisible = chkHaloVisible.Checked;
            lb.ShadowVisible = chkShadowVisible.Checked;

            lb.HaloColor = MapWinUtility.Colors.ColorToUInteger(clpHalo.Color);
            lb.ShadowColor = MapWinUtility.Colors.ColorToUInteger(clpShadow.Color);

            lb.HaloSize = (int)udHaloSize.Value;
            lb.ShadowOffsetX = (int)udShadowOffsetX.Value;
            lb.ShadowOffsetY = (int)udShadowOffsetY.Value;
            
            // frame fill
            lb.FrameBackColor = MapWinUtility.Colors.ColorToUInteger(clpFrame1.Color);

            if (tabControl1.SelectedTab.Name == "tabFrameFill")
            {
                lb.FrameVisible = chkUseFrame.Checked;
                lb.FrameType = (MapWinGIS.tkLabelFrameType)icbFrameType.SelectedIndex;
            }

            // frame outline
            lb.FrameOutlineColor = MapWinUtility.Colors.ColorToUInteger(clpFrameBorder.Color);
            if (icbLineType.SelectedIndex >= 0)
            {
                lb.FrameOutlineStyle = (MapWinGIS.tkDashStyle)icbLineType.SelectedIndex;
            }
            lb.FrameOutlineWidth = (int)icbLineWidth.SelectedIndex + 1;

            lb.FrameTransparency = transparencyControl1.Value;
            lb.FontTransparency = transparencyControl1.Value;

            //if (!m_categoryEdited)
            //    m_shapefile.Labels.UseGdiPlus = !chkGDIMode.Checked;

            // passed from the main form
            m_shapefile.Labels.RemoveDuplicates = chkLabelsRemoveDuplicates.Checked;
            m_shapefile.Labels.AvoidCollisions = chkAviodCollisions.Checked;
            m_shapefile.Labels.ScaleLabels = chkScaleLabels.Checked;
            
            double val;
            m_shapefile.Labels.BasicScale = (double.TryParse(cboBasicScale.Text, out val)) ? val : 0.0;
            m_shapefile.Labels.VerticalPosition = (MapWinGIS.tkVerticalPosition)cboLabelsVerticalPosition.SelectedIndex;

            lb.OffsetX = (double)udLabelOffsetX.Value;
            lb.OffsetY = (double)udLabelOffsetY.Value;
            m_shapefile.Labels.CollisionBuffer = (int)udLabelsBuffer.Value;

            // alignment
            if (optAlignBottomCenter.Checked) lb.Alignment = tkLabelAlignment.laBottomCenter;
            if (optAlignBottomLeft.Checked) lb.Alignment = tkLabelAlignment.laBottomLeft;
            if (optAlignBottomRight.Checked) lb.Alignment = tkLabelAlignment.laBottomRight;
            if (optAlignCenter.Checked) lb.Alignment = tkLabelAlignment.laCenter;
            if (optAlignCenterLeft.Checked) lb.Alignment = tkLabelAlignment.laCenterLeft;
            if (optAlignCenterRight.Checked) lb.Alignment = tkLabelAlignment.laCenterRight;
            if (optAlignTopCenter.Checked) lb.Alignment = tkLabelAlignment.laTopCenter;
            if (optAlignTopLeft.Checked) lb.Alignment = tkLabelAlignment.laTopLeft;
            if (optAlignTopRight.Checked) lb.Alignment = tkLabelAlignment.laTopRight;

            // categories will have the same alignment
            if (!m_categoryEdited)
            {
                for (int i = 0; i < m_shapefile.Labels.NumCategories; i++)
                {
                    MapWinGIS.LabelCategory cat = m_shapefile.Labels.get_Category(i);
                    cat.Alignment = lb.Alignment;
                    cat.OffsetX = lb.OffsetX;
                    cat.OffsetY = lb.OffsetY;
                }
            }

            if (!m_categoryEdited)
            {
                if (double.TryParse(cboMinScale.Text, out val))
                {
                    m_shapefile.Labels.MinVisibleScale = val;
                }

                if (double.TryParse(cboMaxScale.Text, out val))
                {
                    m_shapefile.Labels.MaxVisibleScale = val;
                }
                m_shapefile.Labels.DynamicVisibility = chkDynamicVisibility.Checked;
                
                
            }

            m_shapefile.Labels.TextRenderingHint = (tkTextRenderingHint)cboTextRenderingHint.SelectedIndex;
            this.labelList1.TextRenderingHint = (TextRenderingHint)cboTextRenderingHint.SelectedIndex;
           
            btnApply.Enabled = true;
            this.DrawPreview(null, null);
        }

        /// <summary>
        /// Draws preview of the label
        /// </summary>
        private void DrawPreview(object sender, EventArgs e)
        {
            // this function is called after each change of state, therefore it makes sense to update availability of controls here
            this.RefreshControls();

            if (!m_noEvents)
            {
                string text = m_categoryEdited ? m_shapefile.Labels.Expression : richTextBox1.Text;
                mwSymbology.LabelUtilities.DrawPreview(m_category, m_shapefile, pctPreview, text, true, Color.FromKnownColor(KnownColor.Control));
            }
        }

        /// <summary>
        /// Enables or disables controls which are dependant upon others
        /// </summary>
        private void RefreshControls()
        {
            m_noEvents = true;

            // drawing of frame
            bool drawFrame = chkUseFrame.Checked;
            clpFrame1.Enabled = drawFrame;
            icbLineType.Enabled = drawFrame;
            icbLineWidth.Enabled = drawFrame;
            clpFrameBorder.Enabled = drawFrame;
            udFramePaddingX.Enabled = drawFrame;
            udFramePaddingY.Enabled = drawFrame;

            //outlines
            udHaloSize.Enabled = chkHaloVisible.Checked;
            clpHalo.Enabled = chkHaloVisible.Checked;
            udShadowOffsetX.Enabled = chkShadowVisible.Checked;
            udShadowOffsetY.Enabled = chkShadowVisible.Checked;
            clpShadow.Enabled = chkShadowVisible.Checked;

            icbFrameType.Enabled = chkUseFrame.Checked;
            btnSetFrameGradient.Enabled = chkUseFrame.Checked;

            cboMaxScale.Enabled = chkDynamicVisibility.Checked;
            cboMinScale.Enabled = chkDynamicVisibility.Checked;

            cboBasicScale.Enabled = chkScaleLabels.Checked;
            btnSetCurrent.Enabled = chkScaleLabels.Checked;
            label18.Enabled = chkScaleLabels.Checked;

            btnSetMinScale.Enabled = chkDynamicVisibility.Checked;
            btnSetMaxScale.Enabled = chkDynamicVisibility.Checked;
            cboTextRenderingHint.Enabled = !m_categoryEdited && richTextBox1.Text.Length > 0;

            if (!m_categoryEdited)
            {
                groupBox6.Enabled = richTextBox1.Text.Length > 0;
                groupBox11.Enabled = richTextBox1.Text.Length > 0;
                groupBox13.Enabled = richTextBox1.Text.Length > 0;
                groupBox20.Enabled = richTextBox1.Text.Length > 0;
                groupLabelAlignment.Enabled = richTextBox1.Text.Length > 0;
                chkUseFrame.Enabled = richTextBox1.Text.Length > 0;
                chkGDIMode.Enabled = richTextBox1.Text.Length > 0;
                groupBox2.Enabled = richTextBox1.Text.Length > 0;
                groupBox3.Enabled = richTextBox1.Text.Length > 0;
                groupBox4.Enabled = richTextBox1.Text.Length > 0;
                groupBox5.Enabled = richTextBox1.Text.Length > 0;
                groupBox12.Enabled = richTextBox1.Text.Length > 0;
                btnAddStyle.Enabled = richTextBox1.Text.Length > 0;
                btnRemoveStyle.Enabled = richTextBox1.Text.Length > 0;
                labelList1.Enabled = richTextBox1.Text.Length > 0;
                groupBox14.Enabled = !chkGDIMode.Checked && richTextBox1.Text.Length > 0;
            }
            m_noEvents = false;
        }
        
        /// <summary>
        /// Sets gradient for the frame color
        /// </summary>
        private void btnSetFrameGradient_Click(object sender, EventArgs e)
        {
            frmFontGradient form = new frmFontGradient(m_category, false);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                DrawPreview(null, null);
                clpFrame1.Color = MapWinUtility.Colors.IntegerToColor(m_category.FrameBackColor);
                btnApply.Enabled = true;
            }
        }
        
        /// <summary>
        /// Clears gradient of the frame
        /// </summary>
        private void btnClearFrameGradient_Click(object sender, EventArgs e)
        {
            m_category.FrameGradientMode = tkLinearGradientMode.gmNone;
            DrawPreview(null, null);
        }

        /// <summary>
        ///  Saves the options, closes the form.
        /// </summary>
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!ApplyOptions())
                return;

            tabNumber = tabControl1.SelectedIndex;

            if (m_shapefile.Labels.Serialize() != m_initState)
            {
                m_mapWin.Project.Modified = true;
            }

            SymbologyPlugin.SaveLayerOptions(m_mapWin, m_handle);

            labelList1.SaveToXML();

            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Applies the options
        /// </summary>
        bool ApplyOptions()
        {
            if (m_categoryEdited)
                return true;
            
            if (richTextBox1.Text == "" && m_shapefile.Labels.Count > 0)
            {
                // clear
                if (MessageBox.Show("No expression was entered. Do you want to remove all labels?",
                                 m_mapWin.ApplicationInfo.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    m_shapefile.Labels.Clear();
                    m_shapefile.Labels.Expression = "";
                }
                else
                    return false;
            }
            else if ((!m_shapefile.Labels.Synchronized  || m_shapefile.Labels.Count == 0) && richTextBox1.Text != "")
            {
                // generate
                frmAddLabels form = new frmAddLabels(m_mapWin, m_shapefile, m_category.Alignment);
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    ShpfileType type = SymbologyPlugin.ShapefileType2D(m_shapefile.ShapefileType);
                    if (type == ShpfileType.SHP_POINT || type == ShpfileType.SHP_MULTIPOINT)
                    {
                        m_category.Alignment = form.m_alignment;
                    }

                    form.Dispose();
                }
                else
                {
                    form.Dispose();
                    return false;
                }
            }
            else if (richTextBox1.Text == "" && m_shapefile.Labels.Count == 0)
            {
                //MessageBox.Show("No expression was entered.", m_mapWin.ApplicationInfo.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //return false;
            }

            // applying options
            if (!m_categoryEdited)
            {
                // in case of labels we are editing a copy of the LabelsCategory class, so options should be applied
                m_shapefile.Labels.Options = m_category;

                if (m_shapefile.Labels.Expression != richTextBox1.Text )
                {
                    
                    m_shapefile.Labels.Expression = mwSymbology.LabelUtilities.FixExpression(richTextBox1.Text);
                }
            }
            return true;
        }

        /// <summary>
        ///  Handles the change of transparency by user
        /// </summary>
        private void transparencyControl1_ValueChanged(object sender, byte value)
        {
            GUI2LabelStyle(sender, null);
        }

        #region Expression
        /// <summary>
        /// Adds field to the expression
        /// </summary>
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;

            richTextBox1.SelectedText = "[" + listBox1.SelectedItem.ToString() + "] ";
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectedText = "+ ";
        }

        private void btnQuotes_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectedText = "\"\"";
        }

        private void btnNewLine_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectedText = Environment.NewLine.ToString();
        }

        /// <summary>
        /// Tests expression entered by user
        /// </summary>
        private void btnTest_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text.ToLower() != "<no expression>")
            {
                string expr = mwSymbology.LabelUtilities.FixExpression(richTextBox1.Text);
                if (expr == String.Empty)
                {
                    lblResult.ForeColor = Color.Black;
                    lblResult.Text = "No expression was entered";
                }
                else
                {
                    string err = "";
                    if (!m_shapefile.Table.TestExpression(expr, tkValueType.vtString, ref err))
                    {
                        lblResult.ForeColor = Color.Red;
                        lblResult.Text = err;
                    }
                    else
                    {
                        lblResult.ForeColor = Color.Green;
                        lblResult.Text = "Expression is correct";
                    }
                }
            }
        }

        /// <summary>
        /// Adds field to the expression
        /// </summary>
        private void listBox1_DoubleClick_1(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
                return;

            richTextBox1.SelectedText = "[" + listBox1.SelectedItem.ToString() + "] ";
        }
        #endregion

        /// <summary>
        /// Checks the expression during editing
        /// </summary>
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            this.btnTest_Click(null, null);
            if (!m_noEvents)
            {
                mwSymbology.LabelUtilities.DrawPreview(m_category, m_shapefile, pctPreview, richTextBox1.Text, true, Color.FromKnownColor(KnownColor.Control));
                RefreshControls();
            }
            btnApply.Enabled = true;
        }

        /// <summary>
        /// Clears the expression in the textbox
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text.ToLower() == "<no expression>")
            {
                if (MessageBox.Show("Do you want remove the predefined text for labels?", m_mapWin.ApplicationInfo.ApplicationName,
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    MapWinGIS.Labels lb = m_shapefile.Labels;
                    for (int i = 0; i < lb.Count; i++)
                    {
                        for (int j = 0; j < lb.get_NumParts(i); j++)
                        {
                            lb.get_Label(i, j).Text = "";
                        }
                    }

                    listBox1.Enabled = true;
                    btnPlus.Enabled = true;
                    btnQuotes.Enabled = true;
                    btnNewLine.Enabled = true;
                    richTextBox1.Enabled = true;
                    richTextBox1.Text = "";

                    lb.SavingMode = tkSavingMode.modeXMLOverwrite;
                    lb.Synchronized = true;
                    if (!lb.Synchronized)
                    {
                        lb.Clear();
                    }
                }
            }
            else
            {
                richTextBox1.Text = "";
            }
        }
        
        /// <summary>
        /// Building labels visiblity expression
        /// </summary>
        private void btnLabelExpression_Click(object sender, EventArgs e)
        {
            string s = txtLabelExpression.Text;
            frmQueryBuilder form = new frmQueryBuilder(m_shapefile, m_handle, s, false, m_mapWin);
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (txtLabelExpression.Text != form.Tag.ToString())
                {
                    txtLabelExpression.Text = form.Tag.ToString();
                    m_shapefile.Labels.VisibilityExpression = txtLabelExpression.Text;
                    btnApply.Enabled = true;
                }
            }
            form.Dispose();
        }

        /// <summary>
        /// Clears the label expression
        /// </summary>
        private void btnClearLabelsExpression_Click(object sender, EventArgs e)
        {
            txtLabelExpression.Clear();
            m_shapefile.Labels.VisibilityExpression = "";
        }

        /// <summary>
        /// Saves the options and updates the map without closing the form
        /// </summary>
        private void btnApply_Click(object sender, EventArgs e)
        {
            if (ApplyOptions())
            {
                m_mapWin.View.ForceFullRedraw();
                m_mapWin.Project.Modified = true;
                m_initState = m_shapefile.Labels.Serialize();
                RefreshControls();
                btnApply.Enabled = false;
            }
        }

        /// <summary>
        /// Reverts the changes if cancel was hit
        /// </summary>
        private void frmLabelStyle_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel)
            {
                if (m_categoryEdited)
                {
                    m_category.Deserialize(m_initState);
                }
                else
                {
                    tkSavingMode mode = m_shapefile.Labels.SavingMode;
                    m_shapefile.Labels.SavingMode = tkSavingMode.modeNone;
                    m_shapefile.Labels.Deserialize(m_initState);
                    m_shapefile.Labels.SavingMode = mode;
                }
            }
        }

        /// <summary>
        /// Sets current scale as basic one
        /// </summary>
        private void btnSetCurrent_Click(object sender, EventArgs e)
        {
            AxMapWinGIS.AxMap map = m_mapWin.GetOCX as AxMapWinGIS.AxMap;
            if (map != null)
            {
                cboBasicScale.Text = map.CurrentScale.ToString();
            }
        }

        /// <summary>
        /// Sets max visible scale to current scale
        /// </summary>
        private void btnSetMaxScale_Click(object sender, EventArgs e)
        {
            AxMapWinGIS.AxMap map = m_mapWin.GetOCX as AxMapWinGIS.AxMap;
            if (map != null)
            {
                cboMaxScale.Text = map.CurrentScale.ToString();
                btnApply.Enabled = true;
            }
        }

        /// <summary>
        /// Sets min visible scale to current scale
        /// </summary>
        private void btnSetMinScale_Click(object sender, EventArgs e)
        {
            AxMapWinGIS.AxMap map = m_mapWin.GetOCX as AxMapWinGIS.AxMap;
            if (map != null)
            {
                cboMinScale.Text = map.CurrentScale.ToString();
                btnApply.Enabled = true;
            }
        }

        #region Styles
        /// <summary>
        /// Adds current options as a style to the list
        /// </summary>
        private void btnAddStyle_Click(object sender, EventArgs e)
        {
            string s = m_category.Serialize();
            MapWinGIS.LabelCategory cat = new LabelCategory();
            cat.Deserialize(s);
            LabelStyle style = new LabelStyle(cat);
            labelList1.AddStyle(style);
        }

        /// <summary>
        /// Removes selected style from the list
        /// </summary>
        private void btnRemoveStyle_Click(object sender, EventArgs e)
        {
            labelList1.RemoveStyle(labelList1.SelectedIndex);
        }

        /// <summary>
        /// Handles the change of the style (style is displayed in preview)
        /// </summary>
        private void labelList1_SelectionChanged()
        {
            if (m_noEvents)
                return;

            LabelStyle style = labelList1.SelectedStyle;
            if (style != null)
            {
                string s = style.Serialize();
                tkLabelAlignment alignemnt = m_category.Alignment;
                m_category.Deserialize(s);
                m_category.Alignment = alignemnt;   // preserve the alignment
                this.LabelStyle2GUI(m_category);
                this.DrawPreview(null, null);
                this.RefreshControls();

                btnApply.Enabled = true;
            }
        }
        #endregion

        private void propertyGrid1_Click(object sender, EventArgs e)
        {
            labelList1.Refresh();
        }
    }
}
