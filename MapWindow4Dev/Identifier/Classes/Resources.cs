// ********************************************************************************************************
// <copyright file="Resources.cs" company="Bontepaarden.nl">
//     Copyright (c) Bontepaarden.nl. All rights reserved.
// </copyright>
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http:// Www.mozilla.org/MPL/ 
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The Initial Developer of this version of the Original Code is Paul Meems.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// Change Log: 
// Date         Changed By      Notes
// 31 May 2008  Paul Meems      Initial version
//  5 July 2008 Paul Meems      Moved all common functions to this project
// 11 July 2008 Paul Meems      Changed code as suggested by StyleCop 
// 
// ********************************************************************************************************

#region Usings

using System;
using System.Drawing;
using System.Reflection;
using MapWinUtility;

#endregion

namespace mwIdentifier
{
    /// <summary>
    ///   This class gets the images from the resources
    /// </summary>
    public class Resources : IDisposable
    {
        /// <summary>
        ///   To hold the assembly
        /// </summary>
        private Assembly assembly;

        /// <summary>
        ///   To hold the namespace name
        /// </summary>
        private string namespaceName;

        /// <summary>
        ///   Initializes a new instance of the Resources class
        /// </summary>
        public Resources()
        {
            this.assembly = Assembly.GetCallingAssembly();
            this.GetNamespace();
        }

        /// <summary>
        ///   Initializes a new instance of the Resources class
        /// </summary>
        /// <param name = "assembly">The specific assembly to use</param>
        public Resources(Assembly assembly)
        {
            this.assembly = assembly;
            this.GetNamespace();
        }

        #region IDisposable Members

        /// <summary>
        ///   Dispose all resources
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        ///   Finalizes an instance of the Resources class
        /// </summary>
        ~Resources()
        {
            this.Dispose(false);
        }

        /// <summary>
        ///   Gets the bitmap from the resourcefile
        /// </summary>
        /// <param name = "name">The name of the bitmap in the resourcefile</param>
        /// <param name = "transparant">True to make the bitmap transparent</param>
        /// <returns>The bitmap requested</returns>
        public Bitmap GetEmbeddedBitmap(string name, bool transparant)
        {
            try
            {
                Bitmap bm;
                var resourceName = this.namespaceName + "." + name;
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
                        this.GetListOfEmbeddedResources(resourceName);
                        throw new Exception("GetEmbeddedBitmap has no stream: " + resourceName);
                    }
                }
                return bm;
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR in GetEmbeddedBitmap", ex);
            }
        }

        /// <summary>
        ///   Gets the icon from the resourcefile
        /// </summary>
        /// <param name = "name">The name of the icon in the resourcefile</param>
        /// <returns>The icon requested</returns>
        public Icon GetEmbeddedIcon(string name)
        {
            try
            {
                Icon ic;
                var resourceName = this.namespaceName + "." + name;
                using (var pictureStream = this.assembly.GetManifestResourceStream(resourceName))
                {
                    if (pictureStream != null)
                    {
                        ic = new Icon(pictureStream);
                        pictureStream.Close();
                    }
                    else
                    {
                        this.GetListOfEmbeddedResources(resourceName);
                        throw new Exception("GetEmbeddedIcon has no stream: " + resourceName);
                    }
                }
                return ic;
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR in GetEmbeddedIcon", ex);
            }
        }

        /// <summary>
        ///   Lists all resources in the resourcefile. For debug purposes.
        /// </summary>
        /// <param name = "resourceName">The name of the resource (bitmap, icon)</param>
        /// <returns>The filename of the logfile</returns>
        public string GetListOfEmbeddedResources(string resourceName)
        {
            var logfilename = @"c:\resourcesOf" + this.namespaceName.Replace(" ", "-") + ".log";
            Logger.StartToFile(logfilename, false, false, false);
            Logger.Dbg("In GetListOfEmbeddedResources");
            Logger.Dbg("Cannot find: " + resourceName);
            Logger.Dbg("Resources of " + this.namespaceName);
            var names = this.assembly.GetManifestResourceNames();

            foreach (string name in names)
            {
                Logger.Dbg(name);
            }

            Logger.Flush();
            return logfilename + " created.";
        }

        /// <summary>
        ///   Dispose all resources
        /// </summary>
        /// <param name = "disposing">True to dispose managed resources as well</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Call dispose on any objects referenced by this object
                this.assembly = null;
                this.namespaceName = null;
            }

            // release unmanaged resources:
        }

        /// <summary>
        ///   Determins the namespace name of the resources in the assembly
        /// </summary>
        private void GetNamespace()
        {
            this.namespaceName = this.assembly.GetName().Name + ".Resources";
        }
    }
}