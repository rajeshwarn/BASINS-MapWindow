
namespace MapWindow.Tiles
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using GMap.NET;
    using GMap.NET.CacheProviders;
    using GMap.NET.MapProviders;
    using System.Diagnostics;
    #endregion

    [System.ComponentModel.ToolboxItem(true)]
    public partial class TilesControl : UserControl
    {
        #region Declaration
        private TileClient m_client = null;
        private bool m_noEvents = false;
        private bool m_trackbarScrolled = false;
        #endregion

        #region Contructor
        /// <summary>
        /// Creates a new instance of the TileControl
        /// </summary>
        public TilesControl()
        {
            InitializeComponent();
            this.ResizeControl();

            cboLayers.DataBindings.Add("Enabled", optLayers, "Checked");
            panelBounds.DataBindings.Add("Enabled", optExtents, "Checked");
            btnChooseExtents.DataBindings.Add("Enabled", optExtents, "Checked");

            IEnumerable<GMapProvider> providers = GMapProviders.List;
            this.cboProvider.DataSource = providers;
            this.cboProvider.DisplayMember = "Name";
            this.cboProvider.ValueMember = "Id";
            this.cboProvider.SelectedIndexChanged += new EventHandler(cboProvider_SelectedIndexChanged);

            this.chkServer_CheckedChanged(null, null);

            chkUseDatabase.CheckedChanged += delegate(object sender, EventArgs e)
            {
                m_client.AccessMode = chkUseDatabase.Checked ? AccessMode.ServerAndCache : AccessMode.ServerOnly;
            };
        }

        private void chkServer_CheckedChanged(object sender, EventArgs e)
        {
            if (m_client != null)
                m_client.AccessMode = chkServer.Checked ? AccessMode.ServerOnly : AccessMode.CacheOnly;
        }

        /// <summary>
        /// Handles the changes of the provider
        /// </summary>
        void cboProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_client != null && cboProvider.SelectedItem != null && !m_noEvents)
            {
                m_client.Provider = cboProvider.SelectedItem as GMapProvider;
                this.trackBar1.Minimum = m_client.Provider.MinZoom;
                if (this.trackBar1.Minimum == 0)
                    this.trackBar1.Minimum = 1;
                
                if (m_client.Provider.MaxZoom.HasValue)
                    this.trackBar1.Maximum = m_client.Provider.MaxZoom.Value;
            }
        }
        #endregion

        #region Events
       
        /// <summary>
        /// A clinet associated with control
        /// </summary>
        public TileClient TilesClient
        {
            get 
            { 
                return m_client; 
            }
            set 
            {
                m_client = value;
                if (m_client != null)
                {
                    AxMapWinGIS.AxMap map = m_client.Map;
                    if (map != null)
                    {
                        m_client = value;

                        m_client.ProviderChanged += delegate(object sender, GMapProvider provider)
                        {
                            foreach (var item in cboProvider.Items)
                            {
                                GMapProvider val = item as GMapProvider;
                                if (val != null && val.Name == provider.Name)
                                {
                                    m_noEvents = true;                // prevents sending the event back to the client
                                    cboProvider.SelectedItem = item;
                                    m_noEvents = false;
                                }
                            }
                        };

                        m_client.SettingsChanged += new TileClient.SettingsChangedEventHandler(m_client_SettingsChanged);
                        this.m_client_SettingsChanged(m_client);
                        
                        map.ExtentsChanged += new EventHandler(Map_ExtentsChanged);

                        List<string> names = new List<string>();
                        names.Add("All extents");
                        for (int i = 0; i < map.NumLayers; i++)
                        {
                            names.Add(map.get_LayerName(map.get_LayerHandle(i)));
                        }
                        cboLayers.DataSource = names;
                        cboLayers.SelectedIndex = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Updates control settings
        /// </summary>
        void  m_client_SettingsChanged(object sender)
        {
 	        TileClient client = sender as TileClient;
            if (client != null)
            {
                //this.lblDatabaseCache.Text = string.Format("{0}/{1} MB", 
                //                             client.MemoryCacheSize.ToString("0.0"),
                //                             client.MemoryCacheCapacity.ToString("0.0"));

                cboProvider.SelectedItem = m_client.Provider;
            }
        }

        /// <summary>
        /// Updating information
        /// </summary>
        void Map_ExtentsChanged(object sender, EventArgs e)
        {
            object obj = m_client.Map.GeographicalExtents;
            if (obj != null)
            {
                MapWinGIS.Extents ext = (MapWinGIS.Extents)obj;

                this.txtLat.Text = ((ext.yMin + ext.yMax) / 2).ToString("0.0000");
                this.txtLong.Text = ((ext.xMin + ext.xMax) / 2).ToString("0.0000");
                this.lblRamCache.Text = (m_client.Map.Tiles.RamSize >> 20).ToString("0.0");

                if (m_client.Zoom < this.trackBar1.Minimum)
                    m_client.Zoom = this.trackBar1.Minimum;

                if (m_client.Zoom > this.trackBar1.Maximum)
                    m_client.Zoom = this.trackBar1.Maximum;

                if (!m_trackbarScrolled)
                    this.trackBar1.Value = m_client.Zoom;
            }
        }
        #endregion

        #region Resizing

        /// <summary>
        /// Shows and hides Extents panel of the control
        /// </summary>
        public bool ShowExtentsPanel
        {
            get { return this.panelExtents.Visible; }
            set 
            {
                this.panelExtents.Visible = value;
                this.ResizeControl();
            }
        }

        /// <summary>
        /// Updates the size of control depending of the number of visible panels
        /// </summary>
        private void ResizeControl()
        {
            int height = 186;
            
            if (this.panelExtents.Visible)
                height += this.panelExtents.Height + 10;

            Size size = new Size(298, height);
        }
        #endregion

        #region Setting map position
        /// <summary>
        /// Updates position on map
        /// </summary>
        private void txtLat_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Enter)
            {
                double lat, lng;
                double.TryParse(txtLat.Text, out lat);
                double.TryParse(txtLong.Text, out lng);

                if (m_client != null && m_client.Map != null)
                {
                    this.UpdateMapPosition(lng, lat);
                }
            }
        }

        /// <summary>
        /// Runs geocoding task based upon user input
        /// </summary>
        private void txtFind_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Enter)
            {
                GeoCoderStatusCode status = GeoCoderStatusCode.Unknow;
                GMaps maps = GMaps.Instance;
                
                //PointLatLng? pos = maps.GetLatLngFromGeocoder(txtFind.Text, out status);
                PointLatLng? pos = m_client.SetCurrentPositionByKeywords(txtFind.Text, out status);
                
                if (pos.HasValue && status == GeoCoderStatusCode.G_GEO_SUCCESS)
                {
                    this.UpdateMapPosition(pos.Value.Lng, pos.Value.Lat);
                }
            }
        }

        /// <summary>
        /// Updates position 
        /// </summary>
        private void UpdateMapPosition(double lng, double lat)
        {
            object obj = m_client.Map.GeographicalExtents;
            if (obj != null)
            {
                MapWinGIS.Extents ext = (MapWinGIS.Extents)obj;
                double dx = (ext.xMax - ext.xMin) / 2.0;
                double dy = (ext.yMax - ext.yMin) / 2.0;
                ext.SetBounds(lng - dx, lat - dy, 0.0, lng + dx, lat + dy, 0.0);
                m_client.Map.GeographicalExtents = ext;
            }
        }
        #endregion

        #region Scrollbar zooming
        /// <summary>
        /// Changes zoom by scrollbar
        /// </summary>
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (m_client == null)
                return;
            
            // extracting zoom
            int zoom = this.trackBar1.Value;
            GMapProvider provider = m_client.m_core.provider;
            if (provider.MaxZoom.HasValue && this.trackBar1.Value > provider.MaxZoom.Value)
                zoom = provider.MaxZoom.Value;

            object obj = m_client.Map.GeographicalExtents;
            if (obj != null)
            {
                MapWinGIS.Extents ext = (MapWinGIS.Extents)obj;
                double x = (ext.xMax + ext.xMin)/ 2.0;
                double y = (ext.yMax + ext.yMin)/ 2.0;
                SizeLatLng size = m_client.GetTileSizeLatLon(new PointLatLng(x, y), this.trackBar1.Value);

                //number of tiles to be displayed on the screen
                PureProjection proj = m_client.m_core.provider.Projection;
                double xCount = (double)m_client.Map.Width/(double)proj.TileSize.Width;
                double yCount = (double)m_client.Map.Height / (double)proj.TileSize.Height;

                MapWinGIS.Extents extNew = new MapWinGIS.Extents();
                double dx = xCount/2.0 * size.WidthLng;
                double dy = yCount/2.0 * size.HeightLat;
                extNew.SetBounds(x - dx, y - dy, 0.0, x + dx, y + dy, 0.0);
                m_trackbarScrolled = true;
                m_client.Map.GeographicalExtents = extNew;
                m_trackbarScrolled = false;
            }
        }
        #endregion

        #region Caching
        /// <summary>
        /// Runs cahing
        /// </summary>
        private void btnDoCaching_Click(object sender, EventArgs e)
        {
            this.DoCaching();
        }

        /// <summary>
        /// Converts MapWinGis extents to GMap rectangle
        /// </summary>
        RectLatLng ExtentsToRectangle(MapWinGIS.Extents ext)
        {
            if (ext == null)
            {
                return new RectLatLng();
            }
            else
            {
                double x = (ext.xMax + ext.xMin) / 2.0;
                double y = (ext.yMin + ext.yMax) / 2.0;
                RectLatLng area = new RectLatLng(y, x, ext.xMax - ext.xMin, ext.yMax - ext.yMin);
                return area;
            }
        }

        /// <summary>
        /// Perform caching
        /// </summary>
        private void DoCaching()
        {
            AxMapWinGIS.AxMap map = m_client.Map;
            object obj = map.GeographicalExtents;
            if (obj != null)
            {
                MapWinGIS.Extents ext = obj as MapWinGIS.Extents;
                RectLatLng area = this.ExtentsToRectangle(ext);

                int minZoom = m_client.m_core.provider.MinZoom;
                int maxZoom = m_client.m_core.provider.MaxZoom.Value;
               
                if (!area.IsEmpty)
                {
                    frmPrefetcher form = new frmPrefetcher(m_client, area);
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        // do something
                    }
                }
                else
                {
                    MessageBox.Show("Select map area holding ALT", "MapWindow.Tiles", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Iterartes through all parent controls until parent form will be reached
        /// </summary>
        private Form GetParentForm()
        {
            Control parent = this;
            while(true)
            {
                parent = parent.Parent;
                if (parent == null)
                    return null;
                if (parent is Form)
                    return parent as Form;
            };
        }

        #endregion

        /// <summary>
        /// Shows caching dialog
        /// </summary>
        private void btnTilesSettings_Click(object sender, EventArgs e)
        {
            frmDbCache form = new frmDbCache(m_client);
            form.ShowDialog(this);
            form.Dispose();
        }

        /// <summary>
        /// Shows form displaying the cache content on the map
        /// </summary>
        private void btnBrowseCache_Click(object sender, EventArgs e)
        {
            frmBrowseCache form = new frmBrowseCache(m_client);
            form.ShowDialog(this);
            form.Dispose();
        }

        /// <summary>
        /// Set current extents as extents for caching
        /// </summary>
        private void btnChooseExtents_Click(object sender, EventArgs e)
        {
            MapWinGIS.Extents ext = (MapWinGIS.Extents)m_client.Map.GeographicalExtents;
            if (ext != null)
            {
                txtMinLat.Text = ext.yMin.ToString("0.000");
                txtMaxLat.Text = ext.yMax.ToString("0.000");
                txtMinLon.Text = ext.xMin.ToString("0.000");
                txtMaxLon.Text = ext.xMax.ToString("0.000");
                m_client.CachedExtents = ext;
            }
        }

        /// <summary>
        /// Fills memory cache from the database
        /// </summary>
        private void btnFillMemory_Click(object sender, EventArgs e)
        {
            int count = 0;
            MapWinGIS.Extents ext = m_client.CachedExtents;
            PureImageCache cache = m_client.Cache as PureImageCache;
            if (ext != null && ext != null)
            {
                int providerId = m_client.Provider.DbId;
                List<TileInfo> list = m_client.Cache.GetProviderInfo(providerId);
                
                int minZoom = m_client.Provider.MinZoom;
                int maxZoom = m_client.Provider.MaxZoom.Value;

                for (int i = minZoom; i < maxZoom; i++)
                {
                    GPoint pnt1 = m_client.FromLatLngToXY(new PointLatLng(ext.yMin, ext.xMin), i);
                    GPoint pnt2 = m_client.FromLatLngToXY(new PointLatLng(ext.yMin, ext.xMax), i);
                    GPoint pnt3 = m_client.FromLatLngToXY(new PointLatLng(ext.yMax, ext.xMax), i);
                    GPoint pnt4 = m_client.FromLatLngToXY(new PointLatLng(ext.yMax, ext.xMin), i);

                    int minX = Math.Min(Math.Min(pnt1.X, pnt2.X), Math.Min(pnt3.X, pnt4.X));
                    int maxX = Math.Max(Math.Max(pnt1.X, pnt2.X), Math.Max(pnt3.X, pnt4.X));
                    int minY = Math.Min(Math.Min(pnt1.Y, pnt2.Y), Math.Min(pnt3.Y, pnt4.Y));
                    int maxY = Math.Max(Math.Max(pnt1.Y, pnt2.Y), Math.Max(pnt3.Y, pnt4.Y));

                    IEnumerable<TileInfo> tiles = list.Where(t => t.Zoom == i && t.X >= minX && t.X <= maxX && t.Y >= minY && t.Y <= maxY);
                    foreach(TileInfo tile in tiles)
                    {
                        GPoint point = new GPoint(tile.X, tile.Y);
                        PureImage image = cache.GetImageFromCache(providerId, point, tile.Zoom);
                        m_client.AddTileToMap(image, point, tile.Zoom);
                        count++;
                    }
                }
            }
            this.UpdateMemorySize();
            MessageBox.Show("Number of tiles added to the memory cache: " + count.ToString());
        }

        /// <summary>
        /// Updates label with the size of memory cache
        /// </summary>
        private void UpdateMemorySize()
        {
            this.lblRamCache.Text = m_client.Map.Tiles.RamSize.ToString("0.000");
        }
    }
}

