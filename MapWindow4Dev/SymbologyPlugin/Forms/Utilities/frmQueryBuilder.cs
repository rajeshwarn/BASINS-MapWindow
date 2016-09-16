﻿// ********************************************************************************************************
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
    using MapWindow.Interfaces;
    using System.Collections;
    using mwSymbology.Classes;
    
    public partial class frmQueryBuilder : Form
    {
        MapWinGIS.Shapefile _shapefile = null;
        int _layerHandle = -1;
        bool _selectionMode = false;
        bool _noEvents = false;
        IMapWin _mapWin = null;
        const int CMN_ICON = 0;
        const int CMN_NAME = 1;

        /// <summary>
        /// Creates a new instance of frmQueryBuilder class
        /// </summary>
        public frmQueryBuilder(MapWinGIS.Shapefile sf, int layerHandle, string expression, bool SelectionMode, IMapWin mapWin)
        {
            InitializeComponent();

            if (sf == null)
                return;

            _shapefile = sf;
            _selectionMode = SelectionMode;
            _mapWin = mapWin;
            _layerHandle = layerHandle;

            btnTest.Text = SelectionMode ? "Select" : "Test";

            dgvField.Rows.Clear();
            dgvField.Rows.Add(_shapefile.NumFields);
            
            for (int i = 0; i < _shapefile.NumFields; i++)
            {
                dgvField[CMN_NAME, i].Value = _shapefile.get_Field(i).Name;
                if (_shapefile.get_Field(i).Type == MapWinGIS.FieldType.STRING_FIELD)
                {
                    dgvField[CMN_ICON, i].Value = "Aa";
                    dgvField[CMN_ICON, i].Style.ForeColor = Color.Maroon;
                }
                else
                {
                    dgvField[CMN_ICON, i].Value = "09";
                    //dgvField[CMN_ICON, i].Style.ForeColor = Color.Blue;
                }
            }

            if (dgvField.Rows.Count > 0)
            {
                // TODO: show unique values    
            }
            richTextBox1.Text = expression;
            richTextBox1.HideSelection = false;
            richTextBox1.SelectAll();
            richTextBox1.Focus();

            if (chkShowValues.Checked)
            {
                ShowValues(0);
            }

            _noEvents = true;

            // restoring values
            SymbologySettings settings = SymbologyPlugin.get_LayerSettings(_layerHandle, mapWin);
            chkShowValues.Checked = settings.ShowQueryValues;
            chkShowDynamically.Checked = settings.ShowQueryOnMap;
            
            _noEvents = false;
        }

        /// <summary>
        /// Builds a list of unique values
        /// </summary>
        private void dgvField_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            if (chkShowValues.Checked)
            {
                ShowValues(e.RowIndex);
            }
        }

        /// <summary>
        /// Showing values
        /// </summary>
        private void ShowValues(int FieldIndex)
        {
            _noEvents = true;
            dgvValues.Rows.Clear();

            if (_shapefile.NumFields - 1 < FieldIndex)
            {
                _noEvents = false;
                return;
            }

            MapWinGIS.Table tbl = _shapefile.Table;
            object obj = null;
            SortedDictionary<object, int> hashTable = new SortedDictionary<object, int>();

            bool isString = (_shapefile.get_Field(FieldIndex).Type == MapWinGIS.FieldType.STRING_FIELD);

            if (true)
            {
                this.Cursor = Cursors.WaitCursor;
                
                for (int i = 0; i < tbl.NumRows; i++)
                {
                    obj = tbl.get_CellValue(FieldIndex, i);
                    if (hashTable.ContainsKey(obj))
                    {
                        hashTable[obj] += 1;
                    }
                    else
                    {
                        hashTable.Add(obj, 1);
                    }
                }
                int[] values = hashTable.Values.ToArray();
                object[] keys = hashTable.Keys.ToArray();

                dgvValues.Rows.Add(values.Length);
                for (int i = 0; i < values.Length; i++)
                {
                    if (isString)
                    {
                        dgvValues[1, i].Value = "\"" + keys[i].ToString() + "\"";
                    }
                    else
                    {
                        dgvValues[1, i].Value = keys[i].ToString();
                    }
                    dgvValues[0, i].Value = values[i];
                }

                this.Cursor = Cursors.Default;
            }
            else
            
            // field stats: aren't used currently
            {
                // for numeric fields we shall provide statistics
                dgvValues.Rows.Add(7);
                dgvValues[0, 0].Value = "Avg";
                dgvValues[0, 1].Value = "StDev";
                dgvValues[0, 2].Value = "0%";
                dgvValues[0, 3].Value = "25%";
                dgvValues[0, 4].Value = "50%";
                dgvValues[0, 5].Value = "75%";
                dgvValues[0, 6].Value = "100%";

                List<object> list = new List<object>();
                for (int i = 0; i < tbl.NumRows; i++)
                {
                    list.Add(tbl.get_CellValue(FieldIndex, i));
                }
                list.Sort();

                int quater = list.Count / 4;
                for (int i = 0; i < list.Count; i++)
                {
                    if (i == quater)
                    {
                        dgvValues[1, 3].Value = list[i];
                    }
                    else if (i == quater * 2)
                    {
                        dgvValues[1, 4].Value = list[i];
                    }
                    else if (i == quater * 3)
                    {
                        dgvValues[1, 5].Value = list[i];
                    }
                }

                dgvValues[1, 0].Value = (float)tbl.get_MeanValue(FieldIndex);
                dgvValues[1, 1].Value = (float)tbl.get_StandardDeviation(FieldIndex);
                dgvValues[1, 2].Value = tbl.get_MinValue(FieldIndex);
                dgvValues[1, 6].Value = tbl.get_MaxValue(FieldIndex);

            }

            dgvValues.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            _noEvents = false;
        }

        /// <summary>
        /// Tests the expression typed by user, showing syntax errors
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs e)
        {
            MapWinGIS.Table tbl = _shapefile.Table;
            if (richTextBox1.Text == string.Empty)
            {
                MessageBox.Show("No expression is entered", "MapWindow 4",  MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                object result = null;
                string err = string.Empty;

                //if (tbl.ParseExpression(richTextBox1.Text, ref err))
                if (tbl.Query(richTextBox1.Text, ref result, ref err))
                {
                    lblResult.ForeColor = Color.Green;
                    int[] arr =  result as int[];
                    if (arr != null)
                    {
                        lblResult.Text = "Number of shapes = " + arr.Length.ToString();

                        // updating shapefile selection
                        if (_selectionMode)
                        {
                            ArrayList options = new ArrayList();
                            options.Add("1 - New selection");
                            options.Add("2 - Add to selection");
                            options.Add("3 - Exclude from selection");
                            options.Add("4 - Invert in selection");
                            string s = string.Format("Number of shapes = {0}. Choose the way to update selection", arr.Length);
                            int option = MapWindow.Controls.Dialogs.ChooseOptions(options, 0, s, "Update selection");
                            
                            // updating selection
                            if (option != -1)
                            {
                                _mapWin.View.UpdateSelection(_layerHandle, ref arr, (SelectionOperation)option);
                                _mapWin.View.Redraw();
                            }
                        }
                    }
                }
                else
                {
                    if (err.ToLower() == "selection is empty")
                    {
                        lblResult.ForeColor = Color.Blue;
                        lblResult.Text = err;
                    }
                    else
                    {
                        lblResult.ForeColor = Color.Red;
                        lblResult.Text = err;
                    }
                }
            }
        }

        /// <summary>
        /// Updating selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Tag = richTextBox1.Text;
            SymbologySettings settings = SymbologyPlugin.get_LayerSettings(_layerHandle, _mapWin);
            settings.ShowQueryValues = chkShowValues.Checked;
            settings.ShowQueryOnMap = chkShowDynamically.Checked;
            SymbologyPlugin.SaveLayerSettings(_layerHandle, settings, _mapWin);
        }

        // Adding field to the text control
        private void dgvField_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            richTextBox1.SelectedText = "[" + dgvField[CMN_NAME, e.RowIndex].Value.ToString() + "] ";
        }

        private void dgvValues_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            richTextBox1.SelectedText = dgvValues[1, e.RowIndex].Value.ToString() + " ";
        }

        
        /// <summary>
        /// Adding operators. The text on the buttons is used
        /// </summary>
        private void button0_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
                richTextBox1.SelectedText = btn.Text + " ";
        }

        /// <summary>
        /// Warning while turning on the mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkShowDynamically_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkShowDynamically.Checked)
            //{
            //    MessageBox.Show("Dynamic selection mode was turned on.\nThe selection will be changed after each click\n on the value to show corresponding objects.", 
            //                    "MapWindow 4", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
        }

        /// <summary>
        /// Changing selection while in dynamic mode
        /// </summary>
        private void dgvValues_SelectionChanged(object sender, EventArgs e)
        {
            if (_noEvents)
            {
                return;
            }

            if (chkShowDynamically.Checked)
            {
                if (dgvField.SelectedRows.Count != 0 && dgvValues.SelectedRows.Count != 0)
                {
                    string expr = "[" + dgvField[CMN_NAME, dgvField.SelectedRows[0].Index].Value.ToString() + "] = ";
                    expr += dgvValues[1, dgvValues.SelectedRows[0].Index].Value.ToString();
                    
                    object result = null;
                    string errorMessage = string.Empty;
                    if (_shapefile.Table.Query(expr, ref result, ref errorMessage))
                    {
                        int[] arr = result as int[];
                        _mapWin.View.UpdateSelection(_layerHandle, ref arr, SelectionOperation.SelectNew);
                        _mapWin.View.Redraw();
                    }
                }
            }
        }

        /// <summary>
        /// Clear the expression
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        // Shows the list of unique values
        private void btnShowValues_Click(object sender, EventArgs e)
        {
            if (dgvField.CurrentCell != null)
            {
                ShowValues(dgvField.CurrentCell.RowIndex);
            }
        }

        /// <summary>
        /// Toggles between automatic and manual showing of the unique values
        /// </summary>
        private void chkShowValues_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowValues.Checked)
            {
                if (dgvField.CurrentCell != null)
                {
                    ShowValues(dgvField.CurrentCell.RowIndex);
                }
            }
        }
    }
}
