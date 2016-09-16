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
        /// Building layer visibility expression
        /// </summary>
        private void btnLayerExpression_Click(object sender, EventArgs e)
        {
            string s = txtLayerExpression.Text;
            frmQueryBuilder form = new frmQueryBuilder(m_shapefile, m_layerHandle, s, false, m_mapWin);
            if (form.ShowDialog() == DialogResult.OK)
            {
                txtLayerExpression.Text = form.Tag.ToString();
                m_shapefile.VisibilityExpression = txtLayerExpression.Text;
                RedrawMap();
            }
            form.Dispose();
        }

        /// <summary>
        /// Clears the layer expression
        /// </summary>
        private void btnClearLayerExpression_Click(object sender, EventArgs e)
        {
            txtLayerExpression.Clear();
            m_shapefile.VisibilityExpression = "";
            RedrawMap();
        }

        

        
    }
}
