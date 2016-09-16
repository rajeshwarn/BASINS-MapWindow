using System;
using System.Collections.Generic;
using System.Text;

namespace TestProjectPlugin
{
    public class TestBasicPlugin : MapWindow.PluginInterfaces.IBasePlugin, MapWindow.PluginInterfaces.IProjectEvents 
    {
        #region IBasePlugin Members

        string MapWindow.PluginInterfaces.IBasePlugin.Author
        {
            get { return "Christopher Michaelis"; }
        }

        string MapWindow.PluginInterfaces.IBasePlugin.BuildDate
        {
            get { return "2007"; }
        }

        string MapWindow.PluginInterfaces.IBasePlugin.Description
        {
            get { return "Test plug-in."; }
        }

        void MapWindow.PluginInterfaces.IBasePlugin.Initialize()
        {
            System.Windows.Forms.MessageBox.Show("IBasePlugin Initialize");
        }

        string MapWindow.PluginInterfaces.IBasePlugin.Name
        {
            get { return "PluginDetails+ProjectEvents test"; }
        }

        void MapWindow.PluginInterfaces.IBasePlugin.Terminate()
        {
            System.Windows.Forms.MessageBox.Show("IBasePlugin Terminate");
        }

        string MapWindow.PluginInterfaces.IBasePlugin.Version
        {
            get { return "1.0"; }
        }

        #endregion

        #region IProjectEvents Members

        void MapWindow.PluginInterfaces.IProjectEvents.Initialize()
        {
            System.Windows.Forms.MessageBox.Show("IProjectEvents Initialize");
        }

        void MapWindow.PluginInterfaces.IProjectEvents.ProjectLoading(string ProjectFile, string SettingsString)
        {
            System.Windows.Forms.MessageBox.Show("IProjectEvents ProjectLoading (from " + ProjectFile + ")");
        }

        void MapWindow.PluginInterfaces.IProjectEvents.ProjectSaving(string ProjectFile, ref string SettingsString)
        {
            System.Windows.Forms.MessageBox.Show("IProjectEvents ProjectSaving (to " + ProjectFile + ")");
        }

        void MapWindow.PluginInterfaces.IProjectEvents.Terminate()
        {
            System.Windows.Forms.MessageBox.Show("IProjectEvents Terminate");
        }

        #endregion
    }
}
