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
    using System.Drawing.Drawing2D;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using MapWinGIS;
    using mwSymbology.Classes;
    using System.Xml;
    using System.IO;
    using System.Drawing.Text;
    #endregion

    public partial class frmCategories : Form
    {
        #region Member variables
        // shapefile
        MapWinGIS.Shapefile m_shapefile;

        // reference to MapWindow
        MapWindow.Interfaces.IMapWin m_mapWin;

        // refernce to the plug-in        
        SymbologyPlugin m_plugin;

        // the layer handle 
        int m_layerHandle;

        // supress control events on the loading
        bool m_noEvents = false;

        // the active tab
        static int _tabIndex = 0;

        // serialized state of the categories
        private string m_initState = "";

        /// <summary>
        /// The columns of the dgvLabelCategories control
        /// </summary>
        private const int CMN_VISIBLE = 0;
        private const int CMN_STYLE = 1;
        private const int CMN_NAME = 2;
        private const int CMN_COUNT = 3;
        private const int CMN_WIDTH = 4;
        #endregion

        #region Initialization
        /// <summary>
        /// Creates a new instance of the frmCategories class
        /// </summary>
        public frmCategories(MapWindow.Interfaces.IMapWin mapWin, SymbologyPlugin plugin, MapWinGIS.Shapefile sf, int layerHandle)
        {
            InitializeComponent();
            m_mapWin = mapWin;
            m_shapefile = sf;
            m_plugin = plugin;
            m_layerHandle = layerHandle;

            m_initState = m_shapefile.Categories.Serialize();

            m_noEvents = true;
            m_plugin.LayerColors.SetDefaultColorScheme(m_shapefile);

            icbFillStyle.ComboStyle = ImageComboStyle.HatchStyleWithNone;
            icbLineWidth.ComboStyle = ImageComboStyle.LineWidth;

            groupLine.Visible = false;
            groupFill.Visible = false;
            groupPoint.Visible = false;

            groupLine.Parent = groupFill.Parent;
            groupPoint.Parent = groupFill.Parent;
            groupLine.Top = groupFill.Top;
            groupLine.Left = groupFill.Left;
            groupPoint.Top = groupFill.Top;
            groupPoint.Left = groupFill.Left;

            ShpfileType type = SymbologyPlugin.ShapefileType2D(sf.ShapefileType);
            if (type == ShpfileType.SHP_POINT)
            {
                groupPoint.Visible = true;   
            }
            else if (type == ShpfileType.SHP_POLYLINE)
            {
                groupLine.Visible = true;
            }
            else if (type == ShpfileType.SHP_POLYGON)
            {
                groupFill.Visible = true;
            }

            clpLine.SelectedColorChanged += new EventHandler(GUI2Categories);
            clpPointFill.SelectedColorChanged += new EventHandler(GUI2Categories);
            clpPolygonFill.SelectedColorChanged += new EventHandler(GUI2Categories);

            icbFillStyle.SelectedIndexChanged += new EventHandler(GUI2Categories);
            icbLineWidth.SelectedIndexChanged += new EventHandler(GUI2Categories);
            udPointSize.ValueChanged += new EventHandler(GUI2Categories);

            udFontSize.ValueChanged += new EventHandler(GUI2Categories);
            clpFrame.SelectedColorChanged += new EventHandler(GUI2Categories);
            clpFont.SelectedColorChanged += new EventHandler(GUI2Categories);
            chkFrameVisible.CheckedChanged += new EventHandler(GUI2Categories);

            LegendControl.Layer layer = m_mapWin.View.LegendControl.Layers.ItemByHandle(m_layerHandle);

            // updates the list
            this.RefreshCategoriesList(dgvCategories);
            tabControl1.SelectedTab = tabControl1.TabPages[1];
            this.RefreshCategoriesList(dgvLabels);
            m_noEvents = false;

            this.RefreshControlState(true);
            this.RefreshControlState(false);

            if (m_shapefile.Labels.Count == 0)
            {
                tabControl1.TabPages.Remove(tabControl1.TabPages[1]);
            }

            m_mapWin.View.LockLegend();
            m_mapWin.View.LockMap();

            tabControl1.SelectedIndex = _tabIndex;
        }
        #endregion
                
        #region Buttons
        /// <summary>
        /// Applies the options, saves the settings
        /// </summary>
        private void btnOk_Click(object sender, EventArgs e)
        {
            LegendControl.Layer layer = m_mapWin.View.LegendControl.Layers.ItemByHandle(m_layerHandle);
            if (m_shapefile.Categories.Serialize() != m_initState)
            {
                m_mapWin.Project.Modified = true;
            }
            _tabIndex = tabControl1.SelectedIndex;

            // saves options for default loading behavior
            SymbologyPlugin.SaveLayerOptions(m_mapWin, m_layerHandle);
        }
        #endregion

        #region Categories

        #region Categories buttons
        /// <summary>
        /// Generation of categories with the full set of options
        /// </summary>
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            Form form = null;
            if (tabControl1.SelectedIndex == 0)
            {
                form = new frmGenerateCategories(m_plugin, m_shapefile, m_layerHandle);
            }
            else
            {
                form = new frmGenerateLabelCategories(m_plugin, m_shapefile, m_layerHandle);
            }
            DataGridView dgv = tabControl1.SelectedIndex == 0 ? dgvCategories : dgvLabels;
            
            if (form.ShowDialog() == DialogResult.OK)
            {
                
                this.Enabled = false;
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    RefreshCategoriesList(dgv);
                }
                finally
                {
                    this.Enabled = true;
                    this.Cursor = Cursors.Default;
                }

                if (dgv.RowCount > 0)
                {
                    dgv.CurrentCell = dgv[1, 0];
                }
            }
            form.Dispose();
        }

        /// <summary>
        /// Adds a single category
        /// </summary>
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            bool labels = tabControl1.SelectedIndex == 1;
            frmAddCategories form = new frmAddCategories(m_plugin, labels);
            if (form.ShowDialog() == DialogResult.OK)
            {
                MapWinGIS.ShpfileType type = SymbologyPlugin.ShapefileType2D(m_shapefile.ShapefileType);
                
                ColorBlend blend =  form.icbColors.ColorSchemes.List[form.icbColors.SelectedIndex];
                MapWinGIS.ColorScheme scheme = ColorSchemes.ColorBlend2ColorScheme(blend);
                int count = (int)form.numericUpDownExt1.Value;

                for (int i = 0; i < count; i++)
                {
                    uint color  = 0;
                    if (form.chkRandom.Checked)
                    {
                        color = scheme.get_RandomColor((double)(i + 1) / (double)count);
                    }
                    else
                    {
                        color = scheme.get_GraduatedColor((double)(i + 1) / (double)count);
                    }
                    
                    if (tabControl1.SelectedIndex == 0)
                    {
                        MapWinGIS.ShapefileCategory cat = m_shapefile.Categories.Add("Category " + m_shapefile.Categories.Count);
                        if (type == ShpfileType.SHP_POINT || type == ShpfileType.SHP_POLYGON || type == ShpfileType.SHP_MULTIPOINT)
                        {
                            cat.DrawingOptions.FillColor = color;
                        }
                        else if (type == ShpfileType.SHP_POLYLINE)
                        {
                            cat.DrawingOptions.LineColor = color;
                        }
                    }
                }
                
                form.Dispose();

                DataGridView dgv = tabControl1.SelectedIndex == 0 ? dgvCategories : dgvLabels;
                int rowIndex = - 1;
                if (dgv.CurrentCell != null)
                {
                    rowIndex = dgv.CurrentCell.RowIndex;
                }
                else
                {
                    rowIndex = 0;
                }

                RefreshCategoriesList(dgv);

                if (dgv.RowCount > 0)
                {
                    dgv.CurrentCell = dgv[rowIndex, 0];
                }
            }
        }

        /// <summary>
        /// Removes the selected category
        /// </summary>
        private void btnCategoriesRemove_Click(object sender, EventArgs e)
        {
            DataGridView dgv = tabControl1.SelectedIndex == 0 ? dgvCategories : dgvLabels;

            if (dgv.CurrentCell != null)
            {
                int row = dgv.CurrentCell.RowIndex;
                if (row == 0)   // it's not allowed to remove unclassified values
                    return;

                int cmnIndex = dgv.CurrentCell.ColumnIndex;

                if (tabControl1.SelectedIndex == 0)
                {
                    m_shapefile.Categories.Remove(row - 1);
                }
                else
                {
                    m_shapefile.Labels.RemoveCategory(row - 1);
                }

                this.RefreshCategoriesList(dgv);

                if (dgv.Rows.Count != 0)
                {
                    if (row >= dgv.Rows.Count) 
                    {
                        row--;
                    }
                    dgv.CurrentCell = dgv[cmnIndex, row];
                }

                this.RefreshControlState();
            }
        }

        /// <summary>
        /// Moves the selected category up
        /// </summary>
        private void btnCategoryMoveUp_Click(object sender, EventArgs e)
        {
            DataGridView dgv = tabControl1.SelectedIndex == 0 ? dgvCategories : dgvLabels;

            if (dgv.CurrentCell != null)
            {
                if (dgv.CurrentCell.RowIndex > 0)
                {
                    int row = dgv.CurrentCell.RowIndex;
                    if (row < 2)   // it's not allowed to move unclassified values
                        return;

                    int colIndex = dgv.CurrentCell.ColumnIndex;

                    bool result = false;

                    if (tabControl1.SelectedIndex == 0)
                    {
                        result = m_shapefile.Categories.MoveUp(row - 1);
                    }
                    else
                    {
                        result = m_shapefile.Labels.MoveCategoryUp(row - 1);
                    }

                    if (result)
                    {
                        object temp = dgv[CMN_COUNT, row].Value;
                        dgv[CMN_COUNT, row].Value = dgv[CMN_COUNT, row - 1].Value;
                        dgv[CMN_COUNT, row - 1].Value = temp;

                        this.UpdateRowCategory(row);
                        this.UpdateRowCategory(row - 1);

                        m_noEvents = true;
                        dgv.CurrentCell = dgv[colIndex, row - 1];
                        m_noEvents = false;
                        
                        this.RefreshControlState();
                    }
                }
            }
        }

        /// <summary>
        /// Moves the selected category down
        /// </summary>
        private void btnCategoryMoveDown_Click(object sender, EventArgs e)
        {
            DataGridView dgv = tabControl1.SelectedIndex == 0 ? dgvCategories : dgvLabels;

            if (dgv.CurrentCell != null)
            {
                if (dgv.CurrentCell.RowIndex < dgv.Rows.Count - 1)
                {
                    int row = dgv.CurrentCell.RowIndex;
                    if (row == 0)
                        return;   // it's not allowed to move unclassified values

                    int colIndex = dgv.CurrentCell.ColumnIndex;

                    bool result;
                    if (tabControl1.SelectedIndex == 0)
                    {
                        result = m_shapefile.Categories.MoveDown(row - 1);
                    }
                    else
                    {
                        result = m_shapefile.Labels.MoveCategoryDown(row - 1);
                    }

                    if (result)
                    {
                        this.UpdateRowCategory(row);
                        this.UpdateRowCategory(row + 1);
                    }

                    m_noEvents = true;
                    dgv.CurrentCell = dgv[colIndex, row + 1];
                    m_noEvents = false;

                    this.RefreshControlState();
                }
            }
        }

        /// <summary>
        /// Clears all the categories
        /// </summary>
        private void btnCategoriesClear_Click(object sender, EventArgs e)
        {
            if (SymbologyPlugin.MessageBoxYesNo("Do you want to remove all the categories?") == DialogResult.Yes)
            {
                if (tabControl1.SelectedIndex == 0)
                {
                    m_shapefile.Categories.Clear();
                    this.RefreshCategoriesList(dgvCategories);
                }
                else
                {
                    m_shapefile.Labels.ClearCategories();
                    this.RefreshCategoriesList(dgvLabels);
                }
                this.RefreshControlState();
            }
        }

        /// <summary>
        /// Edit the current expression
        /// </summary>
        private void btnEditExpression_Click(object sender, EventArgs e)
        {
            DataGridView dgv = (tabControl1.SelectedIndex == 0) ? dgvCategories : dgvLabels;
            if (dgv != null)
            {
                if (dgv.CurrentCell != null)
                {
                    int index = dgv.CurrentCell.RowIndex  - 1;
                    if (index < 0)
                        return;

                    if (dgv == dgvCategories)
                    {
                        ShapefileCategory category = m_shapefile.Categories.get_Item(index);
                        frmQueryBuilder form = new frmQueryBuilder(m_shapefile, m_layerHandle, category.Expression, false, m_mapWin);
                        if (form.ShowDialog(this) == DialogResult.OK)
                        {
                            category.Expression = form.Tag.ToString();
                            txtExpression.Text = category.Expression;
                            //RefreshCategoriesCount(true);
                        }
                    }
                    else
                    {
                        LabelCategory category = m_shapefile.Labels.get_Category(index);
                        frmQueryBuilder form = new frmQueryBuilder(m_shapefile, m_layerHandle, category.Expression, false, m_mapWin);
                        if (form.ShowDialog(this) == DialogResult.OK)
                        {
                            category.Expression = form.Tag.ToString();
                            txtLabelExpression.Text = category.Expression;
                            //RefreshCategoriesCount(false);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Changes the style of the selected category
        /// </summary>
        private void btnCategoryStyle_Click(object sender, EventArgs e)
        {
            if (dgvCategories.CurrentCell != null)
            {
                int row = dgvCategories.CurrentCell.RowIndex;
                ChangeCategoryStyle(row);
                
            }
        }

        /// <summary>
        /// Changes the style of the specified category
        /// </summary>
        private void ChangeCategoryStyle(int row)
        {
            //ShapefileCategory cat = m_shapefile.Categories.get_Item(row);
            //if (cat == null) return;

            ShapeDrawingOptions options = this.GetDrawingOptions(row);
            if (options == null) return;

            Form form = m_plugin.GetSymbologyForm(m_layerHandle, m_shapefile.ShapefileType, options, true);
            form.Text = "Category drawing options";

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                this.RefreshControlState(tabControl1.SelectedIndex == 0);
                DataGridView dgv = tabControl1.SelectedIndex == 0 ? dgvCategories : dgvLabels;
                dgv.Invalidate();
                btnApply.Enabled = true;
            }
            form.Dispose();
        }
        #endregion

        #region Filling categories grid
        
        /// <summary>
        /// Updats the representation of category in the grid, by rereading the values
        /// </summary>
        void UpdateRowCategory(int index)
        {
            this.UpdateRowCategory(index, tabControl1.SelectedIndex == 0);
        }
        
        /// <summary>
        /// Updats the representation of category in the grid, by rereading the values
        /// </summary>
        void UpdateRowCategory(int index, bool labels)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                MapWinGIS.LabelCategory cat = this.GetLabelCategory(index);
                LabelStyle style = new LabelStyle(cat);

                // calculating size
                string text = index == 0 ? "default" : "style " + m_shapefile.Labels.NumCategories.ToString();
                Bitmap img = new Bitmap(500, 200);
                Size size = style.MeasureString(Graphics.FromImage(img), text, 20);
                size.Width += 2;

                if (style.FrameVisible)
                {
                    size.Width += (int)(size.Height * 0.3);
                }
                size.Height += 2;
                img.Dispose();

                System.Drawing.Bitmap bmp = (dgvLabels[CMN_STYLE, index].Value as System.Drawing.Bitmap);
                if (bmp != null)
                {
                    bmp.Dispose();
                }
                dgvLabels[CMN_STYLE, index].Value = new System.Drawing.Bitmap(size.Width, size.Height);

                dgvLabels[CMN_VISIBLE, index].Value = cat.Visible;
                dgvLabels[CMN_NAME, index].Value = index == 0 ? "Unclassified" : cat.Name;

                if (size.Height < 21)
                {
                    size.Height = 21;
                }
                dgvLabels.Rows[index].Height = size.Height + 2;
                dgvLabels[CMN_WIDTH, index].Value = size.Width;

                int maxWidth = 0;
                for (int i = 1; i < dgvLabels.Rows.Count; i++)
                {
                    object val = dgvLabels[CMN_WIDTH, i].Value;
                    if (val != null)
                    {
                        if ((int)val > maxWidth)
                        {
                            maxWidth = (int)val;
                        }
                    }
                }
                dgvLabels.Columns[1].Width = maxWidth;
            }
            else
            {
                dgvCategories[CMN_STYLE, index].Value = new Bitmap(30, 14);

                MapWinGIS.ShapeDrawingOptions options = this.GetDrawingOptions(index);
                dgvCategories[CMN_VISIBLE, index].Value = options.Visible;
                
                string name = index > 0 ? m_shapefile.Categories.get_Item(index - 1).Name : "Unclassified";
                dgvCategories[CMN_NAME, index].Value = name;
            }

            if (!m_noEvents)
                btnApply.Enabled = true;
        }
        
        /// <summary>
        /// Fills the data grid view with information about label categories
        /// </summary>
        private void RefreshCategoriesList(DataGridView dgv)
        {
            if (!m_noEvents)
                btnApply.Enabled = true;

            bool noEvents = m_noEvents; // preserving init state

            m_noEvents = true;
            dgv.SuspendLayout();
            dgv.Rows.Clear();

            bool categories = (dgv == dgvCategories);
            int numCategories = categories ? m_shapefile.Categories.Count : m_shapefile.Labels.NumCategories;

            dgv.ColumnHeadersVisible = numCategories > 0;
            if (numCategories > 0)
            {
                // adding categories
                for (int i = 0; i < numCategories + 1; i++)
                {
                    int row = dgv.Rows.Add();
                    
                    // adding unclassified values
                    if (row == 0)
                    {
                        dgv.Rows[row].Visible = false;
                        dgv[CMN_STYLE, row].Value = new Bitmap(30, 14);
                        dgv[CMN_NAME, row].Value = "Unclassified";

                        if (categories)
                        {
                            MapWinGIS.ShapeDrawingOptions options = m_shapefile.DefaultDrawingOptions;
                            dgv[CMN_VISIBLE, row].Value = options.Visible;
                        }
                        else
                        {
                            MapWinGIS.LabelCategory category = m_shapefile.Labels.Options;
                            dgv[CMN_VISIBLE, row].Value = category.Visible;
                        }
                    }
                    
                    dgv.Rows[i].Visible = false;
                    this.UpdateRowCategory(i);
                }

                for (int i = 0; i < numCategories + 1; i++)
                {
                    dgv.Rows[i].Visible = true;
                }

                this.RefreshCategoriesCount(dgv == dgvCategories);
            }
            m_noEvents = noEvents;
            dgv.ResumeLayout();
        }
        #endregion

        #region Categories count
        /// <summary>
        /// Calculates the number of shapes in each category
        /// </summary>
        private void RefreshCategoriesCount(bool categories)
        {
            //if (m_noEvents)
            //    return;
            
            if (categories)
            {
                m_shapefile.Categories.ApplyExpressions();
            }
            else
            {
                m_shapefile.Labels.ApplyCategories();
            }
            
            Dictionary<int, int> values = new Dictionary<int, int>();  // id of category, count
            int category;

            int count = 0;
            if (categories)
            {
                for (int i = 0; i < m_shapefile.NumShapes; i++)
                {
                    category = m_shapefile.get_ShapeCategory(i);
                    if (values.ContainsKey(category))
                    {
                        values[category] += 1;
                    }
                    else
                    {
                        values.Add(category, 1);
                    }
                }
                count = m_shapefile.Categories.Count;
            }
            else
            {
                for (int i = 0; i < m_shapefile.Labels.Count; i++)
                {
                    category = m_shapefile.Labels.get_Label(i, 0).Category;
                    if (values.ContainsKey(category))
                    {
                        values[category] += 1;
                    }
                    else
                    {
                        values.Add(category, 1);
                    }
                }
                count = m_shapefile.Labels.NumCategories;
            }

            DataGridView dgv = categories ? dgvCategories : dgvLabels;
            if (dgv.Rows.Count > 0)
            {
                for (int i = -1; i < count; i++)
                {
                    if (values.ContainsKey(i))
                    {
                        dgv[CMN_COUNT, i + 1].Value = values[i];
                    }
                    else
                    {
                        dgv[CMN_COUNT, i + 1].Value = 0;
                    }
                }
            }
        }
        #endregion

        #region Data grid view events
        /// <summary>
        /// Opening forms for editing the category
        /// </summary>
        private void dgvLabelCategories_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == CMN_STYLE )
            {
                if (tabControl1.SelectedIndex == 0)
                {
                    this.ChangeCategoryStyle(e.RowIndex);
                }
                else
                {
                    this.ChangeLabelCategoryStyle(e.RowIndex);
                }
            }
        }

        /// <summary>
        /// Gets drawing options for specified row in the table. Default drawing options 
        /// are always in the first row.
        /// </summary>
        private ShapeDrawingOptions GetDrawingOptions(int row)
        {
            if (row == 0)
            {
                return m_shapefile.DefaultDrawingOptions;
            }
            else if (row > 0 && row <= m_shapefile.Categories.Count)
            {
                return m_shapefile.Categories.get_Item(row - 1).DrawingOptions;
            }
            else
            {
                throw new NullReferenceException("Invalid row index");
            }
        }

        /// <summary>
        /// Gets label options for specified row in the table. Default options 
        /// are returned for the first row
        private LabelCategory GetLabelCategory(int row)
        {
            if (row == 0)
            {
                return m_shapefile.Labels.Options;
            }
            else if (row > 0 && row <= m_shapefile.Labels.NumCategories)
            {
                return m_shapefile.Labels.get_Category(row - 1);
            }
            else
            {
                throw new NullReferenceException("Invalid row index");
            }
        }

        /// <summary>
        /// Drawing of images in the style column
        /// </summary>
        private void dgvCategories_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == CMN_STYLE)
            {
                if (tabControl1.SelectedIndex == 0)
                {
                    // shapefile categories
                    if (e.RowIndex >= 0 && e.RowIndex <= m_shapefile.Categories.Count)
                    {
                        System.Drawing.Image img = e.Value as System.Drawing.Image;
                        if (img == null) return;

                        //ShapefileCategory cat = m_shapefile.Categories.get_Item(e.RowIndex);
                        //if (cat == null) return;
                        ShapeDrawingOptions sdo = this.GetDrawingOptions(e.RowIndex);
                        if (sdo == null) return;

                        Graphics g = Graphics.FromImage(img);
                        g.Clear(Color.White);
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = SmoothingMode.HighQuality;

                        MapWinGIS.ShpfileType type = SymbologyPlugin.ShapefileType2D(m_shapefile.ShapefileType);

                        if (type == ShpfileType.SHP_POLYGON)
                        {
                            sdo.DrawRectangle(g.GetHdc(), 0.0f, 0.0f, img.Width - 1, img.Height - 1, true, img.Width, img.Height, MapWinUtility.Colors.ColorToUInteger(dgvCategories.BackgroundColor));
                        }
                        else if (type == ShpfileType.SHP_POLYLINE)
                        {
                            sdo.DrawLine(g.GetHdc(), 0.0f, 0.0f, img.Width - 1, img.Height - 1, true, img.Width, img.Height, MapWinUtility.Colors.ColorToUInteger(dgvCategories.BackgroundColor));
                        }
                        else if (type == ShpfileType.SHP_POINT)
                        {
                            sdo.DrawPoint(g.GetHdc(), 0.0f, 0.0f, img.Width, img.Height, MapWinUtility.Colors.ColorToUInteger(dgvCategories.BackgroundColor));
                        }
                        g.ReleaseHdc();
                        g.Dispose();
                    }
                }
                else if (tabControl1.SelectedIndex == 1)
                {
                    // label categories
                    if (e.RowIndex >= 0 && e.RowIndex <= m_shapefile.Labels.NumCategories)
                    {
                        System.Drawing.Image img = e.Value as System.Drawing.Image;
                        if (img == null) return;
                        Graphics g = Graphics.FromImage(img);
                        g.Clear(Color.White);
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                        //LabelCategory cat = m_shapefile.Labels.get_Category(e.RowIndex);
                        LabelCategory cat = this.GetLabelCategory(e.RowIndex);
                        if (cat != null)
                        {
                            LabelStyle style = new LabelStyle(cat);
                            System.Drawing.Point pnt = new System.Drawing.Point();
                            pnt.Y = 0;
                            int size = style.FontSize > 20 ? 20 : style.FontSize;
                            pnt.X = style.FrameVisible ? (int)(size * 0.4) : 0;
                            string text = e.RowIndex == 0? "Default" : "Style " + Convert.ToString(e.RowIndex + 1);
                            style.TextRenderingHint =  (TextRenderingHint)m_shapefile.Labels.TextRenderingHint;
                            style.Draw(g, pnt, text, false, 20);
                        }
                        g.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Drawing the focus rectangle
        /// </summary>
        private void dgvLabelCategories_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView dgv = tabControl1.SelectedIndex == 0 ? dgvCategories : dgvLabels;

            if (dgv.CurrentCell == null) return;
            if (e.ColumnIndex == dgv.CurrentCell.ColumnIndex && e.RowIndex == dgv.CurrentCell.RowIndex)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                using (Pen p = new Pen(Color.Black, 4))
                {
                    Rectangle rect = e.CellBounds;
                    rect.Width -= 1;
                    rect.Height -= 1;
                    ControlPaint.DrawFocusRectangle(e.Graphics, rect);
                }
                e.Handled = true;
            }
        }
        #endregion

        #region Categories interaction

        /// <summary>
        /// Bans the editing of the certain columns
        /// </summary>
        private void dgvCategories_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == CMN_COUNT)
            {
                e.Cancel = true;
            }
            else if (e.ColumnIndex == CMN_NAME && e.RowIndex == 0)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Saves editing of the category names
        /// </summary>
        private void dgvCategories_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 1)
                return;

            if (e.ColumnIndex == CMN_NAME)
            {
                if (tabControl1.SelectedIndex == 0)
                {
                    m_shapefile.Categories.get_Item(e.RowIndex - 1).Name = dgvCategories[CMN_NAME, e.RowIndex].Value.ToString();
                }
                else
                {
                    m_shapefile.Labels.get_Category(e.RowIndex - 1).Name = dgvLabels[CMN_NAME, e.RowIndex].Value.ToString();
                }
            }
        }

        /// <summary>
        /// Toggles visibility of the categories
        /// </summary>
        private void dgvCategories_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (m_noEvents) return;
            if (e.RowIndex == -1 || e.ColumnIndex == -1) return;

            DataGridView dgv = tabControl1.SelectedIndex == 0 ? dgvCategories : dgvLabels;
            if (dgv != null)
            {
                int index = e.RowIndex;
                if (e.ColumnIndex == CMN_VISIBLE)
                {
                    bool visible = (bool)dgv[e.ColumnIndex, e.RowIndex].Value;
                    if (dgv == dgvCategories)
                    {
                        this.GetDrawingOptions(index).Visible = visible;
                    }
                    else
                    {
                        this.GetLabelCategory(index).Visible = visible;
                    }
                }
                btnApply.Enabled = true;
            }
        }

        /// <summary>
        /// Committing changes of the checkbox state immediately, CellValueChanged event won't be triggered otherwise
        /// </summary>
        private void dgvCategories_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DataGridView dgv = tabControl1.SelectedIndex == 0 ? dgvCategories : dgvLabels;
            if (dgv != null)
            {
                if (dgv.CurrentCell.ColumnIndex == CMN_VISIBLE)
                {
                    if (dgv.IsCurrentCellDirty)
                    {
                        dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    }
                }
            }
        }

        /// <summary>
        /// Saves user input to the drawing options
        /// </summary>
        private void GUI2Categories(object sender, EventArgs e)
        {
            if (m_noEvents)
                return;
            
            DataGridView dgv = (tabControl1.SelectedIndex == 0) ? dgvCategories : dgvLabels;
             
            if (dgv != null)
            {
                if (dgv.CurrentCell != null)
                {
                    int index = dgv.CurrentCell.RowIndex;

                    if (dgv == dgvCategories)
                    {
                        //ShapefileCategory category = m_shapefile.Categories.get_Item(index);
                        ShapeDrawingOptions options = this.GetDrawingOptions(index);
                        if (options != null)
                        {
                            ShpfileType type = SymbologyPlugin.ShapefileType2D(m_shapefile.ShapefileType);
                            if (type == ShpfileType.SHP_POINT || type == ShpfileType.SHP_MULTIPOINT)
                            {
                                options.FillColor = MapWinUtility.Colors.ColorToUInteger(clpPointFill.Color);
                                options.PointSize = (float)udPointSize.Value;
                            }
                            else if (type == ShpfileType.SHP_POLYLINE)
                            {
                                options.LineColor = MapWinUtility.Colors.ColorToUInteger(clpLine.Color);
                                options.LineWidth = (float)icbLineWidth.SelectedIndex;
                            }
                            else if (type == ShpfileType.SHP_POLYGON)
                            {
                                options.FillColor = MapWinUtility.Colors.ColorToUInteger(clpPolygonFill.Color);
                                icbFillStyle.Color1 = clpPolygonFill.Color;
                                if (icbFillStyle.SelectedIndex == 0 && options.FillType == tkFillType.ftHatch)
                                {
                                    options.FillType = tkFillType.ftStandard;
                                }
                                else
                                {
                                    options.FillType = tkFillType.ftHatch;
                                    options.FillHatchStyle = (tkGDIPlusHatchStyle)icbFillStyle.SelectedIndex - 1;
                                }
                            }
                        }
                    }
                    else
                    {
                        //LabelCategory category = m_shapefile.Labels.get_Category(index);
                        LabelCategory category = this.GetLabelCategory(index);

                        category.FontColor = MapWinUtility.Colors.ColorToUInteger(clpFont.Color);
                        category.FrameBackColor = MapWinUtility.Colors.ColorToUInteger(clpFrame.Color);
                        category.FontSize = (int)udFontSize.Value;
                        category.FrameVisible = chkFrameVisible.Checked;

                        if (index == 0)  // in case of default options we work with a copy of values
                            m_shapefile.Labels.Options = category;
                    }
                    this.UpdateRowCategory(index);
                    btnApply.Enabled = true;
                }
            }
        }
        #endregion

        #region Category properties
        /// <summary>
        /// Displays the values for the selected category
        /// </summary>
        private void dgvCategories_CurrentCellChanged(object sender, EventArgs e)
        {
            this.RefreshControlState(tabControl1.SelectedIndex == 0);
        }
        
        /// <summary>
        /// Sets the enabled state of the control accorging to the state categories
        /// </summary>
        private void RefreshControlState()
        {
            this.RefreshControlState(tabControl1.SelectedIndex == 0);
        }

        /// <summary>
        /// Sets the enabled state of the control accorging to the state categories
        /// </summary>
        private void RefreshControlState(bool categories)
        {
            if (m_noEvents)
            {
                return;
            }

            DataGridView dgv = categories ? dgvCategories : dgvLabels;
            if (dgv != null)
            {
                bool exists = (dgv.CurrentCell != null);
                bool classified = exists ? dgv.CurrentCell.RowIndex != 0 : false;

                if (dgv == dgvCategories)
                {
                    icbFillStyle.Enabled = exists;
                    clpPolygonFill.Enabled = exists;
                    icbLineWidth.Enabled = exists;
                    udPointSize.Enabled = exists;
                    clpLine.Enabled = exists;
                    clpPointFill.Enabled = exists;
                    txtExpression.Visible = exists;
                    groupExpression.Enabled = exists;
                    groupFill.Enabled = exists;
                    groupLine.Enabled = exists;
                    groupPoint.Enabled = exists;
                    btnCategoryStyle.Enabled = exists;

                    btnCategoryRemove.Enabled = classified;
                    txtExpression.Visible = classified;
                    btnEditExpression.Enabled = classified;

                    if (dgv.CurrentCell != null)
                    {
                        int index = dgv.CurrentCell.RowIndex;
                        this.UpdateCategoryOptions(index);
                        btnCategoryMoveUp.Enabled = index > 1;
                        btnCategoryMoveDown.Enabled = index < m_shapefile.Categories.Count;
                    }
                    else
                    {
                        btnCategoryMoveUp.Enabled = false;
                        btnCategoryMoveDown.Enabled = false;
                        clpLine.Color = Color.White;
                        clpPointFill.Color = Color.White;
                        clpPolygonFill.Color = Color.White;
                        txtExpression.Text = "";
                    }
                }
                else
                {
                    groupLabels.Enabled = exists;
                    txtLabelExpression.Visible = exists;
                    groupLabelExpression.Enabled = exists;
                    btnLabelOptions.Enabled = exists;
                    
                    btnLabelExpression.Enabled = classified;
                    btnLabelsRemove.Enabled = classified;
                    txtLabelExpression.Visible = classified;

                    if (dgv.CurrentCell != null)
                    {
                        int index = dgv.CurrentCell.RowIndex;
                        this.UpdateLabelOptions(index);
                        btnLabelsMoveUp.Enabled = index > 1;
                        btnLabelsMoveDown.Enabled = index < m_shapefile.Labels.NumCategories;
                    }
                    else
                    {
                        clpFont.Color = Color.White;
                        clpFrame.Color = Color.White;
                        txtLabelExpression.Text = "";
                        btnLabelsMoveUp.Enabled = false;
                        btnLabelsMoveDown.Enabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// Updates the state of the category appearance controls
        /// </summary>
        /// <param name="index"></param>
        void UpdateCategoryOptions(int index)
        {
            //ShapefileCategory category = m_shapefile.Categories.get_Item(index);
            ShapeDrawingOptions options = this.GetDrawingOptions(index);
            ShapefileCategory cat = index > 0 ? m_shapefile.Categories.get_Item(index - 1) : null;
            if (options != null)
            {
                m_noEvents = true;

                txtExpression.Text = cat != null ? cat.Expression : "None";
                ShpfileType type = SymbologyPlugin.ShapefileType2D(m_shapefile.ShapefileType);
               
                if (type == ShpfileType.SHP_POINT || type == ShpfileType.SHP_MULTIPOINT)
                {
                    clpPointFill.Color = MapWinUtility.Colors.IntegerToColor(options.FillColor);
                    udPointSize.SetValue(options.PointSize);
                }
                else if (type == ShpfileType.SHP_POLYLINE)
                {
                    clpLine.Color = MapWinUtility.Colors.IntegerToColor(options.LineColor);
                    icbLineWidth.SelectedIndex = (int)options.LineWidth;
                }
                else if (type == ShpfileType.SHP_POLYGON)
                {
                    clpPolygonFill.Color = MapWinUtility.Colors.IntegerToColor(options.FillColor);
                    icbFillStyle.Color1 = clpPolygonFill.Color;

                    bool canChangeColor = options.FillType != tkFillType.ftPicture;
                    icbFillStyle.Enabled = canChangeColor;
                    clpPolygonFill.Enabled = canChangeColor;

                    if (options.FillType != tkFillType.ftHatch)
                    {
                        icbFillStyle.SelectedIndex = 0;
                    }
                    else
                    {
                        icbFillStyle.SelectedIndex = (int)options.FillHatchStyle + 1;
                    }
                    icbFillStyle.Invalidate();
                }
                m_noEvents = false;
            }
        }

        /// <summary>
        ///  Update the state of label appearance controls
        /// </summary>
        void UpdateLabelOptions(int index)
        {
            //LabelCategory category = m_shapefile.Labels.get_Category(index);
            LabelCategory category = this.GetLabelCategory(index);
            if (category != null)
            {
                m_noEvents = true;

                txtLabelExpression.Text = category.Expression;
                ShpfileType type = SymbologyPlugin.ShapefileType2D(m_shapefile.ShapefileType);
               
                clpFont.Color = MapWinUtility.Colors.IntegerToColor(category.FontColor);
                clpFrame.Color = MapWinUtility.Colors.IntegerToColor(category.FrameBackColor);
                udFontSize.SetValue(category.FontSize); 
                chkFrameVisible.Checked = category.FrameVisible;

                m_noEvents = false;
            }
        }
        #endregion

        #endregion

        #region Labels
        
        /// <summary>
        /// Clears all categories
        /// </summary>
        private void btnLabelCategoriesClear_Click(object sender, EventArgs e)
        {
            m_shapefile.Categories.Clear();
            RefreshCategoriesList(dgvLabels);
        }

        /// <summary>
        ///  Changes the style of the active category
        /// </summary>
        private void btnLabelCategoriesStyle_Click(object sender, EventArgs e)
        {
            if (dgvLabels.CurrentRow != null)
            {
                ChangeLabelCategoryStyle(dgvLabels.CurrentRow.Index);
            }
        }

        /// <summary>
        /// Changes the style of the selected category
        /// </summary>
        private void ChangeLabelCategoryStyle(int row)
        {
            frmLabelStyle styleForm = null;
            if (row == 0)
            {
                styleForm = new frmLabelStyle(m_shapefile, m_mapWin, m_layerHandle);
                styleForm.Text = "Default style";
            }
            else
            {
                LabelCategory cat = this.GetLabelCategory(row);
                if (cat == null)
                    return;
                styleForm = new frmLabelStyle(m_shapefile, cat, m_mapWin);
            }

            if (styleForm.ShowDialog(this) == DialogResult.OK)
            {
                this.UpdateRowCategory(row);
                dgvLabels.Invalidate();
                this.RefreshControlState();
                btnApply.Enabled = true;
            }
            styleForm.Dispose();
        }

        /// <summary>
        /// Adds a single category to label categories
        /// </summary>
        private void btnLabelsAdd_Click(object sender, EventArgs e)
        {
            int cmn = 0;
            if (dgvLabels.CurrentCell != null)
            {
                cmn = dgvLabels.CurrentCell.ColumnIndex;
            }
            string name = "Style " + m_shapefile.Labels.NumCategories;
            m_shapefile.Labels.AddCategory(name);
            this.RefreshCategoriesList(dgvLabels);

            dgvLabels.CurrentCell = dgvLabels[cmn, dgvLabels.Rows.Count - 1];
            this.RefreshControlState();
        }
        #endregion
        
        #region Expression
        /// <summary>
        /// Shows apply button after editing expression
        /// </summary>
        private void txtExpression_TextChanged(object sender, EventArgs e)
        {
            if (!m_noEvents)
                btnApply.Enabled = true;
        }

        /// <summary>
        /// Applying the expression entered
        /// </summary>
        private void txtExpression_Validated(object sender, EventArgs e)
        {
            DataGridView dgv = (tabControl1.SelectedIndex == 0) ? dgvCategories : dgvLabels;
            if (dgv != null)
            {
                if (dgv.CurrentCell != null)
                {
                    int index = dgv.CurrentCell.RowIndex;
                    if (index == 0)
                        return;         // no expressions for unclassified values

                    if (dgv == dgvCategories)
                    {
                        ShapefileCategory category = m_shapefile.Categories.get_Item(index - 1);
                        if (category != null)
                            category.Expression = txtExpression.Text;
                    }
                    else
                    {
                        LabelCategory category = m_shapefile.Labels.get_Category(index - 1);
                        if (category != null)
                            category.Expression = txtLabelExpression.Text;
                    }
                }
            }
        }
        #endregion

        #region Copying
        /// <summary>
        /// Copies categories from labels
        /// </summary>
        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (dgvLabels.Rows.Count == 0)
            {
                SymbologyPlugin.MessageBoxInformation("No categories to copy");
                return;
            }

            if (dgvCategories.Rows.Count  > 0)
            {
                if (MessageBox.Show("Do you want to copy categories definitions from labels?", m_mapWin.ApplicationInfo.ApplicationName,
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }
            // clear categories
            ShapefileCategories categories = m_shapefile.Categories;
            Labels labels = m_shapefile.Labels;
            categories.Clear();

            for (int i = 0; i < labels.NumCategories; i++)
            {
                MapWinGIS.LabelCategory labelCat = labels.get_Category(i);
                if (labelCat != null)
                {
                    MapWinGIS.ShapefileCategory cat = categories.Add(labelCat.Name);
                    cat.Expression = labelCat.Expression;
                }
            }
            RefreshCategoriesList(dgvCategories);
            RefreshControlState();
            btnApply.Enabled = true;
        }

        /// <summary>
        /// Copies categories from shapefile
        /// </summary>
        private void btnLabelsCopy_Click(object sender, EventArgs e)
        {
            if (dgvCategories.Rows.Count == 0)
            {
                SymbologyPlugin.MessageBoxInformation("No categories to copy");
                return;
            }

            if (dgvLabels.Rows.Count > 0)
            {
                if (MessageBox.Show("Do you want to copy categories definitions from shapefile?", m_mapWin.ApplicationInfo.ApplicationName,
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }
            // clear categories
            ShapefileCategories categories = m_shapefile.Categories;
            Labels labels = m_shapefile.Labels;
            labels.ClearCategories();

            for (int i = 0; i < categories.Count; i++)
            {
                MapWinGIS.ShapefileCategory cat = categories.get_Item(i);
                if (cat != null)
                {
                    MapWinGIS.LabelCategory labelCat = labels.AddCategory(cat.Name);
                    labelCat.Expression = cat.Expression;
                }
            }
            RefreshCategoriesList(dgvLabels);
            RefreshControlState(true);
            RefreshControlState(false);
            btnApply.Enabled = true;
        }
        #endregion

        #region Context menu
        /// <summary>
        ///  Shows context menu with additional options
        /// </summary>
        private void btnMore_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(Cursor.Position);
        }
        
        /// <summary>
        /// Handles clicks of context menu
        /// </summary>
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            contextMenuStrip1.Visible = false;
            Application.DoEvents();
            switch(e.ClickedItem.Name)
            {
                case "btnSaveCategories":
                    this.SaveToXML();
                    break;
                case "btnLoadCategories":
                    this.LoadFromXML();
                    break;
                case "btnClear":
                    this.btnCategoriesClear_Click(null, null);
                    break;
                case "btnCopyFrom":
                    if (tabControl1.SelectedIndex == 0)
                        this.btnCopy_Click(null, null);
                    else
                        this.btnLabelsCopy_Click(null, null);
                    break;
                case "btnAddRange":
                    if (tabControl1.SelectedIndex == 0)
                        this.btnAddCategory_Click(null, null);
                    else
                        this.btnLabelsAdd_Click(null, null);
                    break;
            }
        }

        /// <summary>
        /// Before opening context menu
        /// </summary>
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            btnCopyFrom.Text = tabControl1.SelectedIndex == 0 ? "Copy from labels" : "Copy from shapefile";
            btnCopyFrom.Enabled = tabControl1.SelectedIndex == 0 ? m_shapefile.Labels.NumCategories > 0 : m_shapefile.Categories.Count > 0;
            btnSaveCategories.Enabled = tabControl1.SelectedIndex == 0 ? m_shapefile.Categories.Count > 0 : m_shapefile.Labels.NumCategories > 0;
            btnClear.Enabled = tabControl1.SelectedIndex == 0 ? m_shapefile.Categories.Count > 0 : m_shapefile.Labels.NumCategories > 0;
            btnSaveCategories.Visible = tabControl1.SelectedIndex == 0;
            btnLoadCategories.Visible = tabControl1.SelectedIndex == 0;
        }
        #endregion

        #region Serialization
        /// <summary>
        /// Saves list of styles to XML
        /// </summary>
        public void SaveToXML()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Legend categories (*.mwleg)|*.mwleg";
            if (m_shapefile.SourceType == tkShapefileSourceType.sstDiskBased)
            {
                dlg.InitialDirectory = Path.GetDirectoryName(m_shapefile.Filename);
            }
            
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                MapWindow.Interfaces.Layer layer = m_mapWin.Layers[m_layerHandle];
                if (layer != null)
                {
                    layer.SaveShapefileCategories(dlg.FileName);
                }
                
                //XmlDocument xmlDoc = new XmlDocument();
                //xmlDoc.LoadXml("<MapWindow version= '" + "'></MapWindow>");     // TODO: add version

                //XmlElement xelRoot = xmlDoc.DocumentElement;
                //XmlAttribute attr = xmlDoc.CreateAttribute("FileType");
                //attr.InnerText = "ShapefileCategories";
                //xelRoot.Attributes.Append(attr);

                //attr = xmlDoc.CreateAttribute("FileVersion");
                //attr.InnerText = "0";
                //xelRoot.Attributes.Append(attr);

                //XmlElement xel = xmlDoc.CreateElement("Categories");
                //string s = m_shapefile.Categories.Serialize();
                //xel.InnerText = s;

                //xelRoot.AppendChild(xel);

                //try
                //{
                //    xmlDoc.Save(dlg.FileName);
                //}
                //catch(System.Xml.XmlException ex)
                //{
                //    SymbologyPlugin.MessageBoxError("Failed to save options: " + ex.Message);
                //}
            }
            dlg.Dispose();
        }

        /// <summary>
        /// Loads all the icons form the current path
        /// </summary>
        public void LoadFromXML()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Legend categories (*.mwleg)|*.mwleg";
            if (m_shapefile.SourceType == tkShapefileSourceType.sstDiskBased)
            {
                dlg.InitialDirectory = Path.GetDirectoryName(m_shapefile.Filename);
            }

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                MapWindow.Interfaces.Layer layer = m_mapWin.Layers[m_layerHandle];
                if (layer != null)
                {
                    layer.LoadShapefileCategories(dlg.FileName);
                    this.RefreshCategoriesList(dgvCategories);
                    this.RefreshControlState(true);
                }
            }
            dlg.Dispose();
        }
        #endregion

        #region Apply and cancel
        /// <summary>
        /// Reverts  changes and closes the form
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // cancel will be run in form_closing handler
        }

        /// <summary>
        /// Saves changes and updates map without closing the form
        /// </summary>
        private void btnApply_Click(object sender, EventArgs e)
        {
            m_shapefile.Categories.ApplyExpressions();
            this.RefreshCategoriesCount(true);
            this.RefreshCategoriesCount(false);
            m_mapWin.View.ForceFullRedraw();
            m_mapWin.Project.Modified = true;
            m_initState = m_shapefile.Categories.Serialize();
            btnApply.Enabled = false;
        }

        /// <summary>
        /// Cancels edits if needed
        /// </summary>
        private void frmCategories_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_mapWin.View.UnlockLegend();
            m_mapWin.View.UnlockMap();
            
            if (this.DialogResult == DialogResult.Cancel)
            {
                CancelEdits();
            }
        }

        /// <summary>
        /// Cancels changes made by user
        /// </summary>
        private void CancelEdits()
        {
            // redraw will occur only when state has changed
            // there is an assumption here that additional serialization will work faster than aditional redraw
            string state = m_shapefile.Categories.Serialize();
            if (m_initState != state)
            {
                m_shapefile.Categories.Deserialize(m_initState);
            }
        }
        #endregion
    }
}


