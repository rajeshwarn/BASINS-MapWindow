// MapWindow.Controls.Projections.MismatcTester
// Author: Sergei Leschinski
// Created: 20 July 2011

namespace MapWindow.Controls.Projections
{
    using System;
    using System.Collections.Generic;
    using System.Collections;
    using System.IO;
    using System.Linq;
    using System.Text;
    using MapWindow.Interfaces;
    using System.Windows.Forms;

    /// <summary>
    /// Possible results of testing
    /// </summary>
    public enum TestingResult
    {
        /// <summary>
        /// Object is ok or user has ignored the mismatch
        /// </summary>
        Ok = 0,
        /// <summary>
        /// File should be skipped
        /// </summary>
        SkipFile = 1,
        /// <summary>
        /// Operatio should be canceled
        /// </summary>
        CancelOperation = 2,
        /// <summary>
        /// Error occured while processing
        /// </summary>
        Error = 3,
        /// <summary>
        /// The layer object was substituted by another file
        /// </summary>
        Substituted = 4
    }

    /// <summary>
    /// A class to handle various projection mismatch scenarious while adding a layer to the map
    /// </summary>
    public class MismatchTester
    {
        #region Declaration
        
        // Reference to MapWindow
        private MapWindow.Interfaces.IMapWin m_mapWin = null;

        // a from to show report in
        private frmTesterReport m_report = new frmTesterReport();

        // prevents showing the dialog for the group of files when the user don't want to        
        private bool m_usePreviousAnswerMismatch = false;

        // prevents showing the dialog for the group of files when the user don't want to        
        private bool m_usePreviousAnswerAbsence = false;

        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new instance of MismatchTester class
        /// </summary>
        public MismatchTester(MapWindow.Interfaces.IMapWin mapWin)
        {
            if (mapWin == null)
                throw new NullReferenceException("No reference to MapWindow was passed");
            
            m_mapWin = mapWin;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the list of files where projection mismatch happened (or projction absence)
        /// </summary>
        public int FileCount
        {
            get 
            {
                if (m_report == null)
                {
                    return 0;
                }
                else
                {
                    return m_report.MismatchedCount;
                }
            }
        }
        #endregion

        #region Report methods
        public void ShowReprojectionProgress(MapWinGIS.GeoProjection proj)
        {
            m_report.InitProgress(proj);
        }

        public void ShowReport(MapWinGIS.GeoProjection proj)
        {
            m_report.ShowReport(proj, "", ReportType.Loading);
        }

        public void HideProgress()
        {
            m_report.Visible = false;
        }
        #endregion

        #region Testing

        /// <summary>
        /// Tests projection of a single layer, type of layer is determined by extention
        /// </summary>
        public TestingResult TestLayer(string filename, out string newName)
        {
            newName = filename;
            LayerSource layer = new LayerSource(filename, this as MapWinGIS.ICallback);
            if (layer.Type == LayerSourceType.Undefined)
            {
                string message = layer.GetErrorMessage();
                if (message == "")
                    message = "Unspecified error";

                MessageBox.Show("Invalid datasource: " + message.ToLower() + Environment.NewLine + filename, m_mapWin.ApplicationInfo.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return TestingResult.Error;
            }
            else
            {
                LayerSource newLayer = null;
                TestingResult result = this.TestLayer(layer, out newLayer);
                if (result == TestingResult.Substituted)
                {
                    newName = newLayer.Filename;
                    newLayer.Close();
                }

                layer.Close();
                return result;
            }
        }
        
        /// <summary>
        /// Tests projection of a single layer
        /// </summary>
        /// <param name="layer">Shapefile or grid object</param>
        /// <param name="mapWin">Reference to MapWindow</param>
        public TestingResult TestLayer(LayerSource layer, out LayerSource newLayer)
        {
            if (layer == null)
                throw new ArgumentException("Empty layer reference was passed");

            newLayer = null;

            MapWinGIS.GeoProjection projectProj = m_mapWin.Project.GeoProjection;
            MapWinGIS.GeoProjection layerProj = layer.Projection;
            bool isSame = projectProj.get_IsSameExt(layerProj, layer.Extents, 10);

            // let's check whether we have a dialect of the project projection
            if (!isSame && !projectProj.IsEmpty)
            {
                ProjectionDatabase db = (ProjectionDatabase)m_mapWin.ProjectionDatabase;
                if (db != null)
                {
                    CoordinateSystem cs = db.GetCoordinateSystem(projectProj, ProjectionSearchType.Enhanced);
                    if (cs != null)
                    {
                        db.ReadDialects(cs);
                        foreach (string dialect in cs.Dialects)
                        {
                            MapWinGIS.GeoProjection projTemp = new MapWinGIS.GeoProjection();
                            if (!projTemp.ImportFromAutoDetect(dialect))
                                continue;

                            if (layerProj.get_IsSame(projTemp))
                            {
                                isSame = true;
                                break;
                            }
                        }
                    }
                }
            }

            // projection can be included in the name of the file as suffix, let's try to search the file with correct suffix
            if (!isSame)
            {
                if (CoordinateTransformation.SeekSubstituteFile(layer, projectProj, out newLayer))
                {
                    m_report.AddFile(layer.Filename, layer.Projection.Name, ProjectionOperaion.Substituted, newLayer.Filename);
                    return TestingResult.Substituted;
                }
            }

            if (!layer.Projection.IsEmpty)
            {
                if (projectProj.IsEmpty)
                {
                    // layer has a projection, project - doesn't; assign to projection, don't prompt user

                    // let's try to find well known projection with EPSG code
                    ProjectionDatabase db = m_mapWin.ProjectionDatabase as ProjectionDatabase;
                    if (db != null)
                    {
                        CoordinateSystem cs = db.GetCoordinateSystem(layerProj, ProjectionSearchType.UseDialects);
                        if (cs != null)
                        {
                            MapWinGIS.GeoProjection proj = new MapWinGIS.GeoProjection();
                            if (proj.ImportFromEPSG(cs.Code))
                                layerProj = proj;
                        }
                    }
                    
                    m_mapWin.Project.GeoProjection = layerProj;
                    return TestingResult.Ok;
                }
                else if (isSame)
                {
                    // the same projection 
                    return TestingResult.Ok;
                }
                else
                {
                    // user should be prompted
                    if (!m_usePreviousAnswerMismatch && !m_mapWin.ApplicationInfo.NeverShowProjectionDialog)
                    {
                        bool dontShow = false;
                        bool useForOthers = false;

                        ArrayList list = new ArrayList();
                        list.Add("Ignore mismatch");
                        list.Add("Reproject file");
                        list.Add("Skip file");

                        frmProjectionMismatch form = new frmProjectionMismatch((ProjectionDatabase)m_mapWin.ProjectionDatabase);

                        int choice = form.ShowProjectionMismatch(list, (int)m_mapWin.ApplicationInfo.ProjectionMismatchBehavior, 
                                                                 projectProj, layer.Projection, out useForOthers, out dontShow);

                        form.Dispose();
                        if (choice == -1)
                            return TestingResult.CancelOperation;

                        m_usePreviousAnswerMismatch = useForOthers;
                        m_mapWin.ApplicationInfo.ProjectionMismatchBehavior = (ProjectionMismatchBehavior)choice;
                        m_mapWin.ApplicationInfo.NeverShowProjectionDialog = dontShow;
                    }

                    MapWindow.Interfaces.ProjectionMismatchBehavior behavior = m_mapWin.ApplicationInfo.ProjectionMismatchBehavior;
                    
                    switch (behavior)
                    {
                        case ProjectionMismatchBehavior.Reproject:
                            TestingResult result = CoordinateTransformation.ReprojectLayer(layer, out newLayer, projectProj, m_report);
                            if (result == TestingResult.Ok || result == TestingResult.Substituted)
                            {
                                ProjectionOperaion oper = result == TestingResult.Ok ? ProjectionOperaion.Reprojected : ProjectionOperaion.Substituted;
                                string newName = newLayer == null ? "" : newLayer.Filename;
                                m_report.AddFile(layer.Filename, layer.Projection.Name, oper, newName);
                                return newName == layer.Filename ? TestingResult.Ok : TestingResult.Substituted;
                            }
                            else
                            {
                                m_report.AddFile(layer.Filename, layer.Projection.Name, ProjectionOperaion.FailedToReproject, "");
                                return TestingResult.Error;
                            }

                        case ProjectionMismatchBehavior.IgnoreMismatch:
                            m_report.AddFile(layer.Filename, layer.Projection.Name, ProjectionOperaion.MismatchIgnored, "");
                            return TestingResult.Ok;
                        
                        case ProjectionMismatchBehavior.SkipFile:
                            m_report.AddFile(layer.Filename, layer.Projection.Name, ProjectionOperaion.Skipped, "");
                            return TestingResult.SkipFile;
                    }
                }
            }
            else if (!projectProj.IsEmpty)          // layer projection is empty
            {
                bool projectProjectionExists = !projectProj.IsEmpty;

                // user should be prompted
                if (!m_usePreviousAnswerAbsence && !m_mapWin.ApplicationInfo.NeverShowProjectionDialog)
                {
                    bool dontShow = false;
                    bool useForOthers = false;

                    ArrayList list = new ArrayList();

                    // when there in projection the first variant should be excluded
                    int val = projectProjectionExists ? 0 : 1;

                    if (projectProjectionExists)
                        list.Add("Assign projection from project");
                    list.Add("Ignore the absence");
                    list.Add("Skip the file");

                    frmProjectionMismatch form = new frmProjectionMismatch((ProjectionDatabase)m_mapWin.ProjectionDatabase);
                    int choice = form.ShowProjectionAbsence(list, (int)m_mapWin.ApplicationInfo.ProjectionAbsenceBehavior - val, projectProj, out useForOthers, out dontShow);
                    form.Dispose();

                    if (choice == -1)
                        return TestingResult.CancelOperation;

                    choice += val;

                    m_usePreviousAnswerAbsence = useForOthers;
                    m_mapWin.ApplicationInfo.ProjectionAbsenceBehavior = (ProjectionAbsenceBehavior)choice;
                    m_mapWin.ApplicationInfo.NeverShowProjectionDialog = dontShow;
                }

                // when there is no projection in project, it can't be assign for layer
                ProjectionAbsenceBehavior behavior = m_mapWin.ApplicationInfo.ProjectionAbsenceBehavior;
                if (!projectProjectionExists && m_mapWin.ApplicationInfo.ProjectionAbsenceBehavior == ProjectionAbsenceBehavior.AssignFromProject)
                {
                    behavior = ProjectionAbsenceBehavior.IgnoreAbsence;
                }

                switch (behavior)
                {
                    case ProjectionAbsenceBehavior.AssignFromProject:
                        m_report.AddFile(layer.Filename, layer.Projection.Name, ProjectionOperaion.Assigned, "");
                        layer.Projection = projectProj;
                        return TestingResult.Ok;

                    case ProjectionAbsenceBehavior.IgnoreAbsence:
                        m_report.AddFile(layer.Filename, layer.Projection.Name, ProjectionOperaion.AbsenceIgnored, "");
                        return TestingResult.Ok;

                    case ProjectionAbsenceBehavior.SkipFile:
                        m_report.AddFile(layer.Filename, layer.Projection.Name, ProjectionOperaion.Skipped, "");
                        return TestingResult.SkipFile;
                }
            }
            else
            {
                // layer doesn't have projection, project either, nothing to do further here
            }

            System.Diagnostics.Debug.Print("Invalid result in projection tester");
            return TestingResult.Ok;
        }

        
        #endregion
    }
}
