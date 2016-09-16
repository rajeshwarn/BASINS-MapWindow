// ********************************************************************************************************
// <copyright file="TopPanel.cs" company="Bontepaarden.nl">
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
//  Change Log: 
//  Date           Changed By      Notes
//  28 August 2009 Paul Meems      Inital upload the MW SVN repository
//  5  May 2011    Paul Meems      Made changes as suggested by ReSharper   
// ********************************************************************************************************

namespace TemplatePluginVS2008.Forms
{
    #region Usings

    using System;
    using System.Windows.Forms;
    using MapWindow.Interfaces;

    #endregion
    
    /// <summary>
    ///   This class holds all code used in this form
    /// </summary>
    public partial class TopPanel : Form
    {
        /// <summary>
        ///   The reference to the MapWindow map object
        /// </summary>
        private readonly IMapWin mapWin;

        /// <summary>
        ///   The reference to the panel object
        /// </summary>
        private readonly Panel topUIPanel;

        /// <summary>
        ///   Initializes a new instance of the TopPanel class
        /// </summary>
        /// <param name = "mapWin">The reference to the MapWindow map object.</param>
        /// <param name = "parentHandle">The window handle of the main MapWindow form.</param>
        public TopPanel(IMapWin mapWin, int parentHandle)
        {
            this.InitializeComponent();
            this.mapWin = mapWin;

            var tempPtr = (IntPtr)parentHandle;
            var mapFrm = (Form)FromHandle(tempPtr);

            mapFrm.AddOwnedForm(this);

            // The trick of creating dockable panels is to create a standard form with a panel,
            // this panel is used to create the dockable panel.

            // Create new mapwindow panel
            this.topUIPanel = this.mapWin.UIPanel.CreatePanel(this.Name, MapWindowDockStyle.Top);

            // Add panel to the mapwindowpanel
            this.topUIPanel.Controls.Add(this.panel1);

            // Set autosize to false
            this.topUIPanel.AutoSize = false;

            // Set the height of the panel to the height of the form
            // If you don't dock it to Top you might need some other values
            this.topUIPanel.Height = this.Height;
        }

        /// <summary>
        ///   Click event of the button
        /// </summary>
        /// <param name = "sender">The sender</param>
        /// <param name = "e">Event arguments</param>
        private void Button1Click(object sender, EventArgs e)
        {
            var frmProgress = new ProgressForm();
            frmProgress.Show(this);
            this.button1.Enabled = false;
        }
    }
}