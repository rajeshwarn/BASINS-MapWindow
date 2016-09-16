//********************************************************************************************************
//File Name: frmLayers.cs
//Description: This class holds all code used in this form
//********************************************************************************************************
//The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
//you may not use this file except in compliance with the License. You may obtain a copy of the License at 
//http://www.mozilla.org/MPL/ 
//Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
//ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
//limitations under the License. 
//
//The Original Code is MapWindow Open Source MeemsTools Plug-in. 
//
//The Initial Developer of this version of the Original Code is Paul Meems.
//
//Contributor(s): (Open source contributors should list themselves and their modifications here). 
// Change Log: 
// Date          Changed By      Notes
// 8 April 2008  Paul Meems      Inital upload the MW SVN repository
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MapWindow.Interfaces;

namespace TemplatePluginVS2005.Forms
{
    public partial class frmLayers : Form
    {
        private MapWindow.Interfaces.IMapWin _mapWin;

        public frmLayers(MapWindow.Interfaces.IMapWin IMapWin)
        {
            InitializeComponent();
            _mapWin = IMapWin;
            //Set versionnr in statuslabel:
            VersionLabel.Text = "v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //Set Copyright in statuslabel:
            CopyrightLabel.Text = TemplatePluginVS2005.Classes.AssemblyInformation.ProductCopyright();
            //Fill combobox:  
            TemplatePluginVS2005.Classes.pluginCode cls = new TemplatePluginVS2005.Classes.pluginCode(_mapWin);
            cls.fillVisibleLayers(this);
        }

        private void cboLayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Get data from combobox:
            TemplatePluginVS2005.Classes.MyLayersList selectedLayerItem = (TemplatePluginVS2005.Classes.MyLayersList)cboLayers.SelectedItem;
            lblHandle.Text  = "Handle: "    + selectedLayerItem.LayerHandle.ToString();
            lblName.Text    = "Name: "      + selectedLayerItem.Name.ToString();
            lblType.Text    = "Type: "      + selectedLayerItem.LayerType.ToString();
        }
    }
}