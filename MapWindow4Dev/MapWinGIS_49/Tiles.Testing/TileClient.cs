// GMap client
// Author: Sergei Leschinski
// Created on: 27 aug 2011

namespace MapWindow.Tiles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics;
    using GMap.NET;
    using GMap.NET.Internals;
    using GMap.NET.MapProviders;
    using GMap.NET.WindowsForms;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using System.ComponentModel;

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
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of TileClient class
        /// </summary>
        /// <param name="map">Reference to the instance of AxMapWinGIS.AxMap control</param>
        public TileClient(AxMapWinGIS.AxMap map)
        {
            if (map == null)
            {
                throw new ArgumentException("Reference to map wasn't passed");
            }
            else
            {
                m_map = map;
                m_map.ExtentsChanged += new EventHandler(map_ExtentsChanged);
            }

            GMaps.Instance.UseMemoryCache = false;
            GMaps.Instance.Mode = AccessMode.ServerOnly;

            GMapProvider.TileImageProxy = wimg;
            m_core.Provider = GMapProviders.YandexHybridMap;
            
            //m_core.OnMapOpen();
            //m_core.CurrentPosition = new PointLatLng(53.9, 27.5);
            //m_core.Zoom = 18;
            //m_core.sizeOfMapArea = new GSize(1000, 1000);
            //m_core.sizeOfMapArea = new GSize(5, 5);

            m_core.OnTileLoadStart += delegate()
            {
                Debug.Print("Start loading tiles");
            };

            m_core.OnTileLoadComplete += delegate(long ElapsedMilliseconds)
            {
                //MessageBox.Show("End loading tiles: {0}", ((double)ElapsedMilliseconds / 1000.0).ToString());
            };

           
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the tiles provider
        /// </summary>
        public GMapProvider Provider
        {
            get { return m_core.Provider; }
            set 
            { 
                m_core.Provider = value;
                this.map_ExtentsChanged(null, null);
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
        #endregion

        #region Events

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
        void ClearGMapMemory()
        {
            //m_core.tileDrawingList.Clear();
            //foreach (GPoint tilePoint in m_core.tileDrawingList.Clear())
            //{
                
            //    if (!m_tiles.ContainsKey(tilePoint))
            //    {
            //        Tile t = m_core.Matrix.GetTileWithNoLock(m_core.Zoom, tilePoint);
            //        if (t != Tile.Empty)
            //        {
            //            m_tiles.Add(tilePoint, 0);
            //        }
        }
                
        
        /// <summary>
        /// Reloads tiles after extents are changed
        /// </summary>
        void map_ExtentsChanged(object sender, EventArgs e)
        {
            Debug.Print("Access mode: " + GMaps.Instance.Mode.ToString());
            Debug.Print("Memory cache: " + GMaps.Instance.UseMemoryCache.ToString());
            
            m_count = 0;

            m_tiles.Clear();
            
            Debug.Print("Tile cache queue count: " + GMaps.Instance.tileCacheQueue.Count);
            Debug.Print("Tiles in emory: " + GMaps.Instance.TilesInMemory.Count);
            
            if (!m_enabled)
                return;
            
            MapWinGIS.Extents ext = (MapWinGIS.Extents)m_map.Extents;
            RectLatLng rect = new RectLatLng(ext.yMin, ext.xMin, ext.yMax - ext.yMin, ext.xMax - ext.xMin);

            int zoom = this.GetZoomLevel();
            m_core.Zoom = zoom;

            PureProjection projection = m_core.Provider.Projection;
            m_core.minOfTiles = projection.GetTileMatrixMinXY(zoom);
            m_core.maxOfTiles = projection.GetTileMatrixMaxXY(zoom);

            //PureProjection projection = m_core.Provider.Projection;
            //List<GPoint> points = projection.GetAreaTileList(rect, zoom, 0);

            // center point
            PointLatLng location = new PointLatLng((ext.yMax + ext.yMin)/2.0, (ext.xMax + ext.xMin)/2.0);
            GPoint centerPoint = this.FromLatLngToXY(location, zoom);
            m_core.centerTileXYLocation = centerPoint;

            // the number of tiles around
            GPoint p1 = this.FromLatLngToXY(new PointLatLng(ext.yMin, ext.xMin), zoom);
            GPoint p2 = this.FromLatLngToXY(new PointLatLng(ext.yMax, ext.xMax), zoom);

            int xCount = Math.Abs(centerPoint.X - p1.X);
            if (Math.Abs(centerPoint.X - p2.X) > xCount)
                xCount = Math.Abs(centerPoint.X - p2.X);

            int yCount = Math.Abs(centerPoint.Y - p1.Y);
            if (Math.Abs(centerPoint.Y - p2.Y) > yCount)
                yCount = Math.Abs(centerPoint.Y - p2.Y);

             m_core.sizeOfMapArea = new GSize(xCount, yCount);

             //m_core.Provider = m_core.Provider;
             m_core.ReloadMap();
        }

        /// <summary>
        /// Adds newly loaded tiles to the map and triggers its update
        /// </summary>
        private void AddTiles()
        {
            //return;
            
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

                            // adding to the listview
                            lock (t.Overlays)
                            {
                                for (int i = 0; i < t.Overlays.Count; i++)
                                {
                                    // position
                                    PointLatLng point = this.FromXYToLngLat(tilePoint, m_core.Zoom);

                                    // scale
                                    SizeLatLng size = this.GetTileSizeLatLon(tilePoint, m_core.Zoom);
                                    //double scale = size.WidthLng * m_map.PixelsPerDegree / 256.0;

                                    PureImage img = t.Overlays[i];
                                    byte[] data = img.Data.ToArray();
                                    m_map.Tiles.Add(data, point.Lng, point.Lat, size.WidthLng, size.HeightLat);
                                    m_count++;

                                    //System.Diagnostics.Debug.Print(string.Format("Tile: lng = {0:000.00}; lat = {1:000.00}, x = {2},y = {3}, zoom = {4}, count = {6}", 
                                      //                                            point.Lng, point.Lat, tilePoint.X, tilePoint.Y, m_core.Zoom, m_count));

                                    m_map.Refresh();
                                    
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
        /// Returns number of tiles loaded in particulr moment
        /// </summary>
        /// <returns></returns>
        public int GetTileCount()
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
        #endregion

        #region Coordinates
        /// <summary>
        /// Finds closest zoom level for particular scale of map
        /// Tile size must be as close to original as possible, but not greater then it
        /// </summary>
        private int GetZoomLevel()
        {
            MapWinGIS.Extents ext = (MapWinGIS.Extents)m_map.Extents;
            double lon = (ext.xMax + ext.xMin)/2.0;
            double lat = (ext.yMax + ext.yMin) / 2.0;

            PointLatLng location = new PointLatLng(lat, lon);

            //double minValue = double.MaxValue;
            int bestZoom = 0;
            for (int i = m_core.Provider.MinZoom; i < m_core.Provider.MaxZoom; i++)
            {
                GPoint point = this.FromLatLngToXY(location, i);
                SizeLatLng size = this.GetTileSizeLatLon(point, i);

                double tileSize = (size.WidthLng + size.HeightLat) * m_map.PixelsPerDegree / 2.0;
                
                if (tileSize > 256 * 0.85)
                {
                    //minValue = Math.Abs(tileSize - 256);
                    bestZoom = i;
                }
                //double x = size.WidthLng;
                //double y = size.HeightLat;
                //SpatialReference.ProjectPoint(ref x, ref y, GEOGRAPHIC_PROJECTION_PROJ4, TILE_PROJECTION_PROJ4);
            }
            return bestZoom;
        }

        /// <summary>
        /// Returns index of tile for particular geographic coordinates
        /// </summary>
        private GPoint FromLatLngToXY(PointLatLng point, int zoom)
        {
            PureProjection projection = m_core.Provider.Projection;
            GPoint pointNew = projection.FromPixelToTileXY(projection.FromLatLngToPixel(point, zoom));
            return pointNew;
        }
        
        /// <summary>
        /// Returns geographic coordinates of tile with given index
        /// </summary>
        private PointLatLng FromXYToLngLat(GPoint point, int zoom)
        {
            PureProjection projection = m_core.Provider.Projection;
            PointLatLng pointNew = projection.FromPixelToLatLng(projection.FromTileXYToPixel(point), zoom);
            return pointNew;
        }

        /// <summary>
        /// Returns the size of a single tile for a g
        /// </summary>
        /// <returns></returns>
        private SizeLatLng GetTileSizeLatLon(GPoint point, int zoom)
        {
            PointLatLng pnt1 = this.FromXYToLngLat(point, zoom);
            
            // moving to the neighbouring tile
            if (point.X < m_core.maxOfTiles.Width - 1) point.X++;
            else point.X--;

            if (point.Y < m_core.maxOfTiles.Height - 1) point.Y++;
            else point.Y--;

            PointLatLng pnt2 = this.FromXYToLngLat(point, zoom);
            
            // size
            SizeLatLng size = new SizeLatLng();
            size.WidthLng = Math.Abs( pnt2.Lng - pnt1.Lng);
            size.HeightLat = Math.Abs(pnt2.Lat - pnt1.Lat);
            return size;
        }

        #endregion

        #region Testing
        /// <summary>
        /// Tests loading at different scales
        /// </summary>
        public void TestScales()
        {
            System.Diagnostics.Stopwatch watch = new Stopwatch();

            m_map.MapUnits = MapWinGIS.tkUnitsOfMeasure.umDecimalDegrees;
            double[] scales = { 100000, 1000000, 50000 };
            foreach (double scale in scales)
            {
                m_map.CurrentScale = scale;
                int zoom = this.GetZoomLevel();
                Debug.Print(string.Format("Scale {0}; zoom = {1}", scale, zoom));
            }

            //watch.Start();
            //PointLatLng location = new PointLatLng(54.0, 27.0);

            //for (int i = m_core.Provider.MinZoom; i < m_core.Provider.MaxZoom; i++)
            //{
            //    GPoint point = this.FromLatLngToXY(location, i);
            //    SizeLatLng size = this.GetTileSizeLatLon(point, i);
            //    System.Diagnostics.Debug.Print(string.Format("Zoom {0}; size = {1}", i, size.WidthLng));
            //}
            Debug.Print(watch.Elapsed.ToString());
        }
        #endregion
    }
}
