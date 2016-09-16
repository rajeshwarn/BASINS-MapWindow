// EPSG Reference: a tool for visualization of EPSG coordinate system database
// Author: Sergei Leschinski

// additional operations not intended for end users will be avialable
//#define UTILITIES

namespace EPSG_Reference
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    using System.IO;
    using System.Collections;
    using System.Diagnostics;
    using MapWindow.Controls.Projections;
    
    public partial class frmMain : Form
    {
        #region Local variables

        

        private const string BTN_ZOOM_IN = "btnZoomIn";
        private const string BTN_ZOOM_OUT = "btnZoomOut";
        private const string BTN_ZOOM_PAN  = "btnPan";
        private const string BTN_SELECT = "btnSelect";
        private const string BTN_CLEAR = "btnClearExtents";
        #endregion

        #region Initialization
        /// <summary>
        /// Creates as new instance of frmMain class
        /// </summary>
        public frmMain()
        {
            InitializeComponent();

            #if UTILITIES
                btnFillCountryByArea.Visible = true;
            #endif

            if (this.Initialize())
            {
                axMap1.ShowRedrawTime = false;
                axMap1.ShowVersionNumber = false;
                axMap1.MapResizeBehavior = MapWinGIS.tkResizeBehavior.rbClassic;
                axMap1.MouseMoveEvent += new AxMapWinGIS._DMapEvents_MouseMoveEventHandler(axMap1_MouseMoveEvent);
                axMap1.SelectBoxFinal += new AxMapWinGIS._DMapEvents_SelectBoxFinalEventHandler(axMap1_SelectBoxFinal);
                this.WindowState = FormWindowState.Maximized;
                axMap1.ZoomToMaxExtents();
                
                Application.DoEvents();
            }
            else
            {
                Application.ExitThread();
            }
        }

        /// <summary>
        /// Checks the necessary files and initializes app
        /// </summary>
        private bool Initialize()
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath) + @"\Projections\";
            if (!Directory.Exists(path))
            {
                MessageBox.Show("EPSG Reference folder isn't found: " + path + Environment.NewLine + "Application is closing...");
                return false;
            }

            string[] files = Directory.GetFiles(path, "*.mdb");
            if (files.Length != 1)
            {
                MessageBox.Show("A single EPSG database is expected. " + files.Length.ToString() + " databases are found." + Environment.NewLine +
                                "Path : " + path + Environment.NewLine + "Application is closing...");
                return false;
            }
            else
            {
                this.projectionTreeView1.Initialize(files[0], null);
                int gcsCount, pcsCount;
                this.projectionTreeView1.RefreshList(out gcsCount,out pcsCount);
                this.UpdateStatusBar(gcsCount, pcsCount);
                //EpsgDatabase db = new EpsgDatabase(files[0]);
            }

            axMap1.LoadStateFromExeName(Application.ExecutablePath);
            return true;
        }
        #endregion

        #region Toolbar
        /// <summary>
        /// Handling item clicked event of toolstrip
        /// </summary>
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == null)
                return;

            ((ToolStripButton)toolStrip1.Items[BTN_ZOOM_IN]).Checked = false;
            ((ToolStripButton)toolStrip1.Items[BTN_ZOOM_OUT]).Checked = false;
            ((ToolStripButton)toolStrip1.Items[BTN_ZOOM_PAN]).Checked = false;
            ((ToolStripButton)toolStrip1.Items[BTN_SELECT]).Checked = false;

            switch (e.ClickedItem.Name.ToLower())
            {
                case "btnzoomin":
                    axMap1.CursorMode = MapWinGIS.tkCursorMode.cmZoomIn;
                    ((ToolStripButton)toolStrip1.Items[BTN_ZOOM_IN]).Checked = true;
                    break;
                case "btnzoomout":
                    axMap1.CursorMode = MapWinGIS.tkCursorMode.cmZoomOut;
                    ((ToolStripButton)toolStrip1.Items[BTN_ZOOM_OUT]).Checked = true;
                    break;
                case "btnpan":
                    axMap1.CursorMode = MapWinGIS.tkCursorMode.cmPan;
                    ((ToolStripButton)toolStrip1.Items[BTN_ZOOM_PAN]).Checked = true;
                    break;
                case "btnselect":
                    axMap1.CursorMode = MapWinGIS.tkCursorMode.cmSelection;
                    ((ToolStripButton)toolStrip1.Items[BTN_SELECT]).Checked = true;
                    break;
                case "btnclearextents":
                    axMap1.ClearBounds();
                    axMap1.ClearCoordinateSystem();
                    int gcsCount, pcsCount;
                    projectionTreeView1.RefreshList(out gcsCount, out pcsCount);
                    UpdateStatusBar(gcsCount, pcsCount);
                    break;
                default:
                    break;
            }
        }
        #endregion

        /// <summary>
        /// Updates status bar with information about the number of selected CS
        /// </summary>
        private void UpdateStatusBar(int gcsCount, int pcsCount)
        {
            lblGCS.Text = "Coordinate Systems: " + gcsCount;
            lblPCS.Text = "Projections:  " + pcsCount;
            this.textBox1.Text = "";
        }

        #region Map Events
        /// <summary>
        /// Chooses extents to show in tree view
        /// </summary>
        private void axMap1_SelectBoxFinal(object sender, AxMapWinGIS._DMapEvents_SelectBoxFinalEvent e)
        {
            if (axMap1.CursorMode != MapWinGIS.tkCursorMode.cmSelection)
                return;
            
            // converting projected coordinates
            double xMax, xMin, yMax, yMin;
            xMax = xMin = yMin = yMax = 0.0;
            axMap1.PixelToProj(e.left, e.bottom, ref xMin, ref yMin);
            axMap1.PixelToProj(e.right, e.top, ref xMax, ref yMax);

            
            MapWinGIS.Extents ext = new MapWinGIS.Extents();
            ext.SetBounds(xMin, yMin, 0.0, xMax, yMax, 0.0);
            
            int gcsCount, pcsCount;
            
            projectionTreeView1.RefreshList(new BoundingBox(xMin, xMax, yMin, yMax), out gcsCount, out pcsCount);

            axMap1.DrawSelectedBounds(ext);

            UpdateStatusBar(gcsCount, pcsCount);
        }

        /// <summary>
        /// Shows cursor position in projected coordinates
        /// </summary>
        private void axMap1_MouseMoveEvent(object sender, AxMapWinGIS._DMapEvents_MouseMoveEvent e)
        {
            double x = 0.0, y = 0.0;
            axMap1.PixelToProj((double)e.x, (double)e.y, ref x, ref y);

            string format = "#.000";
            string sx = (x < -180.000) ? "<180.0" : (x > 180.0) ? ">180.0" : x.ToString(format);
            string sy = (y < -90.0) ? "<90.0" : (y > 90.0) ? ">90.0" : y.ToString(format);
            this.lblPosition.Text = string.Format("X={0}; Y={1}", sx, sy);
        }
        #endregion

        #region TreeView Events
        /// <summary>
        /// Showing selection coordinate system
        /// </summary>
        /// <param name="cs"></param>
        private void projectionTreeView1_CoordinateSystemSelected(Territory cs)
        {
            if (cs == null)
                return;

            this.textBox1.SelectAll();
            this.textBox1.SelectionColor = Color.Black;

            string s;
            MapWinGIS.GeoProjection proj = new MapWinGIS.GeoProjection();
            if (proj.ImportFromEPSG(cs.Code))
            {
                this.textBox1.ShowProjection(proj.ExportToWKT());
            }
            else
            {
                s = "Failed to intitialize GEOProj";
            }
            


            axMap1.DrawCoordinateSystem(cs);
        }

        
        #endregion

        #region Utilities
        /// <summary>
        /// Fills country by field table in EPSG database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFillCountryByArea_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "MS Access (*.mdb)|*.mdb";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ProjectionDatabase.FillCountryByArea(dlg.FileName);
            }
        }
        #endregion
    }
}
