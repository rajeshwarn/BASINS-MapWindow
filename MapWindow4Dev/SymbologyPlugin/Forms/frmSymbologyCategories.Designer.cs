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
using System.Runtime.InteropServices;

    partial class frmSymbologyMain
    {
        // column indices of the categories grid
        private const int CMN_CATEGORYID = 0;
        private const int CMN_VISIBLE = 1;
        private const int CMN_STYLE = 2;
        private const int CMN_NAME = 3;
        private const int CMN_EXPRESSION = 4;
        private const int CMN_COUNT = 5;

        private bool m_cellEditing = false;

        [DllImport("user32.dll")]
        internal static extern bool GetCursorPos(ref System.Drawing.Point point);

        [DllImport("user32.dll")]
        internal static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle rect);

        #region Initialization
        /// <summary>
        /// Initializes the categories tab
        /// </summary>
        private void InitCategoriesTab()
        {
            icbCategories.ComboStyle = ImageComboStyle.ColorSchemeGraduated;
            icbCategories.ColorSchemes = m_plugin.LayerColors;
            if (icbCategories.Items.Count > 0)
            {
                icbCategories.SelectedIndex = 0;
            }

            // layer settings
            chkSetGradient.Checked = m_settings.CategoriesUseGradient;
            chkRandomColors.Checked = m_settings.CategoriesRandomColors;
            udNumCategories.Value = m_settings.CategoriesCount;
            chkUniqueValues.Checked = m_settings.CategoriesClassification == tkClassificationType.ctUniqueValues;
            chkUseVariableSize.Checked = m_settings.CategoriesVariableSize;

            // fills in the list of fields
            this.FillFieldList(m_settings.CategoriesFieldName);

            // setting the color scheme that is in use
            for (int i = 0; i < icbCategories.Items.Count; i++)
            {
                if (m_plugin.LayerColors.List[i] == m_settings.CategoriesColorScheme)
                {
                    icbCategories.SelectedIndex = i;
                    break;
                }
            }

            MapWinGIS.ShpfileType type = SymbologyPlugin.ShapefileType2D(m_shapefile.ShapefileType);
            groupVariableSize.Visible = (type == ShpfileType.SHP_POINT || type == ShpfileType.SHP_POLYLINE);

            switch (type)
            {
                case ShpfileType.SHP_POINT:
                    udMinSize.SetValue(m_shapefile.DefaultDrawingOptions.PointSize);
                    break;
                case ShpfileType.SHP_POLYLINE:
                    udMinSize.SetValue(m_shapefile.DefaultDrawingOptions.LineWidth);
                    break;
            }
            udMaxSize.SetValue((double)udMinSize.Value + m_settings.CategoriesSizeRange);

            this.InitCategoriesDelegates();

            this.RefreshCategoriesList();
        }
        #endregion

        #region InitDelegates
        /// <summary>
        /// Initializes functions to handle controls' events
        /// </summary>
        private void InitCategoriesDelegates()
        {
            if (dgvCategories.Rows.Count > 0 && dgvCategories.Columns.Count > 0)
            {
                dgvCategories[0, 0].Selected = true;
            }

            btnChangeColorScheme.Click += delegate(object sender, EventArgs e)
            {
                frmColorSchemes form = new frmColorSchemes(ref m_plugin.LayerColors);
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    icbCategories.ColorSchemes = m_plugin.LayerColors;
                }
                form.Dispose();
            };

            // Toggles between unique values and natural breaks. Natural break are available for numeric fields only.
            chkUniqueValues.CheckedChanged += delegate(object sender, EventArgs e)
            {
                this.FillFieldList("");
            };

            chkRandomColors.CheckedChanged += delegate(object sender, EventArgs e)
            {
                int index = icbCategories.SelectedIndex;
                icbCategories.ComboStyle = chkRandomColors.Checked ? ImageComboStyle.ColorSchemeRandom : ImageComboStyle.ColorSchemeGraduated;
                if (index >= 0 && index < icbCategories.Items.Count)
                {
                    icbCategories.SelectedIndex = index;
                }
            };

            mnuRemoveCategory.Click += delegate(object sender, EventArgs e) { this.RemoveCategory(); };
            btnCategoryGenerate.Click += delegate(object sender, EventArgs e) { this.GenerateCategories(); };
            mnuAppearance.Click += delegate(object sender, EventArgs e) { this.ChangeCategoryStyle(); };
            btnCategoryClear.Click += delegate(object sender, EventArgs e) { this.ClearCategories(); };
            mnuUpdateCount.Click += delegate(object sender, EventArgs e) 
            {
                m_shapefile.Categories.ApplyExpressions();
                this.RefreshCategoriesList();
            };

            menuCategories.Opening += delegate(object sender, System.ComponentModel.CancelEventArgs e)
            {
                System.Drawing.Point pnt = new System.Drawing.Point(0, 0);
                frmSymbologyMain.GetCursorPos(ref pnt);

                Rectangle rect = new Rectangle();
                frmSymbologyMain.GetWindowRect(dgvCategories.Handle, ref rect);

                if (!m_cellEditing)
                {
                    System.Windows.Forms.DataGridView.HitTestInfo info = dgvCategories.HitTest(pnt.X - rect.X, pnt.Y - rect.Y);
                    if (info.ColumnIndex >= 0 && info.RowIndex >= 0)
                    {
                        if (!dgvCategories[info.ColumnIndex, info.RowIndex].Selected)
                            dgvCategories[info.ColumnIndex, info.RowIndex].Selected = true;

                        bool enabled = dgvCategories.CurrentCell != null;
                        mnuAppearance.Enabled = enabled;
                        mnuVisible.Enabled = dgvCategories.Rows.Count > 0;
                        mnuVisible.Enabled = dgvCategories.Rows.Count > 0;
                        if (enabled)
                            enabled = dgvCategories.CurrentCell.RowIndex != 0;
                        mnuRemoveCategory.Enabled = enabled;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
            };

            mnuVisibleAll.Click += delegate(object sender, EventArgs e)
            {
                this.SetCategoriesVisibility(true);
            };
            mnuVisibleNone.Click += delegate(object sender, EventArgs e)
            {
                this.SetCategoriesVisibility(false);
            };
        }
        #endregion

        #region Generate categories
        /// <summary>
        /// Generates shapefile categories
        /// </summary>
        private void GenerateCategories()
        {
            int count = Convert.ToInt32(udNumCategories.Value);
            MapWinGIS.ShapefileCategories categories = m_shapefile.Categories;

            if (lstFields1.SelectedItem == null) return;
            string name = lstFields1.SelectedItem.ToString().ToLower().Trim();

            int index = -1;
            for (int i = 0; i < m_shapefile.NumFields; i++)
            {
                if (m_shapefile.get_Field(i).Name.ToLower() == name)
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
                return;

            MapWinGIS.tkClassificationType classification = chkUniqueValues.Checked ? tkClassificationType.ctUniqueValues : tkClassificationType.ctNaturalBreaks;

            // preventing the large number of categories
            bool showWaiting = false;
            if (classification == tkClassificationType.ctUniqueValues)
            {
                HashSet<object> set = new HashSet<object>();
                for (int i = 0; i < m_shapefile.NumShapes; i++)
                {
                    object val = m_shapefile.get_CellValue(index, i);
                    set.Add(val);
                }

                if (set.Count > 300)
                {
                    showWaiting = true;
                    string s = string.Format("The chosen field = {1}.\nThe number of unique values = {0}.\n" +
                                             "Large number of categories negatively affects performance.\nDo you want to continue?", set.Count, "[" + name.ToUpper() + "]");
                    if (MessageBox.Show(s, "MapWindow 4", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }
                set.Clear();
            }

            if (showWaiting)
            {
                this.Enabled = false;
                this.Cursor = Cursors.WaitCursor;
            }
            else
            {
                btnCategoryGenerate.Enabled = false;
            }

            // generating
            categories.Generate(index, classification, count);
            categories.Caption = "Categories: " + m_shapefile.get_Field(index).Name;
            this.ApplyColorScheme2Categories();

            if (chkUseVariableSize.Checked)
            {
                this.ApplyVariablePointSize();
            }

            m_shapefile.Categories.ApplyExpressions();

            // updating labels
            //LabelUtilities.GenerateCategories(m_mapWin, m_layerHandle);

            this.RefreshCategoriesList();
            this.RedrawMap();

            // saving the settings
            m_settings.CategoriesClassification = classification;
            m_settings.CategoriesFieldName = name;
            m_settings.CategoriesSizeRange = (int)(udMaxSize.Value - udMinSize.Value);
            m_settings.CategoriesCount = (int)udNumCategories.Value;
            m_settings.CategoriesRandomColors = chkRandomColors.Checked;
            m_settings.CategoriesUseGradient = chkSetGradient.Checked;
            m_settings.CategoriesVariableSize = chkUseVariableSize.Checked;

            // cleaning
            if (showWaiting)
            {
                this.Enabled = true;
                this.Cursor = Cursors.Default;
            }
            else
            {
                btnCategoryGenerate.Enabled = true;
            }

            this.RefreshControlsState(null, null);
            this.MarkStateChanged();
        }
        #endregion
        
        #region Appearance, remove, clear, visibility
        /// <summary>
        /// Changes the style of the selected category
        /// </summary>
        private void ChangeCategoryStyle()
        {
            if (dgvCategories.CurrentRow != null)
            {
                int row = dgvCategories.CurrentRow.Index;
                if (row == 0)
                {
                    btnDrawingOptions_Click(null, null);
                }
                else
                {
                    ShapefileCategory cat = this.GetCategory(row);
                    if (cat == null) return;

                    Form form = m_plugin.GetSymbologyForm(m_layerHandle, m_shapefile.ShapefileType, cat.DrawingOptions, true);
                    form.Text = "Category drawing options: " + cat.Name;

                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        dgvCategories.Invalidate();
                        this.RedrawMap();
                    }
                    form.Dispose();
                }
            }
        }

        /// <summary>
        /// Gets category be row index. Unclassified values is the first category.
        /// </summary>
        /// <param name="rowIndex"></param>
        private MapWinGIS.ShapefileCategory GetCategory(int rowIndex)
        {
            return rowIndex == 0 ? m_defaultCategory : m_shapefile.Categories.get_Item(rowIndex - 1);
        }

        /// <summary>
        /// Removes selected category
        /// </summary>
        private void RemoveCategory()
        {
            if (dgvCategories.CurrentRow != null)
            {
                try
                {
                    int cmn = dgvCategories.CurrentCell.ColumnIndex;
                    int index = dgvCategories.CurrentRow.Index;

                    int realIndex = (int)dgvCategories[CMN_CATEGORYID, dgvCategories.CurrentRow.Index].Value;
                    if (realIndex == -1)
                        return;

                    m_shapefile.Categories.Remove(realIndex);
                    this.RefreshCategoriesList();

                    if (index >= 0 && index < dgvCategories.Rows.Count)
                    {
                        dgvCategories.CurrentCell = dgvCategories[cmn, index];
                    }
                    else if (dgvCategories.Rows.Count > 0)
                    {
                        dgvCategories.CurrentCell = dgvCategories[cmn, dgvCategories.Rows.Count];
                    }

                    // updating the map
                    m_shapefile.Categories.ApplyExpressions();

                    this.RedrawMap();
                }
                catch (System.Exception)
                {
                }
            }
        }

        /// <summary>
        /// Removes all the categories in the list
        /// </summary>
        private void ClearCategories()
        {
            if (SymbologyPlugin.MessageBoxYesNo("Do you want to remove all the categories?") == DialogResult.Yes)
            {
                m_shapefile.Categories.Clear();
                this.RefreshCategoriesList();

                LegendControl.Layer layer = m_mapWin.View.LegendControl.Layers.ItemByHandle(m_layerHandle);
                m_settings.CategoriesClassification = chkUniqueValues.Checked ? tkClassificationType.ctUniqueValues : tkClassificationType.ctNaturalBreaks;

                this.RedrawMap();
            }
        }

        /// <summary>
        /// Toggles visibility of all categories
        /// </summary>
        private void SetCategoriesVisibility(bool visible)
        {
            for (int i = 0; i < dgvCategories.Rows.Count; i++)
            {
                dgvCategories[CMN_VISIBLE, i].Value = visible;
            }
        }
        #endregion

        #region Fill grid view
        /// <summary>
        /// Fills the data grid view with information about categories
        /// </summary>
        private void RefreshCategoriesList()
        {
            dgvCategories.SuspendLayout();
            dgvCategories.Rows.Clear();

            int numCategories = m_shapefile.Categories.Count;
            if (numCategories == 0)
            {
                dgvCategories.ColumnHeadersVisible = false;
                dgvCategories.ResumeLayout();
                this.RefreshControlsState(null, null);
                return;
            }
            else
            {
                dgvCategories.ColumnHeadersVisible = true;
            }

            dgvCategories.Rows.Add(numCategories + 1);

            bool noEventsState = _noEvents;
            _noEvents = true;

            // calculating the number of shapes per category
            Dictionary<int, int> values = new Dictionary<int, int>();  // id of category, count
            int category;

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

            // unclassified values
            int count = values.ContainsKey(-1) ? values[-1] : 0;
            this.ShowCategory(m_defaultCategory, 0, -1, count);
            //dgvCategories.Rows[0].Frozen = true;

            // categories
            for (int i = 0; i < numCategories; i++)
            {
                MapWinGIS.ShapefileCategory cat = m_shapefile.Categories.get_Item(i);
                count = values.ContainsKey(i) ? values[i] : 0;
                this.ShowCategory(cat, i + 1, i, count);
            }
            dgvCategories.ResumeLayout();

            // autosizing columns
            for (int i = 1; i < dgvCategories.Columns.Count; i++)
            {
                
                if (i != CMN_STYLE && i != CMN_COUNT && i != CMN_NAME)
                {
                    dgvCategories.AutoResizeColumn(i, DataGridViewAutoSizeColumnMode.AllCells);
                    dgvCategories.Columns[i].Width += 10;
                }
            }

            dgvCategories.Columns[CMN_NAME].Width = dgvCategories.Width -
                                                    dgvCategories.Columns[CMN_VISIBLE].Width -
                                                    dgvCategories.Columns[CMN_STYLE].Width -
                                                    dgvCategories.Columns[CMN_COUNT].Width - 26;

            this.RefreshControlsState(null, null);
            _noEvents = noEventsState;
        }

        /// <summary>
        /// Fills information about the category in the given row of data grid view
        /// </summary>
        private void ShowCategory(MapWinGIS.ShapefileCategory cat, int row, int id, int count)
        {
            dgvCategories[CMN_CATEGORYID, row].Value = id;
            dgvCategories[CMN_VISIBLE, row].Value = cat.DrawingOptions.Visible;
            dgvCategories[CMN_STYLE, row].Value = new Bitmap(dgvCategories.Columns[CMN_STYLE].Width - 20, dgvCategories.Rows[row].Height - 8);
            dgvCategories[CMN_NAME, row].Value = cat.Name;
            dgvCategories[CMN_EXPRESSION, row].Value = cat.Expression;
            dgvCategories[CMN_COUNT, row].Value = count ;
        }
        #endregion

        #region Apply color scheme and point size
        /// <summary>
        /// Applies color scheme chosen in the image combo to actegories
        /// </summary>
        private void ApplyColorScheme2Categories()
        {
            if (m_shapefile.Categories.Count > 0)
            {
                MapWinGIS.ColorScheme scheme = null;
                if (icbCategories.SelectedIndex >= 0)
                {
                    ColorBlend blend = (ColorBlend)icbCategories.ColorSchemes.List[icbCategories.SelectedIndex];
                    scheme = ColorSchemes.ColorBlend2ColorScheme(blend);

                    // saving the settings
                    LegendControl.Layer layer = m_mapWin.View.LegendControl.Layers.ItemByHandle(m_layerHandle);
                    m_settings.CategoriesColorScheme = blend;
                }
                else
                    return;

                if (chkRandomColors.Checked)
                {
                    m_shapefile.Categories.ApplyColorScheme(MapWinGIS.tkColorSchemeType.ctSchemeRandom, scheme);
                }
                else
                {
                    m_shapefile.Categories.ApplyColorScheme(MapWinGIS.tkColorSchemeType.ctSchemeGraduated, scheme);
                }

                MapWinGIS.ShapefileCategories categories = m_shapefile.Categories;
                if (chkSetGradient.Checked)
                {
                    for (int i = 0; i < categories.Count; i++)
                    {
                        ShapeDrawingOptions options = categories.get_Item(i).DrawingOptions;
                        options.SetGradientFill(options.FillColor, 75);
                        options.FillType = tkFillType.ftGradient;
                    }
                }
                else
                {
                    for (int i = 0; i < categories.Count; i++)
                    {
                        ShapeDrawingOptions options = categories.get_Item(i).DrawingOptions;
                        options.FillColor2 = options.FillColor;
                        options.FillType = tkFillType.ftStandard;
                    }
                }
            }
        }

        /// <summary>
        /// Sets symbols with variable size for point categories 
        /// </summary>
        private void ApplyVariablePointSize()
        {
            if (chkUseVariableSize.Checked && (udMinSize.Value != udMaxSize.Value))
            {
                MapWinGIS.ShapefileCategories categories = m_shapefile.Categories;
                if (m_shapefile.ShapefileType == ShpfileType.SHP_POINT || m_shapefile.ShapefileType == ShpfileType.SHP_MULTIPOINT)
                {
                    double step = (double)(udMaxSize.Value - udMinSize.Value) / ((double)categories.Count - 1);
                    for (int i = 0; i < categories.Count; i++)
                    {
                        categories.get_Item(i).DrawingOptions.PointSize = (int)udMinSize.Value + Convert.ToInt32(i * step);
                    }
                }
                else if (m_shapefile.ShapefileType == ShpfileType.SHP_POLYLINE)
                {
                    double step = (double)(udMaxSize.Value + udMinSize.Value) / (double)categories.Count;
                    for (int i = 0; i < categories.Count; i++)
                    {
                        categories.get_Item(i).DrawingOptions.LineWidth = (int)udMinSize.Value + Convert.ToInt32(i * step);
                    }
                }
            }
        }
        #endregion

        #region GridView events
       
        /// <summary>
        /// Opening forms for editing the category
        /// </summary>
        private void dgvCategories_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == CMN_STYLE && e.RowIndex >= 0)
            {
                this.ChangeCategoryStyle();
            }
        }

        /// <summary>
        /// Drawing of images in the style column
        /// </summary>
        private void dgvCategories_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex != CMN_STYLE) return;
            if (e.RowIndex >= 0 && e.RowIndex < m_shapefile.Categories.Count + 1)
            {
                System.Drawing.Image img = e.Value as System.Drawing.Image;
                if (img == null) return;

                ShapefileCategory cat = this.GetCategory(e.RowIndex);
                if (cat == null) return;
                ShapeDrawingOptions sdo = cat.DrawingOptions;

                Graphics g = Graphics.FromImage(img);
                g.Clear(Color.White);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;

                if (m_shapefile.ShapefileType == ShpfileType.SHP_POLYGON)
                {
                    sdo.DrawRectangle(g.GetHdc(), 0, 0, img.Width - 1, img.Height - 1, true, img.Width, img.Height, MapWinUtility.Colors.ColorToUInteger(dgvCategories.BackgroundColor));
                }
                else if (m_shapefile.ShapefileType == ShpfileType.SHP_POLYLINE)
                {
                    sdo.DrawLine(g.GetHdc(), 0, 0, img.Width - 1, img.Height - 1, true, img.Width, img.Height, MapWinUtility.Colors.ColorToUInteger(dgvCategories.BackgroundColor));
                }
                else if (m_shapefile.ShapefileType == ShpfileType.SHP_POINT)
                {
                    sdo.DrawPoint(g.GetHdc(), 0.0f, 0.0f, img.Width, img.Height, MapWinUtility.Colors.ColorToUInteger(dgvCategories.BackgroundColor));
                }

                g.ReleaseHdc();
                g.Dispose();
            }
        }

        /// <summary>
        /// Drawing the focus rectangle
        /// </summary>
        private void dgvCategories_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (this.dgvCategories.CurrentCell == null) return;
            if (e.ColumnIndex == this.dgvCategories.CurrentCell.ColumnIndex && e.RowIndex == this.dgvCategories.CurrentCell.RowIndex)
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

        /// <summary>
        /// Toggles visibility of the categories
        /// </summary>
        private void dgvCategories_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1 || _noEvents) return;

            if (e.ColumnIndex == CMN_VISIBLE)
            {
                //m_shapefile.Categories.get_Item(index).DrawingOptions.Visible = (bool)dgvCategories[e.ColumnIndex, e.RowIndex].Value;
                MapWinGIS.ShapefileCategory category = this.GetCategory(e.RowIndex);
                category.DrawingOptions.Visible = (bool)dgvCategories[e.ColumnIndex, e.RowIndex].Value;

                // toggle labels in case they are present
                int index = (int)dgvCategories[CMN_CATEGORYID, e.RowIndex].Value;
                if (index > 0)
                {
                    MapWinGIS.LabelCategory cat = m_shapefile.Labels.get_Category(index);
                    if (cat != null && cat.Enabled)
                    {
                        cat.Visible = (bool)dgvCategories[e.ColumnIndex, e.RowIndex].Value;
                    }
                }
                this.RedrawMap();
            }
        }

        /// <summary>
        /// Committing changes of the checkbox state immediately, CellValueChanged event won't be triggered otherwise
        /// </summary>
        private void dgvCategories_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvCategories.CurrentCell.ColumnIndex == CMN_VISIBLE)
            {
                if (dgvCategories.IsCurrentCellDirty)
                {
                    dgvCategories.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
        }

        /// <summary>
        /// Bans editing of the count column
        /// </summary>
        private void dgvCategories_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == CMN_COUNT)
                e.Cancel = true;

            if (e.RowIndex == 0 && e.ColumnIndex != CMN_VISIBLE)
                e.Cancel = true;    // no editing for unclassified values

            if (e.ColumnIndex != CMN_VISIBLE)
                m_cellEditing = true;
        }

        /// <summary>
        /// Saves editing of the category names
        /// </summary>
        private void dgvCategories_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == CMN_NAME)
            {
                if (e.RowIndex > 0)
                {
                    m_shapefile.Categories.get_Item(e.RowIndex - 1).Name = dgvCategories[CMN_NAME, e.RowIndex].Value.ToString();
                    RedrawLegend();
                    this.MarkStateChanged();
                }
            }

            m_cellEditing = false;
        }
        #endregion

        #region Fields list
        /// <summary>
        /// Fills the list of fields
        /// </summary>
        private void FillFieldList(string name)
        {
            // we need to preserve currently selected field
            if (name == string.Empty && lstFields1.SelectedItem != null)
                name = lstFields1.SelectedItem.ToString().Trim();

            lstFields1.Items.Clear();

            // adding names
            for (int i = 0; i < m_shapefile.NumFields; i++)
            {
                Field field = m_shapefile.get_Field(i);
                if (!chkUniqueValues.Checked && field.Type == FieldType.STRING_FIELD)
                    continue;
                lstFields1.Items.Add("  " + m_shapefile.get_Field(i).Name);
            }

            // setting the selected field back
            if (name != string.Empty)
            {
                for (int i = 0; i < lstFields1.Items.Count; i++)
                {
                    if (lstFields1.Items[i].ToString().ToLower().Trim() == name.ToLower())
                    {
                        lstFields1.SelectedIndex = i;
                        break;
                    }
                }
            }

            if (lstFields1.SelectedItem == null && lstFields1.Items.Count > 0)
            {
                lstFields1.SelectedIndex = 0;
            }
            this.RefreshControlsState(null, null);
        }
        #endregion

        /// <summary>
        /// Sets the changed flag to the layer state
        /// </summary>
        private void MarkStateChanged()
        {
            m_stateChanged = true;
            btnSaveChanges.Enabled = true;
        }
    }
}
