// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Error.cs">
//   
// </copyright>
// <summary>
//   Class for recording and retrieving error messages.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace MapWinGeoProc
{
    #region

    using System.Diagnostics;
    using System.IO;

    #endregion

    /// <summary>
    /// Class for recording and retrieving error messages.
    /// </summary>
    public class Error
    {
        #region Constants and Fields

        /// <summary>
        /// The file name.
        /// </summary>
        private static string fileName = "ErrorLog.txt";

        #endregion

        #region Public Methods

        /// <summary>
        /// Deletes all previous error messages.
        /// </summary>
        public static void ClearErrorLog()
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

        /// <summary>
        /// Provides access to the last error message recieved through 
        ///   the MapWinGeoProc library.
        /// </summary>
        /// <returns>
        /// A description of the problem encountered. 
        /// </returns>
        public static string GetLastErrorMsg()
        {
            StreamReader sr;
            var sLine = string.Empty;
            var errorMsg = string.Empty;
            if (File.Exists(fileName))
            {
                try
                {
                    sr = new StreamReader(fileName);
                    while ((sLine = sr.ReadLine()) != null)
                    {
                        errorMsg = sLine;
                    }

                    sr.Close();
                }
                catch (IOException)
                {
                    return "Error Reading File!!";
                }
            }

            if (errorMsg.Equals(string.Empty))
            {
                errorMsg = "No errors were recorded.";
            }

            return errorMsg;
        }

        /// <summary>
        /// Sets the last error message recieved through
        ///   the MapWinGeoProc library.
        /// </summary>
        /// <param name="errorMsg">
        /// A string describing the problem encountered.
        /// </param>
        public static void SetErrorMsg(string errorMsg)
        {
            StreamWriter sw;
            if (!File.Exists(fileName))
            {
                try
                {
                    sw = File.CreateText(fileName);
                    sw.WriteLine(errorMsg);
                    sw.Close();
                }
                catch (IOException)
                {
                    Debug.WriteLine("Error writing to file!!");
                }
            }
            else
            {
                try
                {
                    sw = File.AppendText(fileName);
                    sw.WriteLine(errorMsg);
                    sw.Close();
                }
                catch (IOException)
                {
                    Debug.WriteLine("Error writing to file!!!");
                }
            }
        }

        #endregion
    }
}