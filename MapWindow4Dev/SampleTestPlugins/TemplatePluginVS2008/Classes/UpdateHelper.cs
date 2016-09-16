// ********************************************************************************************************
// <copyright file="UpdateHelper.cs" company="Bontepaarden.nl">
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
// The Original Code is this.mapWindow Open Source MeemsTools Plug-in. 
// 
// The Initial Developer of this version of the Original Code is Paul Meems.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// Change Log: 
// Date           Changed By      Notes
// 11 Sept 2008   Paul Meems      Inital upload the MW SVN repository (many thanks to Jeen de Vegt for providing this thread form sample)
// 5  May 2011    Paul Meems      Made changes as suggested by ReSharper   
// ********************************************************************************************************

namespace TemplatePluginVS2008.Classes
{
    #region Usings

    using System.ComponentModel;

    #endregion

    /// <summary>
    ///   A helper class for the thread and the progress form
    /// </summary>
    public class UpdateHelper
    {
        #region fields
        /// <summary>
        ///   The object to synchronize: the progress form
        /// </summary>
        protected ISynchronizeInvoke syncObject;
        #endregion
        
        /// <summary>
        ///   Initializes a new instance of the UpdateHelper class.
        /// </summary>
        /// <param name = "syncObject">The object to synchronize: the progress form</param>
        public UpdateHelper(ISynchronizeInvoke syncObject)
        {
            this.syncObject = syncObject;
        }

        #region Delegates

        /// <summary>
        ///   The delegate to the Finished event handler
        /// </summary>
        public delegate void FinishedEventHandler();

        /// <summary>
        ///   The delegate to the Update event handler
        /// </summary>
        /// <param name = "value">The value of the procress bar</param>
        /// <param name = "label">The label on the progress form</param>
        public delegate void UpdateEventHandler(int value, string label);

        #endregion

        #region Nested type: FinishedDelegate

        /// <summary>
        ///   The delegate to finish the progress form
        /// </summary>
        protected delegate void FinishedDelegate();

        #endregion

        #region Nested type: UpdateDelegate

        /// <summary>
        ///   The delegate to update the progress form
        /// </summary>
        /// <param name = "value">The value of the procress bar</param>
        /// <param name = "label">The label on the progress form</param>
        protected delegate void UpdateDelegate(int value, string label);

        #endregion

        #region events
        /// <summary>
        ///   The event to update the progress form
        /// </summary>
        public event UpdateEventHandler UpdateEvent;

        /// <summary>
        ///   The event to finish the progress form
        /// </summary>
        public event FinishedEventHandler FinishedEvent;

        /// <summary>
        ///   Finishes the progressbar
        /// </summary>
        public void Finished()
        {
            if (this.syncObject.InvokeRequired)
            {
                this.syncObject.BeginInvoke(new FinishedDelegate(this.Finished), null);
            }
            else
            {
                this.FinishedEvent();
            }
        }

        /// <summary>
        ///   Updates the progressbar
        /// </summary>
        /// <param name = "value">The value of the procress bar</param>
        /// <param name = "label">The label on the progress form</param>
        public void Update(int value, string label)
        {
            if (this.syncObject.InvokeRequired)
            {
                this.syncObject.BeginInvoke(new UpdateDelegate(this.Update), new object[] { value, label });
            }
            else
            {
                this.UpdateEvent(value, label);
            }
        }
        #endregion
    }
}