
namespace GMap.NET
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Forms;
    using GMap.NET.Internals;
    using GMap.NET.MapProviders;
    using MapWindow.Tiles;
    #endregion

   /// <summary>
   /// form helping to prefetch tiles on local db
   /// </summary>
    public partial class frmPrefetcher : Form
    {
        #region Declarations
        private TileClient m_client = null;
        private RectLatLng m_area;
        #endregion

        #region Contsructor
        /// <summary>
        /// Creates a new instance of the tile prefetcher
        /// </summary>
        public frmPrefetcher(TileClient client, RectLatLng area)
        {
            InitializeComponent();

            m_client = client;
            m_area = area;

            this.txtExtents.Text = string.Format("Lat: {0} to {1}; Lng: {2} to {3}",
                                                  area.Left.ToString("0.000"),
                                                  area.Right.ToString("0.000"),
                                                  area.Bottom.ToString("0.000"),
                                                  area.Top.ToString("0.000"));

            this.FillTreeView();

            if (this.treeView1.Nodes.Count > 0)
                this.treeView1.SelectedNode = this.treeView1.Nodes[0];
        }

        /// <summary>
        /// Fills tree view with tiles
        /// </summary>
        private void FillTreeView()
        {
            this.treeView1.Nodes.Clear();
            IEnumerable<GMapProvider> list = GMapProviders.List.Where(p => !p.Name.Equals("None", StringComparison.CurrentCultureIgnoreCase)); // m_client.Cache.GetProvidersList();
            
            foreach (GMapProvider item in list)
            {
                TreeNode node = this.treeView1.Nodes.Add(item.Name);
                node.Tag = item;
            }
        }

        /// <summary>
        /// Fills tiles info by scale for a given provider
        /// </summary>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.listView1.Items.Clear();

            GMapProvider provider = this.SelectedProvider;
            if (provider != null)
            {
                int minZoom = provider.MinZoom;
                int maxZoom = provider.MaxZoom.Value;

                int number = 0;
                for (int i = (int)minZoom; i <= maxZoom; i++)
                {
                    if (number > 5000)
                    {
                        ListViewItem item = this.listView1.Items.Add("");
                        item.Checked = false;
                        item.SubItems.Add(i.ToString());
                        item.SubItems.Add(">" + number.ToString());
                        item.SubItems.Add(">" + (number * 0.02).ToString("0.00"));
                        item.SubItems.Add("");
                    }
                    else
                    {
                        List<GPoint> points = provider.Projection.GetAreaTileList(m_area, i, 0);
                        if (points.Count > 1)
                        {
                            ListViewItem item = this.listView1.Items.Add("");
                            item.Checked = true;
                            item.SubItems.Add(i.ToString());
                            item.SubItems.Add(points.Count.ToString());
                            item.SubItems.Add((points.Count * 0.02).ToString("0.00"));
                            item.SubItems.Add("");
                        }
                        number = points.Count;
                    }
                }
            }
        }
        #endregion

        #region Start prefetching
        /// <summary>
        /// Starts the loading routine
        /// </summary>
        private void Ok_Click(object sender, EventArgs e)
        {
            GMapProvider provider = this.SelectedProvider;
            if (provider != null)
            {
                for (int i = 0; i < this.listView1.Items.Count; i++)
                {
                    if (this.listView1.Items[i].Checked)
                    {
                        int scale = Int32.Parse(this.listView1.Items[i].SubItems[1].Text);
                        TilePrefetcher form = new TilePrefetcher();
                        form.Owner = this;
                        form.ShowCompleteMessage = false ;
                        form.Start(m_area, scale, provider, 100);
                        if (form.DialogResult == DialogResult.Abort)
                        {
                            this.listView1.Items[i].SubItems[4].Text = "Cancelled";
                            break;
                        }
                        else
                        {
                            this.listView1.Items[i].SubItems[4].Text = "Ok";
                        }
                    }
                }
            }
        }
        #endregion

        #region Utilities
        /// <summary>
        /// Toggles on/off all the checkboxes
        /// </summary>
        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < this.listView1.Items.Count; i++)
            {
                this.listView1.Items[i].Checked = this.chkSelectAll.Checked;
            }
        }

        /// <summary>
        /// Gets selected provider
        /// </summary>
        private GMapProvider SelectedProvider
        {
            get
            {
                return this.treeView1.SelectedNode != null ? this.treeView1.SelectedNode.Tag as GMapProvider : null;
            }
        }
        #endregion
    }
}
