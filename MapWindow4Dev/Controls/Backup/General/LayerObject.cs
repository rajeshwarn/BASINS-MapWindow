
namespace MapWindow.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Diagnostics;
    
    public enum LayerSourceType
    {
        Shapefile = 0,
        Image = 1, 
        Grid = 2,
        Undefined = 3,
    }

    public enum LayerSourceError
    {
        None = 0,
        DbfIsMissing = 1,
        DbfRecordCountMismatch = 2,
        OcxBased = 3,
    }

    /// <summary>
    /// A simple wrapper exposing common functionality for each type of MapWinGIS layers:
    /// shapefile, image, grid
    /// </summary>
    public class LayerSource
    {
        #region Declarations
        private MapWinGIS.Shapefile m_shapefile = null;
        private MapWinGIS.Image m_image = null;
        private MapWinGIS.Grid m_grid = null;
        private LayerSourceError m_error = LayerSourceError.None;
        private string m_ErrorString = "";
        #endregion

        #region Initilization
        /// <summary>
        /// Creates a new instance of LayerSource class
        /// </summary>
        public LayerSource(object obj)
        {
            if (obj is MapWinGIS.Shapefile)
            {
                m_shapefile = obj as MapWinGIS.Shapefile;
            }
            else if (obj is MapWinGIS.Image)
            {
                m_image = obj as MapWinGIS.Image;
            }
            else if (obj is MapWinGIS.Grid)
            {
                m_grid = obj as MapWinGIS.Grid;
            }
        }
        
        /// <summary>
        /// Creates a new instance of LayerSource class
        /// </summary>
        public LayerSource(MapWinGIS.Shapefile sf)
        {
            m_shapefile = sf;
        }

        /// <summary>
        /// Creates a new instance of LayerSource class
        /// </summary>
        public LayerSource(MapWinGIS.Grid grid)
        {
            m_grid = grid;
        }

        /// <summary>
        /// Creates a new instance of LayerSource class
        /// </summary>
        public LayerSource(MapWinGIS.Image image)
        {
            m_image = image;
        }

         /// <summary>
        /// Initializes object from the filename
        /// </summary>
        public LayerSource(string filename): this(filename, null) {}

        /// <summary>
        /// Initializes object from the filename
        /// </summary>
        public LayerSource(string filename, MapWinGIS.ICallback callback)
        {
            this.Open(filename, callback);
        }
                
        #endregion

        #region Properties

        /// <summary>
        /// Returns string description of the last error code
        /// </summary>
        public string GetErrorMessage()
        {
            string s = "";
            switch (m_error)
            {
                case LayerSourceError.DbfIsMissing: 
                    s = "Dbf table is missing";
                    break;
                case LayerSourceError.DbfRecordCountMismatch: 
                    s = "Number of shapes is different from the number of rows in dbf table";
                    break;
                case LayerSourceError.OcxBased:
                    s = m_ErrorString;
                    break;
            }
            m_ErrorString = "";
            m_error = LayerSourceError.None;
            return s;
        }


        /// <summary>
        /// Gets or sets associated callback object
        /// </summary>
        public MapWinGIS.ICallback Callback
        {
            get
            {
                switch (this.Type)
                {
                    case LayerSourceType.Shapefile:
                        return m_shapefile.GlobalCallback;
                    case LayerSourceType.Image:
                        return m_image.GlobalCallback;
                    case LayerSourceType.Grid:
                        return m_grid.GlobalCallback;
                    default:
                        return null;
                }
            }
            set
            {
                switch (this.Type)
                {
                    case LayerSourceType.Shapefile:
                        m_shapefile.GlobalCallback = value;
                        break;
                    case LayerSourceType.Image:
                        m_image.GlobalCallback = value;
                        break;
                    case LayerSourceType.Grid:
                        m_grid.GlobalCallback = value;
                        break;
                    default:
                        // do nothing
                        break;
                 }
            }
        }
        
        /// <summary>
        /// Returns filename of the underlying datasource
        /// </summary>
        public string Filename
        {
            get
            {
                switch (this.Type)
                {
                    case LayerSourceType.Shapefile:
                        return m_shapefile.Filename;
                    case LayerSourceType.Image:
                        return m_image.Filename;
                    case LayerSourceType.Grid:
                        return m_grid.Filename;
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// Returns underlying MapWinGIS interface as pointer of undefined type
        /// </summary>
        public object GetObject
        {
            get
            {
                switch (this.Type)
                {
                    case LayerSourceType.Shapefile:
                        return (object)m_shapefile;
                    case LayerSourceType.Image:
                        return (object)m_image;
                    case LayerSourceType.Grid:
                        return (object)m_grid;
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// Returns type of the layer
        /// </summary>
        public LayerSourceType Type
        {
            get
            {
                if (m_shapefile != null)
                {
                    return LayerSourceType.Shapefile;
                }
                else if (m_image != null)
                {
                    return LayerSourceType.Image;
                }
                else if (m_grid != null)
                {
                    return LayerSourceType.Grid;
                }
                else
                {
                    return LayerSourceType.Undefined;
                }
            }
        }

        /// <summary>
        /// Returns geoprojection of the layer
        /// </summary>
        public MapWinGIS.GeoProjection Projection
        {
            get
            {
                switch (this.Type)
                {
                    case LayerSourceType.Shapefile:
                        return m_shapefile.GeoProjection;
                    case LayerSourceType.Grid:
                        return m_grid.Header.GeoProjection;
                    case LayerSourceType.Image:
                        MapWinGIS.GeoProjection proj = new MapWinGIS.GeoProjection();
                        proj.ImportFromAutoDetect(m_image.GetProjection());
                        return proj;
                    default:
                        return null;
                }
            }

            set
            {
                switch (this.Type)
                {
                    case LayerSourceType.Shapefile:
                        m_shapefile.GeoProjection = value;
                        break;
                    case LayerSourceType.Grid:
                        string s = value != null && !value.IsEmpty ? value.ExportToProj4() : "";
                        m_grid.AssignNewProjection(s);
                        break;
                    case LayerSourceType.Image:
                        // TODO: implement
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Returns filename of the underlying datasource
        /// </summary>
        public MapWinGIS.Extents Extents
        {
            get
            {
                switch (this.Type)
                {
                    case LayerSourceType.Shapefile:
                        return m_shapefile.Extents;
                    case LayerSourceType.Image:
                        return m_image.Extents;
                    case LayerSourceType.Grid:
                        // temporary, for not returing empty object
                        MapWinGIS.Extents ext = new MapWinGIS.Extents();
                        ext.SetBounds(0.0, 0.0, 0.0, 0.0, 0.0, 0.0);
                        return ext;
                    default:
                        return null;
                }
            }
        }

        /// <summary>
        /// Gets shapefile
        /// </summary>
        public MapWinGIS.Shapefile Shapefile
        {
            get { return m_shapefile; }
        }

        /// <summary>
        /// Gets image
        /// </summary>
        public MapWinGIS.Image Image
        {
            get { return m_image; }
        }

        /// <summary>
        /// Gets image
        /// </summary>
        public MapWinGIS.Grid Grid
        {
            get { return m_grid; }
        }

        #endregion

        #region Methods()
        /// <summary>
        /// Closes the layer. Sets layer type to undefined
        /// </summary>
        public void Close()
        {
            m_error = LayerSourceError.None;
            switch (this.Type)
            {
                case LayerSourceType.Shapefile:
                    m_shapefile.Close();
                    m_shapefile = null;
                    break;
                case LayerSourceType.Image:
                    m_image.Close();
                    m_image = null;
                    break;
                case LayerSourceType.Grid:
                    m_grid.Close();
                    m_grid = null;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Opens the layer from the specified data source
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public bool Open(string filename, MapWinGIS.ICallback callback)
        {
            this.Close();

            if (filename.ToLower().EndsWith(".shp"))
            {
                MapWinGIS.Shapefile sf = new MapWinGIS.Shapefile();
                if (sf.Open(filename, callback))
                {
                    // check that dbf exists
                    bool error = false;
                    if (!File.Exists(Path.ChangeExtension(sf.Filename, ".dbf")))
                    {
                        m_error = LayerSourceError.DbfIsMissing;
                        error = true;
                    }

                    // Check that the number of DBF records matches the number of shapes.
                    MapWinGIS.Table table = new MapWinGIS.Table();
                    table.Open(Path.ChangeExtension(sf.Filename, ".dbf"), null);
                    if (sf.NumShapes != table.NumRows)
                    {
                        m_error = LayerSourceError.DbfRecordCountMismatch;
                        error = true;
                    }

                    table.Close();

                    if (error)
                    {
                        sf.Close();
                    }
                    else
                    {
                        m_shapefile = sf;
                    }
                    return !error;
                }
                else
                {
                    m_error = LayerSourceError.OcxBased;
                    m_ErrorString = sf.get_ErrorMsg(sf.LastErrorCode);
                }
            }
            else
            {
                // TODO: probably more intelligent choice between grid/image should be made using application settings
                MapWinGIS.Grid grid = new MapWinGIS.Grid();
                if (grid.Open(filename, MapWinGIS.GridDataType.UnknownDataType, false, MapWinGIS.GridFileType.UseExtension, callback))
                {
                    m_grid = grid;
                    return true;
                }
                else
                {
                    MapWinGIS.Image image = new MapWinGIS.Image();
                    if (image.Open(filename, MapWinGIS.ImageType.USE_FILE_EXTENSION, false, callback))
                    {
                        m_image = image;
                        return true;
                    }
                    else
                    {
                        m_error = LayerSourceError.OcxBased;
                        m_ErrorString = image.get_ErrorMsg(image.LastErrorCode);
                    }
                }
            }
            return false;
        }
        #endregion
    }
}
