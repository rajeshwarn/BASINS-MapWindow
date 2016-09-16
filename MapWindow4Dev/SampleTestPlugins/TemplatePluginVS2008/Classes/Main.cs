// ********************************************************************************************************
// <copyright file="Main.cs" company="Bontepaarden.nl">
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
// 13 April 2008  Paul Meems      Inital upload the MW SVN repository
// 28 August 2009 Paul Meems      Made the code StyleCop compliant and implemented IDisposable
// 22 Febr 2010   Paul Meems      Removed IDisposable again and made some minor changes
// 5  May 2011    Paul Meems      Made changes as suggested by ReSharper   
// ********************************************************************************************************

using System.Collections.Generic;

namespace TemplatePluginVS2008.Classes
{
    #region Usings

    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;
    using Forms;
    using MapWindow.Interfaces;

    #endregion 
   
    /// <summary>
    ///   This is the start of the plug-in. In here all methods of the IPlugin interface are implemented.
    /// </summary>
    public class TemplatePluginVs2008Plugin : IPlugin
    {
        #region private fields

        /// <summary>
        ///   The name of your button. This need to be a constant
        /// </summary>
        private const string BtnNameLoadData = "TemplatePluginVS2008Plugin";

        /// <summary>
        ///   The name of your top menu. This need to be a constant
        /// </summary>
        private const string MnuTopName = "TemplateTopMenu";

        /// <summary>
        ///   The name of your sub menu. This need to be a constant
        /// </summary>
        private const string MnuTools = "TemplateSubMenu";

        /// <summary>
        ///   Change this to your own address
        /// </summary>
        private const string ReportEmailaddress = "bontepaarden@gmail.com";

        /// <summary>
        ///   The tooltip of your button
        /// </summary>
        private readonly string toolTipLoadData = AssemblyInformation.ProductDescription();

        /// <summary>
        ///   The name of the toolbar
        /// </summary>
        private readonly string toolbarName = AssemblyInformation.ProductCompany();

        /// <summary>
        ///   The form with the layers
        /// </summary>
        private Forms.Layers frmLayers;

        /// <summary>
        ///   The panel with the buttons
        /// </summary>
        private TopPanel frmPanel;

        /// <summary>
        ///   The reference to the MapWindow map object
        /// </summary>
        private IMapWin mapWin;

        /// <summary>
        ///   Used to not let the forms fall behind MapWindow
        /// </summary>
        private int parentHandle;

        /// <summary>
        ///   Used to check if this plug-in should react on GUI
        /// </summary>
        private bool userActivatedPlugin;

        private System.Collections.Generic.Stack<string> addedMenus = new Stack<string>();
        private System.Collections.Generic.Stack<string> addedToolbuttons = new Stack<string>();
        #endregion

        #region Assembly stuff

        /// <summary>
        ///   Gets the date that the plugin was built.
        /// </summary>
        public string BuildDate
        {
            get { return File.GetCreationTime(Assembly.GetExecutingAssembly().Location).ToString(); }
        }

        /// <summary>
        ///   Gets the description of the plugin.
        /// </summary>
        public string Description
        {
            get { return AssemblyInformation.ProductDescription(); }
        }

        /// <summary>
        ///   Gets the author of the plugin.
        /// </summary>
        public string Author
        {
            get { return AssemblyInformation.ProductCompany(); }
        }

        /// <summary>
        ///   Gets the name of the plugin.
        /// </summary>
        public string Name
        {
            get { return AssemblyInformation.ProductTitle(); }
        }

        /// <summary>
        ///   Gets the version of the plugin.
        /// </summary>
        public string Version
        {
            get
            {
                // C#3.0: Implicitly Typed Local Variables and Arrays
                var f = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
                return f.FileMajorPart + "." + f.FileMinorPart + "." + f.FileBuildPart;
            }
        }

        /// <summary>
        ///   Gets the serial number of the plugin.
        ///   The serial number and the name are tied together.  For each name there is a corresponding serial number.
        /// </summary>
        public string SerialNumber
        {
            get { return "This is Open Source"; }
        }

        #endregion

        #region Implementation of IPlugin

        /// <summary>
        ///   This event is called when a plugin is loaded or turned on in the MapWindow.
        /// </summary>
        /// <param name = "myMapWin">The interface to use to access the MapWindow.</param>
        /// <param name = "myParentHandle">The window handle of the main MapWindow form. This handle is useful for making the this.mapWindow the owner of plugin forms.</param>
        public void Initialize(IMapWin myMapWin, int myParentHandle)
        {
            try
            {
                this.mapWin = myMapWin;
                this.parentHandle = myParentHandle; // Needed to not let the forms fall behind MapWindow
                // Resources:
                using (var imageResources = new Resources())
                {
                    this.CreateToolbar(imageResources);

                    this.CreateMenu(imageResources);
                }
            }
            catch (Exception ex)
            {
                this.mapWin.ShowErrorDialog(ex, ReportEmailaddress);
            }
        }

        /// <summary>
        ///   This method is called when a plugin is unloaded.  The plugin should remove all toolbars, buttons and menus that it added.
        /// </summary>
        public void Terminate()
        {
            #region close forms
            // Close form if it is open:
            if (this.frmLayers != null)
            {
                if (!this.frmLayers.IsDisposed)
                {
                    this.frmLayers.Close();
                }
            }

            // Close form if it is open:
            if (this.frmPanel != null)
            {
                if (!this.frmPanel.IsDisposed)
                {
                    this.frmPanel.Close();
                }
            }
            #endregion

            #region Remove buttons
            // Unloads the buttons in reverse order
            while (this.addedToolbuttons.Count > 0)
            {
                this.mapWin.Toolbar.RemoveButton(this.addedToolbuttons.Pop());
            }

            // If all buttons are removed, remove toolbar as well:
            if (this.mapWin.Toolbar.NumToolbarButtons(this.toolbarName) == 0)
            {
                this.mapWin.Toolbar.RemoveToolbar(this.toolbarName);
            }
            #endregion

            #region Remove menu:
            // Unloads the menus in reverse order
            while (this.addedMenus.Count > 0)
            {
                this.mapWin.Menus.Remove(this.addedMenus.Pop());
            }
            #endregion
        }

        /// <summary>
        ///   Called when the this.mapWindow loads a new project.
        /// </summary>
        /// <param name = "projectFile">The filename of the project file being loaded.</param>
        /// <param name = "settingsString">The settings string that was saved in the project file for this plugin.</param>
        public void ProjectLoading(string projectFile, string settingsString)
        {
        }

        /// <summary>
        ///   ProjectSaving is called when the this.mapWindow saves a project.  This is a good chance for the 
        ///   plugin to save any custom settings and create a SettingsString to place in the project file.
        /// </summary>
        /// <param name = "projectFile">The name of the project file being saved.</param>
        /// <param name = "settingsString">Reference parameter.  The settings string that will be saved in the project file for this plugin.</param>
        public void ProjectSaving(string projectFile, ref string settingsString)
        {
        }

        /// <summary>
        ///   Method used by plugins to communicate with other plugins.
        /// </summary>
        /// <param name = "msg">The messsage being recieved.</param>
        /// <param name = "handled">Reference parameter.  Set thist to true if this plugin handles recieving the message.  When set to true, no other plugins will receive the message.</param>
        public void Message(string msg, ref bool handled)
        {
            if (!this.userActivatedPlugin || !msg.ToUpper().Contains("LAYERVISIBLECHANGED"))
            {
                return;
            }

            // Re-initialize form:
            if (this.frmLayers == null)
            {
                return;
            }

            var pluginClass = this.frmLayers.PluginClass();
            pluginClass.FillVisibleLayers(this.frmLayers);
        }

        /// <summary>
        ///   Method that is called when the this.mapWindow extents change.
        /// </summary>
        public void MapExtentsChanged()
        {
        }

        /// <summary>
        ///   Occurs when a user clicks on a toolbar button or menu item.
        /// </summary>
        /// <param name = "itemName">The name of the item clicked on.</param>
        /// <param name = "handled">Reference parameter.  Setting Handled to true prevents other plugins from receiving this event.</param>
        public void ItemClicked(string itemName, ref bool handled)
        {
            try
            {
                switch (itemName)
                {
                    case BtnNameLoadData:
                        this.userActivatedPlugin = true;
                        handled = true;

                        // Open the form:                        
                        this.frmLayers = new Forms.Layers(this.mapWin);

                        // Show it on top of this.mapWindow
                        this.frmLayers.Show(Control.FromHandle(new IntPtr(this.parentHandle)));

                        break;
                    case MnuTools:
                        this.userActivatedPlugin = true;
                        handled = true;

                        // Open the panel:                        
                        this.frmPanel = new TopPanel(this.mapWin, this.parentHandle);

                        break;

                    default:
                        if (this.frmLayers == null)
                        {
                            // Some other button was clicked and the form was closed
                            this.userActivatedPlugin = false;
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                this.mapWin.ShowErrorDialog(ex);
            }
        }

        /// <summary>
        ///   Called when 1 or more layers are added to the MapWindow.
        /// </summary>
        /// <param name = "layers">Array of layer objects containing references to all layers added.</param>
        public void LayersAdded(Layer[] layers)
        {
        }

        /// <summary>
        ///   Called when a layer is selected or made to be the active layer.
        /// </summary>
        /// <param name = "handle">The layer handle of the newly selected layer.</param>
        public void LayerSelected(int handle)
        {
        }

        /// <summary>
        ///   Occurs after a user selects a rectangular area in the MapWindow.  Normally this implies 
        ///   selection.
        /// </summary>
        /// <param name = "bounds">The rectangle selected.</param>
        /// <param name = "handled">Reference parameter. Setting Handled to true prevents other plugins from receiving this event.</param>
        public void MapDragFinished(Rectangle bounds, ref bool handled)
        {
        }

        /// <summary>
        ///   Occurs when a user releases a mouse button on the this.mapWindow main map display.
        /// </summary>
        /// <param name = "button">An integer representation of which button(s) were released.  Uses vb6 button constants.</param>
        /// <param name = "shift">An integer representation of the shift/alt/ctrl keys that were pressed at the time the mouse button was released.  Uses vb6 shift constants.</param>
        /// <param name = "x">X coordinate in pixels.</param>
        /// <param name = "y">Y coordinate in pixels.</param>
        /// <param name = "handled">Reference parameter.  Prevents other plugins from getting this event.</param>
        public void MapMouseUp(int button, int shift, int x, int y, ref bool handled)
        {
        }

        /// <summary>
        ///   Occurs when the user presses a mouse button on the this.mapWindow map display.
        /// </summary>
        /// <param name = "button">The integer representation of the button pressed by the user.  This parameter uses the vb6 mouse button constants (vbLeftButton, etc.).</param>
        /// <param name = "shift">The integer representation of which shift/alt/control keys are pressed. This parameter uses the vb6 shift constants.</param>
        /// <param name = "x">X coordinate in pixels.</param>
        /// <param name = "y">Y coordinate in pixels.</param>
        /// <param name = "handled">Reference parameter.  When set to true, no other plugins will receive this event.</param>
        public void MapMouseDown(int button, int shift, int x, int y, ref bool handled)
        {
        }

        /// <summary>
        ///   Occurs when a user double clicks on the legend.
        /// </summary>
        /// <param name = "handle">The handle of the legend group or item that was clicked on.</param>
        /// <param name = "location">Enumerated.  The location clicked on.</param>
        /// <param name = "handled">Reference parameter.  When set to true it prevents additional plugins from getting this event.</param>
        public void LegendDoubleClick(int handle, ClickLocation location, ref bool handled)
        {
        }

        /// <summary>
        ///   Occurs when a user presses a mouse button on the legend.
        /// </summary>
        /// <param name = "handle">Layer or group handle that was clicked.</param>
        /// <param name = "button">The integer representation of the button used.  Uses vb6 mouse button constants.</param>
        /// <param name = "location">The part of the legend that was clicked.</param>
        /// <param name = "handled">Reference parameter.  Prevents other plugins from getting this event when set to true.</param>
        public void LegendMouseDown(int handle, int button, ClickLocation location, ref bool handled)
        {
        }

        /// <summary>
        ///   Occurs when the user releases a mouse button over the legend.
        /// </summary>
        /// <param name = "handle">The handle of the group or layer.</param>
        /// <param name = "button">The integer representation of the button released.  Uses vb6 button constants.</param>
        /// <param name = "location">Enumeration.  Specifies if a group, layer or neither was clicked on.</param>
        /// <param name = "handled">Reference parameter.  Prevents other plugins from getting this event.</param>
        public void LegendMouseUp(int handle, int button, ClickLocation location, ref bool handled)
        {
        }

        /// <summary>
        ///   Occurs when a layer is removed from the MapWindow.
        /// </summary>
        /// <param name = "handle">The handle of the layer being removed.</param>
        public void LayerRemoved(int handle)
        {
        }

        /// <summary>
        ///   Occurs when a user moves the mouse over the this.mapWindow main display.
        /// </summary>
        /// <param name = "screenX">X coordinate in pixels.</param>
        /// <param name = "screenY">Y coordinate in pixels.</param>
        /// <param name = "handled">Reference parameter.  Prevents other plugins from getting this event.</param>
        public void MapMouseMove(int screenX, int screenY, ref bool handled)
        {
        }

        /// <summary>
        ///   Occurs when the "Clear all layers" button is pressed in the MapWindow.
        /// </summary>
        public void LayersCleared()
        {
        }

        /// <summary>
        ///   Occurs when shapes have been selected in the MapWindow.
        /// </summary>
        /// <param name = "handle">The handle of the layer that was selected on.</param>
        /// <param name = "selectInfo">Information about all the shapes that were selected.</param>
        public void ShapesSelected(int handle, SelectInfo selectInfo)
        {
        }

        #endregion

        /// <summary>
        ///   Create the toolbar
        /// </summary>
        /// <param name = "imageResources">The images to show on the toolbar.</param>
        private void CreateToolbar(Resources imageResources)
        {
            #region add toolbar
            // Add toolbar with buttons:
            var t = this.mapWin.Toolbar;
            t.AddToolbar(this.toolbarName);
            #endregion

            #region add button
            // First button:
            var b = t.AddButton(BtnNameLoadData, this.toolbarName, string.Empty, string.Empty);
            b.BeginsGroup = true;
            b.Tooltip = this.toolTipLoadData;
            b.Category = this.toolbarName;
            b.Text = "Template";
            using (var toolbarImage = imageResources.GetEmbeddedBitmap("VS2008.png", false))
            {
                b.Picture = new Bitmap(toolbarImage);
            }


            // Save to remove it easily on terminate:
            this.addedToolbuttons.Push(BtnNameLoadData);
            #endregion

            // Optional: more buttons
        }

        /// <summary>
        ///   Create the menu
        /// </summary>
        /// <param name = "imageResources">The images to show on the toolbar.</param>
        private void CreateMenu(Resources imageResources)
        {
            // Add menu with items:
            var m = this.mapWin.Menus;
            m.AddMenu(MnuTopName, null, imageResources.GetEmbeddedBitmap("VS2008.png", false), "Top menu");
            m.AddMenu(MnuTools, MnuTopName, imageResources.GetEmbeddedBitmap("Tools.png", false), "Sub menu");

            // Save to remove it easiluy on terminate:
            this.addedMenus.Push(MnuTopName);
        }
    }
}