
namespace GMap.NET.CacheProviders
{
    #region Usings
    using System.Data.SQLite;
    using System;
    using System.Diagnostics;
    using System.Data.Common;
    using System.Data;
    using System.Linq;
    using System.Collections.Generic;
    using GMap.NET.MapProviders;
    #endregion

    #region TileInfo
    /// <summary>
    /// A stricture to hold information about individual tile
    /// </summary>
    public class TileInfo
    {
        public int X;
        public int Y;
        public int Zoom;
    }
    #endregion

    /// <summary>
    /// Extentended functionality for SQLitePureImageCache class
    /// </summary>
    public partial class SQLitePureImageCache: PureImageCache 
    {
        #region Columns indices
        private const int CMN_ID = 0;
        private const int CMN_X = 1;
        private const int CMN_Y = 2;
        private const int CMN_ZOOM = 3;
        private const int CMN_TYPE = 4;
        private const int CMN_CACHE_TIME = 4;
        #endregion

        #region Extract info
        /// <summary>
        /// Returns the list of providers from the database. Not finished
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GMapProvider> GetProvidersList()
        {
            IEnumerable<GMapProvider> selection = new List<GMapProvider>();
            List<int> types = new List<int>();

            try
            {
                using (SQLiteConnection cn = new SQLiteConnection(this.ConnectionString))
                {
                    cn.Open();
                    {
                        using (DbCommand command = cn.CreateCommand())
                        {
                            command.CommandText = "SELECT DISTINCT [Type] from [Tiles]";
                            using (DbDataReader reader = command.ExecuteReader(CommandBehavior.SequentialAccess))
                            {
                                while (reader.Read())
                                {
                                    types.Add(reader.GetInt32(0));
                                }
                            }
                        }
                    }
                    cn.Close();
                }

                selection = GMapProviders.List.Where(p => types.Contains(p.DbId));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetProvidersList: " + ex.ToString());
            }
            return selection;
        }

        /// <summary>
        /// Reads information about tiles of the specified provider
        /// </summary>
        public List<TileInfo> GetProviderInfo(int providerDbId)
        {
            List<TileInfo> list = new List<TileInfo>();
            try
            {
                using (SQLiteConnection cn = new SQLiteConnection())
                {
                    cn.ConnectionString = this.ConnectionString;
                    cn.Open();

                    using (DbCommand command = cn.CreateCommand())
                    {
                        command.CommandText = "SELECT * from [Tiles] WHERE Type = " + providerDbId.ToString();
                        using (DbDataReader reader = command.ExecuteReader(CommandBehavior.SequentialAccess))
                        {
                            while (reader.Read())
                            {
                                TileInfo info = new TileInfo();
                                info.X = reader.GetInt32(CMN_X);
                                info.Y = reader.GetInt32(CMN_Y);
                                info.Zoom = reader.GetInt32(CMN_ZOOM);
                                list.Add(info);
                            }
                        }
                    }
                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetProviderInfo: " + ex.ToString());
            }
            return list;
        }
        #endregion

        #region Remove
        /// <summary>
        /// Removes all the cached tiles for a given provider from the database
        /// </summary>
        public bool RemoveTiles(int providerDbId)
        {
            return this.RemoveTiles(providerDbId, -1);
        }
        
        /// <summary>
        /// Removes tiles for the given provider and scale
        /// </summary>
        public bool RemoveTiles(int providerDbId, int scale)
        {
            int result = 0;
            try
            {
                using (SQLiteConnection cn = new SQLiteConnection())
                {
                    cn.ConnectionString = this.ConnectionString;
                    cn.Open();

                    using (DbCommand command = cn.CreateCommand())
                    {
                        string scaleClause = scale == -1 ? "" : " and [Zoom] = " + scale.ToString();
                        command.CommandText = "DELETE from [Tiles] WHERE Type = " + providerDbId.ToString() + scaleClause;
                        result = command.ExecuteNonQuery();
                    }
                    cn.Close();
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetImageFromCache: " + ex.ToString());
            }
            return result > 0;
        }
        #endregion
    }
}

