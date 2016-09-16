
namespace MapWindow.Controls.Projections
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    using System.Collections;

    /// <summary>
    /// Shows list of options to choose from
    /// </summary>
    internal partial class frmProjectionMismatch : Form
    {
        // project projection
        private MapWinGIS.GeoProjection m_projectProj = null;
        
        // layer projection
        private MapWinGIS.GeoProjection m_layerProj = null;

        // db to show projection infor
        private ProjectionDatabase m_database = null;
        
        /// <summary>
        /// Creates a new instance of frmProjectionMismatch class. ShowProjectionMismatch and ShowProjectionAbsence
        /// calls are needed to to the job.
        /// </summary>
        internal frmProjectionMismatch(ProjectionDatabase database)
        {
            InitializeComponent();

            m_database = database;

            btnLayer.Click += delegate(object sender, EventArgs e)
            {
                frmProjectionCompare form = new frmProjectionCompare(m_projectProj, m_layerProj, m_database);
                form.ShowDialog(this);
                form.Dispose();
                
            };
        }

        /// <summary>
        /// Closes the form
        /// </summary>
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndex >= 0)
                this.DialogResult = DialogResult.OK;
        }
      
        /// <summary>
        /// Shows list of options for projection mismatch situations
        /// </summary>
        /// <param name="dontShow">User has marked 'never show the dialog' checkbox</param>
        public int ShowProjectionMismatch(ArrayList list, int selectedIndex, MapWinGIS.GeoProjection projectProj, MapWinGIS.GeoProjection layerProj, out bool useForOthers, out bool dontShow)
        {
            if (projectProj == null || layerProj == null)
                throw new ArgumentException("No projections for mismatch dialog specified");

            m_projectProj = projectProj;
            m_layerProj = layerProj;

            this.Text = "Projection mismatch";
            this.lblMessage.Text = "Layer projection is different from project one." + Environment.NewLine +  
                                   "Choose the way how to handle it:";
            return this.ShowProjectionDialog(list, selectedIndex, out useForOthers, out dontShow);
        }

        /// <summary>
        /// Shows projection absence dialog
        /// </summary>
        public int ShowProjectionAbsence(ArrayList list, int selectedIndex, MapWinGIS.GeoProjection projectProj, out bool useForOthers, out bool dontShow)
        {
            if (projectProj == null)
                throw new ArgumentException("No projections for mismatch dialog specified");

            m_projectProj = projectProj;

            this.Text = "Projection absence";
            this.lblMessage.Text = "Layer projection isn't specified." + Environment.NewLine +
                                   "Choose the way how to handle it:";
            btnLayer.Visible = false;
            return this.ShowProjectionDialog(list, selectedIndex, out useForOthers, out dontShow);
        }

        /// <summary>
        /// Shows the dialog
        /// </summary>
        private int ShowProjectionDialog(ArrayList list, int selectedIndex, out bool useForOthers, out bool neverShowDialog)
        {
            if (list.Count == 0)
                throw new ArgumentException("List of options must not be empty");

            this.listBox1.DataSource = list;
            if (selectedIndex > 0 && selectedIndex < this.listBox1.Items.Count)
            {
                this.listBox1.SelectedIndex = selectedIndex;
            }
            else if (this.listBox1.Items.Count > 0)
            {
                this.listBox1.SelectedIndex = 0;
            }

            int index = (this.ShowDialog() == DialogResult.OK) ? this.listBox1.SelectedIndex : -1;
            neverShowDialog = this.chkShowMismatchWarning.Checked;
            useForOthers = this.chkUseAnswerLater.Checked;

            return index;
        }

        
    }
}
