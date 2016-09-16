
namespace GMap.NET.Internals
{
    #region Usings
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using GMap.NET.CacheProviders;
    #endregion

    /// <summary>
    /// An extention for GMap Cache class (alternative constructor)
    /// </summary>
    public partial class Cache : Singleton<Cache>
    {
        /// <summary>
        /// Overloaded constructor for the class
        /// </summary>
        public Cache()
        {
            #region singleton check
            if (Instance != null)
            {
                throw (new System.Exception("You have tried to create a new singleton class where you should have instanced it. Replace your \"new class()\" with \"class.Instance\""));
            }
            #endregion

            ImageCache = new SQLitePureImageCache();

            CacheLocation = @"c:\TilesCache\";
        }
    }
}

