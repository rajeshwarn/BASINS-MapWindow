//********************************************************************************************************
//File Name: clsMain.cs
//Description: This is the start of the plug-in. In here all methods of the IPlugin interface are implemented.
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
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace TemplatePluginVS2005.Classes
{
    public class TemplatePluginVS2005Plugin : MapWindow.Interfaces.IPlugin
    {
        private MapWindow.Interfaces.IMapWin _mapWin;
        private int _parentHandle;
        //multiple buttons/plug-ins can be placed on the same toolbar
        private const string _toolbarName = "<<ADD YOUR OWN NAME HERE>>"; 
        private const string _btnNameLoadData = "<<ADD YOUR PLUG-IN NAME HERE>>";
        private const string _toolTipLoadData = "<<ADD YOUR PLUG-IN TOOLTIP HERE>>";

        public bool UserActivatedPlugin = false; //used to check if this plug-in should react on GUI

        #region Implementation of IPlugin

        /// <summary>
        /// This event is called when a plugin is loaded or turned on in the MapWindow.
        /// </summary>
        /// <param name="MapWin">The interface to use to access the MapWindow.</param>
        /// <param name="ParentHandle">The window handle of the MapWindow form.  This handle is useful for 
        /// making the MapWindow the owner of plugin forms.</param>
        public void Initialize(MapWindow.Interfaces.IMapWin MapWin, int ParentHandle)
        {
            try
            {
                _mapWin = MapWin;
                _parentHandle = ParentHandle; //Needed to not let the forms fall behind MapWindow

                MapWindow.Interfaces.Toolbar t = MapWin.Toolbar;
                //Resources:
                clsResources cResources = new clsResources();

                t.AddToolbar(_toolbarName);
                MapWindow.Interfaces.ToolbarButton b = t.AddButton(_btnNameLoadData, _toolbarName, "", "");
                b.BeginsGroup = true;
                b.Tooltip = _toolTipLoadData;
                b.Category = _toolbarName;
                System.Drawing.Icon oIcon = cResources.GetEmbeddedIcon("TemplatePluginVS2005.ico");
                b.Picture = new System.Drawing.Icon(oIcon, new System.Drawing.Size(16, 16));

                //Clean up:
                t = null;
                b = null;
            }
            catch (Exception ex)
            {
                _mapWin.ShowErrorDialog(ex);
            }
        }

        /// <summary>
        /// This method is called when a plugin is unloaded.  The plugin should remove all toolbars, buttons and menus that it added.
        /// </summary>
        public void Terminate()
        {
            _mapWin.Toolbar.RemoveButton(_btnNameLoadData);
            //Enhancement #703:
            if (_mapWin.Toolbar.NumToolbarButtons(_toolbarName) == 0)
            {
                //If all buttons are removed, remove toolbar as well:
                _mapWin.Toolbar.RemoveToolbar(_toolbarName);
            }
        }

        /// <summary>
        /// Called when the MapWindow loads a new project.
        /// </summary>
        /// <param name="ProjectFile">The filename of the project file being loaded.</param>
        /// <param name="SettingsString">The settings string that was saved in the project file for this plugin.</param>
        public void ProjectLoading(string ProjectFile, string SettingsString)
        {

        }

        /// <summary>
        /// ProjectSaving is called when the MapWindow saves a project.  This is a good chance for the 
        /// plugin to save any custom settings and create a SettingsString to place in the project file.
        /// </summary>
        /// <param name="ProjectFile">The name of the project file being saved.</param>
        /// <param name="SettingsString">Reference parameter.  The settings string that will be saved in the project file for this plugin.</param>
        public void ProjectSaving(string ProjectFile, ref string SettingsString)
        {
        }

        /// <summary>
        /// Method used by plugins to communicate with other plugins.
        /// </summary>
        /// <param name="msg">The messsage being recieved.</param>
        /// <param name="Handled">Reference parameter.  Set thist to true if this plugin handles recieving the message.  When set to true, no other plugins will receive the message.</param>
        public void Message(string msg, ref bool Handled)
        {

        }

        /// <summary>
        /// Method that is called when the MapWindow extents change.
        /// </summary>
        public void MapExtentsChanged()
        {

        }

        /// <summary>
        /// Occurs when a user clicks on a toolbar button or menu item.
        /// </summary>
        /// <param name="ItemName">The name of the item clicked on.</param>
        /// <param name="Handled">Reference parameter.  Setting Handled to true prevents other plugins from receiving this event.</param>
        public void ItemClicked(string ItemName, ref bool Handled)
        {
            try
            {
                switch (ItemName)
                {
                    case _btnNameLoadData:
                        UserActivatedPlugin = true;
                        Handled = true;
                        //Open the form        
                        
                        TemplatePluginVS2005.Forms.frmLayers frm = new TemplatePluginVS2005.Forms.frmLayers(_mapWin);
                        //Show it on top of MapWindow
                        frm.Show(Form.FromHandle(new IntPtr(_parentHandle)));

                        break;

                    default:
                        UserActivatedPlugin = false; //Some other button was clicked
                        break;
                }
            }
            catch (System.Exception ex)
            {
                _mapWin.ShowErrorDialog(ex);
            }
        }

        /// <summary>
        /// Called when 1 or more layers are added to the MapWindow.
        /// </summary>
        /// <param name="Layers">Array of layer objects containing references to all layers added.</param>
        public void LayersAdded(MapWindow.Interfaces.Layer[] Layers)
        {

        }

        /// <summary>
        /// Called when a layer is selected or made to be the active layer.
        /// </summary>
        /// <param name="Handle">The layer handle of the newly selected layer.</param>
        public void LayerSelected(int Handle)
        {

        }

        /// <summary>
        /// Occurs after a user selects a rectangular area in the MapWindow.  Normally this implies 
        /// selection.
        /// </summary>
        /// <param name="Bounds">The rectangle selected.</param>
        /// <param name="Handled">Reference parameter.  Setting Handled to true prevents other plugins from 
        /// receiving this event.</param>
        public void MapDragFinished(System.Drawing.Rectangle Bounds, ref bool Handled)
        {

        }



        /// <summary>
        /// Occurs when a user releases a mouse button on the MapWindow main map display.
        /// </summary>
        /// <param name="Button">An integer representation of which button(s) were released.  Uses vb6 button constants.</param>
        /// <param name="Shift">An integer representation of the shift/alt/ctrl keys that were pressed at the time the mouse button was released.  Uses vb6 shift constants.</param>
        /// <param name="x">X coordinate in pixels.</param>
        /// <param name="y">Y coordinate in pixels.</param>
        /// <param name="Handled">Reference parameter.  Prevents other plugins from getting this event.</param>
        public void MapMouseUp(int Button, int Shift, int x, int y, ref bool Handled)
        {

        }

        /// <summary>
        /// Occurs when the user presses a mouse button on the MapWindow map display.
        /// </summary>
        /// <param name="Button">The integer representation of the button pressed by the user.  This parameter uses the vb6 mouse button constants (vbLeftButton, etc.).</param>
        /// <param name="Shift">The integer representation of which shift/alt/control keys are pressed. This parameter uses the vb6 shift constants.</param>
        /// <param name="x">X coordinate in pixels.</param>
        /// <param name="y">Y coordinate in pixels.</param>
        /// <param name="Handled">Reference parameter.  When set to true, no other plugins will receive this event.</param>
        public void MapMouseDown(int Button, int Shift, int x, int y, ref bool Handled)
        {
        }

        /// <summary>
        /// Occurs when a user double clicks on the legend.
        /// </summary>
        /// <param name="Handle">The handle of the legend group or item that was clicked on.</param>
        /// <param name="Location">Enumerated.  The location clicked on.</param>
        /// <param name="Handled">Reference parameter.  When set to true it prevents additional plugins from getting this event.</param>
        public void LegendDoubleClick(int Handle, MapWindow.Interfaces.ClickLocation Location, ref bool Handled)
        {

        }

        /// <summary>
        /// Occurs when a user presses a mouse button on the legend.
        /// </summary>
        /// <param name="Handle">Layer or group handle that was clicked.</param>
        /// <param name="Button">The integer representation of the button used.  Uses vb6 mouse button constants.</param>
        /// <param name="Location">The part of the legend that was clicked.</param>
        /// <param name="Handled">Reference parameter.  Prevents other plugins from getting this event when set to true.</param>
        public void LegendMouseDown(int Handle, int Button, MapWindow.Interfaces.ClickLocation Location, ref bool Handled)
        {

        }

        /// <summary>
        /// Occurs when the user releases a mouse button over the legend.
        /// </summary>
        /// <param name="Handle">The handle of the group or layer.</param>
        /// <param name="Button">The integer representation of the button released.  Uses vb6 button constants.</param>
        /// <param name="Location">Enumeration.  Specifies if a group, layer or neither was clicked on.</param>
        /// <param name="Handled">Reference parameter.  Prevents other plugins from getting this event.</param>
        public void LegendMouseUp(int Handle, int Button, MapWindow.Interfaces.ClickLocation Location, ref bool Handled)
        {

        }

        /// <summary>
        /// Occurs when a layer is removed from the MapWindow.
        /// </summary>
        /// <param name="Handle">The handle of the layer being removed.</param>
        public void LayerRemoved(int Handle)
        {

        }

        /// <summary>
        /// Occurs when a user moves the mouse over the MapWindow main display.
        /// </summary>
        /// <param name="ScreenX">X coordinate in pixels.</param>
        /// <param name="ScreenY">Y coordinate in pixels.</param>
        /// <param name="Handled">Reference parameter.  Prevents other plugins from getting this event.</param>
        public void MapMouseMove(int ScreenX, int ScreenY, ref bool Handled)
        {

        }

        /// <summary>
        /// Occurs when the "Clear all layers" button is pressed in the MapWindow.
        /// </summary>
        public void LayersCleared()
        {

        }

        /// <summary>
        /// Occurs when shapes have been selected in the MapWindow.
        /// </summary>
        /// <param name="Handle">The handle of the layer that was selected on.</param>
        /// <param name="SelectInfo">Information about all the shapes that were selected.</param>
        public void ShapesSelected(int Handle, MapWindow.Interfaces.SelectInfo SelectInfo)
        {

        }
#endregion

#region Assembly stuff
        /// <summary>
        /// Date that the plugin was built.
        /// </summary>
        public string BuildDate
        {
            get {return System.IO.File.GetLastAccessTime(Assembly.GetExecutingAssembly().Location).ToString(); }
        }

        /// <summary>
        /// Description of the plugin.
        /// </summary>
        public string Description
        {
            get { return AssemblyInformation.ProductDescription(); }

        }

        /// <summary>
        /// Author of the plugin.
        /// </summary>
        public string Author
        {
            get { return AssemblyInformation.ProductCompany(); }
        }
        /// <summary>
        /// Name of the plugin.
        /// </summary>
        public string Name
        {
            get { return AssemblyInformation.ProductTitle(); }
        }

        /// <summary>
        /// Version of the plugin.
        /// </summary>
        public string Version
        {
            get
            {
                System.Diagnostics.FileVersionInfo f = System.Diagnostics.FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
                return f.FileMajorPart.ToString() + "." + f.FileMinorPart.ToString() + "." + f.FileBuildPart.ToString();
            }
        }

        /// <summary>
        /// Serial number of the plugin.
        /// The serial number and the name are tied together.  For each name there is a corresponding serial number.
        /// </summary>
        public string SerialNumber
        {
            get { return "This is Open Source"; }
        }
        #endregion
    
    }
}
