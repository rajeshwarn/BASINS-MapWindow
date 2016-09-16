// ********************************************************************************************************
// <copyright file="ProgressForm.cs" company="Bontepaarden.nl">
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
// The Original Code is this.mapWindow Open Source MeemsTools Plug-in. 
// 
// The Initial Developer of this version of the Original Code is Paul Meems.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// Change Log: 
// Date           Changed By      Notes
// 11 Sept 2008   Paul Meems      Inital upload the MW SVN repository (many thanks to Jeen de Vegt for providing this thread form sample)
// 5  May 2011    Paul Meems      Made changes as suggested by ReSharper   
// ********************************************************************************************************

namespace TemplatePluginVS2008.Forms
{
    #region Usings

    using System;
    using System.Threading;
    using System.Windows.Forms;
    using TemplatePluginVS2008.Classes;

    #endregion
    
    /// <summary>
    ///   This class holds all code used in this form
    /// </summary>
    public partial class ProgressForm : Form
    {
        /// <summary>
        ///   The thread that will be used
        /// </summary>
        private Thread theThread;

        /// <summary>
        ///   Initializes a new instance of the ProgressForm class.
        /// </summary>
        public ProgressForm()
        {
            this.InitializeComponent();
        }

        /// <summary>
        ///   Button click event
        /// </summary>
        /// <param name = "sender">The sender</param>
        /// <param name = "e">Event arguments</param>
        private void StopThreadButtonClick(object sender, EventArgs e)
        {
            this.theThread.Abort();
            WorkWithThread.Cleanup();
            this.Close();
        }

        /// <summary>
        ///   Form load event
        /// </summary>
        /// <param name = "sender">The sender</param>
        /// <param name = "e">Event arguments</param>
        private void ProgressFormLoad(object sender, EventArgs e)
        {
            this.theThread = new Thread(WorkWithThread.DoWork);
            this.theThread.Start(this);
        }

        /// <summary>
        ///   Form closing event
        /// </summary>
        /// <param name = "sender">The sender</param>
        /// <param name = "e">Event arguments</param>
        private void ProgressFormFormClosing(object sender, FormClosingEventArgs e)
        {
            var owner = (TopPanel)this.Owner;
            owner.button1.Enabled = true;
        }
    }
}