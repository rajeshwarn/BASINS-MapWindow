// ********************************************************************************************************
// <copyright file="WorkWithThread.cs" company="Bontepaarden.nl">
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

    using System;
    using System.Threading;
    using Forms;

    #endregion

    /// <summary>
    ///   This class runs a long-running process on a different thread and updates a 
    ///   progress bar on a separate progress form.
    /// </summary>
    public class WorkWithThread
    {
        #region private fields
        /// <summary>
        ///   The max value of the progress bar
        /// </summary>
        private const int MaxValue = 100;

        /// <summary>
        ///   The progress form
        /// </summary>
        private readonly ProgressForm info;

        /// <summary>
        ///   An instance of the Update helper class
        /// </summary>
        private readonly UpdateHelper updater;

        /// <summary>
        ///   An instance of the Worker class
        /// </summary>
        private static WorkWithThread threadInstance;
        #endregion

        /// <summary>
        ///   Initializes a new instance of the WorkWithThread class.
        /// </summary>
        /// <param name = "tempInfo">The progress form</param>
        private WorkWithThread(ProgressForm tempInfo)
        {
            this.info = tempInfo;
            this.updater = new UpdateHelper(this.info);
            this.updater.UpdateEvent += this.UpdaterUpdateEvent;
            this.updater.FinishedEvent += this.UpdaterFinishedEvent;

            this.info.progressBar1.Minimum = 0;
            this.info.progressBar1.Maximum = MaxValue;
        }

        /// <summary>
        ///   This runs the long-running process on another thread.
        /// </summary>
        /// <param name = "progressForm">The form with the progressbar</param>
        public static void DoWork(object progressForm)
        {
            var info = progressForm as ProgressForm;
            if (info == null)
            {
                throw new InvalidProgramException("Parameter for thread must be a of type ProgressForm");
            }

            if (threadInstance == null)
            {
                threadInstance = new WorkWithThread(info);
            }
            else
            {
                return;
            }

            // Do your long-running process here on another thread:
            const double TwoPi = Math.PI * 2.0;
            const double StepCnt = 1000000.0;
            const double Step = TwoPi / StepCnt;

            for (var i = 0; i < MaxValue; i++)
            {
                double theta;
                for (theta = 0.0; theta < TwoPi; theta += Step)
                {
                }

                threadInstance.updater.Update(i, "Step " + i);
            }

            // Finish progress bar:
            threadInstance.updater.Finished();
        }

        /// <summary>
        ///   Cleans up the thread instance
        /// </summary>
        public static void Cleanup()
        {
            if (threadInstance != null)
            {
                threadInstance = null;
            }
        }

        /// <summary>
        ///   This event fires when the updater is updated
        /// </summary>
        /// <param name = "value">The value of the progressbar</param>
        /// <param name = "label">The label on the progress form</param>
        private void UpdaterUpdateEvent(int value, string label)
        {
            this.info.progressBar1.Value = value;
            this.info.label1.Text = label;
        }

        /// <summary>
        ///   This event fires when the updater is finished
        /// </summary>
        private void UpdaterFinishedEvent()
        {
            this.UpdaterUpdateEvent(this.info.progressBar1.Maximum, "Finished");
            Thread.Sleep(1000);
            Cleanup();
            this.info.Close();
        }
    }
}