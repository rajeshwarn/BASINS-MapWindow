
namespace GMap.NET
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    using MapWindow.Tiles;
    using GMap.NET.MapProviders;
    using GMap.NET.CacheProviders;
    using System.Linq;
    using System.IO;
    using System.Diagnostics;
    #endregion

    /// <summary>
    /// Provides GUI for browsing tiles database
    /// </summary>
    public partial class frmBrowseCache : Form
    {
        private TileClient m_client = null;
        private List<TileInfo> m_tiles = null;
        private int m_layerHandle = -1;

        #region Constructor
        /// <summary>
        /// Creates a new instance of the frmBrowseCache class
        /// </summary>
        public frmBrowseCache(TileClient client)
        {
            InitializeComponent();

            axMap1.ShowRedrawTime = false;
            axMap1.ShowVersionNumber = false;

            m_client = client;
            this.UpdateTreeView();
            
            if (treeView1.Nodes.Count > 0)
                this.treeView1.SelectedNode = this.treeView1.Nodes[0];

            string filename = Path.GetDirectoryName(Application.ExecutablePath) + @"\map\world.state";
            this.LoadStateFromFile(filename);
        }

        /// <summary>
        /// Loads world project
        /// </summary>
        /// <param name="filename">The filename to load map state from</param>
        /// <returns>True on success and false otherwise</returns>
        public bool LoadStateFromFile(string filename)
        {
            if (File.Exists(filename))
            {
                if (!axMap1.LoadMapState(filename, null))
                {
                    System.Diagnostics.Debug.Print("Failed to load map state: " + filename + Environment.NewLine + "Application is closing...");
                    return false;
                }
                else
                {
                    axMap1.ZoomToMaxExtents();
                    return true;
                }
            }
            else
            {
                System.Diagnostics.Debug.Print("World state file wasn't found: " + filename + Environment.NewLine + "Application is closing...");
                return false;
            }
        }
        #endregion

        #region Update lists
        /// <summary>
        /// Updates treeview with providers from the database
        /// </summary>
        private void UpdateTreeView()
        {
            this.treeView1.Nodes.Clear();
            IEnumerable<GMapProvider> list = m_client.Cache.GetProvidersList();
            foreach (GMapProvider item in list)
            {
                TreeNode node = this.treeView1.Nodes.Add(item.Name);
                node.Tag = item;
            }
        }

        /// <summary>
        /// Shows information about the selected provider
        /// </summary>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.listView1.Items.Clear();
            GMapProvider provider = this.SelectedProvider;
            if (provider != null)
            {
                m_tiles = m_client.Cache.GetProviderInfo(provider.DbId);
                IEnumerable<IGrouping<int, TileInfo>> scales = m_tiles.GroupBy(t => t.Zoom).OrderBy(t => t.Key);
                foreach (var scale in scales)
                {
                    List<TileInfo> tiles = scale.ToList();
                    ListViewItem item = this.listView1.Items.Add(scale.Key.ToString());
                    item.SubItems.Add(tiles.Count().ToString());
                    //item.SubItems.Add(string.Format("{0} - {1}", tiles.Select(t => t.X).Min().ToString(), tiles.Select(t => t.X).Max().ToString()));
                    //item.SubItems.Add(string.Format("{0} - {1}", tiles.Select(t => t.Y).Min().ToString(), tiles.Select(t => t.Y).Max().ToString()));
                }
            }
            else
                m_tiles = null;
        }
        #endregion

        #region Remove
        /// <summary>
        /// Removes tiles from the cache
        /// </summary>
        private void btnRemoveProvider_Click(object sender, EventArgs e)
        {
            GMapProvider provider = this.SelectedProvider;
            if (provider != null)
            {
                m_client.Cache.RemoveTiles(provider.DbId);
                this.UpdateTreeView();
            }
        }

        /// <summary>
        /// Removes scale for the given provider
        /// </summary>
        private void btnRemoveScale_Click(object sender, EventArgs e)
        {
            GMapProvider provider = this.SelectedProvider;
            if (provider != null)
            {
                if (this.listView1.SelectedItems.Count > 0)
                {
                    int scale = Int32.Parse(this.listView1.SelectedItems[0].Text);
                    m_client.Cache.RemoveTiles(provider.DbId, scale);
                    this.treeView1_AfterSelect(null, null);
                }
            }
        }
        #endregion

        #region Utilities
        /// <summary>
        /// Returns provider selected in the tree view
        /// </summary>
        private GMapProvider SelectedProvider
        {
            get
            {
                TreeNode node = this.treeView1.SelectedNode;
                return node != null ? node.Tag as GMapProvider : null;
            }
        }
        /// <summary>
        ///  Gets selected scale
        /// </summary>
        private int SelectedScale
        {
            get
            {
                return this.listView1.SelectedItems.Count > 0 ? Int32.Parse(this.listView1.SelectedItems[0].Text) : -1;
            }
        }
        #endregion

        #region Map
        
        /// <summary>
        /// Shows tiles of the selected scale on the map
        /// </summary>
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int zoom = this.SelectedScale;
            GMapProvider provider = this.SelectedProvider;
            if (provider != null && zoom >= 0 && m_tiles != null)
            {
                PureProjection projection = provider.Projection;

                axMap1.RemoveLayer(m_layerHandle);
                MapWinGIS.Shapefile sf = new MapWinGIS.Shapefile();
                sf.CreateNew("", MapWinGIS.ShpfileType.SHP_POLYGON);

                foreach (TileInfo tile in m_tiles.Where(t => t.Zoom == zoom))
                {
                    // first point
                    GPoint pnt = new GPoint(tile.X, tile.Y);
                    PointLatLng point = projection.FromPixelToLatLng(projection.FromTileXYToPixel(pnt), zoom);

                    // next point
                    pnt.X++; pnt.Y++;
                    PointLatLng pointNext = projection.FromPixelToLatLng(projection.FromTileXYToPixel(pnt), zoom);

                    //GSize size = projection.GetTileMatrixSizeXY(zoom);
                    RectLatLng rect = new RectLatLng(point, new SizeLatLng(point.Lat - pointNext.Lat, point.Lng - pointNext.Lng)); //360.0/size.Width));

                    MapWinGIS.Shape shp = new MapWinGIS.Shape();
                    shp.Create(MapWinGIS.ShpfileType.SHP_POLYGON);
                    this.InsertPart(shp, rect.Left, rect.Right, rect.Bottom, rect.Top);

                    int shpIndex = sf.NumShapes;
                    sf.EditInsertShape(shp, ref shpIndex);
                }

                m_layerHandle = axMap1.AddLayer(sf, true);

                MapWinGIS.Utils utils = new MapWinGIS.Utils();
                sf.DefaultDrawingOptions.FillColor = utils.ColorByName(MapWinGIS.tkMapColor.LightBlue);
                sf.DefaultDrawingOptions.FillTransparency = 120;
                sf.DefaultDrawingOptions.LineColor = utils.ColorByName(MapWinGIS.tkMapColor.Blue);
                sf.DefaultDrawingOptions.LineStipple = MapWinGIS.tkDashStyle.dsDash;
                sf.DefaultDrawingOptions.LineWidth = 2;

                axMap1.Redraw();
            }
        }

        /// <summary>
        /// Insers part to polygon based on given rectange
        /// </summary>
        private void InsertPart(MapWinGIS.Shape shp, double xMin, double xMax, double yMin, double yMax)
        {
            int numParts = shp.NumParts;
            shp.InsertPart(shp.numPoints, ref numParts);

            // to left
            int index = shp.numPoints;
            MapWinGIS.Point pnt = new MapWinGIS.Point();
            pnt.x = xMin; pnt.y = yMax;
            shp.InsertPoint(pnt, ref index); index++;

            pnt = new MapWinGIS.Point();
            pnt.x = xMax; pnt.y = yMax;
            shp.InsertPoint(pnt, ref index); index++;

            pnt = new MapWinGIS.Point();
            pnt.x = xMax; pnt.y = yMin;
            shp.InsertPoint(pnt, ref index); index++;

            pnt = new MapWinGIS.Point();
            pnt.x = xMin; pnt.y = yMin;
            shp.InsertPoint(pnt, ref index); index++;

            pnt = new MapWinGIS.Point();
            pnt.x = xMin; pnt.y = yMax;
            shp.InsertPoint(pnt, ref index); index++;

            //if (!shp.get_PartIsClockWise(0))
            //{
            //    bool val = shp.ReversePointsOrder(0);
            //    if (!val)
            //    {
            //        System.Diagnostics.Debug.Print("CCW");
            //    }
            //}
        }
        #endregion
    }
}
