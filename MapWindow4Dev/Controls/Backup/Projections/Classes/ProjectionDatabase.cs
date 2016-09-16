// EPSG Reference: a tool for visualization of EPSG coordinate system database
// Author: Sergei Leschinski

#define SQLITE_DATABASE

namespace MapWindow.Controls.Projections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using MapWindow.Data;
    using System.Data.Common;

    /// <summary>
    /// Serach types used by GetCoordinateSystemFunction
    /// </summary>
    public enum ProjectionSearchType
    {
        /// <summary>
        /// Only comparison by name among geographic and projected coordinate systems is made
        /// </summary>
        Standard = 0,

        /// <summary>
        /// Same as standard, but additional after using proj4 export-import operation
        /// </summary>
        Enhanced = 1,

        /// <summary>
        /// The same as enhanced, but dialects are used
        /// </summary>
        UseDialects = 2,
    }

    /// <summary>
    /// Encapsulates all the interaction with modified EPSG projections database
    /// </summary>
    public class ProjectionDatabase: MapWindow.Interfaces.IProjectionDatabase 
    {
        #region Constants

        // names of columns in Regions table
        const string CMN_REGION_CODE = "Region_Code";
        const string CMN_REGION_NAME = "Region_Name";
        const string CMN_REGION_PARENT = "Parent_Region_Code";
        const string CMN_REGION_INDEX = "Index";

        // names of columns in Country table
        const string CMN_COUNTRY_CODE = "Country_Code";
        const string CMN_COUNTRY_NAME = "Country_Name";
        const string CMN_COUNTRY_PARENT = "Region_Code";

        const string CMN_COUNTRY_XMIN = "xMin";
        const string CMN_COUNTRY_XMAX = "xMax";
        const string CMN_COUNTRY_YMIN = "yMin";
        const string CMN_COUNTRY_YMAX = "yMax";

        // names of columns for both GCS and PCS tables
        const string CMN_CS_CODE = "COORD_REF_SYS_CODE";
        const string CMN_CS_NAME = "COORD_REF_SYS_NAME";
        const string CMN_CS_AREA_CODE = "AREA_CODE";
        const string CMN_CS_SOUTH = "AREA_SOUTH_BOUND_LAT";
        const string CMN_CS_NORTH = "AREA_NORTH_BOUND_LAT";
        const string CMN_CS_LEFT = "AREA_WEST_BOUND_LON";
        const string CMN_CS_RIGHT = "AREA_EAST_BOUND_LON";
        const string CMN_CS_SOURCE = "SOURCE_GEOGCRS_CODE";
        const string CMN_CS_PROJECTION = "PROJECTION_TYPE";
        const string CMN_CS_SCOPE = "CRS_SCOPE";
        const string CMN_CS_LOCAL = "LOCAL";
        const string CMN_CS_AREA_NAME = "AREA_OF_USE";
        const string CMN_CS_REMARKS = "REMARKS";
        const string CMN_CS_Proj4 = "Proj4";

        // types of CS used
        const string CS_TYPE_GEOGRAPHIC_2D = "geographic 2D";
        const string CS_TYPE_GEOGRAPHIC_3D = "geographic 3D";
        const string CS_TYPE_PROJECTED = "projected";

        // sql queries
        const string SQL_REGIONS = "SELECT * FROM [Countries] WHERE [Level] < 3"; //"SELECT * FROM [mwRegions] ORDER BY [Index]";
        const string SQL_COUNTRIES = "SELECT * FROM [Countries] WHERE [Level] = 3";
        const string SQL_GCS = "SELECT * FROM [Coordinate Systems] WHERE [COORD_REF_SYS_KIND] = " + "\"" + CS_TYPE_GEOGRAPHIC_2D + "\"";
        const string SQL_PCS = "SELECT * FROM [Coordinate Systems] WHERE [COORD_REF_SYS_KIND] = " + "\"" + CS_TYPE_PROJECTED + "\"";
        const string SQL_CS_BY_COUNTRY = "SELECT * FROM [Country by Coordinate System] WHERE [REGION] = 0";
        const string SQL_GCS_BY_REGION = "SELECT * FROM [Country by Coordinate System] WHERE [REGION] <> 0";

        private int CODE_AREA_WORLD = 1262;

        #endregion

        #region Local variables

        // The filename of the database 
        private string m_dbname = "";

        // return GCS structure by its code
        private Hashtable m_dctRegions = new Hashtable();
        private Hashtable m_dctCountries = new Hashtable();
        private Hashtable m_dctGCS = new Hashtable();
        private Dictionary<string, int> m_dialects = new Dictionary<string,int>();

        // list of geographic CS
        private List<Region> m_listRegions = new List<Region>();
        private List<Country> m_listCountries = new List<Country>();
        private List<GeographicCS> m_listGCS = new List<GeographicCS>();
        private List<ProjectedCS> m_listPCS = new List<ProjectedCS>();

        private IDataProvider m_provider = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor. Sets SqLite provider
        /// </summary>
        public ProjectionDatabase()
        {
            #if SQLITE_DATABASE
            m_provider = new SQLiteProvider();
            #else
            m_provider = new OleDbProvider();
            #endif
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public ProjectionDatabase(IDataProvider provider) 
        {
            if (provider == null)
                throw new NullReferenceException("Provider for projections database wasn't specified");
            m_provider = provider;
        }

        /// <summary>
        /// Creates a new instance of the EpsgDatabase class
        /// </summary>
        public ProjectionDatabase(string databaseName, IDataProvider provider)
        {
            if (provider == null)
            {
                throw new NullReferenceException("Provider for projections database wasn't specified");
            }
            m_provider = provider;
            
            if (!System.IO.File.Exists(databaseName))
            {
                throw new System.IO.FileNotFoundException("EPSG database wan't found: " + databaseName);
            }
            this.Read(databaseName);
        }
        #endregion

        #region Coordinate system searching
        /// <summary>
        /// Gets coordinate system by EPSG code
        /// </summary>
        /// <param name="epsgCode">EPSG code of the projection</param>
        public CoordinateSystem GetCoordinateSystem(int epsgCode)
        {
            foreach (GeographicCS gcs in this.m_listGCS)
            {
                if (gcs.Code == epsgCode)
                    return gcs;
            }
            foreach (ProjectedCS pcs in this.m_listPCS)
            {
                if (pcs.Code == epsgCode)
                    return pcs;
            }
            return null;
        }
        
        /// <summary>
        /// Gets coordinate system from database for specified projection string. Any projection format can be used.
        /// </summary>
        public CoordinateSystem GetCoordinateSystem(string str, ProjectionSearchType searchType)
        {
            CoordinateSystem cs = null;
            MapWinGIS.GeoProjection proj = new MapWinGIS.GeoProjection();
            if (proj.ImportFromAutoDetect(str))
            {
                cs = this.GetCoordinateSystem(proj, searchType);
            }
            return cs;
        }
        
        /// <summary>
        /// Returns coordinate system object associated with given GeoProjection.
        /// This property is computationally expensive.
        /// </summary>
        public CoordinateSystem GetCoordinateSystem(MapWinGIS.GeoProjection projection, ProjectionSearchType searchType)
        {
            if (m_dbname == "")
                return null;

            // standard
            CoordinateSystem cs = this.GetCoordinateSystemCore(projection);
            if (searchType == ProjectionSearchType.Standard || cs != null)
                return cs;

            // enhanced
            MapWinGIS.GeoProjection projTest = new MapWinGIS.GeoProjection();
            if (projTest.ImportFromProj4(projection.ExportToProj4()))
                cs = this.GetCoordinateSystemCore(projection);
            if (searchType == ProjectionSearchType.Enhanced || cs != null)
                return cs;
            
            // dialects
            this.RefreshDialects();
            int code = this.EpsgCodeByDialectString(projection);
            if (code != -1)
            {
                return this.GetCoordinateSystem(code);
            }
            return null;
        }

        /// <summary>
        /// Searches coordinate system
        /// </summary>
        private CoordinateSystem GetCoordinateSystemCore(MapWinGIS.GeoProjection proj)
        {
            string name = proj.Name.ToLower();

            if (proj.IsEmpty)
            {
                return null;
            }
            else if (proj.IsGeographic)
            {
                foreach (GeographicCS gcs in this.m_listGCS)
                {
                    if (gcs.Name.ToLower() == name)
                        return gcs;
                }
            }
            else if (proj.IsProjected)
            {
                foreach (ProjectedCS pcs in this.m_listPCS)
                {
                    if (pcs.Name.ToLower() == name)
                        return pcs;
                }
            }
            return null;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets databass name
        /// </summary>
        public string Name
        {
            get { return m_dbname; }
        }
        
        /// <summary>
        /// Returns list of regions
        /// </summary>
        public List<Region> Regions
        {
            get { return m_listRegions; }
        }

        /// <summary>
        /// Returns list of regions
        /// </summary>
        public List<Country> Countries
        {
            get { return m_listCountries; }
        }

        /// <summary>
        /// Returns list of regions
        /// </summary>
        public List<GeographicCS> GeographicCS
        {
            get { return m_listGCS; }
        }

        /// <summary>
        /// Returns list of regions
        /// </summary>
        public List<ProjectedCS> ProjectedCS
        {
            get { return m_listPCS; }
        }

        /// <summary>
        /// Gets the unified list of geographical and projected ccordinate systems
        /// </summary>
        public IEnumerable<CoordinateSystem> CoordinateSystems
        {
            get
            {
                return m_listGCS.Cast<CoordinateSystem>().Union(m_listPCS.Cast<CoordinateSystem>());
            }
        }

        #endregion

        #region Dialects
        /// <summary>
        /// Save dialects to the database
        /// </summary>
        public void SaveDialects(CoordinateSystem cs)
        {
            using (DbConnection conn = m_provider.CreateConnection()) 
            {
                conn.ConnectionString = m_provider.CreateConnectionString(m_dbname);
                conn.Open();
                try
                {
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        // deleting recodrs
                        cmd.CommandText = "DELETE FROM [Dialects] WHERE Code = ?";
                        DbParameter param = m_provider.CreateParameter();
                        param.Value = cs.Code;
                        cmd.Parameters.Add(param);
                        cmd.ExecuteNonQuery();

                        //cmd.Parameters.Add( new OleDbParameter("p1", cs.Code));

                        // inserting records
                        cmd.CommandText = "INSERT INTO [Dialects] VALUES(?, ?)";
                        cmd.Parameters.Add(m_provider.CreateParameter());

                        //cmd.Parameters.Add(new OleDbParameter());
                        foreach (string s in cs.Dialects)
                        {
                            cmd.Parameters[1].Value = s.Trim();
                            cmd.ExecuteNonQuery();
                        }
                        cmd.Dispose();
                    }
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Reads dialects for a given coordinate system
        /// </summary>
        public void ReadDialects(CoordinateSystem cs)
        {
            cs.Dialects.Clear();

            using (DbConnection conn = m_provider.CreateConnection(m_dbname))
            {
                conn.Open();
                try
                {
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        // deleting recodrs
                        cmd.CommandText = "SELECT [ProjString] FROM [Dialects] WHERE Code = ?";
                        DbParameter param = m_provider.CreateParameter();
                        param.Value = cs.Code;
                        cmd.Parameters.Add(param);
                        DbDataReader reader = cmd.ExecuteReader();
                        
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                cs.Dialects.Add(reader.GetString(0));
                            }
                        }
                        cmd.Dispose();
                    }
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Refreshes the dictionary with the complete list of dialects. Should be called a single time before EpsgCodeByDialectString call.
        /// </summary>
        public void RefreshDialects()
        {
            m_dialects.Clear();

            using (DbConnection conn = m_provider.CreateConnection(m_dbname))
            {
                conn.Open();
                try
                {
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        // deleting recodrs
                        cmd.CommandText = "SELECT * FROM [Dialects]";
                        DbDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                m_dialects.Add(reader.GetString(1), reader.GetInt32(0));
                            }
                        }
                        cmd.Dispose();
                    }
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Returns code that correspnds to the given dialect string
        /// </summary>
        /// <param name="proj">Projection string in either proj4 or WKT format</param>
        /// <returns>EPSG code</returns>
        public int EpsgCodeByDialectString(string proj)
        {
            return m_dialects.ContainsKey(proj) ? m_dialects[proj] : -1;
        }

        /// <summary>
        /// Returns code that correspnds to the given dialect string
        /// </summary>
        /// <param name="proj">Projection object; WKT, proj4 formats will be tested</param>
        /// <returns>EPSG code</returns>
        public int EpsgCodeByDialectString(MapWinGIS.GeoProjection proj)
        {
            int code = this.EpsgCodeByDialectString(proj.ExportToProj4());
            if (code == -1)
            {
                code = this.EpsgCodeByDialectString(proj.ExportToWKT());
            }
            return code;
        }
        #endregion

        #region Reading
        /// <summary>
        /// Reads the database
        /// </summary>
        /// <param name="executablePath">The path to MapWindow.exe</param>
        /// <returns></returns>
        public bool ReadFromExecutablePath(string executablePath)
        {
            string path = System.IO.Path.GetDirectoryName(executablePath) + @"\Projections\";
            if (!Directory.Exists(path))
            {
                MessageBox.Show("Projections folder isn't found: " + path);
                return false;
            }

            string extention = "";            
            #if SQLITE_DATABASE
                extention = "*.db3";
            #else
                extention = "*.mdb";
            #endif

            string[] files = Directory.GetFiles(path, extention);
            if (files.Length != 1)
            {
                MessageBox.Show("A single database is expected. " + files.Length.ToString() + " databases are found." + Environment.NewLine +
                                "Path : " + path + Environment.NewLine);
                return false;
            }
            else
            {
                return this.Read(files[0]);
            }
        }
        
        /// <summary>
        /// Queries database and fill the list of GCS
        /// There are 4 levels in hierarchy: Regions->Countries->GCS->PCS
        /// The linking inforation:
        /// Regions->Countries (Countries.REGION_CODE on Region.REGION_cODE)
        /// Regions->GCS (mwGCSByRegion)
        /// Countries->GCS (mwGCSByCountry)
        /// GCS->PCS (PCS.SOUCRCE_CODE on GCS.CODE)
        /// </summary>
        public bool Read(string dbName)
        {
            //System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            //watch.Start();
            
            m_dbname = dbName;
            
            // regions
            m_listRegions = new List<Region>(30);
            this.ReadRegions(ref m_listRegions);

            // countries
            m_listCountries = new List<Country>(300);
            this.ReadCountries(ref m_listCountries);

            // geographic CS
            m_listGCS = new List<GeographicCS>(800);
            this.ReadGCS(ref m_listGCS);

            // projected CS
            m_listPCS = new List<ProjectedCS>(3000);
            this.ReadPCS(ref m_listPCS);

            // linking regions and countries
            foreach (Country country in m_listCountries)
            {
                if (m_dctRegions.ContainsKey(country.RegionCode))
                {
                    Region region = (Region)m_dctRegions[country.RegionCode];
                    region.Countries.Add(country);
                }
                else
                {
                    System.Diagnostics.Debug.Print("Specified region wasn't found: " + country.RegionCode);
                }
            }

            // regional GCS
            this.LinkGCSToRegion();

            // linking GCS to country
            this.LinkGCSToCountry();

            // linking PCS to GCS
            foreach (ProjectedCS pcs in m_listPCS)
            {
                if (m_dctGCS.ContainsKey(pcs.SourceCode))
                {
                    GeographicCS gcs = (GeographicCS)m_dctGCS[pcs.SourceCode];
                    gcs.Projections.Add(pcs);
                }
                else
                {
                    System.Diagnostics.Debug.Print("Source geographic CS for projected CS wasn't found: " + pcs.Code);
                }
            
            }

            // reading dialects
            this.RefreshDialects();
            
            return true;
        }

        /// <summary>
        /// Links geographic CS to countries
        /// </summary>
        private void LinkGCSToRegion()
        {
            DbConnection conn = m_provider.CreateConnection(m_dbname);
            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = SQL_GCS_BY_REGION;
            conn.Open();
            DbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int codeGcs = reader.GetInt32(1);
                    if (m_dctGCS.ContainsKey(codeGcs))
                    {
                        GeographicCS gcs = (GeographicCS)m_dctGCS[codeGcs];
                        gcs.Type = GeographicalCSType.Regional;
                        gcs.RegionCode = reader.GetInt32(0); ;
                    }
                }
            }
        }

        /// <summary>
        /// Links geographic CS to countries
        /// </summary>
        private void LinkGCSToCountry()
        {
            DbConnection conn = m_provider.CreateConnection(m_dbname); //new OleDbConnection(this.get_ConectionString());
            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = SQL_CS_BY_COUNTRY;

            conn.Open();
            DbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int codeCountry = reader.GetInt32(0);
                    if (m_dctCountries.ContainsKey(codeCountry))
                    {
                        Country country = (Country)m_dctCountries[codeCountry];

                        string projType = reader.GetString(2);
                        if (projType == CS_TYPE_GEOGRAPHIC_2D)
                        {
                            int codeGcs = reader.GetInt32(1);
                            if (m_dctGCS.ContainsKey(codeGcs))
                            {
                                GeographicCS gcs = (GeographicCS)m_dctGCS[codeGcs];
                                country.GeographicCS.Add(gcs);
                            }
                        }
                        else if (projType == CS_TYPE_PROJECTED)
                        {
                            int codePCS = reader.GetInt32(1);
                            country.ProjectedCS.Add(codePCS);
                        }
                    }
                }
            }

            // global CS which should be added for each country
            var listGlobalCS = m_listGCS.Where(gcs => gcs.AreaCode == CODE_AREA_WORLD);
            foreach (Country country in m_listCountries)
            {
                foreach (var gcs in listGlobalCS)
                    country.GeographicCS.Add(gcs);
            }
        }

        /// <summary>
        /// Reads region from database and 
        /// </summary>
        /// <returns></returns>
        private bool ReadRegions(ref List<Region> list)
        {
            m_listRegions.Clear();

            DbConnection conn = m_provider.CreateConnection(m_dbname); // new DbConnection(this.get_ConectionString());
            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = SQL_REGIONS;

            conn.Open();
            DbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            int codeColumn = ColumnIndexByName(reader, CMN_COUNTRY_CODE);
            int nameColumn = ColumnIndexByName(reader, CMN_COUNTRY_NAME);
            int parentColumn = ColumnIndexByName(reader, CMN_COUNTRY_PARENT);

            if (codeColumn == -1 || nameColumn == -1 || parentColumn == -1)
            {
                MessageBox.Show("The expected field isn't found in the [Region] table", "Field not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Region region = new Region();
                    region.Code = reader.GetInt32(codeColumn);
                    region.Name = reader.GetString(nameColumn);
                    region.ParentCode = reader.GetInt32(parentColumn);
                    m_listRegions.Add(region);
                    m_dctRegions.Add(region.Code, region);
                }
            }
            return true;
        }

        /// <summary>
        /// Returns index of the field by its name and -1 in case none field with such name exists
        /// </summary>
        private int ColumnIndexByName(DbDataReader reader, string name)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (name.ToLower() == reader.GetName(i).ToLower())
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Reading countries
        /// </summary>
        private bool ReadCountries(ref List<Country> list)
        {
            // adding countries
            DataTable dtCountries = this.get_Table(SQL_COUNTRIES, false);
            if (dtCountries != null)
            {
                foreach (DataRow row in dtCountries.Rows)
                {
                    Country country = new Country();
                    country.Code = (int)row[CMN_COUNTRY_CODE];
                    country.Name = (string)row[CMN_COUNTRY_NAME];
                    country.RegionCode = (int)row[CMN_COUNTRY_PARENT];
                    country.Left = this.ParseDouble(row[CMN_COUNTRY_XMIN]);
                    country.Right = this.ParseDouble(row[CMN_COUNTRY_XMAX]);
                    country.Bottom = this.ParseDouble(row[CMN_COUNTRY_YMIN]);
                    country.Top = this.ParseDouble(row[CMN_COUNTRY_YMAX]);
                    list.Add(country);
                    m_dctCountries.Add(country.Code, country);
                }
            }
            return true;
        }

        /// <summary>
        /// Reads list of geographic CS
        /// </summary>
        private bool ReadGCS(ref List<GeographicCS> list)
        {
            DataTable dt = this.get_Table(SQL_GCS, false);
            if (dt == null)
            {
                throw new Exception("mwGRS query wansn't found in the database: " + m_dbname);
            }

            foreach (DataRow row in dt.Rows)
            {
                GeographicCS gcs = new GeographicCS();
                gcs.Code = int.Parse(row[CMN_CS_CODE].ToString());
                gcs.Name = row[CMN_CS_NAME].ToString();
                gcs.Left = this.ParseDouble(row[CMN_CS_LEFT]);
                gcs.Right = this.ParseDouble(row[CMN_CS_RIGHT]);
                gcs.Top = this.ParseDouble(row[CMN_CS_NORTH]);
                gcs.Bottom = this.ParseDouble(row[CMN_CS_SOUTH]);
                gcs.AreaCode = int.Parse(row[CMN_CS_AREA_CODE].ToString());
                gcs.Scope = (row[CMN_CS_SCOPE]).ToString();
                gcs.AreaName = (row[CMN_CS_AREA_NAME]).ToString();
                gcs.Remarks = (row[CMN_CS_REMARKS]).ToString();
                gcs.proj4 = (row[CMN_CS_Proj4]).ToString();

                // setting type of GCS
                gcs.Type = (gcs.Left != -180.0f || gcs.Bottom != -90.0f || gcs.Right != 180.0f || gcs.Top != 90.0f) ?
                            GeographicalCSType.Local : GeographicalCSType.Global;

                list.Add(gcs);

                // adding to hastable
                m_dctGCS.Add(gcs.Code, gcs);
            }

            return true;
        }

        /// <summary>
        /// Reads list of Projected CS
        /// </summary>
        /// <returns></returns>
        private bool ReadPCS(ref List<ProjectedCS> list)
        {
            DbConnection conn = m_provider.CreateConnection(m_dbname); // new DbConnection(this.get_ConectionString());
            DbCommand cmd = conn.CreateCommand();
            cmd.CommandText = SQL_PCS;

            conn.Open();
            DbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            int codeColumn = ColumnIndexByName(reader, CMN_CS_CODE);
            int nameColumn = ColumnIndexByName(reader, CMN_CS_NAME);
            int sourceColumn = ColumnIndexByName(reader, CMN_CS_SOURCE);
            int leftColumn = ColumnIndexByName(reader, CMN_CS_LEFT);
            int rightColumn = ColumnIndexByName(reader, CMN_CS_RIGHT);
            int bottomColumn = ColumnIndexByName(reader, CMN_CS_SOUTH);
            int topColumn = ColumnIndexByName(reader, CMN_CS_NORTH);
            int areaNameColumn = ColumnIndexByName(reader, CMN_CS_AREA_NAME);
            int remarksColumn = ColumnIndexByName(reader, CMN_CS_REMARKS);
            int scopeColumn = ColumnIndexByName(reader, CMN_CS_SCOPE);
            int typeColumn = ColumnIndexByName(reader, CMN_CS_PROJECTION);
            int localColumn = ColumnIndexByName(reader, CMN_CS_LOCAL);
            int proj4Column = ColumnIndexByName(reader, CMN_CS_Proj4);


            if (codeColumn == -1 || nameColumn == -1 || sourceColumn == -1 ||
                leftColumn == -1 || rightColumn == -1 || bottomColumn == -1 ||
                topColumn == -1 || areaNameColumn == -1 || remarksColumn == -1 ||
                scopeColumn == -1 || typeColumn == -1 || localColumn == -1 || proj4Column == -1)
            {
                MessageBox.Show("The expected field isn't found in the [Coordinate Systems] table", "Field not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ProjectedCS pcs = new ProjectedCS();
                    pcs.Code = reader.GetInt32(codeColumn);
                    pcs.Name = reader.GetString(nameColumn);
                    pcs.Left = reader.GetDouble(leftColumn);
                    pcs.Right = reader.GetDouble(rightColumn);
                    pcs.Top = reader.GetDouble(topColumn);
                    pcs.Bottom = reader.GetDouble(bottomColumn);
                    pcs.SourceCode = reader.GetInt32(sourceColumn);
                    pcs.Scope = reader.GetString(scopeColumn);
                    pcs.AreaName = reader.GetString(areaNameColumn);
                    pcs.proj4 = (!reader.IsDBNull(proj4Column)) ? reader.GetString(proj4Column) : "";

                    pcs.ProjectionType = (!reader.IsDBNull(typeColumn)) ? reader.GetString(typeColumn) : "";
                    pcs.Remarks = (!reader.IsDBNull(remarksColumn)) ? reader.GetString(remarksColumn) : "";

                    pcs.Local = false;
                    if (!reader.IsDBNull(localColumn))
                        pcs.Local = reader.GetBoolean(localColumn);
                    
                    list.Add(pcs);
                }
            }
            return true;
        }

        #endregion

        #region Utilities
        /// <summary>
        /// Parses double value.
        /// </summary>
        /// <param name="val"></param>
        /// <returns>0.0 on failure.</returns>
        private double ParseDouble(object val)
        {
            if (val == null)
            {
                return 0.0;
            }
            else
            {
                string s = val.ToString();
                if (s != "")
                    return double.Parse(s);
                else
                    return 0.0;
            }
        }

        /// <summary>
        /// Returns coonection string for EPSG database
        /// </summary>
        //internal string get_ConectionString()
        //{
        //    return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_dbname + ";User Id=admin;Password=;";
        //}

        /// <summary>
        /// Returns selected table form the database
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="tableDirect"></param>
        internal DataTable get_Table(string commandText, bool tableDirect)
        {
            DbDataAdapter adapter = m_provider.CreateDataAdapter();
            DbConnection conn = m_provider.CreateConnection(m_dbname); // new DbConnection(this.get_ConectionString());

            try
            {
                DataTable dt = new DataTable();
                DbCommand cmd = conn.CreateCommand();

                cmd.CommandType = tableDirect ? CommandType.TableDirect : CommandType.Text;
                cmd.CommandText = commandText;

                adapter.SelectCommand = cmd;
                adapter.Fill(dt);

                return dt;

                // Data reader can be used instead, probably it'll be faster
                //OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
                return null;
            }
        }
        #endregion

        #region Fill country by area
        /// <summary>
        /// Utility function to fill one of the tables fo modified EPSG database
        /// </summary>
        /// <param name="dbName">The filename of database to work with</param>
        public void FillCountryByArea(string dbName)
        {
            string connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dbName + ";User Id=admin;Password=;";
            DataSet ds = new DataSet();
            DbDataAdapter adapter = m_provider.CreateDataAdapter();
            DbConnection conn = m_provider.CreateConnection(m_dbname); // new DbConnection(this.get_ConectionString());

            try
            {
                DataTable dtArea = new DataTable();
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM [Area]";
                adapter.SelectCommand = cmd;
                adapter.Fill(dtArea);

                DataTable dtCoutries = new DataTable();
                cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM [Countries] WHERE [Level] = 3";
                adapter.SelectCommand = cmd;
                adapter.Fill(dtCoutries);

                DataTable dtResults = new DataTable();
                cmd.CommandType = CommandType.TableDirect;
                cmd.CommandText = "Country By Area";
                adapter.SelectCommand = cmd;
                adapter.Fill(dtResults);

                //OleDbCommandBuilder cb = new OleDbCommandBuilder(adapter);
                //adapter.InsertCommand = cb.GetInsertCommand();

                // CommandBuilder seems to be not clever enough to put brackets around the name of the table
                DbCommand cmdInsert = m_provider.CreateCommand();
                cmdInsert.CommandText = "INSERT INTO [Country By Area] (AREA_CODE, COUNTRY_CODE) VALUES (@AREA_CODE, @COUNTRY_CODE)";
                cmdInsert.Connection = conn;

                // Add the parameters for the InsertCommand.
                DbParameter param = m_provider.CreateParameter();
                param.ParameterName = "@AREA_CODE";
                param.DbType = DbType.Int32;
                param.Size = 10;
                param.SourceColumn = "AREA_CODE";
                cmdInsert.Parameters.Add(param);

                param = m_provider.CreateParameter();
                param.ParameterName = "@COUNTRY_CODE";
                param.DbType = DbType.Int32;
                param.Size = 10;
                param.SourceColumn = "COUNTRY_CODE";
                cmdInsert.Parameters.Add(param);
                
                //cmdInsert.Parameters.Add("@AREA_CODE", OleDbType.Integer, 10, "AREA_CODE");
                //cmdInsert.Parameters.Add("@COUNTRY_CODE", OleDbType.Integer, 10, "COUNTRY_CODE");

                adapter.InsertCommand = cmdInsert;

                string[] list = new string[dtCoutries.Rows.Count];

                for (int i = 0; i < dtCoutries.Rows.Count; i++)
                {
                    string s = dtCoutries.Rows[i]["Alias"].ToString().ToLower();
                    list[i] = s;
                    System.Diagnostics.Debug.Print(s);
                }

                int count = 0;

                for (int j = 0; j < list.Length; j++)
                {
                    string exclude = "";
                    if (list[j] == "niger") exclude = "nigeria";
                    if (list[j] == "oman") exclude = "romania";
                    if (list[j] == "mexico") exclude = "new mexico";
                    if (list[j] == "georgia") exclude = "usa";
                    if (list[j] == "georgia") exclude = "south georgia";
                    if (list[j] == "jersey") exclude = "new jersey";
                    if (list[j] == "india") exclude = "indiana";
                    if (list[j] == "antarctic") exclude = "australian";
                    if (list[j] == "canada") exclude = "canada plantation";
                    if (list[j] == "netherlands") exclude = "netherlands antilles";
                    if (list[j] == "china") exclude = "hong kong";
                    if (list[j] == "india") exclude = "bassas da india";
                    if (list[j] == "guinea") exclude = "papua new guinea";

                    for (int i = 0; i < dtArea.Rows.Count; i++)
                    {
                        string s = dtArea.Rows[i][2].ToString().ToLower();
                        if (s.Contains(list[j]))
                        {
                            if (exclude != "" && s.Contains(exclude))  // excluding unwanted coutries
                                continue;

                            DataRow row = dtResults.NewRow();
                            row[0] = dtArea.Rows[i][0];
                            row[1] = dtCoutries.Rows[j][0];
                            dtResults.Rows.Add(row);

                            System.Diagnostics.Debug.Print(s + ": " + list[j]);
                            count++;
                        }
                    }
                    System.Diagnostics.Debug.Print("--------------------------------");
                }

                adapter.Update(dtResults);
                MessageBox.Show(string.Format("Matches found: {0}", count));
            }
            finally
            {
                ds.Clear();
            }
        }
        #endregion

        #region UpdateProj4
        /// <summary>
        /// Updates proj4 strings in the proj4 column of coordinate reference systems table
        /// Assumes that there is Coordinate Reference System table in the database (was removed from Sqlite).
        /// </summary>
        /// <returns></returns>
        public void UpdateProj4Strings(string dbName)
        {
            DataSet ds = new DataSet();
            DbDataAdapter adapter = m_provider.CreateDataAdapter();
            DbConnection conn = m_provider.CreateConnection(dbName);

            try
            {
                DataTable dt = new DataTable();
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT [COORD_REF_SYS_CODE], [proj4] FROM [Coordinate Reference System]";
                adapter.SelectCommand = cmd;
                adapter.Fill(dt);

                DbCommand cmdUpdate = conn.CreateCommand();
                cmdUpdate.CommandText = "UPDATE [Coordinate Reference System] SET proj4 = @proj4 WHERE COORD_REF_SYS_CODE = @cs";

                DbParameter param = m_provider.CreateParameter();
                param.ParameterName = "@proj4";
                param.DbType = DbType.String;
                param.Size = 254;
                param.SourceColumn = "proj4";
                cmdUpdate.Parameters.Add(param);

                param = m_provider.CreateParameter();
                param.ParameterName = "@cs";
                param.DbType = DbType.Int32;
                param.Size = 10;
                param.SourceColumn = "COORD_REF_SYS_CODE";
                cmdUpdate.Parameters.Add(param);
                
                //cmdUpdate.Parameters.Add("@proj4", OleDbType.Char, 254, "proj4");
                //cmdUpdate.Parameters.Add("@cs", OleDbType.Integer, 10, "COORD_REF_SYS_CODE");
                adapter.UpdateCommand = cmdUpdate;

                int count = 0;
                MapWinGIS.GeoProjection proj = new MapWinGIS.GeoProjection();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int code = (int)dt.Rows[i][0];
                    if (proj.ImportFromEPSG(code))
                    {
                        string proj4 = proj.ExportToProj4();
                        dt.Rows[i][1] = proj4;
                        count++;
                    }
                }

                adapter.Update(dt);
                MessageBox.Show(string.Format("Records updated: {0}", count));
            }
            finally
            {
                ds.Clear();
            }
        }
        #endregion
    }
}
