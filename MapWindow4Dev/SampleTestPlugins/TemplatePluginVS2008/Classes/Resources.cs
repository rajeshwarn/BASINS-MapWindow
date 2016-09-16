// ********************************************************************************************************
// <copyright file="Resources.cs" company="Bontepaarden.nl">
//     Copyright (c) Bontepaarden.nl. All rights reserved.
// </copyright>
// ********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http:// www.mozilla.org/MPL/ 
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
// 
// The Original Code is MapWindow Open Source MeemsTools Plug-in. 
// 
// The Initial Developer of this version of the Original Code is Paul Meems.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//  Change Log: 
//  Date           Changed By      Notes
//  13 April 2008  Paul Meems      Inital upload the MW SVN repository
//  28 August 2009 Paul Meems      Made the code StyleCop compliant and implemented IDisposable
//  5  May 2011    Paul Meems      Made changes as suggested by ReSharper   
// ********************************************************************************************************

// This is a very generic class. Would be good to add it to the MapWinUtillity class

namespace TemplatePluginVS2008.Classes
{
    #region Usings

    using System;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;
    using MapWinUtility;

    #endregion
    
    /// <summary>
    ///   This class gets the images from the resource file
    /// </summary>
    public class Resources : IDisposable
    {
        /// <summary>
        ///   The assembly which holds the resource file
        /// </summary>
        private Assembly assembly;

        /// <summary>
        ///   Track whether Dispose has been called.
        /// </summary>
        private bool disposed;

        /// <summary>
        ///   The namespace which holds the assambly
        /// </summary>
        private string myNamespace;

        /// <summary>
        ///   Initializes a new instance of the Resources class.
        ///   Using the executing assembly as the location of the embedded resources
        /// </summary>
        /// <remarks>
        ///   Overloaded constructor with no parameters. But the constructor initializes 
        ///   the values by calling the other constructor by using the keyword "this".
        /// </remarks>
        public Resources() : this(false)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the Resources class.
        /// </summary>
        /// <param name = "useEntryAssembly">Use EntryAssembly as the location of the embedded resources</param>
        public Resources(bool useEntryAssembly)
        {
            if (useEntryAssembly)
            {
                this.assembly = Assembly.GetEntryAssembly();
                this.GetNamespace();
            }
            else
            {
                this.assembly = Assembly.GetExecutingAssembly();
                this.GetNamespace();
            }
        }

        /// <summary>
        ///   Finalizes an instance of the Resources class.
        /// </summary>
        ~Resources()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            this.Dispose(false);
        }

        #region IDisposable Members

        /// <summary>
        ///   Implement IDisposable.
        ///   Do not make this method virtual.
        ///   A derived class should not be able to override this method.
        ///   <see cref = "http://msdn.microsoft.com/en-us/library/system.idisposable.dispose.aspx" />
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        ///   Gets the embedded bitmap
        /// </summary>
        /// <param name = "name">The name of the resource</param>
        /// ///
        /// <param name = "transparant">Make transparent</param>
        /// <returns>The embedded bitmap</returns>
        public Bitmap GetEmbeddedBitmap(string name, bool transparant)
        {
            try
            {
                Bitmap bm;
                var resourceName = this.myNamespace + "." + name;
                //// C#3.0: Implicitly Typed Local Variables and Arrays:
                using (var pictureStream = this.assembly.GetManifestResourceStream(resourceName))
                {
                    if (pictureStream != null)
                    {
                        bm = new Bitmap(pictureStream);
                        if (transparant)
                        {
                            bm.MakeTransparent();
                        }

                        pictureStream.Close();
                    }
                    else
                    {
                        this.GetListOfEmbeddedResources();
                        throw new Exception("GetEmbeddedBitmap has no stream: " + resourceName);
                    }
                }

                return bm;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in GetEmbeddedBitmap: " + ex);
                throw;
            }
        }

        /// <summary>
        ///   Gets the embedded icon
        /// </summary>
        /// <param name = "name">The name of the resource</param>
        /// <returns>The embedded icon</returns>
        public Icon GetEmbeddedIcon(string name)
        {
            try
            {
                Icon ic;
                var resourceName = this.myNamespace + "." + name;
                //// C#3.0: Implicitly Typed Local Variables and Arrays:
                using (var pictureStream = this.assembly.GetManifestResourceStream(resourceName))
                {
                    if (pictureStream != null)
                    {
                        ic = new Icon(pictureStream);
                        pictureStream.Close();
                    }
                    else
                    {
                        this.GetListOfEmbeddedResources();
                        throw new Exception("GetEmbeddedIcon has no stream: " + resourceName);
                    }
                }

                return ic;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR in GetEmbeddedIcon: " + ex);
                throw;
            }
        }

        /// <summary>
        ///   Logs the list of embedded resources in to the log file
        /// </summary>
        public void GetListOfEmbeddedResources()
        {
            var logfilename = @"c:\resourcesOf" + this.myNamespace.Replace(" ", "-") + ".log";
            Logger.StartToFile(logfilename, false, false, false);
            Logger.Dbg("In GetListOfEmbeddedResources");
            Logger.Dbg("Resources of " + this.myNamespace);
            var names = this.assembly.GetManifestResourceNames();
            MessageBox.Show("Found " + names.Length + " resources");
            foreach (var name in names)
            {
                Logger.Dbg(name);
            }

            Logger.Flush();
            MessageBox.Show(logfilename + " created.");
        }

        /// <summary>
        ///   Dispose(bool disposing) executes in two distinct scenarios.
        ///   If disposing equals true, the method has been called directly
        ///   or indirectly by a user's code. Managed and unmanaged resources
        ///   can be disposed.
        ///   If disposing equals false, the method has been called by the
        ///   runtime from inside the finalizer and you should not reference
        ///   other objects. Only unmanaged resources can be disposed.
        /// </summary>
        /// <param name = "disposing">Track whether Dispose has been called.</param>
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (this.disposed)
            {
                return;
            }

            // If disposing equals true, dispose all managed
            // and unmanaged resources.
            if (disposing)
            {
                // Dispose managed resources.
            }

            // Call the appropriate methods to clean up
            // unmanaged resources here.
            // If disposing is false,
            // only the following code is executed.
            this.assembly = null;

            // Note disposing has been done.
            this.disposed = true;
        }

        /// <summary>
        ///   Gets the fully qualified namespace name
        /// </summary>
        private void GetNamespace()
        {
            this.myNamespace = this.assembly.GetName().Name + ".Resources";
        }
    }
}