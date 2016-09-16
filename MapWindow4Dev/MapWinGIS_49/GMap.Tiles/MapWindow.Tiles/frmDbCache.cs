
namespace MapWindow.Tiles
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using GMap.NET;
    using GMap.NET.CacheProviders;
    using GMap.NET.MapProviders;
    #endregion

    /// <summary>
    /// Provides GUI for choosing caching database
    /// </summary>
    public partial class frmDbCache : Form
    {
        private TileClient m_client = null;
        
        #region Constructor
        /// <summary>
        /// Creates a new instance of the frmDbCache class
        /// </summary>
        public frmDbCache(TileClient client)
        {
            InitializeComponent();
            if (client == null)
                throw new NullReferenceException("Client reference wasn't passed");
            
            m_client = client;

            SQLitePureImageCache cache = m_client.Cache;
            if (cache != null)
            {
                this.txtDatabase.Text = cache.DbName;
            }

            //this.UpdateTreeView();
        }
        #endregion

        #region Create/open
        /// <summary>
        /// Creates empty database
        /// </summary>
        private void btnCreate_Click(object sender, EventArgs e)
        {
            //if (this.txtDatabase.Text == "")
            //{
            //    Globals.MessageBoxInformation("Database name is not specified");
            //    return;
            //}

            //string filename = this.txtDatabase.Text;
            //if (File.Exists(filename))
            //{
            //    Globals.MessageBoxInformation("A file with such name alaready exists");
            //    return;
            //}
            //else
            //{
            //    SQLitePureImageCache.CreateEmptyDB(filename);
            //}

            SaveFileDialog dlg = new SaveFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {

            }
            dlg.Dispose();
        }

        /// <summary>
        /// Opens a database
        /// </summary>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Database file (*.gmdb)|*.gmdb";
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                MessageBox.Show("Database is about to be opened: " + dialog.FileName);
            }
            dialog.Dispose();
        }
        #endregion

        
    }
}
