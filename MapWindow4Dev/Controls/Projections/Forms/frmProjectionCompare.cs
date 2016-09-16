
namespace MapWindow.Controls.Projections
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    
    public partial class frmProjectionCompare : Form
    {
        // project projection
        private MapWinGIS.GeoProjection m_projectProj = null;

        // layer projection
        private MapWinGIS.GeoProjection m_layerProj = null;

        private ProjectionDatabase m_database = null;

        /// <summary>
        /// Creates a new instance of the frmProjectionCompare class
        /// </summary>
        public frmProjectionCompare(MapWinGIS.GeoProjection projectProj, MapWinGIS.GeoProjection layerProj, ProjectionDatabase database)
        {
            InitializeComponent();

            m_projectProj = projectProj;
            m_layerProj = layerProj;
            m_database = database;

            this.lblProject.Text = "Project: " + projectProj.Name;
            this.lblLayer.Text = "Layer: " + layerProj.Name;

            this.txtProject.Text = projectProj.ExportToProj4();
            this.txtLayer.Text = layerProj.ExportToProj4();

            this.btnLayer.Click += delegate(object sender, EventArgs e)
            {
                this.ShowProjectionProperties(m_layerProj);
            };

            this.btnProject.Click += delegate(object sender, EventArgs e)
            {
                this.ShowProjectionProperties(m_projectProj);
            };
        }

        /// <summary>
        /// Shows properties for the selected projection
        /// </summary>
        private void ShowProjectionProperties(MapWinGIS.GeoProjection proj)
        {
            if (proj == null || proj.IsEmpty)
                return;

            CoordinateSystem cs = null;
            if (m_database != null)
                cs = m_database.GetCoordinateSystem(proj, ProjectionSearchType.Enhanced);

            if (cs != null)
            {
                frmProjectionProperties form = new frmProjectionProperties(cs, m_database);
                form.ShowDialog(this);
                form.Dispose();
            }
            else
            {
                frmProjectionProperties form = new frmProjectionProperties(proj);
                form.ShowDialog(this);
                form.Dispose();
            }
        }
    }
}
