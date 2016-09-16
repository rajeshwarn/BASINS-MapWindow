//********************************************************************************************************
//The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
//you may not use this file except in compliance with the License. You may obtain a copy of the License at 
//http://www.mozilla.org/MPL/ 
//Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
//ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
//limitations under the License. 
//
//The Original Code is MapWindow Identifier Plug-in. 
//
//The Initial Developer of this version of the Original Code is Daniel P. Ames using portions created by 
//Utah State University and the Idaho National Engineering and Environmental Lab that were released as 
//public domain in March 2004.  
//
//Contributor(s): (Open source contributors should list themselves and their modifications here). 
//2/1/2005 - jlk - allow toolbar to be added to MapWindow main toolbar
//9/29/2005 - dpa - removed the labeler and placed it in a separate plug-in.
//					also renamed this just "Identifier" since it identifies both features and grids.
//6/5/2006 - Chris Michaelis - Changed this plug-in to use the MapWindow UIPanel w/ pushpin icons
//********************************************************************************************************

#region Usings

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using MapWindow.Interfaces;
using MapWinGIS;
using MapWinUtility;
using mwIdentifier.Forms;

#endregion

namespace mwIdentifier
{
    public struct Bounds
    {
        public int x1;
        public int x2;
        public int y1;
        public int y2;
    }

    public class mwIdentPlugin : IPlugin
    {
        //Internationalization added 11/1/2005 Lailin Chen
        private const string ToolbarName = "tlbIdentify";
        private const string ButtonNameIdentify = "Identify";
        //private const string ButtonNameIdentifyShapes = "Identify by Shape(s)";
        private readonly Color YELLOW = Color.Yellow;
        private readonly Cursor m_Cursor;

        private readonly ResourceManager resMan = new ResourceManager(
            "mwIdentifier.Resource", Assembly.GetExecutingAssembly());

        private frmIdentByShape identbyshapeForm;

        public bool m_Activated;
        private Bounds m_Bounds;
        public frmGridProp m_GridPropfrm;
        public bool m_HavePanel;
        private ToolbarButton m_IdentBtn;
        // private ToolbarButton m_IdentByShape;
        public IMapWin m_MapWin;
        public bool m_MouseDown;
        private bool m_OnWMSLayer;
        public int m_ParentHandle;
        public tkCursor m_PreviousCursor;
        public tkCursorMode m_PreviousCursorMode;
        private Panel m_UIPanel;
        private string m_UIPanel_LastType = "";
        private string m_WMSLayerIDReturn;
        public frmWMSProp m_WMSPropfrm;
        public int m_hDraw;
        public frmShapeFileProp m_shpFilePropfrm;

        //Constructor
        public mwIdentPlugin()
        {
            try
            {
                this.Deactivate();
                //m_Cursor = new Cursor(this.GetType(), "ico_help.ico");
                this.m_Cursor = new Cursor(this.GetType(), "Ident10262006_2.ico");
            }
            catch (Exception ex)
            {
                this.ShowErrorBox("mwIdentPlugin()", ex.Message);
            }
        }

        public bool Activated
        {
            get { return this.m_Activated; }
            set
            {
                try
                {
                    if (value)
                    {
                        this.m_Activated = true;

                        //save the previous mode and cursor
                        if (this.m_MapWin.View.UserCursorHandle != (int)this.m_Cursor.Handle)
                        {
                            this.m_PreviousCursorMode = this.m_MapWin.View.CursorMode;
                            this.m_PreviousCursor = this.m_MapWin.View.MapCursor;
                        }

                        this.m_MapWin.View.CursorMode = tkCursorMode.cmNone;
                        this.m_MapWin.View.UserCursorHandle = (int)this.m_Cursor.Handle;
                        this.m_MapWin.View.MapCursor = tkCursor.crsrUserDefined;
                        //make the button state pressed
                        this.m_IdentBtn.Pressed = true;

                        //load the current layers properies
                        this.LoadLayer();
                    }
                    else
                    {
                        this.Deactivate();
                        this.m_MapWin.View.Draw.ClearDrawing(this.m_hDraw);
                        this.m_MapWin.View.Draw.ClearDrawing(this.m_GridPropfrm.m_hDraw);

                        //make the button sate unPressed
                        this.m_IdentBtn.Pressed = false;

                        //hide all the forms
                        this.m_GridPropfrm.Hide();
                        this.m_shpFilePropfrm.Hide();
                        this.m_WMSPropfrm.Hide();

                        //close everything
                        this.m_shpFilePropfrm.Unitialize();

                        //set the cursorMode and MapCursor back to it last state --
                        //if it is still mine -- otherwise probably launched another tool already.
                        if (this.m_MapWin.View.UserCursorHandle == (int)this.m_Cursor.Handle)
                        {
                            this.m_MapWin.View.UserCursorHandle = -1;
                            this.m_MapWin.View.CursorMode = this.m_PreviousCursorMode;
                            this.m_MapWin.View.MapCursor = this.m_PreviousCursor;
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.ShowErrorBox("Activated()", ex.Message);
                }
            }
        }

        #region IPlugin Members

        public string Author
        {
            get { return "MapWindow Open Source Team"; }
        }

        public string BuildDate
        {
            get { return File.GetLastWriteTime(this.GetType().Assembly.Location).ToString(); }
        }

        public string Description
        {
            get { return "Identifier for raster and vector data."; }
        }

        public string Name
        {
            get { return this.resMan.GetString("mwIdentifier.Name"); }
        }

        public string SerialNumber
        {
            get { return ""; }
        }

        public string Version
        {
            get
            {
                var major =
                    FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileMajorPart.ToString();
                var minor =
                    FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileMinorPart.ToString();
                var build =
                    FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileBuildPart.ToString();

                return major + "." + minor + "." + build;
            }
        }

        public void Initialize(IMapWin mapWin, int parentHandle)
        {
            try
            {
                this.m_MapWin = mapWin;
                this.m_ParentHandle = parentHandle;

                this.m_shpFilePropfrm = new frmShapeFileProp(this);
                this.m_GridPropfrm = new frmGridProp(this);
                this.m_WMSPropfrm = new frmWMSProp(this);

                this.CreateToolbar();
            }
            catch (Exception ex)
            {
                this.ShowErrorBox("Initialize()", ex.Message);
            }
        }

        public void Terminate()
        {
            if (this.m_HavePanel && this.m_UIPanel != null)
            {
                this.m_MapWin.UIPanel.DeletePanel(ButtonNameIdentify);
                this.m_HavePanel = false;
                this.m_UIPanel = null;
            }

            // unload the buttons
            this.m_MapWin.Toolbar.RemoveButton(ButtonNameIdentify);
            // this.m_MapWin.Toolbar.RemoveButton(ButtonNameIdentifyShapes);

            //unload the toolbar
            /*
            if (ToolbarName.Length > 0)
            {
                if (this.m_MapWin.Toolbar.NumToolbarButtons(ToolbarName) == 0)
                {
                    // If all buttons are removed, remove toolbar as well:
                    this.m_MapWin.Toolbar.RemoveToolbar(ToolbarName);
                }
            }
            */

            this.m_MapWin = null;
        }

        public void ItemClicked(string itemName, ref bool handled)
        {
            try
            {
                //check to see if it was the Identifier pressed
                if (itemName == ButtonNameIdentify)
                {
                    this.Activated = !this.Activated;

                    //we handled this event
                    handled = true;
                }
                else if (this.Activated)
                {
                    this.Activated = false;
                }

                /*
                if (itemName == ButtonNameIdentifyShapes)
                {
                    if (this.identbyshapeForm == null || this.identbyshapeForm.IsDisposed)
                    {
                        this.identbyshapeForm = new frmIdentByShape(this.m_MapWin, this);
                    }

                    //identbyshapeForm.Show();
                    //Paul Meems, 4 april 2008
                    //Add parenthandle so the form stays above MW instead of falling behind:
                    this.identbyshapeForm.Show(Control.FromHandle(new IntPtr(this.m_ParentHandle)));
                }
                */
            }
            catch (Exception ex)
            {
                this.ShowErrorBox("ItemClicked()", ex.Message);
            }
        }

        public void LayerRemoved(int Handle)
        {
            this.UpdateButtons();
        }

        public void LayersCleared()
        {
            this.UpdateButtons();
        }

        public void LayerSelected(int Handle)
        {
            if (Handle == -1)
            {
                return;
            }

            if (!this.Activated)
            {
                return;
            }

            //only do this if not on a wms layer, because wms layers are handled through the Message
            if (this.m_OnWMSLayer)
            {
                return;
            }

            // Force reloading of the new layer by toggling activation
            var oldExts = this.m_MapWin.View.Extents;
            //Activated = false;
            //Activated = true;
            this.LoadLayer();
            this.m_MapWin.View.Extents = oldExts;
        }

        public void MapMouseDown(int Button, int Shift, int x, int y, ref bool Handled)
        {
            if (!this.Activated)
            {
                return;
            }

            this.m_MouseDown = true;

            //clear the previous drawing
            this.m_MapWin.View.Draw.ClearDrawing(this.m_hDraw);

            this.m_Bounds.x1 = x;
            this.m_Bounds.y1 = y;

            //set handled
            Handled = true;
        }

        public void MapMouseUp(int Button, int Shift, int x, int y, ref bool Handled)
        {
            try
            {
                if (this.Activated)
                {
                    this.m_MouseDown = false;

                    //exit if their is no layers
                    if (this.m_MapWin.Layers.NumLayers <= 0)
                    {
                        return;
                    }

                    //if a wms layer is selected and visible, broadcast so position can be checked
                    if (this.m_OnWMSLayer)
                    {
                        if (this.m_MapWin.Layers[this.m_MapWin.Layers.CurrentLayer].Visible)
                        {
                            this.m_MapWin.Plugins.BroadcastMessage("Identifier_Clicked_WMS " + x + " " + y);
                        }
                        else
                        {
                            this.m_WMSLayerIDReturn = "Error: WMS Data Not Visible. Try Zooming In.";
                            this.LoadLayer();
                        }
                    }
                    else //Go about the normal selection
                    {
                        Extents BoundBox;
                        var selShapes = new object();
                        double tolx1 = 0, toly1 = 0, tolx2 = 0, toly2 = 0, tol = 0;
                        var obj = this.m_MapWin.Layers[this.m_MapWin.Layers.CurrentLayer].GetObject();
                        var layerName = this.m_MapWin.Layers[this.m_MapWin.Layers.CurrentLayer].Name;
                        var LayerType = this.m_MapWin.Layers[this.m_MapWin.Layers.CurrentLayer].LayerType;

                        //get the selected region
                        BoundBox = this.GetBoundBox(this.m_Bounds);

                        //if layer is a shapefile then do the following
                        if (LayerType == eLayerType.LineShapefile
                            || LayerType == eLayerType.PointShapefile
                            || LayerType == eLayerType.PolygonShapefile)
                        {
                            if (this.m_GridPropfrm.Visible)
                            {
                                this.m_GridPropfrm.Hide();
                            }

                            var shpFile = (Shapefile)obj;

                            //clear all the selected shapes
                            //this.m_MapWin.View.SelectedShapes.ClearSelectedShapes();
                            this.m_MapWin.View.ClearSelectedShapes();

                            //calculate tolerance
                            double r;
                            if (this.m_MapWin.Layers[this.m_MapWin.Layers.CurrentLayer].PointType ==
                                tkPointType.ptUserDefined)
                            {
                                var image = this.m_MapWin.Layers[this.m_MapWin.Layers.CurrentLayer].UserPointType;
                                r = (image.Width + image.Height) / 2;
                            }
                            else
                            {
                                r = 3;
                            }

                            this.m_MapWin.View.PixelToProj(x - r, y - r, ref tolx1, ref toly1);
                            this.m_MapWin.View.PixelToProj(x + r, y + r, ref tolx2, ref toly2);
                            tol = Math.Sqrt(Math.Pow((tolx1 - tolx2), 2) + Math.Pow((toly1 - toly2), 2));

                            var SelectedShapes = shpFile.SelectShapes(
                                BoundBox, tol, SelectMode.INTERSECTION, ref selShapes);

                            //display the selected shapes
                            if (SelectedShapes)
                            {
                                this.AddSelectShapes(selShapes);
                                this.m_shpFilePropfrm.PopulateForm(
                                    !this.m_HavePanel,
                                    shpFile,
                                    (int[])selShapes,
                                    this.m_MapWin.Layers[this.m_MapWin.Layers.CurrentLayer].Name,
                                    false);
                            }
                            else
                            {
                                this.m_shpFilePropfrm.PopulateForm(
                                    !this.m_HavePanel,
                                    shpFile,
                                    this.m_MapWin.Layers[this.m_MapWin.Layers.CurrentLayer].Name,
                                    false);
                            }

                            m_MapWin.View.Redraw();
                        }
                            //if the layer is a grid do the following
                        else if (LayerType == eLayerType.Grid)
                        {
                            if (this.m_shpFilePropfrm.Visible)
                            {
                                this.m_WMSPropfrm.Hide();
                                this.m_shpFilePropfrm.Hide();
                            }

                            //get the grid object
                            var grid = this.m_MapWin.Layers[this.m_MapWin.Layers.CurrentLayer].GetGridObject;

                            if (grid != null)
                            {
                                this.m_GridPropfrm.PopulateForm(
                                    !this.m_HavePanel, grid, layerName, BoundBox, this.m_MapWin.Layers.CurrentLayer);
                            }
                            else
                            {
                                this.m_GridPropfrm.Hide();
                            }
                        }
                            //if the layer is a image do the following
                        else if (LayerType == eLayerType.Image)
                        {
                            if (this.m_shpFilePropfrm.Visible)
                            {
                                this.m_WMSPropfrm.Hide();
                                this.m_shpFilePropfrm.Hide();
                            }

                            var grid = this.FindAssociatedGrid(((MapWinGIS.Image)obj).Filename);

                            if (grid != null)
                            {
                                this.m_GridPropfrm.PopulateForm(
                                    !this.m_HavePanel, grid, layerName, BoundBox, this.m_MapWin.Layers.CurrentLayer);
                            }
                            else
                            {
                                // Identify by values -- so just open the first band of the image.
                                // This only works for greyscale of course in this version (one band grids only)

                                var tmpGrid = new Grid();
                                try
                                {
                                    // Open as a grid to get values
                                    if (tmpGrid.Open(
                                        ((MapWinGIS.Image)obj).Filename,
                                        GridDataType.UnknownDataType,
                                        false,
                                        GridFileType.UseExtension,
                                        null))
                                    {
                                        this.m_GridPropfrm.PopulateForm(
                                            !this.m_HavePanel,
                                            tmpGrid,
                                            layerName,
                                            BoundBox,
                                            this.m_MapWin.Layers.CurrentLayer);
                                    }
                                    else
                                    {
                                        Logger.Message(
                                            "No information on this image can be displayed.",
                                            "Identifier",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information,
                                            DialogResult.OK);
                                        this.m_GridPropfrm.Hide();
                                    }
                                }
                                catch
                                {
                                    this.m_GridPropfrm.Hide();
                                }
                            }
                        }
                    }

                    //set handled
                    Handled = true;
                }
            }
            catch (Exception ex)
            {
                this.ShowErrorBox("MapMouseUp()", ex.Message);
            }
        }

        public void MapMouseMove(int ScreenX, int ScreenY, ref bool Handled)
        {
            if (!this.Activated)
            {
                return;
            }

            double x = 0, y = 0;

            //update coordinates
            this.m_MapWin.View.PixelToProj(ScreenX, ScreenY, ref x, ref y);

            // Chris Michaelis - moved to main MapWindow app.
            //m_Panel.Text = "X: " + Math.Round(x,3).ToString() + "    Y: " + Math.Round(y,3).ToString();

            this.m_Bounds.x2 = ScreenX;
            this.m_Bounds.y2 = ScreenY;
            double x1 = this.m_Bounds.x1;
            double y1 = this.m_Bounds.y1;
            double x2 = this.m_Bounds.x2;
            double y2 = this.m_Bounds.y2;

            //get black color
            var black = Color.Black;

            //draw a selection box if your dragging
            if (!this.m_MouseDown)
            {
                return;
            }

            //clear the previous drawing
            this.m_MapWin.View.Draw.ClearDrawing(this.m_hDraw);

            //draw the bound box
            this.m_hDraw = this.m_MapWin.View.Draw.NewDrawing(tkDrawReferenceList.dlScreenReferencedList);
            this.m_MapWin.View.Draw.DrawLine(x1, y1, x2, y1, 1, black);
            this.m_MapWin.View.Draw.DrawLine(x1, y1, x1, y2, 1, black);
            this.m_MapWin.View.Draw.DrawLine(x1, y2, x2, y2, 1, black);
            this.m_MapWin.View.Draw.DrawLine(x2, y1, x2, y2, 1, black);
        }

        public void MapDragFinished(Rectangle Bounds, ref bool Handled)
        {
        }

        public void LegendDoubleClick(int Handle, ClickLocation Location, ref bool Handled)
        {
        }

        public void LegendMouseDown(int Handle, int Button, ClickLocation Location, ref bool Handled)
        {
        }

        public void LegendMouseUp(int Handle, int Button, ClickLocation Location, ref bool Handled)
        {
        }

        public void MapExtentsChanged()
        {
        }

        public void Message(string msg, ref bool Handled)
        {
            if (msg.StartsWith("ODP_Selected_WMS_Layer") || msg.StartsWith("ODP_Added_WMS_Layer"))
            {
                this.m_OnWMSLayer = true;
                this.m_WMSLayerIDReturn = "";

                if (this.Activated)
                {
                    // Force reloading of the new layer by toggling activation
                    var oldExts = this.m_MapWin.View.Extents;
                    this.LoadLayer();
                    this.m_MapWin.View.Extents = oldExts;
                }
            }
            else if (msg.StartsWith("ODP_Selected_Non_WMS_Layer"))
            {
                this.m_OnWMSLayer = false;
                this.m_WMSLayerIDReturn = "";
            }
            else if (msg.StartsWith("ODP_Identifier_Data_Return"))
            {
                if (this.Activated)
                {
                    this.m_WMSLayerIDReturn = msg.Replace("ODP_Identifier_Data_Return ", "");
                    // Force reloading of the new layer by toggling activation
                    var oldExts = this.m_MapWin.View.Extents;
                    this.LoadLayer();
                    this.m_MapWin.View.Extents = oldExts;
                }
            }
        }

        public void ProjectLoading(string ProjectFile, string SettingsString)
        {
            if (SettingsString != string.Empty)
            {
                var a = new[] { '=' };
                var s = SettingsString.Split(a);
                if (s[1].ToLower() == bool.TrueString.ToLower())
                {
                    this.m_shpFilePropfrm.Editable = true;
                }
                else if (s[1].ToLower() == bool.FalseString.ToLower())
                {
                    this.m_shpFilePropfrm.Editable = false;
                }
                else
                {
                    this.m_shpFilePropfrm.Editable = true;
                }
            }
            else
            {
                this.m_shpFilePropfrm.Editable = true;
            }
        }

        public void ProjectSaving(string ProjectFile, ref string SettingsString)
        {
            SettingsString = "Editable=" + this.m_shpFilePropfrm.Editable;
        }

        public void ShapesSelected(int Handle, SelectInfo SelectInfo)
        {
            if (this.identbyshapeForm != null && this.identbyshapeForm.Visible)
            {
                this.identbyshapeForm.UpdateSelectedShapes();
            }
        }

        public void LayersAdded(Layer[] Layers)
        {
            this.UpdateButtons();
        }

        #endregion

        private void CreateToolbar()
        {
            try
            {
                var t = this.m_MapWin.Toolbar;
                
                // t.AddToolbar(ToolbarName);
                var b = t.AddButton(ButtonNameIdentify, "tlbMain", string.Empty, string.Empty);
                b.BeginsGroup = true;
                b.Tooltip = ButtonNameIdentify;
                b.Category = ToolbarName;
                b.Text = ButtonNameIdentify;

                // Resources:
                using (var imageResources = new Resources())
                {
                    using (var toolbarIcon = imageResources.GetEmbeddedBitmap("indentifyNew.png", false))
                    {
                        b.Picture = new Bitmap(toolbarIcon, new Size(24, 24));
                    }
                }

                // Save for later use:
                this.m_IdentBtn = b;

                // Paul Meems, Don't use this anymore. Use the spatial query instead (it's in the toolbox):
                /*
                b = t.AddButton(ButtonNameIdentifyShapes, ToolbarName, string.Empty, ButtonNameIdentify);
                // b.BeginsGroup = true;
                b.Tooltip = ButtonNameIdentifyShapes;
                b.Category = ToolbarName;
                b.Text = "By shape";

                // Resources:
                using (var imageResources = new Resources())
                {
                    using (var toolbarIcon = imageResources.GetEmbeddedBitmap("info-by-shape.png", false))
                    {
                        b.Picture = new Bitmap(toolbarIcon, new Size(24, 24));
                    }
                }

                // Save for later use:
                this.m_IdentByShape = b;
                */

                this.UpdateButtons();
            }
            catch (Exception ex)
            {
                this.ShowErrorBox("Initialize()", ex.Message);
            }
        }

        public void ActivateNoLoad()
        {
            this.m_Activated = true;

            //save the previous mode and cursor
            if (this.m_MapWin.View.UserCursorHandle != (int)this.m_Cursor.Handle)
            {
                this.m_PreviousCursorMode = this.m_MapWin.View.CursorMode;
                this.m_PreviousCursor = this.m_MapWin.View.MapCursor;
            }
            this.m_MapWin.View.CursorMode = tkCursorMode.cmNone;
            this.m_MapWin.View.UserCursorHandle = (int)this.m_Cursor.Handle;
            this.m_MapWin.View.MapCursor = tkCursor.crsrUserDefined;
            //make the button sate pressed
            this.m_IdentBtn.Pressed = true;
        }

        public Extents GetBoundBox(Bounds bounds)
        {
            double maxX, maxY, minX, minY;
            double x1 = 0, y1 = 0, x2 = 0, y2 = 0;
            var boundBox = new Extents();

            //get the projection points
            this.m_MapWin.View.PixelToProj(bounds.x1, bounds.y1, ref x1, ref y1);
            this.m_MapWin.View.PixelToProj(bounds.x2, bounds.y2, ref x2, ref y2);

            //Set max and min X values
            if (x1 >= x2)
            {
                maxX = x1;
                minX = x2;
            }
            else
            {
                maxX = x2;
                minX = x1;
            }

            //Set max and min Y values
            if (y1 >= y2)
            {
                maxY = y1;
                minY = y2;
            }
            else
            {
                maxY = y2;
                minY = y1;
            }

            boundBox.SetBounds(minX, minY, 0, maxX, maxY, 0);

            return boundBox;
        }

        public void AddSelectShapes(object selShapes)
        {
            try
            {
                var shapes = (int[])selShapes;

                //display selected shapes
                var size = shapes.GetUpperBound(0);

                MapWindow.Interfaces.SelectInfo info = this.m_MapWin.View.SelectedShapes;
                for (var i = 0; i <= size; i++)
                {
                    info.AddByIndex(shapes[i], this.YELLOW);
                }
            }
            catch (Exception ex)
            {
                this.ShowErrorBox("SelectShapes()", ex.Message);
            }
        }

        private void OnPanelClose(string Caption)
        {
            if (Caption == ButtonNameIdentify)
            {
                this.Activated = false;
            }

            try
            {
                Control.FromHandle(new IntPtr(this.m_ParentHandle)).Focus();
            }
            catch
            {
            }
        }

        public void LoadLayerAlternate(eLayerType LayerType, string layerName)
        {
            try
            {
                //exit if their is no layers to load
                if (this.m_MapWin.Layers.NumLayers <= 0)
                {
                    return;
                }

                // Always try to create the panel; if it doesn't exist,
                // it will be created, and if it does exist, a handle will be returned.
                this.m_UIPanel = this.m_MapWin.UIPanel.CreatePanel(ButtonNameIdentify, DockStyle.None);
                // Not intuitive -- m_UIPanel = m_MapWin.UIPanel.CreatePanel("Identifier", MapWindow.Interfaces.MapWindowDockStyle.RightAutoHide);
                if (this.m_UIPanel != null)
                {
                    this.m_MapWin.UIPanel.AddOnCloseHandler(ButtonNameIdentify, this.OnPanelClose);
                    this.m_HavePanel = true;
                }

                //if the layer is a shapefile
                if (LayerType == eLayerType.LineShapefile
                    || LayerType == eLayerType.PointShapefile
                    || LayerType == eLayerType.PolygonShapefile)
                {
                    if (this.m_GridPropfrm.Visible)
                    {
                        this.m_GridPropfrm.Hide();
                    }
                }
                    //if the layer is a grid
                else if (LayerType == eLayerType.Grid)
                {
                    if (this.m_shpFilePropfrm.Visible)
                    {
                        this.m_shpFilePropfrm.Hide();
                        this.m_WMSPropfrm.Hide();
                    }
                }
                    //if the layer is a image
                else if (LayerType == eLayerType.Image)
                {
                    if (this.m_shpFilePropfrm.Visible)
                    {
                        this.m_shpFilePropfrm.Hide();
                        this.m_WMSPropfrm.Hide();
                    }
                }

                if (this.m_HavePanel && this.m_UIPanel != null)
                {
                    //if the layer is a shapefile
                    if (LayerType == eLayerType.LineShapefile
                        || LayerType == eLayerType.PointShapefile
                        || LayerType == eLayerType.PolygonShapefile)
                    {
                        if (this.m_shpFilePropfrm.panel1 == null || this.m_shpFilePropfrm.panel1.IsDisposed)
                        {
                            this.m_shpFilePropfrm = new frmShapeFileProp(this);
                        }

                        if (this.m_UIPanel_LastType != "shapefile" && this.m_UIPanel_LastType != "")
                        {
                            while (this.m_UIPanel.Controls.Count > 0)
                            {
                                this.m_UIPanel.Controls.RemoveAt(0);
                            }
                        }

                        if (this.m_UIPanel.Controls.Count == 0)
                        {
                            this.m_UIPanel.Controls.Add(this.m_shpFilePropfrm.panel1);
                            this.m_UIPanel_LastType = "shapefile";
                        }
                        else
                        {
                            this.m_MapWin.UIPanel.SetPanelVisible(ButtonNameIdentify, true);
                        }

                        this.m_shpFilePropfrm.Hide();
                    }
                        //if the layer is a grid or image
                    else if (LayerType == eLayerType.Grid || LayerType == eLayerType.Image)
                    {
                        if (this.m_GridPropfrm.panel1 == null || this.m_GridPropfrm.panel1.IsDisposed)
                        {
                            this.m_GridPropfrm = new frmGridProp(this);
                        }

                        if (this.m_UIPanel_LastType != "grid" && this.m_UIPanel_LastType != "")
                        {
                            while (this.m_UIPanel.Controls.Count > 0)
                            {
                                this.m_UIPanel.Controls.RemoveAt(0);
                            }
                        }

                        if (this.m_UIPanel.Controls.Count == 0)
                        {
                            this.m_UIPanel.Controls.Add(this.m_GridPropfrm.panel1);
                            this.m_UIPanel_LastType = "grid";
                        }
                        else
                        {
                            this.m_MapWin.UIPanel.SetPanelVisible(ButtonNameIdentify, true);
                        }

                        this.m_GridPropfrm.Hide();
                    }
                }
                // else Normal identifier with a separate window
            }
            catch (Exception ex)
            {
                this.ShowErrorBox("LoadLayer()", ex.Message);
            }
        }

        private void LoadLayer()
        {
            try
            {
                // exit if their is no layers to load
                if (this.m_MapWin.Layers.NumLayers <= 0)
                {
                    return;
                }

                // Always try to create the panel; if it doesn't exist,
                // it will be created, and if it does exist, a handle will be returned.
                this.m_UIPanel = this.m_MapWin.UIPanel.CreatePanel(ButtonNameIdentify, DockStyle.None);
                // Not intuitive -- m_UIPanel = m_MapWin.UIPanel.CreatePanel("Identifier", MapWindow.Interfaces.MapWindowDockStyle.RightAutoHide);
                if (this.m_UIPanel != null)
                {
                    this.m_MapWin.UIPanel.AddOnCloseHandler(ButtonNameIdentify, this.OnPanelClose);
                    this.m_HavePanel = true;
                }

                if (this.m_OnWMSLayer)
                {
                    if (this.m_GridPropfrm.Visible || this.m_shpFilePropfrm.Visible)
                    {
                        this.m_shpFilePropfrm.Hide();
                        this.m_GridPropfrm.Hide();
                    }

                    if (this.m_WMSLayerIDReturn != "")
                    {
                        this.m_WMSPropfrm.PopulateForm(
                            !this.m_HavePanel,
                            this.m_WMSLayerIDReturn,
                            this.m_MapWin.Layers[this.m_MapWin.Layers.CurrentLayer].Name,
                            false);
                    }

                    if (this.m_HavePanel && this.m_UIPanel != null)
                    {
                        if (this.m_WMSPropfrm.panel1 == null || this.m_WMSPropfrm.panel1.IsDisposed)
                        {
                            this.m_WMSPropfrm = new frmWMSProp(this);
                        }

                        if (this.m_UIPanel_LastType != "WMS" && this.m_UIPanel_LastType != "")
                        {
                            while (this.m_UIPanel.Controls.Count > 0)
                            {
                                this.m_UIPanel.Controls.RemoveAt(0);
                            }
                        }

                        if (this.m_UIPanel.Controls.Count == 0)
                        {
                            this.m_UIPanel.Controls.Add(this.m_WMSPropfrm.panel1);
                            this.m_UIPanel_LastType = "WMS";
                        }
                        else
                        {
                            this.m_MapWin.UIPanel.SetPanelVisible(ButtonNameIdentify, true);
                        }

                        this.m_WMSPropfrm.Hide();
                    }
                }
                else
                {
                    //get the object from mapwindow
                    var obj = this.m_MapWin.Layers[this.m_MapWin.Layers.CurrentLayer].GetObject();
                    var layerName = this.m_MapWin.Layers[this.m_MapWin.Layers.CurrentLayer].Name;
                    var LayerType = this.m_MapWin.Layers[this.m_MapWin.Layers.CurrentLayer].LayerType;

                    //if the layer is a shapefile
                    if (LayerType == eLayerType.LineShapefile
                        || LayerType == eLayerType.PointShapefile
                        || LayerType == eLayerType.PolygonShapefile)
                    {
                        if (this.m_GridPropfrm.Visible || this.m_WMSPropfrm.Visible)
                        {
                            this.m_WMSPropfrm.Hide();
                            this.m_GridPropfrm.Hide();
                        }

                        //check to see if there is any previously selected shapes
                        if (this.m_MapWin.View.SelectedShapes.NumSelected > 0)
                        {
                            var numSel = this.m_MapWin.View.SelectedShapes.NumSelected;
                            var shpIndex = new int[numSel];

                            MapWindow.Interfaces.SelectInfo info = this.m_MapWin.View.SelectedShapes;
                            for (var i = 0; i < numSel; i++)
                            {
                                shpIndex[i] = info[i].ShapeIndex;
                            }
                            this.m_shpFilePropfrm.PopulateForm(
                                !this.m_HavePanel,
                                (Shapefile)obj,
                                shpIndex,
                                this.m_MapWin.Layers[this.m_MapWin.Layers.CurrentLayer].Name,
                                false);
                        }
                        else
                        {
                            this.m_shpFilePropfrm.PopulateForm(
                                !this.m_HavePanel,
                                (Shapefile)obj,
                                this.m_MapWin.Layers[this.m_MapWin.Layers.CurrentLayer].Name,
                                false);
                        }
                    }
                        //if the layer is a grid
                    else if (LayerType == eLayerType.Grid)
                    {
                        if (this.m_shpFilePropfrm.Visible || this.m_WMSPropfrm.Visible)
                        {
                            this.m_WMSPropfrm.Hide();
                            this.m_shpFilePropfrm.Hide();
                        }

                        //get the grid object
                        var grid = this.m_MapWin.Layers[this.m_MapWin.Layers.CurrentLayer].GetGridObject;

                        if (grid != null)
                        {
                            this.m_GridPropfrm.PopulateForm(
                                !this.m_HavePanel, grid, layerName, null, this.m_MapWin.Layers.CurrentLayer);
                        }
                        else
                        {
                            this.m_GridPropfrm.Hide();
                        }
                    }
                        //if the layer is a image
                    else if (LayerType == eLayerType.Image)
                    {
                        // Chris Michaelis - 5-17-2007 - Allow identification of image values (the color values), better than nothing.
                        // Do this when no associated grid is found of course.

                        if (this.m_shpFilePropfrm.Visible || this.m_WMSPropfrm.Visible)
                        {
                            this.m_WMSPropfrm.Hide();
                            this.m_shpFilePropfrm.Hide();
                        }

                        var grid = this.FindAssociatedGrid(((MapWinGIS.Image)obj).Filename);

                        if (grid != null)
                        {
                            this.m_GridPropfrm.PopulateForm(
                                !this.m_HavePanel, grid, layerName, null, this.m_MapWin.Layers.CurrentLayer);
                        }
                        else
                        {
                            // Identify by values -- so just open the first band of the image.
                            // This only works for greyscale of course in this version (one band grids only)

                            var tmpGrid = new Grid();
                            try
                            {
                                // Open as a grid to get values
                                if (tmpGrid.Open(
                                    ((MapWinGIS.Image)obj).Filename,
                                    GridDataType.UnknownDataType,
                                    false,
                                    GridFileType.UseExtension,
                                    null))
                                {
                                    this.m_GridPropfrm.PopulateForm(
                                        !this.m_HavePanel, tmpGrid, layerName, null, this.m_MapWin.Layers.CurrentLayer);
                                }
                                else
                                {
                                    Logger.Message(
                                        "No information on this image can be displayed.",
                                        "Identifier",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information,
                                        DialogResult.OK);
                                    this.m_GridPropfrm.Hide();
                                }
                            }
                            catch
                            {
                                this.m_GridPropfrm.Hide();
                            }
                        }
                    }

                    if (this.m_HavePanel && this.m_UIPanel != null)
                    {
                        //if the layer is a shapefile
                        if (LayerType == eLayerType.LineShapefile
                            || LayerType == eLayerType.PointShapefile
                            || LayerType == eLayerType.PolygonShapefile)
                        {
                            if (this.m_shpFilePropfrm.panel1 == null || this.m_shpFilePropfrm.panel1.IsDisposed)
                            {
                                this.m_shpFilePropfrm = new frmShapeFileProp(this);
                            }

                            if (this.m_UIPanel_LastType != "shapefile" && this.m_UIPanel_LastType != "")
                            {
                                while (this.m_UIPanel.Controls.Count > 0)
                                {
                                    this.m_UIPanel.Controls.RemoveAt(0);
                                }
                            }

                            if (this.m_UIPanel.Controls.Count == 0)
                            {
                                this.m_UIPanel.Controls.Add(this.m_shpFilePropfrm.panel1);
                                this.m_UIPanel_LastType = "shapefile";
                            }
                            else
                            {
                                this.m_MapWin.UIPanel.SetPanelVisible(ButtonNameIdentify, true);
                            }

                            this.m_shpFilePropfrm.Hide();
                        }
                            //if the layer is a grid or image
                        else if (LayerType == eLayerType.Grid || LayerType == eLayerType.Image)
                        {
                            if (this.m_GridPropfrm.panel1 == null || this.m_GridPropfrm.panel1.IsDisposed)
                            {
                                this.m_GridPropfrm = new frmGridProp(this);
                            }

                            if (this.m_UIPanel_LastType != "grid" && this.m_UIPanel_LastType != "")
                            {
                                while (this.m_UIPanel.Controls.Count > 0)
                                {
                                    this.m_UIPanel.Controls.RemoveAt(0);
                                }
                            }

                            if (this.m_UIPanel.Controls.Count == 0)
                            {
                                this.m_UIPanel.Controls.Add(this.m_GridPropfrm.panel1);
                                this.m_UIPanel_LastType = "grid";
                            }
                            else
                            {
                                this.m_MapWin.UIPanel.SetPanelVisible(ButtonNameIdentify, true);
                            }

                            this.m_GridPropfrm.Hide();
                        }
                    }
                    // else Normal identifier with a separate window
                }
            }
            catch (Exception ex)
            {
                this.ShowErrorBox("LoadLayer()", ex.Message);
            }
        }

        public void ToggleDockedStatus(Form Sender, Panel ToggledPanel)
        {
            if (this.m_HavePanel)
            {
                if (this.m_OnWMSLayer)
                {
                    this.m_UIPanel.Controls.Remove(this.m_WMSPropfrm.panel1);
                    this.m_MapWin.UIPanel.DeletePanel(ButtonNameIdentify);
                    this.m_HavePanel = false;
                    this.m_UIPanel = null;
                    this.m_WMSPropfrm.Show();
                }
                else if (this.m_MapWin.Layers[this.m_MapWin.Layers.CurrentLayer].LayerType == eLayerType.LineShapefile ||
                         this.m_MapWin.Layers[this.m_MapWin.Layers.CurrentLayer].LayerType == eLayerType.PointShapefile ||
                         this.m_MapWin.Layers[this.m_MapWin.Layers.CurrentLayer].LayerType == eLayerType.PolygonShapefile)
                {
                    this.m_UIPanel.Controls.Remove(this.m_shpFilePropfrm.panel1);
                    this.m_MapWin.UIPanel.DeletePanel(ButtonNameIdentify);
                    this.m_HavePanel = false;
                    this.m_UIPanel = null;
                    this.m_shpFilePropfrm.Show();
                }
                else
                {
                    this.m_UIPanel.Controls.Remove(this.m_GridPropfrm.panel1);
                    this.m_MapWin.UIPanel.DeletePanel(ButtonNameIdentify);
                    this.m_HavePanel = false;
                    this.m_UIPanel = null;
                    this.m_GridPropfrm.Show();
                }
            }
            else
            {
                if (this.m_OnWMSLayer)
                {
                    this.m_UIPanel = this.m_MapWin.UIPanel.CreatePanel(ButtonNameIdentify, DockStyle.None);
                    if (this.m_UIPanel == null)
                    {
                        return;
                    }
                    this.m_HavePanel = true;
                    this.m_UIPanel.Controls.Add(this.m_WMSPropfrm.panel1);
                    this.m_WMSPropfrm.Hide();
                }
                else if (this.m_MapWin.Layers[this.m_MapWin.Layers.CurrentLayer].LayerType == eLayerType.LineShapefile ||
                         this.m_MapWin.Layers[this.m_MapWin.Layers.CurrentLayer].LayerType == eLayerType.PointShapefile ||
                         this.m_MapWin.Layers[this.m_MapWin.Layers.CurrentLayer].LayerType == eLayerType.PolygonShapefile)
                {
                    this.m_UIPanel = this.m_MapWin.UIPanel.CreatePanel(ButtonNameIdentify, DockStyle.None);
                    if (this.m_UIPanel == null)
                    {
                        return;
                    }
                    this.m_HavePanel = true;
                    this.m_UIPanel.Controls.Add(this.m_shpFilePropfrm.panel1);
                    this.m_shpFilePropfrm.Hide();
                }
                else
                {
                    this.m_UIPanel = this.m_MapWin.UIPanel.CreatePanel(ButtonNameIdentify, DockStyle.None);
                    if (this.m_UIPanel == null)
                    {
                        return;
                    }
                    this.m_HavePanel = true;
                    this.m_UIPanel.Controls.Add(this.m_GridPropfrm.panel1);
                    this.m_GridPropfrm.Hide();
                }
            }
        }

        private Grid FindAssociatedGrid(string fileName)
        {
            var esriManager = new ESRIGridManager();
            var bgdGrid = Path.ChangeExtension(fileName, ".bgd");
            var ascGrid = Path.ChangeExtension(fileName, ".asc");

            Grid grid = new GridClass();

            //check for all grid types to see if one exists
            if (File.Exists(bgdGrid))
            {
                grid.Open(bgdGrid, GridDataType.UnknownDataType, true, GridFileType.UseExtension, null);
                return grid;
            }
            else if (File.Exists(ascGrid))
            {
                grid.Open(ascGrid, GridDataType.UnknownDataType, true, GridFileType.UseExtension, null);
                return grid;
            }
            else if (esriManager.CanUseESRIGrids())
            {
                if (esriManager.IsESRIGrid(fileName))
                {
                    grid.Open(fileName, GridDataType.UnknownDataType, true, GridFileType.UseExtension, null);
                    return grid;
                }
            }

            grid.Close();

            //could not find any info on this image
            // No need to warn -- we'll show pixel values -> MapWinUtility.Logger.Message("Could not find any information about this image", "Identifier", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            return null;
        }

        private void ShowErrorBox(string functionName, string errorMsg)
        {
            Logger.Message(
                "Error in " + functionName + ", Message: " + errorMsg,
                "Identifier",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error,
                DialogResult.OK);
        }

        public void Deactivate()
        {
            this.m_Activated = false;
            if (this.m_HavePanel && this.m_UIPanel != null)
            {
                // m_MapWin.UIPanel.DeletePanel("Identifier");
                // m_HavePanel = false;
                // m_UIPanel = null;
                this.m_MapWin.UIPanel.SetPanelVisible(ButtonNameIdentify, false);
            }
        }

        private void UpdateButtons()
        {
            this.m_IdentBtn.Enabled = (this.m_MapWin.Layers.NumLayers > 0);
            // this.m_IdentByShape.Enabled = (this.m_MapWin.Layers.NumLayers > 0);
        }
    }
}