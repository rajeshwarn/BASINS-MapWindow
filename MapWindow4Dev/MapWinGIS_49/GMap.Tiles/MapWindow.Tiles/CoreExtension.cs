
namespace GMap.NET.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using GMap.NET.MapProviders;
    using System.Threading;
    using System.Diagnostics;
    
    /// <summary>
    /// Extention for Core class (additional events)
    /// </summary>
    public partial class Core
    {
        /// <summary>
        /// updates map bounds
        /// </summary>
        public void UpdateBounds(List<GPoint> list)
        {
            if (!IsStarted || Provider.Equals(EmptyProvider.Instance))
            {
                return;
            }

            Monitor.Enter(tileLoadQueue);
            try
            {
                tileDrawingListLock.AcquireWriterLock();
                try
                {
                    tileDrawingList.Clear();

                    foreach (GPoint point in list)
                    {
                        tileDrawingList.Add(point);
                    }

                    if (GMaps.Instance.ShuffleTilesOnLoad)
                    {
                        Stuff.Shuffle<GPoint>(tileDrawingList);
                    }

                    foreach (GPoint p in tileDrawingList)
                    {
                        LoadTask task = new LoadTask(p, this.Zoom);
                        {
                            if (!tileLoadQueue.Contains(task))
                            {
                                tileLoadQueue.Push(task);
                            }
                        }
                    }
                }
                finally
                {
                    tileDrawingListLock.ReleaseWriterLock();
                }

                #region -- starts loader threads if needed --

                lock (GThreadPool)
                {
                    while (GThreadPool.Count < GThreadPoolSize)
                    {
                        Thread t = new Thread(new ThreadStart(ProcessLoadTask));
                        {
                            t.Name = "TileLoader: " + GThreadPool.Count;
                            t.IsBackground = true;
                            t.Priority = ThreadPriority.BelowNormal;
                        }
                        GThreadPool.Add(t);

                        Debug.WriteLine("add " + t.Name + " to GThreadPool");

                        t.Start();
                    }
                }
                #endregion

                {
                    LastTileLoadStart = DateTime.Now;
                    Debug.WriteLine("OnTileLoadStart - at zoom " + Zoom + ", time: " + LastTileLoadStart.TimeOfDay);
                }

                loadWaitCount = 0;
                Monitor.PulseAll(tileLoadQueue);
            }
            finally
            {
                Monitor.Exit(tileLoadQueue);
            }

            if (OnTileLoadStart != null)
            {
                OnTileLoadStart();
            }
        }

        /// <summary>
        /// reloads map
        /// </summary>
        public void ReloadMap(List<GPoint> points)
        {
            if (IsStarted)
            {
                Debug.WriteLine("------------------");

                Monitor.Enter(tileLoadQueue);
                try
                {
                    tileLoadQueue.Clear();
                }
                finally
                {
                    Monitor.Exit(tileLoadQueue);
                }

                Matrix.ClearAllLevels();

                lock (FailedLoads)
                {
                    FailedLoads.Clear();
                    RaiseEmptyTileError = true;
                }

                Refresh.Set();

                this.UpdateBounds(points);
            }
            else
            {
                throw new Exception("Please, do not call ReloadMap before form is loaded, it's useless");
            }
        }
    }
}
