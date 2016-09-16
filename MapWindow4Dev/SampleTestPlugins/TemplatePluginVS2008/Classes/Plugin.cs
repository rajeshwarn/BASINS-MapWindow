// ********************************************************************************************************
// <copyright file="Plugin.cs" company="Bontepaarden.nl">
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
// Change Log: 
// Date           Changed By      Notes
// 13 April 2008  Paul Meems      Inital upload the MW SVN repository
// 28 August 2009 Paul Meems      Made the code StyleCop compliant, made better use of LINQ and
//                                implemented IDisposable
// 5  May 2011    Paul Meems      Made changes as suggested by ReSharper   
// ********************************************************************************************************

namespace TemplatePluginVS2008.Classes
{
    #region Usings

    using System;
    using System.Linq;
    using MapWindow.Interfaces;

    #endregion
    
    /// <summary>
    ///   This class holds all code used in this plug-in
    /// </summary>
    public class PluginCode : IDisposable
    {
        /// <summary>
        ///   Track whether Dispose has been called.
        /// </summary>
        private bool disposed;

        /// <summary>
        ///   The reference to the MapWindow map object
        /// </summary>
        private IMapWin mapWin;

        /// <summary>
        ///   Initializes a new instance of the PluginCode class
        /// </summary>
        /// <param name = "mapWin">The reference to the MapWindow map object</param>
        public PluginCode(IMapWin mapWin)
        {
            this.mapWin = mapWin;
        }

        /// <summary>
        ///   Finalizes an instance of the PluginCode class.
        ///   Use C# destructor syntax for finalization code.
        ///   This destructor will run only if the Dispose method
        ///   does not get called.
        ///   It gives your base class the opportunity to finalize.
        ///   Do not provide destructors in types derived from this class.
        /// </summary>
        ~PluginCode()
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
        ///   <see cref = "http:// msdn.microsoft.com/en-us/library/system.idisposable.dispose.aspx" />
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
        ///   Fills the combobox with all visible layers in the map using LINQ
        /// </summary>
        /// <param name = "frm">The form to work with</param>
        public void FillVisibleLayers(Forms.Layers frm)
        {
            // C#3.0: LINQ to Entities: Language-Integrated Query:            
            var visibleLayers = from l in this.mapWin.Layers.Cast<Layer>()
                                where l.Visible
                                orderby l.GlobalPosition descending
                                select l;

            frm.cboLayers.DisplayMember = "Name";
            frm.cboLayers.ValueMember = "Handle";
            frm.cboLayers.DataSource = visibleLayers.ToArray();
            frm.cboLayers.SelectedValue = this.mapWin.Layers.CurrentLayer;
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
            this.mapWin = null;

            // Note disposing has been done.
            this.disposed = true;
        }
    }
}