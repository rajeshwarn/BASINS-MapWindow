// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShapefileEditorClass.cs" company="MapWindow GIS">
//     Copyright (c) MapWindow Development team. All rights reserved.
// </copyright>
// <summary>
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Original Code is MapWindow Open Source. 
//
// The Initial Developer of this version of the Original Code is Daniel P. Ames using portions created by 
// Utah State University and the Idaho National Engineering and Environmental Lab that were released as 
// public domain in March 2004.  
// </summary>
// <changelog>
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// As of 1/29/2005 only one function has been added (CloseAddShapeForm) over the public domain version.
// Date            Changed By     Notes
// June 1 2011     Paul Meems     Added text properties to the toolbar buttons because of the new toolstrips
// June 1 2011     Paul Meems     Started with making changes as suggested by StyleCop and ReSharper
// </changelog>
// --------------------------------------------------------------------------------------------------------------------

namespace ShapefileEditor
{
    #region

    using System;
    using System.Collections;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    using MapWindow.Interfaces;

    using MapWinGIS;

    using ShapefileEditor.Forms;

    using Shape = MapWinGIS.Shape;

    #endregion

    /// <summary>
    /// The class that implement the MapWindow IPlugin interface
    /// </summary>
    public class ShapefileEditorClass : IPlugin
    {
        #region Constants and Fields

        /// <summary>
        /// The m_added buttons.
        /// </summary>
        private readonly Stack addedButtons = new Stack();

        /// <summary>
        /// The m_ add shape class.
        /// </summary>
        private AddShapeClass addShapeClass;

        /// <summary>
        /// The m_ add shapefile class.
        /// </summary>
        private AddShapefileClass addShapefileClass;

        /// <summary>
        /// The m_ add vertex.
        /// </summary>
        private AddVertex addVertex;

        /// <summary>
        /// The m_ insert stock shape class.
        /// </summary>
        private InsertStockShapeClass insertStockShapeClass;

        /// <summary>
        /// The m_ move shape class.
        /// </summary>
        private MoveShapeClass moveShapeClass;

        /// <summary>
        /// The m_ move vertex.
        /// </summary>
        private MoveVertexClass moveVertex;

        /// <summary>
        /// The m_ remove shape class.
        /// </summary>
        private RemoveShapeClass removeShapeClass;

        /// <summary>
        /// The m_ remove vertex.
        /// </summary>
        private RemoveVertex removeVertex;

        /// <summary>
        /// The m_ resize shape class.
        /// </summary>
        private ResizeShapeClass resizeShapeClass;

        /// <summary>
        /// The m_ rotate shape class.
        /// </summary>
        private RotateShapeClass rotateShapeClass;

        /// <summary>
        /// The m_globals.
        /// </summary>
        private GlobalFunctions globals;

        /// <summary>
        /// The resourcemanager
        /// </summary>
        private ResourceManager res;

        #endregion

        #region Properties

        /// <summary>
        ///   Author of the plugin.
        /// </summary>
        public string Author
        {
            get
            {
                return
                    "MapWindow Open Source Team, with significant contributions courtesy of StrateGis Groep (www.StrateGis.nl)";
            }
        }

        /// <summary>
        ///   Date that the plugin was built.
        /// </summary>
        public string BuildDate
        {
            get
            {
                return
                    System.IO.File.GetCreationTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString();
            }
        }

        /// <summary>
        ///   Description of the plugin.
        /// </summary>
        public string Description
        {
            get
            {
                return "Edit and create shapefiles and shape geometry.";
            }
        }

        /// <summary>
        ///   Name of the plugin.
        /// </summary>
        public string Name
        {
            get
            {
                return "Shapefile Editor";
            }
        }

        /// <summary>
        ///   Serial number of the plugin.
        ///   The serial number and the name are tied together.  For each name there is a corresponding serial number.
        /// </summary>
        public string SerialNumber
        {
            get
            {
                return "OCGHOFNASNKGD1C";
            }
        }

        /// <summary>
        ///   Version of the plugin.
        /// </summary>
        public string Version
        {
            get
            {
                var f =
                    System.Diagnostics.FileVersionInfo.GetVersionInfo(
                        System.Reflection.Assembly.GetExecutingAssembly().Location);
                return f.FileMajorPart + "." + f.FileMinorPart + "." + f.FileBuildPart;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The extents fully contained.
        /// </summary>
        /// <param name="Container">
        /// The container.
        /// </param>
        /// <param name="Contained">
        /// The contained.
        /// </param>
        /// <returns>
        /// The extents fully contained.
        /// </returns>
        public static bool ExtentsFullyContained(Extents Container, Extents Contained)
        {
            return Contained.xMin >= Container.xMin && Contained.yMin >= Container.yMin
                    && Contained.xMax <= Container.xMax && Contained.yMax <= Container.yMax;
        }

        /// <summary>
        /// The extents fully contained x.
        /// </summary>
        /// <param name="Container">
        /// The container.
        /// </param>
        /// <param name="Contained">
        /// The contained.
        /// </param>
        /// <returns>
        /// The extents fully contained x.
        /// </returns>
        public static int ExtentsFullyContainedX(Extents Container, Extents Contained)
        {
            if (Contained.xMin >= Container.xMin && Contained.yMin >= Container.yMin && Contained.xMax <= Container.xMax
                && Contained.yMax <= Container.yMax)
            {
                return 0;
            }

            if (Contained.xMin >= Container.xMin)
            {
                return -1;
            }
            else
            {
                return +1;
            }
        }

        /// <summary>
        /// The extents fully contained y.
        /// </summary>
        /// <param name="Container">
        /// The container.
        /// </param>
        /// <param name="Contained">
        /// The contained.
        /// </param>
        /// <returns>
        /// The extents fully contained y.
        /// </returns>
        public static int ExtentsFullyContainedY(Extents Container, Extents Contained)
        {
            if (Contained.yMin >= Container.yMin && Contained.yMin >= Container.yMin && Contained.yMax <= Container.yMax
                && Contained.yMax <= Container.yMax)
            {
                return 0;
            }

            if (Contained.yMin >= Container.yMin)
            {
                return -1;
            }
            else
            {
                return +1;
            }
        }

        /// <summary>
        /// The out of view.
        /// </summary>
        /// <param name="View">
        /// The view.
        /// </param>
        /// <param name="Consider">
        /// The consider.
        /// </param>
        /// <returns>
        /// The out of view.
        /// </returns>
        public static bool OutOfView(Extents View, Extents Consider)
        {
            return (Consider.xMin > View.xMax || Consider.xMax < View.xMin)
                    || (Consider.yMin > View.yMax || Consider.yMax < View.yMin)
                        ? true
                        : false;
        }

        /// <summary>
        /// The out of view x.
        /// </summary>
        /// <param name="View">
        /// The view.
        /// </param>
        /// <param name="Consider">
        /// The consider.
        /// </param>
        /// <returns>
        /// The out of view x.
        /// </returns>
        public static int OutOfViewX(Extents View, Extents Consider)
        {
            if (Consider.xMin > View.xMax)
            {
                return -1;
            }

            if (Consider.xMax < View.xMin)
            {
                return +1;
            }

            return 0;
        }

        /// <summary>
        /// The out of view y.
        /// </summary>
        /// <param name="View">
        /// The view.
        /// </param>
        /// <param name="Consider">
        /// The consider.
        /// </param>
        /// <returns>
        /// The out of view y.
        /// </returns>
        public static int OutOfViewY(Extents View, Extents Consider)
        {
            if (Consider.yMin > View.yMax)
            {
                return -1;
            }

            if (Consider.yMax < View.yMin)
            {
                return +1;
            }

            return 0;
        }

        /// <summary>
        /// The find field.
        /// </summary>
        /// <param name="Name">
        /// The name.
        /// </param>
        /// <param name="sf">
        /// The sf.
        /// </param>
        /// <returns>
        /// The find field.
        /// </returns>
        public int FindField(string Name, ref Shapefile sf)
        {
            for (var z = 0; z < sf.NumFields; z++)
            {
                if (sf.get_Field(z).Name.ToLower() == Name.ToLower())
                {
                    return z;
                }
            }

            return -1;
        }

        /// <summary>
        /// The merge shapes.
        /// </summary>
        /// <returns>
        /// The merge shapes.
        /// </returns>
        public bool MergeShapes()
        {
            var lstSelected = this.globals.MapWin.View.SelectedShapes;
            if (lstSelected.NumSelected < 2)
            {
                MapWinUtility.Logger.Message(
                    "You must select at least two shapes to merge.", 
                    "Merge Shapes", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information, 
                    DialogResult.OK);
                return false;
            }

            // Paul Meems, 26 Oct. 2009, fix for bug #1460
            // Added check for in-memory shapefiles, those have no filename:
            if (!this.globals.MapWin.Layers.IsValidHandle(this.globals.MapWin.Layers.CurrentLayer))
            {
                return false;
            }

            // End modifications Paul Meems, 26 Oct. 2009
            var sl = new SortedList();
            var sf = (Shapefile)
                           this.globals.MapWin.Layers[this.globals.MapWin.Layers.CurrentLayer].GetObject();
            var sfname = sf.Filename;

            var mergeShape = sf.get_Shape(lstSelected[0].ShapeIndex);
            sl.Add(lstSelected[0].ShapeIndex, lstSelected[0].ShapeIndex);

            for (var i = 1; i < lstSelected.NumSelected; i++)
            {
                sl.Add(lstSelected[i].ShapeIndex, lstSelected[i].ShapeIndex);

                var currShape = sf.get_Shape(lstSelected[i].ShapeIndex);

                if (!MapWinGeoProc.SpatialOperations.MergeShapes(ref mergeShape, ref currShape, out mergeShape))
                {
                    MapWinUtility.Logger.Message(
                        "Merge Failed.", "Merge Shapes", MessageBoxButtons.OK, MessageBoxIcon.Error, DialogResult.OK);
                    sf.Close();
                    return false;
                }
            }

            sf.StartEditingShapes(true, null);
            var idxMerged = sf.NumShapes;
            sf.EditInsertShape(mergeShape, ref idxMerged);

            for (var i = 0; i < sf.NumFields; i++)
            {
                sf.EditCellValue(i, idxMerged, sf.get_CellValue(i, lstSelected[0].ShapeIndex));
            }

            this.globals.MapWin.View.ClearSelectedShapes();

            for (var i = sl.Count - 1; i >= 0; i--)
            {
                sf.EditDeleteShape((int)sl.GetByIndex(i));
            }

            sf.StopEditingShapes(true, true, null);

            this.globals.MapWin.Plugins.BroadcastMessage(
                "ShapefileEditor: Layer " + this.globals.MapWin.Layers.CurrentLayer + ": Shapes Merged");

            this.globals.m_MergeForm.Close();
            this.globals.MapWin.View.LockMap();
            this.globals.MapWin.View.LockLegend();
            sf.Close();
            sf.Open(sfname, null);
            this.globals.MapWin.View.UnlockMap();
            this.globals.MapWin.View.UnlockLegend();
            this.globals.MapWin.View.Redraw();

            return true;
        }

        #endregion

        #region Implemented Interfaces

        #region IPlugin

        /// <summary>
        /// This event is called when a plugin is loaded or turned on in the MapWindow.
        /// </summary>
        /// <param name="mapWin">
        /// The interface to use to access the MapWindow.
        /// </param>
        /// <param name="parentHandle">
        /// The window handle of the MapWindow form.  This handle is useful for making the MapWindow the owner of plugin forms.
        /// </param>
        public void Initialize(IMapWin mapWin, int parentHandle)
        {
            const string Category = "Shapefile Editor";

            this.globals = new GlobalFunctions(mapWin, (Form)Control.FromHandle(new IntPtr(parentHandle)));
            this.globals.SetLogFile();

            this.res = new ResourceManager(
                "ShapefileEditor.Resource", System.Reflection.Assembly.GetExecutingAssembly());

            var t = mapWin.Toolbar;
            t.AddToolbar(GlobalFunctions.c_ToolbarName);

            // Why is this:
            if (GlobalFunctions.c_ToolbarName.Length > 0)
            {
                t.AddToolbar(GlobalFunctions.c_ToolbarName);
            }

            // Clear the stack
            this.addedButtons.Clear();

            var b = t.AddButton(GlobalFunctions.c_NewButton, GlobalFunctions.c_ToolbarName, string.Empty, string.Empty);
            b.Tooltip = this.res.GetString("tooltipCreateShp");
            b.Category = Category;
            b.Picture = new Icon(this.GetType(), "NewShapefile.ico");
            b.Text = this.res.GetString("textCreateShp");
            this.addedButtons.Push(GlobalFunctions.c_NewButton);

            // Chris Michaelis 12/28/2006
            b = t.AddButton(GlobalFunctions.c_InsertStockShapeButton, GlobalFunctions.c_ToolbarName, string.Empty, string.Empty);
            b.BeginsGroup = true;
            b.Tooltip = this.res.GetString("tooltipAddRegularShp");
            b.Category = Category;
            b.Picture = new Icon(this.GetType(), "InsertStockShape.ico");
            b.Text = this.res.GetString("textAddRegularShp");
            this.addedButtons.Push(GlobalFunctions.c_InsertStockShapeButton);

            b = t.AddButton(GlobalFunctions.c_AddShapeButton, GlobalFunctions.c_ToolbarName, string.Empty, string.Empty);
            b.Tooltip = this.res.GetString("tooltipAddShp");
            b.Category = Category;
            b.Picture = new Icon(this.GetType(), "AddShape.ico");
            b.Text = this.res.GetString("textAddShp");
            this.addedButtons.Push(GlobalFunctions.c_AddShapeButton);

            b = t.AddButton(GlobalFunctions.c_RemoveShapeButton, GlobalFunctions.c_ToolbarName, string.Empty, string.Empty);
            b.Tooltip = this.res.GetString("tooltipRemoveShp");
            b.Category = Category;
            b.Picture = new Icon(this.GetType(), "RemoveShape.ico");
            b.Text = this.res.GetString("textRemoveShp");
            this.addedButtons.Push(GlobalFunctions.c_RemoveShapeButton);

            b = t.AddButton(GlobalFunctions.c_CopyShapeButton, GlobalFunctions.c_ToolbarName, string.Empty, string.Empty);
            b.BeginsGroup = true;
            b.Tooltip = this.res.GetString("tooltipCopyShpToClipboard");
            b.Category = Category;
            b.Picture = new Icon(this.GetType(), "copy.ico");
            b.Text = this.res.GetString("textCopyShpToClipboard");
            this.addedButtons.Push(GlobalFunctions.c_CopyShapeButton);

            b = t.AddButton(GlobalFunctions.c_PasteShapeButton, GlobalFunctions.c_ToolbarName, string.Empty, string.Empty);
            b.BeginsGroup = true;
            b.Tooltip = this.res.GetString("tooltipPasteShpFromClipboard");
            b.Category = Category;
            b.Picture = new Icon(this.GetType(), "paste.ico");
            b.Text = this.res.GetString("textPasteShpFromClipboard");
            this.addedButtons.Push(GlobalFunctions.c_PasteShapeButton);

            // ARA 04/02/07
            b = t.AddButton(GlobalFunctions.c_MergeShapesButton, GlobalFunctions.c_ToolbarName, string.Empty, string.Empty);
            b.BeginsGroup = true;
            b.Tooltip = this.res.GetString("tooltipMergeShp");
            b.Category = Category;
            b.Picture = new Icon(this.GetType(), "mergeShapes.ico");
            b.Text = this.res.GetString("textMergeShp");
            this.addedButtons.Push(GlobalFunctions.c_MergeShapesButton);

            b = t.AddButton(GlobalFunctions.c_EraseWithShapeButton, GlobalFunctions.c_ToolbarName, string.Empty, string.Empty);
            b.BeginsGroup = true;
            b.Tooltip = this.res.GetString("tooltipEraseLayerShp");
            b.Category = Category;
            b.Picture = new Icon(this.GetType(), "erase.ico");
            b.Text = this.res.GetString("textEraseLayerShp");
            this.addedButtons.Push(GlobalFunctions.c_EraseWithShapeButton);

            b = t.AddButton(GlobalFunctions.c_EraseBeneathShapeButton, GlobalFunctions.c_ToolbarName, string.Empty, string.Empty);
            b.BeginsGroup = true;
            b.Tooltip = this.res.GetString("tooltipEraseLayerBeneathShp");
            b.Category = Category;
            b.Picture = new Icon(this.GetType(), "createIsland.ico");
            b.Text = this.res.GetString("textEraseLayerBeneathShp");
            this.addedButtons.Push(GlobalFunctions.c_EraseBeneathShapeButton);

            ////////////////////////////////////////////////////////////////////////////////////////////////
            // D.S. Modiwirijo 
            // Moveshapes added.
            ////////////////////////////////////////////////////////////////////////////////////////////////
            b = t.AddButton(GlobalFunctions.c_MoveShapesButton, GlobalFunctions.c_ToolbarName, string.Empty, string.Empty);
            b.BeginsGroup = true;
            b.Tooltip = this.res.GetString("tooltipMoveShp");
            b.Category = Category;
            b.Picture = new Icon(this.GetType(), "Moveshapes.ico");
            b.Text = this.res.GetString("textMoveShp");
            this.addedButtons.Push(GlobalFunctions.c_MoveShapesButton);

            // Chris M 12/21/2006
            b = t.AddButton(GlobalFunctions.c_RotateShapeButton, GlobalFunctions.c_ToolbarName, string.Empty, string.Empty);
            b.BeginsGroup = true;
            b.Tooltip = this.res.GetString("tooltipRotateShp");
            b.Category = Category;
            b.Picture = new Icon(this.GetType(), "rotate.ico");
            b.Text = this.res.GetString("textRotateShp");
            this.addedButtons.Push(GlobalFunctions.c_RotateShapeButton);

            // Chris M 12/28/2006
            b = t.AddButton(GlobalFunctions.c_ResizeShapeButton, GlobalFunctions.c_ToolbarName, string.Empty, string.Empty);
            b.BeginsGroup = true;
            b.Tooltip = this.res.GetString("tooltipResizeShp");
            b.Category = Category;
            b.Picture = new Icon(this.GetType(), "resize.ico");
            b.Text = this.res.GetString("textResizeShp");
            this.addedButtons.Push(GlobalFunctions.c_ResizeShapeButton);

            b = t.AddButton(GlobalFunctions.c_MoveVertexButton, GlobalFunctions.c_ToolbarName, string.Empty, string.Empty);
            b.BeginsGroup = true;
            b.Tooltip = this.res.GetString("tooltipMoveVertex");
            b.Category = Category;
            b.Picture = new Icon(this.GetType(), "MoveVertex.ico");
            b.Text = this.res.GetString("textMoveVertex");
            this.addedButtons.Push(GlobalFunctions.c_MoveVertexButton);

            b = t.AddButton(GlobalFunctions.c_AddVertexButton, GlobalFunctions.c_ToolbarName, string.Empty, string.Empty);
            b.Tooltip = this.res.GetString("tooltipAddVertex");
            b.Category = Category;
            b.Picture = new Icon(this.GetType(), "AddVertex.ico");
            b.Text = this.res.GetString("textAddVertex");
            this.addedButtons.Push(GlobalFunctions.c_AddVertexButton);

            b = t.AddButton(GlobalFunctions.c_RemoveVertexButton, GlobalFunctions.c_ToolbarName, string.Empty, string.Empty);
            b.Tooltip = this.res.GetString("tooltipRemoveVertex");
            b.Category = Category;
            b.Picture = new Icon(this.GetType(), "RemoveVertex.ico");
            b.Text = this.res.GetString("textRemoveVertex");
            this.addedButtons.Push(GlobalFunctions.c_RemoveVertexButton);

            b = t.AddButton(GlobalFunctions.c_CleanupButton, GlobalFunctions.c_ToolbarName, string.Empty, string.Empty);
            b.Tooltip = this.res.GetString("tooltipCheckCleanShp");
            b.Category = Category;
            b.Picture = new Icon(this.GetType(), "cleanupcheck.ico");
            b.Text = this.res.GetString("textCheckCleanShp");
            this.addedButtons.Push(GlobalFunctions.c_CleanupButton);

            b = t.AddButton(GlobalFunctions.c_UndoDropdownButton, GlobalFunctions.c_ToolbarName, true);
            b.Text = string.Empty;
            b.Tooltip = this.res.GetString("tooltipUndo");
            b.Category = Category;
            b.Picture = new Icon(this.GetType(), "undo.ico");
            b.Text = this.res.GetString("textUndo");
            this.addedButtons.Push(GlobalFunctions.c_UndoDropdownButton);

            b = t.AddButton(
                GlobalFunctions.c_UndoLastChangeButton, 
                GlobalFunctions.c_ToolbarName, 
                GlobalFunctions.c_UndoDropdownButton, 
                string.Empty);
            b.Tooltip = this.res.GetString("tooltipUndoLastChange");
            b.Text = this.res.GetString("textUndoLastChange");
            b.Category = Category;
            b.Text = this.res.GetString("textUndoLastChange");
            this.addedButtons.Push(GlobalFunctions.c_UndoLastChangeButton);

            b = t.AddButton(
                GlobalFunctions.c_UndoEnableButton, 
                GlobalFunctions.c_ToolbarName, 
                GlobalFunctions.c_UndoDropdownButton, 
                string.Empty);
            b.Tooltip = this.res.GetString("tooltipEnableUndo");
            b.Text = this.res.GetString("textEnableUndo");
            b.Category = Category;
            this.addedButtons.Push(GlobalFunctions.c_UndoEnableButton);

            b = t.AddButton(
                GlobalFunctions.c_UndoDisableButton, 
                GlobalFunctions.c_ToolbarName, 
                GlobalFunctions.c_UndoDropdownButton, 
                string.Empty);
            b.Tooltip = this.res.GetString("tooltipDisableUndo");
            b.Text = this.res.GetString("textDisableUndo");
            b.Enabled = false;
            b.Category = Category;
            this.addedButtons.Push(GlobalFunctions.c_UndoDisableButton);

            var m = mapWin.Menus;

            var i = m.AddMenu(GlobalFunctions.c_MenuName);
            i.Text = this.res.GetString("mnuShpEditor");
            i.Category = "Shapefile Editor";

            i = m.AddMenu(GlobalFunctions.c_ShowVerticesButton, GlobalFunctions.c_MenuName);
            i.Text = this.res.GetString("mnuShowVertices");
            i.Checked = false;
            i.Category = "Shapefile Editor";
            this.globals.MapWin.Menus[GlobalFunctions.c_ShowVerticesButton].Checked = true;

            i = m.AddMenu(GlobalFunctions.c_SnapToVerticesButton, GlobalFunctions.c_MenuName);
            i.Text = this.res.GetString("mnuSnapToVertices");
            i.Checked = true;
            i.Category = "Shapefile Editor";

            i = m.AddMenu(GlobalFunctions.c_SnapToAllLayersButton, GlobalFunctions.c_MenuName);
            i.Text = this.res.GetString("mnuSnapToAllLayers");
            i.Checked = false;
            i.Category = "Shapefile Editor";

            i = m.AddMenu(GlobalFunctions.c_StayInAddModeButton, GlobalFunctions.c_MenuName);
            i.Text = this.res.GetString("mnuStayInAddMode");
            i.Checked = false;
            i.Category = "Shapefile Editor";

            this.globals.SaveCusorMode();
            this.globals.DoEnables();

            if (this.globals.MapWin.Layers.NumLayers > 0)
            {
                this.globals.UpdateCurrentLayer();
            }

            this.addShapefileClass = new AddShapefileClass(this.globals);
            this.addShapeClass = new AddShapeClass(this.globals);
            this.moveShapeClass = new MoveShapeClass(this.globals);
            this.rotateShapeClass = new RotateShapeClass(this.globals);
            this.resizeShapeClass = new ResizeShapeClass(this.globals);
            this.insertStockShapeClass = new InsertStockShapeClass(this.globals);
            this.removeShapeClass = new RemoveShapeClass(this.globals);
            this.addVertex = new AddVertex(this.globals);
            this.moveVertex = new MoveVertexClass(this.globals);
            this.removeVertex = new RemoveVertex(this.globals);

            this.globals.Events.FireInitializeEvent(mapWin, parentHandle);
        }

        /// <summary>
        /// Occurs when a user clicks on a toolbar button or menu item.
        /// </summary>
        /// <param name="ItemName">
        /// The name of the item clicked on.
        /// </param>
        /// <param name="Handled">
        /// Reference parameter.  Setting Handled to true prevents other plugins from receiving this event.
        /// </param>
        public void ItemClicked(string ItemName, ref bool Handled)
        {
            try
            {
                // Chris M -- This is no longer necessary here as of MW 4.2, it's a little stricter on enforcing the button mode just clicked when coming out of a plug-in button mode
                // if (m_globals.CurrentMode != GlobalFunctions.Modes.None)
                // 	m_globals.RestoreCursorMode();
                this.globals.CurrentMode = GlobalFunctions.Modes.None;
                this.globals.Events.FireItemClickedEvent(ItemName, ref Handled);
                this.globals.LogWrite("ItemClicked:" + ItemName);
                switch (ItemName)
                {
                    case GlobalFunctions.c_CopyShapeButton:
                        this.DoCopyShape();
                        Handled = true;
                        break;

                    case GlobalFunctions.c_PasteShapeButton:
                        this.DoPasteShape();
                        Handled = true;
                        break;

                        // ARA 04/03/07
                    case GlobalFunctions.c_MergeShapesButton:
                        this.DoMergeShape();
                        Handled = true;
                        break;
                    case GlobalFunctions.c_EraseWithShapeButton:
                        this.DoEraseWithShape(false);
                        Handled = true;
                        break;
                    case GlobalFunctions.c_EraseBeneathShapeButton:
                        this.DoEraseWithShape(true);
                        Handled = true;
                        break;

                    case GlobalFunctions.c_UndoDropdownButton:

                        // No action needed for outer dropdown button
                        Handled = true;
                        break;

                    case GlobalFunctions.c_UndoDisableButton:
                        this.globals.UndoStack.Clear();
                        this.globals.MapWin.Toolbar.ButtonItem(GlobalFunctions.c_UndoDisableButton).Enabled = false;
                        this.globals.MapWin.Toolbar.ButtonItem(GlobalFunctions.c_UndoLastChangeButton).Enabled = false;
                        this.globals.MapWin.Toolbar.ButtonItem(GlobalFunctions.c_UndoEnableButton).Enabled = true;
                        Handled = true;
                        break;

                    case GlobalFunctions.c_UndoEnableButton:
                        this.globals.UndoStack.Clear();
                        this.globals.MapWin.Toolbar.ButtonItem(GlobalFunctions.c_UndoDisableButton).Enabled = true;
                        this.globals.MapWin.Toolbar.ButtonItem(GlobalFunctions.c_UndoLastChangeButton).Enabled = false;
                            
                            // Disabled until something can be undone
                        this.globals.MapWin.Toolbar.ButtonItem(GlobalFunctions.c_UndoEnableButton).Enabled = false;
                        Handled = true;
                        break;

                    case GlobalFunctions.c_UndoLastChangeButton:
                        this.globals.UndoLastChange();
                        Handled = true;
                        break;

                    case GlobalFunctions.c_CleanupButton:
                        var frm =
                            new CleanupForm(
                                (MapWinGIS.Shapefile)
                                this.globals.MapWin.Layers[this.globals.MapWin.Layers.CurrentLayer].GetObject(), 
                                this.globals);
                        frm.ShowDialog();

                        this.globals.UpdateView();
                        Handled = true;
                        break;

                    case GlobalFunctions.c_GridSnapButton:
                        Handled = true;
                        break;

                    case GlobalFunctions.c_ShowVerticesButton:
                        this.globals.MapWin.Menus[GlobalFunctions.c_ShowVerticesButton].Checked =
                            !this.globals.MapWin.Menus[GlobalFunctions.c_ShowVerticesButton].Checked;

                        Handled = true;
                        break;
                    case GlobalFunctions.c_SnapToAllLayersButton:
                        this.globals.MapWin.Menus[GlobalFunctions.c_SnapToAllLayersButton].Checked =
                            !this.globals.MapWin.Menus[GlobalFunctions.c_SnapToAllLayersButton].Checked;

                        Handled = true;
                        break;
                    case GlobalFunctions.c_SnapOptionsButton:
                        Handled = true;
                        break;
                    case GlobalFunctions.c_SnapToGridButton:
                        Handled = true;
                        break;
                    case GlobalFunctions.c_SnapToVerticesButton:
                        this.globals.MapWin.Menus[GlobalFunctions.c_SnapToVerticesButton].Checked =
                            !this.globals.MapWin.Menus[GlobalFunctions.c_SnapToVerticesButton].Checked;

                        Handled = true;
                        break;
                    case GlobalFunctions.c_VertexSizeButton:
                        Handled = true;
                        break;

                    case GlobalFunctions.c_StayInAddModeButton:
                        Handled = true;
                        GlobalFunctions.m_StayInAddMode =
                            !this.globals.MapWin.Menus[GlobalFunctions.c_StayInAddModeButton].Checked;
                        this.globals.MapWin.Menus[GlobalFunctions.c_StayInAddModeButton].Checked =
                            GlobalFunctions.m_StayInAddMode;
                        if (!GlobalFunctions.m_StayInAddMode)
                        {
                            if (this.addShapeClass != null)
                            {
                                this.addShapeClass.CloseAddShapeForm();
                            }
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                this.globals.MapWin.ShowErrorDialog(ex);
            }
        }

        /// <summary>
        /// Occurs when a layer is removed from the MapWindow.
        /// </summary>
        /// <param name="Handle">
        /// The handle of the layer being removed.
        /// </param>
        public void LayerRemoved(int Handle)
        {
            this.globals.RestoreCursorMode();
            this.globals.CurrentMode = GlobalFunctions.Modes.None;
            this.globals.DoEnables();
            this.globals.UpdateButtons();
            this.globals.Events.FireLayerRemovedEvent(Handle);
        }

        /// <summary>
        /// Called when a layer is selected or made to be the active layer.
        /// </summary>
        /// <param name="Handle">
        /// The layer handle of the newly selected layer.
        /// </param>
        public void LayerSelected(int Handle)
        {
            this.globals.UpdateCurrentLayer();

            this.globals.RestoreCursorMode();
            this.globals.CurrentMode = GlobalFunctions.Modes.None;
            this.globals.DoEnables();
            this.globals.UpdateButtons();

            // 			m_globals.Events.FireLayerSelectedEvent(Handle);
        }

        /// <summary>
        /// Called when 1 or more layers are added to the MapWindow.
        /// </summary>
        /// <param name="Layers">
        /// Array of layer objects containing references to all layers added.
        /// </param>
        public void LayersAdded(Layer[] Layers)
        {
            this.globals.Events.FireLayersAddedEvent(Layers);
        }

        /// <summary>
        /// Occurs when the "Clear all layers" button is pressed in the MapWindow.
        /// </summary>
        public void LayersCleared()
        {
            this.globals.RestoreCursorMode();
            this.globals.CurrentMode = GlobalFunctions.Modes.None;
            this.globals.DoEnables();
            this.globals.UpdateButtons();
            this.globals.Events.FireLayersClearedEvent();
        }

        /// <summary>
        /// Occurs when a user double clicks on the legend.
        /// </summary>
        /// <param name="Handle">
        /// The handle of the legend group or item that was clicked on.
        /// </param>
        /// <param name="Location">
        /// Enumerated.  The location clicked on.
        /// </param>
        /// <param name="Handled">
        /// Reference parameter.  When set to true it prevents additional plugins from getting this event.
        /// </param>
        public void LegendDoubleClick(int Handle, ClickLocation Location, ref bool Handled)
        {
            this.globals.Events.FireLegendDoubleClickEvent(Handle, Location, ref Handled);
        }

        /// <summary>
        /// Occurs when a user presses a mouse button on the legend.
        /// </summary>
        /// <param name="Handle">
        /// Layer or group handle that was clicked.
        /// </param>
        /// <param name="Button">
        /// The integer representation of the button used.  Uses vb6 mouse button constants.
        /// </param>
        /// <param name="Location">
        /// The part of the legend that was clicked.
        /// </param>
        /// <param name="Handled">
        /// Reference parameter.  Prevents other plugins from getting this event when set to true.
        /// </param>
        public void LegendMouseDown(
            int Handle, int Button, ClickLocation Location, ref bool Handled)
        {
            this.globals.Events.FireLegendMouseDownEvent(Handle, Button, Location, ref Handled);
        }

        /// <summary>
        /// Occurs when the user releases a mouse button over the legend.
        /// </summary>
        /// <param name="Handle">
        /// The handle of the group or layer.
        /// </param>
        /// <param name="Button">
        /// The integer representation of the button released.  Uses vb6 button constants.
        /// </param>
        /// <param name="Location">
        /// Enumeration.  Specifies if a group, layer or neither was clicked on.
        /// </param>
        /// <param name="Handled">
        /// Reference parameter.  Prevents other plugins from getting this event.
        /// </param>
        public void LegendMouseUp(int Handle, int Button, ClickLocation Location, ref bool Handled)
        {
            this.globals.Events.FireLegendMouseUpEvent(Handle, Button, Location, ref Handled);
        }

        /// <summary>
        /// Occurs after a user selects a rectangular area in the MapWindow.  Normally this implies selection.
        /// </summary>
        /// <param name="Bounds">
        /// The rectangle selected.
        /// </param>
        /// <param name="Handled">
        /// Reference parameter.  Setting Handled to true prevents other plugins from receiving this event.
        /// </param>
        public void MapDragFinished(Rectangle Bounds, ref bool Handled)
        {
            this.globals.Events.FireMapDragFinishedEvent(Bounds, ref Handled);
        }

        /// <summary>
        /// Method that is called when the MapWindow extents change.
        /// </summary>
        public void MapExtentsChanged()
        {
            this.globals.Events.FireMapExtentsChangedEvent();
        }

        /// <summary>
        /// Occurs when the user presses a mouse button on the MapWindow map display.
        /// </summary>
        /// <param name="Button">
        /// The integer representation of the button pressed by the user.  This parameter uses the vb6 mouse button constants (vbLeftButton, etc.).
        /// </param>
        /// <param name="Shift">
        /// The integer representation of which shift/alt/control keys are pressed. This parameter uses the vb6 shift constants.
        /// </param>
        /// <param name="x">
        /// X coordinate in pixels.
        /// </param>
        /// <param name="y">
        /// Y coordinate in pixels.
        /// </param>
        /// <param name="Handled">
        /// Reference parameter.  When set to true, no other plugins will receive this event.
        /// </param>
        public void MapMouseDown(int Button, int Shift, int x, int y, ref bool Handled)
        {
            this.globals.Events.FireMapMouseDownEvent(Button, Shift, x, y, ref Handled);

            ////m_globals.LogWrite("m_globals.CurrentMode = " + m_globals.CurrentMode.ToString());
        }

        /// <summary>
        /// Occurs when a user moves the mouse over the MapWindow main display.
        /// </summary>
        /// <param name="ScreenX">
        /// X coordinate in pixels.
        /// </param>
        /// <param name="ScreenY">
        /// Y coordinate in pixels.
        /// </param>
        /// <param name="Handled">
        /// Reference parameter.  Prevents other plugins from getting this event.
        /// </param>
        public void MapMouseMove(int ScreenX, int ScreenY, ref bool Handled)
        {
            ////if (m_globals.CurrentLayer != null && m_blnMoveshapes && m_blnMousedown)
            ////{
            // ////MovePolygons(ScreenX, ScreenY);
            // ////m_dblInitX = m_dblDestinyX;
            // ////m_dblInitY = m_dblDestinyY;
            ////}			
            this.globals.Events.FireMapMouseMoveEvent(ScreenX, ScreenY, ref Handled);
        }

        /// <summary>
        /// Occurs when a user releases a mouse button on the MapWindow main map display.
        /// </summary>
        /// <param name="Button">
        /// An integer representation of which button(s) were released.  Uses vb6 button constants.
        /// </param>
        /// <param name="Shift">
        /// An integer representation of the shift/alt/ctrl keys that were pressed at the time the mouse button was released.  Uses vb6 shift constants.
        /// </param>
        /// <param name="x">
        /// X coordinate in pixels.
        /// </param>
        /// <param name="y">
        /// Y coordinate in pixels.
        /// </param>
        /// <param name="Handled">
        /// Reference parameter.  Prevents other plugins from getting this event.
        /// </param>
        public void MapMouseUp(int Button, int Shift, int x, int y, ref bool Handled)
        {
            ////m_blnMousedown = false;
            ////m_globals.LogWrite("MapMouseUp");
            this.globals.Events.FireMapMouseUpEvent(Button, Shift, x, y, ref Handled);

            try
            {
                switch (this.globals.CurrentMode)
                {
                        // TODO:
                    case GlobalFunctions.Modes.AddShape:

                        // 						if (Button == (int)MapWindow.Interfaces.vb6Buttons.Left)
                        // 						{
                        // 							// Add this point to the current shape.
                        // 							double mapX = 0, mapY = 0;
                        // 							m_globals.MapWin.View.PixelToProj(x,y,ref mapX, ref mapY);
                        // 							m_AddShapeForm.AddPoint(mapX, mapY);
                        // 						}
                        // 						else if(Button == (int)MapWindow.Interfaces.vb6Buttons.Right)
                        // 						{
                        // 							// Finish this shape (this really only applies to polygon/line shapes but should work with point shapes too).
                        // 							m_AddShapeForm.Close();
                        // 							MapWinGIS.Shape shp = m_AddShapeForm.GetNewShape;
                        // 							if (m_CurrentShapefile != null)
                        // 							{
                        // 								if(m_CurrentShapefile.EditingShapes == false)
                        // 									m_CurrentShapefile.StartEditingShapes(false,null);
                        // 								int newIndex = m_CurrentShapefile.NumShapes;
                        // 								m_CurrentShapefile.EditInsertShape(shp, ref newIndex);
                        // 							}		
                        // 							else // the current layer got changed without me knowing somehow.  Set the current mode to none.
                        // 							{
                        // 								m_globals.CurrentMode = GlobalFunctions.Modes.None;
                        // 								m_globals.DoEnables();
                        // 							}
                        // 						}
                        break;

                        // end case Modes.AddShape:
                    default:
                        break;
                }
            }
            catch
            {
                this.globals.CurrentMode = GlobalFunctions.Modes.None;
                this.globals.DoEnables();
            }
        }

        /// <summary>
        /// Method used by plugins to communicate with other plugins.
        /// </summary>
        /// <param name="msg">
        /// The messsage being recieved.
        /// </param>
        /// <param name="Handled">
        /// Reference parameter.  Set thist to true if this plugin handles recieving the message.  When set to true, no other plugins will receive the message.
        /// </param>
        public void Message(string msg, ref bool Handled)
        {
            if (this.globals != null)
            {
                this.globals.Events.FireMessageEvent(msg, ref Handled);
            }
        }

        /// <summary>
        /// Called when the MapWindow loads a new project.
        /// </summary>
        /// <param name="ProjectFile">
        /// The filename of the project file being loaded.
        /// </param>
        /// <param name="SettingsString">
        /// The settings string that was saved in the project file for this plugin.
        /// </param>
        public void ProjectLoading(string ProjectFile, string SettingsString)
        {
            this.globals.RestoreCursorMode();
            this.globals.CurrentMode = GlobalFunctions.Modes.None;
            this.globals.DoEnables();
            this.globals.UpdateButtons();
            this.globals.Events.FireProjectLoadingEvent(ProjectFile, SettingsString);
        }

        /// <summary>
        /// ProjectSaving is called when the MapWindow saves a project.  This is a good chance for the plugin to save any custom settings and create a SettingsString to place in the project file.
        /// </summary>
        /// <param name="ProjectFile">
        /// The name of the project file being saved.
        /// </param>
        /// <param name="SettingsString">
        /// Reference parameter.  The settings string that will be saved in the project file for this plugin.
        /// </param>
        public void ProjectSaving(string ProjectFile, ref string SettingsString)
        {
            this.globals.Events.FireProjectSavingEvent(ProjectFile, ref SettingsString);
        }

        /// <summary>
        /// Occurs when shapes have been selected in the MapWindow.
        /// </summary>
        /// <param name="Handle">
        /// The handle of the layer that was selected on.
        /// </param>
        /// <param name="SelectInfo">
        /// Information about all the shapes that were selected.
        /// </param>
        public void ShapesSelected(int Handle, SelectInfo SelectInfo)
        {
            this.globals.Events.FireShapesSelectedEvent(Handle, SelectInfo);
            this.globals.DoEnables();

            // TODO:
            this.globals.Events.FireLayersClearedEvent();
        }

        /// <summary>
        /// This method is called when a plugin is unloaded.  The plugin should remove all toolbars, buttons and menus that it added.
        /// </summary>
        public void Terminate()
        {
            this.globals.MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmPan;
            this.globals.Events.FireTerminateEvent();

            var t = this.globals.MapWin.Toolbar;

            while (this.addedButtons.Count > 0)
            {
                try
                {
                    t.RemoveButton((string)this.addedButtons.Pop());
                }
                catch
                {
                }
            }

            if (GlobalFunctions.c_ToolbarName.Length > 0)
            {
                t.RemoveToolbar(GlobalFunctions.c_ToolbarName);
            }

            var m = this.globals.MapWin.Menus;
            m.Remove(GlobalFunctions.c_ShowVerticesButton);
            m.Remove(GlobalFunctions.c_SnapToVerticesButton);
            m.Remove(GlobalFunctions.c_SnapToAllLayersButton);
            m.Remove(GlobalFunctions.c_SnapToGridButton);
            m.Remove(GlobalFunctions.c_SnapOptionsButton);
            m.Remove(GlobalFunctions.c_GridSnapButton);
            m.Remove(GlobalFunctions.c_MenuName);

            t = null;
            m = null;
            this.globals = null;
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// The do copy shape.
        /// </summary>
        private void DoCopyShape()
        {
            var copystring = "MWShapeCopy:";
            for (var i = 0; i < this.globals.MapWin.View.SelectedShapes.NumSelected; i++)
            {
                // Paul Meems
                // Why get the shapefile for every selected shape?
                // It is always the same layer so alwaus the same shapefile.
                var sf =
                    (MapWinGIS.Shapefile)
                    this.globals.MapWin.Layers[this.globals.MapWin.Layers.CurrentLayer].GetObject();
                copystring += sf.get_Shape(this.globals.MapWin.View.SelectedShapes[i].ShapeIndex).SerializeToString();

                copystring += "FIELDS||";
                for (var j = 0; j < sf.NumFields; j++)
                {
                    copystring += sf.get_Field(j).Name + "|"
                                  + sf.get_CellValue(j, this.globals.MapWin.View.SelectedShapes[i].ShapeIndex) + "||";
                }

                copystring += "\n";
            }

            System.Windows.Forms.Clipboard.SetText(copystring);
        }

        /// <summary>
        /// The do erase with shape.
        /// </summary>
        /// <param name="KeepSelectedShape">
        /// The keep selected shape.
        /// </param>
        private void DoEraseWithShape(bool KeepSelectedShape)
        {
            var sf = new Shapefile();
            
            var tmpSF = new Shapefile();
            Shape currSelected;
            Layer currLyr;
            Extents currExt;
            ShapefileColorScheme currCS;
            string sfname, tmpsfName, tmpLyrProp, lyrName, projstr;
            int lyrGlobalPos, lyrGroupHandle, lyrGroupPos, currSelectedIdx, numShapes;
            var cellVals = new ArrayList();
            var undoState = this.globals.MapWin.Toolbar.ButtonItem(GlobalFunctions.c_UndoDisableButton).Enabled;

            this.globals.CreateUndoPoint();

            currExt = this.globals.MapWin.View.Extents;
            currLyr = this.globals.MapWin.Layers[this.globals.MapWin.Layers.CurrentLayer];
            lyrName = currLyr.Name;
            sfname = currLyr.FileName;
            tmpsfName = System.IO.Path.ChangeExtension(sfname, "tmp.shp");
            tmpLyrProp = System.IO.Path.ChangeExtension(sfname, "temp.mwsr");

            lyrGlobalPos = currLyr.GlobalPosition;
            lyrGroupHandle = currLyr.GroupHandle;
            lyrGroupPos = currLyr.GroupPosition;
            currCS = (MapWinGIS.ShapefileColorScheme)currLyr.ColoringScheme;
            currLyr.SaveShapeLayerProps(tmpLyrProp);

            sf.Open(sfname, null);
            numShapes = sf.NumShapes;
            projstr = sf.Projection;
            currSelectedIdx = this.globals.MapWin.View.SelectedShapes[0].ShapeIndex;
            currSelected = sf.get_Shape(currSelectedIdx);

            if (KeepSelectedShape)
            {
                cellVals.Clear();
                for (var i = 0; i < sf.NumFields; i++)
                {
                    cellVals.Add(sf.get_CellValue(i, currSelectedIdx));
                }
            }

            this.globals.MapWin.View.LockMap();
            this.globals.MapWin.View.LockLegend();
            this.globals.MapWin.Layers.Remove(currLyr.Handle);

            if (MapWinGeoProc.SpatialOperations.Erase(ref sfname, ref currSelected, ref tmpsfName, true, true))
            {
                sf.Close();
                MapWinGeoProc.DataManagement.DeleteShapefile(ref sfname);
                tmpSF.Open(tmpsfName, null);
                tmpSF.Projection = projstr;

                tmpSF.StartEditingShapes(true, null);

                // tmpSF.EditDeleteShape(0);
                if (KeepSelectedShape)
                {
                    currSelectedIdx = tmpSF.NumShapes;
                    tmpSF.EditInsertShape(currSelected, ref currSelectedIdx);
                    for (var i = 0; i < tmpSF.NumFields; i++)
                    {
                        tmpSF.EditCellValue(i, currSelectedIdx, cellVals[i]);
                    }
                }

                tmpSF.StopEditingShapes(true, true, null);

                if (!tmpSF.SaveAs(sfname, null))
                {
                    MapWinUtility.Logger.Message(
                        "Saving the temporary file failed.", 
                        "Erase With Current Shape", 
                        System.Windows.Forms.MessageBoxButtons.OK, 
                        MessageBoxIcon.Error, 
                        DialogResult.OK);
                }

                tmpSF.Close();
                MapWinGeoProc.DataManagement.DeleteShapefile(ref tmpsfName);
            }
            else
            {
                MapWinUtility.Logger.Message(
                    "The erase function failed.", 
                    "Erase With Current Shape", 
                    System.Windows.Forms.MessageBoxButtons.OK, 
                    MessageBoxIcon.Error, 
                    DialogResult.OK);
            }

            currLyr = this.globals.MapWin.Layers.Add(sfname, lyrName);
            currLyr.LoadShapeLayerProps(tmpLyrProp);
            currLyr.GlobalPosition = lyrGlobalPos;
            currLyr.GroupHandle = lyrGroupHandle;
            currLyr.GroupPosition = lyrGroupPos;
            currLyr.ColoringScheme = currCS;
            System.IO.File.Delete(tmpLyrProp);

            if (undoState)
            {
                this.globals.MapWin.Toolbar.ButtonItem(GlobalFunctions.c_UndoDisableButton).Enabled = true;
                this.globals.MapWin.Toolbar.ButtonItem(GlobalFunctions.c_UndoEnableButton).Enabled = false;
            }

            this.globals.MapWin.View.Extents = currExt;
            this.globals.MapWin.View.UnlockMap();
            this.globals.MapWin.View.UnlockLegend();
            this.globals.MapWin.View.Redraw();
        }

        /// <summary>
        /// The do merge shape.
        /// </summary>
        private void DoMergeShape()
        {
            this.globals.CreateUndoPoint();
            this.globals.m_MergeForm = new frmMergeShapes();

            this.globals.m_MergeForm.pluginClass = this;
            this.globals.m_MergeForm.Show();

            this.globals.MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmSelection;
        }

        /// <summary>
        /// The do paste shape.
        /// </summary>
        private void DoPasteShape()
        {
            var cptext = System.Windows.Forms.Clipboard.GetText();
            if (!cptext.StartsWith("MWShapeCopy:"))
            {
                return;
            }

            // Paul Meems, 26 Oct. 2009, fix for bug #1460
            // Added check for in-memory shapefiles, those have no filename:
            if (!this.globals.MapWin.Layers.IsValidHandle(this.globals.MapWin.Layers.CurrentLayer))
            {
                return;
            }

            // End modifications Paul Meems, 26 Oct. 2009
            cptext = cptext.Replace("MWShapeCopy:", string.Empty);
            var shapes = cptext.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var added = new ArrayList();
            if (shapes.Length > 0)
            {
                var sf =
                    (MapWinGIS.Shapefile)
                    this.globals.MapWin.Layers[this.globals.MapWin.Layers.CurrentLayer].GetObject();
                sf.StartEditingShapes(true, null);

                foreach (var s in shapes)
                {
                    var shpgeom = s.Substring(0, s.IndexOf("FIELDS||"));
                    var fieldvalues = s.Substring(s.IndexOf("FIELDS||") + 8).Split(
                        new[] { "||" }, StringSplitOptions.None);

                    var newShp = new Shape();
                    newShp.CreateFromString(shpgeom);

                    if (newShp.ShapeType != sf.ShapefileType)
                    {
                        MapWinUtility.Logger.Message(
                            "Warning: Skipping a pasted shape because it is not of the same tye of the shapefile being edited (e.g., line versus polygon).", 
                            "Skipping Shape", 
                            System.Windows.Forms.MessageBoxButtons.OK, 
                            System.Windows.Forms.MessageBoxIcon.Information, 
                            DialogResult.OK);
                        continue;
                    }

                    // If shape is entirely in current view, shift ths shape slightly so
                    // it's apparent that it was added
                    // If out of current bounds, move it so at least part is in current bounds
                    if (!OutOfView(this.globals.MapWin.View.Extents, newShp.Extents))
                    {
                        var shiftX = (newShp.Extents.xMax - newShp.Extents.xMin) / 6;
                        var shiftY = (newShp.Extents.yMax - newShp.Extents.yMin) / 6;
                        for (var i = 0; i < newShp.numPoints; i++)
                        {
                            newShp.get_Point(i).x += shiftX;
                        }
                    }
                    else
                    {
                        while (OutOfView(this.globals.MapWin.View.Extents, newShp.Extents))
                        {
                            if (OutOfViewX(this.globals.MapWin.View.Extents, newShp.Extents) == -1)
                            {
                                var shiftX = newShp.Extents.xMax - newShp.Extents.xMin;
                                for (var i = 0; i < newShp.numPoints; i++)
                                {
                                    newShp.get_Point(i).x -= shiftX;
                                }
                            }
                            else if (OutOfViewX(this.globals.MapWin.View.Extents, newShp.Extents) == +1)
                            {
                                var shiftX = newShp.Extents.xMax - newShp.Extents.xMin;
                                for (var i = 0; i < newShp.numPoints; i++)
                                {
                                    newShp.get_Point(i).x += shiftX;
                                }
                            }

                            if (OutOfViewY(this.globals.MapWin.View.Extents, newShp.Extents) == -1)
                            {
                                var shiftY = newShp.Extents.yMax - newShp.Extents.yMin;
                                for (var i = 0; i < newShp.numPoints; i++)
                                {
                                    newShp.get_Point(i).y -= shiftY;
                                }
                            }
                            else if (OutOfViewY(this.globals.MapWin.View.Extents, newShp.Extents) == +1)
                            {
                                var shiftY = newShp.Extents.yMax - newShp.Extents.yMin;
                                for (var i = 0; i < newShp.numPoints; i++)
                                {
                                    newShp.get_Point(i).y += shiftY;
                                }
                            }
                        }

                        // If we adjusted it into view, and if the shape's total
                        // width and height is less than the view's,
                        // try to wiggle it completely into the view
                        if ((this.globals.MapWin.View.Extents.xMax - this.globals.MapWin.View.Extents.xMin)
                            > (newShp.Extents.xMax - newShp.Extents.xMin)
                            &&
                            (this.globals.MapWin.View.Extents.yMax - this.globals.MapWin.View.Extents.yMin)
                            > (newShp.Extents.yMax - newShp.Extents.yMin))
                        {
                            // But give up before we make user wait too long, if hard to fit into view
                            var MaxReps = 200;
                            var CurrReps = 0;
                            while (CurrReps < MaxReps
                                   && !ExtentsFullyContained(this.globals.MapWin.View.Extents, newShp.Extents))
                            {
                                CurrReps++;
                                if (ExtentsFullyContainedX(this.globals.MapWin.View.Extents, newShp.Extents) == -1)
                                {
                                    var shiftX = (newShp.Extents.xMax - newShp.Extents.xMin) / 20;
                                    for (var i = 0; i < newShp.numPoints; i++)
                                    {
                                        newShp.get_Point(i).x -= shiftX;
                                    }
                                }
                                else if (ExtentsFullyContainedX(this.globals.MapWin.View.Extents, newShp.Extents) == +1)
                                {
                                    var shiftX = (newShp.Extents.xMax - newShp.Extents.xMin) / 20;
                                    for (var i = 0; i < newShp.numPoints; i++)
                                    {
                                        newShp.get_Point(i).x += shiftX;
                                    }
                                }

                                if (ExtentsFullyContainedY(this.globals.MapWin.View.Extents, newShp.Extents) == -1)
                                {
                                    var shiftY = (newShp.Extents.yMax - newShp.Extents.yMin) / 20;
                                    for (var i = 0; i < newShp.numPoints; i++)
                                    {
                                        newShp.get_Point(i).y -= shiftY;
                                    }
                                }
                                else if (ExtentsFullyContainedY(this.globals.MapWin.View.Extents, newShp.Extents) == +1)
                                {
                                    var shiftY = (newShp.Extents.yMax - newShp.Extents.yMin) / 20;
                                    for (var i = 0; i < newShp.numPoints; i++)
                                    {
                                        newShp.get_Point(i).y += shiftY;
                                    }
                                }
                            }
                        }
                    }

                    // Done, insert.
                    var shpindex = sf.NumShapes;
                    sf.EditInsertShape(newShp, ref shpindex);
                    added.Add(shpindex);

                    // And add field values:
                    for (var f = 0; f < fieldvalues.Length; f++)
                    {
                        var fieldvaluepair = fieldvalues[f].Split(new[] { '|' }, StringSplitOptions.None);
                        if (fieldvaluepair.Length == 2)
                        {
                            if (fieldvaluepair[0].ToLower() != "mwshapeid" && fieldvaluepair[0].ToLower() != "id")
                            {
                                var fldIdx = this.FindField(fieldvaluepair[0], ref sf);
                                if (fldIdx != -1)
                                {
                                    sf.EditCellValue(fldIdx, shpindex, fieldvaluepair[1]);
                                }
                            }
                        }
                    }
                }

                this.globals.CreateUndoPoint();

                sf.StopEditingShapes(true, true, null);

                this.globals.UpdateView();

                // Select the added one(s) to be moved easily (or deleted etc)
                this.globals.MapWin.View.SelectedShapes.ClearSelectedShapes();
                for (var i = 0; i < added.Count; i++)
                {
                    this.globals.MapWin.View.SelectedShapes.AddByIndex(
                        (int)added[i], this.globals.MapWin.View.SelectColor);
                }

                this.globals.MapWin.Plugins.BroadcastMessage(
                    "ShapefileEditor: Layer " + this.globals.MapWin.Layers.CurrentLayer + ": New Shape Added");
            }
        }

        #endregion
    }
}