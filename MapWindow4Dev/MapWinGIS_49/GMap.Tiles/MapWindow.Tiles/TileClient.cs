// GMap client
// Author: Sergei Leschinski
// Created on: 27 aug 2011

namespace MapWindow.Tiles
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Diagnostics;
    using GMap.NET;
    using GMap.NET.Internals;
    using GMap.NET.MapProviders;
    using GMap.NET.WindowsForms;
    using System.Drawing;
    using System.Threading;
    using System.ComponentModel;
    using System.Windows.Forms;
    using Microsoft.Win32;
    using System.Net;
using GMap.NET.CacheProviders;
    #endregion

    /// <summary>
    /// A class providing access to the GMap tiles functionality
    /// </summary>
    public class TileClient
    {
        #region Constants
        public const string GEOGRAPHIC_PROJECTION_PROJ4 = "+proj=latlong +datum=NAD83";
        public const string TILE_PROJECTION_PROJ4 = "+proj=merc +lon_0=0 +lat_ts=0 +x_0=0 +y_0=0 +a=6378137 +b=6378137 +units=m +no_defs";
        #endregion

        #region Declarations
        /// Proxy class for GMap
        static readonly WindowsFormsImageProxy wimg = new WindowsFormsImageProxy();
        
        /// GMap core functionality
        public Core m_core = new Core();
        
        // refrence to MapWinGIS
        private AxMapWinGIS.AxMap m_map = null;

        // loader is active
        private bool m_enabled = false;

        // the loader
        private BackgroundWorker m_worker = null;

        // list of tiles that were passed to the map; should be refreshed after the change of extents
        private Dictionary<GPoint, int> m_tiles = new Dictionary<GPoint, int>();

        // number of tiles loaded for the given extents
        private int m_count = 0;
        
        // extents to cache tiles in he memory
        private MapWinGIS.Extents m_cachedExtents = null;

        public MapWinGIS.Extents CachedExtents
        {
            get { return m_cachedExtents; }
            set { m_cachedExtents = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of TileClient class
        /// </summary>
        /// <param name="map">Reference to the instance of AxMapWinGIS.AxMap control</param>
        public TileClient(AxMapWinGIS.AxMap map)
        {
            if (map == null)
                throw new ArgumentException("Reference to map wasn't passed");
            
            m_map = map;
            m_map.ExtentsChanged += new EventHandler(map_ExtentsChanged);

            // memory cache is implemented in MapWinGIS
            GMaps.Instance.UseMemoryCache = false;      
            GMaps.Instance.Mode = AccessMode.ServerAndCache;

            GMapProvider.TileImageProxy = wimg;
            m_core.Provider = GMapProviders.OpenStreetMap;

            string proxy = this.GetDefaultProxy();
            if (proxy != "")
                GMapProvider.WebProxy = new WebProxy(proxy);

            GMaps.Instance.BeforeCaching += new GMaps.BeforeCachingEventHandler(Instance_BeforeCaching); 

            m_core.OnTileLoadStart += delegate()
            { Debug.Print("Start loading tiles"); };

            m_core.OnTileLoadComplete += delegate(long ElapsedMilliseconds)
            { Debug.Print("End loading tiles: {0}", ((double)ElapsedMilliseconds / 1000.0).ToString()); };
        }

        /// <summary>
        /// Destructor. Stops all asynchronous tasks
        /// </summary>
        ~TileClient()
        {
            m_core.CancelAsyncTasks();
            GMaps.Instance.CancelTileCaching();
        }

        /// <summary>
        /// Checks whether tile shoud be added to the database cache. Sets cancel to true in case it should not.
        /// </summary>
        /// <param name="tile">A tile structure which is about to be cached</param>
        /// <param name="cancel">Sets true in case the caching of tile was cancelled by user</param>
        private void Instance_BeforeCaching(RawTile tile, ref bool cancel)
        {
            if (m_cachedExtents != null)
            {
                PointLatLng point = this.FromXYToLngLat(tile.Pos, tile.Zoom);
                if (point.Lng < m_cachedExtents.xMin ||
                    point.Lng > m_cachedExtents.xMax ||
                    point.Lat < m_cachedExtents.yMin ||
                    point.Lat > m_cachedExtents.yMax)
                {
                    cancel = true;
                    return;
                }
            }
            cancel = false;
        }

        /// <summary>
        /// Gets the default IE proxy
        /// </summary>
        private string GetDefaultProxy()
        {
            string proxy = "";
            try
            {
                RegistryKey registry = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", false);
                bool enabled = (int)registry.GetValue("ProxyEnable") == 0 ? false : true;
                if (enabled)
                    proxy = (string)registry.GetValue("ProxyServer");
            }
            catch (Exception ex) { }
            return proxy;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Map control associated with the client
        /// </summary>
        public AxMapWinGIS.AxMap Map
        {
            get { return m_map; }
            set { m_map = value; }
        }
        
        /// <summary>
        /// Gets or sets the tiles provider
        /// </summary>
        public GMapProvider Provider
        {
            get { return m_core.Provider; }
            set 
            {
                if (m_core.provider != value)
                {
                    m_core.Provider = value;
                    this.map_ExtentsChanged(null, null);
                    this.FireTilesProviderChanged(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets zoom level (defined by provider)
        /// </summary>
        public int Zoom
        {
            get { return m_core.Zoom; }
            set { m_core.Zoom = value; }
        }

        /// <summary>
        /// The current mode for accessing tiles
        /// </summary>
        public AccessMode AccessMode
        {
            get { return GMaps.Instance.Mode; }
            set 
            { 
                GMaps.Instance.Mode = value;
                this.FireSettingsChanged(this);
            }
        }

        /// <summary>
        /// Gets reference of the SqLite database cache provider
        /// </summary>
        public SQLitePureImageCache Cache
        {
            get { return GMaps.Instance.ImageCacheLocal as SQLitePureImageCache; }
        }
        #endregion

        #region Events
        #region Tile loaded
        // Occurs when after loading of a particular tile
        public event TileLoadedDelegate TileLoaded;

        // Delegate for TileLoaded event
        public delegate void TileLoadedDelegate();

        /// <summary>
        /// Passes the event to the listeners
        /// </summary>
        private void FireTileLoaded()
        {
            if (this.TileLoaded != null)
                this.TileLoaded();
        }
        #endregion

        #region Loading finished
        // Occurs when all tiles for the given extents are loaded
        public event LoadingFinishedDelegate LoadingFinished;

        // Delegate for the LoadingFinished event
        public delegate void LoadingFinishedDelegate();

        /// <summary>
        /// Passes the event to the listeners
        /// </summary>
        private void FireLoadingFinished()
        {
            if (this.LoadingFinished != null)
                this.LoadingFinished();
        }
        #endregion

        #region Provider changed
        // A delegate for TilesProviderChanged event
        public delegate void ProviderChangedEventHandler(object sender, GMapProvider provider);

        // Fired when provider is changed via the code or by user
        public event ProviderChangedEventHandler ProviderChanged;

        internal void FireTilesProviderChanged(GMapProvider provider)
        {
            if (ProviderChanged != null)
                ProviderChanged(this, provider);
        }
        #endregion

        #region Settings changed
        // A delegate for TilesProviderChanged event
        public delegate void SettingsChangedEventHandler(object sender);

        // Fired when provider is changed via the code or by user
        public event SettingsChangedEventHandler SettingsChanged;

        internal void FireSettingsChanged(object sender)
        {
            if (SettingsChanged != null)
                SettingsChanged(sender);
        }
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Starts tile loader
        /// </summary>
        public void Start()
        {
            m_enabled = true;
            m_worker = m_core.OnMapOpen();

            m_worker.ProgressChanged += delegate(object sender, ProgressChangedEventArgs e)
            {
                this.AddTiles();
            };

            m_worker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs e)
            {
                Debug.Print("Loading is complete");
            };

            this.map_ExtentsChanged(null, null);
        }

        /// <summary>
        /// Stops the loading
        /// </summary>
        public void Stop()
        {
            m_core.CancelAsyncTasks();
        }
        #endregion
 
        #region Updating map
        /// <summary>
        /// Reloads tiles after extents are changed
        /// </summary>
        void map_ExtentsChanged(object sender, EventArgs e)
        {
            m_count = 0;

            m_tiles.Clear();
            m_map.Tiles.Clear();
            
            if (!m_enabled)
                return;

            object obj = m_map.GeographicalExtents;
            if (obj != null)
            {
                MapWinGIS.Extents ext = (MapWinGIS.Extents)obj;

                int zoom = this.GetZoomLevel(ext);
                m_core.Zoom = zoom;

                // skipping the layers with no tiles; probably would be enough to just skip the 0 level
                PureProjection projection = m_core.Provider.Projection;
                do
                {
                    m_core.minOfTiles = projection.GetTileMatrixMinXY(zoom);
                    m_core.maxOfTiles = projection.GetTileMatrixMaxXY(zoom);
                    if (m_core.maxOfTiles.Height == 0 || m_core.maxOfTiles.Width == 0)
                    {
                        zoom++;
                    }
                    else
                    {
                        m_core.Zoom = zoom;
                        break;
                    }
                }
                while (zoom < m_core.provider.MaxZoom);

                // the number of tiles around
                GPoint p1 = this.FromLatLngToXY(new PointLatLng(ext.yMin, ext.xMin), zoom);
                GPoint p2 = this.FromLatLngToXY(new PointLatLng(ext.yMax, ext.xMax), zoom);

                int xMin = Math.Min(p1.X, p2.X) - 1;
                int yMin = Math.Min(p1.Y, p2.Y) - 1;
                int xMax = Math.Max(p1.X, p2.X) + 1;
                int yMax = Math.Max(p1.Y, p2.Y) + 1;

                if (xMin < 0)
                    xMin = 0;

                if (yMin < 0)
                    yMin = 0;

                if (xMax > m_core.maxOfTiles.Width)
                    xMax = m_core.maxOfTiles.Width;

                if (yMax > m_core.maxOfTiles.Height)
                    yMax = m_core.maxOfTiles.Height;

                string provider = m_core.provider.Name;
                List<GPoint> points = new List<GPoint>();
                MapWinGIS.Tiles tiles = m_map.Tiles;
                for (int x = xMin; x <= xMax; x++)
                {
                    for (int y = yMin; y <= yMax ; y++)
                    {
                        if (!tiles.PrepareFromCache(provider, m_core.Zoom, x, y))
                        {
                            points.Add(new GPoint(x, y));
                        }
                    }
                }

                if (points.Count > 0)
                    m_core.ReloadMap(points);
            }
        }

        /// <summary>
        /// Adds newly loaded tiles to the map and triggers its update
        /// </summary>
        private void AddTiles()
        {
            m_core.Matrix.EnterReadLock();
            m_core.tileDrawingListLock.AcquireReaderLock();

            try
            {
                foreach (GPoint tilePoint in m_core.tileDrawingList)
                {
                    if (!m_tiles.ContainsKey(tilePoint))
                    {
                        Tile t = m_core.Matrix.GetTileWithNoLock(m_core.Zoom, tilePoint);
                        if (t != Tile.Empty)
                        {
                            m_tiles.Add(tilePoint, 0);

                            lock (t.Overlays)
                            {
                                for (int i = 0; i < t.Overlays.Count; i++)
                                {
                                    PureImage img = t.Overlays[i];
                                    this.AddTileToMap(img, tilePoint, m_core.Zoom);
                                    m_map.Refresh();

                                    //System.Diagnostics.Debug.Print(string.Format("Tile: lng = {0:000.00}; lat = {1:000.00}, x = {2},y = {3}, zoom = {4}, count = {6}", 
                                    //                                            point.Lng, point.Lat, tilePoint.X, tilePoint.Y, m_core.Zoom, m_count));
                                 }
                            }
                        }
                    }
                }
            }
            finally
            {
                m_core.tileDrawingListLock.ReleaseReaderLock();
                m_core.Matrix.LeaveReadLock();
            }
        }

        /// <summary>
        /// Adds tile to the map
        /// </summary>
        internal void AddTileToMap(PureImage img, GPoint tilePoint, int zoom)
        {
            byte[] data = img.Data.ToArray();

            // position
            PointLatLng point = this.FromXYToLngLat(tilePoint, zoom);

            // scale
            SizeLatLng size = this.GetTileSizeLatLon(tilePoint, zoom);

            MapWinGIS.Tile tile = m_map.Tiles.Add(data, point.Lng, point.Lat, size.WidthLng, size.HeightLat);
            if (tile != null)
            {
                // informational parameters to enable caching
                tile.Provider = m_core.provider.Name;
                tile.TileScale = m_core.Zoom;
                tile.TileX = tilePoint.X;
                tile.TileY = tilePoint.Y;
                m_count++;

                // add to the RAM cache
                if (m_cachedExtents != null)
                {
                    if (!(tile.xLongitude + tile.WidthLongitude < m_cachedExtents.xMin ||
                        tile.xLongitude > m_cachedExtents.xMax ||
                        tile.yLatitude + tile.HeightLatitude < m_cachedExtents.yMin ||
                        tile.yLatitude > m_cachedExtents.yMax))
                    {
                        tile.Cached = true;
                    }
                }
            }
        }

        /// <summary>
        /// Returns number of tiles loaded in particulr moment
        /// </summary>
        public int TileCount
        {
            get
            {
                int count = 0;
                lock (m_core.tileDrawingList)
                {
                    foreach (var tilePoint in m_core.tileDrawingList)
                    {
                        Tile t = m_core.Matrix.GetTileWithNoLock(m_core.Zoom, tilePoint);
                        if (t != Tile.Empty)
                        {
                            count++;
                        }
                    }
                }
                return count;
            }
        }
        #endregion

        #region Coordinates
        /// <summary>
        /// Finds closest zoom level for particular scale of map
        /// Tile size must be as close to original as possible, but not greater then it
        /// Geographic extents are expected as input
        /// </summary>
        private int GetZoomLevel(MapWinGIS.Extents ext)
        {
            double lon = (ext.xMax + ext.xMin) / 2.0;
            double lat = (ext.yMax + ext.yMin) / 2.0;

            PointLatLng location = new PointLatLng(lat, lon);

            //double minValue = double.MaxValue;
            int bestZoom = m_core.Provider.MinZoom;
            for (int i = m_core.Provider.MinZoom; i <= m_core.Provider.MaxZoom; i++)
            {
                GPoint point = this.FromLatLngToXY(location, i);
                SizeLatLng size = this.GetTileSizeLatLon(point, i);

                double tileSize = (size.WidthLng + size.HeightLat) * m_map.PixelsPerDegree / 2.0;

                if (tileSize > 256 * 0.85)
                {
                    //minValue = Math.Abs(tileSize - 256);
                    bestZoom = i;
                }
            }
            return bestZoom;
        }

        /// <summary>
        /// Returns index of tile for particular geographic coordinates
        /// </summary>
        internal GPoint FromLatLngToXY(PointLatLng point, int zoom)
        {
            PureProjection projection = m_core.Provider.Projection;
            GPoint pointNew = projection.FromPixelToTileXY(projection.FromLatLngToPixel(point, zoom));
            return pointNew;
        }
        
        /// <summary>
        /// Returns geographic coordinates of tile with given index
        /// </summary>
        internal PointLatLng FromXYToLngLat(GPoint point, int zoom)
        {
            PureProjection projection = m_core.Provider.Projection;
            PointLatLng pointNew = projection.FromPixelToLatLng(projection.FromTileXYToPixel(point), zoom);
            return pointNew;
        }

        /// <summary>
        /// Returns the size of a single tile for a given location
        /// </summary>
        internal SizeLatLng GetTileSizeLatLon(PointLatLng point, int zoom)
        {
            GPoint pnt = this.FromLatLngToXY(point, zoom);
            return this.GetTileSizeLatLon(pnt, zoom);
        }

        /// <summary>
        /// Returns the size of a single tile for a given location
        /// </summary>
        internal SizeLatLng GetTileSizeLatLon(GPoint point, int zoom)
        {
            PointLatLng pnt1 = this.FromXYToLngLat(point, zoom);

            // moving to the neighbouring tile
            GPoint newPoint = new GPoint(point.X, point.Y);
            newPoint.X++;
            MapWinGIS.GeoProjection proj = m_map.GeoProjection as MapWinGIS.GeoProjection;
            //if (proj != null && proj.IsGeographic)
            //{
            //    newPoint.Y++;   // for equirectangular projection
            //}
            //else
            //{
            //    newPoint.Y--;   // for Meractor projection
            //}

            newPoint.Y++;

            PointLatLng pnt2 = this.FromXYToLngLat(newPoint, zoom);
            
            // size
            SizeLatLng size = new SizeLatLng();
            size.WidthLng = Math.Abs( pnt2.Lng - pnt1.Lng);
            size.HeightLat = Math.Abs(pnt2.Lat - pnt1.Lat);
            return size;
        }

        #endregion

        /// <summary>
        /// set current position using keywords
        /// </summary>
        /// <param name="keys"></param>
        /// <returns>true if successfull</returns>
        public PointLatLng? SetCurrentPositionByKeywords(string keys, out GeoCoderStatusCode status)
        {
            status = GeoCoderStatusCode.Unknow;
            Nullable<PointLatLng> pt = null;

            GeocodingProvider gp = GMapProviders.GoogleMap as GeocodingProvider;
            if (gp != null)
            {
                pt = gp.GetPoint(keys, out status);
                if (status == GeoCoderStatusCode.G_GEO_SUCCESS && pt.HasValue)
                {
                    return pt;
                }
            }
            return pt;
        }
    }
}
