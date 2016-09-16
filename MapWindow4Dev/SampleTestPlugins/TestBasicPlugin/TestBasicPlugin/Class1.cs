using System;
using System.Collections.Generic;
using System.Text;

namespace TestBasicPlugin
{
    public class TestBasicPlugin : MapWindow.PluginInterfaces.IBasePlugin
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
            System.Windows.Forms.MessageBox.Show("PluginDetails Initialize");
        }

        string MapWindow.PluginInterfaces.IBasePlugin.Name
        {
            get { return "PluginDetails test"; }
        }

        void MapWindow.PluginInterfaces.IBasePlugin.Terminate()
        {
            System.Windows.Forms.MessageBox.Show("PluginDetails Terminate");
        }

        string MapWindow.PluginInterfaces.IBasePlugin.Version
        {
            get { return "1.0"; }
        }

        #endregion
    }
}
