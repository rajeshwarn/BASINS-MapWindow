
namespace GMap.NET
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Forms;
    using GMap.NET.Internals;
    using GMap.NET.MapProviders;
    #endregion

   /// <summary>
   /// form helping to prefetch tiles on local db
   /// </summary>
    public partial class TilePrefetcher : Form
    {
        #region Declarations
        BackgroundWorker worker = new BackgroundWorker();
        List<GPoint> list;
        int zoom;
        GMapProvider provider;
        int sleep;
        int all;
        public bool ShowCompleteMessage = false;
        RectLatLng area;
        GMap.NET.GSize maxOfTiles;
        #endregion

        #region Contsructor
        /// <summary>
        /// Creates a new instance of the tile prefetcher
        /// </summary>
        public TilePrefetcher()
        {
            InitializeComponent();

            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
        }
        #endregion

        #region Start/stop
        /// <summary>
        /// Starts the caching task
        /// </summary>
        /// <param name="area"></param>
        /// <param name="zoom"></param>
        /// <param name="provider"></param>
        /// <param name="sleep"></param>
        public void Start(RectLatLng area, int zoom, GMapProvider provider, int sleep)
        {
            if(!worker.IsBusy)
            {
                this.label1.Text = "...";
                this.progressBar1.Value = 0;

                this.area = area;
                this.zoom = zoom;
                this.provider = provider;
                this.sleep = sleep;

                GMaps.Instance.UseMemoryCache = false;
                GMaps.Instance.CacheOnIdleRead = false;

                worker.RunWorkerAsync();

                this.ShowDialog();
            }
        }

        public void Stop()
        {
            if(worker.IsBusy)
            {
                worker.CancelAsync();
            }
        }
        #endregion

        #region Worker
        /// <summary>
        /// Performs cahing work
        /// </summary>
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (list != null)
            {
                list.Clear();
                list = null;
            }
            list = provider.Projection.GetAreaTileList(area, zoom, 0);
            maxOfTiles = provider.Projection.GetTileMatrixMaxXY(zoom);
            all = list.Count;

            int countOk = 0;
            int retry = 0;

            Stuff.Shuffle<GPoint>(list);

            for (int i = 0; i < all; i++)
            {
                if (worker.CancellationPending)
                    break;

                GPoint p = list[i];
                {
                    if (CacheTiles(zoom, p))
                    {
                        countOk++;
                        retry = 0;
                    }
                    else
                    {
                        if (++retry <= 1) // retry only one
                        {
                            i--;
                            System.Threading.Thread.Sleep(1111);
                            continue;
                        }
                        else
                        {
                            retry = 0;
                        }
                    }
                }

                worker.ReportProgress((int)((i + 1) * 100 / all), i + 1);
                System.Threading.Thread.Sleep(sleep);
            }
            e.Result = countOk;
        }

        /// <summary>
        /// Reports the result of the cahing work
        /// </summary>
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(ShowCompleteMessage)
            {
                if(!e.Cancelled)
                {
                   MessageBox.Show("Prefetch Complete! => " + ((int)e.Result).ToString() + " of " + all);
                }
                else
                {
                   MessageBox.Show("Prefetch Canceled! => " + ((int)e.Result).ToString() + " of " + all);
                }
            }

            list.Clear();

            GMaps.Instance.UseMemoryCache = true;
            GMaps.Instance.CacheOnIdleRead = true;

            this.Close();
        }
               
        /// <summary>
        /// Reports cahing progress
        /// </summary>
        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.label1.Text = "Fetching tile at zoom (" + zoom + "): " + ((int)e.UserState).ToString() + " of " + all + ", complete: " + e.ProgressPercentage.ToString() + "%";
            this.progressBar1.Value = e.ProgressPercentage;
        }
        #endregion

        #region Utility
        bool CacheTiles(int zoom, GPoint p)
        {
            foreach (var pr in provider.Overlays)
            {
                Exception ex;
                PureImage img;

                // tile number inversion(BottomLeft -> TopLeft) for pergo maps
                if (pr is TurkeyMapProvider)
                {
                    img = GMaps.Instance.GetImageFrom(pr, new GPoint(p.X, maxOfTiles.Height - p.Y), zoom, out ex);
                }
                else // ok
                {
                    img = GMaps.Instance.GetImageFrom(pr, p, zoom, out ex);
                }

                if (img != null)
                {
                    img.Dispose();
                    img = null;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region Form events
        /// <summary>
        /// Closes the form on the escape key
        /// </summary>
        private void Prefetch_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }
        }

        /// <summary>
        /// Stops caching on form closing
        /// </summary>
        private void Prefetch_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Stop();
        }
        #endregion
    }
}
