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
    
    public partial class frmAddCategories : Form
    {
        SymbologyPlugin _plugin = null;
        
        public frmAddCategories(SymbologyPlugin plugin, bool labels)
        {
            InitializeComponent();

            _plugin = plugin;
            icbColors.ColorSchemes = _plugin.LayerColors;

            icbColors.ComboStyle = ImageComboStyle.ColorSchemeGraduated;
            if (icbColors.Items.Count >= 0)
            {
                icbColors.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Toggles between graduated and random color scheme
        /// </summary>
        private void chkRandom_CheckedChanged(object sender, EventArgs e)
        {
            int index = icbColors.SelectedIndex;
            if (chkRandom.Checked)
            {
                icbColors.ComboStyle = ImageComboStyle.ColorSchemeRandom;
            }
            else
            {
                icbColors.ComboStyle = ImageComboStyle.ColorSchemeGraduated;
            }

            if (index >= 0 && index < icbColors.Items.Count)
            {
                icbColors.SelectedIndex = index;
            }
        }
    }
}
