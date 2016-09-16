

namespace GMapClient
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using GMap.NET;
    using GMap.NET.Internals;
    using GMap.NET.MapProviders;
    using GMap.NET.WindowsForms;
    using System.Diagnostics;
    using MapWindow.Tiles;
    using System.Net;

    public partial class Form1 : Form
    {
        private TileClient m_client = null;
        
        /// <summary>
        /// Creates a new instance of the form
        /// </summary>
        public Form1()
        {
            InitializeComponent();

            axMap1.ShowVersionNumber = true;
            axMap1.ShowRedrawTime = true;
            axMap1.MapUnits = MapWinGIS.tkUnitsOfMeasure.umDecimalDegrees;
            axMap1.ExtentsChanged += new EventHandler(axMap1_ExtentsChanged);
            m_client = new TileClient(this.axMap1);
            this.tilesControl1.TilesClient = m_client;
        }

        /// <summary>
        /// Clears the tiles
        /// </summary>
        void axMap1_ExtentsChanged(object sender, EventArgs e)
        {
            axMap1.Tiles.Clear();
            //MapWinGIS.Extents ext = (MapWinGIS.Extents) axMap1.GeographicalExtents;
            //if (ext != null)
            //{
            //    MessageBox.Show(string.Format("x = {0} - {1}; y = {2} - {3}", ext.xMin, ext.xMax, ext.yMin, ext.yMax));
            //}
        }
       
        /// <summary>
        /// Opens shapefile
        /// </summary>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            MapWinGIS.Shapefile sf = new MapWinGIS.Shapefile();
            dlg.Filter = sf.CdlgFilter;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (sf.Open(dlg.FileName, null))
                {
                    
                    MapWinGIS.Utils utils = new MapWinGIS.Utils();
                    //(this.axMap1 as MapWinGIS.Map).BackColor = utils.ColorByName(MapWinGIS.tkMapColor.Orange);
                    sf.DefaultDrawingOptions.FillVisible = true;
                    sf.DefaultDrawingOptions.FillTransparency = 50;
                    //sf.DefaultDrawingOptions.LineTransparency = 75.0f;
                    sf.DefaultDrawingOptions.FillColor = utils.ColorByName(MapWinGIS.tkMapColor.Blue);
                    sf.DefaultDrawingOptions.LineWidth = 1.0f;
                    sf.DefaultDrawingOptions.LineVisible = true;

                    sf.DefaultDrawingOptions.LineColor = utils.ColorByName(MapWinGIS.tkMapColor.Gray);
                    int handle = axMap1.AddLayer(sf, true);

                    axMap1.GeoProjection = sf.GeoProjection;
                    //axMap1.MapUnits = MapWinGIS.tkUnitsOfMeasure.umDecimalDegrees;
                    axMap1.ZoomToLayer(handle);

                    m_client.Start();
                    axMap1.Tiles.Visible = true;
                }
            }
        }
    }
}
