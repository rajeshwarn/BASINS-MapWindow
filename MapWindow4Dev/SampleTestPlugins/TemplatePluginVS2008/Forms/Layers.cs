// ********************************************************************************************************
// <copyright file="Layers.cs" company="Bontepaarden.nl">
//     Copyright (c) Bontepaarden.nl. All rights reserved.
// </copyright>
// ********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http:// www.mozilla.org/MPL/ 
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
// 
// The Original Code is MapWindow Open Source MeemsTools Plug-in. 
// 
// The Initial Developer of this version of the Original Code is Paul Meems.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// Change Log: 
// Date           Changed By      Notes
// 13 April 2008  Paul Meems      Inital upload the MW SVN repository
// 28 August 2009 Paul Meems      Made the code StyleCop compliant
// 5  May 2011    Paul Meems      Made changes as suggested by ReSharper   
// ********************************************************************************************************

namespace TemplatePluginVS2008.Forms
{
    #region Usings

    using System;
    using System.Reflection;
    using System.Windows.Forms;
    using Classes;
    using MapWindow.Interfaces;

    #endregion
    
    /// <summary>
    ///   This class holds all code used in this form
    /// </summary>
    public partial class Layers : Form
    {
        /// <summary>
        ///   The reference to the MapWindow map object
        /// </summary>
        private readonly IMapWin mapWin;

        /// <summary>
        ///   The property to expose PluginClass
        /// </summary>
        private readonly PluginCode pluginClass;

        /// <summary>
        ///   Initializes a new instance of the Layers class.
        /// </summary>
        /// <param name = "mapWin">The reference to the MapWindow map object</param>
        public Layers(IMapWin mapWin)
        {
            this.InitializeComponent();
            this.mapWin = mapWin;

            // Set versionnr in statuslabel:
            this.VersionLabel.Text = "v" + Assembly.GetExecutingAssembly().GetName().Version;

            // Set Copyright in statuslabel:
            this.CopyrightLabel.Text = AssemblyInformation.ProductCopyright();

            // Fill combobox:  
            this.pluginClass = new PluginCode(this.mapWin);
            this.pluginClass.FillVisibleLayers(this);
        }

        /// <summary>
        ///   To access the PluginCode class
        /// </summary>
        /// <returns>The reference to a local PluginCode class</returns>
        public PluginCode PluginClass()
        {
            return this.pluginClass;
        }

        /// <summary>
        ///   Index changed event of cboLayers
        /// </summary>
        /// <param name = "sender">Sender (object)</param>
        /// <param name = "e">Event arguments</param>
        private void CboLayersSelectedIndexChanged(object sender, EventArgs e)
        {
            // Get data from combobox
            // C#3.0: Implicitly Typed Local Variables and Arrays:
            var selectedLayerItem = (Layer)this.cboLayers.SelectedItem;
            if (selectedLayerItem == null)
            {
                return;
            }

            this.lblHandle.Text = "Handle: " + selectedLayerItem.Handle;
            this.lblName.Text = "Name: " + selectedLayerItem.Name;
            this.lblType.Text = "Type: " + selectedLayerItem.LayerType;
        }
    }
}