﻿namespace MapWindow.Controls.Projections
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    public enum ProjectionOperaion
    {
        Reprojected = 0,
        Assigned = 1,
        Skipped = 2,
        AbsenceIgnored = 3,
        MismatchIgnored = 4,
        Substituted = 5,
        FailedToReproject = 6,
        SameProjection = 7,
    }

    /// <summary>
    /// Types of projection report
    /// </summary>
    public enum ReportType
    {
        Loading = 0,
        Assignment = 1,
    }

    public partial class frmTesterReport : Form, MapWinGIS.ICallback 
    {
        #region Declaration
        // Data grid view columns
        private const int CMN_FILENAME = 0;
        private const int CMN_PROJECTION = 1;
        private const int CMN_OPERATION = 2;
        private const int CMN_NEW_NAME = 3;
        private const int CMN_ERROR = 4;
        #endregion

        /// <summary>
        /// Creates a new instance of the frmTesterReport class
        /// </summary>
        public frmTesterReport()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the number of mismatched files
        /// </summary>
        public int MismatchedCount
        {
            get 
            {
                return listView1.Items.Count;
            }
        }

        /// <summary>
        /// Shows the report window as non-modal form
        /// </summary>
        /// <param name="proj">Project projection</param>
        public void InitProgress(MapWinGIS.GeoProjection proj)
        {
            this.Text = "Reprojecting";
            this.label1.Text = "Reprojection of files is performed.";
            this.SetProjectProjection(proj, ReportType.Loading);
            this.lblProjection.Visible = false;
            this.Show();
        }

        /// <summary>
        /// Shows the report as modal dialog
        /// </summary>
        /// <param name="proj">Project projection</param>
        public void ShowReport(MapWinGIS.GeoProjection proj, string message, ReportType type)
        {
            this.Text = type == ReportType.Loading ? "Projection checking results" : "Projection assignment results";
            this.SetProjectProjection(proj, type);
            this.ShowReportCore(proj, message);
            this.ShowDialog();
        }

        /// <summary>
        /// Performs common actions for report showing which don't depend on the report type (layer loading or assignment of projection)
        /// </summary>
        /// <param name="proj"></param>
        /// <param name="message"></param>
        private void ShowReportCore(MapWinGIS.GeoProjection proj, string message)
        {
            if (message == "")
            {
                message = "The following files were affected because of the projection mismatch or absence:";
            }
            
            this.label1.Text = message;
            this.lblProjection.Visible = true;
            this.lblFile.Visible = false;
            this.progressBar1.Visible = false;
            this.button1.Visible = true;
            if (this.Visible)
            {
                this.Visible = false;
            }
        }

        /// <summary>
        /// Displays project projection
        /// </summary>
        /// <param name="proj"></param>
        private void SetProjectProjection(MapWinGIS.GeoProjection proj, ReportType reportType)
        {
            string suffix = "Target projection: ";
            
            if (proj == null || proj.IsEmpty)
            {
                lblProjection.Text = suffix + "not defined";
            }
            else
            {
                lblProjection.Text = suffix + proj.Name;
            }
        }

        /// <summary>
        /// Adds new file to the report
        /// </summary>
        /// <param name="name">Filename</param>
        /// <param name="projection">Projection name</param>
        /// <param name="operation">Operation that was applied to the file</param>
        /// <param name="newName">The new name of the file (in case of reprojection)</param>
        public void AddFile(string filename, string projection, ProjectionOperaion operation, string newName)
        {
            string s = operation.ToString();
            switch (operation)
            {
                case ProjectionOperaion.AbsenceIgnored:
                    s = "Absence ignored";
                    break;
                case ProjectionOperaion.MismatchIgnored:
                    s = "Mismatch ignored";
                    break;
                case ProjectionOperaion.FailedToReproject:
                    s = "Failed to reproject";
                    break;
            }
            
            ListViewItem item = listView1.Items.Add(System.IO.Path.GetFileName(filename));
            item.SubItems.Add(projection == "" ? "none" : projection);
            item.SubItems.Add(s);
            item.SubItems.Add(System.IO.Path.GetFileName(newName));

            if (operation == ProjectionOperaion.Skipped || operation == ProjectionOperaion.FailedToReproject)
            {
                MapWinGIS.GlobalSettings settings = new MapWinGIS.GlobalSettings();
                item.SubItems.Add(settings.GdalReprojectionErrorMsg);
            }
            else
            {
                item.SubItems.Add("");
            }
            listView1.Refresh();
            Globals.AutoResizeColumns(this.listView1);
            Application.DoEvents();
        }

        #region ICallback Members

        public void ShowFilename(string filename)
        {
            lblFile.Text = "File: " + filename;
            lblFile.Visible = true;
            this.progressBar1.Visible = true;
            this.Refresh();
            Application.DoEvents();
        }

        public void ClearFilename()
        {
            lblFile.Visible = false;
            this.progressBar1.Visible = false;
        }

        public void Progress(string KeyOfSender, int Percent, string Message)
        {
            this.progressBar1.Value = Percent;
        }

        public void Error(string KeyOfSender, string ErrorMsg)
        {
            // do nothing
            //throw new NotImplementedException();
        }
        #endregion
    }
}
