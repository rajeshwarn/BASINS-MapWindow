
namespace MapWindow.Tiles
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;
    #endregion

    /// <summary>
    /// Global static functions
    /// </summary>
    public static class Globals
    {
        public static string AppName = "MapWindow.Tiles";
        
        #region Functions
        /// <summary>
        /// Shows error message box
        /// </summary>
        public static void MessageBoxError(string message)
        {
            MessageBox.Show(message, Globals.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Shows information message box
        /// </summary>
        public static void MessageBoxInformation(string message)
        {
            MessageBox.Show(message, Globals.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Shows question message box
        /// </summary>
        public static DialogResult MessageBoxYesNo(string message)
        {
            return MessageBox.Show(message, Globals.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        /// <summary>
        /// Shows question message box with cancel button
        /// </summary>
        public static DialogResult MessageBoxYesNoCancel(string message)
        {
            return MessageBox.Show(message, Globals.AppName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }
        #endregion
    }

    
}
