//********************************************************************************************************
//The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
//you may not use this file except in compliance with the License. You may obtain a copy of the License at 
//http://www.mozilla.org/MPL/ 
//Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
//ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
//limitations under the License. 
//
//The Initial Developer of this version of the Original Code is Lailin Chen
//
//Contributor(s): (Open source contributors should list themselves and their modifications here). 
//6/27/2006 - Initial Version, developed by Lailin Chen
//********************************************************************************************************

using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;

using MySql.Data.MySqlClient;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Database
{
    /// <summary>
    /// Summary description for ShapefileEditorClass.
    /// </summary>
    public class OpenHydroPluginClass : MapWindow.Interfaces.IPlugin
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        private string tempPath = System.Environment.GetEnvironmentVariable("TEMP") + "\\" + DateTime.Now.Millisecond.ToString() + "\\";
        private MapWindow.Interfaces.IMapWin MapWin;



        /// <summary>
        /// Constructor:
        /// The ShapefileEditorClass implements the MapWindow.Interfaces.IPlugin interface.  This class handles all the direct interaction with the MapWindow.
        /// </summary>
        public OpenHydroPluginClass()
        {
            log.Debug("OpenHydroPluginClass");
        }

        #region Implementation of IPlugin
        /// <summary>
        /// Called when the MapWindow loads a new project.
        /// </summary>
        /// <param name="ProjectFile">The filename of the project file being loaded.</param>
        /// <param name="SettingsString">The settings string that was saved in the project file for this plugin.</param>
        public void ProjectLoading(string ProjectFile, string SettingsString)
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
        /// This method is called when a plugin is unloaded.  The plugin should remove all toolbars, buttons and menus that it added.
        /// </summary>
        public void Terminate()
        {
            MapWin.Menus.Remove("Open Hydro");

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
        /// ProjectSaving is called when the MapWindow saves a project.  This is a good chance for the plugin to save any custom settings and create a SettingsString to place in the project file.
        /// </summary>
        /// <param name="ProjectFile">The name of the project file being saved.</param>
        /// <param name="SettingsString">Reference parameter.  The settings string that will be saved in the project file for this plugin.</param>
        public void ProjectSaving(string ProjectFile, ref string SettingsString)
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
                    case "Load Features":
                        string exePath = Application.ExecutablePath;
                        string pluginPath = exePath.Substring(0, exePath.LastIndexOf("\\")) + "\\ApplicationPlugins\\database\\";
                        Environment.SetEnvironmentVariable("path", pluginPath + ";" + Environment.GetEnvironmentVariable("path"));

                        System.Console.WriteLine("Load Featuer ............");
                        Database.Forms.Connect2Database frmConnect = new Database.Forms.Connect2Database();
                        frmConnect.ShowDialog();

                        if (frmConnect.SqlConnection == null)
                        {
                            System.Windows.Forms.MessageBox.Show("database is not connected");
                            return;
                        }
                        string tempPath = System.Environment.GetEnvironmentVariable("TEMP") + "\\" + DateTime.Now.Millisecond.ToString() + "\\";
                        System.IO.Directory.CreateDirectory(tempPath);

                        Database.Forms.ChoseLayer frmChoseLayer = new Database.Forms.ChoseLayer(frmConnect, tempPath);
                        frmChoseLayer.ShowDialog();

                      
                        string[] shpFiles = System.IO.Directory.GetFiles(tempPath, "*.shp");
                        foreach (string shpFileName in shpFiles)
                        {
                            MapWinGIS.Shapefile sf = new MapWinGIS.ShapefileClass();
                            sf.Open(shpFileName, null);
                            string fullLayerName = sf.Filename.Substring(sf.Filename.LastIndexOf("\\") + 1, sf.Filename.Length - sf.Filename.LastIndexOf("\\") - 1);
                            string layerName = fullLayerName.Substring(0, fullLayerName.Length - 4);

                            MapWin.Layers.Add(ref sf, layerName);
                        }
                        break;
                    case "Export Database to Shapefiles":
                        //Connect to geodatabase:
                        frmConnect = new Database.Forms.Connect2Database();
                        frmConnect.ShowDialog();

                        if (frmConnect.SqlConnection == null)
                        {
                            System.Windows.Forms.MessageBox.Show("database is not connected");
                            return;
                        }
                        frmChoseLayer 
                            = new Database.Forms.ChoseLayer(frmConnect, null);
                        frmChoseLayer.ShowDialog();
                        break;

                    case "Import Shapefile into Database":
                        //Connect to geodatabase:
                        frmConnect = new Database.Forms.Connect2Database();
                        frmConnect.ShowDialog();

                        if (frmConnect.SqlConnection == null)
                        {
                            System.Windows.Forms.MessageBox.Show("database is not connected");
                            return;
                        }
                        OpenFileDialog dlg = new OpenFileDialog();
                        dlg.Filter = "Shape Files|*.shp";
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {//Do the import

                            string fileName = dlg.FileName;
                            string shortFileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                            string tableName = shortFileName.Substring(0, shortFileName.Length - 4);
                            string strCmd = " -c \"" + fileName + "\" " + tableName;

                            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                            info.FileName = "shp2pgsql.exe";
                            info.Arguments = strCmd;
                            info.CreateNoWindow = true;
                            info.UseShellExecute = false;
                            info.RedirectStandardOutput = true;
                            System.Diagnostics.Process proc = new System.Diagnostics.Process();
                            proc.StartInfo = info;
                            proc.Start();
                            System.IO.StreamReader reader = proc.StandardOutput;
                            System.Text.StringBuilder builder = new System.Text.StringBuilder();
                            IDbCommand daPgsql;

                            while (reader.Peek() >= 0)
                            {
                                string sqlSegment = reader.ReadLine();
                                builder.Append(sqlSegment);
                                if (builder.Length > 1000)
                                {
                                    daPgsql = new Npgsql.NpgsqlCommand(builder.ToString(), (Npgsql.NpgsqlConnection)frmConnect.SqlConnection);
                                    try
                                    {
                                        daPgsql.ExecuteNonQuery();
                                        builder = new StringBuilder();
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.ToString());
                                        return;
                                    }
                                }
                            }
                            //Excute the last sql you just got:
                            if (builder.Length > 0)
                            {
                                daPgsql = new Npgsql.NpgsqlCommand(builder.ToString(), (Npgsql.NpgsqlConnection)frmConnect.SqlConnection);
                                try
                                {
                                    daPgsql.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.ToString());
                                }
                            }

                            

                        }
                        else
                        {
                            return;
                        }
                        
                        
                        break;

                    case "":


                        break;

                    default:
                        break;
                }
            }
            catch (System.Exception ex)
            {
                log.Debug(ex.ToString());
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
        /// Occurs after a user selects a rectangular area in the MapWindow.  Normally this implies selection.
        /// </summary>
        /// <param name="Bounds">The rectangle selected.</param>
        /// <param name="Handled">Reference parameter.  Setting Handled to true prevents other plugins from receiving this event.</param>
        public void MapDragFinished(System.Drawing.Rectangle Bounds, 
            ref bool Handled)
        {

        }

        /// <summary>
        /// This event is called when a plugin is loaded or turned on in the MapWindow.
        /// </summary>
        /// <param name="MapWin">The interface to use to access the MapWindow.</param>
        /// <param name="ParentHandle">The window handle of the MapWindow form.  This handle is useful for making the MapWindow the owner of plugin forms.</param>
        public void Initialize(MapWindow.Interfaces.IMapWin MapWin, int ParentHandle)
        {
            log.Debug("Initializing .....");

            this.MapWin = MapWin;
            //Remove Allen's watershed menus
            MapWin.Menus.AddMenu("Geodatabase", "mnuFile", null, "Geodatabase","mnuOpen");
            MapWin.Menus.AddMenu("Load Features", "Geodatabase");
            MapWin.Menus.AddMenu("Export Database to Shapefiles", "Geodatabase");
            MapWin.Menus.AddMenu("Import Shapefile into Database", "Geodatabase");

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
            try
            {

            }
            catch
            {

            }
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

        /// <summary>
        /// Date that the plugin was built.
        /// </summary>
        public string BuildDate
        {
            get
            {
                return System.IO.File.GetLastAccessTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString();
            }
        }

        /// <summary>
        /// Description of the plugin.
        /// </summary>
        public string Description
        {
            get
            {
               return "Open Hydro Data Model and Tools";
            }
        }

        /// <summary>
        /// Author of the plugin.
        /// </summary>
        public string Author
        {
            get
            {
               return "Lailin Chen - Idaho State University GIS Lab";
            }
        }
        /// <summary>
        /// Name of the plugin.
        /// </summary>
        public string Name
        {
            get
            {
              return "DatabaseTools";
            }
        }

        /// <summary>
        /// Version of the plugin.
        /// </summary>
        public string Version
        {
            get
            {
                System.Diagnostics.FileVersionInfo f = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
                return f.FileMajorPart.ToString() + "." + f.FileMinorPart.ToString() + "." + f.FileBuildPart.ToString();
            }
        }

        /// <summary>
        /// Serial number of the plugin.
        /// The serial number and the name are tied together.  For each name there is a corresponding serial number.
        /// </summary>
        public string SerialNumber
        {
            get
            {
                return "This is open source now";
            }
        }
        #endregion

    }
}
