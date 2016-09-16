
namespace GMap.NET
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics;
    using System.Threading;
using GMap.NET.Internals;
    #endregion

    /// <summary>
    /// Extention of the GMaps class
    /// </summary>
    public partial  class GMaps : Singleton<GMaps>
    {
        #region events
        public delegate void BeforeCachingEventHandler(RawTile tile, ref bool cancel);
        public event BeforeCachingEventHandler BeforeCaching;
        private void FireBeforeCaching(RawTile tile, ref bool cancel)
        {
            if (BeforeCaching != null)
                BeforeCaching(tile, ref cancel);
        }
        #endregion

        /// <summary>
        /// Enqueueens tile to cache. Checks extents
        /// </summary>
        /// <param name="task"></param>
        public void EnqueueCacheTask(CacheQueueItem task)
        {
            // ask the client whether this tile is actually needed
            bool cancel = false;
            this.FireBeforeCaching(task.Tile, ref cancel);
            if (cancel)
                return;
            
            lock(tileCacheQueue)
            {
                if(!tileCacheQueue.Contains(task))
                {
                    Debug.WriteLine("EnqueueCacheTask: " + task);

                    tileCacheQueue.Enqueue(task);

                    if(CacheEngine != null && CacheEngine.IsAlive)
                    {
                        WaitForCache.Set();
                    }
                    #if PocketPC
                        else if(CacheEngine == null || CacheEngine.State == ThreadState.Stopped || CacheEngine.State == ThreadState.Unstarted)
                    #else
                        else if(CacheEngine == null || CacheEngine.ThreadState == System.Threading.ThreadState.Stopped || CacheEngine.ThreadState == System.Threading.ThreadState.Unstarted)
                    #endif
                    {
                        CacheEngine = null;
                        CacheEngine = new Thread(new ThreadStart(CacheEngineLoop));
                        CacheEngine.Name = "CacheEngine";
                        CacheEngine.IsBackground = false;
                        CacheEngine.Priority = ThreadPriority.Lowest;

                        abortCacheLoop = false;
                        CacheEngine.Start();
                   }
                }
            }
        }
    }
}
